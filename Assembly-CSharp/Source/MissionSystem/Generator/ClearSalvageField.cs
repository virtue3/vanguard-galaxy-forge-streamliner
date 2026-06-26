using System;
using System.Collections.Generic;
using Behaviour.Weapons;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D4 RID: 212
	public class ClearSalvageField : MissionGenerator
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x00040739 File Offset: 0x0003E939
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0004073C File Offset: 0x0003E93C
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			if (GamePlayer.current.starterSpecialization == 8)
			{
				return 1f;
			}
			return (float)((GamePlayer.current.IsMissionCompleted("tutorial_9") || GamePlayer.current.IsInSandBox()) ? 1 : 0);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00040773 File Offset: 0x0003E973
		public override GameplayType GetMissionType()
		{
			return GameplayType.Combat;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00040778 File Offset: 0x0003E978
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			float rewardValue = this.GetRewardValue(poi.level);
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			string text = "@ClearSalvageFieldMissionDesc";
			string text2 = "@ClearSalvageFieldObjective";
			if (enemyFaction != Faction.marauders)
			{
				text = "@ClearSalvageField2MissionDesc";
				text2 = "@ClearSalvageField2Objective";
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@ClearSalvageFieldMissionName", Array.Empty<object>()),
				category = Translation.Translate("@ClearSalvageFieldMission", Array.Empty<object>()),
				description = Translation.Translate(text, new object[]
				{
					enemyFaction.name
				}),
				completionText = Translation.Translate("@ClearSalvageFieldMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				iconName = "Combat",
				enemyFaction = enemyFaction,
				canBeIdled = this.canBeIdled
			};
			Combat combat = new Combat();
			poi.system.SetupPOI(combat, null, null, 0);
			combat.faction = enemyFaction;
			combat.dangerLevel = (Faction.player.IsEnemy(enemyFaction) ? "@MapPOIDangerPirates" : "@MapPOIAttackFriendlies");
			float num = 0f;
			for (int i = 0; i < 3; i++)
			{
				num += random.RandomRange(9f, 14f);
				SalvageData salvageData = new SalvageData
				{
					position = combat.GetWorldPosition() + new Vector2(num, random.RandomRange(-12f, 12f)),
					angle = (float)random.RandomRange(0, 360),
					shipTemplate = (random.RandomBool(0.5f) ? "AncientWreck" : "DroneWreck"),
					initialBattleDamage = 80
				};
				salvageData.AddScrapContent(combat.level, 0.5f, 2);
				salvageData.AddStructuralContent(combat.level, 2, 1f);
				combat.AddPersistable(salvageData);
			}
			combat.AddCargoContainers(new Vector2(50f, 16f), 1, 0.25f);
			List<AbstractUnitData> list = combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Salvage), null, 0, 0, 1, 5, null);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].autoActions = "AmbientSalvager";
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
				trigger = MissionTrigger.SalvagerChasedOff,
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
