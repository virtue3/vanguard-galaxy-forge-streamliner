using System;
using Behaviour.Managers;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile.Mining
{
	// Token: 0x02000353 RID: 851
	public class MiningAutocannonProjectile : AbstractMiningProjectile
	{
		// Token: 0x06002071 RID: 8305 RVA: 0x000BEA18 File Offset: 0x000BCC18
		protected override void HandleDirectImpact()
		{
			if (!this.damageData.source)
			{
				return;
			}
			if (this.canMineSurface && !this.initialAsteroid.IsSurfaceOreDepleted())
			{
				DamageData damageData = this.CreateNewDamageData();
				UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(this.initialAsteroid.surfaceCollider, damageData, base.name, base.transform);
				this.initialAsteroid.TakeSurfaceDamage(damageData);
				UnityEngine.Object.Destroy(obj);
			}
			if (this.canMineCore)
			{
				DamageData damageData2 = this.CreateNewDamageData();
				UnityEngine.Object obj2 = AsteroidHelper.SetTrackingTargetData(this.initialAsteroid.coreCollider, damageData2, base.name, base.transform);
				this.initialAsteroid.TakeInnerCoreDamage(damageData2, true);
				UnityEngine.Object.Destroy(obj2);
			}
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x000BEAC0 File Offset: 0x000BCCC0
		protected override void TriggerAfterEffects()
		{
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x000BEAC2 File Offset: 0x000BCCC2
		protected override void PlayImpactEffect()
		{
			Singleton<EffectManager>.Instance.PlayFlashEffect(base.transform.position, this.damageData.effectColor, this.damageData.damageAmount / 5f);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x000BEAFC File Offset: 0x000BCCFC
		private DamageData CreateNewDamageData()
		{
			return new DamageData(this.damageData.source)
			{
				damageAmount = this.damageData.damageAmount,
				power = this.damageData.power,
				yield = this.damageData.yield
			};
		}
	}
}
