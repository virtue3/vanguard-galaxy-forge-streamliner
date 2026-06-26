using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Persistables;
using Behaviour.Tractoring;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Crew;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Managers
{
	// Token: 0x02000305 RID: 773
	public class LootManager : Singleton<LootManager>
	{
		// Token: 0x06001C85 RID: 7301 RVA: 0x000AC0BC File Offset: 0x000AA2BC
		public void DropLoot(AbstractUnit unit, DamageData cause, bool player = false)
		{
			if (MapPointOfInterest.current is IndustryStation)
			{
				return;
			}
			if (player && unit.faction != Faction.player)
			{
				float experience = (float)GameMath.GetExperienceRewardValue(unit.baseExperienceReward, unit.level) * unit.unitData.unitRank.GetAdjustedPoints();
				GameplayManager.Instance.spaceShip.AddCrewExperience(experience, new CommanderSpecialization?(CommanderSpecialization.Offense), true);
				if (unit.faction == Faction.marauders)
				{
					SteamStatsManager.Add(SteamStatType.PiratesDestroyed, 1);
				}
				else if (unit.faction == Faction.fanatics)
				{
					SteamStatsManager.Add(SteamStatType.FanaticsDestroyed, 1);
				}
				SpaceShip spaceShip = unit as SpaceShip;
				if (spaceShip != null && spaceShip.shipRoleType.GetRole() == SpaceShipRole.Combat && spaceShip.shipRoleType.GetShipType() == SpaceShipType.Size4)
				{
					SteamAchievement.Trigger("DestroyFrigate");
				}
				if (!unit.unitData.noReputationLoss && !GamePlayer.current.hasUmbralTransponder)
				{
					Drone drone = unit as Drone;
					if (drone != null)
					{
						AbstractUnit droneCommander = drone.droneCommander;
						if (droneCommander != null && droneCommander.unitData.noReputationLoss)
						{
							goto IL_1D4;
						}
					}
					int num = Mathf.CeilToInt(unit.baseExperienceReward * 0.5f);
					int change = Mathf.CeilToInt((float)num * 0.2f);
					if ((float)unit.faction.GetReputation(Faction.player) > -500f)
					{
						num *= 20;
						Singleton<EventLogManager>.Instance.NewEvent("rep", Translation.Translate("@DestroyedFriendlyTarget", new object[]
						{
							num,
							unit.faction.name
						}));
					}
					unit.faction.ChangePlayerReputation(-num);
					Faction faction = SystemMapData.current.faction;
					if (faction != null && faction.IsEnemy(unit.faction) && (float)faction.GetReputation(Faction.player) > -500f && (float)faction.GetReputation(Faction.player) < 1500f)
					{
						faction.ChangePlayerReputation(change);
					}
				}
			}
			IL_1D4:
			Vector3 position = unit.transform.position;
			foreach (ValueTuple<InventoryItemType, int, bool> valueTuple in unit.unitData.CreateLootTable())
			{
				if ((valueTuple.Item3 || !GamePlayer.current.autoPlay || !SeededRandom.Global.RandomBool(0.5f)) && (valueTuple.Item1.itemCategory != ItemCategory.Ammo || unit.faction != Faction.player || !(unit is Drone)))
				{
					this.CreateLootItem(position, valueTuple.Item1, valueTuple.Item2, Faction.player, false);
				}
			}
			if (GamePlayer.current.commander.LeadershipUnlocked())
			{
				SpaceShip spaceShip2 = unit as SpaceShip;
				if (spaceShip2 != null)
				{
					int num2 = (spaceShip2.maxGrunts == 0) ? 10 : spaceShip2.maxGrunts;
					int num3 = SeededRandom.Global.RandomRange(0, Mathf.FloorToInt((float)num2 * 0.25f));
					for (int i = 0; i < num3; i++)
					{
						string random = Behaviour.Crew.Crew.GetRandom(unit.level, true);
						this.CreateCrewPod(random, 1, unit.transform, false);
					}
				}
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000AC3D4 File Offset: 0x000AA5D4
		public TractorableItem SpawnLootItem(TractorableItemData data, Transform parent)
		{
			return this.InstantiateTractorableItem(data, parent ?? this.GetSpawnParent());
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000AC3E8 File Offset: 0x000AA5E8
		public void CreateLootItem(Vector2 position, InventoryItemType item, int amount, Faction owner, bool jettisoned = false)
		{
			if (!Singleton<TravelManager>.Instance.localPoiManager)
			{
				return;
			}
			TractorableItemData tractorableItemData = new TractorableItemData();
			tractorableItemData.itemType = item;
			tractorableItemData.jettisoned = jettisoned;
			tractorableItemData.itemAmount = amount;
			tractorableItemData.ownerFaction = owner;
			tractorableItemData.position = position;
			tractorableItemData.impulse = Vector2.one / 2f + UnityEngine.Random.insideUnitCircle;
			Singleton<TravelManager>.Instance.localPoiManager.poi.AddPersistable(tractorableItemData);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000AC468 File Offset: 0x000AA668
		private TractorableItem InstantiateTractorableItem(TractorableItemData tractorableData, Transform parent)
		{
			TractorableItem tractorableItem = UnityEngine.Object.Instantiate<TractorableItem>(this.tractorablePrefab, tractorableData.position, tractorableData.rotation, parent);
			ItemCategory itemCategory = tractorableData.itemType.itemCategory;
			if (itemCategory == ItemCategory.Ore || itemCategory == ItemCategory.Salvage || itemCategory == ItemCategory.Crystal)
			{
				tractorableItem.SetItemSprite(tractorableData.itemType.icon);
			}
			tractorableItem.SetData(tractorableData);
			return tractorableItem;
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000AC4C8 File Offset: 0x000AA6C8
		private Transform GetSpawnParent()
		{
			BasePoiManager localPoiManager = Singleton<TravelManager>.Instance.localPoiManager;
			if (!localPoiManager)
			{
				return base.transform;
			}
			return localPoiManager.transform;
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000AC4F8 File Offset: 0x000AA6F8
		public void RemoveLootItem(TractorableItemData data)
		{
			BasePoiManager localPoiManager = Singleton<TravelManager>.Instance.localPoiManager;
			if (localPoiManager)
			{
				localPoiManager.poi.RemovePersistable(data);
			}
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000AC524 File Offset: 0x000AA724
		public void CreateCrewPod(string type, int amount, Transform originTransform, bool jettisoned = false)
		{
			Behaviour.Crew.Crew crew = Behaviour.Crew.Crew.Get(type);
			SeededRandom global = SeededRandom.Global;
			Vector2 a = global.RandomBool(0.5f) ? originTransform.up : (-originTransform.up);
			float d = global.RandomRange(1.2f, 1.5f);
			Vector2 b = UnityEngine.Random.insideUnitCircle * 0.25f;
			Vector2 vector = a * d + b;
			InventoryItemType itemType = ItemBuilder.Get("CrewPod").CreateCrewPod(crew, amount);
			CrewPodData persistable = new CrewPodData
			{
				jettisoned = jettisoned,
				itemType = itemType,
				position = originTransform.position,
				angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f - 90f,
				impulse = vector,
				ownerFaction = Faction.player
			};
			MapPointOfInterest.current.AddPersistable(persistable);
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000AC614 File Offset: 0x000AA814
		public void CreateOfficerPod(int level, Vector2 position, CrewMemberData crewmember = null)
		{
			if (crewmember == null)
			{
				crewmember = CrewMemberData.CreateRandomCrewMember(null);
			}
			InventoryItemType itemType = ItemBuilder.Get("OfficerPod").CreateOfficerPod(crewmember);
			TractorableItemData persistable = new TractorableItemData
			{
				itemType = itemType,
				position = position,
				impulse = Vector2.one / 4f + UnityEngine.Random.insideUnitCircle,
				ownerFaction = Faction.player
			};
			MapPointOfInterest.current.AddPersistable(persistable);
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000AC688 File Offset: 0x000AA888
		public void CreateLootBox(int level, Vector2 position)
		{
			InventoryItemType lootBoxItem = ItemBuilder.Get("LootBox").CreateLootboxItem(level);
			LootBoxData lootBoxData = new LootBoxData
			{
				position = position,
				impulse = Vector2.one / 4f + UnityEngine.Random.insideUnitCircle,
				ownerFaction = Faction.player
			};
			lootBoxData.Init(lootBoxItem);
			MapPointOfInterest.current.AddPersistable(lootBoxData);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000AC6F0 File Offset: 0x000AA8F0
		public Dictionary<InventoryItemType, int> OpenLootBox(LootBoxItem lootBox)
		{
			SeededRandom seededRandom = new SeedGenerator().Add(lootBox.seed).CreateRandom();
			Dictionary<InventoryItemType, int> dictionary = new Dictionary<InventoryItemType, int>();
			if (seededRandom.RandomBool(0.001f))
			{
				dictionary.Add(ItemBuilder.Get("LootBox").CreateLootboxItem(lootBox.item.itemLevel), 1);
			}
			if (seededRandom.RandomBool(0.01f) && Register.GetCounter("LootboxSkillPointsGained", 0) < GameMath.MaxLootBoxSkillPoints)
			{
				dictionary.Add(InventoryItemType.Get("BonusSkillPointTemplate"), 1);
			}
			if (lootBox.item.itemLevel > 15)
			{
				InventoryItemType inventoryItemType = InventoryItemType.PotentiallyGetCrystalItem(lootBox.item.itemLevel, seededRandom);
				if (inventoryItemType != null)
				{
					dictionary.Add(inventoryItemType, seededRandom.RandomRange(1, 3));
				}
			}
			if (seededRandom.RandomBool(0.15f))
			{
				Dictionary<Rarity, float> rarityChances = LootManager.GetRarityChances(lootBox.item.itemLevel);
				Rarity value = seededRandom.Choose<Rarity>(rarityChances);
				InventoryItemType inventoryItemType2 = ItemBuilder.Get("Blueprint").CreateRandomBlueprint(lootBox.item.itemLevel, new Rarity?(value), seededRandom, false);
				if (inventoryItemType2)
				{
					dictionary.Add(inventoryItemType2, 1);
				}
			}
			if (seededRandom.RandomBool(0.5f))
			{
				Dictionary<Rarity, float> rarityChances2 = LootManager.GetRarityChances(lootBox.item.itemLevel);
				Rarity rarity = seededRandom.Choose<Rarity>(rarityChances2);
				dictionary.Add(EquipmentBuilder.GetRandom((EquipmentBuilder _) => true, lootBox.item.itemLevel, true, seededRandom).CreateItemType(rarity, lootBox.item.itemLevel, false, lootBox.seed, false, false), 1);
			}
			List<InventoryItemType> refinedItemsWithDropChance = InventoryItemType.GetRefinedItemsWithDropChance(lootBox.item.itemLevel + 3, seededRandom);
			if (dictionary.Count < 3 && seededRandom.RandomBool(0.3f) && refinedItemsWithDropChance.Count > 0)
			{
				dictionary.Add(seededRandom.Choose<InventoryItemType>(refinedItemsWithDropChance), seededRandom.RandomRange(2, 4));
			}
			if (dictionary.Count < 3 && seededRandom.RandomBool(0.3f))
			{
				if (seededRandom.RandomBool(0.05f))
				{
					dictionary.Add(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f), seededRandom.RandomRange(1, 3));
				}
				else
				{
					dictionary.Add(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f), seededRandom.RandomRange(2, 6));
				}
			}
			int a = (int)((float)Mathf.RoundToInt((float)GameMath.GetCreditsValue(50f, lootBox.item.itemLevel)) * 0.3f);
			int b = 0;
			Rarity rarity2 = Rarity.Standard;
			if (dictionary.Count < 3 && seededRandom.RandomBool(0.7f))
			{
				int lo = 10;
				int hi = 60;
				if (seededRandom.RandomBool(0.05f))
				{
					lo = 150;
					hi = 250;
					rarity2 = Rarity.HighGrade;
				}
				else if (seededRandom.RandomBool(0.2f))
				{
					lo = 60;
					hi = 150;
					rarity2 = Rarity.Enhanced;
				}
				b = Mathf.RoundToInt((float)GameMath.GetCreditsValue((float)seededRandom.RandomRange(lo, hi), lootBox.item.itemLevel));
			}
			int amount = Mathf.Max(a, b);
			InventoryItemType inventoryItemType3 = ItemBuilder.Get("Credits").CreateCreditsItem(amount);
			inventoryItemType3.SetRarity(rarity2);
			dictionary.Add(inventoryItemType3, 1);
			if (seededRandom.RandomBool(0.01f))
			{
				dictionary.Add(InventoryItemType.Get("LockedContainerKey"), 1);
			}
			if (dictionary.Count < 3)
			{
				dictionary.Add(lootBox.systemMapData.systemOreData.GetRandomOre(true, seededRandom).item, seededRandom.RandomRange(4, 9));
			}
			return dictionary;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000ACA60 File Offset: 0x000AAC60
		private static Dictionary<Rarity, float> GetRarityChances(int level)
		{
			Dictionary<Rarity, float> dictionary = new Dictionary<Rarity, float>
			{
				{
					Rarity.Standard,
					0.4f
				},
				{
					Rarity.Enhanced,
					0.3f
				}
			};
			if (level >= 11)
			{
				dictionary.Add(Rarity.HighGrade, Mathf.Clamp(0.2f + (float)level / 1000f, 0.2f, 0.3f));
			}
			if (level >= 25)
			{
				dictionary.Add(Rarity.Exotic, Mathf.Clamp(0.1f + (float)level / 2000f, 0.1f, 0.15f));
			}
			if (level >= 60)
			{
				dictionary.Add(Rarity.Legendary, (float)(level - 60) / 2000f);
			}
			return dictionary;
		}

		// Token: 0x040011BA RID: 4538
		public TractorableItem tractorablePrefab;

		// Token: 0x040011BB RID: 4539
		public LootBox lootBoxPrefab;

		// Token: 0x040011BC RID: 4540
		public CrewPod crewPodPrefab;
	}
}
