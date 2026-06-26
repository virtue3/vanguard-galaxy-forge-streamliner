using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using LightJson;
using Source.Util;

namespace Source.Crew
{
	// Token: 0x02000130 RID: 304
	public class SkillTreeData : IJsonSource
	{
		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00053B8A File Offset: 0x00051D8A
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00053B92 File Offset: 0x00051D92
		public bool locked { get; private set; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00053B9B File Offset: 0x00051D9B
		public float maxExperience
		{
			get
			{
				return GameMath.GetMaxExperienceForLevel(this.masteryLevel);
			}
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00053BA8 File Offset: 0x00051DA8
		public SkillTreeData(Skilltree tree)
		{
			this.skilltree = tree;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00053BC2 File Offset: 0x00051DC2
		public SkillTreeData(Skilltree tree, bool locked)
		{
			this.locked = locked;
			this.skilltree = tree;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00053BE4 File Offset: 0x00051DE4
		public List<SkilltreeNode> GetSkillsForSkillPoints(int skillPoints)
		{
			List<SkilltreeNode> list = new List<SkilltreeNode>();
			foreach (SkilltreeNode skilltreeNode in this.skilltree.allNodes)
			{
				if (skilltreeNode.CanInvestSkillPoints())
				{
					list.Add(skilltreeNode);
				}
			}
			return list;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00053C48 File Offset: 0x00051E48
		public IEnumerable<KeyValuePair<SkilltreeNode, int>> GetActivatedSkills()
		{
			foreach (SkilltreeNode skilltreeNode in this.skilltree.allNodes)
			{
				SkillNodeData skillNodeData;
				this.nodes.TryGetValue(skilltreeNode.identifier, out skillNodeData);
				yield return KeyValuePair.Create<SkilltreeNode, int>(skilltreeNode, (skillNodeData != null) ? skillNodeData.currentPoints : 0);
			}
			IEnumerator<SkilltreeNode> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00053C58 File Offset: 0x00051E58
		public int GetCurrentPoints(SkilltreeNode node)
		{
			SkillNodeData skillNodeData;
			if (this.nodes.TryGetValue(node.identifier, out skillNodeData))
			{
				return skillNodeData.currentPoints;
			}
			return 0;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00053C84 File Offset: 0x00051E84
		public int GetInvestedSkillPoints()
		{
			int num = 0;
			foreach (SkillNodeData skillNodeData in this.nodes.Values)
			{
				num += skillNodeData.currentPoints;
			}
			return num;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00053CE4 File Offset: 0x00051EE4
		public void InvestSkillPoints(SkilltreeNode node, int count)
		{
			SkillNodeData skillNodeData;
			if (!this.nodes.TryGetValue(node.identifier, out skillNodeData))
			{
				skillNodeData = new SkillNodeData(this, node);
				this.nodes[node.identifier] = skillNodeData;
			}
			skillNodeData.InvestSkillPoints(count);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x00053D28 File Offset: 0x00051F28
		public void RemoveSkillPoints(SkilltreeNode node, int count)
		{
			SkillNodeData skillNodeData;
			if (this.nodes.TryGetValue(node.identifier, out skillNodeData))
			{
				skillNodeData.RemoveSkillPoints(count);
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x00053D54 File Offset: 0x00051F54
		public void AddMasteryXp(float exp)
		{
			if (this.masteryLevel >= GameMath.maxLevel)
			{
				return;
			}
			if (this.locked)
			{
				return;
			}
			this.masteryXp += exp;
			while (this.masteryXp >= this.maxExperience)
			{
				this.masteryXp -= this.maxExperience;
				this.masteryLevel++;
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x00053DB8 File Offset: 0x00051FB8
		public void RefundSkillpoints()
		{
			(from kv in this.nodes
			where kv.Value.currentPoints > 0
			select kv.Value.skill).ToList<SkilltreeNode>();
			this.nodes.Clear();
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00053E24 File Offset: 0x00052024
		public List<SkillNodeData> CopyNodes()
		{
			List<SkillNodeData> list = new List<SkillNodeData>();
			foreach (KeyValuePair<string, SkillNodeData> keyValuePair in this.nodes)
			{
				list.Add(keyValuePair.Value.GetCopy());
			}
			return list;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x00053E8C File Offset: 0x0005208C
		public JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (SkillNodeData skillNodeData in this.nodes.Values)
			{
				jsonArray.Add(skillNodeData.ToJson());
			}
			return new JsonObject
			{
				{
					"skilltree",
					this.skilltree.identifier
				},
				{
					"skillNodes",
					jsonArray
				},
				{
					"masteryXp",
					new double?((double)this.masteryXp)
				},
				{
					"masteryLevel",
					new double?((double)this.masteryLevel)
				},
				{
					"locked",
					new bool?(this.locked)
				}
			};
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00053F80 File Offset: 0x00052180
		public static SkillTreeData FromJson(JsonValue data)
		{
			SkillTreeData skillTreeData = new SkillTreeData(Skilltree.Get(data["skilltree"]));
			foreach (JsonValue data2 in data["skillNodes"].AsJsonArray)
			{
				SkillNodeData skillNodeData = SkillNodeData.FromJson(skillTreeData, data2);
				if (skillNodeData != null)
				{
					skillTreeData.nodes[skillNodeData.skill.identifier] = skillNodeData;
				}
			}
			if (!data["masteryXp"].IsNull)
			{
				skillTreeData.masteryXp = (float)data["masteryXp"].AsNumber;
			}
			if (!data["masteryLevel"].IsNull)
			{
				skillTreeData.masteryLevel = data["masteryLevel"];
			}
			if (!data["locked"].IsNull)
			{
				skillTreeData.locked = data["locked"];
			}
			return skillTreeData;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x000540A4 File Offset: 0x000522A4
		public static IEnumerable<SkillTreeData> GenerateForSpecialization(CommanderSpecialization starterSpecialization)
		{
			yield return new SkillTreeData(Skilltree.Get(SkillTreeData.GetSpecializationTreeName(starterSpecialization)));
			yield break;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x000540B4 File Offset: 0x000522B4
		public static string GetSpecializationTreeName(CommanderSpecialization starterSpecialization)
		{
			string result;
			switch (starterSpecialization)
			{
			case CommanderSpecialization.Leadership:
				result = "Leadership";
				break;
			case CommanderSpecialization.Mining:
				result = "Mining";
				break;
			case CommanderSpecialization.Drones:
				result = "Drones";
				break;
			case CommanderSpecialization.Engineering:
				result = "PromptEngineering";
				break;
			case CommanderSpecialization.Industrial:
				result = "Industrial";
				break;
			case CommanderSpecialization.Salvaging:
				result = "Salvaging";
				break;
			case CommanderSpecialization.Economy:
				result = "Economy";
				break;
			case CommanderSpecialization.Offense:
				result = "Combat - Offensive";
				break;
			case CommanderSpecialization.Defense:
				result = "Defense";
				break;
			default:
				result = "Mining";
				break;
			}
			return result;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00054140 File Offset: 0x00052340
		public bool HasSkill(SkilltreeNode node)
		{
			SkillNodeData skillNodeData;
			this.nodes.TryGetValue(node.identifier, out skillNodeData);
			return ((skillNodeData != null) ? skillNodeData.currentPoints : 0) > 0;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00054170 File Offset: 0x00052370
		public void Unlock()
		{
			this.locked = false;
		}

		// Token: 0x04000639 RID: 1593
		public readonly Skilltree skilltree;

		// Token: 0x0400063A RID: 1594
		private Dictionary<string, SkillNodeData> nodes = new Dictionary<string, SkillNodeData>();

		// Token: 0x0400063C RID: 1596
		public float masteryXp;

		// Token: 0x0400063D RID: 1597
		public int masteryLevel;
	}
}
