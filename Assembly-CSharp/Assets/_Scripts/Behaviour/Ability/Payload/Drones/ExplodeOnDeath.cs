using System;
using System.Linq;
using Behaviour.Ability;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Assets._Scripts.Behaviour.Ability.Payload.Drones
{
	// Token: 0x020001A2 RID: 418
	public class ExplodeOnDeath : TriggeredPayload
	{
		// Token: 0x06000EB3 RID: 3763 RVA: 0x00068AA4 File Offset: 0x00066CA4
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			AbstractUnit componentInParent = base.GetComponentInParent<AbstractUnit>();
			if (componentInParent != null)
			{
				DamageData damageData2 = new DamageData(componentInParent.gameObject)
				{
					damageAmount = componentInParent.maxTotalHP * (this.damagePercentage * (float)stackSize),
					type = DamageType.Heat
				};
				this.Explode(damageData2);
				this.PlayImpactEffect();
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00068AFE File Offset: 0x00066CFE
		private void PlayImpactEffect()
		{
			Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(base.transform.position, 0.3f, 0f);
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00068B24 File Offset: 0x00066D24
		private void Explode(DamageData damageData)
		{
			if (!damageData.source)
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
					DamageData damageData2 = AreaDamageHelper.CreateNewDamageData(damageData, base.transform, this.explosionRadius, collider2D, true);
					UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(collider2D, damageData2, base.name, base.transform);
					abstractUnit.TakeDamage(damageData2);
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x04000849 RID: 2121
		[SerializeField]
		private float damagePercentage = 0.5f;

		// Token: 0x0400084A RID: 2122
		[SerializeField]
		private float explosionRadius = 12f;
	}
}
