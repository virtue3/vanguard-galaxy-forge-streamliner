using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D1 RID: 977
	public class ArmorLeech : TriggeredPayload
	{
		// Token: 0x0600258B RID: 9611 RVA: 0x000D1710 File Offset: 0x000CF910
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			if (damageData.sourceUnit == null)
			{
				return;
			}
			AbstractUnit sourceUnit = damageData.sourceUnit;
			if (sourceUnit != null)
			{
				float num = damageData.damageAmount * (0.005f * (float)stackSize);
				float a = sourceUnit.currentArmorHP + num;
				sourceUnit.currentArmorHP = Mathf.Min(a, sourceUnit.maxArmorHP);
			}
		}

		// Token: 0x040016D5 RID: 5845
		public const float ArmorLeechAmount = 0.005f;
	}
}
