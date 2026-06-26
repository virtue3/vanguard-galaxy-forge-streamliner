using System;
using System.Collections;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Galaxy;
using UnityEngine;

namespace Source.SpaceShip
{
	// Token: 0x02000057 RID: 87
	public abstract class AutoActions
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0001CE98 File Offset: 0x0001B098
		protected virtual float leaveTriggerTime
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0001CE9F File Offset: 0x0001B09F
		protected virtual bool automaticallyLeave
		{
			get
			{
				AbstractUnit abstractUnit = this.parent;
				return abstractUnit != null && !abstractUnit.IsEnemy(Faction.player);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0001CEBB File Offset: 0x0001B0BB
		// (set) Token: 0x06000366 RID: 870 RVA: 0x0001CEC3 File Offset: 0x0001B0C3
		public bool leaving { get; protected set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0001CECC File Offset: 0x0001B0CC
		public virtual bool skipLeavePosition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000368 RID: 872
		public abstract void OnDamageTaken(DamageData data);

		// Token: 0x06000369 RID: 873 RVA: 0x0001CECF File Offset: 0x0001B0CF
		public AutoActions(AbstractUnit parent)
		{
			this.parent = parent;
			this.spaceShip = (parent as Behaviour.Unit.SpaceShip);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0001CEEC File Offset: 0x0001B0EC
		public static AutoActions Create(string name, AbstractUnit parent)
		{
			return (AutoActions)Type.GetType("Source.Behaviour.Unit.SpaceShip.Auto." + name + "Actions").GetConstructor(new Type[]
			{
				typeof(AbstractUnit)
			}).Invoke(new object[]
			{
				parent
			});
		}

		// Token: 0x0600036B RID: 875
		public abstract bool DoWeRespawn();

		// Token: 0x0600036C RID: 876 RVA: 0x0001CF3A File Offset: 0x0001B13A
		public virtual void LeaveMissionTrigger()
		{
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001CF3C File Offset: 0x0001B13C
		public virtual void SpaceShipHasArrived()
		{
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0001CF40 File Offset: 0x0001B140
		public virtual void Update(float delta)
		{
			if (this.spaceShip == null)
			{
				return;
			}
			if (!this.parent.unitData.travelling || !this.leaving)
			{
				if (!this.leaving && this.automaticallyLeave)
				{
					this.leaveTimer += delta;
					if (this.leaveTimer > this.leaveTriggerTime)
					{
						this.StartExitCoroutine();
					}
				}
				return;
			}
			if (Vector2.Distance(this.spaceShip.targetablePosition, this.leavePos) < 3f)
			{
				this.RemoveUnit();
				return;
			}
			this.spaceShip.unitData.travelSpeed = Mathf.Clamp(this.spaceShip.unitData.travelSpeed + this.spaceShip.baseWarpAcceleration * Time.deltaTime, 0f, 50f);
			this.spaceShip.MoveTowards(this.leavePos);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0001D01F File Offset: 0x0001B21F
		public void ExitPOI()
		{
			this.StartExitCoroutine();
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0001D028 File Offset: 0x0001B228
		protected virtual void RemoveUnit()
		{
			UnityEngine.Object.Destroy(this.spaceShip.gameObject);
			if (MapPointOfInterest.current != null)
			{
				MapPointOfInterest.current.RemoveUnit(this.spaceShip.unitData);
				if (this.DoWeRespawn())
				{
					this.parent.SetCurrentHP(true);
					this.parent.unitData.cargo.Clear();
					this.AddRespawnPayload();
					return;
				}
				this.LeaveMissionTrigger();
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0001D098 File Offset: 0x0001B298
		private void AddRespawnPayload()
		{
			MapTriggeredPayload mapTriggeredPayload = new MapTriggeredPayload(MapPointOfInterest.current)
			{
				triggerTime = (float)SeededRandom.Global.RandomRange(40, 60),
				spawnAtPlayer = false
			};
			mapTriggeredPayload.AddUnit(this.parent.unitData);
			MapPointOfInterest.current.AddPayload(mapTriggeredPayload);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
		protected virtual void SetLeavePos()
		{
			this.leavePos = this.spaceShip.targetablePosition + new Vector2((float)SeededRandom.Global.RandomRange(-30, 30), (float)(SeededRandom.Global.RandomBool(0.5f) ? 40 : -40));
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001D137 File Offset: 0x0001B337
		protected void StartExitCoroutine()
		{
			if (this.leaving)
			{
				return;
			}
			this.SetLeavePos();
			this.spaceShip.StartCoroutine(this.StartExit());
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001D15A File Offset: 0x0001B35A
		private IEnumerator StartExit()
		{
			this.leaving = true;
			if (!this.skipLeavePosition)
			{
				this.SetInitialJumpPosition();
				this.spaceShip.SetOverrideDestination(this.initialJumpPosition, false, false, false);
				yield return new WaitUntil(() => Vector2.Distance(this.spaceShip.targetablePosition, this.initialJumpPosition) < 1f);
			}
			else
			{
				yield return new WaitForSeconds(3f);
			}
			yield return this.spaceShip.PrepareForInSystemTravel(this.leavePos, false);
			yield break;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001D16C File Offset: 0x0001B36C
		protected virtual void SetInitialJumpPosition()
		{
			bool flag = this.leavePos.y < 0f;
			this.initialJumpPosition = this.spaceShip.targetablePosition + new Vector2((float)SeededRandom.Global.RandomRange(-3, 3), (float)SeededRandom.Global.RandomRange(flag ? -3 : 1, flag ? -1 : 3));
		}

		// Token: 0x040001E4 RID: 484
		protected AbstractUnit parent;

		// Token: 0x040001E5 RID: 485
		protected Behaviour.Unit.SpaceShip spaceShip;

		// Token: 0x040001E6 RID: 486
		protected Vector2 initialJumpPosition;

		// Token: 0x040001E7 RID: 487
		protected Vector2 leavePos;

		// Token: 0x040001E8 RID: 488
		protected float leaveTimer;
	}
}
