using System;
using System.Collections.Generic;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.UI.HUD;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Ability;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000369 RID: 873
	public class CombatModule : AbstractTargetingModule
	{
		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000C34FE File Offset: 0x000C16FE
		public override bool maintainTargetAngle
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000C3501 File Offset: 0x000C1701
		protected override TargetingPriority baseTargetPriority
		{
			get
			{
				return TargetingPriority.High;
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000C3504 File Offset: 0x000C1704
		protected override void Awake()
		{
			base.Awake();
			base.approachDistance = 5f;
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000C3518 File Offset: 0x000C1718
		public override void UpdateAvailableTargets(IEnumerable<TargetableUnit> targets)
		{
			bool flag = base.autoTarget != null && base.autoTarget.gameObject.activeSelf;
			bool flag2 = base.manualTarget != null && base.manualTarget.gameObject.activeSelf;
			this.filteredTargets.Clear();
			int num = 0;
			foreach (TargetableUnit targetableUnit in targets)
			{
				if (this.IsValidTarget(targetableUnit))
				{
					num++;
					if (!flag && !flag2)
					{
						this.filteredTargets.Add(targetableUnit);
					}
				}
			}
			if (!base.IsPlayer(true))
			{
				return;
			}
			this.CheckForHordeDefense(num);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x000C35DC File Offset: 0x000C17DC
		private void CheckForHordeDefense(int amount)
		{
			if ((float)amount >= 3f)
			{
				base.parent.CheckTriggerAbility(AbilityTrigger.OnOverwhelmedByEnemies, this, null);
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000C35F5 File Offset: 0x000C17F5
		public override void SetManualTarget(TargetableUnit target)
		{
			base.SetManualTarget(target);
			if (base.parent == GameplayManager.Instance.spaceShip)
			{
				HudManager.Instance.ShowTargetIndicator(base.manualTarget);
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000C3628 File Offset: 0x000C1828
		public void SetCombatTurrets(AbstractCombatTurret[] turrets)
		{
			base.turrets = turrets;
			for (int i = 0; i < turrets.Length; i++)
			{
				turrets[i].SetTargetingModule(this);
			}
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000C3658 File Offset: 0x000C1858
		public override bool IsValidTarget(TargetableUnit target)
		{
			AbstractUnit abstractUnit = target as AbstractUnit;
			return abstractUnit && !abstractUnit.travelling && base.parent.faction != null && base.parent.IsEnemy(abstractUnit) && abstractUnit.gameObject.activeInHierarchy && !abstractUnit.isSalvage;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000C36B0 File Offset: 0x000C18B0
		protected override int GetMaxDistanceToTarget()
		{
			if (base.IsPlayer(true))
			{
				return 40;
			}
			AbstractUnit abstractUnit = this.priorityTarget as AbstractUnit;
			if (abstractUnit != null && abstractUnit.IsPlayer(false))
			{
				return (int)Math.Floor((double)abstractUnit.GetMaxCombatRange());
			}
			return 8;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000C36F0 File Offset: 0x000C18F0
		protected override int GetMinDistanceToTarget()
		{
			return 6;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000C36F3 File Offset: 0x000C18F3
		public override MainStat GetMainStat()
		{
			return new MainStat("Combat Module", 0f);
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000C3704 File Offset: 0x000C1904
		protected override void SetMainSubStats()
		{
		}

		// Token: 0x040013E3 RID: 5091
		public const float DefaultApproachDistance = 5f;

		// Token: 0x040013E4 RID: 5092
		public const float OverwhelmedByEnemiesAmount = 3f;
	}
}
