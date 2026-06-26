using System;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x0200022B RID: 555
	public class WorkshopStat : UITooltipContent, ITooltipCustomSource, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x000868A8 File Offset: 0x00084AA8
		public override float Height
		{
			get
			{
				return ((RectTransform)base.transform).sizeDelta.y + 2f;
			}
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x000868C8 File Offset: 0x00084AC8
		public void SetStat(EquipStatLine stat, int index, SalvageWorkshop salvageWorkshop, bool rerolledBefore)
		{
			this.stat = stat;
			this.index = index;
			this.rerollCost = salvageWorkshop.rerollStatCost;
			this.SetRerollPossibility(stat.canReroll, rerolledBefore);
			this.salvageWorkshop = salvageWorkshop;
			this.label.TL(stat.ToReadableString(false), Array.Empty<object>());
			this.label.color = ColorHelper.modifierColor;
			if (!this.canReroll)
			{
				this.background.enabled = false;
			}
			else
			{
				this.background.color = this.backgroundColor;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 24f);
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00086978 File Offset: 0x00084B78
		private void SetRerollPossibility(bool canReroll, bool rerolledBefore)
		{
			this.canReroll = canReroll;
			this.rerolledBefore = rerolledBefore;
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x00086988 File Offset: 0x00084B88
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (!this.canReroll)
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorStatLocked", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@SalWorCannotReroll", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
				return;
			}
			if (this.rerolledBefore)
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorRerollStat", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@SalWorCanReroll", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			}
			else
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorRerollStat", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@SalWorLockOtherStats", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			}
			bool flag = GamePlayer.current.CanAfford((float)this.rerollCost);
			Color color = flag ? ColorHelper.greenish : ColorHelper.red90;
			string text = "$" + GameMath.FormatNumber((float)this.rerollCost, -1);
			tooltip.AddTextLine(Translation.Translate("@SalWorCost", Array.Empty<object>()) + " " + text.HighlightWithColor(color), 12, 8f);
			if (flag)
			{
				tooltip.AddTextLine(Translation.Translate("@SalWorLCInteract", Array.Empty<object>()), 12, 8f);
			}
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00086B60 File Offset: 0x00084D60
		public void OnPointerDown(PointerEventData eventData)
		{
			if (!this.canReroll)
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				if (!this.rerolledBefore)
				{
					AlertPopup.ShowQuery(Translation.Translate("@SalWorWarningStat", Array.Empty<object>()).HighlightWithColor(ColorHelper.reddish) + "\n" + Translation.Translate("@SalWorContinueStat", Array.Empty<object>()), Translation.Translate("@UIYes", Array.Empty<object>()), Translation.Translate("@UICancel", Array.Empty<object>()), delegate
					{
						this.RerollStat();
					}, null, null, null);
					return;
				}
				this.RerollStat();
			}
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00086BF1 File Offset: 0x00084DF1
		private void RerollStat()
		{
			this.salvageWorkshop.RerollStat(this.stat, this.index);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00086C0A File Offset: 0x00084E0A
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!this.canReroll)
			{
				return;
			}
			this.background.color = this.hoverColor;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00086C26 File Offset: 0x00084E26
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this.canReroll)
			{
				return;
			}
			this.background.color = this.backgroundColor;
		}

		// Token: 0x04000C2C RID: 3116
		[SerializeField]
		private Image background;

		// Token: 0x04000C2D RID: 3117
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000C2E RID: 3118
		[SerializeField]
		private Color backgroundColor;

		// Token: 0x04000C2F RID: 3119
		[SerializeField]
		private Color hoverColor;

		// Token: 0x04000C30 RID: 3120
		[SerializeField]
		private Color cantRerollColor;

		// Token: 0x04000C31 RID: 3121
		private bool canReroll;

		// Token: 0x04000C32 RID: 3122
		private bool rerolledBefore;

		// Token: 0x04000C33 RID: 3123
		private EquipStatLine stat;

		// Token: 0x04000C34 RID: 3124
		private int index;

		// Token: 0x04000C35 RID: 3125
		private int rerollCost;

		// Token: 0x04000C36 RID: 3126
		private SalvageWorkshop salvageWorkshop;
	}
}
