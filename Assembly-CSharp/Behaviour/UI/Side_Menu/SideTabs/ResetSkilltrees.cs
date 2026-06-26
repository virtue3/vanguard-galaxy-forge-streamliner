using System;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002AD RID: 685
	public class ResetSkilltrees : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001974 RID: 6516 RVA: 0x0009E77C File Offset: 0x0009C97C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@Respec", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(Translation.Translate("@RespecInfo", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			if (SpaceStationInterior.instance == null)
			{
				tooltip.AddTextLine(Translation.Translate("@RespectCantInteract", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				return;
			}
			if (GamePlayer.current.commander.GetInvestedSkillPoints() > 0)
			{
				tooltip.AddTextLine(Translation.Translate("@RespecInteract", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
			}
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0009E858 File Offset: 0x0009CA58
		public void ResetSkillTrees()
		{
			if (SpaceStationInterior.instance == null)
			{
				return;
			}
			if (GamePlayer.current.commander.GetInvestedSkillPoints() == 0)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@RespecNoPoints", Array.Empty<object>())).WithColor(ColorHelper.greenish).Show();
				return;
			}
			AlertPopup.ShowQuery("@RespecPopup", "@UIYes", "@UINo", delegate
			{
				this.ReallyReset();
			}, null, null, null);
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0009E8D0 File Offset: 0x0009CAD0
		private void ReallyReset()
		{
			if (SpaceStationInterior.instance == null)
			{
				return;
			}
			GamePlayer.current.commander.RefundAllSkills(true);
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@RespecSkills", Array.Empty<object>())).WithColor(ColorHelper.greenish).Show();
			SidePanel.instance.RefreshIfOpen();
		}
	}
}
