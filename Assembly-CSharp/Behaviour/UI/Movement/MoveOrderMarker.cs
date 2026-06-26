using System;
using System.Collections;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Movement
{
	// Token: 0x02000257 RID: 599
	public class MoveOrderMarker : MonoBehaviour
	{
		// Token: 0x0600161D RID: 5661 RVA: 0x0008CC56 File Offset: 0x0008AE56
		public void Initialize(Vector3 worldPos, Camera cam)
		{
			this.targetWorldPosition = worldPos;
			this.mainCamera = cam;
			base.StartCoroutine(this.FadeSequence());
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x0008CC74 File Offset: 0x0008AE74
		private void Update()
		{
			if (this.mainCamera != null)
			{
				Vector2 v = this.mainCamera.WorldToScreenPoint(this.targetWorldPosition);
				base.transform.position = v;
			}
			float z = Mathf.Sin(Time.time * this.oscillationSpeed) * this.rotationAmount;
			base.transform.localRotation = Quaternion.Euler(0f, 0f, z);
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0008CCEB File Offset: 0x0008AEEB
		private IEnumerator FadeSequence()
		{
			yield return this.FadeIn();
			yield return new WaitForSecondsRealtime(this.lifetime);
			yield return this.FadeOut();
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x0008CCFA File Offset: 0x0008AEFA
		private IEnumerator FadeIn()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(num);
				this.SetScale(num);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0008CD09 File Offset: 0x0008AF09
		private IEnumerator FadeOut()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.unscaledDeltaTime;
				float num = 1f - Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(num);
				this.SetScale(num);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0008CD18 File Offset: 0x0008AF18
		private void SetAlpha(float alpha)
		{
			if (this.markerImage != null)
			{
				this.markerImage.color = this.markerImage.color.WithAlpha(alpha);
			}
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0008CD44 File Offset: 0x0008AF44
		private void SetScale(float t)
		{
			base.transform.localScale = Vector3.Lerp(this.minScale, this.maxScale, t);
		}

		// Token: 0x04000D54 RID: 3412
		private Vector3 targetWorldPosition;

		// Token: 0x04000D55 RID: 3413
		private Camera mainCamera;

		// Token: 0x04000D56 RID: 3414
		private float lifetime = 2f;

		// Token: 0x04000D57 RID: 3415
		private float fadeDuration = 0.25f;

		// Token: 0x04000D58 RID: 3416
		private Vector3 minScale = new Vector3(0.85f, 0.85f, 0.85f);

		// Token: 0x04000D59 RID: 3417
		private Vector3 maxScale = new Vector3(1f, 1f, 1f);

		// Token: 0x04000D5A RID: 3418
		[SerializeField]
		private Image markerImage;

		// Token: 0x04000D5B RID: 3419
		[SerializeField]
		private float rotationAmount = 15f;

		// Token: 0x04000D5C RID: 3420
		[SerializeField]
		private float oscillationSpeed = 2f;
	}
}
