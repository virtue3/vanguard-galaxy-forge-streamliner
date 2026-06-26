using System;
using System.Collections.Generic;
using Behaviour.Equipment.Turret;
using Behaviour.Salvage;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Galaxy;
using _Scripts.Behaviour.Equipment.Turret.Salvage;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200036B RID: 875
	public class SalvageModule : AbstractTargetingModule
	{
		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x060021A8 RID: 8616 RVA: 0x000C3C49 File Offset: 0x000C1E49
		protected override TargetingPriority baseTargetPriority
		{
			get
			{
				return TargetingPriority.Low;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x000C3C4C File Offset: 0x000C1E4C
		public override bool maintainTargetAngle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000C3C4F File Offset: 0x000C1E4F
		protected override void Awake()
		{
			base.Awake();
			base.approachDistance = 4f;
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000C3C64 File Offset: 0x000C1E64
		public override void UpdateAvailableTargets(IEnumerable<TargetableUnit> targets)
		{
			if (base.autoTarget && !this.IsTargetSalvagable(base.autoTarget.GetComponent<SalvageContainer>()))
			{
				base.ResetAutoTarget();
			}
			if (base.manualTarget && !this.IsTargetSalvagable(base.manualTarget.GetComponent<SalvageContainer>()) && !base.manualTarget.damagableByAll)
			{
				base.ResetCurrentTargets();
			}
			this.FilterAvailableTargets((List<TargetableUnit>)targets, true);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000C3CD8 File Offset: 0x000C1ED8
		private void FilterAvailableTargets(List<TargetableUnit> targets, bool avoidPlayer)
		{
			this.filteredTargets.Clear();
			foreach (TargetableUnit targetableUnit in targets)
			{
				SalvageContainer component = targetableUnit.GetComponent<SalvageContainer>();
				if (component && (!avoidPlayer || base.parent.faction == Faction.player || !component.IsTargetedBy(GameplayManager.Instance.spaceShip)) && (!avoidPlayer || targetableUnit.targetedCount <= 0) && this.IsTargetSalvagable(component))
				{
					this.filteredTargets.Add(component);
				}
			}
			if (avoidPlayer && this.filteredTargets.Count == 0)
			{
				this.FilterAvailableTargets(targets, false);
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x000C3D9C File Offset: 0x000C1F9C
		public override void SetManualTarget(TargetableUnit manualTarget)
		{
			if (manualTarget != this.priorityTarget)
			{
				AbstractTurret[] turrets = base.turrets;
				for (int i = 0; i < turrets.Length; i++)
				{
					SalvageStructuralGrinder salvageStructuralGrinder = ((AbstractSalvageTurret)turrets[i]) as SalvageStructuralGrinder;
					if (salvageStructuralGrinder != null && salvageStructuralGrinder.tetherSecured)
					{
						salvageStructuralGrinder.ReleaseTether();
					}
				}
			}
			base.SetManualTarget(manualTarget);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000C3DF4 File Offset: 0x000C1FF4
		public void ReleaseTarget()
		{
			base.ResetCurrentTargets();
			AbstractTurret[] turrets = base.turrets;
			for (int i = 0; i < turrets.Length; i++)
			{
				SalvageStructuralGrinder salvageStructuralGrinder = ((AbstractSalvageTurret)turrets[i]) as SalvageStructuralGrinder;
				if (salvageStructuralGrinder != null)
				{
					salvageStructuralGrinder.ReleaseTether();
				}
			}
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000C3E34 File Offset: 0x000C2034
		public bool IsTargetSalvagable(SalvageContainer target)
		{
			if (!target)
			{
				return false;
			}
			foreach (AbstractSalvageTurret turret in base.turrets)
			{
				if (target.CanBeDamagedBy(turret))
				{
					return true;
				}
			}
			if (base.parent.droneBayModule)
			{
				foreach (Drone drone in base.parent.droneBayModule.drones)
				{
					foreach (AbstractSalvageTurret turret2 in drone.GetSalvageTurrets())
					{
						if (target.CanBeDamagedBy(turret2))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000C3F00 File Offset: 0x000C2100
		public void SetSalvageTurrets(AbstractSalvageTurret[] turrets)
		{
			base.turrets = turrets;
			List<TargetLayer> list = new List<TargetLayer>();
			foreach (AbstractSalvageTurret abstractSalvageTurret in turrets)
			{
				abstractSalvageTurret.SetTargetingModule(this);
				if (!list.Contains(abstractSalvageTurret.targetLayer))
				{
					list.Add(abstractSalvageTurret.targetLayer);
				}
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000C3F55 File Offset: 0x000C2155
		public override MainStat GetMainStat()
		{
			return new MainStat("Salvage Module", 0f);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000C3F66 File Offset: 0x000C2166
		protected override void SetMainSubStats()
		{
		}
	}
}
