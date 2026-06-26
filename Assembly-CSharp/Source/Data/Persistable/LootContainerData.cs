using System;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Util;
using LightJson;
using Source.Item;
using Source.Mining;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000116 RID: 278
	public class LootContainerData : PersistableData
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0004F346 File Offset: 0x0004D546
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x0004F34E File Offset: 0x0004D54E
		public Dictionary<InventoryItemType, int> loot { get; private set; } = new Dictionary<InventoryItemType, int>();

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0004F357 File Offset: 0x0004D557
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x0004F35F File Offset: 0x0004D55F
		public int maxHealth { get; private set; } = 10;

		// Token: 0x06000A9E RID: 2718 RVA: 0x0004F368 File Offset: 0x0004D568
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			if (this.angle == 0f)
			{
				this.angle = (float)SeededRandom.Global.RandomRange(0, 360);
			}
			LootContainer lootContainer = UnityEngine.Object.Instantiate<LootContainer>(PersistentSingleton<GameManager>.Instance.lootContainerPrefab, this.position, base.rotation, parent.transform);
			lootContainer.InitObject(this);
			return lootContainer.gameObject;
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0004F3CC File Offset: 0x0004D5CC
		public void AddLoot(InventoryItemType item, int amount = 1)
		{
			if (this.loot.ContainsKey(item))
			{
				Dictionary<InventoryItemType, int> loot = this.loot;
				loot[item] += amount;
				return;
			}
			this.loot.Add(item, amount);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0004F410 File Offset: 0x0004D610
		public void AddRandomLoot(AsteroidFieldData asteroidFieldData)
		{
			if (SeededRandom.Global.RandomRange(0f, 1f) > 0.8f)
			{
				this.AddLoot(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f), 1);
			}
			if (SeededRandom.Global.RandomRange(0f, 1f) > 0.3f)
			{
				this.AddLoot(InventoryItemType.Get(asteroidFieldData.GetRandomOre(true, null).name), SeededRandom.Global.RandomRange(2, 5));
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0004F494 File Offset: 0x0004D694
		public override bool ShouldCleanUp()
		{
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.loot)
			{
				if (keyValuePair.Key.rarity >= Rarity.Exotic)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0004F4F8 File Offset: 0x0004D6F8
		public override void DataToJson(JsonObject json)
		{
			JsonArray jsonArray = new JsonArray();
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.loot)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"item",
						keyValuePair.Key.ToJson()
					},
					{
						"count",
						new double?((double)keyValuePair.Value)
					}
				});
			}
			json["boxContent"] = jsonArray;
			json["name"] = this.name;
			json["maxHealth"] = new double?((double)this.maxHealth);
			json["damageTaken"] = new double?((double)this.damageTaken);
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0004F5F0 File Offset: 0x0004D7F0
		public override void LoadFromJson(JsonObject json)
		{
			foreach (JsonValue jsonValue in json["boxContent"].AsJsonArray)
			{
				this.loot[InventoryItemType.FromJson(jsonValue["item"])] = jsonValue["count"];
			}
			this.name = json["name"];
			this.maxHealth = json["maxHealth"];
			this.damageTaken = json["damageTaken"];
		}

		// Token: 0x040005B2 RID: 1458
		public string name;

		// Token: 0x040005B5 RID: 1461
		public int damageTaken;
	}
}
