using System;
using System.Collections.Generic;
using Behaviour.Weapons;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.MissionSystem.Objectives;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D3 RID: 211
	public class ClearAsteroidField : MissionGenerator
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x000402D6 File Offset: 0x0003E4D6
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x000402D9 File Offset: 0x0003E4D9
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			if (GamePlayer.current.starterSpecialization == 8)
			{
				return 1f;
			}
			return (float)((GamePlayer.current.IsMissionCompleted("tutorial_9") || GamePlayer.current.IsInSandBox()) ? 1 : 0);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00040310 File Offset: 0x0003E510
		public override GameplayType GetMissionType()
		{
			return GameplayType.Combat;
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00040314 File Offset: 0x0003E514
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			float rewardValue = this.GetRewardValue(poi.level);
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			string text = "@ClearAsteroidFieldMissionDesc";
			string text2 = "@ClearAsteroidFieldObjective";
			if (enemyFaction != Faction.marauders)
			{
				text = "@ClearAsteroidField2MissionDesc";
				text2 = "@ClearAsteroidField2Objective";
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@ClearAsteroidFieldMissionName", Array.Empty<object>()),
				category = Translation.Translate("@ClearAsteroidFieldMission", Array.Empty<object>()),
				description = Translation.Translate(text, new object[]
				{
					enemyFaction.name
				}),
				completionText = Translation.Translate("@ClearAsteroidFieldMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				iconName = "Combat",
				enemyFaction = enemyFaction,
				canBeIdled = this.canBeIdled
			};
			Combat combat = new Combat();
			poi.system.SetupPOI(combat, null, null, 0);
			combat.faction = enemyFaction;
			combat.dangerLevel = (Faction.player.IsEnemy(enemyFaction) ? "@MapPOIDangerPirates" : "@MapPOIAttackFriendlies");
			AsteroidFieldOreSet surface = AsteroidFieldData.CreateOreSet(poi.level, true);
			AsteroidFieldOreSet core = AsteroidFieldData.CreateOreSet(poi.level, false);
			int amount = random.RandomRange(8, 13);
			combat.SetAsteroidFieldData(new AsteroidFieldData(amount, 0.5f, 1f, surface, core, -1f), 0);
			combat.InitializeAsteroids(false, false);
			combat.AddCargoContainers(new Vector2(50f, 16f), 1, 0.25f);
			List<AbstractUnitData> list = combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), null, 0, 0, 1, 5, null);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].autoActions = "AmbientMiner";
			}
			combat.AddGuards(list, null);
			int totalUnitCount = combat.totalUnitCount;
			if (difficulty == MissionDifficulty.Easy)
			{
				combat.AddPirateTurrets(2, random, null);
			}
			else if (difficulty == MissionDifficulty.Normal)
			{
				combat.AddPirateTurrets(2, random, null);
				combat.AddGuards(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), random);
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				combat.AddPirateTurrets(2, random, null);
				combat.AddGuards(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), random);
				combat.AddTriggeredSpawn(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(5, 15), 0, false, true);
			}
			else if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				combat.AddPirateTurrets(3, random, null);
				combat.AddGuards(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), random);
				combat.AddTriggeredSpawn(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(5, 15), 0, false, true);
				combat.AddTriggeredSpawn(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(15, 25), 0, false, true);
				combat.AddTriggeredSpawn(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(25, 35), 0, false, true);
			}
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = combat
			};
			missionStep.objectives.Add(new TriggerObjective
			{
				trigger = MissionTrigger.MinerChasedOff,
				description = Translation.Translate(text2, new object[]
				{
					enemyFaction.name
				}),
				requiredAmount = totalUnitCount,
				gameplayType = GameplayType.Combat
			});
			mission.steps.Add(missionStep);
			this.AddRewards(mission, rewardValue, difficulty, random, poi.faction);
			return mission;
		}
	}
}
