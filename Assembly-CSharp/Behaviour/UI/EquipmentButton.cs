using System;
using Behaviour.Item;
using Behaviour.UI.ShipCarousel;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001EC RID: 492
	public abstract class EquipmentButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IHighlightable
	{
		// Token: 0x060012AB RID: 4779
		protected abstract void SetHoveringButton(bool toggle);

		// Token: 0x060012AC RID: 4780
		protected abstract void AssignSlot<T>(T slot);

		// Token: 0x060012AD RID: 4781
		public abstract string GetSlotText<T>(T slot);

		// Token: 0x060012AE RID: 4782 RVA: 0x0007A060 File Offset: 0x00078260
		private void Awake()
		{
			foreach (TooltipSource tooltipSource in base.GetComponents<TooltipSource>())
			{
				if (!(tooltipSource is ItemTooltipSource))
				{
					this.emptyTooltip = tooltipSource;
					return;
				}
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0007A098 File Offset: 0x00078298
		public void SetItem<T>(InventoryItemType item, T slot)
		{
			this.backgroundImage = base.GetComponent<Image>();
			this.normalMaterial = this.backgroundImage.material;
			this.installedEquipment = item;
			this.AssignSlot<T>(slot);
			if (item != null)
			{
				this.buttonIcon.sprite = item.icon;
				this.buttonIcon.preserveAspect = true;
				this.buttonIcon.gameObject.SetActive(true);
			}
			else
			{
				this.buttonIcon.gameObject.SetActive(false);
			}
			if (this.emptyTooltip)
			{
				this.emptyTooltip.BodyText = this.GetSlotText<T>(slot);
				this.emptyTooltip.enabled = (item == null);
			}
			this.borderIcon.color = ColorHelper.boringGrey;
			this.SetRarityBackgroundColor();
			base.GetComponent<ItemTooltipSource>().SetItem(item, 1, false, ItemTooltipContext.InCarousel, false, null);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0007A172 File Offset: 0x00078372
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			if (InventoryInteractionManager.Instance)
			{
				this.SetHoveringButton(true);
			}
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0007A187 File Offset: 0x00078387
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			if (InventoryInteractionManager.Instance)
			{
				this.SetHoveringButton(false);
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0007A19C File Offset: 0x0007839C
		protected void SetRarityBackgroundColor()
		{
			Color color = Rarity.Standard.GetColor();
			if (this.installedEquipment != null)
			{
				color = this.installedEquipment.rarity.GetColor();
			}
			this.backgroundImage.color = new Color(color.r, color.g, color.b, 0.7f);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0007A1F6 File Offset: 0x000783F6
		public void ShowHighlight()
		{
			if (!this.backgroundImage)
			{
				return;
			}
			this.backgroundImage.material = this.highlightMaterial;
			this.backgroundImage.color = this.highlightMaterial.GetColor("_Color");
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x0007A232 File Offset: 0x00078432
		public void HideHighlight()
		{
			if (!this.backgroundImage)
			{
				return;
			}
			this.backgroundImage.material = this.normalMaterial;
			this.SetRarityBackgroundColor();
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x0007A25C File Offset: 0x0007845C
		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.inPersonalHangar)
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right && this.installedEquipment && PersonalHangar.current)
			{
				InventoryInteractionManager.Instance.UnequipEquipment(this);
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left && this.installedEquipment && SalvageWorkshop.current && this.installedEquipment.CanGoInWorkshop())
			{
				SalvageWorkshop.current.SelectItem(this.installedEquipment, true, null);
			}
		}

		// Token: 0x04000A74 RID: 2676
		public bool inPersonalHangar;

		// Token: 0x04000A75 RID: 2677
		protected InventoryItemType installedEquipment;

		// Token: 0x04000A76 RID: 2678
		protected Image backgroundImage;

		// Token: 0x04000A77 RID: 2679
		protected Material normalMaterial;

		// Token: 0x04000A78 RID: 2680
		[SerializeField]
		protected Image buttonIcon;

		// Token: 0x04000A79 RID: 2681
		[SerializeField]
		protected Image borderIcon;

		// Token: 0x04000A7A RID: 2682
		[SerializeField]
		protected Material highlightMaterial;

		// Token: 0x04000A7B RID: 2683
		private TooltipSource emptyTooltip;
	}
}
