using System;
using System.Collections.Generic;
using Behaviour.Item;
using LightJson;
using Source.Galaxy.POI;
using UnityEngine;

namespace Source.Item
{
	// Token: 0x020000F9 RID: 249
	public class ShopInventory : Inventory
	{
		// Token: 0x0600094A RID: 2378 RVA: 0x000479FA File Offset: 0x00045BFA
		public ShopInventory(SpaceStation parent) : base(false)
		{
			this.parent = parent;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00047A0A File Offset: 0x00045C0A
		public void AddPermanentItem(InventoryItemType item, int count = 1)
		{
			if (this.permanentItems == null)
			{
				this.permanentItems = new List<Inventory.InventoryItem>();
			}
			this.permanentItems.Add(new Inventory.InventoryItem(item, this, -1, count, false));
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00047A34 File Offset: 0x00045C34
		public void AddToShop(InventoryItemType item, int amount = 1, float amountValue = 0f)
		{
			if (amount <= 0)
			{
				amount = Mathf.CeilToInt(amountValue / (float)item.cost);
			}
			if (item.ShowItemInShop(this.parent.level))
			{
				this.Add(item, amount, false, false);
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00047A68 File Offset: 0x00045C68
		public override bool Remove(Inventory.InventoryItem item, int count)
		{
			if (this.permanentItems != null)
			{
				for (int i = 0; i < this.permanentItems.Count; i++)
				{
					Inventory.InventoryItem inventoryItem = this.permanentItems[i];
					if (inventoryItem.item == item.item && inventoryItem.InternalRemove(count) && inventoryItem.count <= 0)
					{
						this.permanentItems.RemoveAt(i);
						break;
					}
				}
			}
			return base.Remove(item, count);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00047ADC File Offset: 0x00045CDC
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			if (this.permanentItems != null)
			{
				result["permanentItems"] = this.permanentItems.ToJsonArray<Inventory.InventoryItem>();
			}
			return result;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00047B18 File Offset: 0x00045D18
		protected override void DataFromJson(JsonObject json)
		{
			base.DataFromJson(json);
			if (json["permanentItems"].IsJsonArray)
			{
				this.permanentItems = new List<Inventory.InventoryItem>();
				this.permanentItems.FromJsonArray(json["permanentItems"], (JsonValue data) => Inventory.InventoryItem.FromJson(data, this, -1));
			}
		}

		// Token: 0x04000521 RID: 1313
		public SpaceStation parent;

		// Token: 0x04000522 RID: 1314
		public SpaceStationFacility facility;

		// Token: 0x04000523 RID: 1315
		public List<Inventory.InventoryItem> permanentItems;
	}
}
