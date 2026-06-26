using System;
using Behaviour.Equipment.Aspect;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Item;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000229 RID: 553
	public class WorkshopAspect : UITooltipContent, ITooltipCustomSource, IPointerDownHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060014BE RID: 5310 RVA: 0x000858E9 File Offset: 0x00083AE9
		public override float Height
		{
			get
			{
				return ((RectTransform)base.transform).sizeDelta.y + 6f;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x00085906 File Offset: 0x00083B06
		// (set) Token: 0x060014C0 RID: 5312 RVA: 0x0008590E File Offset: 0x00083B0E
		public int index { get; private set; }

		// Token: 0x060014C1 RID: 5313 RVA: 0x00085918 File Offset: 0x00083B18
		public void SetAspect(AspectSlot aspectSlot, SalvageWorkshop salvageWorkshop, int index)
		{
			this.salvageWorkshop = salvageWorkshop;
			this.aspect = aspectSlot.equipAspect;
			this.aspectSlot = aspectSlot;
			this.index = index;
			this.canStrip = true;
			this.background.color = this.backgroundColor;
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
				this.backgroundColor = this.emptyColor;
			}
			this.extractCost = salvageWorkshop.selectedItem.sellValue;
			if (aspectSlot.equipAspect != null && !aspectSlot.equipAspect.common)
			{
				this.extractCost *= 2;
			}
			this.installCost = salvageWorkshop.installAspectCost;
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, this.description.preferredHeight + 44f);
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00085AC8 File Offset: 0x00083CC8
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			AspectItem aspectItem;
			if (this.IsItemBeingDragged() && InventoryInteractionManager.Instance.selectedItem.contained.item.TryGetComponent<AspectItem>(out aspectItem))
			{
				if (this.CanWeFitAspect(aspectItem.equipAspect))
				{
					tooltip.AddTextLine(Translation.Translate("@SalWorInsAspect", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
					tooltip.AddSeparator(null);
					if (this.canStrip)
					{
						tooltip.AddTextLine(Translation.Translate("@SalWorInsAspectDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
					}
					else
					{
						tooltip.AddTextLine(Translation.Translate("@SalWorInsAspectEmptyDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
					}
					Color color = GamePlayer.current.CanAfford((float)this.installCost) ? ColorHelper.greenish : ColorHelper.red90;
					string text = "$" + GameMath.FormatNumber((float)this.installCost, -1);
					tooltip.AddTextLine(Translation.Translate("@SalWorCost", Array.Empty<object>()) + " " + text.HighlightWithColor(color), 12, 8f);
					tooltip.AddTextLine(Translation.Translate("@SalWorLCInteractInsAspect", Array.Empty<object>()), 12, 8f);
					return;
				}
				tooltip.AddTextLine(Translation.Translate("@SalWorInsAspectNoFit", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@SalWorInsAspectNoFitDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
				return;
			}
			else
			{
				if (!this.canStrip)
				{
					tooltip.AddTextLine(Translation.Translate("@SalWorCannotSalAspect", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
					tooltip.AddSeparator(null);
					tooltip.AddTextLine(Translation.Translate("@SalWorLockedSalAspect", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
					return;
				}
				if (this.aspect != null)
				{
					tooltip.AddTextLine(Translation.Translate("@SalWorCanSalAspect", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
					tooltip.AddSeparator(null);
					tooltip.AddTextLine(Translation.Translate("@SalWorSalAspect", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
					tooltip.AddTextLine(Translation.Translate("@SalWorLCInteractSalAspect", Array.Empty<object>()), 12, 8f);
					Color color2 = GamePlayer.current.CanAfford((float)this.extractCost) ? ColorHelper.greenish : ColorHelper.red90;
					string text2 = "$" + GameMath.FormatNumber((float)this.extractCost, -1);
					tooltip.AddTextLine(Translation.Translate("@SalWorCost", Array.Empty<object>()) + " " + text2.HighlightWithColor(color2), 12, 8f);
					return;
				}
				tooltip.AddTextLine(Translation.Translate("@SalWorAspectEmpty", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@SalWorAspectEmptyDesc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
				return;
			}
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00085E6E File Offset: 0x0008406E
		public bool CanWeFitAspect(EquipAspect aspect)
		{
			return this.salvageWorkshop.selectedItem.equipmentBuilder.CanFitAspect(aspect);
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00085E88 File Offset: 0x00084088
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.IsItemBeingDragged())
			{
				InventoryItemType item = InventoryInteractionManager.Instance.selectedItem.contained.item;
				AspectItem component = item.GetComponent<AspectItem>();
				if (component != null && this.CanWeFitAspect(component.equipAspect))
				{
					this.InstallAspect(component.equipAspect, item);
					UITooltip.Refresh();
					return;
				}
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorInsAspectNoFitDesc", Array.Empty<object>())).WithColor(ColorHelper.reddish).Show();
				UITooltip.Refresh();
				return;
			}
			else
			{
				if (!this.canStrip)
				{
					return;
				}
				if (!this.aspect)
				{
					return;
				}
				if (eventData.button == PointerEventData.InputButton.Left)
				{
					AlertPopup.ShowQuery(Translation.Translate("@SalWorWarningSalAspect", Array.Empty<object>()).HighlightWithColor(ColorHelper.reddish) + "\n" + Translation.Translate("@SalWorContinueSalAspect", Array.Empty<object>()), Translation.Translate("@UIYes", Array.Empty<object>()), Translation.Translate("@UICancel", Array.Empty<object>()), delegate
					{
						this.SalvageAspect();
					}, null, null, null);
					return;
				}
				return;
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00085F95 File Offset: 0x00084195
		private void SalvageAspect()
		{
			this.salvageWorkshop.SalvageAspect(this.aspect, this.extractCost);
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00085FAE File Offset: 0x000841AE
		private void InstallAspect(EquipAspect installAspect, InventoryItemType aspectItemToDelete)
		{
			this.salvageWorkshop.InstallAspect(installAspect, this.index, aspectItemToDelete);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00085FC4 File Offset: 0x000841C4
		private bool IsItemBeingDragged()
		{
			InventoryInteractionManager instance = InventoryInteractionManager.Instance;
			return !(instance.draggableItem == null) && !(instance.selectedItem == null) && !(instance.selectedItem.contained.item == null);
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x00086012 File Offset: 0x00084212
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.background.color = this.hoverColor;
			if (InventoryInteractionManager.Instance)
			{
				this.SetHoveringButton(true);
			}
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00086038 File Offset: 0x00084238
		public void OnPointerExit(PointerEventData eventData)
		{
			this.background.color = this.backgroundColor;
			if (InventoryInteractionManager.Instance)
			{
				this.SetHoveringButton(false);
			}
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0008605E File Offset: 0x0008425E
		private void SetHoveringButton(bool toggle)
		{
			InventoryInteractionManager.Instance.hoveringAspectButton = (toggle ? this : null);
		}

		// Token: 0x04000C0F RID: 3087
		private bool canStrip;

		// Token: 0x04000C10 RID: 3088
		[SerializeField]
		private Image background;

		// Token: 0x04000C11 RID: 3089
		[SerializeField]
		private Image icon;

		// Token: 0x04000C12 RID: 3090
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000C13 RID: 3091
		[SerializeField]
		private TMP_Text description;

		// Token: 0x04000C14 RID: 3092
		private bool emptySlot;

		// Token: 0x04000C15 RID: 3093
		private EquipAspect aspect;

		// Token: 0x04000C16 RID: 3094
		private AspectSlot aspectSlot;

		// Token: 0x04000C17 RID: 3095
		private SalvageWorkshop salvageWorkshop;

		// Token: 0x04000C19 RID: 3097
		private int extractCost;

		// Token: 0x04000C1A RID: 3098
		private int installCost;

		// Token: 0x04000C1B RID: 3099
		[SerializeField]
		private Color backgroundColor;

		// Token: 0x04000C1C RID: 3100
		[SerializeField]
		private Color hoverColor;

		// Token: 0x04000C1D RID: 3101
		[SerializeField]
		private Color emptyColor;
	}
}
