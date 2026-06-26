using System;
using System.Collections;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.Salvage;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Player;
using Source.Util;
using Unity.VisualScripting;
using UnityEngine;
using _Scripts.Behaviour.Equipment.Turret.Projectile;

namespace _Scripts.Behaviour.Equipment.Turret.Salvage
{
	// Token: 0x02000198 RID: 408
	public class SalvageStructuralGrinder : AbstractSalvageTurret
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0006787E File Offset: 0x00065A7E
		// (set) Token: 0x06000E75 RID: 3701 RVA: 0x00067886 File Offset: 0x00065A86
		public bool tetherSecured { get; private set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0006788F File Offset: 0x00065A8F
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Salvage;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00067892 File Offset: 0x00065A92
		public override TargetLayer targetLayer
		{
			get
			{
				return TargetLayer.Core;
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00067898 File Offset: 0x00065A98
		protected override void Awake()
		{
			base.Awake();
			this.originalProjectileSpeed = this._projectileSpeed;
			Drone drone = base.parent as Drone;
			if (drone != null)
			{
				drone.keepMoving = false;
			}
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000678CD File Offset: 0x00065ACD
		protected override void Start()
		{
			base.Start();
			if (base.parent is Drone)
			{
				base.customApproachDistance = Mathf.Max(base.parent.height, 0.5f);
			}
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000678FD File Offset: 0x00065AFD
		protected override void UpdateForCollider()
		{
			base.trackingTarget.transform.position = base.currentTarget.surfaceCollider.ClosestPoint(base.transform.position);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00067934 File Offset: 0x00065B34
		protected override bool FireInternal()
		{
			if (!this.tetherSecured && !this.currentHarpoon)
			{
				this.SetParentHoldPosition();
				Quaternion rhs = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.accuracyAngle, this.accuracyAngle));
				Quaternion rotation = this.firePoints[this.firePointIndex].rotation * rhs;
				this.currentHarpoon = UnityEngine.Object.Instantiate<Harpoon>(this.harpoonPrefab, this.firePoints[this.firePointIndex].position, rotation);
				base.PlayFiringSound();
				if (this.currentHarpoon)
				{
					float num = 0f;
					Vector3 zero = Vector3.zero;
					this.firePoints[this.firePointIndex].rotation.ToAngleAxis(out num, out zero);
					this.currentHarpoon.Initialize(this.CreateDamageData(base.trackingTarget.transform, new Vector2?(base.trackingTarget.transform.localPosition), this.targetLayer), base.projectileSpeed, this);
					base.TriggerFireProjectile(this.currentHarpoon);
					this.hardpoonSpring = UnityEngine.Object.Instantiate<HarpoonSpring>(this.harpoonSpringPrefab, this.firePoints[this.firePointIndex]);
					base.customApproachDistance = base.parent.height;
				}
				else
				{
					Debug.LogWarning("Harpoon prefab does not exist");
				}
				base.StartCoroutine(this.PlayWeaponEffect());
				return true;
			}
			if (this.targetAligned && base.currentTarget)
			{
				DamageData data = this.CreateDamageData(base.trackingTarget.transform, new Vector2?(base.trackingTarget.transform.position), TargetLayer.Core);
				global::Behaviour.Util.Singleton<EffectManager>.Instance.PlaySparksEffect(base.trackingTarget.transform.position, 0.1f, 0.2f, 1.5f, new Color?(ColorHelper.flashExplosionUnit), this.currentHarpoon.transform);
				global::Behaviour.Util.Singleton<EffectManager>.Instance.PlayDroneMiningEffect(base.trackingTarget.transform.position, 0.2f, 15f, 1f, this.currentHarpoon.transform);
				global::Behaviour.Util.Singleton<EffectManager>.Instance.PlaySmokeEffect(base.trackingTarget.transform.position, 0.5f, null, 4f, 15);
				if (base.currentTarget != null && base.currentTarget.CanBeDamagedBy(this))
				{
					base.currentTarget.TakeDamage(data);
				}
				base.parent.SetHoldPosition(true);
				return true;
			}
			return false;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x00067BCD File Offset: 0x00065DCD
		private void SetParentHoldPosition()
		{
			if (base.parent.IsPlayer(true))
			{
				this.parentHoldPosition = GamePlayer.current.holdPosition;
			}
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x00067BED File Offset: 0x00065DED
		public override void Deactivate()
		{
			base.Deactivate();
			this.ReleaseTether();
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00067BFB File Offset: 0x00065DFB
		public override void ToggleActive()
		{
			if (base.active)
			{
				this.ReleaseTether();
			}
			base.ToggleActive();
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x00067C14 File Offset: 0x00065E14
		public override void ClearTarget()
		{
			SalvageContainer salvageContainer = base.currentTarget as SalvageContainer;
			if (salvageContainer != null && this.ParentHasSurfaceSalvageFor(salvageContainer))
			{
				return;
			}
			base.ClearTarget();
			if (base.active)
			{
				this.ReleaseTether();
			}
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00067C50 File Offset: 0x00065E50
		private void AddSpringJoint()
		{
			this.springJoint = base.parent.AddComponent<SpringJoint2D>();
			this.springJoint.anchor = base.transform.parent.localPosition;
			this.springJoint.enableCollision = true;
			this.springJoint.distance = 0f;
			this.springJoint.autoConfigureDistance = false;
			this.springJoint.frequency = 0.2f;
			this.springJoint.dampingRatio = 0.8f;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x00067CD6 File Offset: 0x00065ED6
		protected override IEnumerator PlayWeaponEffect()
		{
			if (!this.tetherSecured)
			{
				return base.PlayWeaponEffect();
			}
			return null;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00067CE8 File Offset: 0x00065EE8
		public void DestroySpring()
		{
			if (this.hardpoonSpring)
			{
				UnityEngine.Object.Destroy(this.hardpoonSpring.gameObject);
			}
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x00067D08 File Offset: 0x00065F08
		private bool ParentHasSurfaceSalvageFor(SalvageContainer container)
		{
			if (container == null || !container.data.HasSalvage(TargetLayer.Surface))
			{
				return false;
			}
			foreach (AbstractSalvageTurret abstractSalvageTurret in base.parent.GetComponentsInChildren<AbstractSalvageTurret>())
			{
				if (abstractSalvageTurret.active && abstractSalvageTurret.targetsSurface)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x00067D60 File Offset: 0x00065F60
		private void FixedUpdate()
		{
			if (this.currentHarpoon)
			{
				if (this.tetherSecured && (!this.currentTargetContainer || (this.currentTargetContainer && !this.currentTargetContainer.data.HasSalvage(this.targetLayer))))
				{
					this.ReleaseTether();
					this.ClearTarget();
				}
				this.harpoonOnTurret.SetActive(false);
				this.hardpoonSpring.SetPositions(this.currentHarpoon.transform.position, base.transform.parent.position);
				if (!this.currentTargetContainer || !base.currentTarget)
				{
					return;
				}
				Quaternion quaternion = base.currentTarget.transform.rotation;
				if (Quaternion.Angle(base.parent.transform.rotation, quaternion) > 90f)
				{
					quaternion *= Quaternion.AngleAxis(180f, Vector3.forward);
				}
				base.parent.forceWorldAngle = new Quaternion?(quaternion);
				this.targetAligned = (base.parent.collisionIsSalvage || this.targetAligned);
				if (this.springJoint && this.tetherSecured)
				{
					this.currentHarpoon.transform.position = base.currentTarget.surfaceCollider.ClosestPoint(base.transform.position);
					this.springJoint.connectedAnchor = this.currentHarpoon.transform.localPosition;
					if (base.parent is Drone && Vector2.Distance(base.parent.transform.position, this.currentHarpoon.transform.position) <= Mathf.Max(base.parent.height * 2f, 1f))
					{
						this.targetAligned = true;
						return;
					}
				}
			}
			else
			{
				if (!this.currentHarpoon && this.tetherSecured)
				{
					this.ReleaseTether();
					return;
				}
				if (!this.harpoonOnTurret.activeSelf)
				{
					this.harpoonOnTurret.SetActive(true);
				}
			}
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00067FA0 File Offset: 0x000661A0
		public void OnHarpoonHit(SalvageContainer salvageContainer, Transform hitPosition)
		{
			if (this.tetherSecured)
			{
				return;
			}
			this.tetherSecured = true;
			this._projectileSpeed = 0f;
			this.AddSpringJoint();
			this.springJoint.connectedBody = salvageContainer.shipBase.rigidbody;
			this.springJoint.connectedAnchor = hitPosition.localPosition;
			this.currentTargetContainer = salvageContainer;
			Drone drone = base.parent as Drone;
			if (drone != null)
			{
				drone.gameObject.layer = LayerMask.NameToLayer("Default");
			}
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x00068028 File Offset: 0x00066228
		public void ReleaseTether()
		{
			base.parent.SetHoldPosition(this.parentHoldPosition && base.parent.IsPlayer(true) && !GamePlayer.current.autoPlay);
			base.parent.forceWorldAngle = null;
			this.tetherSecured = false;
			this.targetAligned = false;
			this._projectileSpeed = this.originalProjectileSpeed;
			this.currentTargetContainer = null;
			if (base.currentTarget && base.currentTarget.GetComponent<SalvageContainer>())
			{
				base.currentTarget.GetComponent<SalvageContainer>().gameObject.layer = LayerMask.NameToLayer("Asteroid");
			}
			if (this.springJoint)
			{
				UnityEngine.Object.Destroy(this.springJoint);
			}
			if (this.currentHarpoon)
			{
				UnityEngine.Object.Destroy(this.currentHarpoon.gameObject);
			}
			if (this.hardpoonSpring)
			{
				UnityEngine.Object.Destroy(this.hardpoonSpring.gameObject);
			}
			Drone drone = base.parent as Drone;
			if (drone != null)
			{
				drone.gameObject.layer = LayerMask.NameToLayer("Drone");
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0006814C File Offset: 0x0006634C
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.ReleaseTether();
		}

		// Token: 0x04000823 RID: 2083
		[SerializeField]
		private Harpoon harpoonPrefab;

		// Token: 0x04000824 RID: 2084
		[SerializeField]
		private HarpoonSpring harpoonSpringPrefab;

		// Token: 0x04000825 RID: 2085
		[SerializeField]
		private GameObject harpoonOnTurret;

		// Token: 0x04000827 RID: 2087
		private bool targetAligned;

		// Token: 0x04000828 RID: 2088
		private SpringJoint2D springJoint;

		// Token: 0x04000829 RID: 2089
		private Harpoon currentHarpoon;

		// Token: 0x0400082A RID: 2090
		private HarpoonSpring hardpoonSpring;

		// Token: 0x0400082B RID: 2091
		private SalvageContainer currentTargetContainer;

		// Token: 0x0400082C RID: 2092
		private float originalProjectileSpeed;

		// Token: 0x0400082D RID: 2093
		private bool parentHoldPosition;
	}
}
