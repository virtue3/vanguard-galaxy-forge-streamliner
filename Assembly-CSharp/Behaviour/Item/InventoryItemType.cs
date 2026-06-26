using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Crafting;
using Behaviour.Crew;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Unit;
using LightJson;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item
{
	// Token: 0x02000308 RID: 776
	public class InventoryItemType : MonoBehaviour, IJsonSource
	{
		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x000AE3D2 File Offset: 0x000AC5D2
		public static IEnumerable<InventoryItemType> all
		{
			get
			{
				return InventoryItemType.allItems.Values;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x000AE3DE File Offset: 0x000AC5DE
		// (set) Token: 0x06001CDA RID: 7386 RVA: 0x000AE3E6 File Offset: 0x000AC5E6
		public string identifier { get; private set; }

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x000AE3EF File Offset: 0x000AC5EF
		// (set) Token: 0x06001CDC RID: 7388 RVA: 0x000AE3F7 File Offset: 0x000AC5F7
		public ItemCategory itemCategory { get; private set; }

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x000AE400 File Offset: 0x000AC600
		// (set) Token: 0x06001CDE RID: 7390 RVA: 0x000AE408 File Offset: 0x000AC608
		public StorageOverride storageOverride { get; private set; }

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x000AE411 File Offset: 0x000AC611
		// (set) Token: 0x06001CE0 RID: 7392 RVA: 0x000AE419 File Offset: 0x000AC619
		public bool missionItem { get; private set; }

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x000AE422 File Offset: 0x000AC622
		// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x000AE42A File Offset: 0x000AC62A
		public string displayName { get; private set; }

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x000AE433 File Offset: 0x000AC633
		// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x000AE43B File Offset: 0x000AC63B
		public string description { get; private set; }

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x000AE444 File Offset: 0x000AC644
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000AE44C File Offset: 0x000AC64C
		public Sprite icon { get; private set; }

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000AE455 File Offset: 0x000AC655
		// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x000AE45D File Offset: 0x000AC65D
		public float m3 { get; private set; }

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x000AE466 File Offset: 0x000AC666
		// (set) Token: 0x06001CEA RID: 7402 RVA: 0x000AE46E File Offset: 0x000AC66E
		public int baseCost { get; private set; }

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x000AE477 File Offset: 0x000AC677
		// (set) Token: 0x06001CEC RID: 7404 RVA: 0x000AE47F File Offset: 0x000AC67F
		public Rarity rarity { get; private set; }

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x000AE488 File Offset: 0x000AC688
		// (set) Token: 0x06001CEE RID: 7406 RVA: 0x000AE490 File Offset: 0x000AC690
		public int itemLevel { get; private set; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x000AE499 File Offset: 0x000AC699
		// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x000AE4A1 File Offset: 0x000AC6A1
		public ShopItemData shopItemData { get; private set; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06001CF1 RID: 7409 RVA: 0x000AE4AA File Offset: 0x000AC6AA
		// (set) Token: 0x06001CF2 RID: 7410 RVA: 0x000AE4B2 File Offset: 0x000AC6B2
		public bool canJettison { get; private set; } = true;

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001CF3 RID: 7411 RVA: 0x000AE4BB File Offset: 0x000AC6BB
		// (set) Token: 0x06001CF4 RID: 7412 RVA: 0x000AE4C3 File Offset: 0x000AC6C3
		public bool canSell { get; private set; } = true;

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000AE4CC File Offset: 0x000AC6CC
		// (set) Token: 0x06001CF6 RID: 7414 RVA: 0x000AE4D4 File Offset: 0x000AC6D4
		public bool criticalItem { get; private set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001CF7 RID: 7415 RVA: 0x000AE4DD File Offset: 0x000AC6DD
		// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x000AE4E5 File Offset: 0x000AC6E5
		public bool overrideManufacturer { get; private set; }

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x000AE4EE File Offset: 0x000AC6EE
		// (set) Token: 0x06001CFA RID: 7418 RVA: 0x000AE4F6 File Offset: 0x000AC6F6
		public Manufacturer manufacturer { get; private set; }

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001CFB RID: 7419 RVA: 0x000AE4FF File Offset: 0x000AC6FF
		// (set) Token: 0x06001CFC RID: 7420 RVA: 0x000AE507 File Offset: 0x000AC707
		public EquipmentBuilder equipmentBuilder { get; set; }

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06001CFD RID: 7421 RVA: 0x000AE510 File Offset: 0x000AC710
		// (set) Token: 0x06001CFE RID: 7422 RVA: 0x000AE518 File Offset: 0x000AC718
		public ItemBuilder itemBuilder { get; set; }

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06001CFF RID: 7423 RVA: 0x000AE521 File Offset: 0x000AC721
		// (set) Token: 0x06001D00 RID: 7424 RVA: 0x000AE529 File Offset: 0x000AC729
		public EquipmentBuilderVisual visual { get; private set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000AE532 File Offset: 0x000AC732
		// (set) Token: 0x06001D02 RID: 7426 RVA: 0x000AE53A File Offset: 0x000AC73A
		public bool equipmentInitialized { get; private set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x000AE543 File Offset: 0x000AC743
		// (set) Token: 0x06001D04 RID: 7428 RVA: 0x000AE54B File Offset: 0x000AC74B
		public string searchField { get; private set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000AE554 File Offset: 0x000AC754
		public GameplayType gameplayType
		{
			get
			{
				AbstractEquipment component = base.GetComponent<AbstractEquipment>();
				GameplayType result;
				if (component)
				{
					if (!(component is AbstractCombatTurret))
					{
						if (!(component is AbstractMiningTurret))
						{
							if (!(component is AbstractSalvageTurret))
							{
								if (!(component is HullModule))
								{
									if (!(component is ArmorModule))
									{
										if (!(component is ShieldGeneratorModule))
										{
											result = GameplayType.Generic;
										}
										else
										{
											result = GameplayType.Combat;
										}
									}
									else
									{
										result = GameplayType.Combat;
									}
								}
								else
								{
									result = GameplayType.Combat;
								}
							}
							else
							{
								result = GameplayType.Salvage;
							}
						}
						else
						{
							result = GameplayType.Mining;
						}
					}
					else
					{
						result = GameplayType.Combat;
					}
					return result;
				}
				ItemCategory itemCategory = this.itemCategory;
				if (itemCategory != ItemCategory.Ore)
				{
					if (itemCategory != ItemCategory.Salvage)
					{
						result = GameplayType.Generic;
					}
					else
					{
						result = GameplayType.Salvage;
					}
				}
				else
				{
					result = GameplayType.Mining;
				}
				return result;
			}
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000AE5DB File Offset: 0x000AC7DB
		public int cost
		{
			get
			{
				if (this.calcCost < 0f)
				{
					this.calcCost = this.CalculateDynamicCost();
				}
				return Mathf.RoundToInt(this.calcCost * this.discount);
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000AE608 File Offset: 0x000AC808
		public float discount
		{
			get
			{
				float num = 1f;
				if (MapPointOfInterest.current is SpaceStation)
				{
					Faction faction = MapPointOfInterest.current.faction;
					int reputation = (faction != null) ? faction.GetReputation(Faction.player) : 0;
					num -= ReputationLevelExtensions.GetShopDiscount(reputation);
					if (this.itemCategory == ItemCategory.Module || this.itemCategory == ItemCategory.Booster || this.itemCategory == ItemCategory.Turret)
					{
						num -= SkilltreeNode.economyEquipmentCost.currentIncrease;
					}
					num -= SkilltreeNode.economyGlobalValue.currentIncrease;
				}
				return num;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000AE682 File Offset: 0x000AC882
		public int sellValue
		{
			get
			{
				return this.GetSellValue(true);
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x000AE68C File Offset: 0x000AC88C
		public int shipComponentSellValue
		{
			get
			{
				int cost = this.cost;
				return Mathf.Clamp(Mathf.RoundToInt(this.calcCost * this.itemCategory.GetSellValueMultiplier() * this.sellValueMultiplier * this.discount), 1, Mathf.RoundToInt(Mathf.Max(1f, this.calcCost - 1f)));
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000AE6E6 File Offset: 0x000AC8E6
		public bool buyBack
		{
			get
			{
				return this.itemCategory.HasBuybackValue();
			}
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000AE6F4 File Offset: 0x000AC8F4
		public int GetSellValue(bool applySkillBonuses = true)
		{
			int cost = this.cost;
			float num = this.calcCost * this.itemCategory.GetSellValueMultiplier() * this.sellValueMultiplier;
			if (applySkillBonuses)
			{
				num *= (1f + SkilltreeNode.economySellValue.currentIncrease) * (1f + SkilltreeNode.economyGlobalValue.currentIncrease);
				if (this.isCraftedItem || (this.equipmentBuilder && base.GetComponent<AbstractEquipment>().isCrafted))
				{
					num *= 1f + SkilltreeNode.industrialSellValue.currentIncrease;
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num), 1, Mathf.RoundToInt(Mathf.Max(1f, this.calcCost - 1f)));
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000AE7A8 File Offset: 0x000AC9A8
		public bool EnoughRepWithFaction(Faction faction)
		{
			if (this.shopItemData.factionPrereq.Count == 0)
			{
				return true;
			}
			foreach (FactionPrerequisites factionPrerequisites in this.shopItemData.factionPrereq)
			{
				if (faction.identifier == factionPrerequisites.faction && faction.GetReputation(Faction.player) >= factionPrerequisites.reputationLevel.GetReputationThreshold())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000AE840 File Offset: 0x000ACA40
		public void SetItemLevel(int power)
		{
			this.itemLevel = power;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000AE849 File Offset: 0x000ACA49
		public void SetBaseValue(int cost)
		{
			this.baseCost = cost;
			this.calcCost = this.CalculateDynamicCost();
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000AE85E File Offset: 0x000ACA5E
		public void RecalculateCost()
		{
			this.calcCost = this.CalculateDynamicCost();
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000AE86C File Offset: 0x000ACA6C
		public void SetRarity(Rarity r)
		{
			this.rarity = r;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000AE875 File Offset: 0x000ACA75
		public void SetBaseCost(int cost)
		{
			this.baseCost = cost;
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000AE87E File Offset: 0x000ACA7E
		public void SetIcon(Sprite sprite)
		{
			this.icon = sprite;
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000AE888 File Offset: 0x000ACA88
		public void SetVisual(EquipmentBuilderVisual visual)
		{
			if (!string.IsNullOrEmpty(visual.displayName))
			{
				this.displayName = visual.displayName;
			}
			if (!string.IsNullOrEmpty(visual.description))
			{
				this.description = visual.description;
			}
			if (visual.icon != null)
			{
				this.icon = visual.icon;
			}
			this.visual = visual;
		}

		// Token: 0x06001D14 RID: 7444 RVA: 0x000AE8E8 File Offset: 0x000ACAE8
		public void SetDisplayName(string name)
		{
			this.displayName = name;
		}

		// Token: 0x06001D15 RID: 7445 RVA: 0x000AE8F1 File Offset: 0x000ACAF1
		public void SetDescription(string description)
		{
			this.description = description;
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000AE8FC File Offset: 0x000ACAFC
		public Manufacturer? GetManufacturer()
		{
			if (this.overrideManufacturer)
			{
				return new Manufacturer?(this.manufacturer);
			}
			EquipmentBuilderVisual visual = this.visual;
			if (visual == null)
			{
				return null;
			}
			return new Manufacturer?(visual.manufacturer);
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000AE93C File Offset: 0x000ACB3C
		public virtual bool CanStackWith(InventoryItemType other)
		{
			if (!other || !this)
			{
				return false;
			}
			if (this.equipmentBuilder)
			{
				return false;
			}
			if (this.itemBuilder)
			{
				InventoryItemPart[] components = base.GetComponents<InventoryItemPart>();
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].CanStackWith(other))
					{
						return true;
					}
				}
			}
			return other == this;
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000AE9A0 File Offset: 0x000ACBA0
		public virtual void SplitStack(Inventory inventory, int slot1, int amount1)
		{
			if (this.itemBuilder)
			{
				InventoryItemPart[] components = base.GetComponents<InventoryItemPart>();
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].SplitStack(inventory, slot1, amount1))
					{
						return;
					}
				}
			}
			if (inventory.hasSearchFilter)
			{
				inventory.Add(this, amount1, false, true);
				return;
			}
			inventory.Set(slot1, this, amount1, false);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000AE9FC File Offset: 0x000ACBFC
		public virtual int AddStackToSelf(int ownCount, InventoryItemType other, int otherCount)
		{
			if (this.itemBuilder)
			{
				foreach (InventoryItemPart inventoryItemPart in base.GetComponents<InventoryItemPart>())
				{
					if (inventoryItemPart.CanStackWith(other))
					{
						return inventoryItemPart.AddStackToSelf(ownCount, other, otherCount);
					}
				}
			}
			return ownCount + otherCount;
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000AEA45 File Offset: 0x000ACC45
		public bool IsUsable()
		{
			return base.GetComponent<UsableItem>();
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000AEA52 File Offset: 0x000ACC52
		public bool IsEquippable()
		{
			return this.itemCategory.CanBeEquiped();
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000AEA5F File Offset: 0x000ACC5F
		public bool IsUsableInSpaceStation()
		{
			UsableItem component = base.GetComponent<UsableItem>();
			return component != null && component.canUseInSpacestation;
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000AEA74 File Offset: 0x000ACC74
		private float CalculateDynamicCost()
		{
			InventoryItemPart[] components = base.GetComponents<InventoryItemPart>();
			for (int i = 0; i < components.Length; i++)
			{
				int dynamicValue = components[i].GetDynamicValue();
				if (dynamicValue != 0)
				{
					return (float)dynamicValue;
				}
			}
			if (!this.equipmentBuilder && !this.itemBuilder && this.itemCategory != ItemCategory.Ammo)
			{
				foreach (CraftingRecipe craftingRecipe in CraftingRecipe.all)
				{
					int resultCount = craftingRecipe.GetResultCount(this);
					if (resultCount > 0)
					{
						this.isCraftedItem = true;
						return craftingRecipe.value / (float)resultCount;
					}
				}
			}
			return (float)this.baseCost;
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000AEB30 File Offset: 0x000ACD30
		public void ResetDynamicValue()
		{
			this.calcCost = -1f;
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000AEB3D File Offset: 0x000ACD3D
		public bool ShowItemInShop(int stationLevel)
		{
			return this.shopItemData.minAreaLevelRequirement <= stationLevel && (this.shopItemData.maxAreaLevelRequirement == 0 || stationLevel <= this.shopItemData.maxAreaLevelRequirement);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000AEB70 File Offset: 0x000ACD70
		public void InitializeItem(InventoryItemType overrideItemType = null)
		{
			InventoryItemPart[] components = base.GetComponents<InventoryItemPart>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].SetItem(overrideItemType ?? this);
			}
			this.calcCost = -1f;
			if (this.equipmentInitialized)
			{
				return;
			}
			AbstractEquipment component = base.GetComponent<AbstractEquipment>();
			if (component != null)
			{
				component.InitializeEquipment();
			}
			this.SetSearchFieldContent();
			this.equipmentInitialized = true;
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000AEBD2 File Offset: 0x000ACDD2
		public void SetEquipmentInitialized(bool initialized)
		{
			this.equipmentInitialized = true;
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000AEBDC File Offset: 0x000ACDDC
		public bool CanBeEquippedOn(SpaceShipData ss)
		{
			if (!this.itemCategory.CanBeEquiped())
			{
				return false;
			}
			AbstractEquipment component = base.GetComponent<AbstractEquipment>();
			if (!component)
			{
				return false;
			}
			if (component.slot == EquipmentSlot.Hardpoint)
			{
				SpaceShipHardpoint[] hardpointSlots = ss.shipClass.hardpointSlots;
				for (int i = 0; i < hardpointSlots.Length; i++)
				{
					if (hardpointSlots[i].size == component.size)
					{
						return true;
					}
				}
				return false;
			}
			if (component.slot == EquipmentSlot.Booster)
			{
				SpaceShipBooster[] boosterSlots = ss.shipClass.boosterSlots;
				for (int i = 0; i < boosterSlots.Length; i++)
				{
					if (boosterSlots[i].size == component.size)
					{
						return true;
					}
				}
				return false;
			}
			foreach (SpaceShipModule spaceShipModule in ss.shipClass.moduleSlots)
			{
				if (spaceShipModule.slot == component.slot && spaceShipModule.size == component.size)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000AECBC File Offset: 0x000ACEBC
		private static void GetCraftingMaterials(InventoryItemType type, float[] toArray)
		{
			CraftingRecipe sourceRecipe = CraftingRecipe.GetSourceRecipe(type);
			if (sourceRecipe)
			{
				foreach (ValueTuple<RefinedMaterial, float> valueTuple in sourceRecipe.GetIngredientMaterials(0))
				{
					toArray[(int)valueTuple.Item1] += valueTuple.Item2;
				}
				foreach (ValueTuple<InventoryItemType, int> valueTuple2 in sourceRecipe.GetIngredientItems(0))
				{
					InventoryItemType.GetCraftingMaterials(valueTuple2.Item1, toArray);
				}
			}
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000AED68 File Offset: 0x000ACF68
		public IEnumerable<ValueTuple<RefinedMaterial, float>> GetCraftingMaterials()
		{
			float[] materials = new float[Enum.GetValues(typeof(RefinedMaterial)).Length];
			InventoryItemType.GetCraftingMaterials(this, materials);
			int num;
			for (int i = 0; i < materials.Length; i = num + 1)
			{
				yield return new ValueTuple<RefinedMaterial, float>((RefinedMaterial)i, materials[i]);
				num = i;
			}
			yield break;
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000AED78 File Offset: 0x000ACF78
		private void SetSearchFieldContent()
		{
			this.searchField = string.Concat(new string[]
			{
				Translation.Translate(this.displayName, Array.Empty<object>()),
				this.rarity.ToString(),
				this.rarity.GetAlternateNames(),
				this.itemCategory.ToString(),
				this.gameplayType.ToString()
			});
			AbstractEquipment abstractEquipment;
			MiningClaimItem miningClaimItem;
			if (base.TryGetComponent<AbstractEquipment>(out abstractEquipment))
			{
				this.searchField += Translation.Translate("@Equipment", Array.Empty<object>());
				this.searchField += Translation.Translate("@EquipSlot" + abstractEquipment.slot.ToString(), Array.Empty<object>());
				this.searchField += Translation.Translate(abstractEquipment.size.GetDisplayName(), Array.Empty<object>());
				this.searchField += abstractEquipment.typeDisplayName;
				foreach (EquipStatLine equipStatLine in abstractEquipment.stats)
				{
					this.searchField += Translation.Translate("@EquipStat" + equipStatLine.stat.ToString(), Array.Empty<object>());
				}
				if (abstractEquipment.aspectSlots.Count > 0)
				{
					foreach (AspectSlot aspectSlot in abstractEquipment.aspectSlots)
					{
						if (aspectSlot.equipAspect)
						{
							this.searchField += Translation.Translate("@Aspect" + aspectSlot.equipAspect.name, Array.Empty<object>());
						}
					}
					this.searchField += "aspect";
				}
				else if (this.itemCategory != ItemCategory.Booster)
				{
					this.searchField += "noaspect";
				}
				if (abstractEquipment.isCrafted)
				{
					this.searchField += Translation.Translate("@EquipCrafted", Array.Empty<object>());
				}
				if (abstractEquipment.salvageWorkShopSpent > 0)
				{
					this.searchField += Translation.Translate("@EquipWorkshopped", Array.Empty<object>());
					return;
				}
			}
			else if (base.TryGetComponent<MiningClaimItem>(out miningClaimItem))
			{
				this.searchField += miningClaimItem.GetAllOreNamesInField();
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000AF050 File Offset: 0x000AD250
		public void UpdateSearchFieldContent()
		{
			MiningClaimItem miningClaimItem;
			if (this.itemCategory == ItemCategory.Usable && base.TryGetComponent<MiningClaimItem>(out miningClaimItem) && !this.searchField.Contains(miningClaimItem.GetSystemName()))
			{
				this.searchField += miningClaimItem.GetSystemName();
			}
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000AF09C File Offset: 0x000AD29C
		public virtual JsonValue ToJson()
		{
			if (this.itemBuilder)
			{
				return this.itemBuilder.ToJson(this);
			}
			if (this.equipmentBuilder)
			{
				return this.equipmentBuilder.ToJson(this);
			}
			return this.identifier;
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000AF0E8 File Offset: 0x000AD2E8
		public virtual void ItemDataFromJson(JsonObject data)
		{
			this.DataFromJson(data);
			this.displayName = data["displayName"];
			if (!data["description"].IsNull)
			{
				this.description = data["description"];
			}
			this.baseCost = data["baseCost"];
			if (this.itemCategory == ItemCategory.Usable)
			{
				if (base.GetComponent<UsableItem>())
				{
					base.GetComponent<UsableItem>().DataFromJson(data);
					return;
				}
				Debug.LogWarning("Expected UsableData on usable item, but none found.");
			}
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000AF184 File Offset: 0x000AD384
		public virtual void EquipmentDataFromJson(JsonObject data)
		{
			this.DataFromJson(data);
			this.SetVisual(this.equipmentBuilder.GetVisual(data["visual"]));
			this.SetDisplayName(data["displayName"]);
			if (!data["description"].IsNull)
			{
				this.description = data["description"];
			}
			this.baseCost = data["baseCost"];
			AbstractEquipment component = base.GetComponent<AbstractEquipment>();
			if (component)
			{
				component.DataFromJson(data);
			}
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000AF228 File Offset: 0x000AD428
		private void DataFromJson(JsonObject data)
		{
			this.rarity = Enum.Parse<Rarity>(data["rarity"]);
			this.itemLevel = data["itemLevel"].AsInteger;
			if (!data["cs"].IsNull)
			{
				this.canSell = data["cs"].AsBoolean;
			}
			if (!data["cj"].IsNull)
			{
				this.canJettison = data["cj"].AsBoolean;
			}
			if (!data["favourite"].IsNull)
			{
				this.favouriteItem = data["favourite"].AsBoolean;
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x000AF2F4 File Offset: 0x000AD4F4
		public virtual void DataToJson(JsonObject data)
		{
			if (this.itemCategory == ItemCategory.Usable)
			{
				if (base.GetComponent<UsableItem>())
				{
					base.GetComponent<UsableItem>().DataToJson(data);
				}
				else
				{
					Debug.LogWarning("Expected UsableData on usable item, but none found.");
				}
			}
			data["displayName"] = this.displayName;
			if (this.description != "")
			{
				data["description"] = this.description;
			}
			data["baseCost"] = new double?((double)this.baseCost);
			data["rarity"] = this.rarity.ToString();
			string key = "visual";
			EquipmentBuilderVisual visual = this.visual;
			data[key] = ((visual != null) ? visual.name : null);
			data["itemLevel"] = new double?((double)this.itemLevel);
			data["favourite"] = new bool?(this.favouriteItem);
			if (!this.canSell)
			{
				data["cs"] = new bool?(false);
			}
			if (!this.canJettison)
			{
				data["cj"] = new bool?(false);
			}
			AbstractEquipment component = base.GetComponent<AbstractEquipment>();
			if (component)
			{
				component.DataToJson(data);
			}
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000AF45C File Offset: 0x000AD65C
		public static InventoryItemType FromJson(JsonValue data)
		{
			InventoryItemType result;
			if (data.IsJsonObject)
			{
				if (!data["equipmentType"].IsNull)
				{
					result = EquipmentBuilder.Get(data["equipmentType"]).CreateItemType(data);
				}
				else
				{
					result = ItemBuilder.Get(data["itemType"]).CreateItemType(data);
				}
			}
			else
			{
				result = data.AsString;
			}
			return result;
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x000AF4DE File Offset: 0x000AD6DE
		public static InventoryItemType FromJson(string key, JsonValue data)
		{
			return InventoryItemType.FromJson(data);
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000AF4E6 File Offset: 0x000AD6E6
		public static InventoryItemType Get(string name)
		{
			return InventoryItemType.allItems[name];
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x000AF4F3 File Offset: 0x000AD6F3
		public static bool TryGet(string itemName, out InventoryItemType item)
		{
			return InventoryItemType.allItems.TryGetValue(itemName, out item);
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x000AF501 File Offset: 0x000AD701
		public static implicit operator string(InventoryItemType iit)
		{
			return iit.identifier;
		}

		// Token: 0x06001D31 RID: 7473 RVA: 0x000AF509 File Offset: 0x000AD709
		public static implicit operator InventoryItemType(string id)
		{
			return InventoryItemType.Get(id);
		}

		// Token: 0x06001D32 RID: 7474 RVA: 0x000AF514 File Offset: 0x000AD714
		public static InventoryItemType GetRandom()
		{
			int count = InventoryItemType.allItems.Count;
			int index = UnityEngine.Random.Range(0, count);
			string key = InventoryItemType.allItems.Keys.ToList<string>()[index];
			return InventoryItemType.allItems[key];
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x000AF558 File Offset: 0x000AD758
		public static void LoadAll()
		{
			InventoryItemType.allItems.Clear();
			InventoryItemType[] array = Resources.LoadAll<InventoryItemType>("Items");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					array[i].identifier = array[i].name;
					InventoryItemType.allItems[array[i].identifier] = array[i];
					array[i].InitializeItem(null);
				}
			}
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x000AF5CC File Offset: 0x000AD7CC
		public static Dictionary<string, InventoryItemType> GetAllItemsInCategory(ItemCategory itemCategory)
		{
			Dictionary<string, InventoryItemType> dictionary = new Dictionary<string, InventoryItemType>();
			foreach (KeyValuePair<string, InventoryItemType> keyValuePair in InventoryItemType.allItems)
			{
				if (keyValuePair.Value.itemCategory == itemCategory)
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return dictionary;
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000AF644 File Offset: 0x000AD844
		public static int GetMagSizeForAmmo(InventoryItemType ammo)
		{
			foreach (KeyValuePair<string, InventoryItemType> keyValuePair in InventoryItemType.allItems)
			{
				if (keyValuePair.Value.itemCategory == ItemCategory.Turret)
				{
					AbstractTurret component = keyValuePair.Value.GetComponent<AbstractTurret>();
					if (component.ammoType == ammo)
					{
						return component.GetMaxMagSize();
					}
				}
			}
			return 1;
		}

		// Token: 0x06001D36 RID: 7478 RVA: 0x000AF6C8 File Offset: 0x000AD8C8
		public static List<InventoryItemType> GetRefinedItemsWithDropChance(int maxLevel, SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			List<InventoryItemType> list = new List<InventoryItemType>();
			foreach (InventoryItemType inventoryItemType in InventoryItemType.all)
			{
				if (inventoryItemType.itemCategory == ItemCategory.RefinedProduct && !inventoryItemType.missionItem && inventoryItemType.itemLevel < maxLevel)
				{
					Rarity rarity = inventoryItemType.rarity;
					float num;
					if (rarity != Rarity.Standard)
					{
						if (rarity != Rarity.Enhanced)
						{
							num = 0.1f;
						}
						else
						{
							num = 0.3f;
						}
					}
					else
					{
						num = 1f;
					}
					float chanceOfTrue = num;
					if (random.RandomBool(chanceOfTrue))
					{
						list.Add(inventoryItemType);
					}
				}
			}
			return list;
		}

		// Token: 0x06001D37 RID: 7479 RVA: 0x000AF778 File Offset: 0x000AD978
		public static InventoryItemType PotentiallyGetCrystalItem(int poiLevel, SeededRandom random)
		{
			float num = 12f;
			float num2 = 25f;
			float a = 0.001f;
			float b = 0.02f;
			float t = Mathf.Clamp01(((float)poiLevel - num) / (num2 - num));
			float chanceOfTrue = Mathf.Lerp(a, b, t);
			if (random.RandomBool(chanceOfTrue))
			{
				return InventoryItemType.GetCrystalItem(random);
			}
			return null;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000AF7C8 File Offset: 0x000AD9C8
		public static InventoryItemType GetCrystalItem(SeededRandom random)
		{
			string[] list = new string[]
			{
				"BallisticCrystal",
				"EnergyCrystal",
				"KineticCrystal",
				"ModuleCrystal"
			};
			return InventoryItemType.Get(random.Choose<string>(list));
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000AF808 File Offset: 0x000ADA08
		public bool CanGoInArmory()
		{
			if (this.criticalItem)
			{
				return true;
			}
			switch (this.storageOverride)
			{
			case StorageOverride.Armory:
				return true;
			case StorageOverride.Materials:
				return false;
			}
			switch (this.itemCategory)
			{
			case ItemCategory.Ammo:
				return true;
			case ItemCategory.Turret:
				return true;
			case ItemCategory.Module:
				return true;
			case ItemCategory.Booster:
				return true;
			case ItemCategory.Drone:
				return true;
			case ItemCategory.Torpedo:
				return true;
			case ItemCategory.JumpgatePass:
				return true;
			case ItemCategory.Usable:
				return true;
			case ItemCategory.Currency:
				return true;
			}
			return false;
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000AF8B4 File Offset: 0x000ADAB4
		public bool CanGoInMaterials()
		{
			if (this.criticalItem)
			{
				return false;
			}
			switch (this.storageOverride)
			{
			case StorageOverride.Armory:
				return false;
			case StorageOverride.Materials:
				return true;
			}
			ItemCategory itemCategory = this.itemCategory;
			if (itemCategory <= ItemCategory.RefinedProduct)
			{
				if (itemCategory == ItemCategory.Ore)
				{
					return true;
				}
				if (itemCategory == ItemCategory.Junk)
				{
					return true;
				}
				if (itemCategory == ItemCategory.RefinedProduct)
				{
					return true;
				}
			}
			else
			{
				if (itemCategory == ItemCategory.TradeGoods)
				{
					return true;
				}
				if (itemCategory == ItemCategory.Salvage)
				{
					return true;
				}
				if (itemCategory == ItemCategory.Crystal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000AF934 File Offset: 0x000ADB34
		public bool CanGoInWorkshop()
		{
			if (this.criticalItem)
			{
				return false;
			}
			ItemCategory itemCategory = this.itemCategory;
			return itemCategory == ItemCategory.Turret || itemCategory == ItemCategory.Module;
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000AF968 File Offset: 0x000ADB68
		public bool CanRefinedProductGoInWorkshop()
		{
			return !this.criticalItem && !this.missionItem && GamePlayer.current != null && this.itemLevel <= GamePlayer.current.level && GamePlayer.current.salvageWorkshopTradeInMaterialsUnlocked && this.itemCategory == ItemCategory.RefinedProduct;
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000AF9C5 File Offset: 0x000ADBC5
		public bool HasInfiniteShopSupply()
		{
			return this.itemCategory == ItemCategory.Ammo;
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000AF9D0 File Offset: 0x000ADBD0
		public bool CanFavourite()
		{
			return this.equipmentBuilder != null || this.itemBuilder != null;
		}

		// Token: 0x040011DA RID: 4570
		private static Dictionary<string, InventoryItemType> allItems = new Dictionary<string, InventoryItemType>();

		// Token: 0x040011E9 RID: 4585
		[SerializeField]
		public float sellValueMultiplier = 1f;

		// Token: 0x040011EA RID: 4586
		public bool favouriteItem;

		// Token: 0x040011F1 RID: 4593
		private float calcCost = -1f;

		// Token: 0x040011F2 RID: 4594
		private bool isCraftedItem;
	}
}
