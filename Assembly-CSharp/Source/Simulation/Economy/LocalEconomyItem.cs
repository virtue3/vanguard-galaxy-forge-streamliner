using System;
using Behaviour.Item;
using LightJson;
using UnityEngine;

namespace Source.Simulation.Economy
{
	// Token: 0x0200008E RID: 142
	public class LocalEconomyItem : IJsonSource
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x00031018 File Offset: 0x0002F218
		public int cost
		{
			get
			{
				return this.GetCost(this.currentValue);
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00031026 File Offset: 0x0002F226
		public LocalEconomyItem(InventoryItemType iit)
		{
			this.item = iit;
			Array.Fill<float>(this.historicalValue, 1f);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00031060 File Offset: 0x0002F260
		public void CycleHistory()
		{
			for (int i = this.historicalValue.Length - 1; i > 0; i--)
			{
				this.historicalValue[i] = this.historicalValue[i - 1];
			}
			this.historicalValue[0] = this.currentValue;
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x000310A2 File Offset: 0x0002F2A2
		public int GetCost(float val)
		{
			return Mathf.RoundToInt(val * (float)this.item.cost);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x000310B8 File Offset: 0x0002F2B8
		public JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			for (int i = 0; i < this.historicalValue.Length; i++)
			{
				jsonArray.Add(new double?((double)this.historicalValue[i]));
			}
			return new JsonObject
			{
				{
					"historicalValue",
					jsonArray
				},
				{
					"currentValue",
					new double?((double)this.currentValue)
				},
				{
					"supplyModifier",
					new double?((double)this.supplyModifier)
				},
				{
					"currentSupply",
					new double?((double)this.currentSupply)
				}
			};
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0003116C File Offset: 0x0002F36C
		public static LocalEconomyItem FromJson(InventoryItemType item, JsonObject data)
		{
			LocalEconomyItem localEconomyItem = new LocalEconomyItem(item)
			{
				currentValue = (float)data["currentValue"].AsNumber,
				supplyModifier = (float)data["supplyModifier"].AsNumber,
				currentSupply = data["currentSupply"]
			};
			JsonValue jsonValue = data["historicalValue"];
			if (jsonValue.IsJsonArray)
			{
				for (int i = 0; i < localEconomyItem.historicalValue.Length; i++)
				{
					localEconomyItem.historicalValue[i] = (float)jsonValue[i].AsNumber;
				}
			}
			return localEconomyItem;
		}

		// Token: 0x040002B0 RID: 688
		public readonly InventoryItemType item;

		// Token: 0x040002B1 RID: 689
		public float[] historicalValue = new float[20];

		// Token: 0x040002B2 RID: 690
		public float supplyModifier;

		// Token: 0x040002B3 RID: 691
		public float currentValue = 1f;

		// Token: 0x040002B4 RID: 692
		public int currentSupply;
	}
}
