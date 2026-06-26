using System;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x02000203 RID: 515
	public class CompareTooltipAspect : UITooltipContent
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0007E57A File Offset: 0x0007C77A
		public override float Height
		{
			get
			{
				return ((RectTransform)base.transform).sizeDelta.y + 6f;
			}
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0007E598 File Offset: 0x0007C798
		public void SetAspect(AspectSlot aspectSlot)
		{
			if (aspectSlot.equipAspect != null)
			{
				this.icon.sprite = aspectSlot.equipAspect.icon;
				this.label.TL(aspectSlot.equipAspect.displayName, Array.Empty<object>());
				this.description.TL(aspectSlot.equipAspect.description, Array.Empty<object>());
				this.label.color = (aspectSlot.equipAspect.common ? ColorHelper.greenBadge : ColorHelper.purpleBadge);
			}
			else
			{
				this.label.text = Translation.Translate("@AspectLabel", Array.Empty<object>());
				this.description.text = Translation.Translate("@EmptyAspectDesc", Array.Empty<object>());
				this.description.color = ColorHelper.boringGrey;
				this.label.color = ColorHelper.boringGrey;
				this.background.color = this.emptyColor;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, this.description.preferredHeight + 44f);
		}

		// Token: 0x04000AFA RID: 2810
		[SerializeField]
		private Image icon;

		// Token: 0x04000AFB RID: 2811
		[SerializeField]
		private Image background;

		// Token: 0x04000AFC RID: 2812
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000AFD RID: 2813
		[SerializeField]
		private TMP_Text description;

		// Token: 0x04000AFE RID: 2814
		[SerializeField]
		private Color emptyColor;
	}
}
