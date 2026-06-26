using System;
using Behaviour.UI;
using Source.Util;

namespace Source.Galaxy.POI
{
	// Token: 0x02000151 RID: 337
	public class ConquestStation : SpaceStation
	{
		// Token: 0x06000CF4 RID: 3316 RVA: 0x0005D43C File Offset: 0x0005B63C
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			if (base.umbralControlLevel > 0f)
			{
				tooltip.AddTextLine(Translation.Highlight("@UmbralControlLevel", ColorHelper.umbralColor, new object[]
				{
					GameMath.FormatPercentage(base.umbralControlLevel, FormatPercentageMode.Default, 1)
				}), 12, 8f);
				tooltip.AddSeparator(null);
			}
			base.AddTooltipInfo(tooltip);
		}
	}
}
