using System;
using Behaviour.Equipment.Module;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Data;
using Source.Galaxy;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x0200006B RID: 107
	public class SecurityPatrolActions : AutoActions
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x0001F1CD File Offset: 0x0001D3CD
		protected override bool automaticallyLeave
		{
			get
			{
				return this.leave;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001F1D5 File Offset: 0x0001D3D5
		protected override float leaveTriggerTime
		{
			get
			{
				return 10f;
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001F1DC File Offset: 0x0001D3DC
		public SecurityPatrolActions(AbstractUnit parent) : base(parent)
		{
			this.worldCoordinates = BasePoiManager.current.worldCoordinates;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001F1F8 File Offset: 0x0001D3F8
		public override void Update(float delta)
		{
			base.Update(delta);
			if (!this.spaceShip || MapPointOfInterest.current == null)
			{
				return;
			}
			if (this.spaceShip.targetProvider && !this.spaceShip.targetProvider.priorityTarget)
			{
				foreach (AbstractUnitData abstractUnitData in MapPointOfInterest.current.GetUnits(false))
				{
					if (abstractUnitData.IsEnemy(this.spaceShip))
					{
						this.spaceShip.targetProvider.SetManualTarget(abstractUnitData.unit);
						this.spaceShip.ClearOverrideDestination();
						this.spaceShip.maxSpeed = -1f;
					}
				}
			}
			AbstractTargetingModule targetProvider = this.spaceShip.targetProvider;
			if (!((targetProvider != null) ? targetProvider.priorityTarget : null))
			{
				this.spaceShip.maxSpeed = 2f;
				if (!this.visitedLeft)
				{
					Vector2 b = new Vector2(this.worldCoordinates.xMin, this.spaceShip.rigidbody.position.y);
					if (Vector2.Distance(this.spaceShip.rigidbody.position, b) < 3f)
					{
						this.visitedLeft = true;
					}
					else
					{
						this.spaceShip.SetOverrideDestination(new Vector2(this.worldCoordinates.xMin, this.spaceShip.rigidbody.position.y), false, false, false);
					}
				}
				else if (!this.visitedRight)
				{
					Vector2 b2 = new Vector2(this.worldCoordinates.xMax, this.spaceShip.rigidbody.position.y);
					if (Vector2.Distance(this.spaceShip.rigidbody.position, b2) < 3f)
					{
						this.visitedRight = true;
					}
					else
					{
						this.spaceShip.SetOverrideDestination(new Vector2(this.worldCoordinates.xMax, this.spaceShip.rigidbody.position.y), false, false, false);
					}
				}
			}
			if (this.visitedLeft && this.visitedRight)
			{
				this.leave = true;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001F430 File Offset: 0x0001D630
		public override void OnDamageTaken(DamageData data)
		{
			if (this.parent.fractionHullHP < 0.2f)
			{
				base.StartExitCoroutine();
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001F44A File Offset: 0x0001D64A
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x0400022E RID: 558
		public const float FleeHP = 0.2f;

		// Token: 0x0400022F RID: 559
		private bool leave;

		// Token: 0x04000230 RID: 560
		private Rect worldCoordinates;

		// Token: 0x04000231 RID: 561
		private bool visitedLeft;

		// Token: 0x04000232 RID: 562
		private bool visitedRight;
	}
}
