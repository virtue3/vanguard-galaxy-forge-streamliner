using System;
using Behaviour.Equipment.Module;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.MissionSystem;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000063 RID: 99
	public class AmbientSalvagerActions : AutoActions
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0001E730 File Offset: 0x0001C930
		protected override float leaveTriggerTime
		{
			get
			{
				return (float)SeededRandom.Global.RandomRange(60, 120);
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001E741 File Offset: 0x0001C941
		public AmbientSalvagerActions(AbstractUnit parent) : base(parent)
		{
			this.salvageModule = parent.GetComponentInChildren<SalvageModule>();
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001E758 File Offset: 0x0001C958
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

		// Token: 0x060003BA RID: 954 RVA: 0x0001E7E0 File Offset: 0x0001C9E0
		public override bool DoWeRespawn()
		{
			return this.parent.fractionHullHP > 0.4f;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
		public override void LeaveMissionTrigger()
		{
			MissionObjective.Trigger(MissionTrigger.SalvagerChasedOff, this.parent.unitData, null, false);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0001E80A File Offset: 0x0001CA0A
		public override void OnDamageTaken(DamageData data)
		{
			if (this.parent.fractionHullHP < 0.4f)
			{
				base.StartExitCoroutine();
			}
		}

		// Token: 0x0400021F RID: 543
		public const float FleeHP = 0.4f;

		// Token: 0x04000220 RID: 544
		private SalvageModule salvageModule;

		// Token: 0x04000221 RID: 545
		private float targetTimer;
	}
}
