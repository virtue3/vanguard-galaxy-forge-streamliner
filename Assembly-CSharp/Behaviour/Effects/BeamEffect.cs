using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

namespace Behaviour.Effects
{
	// Token: 0x0200037F RID: 895
	public class BeamEffect : AbstractEffect
	{
		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x000C74EC File Offset: 0x000C56EC
		// (set) Token: 0x06002277 RID: 8823 RVA: 0x000C74F4 File Offset: 0x000C56F4
		public GameObject spawnObject { get; protected set; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002278 RID: 8824 RVA: 0x000C74FD File Offset: 0x000C56FD
		// (set) Token: 0x06002279 RID: 8825 RVA: 0x000C7505 File Offset: 0x000C5705
		public GameObject targetObject { get; protected set; }

		// Token: 0x0600227A RID: 8826 RVA: 0x000C7510 File Offset: 0x000C5710
		protected override void Awake()
		{
			base.Awake();
			this.spawnPositionIdentifier = Shader.PropertyToID("SpawnPosition");
			this.targetPositionIdentifier = Shader.PropertyToID("TargetPosition");
			this.light = base.GetComponent<Light2D>();
			if (this.light)
			{
				this.light.enabled = false;
				this.freeFormLightLine = new FreeFormLightLine(this.light);
				Color color = base.visualEffect.GetVector4("Color");
				this.light.color = new Color(color.r, color.g, color.b, 1f);
			}
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000C75B6 File Offset: 0x000C57B6
		public override void Play()
		{
			base.Play();
			VisualEffect visualEffect = base.visualEffect;
			if (visualEffect != null)
			{
				visualEffect.SetBool("Alive", true);
			}
			if (this.light)
			{
				this.light.enabled = true;
			}
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000C75F0 File Offset: 0x000C57F0
		public override void Stop()
		{
			base.Stop();
			VisualEffect visualEffect = base.visualEffect;
			if (visualEffect != null)
			{
				visualEffect.ResetOverride("SpawnPosition");
			}
			VisualEffect visualEffect2 = base.visualEffect;
			if (visualEffect2 != null)
			{
				visualEffect2.ResetOverride("TargetPosition");
			}
			VisualEffect visualEffect3 = base.visualEffect;
			if (visualEffect3 != null)
			{
				visualEffect3.SetBool("Alive", false);
			}
			if (this.light && this.light.enabled)
			{
				this.light.enabled = false;
			}
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000C766C File Offset: 0x000C586C
		protected override void Update()
		{
			base.transform.localRotation = Quaternion.identity;
			base.Update();
			if (this.spawnObject && this.targetObject && base.visualEffect)
			{
				this.SetSpawnPosition(this.spawnObject.transform.position);
				this.SetTargetPosition(this.targetObject.transform.position);
				if (this.light && this.light.enabled)
				{
					this.freeFormLightLine.UpdateFreeformLight(this.spawnObject.transform.position, this.targetObject.transform.position);
				}
			}
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000C7737 File Offset: 0x000C5937
		public void SetObjectsToTrack(GameObject spawn, GameObject target)
		{
			this.spawnObject = spawn;
			this.targetObject = target;
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000C7747 File Offset: 0x000C5947
		public void OnDestroy()
		{
			base.visualEffect.Stop();
			UnityEngine.Object.Destroy(base.visualEffect);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000C775F File Offset: 0x000C595F
		public void SetSpawnPosition(Vector3 spawnPosition)
		{
			this.SpawnPosition = spawnPosition;
			base.visualEffect.SetVector3(this.spawnPositionIdentifier, spawnPosition);
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000C777A File Offset: 0x000C597A
		public void SetTargetPosition(Vector3 targetPosition)
		{
			this.TargetPosition = targetPosition;
			base.visualEffect.SetVector3(this.targetPositionIdentifier, targetPosition);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000C7798 File Offset: 0x000C5998
		public void SetColor(Color color)
		{
			this.color = color;
			if (this.light)
			{
				color.a = 1f;
				this.light.color = color;
			}
			base.visualEffect.SetVector4("Color", color);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000C77E7 File Offset: 0x000C59E7
		public void SetPower(float power)
		{
			this.power = power;
			base.visualEffect.SetFloat("Power", power);
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000C7801 File Offset: 0x000C5A01
		public void SetFrequency(float frequency)
		{
			this.frequency = frequency;
			base.visualEffect.SetFloat("Frequency", frequency);
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000C781B File Offset: 0x000C5A1B
		public void SetSize(float size)
		{
			this.size = size;
			base.visualEffect.SetFloat("Size", size);
		}

		// Token: 0x0400145C RID: 5212
		[SerializeField]
		private Vector3 SpawnPosition;

		// Token: 0x0400145D RID: 5213
		[SerializeField]
		private Vector3 TargetPosition;

		// Token: 0x0400145E RID: 5214
		[SerializeField]
		private Color color;

		// Token: 0x0400145F RID: 5215
		[SerializeField]
		private float power;

		// Token: 0x04001460 RID: 5216
		[SerializeField]
		private float frequency;

		// Token: 0x04001461 RID: 5217
		[SerializeField]
		private float size;

		// Token: 0x04001462 RID: 5218
		private Light2D light;

		// Token: 0x04001463 RID: 5219
		private FreeFormLightLine freeFormLightLine;

		// Token: 0x04001466 RID: 5222
		private int spawnPositionIdentifier;

		// Token: 0x04001467 RID: 5223
		private int targetPositionIdentifier;
	}
}
