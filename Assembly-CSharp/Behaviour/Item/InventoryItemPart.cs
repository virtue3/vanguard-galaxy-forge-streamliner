using System;
using Source.Item;
using UnityEngine;

namespace Behaviour.Item
{
	// Token: 0x02000307 RID: 775
	public class InventoryItemPart : MonoBehaviour
	{
		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001CCE RID: 7374 RVA: 0x000AE398 File Offset: 0x000AC598
		// (set) Token: 0x06001CCF RID: 7375 RVA: 0x000AE3A0 File Offset: 0x000AC5A0
		public InventoryItemType item { get; private set; }

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000AE3A9 File Offset: 0x000AC5A9
		public void SetItem(InventoryItemType item)
		{
			this.item = item;
			this.InitializeItem();
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000AE3B8 File Offset: 0x000AC5B8
		protected virtual void InitializeItem()
		{
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000AE3BA File Offset: 0x000AC5BA
		public virtual void OnPurchase(int amount)
		{
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000AE3BC File Offset: 0x000AC5BC
		public virtual int GetDynamicValue()
		{
			return 0;
		}

		// Token: 0x06001CD4 RID: 7380 RVA: 0x000AE3BF File Offset: 0x000AC5BF
		public virtual bool CanStackWith(InventoryItemType other)
		{
			return false;
		}

		// Token: 0x06001CD5 RID: 7381 RVA: 0x000AE3C2 File Offset: 0x000AC5C2
		public virtual int AddStackToSelf(int ownCount, InventoryItemType other, int otherCount)
		{
			return ownCount + otherCount;
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000AE3C7 File Offset: 0x000AC5C7
		public virtual bool SplitStack(Inventory inventory, int slot1, int amount1)
		{
			return false;
		}
	}
}
