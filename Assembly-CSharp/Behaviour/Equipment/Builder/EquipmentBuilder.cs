using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Equipment.Aspect;
using Behaviour.Equipment.Module;
using Behaviour.Item;
using Behaviour.UI.DebugScreen;
using Behaviour.Util;
using LightJson;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Builder
{
	// Token: 0x02000373 RID: 883
	public class EquipmentBuilder : MonoBehaviour
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06002208 RID: 8712 RVA: 0x000C5710 File Offset: 0x000C3910
		public static IEnumerable<EquipmentBuilder> availableBuilders
		{
			get
			{
				foreach (EquipmentBuilder equipmentBuilder in EquipmentBuilder.allBuilders.Values)
				{
					if (equipmentBuilder.AvailableForPlayer())
					{
						yield return equipmentBuilder;
					}
				}
				Dictionary<string, EquipmentBuilder>.ValueCollection.Enumerator enumerator = default(Dictionary<string, EquipmentBuilder>.ValueCollection.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06002209 RID: 8713 RVA: 0x000C5719 File Offset: 0x000C3919
		// (set) Token: 0x0600220A RID: 8714 RVA: 0x000C5721 File Offset: 0x000C3921
		public string identifier { get; private set; }

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000C572A File Offset: 0x000C392A
		// (set) Token: 0x0600220C RID: 8716 RVA: 0x000C5732 File Offset: 0x000C3932
		public int minLevel { get; private set; }

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000C573B File Offset: 0x000C393B
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x000C5743 File Offset: 0x000C3943
		public int maxLevel { get; private set; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x0600220F RID: 8719 RVA: 0x000C574C File Offset: 0x000C394C
		// (set) Token: 0x06002210 RID: 8720 RVA: 0x000C5754 File Offset: 0x000C3954
		public bool rarityStandard { get; private set; } = true;

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06002211 RID: 8721 RVA: 0x000C575D File Offset: 0x000C395D
		// (set) Token: 0x06002212 RID: 8722 RVA: 0x000C5765 File Offset: 0x000C3965
		public bool rarityEnhanced { get; private set; } = true;

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002213 RID: 8723 RVA: 0x000C576E File Offset: 0x000C396E
		// (set) Token: 0x06002214 RID: 8724 RVA: 0x000C5776 File Offset: 0x000C3976
		public bool rarityHighGrade { get; private set; } = true;

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000C577F File Offset: 0x000C397F
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x000C5787 File Offset: 0x000C3987
		public bool rarityExotic { get; private set; } = true;

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x000C5790 File Offset: 0x000C3990
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x000C5798 File Offset: 0x000C3998
		public bool rarityLegendary { get; private set; } = true;

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x000C57A1 File Offset: 0x000C39A1
		public ModuleSize equipmentSize
		{
			get
			{
				return this.equipment.size;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x0600221A RID: 8730 RVA: 0x000C57AE File Offset: 0x000C39AE
		public EquipmentSlot slot
		{
			get
			{
				return this.equipment.slot;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600221B RID: 8731 RVA: 0x000C57BB File Offset: 0x000C39BB
		public InventoryItemType prefab
		{
			get
			{
				return this.baseItem;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x000C57C3 File Offset: 0x000C39C3
		public IEnumerable<EquipStat> availableStats
		{
			get
			{
				foreach (EquipmentBuilderStat equipmentBuilderStat in this.mainStats)
				{
					yield return equipmentBuilderStat.stat;
				}
				List<EquipmentBuilderStat>.Enumerator enumerator = default(List<EquipmentBuilderStat>.Enumerator);
				foreach (EquipmentBuilderStat equipmentBuilderStat2 in this.optionalStats)
				{
					yield return equipmentBuilderStat2.stat;
				}
				enumerator = default(List<EquipmentBuilderStat>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000C57D4 File Offset: 0x000C39D4
		public void Setup()
		{
			this.mainStats = new List<EquipmentBuilderStat>();
			this.optionalStats = new List<EquipmentBuilderStat>();
			this.equipment = this.baseItem.GetComponent<AbstractEquipment>();
			this.visuals = base.GetComponentsInChildren<EquipmentBuilderVisual>();
			this.fields = base.GetComponentsInChildren<EquipmentBuilderField>();
			foreach (EquipAspect equipAspect in base.GetComponentsInChildren<EquipAspect>())
			{
				if (equipAspect.common)
				{
					this.commonAspects.Add(equipAspect);
				}
				else
				{
					this.rareAspects.Add(equipAspect);
				}
			}
			foreach (EquipmentBuilderStat equipmentBuilderStat in base.GetComponentsInChildren<EquipmentBuilderStat>())
			{
				if (equipmentBuilderStat.isMainStat)
				{
					this.mainStats.Add(equipmentBuilderStat);
				}
				else
				{
					this.optionalStats.Add(equipmentBuilderStat);
				}
			}
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000C589C File Offset: 0x000C3A9C
		public EquipmentBuilderVisual GetVisual(string name)
		{
			foreach (EquipmentBuilderVisual equipmentBuilderVisual in this.visuals)
			{
				if (equipmentBuilderVisual.name == name)
				{
					return equipmentBuilderVisual;
				}
			}
			if (name == "Test")
			{
				return this.GetVisual("Default");
			}
			return null;
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000C58EC File Offset: 0x000C3AEC
		public Rarity IsRarityAvailable(Rarity rarity)
		{
			bool flag;
			switch (rarity)
			{
			case Rarity.Standard:
				flag = this.rarityStandard;
				break;
			case Rarity.Enhanced:
				flag = this.rarityEnhanced;
				break;
			case Rarity.HighGrade:
				flag = this.rarityHighGrade;
				break;
			case Rarity.Exotic:
				flag = this.rarityExotic;
				break;
			case Rarity.Legendary:
				flag = this.rarityLegendary;
				break;
			default:
				flag = true;
				break;
			}
			if (flag)
			{
				return rarity;
			}
			if (this.rarityStandard)
			{
				return Rarity.Standard;
			}
			if (this.rarityEnhanced)
			{
				return Rarity.Enhanced;
			}
			if (this.rarityHighGrade)
			{
				return Rarity.HighGrade;
			}
			if (this.rarityExotic)
			{
				return Rarity.Exotic;
			}
			if (this.rarityLegendary)
			{
				return Rarity.Legendary;
			}
			return Rarity.Standard;
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000C597C File Offset: 0x000C3B7C
		public InventoryItemType CreateItemType(Rarity rarity, int level, bool exactLevel = false, string seed = null, bool ignoreLevelCap = false, bool isCrafted = false)
		{
			rarity = this.IsRarityAvailable(rarity);
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.equipmentBuilderRoot);
			inventoryItemType.equipmentBuilder = this;
			inventoryItemType.SetRarity(rarity);
			SeededRandom seededRandom = new SeedGenerator().Add(seed ?? ItemHelper.RandomItemSeed()).CreateRandom();
			int num = Math.Max(1, level + seededRandom.RandomRange(-1, 2));
			if (exactLevel)
			{
				num = level;
			}
			int num2 = (GamePlayer.current == null) ? 1 : GamePlayer.current.level;
			if (num > num2 && !ignoreLevelCap)
			{
				int num3 = num - num2;
				num = num2 + Mathf.CeilToInt(Mathf.Pow((float)num3, 0.7f));
			}
			inventoryItemType.SetItemLevel(num);
			this.SetupItem(inventoryItemType, seededRandom, isCrafted);
			return inventoryItemType;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000C5A30 File Offset: 0x000C3C30
		private void SetupItem(InventoryItemType item, SeededRandom random, bool isCrafted = false)
		{
			int itemLevel = item.itemLevel;
			Rarity rarity = item.rarity;
			int num = Mathf.RoundToInt(25f * GameMath.DamageMultiplier((float)itemLevel));
			List<EquipmentBuilderVisual> list = new List<EquipmentBuilderVisual>();
			foreach (EquipmentBuilderVisual equipmentBuilderVisual in this.visuals)
			{
				if (equipmentBuilderVisual.availableRarity == rarity)
				{
					list.Add(equipmentBuilderVisual);
				}
			}
			item.SetVisual((list.Count > 0) ? random.Choose<EquipmentBuilderVisual>(list) : this.visuals[0]);
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			component.SetCrafted(isCrafted);
			foreach (EquipmentBuilderField equipmentBuilderField in this.fields)
			{
				float num2 = random.RandomRange(equipmentBuilderField.minValue, equipmentBuilderField.maxValue);
				ReactorModule reactorModule;
				if (equipmentBuilderField.field == "capacityCost" && !this.equipment.TryGetComponent<ReactorModule>(out reactorModule))
				{
					ModuleSize size = this.equipment.size;
					if (size != ModuleSize.Medium)
					{
						if (size == ModuleSize.Large)
						{
							num2 *= 3f;
						}
					}
					else
					{
						num2 *= 2f;
					}
				}
				if (equipmentBuilderField.levelScaling > 0f)
				{
					float num3 = Mathf.Pow((float)num, equipmentBuilderField.levelScaling);
					num2 *= num3;
				}
				if (equipmentBuilderField.rarityScaling > 0f)
				{
					float num4 = Mathf.Pow(rarity.GetPowerMultiplier(), equipmentBuilderField.rarityScaling);
					num2 *= num4;
				}
				num2 = (float)Math.Round((double)num2, 2);
				component.SetDynamicField(equipmentBuilderField.field, num2);
			}
			List<EquipmentBuilderStat> list2 = (from s in new List<EquipmentBuilderStat>(this.optionalStats)
			where (float)itemLevel >= s.minSpawnLevel
			orderby random.RandomFloat() / s.spawnWeight
			select s).ToList<EquipmentBuilderStat>();
			int num5 = rarity.GetStatCount();
			for (int j = 0; j < this.mainStats.Count; j++)
			{
				list2.Insert(0, this.mainStats[j]);
				num5++;
			}
			int num6 = 0;
			while (num6 < num5 && num6 < list2.Count)
			{
				EquipmentBuilderStat equipmentBuilderStat = list2[num6];
				float num7 = random.RandomRange(equipmentBuilderStat.minValue, equipmentBuilderStat.maxValue);
				if (this.AutoScaleBySize(equipmentBuilderStat))
				{
					ModuleSize size = this.equipment.size;
					if (size != ModuleSize.Medium)
					{
						if (size == ModuleSize.Large)
						{
							num7 *= 2.25f;
						}
					}
					else
					{
						num7 *= 1.5f;
					}
				}
				if (equipmentBuilderStat.levelScaling > 0f)
				{
					float num8 = Mathf.Pow((float)num, equipmentBuilderStat.levelScaling);
					num7 *= num8;
				}
				if (equipmentBuilderStat.rarityScaling > 0f)
				{
					float num9 = Mathf.Pow(rarity.GetPowerMultiplier(), equipmentBuilderStat.rarityScaling);
					num7 *= num9;
				}
				if (equipmentBuilderStat.isMultiplier)
				{
					component.stats.Add(new EquipStatLine(equipmentBuilderStat.stat, 0f, 1f + num7, true));
				}
				else
				{
					component.stats.Add(new EquipStatLine(equipmentBuilderStat.stat, num7, 1f, true));
				}
				num6++;
			}
			this.SetAspectSlots(rarity, random, component);
			item.SetBaseValue(Mathf.RoundToInt(EquipmentBuilder.GetBaseValue(item) * (float)GameMath.GetCreditsValue(this.valueMultiplier, itemLevel) * rarity.GetCostMultiplier() * random.RandomRange(0.9f, 1.2f)));
			if (item.visual.useSuffix)
			{
				item.SetDisplayName(item.displayName + " " + ItemHelper.GetMkDesignation(itemLevel));
			}
			item.InitializeItem(null);
			this.AddCustomContent(component, random);
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000C5E08 File Offset: 0x000C4008
		private bool AutoScaleBySize(EquipmentBuilderStat stat)
		{
			return stat.stat != EquipStat.EnergyCapacity && !stat.stat.IsPercentageStat() && (!stat.isMainStat || (stat.stat != EquipStat.CombatPower && stat.stat != EquipStat.MiningPower && stat.stat != EquipStat.SalvagePower));
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000C5E5C File Offset: 0x000C405C
		private ValueTuple<int, int> GetAspectSlotAmount(Rarity rarity, SeededRandom random)
		{
			int item = 0;
			int num = 0;
			switch (rarity)
			{
			case Rarity.Standard:
				item = 0;
				num = 0;
				break;
			case Rarity.Enhanced:
				item = (random.RandomBool(0.25f) ? 1 : 0);
				break;
			case Rarity.HighGrade:
				num = (random.RandomBool(0.25f) ? 1 : 0);
				item = ((num == 0) ? 1 : 0);
				break;
			case Rarity.Exotic:
				num = 1;
				item = (random.RandomBool(0.25f) ? 1 : 0);
				break;
			case Rarity.Legendary:
				if (random.RandomBool(0.25f))
				{
					num = 2;
					item = 0;
				}
				else
				{
					num = 1;
					item = 1;
				}
				break;
			}
			return new ValueTuple<int, int>(item, num);
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x000C5EF4 File Offset: 0x000C40F4
		private void SetAspectSlots(Rarity rarity, SeededRandom random, AbstractEquipment equip)
		{
			ValueTuple<int, int> aspectSlotAmount = this.GetAspectSlotAmount(rarity, random);
			int item = aspectSlotAmount.Item1;
			int item2 = aspectSlotAmount.Item2;
			int num = item;
			int num2 = item2;
			int num3 = item + item2;
			for (int i = 0; i < num3; i++)
			{
				equip.AddAspectSlot();
			}
			foreach (AspectSlot aspectSlot in equip.aspectSlots)
			{
				if (num2 > 0)
				{
					if (this.rareAspects.Count > 0)
					{
						aspectSlot.SetEquipAspect(random.Choose<EquipAspect>(this.rareAspects));
					}
					else if (this.commonAspects.Count > 0)
					{
						aspectSlot.SetEquipAspect(random.Choose<EquipAspect>(this.commonAspects));
					}
					num2--;
				}
				else if (num > 0)
				{
					if (this.commonAspects.Count > 0)
					{
						aspectSlot.SetEquipAspect(random.Choose<EquipAspect>(this.commonAspects));
					}
					num--;
				}
			}
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x000C5FF4 File Offset: 0x000C41F4
		public void AddRandomStat(InventoryItemType item, int index, bool replace = true)
		{
			SeededRandom random = SeededRandom.Global;
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			List<EquipmentBuilderStat> list = (from s in new List<EquipmentBuilderStat>(this.optionalStats)
			where (float)item.itemLevel >= s.minSpawnLevel
			orderby random.RandomFloat() / s.spawnWeight
			select s).ToList<EquipmentBuilderStat>();
			IEnumerable<ValueTuple<EquipStatLine, int>> statsWithIndex = component.GetStatsWithIndex(false);
			int num = 50;
			int i = 0;
			EquipmentBuilderStat equipmentBuilderStat = list[0];
			while (i < num)
			{
				EquipmentBuilderStat equipmentBuilderStat2 = list[random.RandomRange(0, list.Count)];
				bool flag = false;
				foreach (ValueTuple<EquipStatLine, int> valueTuple in statsWithIndex)
				{
					if (equipmentBuilderStat2.stat == valueTuple.Item1.stat)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					equipmentBuilderStat = equipmentBuilderStat2;
					break;
				}
				i++;
			}
			float num2 = random.RandomRange(equipmentBuilderStat.minValue, equipmentBuilderStat.maxValue);
			if (this.AutoScaleBySize(equipmentBuilderStat))
			{
				ModuleSize size = component.size;
				if (size != ModuleSize.Medium)
				{
					if (size == ModuleSize.Large)
					{
						num2 *= 2.25f;
					}
				}
				else
				{
					num2 *= 1.5f;
				}
			}
			int num3 = Mathf.RoundToInt(25f * GameMath.DamageMultiplier((float)item.itemLevel));
			if (equipmentBuilderStat.levelScaling > 0f)
			{
				float num4 = Mathf.Pow((float)num3, equipmentBuilderStat.levelScaling);
				num2 *= num4;
			}
			if (equipmentBuilderStat.rarityScaling > 0f)
			{
				float num5 = Mathf.Pow(item.rarity.GetPowerMultiplier(), equipmentBuilderStat.rarityScaling);
				num2 *= num5;
			}
			if (equipmentBuilderStat.isMultiplier)
			{
				if (replace)
				{
					component.stats.Insert(index, new EquipStatLine(equipmentBuilderStat.stat, 0f, 1f + num2, true));
					return;
				}
				component.stats.Add(new EquipStatLine(equipmentBuilderStat.stat, 0f, 1f + num2, true));
				return;
			}
			else
			{
				if (replace)
				{
					component.stats.Insert(index, new EquipStatLine(equipmentBuilderStat.stat, num2, 1f, true));
					return;
				}
				component.stats.Add(new EquipStatLine(equipmentBuilderStat.stat, num2, 1f, true));
				return;
			}
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x000C625C File Offset: 0x000C445C
		public InventoryItemType RebuildItemWithAspect(InventoryItemType item, EquipAspect replaceAspect = null, int aspectIndex = 0)
		{
			return this.RebuildItem(item, null, null, 0, replaceAspect, aspectIndex);
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x000C6284 File Offset: 0x000C4484
		public InventoryItemType RebuildItemWithLevel(InventoryItemType item, int level)
		{
			return this.RebuildItem(item, null, null, level, null, 0);
		}

		// Token: 0x06002228 RID: 8744 RVA: 0x000C62A7 File Offset: 0x000C44A7
		public InventoryItemType RebuildItemWithRarity(InventoryItemType item, Rarity rarity)
		{
			return this.RebuildItem(item, new Rarity?(rarity), null, 0, null, 0);
		}

		// Token: 0x06002229 RID: 8745 RVA: 0x000C62BC File Offset: 0x000C44BC
		public InventoryItemType RebuildItem(InventoryItemType oldItem, Rarity? newRarity = null, SeededRandom seed = null, int level = 0, EquipAspect replaceAspect = null, int aspectIndex = 0)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.equipmentBuilderRoot);
			inventoryItemType.equipmentBuilder = this;
			SeededRandom seededRandom = seed ?? SeededRandom.Global;
			int num = oldItem.itemLevel;
			if (level != 0)
			{
				num = level;
			}
			inventoryItemType.SetItemLevel(num);
			int num2 = Mathf.RoundToInt(25f * GameMath.DamageMultiplier((float)num));
			inventoryItemType.SetRarity(newRarity ?? oldItem.rarity);
			Rarity rarity = inventoryItemType.rarity;
			inventoryItemType.SetVisual(oldItem.visual);
			inventoryItemType.SetDisplayName(oldItem.displayName);
			AbstractEquipment component = oldItem.GetComponent<AbstractEquipment>();
			AbstractEquipment component2 = inventoryItemType.GetComponent<AbstractEquipment>();
			component2.SetCrafted(component.isCrafted);
			foreach (EquipmentBuilderField equipmentBuilderField in this.fields)
			{
				float num3 = seededRandom.RandomRange(equipmentBuilderField.minValue, equipmentBuilderField.maxValue);
				ReactorModule reactorModule;
				if (equipmentBuilderField.field == "capacityCost" && !this.equipment.TryGetComponent<ReactorModule>(out reactorModule))
				{
					ModuleSize size = this.equipment.size;
					if (size != ModuleSize.Medium)
					{
						if (size == ModuleSize.Large)
						{
							num3 *= 3f;
						}
					}
					else
					{
						num3 *= 2f;
					}
				}
				if (equipmentBuilderField.levelScaling > 0f)
				{
					float num4 = Mathf.Pow((float)num2, equipmentBuilderField.levelScaling);
					num3 *= num4;
				}
				if (equipmentBuilderField.rarityScaling > 0f)
				{
					float num5 = Mathf.Pow(rarity.GetPowerMultiplier(), equipmentBuilderField.rarityScaling);
					num3 *= num5;
				}
				num3 = (float)Math.Round((double)num3, 2);
				component2.SetDynamicField(equipmentBuilderField.field, num3);
			}
			DroneBayModule droneBayModule = component2 as DroneBayModule;
			if (droneBayModule != null)
			{
				DroneBayModule droneBayModule2 = component as DroneBayModule;
				if (droneBayModule2 != null)
				{
					droneBayModule.droneSeed = droneBayModule2.droneSeed;
					goto IL_209;
				}
			}
			TractorModule tractorModule = component2 as TractorModule;
			if (tractorModule != null)
			{
				TractorModule tractorModule2 = component as TractorModule;
				if (tractorModule2 != null)
				{
					tractorModule.amountOfBeams = tractorModule2.amountOfBeams;
					tractorModule.amountOfBonusBeams = tractorModule2.amountOfBonusBeams;
				}
			}
			IL_209:
			component2.SetAspectSlots(component.aspectSlots);
			if (replaceAspect == null)
			{
				if (newRarity != null && newRarity != null)
				{
					ValueTuple<int, int> aspectSlotAmount = this.GetAspectSlotAmount(rarity, seededRandom);
					int item = aspectSlotAmount.Item1;
					int item2 = aspectSlotAmount.Item2;
					int num6 = item + item2;
					int count = component2.aspectSlots.Count;
					int num7 = num6 - count;
					if (num7 > 0)
					{
						for (int j = 0; j < num7; j++)
						{
							component2.AddAspectSlot();
						}
					}
				}
			}
			else
			{
				this.ReplaceAspect(inventoryItemType, replaceAspect, aspectIndex);
			}
			inventoryItemType.InitializeItem(null);
			component2.SetStats(component.stats);
			int num8 = 0;
			for (int k = 0; k < component2.stats.Count; k++)
			{
				EquipStatLine stat = component2.stats[k];
				EquipmentBuilderStat equipmentBuilderStat = null;
				float num9 = 0f;
				MainStat mainStat = component2.GetMainStat();
				if (stat.stat.GetDisplayName() == mainStat.mainStatName)
				{
					if (stat.multiplier != 1f)
					{
						string b = mainStat.mainStatAmount + " " + Translation.Translate(mainStat.mainStatName, Array.Empty<object>());
						if (stat.ToReadableString(true) == b)
						{
							equipmentBuilderStat = this.mainStats.FirstOrDefault((EquipmentBuilderStat s) => s.stat == stat.stat);
							num9 = seededRandom.RandomRange(equipmentBuilderStat.minValue, equipmentBuilderStat.maxValue);
						}
					}
					else if (GameMath.FormatNumber(stat.amount, -1) == mainStat.mainStatAmount)
					{
						equipmentBuilderStat = this.mainStats.FirstOrDefault((EquipmentBuilderStat s) => s.stat == stat.stat);
						num9 = seededRandom.RandomRange(equipmentBuilderStat.minValue, equipmentBuilderStat.maxValue);
					}
				}
				EquipmentBuilderStat equipmentBuilderStat2 = this.optionalStats.FirstOrDefault((EquipmentBuilderStat s) => s.stat == stat.stat);
				if (equipmentBuilderStat)
				{
					equipmentBuilderStat2 = equipmentBuilderStat;
				}
				if (equipmentBuilderStat2 != null)
				{
					float num10 = seededRandom.RandomRange(equipmentBuilderStat2.minValue, equipmentBuilderStat2.maxValue);
					if (num9 > 0f)
					{
						num10 = num9;
					}
					if (this.AutoScaleBySize(equipmentBuilderStat2))
					{
						ModuleSize size = this.equipment.size;
						if (size != ModuleSize.Medium)
						{
							if (size == ModuleSize.Large)
							{
								num10 *= 2.25f;
							}
						}
						else
						{
							num10 *= 1.5f;
						}
					}
					if (equipmentBuilderStat2.levelScaling > 0f)
					{
						num10 *= Mathf.Pow((float)num2, equipmentBuilderStat2.levelScaling);
					}
					if (equipmentBuilderStat2.rarityScaling > 0f)
					{
						num10 *= Mathf.Pow(rarity.GetPowerMultiplier(), equipmentBuilderStat2.rarityScaling);
					}
					if (equipmentBuilderStat2.isMultiplier)
					{
						float multiplier = Mathf.Max(stat.multiplier, num10);
						component2.stats[k] = new EquipStatLine(stat.stat, 0f, multiplier, !(equipmentBuilderStat != null) && stat.canReroll);
					}
					else
					{
						float amt = Mathf.Max(stat.amount, num10);
						component2.stats[k] = new EquipStatLine(stat.stat, amt, 1f, !(equipmentBuilderStat != null) && stat.canReroll);
					}
				}
				else
				{
					component2.stats.Remove(stat);
					this.AddRandomStat(inventoryItemType, k, true);
				}
				num8++;
			}
			this.TryAddMoreStats(inventoryItemType, num8);
			component2.AddWorkshopPenalty(component.salvageWorkShopPenalty);
			component2.AddSalvageCreditsSpent(component.salvageWorkShopSpent);
			component2.AddItemChanged(component.salvageWorkShopItemChangedAmount);
			inventoryItemType.SetBaseValue(Mathf.RoundToInt(EquipmentBuilder.GetBaseValue(inventoryItemType) * (float)GameMath.GetCreditsValue(this.valueMultiplier, num) * rarity.GetCostMultiplier() * seededRandom.RandomRange(1.1f, 1.2f)));
			return inventoryItemType;
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x000C68D0 File Offset: 0x000C4AD0
		private void TryAddMoreStats(InventoryItemType item, int statAmount)
		{
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			int num = item.rarity.GetStatCount() - component.GetStatsWithIndex(false).Count<ValueTuple<EquipStatLine, int>>();
			for (int i = 0; i < num; i++)
			{
				this.AddRandomStat(item, i, false);
			}
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x000C6914 File Offset: 0x000C4B14
		public void ReplaceAspect(InventoryItemType item, EquipAspect aspect, int index)
		{
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			AspectSlot aspectSlot = component.aspectSlots.FirstOrDefault((AspectSlot i) => i.indexSlot == index);
			if (aspectSlot == null)
			{
				aspectSlot = component.aspectSlots[index];
			}
			aspectSlot.equipAspect = aspect;
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x000C696C File Offset: 0x000C4B6C
		public bool CanFitAspect(EquipAspect aspect)
		{
			if (aspect.common)
			{
				using (List<EquipAspect>.Enumerator enumerator = this.commonAspects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EquipAspect equipAspect = enumerator.Current;
						if (aspect.displayName == equipAspect.displayName)
						{
							return true;
						}
					}
					return false;
				}
			}
			foreach (EquipAspect equipAspect2 in this.rareAspects)
			{
				if (aspect.displayName == equipAspect2.displayName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000C6A2C File Offset: 0x000C4C2C
		private void AddCustomContent(AbstractEquipment item, SeededRandom random)
		{
			DroneBayModule droneBayModule = item as DroneBayModule;
			if (droneBayModule != null)
			{
				droneBayModule.droneSeed = random.RandomItemSeed();
				return;
			}
			TractorModule tractorModule = item as TractorModule;
			if (tractorModule != null)
			{
				tractorModule.amountOfBeams = Mathf.Max(tractorModule.NewBeamCount(item, random), tractorModule.amountOfBeams);
				tractorModule.amountOfBonusBeams = Mathf.Max(tractorModule.NewBeamCount(item, random), tractorModule.amountOfBonusBeams);
			}
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000C6A8C File Offset: 0x000C4C8C
		public bool AvailableForLevel(int level)
		{
			return level >= this.minLevel && (this.maxLevel <= 0 || level <= this.maxLevel);
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x000C6AB0 File Offset: 0x000C4CB0
		public bool AvailableForPlayer()
		{
			if (!this.playerAvailable)
			{
				return false;
			}
			using (List<Storyteller>.Enumerator enumerator = GamePlayer.current.storytellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.ItemIsPlayerAvailable(this))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x000C6B18 File Offset: 0x000C4D18
		public InventoryItemType CreateItemType(JsonObject data)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.equipmentBuilderRoot);
			inventoryItemType.equipmentBuilder = this;
			if (GamePlayer.current.recreateItems)
			{
				this.SetupItem(inventoryItemType, SeededRandom.Global, false);
			}
			else
			{
				inventoryItemType.EquipmentDataFromJson(data);
			}
			inventoryItemType.InitializeItem(null);
			return inventoryItemType;
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x000C6B6C File Offset: 0x000C4D6C
		public JsonValue ToJson(InventoryItemType type)
		{
			JsonObject jsonObject = new JsonObject();
			type.DataToJson(jsonObject);
			jsonObject["equipmentType"] = this.identifier;
			return jsonObject;
		}

		// Token: 0x06002232 RID: 8754 RVA: 0x000C6BA4 File Offset: 0x000C4DA4
		public static float GetBaseValue(InventoryItemType item)
		{
			float num = 120f;
			if (item.itemCategory == ItemCategory.Turret)
			{
				num *= 1.2f;
			}
			else if (item.itemCategory == ItemCategory.Module)
			{
				num *= 1.4f;
			}
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			if (component)
			{
				if (component.size == ModuleSize.Medium)
				{
					num *= 1.8f;
				}
				else if (component.size == ModuleSize.Large)
				{
					num *= 3.4f;
				}
			}
			return num;
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000C6C0F File Offset: 0x000C4E0F
		public static EquipmentBuilder Get(string name)
		{
			return EquipmentBuilder.allBuilders[name];
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000C6C1C File Offset: 0x000C4E1C
		public static EquipmentBuilder GetRandom(EquipmentBuilder.BuilderFilter filter, int level, bool forPlayer, SeededRandom random = null)
		{
			List<EquipmentBuilder> list = new List<EquipmentBuilder>();
			IEnumerable<EquipmentBuilder> enumerable;
			if (!forPlayer || Singleton<ConsoleScreen>.Instance.SkipLevelCheck())
			{
				IEnumerable<EquipmentBuilder> values = EquipmentBuilder.allBuilders.Values;
				enumerable = values;
			}
			else
			{
				enumerable = EquipmentBuilder.availableBuilders;
			}
			foreach (EquipmentBuilder equipmentBuilder in enumerable)
			{
				if (filter(equipmentBuilder) && equipmentBuilder.AvailableForLevel(level))
				{
					list.Add(equipmentBuilder);
				}
			}
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			return random.Choose<EquipmentBuilder>(list);
		}

		// Token: 0x06002235 RID: 8757 RVA: 0x000C6CB0 File Offset: 0x000C4EB0
		public static List<EquipmentBuilder> GetItemsWithStat(EquipStat[] statList, int level)
		{
			List<EquipmentBuilder> list = new List<EquipmentBuilder>();
			using (IEnumerator<EquipmentBuilder> enumerator = EquipmentBuilder.availableBuilders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					EquipmentBuilder builder = enumerator.Current;
					if (statList.Any((EquipStat stat) => builder.availableStats.Any((EquipStat a) => a == stat)) && builder.AvailableForLevel(level))
					{
						list.Add(builder);
					}
				}
			}
			return list;
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000C6D38 File Offset: 0x000C4F38
		public static List<EquipmentBuilder> GetItemsForGeneralShop(int level)
		{
			List<EquipmentBuilder> list = new List<EquipmentBuilder>();
			foreach (EquipmentBuilder equipmentBuilder in EquipmentBuilder.availableBuilders)
			{
				if (equipmentBuilder.AvailableForLevel(level))
				{
					list.Add(equipmentBuilder);
				}
			}
			return list;
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000C6D94 File Offset: 0x000C4F94
		public static List<EquipmentBuilder> GetItemsForSalvage(string shipName, int level)
		{
			return EquipmentBuilder.GetItemsForGeneralShop(level);
		}

		// Token: 0x06002238 RID: 8760 RVA: 0x000C6D9C File Offset: 0x000C4F9C
		public static List<EquipmentBuilder> GetItemsForMissionReward(Mission m)
		{
			return EquipmentBuilder.GetItemsForGeneralShop(m.level);
		}

		// Token: 0x06002239 RID: 8761 RVA: 0x000C6DAC File Offset: 0x000C4FAC
		public static void LoadAll()
		{
			EquipmentBuilder.allBuilders.Clear();
			EquipmentBuilder[] array = Resources.LoadAll<EquipmentBuilder>("EquipmentBuilder");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					array[i].identifier = array[i].name;
					array[i].Setup();
					EquipmentBuilder.allBuilders[array[i].identifier] = array[i];
				}
			}
		}

		// Token: 0x0400141E RID: 5150
		public const float BaseValueMultiplier = 120f;

		// Token: 0x0400141F RID: 5151
		private static Dictionary<string, EquipmentBuilder> allBuilders = new Dictionary<string, EquipmentBuilder>();

		// Token: 0x04001421 RID: 5153
		[SerializeField]
		private InventoryItemType baseItem;

		// Token: 0x04001422 RID: 5154
		[SerializeField]
		private float valueMultiplier = 1f;

		// Token: 0x04001423 RID: 5155
		[SerializeField]
		private bool playerAvailable = true;

		// Token: 0x0400142B RID: 5163
		private EquipmentBuilderVisual[] visuals;

		// Token: 0x0400142C RID: 5164
		private EquipmentBuilderField[] fields;

		// Token: 0x0400142D RID: 5165
		private List<EquipmentBuilderStat> mainStats;

		// Token: 0x0400142E RID: 5166
		private List<EquipmentBuilderStat> optionalStats;

		// Token: 0x0400142F RID: 5167
		private List<EquipAspect> commonAspects = new List<EquipAspect>();

		// Token: 0x04001430 RID: 5168
		private List<EquipAspect> rareAspects = new List<EquipAspect>();

		// Token: 0x04001431 RID: 5169
		private AbstractEquipment equipment;

		// Token: 0x020005DA RID: 1498
		// (Invoke) Token: 0x06002EF5 RID: 12021
		public delegate bool BuilderFilter(EquipmentBuilder builder);
	}
}
