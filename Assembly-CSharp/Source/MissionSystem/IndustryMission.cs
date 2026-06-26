using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.UI.Missions;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.World.POI;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A2 RID: 162
	public class IndustryMission : Mission
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x00037298 File Offset: 0x00035498
		public override void ClaimRewards(bool force = false)
		{
			base.ClaimRewards(force);
			GamePlayer.current.maxIndustryLevel = Math.Max(GamePlayer.current.maxIndustryLevel, base.level);
			if (this.industryLevel == 2 && this.wave % 2 == 0)
			{
				GamePlayer.current.industryRank++;
			}
			IndustryMission industryMission = IndustryMission.Create(this.sourcePoi as SpaceStation, base.dynamicPointOfInterest as IndustryStation, this.industryLevel, this.wave + 1);
			GamePlayer.current.currentIndustry = industryMission;
			Singleton<FocusedMissionHandler>.Instance.SetMission(industryMission);
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00037330 File Offset: 0x00035530
		public override void OnMissionAbandoned()
		{
			base.OnMissionAbandoned();
			base.steps[0].GetMissionPoi().ClearUnits();
			base.steps[0].GetMissionPoi().ClearPayloads();
			if (this.industryLevel == 2 && (this.wave == 0 || this.wave > 4))
			{
				GamePlayer.current.industryRank = Math.Max(0, GamePlayer.current.industryRank - 1);
			}
			foreach (InventoryItemType item in new List<InventoryItemType>
			{
				"IndustrialAmmoPack",
				"IndustrialRepairPack",
				"IndustrialSupplyPack",
				"IndustrialTurretPack",
				"IndustrialComponent1",
				"IndustrialComponent2",
				"IndustrialComponent3"
			})
			{
				GamePlayer.current.currentSpaceShip.cargo.Remove(item, 999999);
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00037470 File Offset: 0x00035670
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			result["industryLevel"] = new double?((double)this.industryLevel);
			result["wave"] = new double?((double)this.wave);
			return result;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000374BF File Offset: 0x000356BF
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			this.industryLevel = data["industryLevel"];
			this.wave = data["wave"];
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000374F4 File Offset: 0x000356F4
		public static IndustryMission FromJson(JsonObject data)
		{
			IndustryMission industryMission = new IndustryMission();
			industryMission.DataFromJson(data);
			return industryMission;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00037504 File Offset: 0x00035704
		internal static IndustryMission Create(SpaceStation source, IndustryStation poi, int missionIndex, int wave)
		{
			string text;
			if (missionIndex != 1)
			{
				if (missionIndex != 2)
				{
					text = "@BountyDiffNormal";
				}
				else
				{
					text = "@BountyDiffExtreme";
				}
			}
			else
			{
				text = "@BountyDiffElevated";
			}
			string text2 = text;
			IndustryMission industryMission = new IndustryMission
			{
				name = Translation.Translate("@IndustryMissionName", new object[]
				{
					poi.system.name
				}),
				description = Translation.TranslateOnly("@IndustryMissionDesc", new object[]
				{
					poi.system.name
				}),
				category = Translation.Translate(text2, new object[]
				{
					poi.level - source.level
				}),
				completionText = Translation.Translate("@IndustryMissionCompleted", Array.Empty<object>()),
				industryLevel = missionIndex,
				difficulty = MissionDifficulty.Hard,
				sourcePoi = source,
				sourceFaction = source.faction,
				autoComplete = true,
				iconName = "Combat"
			};
			industryMission.wave = wave;
			(poi.storyteller as IndustrialOutpost).RefreshPersistables();
			MissionStep missionStep = new MissionStep();
			missionStep.system = poi.system;
			missionStep.dynamicPointOfInterest = poi;
			missionStep.objectives.Add(new TriggerObjective
			{
				requiredAmount = 4 + missionIndex + wave,
				trigger = MissionTrigger.IndustryBoardCraft,
				description = Translation.Translate("@IndustryMissionObjective", Array.Empty<object>())
			});
			industryMission.steps.Add(missionStep);
			SeededRandom seededRandom = new SeedGenerator().Add("IndustryMissionRewards").Add(missionIndex).Add(source.guid).Add(DateTime.Now.DayOfYear).CreateRandom();
			float num;
			if (missionIndex != 0)
			{
				if (missionIndex != 2)
				{
					num = 1f;
				}
				else
				{
					num = 1.3f;
				}
			}
			else
			{
				num = 0.7f;
			}
			float num2 = num;
			num2 += 0.1f * (float)wave;
			industryMission.rewards.Add(new Source.MissionSystem.Rewards.Item
			{
				item = "IndustryCurrency",
				amount = Mathf.RoundToInt(seededRandom.RandomRange(7f, 8f) * GameMath.CostMultiplier(poi.level) * num2)
			});
			industryMission.rewards.Add(new Credits
			{
				amount = GameMath.GetCreditsValue(25f, poi.level)
			});
			industryMission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
			{
				faction = Faction.industrialGuild,
				amount = 325
			});
			return industryMission;
		}

		// Token: 0x0400039A RID: 922
		public int industryLevel;

		// Token: 0x0400039B RID: 923
		public int wave;
	}
}
