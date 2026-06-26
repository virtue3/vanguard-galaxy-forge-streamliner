using System;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001DA RID: 474
	public class ItemSlotActive : MonoBehaviour
	{
		// Token: 0x060011C4 RID: 4548 RVA: 0x000758FE File Offset: 0x00073AFE
		public void ToggleActive(bool toggle)
		{
			this.image.color = (toggle ? ColorHelper.greenish : ColorHelper.reddish);
		}

		// Token: 0x040009CA RID: 2506
		[SerializeField]
		private Image image;
	}
}
