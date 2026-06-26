using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.Util;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D5 RID: 213
	public class Courier : MissionGenerator
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00040C19 File Offset: 0x0003EE19
		public override bool canBeIdled
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00040C1C File Offset: 0x0003EE1C
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			return (float)((source.level > 2) ? 1 : 0);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00040C2C File Offset: 0x0003EE2C
		public override GameplayType GetMissionType()
		{
			return GameplayType.Cargo;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00040C30 File Offset: 0x0003EE30
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty diff, SeededRandom random, TargetLayer? targetLayer = null)
		{
			string[] list = new string[]
			{
				"CourierMissionItem0",
				"CourierMissionItem1",
				"CourierMissionItem2",
				"CourierMissionItem3"
			};
			float itemValue = this.GetRewardValue(poi.level) * 0.6f;
			InventoryItemType inventoryItemType = random.Choose<string>(list);
			int num = 30;
			int num2 = 1;
			if (diff == MissionDifficulty.Easy)
			{
				num = 30;
				num2++;
			}
			else if (diff == MissionDifficulty.Normal)
			{
				num = 40;
				num2++;
			}
			else if (diff == MissionDifficulty.Hard)
			{
				num = 80;
				num2 += 2;
			}
			else if (diff == MissionDifficulty.Skull)
			{
				num = 120;
				num2 += 3;
			}
			else if (diff == MissionDifficulty.Insane)
			{
				num = 200;
				num2 += 3;
			}
			int num3 = 0;
			SpaceStation spaceStation = null;
			HashSet<SystemMapData> hashSet = new HashSet<SystemMapData>();
			while (spaceStation == null)
			{
				SystemMapData systemMapData = poi.system;
				hashSet.Clear();
				for (int i = 0; i < num2; i++)
				{
					hashSet.Add(systemMapData);
					List<JumpGate> list2 = new List<JumpGate>(systemMapData.GetJumpGateList(false));
					random.Shuffle<JumpGate>(list2);
					foreach (JumpGate jumpGate in list2)
					{
						SystemMapData targetSystem = jumpGate.targetSystem;
						if (targetSystem != null && !hashSet.Contains(targetSystem) && !targetSystem.pocketSystem && targetSystem.isUnlocked)
						{
							systemMapData = targetSystem;
							break;
						}
					}
				}
				if (systemMapData != poi.system)
				{
					foreach (SpaceStation spaceStation2 in systemMapData.GetPointsOfInterest<SpaceStation>())
					{
						if (!spaceStation2.faction.IsEnemy(poi.faction) || spaceStation2 is EmbassyStation)
						{
							spaceStation = spaceStation2;
							break;
						}
					}
				}
				if (spaceStation == null)
				{
					num3++;
					if (num3 > 60)
					{
						return null;
					}
					if (num3 % 20 == 0)
					{
						num2++;
					}
				}
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@CourierMissionName", new object[]
				{
					spaceStation.name
				}),
				category = Translation.Translate("@CourierMission", Array.Empty<object>()),
				description = Translation.Translate("@CourierMissionDesc", new object[]
				{
					inventoryItemType.displayName,
					spaceStation.name,
					spaceStation.system.name
				}),
				completionText = Translation.Translate("@CourierMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				turnIn = spaceStation,
				canBeIdled = this.canBeIdled
			};
			mission.missionItems.Add(inventoryItemType, num);
			mission.removeItemsOnAbandon = true;
			MissionStep missionStep = new MissionStep
			{
				poiHint = spaceStation
			};
			missionStep.objectives.Add(new TradeOffer
			{
				itemType = inventoryItemType,
				requiredAmount = num,
				deliverTo = spaceStation,
				gameplayType = GameplayType.Cargo
			});
			mission.steps.Add(missionStep);
			this.AddRewards(mission, itemValue, diff, random, poi.faction);
			return mission;
		}
	}
}
