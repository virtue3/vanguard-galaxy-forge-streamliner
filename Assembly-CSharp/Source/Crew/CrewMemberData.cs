using System;
using System.Collections.Generic;
using Behaviour.Crew;
using LightJson;
using Source.Galaxy;
using Source.Item;
using Source.Player;
using Source.Util;

namespace Source.Crew
{
	// Token: 0x02000127 RID: 295
	public class CrewMemberData : AbstractCrewData
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x00052D8B File Offset: 0x00050F8B
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x00052D93 File Offset: 0x00050F93
		public string guid { get; private set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x00052D9C File Offset: 0x00050F9C
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x00052DA4 File Offset: 0x00050FA4
		public Rarity rarity { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x00052DAD File Offset: 0x00050FAD
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x00052DB5 File Offset: 0x00050FB5
		public List<SkilltreeNode> skillNodes { get; private set; } = new List<SkilltreeNode>();

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00052DBE File Offset: 0x00050FBE
		// (set) Token: 0x06000B56 RID: 2902 RVA: 0x00052DC6 File Offset: 0x00050FC6
		public bool critical { get; private set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00052DCF File Offset: 0x00050FCF
		public IEnumerable<SkilltreeNode> unlockedNodes
		{
			get
			{
				foreach (SkilltreeNode skilltreeNode in this.skillNodes)
				{
					if (skilltreeNode && skilltreeNode.crewLevelRequired <= base.level)
					{
						yield return skilltreeNode;
					}
				}
				List<SkilltreeNode>.Enumerator enumerator = default(List<SkilltreeNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00052DDF File Offset: 0x00050FDF
		public int purchaseCost
		{
			get
			{
				return GameMath.GetCreditsValue(this.rarity.GetCostMultiplier() * 200f, SystemMapData.current.level);
			}
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00052E04 File Offset: 0x00051004
		public CrewMemberData()
		{
			this.guid = Guid.NewGuid().ToString();
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00052E3B File Offset: 0x0005103B
		public CrewMemberData(string guid)
		{
			this.guid = guid;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00052E58 File Offset: 0x00051058
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			JsonArray jsonArray = new JsonArray();
			foreach (SkilltreeNode skilltreeNode in this.skillNodes)
			{
				jsonArray.Add((skilltreeNode != null) ? skilltreeNode.identifier : null);
			}
			result["guid"] = this.guid;
			result["rarity"] = this.rarity.ToString();
			result["profession"] = this.profession.ToString();
			result["skillNodes"] = jsonArray;
			result["cr"] = new bool?(this.critical);
			return result;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00052F58 File Offset: 0x00051158
		public static CrewMemberData FromJson(JsonValue data)
		{
			if (data.IsString)
			{
				return GamePlayer.current.GetCrewMember(data);
			}
			CrewMemberData crewMemberData = new CrewMemberData(data["guid"].AsString);
			crewMemberData.DataFromJson(data);
			crewMemberData.rarity = Enum.Parse<Rarity>(data["rarity"]);
			crewMemberData.profession = Enum.Parse<Profession>(data["profession"]);
			if (data["skillNodes"].IsJsonArray)
			{
				foreach (JsonValue jsonValue in data["skillNodes"].AsJsonArray)
				{
					SkilltreeNode crewNode = SkilltreeNode.GetCrewNode(jsonValue);
					if (crewNode != null)
					{
						crewMemberData.skillNodes.Add(crewNode);
					}
				}
			}
			if (!data["cr"].IsNull)
			{
				crewMemberData.critical = data["cr"];
			}
			return crewMemberData;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00053088 File Offset: 0x00051288
		protected static CrewMemberData CreateBasicMember(SeededRandom random)
		{
			CrewMemberData crewMemberData = new CrewMemberData();
			crewMemberData.gender = random.ChooseEnum<Gender>(0);
			NameGenerator.GiveCrewMemberRandomName(crewMemberData, random, null);
			crewMemberData.SetIcon(CrewIcons.GetRandom(crewMemberData.gender == Gender.Male, random));
			return crewMemberData;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000530B9 File Offset: 0x000512B9
		public static CrewMemberData CreateCustomCrewMember(Gender gender, string firstName, string callsign, string lastName, CrewIcon icon, Profession profession, Rarity rarity, SeededRandom random = null, bool critical = false)
		{
			CrewMemberData crewMemberData = new CrewMemberData();
			crewMemberData.SetName(firstName, callsign, lastName);
			crewMemberData.gender = gender;
			crewMemberData.profession = profession;
			crewMemberData.rarity = rarity;
			crewMemberData.SetIcon(icon);
			crewMemberData.critical = critical;
			CrewMemberData.SetSkillNodes(crewMemberData, random);
			return crewMemberData;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x000530F8 File Offset: 0x000512F8
		public static CrewMemberData CreateRandomCrewMember(SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			CrewMemberData crewMemberData = CrewMemberData.CreateBasicMember(random);
			crewMemberData.profession = random.ChooseEnum<Profession>(1);
			crewMemberData.rarity = Rarity.Standard;
			CrewMemberData.SetSkillNodes(crewMemberData, random);
			return crewMemberData;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00053128 File Offset: 0x00051328
		private static void SetSkillNodes(CrewMemberData crewMemberData, SeededRandom random = null)
		{
			if (random == null)
			{
				random = new SeedGenerator().Add(SeededRandom.Global.RandomString(16)).CreateRandom();
			}
			List<SkilltreeNode> list = new List<SkilltreeNode>(Skilltree.GetNodesForCrew(crewMemberData.profession));
			crewMemberData.skillNodes.Add(CrewMemberData.ExtractMinorNode(list, 1, random));
			crewMemberData.skillNodes.Add(CrewMemberData.ExtractMinorNode(list, 2, random));
			crewMemberData.skillNodes.Add(random.Choose<SkilltreeNode>(SkilltreeNode.GetMajorNodes(crewMemberData.profession, 3)));
			if (crewMemberData.rarity >= Rarity.Enhanced)
			{
				crewMemberData.skillNodes.Add(CrewMemberData.ExtractMinorNode(list, 4, random));
				crewMemberData.skillNodes.Add(CrewMemberData.ExtractMinorNode(list, 5, random));
				crewMemberData.skillNodes.Add(random.Choose<SkilltreeNode>(SkilltreeNode.GetMajorNodes(crewMemberData.profession, 6)));
			}
			if (crewMemberData.rarity >= Rarity.HighGrade)
			{
				crewMemberData.skillNodes.Add(CrewMemberData.ExtractMinorNode(list, 7, random));
				crewMemberData.skillNodes.Add(CrewMemberData.ExtractMinorNode(list, 8, random));
				crewMemberData.skillNodes.Add(random.Choose<SkilltreeNode>(SkilltreeNode.GetMajorNodes(crewMemberData.profession, 9)));
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00053244 File Offset: 0x00051444
		private static SkilltreeNode ExtractMinorNode(List<SkilltreeNode> list, int tier, SeededRandom random)
		{
			List<SkilltreeNode> list2 = new List<SkilltreeNode>();
			foreach (SkilltreeNode skilltreeNode in list)
			{
				if (skilltreeNode.tier == tier)
				{
					list2.Add(skilltreeNode);
				}
			}
			SkilltreeNode skilltreeNode2 = random.Choose<SkilltreeNode>(list2);
			list.Remove(skilltreeNode2);
			return skilltreeNode2;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x000532B4 File Offset: 0x000514B4
		public void SetCritical(bool critical)
		{
			this.critical = critical;
		}

		// Token: 0x04000611 RID: 1553
		public Profession profession;
	}
}
