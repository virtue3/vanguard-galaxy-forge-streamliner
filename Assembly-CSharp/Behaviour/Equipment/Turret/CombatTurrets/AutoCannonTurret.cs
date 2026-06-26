using System;
using Behaviour.Equipment.Turret.Projectile;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Turret.CombatTurrets
{
	// Token: 0x02000358 RID: 856
	public class AutoCannonTurret : AbstractCombatTurret
	{
		// Token: 0x0600209F RID: 8351 RVA: 0x000BF320 File Offset: 0x000BD520
		protected override bool FireInternal()
		{
			Quaternion rhs = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.accuracyAngle, this.accuracyAngle));
			Quaternion rotation = this.firePoints[this.firePointIndex].rotation * rhs;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.projectilePrefab, this.firePoints[this.firePointIndex].position, rotation);
			base.PlayFiringSound();
			AutocannonProjectile autocannonProjectile;
			if (gameObject.TryGetComponent<AutocannonProjectile>(out autocannonProjectile))
			{
				float num = 0f;
				Vector3 zero = Vector3.zero;
				this.firePoints[this.firePointIndex].rotation.ToAngleAxis(out num, out zero);
				autocannonProjectile.Initialize(this.CreateDamageData(null, null, TargetLayer.Surface), base.projectileSpeed);
				base.TriggerFireProjectile(autocannonProjectile);
			}
			else
			{
				Debug.LogWarning("Projectile prefab does not have an AutoCannonProjectile component.");
			}
			base.StartCoroutine(this.PlayWeaponEffect());
			return true;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000BF400 File Offset: 0x000BD600
		protected override DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			DamageData damageData = base.CreateDamageData(targetTransform, hitCoordinates, targetLayer);
			damageData.effectColor = this.color;
			return damageData;
		}

		// Token: 0x04001374 RID: 4980
		[Header("AutoCannon Specific")]
		public GameObject projectilePrefab;

		// Token: 0x04001375 RID: 4981
		public Color color;
	}
}
