using System;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs.Captain.CaptainReputation
{
	// Token: 0x020002C4 RID: 708
	public class FactionReputationTooltip : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060019F3 RID: 6643 RVA: 0x000A1838 File Offset: 0x0009FA38
		private void Start()
		{
			this.parent = base.GetComponentInParent<FactionReputation>();
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x000A1848 File Offset: 0x0009FA48
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			string text = "@Bonus";
			Color color = ColorHelper.greenish;
			if (this.parent.reputationLevel < -500)
			{
				text = "@Penalty";
				color = ColorHelper.reddish;
			}
			tooltip.AddTextLine(Translation.Translate(this.parent.faction.name, Array.Empty<object>()), 14, 8f);
			tooltip.AddTextLine(Translation.Translate("@" + this.parent.reputation.ToString(), Array.Empty<object>()), 12, 8f).Text.color = this.parent.reputation.GetReputationColor();
			if (this.parent.reputation > ReputationLevel.Neutral)
			{
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@Faction", Array.Empty<object>()) + " " + Translation.Translate(text, Array.Empty<object>()).HighlightWithColor(color), 12, 8f);
			}
			float shopDiscount = this.parent.reputation.GetShopDiscount();
			if (shopDiscount > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepShopDiscount", Array.Empty<object>()) + ": " + GameMath.FormatPercentage(shopDiscount, FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			int bonusMissionAmount = this.parent.reputation.GetBonusMissionAmount();
			string text2 = "+" + bonusMissionAmount.ToString();
			if (bonusMissionAmount > 0)
			{
				tooltip.AddTextLine(Translation.Translate("@RepBonusMissions", Array.Empty<object>()) + ": " + text2.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float missionRewardMultiplier = this.parent.reputation.GetMissionRewardMultiplier();
			string text3 = "+" + GameMath.FormatPercentage(missionRewardMultiplier, FormatPercentageMode.Default, 1);
			if (missionRewardMultiplier != 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepCreditReward", Array.Empty<object>()) + ": " + text3.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float repairSpeedMultiplier = this.parent.reputation.GetRepairSpeedMultiplier();
			float num = 1f / repairSpeedMultiplier;
			string text4 = "x" + GameMath.FormatNumber(num, 1);
			if (repairSpeedMultiplier < 1f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepRepairSpeed", Array.Empty<object>()) + ": " + text4.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float repairCostDiscount = this.parent.reputation.GetRepairCostDiscount();
			string text5 = GameMath.FormatPercentage(repairCostDiscount, FormatPercentageMode.Default, 1);
			if (repairCostDiscount > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepRepairCostDiscount", Array.Empty<object>()) + ": " + text5.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
		}

		// Token: 0x04001054 RID: 4180
		private FactionReputation parent;
	}
}
