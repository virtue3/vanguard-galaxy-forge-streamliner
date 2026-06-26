using System;
using System.Collections.Generic;
using Behaviour.Item;
using Source.Galaxy;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator.Umbral
{
	// Token: 0x020000E3 RID: 227
	public abstract class UmbralMissionGenerator : MissionGenerator
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00044405 File Offset: 0x00042605
		public override bool canBeIdled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00044408 File Offset: 0x00042608
		protected override float GetRewardValue(int level)
		{
			return base.GetRewardValue(level) * 5f;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x00044418 File Offset: 0x00042618
		protected override void AddRewards(Mission mission, float itemValue, MissionDifficulty missionDifficulty, SeededRandom random, Faction faction)
		{
			ConquestRank conquestRank = faction.GetConquestRank();
			float num = random.RandomRange(32f, 50f);
			if (mission.sourcePoi.system.storyteller is ConquestSystem)
			{
				mission.rewards.Add(new UmbralControl
				{
					amount = Mathf.RoundToInt(num)
				});
			}
			float num2 = 1f;
			num2 += conquestRank.GetMissionCommendationsRewardBonus(faction.identifier);
			InventoryItemType item = "ConquestCurrency";
			int amount = Mathf.RoundToInt(random.RandomRange(num * 2f, num * 3f) * GameMath.CostMultiplier(mission.sourcePoi.level - 30) * num2);
			mission.rewards.Add(new Item
			{
				item = item,
				amount = amount
			});
			List<string> list = new List<string>
			{
				"UmbralTransponder",
				"UmbralCargoScanner",
				"UmbralTrackingBeacon"
			};
			mission.rewards.Add(new Item
			{
				item = random.Choose<string>(list)
			});
			ReputationLevel reputationLevel = faction.GetReputationLevel(Faction.player);
			float num3 = 1f;
			num3 += reputationLevel.GetMissionRewardMultiplier();
			num3 += conquestRank.GetCreditRewardMultiplier();
			int amount2 = Mathf.RoundToInt(itemValue * random.RandomRange(0.8f, 1.25f) * num3);
			mission.rewards.Add(base.CreditReward(amount2, faction));
			int[] list2 = new int[]
			{
				400,
				500,
				600,
				700
			};
			float num4 = 1f;
			num4 += conquestRank.GetReputationBonus();
			mission.rewards.Add(new Reputation
			{
				amount = Mathf.RoundToInt((float)random.Choose<int>(list2) * num4),
				faction = Faction.puppeteers
			});
			if (GamePlayer.current.level < GameMath.maxLevel)
			{
				mission.rewards.Add(new Experience
				{
					amount = Mathf.RoundToInt((float)GameMath.GetExperienceRewardValue(150f, mission.level) * random.RandomRange(0.8f, 1.25f))
				});
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00044635 File Offset: 0x00042835
		public override Faction GetEnemyFaction(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random)
		{
			return random.Choose<Faction>(Faction.corporations);
		}
	}
}
