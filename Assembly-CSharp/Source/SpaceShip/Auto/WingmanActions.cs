using System;
using Behaviour.Equipment.Module;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Crew;
using Source.Galaxy;
using Source.Player;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x0200006F RID: 111
	public class WingmanActions : AutoActions
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001FAA6 File Offset: 0x0001DCA6
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001FAA9 File Offset: 0x0001DCA9
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0001FAB1 File Offset: 0x0001DCB1
		public Vector2 formationPos { get; protected set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x0001FABA File Offset: 0x0001DCBA
		public Vector2 relativeFormationPos
		{
			get
			{
				return this.leader.transform.rotation * this.formationPos;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001FAE1 File Offset: 0x0001DCE1
		public WingmanActions(AbstractUnit parent) : base(parent)
		{
			this.SetLeader();
			Debug.Log("Create wingman actions for: " + ((parent != null) ? parent.ToString() : null) + ", leader: " + this.leader);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001FB1C File Offset: 0x0001DD1C
		private void SetLeader()
		{
			this.leader = GameplayManager.Instance.spaceShip;
			SpaceShipData spaceShipData = this.parent.unitData as SpaceShipData;
			this.mercenary = (((spaceShipData != null) ? spaceShipData.commanderData : null) as Mercenary);
			if (this.leader)
			{
				this.formationPos = this.spaceShip.GetFormationPos(this.leader);
			}
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001FB84 File Offset: 0x0001DD84
		public override void Update(float delta)
		{
			base.Update(delta);
			if (this.leader != GameplayManager.Instance.spaceShip)
			{
				this.SetLeader();
			}
			if (GameplayManager.Instance.spaceShip && !this.parent.unitData.travelling && this.mercenary != null && !base.leaving && this.leader)
			{
				TravelManager instance = Singleton<TravelManager>.Instance;
				if (instance == null || !instance.TravelActive())
				{
					WingmanBehaviour behaviour = this.mercenary.behaviour;
					bool flag = true;
					AbstractTargetingModule targetProvider = this.spaceShip.targetProvider;
					TargetableUnit x = (targetProvider != null) ? targetProvider.priorityTarget : null;
					AbstractTargetingModule targetProvider2 = this.leader.targetProvider;
					TargetableUnit targetableUnit = (targetProvider2 != null) ? targetProvider2.priorityTarget : null;
					if (behaviour == WingmanBehaviour.Formation)
					{
						if (targetableUnit)
						{
							AbstractTargetingModule targetProvider3 = this.spaceShip.targetProvider;
							if (targetProvider3 != null && targetProvider3.IsValidTarget(targetableUnit))
							{
								AbstractTargetingModule targetProvider4 = this.spaceShip.targetProvider;
								if (targetProvider4 != null)
								{
									targetProvider4.SetManualTarget(targetableUnit);
								}
							}
						}
					}
					else if (behaviour == WingmanBehaviour.Defensive)
					{
						AbstractUnit highestDamageDealer = this.leader.GetHighestDamageDealer();
						if (highestDamageDealer != null)
						{
							AbstractTargetingModule targetProvider5 = this.spaceShip.targetProvider;
							if (targetProvider5 != null && targetProvider5.IsValidTarget(highestDamageDealer))
							{
								if (x != highestDamageDealer)
								{
									this.spaceShip.targetProvider.SetManualTarget(highestDamageDealer);
								}
								flag = false;
								goto IL_1D8;
							}
						}
						if (targetableUnit)
						{
							if (x != targetableUnit)
							{
								AbstractTargetingModule targetProvider6 = this.spaceShip.targetProvider;
								if (targetProvider6 != null && targetProvider6.IsValidTarget(targetableUnit))
								{
									AbstractTargetingModule targetProvider7 = this.spaceShip.targetProvider;
									if (targetProvider7 != null)
									{
										targetProvider7.SetManualTarget(targetableUnit);
									}
								}
							}
						}
						else
						{
							highestDamageDealer = this.spaceShip.GetHighestDamageDealer();
							if (highestDamageDealer)
							{
								flag = false;
							}
						}
					}
					else
					{
						AbstractTargetingModule targetProvider8 = this.spaceShip.targetProvider;
						if (((targetProvider8 != null) ? targetProvider8.priorityTarget : null) != null)
						{
							flag = false;
						}
					}
					IL_1D8:
					if (!flag)
					{
						this.spaceShip.ClearOverrideDestination();
						return;
					}
					this.spaceShip.SetOverrideDestination(this.leader.transform.position + this.relativeFormationPos + this.leader.rigidbody.linearVelocity, false, false, false);
					if (Vector2.Distance(this.spaceShip.overrideTarget.Value, this.parent.transform.position) < this.parent.radius * 3f)
					{
						this.spaceShip.forceWorldAngle = new Quaternion?(this.leader.transform.rotation);
						return;
					}
					this.spaceShip.forceWorldAngle = null;
					return;
				}
			}
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001FE34 File Offset: 0x0001E034
		protected override void RemoveUnit()
		{
			if (GamePlayer.current.hiredMercenary != null && this.mercenary != null && !this.mercenary.isExtra)
			{
				GamePlayer.current.RemoveHiredMercenary(false);
				return;
			}
			GameplayManager.Instance.RemoveFleetShip(this.spaceShip);
			GamePlayer.current.RemoveFromFleet(this.parent.unitData as SpaceShipData);
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001FE98 File Offset: 0x0001E098
		public void LeavePlayer()
		{
			if (base.leaving)
			{
				return;
			}
			if (this.spaceShip.unitData.travelling)
			{
				this.leavePos = SystemMapData.current.GetDynamicMissionPosition();
				base.leaving = true;
				return;
			}
			base.StartExitCoroutine();
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001FED4 File Offset: 0x0001E0D4
		public override void OnDamageTaken(DamageData data)
		{
			if (this.parent.fractionHullHP < 0.1f && !base.leaving)
			{
				Debug.Log("WingmanActions OnDamage, leaving!");
				this.mercenary.repairing = true;
				this.parent.unitData.StartFleetRepair(this.mercenary.repairTime);
				base.StartExitCoroutine();
			}
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001FF32 File Offset: 0x0001E132
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x0400023E RID: 574
		public const float FleeHP = 0.1f;

		// Token: 0x0400023F RID: 575
		private Behaviour.Unit.SpaceShip leader;

		// Token: 0x04000240 RID: 576
		private Mercenary mercenary;
	}
}
