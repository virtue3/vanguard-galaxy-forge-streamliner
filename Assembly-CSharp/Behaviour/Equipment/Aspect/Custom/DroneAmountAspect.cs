using System;
using Behaviour.Equipment.Module;
using UnityEngine;

namespace Behaviour.Equipment.Aspect.Custom
{
	// Token: 0x0200037C RID: 892
	public class DroneAmountAspect : EquipAspect
	{
		// Token: 0x06002264 RID: 8804 RVA: 0x000C735C File Offset: 0x000C555C
		public override void Initialize(AbstractEquipment equipment)
		{
			DroneBayModule droneBayModule = equipment as DroneBayModule;
			if (droneBayModule != null)
			{
				droneBayModule.droneBonusAmount += this.amount;
			}
		}

		// Token: 0x04001455 RID: 5205
		[SerializeField]
		private int amount = 1;
	}
}
