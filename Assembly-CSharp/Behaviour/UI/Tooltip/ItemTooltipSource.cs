using System;
using Behaviour.Item;
using Source.Item;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x02000204 RID: 516
	public class ItemTooltipSource : TooltipSource
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x0007E6C3 File Offset: 0x0007C8C3
		// (set) Token: 0x0600134A RID: 4938 RVA: 0x0007E6CB File Offset: 0x0007C8CB
		public InventoryItemType item { get; private set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600134B RID: 4939 RVA: 0x0007E6D4 File Offset: 0x0007C8D4
		// (set) Token: 0x0600134C RID: 4940 RVA: 0x0007E6DC File Offset: 0x0007C8DC
		public int count { get; private set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600134D RID: 4941 RVA: 0x0007E6E5 File Offset: 0x0007C8E5
		// (set) Token: 0x0600134E RID: 4942 RVA: 0x0007E6ED File Offset: 0x0007C8ED
		public bool allowCompare { get; private set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x0007E6F6 File Offset: 0x0007C8F6
		// (set) Token: 0x06001350 RID: 4944 RVA: 0x0007E6FE File Offset: 0x0007C8FE
		public bool allowBuyback { get; private set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x0007E707 File Offset: 0x0007C907
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x0007E70F File Offset: 0x0007C90F
		public ItemTooltipContext context { get; private set; }

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0007E718 File Offset: 0x0007C918
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x0007E720 File Offset: 0x0007C920
		public Inventory.InventoryItem inInventory { get; private set; }

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x0007E729 File Offset: 0x0007C929
		public override UITooltip Prefab
		{
			get
			{
				return UITooltipParent.ItemTooltipPrefab;
			}
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0007E730 File Offset: 0x0007C930
		public void SetItem(InventoryItemType item, int count, bool allowCompare, ItemTooltipContext context, bool allowBuyback = false, Inventory.InventoryItem inInventory = null)
		{
			this.item = item;
			this.count = count;
			this.allowCompare = allowCompare;
			this.context = context;
			this.allowBuyback = allowBuyback;
			this.inInventory = inInventory;
			base.enabled = (item != null);
		}
	}
}
