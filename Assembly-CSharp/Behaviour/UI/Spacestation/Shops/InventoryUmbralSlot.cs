using System;
using Source.Util;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Shops
{
	// Token: 0x0200021D RID: 541
	public class InventoryUmbralSlot : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x0600141A RID: 5146 RVA: 0x00081ADA File Offset: 0x0007FCDA
		public void OnPointerClick(PointerEventData eventData)
		{
			base.GetComponentInParent<InventoryShop>().ToggleUmbralInventory();
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x00081AE7 File Offset: 0x0007FCE7
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.border.color = ColorHelper.white75;
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00081AF9 File Offset: 0x0007FCF9
		public void OnPointerExit(PointerEventData eventData)
		{
			this.border.color = ColorHelper.middleBlue;
		}

		// Token: 0x04000B9F RID: 2975
		[SerializeField]
		private Image border;
	}
}
