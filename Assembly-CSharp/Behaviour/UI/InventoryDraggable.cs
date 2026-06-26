using System;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D8 RID: 472
	public class InventoryDraggable : MonoBehaviour
	{
		// Token: 0x17000301 RID: 769
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00075811 File Offset: 0x00073A11
		// (set) Token: 0x060011BB RID: 4539 RVA: 0x00075819 File Offset: 0x00073A19
		public RectTransform rectTransform { get; private set; }

		// Token: 0x060011BC RID: 4540 RVA: 0x00075822 File Offset: 0x00073A22
		private void Awake()
		{
			this.rectTransform = (base.transform as RectTransform);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00075838 File Offset: 0x00073A38
		public void SetItem(Sprite icon, int amount)
		{
			this.itemIcon.sprite = icon;
			if (amount > 1)
			{
				this.countText.text = GameMath.FormatNumber((float)amount, -1);
				this.countText.gameObject.SetActive(true);
				return;
			}
			this.countText.gameObject.SetActive(false);
		}

		// Token: 0x040009C4 RID: 2500
		public Image itemIcon;

		// Token: 0x040009C5 RID: 2501
		[SerializeField]
		private TMP_Text countText;
	}
}
