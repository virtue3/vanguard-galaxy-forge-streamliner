using System;
using System.Collections;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000386 RID: 902
	public class SalvageEffect : AbstractEffect
	{
		// Token: 0x0600229D RID: 8861 RVA: 0x000C7D06 File Offset: 0x000C5F06
		protected override void Awake()
		{
			base.Awake();
			base.StartCoroutine(this.FadeOut());
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000C7D1B File Offset: 0x000C5F1B
		private IEnumerator FadeOut()
		{
			for (;;)
			{
				yield return new WaitForSeconds(this.fadeOutTime);
				this.alpha = 1f;
				this.fadeOut = true;
				yield return new WaitForSeconds(this.fadeOutTime);
				this.fadeOut = false;
			}
			yield break;
		}

		// Token: 0x0600229F RID: 8863 RVA: 0x000C7D2C File Offset: 0x000C5F2C
		protected override void Update()
		{
			base.Update();
			if (this.playing)
			{
				base.visualEffect.SetFloat("Angle", base.transform.parent.rotation.eulerAngles.z);
				if (this.fadeOut && this.alpha > 0f)
				{
					this.alpha -= Time.deltaTime / this.fadeOutTime;
				}
				else if (!this.fadeOut && this.alpha < 1f)
				{
					this.alpha += Time.deltaTime / this.fadeInTime;
				}
				this.alpha = Mathf.Clamp(this.alpha, 0f, 1f);
				base.visualEffect.SetFloat("ShieldAlpha", this.alpha);
			}
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x000C7E06 File Offset: 0x000C6006
		public void SetColor(Color color)
		{
			this.shieldColor = color;
			base.visualEffect.SetVector4("ShieldColor", color);
			base.visualEffect.SetVector4("HitColor", color);
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000C7E3B File Offset: 0x000C603B
		public void SetSize(float size)
		{
			this.shieldSize = size;
			base.visualEffect.SetFloat("ShieldSize", size);
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000C7E55 File Offset: 0x000C6055
		public void SetMainTexture(Texture2D texture)
		{
			this.texture = texture;
			base.visualEffect.SetTexture("MainTexture", texture);
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000C7E6F File Offset: 0x000C606F
		public void SetScaleFactor(Vector2 scaleFactor)
		{
			this.SetSize(this.shieldSize);
			this.SetColor(this.shieldColor);
			base.visualEffect.SetVector2("ScaleFactor", scaleFactor);
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x000C7E9C File Offset: 0x000C609C
		public void SetTime(float shieldTime, bool reset)
		{
			base.visualEffect.SetFloat("ShieldTime", shieldTime);
			if (reset)
			{
				this.fadeOut = false;
				this.fadeOutTime = this.fadeOutTimeNormal;
				base.visualEffect.SetBool("ShowHit", false);
				base.visualEffect.SetVector4("HitColor", this.shieldColor);
				base.StartCoroutine(this.FadeOut());
			}
		}

		// Token: 0x0400147C RID: 5244
		[SerializeField]
		private Color shieldColor;

		// Token: 0x0400147D RID: 5245
		[SerializeField]
		private float shieldSize;

		// Token: 0x0400147E RID: 5246
		private Texture2D texture;

		// Token: 0x0400147F RID: 5247
		private bool fadeOut;

		// Token: 0x04001480 RID: 5248
		private float fadeOutTime = 2f;

		// Token: 0x04001481 RID: 5249
		public float fadeOutTimeHit;

		// Token: 0x04001482 RID: 5250
		public float fadeOutTimeNormal;

		// Token: 0x04001483 RID: 5251
		public float fadeInTime;

		// Token: 0x04001484 RID: 5252
		private float alpha = 1f;
	}
}
