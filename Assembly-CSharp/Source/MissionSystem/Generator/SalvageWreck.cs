using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000DC RID: 220
	public class SalvageWreck : MissionGenerator
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00042BA6 File Offset: 0x00040DA6
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00042BA9 File Offset: 0x00040DA9
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			return (float)((GamePlayer.current.IsMissionCompleted("tutorial_7") || GamePlayer.current.IsInSandBox()) ? 1 : 0);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00042BCD File Offset: 0x00040DCD
		public override GameplayType GetMissionType()
		{
			return GameplayType.Salvage;
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00042BD0 File Offset: 0x00040DD0
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			string[] list = new string[]
			{
				"SalvageMissionItem0",
				"SalvageMissionItem1",
				"SalvageMissionItem2",
				"SalvageMissionItem3"
			};
			InventoryItemType inventoryItemType = random.Choose<string>(list);
			float num = this.GetRewardValue(poi.level);
			Faction faction = null;
			if (difficulty > MissionDifficulty.Easy)
			{
				faction = this.GetEnemyFaction(poi, difficulty, random);
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@SalvageWreckMissionName", new object[]
				{
					inventoryItemType.displayName
				}),
				category = Translation.Translate("@SalvageWreckMission", Array.Empty<object>()),
				description = Translation.Translate("@SalvageWreckMissionDesc", new object[]
				{
					inventoryItemType.displayName
				}),
				completionText = Translation.Translate("@SalvageWreckMissionComplete", new object[]
				{
					inventoryItemType.displayName,
					poi.name
				}),
				sourcePoi = poi,
				enemyFaction = faction,
				canBeIdled = this.canBeIdled
			};
			Source.Galaxy.POI.Salvage salvage = poi.system.SetupPOI(new Source.Galaxy.POI.Salvage(), null, null, 0) as Source.Galaxy.POI.Salvage;
			salvage.faction = faction;
			List<Faction> list2 = new List<Faction>
			{
				Faction.red,
				Faction.gold,
				Faction.blue,
				Faction.tradingGuild,
				Faction.miningGuild
			};
			SalvageData salvageData = new SalvageData
			{
				position = salvage.GetWorldPosition() + new Vector2(8f, 2f),
				angle = (float)random.RandomRange(0, 360),
				shipTemplate = salvage.FindSalvageShipTemplate(random.Choose<Faction>(list2))
			};
			salvageData.itemContent.Add(new SalvageItemData(inventoryItemType, 9999f, true));
			if (random.RandomBool(this.GetItemChanceForDifficulty(difficulty)))
			{
				salvageData.AddItemContent(poi.level, this.GetItemCountForDifficulty(difficulty, random), 1f);
			}
			salvageData.AddScrapContent(poi.level, 0.5f, 2);
			salvageData.AddStructuralContent(poi.level, 2, 1f);
			salvage.AddPersistable(salvageData);
			salvage.AddCargoContainers(new Vector2(20f, 16f), 1, 0.2f);
			if (difficulty == MissionDifficulty.Normal)
			{
				salvage.dangerLevel = "@MapPOIDangerTurrets";
				salvage.AddPirateTurrets(2, random, null);
				num *= 1.2f;
			}
			else if (difficulty == MissionDifficulty.Hard)
			{
				salvage.dangerLevel = "@MapPOIDangerDrones";
				salvage.AddGuards(salvage.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 10, 1, 5, null), random);
				if (random.RandomBool(0.5f))
				{
					salvage.AddTriggeredSpawn(salvage.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 10, 1, 5, null), (float)random.RandomRange(100, 200), 0, false, true);
				}
				num *= 1.4f;
			}
			else if (difficulty == MissionDifficulty.Skull || difficulty == MissionDifficulty.Insane)
			{
				salvage.dangerLevel = "@MapPOIDangerPirates";
				salvage.AddGuards(salvage.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 10, 1, 5, null), random);
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
			this.AddRewards(mission, num, difficulty, random, poi.faction);
			return mission;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00043040 File Offset: 0x00041240
		private float GetItemChanceForDifficulty(MissionDifficulty difficulty)
		{
			float result;
			switch (difficulty)
			{
			case MissionDifficulty.Normal:
				result = 0.6f;
				break;
			case MissionDifficulty.Hard:
				result = 0.7f;
				break;
			case MissionDifficulty.Skull:
				result = 0.8f;
				break;
			case MissionDifficulty.Insane:
				result = 0.9f;
				break;
			default:
				result = 0.5f;
				break;
			}
			return result;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00043090 File Offset: 0x00041290
		private int GetItemCountForDifficulty(MissionDifficulty difficulty, SeededRandom random)
		{
			int result;
			switch (difficulty)
			{
			case MissionDifficulty.Normal:
				result = random.RandomRange(1, 2);
				break;
			case MissionDifficulty.Hard:
				result = random.RandomRange(1, 2);
				break;
			case MissionDifficulty.Skull:
				result = random.RandomRange(1, 3);
				break;
			case MissionDifficulty.Insane:
				result = random.RandomRange(2, 3);
				break;
			default:
				result = 1;
				break;
			}
			return result;
		}
	}
}
