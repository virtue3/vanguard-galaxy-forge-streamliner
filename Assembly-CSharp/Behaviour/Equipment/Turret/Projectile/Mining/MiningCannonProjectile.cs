using System;
using System.Linq;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile.Mining
{
	// Token: 0x02000354 RID: 852
	public class MiningCannonProjectile : AbstractMiningProjectile
	{
		// Token: 0x06002076 RID: 8310 RVA: 0x000BEB54 File Offset: 0x000BCD54
		protected override void HandleDirectImpact()
		{
			if (this.canMineCore && this.damageData.source && this.initialAsteroid.hasInnerCore)
			{
				DamageData damageData = AreaDamageHelper.CreateNewDamageData(this.damageData, this.canMineCore, base.transform, this.explosionRadius, false, null);
				UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(this.initialAsteroid.coreCollider, damageData, base.name, base.transform);
				this.initialAsteroid.TakeInnerCoreDamage(damageData, true);
				UnityEngine.Object.Destroy(obj);
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x000BEBD7 File Offset: 0x000BCDD7
		protected override void TriggerAfterEffects()
		{
			this.Explode();
		}

		// Token: 0x06002078 RID: 8312 RVA: 0x000BEBE0 File Offset: 0x000BCDE0
		protected override void PlayImpactEffect()
		{
			Singleton<EffectManager>.Instance.PlayExplosionEffect(base.transform.position, Vector2.zero, 2f, this.initialAsteroid.surfaceItem.depositColor, 0f);
			Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(base.transform.position, 0.7f, 0f);
			PhysicsInteraction.ApplyShockwaveToNearbyShips(base.transform.position, 0.2f, 0.62f);
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000BEC6C File Offset: 0x000BCE6C
		private void Explode()
		{
			if (!this.damageData.source)
			{
				return;
			}
			foreach (Collider2D collider2D in (from collider in Physics2D.OverlapCircleAll(base.transform.position, this.explosionRadius)
			orderby (collider.transform.position - base.transform.position).sqrMagnitude
			select collider).ToArray<Collider2D>())
			{
				if (!(collider2D == this.initialAsteroid.surfaceCollider) && !(collider2D == this.initialAsteroid.coreCollider))
				{
					Asteroid component = collider2D.GetComponent<Asteroid>();
					if (component != null)
					{
						Asteroid asteroid = component;
						DamageData damageData = AreaDamageHelper.CreateNewDamageData(this.damageData, this.canMineCore, base.transform, this.explosionRadius, true, collider2D);
						UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(collider2D, damageData, base.name, base.transform);
						if (this.canMineSurface && !asteroid.IsSurfaceOreDepleted())
						{
							asteroid.TakeSurfaceDamage(damageData);
						}
						UnityEngine.Object.Destroy(obj);
					}
				}
			}
		}

		// Token: 0x04001366 RID: 4966
		private float explosionRadius = 8f;
	}
}
