using System;
using Behaviour.Item.Usable;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D9 RID: 473
	public class ItemChargeIndicator : MonoBehaviour
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x00075893 File Offset: 0x00073A93
		// (set) Token: 0x060011C0 RID: 4544 RVA: 0x0007589B File Offset: 0x00073A9B
		public WarpFuelItem depletableItem { get; private set; }

		// Token: 0x060011C1 RID: 4545 RVA: 0x000758A4 File Offset: 0x00073AA4
		public void SetItem(WarpFuelItem item)
		{
			this.depletableItem = item;
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x000758B0 File Offset: 0x00073AB0
		public void UpdateFillAmount()
		{
			float remaining = this.depletableItem.remaining;
			if (Mathf.Approximately(this.lastRemaining, remaining))
			{
				return;
			}
			this.lastRemaining = remaining;
			this.image.fillAmount = remaining;
		}

		// Token: 0x040009C7 RID: 2503
		[SerializeField]
		private Image image;

		// Token: 0x040009C8 RID: 2504
		private float lastRemaining = -1f;
	}
}
