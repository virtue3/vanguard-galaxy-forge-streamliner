using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Source.SpaceShip.Auto;
using UnityEngine;

namespace Behaviour.Spacestation.Docking
{
	// Token: 0x020002E5 RID: 741
	public abstract class DockingOption : MonoBehaviour
	{
		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x000A57D3 File Offset: 0x000A39D3
		// (set) Token: 0x06001AE6 RID: 6886 RVA: 0x000A57DB File Offset: 0x000A39DB
		public Transform dockingTransform { get; protected set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000A57E4 File Offset: 0x000A39E4
		// (set) Token: 0x06001AE8 RID: 6888 RVA: 0x000A57EC File Offset: 0x000A39EC
		public DockingOptionSize dockingOptionSize { get; protected set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000A57F5 File Offset: 0x000A39F5
		public bool occupied
		{
			get
			{
				return this.dockingSpaceship != null;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001AEA RID: 6890 RVA: 0x000A5803 File Offset: 0x000A3A03
		// (set) Token: 0x06001AEB RID: 6891 RVA: 0x000A580B File Offset: 0x000A3A0B
		public SpaceShip dockingSpaceship { get; protected set; }

		// Token: 0x06001AEC RID: 6892 RVA: 0x000A5814 File Offset: 0x000A3A14
		private void Update()
		{
			if (this.dockingSpaceship == null)
			{
				return;
			}
			if (Singleton<TravelManager>.Instance.TravelActive() && this.dockingSpaceship.IsPlayer(false))
			{
				if (this.dockingCoroutine != null && this.dockingSpaceship)
				{
					base.StopCoroutine(this.dockingCoroutine);
					this.dockingCoroutine = null;
					this.dockingSpaceship.transform.localScale = Vector3.one;
					this.dockingSpaceship.ToggleStateForDocking(true);
				}
				return;
			}
			DockingState? dockingState = this.dockingSpaceship.spaceShipData.dockingState;
			DockingState dockingState2 = DockingState.Leaving;
			if (!(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null))
			{
				dockingState = this.dockingSpaceship.spaceShipData.dockingState;
				dockingState2 = DockingState.Docked;
				if (!(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null))
				{
					dockingState = this.dockingSpaceship.spaceShipData.dockingState;
					dockingState2 = DockingState.DockingAssigned;
					if (!(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null))
					{
						dockingState = this.dockingSpaceship.spaceShipData.dockingState;
						dockingState2 = DockingState.Docking;
						if (!(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null))
						{
							return;
						}
					}
					this.PerformDocking(false);
					return;
				}
			}
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x000A5938 File Offset: 0x000A3B38
		private void SetDockingAssignedVars()
		{
			if (Vector2.Distance(this.dockingTransform.position, this.dockingSpaceship.transform.position) < 3f)
			{
				this.currentWaypoint = this.dockingTransform.position;
				this.dockingSpaceship.SetOverrideDestination(this.dockingTransform.position, false, false, false);
				Quaternion bestDockingRotation = this.GetBestDockingRotation();
				Quaternion value = this.dockingSpaceship.dockSideways ? (this.dockingTransform.rotation * Quaternion.Euler(0f, 0f, 90f)) : bestDockingRotation;
				this.dockingSpaceship.forceWorldAngle = new Quaternion?(value);
				this.dockingSpaceship.ToggleStateForDocking(false);
				return;
			}
			this.dockingSpaceship.SetOverrideDestination(this.GetApproachWaypoint(), false, false, false);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x000A5A1C File Offset: 0x000A3C1C
		private Vector2 GetApproachWaypoint()
		{
			return this.dockingTransform.position + (this.dockingSpaceship.transform.position - this.dockingTransform.position).normalized * 2.5f;
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x000A5A70 File Offset: 0x000A3C70
		private void PerformDocking(bool skipCoroutine = false)
		{
			float magnitude = (this.dockingTransform.position - this.dockingSpaceship.transform.position).magnitude;
			this.SetDockingAssignedVars();
			if (magnitude < 0.5f && Mathf.Abs(this.dockingSpaceship.GetCurrentAngleToTarget()) < 2f && this.dockingSpaceship.CanDock())
			{
				this.dockingSpaceship.ToggleStateForDocking(false);
				this.dockingSpaceship.spaceShipData.dockingState = new DockingState?(DockingState.Docking);
				if (skipCoroutine)
				{
					this.DockQuick();
				}
				else
				{
					this.dockingCoroutine = base.StartCoroutine(this.Dock(false));
				}
				this.PrepareLanding();
			}
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x000A5B28 File Offset: 0x000A3D28
		public void AssignSpaceshipForDocking(SpaceShip spaceShip, bool skipCoroutine = false)
		{
			spaceShip.spaceShipData.dockingState = new DockingState?(DockingState.DockingAssigned);
			this.dockingSpaceship = spaceShip;
			this.PerformDocking(skipCoroutine);
			DockingState? dockingState = this.dockingSpaceship.spaceShipData.dockingState;
			DockingState dockingState2 = DockingState.Docking;
			if (dockingState.GetValueOrDefault() == dockingState2 & dockingState != null)
			{
				return;
			}
			this.DockingShipSetup();
			this.currentWaypoint = this.GetApproachWaypoint();
			this.dockingSpaceship.SetOverrideDestination(this.currentWaypoint, false, false, false);
			if (!skipCoroutine)
			{
				this.PrepareApproach();
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x000A5BAB File Offset: 0x000A3DAB
		public IEnumerator AssignSpaceshipForUnDocking(SpaceShip spaceShip)
		{
			this.dockingSpaceship = spaceShip;
			this.DockingShipSetup();
			yield return this.Undock();
			yield break;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000A5BC1 File Offset: 0x000A3DC1
		protected bool IsApproachColliderFree()
		{
			return true;
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x000A5BC4 File Offset: 0x000A3DC4
		protected void DockQuick()
		{
			this.DockingProcedureQuick();
			this.dockingSpaceship.SetEngineState(false, true);
			if (this.dockingSpaceship.IsPlayer(false))
			{
				SpacestationExteriorManager.Instance.EnterSpacestation();
			}
			this.dockingSpaceship.spaceShipData.dockingState = new DockingState?(DockingState.Docked);
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x000A5C12 File Offset: 0x000A3E12
		protected IEnumerator Dock(bool skipCoroutine = false)
		{
			if (this.dockingSpaceship.IsPlayer(false))
			{
				SpacestationExteriorManager.Instance.EnterSpacestation();
			}
			yield return this.DockingProcedure(skipCoroutine);
			if (!this.dockingSpaceship)
			{
				yield break;
			}
			this.dockingSpaceship.SetEngineState(false, true);
			this.dockingSpaceship.spaceShipData.dockingState = new DockingState?(DockingState.Docked);
			yield break;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x000A5C28 File Offset: 0x000A3E28
		public void StartUndockCoroutine()
		{
			if (!this.dockingSpaceship)
			{
				return;
			}
			base.StartCoroutine(this.Undock());
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x000A5C45 File Offset: 0x000A3E45
		protected IEnumerator Undock()
		{
			this.dockingSpaceship.spaceShipData.dockingState = new DockingState?(DockingState.Undocking);
			yield return this.UndockingProcedure();
			yield return new WaitUntil(() => !this.CheckCollidersForDockingShip(), new TimeSpan(0, 0, 5), delegate()
			{
			}, WaitTimeoutMode.Realtime);
			this.dockingSpaceship.SetEngineState(true, true);
			this.dockingSpaceship.ToggleStateForDocking(true);
			this.dockingSpaceship.forceWorldAngle = null;
			this.dockingSpaceship.spaceShipData.dockingState = new DockingState?(DockingState.Leaving);
			if (this.dockingSpaceship.IsPlayer(false) && this.dockingSpaceship.overrideTarget == null)
			{
				this.dockingSpaceship.SetOverrideDestination(SpacestationExteriorManager.Instance.GetClosestWarpInPosition(this.dockingSpaceship), false, false, false);
			}
			this.ResetDockingOption();
			yield break;
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000A5C54 File Offset: 0x000A3E54
		protected void EmergencyUndock()
		{
			if (this.dockingSpaceship == null)
			{
				return;
			}
			this.dockingSpaceship.SetEngineState(true, true);
			this.dockingSpaceship.ToggleStateForDocking(true);
			this.dockingSpaceship.forceWorldAngle = null;
			this.dockingSpaceship.spaceShipData.dockingState = new DockingState?(DockingState.Leaving);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x000A5CB0 File Offset: 0x000A3EB0
		private bool CheckCollidersForDockingShip()
		{
			List<Collider2D> results = new List<Collider2D>();
			return Physics2D.OverlapCollider(this.dockingSpaceship.surfaceCollider, results) > 0;
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x000A5CDA File Offset: 0x000A3EDA
		public virtual void ResetDockingOption()
		{
			this.currentWaypoint = default(Vector2);
			this.dockingSpaceship = null;
			this.dockingApproachLights.StopDockingLightsSequence();
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x000A5CFC File Offset: 0x000A3EFC
		public void OnDestroy()
		{
			if (SpacestationExteriorManager.Instance && base.gameObject.scene.isLoaded)
			{
				this.EmergencyUndock();
				SpacestationExteriorManager.Instance.TryRemoveDockingOption(this);
			}
		}

		// Token: 0x06001AFB RID: 6907
		protected abstract void PrepareLanding();

		// Token: 0x06001AFC RID: 6908
		protected abstract void PrepareApproach();

		// Token: 0x06001AFD RID: 6909
		protected abstract void DockingProcedureQuick();

		// Token: 0x06001AFE RID: 6910
		protected abstract IEnumerator DockingProcedure(bool skipCoroutine = false);

		// Token: 0x06001AFF RID: 6911
		protected abstract IEnumerator UndockingProcedure();

		// Token: 0x06001B00 RID: 6912
		protected abstract void DockingShipSetup();

		// Token: 0x06001B01 RID: 6913 RVA: 0x000A5D3B File Offset: 0x000A3F3B
		public bool CanDock(SpaceShip spaceship)
		{
			return this.CanDock(spaceship.shipRoleType.GetDockingOptionSize());
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x000A5D4E File Offset: 0x000A3F4E
		public bool CanDock(DockingOptionSize size)
		{
			return this.dockingOptionSize == size;
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x000A5D59 File Offset: 0x000A3F59
		protected virtual Quaternion GetBestDockingRotation()
		{
			return this.dockingTransform.rotation;
		}

		// Token: 0x040010FC RID: 4348
		[SerializeField]
		protected BoxCollider2D approachCollider;

		// Token: 0x040010FD RID: 4349
		[SerializeField]
		protected BoxCollider2D dockingCollider;

		// Token: 0x040010FE RID: 4350
		[SerializeField]
		protected DockingApproachLights dockingApproachLights;

		// Token: 0x040010FF RID: 4351
		public bool spawn;

		// Token: 0x04001100 RID: 4352
		private Vector2 currentWaypoint;

		// Token: 0x04001102 RID: 4354
		protected Coroutine dockingCoroutine;

		// Token: 0x04001103 RID: 4355
		protected List<GameObject> dockingOptionLights;
	}
}
