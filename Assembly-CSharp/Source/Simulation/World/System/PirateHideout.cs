using System;
using System.Collections.Generic;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Util;

namespace Source.Simulation.World.System
{
	// Token: 0x0200007C RID: 124
	public class PirateHideout : SystemStoryteller
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00025ECA File Offset: 0x000240CA
		public PirateHideout(SystemMapData system) : base(system)
		{
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00025ED4 File Offset: 0x000240D4
		public override void SetupSystem()
		{
			SeededRandom random = SeededRandom.Global;
			this.system.pocketSystem = true;
			this.system.name = SandboxWorld.EnsureUniqueName(() => Translation.Translate("@PocketSystemPirateHideout", new object[]
			{
				random.Choose<string>(PirateHideout.systemNames)
			}));
			this.system.faction = Faction.marauders;
			for (int i = 0; i < 3; i++)
			{
				Combat combat = this.system.AddCombat(null, null, 0);
				combat.dangerLevel = "@MapPOIDangerPirates";
				combat.AddGuards(combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), null);
				if (random.RandomBool(0.5f))
				{
					combat.AddTriggeredSpawn(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), random.RandomRange(20f, 40f), 0, false, true);
				}
			}
			this.CreateLargeCombatStation();
			JumpGate entranceJumpgate = this.system.GetEntranceJumpgate();
			JumpGate jumpGate = entranceJumpgate.GetTargetPOI() as JumpGate;
			jumpGate.AddGuards(jumpGate.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), null);
			jumpGate.dangerLevel = "@MapPOIDangerDrones";
			entranceJumpgate.level = jumpGate.level;
			entranceJumpgate.AddGuards(entranceJumpgate.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), null);
			entranceJumpgate.dangerLevel = "@MapPOIDangerPirates";
			List<SystemMapData> list = new List<SystemMapData>();
			foreach (SystemMapData systemMapData in this.system.sector.allSystems)
			{
				if (systemMapData.jumpgateOpen)
				{
					list.Add(systemMapData);
				}
			}
			Combat combat2 = random.Choose<SystemMapData>(list).AddCombat(Faction.marauders, null, 0);
			combat2.AddGuards(combat2.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), null);
			List<AbstractUnitData> list2 = combat2.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null);
			list2[0].AddLoot(ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(entranceJumpgate, null), 1);
			combat2.AddTriggeredSpawn(list2, (float)random.RandomRange(15, 35), 0, false, true);
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00026178 File Offset: 0x00024378
		private void CreateLargeCombatStation()
		{
			Combat combat = this.system.AddCombatStation(Faction.marauders, null, true, 0);
			combat.name = "Corsair Hideout";
			combat.dangerLevel = "@MapPOIDangerSpaceStation";
			CombatStationData combatStationData = CombatStationFactory.CreateLargeStation(combat);
			combatStationData.AddLoot(this.CreateLoot((EquipmentBuilder b) => b.slot != EquipmentSlot.Hardpoint && b.slot != EquipmentSlot.Booster), 1);
			combatStationData.AddLoot(this.CreateLoot((EquipmentBuilder b) => b.slot == EquipmentSlot.Hardpoint), 1);
			combatStationData.GetPart(CombatStationPartType.Core).AddLoot("BonusSkillPointTemplate", 1);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0002622C File Offset: 0x0002442C
		private InventoryItemType CreateLoot(EquipmentBuilder.BuilderFilter filter)
		{
			Rarity rarity = Rarity.HighGrade;
			if (this.system.level > 20 && SeededRandom.Global.RandomBool(0.05f))
			{
				rarity = Rarity.Exotic;
			}
			else if (this.system.level > 30 && SeededRandom.Global.RandomBool(0.1f))
			{
				rarity = Rarity.Exotic;
			}
			return EquipmentBuilder.GetRandom(filter, this.system.level, true, null).CreateItemType(rarity, this.system.level, false, null, false, false);
		}

		// Token: 0x04000276 RID: 630
		private static string[] systemNames = new string[]
		{
			"Epsilon-11",
			"Barbados-5",
			"Antigua-3C",
			"Havana-12",
			"Tortuga-414",
			"Beta-5",
			"Redshift-9",
			"Blackhole-3",
			"Terror-13",
			"Mayhem-5",
			"Nassau-6",
			"Brimstone-7",
			"Cartagena-8",
			"Kingston-9",
			"Plunder-11",
			"Reaver-3",
			"Specter-8",
			"Dread-7"
		};
	}
}
