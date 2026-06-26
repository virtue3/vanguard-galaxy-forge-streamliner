using System;
using Behaviour.Crew;
using Behaviour.UI.HUD;
using LightJson;
using Source.Player;
using UnityEngine;

namespace Source.Crew
{
	// Token: 0x0200012E RID: 302
	public class SkillNodeData : IJsonSource
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x000539C1 File Offset: 0x00051BC1
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x000539C9 File Offset: 0x00051BC9
		public SkilltreeNode skill { get; private set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x000539D2 File Offset: 0x00051BD2
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x000539DA File Offset: 0x00051BDA
		public int currentPoints { get; private set; }

		// Token: 0x06000B77 RID: 2935 RVA: 0x000539E3 File Offset: 0x00051BE3
		public SkillNodeData(SkillTreeData parent, SkilltreeNode skill)
		{
			this.parent = parent;
			this.skill = skill;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000539F9 File Offset: 0x00051BF9
		public SkillNodeData GetCopy()
		{
			return new SkillNodeData(this.parent, this.skill)
			{
				currentPoints = this.currentPoints
			};
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00053A18 File Offset: 0x00051C18
		public bool InvestSkillPoints(int skillpoints)
		{
			if (GamePlayer.current.commander.GetRemainingSkillPoints() > 0 && this.skill.maxSkillPoints >= this.currentPoints + skillpoints)
			{
				this.currentPoints += skillpoints;
				if (GameplayManager.Instance)
				{
					GameplayManager.Instance.spaceShip.UpdateCommanderSkills();
					AbilityHud.instance.ResetHud(true);
				}
				int currentPoints = this.currentPoints;
				return true;
			}
			return false;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00053A8C File Offset: 0x00051C8C
		public bool RemoveSkillPoints(int skillpoints)
		{
			if (this.currentPoints > 0)
			{
				this.currentPoints -= Mathf.Min(skillpoints, this.currentPoints);
				if (GameplayManager.Instance)
				{
					GameplayManager.Instance.spaceShip.UpdateCommanderSkills();
					AbilityHud.instance.ResetHud(true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x00053AE4 File Offset: 0x00051CE4
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"skill",
					this.skill.identifier
				},
				{
					"currentPoints",
					new double?((double)this.currentPoints)
				}
			};
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x00053B34 File Offset: 0x00051D34
		public static SkillNodeData FromJson(SkillTreeData parent, JsonValue data)
		{
			SkilltreeNode node = parent.skilltree.GetNode(data["skill"]);
			if (node == null)
			{
				return null;
			}
			return new SkillNodeData(parent, node)
			{
				currentPoints = data["currentPoints"].AsInteger
			};
		}

		// Token: 0x04000630 RID: 1584
		public readonly SkillTreeData parent;
	}
}
