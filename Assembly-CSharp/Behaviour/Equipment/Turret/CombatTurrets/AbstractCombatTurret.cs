using System;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.CombatTurrets
{
	// Token: 0x02000357 RID: 855
	public abstract class AbstractCombatTurret : AbstractTurret
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x000BF2A9 File Offset: 0x000BD4A9
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Combat;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x000BF2AC File Offset: 0x000BD4AC
		public override EquipStat powerStat
		{
			get
			{
				return EquipStat.CombatPower;
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000BF2B0 File Offset: 0x000BD4B0
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000BF2B8 File Offset: 0x000BD4B8
		protected override void RandomizeTrackingTarget()
		{
			this.shotReset = false;
			if (base.projectileSpeed > 0f)
			{
				return;
			}
			Vector2 v = base.currentTarget.surfaceCollider.ClosestPoint(this.firePoints[this.firePointIndex].position);
			base.trackingTarget.transform.position = v;
		}
	}
}
