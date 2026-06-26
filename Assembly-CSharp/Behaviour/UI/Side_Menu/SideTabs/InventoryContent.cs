using System;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B4 RID: 692
	public class InventoryContent : SideTabContent
	{
		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600199A RID: 6554 RVA: 0x0009F7AF File Offset: 0x0009D9AF
		// (set) Token: 0x0600199B RID: 6555 RVA: 0x0009F7B7 File Offset: 0x0009D9B7
		public InventoryType inventoryType { get; private set; }

		// Token: 0x0600199C RID: 6556 RVA: 0x0009F7C0 File Offset: 0x0009D9C0
		private void Start()
		{
			InventoryPanel inventoryPanel = UnityEngine.Object.Instantiate<InventoryPanel>(this.inventoryPanel, base.transform);
			Inventory inventory = null;
			switch (this.inventoryType)
			{
			case InventoryType.Stash:
				inventory = GamePlayer.current.globalInventory;
				break;
			case InventoryType.Cargo:
				inventory = GamePlayer.current.currentSpaceShip.cargo;
				break;
			case InventoryType.Materials:
				inventory = (GamePlayer.current.currentPointOfInterest as SpaceStation).materialStorage;
				break;
			}
			if (inventory != null)
			{
				inventory.SetExtraSearchCriteria();
			}
			inventoryPanel.SetInventory(inventory, this.inventoryType);
		}

		// Token: 0x0400100F RID: 4111
		[SerializeField]
		private InventoryPanel inventoryPanel;
	}
}
