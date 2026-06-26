using System;
using System.Collections;
using System.Collections.Generic;
using Behavior.Equipment.Booster;
using Behaviour.Equipment;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.ShipCarousel;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Spacestation.Shops;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Behaviour.UI
{
	// Token: 0x020001DC RID: 476
	public class InventoryInteractionManager : MonoBehaviour
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x00075922 File Offset: 0x00073B22
		// (set) Token: 0x060011C7 RID: 4551 RVA: 0x0007592A File Offset: 0x00073B2A
		public InventoryItemSlot selectedItem { get; private set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00075933 File Offset: 0x00073B33
		// (set) Token: 0x060011C9 RID: 4553 RVA: 0x0007593B File Offset: 0x00073B3B
		public InventoryPanel inventoryPanel { get; private set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x00075944 File Offset: 0x00073B44
		// (set) Token: 0x060011CB RID: 4555 RVA: 0x0007594C File Offset: 0x00073B4C
		public SpaceShipHardpoint hoveringHardpoint { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00075955 File Offset: 0x00073B55
		// (set) Token: 0x060011CD RID: 4557 RVA: 0x0007595D File Offset: 0x00073B5D
		public ModuleButton hoveringModuleButton { get; set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00075966 File Offset: 0x00073B66
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x0007596E File Offset: 0x00073B6E
		public BoosterButton hoveringBoosterButton { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00075977 File Offset: 0x00073B77
		// (set) Token: 0x060011D1 RID: 4561 RVA: 0x0007597F File Offset: 0x00073B7F
		public WorkshopAspect hoveringAspectButton { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00075988 File Offset: 0x00073B88
		public bool isShopOpen
		{
			get
			{
				return this.inventoryShop;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00075995 File Offset: 0x00073B95
		public bool isPersonalHangarOpen
		{
			get
			{
				return this.personalHangar;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x000759A2 File Offset: 0x00073BA2
		public bool isSalvageWorkshopOpen
		{
			get
			{
				return this.salvageWorkshop;
			}
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x000759AF File Offset: 0x00073BAF
		private void Awake()
		{
			InventoryInteractionManager.Instance = this;
			this.draggableItem = UnityEngine.Object.Instantiate<InventoryDraggable>(this.draggableItem, base.transform);
			this.draggableItem.gameObject.SetActive(false);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x000759E0 File Offset: 0x00073BE0
		private void Update()
		{
			if (this.selectedItem)
			{
				this.draggableItem.rectTransform.anchoredPosition = GlobalControls.mousePosition / this.draggableItem.itemIcon.canvas.scaleFactor;
				if (GlobalControls.mouseReleased && this.canReleaseItem)
				{
					this.DropItem();
					return;
				}
				if (GlobalControls.mouseReleased)
				{
					this.canReleaseItem = true;
					return;
				}
			}
			else if (this.draggableItem)
			{
				this.draggableItem.gameObject.SetActive(false);
			}
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00075A6C File Offset: 0x00073C6C
		public void RegisterShop(InventoryShop inventoryShop)
		{
			this.inventoryShop = inventoryShop;
			this.personalHangar = null;
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00075A7C File Offset: 0x00073C7C
		public void ClearShop()
		{
			Debug.Log("Clearing shop");
			this.inventoryShop = null;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00075A8F File Offset: 0x00073C8F
		public void RegisterInventory(InventoryPanel inventoryPanel)
		{
			this.inventoryPanel = inventoryPanel;
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00075A98 File Offset: 0x00073C98
		public void RegisterPersonalHanger(PersonalHangar personalHangar)
		{
			this.personalHangar = personalHangar;
			this.inventoryShop = null;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00075AA8 File Offset: 0x00073CA8
		public void RegisterSalvageWorkshop(SalvageWorkshop salvageWorkshop)
		{
			this.salvageWorkshop = salvageWorkshop;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00075AB1 File Offset: 0x00073CB1
		public InventoryItemSlot CreateInventorySlot(RectTransform parent, Inventory i, int slot)
		{
			InventoryItemSlot inventoryItemSlot = UnityEngine.Object.Instantiate<InventoryItemSlot>(this.inventorySlotPrefab, parent);
			inventoryItemSlot.SetInventory(i, slot);
			return inventoryItemSlot;
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00075AC7 File Offset: 0x00073CC7
		public IEnumerable<InventoryItemType> GetItemTypesForCompare(InventoryItemType itemType)
		{
			if (this.personalHangar && this.personalHangar.selectedShipData != null)
			{
				return this.personalHangar.selectedShipData.GetEquipedItemsOfType(itemType);
			}
			return GamePlayer.current.currentSpaceShip.GetEquipedItemsOfType(itemType);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00075B05 File Offset: 0x00073D05
		private int GetSelectedItemCount()
		{
			if (!this.stack)
			{
				return this.selectedItem.contained.count;
			}
			return this.selectedItem.contained.count / 2;
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00075B34 File Offset: 0x00073D34
		public void SetSelectedItem(InventoryItemSlot slot, bool stack = false)
		{
			if (this.selectedItem)
			{
				return;
			}
			slot.MarkAsSelected();
			this.stack = stack;
			this.selectedItem = slot;
			this.draggableItem.SetItem(slot.contained.item.icon, this.GetSelectedItemCount());
			this.draggableItem.gameObject.SetActive(true);
			this.canReleaseItem = !GlobalControls.mouseReleased;
			if (this.personalHangar)
			{
				this.HighlightDropTargets(this.selectedItem);
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00075BBC File Offset: 0x00073DBC
		public bool ClearSelectedItem()
		{
			if (this.selectedItem)
			{
				this.ClearHighlightables();
				if (this.selectedItem.transform.parent == base.transform)
				{
					UnityEngine.Object.Destroy(this.selectedItem.gameObject);
				}
				else
				{
					this.selectedItem.ClearDragging();
				}
				this.draggableItem.gameObject.SetActive(false);
				this.selectedItem = null;
				this.stack = false;
				return true;
			}
			return false;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00075C38 File Offset: 0x00073E38
		public void DropItem()
		{
			if (this.personalHangar && this.hoveringHardpoint)
			{
				this.EquipHardpoint(this.hoveringHardpoint, this.selectedItem);
			}
			else if (this.personalHangar && this.hoveringModuleButton)
			{
				this.EquipModule(this.hoveringModuleButton, this.selectedItem);
			}
			else if (this.personalHangar && this.hoveringBoosterButton)
			{
				this.EquipBooster(this.hoveringBoosterButton.spaceShipBooster, this.hoveringBoosterButton.index, this.selectedItem);
			}
			else if (this.isSalvageWorkshopOpen && this.hoveringAspectButton)
			{
				AspectItem aspectItem;
				if (this.selectedItem.contained.item.TryGetComponent<AspectItem>(out aspectItem) && this.hoveringAspectButton.CanWeFitAspect(aspectItem.equipAspect))
				{
					this.salvageWorkshop.InstallAspect(aspectItem.equipAspect, this.hoveringAspectButton.index, this.selectedItem.contained.item);
				}
			}
			else
			{
				PointerEventData eventData = new PointerEventData(EventSystem.current)
				{
					position = GlobalControls.mousePosition
				};
				List<RaycastResult> list = new List<RaycastResult>();
				EventSystem.current.RaycastAll(eventData, list);
				if (list.Count == 0)
				{
					if (this.selectedItem.contained.item.IsUsable())
					{
						this.UseItem(this.selectedItem);
					}
					else if (!SpaceStationInterior.instance)
					{
						this.JettisonItem(this.selectedItem, this.GetSelectedItemCount());
					}
				}
				else
				{
					foreach (RaycastResult raycastResult in list)
					{
						InventoryItemSlot component = raycastResult.gameObject.GetComponent<InventoryItemSlot>();
						if (component && component != this.selectedItem)
						{
							this.DropItemOnSlot(this.selectedItem, component);
							string str = "SelectedItem: ";
							InventoryItemSlot selectedItem = this.selectedItem;
							Debug.Log(str + ((selectedItem != null) ? selectedItem.ToString() : null));
							break;
						}
						TabMenuButton component2 = raycastResult.gameObject.GetComponent<TabMenuButton>();
						if (component2 && !component2.Deactivated())
						{
							InventoryContent inventoryContent = component2.sideTabContent as InventoryContent;
							if (inventoryContent != null && !(this.selectedItem.inventory is ShopInventory))
							{
								Inventory inventory;
								switch (inventoryContent.inventoryType)
								{
								case InventoryType.Stash:
									inventory = Inventory.global;
									break;
								case InventoryType.Cargo:
									inventory = Inventory.cargo;
									break;
								case InventoryType.Materials:
									inventory = Inventory.materials;
									break;
								default:
									throw new NotImplementedException("Unsupported inventory type!");
								}
								Inventory inventory2 = inventory;
								if (inventory2 != null && inventory2 != this.selectedItem.inventory)
								{
									bool flag = false;
									ItemCategory itemCategory = this.selectedItem.contained.item.itemCategory;
									switch (inventoryContent.inventoryType)
									{
									case InventoryType.Stash:
										flag = this.selectedItem.contained.item.CanGoInArmory();
										break;
									case InventoryType.Cargo:
										flag = true;
										break;
									case InventoryType.Materials:
										flag = this.selectedItem.contained.item.CanGoInMaterials();
										break;
									}
									if (flag)
									{
										int selectedItemCount = this.GetSelectedItemCount();
										float m = this.selectedItem.contained.item.m3;
										if (inventory2.GetSpaceAvailable() >= (float)selectedItemCount * m)
										{
											inventory2.Add(this.selectedItem.contained.item, selectedItemCount, false, false);
											this.selectedItem.inventory.Remove(this.selectedItem.contained, selectedItemCount);
										}
										else
										{
											int num = Mathf.FloorToInt(inventory2.GetSpaceAvailable() / m);
											if (num > 0)
											{
												inventory2.Add(this.selectedItem.contained.item, num, false, false);
												this.selectedItem.inventory.Remove(this.selectedItem.contained.item, num);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			this.ReloadUI();
			this.ClearSelectedItem();
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0007607C File Offset: 0x0007427C
		public void QuickAction(InventoryItemSlot slot)
		{
			this.selectedItem = slot;
			if (this.inventoryShop && slot.inventory is ShopInventory)
			{
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Inventory, 0);
				this.BuyAmount(slot, 1, null, GamePlayer.current.currentSpaceShip.cargo);
			}
			else if (this.inventoryShop && !(slot.inventory is ShopInventory))
			{
				this.SellAmount(slot, 1, null);
			}
			else if ((slot.contained.inventory == Inventory.cargo || slot.contained.inventory == Inventory.global) && slot.contained.item.IsUsable())
			{
				this.UseItem(slot);
			}
			else if (this.isSalvageWorkshopOpen)
			{
				if (slot.contained.item.CanGoInWorkshop())
				{
					this.SetItemInWorkshop(slot);
				}
				else if (slot.contained.item.CanRefinedProductGoInWorkshop())
				{
					int itemTradeInValue = this.salvageWorkshop.GetItemTradeInValue(slot.contained.item);
					this.CreatePopup(slot, "Trade in", delegate
					{
						this.AddShopCreditToWorkshop(slot.contained, this.actionPopup.GetAmount());
						this.DestroyPopup();
					}, false, new int?(itemTradeInValue));
				}
			}
			else if (SpaceStationInterior.instance && SpaceStationInterior.instance.spacestation.HasFacility(SpaceStationFacility.PersonalHangar) && slot.contained.item.IsEquippable() && !(slot.inventory is ShopInventory))
			{
				this.ClearHighlightables();
				base.StartCoroutine(this.EquipItemCoroutine(slot));
			}
			else if (!SpaceStationInterior.instance && slot.contained.inventory == Inventory.cargo)
			{
				this.JettisonItem(slot, 1);
			}
			this.ClearSelectedItem();
			this.ReloadUI();
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x000762B2 File Offset: 0x000744B2
		private void SetItemInWorkshop(InventoryItemSlot item)
		{
			this.salvageWorkshop.SelectItem(item.contained.item, false, null);
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x000762CC File Offset: 0x000744CC
		private void AddShopCreditToWorkshop(Inventory.InventoryItem item, int amount)
		{
			this.salvageWorkshop.AddShopCredit(item.item, amount);
			item.inventory.Remove(item, amount);
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000762EE File Offset: 0x000744EE
		private IEnumerator EquipItemCoroutine(InventoryItemSlot slot)
		{
			if (!this.personalHangar)
			{
				yield return SpaceStationInterior.instance.AutomaticallySwitchTab(SpaceStationFacility.PersonalHangar);
				yield return null;
				yield return null;
			}
			if (slot.contained.item.itemCategory == ItemCategory.Turret)
			{
				List<SpaceShipHardpoint> list = this.FindHardpointsForTurret(slot, false);
				if (list.Count == 1)
				{
					this.EquipHardpoint(list[0], slot);
				}
				else if (list.Count > 1)
				{
					bool flag = false;
					foreach (SpaceShipHardpoint spaceShipHardpoint in list)
					{
						if (!spaceShipHardpoint.GetComponentInChildren<AbstractEquipment>())
						{
							this.EquipHardpoint(spaceShipHardpoint, slot);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTooManyEquipOptions", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
					}
				}
			}
			else if (slot.contained.item.itemCategory == ItemCategory.Module)
			{
				List<ModuleButton> list2 = this.FindModuleButtonForEquipment(slot);
				if (list2.Count == 1)
				{
					this.EquipModule(list2[0], slot);
				}
				else if (list2.Count > 1)
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTooManyEquipOptions", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				}
			}
			else if (slot.contained.item.itemCategory == ItemCategory.Booster)
			{
				InventoryItemType[] boosters = this.personalHangar.selectedShipData.boosters;
				SpaceShipBooster[] boosterSlots = this.personalHangar.selectedShipData.unitDefinition.boosterSlots;
				if (boosterSlots.Length == 1)
				{
					this.EquipBooster(boosterSlots[0], 0, slot);
				}
				else
				{
					bool flag2 = false;
					for (int i = 0; i < boosters.Length; i++)
					{
						if (boosters[i] == null)
						{
							this.EquipBooster(boosterSlots[i], i, slot);
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSTooManyEquipOptions", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
					}
				}
			}
			yield break;
		}

		// Token: 0x060011E6 RID: 4582 RVA: 0x00076304 File Offset: 0x00074504
		public void QuickPopup(InventoryItemSlot slot)
		{
			this.selectedItem = slot;
			if (this.inventoryShop && slot.inventory is ShopInventory)
			{
				if (slot.contained.count == 1)
				{
					this.BuyAmount(slot, 1, null, null);
				}
				else
				{
					this.CreatePopup(slot, "Buy", delegate
					{
						this.BuyAmount(slot, this.actionPopup.GetAmount(), null, null);
						this.DestroyPopup();
					}, false, null);
				}
			}
			else if (this.inventoryShop && !(slot.inventory is ShopInventory))
			{
				if (slot.contained.count == 1)
				{
					this.SellAmount(slot, 1, null);
				}
				else
				{
					this.CreatePopup(slot, "Sell", delegate
					{
						this.SellAmount(slot, this.actionPopup.GetAmount(), null);
						this.DestroyPopup();
					}, false, null);
				}
			}
			else if (slot.contained.inventory == Inventory.cargo)
			{
				if (slot.contained.count == 1)
				{
					if (slot.contained.item.IsUsable())
					{
						this.UseItem(slot);
					}
					else if (!SpaceStationInterior.instance)
					{
						this.JettisonItem(slot, 1);
					}
				}
				else if (!SpaceStationInterior.instance)
				{
					this.CreatePopup(slot, "Jettison", delegate
					{
						this.JettisonItem(slot, this.actionPopup.GetAmount());
						this.DestroyPopup();
					}, true, null);
				}
			}
			this.ClearSelectedItem();
			this.ReloadUI();
		}

		// Token: 0x060011E7 RID: 4583 RVA: 0x000764C4 File Offset: 0x000746C4
		public void PopupDogTagItem(DogTagItem dogTag)
		{
			this.actionPopup = UnityEngine.Object.Instantiate<DragableActionPopup>(this.popupPrefab, base.transform);
			this.actionPopup.exchangeItem = dogTag.GetCurrentCommendation();
			this.actionPopup.SetInventoryItem(dogTag.item, "Exchange", this.GetSelectedItemCount(), new int?(dogTag.rewardAmount), null);
			this.actionPopup.cancelButton.onClick.AddListener(new UnityAction(this.OnCancel));
			this.actionPopup.actionButton.onClick.AddListener(delegate()
			{
				InventoryItemType currentCommendation = dogTag.GetCurrentCommendation();
				int amount = this.actionPopup.GetAmount();
				if (currentCommendation != null && GamePlayer.current.ConsumeAvailableItems(dogTag.item, amount))
				{
					GamePlayer.current.currentSpaceShip.AddCargo(currentCommendation, dogTag.rewardAmount * amount, false);
				}
				this.DestroyPopup();
			});
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00076588 File Offset: 0x00074788
		private void DropItemOnSlot(InventoryItemSlot source, InventoryItemSlot target)
		{
			if (source.inventory == target.inventory)
			{
				if (source.contained != null && target.contained != null && source.contained.item == target.contained.item)
				{
					this.DropItemsOnEqualItem(source, target);
					return;
				}
				if (this.stack)
				{
					this.SplitStack(source, target);
					return;
				}
				if (!source.inventory.hasSearchFilter)
				{
					source.inventory.SwapItems(source.slot, target.slot);
					return;
				}
			}
			else if (source.inventory is ShopInventory && !(target.inventory is ShopInventory))
			{
				if (source.contained.count == 1)
				{
					this.BuyAmount(source, 1, target, null);
					return;
				}
				this.CreatePopup(source, "Buy", delegate
				{
					this.BuyAmount(source, this.actionPopup.GetAmount(), target, null);
					this.DestroyPopup();
				}, false, null);
				return;
			}
			else if (!(source.inventory is ShopInventory) && target.inventory is ShopInventory)
			{
				if (source.contained.count == 1)
				{
					this.SellAmount(source, 1, target);
					return;
				}
				this.CreatePopup(source, "Sell", delegate
				{
					this.SellAmount(source, this.actionPopup.GetAmount(), target);
					this.DestroyPopup();
				}, false, null);
			}
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00076760 File Offset: 0x00074960
		private void SplitStack(InventoryItemSlot source, InventoryItemSlot target)
		{
			if (target.contained == null)
			{
				int selectedItemCount = this.GetSelectedItemCount();
				if (this.selectedItem.contained.count - selectedItemCount == 0)
				{
					source.inventory.Remove(this.selectedItem.contained, selectedItemCount);
				}
				else
				{
					this.selectedItem.contained.SpecialAddCount(-selectedItemCount);
				}
				this.selectedItem.contained.item.SplitStack(source.inventory, target.slot, selectedItemCount);
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x000767E0 File Offset: 0x000749E0
		private void DropItemsOnEqualItem(InventoryItemSlot source, InventoryItemSlot target)
		{
			int selectedItemCount = this.GetSelectedItemCount();
			int num = this.selectedItem.contained.count - selectedItemCount;
			target.contained.SpecialAddCount(selectedItemCount);
			Debug.Log("Piling item: " + target.contained.item + ", neweSelected: " + num.ToString());
			if (num == 0)
			{
				source.inventory.Remove(this.selectedItem.contained, selectedItemCount);
				return;
			}
			this.selectedItem.contained.SpecialAddCount(-selectedItemCount);
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x0007686C File Offset: 0x00074A6C
		private void UseItem(InventoryItemSlot slot)
		{
			if (!GameplayManager.Instance)
			{
				return;
			}
			Inventory.InventoryItem contained = slot.contained;
			if (!contained.item.IsUsable())
			{
				return;
			}
			Inventory inventory = this.selectedItem.inventory;
			InventoryShop inventoryShop = this.inventoryShop;
			if (inventory == ((inventoryShop != null) ? inventoryShop.shopInventory : null))
			{
				return;
			}
			if (SpaceStationInterior.instance && !contained.item.IsUsableInSpaceStation())
			{
				return;
			}
			UsableItem usableItem;
			if (contained.item.TryGetComponent<UsableItem>(out usableItem) && !(usableItem is JumpgatePassItem))
			{
				if (usableItem.OnUse())
				{
					if (contained.inventory == Inventory.cargo)
					{
						Inventory.cargo.Remove(contained, 1);
						return;
					}
					if (contained.inventory == Inventory.global)
					{
						Inventory.global.Remove(contained, 1);
						return;
					}
				}
			}
			else if (!SpaceStationInterior.instance)
			{
				this.JettisonItem(slot, 1);
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00076940 File Offset: 0x00074B40
		private void JettisonItem(InventoryItemSlot slot, int count = -1)
		{
			if (!GameplayManager.Instance)
			{
				return;
			}
			if (!slot.contained.item.canJettison)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCantJettison", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (count < 0)
			{
				count = slot.contained.count;
			}
			Inventory.InventoryItem contained = slot.contained;
			if (Singleton<TravelManager>.Instance.TravelActive())
			{
				return;
			}
			if (contained.inventory == Inventory.cargo && Inventory.cargo.Remove(contained, count))
			{
				Singleton<LootManager>.Instance.CreateLootItem(GameplayManager.Instance.spaceShip.transform.position, contained.item, count, Faction.player, true);
				Singleton<EventLogManager>.Instance.NewEvent("Jettison" + contained.item.identifier, Translation.Translate("@LogJettisonedItem", new object[]
				{
					count,
					contained.item.displayName
				}));
				return;
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoItems", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00076A78 File Offset: 0x00074C78
		private void EquipHardpoint(SpaceShipHardpoint hardpoint, InventoryItemSlot slot)
		{
			AbstractTurret component = slot.contained.item.GetComponent<AbstractTurret>();
			if (!component)
			{
				return;
			}
			if (slot.inventory != Inventory.cargo && slot.inventory != Inventory.global)
			{
				return;
			}
			if (hardpoint.size >= component.size)
			{
				this.personalHangar.ReplaceTurret(slot.contained, hardpoint);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate(slot.contained.item.displayName, Array.Empty<object>()) + " " + Translation.Translate("@SSInstalled", Array.Empty<object>())).WithColor(slot.contained.item.rarity.GetColor()).Show();
			}
			else
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSHardpointSizeTurret", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
			}
			this.ReloadUI();
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00076B68 File Offset: 0x00074D68
		private void EquipModule(ModuleButton button, InventoryItemSlot slot)
		{
			AbstractModule component = slot.contained.item.GetComponent<AbstractModule>();
			if (!component)
			{
				return;
			}
			if (slot.inventory != Inventory.cargo && slot.inventory != Inventory.global)
			{
				return;
			}
			if (button.spaceShipModule.slot == component.slot && button.spaceShipModule.size == component.size)
			{
				this.personalHangar.InstallModule(slot.contained, button.spaceShipModule.slot);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate(slot.contained.item.displayName, Array.Empty<object>()) + " " + Translation.Translate("@SSInstalled", Array.Empty<object>())).WithColor(slot.contained.item.rarity.GetColor()).Show();
			}
			else
			{
				this.SizeNotification();
			}
			this.ReloadUI();
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00076C5C File Offset: 0x00074E5C
		private void EquipBooster(SpaceShipBooster boosterSlot, int index, InventoryItemSlot slot)
		{
			AbstractBooster component = slot.contained.item.GetComponent<AbstractBooster>();
			if (!component)
			{
				return;
			}
			if (slot.inventory != Inventory.cargo && slot.inventory != Inventory.global)
			{
				return;
			}
			if (boosterSlot.size == component.size)
			{
				this.personalHangar.InstallBooster(slot.contained, index);
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate(slot.contained.item.displayName, Array.Empty<object>()) + " " + Translation.Translate("@SSInstalled", Array.Empty<object>())).WithColor(slot.contained.item.rarity.GetColor()).Show();
			}
			else
			{
				this.SizeNotification();
			}
			this.ReloadUI();
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00076D28 File Offset: 0x00074F28
		private void SizeNotification()
		{
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSModuleSlotSize", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x00076D54 File Offset: 0x00074F54
		private void CreatePopup(InventoryItemSlot item, string action, UnityAction onSubmitAction, bool hideCost = false, int? overrideCost = null)
		{
			this.actionPopup = UnityEngine.Object.Instantiate<DragableActionPopup>(this.popupPrefab, base.transform);
			this.actionPopup.targetInventory = this.inventoryPanel.inventory;
			if (item.contained.costItem)
			{
				overrideCost = new int?(item.contained.costItemCount);
			}
			this.actionPopup.SetInventoryItem(item.contained.item, action, this.GetSelectedItemCount(), overrideCost, item.contained.costItem);
			this.actionPopup.cancelButton.onClick.AddListener(new UnityAction(this.OnCancel));
			this.actionPopup.actionButton.onClick.AddListener(onSubmitAction);
			if (hideCost)
			{
				this.actionPopup.HideCost();
			}
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x00076E24 File Offset: 0x00075024
		public bool BuyAmount(InventoryItemType itemType, int amount)
		{
			Inventory.InventoryItem item = this.inventoryShop.shopInventory.Get(itemType);
			return this.BuyAmount(item, amount, null, Inventory.cargo);
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00076E51 File Offset: 0x00075051
		private bool BuyAmount(InventoryItemSlot item, int amount, InventoryItemSlot targetSlot = null, Inventory targetInventory = null)
		{
			SpacestationExteriorManager.Instance.GetDockedSpaceship(GameplayManager.Instance.spaceShip);
			return this.BuyAmount(item.contained, amount, targetSlot, targetInventory);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00076E80 File Offset: 0x00075080
		private bool BuyAmount(Inventory.InventoryItem item, int amount, InventoryItemSlot targetSlot = null, Inventory targetInventory = null)
		{
			if (amount <= 0 || amount > item.count)
			{
				return false;
			}
			MapPointOfInterest currentPointOfInterest = GamePlayer.current.currentPointOfInterest;
			Faction faction = (currentPointOfInterest != null) ? currentPointOfInterest.faction : null;
			if (faction != null && !item.item.EnoughRepWithFaction(faction))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSRepTooLow", new object[]
				{
					faction.name
				})).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (GamePlayer.current.level < item.item.shopItemData.levelRequirement)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSLevelTooLow", new object[]
				{
					item.item.shopItemData.levelRequirement
				})).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (targetInventory == null)
			{
				targetInventory = (((targetSlot != null) ? targetSlot.inventory : null) ?? this.inventoryPanel.inventory);
			}
			SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
			if (targetInventory == GamePlayer.current.globalInventory)
			{
				if (!item.item.CanGoInArmory())
				{
					targetInventory = spaceStation.materialStorage;
				}
			}
			else if (targetInventory == ((spaceStation != null) ? spaceStation.materialStorage : null) && !item.item.CanGoInMaterials())
			{
				targetInventory = GamePlayer.current.globalInventory;
			}
			if (targetInventory.IsFull(item.item.m3 * (float)amount))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCargoFull", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (item.costItem)
			{
				int num = item.costItemCount * amount;
				if (GamePlayer.current.CountAvailableItems(item.costItem) < num)
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCurrency", new object[]
					{
						item.costItem.displayName
					})).WithColor(ColorHelper.red90).Show();
					return false;
				}
				GamePlayer.current.ConsumeAvailableItems(item.costItem, num);
			}
			else
			{
				int num2 = item.cost * amount;
				if (!GamePlayer.current.CanAfford((float)num2))
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
					return false;
				}
				GamePlayer.current.RemoveCredits((float)num2);
			}
			InventoryItemType item2 = item.item;
			if (targetInventory.hasSearchFilter || targetSlot == null || targetSlot.contained != null)
			{
				targetInventory.Add(item2, amount, false, false);
			}
			else
			{
				targetInventory.Set(targetSlot.slot, item2, amount, false);
			}
			if (!item.item.HasInfiniteShopSupply())
			{
				item.inventory.Remove(item, amount);
			}
			InventoryItemPart[] components = item2.GetComponents<InventoryItemPart>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].OnPurchase(amount);
			}
			return true;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x0007715E File Offset: 0x0007535E
		private bool SellAmount(InventoryItemSlot item, int amount, InventoryItemSlot targetSlot = null)
		{
			return this.SellAmount(item.contained, amount, targetSlot);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00077170 File Offset: 0x00075370
		public bool SellAmount(Inventory.InventoryItem item, int amount, InventoryItemSlot targetSlot = null)
		{
			if (amount <= 0 || amount > item.count)
			{
				return false;
			}
			if (!item.item.canSell)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCantSell", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (item.item.sellValue == 0)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoSellValue", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			InventoryItemType item2 = item.item;
			GamePlayer.current.credits += (long)(item.item.sellValue * amount);
			Inventory inventory = ((targetSlot != null) ? targetSlot.inventory : null) ?? this.inventoryShop.shopInventory;
			if ((inventory != null && inventory.hasSearchFilter) || targetSlot == null || targetSlot.contained != null)
			{
				inventory.Add(item2, amount, item.item.buyBack, false);
			}
			else
			{
				inventory.Set(targetSlot.slot, item2, amount, item.item.buyBack);
			}
			item.inventory.Remove(item, amount);
			return true;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00077297 File Offset: 0x00075497
		private void DestroyPopup()
		{
			UnityEngine.Object.Destroy(this.actionPopup.gameObject);
			this.ReloadUI();
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x000772AF File Offset: 0x000754AF
		private void OnCancel()
		{
			this.DestroyPopup();
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x000772B7 File Offset: 0x000754B7
		public void ReloadUI()
		{
			if (this.inventoryPanel)
			{
				this.inventoryPanel.ReloadUI(false);
			}
			if (this.inventoryShop)
			{
				this.inventoryShop.ReloadUI();
			}
			UITooltip.Refresh();
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x000772F0 File Offset: 0x000754F0
		public void HighlightDropTargets(InventoryItemSlot itemSlot)
		{
			if (!this.personalHangar)
			{
				return;
			}
			if (itemSlot.contained.item.itemCategory == ItemCategory.Turret)
			{
				foreach (SpaceShipHardpoint spaceShipHardpoint in this.FindHardpointsForTurret(itemSlot, true))
				{
					spaceShipHardpoint.ShowHighlight();
					this.highlightables.Add(spaceShipHardpoint);
				}
			}
			if (itemSlot.contained.item.itemCategory == ItemCategory.Module)
			{
				foreach (ModuleButton moduleButton in this.FindModuleButtonForEquipment(itemSlot))
				{
					moduleButton.ShowHighlight();
					this.highlightables.Add(moduleButton);
				}
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x000773D4 File Offset: 0x000755D4
		private List<ModuleButton> FindModuleButtonForEquipment(InventoryItemSlot itemSlot)
		{
			List<ModuleButton> list = new List<ModuleButton>();
			AbstractEquipment component = itemSlot.contained.item.gameObject.GetComponent<AbstractEquipment>();
			foreach (ModuleButton moduleButton in this.personalHangar.GetModulesButtons())
			{
				if (moduleButton.spaceShipModule.slot == component.slot && moduleButton.spaceShipModule.size == component.size)
				{
					list.Add(moduleButton);
				}
			}
			return list;
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00077470 File Offset: 0x00075670
		private List<SpaceShipHardpoint> FindHardpointsForTurret(InventoryItemSlot itemSlot, bool allowSmaller = true)
		{
			List<SpaceShipHardpoint> list = new List<SpaceShipHardpoint>();
			AbstractTurret component = itemSlot.contained.item.gameObject.GetComponent<AbstractTurret>();
			foreach (SpaceShipHardpoint spaceShipHardpoint in this.personalHangar.GetSelectedShip().GetComponentsInChildren<SpaceShipHardpoint>())
			{
				if ((allowSmaller && spaceShipHardpoint.size >= component.size) || (!allowSmaller && spaceShipHardpoint.size == component.size))
				{
					list.Add(spaceShipHardpoint);
				}
			}
			return list;
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x000774EC File Offset: 0x000756EC
		public void ClearHighlightables()
		{
			foreach (IHighlightable highlightable in this.highlightables)
			{
				highlightable.HideHighlight();
			}
			this.highlightables.Clear();
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00077548 File Offset: 0x00075748
		public void DisableScrolling(bool disable)
		{
			if (this.inventoryShop)
			{
				this.inventoryShop.DisableScrolling(disable);
			}
			if (this.inventoryPanel)
			{
				this.inventoryPanel.DisableScrolling(disable);
			}
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x0007757C File Offset: 0x0007577C
		public void UnequipHardpoint(SpaceShipHardpoint hardpoint)
		{
			this.personalHangar.ReplaceTurret(null, hardpoint);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x0007758C File Offset: 0x0007578C
		public void UnequipEquipment(EquipmentButton equipmentButton)
		{
			ModuleButton moduleButton = equipmentButton as ModuleButton;
			if (moduleButton != null)
			{
				this.personalHangar.InstallModule(null, moduleButton.spaceShipModule.slot);
				return;
			}
			BoosterButton boosterButton = equipmentButton as BoosterButton;
			if (boosterButton != null)
			{
				this.personalHangar.InstallBooster(null, boosterButton.index);
			}
		}

		// Token: 0x040009D1 RID: 2513
		public static InventoryInteractionManager Instance;

		// Token: 0x040009D3 RID: 2515
		private bool canReleaseItem;

		// Token: 0x040009D4 RID: 2516
		private bool stack;

		// Token: 0x040009D5 RID: 2517
		[SerializeField]
		public InventoryItemSlot inventorySlotPrefab;

		// Token: 0x040009D6 RID: 2518
		[SerializeField]
		public InventoryDraggable draggableItem;

		// Token: 0x040009D7 RID: 2519
		[SerializeField]
		private DragableActionPopup popupPrefab;

		// Token: 0x040009D8 RID: 2520
		private DragableActionPopup actionPopup;

		// Token: 0x040009D9 RID: 2521
		private InventoryShop inventoryShop;

		// Token: 0x040009DB RID: 2523
		private PersonalHangar personalHangar;

		// Token: 0x040009DC RID: 2524
		private SalvageWorkshop salvageWorkshop;

		// Token: 0x040009DD RID: 2525
		public const string BUY = "Buy";

		// Token: 0x040009DE RID: 2526
		public const string SELL = "Sell";

		// Token: 0x040009DF RID: 2527
		public const string TRANSFER = "Transfer";

		// Token: 0x040009E0 RID: 2528
		public const string JETTISON = "Jettison";

		// Token: 0x040009E1 RID: 2529
		public const string WORKSHOPDEPOSIT = "Trade in";

		// Token: 0x040009E2 RID: 2530
		public const string EXTRACT = "Extract";

		// Token: 0x040009E3 RID: 2531
		public const string EXCHANGEDOGTAG = "Exchange";

		// Token: 0x040009E8 RID: 2536
		private List<IHighlightable> highlightables = new List<IHighlightable>();
	}
}
