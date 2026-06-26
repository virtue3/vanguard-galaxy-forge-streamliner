using System;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs.Captain
{
	// Token: 0x020002C3 RID: 707
	public class FactionTitleButton : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060019F0 RID: 6640 RVA: 0x000A17F9 File Offset: 0x0009F9F9
		public void Setup(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x000A1802 File Offset: 0x0009FA02
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.faction == null)
			{
				return;
			}
			tooltip.AddTextLine(Translation.Translate(this.faction.name, Array.Empty<object>()), 12, 8f);
		}

		// Token: 0x04001053 RID: 4179
		private Faction faction;
	}
}
