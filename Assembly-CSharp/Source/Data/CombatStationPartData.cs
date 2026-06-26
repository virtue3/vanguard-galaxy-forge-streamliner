using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Mining;
using Behaviour.Unit;
using LightJson;
using Source.Galaxy;
using Source.Item;
using Source.Player;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.Data
{
	// Token: 0x02000108 RID: 264
	public class CombatStationPartData : AbstractUnitData
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0004DC6F File Offset: 0x0004BE6F
		public override string type
		{
			get
			{
				return "CombatStationPart";
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0004DC76 File Offset: 0x0004BE76
		public CombatStationPart partPrefab
		{
			get
			{
				return this.unitDefinition as CombatStationPart;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0004DC83 File Offset: 0x0004BE83
		public List<CombatStationConnector> connectors
		{
			get
			{
				return this.partPrefab.connectors;
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0004DC90 File Offset: 0x0004BE90
		public CombatStationPartData(CombatStationPart prefab) : this(Guid.NewGuid().ToString(), prefab)
		{
			this.gameplayType = new GameplayType?(GameplayType.Combat);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0004DCC3 File Offset: 0x0004BEC3
		public CombatStationPartData(string guid, CombatStationPart prefab)
		{
			base.Initialize(guid, prefab);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0004DCD4 File Offset: 0x0004BED4
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			result["prefabName"] = this.unitDefinition.name;
			return result;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0004DD08 File Offset: 0x0004BF08
		public new static CombatStationPartData FromJson(JsonValue json)
		{
			CombatStationPartData combatStationPartData = new CombatStationPartData(json["guid"].AsString, json["prefabName"].AsString);
			AbstractUnitData.FromJson(combatStationPartData, json);
			return combatStationPartData;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0004DD4E File Offset: 0x0004BF4E
		public override IEnumerable<ValueTuple<InventoryItemType, int, bool>> CreateLootTable()
		{
			SeededRandom random = SeededRandom.Global;
			CombatStationPartType partType = this.partPrefab.partType;
			if ((partType == CombatStationPartType.Connector || partType == CombatStationPartType.DockingPad || partType == CombatStationPartType.DockingTunnel || partType == CombatStationPartType.CargoDock) && random.RandomBool(0.7f))
			{
				string[] refinedProducts = new string[]
				{
					"Titanium Plate",
					"Bulkhead",
					"Hull Beam",
					"Structural Component"
				};
				int count = 1;
				if (base.level > 25 && random.RandomBool(0.5f))
				{
					count = 2;
				}
				int j;
				for (int i = 0; i < count; i = j + 1)
				{
					string text = random.Choose<string>(refinedProducts);
					InventoryItemType.Get(text);
					int lo = Mathf.RoundToInt((float)base.level / 12f);
					int hi = Mathf.RoundToInt((float)base.level / 8f);
					int num = 1 + random.RandomRange(lo, hi);
					if (partType == CombatStationPartType.Connector)
					{
						num = Mathf.Max(1, num / 2);
					}
					yield return new ValueTuple<InventoryItemType, int, bool>(text, num, false);
					j = i;
				}
				refinedProducts = null;
			}
			if (partType == CombatStationPartType.CargoDock && random.RandomBool(0.7f))
			{
				yield return new ValueTuple<InventoryItemType, int, bool>(ItemBuilder.Get("Credits").CreateCreditsItem(GameMath.GetCreditsValue(random.RandomRange(4.5f, 5.25f), base.level)), 1, false);
			}
			if (partType == CombatStationPartType.Energy)
			{
				if (random.RandomBool(0.5f))
				{
					InventoryItemType item = ItemBuilder.Get("WarpFuel").CreateWarpFuel(Rarity.HighGrade);
					int lo2 = Mathf.RoundToInt((float)base.level / 20f);
					int hi2 = Mathf.RoundToInt((float)base.level / 16f);
					int item2 = 1 + random.RandomRange(lo2, hi2);
					yield return new ValueTuple<InventoryItemType, int, bool>(item, item2, false);
				}
				if (base.level > 25 && random.RandomBool(0.04f))
				{
					InventoryItemType item3 = InventoryItemType.Get("EnergyCrystal");
					int lo3 = Mathf.RoundToInt((float)base.level / 45f);
					int hi3 = Mathf.RoundToInt((float)base.level / 20f);
					int item4 = 1 + random.RandomRange(lo3, hi3);
					yield return new ValueTuple<InventoryItemType, int, bool>(item3, item4, false);
				}
			}
			if (partType == CombatStationPartType.Refinery && random.RandomBool(0.5f))
			{
				OreItemData surfaceOre = MapPointOfInterest.current.system.systemOreData.GetRandomOre(true, null);
				OreItemData coreOre = MapPointOfInterest.current.system.systemOreData.GetRandomOre(false, null);
				int count = Mathf.RoundToInt((float)base.level / 6f);
				int i = Mathf.RoundToInt((float)base.level / 4f);
				int amount = 1 + random.RandomRange(count, i);
				yield return new ValueTuple<InventoryItemType, int, bool>(surfaceOre.item, amount, false);
				amount = Mathf.Max(1, amount / 2);
				yield return new ValueTuple<InventoryItemType, int, bool>(coreOre.item, amount, false);
				if (random.RandomBool(0.5f))
				{
					InventoryItemType.Get("Canister" + surfaceOre.contents[0].product.ToString());
					amount = 1 + random.RandomRange(count, i);
					amount *= 4;
					yield return new ValueTuple<InventoryItemType, int, bool>(coreOre.item, amount, false);
					InventoryItemType.Get("Canister" + coreOre.contents[0].product.ToString());
					amount = 1 + random.RandomRange(count, i);
					amount *= 2;
					yield return new ValueTuple<InventoryItemType, int, bool>(coreOre.item, amount, false);
				}
				surfaceOre = null;
				coreOre = null;
			}
			if (partType == CombatStationPartType.DefensePlatform)
			{
				if (this.hardpoints.Length != 0 && SeededRandom.Global.RandomBool(0.4f))
				{
					bool flag = SeededRandom.Global.RandomBool(0.55f);
					Rarity rarity = this.hardpoints.Min((InventoryItemType hardpoint) => hardpoint.rarity);
					List<InventoryItemType> list = new List<InventoryItemType>();
					foreach (InventoryItemType inventoryItemType in this.hardpoints)
					{
						if (!(inventoryItemType == null))
						{
							EquipmentBuilder equipmentBuilder = inventoryItemType.equipmentBuilder;
							if (equipmentBuilder != null && equipmentBuilder.AvailableForPlayer() && (!flag || inventoryItemType.rarity == rarity))
							{
								list.Add(inventoryItemType);
							}
						}
					}
					if (list.Count > 0)
					{
						yield return new ValueTuple<InventoryItemType, int, bool>(base.FilterLoot(random.Choose<InventoryItemType>(list)), 1, false);
					}
				}
				if (random.RandomBool(0.2f))
				{
					float num2 = (float)GameMath.GetCreditsValue((float)random.RandomRange(1, 3), base.level);
					List<Inventory.InventoryItem> list2 = new List<Inventory.InventoryItem>();
					foreach (Inventory.InventoryItem inventoryItem in this.cargo.items)
					{
						if (inventoryItem.item.itemCategory == ItemCategory.Ammo)
						{
							list2.Add(inventoryItem);
						}
					}
					if (list2.Count > 0)
					{
						Inventory.InventoryItem inventoryItem2 = random.Choose<Inventory.InventoryItem>(list2);
						int item5 = Mathf.Min(inventoryItem2.count, Mathf.CeilToInt(num2 / (float)inventoryItem2.item.cost));
						yield return new ValueTuple<InventoryItemType, int, bool>(inventoryItem2.item, item5, false);
					}
				}
			}
			if ((partType == CombatStationPartType.Refinery || partType == CombatStationPartType.Core) && random.RandomBool(0.3f))
			{
				bool flag2 = random.RandomBool(0.985f);
				ItemBuilder itemBuilder = ItemBuilder.Get("Blueprint");
				int level = base.level;
				bool excludeExoticOrHigher = flag2;
				InventoryItemType inventoryItemType2 = itemBuilder.CreateRandomBlueprint(level, null, null, excludeExoticOrHigher);
				if (inventoryItemType2)
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(inventoryItemType2, 1, false);
				}
			}
			if (partType == CombatStationPartType.Core && random.RandomBool(1f))
			{
				yield return new ValueTuple<InventoryItemType, int, bool>(ItemBuilder.Get("Credits").CreateCreditsItem(GameMath.GetCreditsValue(random.RandomRange(13.5f, 15.25f), base.level)), 1, false);
			}
			if (partType == CombatStationPartType.Core && GamePlayer.current.IsInSandBox())
			{
				if (random.RandomBool(0.8f))
				{
					yield return new ValueTuple<InventoryItemType, int, bool>("LockedContainerKey", random.RandomRange(1, 3), false);
				}
				if (SystemMapData.current.storyteller is ConquestSystem)
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(ItemBuilder.Get("LootBox").CreateLootboxItem(base.level), 1, false);
				}
			}
			foreach (ValueTuple<InventoryItemType, int, bool> valueTuple in base.CreateLootTable())
			{
				yield return valueTuple;
			}
			IEnumerator<ValueTuple<InventoryItemType, int, bool>> enumerator2 = null;
			yield break;
			yield break;
		}
	}
}
