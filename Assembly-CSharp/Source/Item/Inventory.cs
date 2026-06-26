using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.UI;
using LightJson;
using Source.Galaxy.POI;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Item
{
	// Token: 0x020000F2 RID: 242
	public class Inventory : IJsonSource
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x000468BB File Offset: 0x00044ABB
		public static Inventory cargo
		{
			get
			{
				return GamePlayer.current.currentSpaceShip.cargo;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x000468CC File Offset: 0x00044ACC
		public static Inventory materials
		{
			get
			{
				SpaceStation current = SpaceStation.current;
				if (current == null)
				{
					return null;
				}
				return current.materialStorage;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x000468DE File Offset: 0x00044ADE
		public static Inventory global
		{
			get
			{
				return GamePlayer.current.globalInventory;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x000468EA File Offset: 0x00044AEA
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x000468F2 File Offset: 0x00044AF2
		public string searchFilter { get; protected set; } = "";

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x000468FB File Offset: 0x00044AFB
		public bool hasSearchFilter
		{
			get
			{
				return !string.IsNullOrEmpty(this.searchFilter);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0004690B File Offset: 0x00044B0B
		// (set) Token: 0x06000914 RID: 2324 RVA: 0x00046913 File Offset: 0x00044B13
		public float scrollPosition { get; protected set; }

		// Token: 0x06000915 RID: 2325 RVA: 0x0004691C File Offset: 0x00044B1C
		public void SetScrollPosition(float value)
		{
			this.scrollPosition = value;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00046925 File Offset: 0x00044B25
		public IEnumerable<Inventory.InventoryItem> filteredItems
		{
			get
			{
				int num;
				for (int i = 0; i < this.visibleItems.Length; i = num + 1)
				{
					if (this.visibleItems[i] != null)
					{
						yield return this.visibleItems[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00046935 File Offset: 0x00044B35
		public IEnumerable<Inventory.InventoryItem> items
		{
			get
			{
				int num;
				for (int i = 0; i < this.allItems.Length; i = num + 1)
				{
					if (this.allItems[i] != null)
					{
						yield return this.allItems[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00046948 File Offset: 0x00044B48
		public float spaceUsed
		{
			get
			{
				float num = 0f;
				foreach (Inventory.InventoryItem inventoryItem in this.items)
				{
					num += inventoryItem.item.m3 * (float)inventoryItem.count;
				}
				return num;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x000469AC File Offset: 0x00044BAC
		public int count
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.allItems.Length; i++)
				{
					if (this.allItems[i] != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x000469DD File Offset: 0x00044BDD
		// (set) Token: 0x0600091B RID: 2331 RVA: 0x00046A17 File Offset: 0x00044C17
		public float capacity
		{
			get
			{
				if (!this.isMaterialStorage)
				{
					return this._capacity;
				}
				if (SkilltreeNode.industrialMaterialStorage2.isActive)
				{
					return 100000f;
				}
				if (!SkilltreeNode.industrialMaterialStorage.isActive)
				{
					return 20000f;
				}
				return 50000f;
			}
			set
			{
				this._capacity = value;
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x00046A20 File Offset: 0x00044C20
		public Inventory(bool isMaterialStorage = false)
		{
			this.isMaterialStorage = isMaterialStorage;
			this._capacity = 100000f;
			this.SetScrollPosition(1f);
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00046A75 File Offset: 0x00044C75
		public float GetSpaceAvailable()
		{
			return this.capacity - this.spaceUsed;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00046A84 File Offset: 0x00044C84
		public bool IsFull(float requiredSpace = 1f)
		{
			return this.spaceUsed + requiredSpace > this.capacity;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00046A96 File Offset: 0x00044C96
		public bool IsEmpty()
		{
			return this.spaceUsed == 0f;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00046AA5 File Offset: 0x00044CA5
		public float GetAvailableSpacePercentage()
		{
			return this.spaceUsed / this.capacity;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00046AB4 File Offset: 0x00044CB4
		public Inventory.InventoryItem GetCheapestMaterial(float volumeRequired, out int amount)
		{
			Inventory.InventoryItem inventoryItem = null;
			int num = 0;
			amount = 0;
			foreach (Inventory.InventoryItem inventoryItem2 in this.items)
			{
				if (inventoryItem2.item.canSell && inventoryItem2.item.sellValue > 0 && (inventoryItem == null || inventoryItem2.item.sellValue < num) && volumeRequired <= inventoryItem2.spaceRequired && GamePlayer.current.RequiredItemCountForMissions(inventoryItem2.item) < inventoryItem2.count)
				{
					inventoryItem = inventoryItem2;
					num = inventoryItem2.item.sellValue;
					amount = Mathf.Clamp((int)(volumeRequired / inventoryItem2.item.m3), 1, inventoryItem2.count - GamePlayer.current.RequiredItemCountForMissions(inventoryItem2.item));
				}
			}
			return inventoryItem;
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00046B90 File Offset: 0x00044D90
		public bool IsWarpFuelAvailable()
		{
			using (IEnumerator<WarpFuelItem> enumerator = this.GetAllWarpFuel().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					WarpFuelItem warpFuelItem = enumerator.Current;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x00046BE0 File Offset: 0x00044DE0
		public bool HasEnoughWarpFuel(int amount)
		{
			int num = 0;
			foreach (Inventory.InventoryItem inventoryItem in this.items)
			{
				WarpFuelItem warpFuelItem;
				if (inventoryItem.item.TryGetComponent<WarpFuelItem>(out warpFuelItem) && warpFuelItem.remaining >= 0f)
				{
					num += inventoryItem.count;
				}
				if (num >= amount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00046C5C File Offset: 0x00044E5C
		public IEnumerable<WarpFuelItem> GetAllWarpFuel()
		{
			foreach (Inventory.InventoryItem inventoryItem in this.items)
			{
				WarpFuelItem warpFuelItem;
				if (((inventoryItem != null) ? inventoryItem.item : null) && inventoryItem.item.TryGetComponent<WarpFuelItem>(out warpFuelItem) && warpFuelItem.remaining > 0f)
				{
					yield return warpFuelItem;
				}
			}
			IEnumerator<Inventory.InventoryItem> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00046C6C File Offset: 0x00044E6C
		public List<Inventory.InventoryItem> GetItemsOfType(ItemCategory category)
		{
			List<Inventory.InventoryItem> list = new List<Inventory.InventoryItem>();
			foreach (Inventory.InventoryItem inventoryItem in this.items)
			{
				if (inventoryItem.item.itemCategory == category)
				{
					list.Add(inventoryItem);
				}
			}
			return list;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x00046CD0 File Offset: 0x00044ED0
		public float GetItemVolumeOfType(ItemCategory category)
		{
			float num = 0f;
			foreach (Inventory.InventoryItem inventoryItem in this.items)
			{
				if (inventoryItem.item.itemCategory == category)
				{
					num += inventoryItem.item.m3;
				}
			}
			return num;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00046D3C File Offset: 0x00044F3C
		public int GetFirstEmptyItemIndex()
		{
			for (int i = 0; i < this.visibleItems.Length; i++)
			{
				if (this.visibleItems[i] == null)
				{
					return i;
				}
			}
			return this.visibleItems.Length;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x00046D70 File Offset: 0x00044F70
		public int GetLastVisibleItemCount()
		{
			int num = 0;
			for (int i = 0; i < this.visibleItems.Length; i++)
			{
				if (this.visibleItems[i] != null)
				{
					num = i;
				}
			}
			return num + 1;
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00046DA1 File Offset: 0x00044FA1
		public Inventory.InventoryItem Get(int slot)
		{
			if (slot < this.visibleItems.Length)
			{
				return this.visibleItems[slot];
			}
			return null;
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00046DB8 File Offset: 0x00044FB8
		public virtual Inventory.InventoryItem Add(InventoryItemType item, int amount, bool buyback = false, bool stack = false)
		{
			int num = -1;
			for (int i = 0; i < this.allItems.Length; i++)
			{
				Inventory.InventoryItem inventoryItem = this.allItems[i];
				if (inventoryItem != null && inventoryItem.item.CanStackWith(item) && !stack)
				{
					this.allItems[i].InternalAdd(item, amount);
					return this.allItems[i];
				}
				if (num < 0 && this.allItems[i] == null)
				{
					num = i;
				}
			}
			if (num < 0)
			{
				num = this.allItems.Length;
				this.EnsureSize(this.allItems.Length + 1);
			}
			this.allItems[num] = new Inventory.InventoryItem(item, this, num, amount, buyback);
			return this.allItems[num];
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00046E59 File Offset: 0x00045059
		public void Set(int slot, InventoryItemType item, int count = 1, bool buyback = false)
		{
			this.EnsureSize(slot + 1);
			if (item == null || count == 0)
			{
				this.allItems[slot] = null;
				return;
			}
			this.allItems[slot] = new Inventory.InventoryItem(item, this, slot, count, buyback);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00046E90 File Offset: 0x00045090
		public int Remove(InventoryItemType item, int amount)
		{
			int num = 0;
			for (int i = 0; i < this.allItems.Length; i++)
			{
				Inventory.InventoryItem inventoryItem = this.allItems[i];
				if (((inventoryItem != null) ? inventoryItem.item : null) == item)
				{
					int num2 = Math.Min(amount, this.allItems[i].count);
					this.allItems[i].InternalRemove(num2);
					if (this.allItems[i].count == 0)
					{
						this.allItems[i] = null;
					}
					amount -= num2;
					num += num2;
					if (amount == 0)
					{
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00046F16 File Offset: 0x00045116
		public virtual bool Remove(Inventory.InventoryItem item, int count)
		{
			if (item.inventory == this)
			{
				if (count == item.count)
				{
					this.allItems[item.slot] = null;
					return true;
				}
				if (count < item.count)
				{
					item.InternalAdd(item.item, -count);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00046F54 File Offset: 0x00045154
		public bool HasItem(InventoryItemType item, int count)
		{
			return this.GetCount(item) >= count;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x00046F64 File Offset: 0x00045164
		public int GetCount(InventoryItemType item)
		{
			int num = 0;
			for (int i = 0; i < this.allItems.Length; i++)
			{
				Inventory.InventoryItem inventoryItem = this.allItems[i];
				bool? flag;
				if (inventoryItem == null)
				{
					flag = null;
				}
				else
				{
					InventoryItemType item2 = inventoryItem.item;
					flag = ((item2 != null) ? new bool?(item2.CanStackWith(item)) : null);
				}
				bool? flag2 = flag;
				if (flag2.GetValueOrDefault())
				{
					num += this.allItems[i].count;
				}
			}
			return num;
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00046FD8 File Offset: 0x000451D8
		public Inventory.InventoryItem Get(InventoryItemType itemType)
		{
			for (int i = 0; i < this.allItems.Length; i++)
			{
				Inventory.InventoryItem inventoryItem = this.allItems[i];
				bool? flag;
				if (inventoryItem == null)
				{
					flag = null;
				}
				else
				{
					InventoryItemType item = inventoryItem.item;
					flag = ((item != null) ? new bool?(item.CanStackWith(itemType)) : null);
				}
				bool? flag2 = flag;
				if (flag2.GetValueOrDefault())
				{
					return this.allItems[i];
				}
			}
			return null;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00047044 File Offset: 0x00045244
		public Inventory.InventoryItem GetByExactType(InventoryItemType itemType)
		{
			return this.items.FirstOrDefault((Inventory.InventoryItem i) => i.item == itemType);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00047075 File Offset: 0x00045275
		public Inventory.InventoryItem FindItem(Func<Inventory.InventoryItem, bool> predicate)
		{
			return this.items.FirstOrDefault(predicate);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00047083 File Offset: 0x00045283
		public void Clear()
		{
			this.allItems = new Inventory.InventoryItem[16];
			this.visibleItems = new Inventory.InventoryItem[16];
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x000470A0 File Offset: 0x000452A0
		public void SwapItems(int slot1, int slot2)
		{
			this.EnsureSize(slot1 + 1);
			this.EnsureSize(slot2 + 1);
			Inventory.InventoryItem inventoryItem = this.allItems[slot1];
			Inventory.InventoryItem inventoryItem2 = this.allItems[slot2];
			if (inventoryItem != null && inventoryItem.item.CanStackWith((inventoryItem2 != null) ? inventoryItem2.item : null))
			{
				this.allItems[slot1] = null;
				inventoryItem2.InternalAdd(inventoryItem.item, inventoryItem.count);
				return;
			}
			if (inventoryItem != null)
			{
				inventoryItem.InternalUpdateSlot(slot2);
			}
			if (inventoryItem2 != null)
			{
				inventoryItem2.InternalUpdateSlot(slot1);
			}
			this.allItems[slot1] = inventoryItem2;
			this.allItems[slot2] = inventoryItem;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0004712F File Offset: 0x0004532F
		private void EnsureSize(int length)
		{
			while (this.allItems.Length < length)
			{
				Array.Resize<Inventory.InventoryItem>(ref this.allItems, this.allItems.Length + 16);
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00047154 File Offset: 0x00045354
		public void SetSearchFilter(string filter)
		{
			this.searchFilter = filter;
			this.SetScrollPosition(1f);
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x00047168 File Offset: 0x00045368
		public void UpdateVisibleItems()
		{
			this.visibleItems = new Inventory.InventoryItem[this.allItems.Length];
			if (string.IsNullOrEmpty(this.searchFilter))
			{
				this.allItems.CopyTo(this.visibleItems, 0);
				return;
			}
			int num = 0;
			for (int i = 0; i < this.allItems.Length; i++)
			{
				if (this.allItems[i] != null && this.allItems[i].MatchesSearchFilter(this.searchFilter))
				{
					this.visibleItems[num] = this.allItems[i];
					num++;
				}
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x000471F0 File Offset: 0x000453F0
		public object[] GetSellableItemParams()
		{
			int num = 0;
			int num2 = 0;
			foreach (Inventory.InventoryItem inventoryItem in this.visibleItems)
			{
				if (inventoryItem != null && inventoryItem.item && inventoryItem.item.canSell && inventoryItem.item.sellValue > 0 && !inventoryItem.item.favouriteItem)
				{
					num += inventoryItem.count;
					num2 += inventoryItem.item.sellValue * inventoryItem.count;
				}
			}
			return new object[]
			{
				num,
				GameMath.FormatNumber((float)num2, -1)
			};
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00047294 File Offset: 0x00045494
		public void SellVisibleItems()
		{
			foreach (Inventory.InventoryItem inventoryItem in this.visibleItems)
			{
				if (inventoryItem != null && inventoryItem.item && inventoryItem.item.canSell && inventoryItem.item.sellValue > 0 && !inventoryItem.item.favouriteItem)
				{
					InventoryInteractionManager.Instance.SellAmount(inventoryItem, inventoryItem.count, null);
				}
			}
			InventoryInteractionManager.Instance.ReloadUI();
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00047310 File Offset: 0x00045510
		public void SortByValue()
		{
			this.allItems = (from item in this.allItems
			where item != null
			orderby item.item.baseCost, item.item.name
			select item).ToArray<Inventory.InventoryItem>();
			this.SetScrollPosition(1f);
			this.SyncItemSlots();
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x000473AC File Offset: 0x000455AC
		public void SortByCategory()
		{
			this.allItems = (from item in this.allItems
			where item != null
			orderby item.item.itemCategory, item.item.gameplayType, item.item.name
			select item).ToArray<Inventory.InventoryItem>();
			this.SetScrollPosition(1f);
			this.SyncItemSlots();
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x0004746C File Offset: 0x0004566C
		private void SyncItemSlots()
		{
			for (int i = 0; i < this.allItems.Length; i++)
			{
				if (this.allItems[i] != null)
				{
					this.allItems[i].InternalUpdateSlot(i);
				}
			}
			this.UpdateVisibleItems();
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x000474AC File Offset: 0x000456AC
		public void SetExtraSearchCriteria()
		{
			foreach (Inventory.InventoryItem inventoryItem in this.allItems)
			{
				if ((inventoryItem != null) ? inventoryItem.item : null)
				{
					inventoryItem.item.UpdateSearchFieldContent();
				}
			}
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000474F0 File Offset: 0x000456F0
		public virtual JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (Inventory.InventoryItem inventoryItem in this.allItems)
			{
				if (inventoryItem != null)
				{
					jsonArray.Add(inventoryItem.ToJson());
				}
				else
				{
					jsonArray.Add(JsonValue.Null);
				}
			}
			while (jsonArray.Count > 0 && jsonArray[jsonArray.Count - 1] == JsonValue.Null)
			{
				jsonArray.Remove(jsonArray.Count - 1);
			}
			return new JsonObject
			{
				{
					"capacity",
					new double?((double)this.capacity)
				},
				{
					"items",
					jsonArray
				}
			};
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x000475A8 File Offset: 0x000457A8
		protected virtual void DataFromJson(JsonObject json)
		{
			this.capacity = (float)json["capacity"].AsNumber;
			JsonArray jsonArray = json["items"];
			this.allItems = new Inventory.InventoryItem[jsonArray.Count + 16];
			this.visibleItems = new Inventory.InventoryItem[jsonArray.Count + 16];
			for (int i = 0; i < jsonArray.Count; i++)
			{
				if (!jsonArray[i].IsNull)
				{
					this.allItems[i] = Inventory.InventoryItem.FromJson(jsonArray[i], this, i);
				}
			}
			this.allItems.CopyTo(this.visibleItems, 0);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00047657 File Offset: 0x00045857
		public static Inventory FromJson(JsonObject json, SpaceStation shopParent = null, bool isMaterial = false)
		{
			ShopInventory shopInventory = (shopParent != null) ? new ShopInventory(shopParent) : new Inventory(isMaterial);
			shopInventory.DataFromJson(json);
			return shopInventory;
		}

		// Token: 0x040004F4 RID: 1268
		private const int DefaultLength = 16;

		// Token: 0x040004F5 RID: 1269
		private Inventory.InventoryItem[] allItems = new Inventory.InventoryItem[16];

		// Token: 0x040004F6 RID: 1270
		private Inventory.InventoryItem[] visibleItems = new Inventory.InventoryItem[16];

		// Token: 0x040004F9 RID: 1273
		public readonly bool isMaterialStorage;

		// Token: 0x040004FA RID: 1274
		private float _capacity;

		// Token: 0x040004FB RID: 1275
		private const float GlobalCapacity = 100000f;

		// Token: 0x040004FC RID: 1276
		private const float NormalMaterialCapacity = 20000f;

		// Token: 0x040004FD RID: 1277
		private const float EnhancedMaterialCapacity = 50000f;

		// Token: 0x040004FE RID: 1278
		private const float ExpertMaterialCapacity = 100000f;

		// Token: 0x02000480 RID: 1152
		public class InventoryItem : IJsonSource
		{
			// Token: 0x170005F9 RID: 1529
			// (get) Token: 0x0600283D RID: 10301 RVA: 0x000E41F7 File Offset: 0x000E23F7
			// (set) Token: 0x0600283E RID: 10302 RVA: 0x000E41FF File Offset: 0x000E23FF
			public int count { get; private set; }

			// Token: 0x170005FA RID: 1530
			// (get) Token: 0x0600283F RID: 10303 RVA: 0x000E4208 File Offset: 0x000E2408
			// (set) Token: 0x06002840 RID: 10304 RVA: 0x000E4210 File Offset: 0x000E2410
			public int slot { get; private set; }

			// Token: 0x170005FB RID: 1531
			// (get) Token: 0x06002841 RID: 10305 RVA: 0x000E4219 File Offset: 0x000E2419
			public int cost
			{
				get
				{
					if (!this.canBuyback)
					{
						return this.item.cost;
					}
					return this.item.sellValue;
				}
			}

			// Token: 0x170005FC RID: 1532
			// (get) Token: 0x06002842 RID: 10306 RVA: 0x000E423A File Offset: 0x000E243A
			public int costItemCount
			{
				get
				{
					int num = this.costCount;
					return Mathf.RoundToInt((float)this.costCount * this.item.discount);
				}
			}

			// Token: 0x170005FD RID: 1533
			// (get) Token: 0x06002843 RID: 10307 RVA: 0x000E425B File Offset: 0x000E245B
			public float spaceRequired
			{
				get
				{
					return (float)this.count * this.item.m3;
				}
			}

			// Token: 0x06002844 RID: 10308 RVA: 0x000E4270 File Offset: 0x000E2470
			public InventoryItem(InventoryItemType item, Inventory parent, int slot, int count = 1, bool buyback = false)
			{
				this.item = item;
				this.inventory = parent;
				this.slot = slot;
				this.count = count;
				this.canBuyback = buyback;
			}

			// Token: 0x06002845 RID: 10309 RVA: 0x000E429D File Offset: 0x000E249D
			public void SpecialAddCount(int count)
			{
				this.count += count;
			}

			// Token: 0x06002846 RID: 10310 RVA: 0x000E42AD File Offset: 0x000E24AD
			public void InternalAdd(InventoryItemType type, int count)
			{
				this.count = this.item.AddStackToSelf(this.count, type, count);
			}

			// Token: 0x06002847 RID: 10311 RVA: 0x000E42C8 File Offset: 0x000E24C8
			public void InternalUpdateSlot(int newSlot)
			{
				this.slot = newSlot;
			}

			// Token: 0x06002848 RID: 10312 RVA: 0x000E42D1 File Offset: 0x000E24D1
			public bool InternalRemove(int count)
			{
				if (this.count >= count)
				{
					this.count -= count;
					return true;
				}
				return false;
			}

			// Token: 0x06002849 RID: 10313 RVA: 0x000E42F0 File Offset: 0x000E24F0
			public bool MatchesSearchFilter(string filter)
			{
				if (string.IsNullOrEmpty(filter))
				{
					return true;
				}
				string[] array = filter.Split(Array.Empty<char>());
				bool flag = true;
				foreach (string text in array)
				{
					bool flag2;
					if (text.StartsWith("<") || text.StartsWith(">") || text.StartsWith("="))
					{
						flag2 = this.ItemMatchesLevelFilter(text);
					}
					else
					{
						flag2 = this.ItemMatchesFilter(text);
					}
					flag = (flag && flag2);
				}
				return flag;
			}

			// Token: 0x0600284A RID: 10314 RVA: 0x000E4368 File Offset: 0x000E2568
			private bool ItemMatchesLevelFilter(string filter)
			{
				try
				{
					int num = int.Parse(filter.Substring(1, filter.Length - 1));
					if (filter.StartsWith("<"))
					{
						return this.item.itemLevel < num;
					}
					if (filter.StartsWith(">"))
					{
						return this.item.itemLevel > num;
					}
					if (filter.StartsWith("="))
					{
						return this.item.itemLevel == num;
					}
				}
				catch (FormatException)
				{
					return false;
				}
				return false;
			}

			// Token: 0x0600284B RID: 10315 RVA: 0x000E4400 File Offset: 0x000E2600
			private bool ItemMatchesFilter(string filter)
			{
				return this.item.searchField != null && this.item.searchField.Contains(filter, StringComparison.OrdinalIgnoreCase) && (!(filter == "aspect") || !this.item.searchField.Contains("noaspect", StringComparison.OrdinalIgnoreCase));
			}

			// Token: 0x0600284C RID: 10316 RVA: 0x000E445C File Offset: 0x000E265C
			public JsonValue ToJson()
			{
				JsonObject jsonObject = new JsonObject
				{
					{
						"item",
						this.item.ToJson()
					},
					{
						"count",
						new double?((double)this.count)
					}
				};
				if (this.canBuyback)
				{
					jsonObject["canBuyback"] = new bool?(true);
				}
				if (this.costItem)
				{
					jsonObject["costItem"] = this.costItem.identifier;
					jsonObject["costCount"] = new double?((double)this.costCount);
				}
				return jsonObject;
			}

			// Token: 0x0600284D RID: 10317 RVA: 0x000E450C File Offset: 0x000E270C
			public static Inventory.InventoryItem FromJson(JsonObject data, Inventory parent, int slot)
			{
				Inventory.InventoryItem inventoryItem = new Inventory.InventoryItem(InventoryItemType.FromJson(data["item"]), parent, slot, data["count"], data["canBuyback"]);
				if (data.ContainsKey("costItem"))
				{
					inventoryItem.costItem = InventoryItemType.Get(data["costItem"]);
					inventoryItem.costCount = data["costCount"];
				}
				return inventoryItem;
			}

			// Token: 0x040018FC RID: 6396
			public readonly InventoryItemType item;

			// Token: 0x040018FD RID: 6397
			public readonly Inventory inventory;

			// Token: 0x04001900 RID: 6400
			public bool canBuyback;

			// Token: 0x04001901 RID: 6401
			public InventoryItemType costItem;

			// Token: 0x04001902 RID: 6402
			public int costCount;
		}
	}
}
