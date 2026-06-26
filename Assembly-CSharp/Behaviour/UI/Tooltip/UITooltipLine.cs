using System;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x0200020C RID: 524
	public class UITooltipLine : UITooltipContent
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001373 RID: 4979 RVA: 0x0007E9B7 File Offset: 0x0007CBB7
		public override float Height
		{
			get
			{
				return this._top + this._image.rectTransform.sizeDelta.y + this._bottom;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x0007E9DC File Offset: 0x0007CBDC
		public override float Spacing
		{
			get
			{
				return this._bottom;
			}
		}

		// Token: 0x06001375 RID: 4981 RVA: 0x0007E9E4 File Offset: 0x0007CBE4
		public void SetLine(Color c, float lineHeight, float top, float bottom)
		{
			this._image.color = c;
			this._top = top;
			this._bottom = bottom;
			this._image.rectTransform.sizeDelta = new Vector2(this._image.rectTransform.sizeDelta.x, lineHeight);
			this._image.rectTransform.anchoredPosition = new Vector2(0f, this._top);
		}

		// Token: 0x04000B1B RID: 2843
		[SerializeField]
		protected Image _image;

		// Token: 0x04000B1C RID: 2844
		protected float _top;

		// Token: 0x04000B1D RID: 2845
		protected float _bottom;
	}
}
