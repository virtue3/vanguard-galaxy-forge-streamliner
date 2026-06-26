using System;
using System.Linq;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D2 RID: 978
	public class ShieldDepletedAoeDamage : TriggeredPayload
	{
		// Token: 0x0600258D RID: 9613 RVA: 0x000D1770 File Offset: 0x000CF970
		public override bool ShouldTrigger(object source)
		{
			AbstractUnit componentInParent = base.GetComponentInParent<AbstractUnit>();
			return !(componentInParent == null) && !(componentInParent.shieldGeneratorModule == null);
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x000D17A0 File Offset: 0x000CF9A0
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			AbstractUnit componentInParent = base.GetComponentInParent<AbstractUnit>();
			if (componentInParent != null && componentInParent.shieldGeneratorModule != null)
			{
				DamageData damageData2 = new DamageData(componentInParent.gameObject)
				{
					damageAmount = componentInParent.maxShieldHP * (this.damagePercentage * (float)stackSize),
					type = DamageType.Energy
				};
				this.Explode(damageData2);
				this.PlayImpactEffect();
			}
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x000D1808 File Offset: 0x000CFA08
		private void PlayImpactEffect()
		{
			Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(base.transform.position, 0.7f, 0f);
			PhysicsInteraction.ApplyShockwaveToNearbyShips(base.transform.position, 1f, 0.62f);
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x000D1858 File Offset: 0x000CFA58
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

		// Token: 0x040016D6 RID: 5846
		[SerializeField]
		private float damagePercentage = 0.05f;

		// Token: 0x040016D7 RID: 5847
		[SerializeField]
		private float explosionRadius = 12f;

		// Token: 0x040016D8 RID: 5848
		private float damage;
	}
}
