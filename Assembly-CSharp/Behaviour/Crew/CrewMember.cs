using System;
using System.Collections.Generic;
using Behaviour.Ability;
using Behaviour.Equipment.Aspect;
using Source.Crew;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A5 RID: 933
	public class CrewMember : MonoBehaviour
	{
		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x000C9FCA File Offset: 0x000C81CA
		// (set) Token: 0x06002343 RID: 9027 RVA: 0x000C9FD2 File Offset: 0x000C81D2
		public CrewMemberData crewMemberData { get; protected set; }

		// Token: 0x06002344 RID: 9028 RVA: 0x000C9FDB File Offset: 0x000C81DB
		public void SetCrewMemberData(CrewMemberData data)
		{
			this.crewMemberData = data;
			this.CheckExperience();
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000C9FEC File Offset: 0x000C81EC
		public virtual void CheckExperience()
		{
			base.transform.DestroyChildren();
			foreach (SkilltreeNode skilltreeNode in this.crewMemberData.unlockedNodes)
			{
				if (skilltreeNode.parent == null)
				{
					this.ProcessSkill(skilltreeNode, 1);
				}
			}
		}

		// Token: 0x06002346 RID: 9030 RVA: 0x000CA058 File Offset: 0x000C8258
		protected void ProcessSkill(SkilltreeNode skill, int stackSize)
		{
			if (stackSize == 0)
			{
				return;
			}
			using (IEnumerator<BoostStat> enumerator = skill.statBoosts.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					BoostStat boostStat = UnityEngine.Object.Instantiate<BoostStat>(enumerator.Current, base.transform);
					boostStat.GetComponent<SkilltreeNode>().SetParent(skill.parent);
					boostStat.gameObject.name = skill.name;
					BoostStat[] componentsInChildren = boostStat.GetComponentsInChildren<BoostStat>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].SetStackSize(stackSize);
					}
				}
			}
			foreach (AbstractAbility original in skill.abilities)
			{
				AbstractAbility abstractAbility = UnityEngine.Object.Instantiate<AbstractAbility>(original, base.transform);
				abstractAbility.SetStackSize(stackSize);
				ActivatedAbility activatedAbility = abstractAbility as ActivatedAbility;
				if (activatedAbility != null && string.IsNullOrEmpty(activatedAbility.descriptionText))
				{
					activatedAbility.descriptionText = skill.GetAbilityDescription();
				}
			}
		}

		// Token: 0x0400151D RID: 5405
		protected static CrewMember crewMember;
	}
}
