using System;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Refinery
{
	// Token: 0x0200024D RID: 589
	public class RefineryResultBadge : MonoBehaviour
	{
		// Token: 0x060015CB RID: 5579 RVA: 0x0008B5C9 File Offset: 0x000897C9
		public void SetContent(RefinedMaterial mat, float amt)
		{
			this.icon.sprite = mat.GetIcon();
			this.label.text = mat.GetDisplayName() + " x" + GameMath.FormatNumber(amt, 2);
		}

		// Token: 0x04000D0E RID: 3342
		[SerializeField]
		private Image icon;

		// Token: 0x04000D0F RID: 3343
		[SerializeField]
		private TMP_Text label;
	}
}
