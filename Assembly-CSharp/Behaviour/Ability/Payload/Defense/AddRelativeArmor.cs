using System;
using Behaviour.Equipment.Aspect;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Ability.Payload.Defense
{
	// Token: 0x020003E0 RID: 992
	public class AddRelativeArmor : MonoBehaviour
	{
		// Token: 0x060025BF RID: 9663 RVA: 0x000D2664 File Offset: 0x000D0864
		private void Start()
		{
			BoostStat component = base.GetComponent<BoostStat>();
			AbstractUnit componentInParent = base.GetComponentInParent<AbstractUnit>();
			component.SetStatBoost(componentInParent.baseHullHP * (0.2f * (float)component.stackSize));
		}

		// Token: 0x040016F6 RID: 5878
		public const float RelativeArmorMultiplier = 0.2f;
	}
}
