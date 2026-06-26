using System;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator.Umbral
{
	// Token: 0x020000E2 RID: 226
	public class SalvageDeadDrop : UmbralMissionGenerator
	{
		// Token: 0x06000895 RID: 2197 RVA: 0x00044131 File Offset: 0x00042331
		public override GameplayType GetMissionType()
		{
			return GameplayType.Salvage;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00044134 File Offset: 0x00042334
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			InventoryItemType inventoryItemType = "SalvageDeadDrop";
			float rewardValue = this.GetRewardValue(poi.level);
			Faction faction = null;
			if (difficulty > MissionDifficulty.Easy)
			{
				faction = this.GetEnemyFaction(poi, difficulty, random);
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@UmbralInfoSalvage", Array.Empty<object>()),
				description = Translation.Translate("@UmbralInfoSalvageDesc", new object[]
				{
					faction.name
				}),
				completionText = Translation.Translate("@UmbralInfoSalvageComplete", Array.Empty<object>()),
				sourcePoi = poi,
				canBeIdled = this.canBeIdled
			};
			Source.Galaxy.POI.Salvage salvage = poi.system.SetupPOI(new Source.Galaxy.POI.Salvage(), null, null, 0) as Source.Galaxy.POI.Salvage;
			salvage.faction = Faction.darkspacers;
			for (int i = 0; i < 3; i++)
			{
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2((float)SeededRandom.Global.RandomRange(8, 40), (float)SeededRandom.Global.RandomRange(-16, 16)),
					angle = (float)random.RandomRange(0, 360),
					shipTemplate = salvage.FindSalvageShipTemplate(faction)
				};
				if (i == 0)
				{
					salvageData.itemContent.Add(new SalvageItemData(inventoryItemType, 9999f, true));
				}
				salvageData.AddItemContent(poi.level, 1, 1f);
				salvageData.AddScrapContent(poi.level, 0.5f, 2);
				salvageData.AddStructuralContent(poi.level, 2, 1f);
				salvage.AddPersistable(salvageData);
			}
			salvage.AddCargoContainers(new Vector2(20f, 16f), 1, 0.2f);
			CombatStationData combatStationData = CombatStationFactory.CreateMediumStation(salvage, new Vector2?(new Vector2(30f, 2f)));
			combatStationData.playerHostile = true;
			combatStationData.noReputationLoss = true;
			salvage.AddPirateTurrets(3, random, null).ForEach(delegate(AbstractUnitData a)
			{
				a.playerHostile = true;
				a.noReputationLoss = true;
			});
			MissionStep missionStep = new MissionStep
			{
				system = poi.system,
				dynamicPointOfInterest = salvage
			};
			missionStep.objectives.Add(new Source.MissionSystem.Objectives.Salvage
			{
				itemType = inventoryItemType,
				requiredAmount = 1,
				targetPOI = salvage.guid
			});
			MissionStep missionStep2 = new MissionStep
			{
				system = poi.system,
				poiHint = poi
			};
			missionStep2.objectives.Add(new TradeOffer
			{
				itemType = inventoryItemType,
				requiredAmount = 1,
				deliverTo = (poi as SpaceStation)
			});
			mission.steps.Add(missionStep);
			mission.steps.Add(missionStep2);
			this.AddRewards(mission, rewardValue, difficulty, random, poi.faction);
			return mission;
		}
	}
}
