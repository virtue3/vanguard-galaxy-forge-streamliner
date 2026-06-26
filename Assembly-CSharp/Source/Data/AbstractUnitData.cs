using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.Item;
using Behaviour.UI;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Weapons;
using LightJson;
using Source.Ability;
using Source.Crew;
using Source.Galaxy;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using Source.Util.NameGen;
using UnityEngine;

namespace Source.Data
{
	// Token: 0x02000107 RID: 263
	public abstract class AbstractUnitData : IJsonSource
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0004C036 File Offset: 0x0004A236
		// (set) Token: 0x060009EB RID: 2539 RVA: 0x0004C03E File Offset: 0x0004A23E
		public string guid { get; protected set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060009EC RID: 2540 RVA: 0x0004C047 File Offset: 0x0004A247
		// (set) Token: 0x060009ED RID: 2541 RVA: 0x0004C04F File Offset: 0x0004A24F
		public UnitPositionData positionData { get; protected set; } = new UnitPositionData();

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0004C058 File Offset: 0x0004A258
		public IEnumerable<InventoryItemType> equippedModules
		{
			get
			{
				return this.equipment.Values;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0004C065 File Offset: 0x0004A265
		public IEnumerable<InventoryItemType> equippedItems
		{
			get
			{
				foreach (InventoryItemType inventoryItemType in this.equipment.Values)
				{
					if (inventoryItemType)
					{
						yield return inventoryItemType;
					}
				}
				Dictionary<EquipmentSlot, InventoryItemType>.ValueCollection.Enumerator enumerator = default(Dictionary<EquipmentSlot, InventoryItemType>.ValueCollection.Enumerator);
				foreach (InventoryItemType inventoryItemType2 in this.hardpoints)
				{
					if (inventoryItemType2)
					{
						yield return inventoryItemType2;
					}
				}
				InventoryItemType[] array = null;
				foreach (InventoryItemType inventoryItemType3 in this.boosters)
				{
					if (inventoryItemType3)
					{
						yield return inventoryItemType3;
					}
				}
				array = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0004C075 File Offset: 0x0004A275
		public float cargoCapacity
		{
			get
			{
				if (!this.unit)
				{
					return this.cargo.capacity;
				}
				return (float)this.unit.cargoCapacity;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0004C09C File Offset: 0x0004A29C
		public float cargoUsed
		{
			get
			{
				return this.cargo.spaceUsed;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0004C0A9 File Offset: 0x0004A2A9
		// (set) Token: 0x060009F3 RID: 2547 RVA: 0x0004C0B1 File Offset: 0x0004A2B1
		public int level { get; protected set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060009F4 RID: 2548 RVA: 0x0004C0BA File Offset: 0x0004A2BA
		// (set) Token: 0x060009F5 RID: 2549 RVA: 0x0004C0C2 File Offset: 0x0004A2C2
		public List<ValueTuple<InventoryItemType, int>> loot { get; private set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060009F6 RID: 2550
		public abstract string type { get; }

		// Token: 0x060009F7 RID: 2551 RVA: 0x0004C0CC File Offset: 0x0004A2CC
		protected void Initialize(string guid, AbstractUnit unitDefinition)
		{
			this.guid = (string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid);
			this.name = unitDefinition.displayName;
			this.unitDefinition = unitDefinition;
			this.equipment = new Dictionary<EquipmentSlot, InventoryItemType>();
			this.boosters = new InventoryItemType[unitDefinition.boosterSlots.Length];
			this.hardpoints = new InventoryItemType[unitDefinition.hardpointSlots.Length];
			this.cargo = new Inventory(false);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0004C150 File Offset: 0x0004A350
		public void ApplyRandomBattleDamage(int loops, int size = 20)
		{
			for (int i = 0; i < loops; i++)
			{
				this.ApplyRandomBattleDamage(size);
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0004C170 File Offset: 0x0004A370
		private void ApplyRandomBattleDamage(int size = 20)
		{
			Bounds bounds = this.unitDefinition.GetComponent<SpriteRenderer>().bounds;
			this.battleDamage.Add(new SpriteBreakPoint(new Vector2(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y)), UnityEngine.Random.Range(size, size * 2), false));
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0004C1E7 File Offset: 0x0004A3E7
		public void SetCargoCapacity(int capacity)
		{
			this.cargo.capacity = (float)capacity;
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0004C1F8 File Offset: 0x0004A3F8
		public void LoadDefaultEquipment(int level, float difficultyModifier = -1f, string seed = null, GameplayType? loadout = null, Rarity? overrideRarity = null, Func<EquipmentSlot, Rarity> rarityFunc = null, bool exactLevel = false, TargetLayer? targetLayer = null)
		{
			if (difficultyModifier < 0f)
			{
				difficultyModifier = 0.4f + (float)level / 100f;
			}
			GameplayType? gameplayType = loadout;
			if (gameplayType == null)
			{
				loadout = this.gameplayType;
			}
			SeededRandom seededRandom = new SeedGenerator().Add(seed ?? SeededRandom.Global.RandomItemSeed()).CreateRandom();
			SpaceShipData spaceShipData = this as SpaceShipData;
			if (spaceShipData != null && spaceShipData.shipClass.pointValue > 10 && !spaceShipData.isPlayer && spaceShipData.customShipName == null)
			{
				spaceShipData.customShipName = ShipNames.GenerateShipName(this.faction, seededRandom);
			}
			SpaceShipModule[] moduleSlots = this.unitDefinition.moduleSlots;
			for (int i = 0; i < moduleSlots.Length; i++)
			{
				SpaceShipModule moduleSlot = moduleSlots[i];
				if (!this.equipment.ContainsKey(moduleSlot.slot))
				{
					EquipmentBuilder equipmentBuilder = EquipmentBuilder.GetRandom((EquipmentBuilder b) => b.slot == moduleSlot.slot && b.equipmentSize == moduleSlot.size, level, this.faction == Faction.player, null);
					if (equipmentBuilder != null)
					{
						Rarity defaultEquipmentRarity = this.GetDefaultEquipmentRarity(moduleSlot.slot, seededRandom, difficultyModifier, overrideRarity, rarityFunc);
						this.EquipItem(equipmentBuilder.CreateItemType(defaultEquipmentRarity, level, exactLevel, seededRandom.RandomItemSeed(), this.faction != Faction.player, false), 0);
					}
					else
					{
						string[] array = new string[7];
						array[0] = this.name;
						array[1] = " - Cannot find equipmentbuilder for: ";
						int num = 2;
						SpaceShipModule moduleSlot2 = moduleSlot;
						array[num] = ((moduleSlot2 != null) ? moduleSlot2.ToString() : null);
						array[3] = " slot: ";
						array[4] = moduleSlot.slot.ToString();
						array[5] = ", size ";
						array[6] = moduleSlot.size.ToString();
						Debug.LogWarning(string.Concat(array));
					}
				}
			}
			if (this.unitDefinition.HasModuleSlot(EquipmentSlot.DroneBay))
			{
				this.LoadDefaultDrones(loadout, null);
			}
			for (int j = 0; j < this.unitDefinition.hardpointSlots.Length; j++)
			{
				if (!this.hardpoints[j])
				{
					SpaceShipHardpoint hardpoint = this.unitDefinition.hardpointSlots[j];
					EquipmentBuilder equipmentBuilder = (loadout == null) ? this.GetHardpointDefaultEquipment(hardpoint) : null;
					if (equipmentBuilder == null)
					{
						EquipmentBuilder.BuilderFilter hardpointDefaultEquipmentFilter = this.GetHardpointDefaultEquipmentFilter(hardpoint, loadout, targetLayer);
						if (hardpointDefaultEquipmentFilter != null)
						{
							equipmentBuilder = EquipmentBuilder.GetRandom(hardpointDefaultEquipmentFilter, level, this.faction == Faction.player, null);
						}
					}
					if (equipmentBuilder == null)
					{
						equipmentBuilder = this.GetHardpointDefaultEquipment(hardpoint);
					}
					if (equipmentBuilder != null)
					{
						Rarity defaultEquipmentRarity2 = this.GetDefaultEquipmentRarity(EquipmentSlot.Hardpoint, seededRandom, difficultyModifier, overrideRarity, rarityFunc);
						InventoryItemType item = equipmentBuilder.CreateItemType(defaultEquipmentRarity2, level, exactLevel, seededRandom.RandomItemSeed(), this.faction != Faction.player, false);
						this.EquipItem(item, 0);
					}
				}
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0004C4E0 File Offset: 0x0004A6E0
		public void LoadDefaultDrones(GameplayType? loadout, TargetLayer? targetLayer = null)
		{
			InventoryItemType equippedItem = this.GetEquippedItem(EquipmentSlot.DroneBay);
			DroneBayModule droneBayModule = (equippedItem != null) ? equippedItem.GetComponent<DroneBayModule>() : null;
			if (droneBayModule)
			{
				Behaviour.Unit.SpaceShip spaceShip = this.unitDefinition as Behaviour.Unit.SpaceShip;
				GameplayType type;
				if (spaceShip != null)
				{
					type = (loadout ?? spaceShip.shipRoleType.GetGameplayType());
				}
				else if (loadout != null)
				{
					type = loadout.Value;
				}
				else
				{
					type = GameplayType.Combat;
				}
				for (int i = 0; i < droneBayModule.droneAmount; i++)
				{
					if (this.droneSlots.Count <= i)
					{
						this.droneSlots.Add(this.GetDefaultDrone(type, targetLayer));
					}
					else if (this.droneSlots[i] == null)
					{
						this.droneSlots[i] = this.GetDefaultDrone(type, targetLayer);
					}
				}
			}
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0004C5B8 File Offset: 0x0004A7B8
		private Behaviour.Unit.Drone GetDefaultDrone(GameplayType type, TargetLayer? targetLayer)
		{
			if (type == GameplayType.Cargo)
			{
				type = GameplayType.Combat;
			}
			List<Behaviour.Unit.Drone> list = new List<Behaviour.Unit.Drone>();
			if (this.faction == Faction.player)
			{
				list.AddRange(GamePlayer.current.GetUnlockedDrones());
			}
			else
			{
				list.AddRange(Behaviour.Unit.Drone.GetAll().Values);
			}
			for (int i = 0; i < list.Count; i++)
			{
				AbstractTurret defaultTurret = list[i].GetDefaultTurret();
				if (defaultTurret && defaultTurret.gameplayType != type)
				{
					if (targetLayer != null)
					{
						TargetLayer targetLayer2 = defaultTurret.targetLayer;
						TargetLayer? targetLayer3 = targetLayer;
						if (!(targetLayer2 == targetLayer3.GetValueOrDefault() & targetLayer3 != null))
						{
							goto IL_8D;
						}
					}
					list.RemoveAt(i);
					i--;
				}
				IL_8D:;
			}
			return SeededRandom.Global.Choose<Behaviour.Unit.Drone>(list);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0004C66C File Offset: 0x0004A86C
		private Rarity GetDefaultEquipmentRarity(EquipmentSlot slot, SeededRandom random, float difficultyModifier, Rarity? overrideRarity, Func<EquipmentSlot, Rarity> rarityFunc)
		{
			float num = random.RandomRange(0f, difficultyModifier);
			if (overrideRarity != null)
			{
				return overrideRarity.Value;
			}
			if (rarityFunc != null)
			{
				return rarityFunc(slot);
			}
			if (num < 0.2f)
			{
				return Rarity.Standard;
			}
			if (num < 0.5f)
			{
				return Rarity.Enhanced;
			}
			if (num < 0.8f)
			{
				return Rarity.HighGrade;
			}
			if (num < 0.95f)
			{
				return Rarity.Exotic;
			}
			return Rarity.Legendary;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0004C6D0 File Offset: 0x0004A8D0
		public void GiveAmmo(int ammoCount = 1000000, bool player = false)
		{
			if (this.unit)
			{
				if (this.unit.torpedoBayModule)
				{
					this.AddCargo(this.unit.torpedoBayModule.ammoType, ammoCount / 100, true);
				}
				foreach (AbstractTurret abstractTurret in this.unit.GetComponentsInChildren<AbstractTurret>())
				{
					if ((player || !this.unit.IsPlayer(true)) && abstractTurret.ammoType != null && !this.cargo.HasItem(abstractTurret.ammoType, ammoCount))
					{
						this.AddCargo(abstractTurret.ammoType, ammoCount, true);
					}
				}
				return;
			}
			if (this.unitDefinition)
			{
				TorpedoBayModule componentInChildren = this.unitDefinition.GetComponentInChildren<TorpedoBayModule>();
				if (componentInChildren)
				{
					this.AddCargo(componentInChildren.ammoType, ammoCount / 100, true);
				}
				SpaceShipHardpoint[] componentsInChildren2 = this.unitDefinition.GetComponentsInChildren<SpaceShipHardpoint>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					AbstractTurret component = componentsInChildren2[i].defaultEquipment.prefab.GetComponent<AbstractTurret>();
					if ((this.unit is Behaviour.Unit.Drone || player || this.faction != Faction.player) && component.ammoType != null && !this.cargo.HasItem(component.ammoType, ammoCount))
					{
						this.AddCargo(component.ammoType, ammoCount, true);
					}
				}
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0004C836 File Offset: 0x0004AA36
		protected virtual EquipmentBuilder GetHardpointDefaultEquipment(SpaceShipHardpoint hardpoint)
		{
			return hardpoint.defaultEquipment;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0004C840 File Offset: 0x0004AA40
		protected virtual EquipmentBuilder.BuilderFilter GetHardpointDefaultEquipmentFilter(SpaceShipHardpoint hardpoint, GameplayType? loadout, TargetLayer? targetLayer)
		{
			GameplayType? gameplayType = loadout;
			GameplayType gameplayType2 = GameplayType.Combat;
			Type turretType;
			if (!(gameplayType.GetValueOrDefault() == gameplayType2 & gameplayType != null))
			{
				gameplayType = loadout;
				gameplayType2 = GameplayType.Cargo;
				if (!(gameplayType.GetValueOrDefault() == gameplayType2 & gameplayType != null))
				{
					gameplayType = loadout;
					gameplayType2 = GameplayType.Mining;
					if (gameplayType.GetValueOrDefault() == gameplayType2 & gameplayType != null)
					{
						turretType = typeof(AbstractMiningTurret);
						goto IL_BA;
					}
					gameplayType = loadout;
					gameplayType2 = GameplayType.Salvage;
					if (gameplayType.GetValueOrDefault() == gameplayType2 & gameplayType != null)
					{
						turretType = typeof(AbstractSalvageTurret);
						goto IL_BA;
					}
					turretType = typeof(AbstractTurret);
					goto IL_BA;
				}
			}
			turretType = typeof(AbstractCombatTurret);
			IL_BA:
			return delegate(EquipmentBuilder b)
			{
				AbstractTurret abstractTurret = (AbstractTurret)b.prefab.GetComponent(turretType);
				bool flag;
				if (targetLayer != null)
				{
					if (abstractTurret == null)
					{
						flag = (targetLayer == null);
					}
					else
					{
						TargetLayer targetLayer2 = abstractTurret.targetLayer;
						TargetLayer? targetLayer3 = targetLayer;
						flag = (targetLayer2 == targetLayer3.GetValueOrDefault() & targetLayer3 != null);
					}
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				return b.slot == EquipmentSlot.Hardpoint && b.equipmentSize == hardpoint.size && abstractTurret && flag2;
			};
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0004C914 File Offset: 0x0004AB14
		public InventoryItemType GetEquippedItem(EquipmentSlot slot)
		{
			InventoryItemType result;
			this.equipment.TryGetValue(slot, out result);
			return result;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0004C931 File Offset: 0x0004AB31
		public IEnumerable<InventoryItemType> GetEquipedItemsOfType(InventoryItemType type)
		{
			if (type.itemCategory == ItemCategory.Module)
			{
				foreach (KeyValuePair<EquipmentSlot, InventoryItemType> keyValuePair in this.equipment)
				{
					AbstractEquipment component = keyValuePair.Value.GetComponent<AbstractEquipment>();
					AbstractEquipment component2 = type.GetComponent<AbstractEquipment>();
					if (component.slot == component2.slot && component.size == component2.size)
					{
						yield return keyValuePair.Value;
					}
				}
				Dictionary<EquipmentSlot, InventoryItemType>.Enumerator enumerator = default(Dictionary<EquipmentSlot, InventoryItemType>.Enumerator);
			}
			if (type.itemCategory == ItemCategory.Turret)
			{
				foreach (InventoryItemType inventoryItemType in this.hardpoints)
				{
					if (!(inventoryItemType == null))
					{
						AbstractTurret component3 = inventoryItemType.GetComponent<AbstractTurret>();
						if (component3)
						{
							AbstractTurret component4 = type.GetComponent<AbstractTurret>();
							if (component3.size == component4.size)
							{
								yield return inventoryItemType;
							}
						}
					}
				}
				InventoryItemType[] array = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0004C948 File Offset: 0x0004AB48
		public InventoryItemType EquipItem(InventoryItemType item, int index = 0)
		{
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			if (component.slot == EquipmentSlot.Hardpoint)
			{
				for (int i = 0; i < this.hardpoints.Length; i++)
				{
					if (this.hardpoints[i] == null)
					{
						this.hardpoints[i] = item;
						this.RecalculateLevel();
						return null;
					}
				}
				InventoryItemType result = this.hardpoints[0];
				this.hardpoints[0] = item;
				this.RecalculateLevel();
				return result;
			}
			if (component.slot == EquipmentSlot.Booster)
			{
				Debug.LogWarning("EquipItem() Should not be used for Boosters. Use EquipBooster in stead.");
				return null;
			}
			return this.EquipModule(item, component.slot);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0004C9D8 File Offset: 0x0004ABD8
		public InventoryItemType EquipModule(InventoryItemType item, EquipmentSlot slot)
		{
			InventoryItemType result;
			this.equipment.TryGetValue(slot, out result);
			if (item)
			{
				this.equipment[slot] = item;
			}
			else
			{
				this.equipment.Remove(slot);
			}
			this.RecalculateLevel();
			return result;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0004CA20 File Offset: 0x0004AC20
		public void RemoveModuleOfType(EquipmentSlot slot)
		{
			foreach (EquipmentSlot equipmentSlot in this.equipment.Keys.ToList<EquipmentSlot>())
			{
				if (equipmentSlot == slot)
				{
					this.equipment.Remove(equipmentSlot);
				}
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0004CA88 File Offset: 0x0004AC88
		public InventoryItemType EquipBooster(InventoryItemType item, int index)
		{
			InventoryItemType result = this.boosters[index];
			this.boosters[index] = item;
			this.RecalculateLevel();
			return result;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0004CAA4 File Offset: 0x0004ACA4
		public int AddCargo(InventoryItemType item, int count, bool force = false)
		{
			if (!force && item.m3 > 0f)
			{
				int num = (int)(this.cargo.GetSpaceAvailable() / item.m3);
				if (num < count)
				{
					count = Math.Min(count, num);
				}
			}
			if (count > 0)
			{
				this.cargo.Add(item, count, false, false);
				if (this.unit != null && this.unit.IsPlayer(true) && this.unit is SpaceShip && !SpaceStationInterior.instance)
				{
					UIInfoTextParent instance = UIInfoTextParent.instance;
					if (instance != null)
					{
						instance.ShowPickupText(item.displayName, count);
					}
				}
				return count;
			}
			return 0;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0004CB45 File Offset: 0x0004AD45
		public bool RemoveCargo(InventoryItemType item, int count, bool partial = false)
		{
			return (partial || this.cargo.GetCount(item) >= count) && this.cargo.Remove(item, count) == count;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0004CB6B File Offset: 0x0004AD6B
		public float GetCargoAvailable()
		{
			return this.cargo.GetSpaceAvailable();
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0004CB78 File Offset: 0x0004AD78
		public bool IsCargoBayFull(float minSpace = 1f)
		{
			return this.cargo.IsFull(minSpace);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0004CB86 File Offset: 0x0004AD86
		public bool IsItemInCargoHold(InventoryItemType item, int count)
		{
			return this.cargo.HasItem(item, count);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0004CB95 File Offset: 0x0004AD95
		public int ItemAmountInCargoHold(InventoryItemType item)
		{
			return this.cargo.GetCount(item);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0004CBA4 File Offset: 0x0004ADA4
		public bool IsItemTypeEquiped(InventoryItemType item, int count)
		{
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			if (component.slot == EquipmentSlot.Hardpoint)
			{
				for (int i = 0; i < this.hardpoints.Length; i++)
				{
					if (this.hardpoints[i] == item)
					{
						return true;
					}
				}
				return false;
			}
			if (component.slot == EquipmentSlot.Booster)
			{
				for (int j = 0; j < this.boosters.Length; j++)
				{
					if (this.boosters[j] == item)
					{
						return true;
					}
				}
				return false;
			}
			InventoryItemType x;
			this.equipment.TryGetValue(component.slot, out x);
			return x == item;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0004CC3A File Offset: 0x0004AE3A
		public void AddLoot(InventoryItemType lootItem, int count = 1)
		{
			if (this.loot == null)
			{
				this.loot = new List<ValueTuple<InventoryItemType, int>>();
			}
			this.loot.Add(new ValueTuple<InventoryItemType, int>(lootItem, count));
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0004CC64 File Offset: 0x0004AE64
		public void RecalculateLevel()
		{
			int num = 0;
			float num2 = 0f;
			int num3 = 0;
			foreach (InventoryItemType inventoryItemType in this.equippedItems)
			{
				num++;
				num2 += (float)inventoryItemType.itemLevel;
				num3 = Math.Max(num3, inventoryItemType.itemLevel);
			}
			int num4 = Mathf.RoundToInt(num2 / (float)num);
			this.level = Mathf.RoundToInt((float)((num4 + num3) / 2));
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0004CCF4 File Offset: 0x0004AEF4
		public void RepairHullHp(float amount)
		{
			if (this.maxHullHP > 0f)
			{
				this.currentHullHP = Mathf.Min(this.currentHullHP + amount, this.maxHullHP);
			}
			if (this.maxHullHP == this.currentHullHP)
			{
				if (this.unit)
				{
					this.unit.TryRestoreSpriteToOriginal();
					return;
				}
				this.battleDamage.Clear();
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0004CD5C File Offset: 0x0004AF5C
		public void RepairArmorHP(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			if (this.currentArmorHP <= 0f && this.maxArmorHP <= 0f)
			{
				return;
			}
			float num = this.currentArmorHP;
			this.currentArmorHP = Mathf.Min(this.currentArmorHP + amount, this.maxArmorHP);
			float num2 = this.currentArmorHP - num;
			if (num2 > 0f && this.unit)
			{
				this.unit.CheckTriggerAbility(AbilityTrigger.OnArmorRepaired, num2, null);
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0004CDDF File Offset: 0x0004AFDF
		public void RepairShieldHp(float amount)
		{
			if (this.maxShieldHP > 0f)
			{
				this.currentShieldHP = Mathf.Min(this.currentShieldHP + amount, this.maxShieldHP);
			}
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0004CE07 File Offset: 0x0004B007
		public float HullDamageTaken()
		{
			return this.maxHullHP - this.currentHullHP;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0004CE16 File Offset: 0x0004B016
		public float ArmorDamageTaken()
		{
			return this.maxArmorHP - this.currentArmorHP;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0004CE25 File Offset: 0x0004B025
		public float ShieldDamageTaken()
		{
			return this.maxShieldHP - this.currentShieldHP;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0004CE34 File Offset: 0x0004B034
		public float DamageTakenPercentage()
		{
			float num = this.currentHullHP + this.currentShieldHP + this.currentArmorHP;
			float num2 = this.maxHullHP + this.maxShieldHP + this.maxArmorHP;
			return 1f - num / num2;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0004CE74 File Offset: 0x0004B074
		public void StartFleetRepair(float repairTime)
		{
			this.repairTimer = repairTime;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0004CE80 File Offset: 0x0004B080
		public bool IsEnemy(AbstractUnit target)
		{
			if (this.alwaysFriendly || target.unitData.alwaysFriendly)
			{
				return false;
			}
			if (this.playerFriendly && target.faction == Faction.player)
			{
				return false;
			}
			if (target.unitData.playerFriendly && this.faction == Faction.player)
			{
				return false;
			}
			if (this.alwaysHostile || target.unitData.alwaysHostile)
			{
				return target.faction != this.faction;
			}
			if (this.playerHostile && target.faction == Faction.player)
			{
				return true;
			}
			if (target.unitData.playerHostile && this.faction == Faction.player)
			{
				return true;
			}
			Faction faction = this.faction;
			return faction == null || faction.IsEnemy(target.faction);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0004CF48 File Offset: 0x0004B148
		public bool IsPlayerEnemy()
		{
			return this.IsEnemy(Faction.player);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0004CF58 File Offset: 0x0004B158
		public bool IsEnemy(Faction faction)
		{
			if (this.playerFriendly && faction == Faction.player)
			{
				return false;
			}
			if (this.alwaysFriendly)
			{
				return false;
			}
			if (this.playerHostile && faction == Faction.player)
			{
				return true;
			}
			if (this.alwaysHostile)
			{
				return this.faction != faction;
			}
			Faction faction2 = this.faction;
			return faction2 == null || faction2.IsEnemy(faction);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0004CFBC File Offset: 0x0004B1BC
		public Behaviour.Unit.Drone GetDroneLoadout(int idx, SeededRandom random)
		{
			if (this.droneSlots.Count > idx && this.droneSlots[idx])
			{
				return this.droneSlots[idx];
			}
			List<Behaviour.Unit.Drone> list;
			if (this.faction == Faction.player)
			{
				list = GamePlayer.current.GetUnlockedDrones();
			}
			else
			{
				list = new List<Behaviour.Unit.Drone>(Drone.GetAll().Values);
			}
			return random.Choose<Behaviour.Unit.Drone>(list);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0004D028 File Offset: 0x0004B228
		public bool IsCombatant()
		{
			return this.autoActions == null || this.autoActions == "Combat";
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0004D044 File Offset: 0x0004B244
		public virtual JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject
			{
				{
					"guid",
					this.guid
				},
				{
					"type",
					this.type
				},
				{
					"currentHullHP",
					new double?((double)this.currentHullHP)
				},
				{
					"currentArmorHP",
					new double?((double)this.currentArmorHP)
				},
				{
					"currentShieldHP",
					new double?((double)this.currentShieldHP)
				},
				{
					"maxHullHP",
					new double?((double)this.maxHullHP)
				},
				{
					"maxArmorHP",
					new double?((double)this.maxArmorHP)
				},
				{
					"maxShieldHP",
					new double?((double)this.maxShieldHP)
				},
				{
					"travelling",
					new bool?(this.travelling)
				},
				{
					"travelSpeed",
					new double?((double)this.travelSpeed)
				},
				{
					"repairTimer",
					new double?((double)this.repairTimer)
				},
				{
					"cargo",
					this.cargo.ToJson()
				},
				{
					"equipment",
					this.equipment.ToJsonObject<EquipmentSlot, InventoryItemType>()
				},
				{
					"boosters",
					this.boosters.ToJsonArray<InventoryItemType>()
				},
				{
					"hardpoints",
					this.hardpoints.ToJsonArray<InventoryItemType>()
				},
				{
					"damageTaken",
					new double?((double)this.damageTaken)
				},
				{
					"autoActions",
					this.autoActions
				},
				{
					"alwaysHostile",
					new bool?(this.alwaysHostile)
				},
				{
					"alwaysFriendly",
					new bool?(this.alwaysFriendly)
				},
				{
					"playerHostile",
					new bool?(this.playerHostile)
				},
				{
					"playerFriendly",
					new bool?(this.playerFriendly)
				},
				{
					"noReputationLoss",
					new bool?(this.noReputationLoss)
				},
				{
					"canBeTracked",
					new bool?(this.canBeTracked)
				},
				{
					"canFlee",
					new bool?(this.canFlee)
				}
			};
			if (this.overrideTarget != null)
			{
				jsonObject["overrideTarget"] = JsonUtil.Vector2ToJson(this.overrideTarget.Value);
			}
			jsonObject["battleDamage"] = this.battleDamage.ToJsonArray<SpriteBreakPoint>();
			jsonObject["clearOverrideWhenReachedDestination"] = new bool?(this.clearOverrideWhenReachedDestination);
			AbstractUnit abstractUnit = this.unit;
			if ((abstractUnit != null) ? abstractUnit.rigidbody : null)
			{
				this.positionData.SetDataFromRigidbody(this.unit);
				jsonObject["positionData"] = this.positionData.ToJson();
			}
			else if (this.positionData != null)
			{
				jsonObject["positionData"] = this.positionData.ToJson();
			}
			if (this.faction != null)
			{
				jsonObject["faction"] = this.faction.identifier;
			}
			if (this.deathTrigger != null)
			{
				jsonObject["deathTrigger"] = this.deathTrigger.Value.ToString();
			}
			if (this.gameplayType != null)
			{
				jsonObject["gameplayType"] = this.gameplayType.ToString();
			}
			jsonObject["unitRank"] = this.unitRank.ToString();
			if (this.loot != null)
			{
				JsonArray jsonArray = new JsonArray();
				foreach (ValueTuple<InventoryItemType, int> valueTuple in this.loot)
				{
					jsonArray.Add(new JsonObject
					{
						{
							"item",
							valueTuple.Item1.ToJson()
						},
						{
							"count",
							new double?((double)valueTuple.Item2)
						}
					});
				}
				jsonObject["loot"] = jsonArray;
			}
			if (this.droneSlots.Count > 0)
			{
				JsonArray jsonArray2 = new JsonArray();
				foreach (Behaviour.Unit.Drone drone in this.droneSlots)
				{
					jsonArray2.Add((drone != null) ? drone.identifier : null);
				}
				jsonObject["droneSlots"] = jsonArray2;
			}
			return jsonObject;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0004D588 File Offset: 0x0004B788
		public static void FromJson(AbstractUnitData data, JsonValue json)
		{
			data.currentHullHP = (float)json["currentHullHP"].AsNumber;
			data.currentArmorHP = (float)json["currentArmorHP"].AsNumber;
			data.currentShieldHP = (float)json["currentShieldHP"].AsNumber;
			data.maxHullHP = (float)json["maxHullHP"].AsNumber;
			data.maxArmorHP = (float)json["maxArmorHP"].AsNumber;
			data.maxShieldHP = (float)json["maxShieldHP"].AsNumber;
			data.travelling = json["travelling"].AsBoolean;
			data.travelSpeed = (float)json["travelSpeed"].AsNumber;
			data.repairTimer = (float)json["repairTimer"].AsNumber;
			data.damageTaken = (float)json["damageTaken"].AsNumber;
			data.equipment.FromJsonObject(json["equipment"], new ClassExtensions.ParseJsonValueDict<InventoryItemType>(InventoryItemType.FromJson));
			data.boosters.FromJsonArray(json["boosters"], new ClassExtensions.ParseJsonValue<InventoryItemType>(InventoryItemType.FromJson));
			data.hardpoints.FromJsonArray(json["hardpoints"], new ClassExtensions.ParseJsonValue<InventoryItemType>(InventoryItemType.FromJson));
			data.cargo = Inventory.FromJson(json["cargo"], null, false);
			data.autoActions = json["autoActions"];
			data.overrideTarget = (json["overrideTarget"] ? new Vector2?(JsonUtil.JsonObjectToVector2(json["overrideTarget"])) : null);
			data.clearOverrideWhenReachedDestination = json["clearOverrideWhenReachedDestination"].AsBoolean;
			data.alwaysHostile = json["alwaysHostile"];
			data.playerHostile = json["playerHostile"];
			data.alwaysFriendly = json["alwaysFriendly"];
			data.playerFriendly = json["playerFriendly"];
			if (!json["noReputationLoss"].IsNull)
			{
				data.noReputationLoss = json["noReputationLoss"];
			}
			if (!json["canBeTracked"].IsNull)
			{
				data.canBeTracked = json["canBeTracked"];
			}
			if (!json["canFlee"].IsNull)
			{
				data.canFlee = json["canFlee"];
			}
			if (!json["positionData"].IsNull)
			{
				data.positionData = UnitPositionData.FromJson(json["positionData"]);
			}
			if (json["battleDamage"].IsJsonArray)
			{
				data.battleDamage.FromJsonArray(json["battleDamage"], new ClassExtensions.ParseJsonValue<SpriteBreakPoint>(SpriteBreakPoint.FromJson));
			}
			if (!json["faction"].IsNull)
			{
				data.faction = Faction.Get(json["faction"]);
			}
			if (!json["deathTrigger"].IsNull)
			{
				data.deathTrigger = new MissionTrigger?(Enum.Parse<MissionTrigger>(json["deathTrigger"]));
			}
			if (json["loot"].IsJsonArray)
			{
				data.loot = new List<ValueTuple<InventoryItemType, int>>();
				foreach (JsonValue jsonValue in json["loot"].AsJsonArray)
				{
					data.loot.Add(new ValueTuple<InventoryItemType, int>(InventoryItemType.FromJson(jsonValue["item"]), jsonValue["count"]));
				}
			}
			if (!json["gameplayType"].IsNull)
			{
				data.gameplayType = new GameplayType?(Enum.Parse<GameplayType>(json["gameplayType"]));
			}
			if (!json["unitRank"].IsNull)
			{
				data.unitRank = Enum.Parse<UnitRank>(json["unitRank"]);
			}
			if (json["droneSlots"].IsJsonArray)
			{
				using (IEnumerator<JsonValue> enumerator = json["droneSlots"].AsJsonArray.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						JsonValue jsonValue2 = enumerator.Current;
						data.droneSlots.Add(jsonValue2.IsString ? Drone.Get(jsonValue2) : null);
					}
					goto IL_58B;
				}
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 1, 0) && data.GetEquippedItem(EquipmentSlot.DroneBay) != null)
			{
				GameplayType? loadout = null;
				SpaceShipData spaceShipData = data as SpaceShipData;
				if (spaceShipData != null)
				{
					loadout = new GameplayType?(spaceShipData.shipClass.shipRoleType.GetGameplayType());
				}
				data.LoadDefaultDrones(loadout, null);
			}
			IL_58B:
			data.RecalculateLevel();
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0004DB44 File Offset: 0x0004BD44
		public static AbstractUnitData FromJson(JsonValue data)
		{
			string a = data["type"];
			if (a == "SpaceShip")
			{
				return SpaceShipData.FromJson(data, false);
			}
			if (a == "Turret")
			{
				return DefensiveTurretData.FromJson(data);
			}
			if (a == "CombatStationPart")
			{
				return CombatStationPartData.FromJson(data);
			}
			throw new NotImplementedException("AbstractUnitData type nog niet geimplementeerd: " + data);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0004DBB5 File Offset: 0x0004BDB5
		public virtual IEnumerable<ValueTuple<InventoryItemType, int, bool>> CreateLootTable()
		{
			if (this.loot != null)
			{
				foreach (ValueTuple<InventoryItemType, int> valueTuple in this.loot)
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(valueTuple.Item1, valueTuple.Item2, true);
				}
				List<ValueTuple<InventoryItemType, int>>.Enumerator enumerator = default(List<ValueTuple<InventoryItemType, int>>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0004DBC8 File Offset: 0x0004BDC8
		protected InventoryItemType FilterLoot(InventoryItemType item)
		{
			AbstractEquipment abstractEquipment;
			if (item.equipmentBuilder && item.TryGetComponent<AbstractEquipment>(out abstractEquipment))
			{
				return item.equipmentBuilder.CreateItemType(item.rarity, item.itemLevel, true, null, false, false);
			}
			return item;
		}

		// Token: 0x04000559 RID: 1369
		public AbstractUnit unitDefinition;

		// Token: 0x0400055A RID: 1370
		public AbstractUnit unit;

		// Token: 0x0400055B RID: 1371
		public List<SpriteBreakPoint> battleDamage = new List<SpriteBreakPoint>();

		// Token: 0x0400055C RID: 1372
		public float damageTaken;

		// Token: 0x0400055E RID: 1374
		public string name;

		// Token: 0x04000560 RID: 1376
		public Vector2? overrideTarget;

		// Token: 0x04000561 RID: 1377
		public bool clearOverrideWhenReachedDestination;

		// Token: 0x04000562 RID: 1378
		public Faction faction;

		// Token: 0x04000563 RID: 1379
		private Dictionary<EquipmentSlot, InventoryItemType> equipment;

		// Token: 0x04000564 RID: 1380
		public InventoryItemType[] boosters;

		// Token: 0x04000565 RID: 1381
		public InventoryItemType[] hardpoints;

		// Token: 0x04000566 RID: 1382
		public GameplayType? gameplayType;

		// Token: 0x04000567 RID: 1383
		public float currentHullHP = -1f;

		// Token: 0x04000568 RID: 1384
		public float currentArmorHP = -1f;

		// Token: 0x04000569 RID: 1385
		public float currentShieldHP = -1f;

		// Token: 0x0400056A RID: 1386
		public float maxHullHP;

		// Token: 0x0400056B RID: 1387
		public float maxArmorHP;

		// Token: 0x0400056C RID: 1388
		public float maxShieldHP;

		// Token: 0x0400056D RID: 1389
		public Inventory cargo;

		// Token: 0x0400056E RID: 1390
		public bool travelling;

		// Token: 0x0400056F RID: 1391
		public float travelSpeed;

		// Token: 0x04000570 RID: 1392
		public string autoActions;

		// Token: 0x04000571 RID: 1393
		public float repairTimer;

		// Token: 0x04000572 RID: 1394
		public MissionTrigger? deathTrigger;

		// Token: 0x04000573 RID: 1395
		public bool alwaysHostile;

		// Token: 0x04000574 RID: 1396
		public bool alwaysFriendly;

		// Token: 0x04000575 RID: 1397
		public bool playerHostile;

		// Token: 0x04000576 RID: 1398
		public bool playerFriendly;

		// Token: 0x04000577 RID: 1399
		public bool noReputationLoss;

		// Token: 0x04000578 RID: 1400
		public bool canBeTracked = true;

		// Token: 0x04000579 RID: 1401
		public bool canFlee = true;

		// Token: 0x0400057C RID: 1404
		public readonly List<Behaviour.Unit.Drone> droneSlots = new List<Behaviour.Unit.Drone>();

		// Token: 0x0400057D RID: 1405
		public UnitRank unitRank;

		// Token: 0x0400057E RID: 1406
		public CrewData crewData;
	}
}
