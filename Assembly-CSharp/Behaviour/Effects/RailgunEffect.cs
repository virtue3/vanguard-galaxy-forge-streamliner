using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Effects
{
	// Token: 0x02000398 RID: 920
	public class RailgunEffect : AbstractEffect
	{
		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x000C8EF5 File Offset: 0x000C70F5
		// (set) Token: 0x060022F0 RID: 8944 RVA: 0x000C8EFD File Offset: 0x000C70FD
		public float chargeTime { get; protected set; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000C8F06 File Offset: 0x000C7106
		// (set) Token: 0x060022F2 RID: 8946 RVA: 0x000C8F0E File Offset: 0x000C710E
		public float dischargeTime { get; protected set; }

		// Token: 0x060022F3 RID: 8947 RVA: 0x000C8F18 File Offset: 0x000C7118
		protected override void Awake()
		{
			base.Awake();
			this.light = base.GetComponent<Light2D>();
			if (this.light)
			{
				this.light.enabled = false;
				Color color = this.chargeColor.Evaluate(1f);
				this.light.color = new Color(color.r, color.g, color.b, 1f);
				this.freeFormLightLine = new FreeFormLightLine(this.light);
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x000C8F9C File Offset: 0x000C719C
		protected override void Update()
		{
			base.Update();
			if (!this.playing)
			{
				return;
			}
			this.lightTimer += Time.deltaTime;
			if (this.lightTimer > this.chargeTime - this.dischargeTime)
			{
				if (!this.light.enabled)
				{
					this.freeFormLightLine.UpdateFreeformLight(this.spawnPosition, this.targetPosition);
					this.light.enabled = true;
				}
				float num = (float)(((this.lightTimer < this.chargeTime + this.dischargeTime / 3f) ? 1 : -1) * 5) * Time.deltaTime;
				this.lightIntensity += 10f * num;
				this.light.intensity = this.lightIntensity;
			}
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x000C9060 File Offset: 0x000C7260
		private void Start()
		{
			base.visualEffect.SetFloat("ChargeTime", this.chargeTime);
			base.visualEffect.SetFloat("ChargeFrequency", this.chargeFrequency);
			base.visualEffect.SetFloat("DischargeTime", this.dischargeTime);
			base.visualEffect.SetGradient("ChargeColor", this.chargeColor);
			base.visualEffect.SetGradient("TargettingColor", this.targettingColor);
			this.lightTimer = 0f;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x000C90E6 File Offset: 0x000C72E6
		public override void Stop()
		{
			base.Stop();
			this.lightTimer = 0f;
			this.lightIntensity = 5f;
			this.light.enabled = false;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x000C9110 File Offset: 0x000C7310
		public void SetChargeTime(float chargeTime)
		{
			this.chargeTime = chargeTime;
			base.visualEffect.SetFloat("ChargeTime", chargeTime);
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000C912A File Offset: 0x000C732A
		public void SetDischargeTime(float dischargeTime)
		{
			this.dischargeTime = dischargeTime;
			base.visualEffect.SetFloat("DischargeTime", dischargeTime);
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x000C9144 File Offset: 0x000C7344
		public void SetSpawnPosition(Vector2 spawnPosition)
		{
			this.spawnPosition = spawnPosition;
			base.visualEffect.SetVector2("SpawnPosition", spawnPosition);
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x000C915E File Offset: 0x000C735E
		public void SetTargetPosition(Vector2 targetPosition)
		{
			this.targetPosition = targetPosition;
			base.visualEffect.SetVector2("TargetPosition", targetPosition);
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x000C9178 File Offset: 0x000C7378
		public float GetPlayTime()
		{
			return this.chargeTime;
		}

		// Token: 0x040014D9 RID: 5337
		[SerializeField]
		private float chargeFrequency;

		// Token: 0x040014DC RID: 5340
		[SerializeField]
		private Gradient chargeColor;

		// Token: 0x040014DD RID: 5341
		[SerializeField]
		private Gradient targettingColor;

		// Token: 0x040014DE RID: 5342
		private float lightTimer;

		// Token: 0x040014DF RID: 5343
		private float lightIntensity = 10f;

		// Token: 0x040014E0 RID: 5344
		private Light2D light;

		// Token: 0x040014E1 RID: 5345
		private FreeFormLightLine freeFormLightLine;

		// Token: 0x040014E2 RID: 5346
		protected Vector2 spawnPosition;

		// Token: 0x040014E3 RID: 5347
		protected Vector2 targetPosition;
	}
}
