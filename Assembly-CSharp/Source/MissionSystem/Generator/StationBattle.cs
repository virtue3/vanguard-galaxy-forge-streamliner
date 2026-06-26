using System;
using System.Collections.Generic;
using Behaviour.Weapons;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Util;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000DD RID: 221
	public class StationBattle : MissionGenerator
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000880 RID: 2176 RVA: 0x000430EE File Offset: 0x000412EE
		public override bool canBeIdled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x000430F1 File Offset: 0x000412F1
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			return (float)((source.level > 5) ? 1 : 0);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00043104 File Offset: 0x00041304
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			this.defend = SeededRandom.Global.RandomBool(0.5f);
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			this.attackingFaction = (this.defend ? enemyFaction : poi.faction);
			this.defendingFaction = (this.defend ? poi.faction : enemyFaction);
			string str = this.defend ? "Defend" : "Attack";
			Mission mission = new Mission
			{
				name = Translation.Translate("@StationBattle" + str + "MissionName", Array.Empty<object>()),
				category = Translation.Translate("@StationBattle" + str + "Mission", Array.Empty<object>()),
				description = Translation.Translate("@StationBattle" + str + "MissionDesc", Array.Empty<object>()),
				completionText = Translation.Translate("@StationBattle" + str + "MissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				iconName = "Combat",
				enemyFaction = enemyFaction,
				canBeIdled = this.canBeIdled
			};
			this.stationPoi = poi.system.AddCombatStation(this.defendingFaction, null, false, 0);
			this.stationPoi.name = this.defendingFaction.GenerateStationName(this.stationPoi);
			this.stationPoi.dangerLevel = (Faction.player.IsEnemy(enemyFaction) ? "@MapPOIDangerPirates" : "@MapPOIAttackFriendlies");
			int totalEnemyCount = this.stationPoi.totalEnemyCount;
			Faction faction = this.defend ? this.attackingFaction : this.defendingFaction;
			float num = this.defend ? 1.5f : 1f;
			this.AddReinforcement(ref totalEnemyCount, faction, 0.5f * num, 5f, 0, false);
			float num2 = this.GetRewardValue(poi.level);
			CombatStationData combatStationData = null;
			switch (difficulty)
			{
			case MissionDifficulty.Easy:
				if (SeededRandom.Global.RandomBool(0.5f))
				{
					combatStationData = CombatStationFactory.CreateEasyStation1(this.stationPoi, null);
				}
				else
				{
					combatStationData = CombatStationFactory.CreateEasyStation2(this.stationPoi);
				}
				this.AddReinforcement(ref totalEnemyCount, faction, 0.7f * num, 10f, 1, true);
				break;
			case MissionDifficulty.Normal:
				combatStationData = CombatStationFactory.CreateMediumStation(this.stationPoi, null);
				this.AddReinforcement(ref totalEnemyCount, faction, 0.6f * num, 10f, 1, true);
				this.AddReinforcement(ref totalEnemyCount, faction, 1f * num, 10f, 2, true);
				num2 *= 1.2f;
				break;
			case MissionDifficulty.Hard:
				combatStationData = CombatStationFactory.CreateLargeStation(this.stationPoi);
				this.AddReinforcement(ref totalEnemyCount, faction, 0.6f * num, 10f, 1, true);
				this.AddReinforcement(ref totalEnemyCount, faction, 1f * num, 10f, 2, true);
				this.AddReinforcement(ref totalEnemyCount, faction, 1.2f * num, 10f, 3, true);
				num2 *= 1.4f;
				break;
			case MissionDifficulty.Skull:
			case MissionDifficulty.Insane:
				combatStationData = CombatStationFactory.CreateLargeStation(this.stationPoi);
				this.AddReinforcement(ref totalEnemyCount, faction, 1f * num, 10f, 1, true);
				this.AddReinforcement(ref totalEnemyCount, faction, 1.2f * num, 10f, 2, true);
				this.AddReinforcement(ref totalEnemyCount, faction, 1.5f * num, 10f, 3, true);
				num2 *= 1.6f;
				break;
			}
			CombatStationPartData part = combatStationData.GetPart(CombatStationPartType.Core);
			if (part != null && this.defend)
			{
				part.autoActions = "DefenseSubject";
			}
			if (this.defend)
			{
				combatStationData.playerFriendly = true;
				MissionStep missionStep = new MissionStep
				{
					system = this.stationPoi.system,
					dynamicPointOfInterest = this.stationPoi
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = this.attackingFaction,
					requiredAmount = totalEnemyCount
				});
				missionStep.objectives.Add(new ProtectUnit
				{
					requiredAmount = 1,
					currentAmount = 1,
					protectText = "@StationBattleDefendMissionObj"
				});
				mission.steps.Add(missionStep);
			}
			else
			{
				MissionStep missionStep2 = new MissionStep
				{
					system = this.stationPoi.system,
					dynamicPointOfInterest = this.stationPoi
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CombatStationDestroyed,
					description = "Destroy the enemy station.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
			}
			this.AddRewards(mission, num2, difficulty, random, poi.faction);
			return mission;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0004359C File Offset: 0x0004179C
		private void AddReinforcement(ref int unitCount, Faction faction, float scale, float delayRange, int triggerSequence = 0, bool waitForNoEnemies = false)
		{
			List<AbstractUnitData> list = this.stationPoi.CreateUnitPayload(scale, new GameplayType?(GameplayType.Combat), faction, 0, 0, 1, 5, null);
			unitCount += list.Count;
			this.stationPoi.AddTriggeredSpawn(list, delayRange, triggerSequence, waitForNoEnemies, true);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000435EB File Offset: 0x000417EB
		public override GameplayType GetMissionType()
		{
			return GameplayType.Combat;
		}

		// Token: 0x04000482 RID: 1154
		public Faction attackingFaction;

		// Token: 0x04000483 RID: 1155
		public Faction defendingFaction;

		// Token: 0x04000484 RID: 1156
		public bool defend = true;

		// Token: 0x04000485 RID: 1157
		public MapPointOfInterest stationPoi;
	}
}
