using System;
using Behaviour.UI.Tooltip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000278 RID: 632
	public class AmountBar : MonoBehaviour, ITooltipCustomSource, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06001721 RID: 5921 RVA: 0x000923DB File Offset: 0x000905DB
		public void InitBar(float maxAmount, float amount, float modifier, BarType barType)
		{
			this.SetModifier(modifier);
			this.SetLabel(barType);
			this.SetHealth(amount, maxAmount);
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000923F4 File Offset: 0x000905F4
		public void SetHealth(float amount, float max)
		{
			this.fillImage.fillAmount = amount / max;
			this.currentAmountText.text = GameMath.FormatNumber(amount, -1) + " / " + GameMath.FormatNumber(max, -1);
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x00092427 File Offset: 0x00090627
		public void ShowLabel(bool show)
		{
			this.labelText.gameObject.SetActive(show);
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x0009243A File Offset: 0x0009063A
		public void SetLabel(BarType barType)
		{
			this.label = Translation.Translate(string.Format("@{0}Modifier", barType), Array.Empty<object>());
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x0009245C File Offset: 0x0009065C
		public void SetModifier(float modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00092465 File Offset: 0x00090665
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(string.Format("{0}: {1}x", this.label, this.modifier), 12, 8f);
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00092490 File Offset: 0x00090690
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.ShowLabel(true);
		}

		// Token: 0x06001728 RID: 5928 RVA: 0x00092499 File Offset: 0x00090699
		public void OnPointerExit(PointerEventData eventData)
		{
			this.ShowLabel(false);
		}

		// Token: 0x04000E38 RID: 3640
		public Image fillImage;

		// Token: 0x04000E39 RID: 3641
		public TextMeshProUGUI currentAmountText;

		// Token: 0x04000E3A RID: 3642
		[SerializeField]
		private RectTransform labelText;

		// Token: 0x04000E3B RID: 3643
		private float modifier;

		// Token: 0x04000E3C RID: 3644
		private string label;
	}
}
