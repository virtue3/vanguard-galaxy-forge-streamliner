using System;
using System.Collections.Generic;
using Source.Item;
using UnityEngine;

namespace Behaviour.Equipment.Aspect
{
	// Token: 0x0200037A RID: 890
	public class TurretBoostStat : MonoBehaviour
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x000C7285 File Offset: 0x000C5485
		public IEnumerable<EquipStatLine> GetStats(int stackSize = 1)
		{
			yield return new EquipStatLine(this.stat, this.statBoost, this.statMultiplier, true);
			yield break;
		}

		// Token: 0x0400144E RID: 5198
		[SerializeField]
		private float statBoost;

		// Token: 0x0400144F RID: 5199
		[SerializeField]
		private float statMultiplier = 1f;

		// Token: 0x04001450 RID: 5200
		[SerializeField]
		private EquipStat stat;
	}
}
