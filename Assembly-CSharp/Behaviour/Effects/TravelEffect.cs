using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Effects
{
	// Token: 0x02000391 RID: 913
	public class TravelEffect : AbstractEffect
	{
		// Token: 0x060022DB RID: 8923 RVA: 0x000C8983 File Offset: 0x000C6B83
		protected override void Awake()
		{
			base.Awake();
			this.travelScaleVariation = new Vector4(0.9f, 0.9f, 2f, 1.5f);
		}

		// Token: 0x060022DC RID: 8924 RVA: 0x000C89AC File Offset: 0x000C6BAC
		public void SetBasicVars(float shipWidth, float startSizeMultiplier, bool fastLaneTravel = false, float widthRatio = 1f)
		{
			this.shipWidth = shipWidth / 3f;
			this.startSize = startSizeMultiplier * this.shipWidth * 1.5f;
			if (fastLaneTravel)
			{
				base.visualEffect.SetVector4("Color", this.fastLaneColor);
				this.light.color = this.fastLaneColor;
			}
			this.light.transform.localPosition = new Vector2((widthRatio < 1f) ? widthRatio : (-widthRatio), 0f);
			this.light.pointLightOuterRadius = this.shipWidth * 6f;
			this.light.pointLightInnerRadius = this.shipWidth * 2f;
		}

		// Token: 0x060022DD RID: 8925 RVA: 0x000C8A66 File Offset: 0x000C6C66
		public void Charge()
		{
			this.status = TravelEffectStatus.charging;
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000C8A6F File Offset: 0x000C6C6F
		public void Travel()
		{
			this.status = TravelEffectStatus.travelling;
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000C8A78 File Offset: 0x000C6C78
		public void Land()
		{
			this.status = TravelEffectStatus.landing;
			this.playTime = 0f;
			this.maxPlayTime = 0.5f;
			this.travelSpeedChange = this.travelSpeed / this.maxPlayTime;
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000C8AAA File Offset: 0x000C6CAA
		public void SetTravelSpeed(float travelSpeed)
		{
			this.travelSpeed = Mathf.Clamp(travelSpeed * 10f, 5f, 15f) * this.shipWidth * 5f;
		}

		// Token: 0x060022E1 RID: 8929 RVA: 0x000C8AD8 File Offset: 0x000C6CD8
		protected override void Update()
		{
			base.Update();
			if (!this.playing)
			{
				return;
			}
			float x = 1f;
			if (this.status == TravelEffectStatus.charging)
			{
				if (this.startSize < this.shipWidth)
				{
					this.startSize += this.shipWidth * Time.deltaTime;
					base.visualEffect.SetVector4("ScaleVariation", this.startSize * this.travelScaleVariation);
				}
				base.visualEffect.SetFloat("ParticleAlpha", 0f);
				this.lightIntensity = Mathf.Clamp(this.lightIntensity + Time.deltaTime * 4f, 0.1f, 0.3f);
				this.increaseIntensity = true;
			}
			else if (this.status == TravelEffectStatus.travelling)
			{
				base.visualEffect.SetVector4("ScaleVariation", this.startSize * this.travelScaleVariation);
				base.visualEffect.SetFloat("ParticleAlpha", 1f);
				x = -this.travelSpeed;
				if (this.lightIntensity < 0.1f && !this.increaseIntensity)
				{
					this.increaseIntensity = true;
				}
				else if (this.lightIntensity > 0.3f && this.increaseIntensity)
				{
					this.increaseIntensity = false;
				}
				if (this.increaseIntensity)
				{
					this.lightIntensity += Time.deltaTime * 2f;
				}
				else
				{
					this.lightIntensity -= Time.deltaTime * 2f;
				}
			}
			else if (this.status == TravelEffectStatus.landing)
			{
				this.startSize -= this.shipWidth * Time.deltaTime;
				base.visualEffect.SetVector4("ScaleVariation", this.startSize * this.travelScaleVariation);
				base.visualEffect.SetFloat("ParticleAlpha", 0f);
				this.travelSpeed -= this.travelSpeedChange * Time.deltaTime;
				x = -this.travelSpeed;
				base.transform.position = Vector2.MoveTowards(base.transform.position, base.transform.position + base.transform.right, Time.deltaTime * this.travelSpeed * 2f);
				this.lightIntensity = Mathf.Clamp(this.lightIntensity - Time.deltaTime * 3f, 0f, 0.3f);
			}
			base.visualEffect.SetVector2("Speed", new Vector2(x, 0f));
			this.light.intensity = this.lightIntensity;
		}

		// Token: 0x040014BC RID: 5308
		[SerializeField]
		private Color fastLaneColor;

		// Token: 0x040014BD RID: 5309
		[SerializeField]
		private Light2D light;

		// Token: 0x040014BE RID: 5310
		private Vector4 startScaleVariation;

		// Token: 0x040014BF RID: 5311
		private Vector4 travelScaleVariation;

		// Token: 0x040014C0 RID: 5312
		private Vector4 landScaleVariation;

		// Token: 0x040014C1 RID: 5313
		private float startSize;

		// Token: 0x040014C2 RID: 5314
		private float shipWidth;

		// Token: 0x040014C3 RID: 5315
		private TravelEffectStatus status;

		// Token: 0x040014C4 RID: 5316
		private float travelSpeed;

		// Token: 0x040014C5 RID: 5317
		private float travelSpeedChange;

		// Token: 0x040014C6 RID: 5318
		private float lightIntensity;

		// Token: 0x040014C7 RID: 5319
		private bool increaseIntensity;
	}
}
