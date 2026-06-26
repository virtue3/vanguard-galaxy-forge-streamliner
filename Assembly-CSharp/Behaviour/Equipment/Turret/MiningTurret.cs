using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Effects;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Util;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Turret
{
	// Token: 0x02000347 RID: 839
	public class MiningTurret : AbstractMiningTurret
	{
		// Token: 0x0600201C RID: 8220 RVA: 0x000BD363 File Offset: 0x000BB563
		protected override void Start()
		{
			base.Start();
			this.CreateLaserEffects();
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x000BD374 File Offset: 0x000BB574
		private void OnEnable()
		{
			foreach (LaserEffect laserEffect in this.laserEffects)
			{
				if (laserEffect != null)
				{
					laserEffect.visualEffect.Reinit();
				}
				if (laserEffect != null)
				{
					laserEffect.Stop();
				}
			}
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x000BD3DC File Offset: 0x000BB5DC
		private void CreateLaserEffects()
		{
			for (int i = 0; i < this.firePoints.Length; i++)
			{
				LaserEffect laserEffect = UnityEngine.Object.Instantiate<LaserEffect>(this.laserEffectPrefab, Vector3.zero, Quaternion.identity, this.firePoints[i]);
				laserEffect.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
				laserEffect.Stop();
				this.laserEffects.Add(laserEffect);
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000BD446 File Offset: 0x000BB646
		protected override IEnumerator PlayWeaponEffect()
		{
			LaserEffect laserEffect = this.laserEffects[this.firePointIndex];
			laserEffect.SetObjectsToTrack(this.firePoints[this.firePointIndex].gameObject, base.trackingTarget);
			laserEffect.SetPower(this.effectPower);
			laserEffect.SetFrequency(this.effectFrequency);
			laserEffect.SetSize(this.effectSize);
			laserEffect.SetColor(this.effectColor);
			laserEffect.Play();
			yield return new WaitForSeconds(this.effectDuration);
			laserEffect.Stop();
			yield break;
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x000BD455 File Offset: 0x000BB655
		protected override bool FireInternal()
		{
			base.StartCoroutine(this.ShootAtTarget());
			return true;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x000BD465 File Offset: 0x000BB665
		private IEnumerator ShootAtTarget()
		{
			Asteroid asteroidTarget = base.asteroidTarget;
			GameObject trackingTarget = base.trackingTarget;
			this.overrideRotationTarget = trackingTarget;
			yield return this.PlayWeaponEffect();
			this.overrideRotationTarget = null;
			if (!trackingTarget)
			{
				yield break;
			}
			Singleton<EffectManager>.Instance.PlaySmokeEffect(trackingTarget.transform.position, 0.2f, null, 4f, 15);
			DamageData damageData = this.CreateDamageData(null, null, TargetLayer.Surface);
			damageData.hitTransform = trackingTarget.transform;
			damageData.hitCoordinates = trackingTarget.transform.position;
			if (asteroidTarget)
			{
				if (base.targetsCore)
				{
					asteroidTarget.TakeInnerCoreDamage(damageData, true);
				}
				else if (!asteroidTarget.IsSurfaceOreDepleted())
				{
					asteroidTarget.TakeSurfaceDamage(damageData);
				}
			}
			else
			{
				TargetableUnit currentTarget = base.currentTarget;
				if (currentTarget == null || currentTarget.CanBeDamagedBy(this))
				{
					TargetableUnit currentTarget2 = base.currentTarget;
					if (currentTarget2 != null)
					{
						currentTarget2.TakeDamage(damageData);
					}
				}
			}
			yield break;
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x000BD474 File Offset: 0x000BB674
		private void OnDisable()
		{
			if (this.laserEffects.Count <= 0)
			{
				return;
			}
			base.StopAllCoroutines();
			foreach (LaserEffect laserEffect in this.laserEffects)
			{
				laserEffect.Stop();
			}
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x000BD4DC File Offset: 0x000BB6DC
		protected override DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			DamageData damageData = base.CreateDamageData(targetTransform, hitCoordinates, targetLayer);
			damageData.hitTransform = base.trackingTarget.transform;
			damageData.hitCoordinates = base.trackingTarget.transform.position;
			return damageData;
		}

		// Token: 0x04001325 RID: 4901
		[Header("Laser Setup")]
		[SerializeField]
		private LaserEffect laserEffectPrefab;

		// Token: 0x04001326 RID: 4902
		[SerializeField]
		private float effectDuration;

		// Token: 0x04001327 RID: 4903
		[SerializeField]
		private Color effectColor;

		// Token: 0x04001328 RID: 4904
		[SerializeField]
		private float effectPower;

		// Token: 0x04001329 RID: 4905
		[SerializeField]
		private float effectFrequency;

		// Token: 0x0400132A RID: 4906
		[SerializeField]
		private float effectSize;

		// Token: 0x0400132B RID: 4907
		private List<LaserEffect> laserEffects = new List<LaserEffect>();
	}
}
