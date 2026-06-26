using System;
using System.Collections.Generic;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator.Umbral
{
	// Token: 0x020000E0 RID: 224
	public class BountyHunt : UmbralMissionGenerator
	{
		// Token: 0x0600088E RID: 2190 RVA: 0x00043BF3 File Offset: 0x00041DF3
		public override GameplayType GetMissionType()
		{
			return GameplayType.Combat;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00043BF8 File Offset: 0x00041DF8
		public override Faction GetEnemyFaction(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random)
		{
			List<Faction> list = new List<Faction>(Faction.corporations);
			list.Remove(poi.system.faction);
			return random.Choose<Faction>(list);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00043C2C File Offset: 0x00041E2C
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			string text = "@UmbralBountyHunt";
			string text2 = "@UmbralBountyHuntDesc";
			string text3 = "@UmbralBountyHuntComplete";
			Mission mission = new Mission
			{
				name = Translation.Translate(text, new object[]
				{
					enemyFaction.name
				}),
				description = Translation.Translate(text2, new object[]
				{
					enemyFaction.name
				}),
				completionText = Translation.Translate(text3, new object[]
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
			mapPointOfInterest.AddGuards(mapPointOfInterest.CreateUnitPayload(1.3f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), random);
			mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest.CreateUnitPayload(1.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(5, 10), 0, true, true);
			mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest.CreateUnitPayload(1.7f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(5, 10), 1, true, true);
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
			this.AddRewards(mission, this.GetRewardValue(poi.level), difficulty, random, poi.faction);
			return mission;
		}
	}
}
