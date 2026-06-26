using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001EF RID: 495
	public class ShipInfoIcon : MonoBehaviour, ITooltipTitleSource, ITooltipTextSource
	{
		// Token: 0x060012C1 RID: 4801 RVA: 0x0007AB73 File Offset: 0x00078D73
		public void SetContent(string text, string tooltipTitle, string tooltipDescription = "")
		{
			this.value.text = Translation.Translate(text, Array.Empty<object>());
			this.tooltipTitle = tooltipTitle;
			this.tooltipDescription = tooltipDescription;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0007AB99 File Offset: 0x00078D99
		public void SetValue(string text)
		{
			this.value.text = text;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0007ABA7 File Offset: 0x00078DA7
		public string GetTooltipTitle()
		{
			if (this.tooltipTitle == null)
			{
				return "";
			}
			return Translation.Translate(this.tooltipTitle, Array.Empty<object>());
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0007ABC7 File Offset: 0x00078DC7
		public string GetTooltipText()
		{
			if (this.tooltipDescription == null)
			{
				return "";
			}
			return Translation.Translate(this.tooltipDescription, Array.Empty<object>());
		}

		// Token: 0x04000A81 RID: 2689
		[SerializeField]
		private TextMeshProUGUI value;

		// Token: 0x04000A82 RID: 2690
		private string tooltipTitle;

		// Token: 0x04000A83 RID: 2691
		private string tooltipDescription;
	}
}
