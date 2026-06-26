using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.MissionSystem;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000062 RID: 98
	public class AmbientMinerActions : AutoActions
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0001E64A File Offset: 0x0001C84A
		protected override float leaveTriggerTime
		{
			get
			{
				return (float)SeededRandom.Global.RandomRange(60, 120);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0001E65B File Offset: 0x0001C85B
		public AmbientMinerActions(AbstractUnit parent) : base(parent)
		{
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0001E664 File Offset: 0x0001C864
		public override void Update(float delta)
		{
			base.Update(delta);
			this.targetTimer -= Time.deltaTime;
			if (this.targetTimer < 0f)
			{
				this.targetTimer = 8f;
				if (this.parent.targetProvider)
				{
					TargetableUnit priorityTarget = this.parent.targetProvider.priorityTarget;
					if (priorityTarget != null && priorityTarget.IsTargetedBy(GameplayManager.Instance.spaceShip))
					{
						this.parent.targetProvider.ResetAutoTarget();
					}
				}
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001E6EC File Offset: 0x0001C8EC
		public override bool DoWeRespawn()
		{
			return this.parent.fractionHullHP > 0.6f;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0001E700 File Offset: 0x0001C900
		public override void LeaveMissionTrigger()
		{
			MissionObjective.Trigger(MissionTrigger.MinerChasedOff, this.parent.unitData, null, false);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001E716 File Offset: 0x0001C916
		public override void OnDamageTaken(DamageData data)
		{
			if (this.parent.fractionHullHP < 0.6f)
			{
				base.StartExitCoroutine();
			}
		}

		// Token: 0x0400021D RID: 541
		public const float FleeHP = 0.6f;

		// Token: 0x0400021E RID: 542
		private float targetTimer;
	}
}
