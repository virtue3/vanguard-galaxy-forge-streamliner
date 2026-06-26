using System;
using System.Collections.Generic;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.Economy;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000DF RID: 223
	public class TradeTerminal : MissionGenerator
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00043810 File Offset: 0x00041A10
		public override bool canBeIdled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00043814 File Offset: 0x00041A14
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			using (IEnumerator<SpaceStation> enumerator = poi.system.GetPointsOfInterest<SpaceStation>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.faction == Faction.tradingGuild)
					{
						return null;
					}
				}
			}
			SpaceStation spaceStation = null;
			foreach (SystemMapData systemMapData in poi.system.sector.allSystems)
			{
				foreach (SpaceStation spaceStation2 in systemMapData.GetPointsOfInterest<SpaceStation>())
				{
					if (spaceStation2.faction == Faction.tradingGuild && spaceStation2.economy != null)
					{
						spaceStation = spaceStation2;
						break;
					}
				}
			}
			if (spaceStation == null)
			{
				return null;
			}
			float num = 0f;
			foreach (SpaceShipData spaceShipData in GamePlayer.current.spaceShips)
			{
				num = Mathf.Max(num, (float)spaceShipData.shipClass._cargoCapacity);
			}
			int num2;
			switch (difficulty)
			{
			case MissionDifficulty.Easy:
				num2 = 50;
				break;
			case MissionDifficulty.Normal:
				num2 = 75;
				break;
			case MissionDifficulty.Hard:
				num2 = 100;
				break;
			case MissionDifficulty.Skull:
				num2 = 150;
				break;
			case MissionDifficulty.Insane:
				num2 = 200;
				break;
			default:
				num2 = 75;
				break;
			}
			float num3 = (float)num2;
			num3 = (float)GameMath.GetCreditsValue(num3, poi.level);
			List<LocalEconomyItem> list = new List<LocalEconomyItem>(spaceStation.economy.allItems);
			if (list.Count == 0)
			{
				return null;
			}
			random.Shuffle<LocalEconomyItem>(list);
			LocalEconomyItem localEconomyItem = null;
			int num4 = 0;
			do
			{
				localEconomyItem = list[0];
				num4 = Mathf.CeilToInt(num3 / (float)list[0].cost);
				list.RemoveAt(0);
			}
			while ((localEconomyItem.currentSupply < num4 || (float)num4 * localEconomyItem.item.m3 > num) && list.Count > 0);
			if (localEconomyItem == null)
			{
				return null;
			}
			Mission mission = new Mission
			{
				name = Translation.Translate("@TradeTerminalMissionName", new object[]
				{
					localEconomyItem.item.displayName
				}),
				category = Translation.Translate("@TradeTerminalMission", Array.Empty<object>()),
				description = Translation.Translate("@TradeTerminalMissionDesc", new object[]
				{
					localEconomyItem.item.displayName
				}),
				completionText = Translation.Translate("@TradeTerminalMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				turnIn = poi,
				canBeIdled = this.canBeIdled
			};
			MissionStep missionStep = new MissionStep
			{
				poiHint = poi
			};
			missionStep.objectives.Add(new TradeOffer
			{
				itemType = localEconomyItem.item,
				requiredAmount = num4,
				deliverTo = (poi as SpaceStation),
				gameplayType = GameplayType.Cargo
			});
			mission.steps.Add(missionStep);
			this.AddRewards(mission, this.GetRewardValue(poi.level), difficulty, random, poi.faction);
			bool flag = false;
			foreach (MissionReward missionReward in mission.rewards)
			{
				Credits credits = missionReward as Credits;
				if (credits != null)
				{
					credits.amount += num4 * localEconomyItem.cost;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				mission.rewards.Add(new Credits
				{
					amount = num4 * localEconomyItem.cost
				});
			}
			return mission;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00043BE8 File Offset: 0x00041DE8
		public override GameplayType GetMissionType()
		{
			return GameplayType.Cargo;
		}
	}
}
