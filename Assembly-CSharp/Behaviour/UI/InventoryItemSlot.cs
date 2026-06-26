using System;
using Behaviour.Equipment;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001DD RID: 477
	public class InventoryItemSlot : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001202 RID: 4610 RVA: 0x000775EA File Offset: 0x000757EA
		// (set) Token: 0x06001203 RID: 4611 RVA: 0x000775F2 File Offset: 0x000757F2
		public Inventory inventory { get; private set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06001204 RID: 4612 RVA: 0x000775FB File Offset: 0x000757FB
		// (set) Token: 0x06001205 RID: 4613 RVA: 0x00077603 File Offset: 0x00075803
		public int slot { get; private set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x0007760C File Offset: 0x0007580C
		// (set) Token: 0x06001207 RID: 4615 RVA: 0x00077614 File Offset: 0x00075814
		public Inventory.InventoryItem contained { get; private set; }

		// Token: 0x06001208 RID: 4616 RVA: 0x0007761D File Offset: 0x0007581D
		private void Awake()
		{
			this.tooltip = base.GetComponent<ItemTooltipSource>();
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x0007762C File Offset: 0x0007582C
		private void Update()
		{
			this.UpdateItemChargeIndicator();
			if (this.mouseOver && Input.GetKeyDown(KeyCode.F) && this.contained != null && this.contained.item.CanFavourite())
			{
				this.contained.item.favouriteItem = !this.contained.item.favouriteItem;
				this.favourite.gameObject.SetActive(this.contained.item.favouriteItem);
			}
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x000776AD File Offset: 0x000758AD
		private void UpdateItemChargeIndicator()
		{
			if (!this.itemChargeIndicator.isActiveAndEnabled && !this.itemChargeIndicator.depletableItem)
			{
				return;
			}
			this.itemChargeIndicator.UpdateFillAmount();
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x000776DA File Offset: 0x000758DA
		public void SetInventory(Inventory inv, int slot)
		{
			this.inventory = inv;
			this.slot = slot;
			this.UpdateItem();
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x000776F0 File Offset: 0x000758F0
		public void UpdateItem()
		{
			ItemTooltipContext itemTooltipContext;
			if (this.inventory is ShopInventory)
			{
				itemTooltipContext = ItemTooltipContext.InShop;
			}
			else
			{
				itemTooltipContext = ItemTooltipContext.InInventory;
			}
			this.favourite.gameObject.SetActive(false);
			this.contained = this.inventory.Get(this.slot);
			ItemTooltipSource itemTooltipSource = this.tooltip;
			if (itemTooltipSource != null)
			{
				Inventory.InventoryItem contained = this.contained;
				InventoryItemType item = (contained != null) ? contained.item : null;
				Inventory.InventoryItem contained2 = this.contained;
				int num = (contained2 != null) ? contained2.count : 0;
				bool allowCompare = true;
				ItemTooltipContext context = itemTooltipContext;
				Inventory.InventoryItem contained3 = this.contained;
				itemTooltipSource.SetItem(item, num, allowCompare, context, contained3 != null && contained3.canBuyback, this.contained);
			}
			if (this.contained != null)
			{
				this.itemIcon.sprite = this.contained.item.icon;
				this.itemIcon.enabled = true;
				if (this.contained.item.itemCategory.CanBeEquiped() && !this.contained.item.CanBeEquippedOn(GamePlayer.current.currentSpaceShip))
				{
					this.itemIcon.color = new Color(0.8f, 0.25f, 0.25f);
				}
				else
				{
					this.itemIcon.color = Color.white;
				}
				Color color = this.contained.item.rarity.GetColor();
				if (this.contained.item.rarity == Rarity.Standard)
				{
					color.a = 0.1f;
				}
				this.rarity.color = color;
				this.rarity.enabled = true;
				this.favourite.gameObject.SetActive(this.contained.item.favouriteItem);
				if (this.inventory is ShopInventory && this.contained.item.HasInfiniteShopSupply())
				{
					this.SetCount(0);
					this.shopBackground.gameObject.SetActive(true);
				}
				else
				{
					this.SetCount(this.contained.count);
				}
				string text = null;
				Color color2 = Color.clear;
				switch (this.contained.item.itemCategory)
				{
				case ItemCategory.Turret:
					text = this.contained.item.GetComponent<AbstractEquipment>().size.GetShortDisplayName();
					color2 = this.GetHardpointColor(this.contained.item);
					break;
				case ItemCategory.Module:
					text = this.contained.item.GetComponent<AbstractEquipment>().size.GetShortDisplayName();
					color2 = new Color(0.25f, 0.25f, 0.3f, 0.5f);
					break;
				case ItemCategory.Booster:
					text = "B";
					color2 = new Color(0.4f, 0.5f, 0.4f, 0.5f);
					break;
				}
				if (text != null)
				{
					this.typeBackground.color = color2;
					this.typeLabel.TL(text, Array.Empty<object>());
					this.typeBackground.gameObject.SetActive(true);
				}
				else
				{
					this.typeBackground.gameObject.SetActive(false);
				}
			}
			else
			{
				this.typeBackground.gameObject.SetActive(false);
				this.itemIcon.enabled = false;
				this.rarity.enabled = false;
				this.SetCount(0);
			}
			this.SetupChargeIndicator();
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00077A0C File Offset: 0x00075C0C
		private void SetCount(int cnt)
		{
			if (cnt <= 1)
			{
				this.countBg.gameObject.SetActive(false);
				this.count.text = "";
				return;
			}
			this.count.text = GameMath.FormatNumber((float)cnt, -1);
			this.countBg.sizeDelta = new Vector2(this.count.preferredWidth + 4f, this.countBg.sizeDelta.y);
			this.countBg.gameObject.SetActive(true);
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00077A94 File Offset: 0x00075C94
		private Color GetHardpointColor(InventoryItemType item)
		{
			Color result;
			switch (item.gameplayType)
			{
			case GameplayType.Combat:
				result = new Color(1f, 0.25f, 0.25f, 0.5f);
				break;
			case GameplayType.Mining:
				result = new Color(0.5f, 0.5f, 1f, 0.5f);
				break;
			case GameplayType.Salvage:
				result = new Color(1f, 0.85f, 0f, 0.5f);
				break;
			default:
				result = new Color(0.5f, 0.25f, 0.25f, 0.5f);
				break;
			}
			return result;
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00077B30 File Offset: 0x00075D30
		private void SetupChargeIndicator()
		{
			if (this.contained != null)
			{
				InventoryItemType item = this.contained.item;
				WarpFuelItem item2;
				if (item != null && item.TryGetComponent<WarpFuelItem>(out item2))
				{
					this.itemChargeIndicator.gameObject.SetActive(true);
					this.itemChargeIndicator.SetItem(item2);
					this.itemChargeIndicator.UpdateFillAmount();
					return;
				}
			}
			this.itemChargeIndicator.gameObject.SetActive(false);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00077B9A File Offset: 0x00075D9A
		public void MarkAsSelected()
		{
			this.itemIcon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00077BC0 File Offset: 0x00075DC0
		public void ClearDragging()
		{
			this.UpdateItem();
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00077BC8 File Offset: 0x00075DC8
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && this.contained != null)
			{
				InventoryInteractionManager.Instance.SetSelectedItem(this, false);
			}
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00077BE8 File Offset: 0x00075DE8
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.contained == null)
			{
				return;
			}
			if (this.contained.count > 1 && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && eventData.button == PointerEventData.InputButton.Left)
			{
				InventoryInteractionManager.Instance.SetSelectedItem(this, true);
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				InventoryInteractionManager.Instance.SetSelectedItem(this, false);
				return;
			}
			if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && eventData.button == PointerEventData.InputButton.Right)
			{
				InventoryInteractionManager.Instance.QuickPopup(this);
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				InventoryInteractionManager.Instance.QuickAction(this);
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00077C8F File Offset: 0x00075E8F
		public void OnDrag(PointerEventData eventData)
		{
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00077C91 File Offset: 0x00075E91
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.border.color = ColorHelper.white75;
			if (this.CanWeShowHighlight())
			{
				InventoryInteractionManager.Instance.HighlightDropTargets(this);
			}
			this.mouseOver = true;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00077CBD File Offset: 0x00075EBD
		public void OnPointerExit(PointerEventData eventData)
		{
			this.border.color = ColorHelper.middleBlue;
			if (this.CanWeShowHighlight())
			{
				InventoryInteractionManager.Instance.ClearHighlightables();
			}
			this.mouseOver = false;
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00077CE8 File Offset: 0x00075EE8
		private bool CanWeShowHighlight()
		{
			return this.contained != null && InventoryInteractionManager.Instance && !InventoryInteractionManager.Instance.selectedItem;
		}

		// Token: 0x040009E9 RID: 2537
		[SerializeField]
		private Image itemIcon;

		// Token: 0x040009EA RID: 2538
		[SerializeField]
		private Image rarity;

		// Token: 0x040009EB RID: 2539
		[SerializeField]
		private Image favourite;

		// Token: 0x040009EC RID: 2540
		[SerializeField]
		private Image typeBackground;

		// Token: 0x040009ED RID: 2541
		[SerializeField]
		private Image shopBackground;

		// Token: 0x040009EE RID: 2542
		[SerializeField]
		private Image border;

		// Token: 0x040009EF RID: 2543
		[SerializeField]
		private TMP_Text typeLabel;

		// Token: 0x040009F0 RID: 2544
		[SerializeField]
		private RectTransform countBg;

		// Token: 0x040009F1 RID: 2545
		[SerializeField]
		private TMP_Text count;

		// Token: 0x040009F2 RID: 2546
		private ItemTooltipSource tooltip;

		// Token: 0x040009F3 RID: 2547
		[SerializeField]
		private ItemChargeIndicator itemChargeIndicator;

		// Token: 0x040009F4 RID: 2548
		[SerializeField]
		private ItemSlotActive itemSlotActive;

		// Token: 0x040009F7 RID: 2551
		public bool mouseOver;
	}
}
