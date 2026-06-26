using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Crew;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using LightJson;
using Source.Crew;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.SpaceShip.Auto;
using Source.Util;
using Source.Util.NameGen;
using UnityEngine;

namespace Source.SpaceShip
{
	// Token: 0x0200005B RID: 91
	public class SpaceShipData : AbstractUnitData
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0001D239 File Offset: 0x0001B439
		public Behaviour.Unit.SpaceShip spaceShip
		{
			get
			{
				return this.unit as Behaviour.Unit.SpaceShip;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600037D RID: 893 RVA: 0x0001D246 File Offset: 0x0001B446
		public override string type
		{
			get
			{
				return "Behaviour.Unit.SpaceShip";
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0001D24D File Offset: 0x0001B44D
		public SpaceShipData(string guid, Behaviour.Unit.SpaceShip cls, bool isPlayer) : this(cls, isPlayer, null)
		{
			base.guid = guid;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0001D260 File Offset: 0x0001B460
		public SpaceShipData(Behaviour.Unit.SpaceShip cls, bool isPlayer, Faction faction = null)
		{
			base.guid = Guid.NewGuid().ToString();
			this.isPlayer = isPlayer;
			this.shipClass = cls;
			this.unitDefinition = cls;
			base.Initialize(base.guid, cls);
			this.crewMembers = new CrewMemberData[cls.maxOfficers];
			this.crewData = new CrewData();
			this.faction = faction;
			if (isPlayer)
			{
				this.faction = Faction.player;
				return;
			}
			if (cls.hasCommander)
			{
				SeededRandom random = new SeedGenerator().Add(SeededRandom.Global.RandomItemSeed()).CreateRandom();
				this.commanderData = CommanderData.CreateRandom(random, (faction != null) ? faction.identifier : null);
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001D325 File Offset: 0x0001B525
		private int GetMaxCrew()
		{
			return 10;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001D32C File Offset: 0x0001B52C
		public void TransferCargo(bool materials = true, bool armory = false, bool keepEssential = true, bool filtered = false)
		{
			List<InventoryItemType> ammoUsedByTurrets = this.GetAmmoUsedByTurrets();
			SpaceStation spaceStation = GamePlayer.current.currentPointOfInterest as SpaceStation;
			if (spaceStation == null)
			{
				return;
			}
			Inventory materialStorage = spaceStation.materialStorage;
			Inventory global = Inventory.global;
			int num = 0;
			foreach (Inventory.InventoryItem inventoryItem in (filtered ? this.cargo.filteredItems : this.cargo.items))
			{
				InventoryItemType item = inventoryItem.item;
				if (!inventoryItem.item.favouriteItem)
				{
					if (materials && item.CanGoInMaterials() && materialStorage.GetSpaceAvailable() >= inventoryItem.spaceRequired)
					{
						materialStorage.Add(item, inventoryItem.count, false, false);
						this.cargo.Set(inventoryItem.slot, null, 1, false);
						num++;
					}
					else if (armory && item.CanGoInArmory() && !Inventory.global.IsFull(inventoryItem.spaceRequired))
					{
						if (keepEssential)
						{
							if (item.itemCategory == ItemCategory.Ammo && ammoUsedByTurrets.Contains(item))
							{
								continue;
							}
							if (item.itemCategory == ItemCategory.Usable)
							{
								UsableItem component = item.GetComponent<UsableItem>();
								if (component && component.keepInCargo)
								{
									continue;
								}
							}
						}
						Inventory.global.Add(item, inventoryItem.count, false, false);
						this.cargo.Set(inventoryItem.slot, null, 1, false);
						num++;
					}
				}
			}
			if (num > 0)
			{
				if (materials && armory)
				{
					if (keepEssential)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTransferredCargoNonEss", Array.Empty<object>())).WithColor(ColorHelper.orange75).Show();
					}
					else
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTransferredCargo", Array.Empty<object>())).WithColor(ColorHelper.orange75).Show();
					}
				}
				else if (materials)
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTransferredMaterials", Array.Empty<object>())).WithColor(ColorHelper.orange75).Show();
				}
				else if (armory)
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTransferredArmory", Array.Empty<object>())).WithColor(ColorHelper.orange75).Show();
				}
			}
			this.cargo.UpdateVisibleItems();
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001D588 File Offset: 0x0001B788
		private List<InventoryItemType> GetAmmoUsedByTurrets()
		{
			List<InventoryItemType> list = new List<InventoryItemType>();
			foreach (InventoryItemType inventoryItemType in this.hardpoints)
			{
				if (!(inventoryItemType == null))
				{
					AbstractTurret component = inventoryItemType.GetComponent<AbstractTurret>();
					if (component)
					{
						list.Add(component.ammoType);
					}
				}
			}
			InventoryItemType equippedItem = base.GetEquippedItem(EquipmentSlot.TopedoBay);
			TorpedoBayModule torpedoBayModule;
			if (equippedItem && equippedItem.TryGetComponent<TorpedoBayModule>(out torpedoBayModule))
			{
				list.Add(torpedoBayModule.ammoType);
			}
			return list;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001D60B File Offset: 0x0001B80B
		public Sprite GetSprite()
		{
			return this.shipClass.GetComponent<SpriteRenderer>().sprite;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001D620 File Offset: 0x0001B820
		public int AddCrew(string crewType, int amount)
		{
			Behaviour.Unit.SpaceShip spaceShip = this.unit as Behaviour.Unit.SpaceShip;
			if (spaceShip == null || spaceShip.maxGrunts <= 0)
			{
				return amount;
			}
			int num = spaceShip.maxGrunts - this.crewData.totalCrew;
			if (num <= 0)
			{
				return amount;
			}
			int num2 = Mathf.Min(amount, num);
			this.crewData.AddCrew(crewType, num2);
			return amount - num2;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0001D678 File Offset: 0x0001B878
		public bool RemoveCrew(string crewType, int amount)
		{
			return this.crewData.RemoveCrew(crewType, amount) > 0;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001D68C File Offset: 0x0001B88C
		public void JettisonAllCrew()
		{
			foreach (KeyValuePair<string, int> keyValuePair in this.crewData.crew)
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					Singleton<LootManager>.Instance.CreateCrewPod(keyValuePair.Key, 1, GameplayManager.Instance.spaceShip.transform, true);
				}
			}
			this.crewData = new CrewData();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0001D71C File Offset: 0x0001B91C
		public int GetEmptySeatCount()
		{
			int num = 0;
			for (int i = 0; i < this.crewMembers.Length; i++)
			{
				if (this.crewMembers[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0001D750 File Offset: 0x0001B950
		public bool AssignCrewMember(CrewMemberData crew)
		{
			for (int i = 0; i < this.crewMembers.Length; i++)
			{
				if (this.crewMembers[i] == null)
				{
					this.crewMembers[i] = crew;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001D788 File Offset: 0x0001B988
		public bool RemoveCrewMember(CrewMemberData crew)
		{
			for (int i = 0; i < this.crewMembers.Length; i++)
			{
				if (this.crewMembers[i] == crew)
				{
					this.crewMembers[i] = null;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0001D7C0 File Offset: 0x0001B9C0
		public float AddCrewExperience(float experience, CommanderSpecialization? skilltree)
		{
			foreach (CrewMemberData crewMemberData in this.crewMembers)
			{
				if (crewMemberData != null)
				{
					crewMemberData.GiveExperience(experience);
				}
			}
			float result = this.commanderData.GiveExperience(experience);
			if (skilltree != null)
			{
				this.AddMasteryExperience(experience, skilltree.Value);
			}
			return result;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001D818 File Offset: 0x0001BA18
		public void AddMasteryExperience(float experience, CommanderSpecialization skilltree)
		{
			Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(skilltree));
			SkillTreeData skillTreeData = this.commanderData.GetSkillTreeData(tree, false);
			if (skillTreeData == null)
			{
				return;
			}
			skillTreeData.AddMasteryXp(experience);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0001D849 File Offset: 0x0001BA49
		public IEnumerable<InventoryItemType> CreateSalvageTable(int count)
		{
			List<InventoryItemType> modules = this.GetEquipedModulesForLootTable(SeededRandom.Global.RandomBool(0.75f));
			modules.AddRange(this.GetEquipedTurretsForLootTable(SeededRandom.Global.RandomBool(0.75f)));
			int i = 0;
			while (i < count && modules.Count > 0)
			{
				InventoryItemType item = SeededRandom.Global.ChooseAndRemove<InventoryItemType>(modules);
				yield return base.FilterLoot(item);
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001D860 File Offset: 0x0001BA60
		public override IEnumerable<ValueTuple<InventoryItemType, int, bool>> CreateLootTable()
		{
			if (SeededRandom.Global.RandomBool(0.12f))
			{
				bool dropLowestRarity = SeededRandom.Global.RandomBool(0.95f);
				List<InventoryItemType> equipedModulesForLootTable = this.GetEquipedModulesForLootTable(dropLowestRarity);
				if (equipedModulesForLootTable.Count > 0)
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(base.FilterLoot(SeededRandom.Global.Choose<InventoryItemType>(equipedModulesForLootTable)), 1, false);
				}
			}
			if (this.hardpoints.Length != 0 && SeededRandom.Global.RandomBool(0.12f))
			{
				bool dropLowestRarity2 = SeededRandom.Global.RandomBool(0.95f);
				List<InventoryItemType> equipedTurretsForLootTable = this.GetEquipedTurretsForLootTable(dropLowestRarity2);
				if (equipedTurretsForLootTable.Count > 0)
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(base.FilterLoot(SeededRandom.Global.Choose<InventoryItemType>(equipedTurretsForLootTable)), 1, false);
				}
			}
			if (SeededRandom.Global.RandomBool(0.3f))
			{
				float num = (float)GameMath.GetCreditsValue((float)SeededRandom.Global.RandomRange(3, 10), base.level);
				List<Inventory.InventoryItem> list = new List<Inventory.InventoryItem>();
				foreach (Inventory.InventoryItem inventoryItem in this.cargo.items)
				{
					if (inventoryItem.item.itemCategory != ItemCategory.Ammo)
					{
						list.Add(inventoryItem);
					}
				}
				if (list.Count > 0)
				{
					Inventory.InventoryItem inventoryItem2 = SeededRandom.Global.Choose<Inventory.InventoryItem>(list);
					int item = Mathf.Min(inventoryItem2.count, Mathf.CeilToInt(num / (float)inventoryItem2.item.cost));
					yield return new ValueTuple<InventoryItemType, int, bool>(inventoryItem2.item, item, false);
				}
			}
			if (SeededRandom.Global.RandomBool(0.1f))
			{
				float num2 = (float)GameMath.GetCreditsValue((float)SeededRandom.Global.RandomRange(1, 3), base.level);
				List<Inventory.InventoryItem> list2 = new List<Inventory.InventoryItem>();
				foreach (Inventory.InventoryItem inventoryItem3 in this.cargo.items)
				{
					if (inventoryItem3.item.itemCategory == ItemCategory.Ammo)
					{
						list2.Add(inventoryItem3);
					}
				}
				if (list2.Count > 0)
				{
					Inventory.InventoryItem inventoryItem4 = SeededRandom.Global.Choose<Inventory.InventoryItem>(list2);
					int item2 = Mathf.Min(inventoryItem4.count, Mathf.CeilToInt(num2 / (float)inventoryItem4.item.cost));
					yield return new ValueTuple<InventoryItemType, int, bool>(inventoryItem4.item, item2, false);
				}
			}
			if (SeededRandom.Global.RandomBool(0.04f))
			{
				InventoryItemType inventoryItemType = ItemBuilder.Get("Blueprint").CreateRandomBlueprint(base.level, (this.unitRank >= UnitRank.Elite) ? new Rarity?(Rarity.HighGrade) : null, null, true);
				if (inventoryItemType)
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(inventoryItemType, 1, false);
				}
			}
			if (SeededRandom.Global.RandomBool(0.3f))
			{
				yield return new ValueTuple<InventoryItemType, int, bool>(ItemBuilder.Get("Credits").CreateCreditsItem(GameMath.GetCreditsValue(SeededRandom.Global.RandomRange(2.5f, 3.25f), base.level)), 1, false);
			}
			if (GamePlayer.current.IsInSandBox())
			{
				if (SeededRandom.Global.RandomBool(0.02f))
				{
					yield return new ValueTuple<InventoryItemType, int, bool>("LockedContainerKey", 1, false);
				}
				if (SeededRandom.Global.RandomBool(0.02f))
				{
					yield return new ValueTuple<InventoryItemType, int, bool>(ItemBuilder.Get("LootBox").CreateLootboxItem(base.level), 1, false);
				}
			}
			if (this.commanderData != null && SectorMapData.current.quadrant == 2 && SeededRandom.Global.RandomBool(0.2f))
			{
				if (this.unitRank >= UnitRank.Champion && SeededRandom.Global.RandomBool(0.25f))
				{
					yield return new ValueTuple<InventoryItemType, int, bool>("CommendationDogTag3", 1, false);
				}
				else if (this.unitRank >= UnitRank.Veteran && SeededRandom.Global.RandomBool(0.33f))
				{
					yield return new ValueTuple<InventoryItemType, int, bool>("CommendationDogTag2", 1, false);
				}
				else
				{
					yield return new ValueTuple<InventoryItemType, int, bool>("CommendationDogTag", 1, false);
				}
			}
			else if (this.commanderData != null && SystemMapData.current.level >= 10 && SeededRandom.Global.RandomBool(0.1f))
			{
				yield return new ValueTuple<InventoryItemType, int, bool>("CommendationDogTag", 1, false);
			}
			foreach (ValueTuple<InventoryItemType, int, bool> valueTuple in base.CreateLootTable())
			{
				yield return valueTuple;
			}
			IEnumerator<ValueTuple<InventoryItemType, int, bool>> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0001D870 File Offset: 0x0001BA70
		private List<InventoryItemType> GetEquipedTurretsForLootTable(bool dropLowestRarity)
		{
			if (this.hardpoints == null || this.hardpoints.Length == 0)
			{
				return new List<InventoryItemType>();
			}
			Rarity rarity = this.hardpoints.Min((InventoryItemType hardpoint) => hardpoint.rarity);
			List<InventoryItemType> list = new List<InventoryItemType>();
			foreach (InventoryItemType inventoryItemType in this.hardpoints)
			{
				if (!(inventoryItemType == null))
				{
					EquipmentBuilder equipmentBuilder = inventoryItemType.equipmentBuilder;
					if (equipmentBuilder != null && equipmentBuilder.AvailableForPlayer() && (!dropLowestRarity || inventoryItemType.rarity == rarity))
					{
						list.Add(inventoryItemType);
					}
				}
			}
			return list;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0001D918 File Offset: 0x0001BB18
		private List<InventoryItemType> GetEquipedModulesForLootTable(bool dropLowestRarity)
		{
			List<InventoryItemType> list = new List<InventoryItemType>(base.equippedModules);
			if (list.Count == 0)
			{
				return list;
			}
			Rarity rarity = list.Min((InventoryItemType module) => module.rarity);
			List<InventoryItemType> list2 = new List<InventoryItemType>();
			foreach (InventoryItemType inventoryItemType in list)
			{
				if (!(inventoryItemType == null))
				{
					EquipmentBuilder equipmentBuilder = inventoryItemType.equipmentBuilder;
					if (equipmentBuilder != null && equipmentBuilder.AvailableForPlayer() && (!dropLowestRarity || inventoryItemType.rarity == rarity))
					{
						list2.Add(inventoryItemType);
					}
				}
			}
			return list2;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001D9DC File Offset: 0x0001BBDC
		public string GetShipName()
		{
			if (!string.IsNullOrWhiteSpace(this.customShipName))
			{
				return this.customShipName;
			}
			return this.shipClass.displayName;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001DA00 File Offset: 0x0001BC00
		public override JsonValue ToJson()
		{
			JsonObject jsonObject = base.ToJson();
			jsonObject["shipClass"] = this.shipClass.identifier;
			JsonArray jsonArray;
			if (this.isPlayer)
			{
				jsonArray = new JsonArray();
				for (int i = 0; i < this.crewMembers.Length; i++)
				{
					JsonArray jsonArray2 = jsonArray;
					CrewMemberData crewMemberData = this.crewMembers[i];
					jsonArray2.Add((crewMemberData != null) ? crewMemberData.guid : null);
				}
				if (!string.IsNullOrEmpty(this.skillLoadout))
				{
					jsonObject["skillLoadout"] = this.skillLoadout;
				}
			}
			else
			{
				jsonArray = this.crewMembers.ToJsonArray<CrewMemberData>();
			}
			jsonObject["crewMembers"] = jsonArray;
			if (!this.isPlayer && this.commanderData != null)
			{
				if (this.commanderData is Mercenary)
				{
					jsonObject["mercenary"] = this.commanderData.ToJson();
				}
				else
				{
					jsonObject["commander"] = this.commanderData.ToJson();
				}
			}
			if (!string.IsNullOrWhiteSpace(this.customShipName))
			{
				jsonObject["customName"] = this.customShipName;
			}
			if (this.dockingState != null)
			{
				jsonObject["dockingState"] = this.dockingState.ToString();
			}
			jsonObject["crewData"] = this.crewData.ToJson();
			if (this.decalPlacements.Count > 0)
			{
				JsonArray jsonArray3 = new JsonArray();
				foreach (DecalPlacement decalPlacement in this.decalPlacements)
				{
					jsonArray3.Add(decalPlacement.ToJson());
				}
				jsonObject["decalPlacements"] = jsonArray3;
			}
			return jsonObject;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001DBE4 File Offset: 0x0001BDE4
		public static SpaceShipData FromJson(JsonValue json, bool isPlayer)
		{
			SpaceShipData spaceShipData = new SpaceShipData(json["guid"].AsString, json["shipClass"].AsString, isPlayer);
			AbstractUnitData.FromJson(spaceShipData, json);
			if (json["crewMembers"].IsJsonArray)
			{
				spaceShipData.crewMembers.FromJsonArray(json["crewMembers"], new ClassExtensions.ParseJsonValue<CrewMemberData>(CrewMemberData.FromJson));
			}
			if (!isPlayer && json["mercenary"].IsJsonObject)
			{
				spaceShipData.commanderData = Mercenary.FromJson(json["mercenary"]);
			}
			else if (!isPlayer && json["commander"].IsJsonObject)
			{
				spaceShipData.commanderData = CommanderData.FromJson(json["commander"]);
			}
			else if (isPlayer)
			{
				spaceShipData.commanderData = GamePlayer.current.commander;
				if (!json["skillLoadout"].IsNull)
				{
					spaceShipData.skillLoadout = json["skillLoadout"];
				}
			}
			if (!json["customName"].IsNull)
			{
				spaceShipData.customShipName = json["customName"].AsString;
			}
			if (!json["dockingState"].IsNull)
			{
				spaceShipData.dockingState = new DockingState?(Enum.Parse<DockingState>(json["dockingState"]));
			}
			if (!json["crewData"].IsNull)
			{
				spaceShipData.crewData = CrewData.FromJson(json["crewData"]);
			}
			if (json["decalPlacements"].IsJsonArray)
			{
				foreach (JsonValue json2 in json["decalPlacements"].AsJsonArray)
				{
					spaceShipData.decalPlacements.Add(DecalPlacement.FromJson(json2));
				}
			}
			return spaceShipData;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001DE14 File Offset: 0x0001C014
		public bool HasLoadout(GameplayType type, TargetLayer targetLayer = TargetLayer.Both)
		{
			if (type == GameplayType.Generic || type == GameplayType.Cargo)
			{
				return true;
			}
			foreach (InventoryItemType inventoryItemType in this.hardpoints)
			{
				AbstractTurret abstractTurret;
				if (inventoryItemType != null && inventoryItemType.TryGetComponent<AbstractTurret>(out abstractTurret) && abstractTurret.gameplayType == type && abstractTurret.CanTargetLayer(targetLayer))
				{
					return true;
				}
			}
			InventoryItemType equippedItem = base.GetEquippedItem(EquipmentSlot.DroneBay);
			DroneBayModule droneBayModule;
			if (equippedItem != null && equippedItem.TryGetComponent<DroneBayModule>(out droneBayModule))
			{
				return droneBayModule.HasLoadout(type, targetLayer, this);
			}
			InventoryItemType equippedItem2 = base.GetEquippedItem(EquipmentSlot.TopedoBay);
			TorpedoBayModule torpedoBayModule;
			return equippedItem2 != null && equippedItem2.TryGetComponent<TorpedoBayModule>(out torpedoBayModule) && torpedoBayModule.HasLoadout(type, targetLayer);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001DEC4 File Offset: 0x0001C0C4
		public bool HasMiningLoadout(bool core)
		{
			foreach (InventoryItemType inventoryItemType in this.hardpoints)
			{
				AbstractMiningTurret abstractMiningTurret;
				if (inventoryItemType && inventoryItemType.TryGetComponent<AbstractMiningTurret>(out abstractMiningTurret) && (core ? abstractMiningTurret.targetsCore : abstractMiningTurret.targetsSurface))
				{
					return true;
				}
			}
			InventoryItemType equippedItem = base.GetEquippedItem(EquipmentSlot.DroneBay);
			DroneBayModule droneBayModule;
			return equippedItem != null && equippedItem.TryGetComponent<DroneBayModule>(out droneBayModule) && droneBayModule.HasMiningLoadout(core);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001DF3A File Offset: 0x0001C13A
		public void LoadFactionData(Faction f)
		{
			this.faction = f;
			if (this.faction == Faction.fanatics && this.commanderData != null)
			{
				this.commanderData.SetName(CaptainFanaticNames.GetRandomFirstName(null), "", CaptainFanaticNames.GetRandomLastName(null));
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0001DF74 File Offset: 0x0001C174
		public bool CheckDroneLoadout()
		{
			bool result = false;
			if (!SkilltreeNode.dronesUnlockDrones1.isActive && this.droneSlots.Count > 0)
			{
				List<Drone> unlockedDrones = GamePlayer.current.GetUnlockedDrones();
				for (int i = 0; i < this.droneSlots.Count; i++)
				{
					if (this.droneSlots[i] != null && !unlockedDrones.Contains(this.droneSlots[i]))
					{
						this.droneSlots[i] = null;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x040001F0 RID: 496
		public readonly Behaviour.Unit.SpaceShip shipClass;

		// Token: 0x040001F1 RID: 497
		public bool isPlayer;

		// Token: 0x040001F2 RID: 498
		public CommanderData commanderData;

		// Token: 0x040001F3 RID: 499
		public CrewMemberData[] crewMembers;

		// Token: 0x040001F4 RID: 500
		public string customShipName;

		// Token: 0x040001F5 RID: 501
		public DockingState? dockingState;

		// Token: 0x040001F6 RID: 502
		public string skillLoadout;

		// Token: 0x040001F7 RID: 503
		public List<DecalPlacement> decalPlacements = new List<DecalPlacement>();
	}
}
