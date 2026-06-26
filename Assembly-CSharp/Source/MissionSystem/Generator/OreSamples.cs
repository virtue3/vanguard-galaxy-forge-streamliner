using System;
using System.Collections.Generic;
using Behaviour.Hazard;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.Weapons;
using Source.Combat;
using Source.Data;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.Mining;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000DA RID: 218
	public class OreSamples : MissionGenerator
	{
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00041F27 File Offset: 0x00040127
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00041F2C File Offset: 0x0004012C
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			if (targetLayer == null)
			{
				targetLayer = new TargetLayer?(base.GetDefaultTargetLayer(difficulty));
			}
			string[] list = new string[]
			{
				"MiningMissionItem0",
				"MiningMissionItem1",
				"MiningMissionItem2",
				"MiningMissionItem3"
			};
			InventoryItemType inventoryItemType = random.Choose<string>(list);
			float num = this.GetRewardValue(poi.level);
			string str = "";
			TargetLayer? targetLayer2 = targetLayer;
			TargetLayer targetLayer3 = TargetLayer.Both;
			if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
			{
				str = Translation.Translate("@OreSamplesMissionTargetLayer", new object[]
				{
					targetLayer.GetName()
				}) + " ";
			}
			Faction faction = null;
			if (difficulty > MissionDifficulty.Easy)
			{
				faction = this.GetEnemyFaction(poi, difficulty, random);
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@OreSamplesMissionName", new object[]
				{
					inventoryItemType.displayName
				}),
				category = Translation.Translate("@OreSamplesMission", Array.Empty<object>()),
				description = Translation.Translate("@OreSamplesMissionDesc", new object[]
				{
					inventoryItemType.displayName
				}) + " " + str,
				completionText = Translation.Translate("@OreSamplesMissionComplete", new object[]
				{
					inventoryItemType.displayName,
					poi.name
				}),
				sourcePoi = poi,
				enemyFaction = faction,
				canBeIdled = this.canBeIdled
			};
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining();
			poi.system.SetupPOI(mining, null, null, 0);
			mining.faction = faction;
			bool flag = random.RandomBool(0.5f);
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Core;
			AsteroidFieldOreSet asteroidFieldOreSet;
			AsteroidFieldOreSet asteroidFieldOreSet2;
			if (targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null)
			{
				asteroidFieldOreSet = new AsteroidFieldOreSet(inventoryItemType.GetComponent<OreItemData>(), null, null);
				asteroidFieldOreSet2 = AsteroidFieldData.CreateOreSet(poi.level, true);
			}
			else
			{
				asteroidFieldOreSet2 = new AsteroidFieldOreSet(inventoryItemType.GetComponent<OreItemData>(), null, null);
				asteroidFieldOreSet = AsteroidFieldData.CreateOreSet(poi.level, false);
			}
			int num2 = random.RandomRange(3, 6) + (flag ? 2 : 0);
			MapPointOfInterest mapPointOfInterest = mining;
			int amount = num2;
			float density = 0.5f;
			float wealth = 4f;
			AsteroidFieldOreSet surface = asteroidFieldOreSet2;
			AsteroidFieldOreSet core = asteroidFieldOreSet;
			targetLayer2 = targetLayer;
			targetLayer3 = TargetLayer.Core;
			mapPointOfInterest.SetAsteroidFieldData(new AsteroidFieldData(amount, density, wealth, surface, core, (float)((targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null) ? 1 : -1)), 0);
			List<AbstractUnitData> list2 = null;
			if (difficulty == MissionDifficulty.Normal)
			{
				mining.dangerLevel = "@MapPOIDangerTurrets";
				list2 = mining.CreateTurretPayload(random.RandomRange(2, mining.asteroidFieldData.amount + 1), null, random);
				num *= 1.2f;
				mining.hazardFieldData = new HazardFieldData
				{
					hazardName = HazardName.DamageInRadius,
					damageType = DamageType.Radiation,
					spawnChance = 0.3f
				};
				mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				mining.dangerLevel = "@MapPOIDangerDrones";
				list2 = mining.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null);
				if (random.RandomBool(0.5f))
				{
					mining.AddTriggeredSpawn(mining.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(100, 200), 0, false, true);
				}
				num *= 1.4f;
				mining.hazardFieldData = new HazardFieldData
				{
					hazardName = HazardName.DamageInRadius,
					damageType = DamageType.Radiation,
					spawnChance = 0.6f
				};
				mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
			}
			else if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				mining.dangerLevel = "@MapPOIDangerPirates";
				list2 = mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null);
				if (random.RandomBool(0.5f))
				{
					mining.AddTriggeredSpawn(mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(100, 200), 0, false, true);
				}
				num *= 1.6f;
				mining.hazardFieldData = new HazardFieldData
				{
					hazardName = HazardName.DamageInRadius,
					damageType = DamageType.Radiation,
					spawnChance = 0.8f
				};
				mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
			}
			if (faction != null && !Faction.player.IsEnemy(faction))
			{
				mining.dangerLevel = "@MapPOIAttackFriendlies";
			}
			mining.InitializeAsteroids(false, false);
			mining.AddCargoContainers(new Vector2(40f, 16f), 1, 0.4f);
			if (list2 != null)
			{
				mining.AddGuards(list2, null);
			}
			if (flag)
			{
				mining.AddGuards(mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), Faction.miningGuild, 0, 0, 1, 1, null), random).ForEach(delegate(AbstractUnitData a)
				{
					a.autoActions = "AmbientMiner";
				});
			}
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = mining
			};
			int oreAmount = this.GetOreAmount(difficulty, random);
			missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
			{
				itemType = inventoryItemType,
				requiredAmount = oreAmount,
				targetLayer = targetLayer
			});
			MissionStep missionStep2 = new MissionStep
			{
				system = poi.system
			};
			missionStep2.objectives.Add(new TradeOffer
			{
				itemType = inventoryItemType,
				requiredAmount = oreAmount,
				deliverTo = (poi as SpaceStation)
			});
			mission.steps.Add(missionStep);
			mission.steps.Add(missionStep2);
			this.AddRewards(mission, num, difficulty, random, poi.faction);
			return mission;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0004252E File Offset: 0x0004072E
		public override GameplayType GetMissionType()
		{
			return GameplayType.Mining;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00042531 File Offset: 0x00040731
		private int GetOreAmount(MissionDifficulty missionDifficulty, SeededRandom random)
		{
			return Mathf.RoundToInt((float)random.RandomRange(10, 20) * missionDifficulty.GetObjectiveAmountMultiplier());
		}
	}
}
