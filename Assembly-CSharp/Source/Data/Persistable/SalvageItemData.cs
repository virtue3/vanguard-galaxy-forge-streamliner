using System;
using Behaviour.Item;
using LightJson;

namespace Source.Data.Persistable
{
	// Token: 0x0200011D RID: 285
	public class SalvageItemData : IJsonSource
	{
		// Token: 0x06000AEA RID: 2794 RVA: 0x000516F7 File Offset: 0x0004F8F7
		public SalvageItemData(InventoryItemType item, float baseChance, bool alwaysAvailable = false)
		{
			this.item = item;
			this.baseChance = baseChance;
			this.criticalItem = alwaysAvailable;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00051714 File Offset: 0x0004F914
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"item",
					this.item.ToJson()
				},
				{
					"baseChance",
					new double?((double)this.baseChance)
				},
				{
					"active",
					new bool?(this.active)
				},
				{
					"criticalItem",
					new bool?(this.criticalItem)
				}
			};
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00051798 File Offset: 0x0004F998
		public static SalvageItemData FromJson(JsonValue data)
		{
			bool alwaysAvailable = data["criticalItem"].IsNull || data["criticalItem"].AsBoolean;
			return new SalvageItemData(InventoryItemType.FromJson(data["item"]), (float)data["baseChance"].AsNumber, alwaysAvailable)
			{
				active = data["active"]
			};
		}

		// Token: 0x040005D7 RID: 1495
		public InventoryItemType item;

		// Token: 0x040005D8 RID: 1496
		public float baseChance;

		// Token: 0x040005D9 RID: 1497
		public bool active;

		// Token: 0x040005DA RID: 1498
		public bool criticalItem;

		// Token: 0x040005DB RID: 1499
		public bool extracted;

		// Token: 0x040005DC RID: 1500
		public bool extractionSuccessful;
	}
}
