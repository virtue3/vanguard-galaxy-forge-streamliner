using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D3 RID: 979
	public class ShieldLeechOnHit : TriggeredPayload
	{
		// Token: 0x06002593 RID: 9619 RVA: 0x000D1950 File Offset: 0x000CFB50
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
				float a = sourceUnit.currentShieldHP + num;
				sourceUnit.currentShieldHP = Mathf.Min(a, sourceUnit.maxShieldHP);
			}
		}

		// Token: 0x040016D9 RID: 5849
		public const float ShieldLeech = 0.005f;
	}
}
