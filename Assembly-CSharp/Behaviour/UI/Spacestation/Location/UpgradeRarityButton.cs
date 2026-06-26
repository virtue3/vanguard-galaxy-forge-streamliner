using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000228 RID: 552
	public class UpgradeRarityButton : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060014BB RID: 5307 RVA: 0x000857F1 File Offset: 0x000839F1
		public void Init(SalvageWorkshop sw)
		{
			this.workshop = sw;
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x000857FC File Offset: 0x000839FC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.workshop.selectedItem == null)
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorNoItemSelected", Array.Empty<object>()), 12, 8f);
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@SalWorUpgradeRarity", Array.Empty<object>()), 12, 8f);
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@SalWorUpgradeRarityDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			string text = "$" + GameMath.FormatNumber((float)this.workshop.upgradeRarityCost, -1);
			tooltip.AddTextLine(Translation.Translate("@SalWorCost", Array.Empty<object>()) + " " + text.HighlightWithColor(ColorHelper.creditsColor), 12, 8f);
		}

		// Token: 0x04000C0E RID: 3086
		private SalvageWorkshop workshop;
	}
}
