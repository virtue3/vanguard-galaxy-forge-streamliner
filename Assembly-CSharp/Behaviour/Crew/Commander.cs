using System;
using System.Collections.Generic;
using Behaviour.Equipment.Aspect;
using Source.Crew;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A1 RID: 929
	public class Commander : CrewMember
	{
		// Token: 0x0600232C RID: 9004 RVA: 0x000C9874 File Offset: 0x000C7A74
		public void SetCommanderData(CommanderData data, IEnumerable<CrewMemberData> crew)
		{
			this.commanderData = data;
			this.crewData = crew;
			this.CheckExperience();
		}

		// Token: 0x0600232D RID: 9005 RVA: 0x000C988C File Offset: 0x000C7A8C
		public override void CheckExperience()
		{
			base.transform.DestroyChildren();
			foreach (SkillTreeData skillTreeData in this.commanderData.skillTrees)
			{
				foreach (KeyValuePair<SkilltreeNode, int> keyValuePair in skillTreeData.GetActivatedSkills())
				{
					base.ProcessSkill(keyValuePair.Key, keyValuePair.Value + this.GetCrewSkillRanks(keyValuePair.Key));
				}
				GameObject[] passiveMasteryBonusses = skillTreeData.skilltree.passiveMasteryBonusses;
				for (int i = 0; i < passiveMasteryBonusses.Length; i++)
				{
					BoostStat[] components = UnityEngine.Object.Instantiate<GameObject>(passiveMasteryBonusses[i], base.transform).GetComponents<BoostStat>();
					for (int j = 0; j < components.Length; j++)
					{
						components[j].SetStackSize(skillTreeData.masteryLevel);
					}
				}
				int num = skillTreeData.masteryLevel / 10;
				GameObject[] milestonesMastery = skillTreeData.skilltree.milestonesMastery;
				int num2 = 0;
				while (num2 < num && num2 < milestonesMastery.Length)
				{
					GameObject gameObject = milestonesMastery[num2];
					if (gameObject != null)
					{
						UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform);
					}
					num2++;
				}
			}
		}

		// Token: 0x0600232E RID: 9006 RVA: 0x000C9A08 File Offset: 0x000C7C08
		private int GetCrewSkillRanks(SkilltreeNode node)
		{
			int num = 0;
			foreach (CrewMemberData crewMemberData in this.crewData)
			{
				if (crewMemberData != null)
				{
					using (IEnumerator<SkilltreeNode> enumerator2 = crewMemberData.unlockedNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current == node)
							{
								num++;
							}
						}
					}
				}
			}
			return num;
		}

		// Token: 0x04001505 RID: 5381
		private CommanderData commanderData;

		// Token: 0x04001506 RID: 5382
		private IEnumerable<CrewMemberData> crewData;
	}
}
