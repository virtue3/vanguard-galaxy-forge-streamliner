using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003CB RID: 971
	public class RepairDroneTrigger : TriggeredPayload
	{
		// Token: 0x0600256B RID: 9579 RVA: 0x000D0F0C File Offset: 0x000CF10C
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			AbstractUnit abstractUnit = ((DamageData)source).targetUnit as AbstractUnit;
			if (abstractUnit != null)
			{
				for (int i = 0; i < stackSize; i++)
				{
					RepairDrone repairDrone = UnityEngine.Object.Instantiate<RepairDrone>(this.repairDronePrefab, abstractUnit.transform.position, Quaternion.identity);
					float value = 6.28318548f / (float)stackSize * (float)i;
					repairDrone.Initialize(abstractUnit, 20f, 0.01f, new float?(value));
				}
			}
		}

		// Token: 0x040016BB RID: 5819
		public const float ArmorRepairAmount = 0.01f;

		// Token: 0x040016BC RID: 5820
		[SerializeField]
		private RepairDrone repairDronePrefab;
	}
}
