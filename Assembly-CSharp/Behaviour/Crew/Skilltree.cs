using System;
using System.Collections.Generic;
using Source.Crew;
using Source.Player;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A6 RID: 934
	public class Skilltree : MonoBehaviour
	{
		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x000CA168 File Offset: 0x000C8368
		public static IEnumerable<Skilltree> all
		{
			get
			{
				return Skilltree.allTrees.Values;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06002349 RID: 9033 RVA: 0x000CA174 File Offset: 0x000C8374
		public string identifier
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x000CA17C File Offset: 0x000C837C
		public string displayName
		{
			get
			{
				return "@SkillTree" + this.identifier;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x0600234B RID: 9035 RVA: 0x000CA18E File Offset: 0x000C838E
		public IEnumerable<SkilltreeNode> allNodes
		{
			get
			{
				return this.nodes;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x000CA196 File Offset: 0x000C8396
		// (set) Token: 0x0600234D RID: 9037 RVA: 0x000CA19E File Offset: 0x000C839E
		public GameObject[] passiveMasteryBonusses { get; private set; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x000CA1A7 File Offset: 0x000C83A7
		// (set) Token: 0x0600234F RID: 9039 RVA: 0x000CA1AF File Offset: 0x000C83AF
		public GameObject[] milestonesMastery { get; private set; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x000CA1B8 File Offset: 0x000C83B8
		public int maxPoints
		{
			get
			{
				int num = 0;
				foreach (SkilltreeNode skilltreeNode in this.nodes)
				{
					num += skilltreeNode.maxSkillPoints;
				}
				return num;
			}
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000CA210 File Offset: 0x000C8410
		public void Initialize()
		{
			base.GetComponentsInChildren<SkilltreeNode>(this.nodes);
			foreach (SkilltreeNode skilltreeNode in this.nodes)
			{
				skilltreeNode.SetParent(this);
				skilltreeNode.Initialize();
			}
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000CA274 File Offset: 0x000C8474
		public int GetInvestedSkillPoints()
		{
			SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(this, false);
			if (skillTreeData == null)
			{
				return 0;
			}
			return skillTreeData.GetInvestedSkillPoints();
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000CA292 File Offset: 0x000C8492
		public int GetMasteryLevel()
		{
			SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(this, false);
			if (skillTreeData == null)
			{
				return 0;
			}
			return skillTreeData.masteryLevel;
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x000CA2B0 File Offset: 0x000C84B0
		public float GetMasteryXp()
		{
			SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(this, false);
			if (skillTreeData == null)
			{
				return 0f;
			}
			return skillTreeData.masteryXp;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000CA2D2 File Offset: 0x000C84D2
		public bool IsLocked()
		{
			return GamePlayer.current.commander.GetSkillTreeData(this, false).locked;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000CA2EC File Offset: 0x000C84EC
		public bool LeadershipUnlocked()
		{
			SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Leadership)), false);
			return skillTreeData != null && !skillTreeData.locked;
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000CA323 File Offset: 0x000C8523
		public void UnlockSkilltree()
		{
			GamePlayer.current.commander.GetSkillTreeData(this, false).Unlock();
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000CA33C File Offset: 0x000C853C
		public SkilltreeNode GetNode(string id)
		{
			foreach (SkilltreeNode skilltreeNode in this.nodes)
			{
				if (skilltreeNode.identifier == id)
				{
					return skilltreeNode;
				}
			}
			return null;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000CA3A0 File Offset: 0x000C85A0
		public static Skilltree Get(string name)
		{
			Skilltree result;
			Skilltree.allTrees.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000CA3BC File Offset: 0x000C85BC
		public static void LoadAll()
		{
			Skilltree.allTrees.Clear();
			Skilltree[] array = Resources.LoadAll<Skilltree>("Crew/Skilltree");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					Skilltree.allTrees[array[i].identifier] = array[i];
					array[i].Initialize();
				}
			}
			SkilltreeNode.LoadStaticNodes();
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000CA422 File Offset: 0x000C8622
		public static IEnumerable<SkilltreeNode> GetNodesForCrew(Profession profession)
		{
			foreach (Skilltree skilltree in Skilltree.all)
			{
				foreach (SkilltreeNode skilltreeNode in skilltree.allNodes)
				{
					if (skilltreeNode.maxSkillPoints > 1 && skilltreeNode.availableForCrew.HasFlag(profession))
					{
						yield return skilltreeNode;
					}
				}
				IEnumerator<SkilltreeNode> enumerator2 = null;
			}
			IEnumerator<Skilltree> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400151F RID: 5407
		public const float IndustrialMasteryRefCraftSpeedIncrease = 0.005f;

		// Token: 0x04001520 RID: 5408
		public const float AutopilotPenaltyReduction = 0.001f;

		// Token: 0x04001521 RID: 5409
		public const float EconomySupplyIncrease = 0.01f;

		// Token: 0x04001522 RID: 5410
		private static Dictionary<string, Skilltree> allTrees = new Dictionary<string, Skilltree>();

		// Token: 0x04001523 RID: 5411
		private List<SkilltreeNode> nodes = new List<SkilltreeNode>();
	}
}
