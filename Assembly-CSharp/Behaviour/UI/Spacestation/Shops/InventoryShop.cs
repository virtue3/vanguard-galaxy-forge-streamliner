using System;
using Behaviour.UI.Timer;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Shops
{
	// Token: 0x0200021C RID: 540
	public class InventoryShop : MonoBehaviour
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x00081648 File Offset: 0x0007F848
		// (set) Token: 0x06001404 RID: 5124 RVA: 0x00081650 File Offset: 0x0007F850
		public float refreshCost { get; private set; } = 5000f;

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x00081659 File Offset: 0x0007F859
		// (set) Token: 0x06001406 RID: 5126 RVA: 0x00081661 File Offset: 0x0007F861
		public Inventory shopInventory { get; private set; }

		// Token: 0x06001407 RID: 5127 RVA: 0x0008166A File Offset: 0x0007F86A
		private void Awake()
		{
			InventoryInteractionManager.Instance.RegisterShop(this);
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00081677 File Offset: 0x0007F877
		private void Start()
		{
			this.ShowRepRefresh();
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0008167F File Offset: 0x0007F87F
		private void ShowRepRefresh()
		{
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00081684 File Offset: 0x0007F884
		private void Update()
		{
			this.countdown.timer = this.GetRemainingTime();
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

		// Token: 0x0600140B RID: 5131 RVA: 0x000816DF File Offset: 0x0007F8DF
		public void SetInventories(Inventory shopInventory)
		{
			this.shopInventory = shopInventory;
			this.PopulateInventories();
			if (shopInventory.hasSearchFilter)
			{
				this.searchInput.SetTextWithoutNotify(shopInventory.searchFilter);
			}
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00081707 File Offset: 0x0007F907
		public void ReloadUI()
		{
			this.shopInventoryRect.DestroyChildren();
			this.shopInventory.UpdateVisibleItems();
			this.PopulateInventories();
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00081725 File Offset: 0x0007F925
		private void PopulateInventories()
		{
			this.PopulateInventory(this.shopInventory, this.shopInventoryRect);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0008173C File Offset: 0x0007F93C
		protected void PopulateInventory(Inventory inv, RectTransform parent)
		{
			parent.DestroyChildren();
			int num = 0;
			foreach (Inventory.InventoryItem inventoryItem in inv.items)
			{
				num = Mathf.Max(num, inventoryItem.slot);
			}
			bool flag = true;
			bool flag2 = false;
			int num2 = 0;
			while (flag || num2 < this.defaultRows)
			{
				flag = false;
				int i = 0;
				while (i < this.defaultColumns)
				{
					int num3 = num2 * this.defaultColumns + i;
					if (i != 0 || num2 != 0)
					{
						goto IL_A9;
					}
					ShopInventory shopInventory = inv as ShopInventory;
					if (shopInventory == null || shopInventory.parent.umbralControlLevel < 0.5f)
					{
						goto IL_A9;
					}
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.umbralShopButton, parent).transform as RectTransform;
					flag2 = true;
					IL_D3:
					rectTransform.anchoredPosition = new Vector2((float)(4 + i * 79), (float)(-4 - num2 * 79));
					i++;
					continue;
					IL_A9:
					if (flag2)
					{
						num3--;
					}
					rectTransform = (InventoryInteractionManager.Instance.CreateInventorySlot(parent, inv, num3).transform as RectTransform);
					if (num3 <= num)
					{
						flag = true;
						goto IL_D3;
					}
					goto IL_D3;
				}
				num2++;
			}
			parent.sizeDelta = new Vector2(parent.sizeDelta.x, (float)(num2 * 79 + 8));
			this.countdown.timer = this.GetRemainingTime();
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x000818A0 File Offset: 0x0007FAA0
		private float GetRemainingTime()
		{
			float num = (float)GamePlayer.current.elapsedTime;
			float num2 = 3600f;
			return (float)(Mathf.FloorToInt(num / num2) + 1) * num2 - num;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x000818CE File Offset: 0x0007FACE
		public void FilterUI(ItemCategory category)
		{
			this.filterCategory = category;
			this.ReloadUI();
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000818DD File Offset: 0x0007FADD
		public void SortByCategory()
		{
			this.shopInventory.SortByCategory();
			this.ReloadUI();
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000818F0 File Offset: 0x0007FAF0
		public void SortByValue()
		{
			this.shopInventory.SortByValue();
			this.ReloadUI();
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x00081903 File Offset: 0x0007FB03
		public void OnSearchInput()
		{
			this.inputTimer = 0.2f;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00081910 File Offset: 0x0007FB10
		private void ProcessSearchInput()
		{
			if (this.shopInventory.searchFilter != this.searchInput.text)
			{
				this.shopInventory.SetSearchFilter(this.searchInput.text);
				this.ReloadUI();
			}
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0008194B File Offset: 0x0007FB4B
		public void ClearSearch()
		{
			this.searchInput.text = "";
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x00081960 File Offset: 0x0007FB60
		public void RefreshShop()
		{
			if (MapPointOfInterest.current.faction.GetReputationLevel(Faction.player).CanRefreshShop())
			{
				return;
			}
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null)
			{
				if (this.shopInventory == spaceStation.generalShopInventory)
				{
					spaceStation.GenerateShopInventory();
				}
				else if (this.shopInventory == spaceStation.miningShopInventory)
				{
					spaceStation.GenerateMiningShopInventory();
				}
				else if (this.shopInventory == spaceStation.salvageShopInventory)
				{
					spaceStation.GenerateSalvageShopInventory();
				}
				else if (this.shopInventory == spaceStation.bountyShopInventory)
				{
					spaceStation.GenerateBountyShopInventory();
				}
				else if (this.shopInventory == spaceStation.patrolShopInventory)
				{
					spaceStation.GeneratePatrolShopInventory();
				}
				else if (this.shopInventory == spaceStation.industryShopInventory)
				{
					spaceStation.GenerateIndustryShopInventory();
				}
				else if (this.shopInventory == spaceStation.conquestShopInventory)
				{
					spaceStation.GenerateConquestShopInventory();
				}
				else if (this.shopInventory == spaceStation.umbralShopInventory)
				{
					spaceStation.GenerateUmbralShopInventory();
				}
			}
			this.ReloadUI();
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00081A54 File Offset: 0x0007FC54
		public void DisableScrolling(bool disable)
		{
			this.scrollRect.enabled = !disable;
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00081A68 File Offset: 0x0007FC68
		public void ToggleUmbralInventory()
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null)
			{
				if (this.shopInventory == spaceStation.shopInventory)
				{
					this.shopInventory = spaceStation.CreateUmbralShopInventory();
				}
				else
				{
					this.shopInventory = spaceStation.shopInventory;
				}
				this.shopInventory.SetSearchFilter(spaceStation.shopInventory.searchFilter);
				this.ReloadUI();
			}
		}

		// Token: 0x04000B93 RID: 2963
		[SerializeField]
		private int defaultColumns;

		// Token: 0x04000B94 RID: 2964
		[SerializeField]
		private int defaultRows;

		// Token: 0x04000B95 RID: 2965
		[SerializeField]
		private ScrollRect scrollRect;

		// Token: 0x04000B96 RID: 2966
		[SerializeField]
		private RectTransform shopInventoryRect;

		// Token: 0x04000B97 RID: 2967
		[SerializeField]
		private TMP_InputField searchInput;

		// Token: 0x04000B98 RID: 2968
		private float inputTimer;

		// Token: 0x04000B99 RID: 2969
		[SerializeField]
		private CountdownTimer countdown;

		// Token: 0x04000B9A RID: 2970
		[SerializeField]
		private RefreshShopButton refreshShopButton;

		// Token: 0x04000B9B RID: 2971
		[SerializeField]
		private RectTransform umbralShopButton;

		// Token: 0x04000B9E RID: 2974
		private ItemCategory filterCategory;
	}
}
