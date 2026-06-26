using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Main_Menu
{
	// Token: 0x0200026F RID: 623
	public class BatRoostGames : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060016EE RID: 5870 RVA: 0x000915D7 File Offset: 0x0008F7D7
		public void OpenURL()
		{
			Application.OpenURL("https://www.batroostgames.com");
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x000915E4 File Offset: 0x0008F7E4
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@MMBTitle", Array.Empty<object>()), 16, 8f).Text.color = ColorHelper.brgBeige;
			tooltip.AddTextLine(Translation.Translate("@MMBText", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.brgBeigeLight;
		}
	}
}
