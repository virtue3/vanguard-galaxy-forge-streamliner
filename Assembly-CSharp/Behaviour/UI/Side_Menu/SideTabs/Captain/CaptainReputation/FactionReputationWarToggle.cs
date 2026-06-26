using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs.Captain.CaptainReputation
{
	// Token: 0x020002C5 RID: 709
	public class FactionReputationWarToggle : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x060019F6 RID: 6646 RVA: 0x000A1B2B File Offset: 0x0009FD2B
		private void Start()
		{
			this.parent = base.GetComponentInParent<FactionReputation>();
			this.toggle = base.GetComponent<Toggle>();
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x000A1B48 File Offset: 0x0009FD48
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.toggle.isOn ? "@AtWarToggleOff" : "@AtWarToggleOn", 12, 8f).Text.color = ColorHelper.detailsColor;
			tooltip.AddTextLine("", 12, 8f);
			if (this.toggle.isOn)
			{
				tooltip.AddTextLine(((float)this.parent.reputationLevel < -500f) ? "@AtWarToggleOffDesc2" : "@AtWarToggleOffDesc", 12, 8f);
				return;
			}
			tooltip.AddTextLine("@AtWarToggleOnDesc", 12, 8f);
		}

		// Token: 0x04001055 RID: 4181
		private FactionReputation parent;

		// Token: 0x04001056 RID: 4182
		private Toggle toggle;
	}
}
