using System;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs.Captain.CaptainReputation
{
	// Token: 0x020002C6 RID: 710
	public class ConquestRankTooltip : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060019F9 RID: 6649 RVA: 0x000A1BF3 File Offset: 0x0009FDF3
		private void Start()
		{
			this.parent = base.GetComponentInParent<FactionConquestRanks>();
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x000A1C04 File Offset: 0x0009FE04
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			ConquestRank conquestRank = this.parent.conquestRank;
			string identifier = this.parent.faction.identifier;
			tooltip.AddTextLine(Translation.Translate(this.parent.faction.name, Array.Empty<object>()), 14, 8f);
			tooltip.AddTextLine(conquestRank.GetConquestRankTranslation(identifier), 12, 8f).Text.color = conquestRank.GetConquestColor();
			tooltip.AddTextLine(string.Format("{0}: {1}", Translation.Translate("@RepThreshold", Array.Empty<object>()), this.parent.playerContribution), 12, 8f).Text.color = ColorHelper.boringGrey;
			if (conquestRank > ConquestRank.None)
			{
				tooltip.AddSeparator(null);
			}
			float missionCommendationsRewardBonus = conquestRank.GetMissionCommendationsRewardBonus(identifier);
			string text = "+" + GameMath.FormatPercentage(missionCommendationsRewardBonus, FormatPercentageMode.Default, 1);
			if (missionCommendationsRewardBonus > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@ConqCommendation", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float fleetStrengthRewardBonus = conquestRank.GetFleetStrengthRewardBonus(identifier);
			string text2 = "+" + GameMath.FormatPercentage(fleetStrengthRewardBonus, FormatPercentageMode.Default, 1);
			if (fleetStrengthRewardBonus > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@ConqFleetStrength", Array.Empty<object>()) + ": " + text2.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float creditRewardMultiplier = conquestRank.GetCreditRewardMultiplier();
			string text3 = "+" + GameMath.FormatPercentage(creditRewardMultiplier, FormatPercentageMode.Default, 1);
			if (creditRewardMultiplier != 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@RepCreditReward", Array.Empty<object>()) + ": " + text3.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			float reputationBonus = conquestRank.GetReputationBonus();
			string text4 = "+" + GameMath.FormatPercentage(reputationBonus, FormatPercentageMode.Default, 1);
			if (reputationBonus > 0f)
			{
				tooltip.AddTextLine(Translation.Translate("@ConqReputation", Array.Empty<object>()) + ": " + text4.HighlightWithColor(ColorHelper.lightCyan), 12, 8f);
			}
			if (conquestRank.HasDestroyerUnlocked(identifier))
			{
				tooltip.AddTextLine(Translation.Translate("@ConqLargeShip", Array.Empty<object>()) ?? "", 12, 8f);
			}
			foreach (DecalDefinition decalDefinition in conquestRank.GetAllUnlockedDecals(identifier))
			{
				tooltip.AddTextLine("Decal: " + decalDefinition.displayName, 12, 8f);
			}
		}

		// Token: 0x04001057 RID: 4183
		private FactionConquestRanks parent;
	}
}
