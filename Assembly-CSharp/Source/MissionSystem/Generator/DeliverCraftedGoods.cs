using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem.Objectives;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000D6 RID: 214
	public class DeliverCraftedGoods : MissionGenerator
	{
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00040F54 File Offset: 0x0003F154
		public override bool canBeIdled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00040F57 File Offset: 0x0003F157
		public override float GetMissionChance(SpaceStation source, MissionDifficulty difficulty, SeededRandom random)
		{
			return (float)((source.forge == null) ? 0 : 1);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00040F66 File Offset: 0x0003F166
		public override GameplayType GetMissionType()
		{
			return GameplayType.Mining;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00040F6C File Offset: 0x0003F16C
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty diff, SeededRandom random, TargetLayer? targetLayer = null)
		{
			SpaceStation deliverTo = poi as SpaceStation;
			float rewardValue = this.GetRewardValue(poi.level);
			float num = rewardValue * diff.GetObjectiveAmountMultiplier() / 2f;
			List<CraftingRecipe> list = new List<CraftingRecipe>(CraftingRecipe.GetAvailable());
			for (int i = 0; i < list.Count; i++)
			{
				InventoryItemType resultForTooltip = list[i].GetResultForTooltip();
				if (resultForTooltip == null || resultForTooltip.itemCategory != ItemCategory.RefinedProduct || resultForTooltip.cost <= 0 || (float)resultForTooltip.cost > num)
				{
					list.RemoveAt(i);
					i--;
				}
			}
			InventoryItemType resultForTooltip2 = random.Choose<CraftingRecipe>(list).GetResultForTooltip();
			if (resultForTooltip2 == null)
			{
				return null;
			}
			int requiredAmount = Mathf.Max(1, Mathf.RoundToInt(num / (float)resultForTooltip2.cost));
			Mission mission = new Mission
			{
				name = Translation.Translate("@DeliverCraftedGoodsMissionName", new object[]
				{
					resultForTooltip2.displayName
				}),
				category = Translation.Translate("@DeliverCraftedGoodsMission", Array.Empty<object>()),
				description = Translation.Translate("@DeliverCraftedGoodsMissionDesc", new object[]
				{
					resultForTooltip2.displayName
				}),
				completionText = Translation.Translate("@DeliverCraftedGoodsMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				canBeIdled = this.canBeIdled
			};
			MissionStep missionStep = new MissionStep();
			missionStep.objectives.Add(new TradeOffer
			{
				itemType = resultForTooltip2,
				requiredAmount = requiredAmount,
				deliverTo = deliverTo
			});
			mission.steps.Add(missionStep);
			this.AddRewards(mission, rewardValue, diff, random, poi.faction);
			return mission;
		}
	}
}
