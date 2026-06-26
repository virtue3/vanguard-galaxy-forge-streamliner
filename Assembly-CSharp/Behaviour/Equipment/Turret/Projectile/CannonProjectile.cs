using System;
using System.Linq;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile
{
	// Token: 0x0200034F RID: 847
	public class CannonProjectile : AutocannonProjectile
	{
		// Token: 0x06002057 RID: 8279 RVA: 0x000BE1B4 File Offset: 0x000BC3B4
		protected override void OnHit(Collider2D collision)
		{
			IDamageable component = collision.GetComponent<IDamageable>();
			if (component != null)
			{
				component.TakeDamage(this.damageData);
			}
			DamageData newDamageData = new DamageData(this.damageData.sourceUnit.gameObject)
			{
				damageAmount = this.damageData.damageAmount * this.damagePercentage,
				type = DamageType.Heat
			};
			this.Explode(newDamageData);
			this.PlayImpactEffect();
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000BE21A File Offset: 0x000BC41A
		private void PlayImpactEffect()
		{
			Singleton<EffectManager>.Instance.PlayExplosionEffect(base.transform.position, Vector2.zero, 0.8f, ColorHelper.lightCyan, 0f);
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000BE24C File Offset: 0x000BC44C
		private void Explode(DamageData newDamageData)
		{
			if (!newDamageData.source)
			{
				return;
			}
			foreach (Collider2D collider2D in (from collider in Physics2D.OverlapCircleAll(base.transform.position, this.explosionRadius)
			orderby (collider.transform.position - base.transform.position).sqrMagnitude
			select collider).ToArray<Collider2D>())
			{
				AbstractUnit abstractUnit;
				if (collider2D.TryGetComponent<AbstractUnit>(out abstractUnit) && abstractUnit.IsPlayerEnemy())
				{
					DamageData damageData = AreaDamageHelper.CreateNewDamageData(newDamageData, base.transform, this.explosionRadius, collider2D, true);
					UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(collider2D, damageData, base.name, base.transform);
					abstractUnit.TakeDamage(damageData);
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x0400134C RID: 4940
		[SerializeField]
		private float damagePercentage = 0.2f;

		// Token: 0x0400134D RID: 4941
		[SerializeField]
		private float explosionRadius = 6f;
	}
}
