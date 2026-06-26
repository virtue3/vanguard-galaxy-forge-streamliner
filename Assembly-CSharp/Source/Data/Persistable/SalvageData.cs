using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Salvage;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x0200011C RID: 284
	public class SalvageData : PersistableData
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000AC3 RID: 2755 RVA: 0x0004FB5B File Offset: 0x0004DD5B
		public int surfaceCount
		{
			get
			{
				return this.scrapContent.Sum((KeyValuePair<InventoryItemType, int> kv) => kv.Value);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0004FB87 File Offset: 0x0004DD87
		public int structureCount
		{
			get
			{
				return this.structuralContent.Sum((KeyValuePair<InventoryItemType, int> kv) => kv.Value);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0004FBB3 File Offset: 0x0004DDB3
		public int surfaceHealth
		{
			get
			{
				return this.healthPerScrap * this.surfaceCount;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0004FBC2 File Offset: 0x0004DDC2
		public int structureHealth
		{
			get
			{
				return this.healthPerStructuralItem * this.structureCount;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0004FBD4 File Offset: 0x0004DDD4
		public List<SalvageItemData> availableItemContent
		{
			get
			{
				int salvagingItemAmount = SalvageData.salvagingItemAmount;
				int num = this.nonCritEquipmentExtracted;
				List<SalvageItemData> list = new List<SalvageItemData>();
				foreach (SalvageItemData salvageItemData in this.itemContent)
				{
					if (!salvageItemData.criticalItem)
					{
						if (num != salvagingItemAmount)
						{
							list.Add(salvageItemData);
							num++;
						}
					}
					else
					{
						list.Add(salvageItemData);
					}
				}
				return list;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x0004FC58 File Offset: 0x0004DE58
		public static int salvagingItemAmount
		{
			get
			{
				return SkilltreeNode.SalvagingEquipmentAmount.currentPoints + 1;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0004FC66 File Offset: 0x0004DE66
		public string displayName
		{
			get
			{
				return Behaviour.Unit.SpaceShip.Get(this.shipTemplate).displayName;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x0004FC78 File Offset: 0x0004DE78
		public SalvageItemData activeItem
		{
			get
			{
				for (int i = 0; i < this.itemContent.Count; i++)
				{
					if (this.itemContent[i].active)
					{
						return this.itemContent[i];
					}
				}
				return null;
			}
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0004FCBC File Offset: 0x0004DEBC
		public int GetUnreachableItemCount()
		{
			int num = 0;
			int salvagingItemAmount = SalvageData.salvagingItemAmount;
			int num2 = this.nonCritEquipmentExtracted;
			using (List<SalvageItemData>.Enumerator enumerator = this.itemContent.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.criticalItem)
					{
						if (num2 >= salvagingItemAmount)
						{
							num++;
						}
						else
						{
							num2++;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0004FD2C File Offset: 0x0004DF2C
		public void AddItemContent(InventoryItemType item)
		{
			this.itemContent.Add(new SalvageItemData(item, this.GetBaseChanceForRarity(item.rarity, SeededRandom.Global), false));
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0004FD54 File Offset: 0x0004DF54
		public void AddItemContent(int level, int itemCount = -1, float itemRarity = 1f)
		{
			SeededRandom global = SeededRandom.Global;
			if (itemCount < 0)
			{
				itemCount = global.RandomRange(2, 3);
			}
			List<EquipmentBuilder> itemsForSalvage = EquipmentBuilder.GetItemsForSalvage(this.shipTemplate, level);
			float num = Mathf.Sqrt((float)level / 20f);
			for (int i = 0; i < itemCount; i++)
			{
				float num2 = global.RandomFloat() / itemRarity;
				Rarity rarity;
				if (level >= 60 && num2 < 0.05f * num)
				{
					rarity = Rarity.Legendary;
				}
				else if (level >= 25 && num2 < 0.08f * num)
				{
					rarity = Rarity.Exotic;
				}
				else if (num2 < 0.2f * num)
				{
					rarity = Rarity.HighGrade;
				}
				else if (num2 < 0.4f * num)
				{
					rarity = Rarity.Enhanced;
				}
				else
				{
					rarity = Rarity.Standard;
				}
				float baseChanceForRarity = this.GetBaseChanceForRarity(rarity, global);
				InventoryItemType inventoryItemType;
				do
				{
					inventoryItemType = global.Choose<EquipmentBuilder>(itemsForSalvage).CreateItemType(rarity, level, false, null, false, false);
				}
				while (inventoryItemType.rarity != rarity);
				this.itemContent.Add(new SalvageItemData(inventoryItemType, baseChanceForRarity, false));
			}
			if (global.RandomBool(0.2f))
			{
				InventoryItemType inventoryItemType2 = ItemBuilder.Get("Blueprint").CreateRandomBlueprint(level, null, null, true);
				if (inventoryItemType2 == null)
				{
					return;
				}
				float baseChanceForRarity2 = this.GetBaseChanceForRarity(inventoryItemType2.rarity, global);
				this.itemContent.Add(new SalvageItemData(inventoryItemType2, baseChanceForRarity2, false));
			}
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x0004FE94 File Offset: 0x0004E094
		public float GetBaseChanceForRarity(Rarity rarity, SeededRandom random)
		{
			float result;
			switch (rarity)
			{
			case Rarity.Enhanced:
				result = random.RandomRange(0.6f, 0.9f);
				break;
			case Rarity.HighGrade:
				result = random.RandomRange(0.4f, 0.7f);
				break;
			case Rarity.Exotic:
				result = random.RandomRange(0.3f, 0.5f);
				break;
			case Rarity.Legendary:
				result = random.RandomRange(0.2f, 0.3f);
				break;
			default:
				result = random.RandomRange(0.7f, 1.2f);
				break;
			}
			return result;
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x0004FF1C File Offset: 0x0004E11C
		public void AddScrapContent(int level, float valueMultiplier = 1f, int totalSalvageTypes = 2)
		{
			Dictionary<InventoryItemType, int> scrapTier = this.GetScrapTier(level, false);
			this.AddScrapToList(totalSalvageTypes, scrapTier, this.scrapContent);
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0004FF40 File Offset: 0x0004E140
		private void AddScrapToList(int totalSalvageTypes, Dictionary<InventoryItemType, int> scrapTier, Dictionary<InventoryItemType, int> content)
		{
			System.Random randomForDict = new System.Random();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in (from x in scrapTier
			orderby randomForDict.Next()
			select x).Take(totalSalvageTypes))
			{
				if (content.ContainsKey(keyValuePair.Key))
				{
					InventoryItemType key = keyValuePair.Key;
					content[key] += keyValuePair.Value;
				}
				else
				{
					content[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0004FFF4 File Offset: 0x0004E1F4
		public void AddStructuralContent(int level, int totalSalvageTypes = 2, float amountMultiplier = 1f)
		{
			Dictionary<InventoryItemType, int> scrapTier = this.GetScrapTier(level, true);
			this.AddScrapToList(totalSalvageTypes, scrapTier, this.structuralContent);
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("Bulkhead", 2);
			dictionary.Add("Hull Beam", 2);
			dictionary.Add("Computing Unit", 1);
			int num = SalvageHelper.GetSizeMultiplierStructure(this.GetShipSize());
			num = (int)((float)num * amountMultiplier);
			foreach (KeyValuePair<string, int> keyValuePair in dictionary)
			{
				this.structuralContent.Add(InventoryItemType.Get(keyValuePair.Key), SeededRandom.Global.RandomRange((keyValuePair.Value - 1) * num, keyValuePair.Value * num));
			}
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x000500C4 File Offset: 0x0004E2C4
		private Dictionary<InventoryItemType, int> GetScrapTier(int level, bool structural = false)
		{
			SeededRandom global = SeededRandom.Global;
			int shipSize = this.GetShipSize();
			List<string> list = SalvageHelper.surfaceMaterials;
			if (structural)
			{
				list = SalvageHelper.structuralMaterials;
			}
			Dictionary<InventoryItemType, int> dictionary = new Dictionary<InventoryItemType, int>();
			int num = structural ? 6 : 0;
			foreach (string material in list)
			{
				int tier = SalvageHelper.RollTier(level);
				int num2 = SalvageHelper.GetScrapAmount(level, tier, shipSize);
				if (structural)
				{
					num2 -= num;
				}
				dictionary.Add(InventoryItemType.Get(SalvageHelper.BuildScrapItemName(material, tier)), num2);
			}
			return dictionary;
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0005016C File Offset: 0x0004E36C
		private int GetShipSize()
		{
			return new SpaceShipData(Behaviour.Unit.SpaceShip.Get(this.shipTemplate), false, null).shipClass.shipRoleType.GetTypeSize();
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x00050190 File Offset: 0x0004E390
		public void InitHealth()
		{
			if (this.healthPerItem == 0 || this.healthPerScrap == 0)
			{
				this.maxHealth = 0;
				int num = 0;
				foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.scrapContent)
				{
					OreItemData component = keyValuePair.Key.GetComponent<OreItemData>();
					if (component)
					{
						this.maxHealth += component.health * keyValuePair.Value;
						num += keyValuePair.Value;
					}
				}
				this.maxHealth = ((this.maxHealth > 0) ? this.maxHealth : 1);
				this.healthPerItem = Mathf.CeilToInt((float)this.maxHealth / ((float)this.scrapContent.Count + 1f));
				this.healthPerScrap = this.maxHealth / ((num == 0) ? 1 : num);
			}
			if (this.healthPerStructuralItem == 0)
			{
				this.maxStructuralHealth = 0;
				int num2 = 0;
				foreach (KeyValuePair<InventoryItemType, int> keyValuePair2 in this.structuralContent)
				{
					OreItemData component2 = keyValuePair2.Key.GetComponent<OreItemData>();
					if (component2)
					{
						this.maxStructuralHealth += component2.health * keyValuePair2.Value;
						num2 += keyValuePair2.Value;
					}
				}
				this.maxStructuralHealth = ((this.maxStructuralHealth > 0) ? this.maxStructuralHealth : 1);
				this.healthPerItem = ((this.healthPerItem == 1) ? Mathf.CeilToInt((float)this.maxStructuralHealth / ((float)this.structuralContent.Count + 1f)) : this.healthPerItem);
				this.healthPerStructuralItem = this.maxStructuralHealth / ((num2 == 0) ? 1 : num2);
			}
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x00050378 File Offset: 0x0004E578
		public void SetActiveItem(InventoryItemType active)
		{
			foreach (SalvageItemData salvageItemData in this.itemContent)
			{
				salvageItemData.active = (salvageItemData.item == active);
			}
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000503D4 File Offset: 0x0004E5D4
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			if (this.angle == 0f)
			{
				this.angle = (float)SeededRandom.Global.RandomRange(0, 360);
			}
			Behaviour.Unit.SpaceShip spaceShip = Behaviour.Unit.SpaceShip.Get(this.shipTemplate);
			bool flag = false;
			if (this.shipData == null)
			{
				this.shipData = new SpaceShipData(spaceShip, false, null);
				this.shipData.ApplyRandomBattleDamage(10, (this.initialBattleDamage > 0) ? this.initialBattleDamage : 20);
				flag = SeededRandom.Global.RandomBool(0.5f);
			}
			Behaviour.Unit.SpaceShip spaceShip2 = UnityEngine.Object.Instantiate<Behaviour.Unit.SpaceShip>(spaceShip, this.position, base.rotation, parent.transform);
			spaceShip2.enabled = false;
			if (flag)
			{
				int filledPixelCount = AsteroidHelper.GetFilledPixelCount(spaceShip2.surfaceSprite.sprite);
				int size = SeededRandom.Global.RandomRange(filledPixelCount / 5, filledPixelCount / 3);
				Vector2 position = new Vector2((float)(SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * spaceShip2.GetBoundsX() / 2f, (float)(SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * spaceShip2.GetBoundsY() / 2f);
				this.shipData.battleDamage.Add(new SpriteBreakPoint(position, size, true));
			}
			spaceShip2.SetData(this.shipData, true, true);
			spaceShip2.isSalvage = true;
			spaceShip2.gameObject.AddComponent<SalvageContainer>().data = this;
			base.AddHazardToWorld(spaceShip2.gameObject, 2f);
			return spaceShip2.gameObject;
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0005054C File Offset: 0x0004E74C
		public bool HasSalvage(TargetLayer targetLayer)
		{
			if (targetLayer == TargetLayer.Core)
			{
				return this.HasItems() || this.HasStructuralContent();
			}
			if (targetLayer == TargetLayer.Surface)
			{
				return this.HasItems() || this.HasScrap();
			}
			return this.HasItems() || this.HasStructuralContent() || this.HasScrap();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0005059A File Offset: 0x0004E79A
		public bool HasItems()
		{
			return this.availableItemContent.Count > 0;
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x000505AC File Offset: 0x0004E7AC
		public bool HasScrap()
		{
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.scrapContent)
			{
				if (keyValuePair.Value > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0005060C File Offset: 0x0004E80C
		public bool HasStructuralContent()
		{
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.structuralContent)
			{
				if (keyValuePair.Value > 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0005066C File Offset: 0x0004E86C
		public bool TakeDamage(DamageData dmg, bool hitByPlayer)
		{
			this.scrapDamageBeforeChunk = 0;
			this.structuralDamageBeforeChunk = 0;
			bool flag = false;
			bool flag2 = this.ExtractItem(dmg);
			if (dmg.targetLayer == TargetLayer.Core && this.HasStructuralContent())
			{
				flag = this.StructuralDamage(dmg, hitByPlayer);
				dmg.yield = Mathf.Clamp(dmg.yield, 0.5f, dmg.yield - 0.5f);
				this.SurfaceScrapDamage(dmg, hitByPlayer);
			}
			else if (dmg.targetLayer == TargetLayer.Surface && this.HasScrap())
			{
				flag2 = (this.SurfaceScrapDamage(dmg, hitByPlayer) || flag2);
			}
			else if (flag2)
			{
				this.scrapDamageBeforeChunk = this.healthPerItem;
				this.structuralDamageBeforeChunk = this.healthPerItem;
			}
			if (flag2 || flag)
			{
				if (dmg.sourceUnit.IsPlayer(false) && GamePlayer.current.IsInSandBox())
				{
					this.CheckLootBoxSpawn(this.HasSalvage(TargetLayer.Both) ? 0.01f : 0.05f);
				}
				SalvageData.CheckPocketSystemAchievement();
			}
			return flag2 || flag;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x00050754 File Offset: 0x0004E954
		private bool StructuralDamage(DamageData dmg, bool hitByPlayer)
		{
			bool result = false;
			this.structuralDamageTaken += (int)dmg.damageAmount;
			for (;;)
			{
				InventoryItemType inventoryItemType = this.ChooseStructuralItem();
				OreItemData oreItemData = (inventoryItemType != null) ? inventoryItemType.GetComponent<OreItemData>() : null;
				int num = oreItemData ? oreItemData.health : (this.healthPerItem / 2);
				if (!inventoryItemType || num > this.structuralDamageTaken)
				{
					break;
				}
				Faction faction = (dmg.sourceUnit.faction != Faction.player && hitByPlayer && SeededRandom.Global.RandomBool(0.5f)) ? Faction.player : dmg.sourceUnit.faction;
				if (faction == Faction.player)
				{
					Register.AddCounter("SalvagingScrapYieldMax", 1, 0);
					GameplayManager.Instance.spaceShip.AddCrewExperience(this.GetSalvageExperience(inventoryItemType), new CommanderSpecialization?(CommanderSpecialization.Salvaging), true);
				}
				this.structuralDamageTaken -= num;
				this.structuralDamageBeforeChunk += num;
				this.structuralContent[inventoryItemType] = this.structuralContent[inventoryItemType] - 1;
				this.CreateScrapLoot(dmg, inventoryItemType, faction);
				result = true;
			}
			return result;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x00050874 File Offset: 0x0004EA74
		private bool SurfaceScrapDamage(DamageData dmg, bool hitByPlayer)
		{
			bool result = false;
			this.scrapDamageTaken += (int)dmg.damageAmount;
			for (;;)
			{
				InventoryItemType inventoryItemType = this.ChooseScrap();
				OreItemData oreItemData = (inventoryItemType != null) ? inventoryItemType.GetComponent<OreItemData>() : null;
				if (!oreItemData || oreItemData.health > this.scrapDamageTaken)
				{
					break;
				}
				Faction faction = (dmg.sourceUnit.faction != Faction.player && hitByPlayer && SeededRandom.Global.RandomBool(0.5f)) ? Faction.player : dmg.sourceUnit.faction;
				if (faction == Faction.player)
				{
					Register.AddCounter("SalvagingScrapYieldMax", 1, 0);
					GameplayManager.Instance.spaceShip.AddCrewExperience(this.GetSalvageExperience(oreItemData.item), new CommanderSpecialization?(CommanderSpecialization.Salvaging), true);
				}
				this.scrapDamageTaken -= oreItemData.health;
				this.scrapDamageBeforeChunk += oreItemData.health;
				this.scrapContent[oreItemData.item] = this.scrapContent[oreItemData.item] - 1;
				this.CreateScrapLoot(dmg, oreItemData.item, faction);
				result = true;
			}
			return result;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0005099C File Offset: 0x0004EB9C
		private void CreateScrapLoot(DamageData dmg, InventoryItemType item, Faction lootFaction)
		{
			float num = dmg.yield;
			if (SeededRandom.Global.RandomBool(num))
			{
				int num2 = 1;
				if (lootFaction == Faction.player && SkilltreeNode.salvagingSalMegaYield.isActive)
				{
					while (num > 1f)
					{
						num -= 1f;
						if (num > 1f || SeededRandom.Global.RandomBool(num))
						{
							num2++;
						}
					}
				}
				Register.AddCounter("SalvagingScrap", num2, 0);
				GamePlayer.current.AddAutopilotStat(IdleStat.Salvage, num2);
				for (int i = 0; i < num2; i++)
				{
					Singleton<LootManager>.Instance.CreateLootItem(dmg.hitCoordinates, item, 1, lootFaction, false);
					MissionObjective.Trigger(MissionTrigger.SalvagedItem, item, null, false);
					this.CheckRefinedMaterialLoot(dmg, lootFaction, item);
					this.CheckMaterialLoot(dmg, lootFaction, item);
				}
			}
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00050A58 File Offset: 0x0004EC58
		private void CheckRefinedMaterialLoot(DamageData dmg, Faction lootFaction, InventoryItemType item)
		{
			if (lootFaction != Faction.player)
			{
				return;
			}
			float currentIncrease = SkilltreeNode.SalvagingRefinedMaterialChance.currentIncrease;
			if (SeededRandom.Global.RandomFloat() <= currentIncrease)
			{
				RefinedMaterial? refinedMaterial = null;
				OreItemData component = item.GetComponent<OreItemData>();
				if (component)
				{
					refinedMaterial = new RefinedMaterial?(component.contents[0].product);
				}
				string name;
				if (refinedMaterial != null && this.refinedDict.TryGetValue(refinedMaterial.Value, out name))
				{
					InventoryItemType item2 = InventoryItemType.Get(name);
					Singleton<LootManager>.Instance.CreateLootItem(dmg.hitCoordinates, item2, 1, lootFaction, false);
				}
			}
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00050AF4 File Offset: 0x0004ECF4
		private void CheckMaterialLoot(DamageData dmg, Faction lootFaction, InventoryItemType item)
		{
			if (lootFaction != Faction.player)
			{
				return;
			}
			float currentIncrease = SkilltreeNode.salvagingMaterialChance.currentIncrease;
			if (!SeededRandom.Global.RandomBool(currentIncrease))
			{
				return;
			}
			RefinedMaterial? refinedMaterial = null;
			float num = 0f;
			OreItemData component = item.GetComponent<OreItemData>();
			if (component)
			{
				refinedMaterial = new RefinedMaterial?(component.contents[0].product);
				num = component.contents[0].customYield;
			}
			if (refinedMaterial != null)
			{
				num *= 5f;
				InventoryItemType item2 = InventoryItemType.Get("Canister" + refinedMaterial.Value.ToString());
				Singleton<LootManager>.Instance.CreateLootItem(dmg.hitCoordinates, item2, Mathf.CeilToInt(num), lootFaction, false);
			}
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00050BBC File Offset: 0x0004EDBC
		private bool ExtractItem(DamageData dmg)
		{
			if (this.activeItem == null)
			{
				using (List<SalvageItemData>.Enumerator enumerator = this.availableItemContent.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						SalvageItemData salvageItemData = enumerator.Current;
						this.SetActiveItem(salvageItemData.item);
					}
				}
			}
			SalvageItemData activeItem = this.activeItem;
			int num = Mathf.RoundToInt(dmg.damageAmount);
			if (activeItem == null || dmg.sourceUnit.faction != Faction.player)
			{
				return false;
			}
			this.damageTaken += num;
			if (this.damageTaken >= this.healthPerItem)
			{
				this.damageTaken -= this.healthPerItem;
				activeItem.extracted = true;
				Register.AddCounter("SalvagingItemMaxYield", 1, 0);
				GameplayManager.Instance.spaceShip.AddCrewExperience((float)GameMath.GetExperienceRewardValue(activeItem.item.rarity.GetPowerMultiplier() * 2f, activeItem.item.itemLevel), new CommanderSpecialization?(CommanderSpecialization.Salvaging), true);
				float chanceOfTrue = activeItem.baseChance * dmg.yield;
				InventoryItemType inventoryItemType = null;
				if (new SeedGenerator().Add(activeItem.baseChance).Add(activeItem.item.gameObject.name).CreateRandom().RandomBool(chanceOfTrue))
				{
					activeItem.extractionSuccessful = true;
					inventoryItemType = activeItem.item;
					if (!inventoryItemType.missionItem)
					{
						foreach (SalvageItemData salvageItemData2 in this.itemContent)
						{
							salvageItemData2.baseChance *= 1f - 1f / (float)this.itemContent.Count;
						}
					}
					Register.AddCounter("SalvagingItemsRetrieved", 1, 0);
					if (inventoryItemType.itemCategory == ItemCategory.Module)
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Modules, 1);
					}
					if (inventoryItemType.itemCategory == ItemCategory.Turret)
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Turrets, 1);
					}
					if (inventoryItemType.GetComponent<AbstractEquipment>())
					{
						MissionObjective.Trigger(MissionTrigger.SalvagedModule, inventoryItemType, null, false);
					}
				}
				else if (this.scrapContent.Count > 0)
				{
					List<InventoryItemType> list = new List<InventoryItemType>();
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.scrapContent)
					{
						list.Add(keyValuePair.Key);
					}
					inventoryItemType = SeededRandom.Global.Choose<InventoryItemType>(list);
				}
				if (inventoryItemType != null)
				{
					Singleton<LootManager>.Instance.CreateLootItem(dmg.hitCoordinates, inventoryItemType, 1, Faction.player, false);
					MissionObjective.Trigger(MissionTrigger.SalvagedItem, inventoryItemType, null, false);
					if (inventoryItemType.GetComponent<AbstractEquipment>() && inventoryItemType.rarity >= Rarity.HighGrade)
					{
						SteamAchievement.Trigger("SalvageRare");
					}
				}
				if (!activeItem.criticalItem)
				{
					this.nonCritEquipmentExtracted++;
				}
				this.itemContent.Remove(activeItem);
				return true;
			}
			dmg.damageAmount /= 2f;
			return false;
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00050EE0 File Offset: 0x0004F0E0
		public void CheckLootBoxSpawn(float chance = 0.01f)
		{
			if (this.lootboxSpawned)
			{
				return;
			}
			chance = (GamePlayer.current.autoPlay ? (0.33f * chance) : chance);
			if (SeededRandom.Global.RandomBool(chance))
			{
				Singleton<LootManager>.Instance.CreateLootBox(MapPointOfInterest.current.level, this.position);
				Register.AddCounter("SalvagingLootboxRetrieved", 1, 0);
				this.lootboxSpawned = true;
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00050F49 File Offset: 0x0004F149
		public float GetSalvageExperience(InventoryItemType item)
		{
			return (float)GameMath.GetExperienceRewardValue(item.rarity.GetPowerMultiplier() * 2f, SystemMapData.current.level);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00050F6C File Offset: 0x0004F16C
		public InventoryItemType ChooseScrap()
		{
			List<InventoryItemType> list = new List<InventoryItemType>();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.scrapContent)
			{
				if (keyValuePair.Value > 0)
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (list.Count > 0)
			{
				return SeededRandom.Global.Choose<InventoryItemType>(list);
			}
			return null;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00050FEC File Offset: 0x0004F1EC
		public InventoryItemType ChooseStructuralItem()
		{
			List<InventoryItemType> list = new List<InventoryItemType>();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.structuralContent)
			{
				if (keyValuePair.Value > 0)
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (list.Count > 0)
			{
				return SeededRandom.Global.Choose<InventoryItemType>(list);
			}
			return null;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0005106C File Offset: 0x0004F26C
		public override void DataToJson(JsonObject data)
		{
			JsonArray jsonArray = new JsonArray();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.scrapContent)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"item",
						keyValuePair.Key.ToJson()
					},
					{
						"count",
						new double?((double)keyValuePair.Value)
					}
				});
			}
			JsonArray jsonArray2 = new JsonArray();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair2 in this.structuralContent)
			{
				jsonArray2.Add(new JsonObject
				{
					{
						"item",
						keyValuePair2.Key.ToJson()
					},
					{
						"count",
						new double?((double)keyValuePair2.Value)
					}
				});
			}
			data["maxHealth"] = new double?((double)this.maxHealth);
			data["healthPerItem"] = new double?((double)this.healthPerItem);
			data["showOutline"] = new bool?(this.showOutline);
			data["damageTaken"] = new double?((double)this.damageTaken);
			data["scrapDamageTaken"] = new double?((double)this.scrapDamageTaken);
			data["shipTemplate"] = this.shipTemplate;
			data["initialBattleDamage"] = new double?((double)this.initialBattleDamage);
			string key = "shipData";
			SpaceShipData spaceShipData = this.shipData;
			data[key] = ((spaceShipData != null) ? spaceShipData.ToJson() : JsonValue.Null);
			data["scrapContent"] = jsonArray;
			data["structuralContent"] = jsonArray2;
			data["itemContent"] = this.itemContent.ToJsonArray<SalvageItemData>();
			data["nonCritEquipmentExtracted"] = new double?((double)this.nonCritEquipmentExtracted);
			data["creditsExtracted"] = new double?((double)this.creditsExtracted);
			data["lootboxExtracted"] = new double?((double)this.lootboxExtracted);
			data["lootBoxSpawned"] = new bool?(this.lootboxSpawned);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00051318 File Offset: 0x0004F518
		public override void LoadFromJson(JsonObject data)
		{
			this.maxHealth = data["maxHealth"];
			this.healthPerItem = data["healthPerItem"];
			this.showOutline = data["showOutline"].AsBoolean;
			this.damageTaken = data["damageTaken"];
			this.scrapDamageTaken = data["scrapDamageTaken"];
			this.nonCritEquipmentExtracted = data["nonCritEquipmentExtracted"];
			this.creditsExtracted = data["creditsExtracted"];
			this.lootboxExtracted = data["lootboxExtracted"];
			this.initialBattleDamage = data["initialBattleDamage"];
			this.lootboxSpawned = data["lootBoxSpawned"];
			this.shipTemplate = data["shipTemplate"];
			if (data["shipData"].IsJsonObject)
			{
				this.shipData = SpaceShipData.FromJson(data["shipData"], false);
			}
			this.itemContent.FromJsonArray(data["itemContent"], new ClassExtensions.ParseJsonValue<SalvageItemData>(SalvageItemData.FromJson));
			foreach (JsonValue jsonValue in data["scrapContent"].AsJsonArray)
			{
				this.scrapContent[InventoryItemType.FromJson(jsonValue["item"])] = jsonValue["count"].AsInteger;
			}
			if (data.ContainsKey("structuralContent"))
			{
				foreach (JsonValue jsonValue2 in data["structuralContent"].AsJsonArray)
				{
					this.structuralContent[InventoryItemType.FromJson(jsonValue2["item"])] = jsonValue2["count"].AsInteger;
				}
			}
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00051564 File Offset: 0x0004F764
		private static void CheckPocketSystemAchievement()
		{
			if (!SystemMapData.current.pocketSystem)
			{
				return;
			}
			if (SystemMapData.current.faction != Faction.salvageGuild)
			{
				return;
			}
			if (!(MapPointOfInterest.current is Salvage))
			{
				return;
			}
			if (!SystemMapData.current.IsStaticPOI(MapPointOfInterest.current))
			{
				return;
			}
			foreach (PersistableData persistableData in MapPointOfInterest.current.GetPersistables())
			{
				SalvageData salvageData = persistableData as SalvageData;
				if (salvageData != null)
				{
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair in salvageData.scrapContent)
					{
						if (keyValuePair.Value > 0)
						{
							return;
						}
					}
					if (salvageData.availableItemContent.Count > 0)
					{
						return;
					}
				}
			}
			SteamAchievement.Trigger("SalvagePocket");
		}

		// Token: 0x040005C1 RID: 1473
		public int damageTaken;

		// Token: 0x040005C2 RID: 1474
		public int scrapDamageTaken;

		// Token: 0x040005C3 RID: 1475
		public int structuralDamageTaken;

		// Token: 0x040005C4 RID: 1476
		public int maxHealth;

		// Token: 0x040005C5 RID: 1477
		public int maxStructuralHealth;

		// Token: 0x040005C6 RID: 1478
		public int healthPerItem;

		// Token: 0x040005C7 RID: 1479
		public int healthPerStructuralItem;

		// Token: 0x040005C8 RID: 1480
		public int healthPerScrap;

		// Token: 0x040005C9 RID: 1481
		public bool showOutline;

		// Token: 0x040005CA RID: 1482
		public int scrapDamageBeforeChunk;

		// Token: 0x040005CB RID: 1483
		public int structuralDamageBeforeChunk;

		// Token: 0x040005CC RID: 1484
		public int nonCritEquipmentExtracted;

		// Token: 0x040005CD RID: 1485
		public string shipTemplate;

		// Token: 0x040005CE RID: 1486
		public int initialBattleDamage;

		// Token: 0x040005CF RID: 1487
		public SpaceShipData shipData;

		// Token: 0x040005D0 RID: 1488
		public bool lootboxSpawned;

		// Token: 0x040005D1 RID: 1489
		public Dictionary<InventoryItemType, int> scrapContent = new Dictionary<InventoryItemType, int>();

		// Token: 0x040005D2 RID: 1490
		public Dictionary<InventoryItemType, int> structuralContent = new Dictionary<InventoryItemType, int>();

		// Token: 0x040005D3 RID: 1491
		public Dictionary<RefinedMaterial, string> refinedDict = new Dictionary<RefinedMaterial, string>
		{
			{
				RefinedMaterial.Titanium,
				"Titanium Plate"
			},
			{
				RefinedMaterial.Oxide,
				"Oxide Canister"
			},
			{
				RefinedMaterial.Silicon,
				"Circuit Board"
			},
			{
				RefinedMaterial.Tungsten,
				"Tungsten Carbide"
			},
			{
				RefinedMaterial.Carbon,
				"Graphite Wafers"
			},
			{
				RefinedMaterial.Iridium,
				"Durable Alloy"
			},
			{
				RefinedMaterial.Platinum,
				"Universal Catalyst"
			},
			{
				RefinedMaterial.Astatine,
				"Substable Wiring"
			}
		};

		// Token: 0x040005D4 RID: 1492
		public List<SalvageItemData> itemContent = new List<SalvageItemData>();

		// Token: 0x040005D5 RID: 1493
		public int creditsExtracted;

		// Token: 0x040005D6 RID: 1494
		public int lootboxExtracted;
	}
}
