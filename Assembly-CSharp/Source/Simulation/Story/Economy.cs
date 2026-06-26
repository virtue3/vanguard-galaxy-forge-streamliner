using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.Crew;
using Behaviour.Item;
using LightJson;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.Economy;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.Story
{
	// Token: 0x02000089 RID: 137
	public class Economy : Storyteller
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x0002A038 File Offset: 0x00028238
		public Economy(GamePlayer ply) : base(ply)
		{
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0002A062 File Offset: 0x00028262
		public override void SetupNewGame()
		{
			this.ResetEconomy();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0002A06C File Offset: 0x0002826C
		public override void Start()
		{
			if (this.InitializeEconomy())
			{
				this.GenerateEconomy();
			}
			foreach (KeyValuePair<InventoryItemType, Economy.EconomyTradeResult> keyValuePair in this.tradeResults)
			{
				int b = this.player.CountAvailableItems(keyValuePair.Key);
				keyValuePair.Value.purchased = Mathf.Min(keyValuePair.Value.purchased, b);
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0002A0F8 File Offset: 0x000282F8
		public void AddTrade(InventoryItemType type, int count, int price)
		{
			Economy.EconomyTradeResult tradeResult = this.GetTradeResult(type);
			if (tradeResult.purchased > 0 && count > 0 && price != tradeResult.cost)
			{
				double num = (double)tradeResult.cost;
				double num2 = (double)price;
				price = (int)Math.Round((num * (double)tradeResult.purchased + num2 * (double)count) / (double)(tradeResult.purchased + count));
				tradeResult.isAverage = true;
			}
			else if (count < 0 && tradeResult.cost > 0)
			{
				int num3 = price - tradeResult.cost;
				if (num3 > 0)
				{
					MissionObjective.Trigger(MissionTrigger.TradeTerminalProfit, num3 * -count, null, false);
					Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Economy));
					SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(tree, false);
					if (skillTreeData != null)
					{
						skillTreeData.AddMasteryXp((float)(num3 * -(float)count));
					}
				}
			}
			tradeResult.purchased = Mathf.Max(0, tradeResult.purchased + count);
			if (count > 0)
			{
				tradeResult.cost = price;
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0002A1D0 File Offset: 0x000283D0
		public Economy.EconomyTradeResult GetTradeResult(InventoryItemType type)
		{
			Economy.EconomyTradeResult economyTradeResult;
			if (!this.tradeResults.TryGetValue(type, out economyTradeResult))
			{
				economyTradeResult = new Economy.EconomyTradeResult();
				this.tradeResults[type] = economyTradeResult;
			}
			return economyTradeResult;
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0002A201 File Offset: 0x00028401
		public override void StoryUpdate(float delta)
		{
			this.economyTimer -= Time.deltaTime;
			if (this.economyTimer < 0f)
			{
				this.EconomyTick();
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0002A228 File Offset: 0x00028428
		private bool InitializeEconomy()
		{
			bool result = false;
			this.economy.Clear();
			foreach (MapPointOfInterest mapPointOfInterest in this.player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && spaceStation.economy != null)
				{
					this.economy.Add(spaceStation.economy);
					if (spaceStation.economy.itemCount == 0)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0002A2B8 File Offset: 0x000284B8
		public void ResetEconomy()
		{
			this.InitializeEconomy();
			foreach (LocalEconomy localEconomy in this.economy)
			{
				localEconomy.ClearItems();
			}
			this.GenerateEconomy();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0002A318 File Offset: 0x00028518
		public void GenerateEconomy()
		{
			this.InitializeEconomy();
			SeededRandom global = SeededRandom.Global;
			List<LocalEconomy> list = new List<LocalEconomy>();
			foreach (LocalEconomy localEconomy in this.economy)
			{
				if (localEconomy.itemCount == 0)
				{
					list.Add(localEconomy);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			SeededRandom global2 = SeededRandom.Global;
			List<InventoryItemType> list2 = new List<InventoryItemType>();
			foreach (KeyValuePair<Rarity, List<InventoryItemType>> keyValuePair in Economy.defaultTradeGoods)
			{
				int num = (keyValuePair.Key == Rarity.Standard || keyValuePair.Key == Rarity.Enhanced) ? 2 : 1;
				for (int i = 0; i < num; i++)
				{
					list2.AddRange(keyValuePair.Value);
				}
				foreach (InventoryItemType type in keyValuePair.Value)
				{
					foreach (LocalEconomy localEconomy2 in list)
					{
						localEconomy2.GetEconomy(type);
					}
				}
			}
			List<InventoryItemType> list3 = new List<InventoryItemType>();
			foreach (LocalEconomy localEconomy3 in list)
			{
				int num2 = global.RandomRange(2, 5);
				for (int j = 0; j < num2; j++)
				{
					if (list3.Count == 0)
					{
						list3.AddRange(list2);
						global.Shuffle<InventoryItemType>(list3);
					}
					InventoryItemType type2 = list3[0];
					list3.RemoveAt(0);
					localEconomy3.GetEconomy(type2).supplyModifier += 1f;
				}
			}
			for (int k = 0; k < 50; k++)
			{
				this.EconomyTick();
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0002A544 File Offset: 0x00028744
		public void EconomyTick()
		{
			SeededRandom global = SeededRandom.Global;
			foreach (LocalEconomy localEconomy in this.economy)
			{
				foreach (LocalEconomyItem localEconomyItem in localEconomy.allItems)
				{
					localEconomyItem.CycleHistory();
					float num = 1f - 0.15f * localEconomyItem.supplyModifier;
					float num2 = (localEconomyItem.currentValue > num) ? 0.45f : 0.55f;
					if (localEconomyItem.currentValue > 2f)
					{
						num2 -= (localEconomyItem.currentValue - 1f) / 10f;
					}
					else if (localEconomyItem.currentValue < 0.7f)
					{
						num2 += 0.2f;
					}
					else if (localEconomyItem.currentValue < 0.8f)
					{
						num2 += 0.1f;
					}
					float num3 = global.RandomRange(0.025f, 0.05f) * (1f + SkilltreeNode.economyMarketVolatility.currentIncrease);
					if (global.RandomBool(0.2f))
					{
						num3 *= 4f;
					}
					if (global.RandomBool(num2))
					{
						localEconomyItem.currentValue *= 1f + num3;
					}
					else
					{
						localEconomyItem.currentValue /= 1f + num3;
					}
					if (localEconomyItem.currentValue < 0.55f)
					{
						localEconomyItem.currentValue = 0.55f;
					}
					float num4 = (float)GameMath.GetCreditsValue(1f + localEconomyItem.supplyModifier, localEconomy.spaceStation.level);
					if (localEconomyItem.currentValue < 0.7f)
					{
						num4 *= 0.6f;
					}
					else if (localEconomyItem.currentValue < 0.8f)
					{
						num4 *= 0.7f;
					}
					else if (localEconomyItem.currentValue < 0.9f)
					{
						num4 *= 0.8f;
					}
					Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Economy));
					SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(tree, false);
					int? num5 = (skillTreeData != null) ? new int?(skillTreeData.masteryLevel) : null;
					float num6 = 1f;
					if (num5 != null)
					{
						num6 += (float)num5.Value * 0.01f;
					}
					localEconomyItem.currentSupply = Mathf.RoundToInt(350f * num4 / (float)localEconomyItem.cost * global.RandomRange(0.5f, 1f) * num6);
				}
				this.CreateDealItemList();
				localEconomy.craftingDealCooldown--;
				if (localEconomy.craftingDealCooldown < 0 || localEconomy.craftingDealCount == 0)
				{
					InventoryItemType inventoryItemType = global.Choose<InventoryItemType>(Economy.dealItems);
					float num7 = (float)GameMath.GetCreditsValue(global.RandomRange(175f, 375f), localEconomy.spaceStation.level);
					localEconomy.craftingDealItem = inventoryItemType;
					localEconomy.craftingDealCount = Mathf.RoundToInt(num7 / (float)inventoryItemType.sellValue);
					localEconomy.craftingDealMultiplier = global.RandomRange(1.5f, 2.5f);
					localEconomy.craftingDealCooldown = 0;
				}
			}
			this.economyTimer = 1200f;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0002A8A4 File Offset: 0x00028AA4
		private void CreateDealItemList()
		{
			if (Economy.dealItems != null)
			{
				return;
			}
			Economy.dealItems = new Dictionary<InventoryItemType, float>();
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.GetAvailable())
			{
				using (IEnumerator<CraftingRecipe.CraftingRecipeItemRow> enumerator2 = craftingRecipe.RawResults.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						InventoryItemType inventoryItemType;
						if (enumerator2.Current.item.TryGetComponent<InventoryItemType>(out inventoryItemType) && inventoryItemType.itemCategory == ItemCategory.RefinedProduct && !inventoryItemType.missionItem)
						{
							Economy.dealItems.Add(inventoryItemType, 100f / Mathf.Sqrt((float)inventoryItemType.sellValue));
						}
					}
				}
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0002A96C File Offset: 0x00028B6C
		public override void DataToJson(JsonObject data)
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<InventoryItemType, Economy.EconomyTradeResult> keyValuePair in this.tradeResults)
			{
				jsonObject[keyValuePair.Key.identifier] = keyValuePair.Value.ToJson();
			}
			data["tradeResults"] = jsonObject;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0002A9F0 File Offset: 0x00028BF0
		public override void DataFromJson(JsonObject data)
		{
			if (data["tradeResults"].IsJsonObject)
			{
				foreach (KeyValuePair<string, JsonValue> keyValuePair in data["tradeResults"].AsJsonObject)
				{
					this.tradeResults.Add(keyValuePair.Key, Economy.EconomyTradeResult.FromJson(keyValuePair.Value));
				}
			}
		}

		// Token: 0x0400029C RID: 668
		public const float EconomyUpdateTime = 1200f;

		// Token: 0x0400029D RID: 669
		public const float ProductionPerTick = 350f;

		// Token: 0x0400029E RID: 670
		public static Dictionary<Rarity, List<InventoryItemType>> defaultTradeGoods = new Dictionary<Rarity, List<InventoryItemType>>
		{
			{
				Rarity.Standard,
				new List<InventoryItemType>
				{
					"Synthblood Pack",
					"Consumer Electronics",
					"Solvent Cartridge",
					"Body Armor"
				}
			},
			{
				Rarity.Enhanced,
				new List<InventoryItemType>
				{
					"Artificial Organs",
					"Dubious Vid-Discs",
					"Liquid Alloy",
					"Hand Cannon"
				}
			},
			{
				Rarity.HighGrade,
				new List<InventoryItemType>
				{
					"Empty Braincage",
					"Massive Vid-Screen",
					"Stasis Chamber",
					"Remote Hacking Tool"
				}
			},
			{
				Rarity.Exotic,
				new List<InventoryItemType>
				{
					"Blank Android Shell",
					"Simreality Headset",
					"Antimatter Particle",
					"Combat Exoskeleton"
				}
			}
		};

		// Token: 0x0400029F RID: 671
		private static Dictionary<InventoryItemType, float> dealItems;

		// Token: 0x040002A0 RID: 672
		public float economyTimer = 1200f;

		// Token: 0x040002A1 RID: 673
		private List<LocalEconomy> economy = new List<LocalEconomy>();

		// Token: 0x040002A2 RID: 674
		private Dictionary<InventoryItemType, Economy.EconomyTradeResult> tradeResults = new Dictionary<InventoryItemType, Economy.EconomyTradeResult>();

		// Token: 0x02000429 RID: 1065
		public class EconomyTradeResult
		{
			// Token: 0x060026F6 RID: 9974 RVA: 0x000D5F3C File Offset: 0x000D413C
			public JsonValue ToJson()
			{
				return new JsonObject
				{
					{
						"purchased",
						new double?((double)this.purchased)
					},
					{
						"cost",
						new double?((double)this.cost)
					},
					{
						"isAverage",
						new bool?(this.isAverage)
					}
				};
			}

			// Token: 0x060026F7 RID: 9975 RVA: 0x000D5FAC File Offset: 0x000D41AC
			public static Economy.EconomyTradeResult FromJson(JsonObject data)
			{
				return new Economy.EconomyTradeResult
				{
					purchased = data["purchased"],
					cost = data["cost"],
					isAverage = data["isAverage"]
				};
			}

			// Token: 0x04001802 RID: 6146
			public int purchased;

			// Token: 0x04001803 RID: 6147
			public int cost;

			// Token: 0x04001804 RID: 6148
			public bool isAverage;
		}
	}
}
