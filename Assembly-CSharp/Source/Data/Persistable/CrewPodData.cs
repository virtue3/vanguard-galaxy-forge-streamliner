using System;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Util;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x02000113 RID: 275
	public class CrewPodData : TractorableItemData
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x0004F276 File Offset: 0x0004D476
		public override GameObject AddToWorld(BasePoiManager parent)
		{
			CrewPod crewPod = UnityEngine.Object.Instantiate<CrewPod>(Singleton<LootManager>.Instance.crewPodPrefab, this.position, base.rotation, parent.transform);
			crewPod.Init(this);
			return crewPod.gameObject;
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0004F2AA File Offset: 0x0004D4AA
		public void Init(InventoryItemType lootBoxItem)
		{
			base.itemType = lootBoxItem;
		}
	}
}
