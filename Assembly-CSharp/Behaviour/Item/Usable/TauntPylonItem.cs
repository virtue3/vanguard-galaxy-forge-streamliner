using System;
using Behaviour.UI;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200031B RID: 795
	public class TauntPylonItem : DefensiveTurretItem
	{
		// Token: 0x06001DD7 RID: 7639 RVA: 0x000B1B80 File Offset: 0x000AFD80
		public override void AddToTooltip(CompareTooltip tooltip)
		{
			Color c;
			if (this.currentHealth < 0.25f)
			{
				c = ColorHelper.reddish;
			}
			else if (this.currentHealth < 0.75f)
			{
				c = ColorHelper.orange75;
			}
			else
			{
				c = ColorHelper.greenish;
			}
			tooltip.AddTextLine(Translation.Highlight("@DefensiveTurretHealth", c, new object[]
			{
				GameMath.FormatPercentage(this.currentHealth, FormatPercentageMode.Default, 1)
			}), 12, 8f);
		}
	}
}
