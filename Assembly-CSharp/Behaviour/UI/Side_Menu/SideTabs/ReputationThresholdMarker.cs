using System;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A9 RID: 681
	public class ReputationThresholdMarker : MonoBehaviour, ITooltipCustomSource, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600194E RID: 6478 RVA: 0x0009D71F File Offset: 0x0009B91F
		public void SetLine(Image line)
		{
			this.linkedLine = line;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0009D728 File Offset: 0x0009B928
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@" + this.level.ToString(), Array.Empty<object>()), 12, 8f).Text.color = this.level.GetReputationColor();
			tooltip.AddTextLine(string.Format("{0}: {1}", Translation.Translate("@RepThreshold", Array.Empty<object>()), this.thresholdValue), 12, 8f).Text.color = ColorHelper.boringGrey;
			if (this.level > ReputationLevel.Neutral)
			{
				tooltip.AddSeparator(null);
			}
			float shopDiscount = this.level.GetShopDiscount();
			if (shopDiscount > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepShopDiscount", Array.Empty<object>()) + ": " + GameMath.FormatPercentage(shopDiscount, FormatPercentageMode.Default, 1).HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			int bonusMissionAmount = this.level.GetBonusMissionAmount();
			string text = "+" + bonusMissionAmount.ToString();
			if (bonusMissionAmount > 0)
			{
				tooltip.AddTextLine(Translation.Translate("@RepBonusMissions", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float missionRewardMultiplier = this.level.GetMissionRewardMultiplier();
			string text2 = "+" + GameMath.FormatPercentage(missionRewardMultiplier, FormatPercentageMode.Default, 1);
			if (missionRewardMultiplier != 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepCreditReward", Array.Empty<object>()) + ": " + text2.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float repairSpeedMultiplier = this.level.GetRepairSpeedMultiplier();
			float num = 1f / repairSpeedMultiplier;
			string text3 = "x" + GameMath.FormatNumber(num, 1);
			if (repairSpeedMultiplier < 1f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepRepairSpeed", Array.Empty<object>()) + ": " + text3.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float repairCostDiscount = this.level.GetRepairCostDiscount();
			string text4 = GameMath.FormatPercentage(repairCostDiscount, FormatPercentageMode.Default, 1);
			if (repairCostDiscount > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepRepairCostDiscount", Array.Empty<object>()) + ": " + text4.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0009D988 File Offset: 0x0009BB88
		public void Initialize(ReputationLevel level, int value)
		{
			this.level = level;
			this.thresholdValue = value;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0009D998 File Offset: 0x0009BB98
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.linkedLine != null)
			{
				this.linkedLine.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0009D9B9 File Offset: 0x0009BBB9
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.linkedLine != null)
			{
				this.linkedLine.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000FBE RID: 4030
		public ReputationLevel level;

		// Token: 0x04000FBF RID: 4031
		public int thresholdValue;

		// Token: 0x04000FC0 RID: 4032
		private Image linkedLine;
	}
}
