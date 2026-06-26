using System;
using Behaviour.Item;
using Behaviour.UI.Tooltip;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Trade
{
	// Token: 0x02000218 RID: 536
	public class TradeTerminalCraftingDeal : MonoBehaviour
	{
		// Token: 0x060013F4 RID: 5108 RVA: 0x00081050 File Offset: 0x0007F250
		public void SetDeal(InventoryItemType item, int count, int buyValue)
		{
			this.icon.sprite = item.icon;
			this.label.TL(item.displayName, Array.Empty<object>());
			this.description.TL("@EconomyCraftingDealDesc", new object[]
			{
				Translation.Plural(item.displayName, 2)
			});
			this.price.TL("@EconomyCraftingDealPrice", new object[]
			{
				count,
				"$" + GameMath.FormatNumber((float)buyValue, -1)
			});
			int num = GamePlayer.current.CountAvailableItems(item);
			if (num > 0)
			{
				this.amount.TL("@TTItemsToSell", new object[]
				{
					num
				});
				this.amount.gameObject.SetActive(true);
				this.sellButton.gameObject.SetActive(true);
			}
			else
			{
				this.amount.gameObject.SetActive(false);
				this.sellButton.gameObject.SetActive(false);
			}
			this.tooltip.SetItem(item, 1, true, ItemTooltipContext.CraftingPreview, false, null);
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00081163 File Offset: 0x0007F363
		public void ButtonSell()
		{
			base.GetComponentInParent<TradeTerminal>().StartCraftingDeal();
		}

		// Token: 0x04000B7D RID: 2941
		[SerializeField]
		private Image icon;

		// Token: 0x04000B7E RID: 2942
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000B7F RID: 2943
		[SerializeField]
		private TMP_Text description;

		// Token: 0x04000B80 RID: 2944
		[SerializeField]
		private TMP_Text price;

		// Token: 0x04000B81 RID: 2945
		[SerializeField]
		private TMP_Text amount;

		// Token: 0x04000B82 RID: 2946
		[SerializeField]
		private Button sellButton;

		// Token: 0x04000B83 RID: 2947
		[SerializeField]
		private ItemTooltipSource tooltip;
	}
}
