using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003CC RID: 972
	public class ShieldCriticalHeal : TriggeredPayload
	{
		// Token: 0x0600256D RID: 9581 RVA: 0x000D0F80 File Offset: 0x000CF180
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			AbstractUnit abstractUnit = ((DamageData)source).targetUnit as AbstractUnit;
			if (abstractUnit != null)
			{
				float a = abstractUnit.currentShieldHP + abstractUnit.maxShieldHP * (0.02f * (float)stackSize);
				abstractUnit.currentShieldHP = Mathf.Min(a, abstractUnit.maxShieldHP);
			}
		}

		// Token: 0x040016BD RID: 5821
		public const float ShieldCriticalHealAmount = 0.02f;
	}
}
