using System;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Equipment.Aspect.Custom
{
	// Token: 0x0200037B RID: 891
	public class ArmorAutoRepairAspect : MonoBehaviour
	{
		// Token: 0x06002261 RID: 8801 RVA: 0x000C72A8 File Offset: 0x000C54A8
		private void Start()
		{
			this.unit = base.GetComponentInParent<AbstractUnit>();
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000C72B8 File Offset: 0x000C54B8
		private void Update()
		{
			this.repairTimer -= Time.deltaTime;
			if (this.repairTimer < 0f)
			{
				this.repairTimer = 5f;
				if (this.unit && this.unit.maxArmorHP > 0f)
				{
					this.unit.currentArmorHP = Mathf.Min(this.unit.maxArmorHP, this.unit.currentArmorHP + this.unit.maxArmorHP * 0.005f);
				}
			}
		}

		// Token: 0x04001451 RID: 5201
		public const float repairInterval = 5f;

		// Token: 0x04001452 RID: 5202
		public const float repairPerTick = 0.005f;

		// Token: 0x04001453 RID: 5203
		private AbstractUnit unit;

		// Token: 0x04001454 RID: 5204
		private float repairTimer = 5f;
	}
}
