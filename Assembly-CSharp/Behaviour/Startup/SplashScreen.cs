using System;
using System.Collections;
using Behaviour.Bootstrap;
using Behaviour.Util;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.Startup
{
	// Token: 0x020002DB RID: 731
	public class SplashScreen : MonoBehaviour
	{
		// Token: 0x06001AAF RID: 6831 RVA: 0x000A5085 File Offset: 0x000A3285
		private void Start()
		{
			base.StartCoroutine(this.StartupSequence());
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x000A5094 File Offset: 0x000A3294
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				this.SkipSplash();
			}
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x000A50A8 File Offset: 0x000A32A8
		private IEnumerator StartupSequence()
		{
			yield return new WaitForSeconds(0.25f);
			yield return this.FadeIn();
			yield return new WaitForSeconds(this.displayTime);
			yield return this.FadeOut();
			yield return new WaitForSeconds(0.25f);
			PersistentSingleton<SceneLoader>.Instance.StartMenu();
			yield break;
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x000A50B7 File Offset: 0x000A32B7
		private IEnumerator FadeIn()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.deltaTime;
				float alpha = Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(alpha);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001AB3 RID: 6835 RVA: 0x000A50C6 File Offset: 0x000A32C6
		private IEnumerator FadeOut()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.deltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(alpha);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001AB4 RID: 6836 RVA: 0x000A50D5 File Offset: 0x000A32D5
		private void SetAlpha(float t)
		{
			this.BRGLogoText.color = this.BRGLogoText.color.WithAlpha(t);
			this.BRGLogoImage.color = this.BRGLogoImage.color.WithAlpha(t);
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x000A510F File Offset: 0x000A330F
		private void SetScale(float t)
		{
			this.BRGLogoImage.gameObject.transform.localScale = Vector3.Lerp(this.minScale, this.maxScale, t);
		}

		// Token: 0x06001AB6 RID: 6838 RVA: 0x000A5138 File Offset: 0x000A3338
		private void SkipSplash()
		{
			base.StopAllCoroutines();
			PersistentSingleton<SceneLoader>.Instance.StartMenu();
		}

		// Token: 0x040010D9 RID: 4313
		[SerializeField]
		private TextMeshProUGUI BRGLogoText;

		// Token: 0x040010DA RID: 4314
		[SerializeField]
		private Image BRGLogoImage;

		// Token: 0x040010DB RID: 4315
		private Vector3 minScale = new Vector3(0.95f, 0.95f, 0.95f);

		// Token: 0x040010DC RID: 4316
		private Vector3 maxScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040010DD RID: 4317
		private readonly float displayTime = 1f;

		// Token: 0x040010DE RID: 4318
		private readonly float fadeDuration = 0.3f;
	}
}
