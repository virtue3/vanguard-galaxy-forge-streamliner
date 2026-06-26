using System;
using Behaviour.Hazard;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Combat;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.Mining;
using Source.MissionSystem.Objectives;
using Source.Simulation.World.POI;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator.Umbral
{
	// Token: 0x020000E1 RID: 225
	public class MiningDeadDrop : UmbralMissionGenerator
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x00043E44 File Offset: 0x00042044
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			InventoryItemType itemType = "MiningDeadDrop";
			float rewardValue = this.GetRewardValue(poi.level);
			Faction faction = null;
			if (difficulty > MissionDifficulty.Easy)
			{
				faction = this.GetEnemyFaction(poi, difficulty, random);
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@UmbralDeadDropMining", Array.Empty<object>()),
				description = Translation.Translate("@UmbralDeadDropMiningDesc", new object[]
				{
					faction.name
				}),
				completionText = Translation.Translate("@UmbralDeadDropMiningComplete", Array.Empty<object>()),
				sourcePoi = poi,
				enemyFaction = faction,
				canBeIdled = this.canBeIdled
			};
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining();
			poi.system.SetupPOI(mining, null, null, 0);
			mining.faction = faction;
			mining.storyteller = new UmbralDeadDrop(mining);
			int amount = random.RandomRange(8, 12);
			mining.SetAsteroidFieldData(new AsteroidFieldData(amount, 0.5f, 2f, AsteroidFieldData.CreateOreSet(poi.level, true), AsteroidFieldData.CreateOreSet(poi.level, false), -1f), 0);
			mining.hazardFieldData = new HazardFieldData
			{
				hazardName = HazardName.DamageInRadius,
				damageType = DamageType.Radiation,
				spawnChance = 0.6f
			};
			mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
			mining.InitializeAsteroids(false, false);
			mining.AddCargoContainers(new Vector2(40f, 16f), 1, 0.4f);
			mining.AddGuards(mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), faction, 0, 0, 1, 5, null), random).ForEach(delegate(AbstractUnitData a)
			{
				a.autoActions = "AmbientMiner";
				a.playerHostile = true;
			});
			mining.AddTriggeredSpawn(mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), 60f, 0, false, true).ForEach(delegate(AbstractUnitData a)
			{
				a.playerHostile = true;
			});
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = mining
			};
			missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
			{
				itemType = itemType,
				requiredAmount = 1,
				targetLayer = targetLayer
			});
			MissionStep missionStep2 = new MissionStep
			{
				system = poi.system
			};
			missionStep2.objectives.Add(new TradeOffer
			{
				itemType = itemType,
				requiredAmount = 1,
				deliverTo = (poi as SpaceStation)
			});
			mission.steps.Add(missionStep);
			mission.steps.Add(missionStep2);
			this.AddRewards(mission, rewardValue, difficulty, random, poi.faction);
			return mission;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00044126 File Offset: 0x00042326
		public override GameplayType GetMissionType()
		{
			return GameplayType.Mining;
		}
	}
}
