using System;
using Behaviour.Equipment.Turret.Projectile;
using Behaviour.Mining;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Turret
{
	// Token: 0x02000345 RID: 837
	public class MiningCannonTurret : AbstractMiningTurret
	{
		// Token: 0x0600200C RID: 8204 RVA: 0x000BCF26 File Offset: 0x000BB126
		protected override void UpdateForCollider()
		{
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000BCF28 File Offset: 0x000BB128
		protected override void RandomizeTrackingTarget()
		{
			this.shotReset = false;
			if (!base.asteroidTarget)
			{
				return;
			}
			if (base.targetsCore)
			{
				base.trackingTarget.transform.position = base.asteroidTarget.surfaceCollider.ClosestPoint(base.transform.position);
			}
			if (base.targetsSurface)
			{
				base.trackingTarget.transform.localPosition = SeededRandom.Global.Choose<Vector2>(base.asteroidTarget.coreCollider.points);
			}
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x000BCFBE File Offset: 0x000BB1BE
		public override bool CanMineAsteroidTarget(Asteroid target)
		{
			return target && ((!target.surfaceAmountDepleted && base.targetsSurface) || (!target.innerCoreDepleted && base.targetsCore));
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x000BCFF0 File Offset: 0x000BB1F0
		protected override bool FireInternal()
		{
			Quaternion rhs = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.accuracyAngle, this.accuracyAngle));
			Quaternion rotation = this.firePoints[this.firePointIndex].rotation * rhs;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.projectilePrefab, this.firePoints[this.firePointIndex].position, rotation);
			base.PlayFiringSound();
			AbstractProjectile abstractProjectile;
			if (gameObject.TryGetComponent<AbstractProjectile>(out abstractProjectile))
			{
				float num = 0f;
				Vector3 zero = Vector3.zero;
				this.firePoints[this.firePointIndex].rotation.ToAngleAxis(out num, out zero);
				abstractProjectile.Initialize(this.CreateDamageData(null, null, TargetLayer.Surface), base.projectileSpeed + base.parent.speed);
				abstractProjectile.SetTargetUnit(base.currentTarget);
				base.TriggerFireProjectile(abstractProjectile);
			}
			else
			{
				Debug.LogWarning("Projectile prefab does not have an AutoCannonProjectile component.");
			}
			base.StartCoroutine(this.PlayWeaponEffect());
			return true;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x000BD0E8 File Offset: 0x000BB2E8
		protected override float GetPowerMultiplier()
		{
			return 1.2f;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000BD0EF File Offset: 0x000BB2EF
		protected override DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			DamageData damageData = base.CreateDamageData(targetTransform, hitCoordinates, targetLayer);
			damageData.effectColor = this.color;
			return damageData;
		}

		// Token: 0x04001320 RID: 4896
		[Header("AutoCannon Specific")]
		public GameObject projectilePrefab;

		// Token: 0x04001321 RID: 4897
		public Color color;
	}
}
