using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.UI.Tooltip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001FA RID: 506
	public class UITooltip : MonoBehaviour
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0007D066 File Offset: 0x0007B266
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x0007D06D File Offset: 0x0007B26D
		public static RectTransform tooltipParent { get; private set; }

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0007D075 File Offset: 0x0007B275
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x0007D07D File Offset: 0x0007B27D
		public TooltipSource Source { get; protected set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060012FF RID: 4863 RVA: 0x0007D086 File Offset: 0x0007B286
		public RectTransform Content
		{
			get
			{
				return this._contentParent;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001300 RID: 4864 RVA: 0x0007D08E File Offset: 0x0007B28E
		public virtual UITooltip Prefab
		{
			get
			{
				return UITooltipParent.TooltipPrefab;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001301 RID: 4865 RVA: 0x0007D095 File Offset: 0x0007B295
		public virtual float SizeX
		{
			get
			{
				return ((RectTransform)base.transform).sizeDelta.x;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x0007D0AC File Offset: 0x0007B2AC
		public virtual float SizeY
		{
			get
			{
				return ((RectTransform)base.transform).sizeDelta.y;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0007D0C3 File Offset: 0x0007B2C3
		private void Awake()
		{
			this.background = base.GetComponent<Image>();
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x0007D0D1 File Offset: 0x0007B2D1
		public static void SetupTooltipContext(RectTransform parent)
		{
			UITooltip.tooltipParent = parent;
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0007D0DC File Offset: 0x0007B2DC
		public UITooltipText AddTextLine(string text, int size = 12, float margin = 8f)
		{
			UITooltipText uitooltipText = UnityEngine.Object.Instantiate<UITooltipText>(this._textPrefab, this._contentParent);
			uitooltipText.SetText(text, size, margin);
			this.AddContent(uitooltipText);
			return uitooltipText;
		}

		// Token: 0x06001306 RID: 4870 RVA: 0x0007D10C File Offset: 0x0007B30C
		public ValueTuple<UITooltipText, UITooltipText> AddHeader(string name, int level, int maxLevel = 0, int size = 12, float margin = 8f)
		{
			UITooltipText uitooltipText = UnityEngine.Object.Instantiate<UITooltipText>(this._textPrefab, this._contentParent);
			uitooltipText.Text.alignment = TextAlignmentOptions.TopRight;
			uitooltipText.overrideHeight = new float?(0f);
			string text = level.ToString();
			if (maxLevel > 1000)
			{
				text += "+";
			}
			else if (maxLevel != 0 && maxLevel > level)
			{
				text = text + "-" + maxLevel.ToString();
			}
			uitooltipText.SetText(Translation.Translate("@TooltipItemLevel", new object[]
			{
				text
			}), 12, 0f);
			this.AddContent(uitooltipText);
			UITooltipText uitooltipText2 = this.AddTextLine(name, size, 8f);
			Vector2 sizeDelta = uitooltipText2.Text.rectTransform.sizeDelta;
			uitooltipText2.Text.rectTransform.sizeDelta = new Vector2(sizeDelta.x - uitooltipText.Text.preferredWidth - 8f, sizeDelta.y);
			return new ValueTuple<UITooltipText, UITooltipText>(uitooltipText2, uitooltipText);
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x0007D204 File Offset: 0x0007B404
		public UITooltipLine AddSeparator(Color c, float thickness, float spacingTop, float spacingBottom)
		{
			if (this._contentList[this._contentList.Count - 1] is UITooltipLine)
			{
				return null;
			}
			UITooltipLine uitooltipLine = UnityEngine.Object.Instantiate<UITooltipLine>(this._linePrefab, this._contentParent);
			uitooltipLine.SetLine(c, thickness, spacingTop, spacingBottom);
			this.AddContent(uitooltipLine);
			return uitooltipLine;
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x0007D258 File Offset: 0x0007B458
		public UITooltipLine AddSeparator(Color? color = null)
		{
			Color c = color ?? ColorHelper.boringGrey;
			return this.AddSeparator(c, 2f, 0f, 8f);
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0007D298 File Offset: 0x0007B498
		public UITooltipSpacer AddMinHeightSpacer(float minHeight)
		{
			UITooltipSpacer uitooltipSpacer = UnityEngine.Object.Instantiate<UITooltipSpacer>(this._spacerPrefab, this._contentParent);
			uitooltipSpacer.height = minHeight;
			this.AddContent(uitooltipSpacer);
			return uitooltipSpacer;
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x0007D2C6 File Offset: 0x0007B4C6
		public void AddContent(UITooltipContent content)
		{
			this._contentList.Add(content);
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x0007D2D4 File Offset: 0x0007B4D4
		public virtual void SetContent(TooltipSource tt)
		{
			if (tt == null)
			{
				return;
			}
			this.Source = tt;
			string title = tt.GetTitle();
			if (!string.IsNullOrEmpty(title))
			{
				this.AddTextLine(title, 16, 10f);
			}
			string bodyText = tt.GetBodyText();
			if (!string.IsNullOrEmpty(bodyText))
			{
				this.AddTextLine(bodyText, 12, 8f);
			}
			tt.AddCustomContent(this);
			RectTransform rectTransform = base.transform as RectTransform;
			float num = 0f;
			float num2 = 0f;
			foreach (UITooltipContent uitooltipContent in this._contentList)
			{
				RectTransform rectTransform2 = uitooltipContent.transform as RectTransform;
				rectTransform2.anchoredPosition = new Vector2(rectTransform2.anchoredPosition.x, -num);
				if (uitooltipContent is UITooltipSpacer)
				{
					num = Mathf.Max(uitooltipContent.Height, num);
				}
				else
				{
					num += uitooltipContent.Height;
				}
				num2 = uitooltipContent.Spacing;
			}
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num - num2 + 20f);
			this.Update();
		}

		// Token: 0x0600130C RID: 4876 RVA: 0x0007D400 File Offset: 0x0007B600
		public virtual void RefreshContent()
		{
			this._contentParent.DestroyChildren();
			this._contentList.Clear();
			this.SetContent(this.Source);
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x0007D424 File Offset: 0x0007B624
		public void SetWidth(float width)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
		}

		// Token: 0x0600130E RID: 4878 RVA: 0x0007D454 File Offset: 0x0007B654
		protected virtual void Update()
		{
			Vector2 vector = GlobalControls.mousePosition / this.background.canvas.scaleFactor;
			RectTransform rectTransform = base.transform as RectTransform;
			Vector2 vector2 = new Vector2(vector.x + 10f, vector.y);
			Vector2 vector3 = new Vector2(0f, 0f);
			if (vector2.x + this.SizeX > (float)Screen.width / this.background.canvas.scaleFactor)
			{
				vector2 = new Vector2(vector.x - 10f, vector.y);
				vector3 = new Vector2(1f, 0f);
			}
			if (vector2.y + this.SizeY > (float)Screen.height / this.background.canvas.scaleFactor)
			{
				vector3 = new Vector2(vector3.x, 1f);
			}
			rectTransform.anchoredPosition = vector2;
			rectTransform.pivot = vector3;
		}

		// Token: 0x0600130F RID: 4879 RVA: 0x0007D546 File Offset: 0x0007B746
		public static void Show(TooltipSource tt, UITooltip prefab)
		{
			if (UITooltip._current)
			{
				UnityEngine.Object.Destroy(UITooltip._current.gameObject);
			}
			if (UITooltip.TooltipEnabled)
			{
				UITooltip._current = UnityEngine.Object.Instantiate<UITooltip>(prefab, UITooltip.tooltipParent);
				UITooltip._current.SetContent(tt);
			}
		}

		// Token: 0x06001310 RID: 4880 RVA: 0x0007D588 File Offset: 0x0007B788
		public static void Refresh()
		{
			if (UITooltip._current && UITooltip._current.enabled && UITooltip._current.Source)
			{
				UITooltip._current.RefreshContent();
				return;
			}
			if (UITooltip._current)
			{
				UnityEngine.Object.Destroy(UITooltip._current.gameObject);
			}
		}

		// Token: 0x06001311 RID: 4881 RVA: 0x0007D5E4 File Offset: 0x0007B7E4
		public static void Refresh(TooltipSource tt)
		{
			if (UITooltip._current && tt && tt.gameObject && UITooltip._current.Source == tt)
			{
				UITooltip.Refresh();
			}
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x0007D61E File Offset: 0x0007B81E
		public static void Hide(TooltipSource tt)
		{
			if (UITooltip._current && UITooltip._current.Source == tt)
			{
				UnityEngine.Object.Destroy(UITooltip._current.gameObject);
			}
		}

		// Token: 0x04000ABB RID: 2747
		public static bool TooltipEnabled = true;

		// Token: 0x04000ABC RID: 2748
		private static UITooltip _current;

		// Token: 0x04000ABE RID: 2750
		[SerializeField]
		private RectTransform _contentParent;

		// Token: 0x04000ABF RID: 2751
		[SerializeField]
		private UITooltipText _textPrefab;

		// Token: 0x04000AC0 RID: 2752
		[SerializeField]
		private UITooltipLine _linePrefab;

		// Token: 0x04000AC1 RID: 2753
		[SerializeField]
		private UITooltipSpacer _spacerPrefab;

		// Token: 0x04000AC2 RID: 2754
		private Image background;

		// Token: 0x04000AC3 RID: 2755
		private List<UITooltipContent> _contentList = new List<UITooltipContent>();
	}
}
