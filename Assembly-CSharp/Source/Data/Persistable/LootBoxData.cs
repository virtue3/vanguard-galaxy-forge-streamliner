using System;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Util;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000115 RID: 277
	public class LootBoxData : TractorableItemData
	{
		// Token: 0x06000A97 RID: 2711 RVA: 0x0004F301 File Offset: 0x0004D501
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			LootBox lootBox = UnityEngine.Object.Instantiate<LootBox>(Singleton<LootManager>.Instance.lootBoxPrefab, this.position, base.rotation, parent.transform);
			lootBox.Init(this);
			return lootBox.gameObject;
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0004F335 File Offset: 0x0004D535
		public void Init(InventoryItemType lootBoxItem)
		{
			base.itemType = lootBoxItem;
		}
	}
}
