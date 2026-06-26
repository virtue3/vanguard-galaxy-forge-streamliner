using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000226 RID: 550
	public class ExtractBlueprintButton : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060014B5 RID: 5301 RVA: 0x00085601 File Offset: 0x00083801
		public void Init(SalvageWorkshop sw)
		{
			this.workshop = sw;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x0008560C File Offset: 0x0008380C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.workshop.selectedItem == null)
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorNoItemSelected", Array.Empty<object>()), 12, 8f);
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@SalWorExtractBlueprint", Array.Empty<object>()), 12, 8f);
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@SalWorExtractBlueprintDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			string text = "$" + GameMath.FormatNumber((float)this.workshop.extractBlueprintCost, -1);
			tooltip.AddTextLine(Translation.Translate("@SalWorCost", Array.Empty<object>()) + " " + text.HighlightWithColor(ColorHelper.creditsColor), 12, 8f);
		}

		// Token: 0x04000C0C RID: 3084
		private SalvageWorkshop workshop;
	}
}
