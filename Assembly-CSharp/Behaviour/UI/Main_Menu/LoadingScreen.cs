using System;
using System.Collections;
using Behaviour.Transparency;
using Source.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main_Menu
{
	// Token: 0x02000271 RID: 625
	public class LoadingScreen : MonoBehaviour
	{
		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0009173A File Offset: 0x0008F93A
		// (set) Token: 0x060016F5 RID: 5877 RVA: 0x00091742 File Offset: 0x0008F942
		public TMP_Text label { get; private set; }

		// Token: 0x060016F6 RID: 5878 RVA: 0x0009174C File Offset: 0x0008F94C
		private void Awake()
		{
			string transparencyMode = GameplayerPrefs.GetTransparencyMode();
			if (transparencyMode == "Transparent" || transparencyMode == "Performant")
			{
				base.gameObject.AddComponent<WindowMinimumY>();
			}
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00091785 File Offset: 0x0008F985
		public void FadeOut()
		{
			base.StartCoroutine(this.FadeOutCoroutine());
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00091794 File Offset: 0x0008F994
		private IEnumerator FadeOutCoroutine()
		{
			this.background.raycastTarget = false;
			float time = 0.5f;
			while (time > 0f)
			{
				time -= Time.unscaledDeltaTime;
				this.group.alpha = time * 2f;
				yield return null;
			}
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x000917A3 File Offset: 0x0008F9A3
		public static void Show(string label = null)
		{
			LoadingScreenParent.instance.Show(label);
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x000917B0 File Offset: 0x0008F9B0
		public static void Hide(bool instant = false)
		{
			LoadingScreenParent.instance.Hide(instant);
		}

		// Token: 0x04000E1E RID: 3614
		[SerializeField]
		private Image background;

		// Token: 0x04000E20 RID: 3616
		[SerializeField]
		private CanvasGroup group;
	}
}
