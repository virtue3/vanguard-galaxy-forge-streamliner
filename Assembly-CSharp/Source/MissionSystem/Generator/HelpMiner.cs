using System;
using System.Linq;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.MissionSystem.Objectives;
using Source.Simulation.World.POI;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D8 RID: 216
	public class HelpMiner : MissionGenerator
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0004138B File Offset: 0x0003F58B
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00041390 File Offset: 0x0003F590
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			if (targetLayer == null)
			{
				targetLayer = new TargetLayer?(base.GetDefaultTargetLayer(difficulty));
			}
			TargetLayer? targetLayer2 = targetLayer;
			TargetLayer targetLayer3 = TargetLayer.Core;
			OreItemData oreItemData = (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)) ? poi.system.systemOreData.surfaceOres.commonOre : poi.system.systemOreData.coreOres.commonOre;
			Faction enemyFaction = this.GetEnemyFaction(poi, difficulty, random);
			Faction faction = Faction.miningGuild;
			string id = "Foreman DC-2";
			if (poi.system.sector.quadrant > 1)
			{
				faction = poi.faction;
				id = "Warden DSF-3";
			}
			float num = this.GetRewardValue(poi.level);
			string text = "";
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Both;
			if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
			{
				text = targetLayer.GetName() + " ";
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@HelpMinerMissionName", new object[]
				{
					oreItemData.item.displayName
				}),
				category = Translation.Translate("@HelpMinerMission", Array.Empty<object>()),
				description = Translation.Translate("@HelpMinerMissionDesc", new object[]
				{
					oreItemData.item.displayName,
					text
				}),
				completionText = Translation.Translate("@HelpMinerMissionComplete", new object[]
				{
					oreItemData.item.displayName,
					poi.name
				}),
				sourcePoi = poi,
				enemyFaction = enemyFaction,
				canBeIdled = this.canBeIdled
			};
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining();
			poi.system.SetupPOI(mining, null, null, 0);
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Core;
			AsteroidFieldOreSet asteroidFieldOreSet = (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)) ? new AsteroidFieldOreSet(oreItemData, null, null) : AsteroidFieldData.CreateOreSet(poi.level, true);
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Core;
			AsteroidFieldOreSet asteroidFieldOreSet2 = (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)) ? AsteroidFieldData.CreateOreSet(poi.level, false) : new AsteroidFieldOreSet(oreItemData, null, null);
			int num2 = random.RandomRange(5, 9);
			mining.oreOwnershipOverride = faction;
			mining.storyteller = new HelpMiner(poi);
			MapPointOfInterest mapPointOfInterest = mining;
			int amount = num2;
			float density = 0.5f;
			float wealth = 4f;
			AsteroidFieldOreSet surface = asteroidFieldOreSet;
			AsteroidFieldOreSet core = asteroidFieldOreSet2;
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Core;
			mapPointOfInterest.SetAsteroidFieldData(new AsteroidFieldData(amount, density, wealth, surface, core, (float)((targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null) ? 1 : -1)), 0);
			mining.InitializeAsteroids(false, false);
			mining.AddCargoContainers(new Vector2(40f, 16f), 1, 0.2f);
			SpaceShipData spaceShipData = new SpaceShipData(id, false, faction)
			{
				canBeTracked = false,
				playerFriendly = true
			};
			spaceShipData.playerFriendly = true;
			spaceShipData.positionData.position = mining.GetPersistables().First<PersistableData>().position + new Vector2(3.5f, random.RandomRange(-4f, 4f));
			spaceShipData.positionData.rotation = (float)random.RandomRange(0, 360);
			spaceShipData.autoActions = "DefenseSubject";
			AbstractUnitData abstractUnitData = spaceShipData;
			int level = mining.level;
			float difficultyModifier = -1f;
			string seed = null;
			GameplayType? loadout = new GameplayType?(GameplayType.Mining);
			TargetLayer? targetLayer4 = targetLayer;
			targetLayer3 = TargetLayer.Surface;
			targetLayer2 = new TargetLayer?((targetLayer4.GetValueOrDefault() == targetLayer3 & targetLayer4 != null) ? TargetLayer.Core : TargetLayer.Surface);
			abstractUnitData.LoadDefaultEquipment(level, difficultyModifier, seed, loadout, null, null, false, targetLayer2);
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Core;
			string name = (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)) ? "Mining Drone Driller" : "Mining Drone Laser";
			for (int i = 0; i < spaceShipData.droneSlots.Count; i++)
			{
				spaceShipData.droneSlots[i] = Drone.Get(name);
			}
			mining.AddUnit(spaceShipData, null, false);
			if (difficulty == MissionDifficulty.Normal)
			{
				mining.dangerLevel = "@MapPOIDangerDrones";
				MapPointOfInterest mapPointOfInterest2 = mining;
				MapPointOfInterest mapPointOfInterest3 = mining;
				float pointsScale = 0.5f;
				Faction f = enemyFaction;
				mapPointOfInterest2.AddTriggeredSpawn(mapPointOfInterest3.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), f, 0, 10, 1, 5, null), (float)random.RandomRange(30, 60), 0, false, true);
				num *= 1.2f;
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				mining.dangerLevel = "@MapPOIDangerDrones";
				MapPointOfInterest mapPointOfInterest4 = mining;
				MapPointOfInterest mapPointOfInterest5 = mining;
				float pointsScale2 = 0.5f;
				Faction f = enemyFaction;
				mapPointOfInterest4.AddTriggeredSpawn(mapPointOfInterest5.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Combat), f, 0, 10, 1, 5, null), (float)random.RandomRange(30, 60), 0, false, true);
				MapPointOfInterest mapPointOfInterest6 = mining;
				MapPointOfInterest mapPointOfInterest7 = mining;
				float pointsScale3 = 0.5f;
				f = enemyFaction;
				mapPointOfInterest6.AddTriggeredSpawn(mapPointOfInterest7.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Combat), f, 0, 10, 1, 5, null), (float)random.RandomRange(90, 120), 0, false, true);
				num *= 1.4f;
			}
			else if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				mining.dangerLevel = "@MapPOIDangerPirates";
				MapPointOfInterest mapPointOfInterest8 = mining;
				MapPointOfInterest mapPointOfInterest9 = mining;
				float pointsScale4 = 1f;
				Faction f = enemyFaction;
				mapPointOfInterest8.AddTriggeredSpawn(mapPointOfInterest9.CreateUnitPayload(pointsScale4, new GameplayType?(GameplayType.Combat), f, 0, 0, 1, 5, null), (float)random.RandomRange(30, 60), 0, false, true);
				MapPointOfInterest mapPointOfInterest10 = mining;
				MapPointOfInterest mapPointOfInterest11 = mining;
				float pointsScale5 = 1f;
				f = enemyFaction;
				mapPointOfInterest10.AddTriggeredSpawn(mapPointOfInterest11.CreateUnitPayload(pointsScale5, new GameplayType?(GameplayType.Combat), f, 0, 0, 1, 5, null), (float)random.RandomRange(90, 120), 0, false, true);
				num *= 1.6f;
			}
			foreach (AbstractUnitData abstractUnitData2 in mining.GetUnits(true))
			{
				if (abstractUnitData2.faction == enemyFaction)
				{
					abstractUnitData2.alwaysHostile = true;
				}
			}
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = mining
			};
			missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
			{
				itemType = oreItemData.item,
				requiredAmount = this.GetOreAmount(difficulty, random),
				miningFaction = faction,
				targetPOI = mining.guid,
				targetLayer = targetLayer
			});
			if (mining.dangerLevel != null)
			{
				missionStep.objectives.Add(new ProtectUnit
				{
					requiredAmount = 1,
					currentAmount = 1,
					protectText = "@ProtectMinerDesc"
				});
			}
			mission.steps.Add(missionStep);
			this.AddRewards(mission, num, difficulty, random, poi.faction);
			return mission;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00041A10 File Offset: 0x0003FC10
		public override GameplayType GetMissionType()
		{
			return GameplayType.Mining;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00041A13 File Offset: 0x0003FC13
		private int GetOreAmount(MissionDifficulty missionDifficulty, SeededRandom random)
		{
			return Mathf.RoundToInt((float)random.RandomRange(10, 20) * missionDifficulty.GetObjectiveAmountMultiplier());
		}
	}
}
