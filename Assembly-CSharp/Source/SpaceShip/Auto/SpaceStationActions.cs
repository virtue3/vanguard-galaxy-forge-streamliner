using System;
using Behaviour.Managers;
using Behaviour.Spacestation.Docking;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x0200006C RID: 108
	public class SpaceStationActions : AutoActions
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0001F44D File Offset: 0x0001D64D
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001F450 File Offset: 0x0001D650
		public SpaceStationActions(AbstractUnit parent) : base(parent)
		{
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001F47C File Offset: 0x0001D67C
		public void CheckDockingState()
		{
			DockingOption dockingOption = SpacestationExteriorManager.Instance.FindDockingOptionAtPosition(this.spaceShip.rigidbody.position, this.spaceShip);
			DockingState? dockingState;
			DockingState dockingState2;
			if (dockingOption)
			{
				dockingState = this.spaceShip.spaceShipData.dockingState;
				dockingState2 = DockingState.Docked;
				if (dockingState.GetValueOrDefault() == dockingState2 & dockingState != null)
				{
					dockingOption.AssignSpaceshipForDocking(this.spaceShip, true);
					this.spaceShip.spaceShipData.dockingState = new DockingState?(DockingState.Docked);
					this.dockTimer = this.dockTime;
					return;
				}
			}
			dockingState = this.spaceShip.spaceShipData.dockingState;
			dockingState2 = DockingState.Arriving;
			if (dockingState.GetValueOrDefault() == dockingState2 & dockingState != null)
			{
				this.spaceShip.StartLandNpcAtPoiCoroutine(SpacestationExteriorManager.Instance.GetLandingPosition(this.spaceShip));
				return;
			}
			dockingState = this.spaceShip.spaceShipData.dockingState;
			dockingState2 = DockingState.Docking;
			if (dockingState.GetValueOrDefault() == dockingState2 & dockingState != null)
			{
				SpacestationExteriorManager.Instance.AssignClosestDockingOption(this.spaceShip, false);
				return;
			}
			if (SpacestationExteriorManager.Instance.DockingOptionAvailableForSize(this.spaceShip.shipRoleType.GetDockingOptionSize()))
			{
				SpacestationExteriorManager.Instance.AssignClosestDockingOption(this.spaceShip, false);
				return;
			}
			this.spaceShip.spaceShipData.dockingState = new DockingState?(DockingState.Leaving);
			base.StartExitCoroutine();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001F5D4 File Offset: 0x0001D7D4
		public override void Update(float delta)
		{
			BasePoiManager current = BasePoiManager.current;
			if (current != null && !current.initializedAndReady)
			{
				return;
			}
			if (base.leaving)
			{
				base.Update(delta);
				return;
			}
			DockingState? dockingState = this.spaceShip.spaceShipData.dockingState;
			DockingState dockingState2 = DockingState.Docked;
			if (dockingState.GetValueOrDefault() == dockingState2 & dockingState != null)
			{
				if (this.dockTimer == -1f)
				{
					this.dockTimer = this.dockTime;
				}
				else if (this.dockTimer <= 0f)
				{
					DockingOption dockingOption = SpacestationExteriorManager.Instance.FindDockingOption(this.spaceShip);
					if (dockingOption)
					{
						dockingOption.StartUndockCoroutine();
					}
					else
					{
						Debug.LogWarning("DockingOption is null!");
					}
					this.dockTimer = -1f;
				}
				this.dockTimer -= delta;
			}
			dockingState = this.spaceShip.spaceShipData.dockingState;
			dockingState2 = DockingState.Arriving;
			if ((dockingState.GetValueOrDefault() == dockingState2 & dockingState != null) && !this.spaceShip.unitData.travelling)
			{
				if (!SpacestationExteriorManager.Instance.AssignClosestDockingOption(this.spaceShip, false))
				{
					base.StartExitCoroutine();
					return;
				}
			}
			else
			{
				dockingState = this.spaceShip.spaceShipData.dockingState;
				dockingState2 = DockingState.Leaving;
				if ((dockingState.GetValueOrDefault() == dockingState2 & dockingState != null) && !base.leaving)
				{
					base.StartExitCoroutine();
				}
			}
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001F723 File Offset: 0x0001D923
		protected override void SetLeavePos()
		{
			this.leavePos = SpacestationExteriorManager.Instance.GetFittingExitPosition(this.spaceShip);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001F73B File Offset: 0x0001D93B
		protected override void SetInitialJumpPosition()
		{
			this.initialJumpPosition = SpacestationExteriorManager.Instance.GetClosestWarpInPosition(this.spaceShip);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001F753 File Offset: 0x0001D953
		public override void OnDamageTaken(DamageData data)
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001F755 File Offset: 0x0001D955
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001F758 File Offset: 0x0001D958
		public override void LeaveMissionTrigger()
		{
		}

		// Token: 0x04000233 RID: 563
		private float dockTime = (float)SeededRandom.Global.RandomRange(30, 60);

		// Token: 0x04000234 RID: 564
		private float dockTimer = -1f;
	}
}
