using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.Unit;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000066 RID: 102
	public class ThreatTable
	{
		// Token: 0x060003CA RID: 970 RVA: 0x0001EB7D File Offset: 0x0001CD7D
		public ThreatTable(AbstractUnit parent)
		{
			this.parent = parent;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0001EBB0 File Offset: 0x0001CDB0
		public void Update(float delta)
		{
			this.updateTimer -= delta;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				this.ignore.Clear();
				float maxCombatRange = this.parent.GetMaxCombatRange();
				float num;
				if (!this.parent.engine)
				{
					num = maxCombatRange * 0.9f;
				}
				else
				{
					num = maxCombatRange * 1.5f;
				}
				foreach (AbstractUnit abstractUnit in this.threat.Keys)
				{
					if (abstractUnit && Vector2.Distance(abstractUnit.targetablePosition, this.parent.targetablePosition) > num)
					{
						this.ignore.Add(abstractUnit);
					}
				}
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0001EC94 File Offset: 0x0001CE94
		public void Add(AbstractUnit unit, float amt)
		{
			if (!this.threat.ContainsKey(unit))
			{
				this.threat[unit] = amt;
			}
			else
			{
				Dictionary<AbstractUnit, float> dictionary = this.threat;
				dictionary[unit] += amt;
			}
			Drone drone = unit as Drone;
			if (drone != null)
			{
				this.Add(drone.droneCommander, amt * 0.6f);
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0001ECF4 File Offset: 0x0001CEF4
		public void Set(AbstractUnit unit, float amt)
		{
			this.threat[unit] = amt;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0001ED03 File Offset: 0x0001CF03
		public AbstractUnit GetPriorityTarget(bool useIgnore = true)
		{
			return this.GetPriorityTargetThreat(useIgnore).Item1;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001ED14 File Offset: 0x0001CF14
		public ValueTuple<AbstractUnit, float> GetPriorityTargetThreat(bool useIgnore = true)
		{
			this.cleanUp.Clear();
			AbstractUnit abstractUnit = null;
			float num = 0f;
			foreach (KeyValuePair<AbstractUnit, float> keyValuePair in this.threat)
			{
				if (!keyValuePair.Key || keyValuePair.Key.isDestroyed || !keyValuePair.Key.gameObject.activeSelf)
				{
					this.cleanUp.Add(keyValuePair.Key);
				}
				else if (keyValuePair.Value > num && (!useIgnore || !this.ignore.Contains(keyValuePair.Key)))
				{
					abstractUnit = keyValuePair.Key;
					num = keyValuePair.Value;
				}
			}
			foreach (AbstractUnit key in this.cleanUp)
			{
				this.threat.Remove(key);
			}
			if (abstractUnit == null && useIgnore)
			{
				return this.GetPriorityTargetThreat(false);
			}
			return new ValueTuple<AbstractUnit, float>(abstractUnit, num);
		}

		// Token: 0x04000226 RID: 550
		private Dictionary<AbstractUnit, float> threat = new Dictionary<AbstractUnit, float>();

		// Token: 0x04000227 RID: 551
		private List<AbstractUnit> cleanUp = new List<AbstractUnit>();

		// Token: 0x04000228 RID: 552
		private AbstractUnit parent;

		// Token: 0x04000229 RID: 553
		private float updateTimer;

		// Token: 0x0400022A RID: 554
		private HashSet<AbstractUnit> ignore = new HashSet<AbstractUnit>();
	}
}
