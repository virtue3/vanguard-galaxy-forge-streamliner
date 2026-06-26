using System;
using Behaviour.Unit;
using Behaviour.Weapons;

namespace Behaviour.Ability.Defense
{
	// Token: 0x020003C9 RID: 969
	public class ImmunityEffect : TriggeredPayload
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x000D0B64 File Offset: 0x000CED64
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			AbstractUnit abstractUnit = damageData.targetUnit as AbstractUnit;
			if (abstractUnit != null && (abstractUnit.armorModule || abstractUnit.shieldGeneratorModule))
			{
				damageData.damageAmount = 0f;
			}
		}
	}
}
