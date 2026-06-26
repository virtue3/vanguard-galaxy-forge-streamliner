using System;
using Behaviour.Effects.Data;
using UnityEngine;
using UnityEngine.VFX;

namespace Behaviour.Effects
{
	// Token: 0x02000387 RID: 903
	public class ShieldEffect : AbstractEffect
	{
		// Token: 0x060022A6 RID: 8870 RVA: 0x000C7F28 File Offset: 0x000C6128
		protected override void Awake()
		{
			base.Awake();
			this.eventAttribute = base.visualEffect.CreateVFXEventAttribute();
			this.positionIdentifier = Shader.PropertyToID("position");
			this.sizeIdentifier = Shader.PropertyToID("size");
			this.lifetimeIdentifier = Shader.PropertyToID("lifetime");
			this.colorIdentifier = Shader.PropertyToID("color");
			this.alphaIdentifier = Shader.PropertyToID("alpha");
			this.ScheduleFadeOut(this.fadeOutTime);
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000C7FA8 File Offset: 0x000C61A8
		private void ScheduleFadeOut(float delay)
		{
			this.fadeOut = false;
			this.fadeOutDelayTimer = delay;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000C7FB8 File Offset: 0x000C61B8
		protected override void Update()
		{
			base.Update();
			if (this.playing)
			{
				base.visualEffect.SetFloat("Angle", base.transform.parent.rotation.eulerAngles.z);
				if (this.fadeOutDelayTimer > 0f)
				{
					this.fadeOutDelayTimer -= Time.deltaTime;
					if (this.fadeOutDelayTimer <= 0f)
					{
						this.fadeOut = true;
						this.alpha = 1f;
					}
				}
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

		// Token: 0x060022A9 RID: 8873 RVA: 0x000C80D0 File Offset: 0x000C62D0
		public void ShowHit(Vector2 position, Vector2 shipSize, Color hitColor)
		{
			Vector2 v = Vector2.zero;
			v = (position + shipSize / 2f) / shipSize;
			base.visualEffect.SetVector2("ShieldHit", v);
			base.visualEffect.SetBool("ShowHit", true);
			base.visualEffect.SetVector4("HitColor", hitColor);
			this.fadeOutTime = this.fadeOutTimeHit;
			this.ScheduleFadeOut(this.fadeOutTimeHit);
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000C814B File Offset: 0x000C634B
		public void SetShieldColor(Color color)
		{
			this.shieldColor = color;
			base.visualEffect.SetVector4("ShieldColor", color);
			base.visualEffect.SetVector4("HitColor", color);
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000C8180 File Offset: 0x000C6380
		public void SetShieldSize(float size)
		{
			this.shieldSize = size;
			base.visualEffect.SetFloat("ShieldSize", size);
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000C819A File Offset: 0x000C639A
		public void SetMainTexture(Texture2D texture)
		{
			this.texture = texture;
			base.visualEffect.SetTexture("MainTexture", texture);
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000C81B4 File Offset: 0x000C63B4
		public void SetScaleFactor(Vector2 scaleFactor)
		{
			this.SetShieldSize(this.shieldSize);
			this.SetShieldColor(this.shieldColor);
			base.visualEffect.SetVector2("ScaleFactor", scaleFactor);
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000C81E0 File Offset: 0x000C63E0
		public void SetShieldTime(float shieldTime, bool reset)
		{
			base.visualEffect.SetFloat("ShieldTime", shieldTime);
			if (reset)
			{
				this.fadeOutTime = this.fadeOutTimeNormal;
				base.visualEffect.SetBool("ShowHit", false);
				base.visualEffect.SetVector4("HitColor", this.shieldColor);
				this.ScheduleFadeOut(this.fadeOutTimeNormal);
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000C8245 File Offset: 0x000C6445
		public void SetHitsData(ShieldHitData data)
		{
			this.ShowHit(data.position, data.shipSize, data.color);
		}

		// Token: 0x04001485 RID: 5253
		[SerializeField]
		private Color shieldColor;

		// Token: 0x04001486 RID: 5254
		[SerializeField]
		private float shieldSize;

		// Token: 0x04001487 RID: 5255
		private VFXEventAttribute eventAttribute;

		// Token: 0x04001488 RID: 5256
		private int positionIdentifier;

		// Token: 0x04001489 RID: 5257
		private int sizeIdentifier;

		// Token: 0x0400148A RID: 5258
		private int lifetimeIdentifier;

		// Token: 0x0400148B RID: 5259
		private int colorIdentifier;

		// Token: 0x0400148C RID: 5260
		private int alphaIdentifier;

		// Token: 0x0400148D RID: 5261
		private Texture2D texture;

		// Token: 0x0400148E RID: 5262
		private bool fadeOut;

		// Token: 0x0400148F RID: 5263
		private float fadeOutTime = 3f;

		// Token: 0x04001490 RID: 5264
		public float fadeOutTimeHit;

		// Token: 0x04001491 RID: 5265
		public float fadeOutTimeNormal;

		// Token: 0x04001492 RID: 5266
		public float fadeInTime;

		// Token: 0x04001493 RID: 5267
		private float alpha = 1f;

		// Token: 0x04001494 RID: 5268
		private float fadeOutDelayTimer;
	}
}
