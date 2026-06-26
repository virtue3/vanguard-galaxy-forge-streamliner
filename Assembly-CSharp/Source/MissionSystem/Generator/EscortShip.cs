using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Util;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D7 RID: 215
	public class EscortShip : MissionGenerator
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0004111A File Offset: 0x0003F31A
		public override bool canBeIdled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0004111D File Offset: 0x0003F31D
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			return (float)((source.level > 5) ? 1 : 0);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00041130 File Offset: 0x0003F330
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			SystemMapData system = poi.system;
			MapElement poi2 = new Escort();
			Faction faction = poi.faction;
			MapPointOfInterest mapPointOfInterest = system.SetupPOI(poi2, null, faction, 0) as MapPointOfInterest;
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			mapPointOfInterest.dangerLevel = (Faction.player.IsEnemy(enemyFaction) ? "@MapPOIDangerPirates" : "@MapPOIAttackFriendlies");
			Mission mission = new Mission
			{
				name = Translation.Translate("@EscortShipMissionName", Array.Empty<object>()),
				category = Translation.Translate("@EscortShipMission", Array.Empty<object>()),
				description = Translation.Translate("@EscortShipMissionDesc", Array.Empty<object>()),
				completionText = Translation.Translate("@EscortShipMissionComplete", new object[]
				{
					poi.name
				}),
				sourcePoi = poi,
				enemyFaction = enemyFaction,
				canBeIdled = this.canBeIdled
			};
			MissionStep missionStep = new MissionStep
			{
				system = mapPointOfInterest.system,
				dynamicPointOfInterest = mapPointOfInterest
			};
			string fixedUnit = "Trundar";
			if (difficulty >= MissionDifficulty.Hard)
			{
				fixedUnit = "Drometar";
			}
			List<AbstractUnitData> list = mapPointOfInterest.CreateFixedPayload(fixedUnit, 1, null, new GameplayType?(GameplayType.Combat), UnitRank.Elite);
			foreach (AbstractUnitData abstractUnitData in list)
			{
				abstractUnitData.canBeTracked = false;
				abstractUnitData.playerFriendly = true;
				abstractUnitData.AddCargo(InventoryItemType.Get("EscortMissionItem0"), 20, false);
				abstractUnitData.autoActions = "AmbientEscort";
			}
			mapPointOfInterest.AddGuards(list, null);
			CombatStationFactory.CreateEscortLocation(mapPointOfInterest).playerFriendly = true;
			missionStep.objectives.Add(new ProtectUnit
			{
				enemyFaction = enemyFaction
			});
			missionStep.objectives.Add(new TriggerObjective
			{
				trigger = MissionTrigger.EscortUnitCargoUnloaded,
				requiredAmount = 20,
				description = "Wait for the cargo ship to unload its cargo"
			});
			mission.steps.Add(missionStep);
			float num = this.GetRewardValue(poi.level);
			switch (difficulty)
			{
			case MissionDifficulty.Normal:
				num *= 1.2f;
				break;
			case MissionDifficulty.Hard:
				num *= 1.4f;
				break;
			case MissionDifficulty.Skull:
			case MissionDifficulty.Insane:
				num *= 1.6f;
				break;
			}
			this.AddRewards(mission, num, difficulty, random, poi.faction);
			return mission;
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00041380 File Offset: 0x0003F580
		public override GameplayType GetMissionType()
		{
			return GameplayType.Combat;
		}
	}
}
