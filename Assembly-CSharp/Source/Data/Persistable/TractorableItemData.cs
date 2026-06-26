using System;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Item;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x0200011E RID: 286
	public class TractorableItemData : PersistableData
	{
		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x00051816 File Offset: 0x0004FA16
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x0005181E File Offset: 0x0004FA1E
		public InventoryItemType itemType { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00051827 File Offset: 0x0004FA27
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x0005182F File Offset: 0x0004FA2F
		public int itemAmount { get; set; } = 1;

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00051838 File Offset: 0x0004FA38
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x00051840 File Offset: 0x0004FA40
		public Vector2 impulse { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00051849 File Offset: 0x0004FA49
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x00051851 File Offset: 0x0004FA51
		public bool jettisoned { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x0005185A File Offset: 0x0004FA5A
		// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x00051862 File Offset: 0x0004FA62
		public Faction ownerFaction { get; set; }

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0005186B File Offset: 0x0004FA6B
		public override bool ShouldCleanUp()
		{
			return this.itemType.rarity < Rarity.Exotic;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0005187B File Offset: 0x0004FA7B
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			return Singleton<LootManager>.Instance.SpawnLootItem(this, parent.transform).gameObject;
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00051894 File Offset: 0x0004FA94
		public override void DataToJson(JsonObject json)
		{
			json["itemType"] = this.itemType.ToJson();
			json["jettisoned"] = new bool?(this.jettisoned);
			json["impulse"] = JsonUtil.Vector2ToJson(this.impulse);
			json["amount"] = new double?((double)this.itemAmount);
			if (this.ownerFaction != null)
			{
				json["ownerFaction"] = this.ownerFaction.identifier;
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0005192C File Offset: 0x0004FB2C
		public override void LoadFromJson(JsonObject json)
		{
			this.itemType = InventoryItemType.FromJson(json["itemType"]);
			this.jettisoned = json["jettisoned"].AsBoolean;
			this.impulse = JsonUtil.JsonObjectToVector2(json["impulse"]);
			this.itemAmount = json["amount"].AsInteger;
			if (json.ContainsKey("ownerFaction"))
			{
				this.ownerFaction = Faction.Get(json["ownerFaction"]);
			}
		}
	}
}
