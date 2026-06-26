using System;
using Behaviour.UI.Spacestation.Shops;
using Behaviour.UI.Tooltip;
using Source.Item;
using UnityEngine;
using UnityEngine.Serialization;

namespace Behaviour.UI
{
	// Token: 0x020001F2 RID: 498
	public class FilterButton : MonoBehaviour, ITooltipTextSource
	{
		// Token: 0x060012D3 RID: 4819 RVA: 0x0007B195 File Offset: 0x00079395
		public void OnSubmit()
		{
			this.inventoryShop.FilterUI(this.category);
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0007B1A8 File Offset: 0x000793A8
		public string GetTooltipText()
		{
			return this.category.GetDisplayName();
		}

		// Token: 0x04000A95 RID: 2709
		public ItemCategory category;

		// Token: 0x04000A96 RID: 2710
		[FormerlySerializedAs("allShop")]
		public InventoryShop inventoryShop;
	}
}
