using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.Spacestation.Trade
{
	// Token: 0x0200021B RID: 539
	public class TradeTerminalStats : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x00081614 File Offset: 0x0007F814
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.statsContainer.SetActive(true);
			this.statsContainer.transform.SetAsLastSibling();
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00081632 File Offset: 0x0007F832
		public void OnPointerExit(PointerEventData eventData)
		{
			this.statsContainer.SetActive(false);
		}

		// Token: 0x04000B92 RID: 2962
		[SerializeField]
		private GameObject statsContainer;
	}
}
