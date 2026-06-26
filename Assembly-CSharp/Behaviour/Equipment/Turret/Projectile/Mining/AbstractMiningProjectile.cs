using System;
using Behaviour.Mining;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile.Mining
{
	// Token: 0x02000352 RID: 850
	public abstract class AbstractMiningProjectile : AbstractProjectile
	{
		// Token: 0x06002069 RID: 8297 RVA: 0x000BE8CA File Offset: 0x000BCACA
		protected override void Update()
		{
			base.transform.Translate(Vector3.right * this.speed * Time.deltaTime);
			this.CheckForAsteroidHit();
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000BE8F8 File Offset: 0x000BCAF8
		private void CheckForAsteroidHit()
		{
			foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(base.transform.position, this.spriteRenderer.bounds.extents.y))
			{
				Asteroid x;
				if (this.canMineSurface && collider2D.TryGetComponent<Asteroid>(out x) && x == this.damageData.targetUnit)
				{
					this.initialAsteroid = x;
					this.HandleAsteroidHit(collider2D);
					return;
				}
				Asteroid x2;
				if (this.canMineCore && collider2D.TryGetComponentInImmediateParent(out x2) && x2 == this.damageData.targetUnit)
				{
					this.initialAsteroid = x2;
					this.HandleAsteroidHit(collider2D);
					return;
				}
				TargetableUnit targetableUnit;
				if (collider2D.TryGetComponent<TargetableUnit>(out targetableUnit) && targetableUnit.damagableByAll)
				{
					targetableUnit.TakeDamage(this.damageData);
					this.PlayImpactEffect();
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x0600206B RID: 8299 RVA: 0x000BE9E8 File Offset: 0x000BCBE8
		private void HandleAsteroidHit(Collider2D collider)
		{
			this.OnHit(collider);
			this.PlayImpactEffect();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x000BEA02 File Offset: 0x000BCC02
		protected override void OnHit(Collider2D collision)
		{
			this.HandleDirectImpact();
			this.TriggerAfterEffects();
		}

		// Token: 0x0600206D RID: 8301
		protected abstract void HandleDirectImpact();

		// Token: 0x0600206E RID: 8302
		protected abstract void TriggerAfterEffects();

		// Token: 0x0600206F RID: 8303
		protected abstract void PlayImpactEffect();

		// Token: 0x04001362 RID: 4962
		[SerializeField]
		protected float yieldBonus;

		// Token: 0x04001363 RID: 4963
		[SerializeField]
		protected bool canMineSurface;

		// Token: 0x04001364 RID: 4964
		[SerializeField]
		protected bool canMineCore;

		// Token: 0x04001365 RID: 4965
		protected Asteroid initialAsteroid;
	}
}
