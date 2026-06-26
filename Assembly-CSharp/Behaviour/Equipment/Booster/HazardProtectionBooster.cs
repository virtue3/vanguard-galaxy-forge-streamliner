using System;
using Behaviour.Ability;
using Behaviour.Hazard;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Equipment.Booster
{
	// Token: 0x02000377 RID: 887
	public class HazardProtectionBooster : TriggeredPayload
	{
		// Token: 0x0600224B RID: 8779 RVA: 0x000C6F14 File Offset: 0x000C5114
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = source as DamageData;
			if (damageData != null && damageData.source && damageData.source.GetComponent<LocalHazard>())
			{
				damageData.OverrideDamageAmount(damageData.damageAmount * this.damageReduction);
				damageData.damageForceMovement = false;
			}
		}

		// Token: 0x04001446 RID: 5190
		[SerializeField]
		private float damageReduction = 0.5f;
	}
}
