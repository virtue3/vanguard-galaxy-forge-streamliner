using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000227 RID: 551
	public class UpgradeLevelButton : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060014B8 RID: 5304 RVA: 0x000856F9 File Offset: 0x000838F9
		public void Init(SalvageWorkshop sw)
		{
			this.workshop = sw;
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x00085704 File Offset: 0x00083904
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.workshop.selectedItem == null)
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorNoItemSelected", Array.Empty<object>()), 12, 8f);
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@SalWorUpgradeLevel", Array.Empty<object>()), 12, 8f);
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@SalWorUpgradeLevelDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			string text = "$" + GameMath.FormatNumber((float)this.workshop.upgradeLevelCost, -1);
			tooltip.AddTextLine(Translation.Translate("@SalWorCost", Array.Empty<object>()) + " " + text.HighlightWithColor(ColorHelper.creditsColor), 12, 8f);
		}

		// Token: 0x04000C0D RID: 3085
		private SalvageWorkshop workshop;
	}
}
