using System;
using Behaviour.Equipment.Aspect;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D0 RID: 976
	public class AddTemporaryResist : TriggeredPayload
	{
		// Token: 0x06002589 RID: 9609 RVA: 0x000D16B0 File Offset: 0x000CF8B0
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			if (damageData.sourceUnit == null)
			{
				return;
			}
			BoostStat component = base.GetComponent<BoostStat>();
			component.SetStat(damageData.type.GetResistStat());
			base.GetComponentInParent<AbstractUnit>();
			component.SetStatBoost(0.02f * (float)component.stackSize);
		}

		// Token: 0x040016D4 RID: 5844
		public const float RelativeResist = 0.02f;
	}
}
