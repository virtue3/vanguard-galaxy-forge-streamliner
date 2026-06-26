using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.Loading
{
	// Token: 0x020002C7 RID: 711
	public class LoadingBar : MonoBehaviour
	{
		// Token: 0x060019FC RID: 6652 RVA: 0x000A1ECC File Offset: 0x000A00CC
		private void Start()
		{
			if (this.loadingBarImage != null)
			{
				this.loadingBarImage.fillAmount = 0f;
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x000A1EEC File Offset: 0x000A00EC
		public IEnumerator StartFilling()
		{
			if (this.loadingBarImage != null)
			{
				this.loadingBarImage.fillAmount = 0f;
				yield return base.StartCoroutine(this.FillBar());
			}
			yield break;
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x000A1EFB File Offset: 0x000A00FB
		private IEnumerator FillBar()
		{
			this.fillSpeed = 10f;
			while (this.loadingBarImage.fillAmount < 1f)
			{
				this.loadingBarImage.fillAmount += this.fillSpeed * Time.deltaTime;
				yield return null;
			}
			this.loadingBarImage.fillAmount = 1f;
			yield break;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x000A1F0A File Offset: 0x000A010A
		public void ResetLoadingBar()
		{
			base.StopAllCoroutines();
			this.loadingBarImage.fillAmount = 0f;
		}

		// Token: 0x04001058 RID: 4184
		[SerializeField]
		private Image loadingBarImage;

		// Token: 0x04001059 RID: 4185
		private float fillSpeed;
	}
}
