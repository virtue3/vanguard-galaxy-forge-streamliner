using System;
using Behaviour.Managers;
using Behaviour.Util;
using UnityEngine;
using _Scripts.Behaviour.Effects.Weapon;

namespace Behaviour.Equipment.Turret.Projectile
{
	// Token: 0x02000351 RID: 849
	public class PlasmaCannonProjectile : AutocannonProjectile
	{
		// Token: 0x06002066 RID: 8294 RVA: 0x000BE864 File Offset: 0x000BCA64
		protected override void Update()
		{
			base.Update();
			this.plasmaEffect.SetAngle(base.transform.rotation.eulerAngles.z);
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000BE89A File Offset: 0x000BCA9A
		protected override void OnHit(Collider2D collision)
		{
			base.OnHit(collision);
			Singleton<EffectManager>.Instance.PlayPlasmaExplosionEffect(base.transform.position, 0.5f);
		}

		// Token: 0x04001361 RID: 4961
		[SerializeField]
		private PlasmaEffect plasmaEffect;
	}
}
