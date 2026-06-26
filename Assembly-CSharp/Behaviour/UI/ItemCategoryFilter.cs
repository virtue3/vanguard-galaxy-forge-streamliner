using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.UI.Spacestation.Shops;
using Source.Item;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F3 RID: 499
	public class ItemCategoryFilter : MonoBehaviour
	{
		// Token: 0x060012D6 RID: 4822 RVA: 0x0007B1C0 File Offset: 0x000793C0
		private void Start()
		{
			List<ItemCategory> list = new List<ItemCategory>();
			this.CreateFilterButton(Resources.Load<Sprite>("Sprites/UI/empty"), ItemCategory.Empty);
			foreach (InventoryItemType inventoryItemType in InventoryItemType.all)
			{
				if (!list.Contains(inventoryItemType.itemCategory))
				{
					list.Add(inventoryItemType.itemCategory);
					this.CreateFilterButton(inventoryItemType.icon, inventoryItemType.itemCategory);
				}
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0007B248 File Offset: 0x00079448
		private void CreateFilterButton(Sprite icon, ItemCategory category)
		{
			FilterButton filterButton = UnityEngine.Object.Instantiate<FilterButton>(this.itemCategoryButton, base.transform);
			filterButton.GetComponent<Image>().sprite = icon;
			filterButton.category = category;
			filterButton.inventoryShop = this.inventoryShop;
		}

		// Token: 0x04000A97 RID: 2711
		[SerializeField]
		private FilterButton itemCategoryButton;

		// Token: 0x04000A98 RID: 2712
		[FormerlySerializedAs("allShop")]
		[SerializeField]
		private InventoryShop inventoryShop;
	}
}
