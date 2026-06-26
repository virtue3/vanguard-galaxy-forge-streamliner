using System;
using System.Collections;
using Behaviour.Effects;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.MiningTurrets
{
	// Token: 0x02000356 RID: 854
	public class MiningDrillerTurret : AbstractMiningTurret
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000BEF4F File Offset: 0x000BD14F
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Mining;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600208B RID: 8331 RVA: 0x000BEF52 File Offset: 0x000BD152
		public override TargetLayer targetLayer
		{
			get
			{
				return TargetLayer.Core;
			}
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000BEF55 File Offset: 0x000BD155
		protected void OnEnable()
		{
			if (this.droneMiningEffect)
			{
				this.droneMiningEffect.Stop();
			}
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000BEF70 File Offset: 0x000BD170
		protected override void Start()
		{
			base.Start();
			Drone drone = base.parent as Drone;
			if (drone != null)
			{
				drone.keepMoving = false;
			}
			this.droneMiningEffect = UnityEngine.Object.Instantiate<DroneMiningEffect>(this.droneMiningEffect, base.transform);
			this.droneMiningEffect.Stop();
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x000BEFBC File Offset: 0x000BD1BC
		protected override void Update()
		{
			if (this.attaching || this.detaching)
			{
				return;
			}
			base.Update();
			Drone drone = base.parent as Drone;
			if (drone != null && (drone.isReturning || drone.cantFitMoreCargo))
			{
				this.CheckForDetach();
				return;
			}
			if (base.asteroidTarget != null && !base.asteroidTarget.CanWeAttach(base.parent as Drone))
			{
				base.targetingModule.ResetCurrentTargets();
				return;
			}
			if (base.asteroidTarget != null && !base.asteroidTarget.innerCoreDepleted)
			{
				if (!this.IsOnSurface())
				{
					this.CheckForDetach();
					this.MoveToSurface();
					return;
				}
				if (!this.attachedToAsteroid)
				{
					this.AttachToSurface();
					return;
				}
				if (!base.isReloading)
				{
					base.StartCoroutine(this.DrillIntoAsteroid());
					return;
				}
			}
			else if (!base.isReloading)
			{
				if (this.attachedToAsteroid)
				{
					this.DetachFromSurface(false);
					return;
				}
				base.parent.ClearOverrideDestination();
			}
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000BF0B0 File Offset: 0x000BD2B0
		protected override bool FireInternal()
		{
			return false;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000BF0B3 File Offset: 0x000BD2B3
		private void CheckForDetach()
		{
			if (this.attachedToAsteroid && !base.isReloading)
			{
				this.DetachFromSurface(false);
			}
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000BF0CC File Offset: 0x000BD2CC
		private void MoveToSurface()
		{
			if (base.asteroidTarget != null)
			{
				base.parent.SetOverrideDestination(this.attachPosition, false, false, false);
			}
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000BF0F8 File Offset: 0x000BD2F8
		private bool IsOnSurface()
		{
			if (base.asteroidTarget == null)
			{
				return false;
			}
			this.attachPosition = base.trackingTarget.transform.position;
			return Vector2.Distance(base.transform.position, this.attachPosition) < 0.1f;
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x000BF154 File Offset: 0x000BD354
		private void AttachToSurface()
		{
			this.sceneParent = base.parent.transform.parent;
			base.parent.SetEngineState(false, false);
			base.parent.SetRigidbodyState(false);
			base.parent.transform.SetParent(base.asteroidTarget.transform);
			this.attachedToAsteroid = true;
			base.parent.transform.Z(ZIndex.AttachedToAsteroid);
			base.asteroidTarget.AttachDrone(base.parent as Drone);
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000BF1DC File Offset: 0x000BD3DC
		private void DetachFromSurface(bool destroyed = false)
		{
			if (!destroyed)
			{
				base.parent.SetEngineState(true, true);
				base.parent.SetRigidbodyState(true);
				base.parent.transform.SetParent(this.sceneParent);
				base.targetingModule.ResetCurrentTargets();
				this.attachedToAsteroid = false;
				this.attachPosition = default(Vector3);
				base.parent.transform.Z(ZIndex.Drone);
			}
			Asteroid asteroidTarget = base.asteroidTarget;
			if (asteroidTarget == null)
			{
				return;
			}
			asteroidTarget.DetachDrone(base.parent as Drone);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000BF265 File Offset: 0x000BD465
		private IEnumerator DrillIntoAsteroid()
		{
			if (!base.asteroidTarget)
			{
				yield break;
			}
			if (base.targetsCore)
			{
				base.Reload(false, true, 1f);
				this.droneMiningEffect.PlayWithSize(0.5f, 20f);
				Singleton<EffectManager>.Instance.PlaySmokeEffect(base.transform.position, 0.5f, null, 4f, 15);
				base.asteroidTarget.TakeDrillCoreDamage(this.CreateDamageData(null, null, TargetLayer.Surface));
				yield return new WaitForSeconds(1f);
				this.droneMiningEffect.Stop();
			}
			yield return new WaitForSeconds(base.reloadDelay);
			yield break;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000BF274 File Offset: 0x000BD474
		public override bool CanMineAsteroidTarget(Asteroid target)
		{
			return target.hasInnerCore && target.CanWeAttach(base.parent as Drone);
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000BF291 File Offset: 0x000BD491
		protected override float GetPowerMultiplier()
		{
			return 1.2f;
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x000BF298 File Offset: 0x000BD498
		public void ForceDetach(bool destroyed = false)
		{
			this.DetachFromSurface(destroyed);
		}

		// Token: 0x0400136E RID: 4974
		[SerializeField]
		private DroneMiningEffect droneMiningEffect;

		// Token: 0x0400136F RID: 4975
		private bool attachedToAsteroid;

		// Token: 0x04001370 RID: 4976
		private bool attaching;

		// Token: 0x04001371 RID: 4977
		private bool detaching;

		// Token: 0x04001372 RID: 4978
		private Vector3 attachPosition;

		// Token: 0x04001373 RID: 4979
		private Transform sceneParent;
	}
}
