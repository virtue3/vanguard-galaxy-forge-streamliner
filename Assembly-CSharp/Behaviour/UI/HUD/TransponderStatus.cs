using System;
using Behaviour.UI.Tooltip;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000288 RID: 648
	public class TransponderStatus : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x00095378 File Offset: 0x00093578
		private void OnEnable()
		{
			this.SetTransponder();
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00095380 File Offset: 0x00093580
		public void SetTransponder()
		{
			if (HudManager.Instance && HudManager.Instance.showHud)
			{
				base.gameObject.SetActive(GamePlayer.current.hasUmbralTransponder);
				return;
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000953BC File Offset: 0x000935BC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine("Decoy Transponder " + "Active".HighlightWithColor(ColorHelper.greenish), 12, 8f).Text.color = ColorHelper.umbralColor;
			tooltip.AddSeparator(null);
			tooltip.AddTextLine("IFF signal cloaked.", 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine("Prevents reputation loss (and gains) due to combat.", 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddSeparator(null);
			tooltip.AddTextLine("Lasts until you next dock at a Space Station.", 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine("Instant docking disabled.", 12, 8f).Text.color = ColorHelper.reddish;
		}
	}
}
