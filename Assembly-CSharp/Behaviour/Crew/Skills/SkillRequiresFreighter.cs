using System;
using Behaviour.Unit;
using Source.Util;
using UnityEngine;

namespace Behaviour.Crew.Skills
{
	// Token: 0x020003A9 RID: 937
	public class SkillRequiresFreighter : MonoBehaviour
	{
		// Token: 0x06002410 RID: 9232 RVA: 0x000CB6BC File Offset: 0x000C98BC
		private void Awake()
		{
			SpaceShip componentInParent = base.GetComponentInParent<SpaceShip>();
			if (componentInParent && componentInParent.shipRoleType.GetGameplayType() != GameplayType.Cargo)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
