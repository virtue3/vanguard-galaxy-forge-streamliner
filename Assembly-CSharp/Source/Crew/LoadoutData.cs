using System;
using System.Collections.Generic;
using LightJson;

namespace Source.Crew
{
	// Token: 0x02000129 RID: 297
	public class LoadoutData : IJsonSource
	{
		// Token: 0x06000B63 RID: 2915 RVA: 0x000532BD File Offset: 0x000514BD
		public LoadoutData(string name)
		{
			this.name = name;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x000532D8 File Offset: 0x000514D8
		public JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (KeyValuePair<string, List<SkillNodeData>> keyValuePair in this.skills)
			{
				JsonObject jsonObject = new JsonObject();
				jsonObject["skillTree"] = keyValuePair.Key;
				jsonObject["skillData"] = keyValuePair.Value.ToJsonArray<SkillNodeData>();
				jsonArray.Add(jsonObject);
			}
			JsonObject jsonObject2 = new JsonObject();
			jsonObject2["name"] = this.name;
			jsonObject2["skillTrees"] = jsonArray;
			return jsonObject2;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x000533A4 File Offset: 0x000515A4
		public static LoadoutData FromJson(JsonValue data, List<SkillTreeData> skillTrees)
		{
			LoadoutData loadoutData = new LoadoutData(data["name"]);
			if (data["skillTrees"].IsNull)
			{
				return loadoutData;
			}
			foreach (JsonValue jsonValue in data["skillTrees"].AsJsonArray)
			{
				List<SkillNodeData> list = new List<SkillNodeData>();
				string key = jsonValue["skillTree"];
				SkillTreeData skillTree = LoadoutData.GetSkillTree(key, skillTrees);
				foreach (JsonValue data2 in jsonValue["skillData"].AsJsonArray)
				{
					list.Add(SkillNodeData.FromJson(skillTree, data2));
				}
				loadoutData.skills.Add(key, list);
			}
			return loadoutData;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x000534BC File Offset: 0x000516BC
		private static SkillTreeData GetSkillTree(string name, List<SkillTreeData> skillTrees)
		{
			foreach (SkillTreeData skillTreeData in skillTrees)
			{
				if (skillTreeData.skilltree.identifier == name)
				{
					return skillTreeData;
				}
			}
			return null;
		}

		// Token: 0x0400061B RID: 1563
		public string name;

		// Token: 0x0400061C RID: 1564
		public Dictionary<string, List<SkillNodeData>> skills = new Dictionary<string, List<SkillNodeData>>();
	}
}
