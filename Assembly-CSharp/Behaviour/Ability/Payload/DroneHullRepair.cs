using System;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D4 RID: 980
	public class DroneHullRepair : MonoBehaviour
	{
		// Token: 0x06002595 RID: 9621 RVA: 0x000D19B0 File Offset: 0x000CFBB0
		private void Start()
		{
			this.unit = base.GetComponentInParent<AbstractUnit>();
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000D19C0 File Offset: 0x000CFBC0
		private void Update()
		{
			this.repairTimer -= Time.deltaTime;
			if (this.repairTimer < 0f)
			{
				this.repairTimer = 2f;
				if (this.unit && this.unit.maxHullHP > 0f)
				{
					this.unit.currentHullHP = Mathf.Min(this.unit.maxHullHP, this.unit.currentHullHP + this.unit.maxHullHP * 0.05f);
				}
			}
		}

		// Token: 0x040016DA RID: 5850
		public const float repairInterval = 2f;

		// Token: 0x040016DB RID: 5851
		public const float repairPerTick = 0.05f;

		// Token: 0x040016DC RID: 5852
		private AbstractUnit unit;

		// Token: 0x040016DD RID: 5853
		private float repairTimer = 2f;
	}
}
