using System;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile
{
	// Token: 0x0200034E RID: 846
	public class AutocannonProjectile : AbstractProjectile
	{
		// Token: 0x06002055 RID: 8277 RVA: 0x000BE193 File Offset: 0x000BC393
		protected override void OnHit(Collider2D collision)
		{
			IDamageable component = collision.GetComponent<IDamageable>();
			if (component == null)
			{
				return;
			}
			component.TakeDamage(this.damageData);
		}
	}
}
