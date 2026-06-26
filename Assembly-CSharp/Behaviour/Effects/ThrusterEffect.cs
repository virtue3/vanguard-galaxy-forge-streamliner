using System;
using System.Collections;
using Behaviour.AudioSystem;
using Behaviour.Util;
using Source.AudioSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Behaviour.Effects
{
	// Token: 0x0200038D RID: 909
	public class ThrusterEffect : AbstractEffect
	{
		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x000C8474 File Offset: 0x000C6674
		public virtual bool isMain
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060022C1 RID: 8897 RVA: 0x000C8478 File Offset: 0x000C6678
		protected virtual void Start()
		{
			this.maxPower = 500f;
			this.SetPowerFactor(0f);
			if (this.light)
			{
				base.visualEffect.SetVector4("Color", this.color);
				this.light.color = this.color;
			}
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000C84D4 File Offset: 0x000C66D4
		protected override void Update()
		{
			base.Update();
			if (this.currentPower > 0f && !base.isPlaying())
			{
				this.Play();
			}
			else if (base.isPlaying() && this.currentPower <= 0f)
			{
				this.Stop();
			}
			if (this.currentPower > 0f)
			{
				this.currentPower -= Time.deltaTime * this.maxPower * 4f;
				base.visualEffect.SetFloat("Power", this.currentPower);
				if (this.sideThrusterSound.clip && this.thrusterEmitter)
				{
					this.thrusterEmitter.ChangePowerVolume(this.currentPower / this.maxPower);
				}
			}
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000C8598 File Offset: 0x000C6798
		public void SetSoundModifier(bool isPlayer)
		{
			if (isPlayer)
			{
				this.npSoundModifier = 1f;
			}
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000C85A8 File Offset: 0x000C67A8
		public override void Play()
		{
			base.Play();
			if (this.light)
			{
				this.light.enabled = true;
			}
			if (this.sideThrusterSound.clip)
			{
				this.thrusterSoundCoroutine = base.StartCoroutine(this.PlaySound());
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000C85F8 File Offset: 0x000C67F8
		public override void Stop()
		{
			base.Stop();
			if (this.light)
			{
				this.light.enabled = false;
			}
			if (this.sideThrusterSound.clip)
			{
				this.StopSound();
			}
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000C8631 File Offset: 0x000C6831
		private IEnumerator PlaySound()
		{
			yield return new WaitForSeconds(this.thrusterCooldown);
			if (this.thrusterEmitter == null && this.thrusterSoundCoroutine != null)
			{
				this.thrusterEmitter = PersistentSingleton<SoundManager>.Instance.CreateSound().WithSoundData(this.sideThrusterSound).WithPosition(base.transform.position).WithFollow(base.transform).WithCustomVolume(this.npSoundModifier).WithPowerVolume(this.currentPower / this.maxPower).PlayReturn();
			}
			yield break;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000C8640 File Offset: 0x000C6840
		private void StopSound()
		{
			if (this.thrusterSoundCoroutine != null)
			{
				base.StopCoroutine(this.thrusterSoundCoroutine);
			}
			this.thrusterSoundCoroutine = null;
			if (this.thrusterEmitter != null)
			{
				this.thrusterEmitter.Stop();
				this.thrusterEmitter = null;
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000C8680 File Offset: 0x000C6880
		public virtual void SetPowerFactor(float powerFactor)
		{
			this.currentPower = Mathf.Max(this.currentPower, powerFactor * this.maxPower);
			base.visualEffect.SetFloat("Power", this.currentPower);
			if (this.light)
			{
				this.light.intensity = powerFactor * this.maxIntensity;
			}
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000C86DC File Offset: 0x000C68DC
		private void OnDisable()
		{
			this.StopSound();
		}

		// Token: 0x040014A6 RID: 5286
		protected float maxPower;

		// Token: 0x040014A7 RID: 5287
		protected float currentPower;

		// Token: 0x040014A8 RID: 5288
		[SerializeField]
		private Color color;

		// Token: 0x040014A9 RID: 5289
		[SerializeField]
		protected Light2D light;

		// Token: 0x040014AA RID: 5290
		[SerializeField]
		protected float maxIntensity;

		// Token: 0x040014AB RID: 5291
		[SerializeField]
		protected SoundData sideThrusterSound;

		// Token: 0x040014AC RID: 5292
		private SoundEmitter thrusterEmitter;

		// Token: 0x040014AD RID: 5293
		[SerializeField]
		private float thrusterCooldown = 0.2f;

		// Token: 0x040014AE RID: 5294
		private Coroutine thrusterSoundCoroutine;

		// Token: 0x040014AF RID: 5295
		private float npSoundModifier = 0.075f;
	}
}
