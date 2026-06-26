using System;
using System.Collections.Generic;
using Behaviour.Equipment.Turret;
using Behaviour.Weapons;
using Source.Crew;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000367 RID: 871
	public abstract class AbstractTargetingModule : AbstractModule
	{
		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06002162 RID: 8546 RVA: 0x000C2FBD File Offset: 0x000C11BD
		public TargetingPriority targetingPriority
		{
			get
			{
				if (!this.manualTarget)
				{
					return this.baseTargetPriority;
				}
				return TargetingPriority.Manual;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x06002163 RID: 8547 RVA: 0x000C2FD4 File Offset: 0x000C11D4
		public override EquipmentSlot slot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x000C2FDB File Offset: 0x000C11DB
		// (set) Token: 0x06002165 RID: 8549 RVA: 0x000C2FE3 File Offset: 0x000C11E3
		public float approachDistance { get; protected set; }

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000C2FEC File Offset: 0x000C11EC
		public virtual bool runFromTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002167 RID: 8551 RVA: 0x000C2FEF File Offset: 0x000C11EF
		public virtual bool maintainTargetAngle
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000C2FF2 File Offset: 0x000C11F2
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000C2FFA File Offset: 0x000C11FA
		public DroneBayModule droneBay { get; private set; }

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000C3003 File Offset: 0x000C1203
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x000C300B File Offset: 0x000C120B
		public TorpedoBayModule torpedoBayModule { get; private set; }

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x000C3014 File Offset: 0x000C1214
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x000C301C File Offset: 0x000C121C
		public TargetableUnit manualTarget { get; private set; }

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000C3025 File Offset: 0x000C1225
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x000C302D File Offset: 0x000C122D
		public TargetableUnit autoTarget { get; private set; }

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06002170 RID: 8560
		protected abstract TargetingPriority baseTargetPriority { get; }

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x000C3036 File Offset: 0x000C1236
		// (set) Token: 0x06002172 RID: 8562 RVA: 0x000C303E File Offset: 0x000C123E
		public AbstractTurret[] turrets { get; protected set; }

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002173 RID: 8563 RVA: 0x000C3047 File Offset: 0x000C1247
		public virtual TargetableUnit priorityTarget
		{
			get
			{
				if (this.manualTarget)
				{
					return this.manualTarget;
				}
				if (this.autoTarget)
				{
					return this.autoTarget;
				}
				return null;
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000C3072 File Offset: 0x000C1272
		protected override void Awake()
		{
			base.Awake();
			this.approachDistance = 5f;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000C3088 File Offset: 0x000C1288
		protected override void Update()
		{
			base.Update();
			if (!this.manualTarget)
			{
				this.manualTarget = null;
			}
			if (!this.autoTarget)
			{
				this.autoTarget = null;
			}
			this.approachDistanceTimer -= Time.deltaTime;
			if (this.approachDistanceTimer < 0f)
			{
				this.UpdateApproachDistance();
				this.approachDistanceTimer = 0.5f;
			}
		}

		// Token: 0x06002176 RID: 8566
		public abstract void UpdateAvailableTargets(IEnumerable<TargetableUnit> targets);

		// Token: 0x06002177 RID: 8567 RVA: 0x000C30F4 File Offset: 0x000C12F4
		protected virtual void UpdateApproachDistance()
		{
			float num = (float)this.GetMaxDistanceToTarget();
			this.trackingTargetPos = null;
			bool flag = false;
			float num2 = (float)this.GetMinDistanceToTarget();
			foreach (AbstractTurret abstractTurret in this.turrets)
			{
				if (this.priorityTarget && abstractTurret.CanTarget(this.priorityTarget))
				{
					flag = true;
					if (abstractTurret.HasAmmo() || !abstractTurret.ammoType || abstractTurret.AmmoAmountAvailable() > 0)
					{
						if (abstractTurret.customApproachDistance > 0f)
						{
							num = Mathf.Min(abstractTurret.customApproachDistance, num);
							if (abstractTurret.customApproachDistance < num2)
							{
								num2 = abstractTurret.customApproachDistance;
							}
						}
						else
						{
							num = Mathf.Min(abstractTurret.range, num) * 0.75f;
						}
						if (abstractTurret.trackingTarget)
						{
							this.trackingTargetPos = new Vector2?(abstractTurret.trackingTarget.transform.position);
						}
					}
				}
			}
			if (!flag && this.droneBay)
			{
				num = 10f;
			}
			else if (!flag)
			{
				this.manualTarget = (this.autoTarget = null);
			}
			this.approachDistance = Mathf.Clamp(num, num2, 30f);
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000C323B File Offset: 0x000C143B
		protected virtual int GetMaxDistanceToTarget()
		{
			return 6;
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000C323E File Offset: 0x000C143E
		protected virtual int GetMinDistanceToTarget()
		{
			return 3;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000C3244 File Offset: 0x000C1444
		public virtual void UpdateAutoTarget()
		{
			if (this.autoTarget)
			{
				return;
			}
			if (!this.ShouldAutoTarget())
			{
				return;
			}
			float num = float.MaxValue;
			foreach (TargetableUnit targetableUnit in this.filteredTargets)
			{
				if (targetableUnit)
				{
					float num2 = Vector2.Distance(base.transform.position, targetableUnit.transform.position);
					if (this.autoTarget == null || num2 < num)
					{
						this.autoTarget = targetableUnit;
						num = num2;
					}
				}
			}
			if (this.autoTarget)
			{
				this.autoTarget.AddTargetedBy(base.parent, 0f);
				if (this.droneBay && this.droneBay.AreDronesDocked())
				{
					this.droneBay.DeployDrones();
				}
			}
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000C3340 File Offset: 0x000C1540
		public virtual bool ShouldAutoTarget()
		{
			if (base.parent.IsPlayer(false) && !base.parent.shouldAutoFire)
			{
				SpaceShipData spaceShipData = base.parent.unitData as SpaceShipData;
				if (spaceShipData != null)
				{
					Mercenary mercenary = spaceShipData.commanderData as Mercenary;
					if (mercenary != null && mercenary.behaviour == WingmanBehaviour.Aggressive)
					{
						return true;
					}
				}
				if (!GamePlayer.current.autoPlay)
				{
					return false;
				}
				if (SeededRandom.Global.RandomBool(0.8333333f))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000C33B8 File Offset: 0x000C15B8
		public void SetDroneBay(DroneBayModule droneBay)
		{
			this.droneBay = droneBay;
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x000C33C1 File Offset: 0x000C15C1
		public void SetTorpedoBay(TorpedoBayModule torpedoBayModule)
		{
			this.torpedoBayModule = torpedoBayModule;
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000C33CC File Offset: 0x000C15CC
		public virtual bool IsValidTarget(TargetableUnit target)
		{
			AbstractTurret[] turrets = this.turrets;
			for (int i = 0; i < turrets.Length; i++)
			{
				if (turrets[i].CanTarget(target))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x000C33FC File Offset: 0x000C15FC
		public virtual void SetManualTarget(TargetableUnit manualTarget)
		{
			this.manualTarget = manualTarget;
			if (manualTarget)
			{
				manualTarget.AddTargetedBy(base.parent, 0f);
			}
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x000C341E File Offset: 0x000C161E
		public void ResetCurrentTargets()
		{
			this.manualTarget = null;
			this.autoTarget = null;
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x000C342E File Offset: 0x000C162E
		public void ResetAutoTarget()
		{
			this.autoTarget = null;
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x000C3437 File Offset: 0x000C1637
		protected virtual void OnDisable()
		{
			this.autoTarget = null;
			this.manualTarget = null;
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000C3448 File Offset: 0x000C1648
		private void OnDrawGizmos()
		{
			if (this.manualTarget)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(base.transform.position + new Vector3(0f, 0.1f), this.manualTarget.transform.position);
			}
			if (this.autoTarget)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(base.transform.position + new Vector3(0f, -0.1f), this.autoTarget.transform.position);
			}
		}

		// Token: 0x040013D6 RID: 5078
		public readonly List<TargetableUnit> filteredTargets = new List<TargetableUnit>();

		// Token: 0x040013DC RID: 5084
		private float approachDistanceTimer;

		// Token: 0x040013DD RID: 5085
		public Vector2? trackingTargetPos;
	}
}
