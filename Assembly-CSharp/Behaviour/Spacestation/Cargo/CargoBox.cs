using System;
using System.Collections.Generic;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Source.Item;
using UnityEngine;

namespace Behaviour.Spacestation.Cargo
{
	// Token: 0x020002E8 RID: 744
	public class CargoBox : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06001B1F RID: 6943 RVA: 0x000A6062 File Offset: 0x000A4262
		// (set) Token: 0x06001B20 RID: 6944 RVA: 0x000A606A File Offset: 0x000A426A
		public int size { get; private set; } = 4;

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001B21 RID: 6945 RVA: 0x000A6073 File Offset: 0x000A4273
		// (set) Token: 0x06001B22 RID: 6946 RVA: 0x000A607B File Offset: 0x000A427B
		public CargoBox.CargoBoxColor color { get; private set; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000A6084 File Offset: 0x000A4284
		// (set) Token: 0x06001B24 RID: 6948 RVA: 0x000A608C File Offset: 0x000A428C
		public int gridWidth { get; private set; } = 1;

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000A6095 File Offset: 0x000A4295
		// (set) Token: 0x06001B26 RID: 6950 RVA: 0x000A609D File Offset: 0x000A429D
		public int gridHeight { get; private set; } = 1;

		// Token: 0x06001B27 RID: 6951 RVA: 0x000A60A6 File Offset: 0x000A42A6
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
		}

		// Token: 0x0400110C RID: 4364
		public List<Inventory.InventoryItem> items = new List<Inventory.InventoryItem>();

		// Token: 0x0200057A RID: 1402
		public enum CargoBoxColor
		{
			// Token: 0x04001C7F RID: 7295
			Grey,
			// Token: 0x04001C80 RID: 7296
			Red,
			// Token: 0x04001C81 RID: 7297
			Blue,
			// Token: 0x04001C82 RID: 7298
			Green,
			// Token: 0x04001C83 RID: 7299
			Teal,
			// Token: 0x04001C84 RID: 7300
			Brown
		}
	}
}
