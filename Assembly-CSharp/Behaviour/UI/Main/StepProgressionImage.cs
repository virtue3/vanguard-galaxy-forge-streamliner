using System;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000266 RID: 614
	public class StepProgressionImage : MonoBehaviour
	{
		// Token: 0x0600168A RID: 5770 RVA: 0x0008F146 File Offset: 0x0008D346
		private void Awake()
		{
			this.defaultColor = this.image.color;
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0008F159 File Offset: 0x0008D359
		public void SetCompleted()
		{
			this.image.color = this.completedColor;
			this.checkmark.gameObject.SetActive(true);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0008F17D File Offset: 0x0008D37D
		public void SetCurrent()
		{
			this.image.color = this.currentColor;
			this.checkmark.gameObject.SetActive(false);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0008F1A1 File Offset: 0x0008D3A1
		public void SetIncomplete()
		{
			this.image.color = this.defaultColor;
			this.checkmark.gameObject.SetActive(false);
		}

		// Token: 0x04000DC1 RID: 3521
		[SerializeField]
		private Image image;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private Image checkmark;

		// Token: 0x04000DC3 RID: 3523
		[SerializeField]
		private Color currentColor;

		// Token: 0x04000DC4 RID: 3524
		[SerializeField]
		private Color completedColor;

		// Token: 0x04000DC5 RID: 3525
		private Color defaultColor;

		// Token: 0x04000DC6 RID: 3526
		public int step;
	}
}
