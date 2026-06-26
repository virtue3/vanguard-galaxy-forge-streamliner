using System;
using System.Collections.Generic;
using Behaviour.Hazard;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Combat;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000DB RID: 219
	public class SalvageSamples : MissionGenerator
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00042552 File Offset: 0x00040752
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00042558 File Offset: 0x00040758
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			if (targetLayer == null)
			{
				targetLayer = new TargetLayer?(base.GetDefaultTargetLayer(difficulty));
			}
			string[] list = new string[]
			{
				"SalvageOreMissionItem0",
				"SalvageOreMissionItem1",
				"SalvageOreMissionItem2",
				"SalvageOreMissionItem3"
			};
			InventoryItemType inventoryItemType = random.Choose<string>(list);
			float num = this.GetRewardValue(poi.level);
			string str = "";
			TargetLayer? targetLayer2 = targetLayer;
			TargetLayer targetLayer3 = TargetLayer.Both;
			if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
			{
				str = Translation.Translate("@SalvageOreMissionTargetLayer", new object[]
				{
					targetLayer.GetSalvageName()
				}) + " ";
			}
			Faction faction = null;
			if (difficulty > MissionDifficulty.Easy)
			{
				faction = this.GetEnemyFaction(poi, difficulty, random);
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@SalvageOreMissionName", new object[]
				{
					inventoryItemType.displayName
				}),
				category = Translation.Translate("@SalvageOreMission", Array.Empty<object>()),
				description = Translation.Translate("@SalvageOreMissionDesc", new object[]
				{
					inventoryItemType.displayName
				}) + " " + str,
				completionText = Translation.Translate("@SalvageOreMissionComplete", new object[]
				{
					inventoryItemType.displayName,
					poi.name
				}),
				sourcePoi = poi,
				enemyFaction = faction,
				canBeIdled = this.canBeIdled
			};
			Source.Galaxy.POI.Salvage salvage = new Source.Galaxy.POI.Salvage();
			poi.system.SetupPOI(salvage, null, null, 0);
			float num2 = 0f;
			DamageType damageType = DamageType.Radiation;
			if (difficulty > MissionDifficulty.Easy)
			{
				salvage.faction = faction;
				num2 = ((difficulty == MissionDifficulty.Normal) ? 0.3f : ((difficulty > MissionDifficulty.Hard) ? 0.8f : 0.6f));
				salvage.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + Translation.Translate("@" + damageType.ToString(), Array.Empty<object>());
			}
			bool flag = random.RandomBool(0.5f);
			int num3 = flag ? 6 : 4;
			if (difficulty == MissionDifficulty.Hard)
			{
				num3++;
			}
			else if (difficulty > MissionDifficulty.Hard)
			{
				num3 += 2;
			}
			float num4 = 0f;
			for (int i = 0; i < num3; i++)
			{
				num4 += random.RandomRange(9f, 14f);
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2(num4, random.RandomRange(-12f, 12f)),
					angle = (float)random.RandomRange(0, 360),
					shipTemplate = (random.RandomBool(0.5f) ? "AncientWreck" : "DroneWreck"),
					initialBattleDamage = 80,
					hazardData = ((num2 > 0f && (i == 0 || random.RandomBool(num2))) ? poi.CreateHazardData(HazardName.DamageInRadius, DamageType.Radiation) : null)
				};
				targetLayer2 = targetLayer;
				targetLayer3 = TargetLayer.Core;
				if (!(targetLayer2.GetValueOrDefault() == targetLayer3 & targetLayer2 != null))
				{
					salvageData.scrapContent.Add(inventoryItemType, random.RandomRange(11, flag ? 28 : 17));
					salvageData.AddStructuralContent(salvage.level, 2, 1f);
				}
				else
				{
					salvageData.structuralContent.Add(inventoryItemType, random.RandomRange(11, flag ? 28 : 17));
					salvageData.AddScrapContent(salvage.level, 1f, 2);
				}
				salvage.AddPersistable(salvageData);
			}
			List<AbstractUnitData> list2 = null;
			if (difficulty == MissionDifficulty.Normal)
			{
				salvage.dangerLevel = "@MapPOIDangerTurrets";
				list2 = salvage.CreateTurretPayload(random.RandomRange(2, 3), null, random);
				num *= 1.2f;
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				salvage.dangerLevel = "@MapPOIDangerDrones";
				list2 = salvage.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null);
				if (random.RandomBool(0.5f))
				{
					salvage.AddTriggeredSpawn(salvage.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(100, 200), 0, false, true);
				}
				num *= 1.4f;
			}
			else if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				salvage.dangerLevel = "@MapPOIDangerPirates";
				list2 = salvage.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null);
				if (random.RandomBool(0.5f))
				{
					salvage.AddTriggeredSpawn(salvage.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), (float)random.RandomRange(100, 200), 0, false, true);
				}
				num *= 1.6f;
			}
			if (faction != null && !Faction.player.IsEnemy(faction))
			{
				salvage.dangerLevel = "@MapPOIAttackFriendlies";
			}
			salvage.AddCargoContainers(new Vector2(30f, 16f), 1, 0.4f);
			if (list2 != null)
			{
				salvage.AddGuards(list2, null);
			}
			if (flag)
			{
				salvage.AddGuards(salvage.CreateUnitPayload(1f, new GameplayType?(GameplayType.Salvage), Faction.salvageGuild, 0, 0, 1, 1, null), random).ForEach(delegate(AbstractUnitData a)
				{
					a.autoActions = "AmbientSalvager";
				});
			}
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = salvage
			};
			int oreAmount = this.GetOreAmount(difficulty, random);
			missionStep.objectives.Add(new Source.MissionSystem.Objectives.Salvage
			{
				itemType = inventoryItemType,
				requiredAmount = oreAmount,
				targetLayer = targetLayer
			});
			MissionStep missionStep2 = new MissionStep
			{
				system = poi.system,
				poiHint = poi
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

		// Token: 0x06000876 RID: 2166 RVA: 0x00042B82 File Offset: 0x00040D82
		public override GameplayType GetMissionType()
		{
			return GameplayType.Salvage;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00042B85 File Offset: 0x00040D85
		private int GetOreAmount(MissionDifficulty missionDifficulty, SeededRandom random)
		{
			return Mathf.RoundToInt((float)random.RandomRange(10, 20) * missionDifficulty.GetObjectiveAmountMultiplier());
		}
	}
}
