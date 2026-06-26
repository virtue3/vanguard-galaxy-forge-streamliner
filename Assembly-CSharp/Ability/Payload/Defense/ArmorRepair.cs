using System;
using Behaviour.Unit;
using UnityEngine;

namespace Ability.Payload.Defense
{
	// Token: 0x020001A3 RID: 419
	public class ArmorRepair : MonoBehaviour
	{
		// Token: 0x06000EB8 RID: 3768 RVA: 0x00068C1C File Offset: 0x00066E1C
		private void Start()
		{
			AbstractUnit abstractUnit;
			if (this.TryGetComponentInParent(out abstractUnit))
			{
				float a = abstractUnit.currentArmorHP + abstractUnit.maxArmorHP * this.repairPercentage;
				abstractUnit.currentArmorHP = Mathf.Min(a, abstractUnit.maxArmorHP);
			}
		}

		// Token: 0x0400084B RID: 2123
		[SerializeField]
		private float repairPercentage = 0.25f;
	}
}
