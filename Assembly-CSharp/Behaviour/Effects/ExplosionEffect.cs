using System;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Effects
{
	// Token: 0x02000380 RID: 896
	public class ExplosionEffect : AbstractEffect
	{
		// Token: 0x06002287 RID: 8839 RVA: 0x000C783D File Offset: 0x000C5A3D
		protected override void Awake()
		{
			base.Awake();
			this.light = base.GetComponent<Light2D>();
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000C7854 File Offset: 0x000C5A54
		public void EnableLight(float scale, Color color)
		{
			this.light.color = color;
			this.light.lightType = Light2D.LightType.Point;
			this.light.pointLightInnerRadius = 0f;
			this.light.pointLightOuterRadius = scale * this.maxIntensity * 5f;
			this.light.intensity = 0f;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000C78B4 File Offset: 0x000C5AB4
		public void GiveSpeed(Vector2 velocity)
		{
			if (velocity != Vector2.zero)
			{
				this.rigidbody = base.gameObject.AddComponent<Rigidbody2D>();
				this.rigidbody.bodyType = RigidbodyType2D.Kinematic;
				this.rigidbody.mass = 1f;
				this.rigidbody.linearVelocity = velocity;
			}
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000C7907 File Offset: 0x000C5B07
		public void SetScale(float scale)
		{
			if (scale > 0.2f)
			{
				this.EnableLight(scale, this.color);
			}
			base.gameObject.transform.localScale = new Vector3(scale, scale, scale);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000C7936 File Offset: 0x000C5B36
		public void SetFlashColor(Color flashColor)
		{
			base.visualEffect.SetVector4("FlashColor", flashColor);
		}

		// Token: 0x0600228C RID: 8844 RVA: 0x000C794E File Offset: 0x000C5B4E
		public void SetColor(Color color)
		{
			this.color = color;
			base.visualEffect.SetVector4("ExplosionColor", color);
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000C7970 File Offset: 0x000C5B70
		protected override void Update()
		{
			base.Update();
			if (this.light)
			{
				this.light.intensity += this.changeFactor * this.maxIntensity * Time.deltaTime / 0.15f;
				if (this.light.intensity >= this.maxIntensity)
				{
					this.changeFactor = -1f;
				}
				if (this.light.intensity <= 0f)
				{
					this.light.enabled = false;
				}
			}
		}

		// Token: 0x04001468 RID: 5224
		public Light2D light;

		// Token: 0x04001469 RID: 5225
		private Color color = new Color(0.92f, 0.43f, 0f, 0.76f);

		// Token: 0x0400146A RID: 5226
		private Color flashColor = ColorHelper.flashExplosionUnit;

		// Token: 0x0400146B RID: 5227
		private const float MAX_INTENSITY = 3f;

		// Token: 0x0400146C RID: 5228
		private const float MAX_INTENSITY_TIME = 0.15f;

		// Token: 0x0400146D RID: 5229
		private float maxIntensity = 3f;

		// Token: 0x0400146E RID: 5230
		private float changeFactor = 1f;

		// Token: 0x0400146F RID: 5231
		private Rigidbody2D rigidbody;
	}
}
