using System;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002AB RID: 683
	public class FactionConquestRankThresholdMarker : MonoBehaviour, ITooltipCustomSource, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x0600195C RID: 6492 RVA: 0x0009DD28 File Offset: 0x0009BF28
		public void SetLine(Image line)
		{
			this.linkedLine = line;
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0009DD34 File Offset: 0x0009BF34
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.rank.GetConquestRankTranslation(this.factionIdentifier), 12, 8f).Text.color = this.rank.GetConquestColor();
			tooltip.AddTextLine(string.Format("{0}: {1}", Translation.Translate("@RepThreshold", Array.Empty<object>()), this.playerContribution), 12, 8f).Text.color = ColorHelper.boringGrey;
			if (this.rank > ConquestRank.None)
			{
				tooltip.AddSeparator(null);
			}
			float missionCommendationsRewardBonus = this.rank.GetMissionCommendationsRewardBonus(this.factionIdentifier);
			string text = "+" + GameMath.FormatPercentage(missionCommendationsRewardBonus, FormatPercentageMode.Default, 1);
			if (missionCommendationsRewardBonus > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@ConqCommendation", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float fleetStrengthRewardBonus = this.rank.GetFleetStrengthRewardBonus(this.factionIdentifier);
			string text2 = "+" + GameMath.FormatPercentage(fleetStrengthRewardBonus, FormatPercentageMode.Default, 1);
			if (fleetStrengthRewardBonus > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@ConqFleetStrength", Array.Empty<object>()) + ": " + text2.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float creditRewardMultiplier = this.rank.GetCreditRewardMultiplier();
			string text3 = "+" + GameMath.FormatPercentage(creditRewardMultiplier, FormatPercentageMode.Default, 1);
			if (creditRewardMultiplier != 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepCreditReward", Array.Empty<object>()) + ": " + text3.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float reputationBonus = this.rank.GetReputationBonus();
			string text4 = "+" + GameMath.FormatPercentage(reputationBonus, FormatPercentageMode.Default, 1);
			if (reputationBonus > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@ConqReputation", Array.Empty<object>()) + ": " + text4.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			if (this.rank.UnlocksDestroyer(this.factionIdentifier))
			{
				tooltip.AddTextLine(Translation.Translate("@ConqLargeShip", Array.Empty<object>()) ?? "", 12, 8f);
			}
			foreach (DecalDefinition decalDefinition in this.rank.GetDecalUnlocks(this.factionIdentifier))
			{
				tooltip.AddTextLine("Decal: " + decalDefinition.displayName, 12, 8f);
			}
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0009DFEC File Offset: 0x0009C1EC
		public void Initialize(ConquestRank rank, int value, string factionIdentifier)
		{
			this.rank = rank;
			this.playerContribution = value;
			this.factionIdentifier = factionIdentifier;
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0009E003 File Offset: 0x0009C203
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.linkedLine != null)
			{
				this.linkedLine.gameObject.SetActive(true);
			}
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0009E024 File Offset: 0x0009C224
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.linkedLine != null)
			{
				this.linkedLine.gameObject.SetActive(false);
			}
		}

		// Token: 0x04000FCD RID: 4045
		public string factionIdentifier;

		// Token: 0x04000FCE RID: 4046
		public ConquestRank rank;

		// Token: 0x04000FCF RID: 4047
		public int playerContribution;

		// Token: 0x04000FD0 RID: 4048
		private Image linkedLine;
	}
}
