using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D2 RID: 210
	public class BountyHunt : MissionGenerator
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0003FEC1 File Offset: 0x0003E0C1
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0003FEC4 File Offset: 0x0003E0C4
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			if (GamePlayer.current.starterSpecialization == 8)
			{
				return 1f;
			}
			return (float)((GamePlayer.current.IsMissionCompleted("tutorial_9") || GamePlayer.current.IsInSandBox()) ? 1 : 0);
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0003FEFB File Offset: 0x0003E0FB
		public override GameplayType GetMissionType()
		{
			return GameplayType.Combat;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0003FF00 File Offset: 0x0003E100
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			float num = this.GetRewardValue(poi.level);
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			string text = "@BountyHuntMissionName";
			string text2 = "@BountyHuntMission";
			string text3 = "@BountyHuntMissionDesc";
			string text4 = "@BountyHuntMissionComplete";
			if (enemyFaction != Faction.marauders)
			{
				text = "@BountyHunt2MissionName";
				text2 = "@BountyHunt2Mission";
				text3 = "@BountyHunt2MissionDesc";
				text4 = "@BountyHunt2MissionComplete";
			}
			Mission mission = new Mission
			{
				name = Translation.Translate(text, new object[]
				{
					enemyFaction.name
				}),
				category = Translation.Translate(text2, new object[]
				{
					enemyFaction.name
				}),
				description = Translation.Translate(text3, new object[]
				{
					enemyFaction.name
				}),
				completionText = Translation.Translate(text4, new object[]
				{
					enemyFaction.name
				}),
				sourcePoi = poi,
				enemyFaction = enemyFaction,
				canBeIdled = this.canBeIdled
			};
			MapPointOfInterest mapPointOfInterest = poi.system.SetupPOI(new Combat(), null, null, 0) as MapPointOfInterest;
			mapPointOfInterest.faction = enemyFaction;
			mapPointOfInterest.dangerLevel = (Faction.player.IsEnemy(enemyFaction) ? "@MapPOIDangerPirates" : "@MapPOIAttackFriendlies");
			if (difficulty == MissionDifficulty.Easy)
			{
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest3 = mapPointOfInterest;
				float pointsScale = 0.8f;
				int maxPointsPerUnit = (mapPointOfInterest.level < 5) ? 10 : 0;
				UnitRank? fixedRank = new UnitRank?(UnitRank.Rookie);
				mapPointOfInterest2.AddGuards(mapPointOfInterest3.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), null, 0, maxPointsPerUnit, 1, 3, fixedRank), random);
			}
			else if (difficulty == MissionDifficulty.Normal)
			{
				MapPointOfInterest mapPointOfInterest4 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest5 = mapPointOfInterest;
				float pointsScale2 = 0.8f;
				GameplayType? gType = new GameplayType?(GameplayType.Combat);
				Faction f = null;
				int minPointsPerUnit = 0;
				int maxPointsPerUnit2 = 0;
				int minUnits = 1;
				int maxUnits = 5;
				UnitRank? fixedRank = null;
				mapPointOfInterest4.AddGuards(mapPointOfInterest5.CreateUnitPayload(pointsScale2, gType, f, minPointsPerUnit, maxPointsPerUnit2, minUnits, maxUnits, fixedRank), random);
				MapPointOfInterest mapPointOfInterest6 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest7 = mapPointOfInterest;
				float pointsScale3 = 0.6f;
				GameplayType? gType2 = new GameplayType?(GameplayType.Combat);
				Faction f2 = null;
				int minPointsPerUnit2 = 0;
				int maxPointsPerUnit3 = 0;
				int minUnits2 = 1;
				int maxUnits2 = 5;
				fixedRank = null;
				mapPointOfInterest6.AddTriggeredSpawn(mapPointOfInterest7.CreateUnitPayload(pointsScale3, gType2, f2, minPointsPerUnit2, maxPointsPerUnit3, minUnits2, maxUnits2, fixedRank), (float)random.RandomRange(25, 25), 0, false, true);
				num *= 1.2f;
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				MapPointOfInterest mapPointOfInterest8 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest9 = mapPointOfInterest;
				float pointsScale4 = 1f;
				GameplayType? gType3 = new GameplayType?(GameplayType.Combat);
				Faction f3 = null;
				int minPointsPerUnit3 = 0;
				int maxPointsPerUnit4 = 0;
				int minUnits3 = 1;
				int maxUnits3 = 5;
				UnitRank? fixedRank = null;
				mapPointOfInterest8.AddGuards(mapPointOfInterest9.CreateUnitPayload(pointsScale4, gType3, f3, minPointsPerUnit3, maxPointsPerUnit4, minUnits3, maxUnits3, fixedRank), random);
				MapPointOfInterest mapPointOfInterest10 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest11 = mapPointOfInterest;
				float pointsScale5 = 1.2f;
				GameplayType? gType4 = new GameplayType?(GameplayType.Combat);
				Faction f4 = null;
				int minPointsPerUnit4 = 0;
				int maxPointsPerUnit5 = 0;
				int minUnits4 = 1;
				int maxUnits4 = 5;
				fixedRank = null;
				mapPointOfInterest10.AddTriggeredSpawn(mapPointOfInterest11.CreateUnitPayload(pointsScale5, gType4, f4, minPointsPerUnit4, maxPointsPerUnit5, minUnits4, maxUnits4, fixedRank), (float)random.RandomRange(25, 35), 0, false, true);
				num *= 1.4f;
			}
			else if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				MapPointOfInterest mapPointOfInterest12 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest13 = mapPointOfInterest;
				float pointsScale6 = 1.2f;
				GameplayType? gType5 = new GameplayType?(GameplayType.Combat);
				Faction f5 = null;
				int minPointsPerUnit5 = 0;
				int maxPointsPerUnit6 = 0;
				int minUnits5 = 1;
				int maxUnits5 = 5;
				UnitRank? fixedRank = null;
				mapPointOfInterest12.AddGuards(mapPointOfInterest13.CreateUnitPayload(pointsScale6, gType5, f5, minPointsPerUnit5, maxPointsPerUnit6, minUnits5, maxUnits5, fixedRank), random);
				MapPointOfInterest mapPointOfInterest14 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest15 = mapPointOfInterest;
				float pointsScale7 = 1f;
				GameplayType? gType6 = new GameplayType?(GameplayType.Combat);
				Faction f6 = null;
				int minPointsPerUnit6 = 0;
				int maxPointsPerUnit7 = 10;
				int minUnits6 = 1;
				int maxUnits6 = 2;
				fixedRank = null;
				mapPointOfInterest14.AddTriggeredSpawn(mapPointOfInterest15.CreateUnitPayload(pointsScale7, gType6, f6, minPointsPerUnit6, maxPointsPerUnit7, minUnits6, maxUnits6, fixedRank), (float)random.RandomRange(15, 20), 0, false, true);
				MapPointOfInterest mapPointOfInterest16 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest17 = mapPointOfInterest;
				float pointsScale8 = 1f;
				GameplayType? gType7 = new GameplayType?(GameplayType.Combat);
				Faction f7 = null;
				int minPointsPerUnit7 = 0;
				int maxPointsPerUnit8 = 20;
				int minUnits7 = 1;
				int maxUnits7 = 2;
				fixedRank = null;
				mapPointOfInterest16.AddTriggeredSpawn(mapPointOfInterest17.CreateUnitPayload(pointsScale8, gType7, f7, minPointsPerUnit7, maxPointsPerUnit8, minUnits7, maxUnits7, fixedRank), (float)random.RandomRange(25, 30), 0, false, true);
				MapPointOfInterest mapPointOfInterest18 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest19 = mapPointOfInterest;
				float pointsScale9 = 1.5f;
				GameplayType? gType8 = new GameplayType?(GameplayType.Combat);
				Faction f8 = null;
				int minPointsPerUnit8 = 0;
				int maxPointsPerUnit9 = 0;
				int minUnits8 = 1;
				int maxUnits8 = 5;
				fixedRank = null;
				mapPointOfInterest18.AddTriggeredSpawn(mapPointOfInterest19.CreateUnitPayload(pointsScale9, gType8, f8, minPointsPerUnit8, maxPointsPerUnit9, minUnits8, maxUnits8, fixedRank), (float)random.RandomRange(35, 45), 0, false, true);
				num *= 1.6f;
			}
			mapPointOfInterest.AddCargoContainers(new Vector2(40f, 16f), 1, 0.2f);
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = mapPointOfInterest
			};
			missionStep.objectives.Add(new KillEnemies
			{
				enemyFaction = enemyFaction,
				requiredAmount = mapPointOfInterest.totalUnitCount
			});
			mission.steps.Add(missionStep);
			this.AddRewards(mission, num, difficulty, random, poi.faction);
			return mission;
		}
	}
}
