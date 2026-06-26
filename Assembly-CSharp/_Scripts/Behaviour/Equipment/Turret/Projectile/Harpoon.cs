using System;
using Behaviour.Equipment.Turret.Projectile;
using Behaviour.Salvage;
using Behaviour.Weapons;
using UnityEngine;
using _Scripts.Behaviour.Equipment.Turret.Salvage;

namespace _Scripts.Behaviour.Equipment.Turret.Projectile
{
	// Token: 0x02000199 RID: 409
	public class Harpoon : AbstractProjectile
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x00068162 File Offset: 0x00066362
		protected override bool destroyOnHit
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00068165 File Offset: 0x00066365
		public void Initialize(DamageData damageData, float projectileSpeed, SalvageStructuralGrinder source)
		{
			base.Initialize(damageData, projectileSpeed);
			this.source = source;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00068178 File Offset: 0x00066378
		protected override void Update()
		{
			if (this.speed != 0f)
			{
				this.destroyTimer += Time.deltaTime;
				if (this.destroyTimer > 3f)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					SalvageStructuralGrinder salvageStructuralGrinder = this.source;
					if (salvageStructuralGrinder != null)
					{
						salvageStructuralGrinder.DestroySpring();
					}
				}
				base.Update();
			}
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x000681D4 File Offset: 0x000663D4
		protected override void OnHit(Collider2D collision)
		{
			this.speed = 0f;
			SalvageContainer componentInChildren = collision.GetComponentInChildren<SalvageContainer>();
			componentInChildren.gameObject.layer = LayerMask.NameToLayer("Default");
			base.transform.parent = componentInChildren.transform;
			this.source.OnHarpoonHit(componentInChildren, this.damageData.hitTransform);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00068230 File Offset: 0x00066430
		protected override bool IsTarget(Collider2D collider)
		{
			SalvageContainer component = collider.GetComponent<SalvageContainer>();
			return component && component.data.HasStructuralContent();
		}

		// Token: 0x0400082E RID: 2094
		private SalvageStructuralGrinder source;

		// Token: 0x0400082F RID: 2095
		private float destroyTimer;
	}
}
