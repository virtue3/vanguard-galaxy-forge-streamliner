using System;
using Behaviour.Equipment.Module;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Player;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000065 RID: 101
	public class CombatActions : AutoActions
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001E8C0 File Offset: 0x0001CAC0
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0001E8C8 File Offset: 0x0001CAC8
		public ThreatTable threat { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x0001E8D1 File Offset: 0x0001CAD1
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0001E8D4 File Offset: 0x0001CAD4
		public CombatActions(AbstractUnit parent) : base(parent)
		{
			this.threat = new ThreatTable(parent);
			this.combatModule = parent.GetComponentInChildren<CombatModule>();
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001E8F5 File Offset: 0x0001CAF5
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001E8F8 File Offset: 0x0001CAF8
		public override void LeaveMissionTrigger()
		{
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001E8FC File Offset: 0x0001CAFC
		public override void Update(float delta)
		{
			base.Update(delta);
			if (GamePlayer.current == null || !this.combatModule)
			{
				return;
			}
			this.threat.Update(delta);
			this.targetTimer -= delta;
			if (this.targetTimer < 0f)
			{
				this.targetTimer = 1f;
				this.ChooseCombatTarget();
				if (!this.combatModule.priorityTarget)
				{
					BasePoiManager current = BasePoiManager.current;
					if (current != null && !current.worldCoordinates.Contains(this.combatModule.parent.currentDestination))
					{
						Vector2 position = SeededRandom.Global.RandomWithinRect(BasePoiManager.current.worldCoordinates);
						this.combatModule.parent.SetOverrideDestination(position, true, false, false);
					}
				}
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001E9C8 File Offset: 0x0001CBC8
		public void ChooseCombatTarget()
		{
			AbstractUnit priorityTarget = this.threat.GetPriorityTarget(true);
			if (priorityTarget)
			{
				this.parent.SetManualTarget(priorityTarget);
				return;
			}
			if (!this.combatModule.autoTarget)
			{
				Collider2D[] array = Physics2D.OverlapCircleAll(this.parent.transform.position, 100f);
				TargetableUnit targetableUnit = null;
				float num = 999f;
				foreach (Collider2D collider2D in array)
				{
					TargetableUnit component;
					if (collider2D.gameObject.activeInHierarchy && (component = collider2D.GetComponent<TargetableUnit>()) && this.combatModule.IsValidTarget(component))
					{
						float num2 = Vector2.Distance(collider2D.gameObject.transform.position, this.parent.transform.position);
						if (num2 < num)
						{
							num = num2;
							targetableUnit = component;
						}
					}
				}
				if (targetableUnit)
				{
					this.parent.SetManualTarget(targetableUnit);
				}
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001EACC File Offset: 0x0001CCCC
		public override void OnDamageTaken(DamageData data)
		{
			if (data.sourceUnit && this.combatModule && this.combatModule.IsValidTarget(data.sourceUnit))
			{
				this.threat.Add(data.sourceUnit, data.totalDamageAmount);
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001EB20 File Offset: 0x0001CD20
		public void TauntedBy(AbstractUnit unit)
		{
			if (this.combatModule && this.combatModule.IsValidTarget(unit))
			{
				ValueTuple<AbstractUnit, float> priorityTargetThreat = this.threat.GetPriorityTargetThreat(true);
				AbstractUnit item = priorityTargetThreat.Item1;
				float item2 = priorityTargetThreat.Item2;
				if (unit != item)
				{
					this.threat.Set(unit, item2 * 1.1f);
				}
			}
		}

		// Token: 0x04000223 RID: 547
		private CombatModule combatModule;

		// Token: 0x04000225 RID: 549
		private float targetTimer;
	}
}
