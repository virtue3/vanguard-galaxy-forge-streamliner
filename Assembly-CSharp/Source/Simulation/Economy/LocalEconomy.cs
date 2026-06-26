using System;
using System.Collections.Generic;
using Behaviour.Item;
using LightJson;
using Source.Galaxy.POI;
using UnityEngine;

namespace Source.Simulation.Economy
{
	// Token: 0x0200008D RID: 141
	public class LocalEconomy : IJsonSource
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x00030D56 File Offset: 0x0002EF56
		public int craftingDealValue
		{
			get
			{
				return Mathf.RoundToInt((float)this.craftingDealItem.sellValue * this.craftingDealMultiplier);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00030D70 File Offset: 0x0002EF70
		public IEnumerable<LocalEconomyItem> allItems
		{
			get
			{
				return this.economyItems.Values;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00030D7D File Offset: 0x0002EF7D
		public int itemCount
		{
			get
			{
				return this.economyItems.Count;
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00030D8A File Offset: 0x0002EF8A
		public LocalEconomy(SpaceStation spaceStation)
		{
			this.spaceStation = spaceStation;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00030DA4 File Offset: 0x0002EFA4
		public LocalEconomyItem GetEconomy(InventoryItemType type)
		{
			LocalEconomyItem result;
			if (this.economyItems.TryGetValue(type, out result))
			{
				return result;
			}
			LocalEconomyItem localEconomyItem = new LocalEconomyItem(type);
			this.economyItems[type] = localEconomyItem;
			return localEconomyItem;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00030DD8 File Offset: 0x0002EFD8
		public void ClearItems()
		{
			this.economyItems.Clear();
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x00030DE8 File Offset: 0x0002EFE8
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<InventoryItemType, LocalEconomyItem> keyValuePair in this.economyItems)
			{
				jsonObject[keyValuePair.Key.identifier] = keyValuePair.Value.ToJson();
			}
			if (this.craftingDealItem != null)
			{
				jsonObject["craftingDeal"] = new JsonObject
				{
					{
						"item",
						this.craftingDealItem.identifier
					},
					{
						"count",
						new double?((double)this.craftingDealCount)
					},
					{
						"cooldown",
						new double?((double)this.craftingDealCooldown)
					},
					{
						"multiplier",
						new double?((double)this.craftingDealMultiplier)
					}
				};
			}
			return jsonObject;
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00030EF8 File Offset: 0x0002F0F8
		public static LocalEconomy FromJson(JsonObject data, SpaceStation ss)
		{
			LocalEconomy localEconomy = new LocalEconomy(ss);
			foreach (KeyValuePair<string, JsonValue> keyValuePair in data)
			{
				if (keyValuePair.Key == "craftingDeal")
				{
					localEconomy.craftingDealItem = keyValuePair.Value["item"].AsString;
					localEconomy.craftingDealCount = keyValuePair.Value["count"];
					localEconomy.craftingDealCooldown = keyValuePair.Value["cooldown"];
					localEconomy.craftingDealMultiplier = (float)keyValuePair.Value["multiplier"].AsNumber;
				}
				else
				{
					InventoryItemType inventoryItemType = keyValuePair.Key;
					localEconomy.economyItems.Add(inventoryItemType, LocalEconomyItem.FromJson(inventoryItemType, keyValuePair.Value));
				}
			}
			return localEconomy;
		}

		// Token: 0x040002AA RID: 682
		public readonly SpaceStation spaceStation;

		// Token: 0x040002AB RID: 683
		private Dictionary<InventoryItemType, LocalEconomyItem> economyItems = new Dictionary<InventoryItemType, LocalEconomyItem>();

		// Token: 0x040002AC RID: 684
		public InventoryItemType craftingDealItem;

		// Token: 0x040002AD RID: 685
		public int craftingDealCount;

		// Token: 0x040002AE RID: 686
		public int craftingDealCooldown;

		// Token: 0x040002AF RID: 687
		public float craftingDealMultiplier;
	}
}
