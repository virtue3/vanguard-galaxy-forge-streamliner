using System;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.UI.Tooltip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Refinery
{
	// Token: 0x0200024C RID: 588
	public class RefineryOreBadge : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0008B52E File Offset: 0x0008972E
		// (set) Token: 0x060015C6 RID: 5574 RVA: 0x0008B536 File Offset: 0x00089736
		public OreItemData ore { get; private set; }

		// Token: 0x060015C7 RID: 5575 RVA: 0x0008B540 File Offset: 0x00089740
		public void SetOre(OreItemData type, int count)
		{
			this.ore = type;
			this.icon.sprite = type.item.icon;
			this.count.text = GameMath.FormatNumber((float)count, -1);
			base.GetComponent<ItemTooltipSource>().SetItem(type.GetComponent<InventoryItemType>(), 1, false, ItemTooltipContext.InInventory, false, null);
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0008B593 File Offset: 0x00089793
		public void SetSelected(bool sel)
		{
			this.highlight.gameObject.SetActive(sel);
		}

		// Token: 0x060015C9 RID: 5577 RVA: 0x0008B5A6 File Offset: 0x000897A6
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				base.GetComponentInParent<RefineryJobTabContents>().SetSelectedOre(this.ore);
			}
		}

		// Token: 0x04000D0A RID: 3338
		[SerializeField]
		private Image icon;

		// Token: 0x04000D0B RID: 3339
		[SerializeField]
		private RectTransform highlight;

		// Token: 0x04000D0C RID: 3340
		[SerializeField]
		private TMP_Text count;
	}
}
