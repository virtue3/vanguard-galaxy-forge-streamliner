using System;
using System.Collections;
using Behaviour.Equipment.Turret.Projectile;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Turret.CombatTurrets
{
	// Token: 0x0200035C RID: 860
	public class MissileTurret : AbstractCombatTurret
	{
		// Token: 0x060020B5 RID: 8373 RVA: 0x000BF88B File Offset: 0x000BDA8B
		protected override bool FireInternal()
		{
			base.StartCoroutine(this.FireMissile());
			base.PlayFiringSound();
			return true;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000BF8A1 File Offset: 0x000BDAA1
		private IEnumerator FireMissile()
		{
			Missile missile = UnityEngine.Object.Instantiate<Missile>(this.missilePrefab, this.firePoints[this.firePointIndex].position + new Vector3(0f, 0f, 1f), this.firePoints[this.firePointIndex].rotation);
			missile.Initialize(base.trackingTarget.transform, this.CreateDamageData(null, null, TargetLayer.Surface), base.projectileSpeed, this.seekingMissiles, base.parent.rigidbody.linearVelocity);
			base.TriggerFireProjectile(missile);
			base.StartCoroutine(this.PlayWeaponEffect());
			yield return null;
			yield break;
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000BF8B0 File Offset: 0x000BDAB0
		protected override DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			DamageData damageData = base.CreateDamageData(targetTransform, hitCoordinates, targetLayer);
			damageData.effectColor = Color.red;
			return damageData;
		}

		// Token: 0x04001381 RID: 4993
		[Header("MissileTurret Specific")]
		public Missile missilePrefab;

		// Token: 0x04001382 RID: 4994
		public bool seekingMissiles;
	}
}
