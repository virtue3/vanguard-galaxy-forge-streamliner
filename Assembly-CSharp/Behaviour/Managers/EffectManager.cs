using System;
using System.Collections.Generic;
using Behaviour.Effects;
using Behaviour.Util;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Managers
{
	// Token: 0x02000302 RID: 770
	public class EffectManager : Singleton<EffectManager>
	{
		// Token: 0x06001C6F RID: 7279 RVA: 0x000ABABC File Offset: 0x000A9CBC
		protected override void Awake()
		{
			base.Awake();
			this.flashEffect = UnityEngine.Object.Instantiate<FlashEffect>(this.flashEffect, base.transform);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000ABADB File Offset: 0x000A9CDB
		private void Update()
		{
			this.effectCheckTimer -= Time.deltaTime;
			if (this.effectCheckTimer < 0f)
			{
				this.DestroyEffects(false);
				this.effectCheckTimer = 0.3f;
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000ABB10 File Offset: 0x000A9D10
		private void DestroyEffects(bool destroyAll = false)
		{
			for (int i = this.killWhenDoneEffects.Count - 1; i >= 0; i--)
			{
				AbstractEffect abstractEffect = this.killWhenDoneEffects[i];
				if (abstractEffect.visualEffect)
				{
					if (destroyAll || (abstractEffect.canBeDestroyed && abstractEffect.visualEffect.aliveParticleCount <= 0))
					{
						UnityEngine.Object.Destroy(abstractEffect.gameObject);
						this.killWhenDoneEffects.RemoveAt(i);
					}
				}
				else
				{
					this.killWhenDoneEffects.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x000ABB90 File Offset: 0x000A9D90
		public void KillAllEffects()
		{
			Debug.Log("Killing all effects remaining: " + this.killWhenDoneEffects.Count.ToString());
			this.DestroyEffects(true);
			this.killWhenDoneEffects.Clear();
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000ABBD4 File Offset: 0x000A9DD4
		public void PlayExplosionEffect(Vector2 position, bool mining = false, float scale = 1f, Color? color = null)
		{
			ExplosionEffect explosionEffect = mining ? this.mineExplosion : this.explosion;
			explosionEffect = UnityEngine.Object.Instantiate<ExplosionEffect>(explosionEffect, position, Quaternion.identity, base.transform);
			if (color != null)
			{
				explosionEffect.SetColor(color.Value);
			}
			explosionEffect.SetScale(scale);
			explosionEffect.Play();
			UnityEngine.Object.Destroy(explosionEffect.gameObject, 3f);
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000ABC40 File Offset: 0x000A9E40
		public void PlayExplosionEffect(Vector2 position, Vector2 velocity, float scale, Color flashColor, float delay = 0f)
		{
			ExplosionEffect explosionEffect = UnityEngine.Object.Instantiate<ExplosionEffect>(this.explosion, position, Quaternion.identity, base.transform);
			explosionEffect.SetFlashColor(flashColor);
			explosionEffect.SetScale(scale);
			explosionEffect.GiveSpeed(velocity);
			if (delay > 0f)
			{
				explosionEffect.maxPlayTime = 1.5f;
				explosionEffect.Play(delay);
				this.AddKillWhenDoneEffect(explosionEffect);
				return;
			}
			explosionEffect.Play();
			UnityEngine.Object.Destroy(explosionEffect.gameObject, 1f);
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x000ABCBA File Offset: 0x000A9EBA
		public void PlayPlasmaExplosionEffect(Vector2 position, float size = 0.5f)
		{
			PlasmaExplosionEffect plasmaExplosionEffect = UnityEngine.Object.Instantiate<PlasmaExplosionEffect>(this.plasmaExplosion, position, Quaternion.identity, base.transform);
			plasmaExplosionEffect.SetSize(size);
			plasmaExplosionEffect.Play();
			UnityEngine.Object.Destroy(plasmaExplosionEffect.gameObject, 1f);
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x000ABCF4 File Offset: 0x000A9EF4
		public void PlayFlashEffect(Vector2 position, Color color, float size)
		{
			FlashEffect flashEffect = UnityEngine.Object.Instantiate<FlashEffect>(this.flashEffect, position, Quaternion.identity, base.transform);
			flashEffect.SetSize(size);
			flashEffect.SetColor(color);
			flashEffect.Play();
			UnityEngine.Object.Destroy(flashEffect.gameObject, 0.4f);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x000ABD40 File Offset: 0x000A9F40
		public void PlayShockwaveExplosionEffect(Vector2 position, float size, float delay = 0.3f)
		{
			AsteroidExplosionEffect asteroidExplosionEffect = UnityEngine.Object.Instantiate<AsteroidExplosionEffect>(this.asteroidExplosion, position, Quaternion.identity, base.transform);
			asteroidExplosionEffect.SetShockWaveSize(size);
			asteroidExplosionEffect.SetDelay(delay);
			asteroidExplosionEffect.Play();
			UnityEngine.Object.Destroy(asteroidExplosionEffect.gameObject, 10f);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x000ABD8C File Offset: 0x000A9F8C
		public void PlayMissileTrailEffect(GameObject follow, float delay, MissileTrailEffect effect)
		{
			MissileTrailEffect missileTrailEffect = UnityEngine.Object.Instantiate<MissileTrailEffect>(effect, follow.transform.position, follow.transform.rotation, base.transform);
			missileTrailEffect.SetFollowObject(follow);
			missileTrailEffect.Play(delay);
			this.AddKillWhenDoneEffect(missileTrailEffect);
			missileTrailEffect = UnityEngine.Object.Instantiate<MissileTrailEffect>(effect, follow.transform.position, follow.transform.rotation, base.transform);
			missileTrailEffect.SetFollowObject(follow, new Vector2(-0.3f, 0f));
			missileTrailEffect.Play(delay);
			this.AddKillWhenDoneEffect(missileTrailEffect);
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000ABE18 File Offset: 0x000AA018
		public void PlaySmokeTrailEffect(GameObject follow, float delay, float size, Vector2 localOffset, float maxPlayTime = 0f, float spawnRate = 50f)
		{
			SmokeTrailEffect smokeTrailEffect = UnityEngine.Object.Instantiate<SmokeTrailEffect>(this.smokeTrailEffect, follow.transform.position, follow.transform.rotation, base.transform);
			smokeTrailEffect.SetFollowObject(follow, localOffset);
			smokeTrailEffect.PlayWithSize(size, delay, spawnRate);
			if (maxPlayTime > 0f)
			{
				smokeTrailEffect.maxPlayTime = maxPlayTime;
			}
			this.AddKillWhenDoneEffect(smokeTrailEffect);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000ABE78 File Offset: 0x000AA078
		public void PlaySmokeEffect(Vector2 position, float size, DamageType? damageType = null, float lifetime = 4f, int count = 15)
		{
			SmokeEffect smokeEffect = UnityEngine.Object.Instantiate<SmokeEffect>(this.smokeEffect, position, Quaternion.identity, base.transform);
			if (damageType != null)
			{
				smokeEffect.SetCombineColor(this.GetColorForDamageType(damageType.Value));
			}
			smokeEffect.PlayWithArgs(size, lifetime, count);
			smokeEffect.maxPlayTime = 12f;
			this.AddKillWhenDoneEffect(smokeEffect);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x000ABEDC File Offset: 0x000AA0DC
		public void PlaySparksEffect(Vector2 position, float size, float frequency = 0.2f, float duration = 0.2f, Color? color = null, Transform follow = null)
		{
			SparksEffect sparksEffect = UnityEngine.Object.Instantiate<SparksEffect>(this.sparksEffect, position, Quaternion.identity, base.transform);
			sparksEffect.maxPlayTime = duration;
			if (follow)
			{
				sparksEffect.SetFollowTransform(follow);
			}
			sparksEffect.setSize(size);
			sparksEffect.setFrequency(frequency);
			if (color != null)
			{
				sparksEffect.SetColor(color.Value);
			}
			sparksEffect.Play();
			this.AddKillWhenDoneEffect(sparksEffect);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x000ABF50 File Offset: 0x000AA150
		public void PlayDroneMiningEffect(Vector2 position, float size, float frequency, float duration, Transform follow = null)
		{
			DroneMiningEffect droneMiningEffect = UnityEngine.Object.Instantiate<DroneMiningEffect>(this.droneMiningEffect, position, Quaternion.identity, base.transform);
			if (follow)
			{
				droneMiningEffect.SetFollowTransform(follow);
			}
			droneMiningEffect.maxPlayTime = duration;
			droneMiningEffect.PlayWithSize(size, frequency);
			this.AddKillWhenDoneEffect(droneMiningEffect);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x000ABFA2 File Offset: 0x000AA1A2
		public void AddKillWhenDoneEffect(AbstractEffect effect)
		{
			this.killWhenDoneEffects.Add(effect);
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x000ABFB0 File Offset: 0x000AA1B0
		public Gradient GetColorForDamageType(DamageType damageType)
		{
			switch (damageType)
			{
			case DamageType.Radiation:
				return this.radiationColor;
			case DamageType.Heat:
				return this.heatColor;
			case DamageType.Cold:
				return this.coldColor;
			case DamageType.Corrosion:
				return this.corrosionColor;
			default:
				return this.genericSmokeColor;
			}
		}

		// Token: 0x0400119F RID: 4511
		[SerializeField]
		private ExplosionEffect explosion;

		// Token: 0x040011A0 RID: 4512
		[SerializeField]
		private PlasmaExplosionEffect plasmaExplosion;

		// Token: 0x040011A1 RID: 4513
		[SerializeField]
		private MineExplosionEffect mineExplosion;

		// Token: 0x040011A2 RID: 4514
		[SerializeField]
		private AsteroidExplosionEffect asteroidExplosion;

		// Token: 0x040011A3 RID: 4515
		[SerializeField]
		private SmokeTrailEffect smokeTrailEffect;

		// Token: 0x040011A4 RID: 4516
		[SerializeField]
		private SmokeEffect smokeEffect;

		// Token: 0x040011A5 RID: 4517
		[SerializeField]
		private FlashEffect flashEffect;

		// Token: 0x040011A6 RID: 4518
		[SerializeField]
		private SparksEffect sparksEffect;

		// Token: 0x040011A7 RID: 4519
		[SerializeField]
		private DroneMiningEffect droneMiningEffect;

		// Token: 0x040011A8 RID: 4520
		public SalvageEffect salvageEffect;

		// Token: 0x040011A9 RID: 4521
		public Gradient corrosionColor;

		// Token: 0x040011AA RID: 4522
		public Gradient coldColor;

		// Token: 0x040011AB RID: 4523
		public Gradient radiationColor;

		// Token: 0x040011AC RID: 4524
		public Gradient heatColor;

		// Token: 0x040011AD RID: 4525
		public Gradient genericSmokeColor;

		// Token: 0x040011AE RID: 4526
		private List<AbstractEffect> killWhenDoneEffects = new List<AbstractEffect>();

		// Token: 0x040011AF RID: 4527
		private float effectCheckTimer = 0.3f;
	}
}
