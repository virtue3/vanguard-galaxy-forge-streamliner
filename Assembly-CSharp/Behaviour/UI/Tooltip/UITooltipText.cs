using System;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x0200020F RID: 527
	public class UITooltipText : UITooltipContent
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x0600137F RID: 4991 RVA: 0x0007EAC0 File Offset: 0x0007CCC0
		public override float Height
		{
			get
			{
				float? num = this.overrideHeight;
				if (num == null)
				{
					return this._text.preferredHeight + this._spacing;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x0007EAF7 File Offset: 0x0007CCF7
		public override float Spacing
		{
			get
			{
				return this._spacing;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001381 RID: 4993 RVA: 0x0007EAFF File Offset: 0x0007CCFF
		public TMP_Text Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0007EB07 File Offset: 0x0007CD07
		public void SetText(string text, int size, float margin)
		{
			this._text.TL(text, Array.Empty<object>());
			this._text.fontSize = (float)size;
			this._spacing = margin;
		}

		// Token: 0x04000B23 RID: 2851
		[SerializeField]
		protected TMP_Text _text;

		// Token: 0x04000B24 RID: 2852
		public float? overrideHeight;

		// Token: 0x04000B25 RID: 2853
		protected float _spacing;
	}
}
