using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Main_Menu
{
	// Token: 0x02000274 RID: 628
	public class SteamLink : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001703 RID: 5891 RVA: 0x000918E4 File Offset: 0x0008FAE4
		public void OpenSteamLinkURL()
		{
			Application.OpenURL("https://store.steampowered.com/app/3471800/Vanguard_Galaxy/#game_area_purchase");
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000918F0 File Offset: 0x0008FAF0
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@MMSTitle", Array.Empty<object>()), 16, 8f).Text.color = ColorHelper.steamBlue;
			tooltip.AddTextLine(Translation.Translate("@MMSJourney", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.steamBlueLight2;
			tooltip.AddTextLine(Translation.Translate("@MMSText", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.steamBlueLight2;
			tooltip.AddTextLine(Translation.Translate("@MMDInviteAction", Array.Empty<object>()), 12, 8f);
		}
	}
}
