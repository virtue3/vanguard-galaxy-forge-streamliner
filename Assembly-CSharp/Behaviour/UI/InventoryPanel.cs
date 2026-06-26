using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.UI.Spacestation;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001DE RID: 478
	public class InventoryPanel : MonoBehaviour
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x00077D1A File Offset: 0x00075F1A
		private void Awake()
		{
			this.inventoryInteractionManager = InventoryInteractionManager.Instance;
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x00077D27 File Offset: 0x00075F27
		private void Start()
		{
			this.FillCargoImage();
			this.SetCreditsUI();
			this.SetQuickButtonsVisibility();
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x00077D3C File Offset: 0x00075F3C
		public void SetInventory(Inventory inventory, InventoryType inventoryType)
		{
			this.inventory = inventory;
			this.inventoryType = inventoryType;
			this.inventoryInteractionManager.RegisterInventory(this);
			if (inventory.hasSearchFilter)
			{
				this.searchInput.SetTextWithoutNotify(inventory.searchFilter);
			}
			GameObject gameObject = this.sellVisibleButton.gameObject;
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			gameObject.SetActive(spaceStation != null && spaceStation.shopInventory != null);
			this.ReloadUI(true);
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00077DB0 File Offset: 0x00075FB0
		private void Update()
		{
			if (GamePlayer.current.currentSpaceShip == null)
			{
				return;
			}
			this.UpdateCreditsUI();
			this.UpdateCargoUI();
			this.FillCargoImage();
			if (this.inputTimer > 0f)
			{
				this.inputTimer -= Time.deltaTime;
				if (this.inputTimer < 0f)
				{
					this.ProcessSearchInput();
					this.inputTimer = 0f;
				}
			}
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00077E19 File Offset: 0x00076019
		private void UpdateCargoUI()
		{
			this.cargoText.text = GameMath.FormatNumber(this.inventory.spaceUsed, -1) + " / " + GameMath.FormatNumber(this.inventory.capacity, -1);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x00077E54 File Offset: 0x00076054
		private void SetCreditsUI()
		{
			long credits = GamePlayer.current.credits;
			this.lastCredits = credits;
			this.creditsText.text = "$ " + GameMath.FormatNumber((float)credits, -1);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00077E90 File Offset: 0x00076090
		private void UpdateCreditsUI()
		{
			long credits = GamePlayer.current.credits;
			if (credits != this.lastCredits)
			{
				if (this.creditsChange != null)
				{
					base.StopCoroutine(this.creditsChange);
				}
				this.creditsChange = base.StartCoroutine(this.CreditChange(credits));
				this.lastCredits = credits;
			}
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x00077EDF File Offset: 0x000760DF
		private IEnumerator CreditChange(long creditAmount)
		{
			float elapsedTime = 0f;
			float fadeDuration = 0.1f;
			while (elapsedTime < fadeDuration)
			{
				elapsedTime += Time.deltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
				this.creditsText.color = this.creditsText.color.WithAlpha(alpha);
				yield return null;
			}
			this.creditsText.text = "$ " + GameMath.FormatNumber((float)creditAmount, -1);
			elapsedTime = 0f;
			fadeDuration = 0.15f;
			while (elapsedTime < fadeDuration)
			{
				elapsedTime += Time.deltaTime;
				float alpha2 = Mathf.Clamp01(elapsedTime / fadeDuration);
				this.creditsText.color = this.creditsText.color.WithAlpha(alpha2);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x00077EF8 File Offset: 0x000760F8
		private void PopulateInventory()
		{
			this.updateScroll = false;
			float num = InventoryPanel.itemSize + InventoryPanel.itemGap;
			this.visibleRowCount = Mathf.CeilToInt(InventoryPanel.contentHeight / num);
			int lastVisibleItemCount = this.inventory.GetLastVisibleItemCount();
			this.allItemRowCount = Mathf.CeilToInt((float)lastVisibleItemCount / (float)this.defaultColumns);
			int firstEmptyItemIndex = this.inventory.GetFirstEmptyItemIndex();
			int num2 = 1;
			if (firstEmptyItemIndex >= lastVisibleItemCount)
			{
				num2++;
			}
			this.allItemRowCount += num2;
			this.scrollRect.verticalScrollbar.numberOfSteps = Mathf.Clamp(this.allItemRowCount - this.visibleRowCount, 0, this.allItemRowCount) + 1;
			this.inventoryRect.sizeDelta = new Vector2(this.inventoryRect.sizeDelta.x, (float)this.allItemRowCount * num + InventoryPanel.itemGap);
			this.RenderItemRows();
			base.StartCoroutine(this.RestoreScrollNextFrame());
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00077FDC File Offset: 0x000761DC
		private void RenderItemRows()
		{
			if (this.inventoryInteractionManager.selectedItem)
			{
				this.inventoryInteractionManager.selectedItem.transform.SetParent(this.inventoryInteractionManager.transform);
			}
			this.inventoryRect.DestroyChildren();
			this.slots.Clear();
			int num = Mathf.RoundToInt((1f - Mathf.Clamp(this.inventory.scrollPosition, 0f, 1f)) * (float)(this.scrollRect.verticalScrollbar.numberOfSteps - 1));
			for (int i = 0; i < this.visibleRowCount; i++)
			{
				int num2 = i + num;
				float y = -InventoryPanel.itemGap - (float)num2 * (InventoryPanel.itemSize + InventoryPanel.itemGap);
				for (int j = 0; j < this.defaultColumns; j++)
				{
					int slot = num2 * this.defaultColumns + j;
					InventoryItemSlot inventoryItemSlot = this.inventoryInteractionManager.CreateInventorySlot(this.inventoryRect, this.inventory, slot);
					((RectTransform)inventoryItemSlot.transform).anchoredPosition = new Vector2(InventoryPanel.itemGap + (float)j * InventoryPanel.itemSize, y);
					this.slots.Add(inventoryItemSlot);
				}
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x0007810C File Offset: 0x0007630C
		private void RestoreScroll()
		{
			this.updateScroll = false;
			this.scrollRect.verticalScrollbar.value = this.inventory.scrollPosition;
			this.updateScroll = true;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00078137 File Offset: 0x00076337
		private IEnumerator RestoreScrollNextFrame()
		{
			yield return null;
			this.updateScroll = true;
			this.RestoreScroll();
			yield break;
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00078146 File Offset: 0x00076346
		public void OnScrollChange()
		{
			if (CanvasUpdateRegistry.IsRebuildingLayout() || !this.updateScroll)
			{
				return;
			}
			this.inventory.SetScrollPosition(this.scrollRect.verticalScrollbar.value);
			this.RenderItemRows();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x0007817C File Offset: 0x0007637C
		private void RefreshInventory(bool init)
		{
			if (init)
			{
				this.PopulateInventory();
			}
			else
			{
				foreach (InventoryItemSlot inventoryItemSlot in this.slots)
				{
					inventoryItemSlot.UpdateItem();
				}
			}
			this.spaceUsed = this.inventory.spaceUsed;
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x000781E8 File Offset: 0x000763E8
		public void ReloadUI(bool init = false)
		{
			this.inventory.UpdateVisibleItems();
			this.RefreshInventory(init);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x000781FC File Offset: 0x000763FC
		private void FillCargoImage()
		{
			if (!this.spaceUsed.ApproximatelyEqual(this.inventory.spaceUsed))
			{
				this.ReloadUI(false);
			}
			this.fillImage.fillAmount = Mathf.Clamp01(this.inventory.GetAvailableSpacePercentage());
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00078238 File Offset: 0x00076438
		public void SetQuickButtonsVisibility()
		{
			if (!SpaceStationInterior.instance)
			{
				this.quickButtons.SetActive(false);
				return;
			}
			this.quickButtons.SetActive(true);
			if (this.inventory == Inventory.cargo)
			{
				this.moveToCargo.SetActive(false);
				return;
			}
			this.moveToCargo.SetActive(true);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00078290 File Offset: 0x00076490
		public void MoveAllToMaterials()
		{
			this.MoveItemsToInventoryType(Inventory.materials);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x0007829D File Offset: 0x0007649D
		public void MoveAllToArmory()
		{
			this.MoveItemsToInventoryType(Inventory.global);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000782AC File Offset: 0x000764AC
		public void MoveAllToCargo()
		{
			if (this.inventory == Inventory.materials || this.inventory == Inventory.global)
			{
				Inventory cargo = GamePlayer.current.currentSpaceShip.cargo;
				foreach (Inventory.InventoryItem inventoryItem in this.inventory.filteredItems.ToList<Inventory.InventoryItem>())
				{
					if (cargo.IsFull(1f))
					{
						break;
					}
					int count = inventoryItem.count;
					this.TryMoveItemToCargo(inventoryItem.item, count, this.inventory, cargo);
				}
				this.ReloadUI(false);
			}
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x00078360 File Offset: 0x00076560
		private void TryMoveItemToCargo(InventoryItemType item, int amount, Inventory inventoryFrom, Inventory shipCargo)
		{
			if (shipCargo.GetSpaceAvailable() >= (float)amount * item.m3)
			{
				shipCargo.Add(item, amount, false, false);
				inventoryFrom.Remove(item, amount);
				return;
			}
			int num = Mathf.FloorToInt(shipCargo.GetSpaceAvailable() / item.m3);
			if (num > 0)
			{
				shipCargo.Add(item, num, false, false);
				inventoryFrom.Remove(item, num);
			}
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000783C4 File Offset: 0x000765C4
		public void MoveItemsToInventoryType(Inventory inventoryDestination)
		{
			if (GamePlayer.current.currentPointOfInterest is SpaceStation)
			{
				bool flag = inventoryDestination == GamePlayer.current.globalInventory;
				GamePlayer.current.currentSpaceShip.TransferCargo(!flag, flag, true, true);
				this.ReloadUI(false);
			}
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x00078410 File Offset: 0x00076610
		private void MoveItem(InventoryItemType item, int quantityToMove, bool armory, SpaceStation ss)
		{
			if (armory)
			{
				GamePlayer.current.currentSpaceShip.cargo.Remove(item, quantityToMove);
				GamePlayer.current.globalInventory.Add(item, quantityToMove, false, false);
				return;
			}
			GamePlayer.current.currentSpaceShip.cargo.Remove(item, quantityToMove);
			ss.materialStorage.Add(item, quantityToMove, false, false);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x00078474 File Offset: 0x00076674
		public void OnSellVisible()
		{
			if (!SpaceStationInterior.instance || SpaceStation.current == null)
			{
				return;
			}
			SpaceStationInterior.instance.GoToLocation(SpaceStation.current.shopInventory.facility, true);
			AlertPopup.ShowQuery(Translation.Translate("@SSSellVisibleConfirm", this.inventory.GetSellableItemParams()), null, null, new Action(this.SellVisible), null, null, null);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000784DA File Offset: 0x000766DA
		public void SellVisible()
		{
			this.inventory.SellVisibleItems();
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000784E7 File Offset: 0x000766E7
		public void OnSearchInput()
		{
			this.inputTimer = 0.2f;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x000784F4 File Offset: 0x000766F4
		private void ProcessSearchInput()
		{
			if (this.inventory.searchFilter != this.searchInput.text)
			{
				this.inventory.SetSearchFilter(this.searchInput.text);
				this.ReloadUI(true);
			}
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00078530 File Offset: 0x00076730
		public void ClearSearch()
		{
			this.searchInput.text = "";
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00078542 File Offset: 0x00076742
		public void SortByCategory()
		{
			this.inventory.SortByCategory();
			this.ReloadUI(false);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00078556 File Offset: 0x00076756
		public void SortByValue()
		{
			this.inventory.SortByValue();
			this.ReloadUI(false);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0007856A File Offset: 0x0007676A
		public void DisableScrolling(bool disable)
		{
			this.scrollRect.enabled = !disable;
		}

		// Token: 0x040009F9 RID: 2553
		public static float contentHeight = 246f;

		// Token: 0x040009FA RID: 2554
		public static float itemSize = 78f;

		// Token: 0x040009FB RID: 2555
		public static float itemGap = 4f;

		// Token: 0x040009FC RID: 2556
		[SerializeField]
		private TextMeshProUGUI cargoText;

		// Token: 0x040009FD RID: 2557
		[SerializeField]
		private TextMeshProUGUI creditsText;

		// Token: 0x040009FE RID: 2558
		[SerializeField]
		public RectTransform inventoryRect;

		// Token: 0x040009FF RID: 2559
		[SerializeField]
		private TMP_InputField searchInput;

		// Token: 0x04000A00 RID: 2560
		[SerializeField]
		private Button sellVisibleButton;

		// Token: 0x04000A01 RID: 2561
		private float inputTimer;

		// Token: 0x04000A02 RID: 2562
		[SerializeField]
		private Image fillImage;

		// Token: 0x04000A03 RID: 2563
		[SerializeField]
		private int defaultColumns;

		// Token: 0x04000A04 RID: 2564
		[SerializeField]
		private int defaultRows;

		// Token: 0x04000A05 RID: 2565
		private int allItemRowCount;

		// Token: 0x04000A06 RID: 2566
		private int visibleRowCount;

		// Token: 0x04000A07 RID: 2567
		[SerializeField]
		private GameObject quickButtons;

		// Token: 0x04000A08 RID: 2568
		[SerializeField]
		private Button moveToMaterials;

		// Token: 0x04000A09 RID: 2569
		[SerializeField]
		private Button moveToArmory;

		// Token: 0x04000A0A RID: 2570
		[SerializeField]
		private GameObject moveToCargo;

		// Token: 0x04000A0B RID: 2571
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x04000A0C RID: 2572
		private InventoryInteractionManager inventoryInteractionManager;

		// Token: 0x04000A0D RID: 2573
		private float spaceUsed;

		// Token: 0x04000A0E RID: 2574
		public Inventory inventory;

		// Token: 0x04000A0F RID: 2575
		private bool updateScroll = true;

		// Token: 0x04000A10 RID: 2576
		public InventoryType inventoryType;

		// Token: 0x04000A11 RID: 2577
		private long lastCredits;

		// Token: 0x04000A12 RID: 2578
		private Coroutine creditsChange;

		// Token: 0x04000A13 RID: 2579
		private List<InventoryItemSlot> slots = new List<InventoryItemSlot>();
	}
}
