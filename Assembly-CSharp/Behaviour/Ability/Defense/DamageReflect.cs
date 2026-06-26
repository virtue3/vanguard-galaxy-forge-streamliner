using System;
using Behaviour.Weapons;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003C8 RID: 968
	public class DamageReflect : TriggeredPayload
	{
		// Token: 0x0600255F RID: 9567 RVA: 0x000D0A74 File Offset: 0x000CEC74
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			if (damageData.sourceUnit == null)
			{
				return;
			}
			float num = damageData.totalDamageAmount * (0.05f * (float)stackSize);
			num = Mathf.Max(num, 1f);
			num = Mathf.Min(num, damageData.sourceUnit.maxTotalHP * 0.05f);
			DamageData data = new DamageData(damageData.targetUnit.gameObject)
			{
				damageAmount = num,
				hitCoordinates = damageData.sourceUnit.transform.position,
				hitTransform = damageData.sourceUnit.transform,
				power = damageData.power,
				type = DamageType.Radiation,
				targetUnit = damageData.sourceUnit,
				reflectedDamage = true,
				criticalChance = 0f
			};
			damageData.damageAmount -= num;
			damageData.sourceUnit.TakeDamage(data);
		}

		// Token: 0x040016AF RID: 5807
		public const float DamageReflectMultiplier = 0.05f;
	}
}
