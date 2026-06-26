using System;
using UnityEngine;

namespace Behaviour.UI.Main_Menu
{
	// Token: 0x02000272 RID: 626
	public class LoadingScreenParent : MonoBehaviour
	{
		// Token: 0x060016FC RID: 5884 RVA: 0x000917C5 File Offset: 0x0008F9C5
		private void Awake()
		{
			LoadingScreenParent.instance = this;
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000917CD File Offset: 0x0008F9CD
		public void Show(string label)
		{
			this.Hide(true);
			this.current = UnityEngine.Object.Instantiate<LoadingScreen>(this.loadingScreenPrefab, base.transform);
			if (label != null)
			{
				this.current.label.TL(label, Array.Empty<object>());
			}
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00091806 File Offset: 0x0008FA06
		public void Hide(bool instant)
		{
			if (!this.current)
			{
				return;
			}
			if (instant || Time.timeScale == 0f)
			{
				UnityEngine.Object.Destroy(this.current.gameObject);
				return;
			}
			this.current.FadeOut();
		}

		// Token: 0x04000E21 RID: 3617
		public static LoadingScreenParent instance;

		// Token: 0x04000E22 RID: 3618
		[SerializeField]
		private LoadingScreen loadingScreenPrefab;

		// Token: 0x04000E23 RID: 3619
		private LoadingScreen current;
	}
}
