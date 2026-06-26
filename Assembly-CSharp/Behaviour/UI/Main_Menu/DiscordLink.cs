using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Main_Menu
{
	// Token: 0x02000270 RID: 624
	public class DiscordLink : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060016F1 RID: 5873 RVA: 0x0009164F File Offset: 0x0008F84F
		public void OpenDiscordInviteURL()
		{
			Application.OpenURL("https://discord.gg/DJ7JtSQZWK");
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x0009165C File Offset: 0x0008F85C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@MMDTitle", Array.Empty<object>()), 16, 8f).Text.color = ColorHelper.discBlue;
			tooltip.AddTextLine(Translation.Translate("@MMDJourney", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.discBlueLight2;
			tooltip.AddTextLine(Translation.Translate("@MMDText", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.discBlueLight2;
			tooltip.AddTextLine(Translation.Translate("@MMDSeeyouthere", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.discBlueLight2;
			tooltip.AddTextLine(Translation.Translate("@MMDInviteAction", Array.Empty<object>()), 12, 8f);
		}
	}
}
