using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Generator
{
	// Token: 0x020000DE RID: 222
	public class TradeMaterials : MissionGenerator
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x000435FD File Offset: 0x000417FD
		public override bool canBeIdled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00043600 File Offset: 0x00041800
		public override Mission GenerateMission(MapPointOfInterest poi, MissionDifficulty difficulty, SeededRandom random, TargetLayer? targetLayer = null)
		{
			List<RefinedMaterial> list = new List<RefinedMaterial>();
			if (difficulty <= MissionDifficulty.Normal)
			{
				list.Add(RefinedMaterial.Titanium);
				list.Add(RefinedMaterial.Oxide);
				list.Add(RefinedMaterial.Silicon);
				list.Add(RefinedMaterial.Tungsten);
				list.Add(RefinedMaterial.Carbon);
			}
			else
			{
				list.Add(RefinedMaterial.Iridium);
				list.Add(RefinedMaterial.Platinum);
				list.Add(RefinedMaterial.Astatine);
			}
			RefinedMaterial self = random.ChooseAndRemove<RefinedMaterial>(list);
			InventoryItemType itemType = InventoryItemType.Get("Canister" + self.ToString());
			float num = random.RandomRange(4.5f, 5f);
			float num2 = this.GetRewardValue(poi.level) * 0.5f * difficulty.GetRewardMultiplier();
			int requiredAmount = Mathf.CeilToInt(num2 * num / self.GetValue());
			RefinedMaterial self2 = random.Choose<RefinedMaterial>(list);
			InventoryItemType item = InventoryItemType.Get("Canister" + self2.ToString());
			Mission mission = new Mission
			{
				name = Translation.Translate("@TradeMaterialsMissionName", new object[]
				{
					self.GetDisplayName()
				}),
				category = Translation.Translate("@TradeMaterialsMission", Array.Empty<object>()),
				description = Translation.Translate("@TradeMaterialsMissionDesc", new object[]
				{
					self.GetDisplayName(),
					self2.GetDisplayName()
				}),
				completionText = Translation.Translate("@TradeMaterialsMissionComplete", Array.Empty<object>()),
				sourcePoi = poi,
				canBeIdled = this.canBeIdled
			};
			MissionStep missionStep = new MissionStep();
			missionStep.objectives.Add(new TradeOffer
			{
				itemType = itemType,
				requiredAmount = requiredAmount,
				deliverTo = (poi as SpaceStation),
				gameplayType = GameplayType.Mining,
				targetLayer = targetLayer
			});
			mission.steps.Add(missionStep);
			mission.rewards.Add(new Source.MissionSystem.Rewards.Item
			{
				item = item,
				amount = Mathf.CeilToInt(num2 * num / self2.GetValue())
			});
			this.AddRewards(mission, num2, MissionDifficulty.Normal, random, poi.faction);
			return mission;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00043805 File Offset: 0x00041A05
		public override GameplayType GetMissionType()
		{
			return GameplayType.Cargo;
		}
	}
}
