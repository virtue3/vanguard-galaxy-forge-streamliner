using System;
using Behaviour.Crew;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002AE RID: 686
	public class SkilltreePoints : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001979 RID: 6521 RVA: 0x0009E940 File Offset: 0x0009CB40
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			CommanderData commander = GamePlayer.current.commander;
			int remainingSkillPoints = GamePlayer.current.commander.GetRemainingSkillPoints();
			Color color = (remainingSkillPoints > 0) ? ColorHelper.greenish : ColorHelper.reddish;
			tooltip.AddTextLine(Translation.Translate("@SkilltreePointsAvailable", Array.Empty<object>()) + ": " + remainingSkillPoints.ToString().HighlightWithColor(color), 14, 8f);
			int num = 0;
			foreach (SkillTreeData skillTreeData in commander.skillTrees)
			{
				num += skillTreeData.GetInvestedSkillPoints();
			}
			tooltip.AddTextLine(Translation.Translate("@SkilltreeTotalPoints", Array.Empty<object>()) + ": " + num.ToString().HighlightWithColor(ColorHelper.orange75), 12, 8f).Text.color = ColorHelper.boringGrey;
			int num2 = 0;
			foreach (CrewMemberData crewMemberData in GamePlayer.current.currentSpaceShip.crewMembers)
			{
				if (crewMemberData != null)
				{
					foreach (SkilltreeNode skilltreeNode in crewMemberData.unlockedNodes)
					{
						num2++;
					}
				}
			}
			tooltip.AddTextLine(Translation.Translate("@SkillTreeCrewBonusSkills", Array.Empty<object>()) + ": " + num2.ToString(), 12, 8f);
		}
	}
}
