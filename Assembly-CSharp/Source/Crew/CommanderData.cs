using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.UI.HUD;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.Util;
using LightJson;
using Source.Player;
using Source.Simulation.Story;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Crew
{
	// Token: 0x02000122 RID: 290
	public class CommanderData : AbstractCrewData
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00051FA5 File Offset: 0x000501A5
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x00051FAD File Offset: 0x000501AD
		public PersonalHistory personalHistory { get; private set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00051FB6 File Offset: 0x000501B6
		// (set) Token: 0x06000B20 RID: 2848 RVA: 0x00051FBE File Offset: 0x000501BE
		public string selectedTitle { get; private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00051FC7 File Offset: 0x000501C7
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x00051FCF File Offset: 0x000501CF
		public Color selectedTitleColor { get; private set; } = Color.white;

		// Token: 0x06000B23 RID: 2851 RVA: 0x00051FD8 File Offset: 0x000501D8
		public void SetTitle(string title, Color color)
		{
			this.selectedTitle = title;
			this.selectedTitleColor = color;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x00051FE8 File Offset: 0x000501E8
		public LoadoutData selectedLoadout
		{
			get
			{
				if (this.loadouts.Count == 0)
				{
					return null;
				}
				SpaceShipData currentSpaceShip = GamePlayer.current.currentSpaceShip;
				string b = (currentSpaceShip != null) ? currentSpaceShip.skillLoadout : null;
				LoadoutData result = null;
				int num = 0;
				foreach (LoadoutData loadoutData in this.loadouts)
				{
					if (loadoutData.name == b)
					{
						result = loadoutData;
						this.selectedLoadoutIndex = num;
						break;
					}
					num++;
				}
				return result;
			}
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x00052080 File Offset: 0x00050280
		public bool TryGiveBonusSkillPoints(int skillPoints, bool force = false)
		{
			bool result;
			if (this.bonusSkillPoints + skillPoints > GameMath.MaxBonusSkillPoints && !force)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@AddBonusSkillPointFailed", new object[]
				{
					GameMath.MaxBonusSkillPoints
				})).WithColor(ColorHelper.red90).WithCustomTime(3f).Show();
				result = false;
			}
			else
			{
				this.bonusSkillPoints += skillPoints;
				SidePanel.instance.NotifyTab(SidePanel.SideTabType.Captain, "Skilltree");
				string text = (skillPoints > 1) ? "skillpoints" : "skillpoint";
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@AddedBonusSkillPoint", new object[]
				{
					skillPoints,
					text
				})).WithColor(ColorHelper.greenish).WithCustomTime(3f).Show();
				result = true;
			}
			if (this.bonusSkillPoints >= GameMath.MaxBonusSkillPoints)
			{
				Default storyteller = GamePlayer.current.GetStoryteller<Default>();
				if (storyteller != null)
				{
					storyteller.RemoveSkillPointsFromShops();
				}
			}
			return result;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x00052177 File Offset: 0x00050377
		public int GetSkillPoints()
		{
			return this.bonusSkillPoints + (base.level - 1);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00052188 File Offset: 0x00050388
		public int GetInvestedSkillPoints()
		{
			int num = 0;
			foreach (SkillTreeData skillTreeData in this.skillTrees)
			{
				num += skillTreeData.GetInvestedSkillPoints();
			}
			return num;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x000521E0 File Offset: 0x000503E0
		public int GetRemainingSkillPoints()
		{
			return this.GetSkillPoints() - this.GetInvestedSkillPoints();
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x000521F0 File Offset: 0x000503F0
		public SkillTreeData GetSkillTreeData(Skilltree tree, bool create = false)
		{
			foreach (SkillTreeData skillTreeData in this.skillTrees)
			{
				if (skillTreeData.skilltree == tree)
				{
					return skillTreeData;
				}
			}
			if (create)
			{
				SkillTreeData skillTreeData2 = new SkillTreeData(tree);
				this.skillTrees.Add(skillTreeData2);
				return skillTreeData2;
			}
			return null;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0005226C File Offset: 0x0005046C
		public float GetAutopilotPenaltyReductionModifier()
		{
			Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Engineering));
			return (float)this.GetSkillTreeData(tree, false).masteryLevel * 0.001f;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0005229C File Offset: 0x0005049C
		public void RefundAllSkills(bool updateLoadout = true)
		{
			foreach (SkillTreeData skillTreeData in this.skillTrees)
			{
				skillTreeData.RefundSkillpoints();
			}
			if (updateLoadout)
			{
				this.UpdateLoadout();
				if (GameplayManager.Instance)
				{
					GameplayManager.Instance.spaceShip.UpdateCommanderSkills();
					AbilityHud.instance.ResetHud(true);
				}
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0005231C File Offset: 0x0005051C
		public bool HasSkill(SkilltreeNode node)
		{
			SkillTreeData skillTreeData = this.GetSkillTreeData(node.parent, false);
			return skillTreeData != null && skillTreeData.HasSkill(node);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00052337 File Offset: 0x00050537
		public void SetPersonalHistory(PersonalHistory personalHistory)
		{
			this.personalHistory = personalHistory;
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00052340 File Offset: 0x00050540
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			result["bonusSkillPoints"] = new double?((double)this.bonusSkillPoints);
			result["selectedTitle"] = this.selectedTitle;
			result["selectedTitleColor"] = "#" + ColorUtility.ToHtmlStringRGB(this.selectedTitleColor);
			result["skillTrees"] = this.skillTrees.ToJsonArray<SkillTreeData>();
			if (this.loadouts.Count > 0)
			{
				result["loadouts"] = this.loadouts.ToJsonArray<LoadoutData>();
			}
			return result;
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000523F5 File Offset: 0x000505F5
		public static CommanderData FromJson(JsonValue data)
		{
			CommanderData commanderData = new CommanderData();
			commanderData.DataFromJson(data);
			return commanderData;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00052408 File Offset: 0x00050608
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			this.bonusSkillPoints = data["bonusSkillPoints"];
			if (data.ContainsKey("selectedTitle"))
			{
				this.selectedTitle = data["selectedTitle"];
			}
			Color selectedTitleColor;
			if (data.ContainsKey("selectedTitleColor") && ColorUtility.TryParseHtmlString(data["selectedTitleColor"], out selectedTitleColor))
			{
				this.selectedTitleColor = selectedTitleColor;
			}
			this.skillTrees.FromJsonArray(data["skillTrees"], new ClassExtensions.ParseJsonValue<SkillTreeData>(SkillTreeData.FromJson));
			if (!data["loadouts"].IsNull)
			{
				foreach (JsonValue data2 in data["loadouts"].AsJsonArray)
				{
					this.loadouts.Add(LoadoutData.FromJson(data2, this.skillTrees));
				}
			}
			CommanderData.ApplyLegacySkilltreeFixes(this);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00052524 File Offset: 0x00050724
		private static CommanderData ApplyLegacySkilltreeFixes(CommanderData data)
		{
			foreach (object obj in Enum.GetValues(typeof(CommanderSpecialization)))
			{
				CommanderSpecialization commanderSpecialization = (CommanderSpecialization)obj;
				if (commanderSpecialization != CommanderSpecialization.Leadership)
				{
					string treeName = SkillTreeData.GetSpecializationTreeName(commanderSpecialization);
					Skilltree tree = Skilltree.Get(treeName);
					if (!data.skillTrees.Any((SkillTreeData st) => st.skilltree.identifier == treeName))
					{
						data.skillTrees.Add(new SkillTreeData(tree, true));
					}
				}
			}
			return data;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000525D0 File Offset: 0x000507D0
		public static CommanderData CreateCommander(CommanderSpecialization starterSpecialization, bool tutorial)
		{
			CommanderData commanderData = new CommanderData();
			commanderData.skillTrees.AddRange(SkillTreeData.GenerateForSpecialization(starterSpecialization));
			foreach (object obj in Enum.GetValues(typeof(CommanderSpecialization)))
			{
				CommanderSpecialization commanderSpecialization = (CommanderSpecialization)obj;
				if (commanderSpecialization != starterSpecialization && commanderSpecialization != CommanderSpecialization.Leadership)
				{
					if (commanderSpecialization == CommanderSpecialization.Engineering)
					{
						commanderData.skillTrees.Add(new SkillTreeData(Skilltree.Get(SkillTreeData.GetSpecializationTreeName(commanderSpecialization)), tutorial));
					}
					else
					{
						commanderData.skillTrees.Add(new SkillTreeData(Skilltree.Get(SkillTreeData.GetSpecializationTreeName(commanderSpecialization)), true));
					}
				}
			}
			return commanderData;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0005268C File Offset: 0x0005088C
		public static CommanderData CreateRandom(SeededRandom random, string faction = null)
		{
			CommanderData commanderData = new CommanderData();
			commanderData.SetRandom(random, null, null, faction);
			return commanderData;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x000526B4 File Offset: 0x000508B4
		public void SetRandom(SeededRandom random, Gender? gender = null, string callsign = null, string faction = null)
		{
			bool flag = (gender != null) ? (gender.Value == Gender.Male) : random.RandomBool(0.5f);
			this.gender = (flag ? Gender.Male : Gender.Female);
			base.SetIcon(CrewIcons.GetRandom(flag, random));
			base.SetName(NameGenerator.GetRandomFirstName(flag, random), callsign ?? NameGenerator.GetFactionCallsign(faction, random), NameGenerator.GetRandomLastName(random));
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0005271C File Offset: 0x0005091C
		public void UnlockSkilltree(string skilltreeName)
		{
			SkillTreeData skillTree = this.GetSkillTree(skilltreeName);
			if (skillTree == null)
			{
				return;
			}
			skillTree.Unlock();
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0005272F File Offset: 0x0005092F
		public bool HasSkilltree(string skilltreeName)
		{
			SkillTreeData skillTree = this.GetSkillTree(skilltreeName);
			return skillTree != null && !skillTree.locked;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00052748 File Offset: 0x00050948
		private SkillTreeData GetSkillTree(string name)
		{
			return this.skillTrees.FirstOrDefault((SkillTreeData st) => st.skilltree.name == name);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0005277C File Offset: 0x0005097C
		public bool LeadershipUnlocked()
		{
			SkillTreeData skillTreeData = this.skillTrees.FirstOrDefault((SkillTreeData t) => t.skilltree.identifier == SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Leadership));
			return skillTreeData != null && !skillTreeData.locked;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000527C4 File Offset: 0x000509C4
		public void AddLoadout(string name)
		{
			LoadoutData loadoutData = new LoadoutData(name);
			foreach (SkillTreeData skillTreeData in this.skillTrees)
			{
				loadoutData.skills.Add(skillTreeData.skilltree.name, skillTreeData.CopyNodes());
			}
			this.loadouts.Add(loadoutData);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00052840 File Offset: 0x00050A40
		public LoadoutData GetLoadoutWithName(string name)
		{
			foreach (LoadoutData loadoutData in this.loadouts)
			{
				if (string.Compare(loadoutData.name, name, StringComparison.CurrentCultureIgnoreCase) == 0)
				{
					return loadoutData;
				}
			}
			return null;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000528A4 File Offset: 0x00050AA4
		public void SetSelectedLoadout(string name)
		{
			if (this.GetLoadoutWithName(name) != null)
			{
				this.InvestSkillsForLoadout();
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x000528B5 File Offset: 0x00050AB5
		public void SetSelectedLoadout(int index)
		{
			SpaceShipData currentSpaceShip = GamePlayer.current.currentSpaceShip;
			LoadoutData loadoutData = this.loadouts[index];
			currentSpaceShip.skillLoadout = ((loadoutData != null) ? loadoutData.name : null);
			this.InvestSkillsForLoadout();
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x000528E4 File Offset: 0x00050AE4
		private void InvestSkillsForLoadout()
		{
			this.RefundAllSkills(false);
			foreach (KeyValuePair<string, List<SkillNodeData>> keyValuePair in this.selectedLoadout.skills)
			{
				SkillTreeData skillTree = this.GetSkillTree(keyValuePair.Key);
				foreach (SkillNodeData skillNodeData in keyValuePair.Value)
				{
					skillTree.InvestSkillPoints(skillNodeData.skill, skillNodeData.currentPoints);
				}
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0005299C File Offset: 0x00050B9C
		public void UpdateLoadout()
		{
			if (this.selectedLoadout == null)
			{
				return;
			}
			foreach (SkillTreeData skillTreeData in this.skillTrees)
			{
				this.selectedLoadout.skills[skillTreeData.skilltree.name] = skillTreeData.CopyNodes();
			}
		}

		// Token: 0x040005F0 RID: 1520
		public int bonusSkillPoints;

		// Token: 0x040005F3 RID: 1523
		public List<SkillTreeData> skillTrees = new List<SkillTreeData>();

		// Token: 0x040005F4 RID: 1524
		public List<LoadoutData> loadouts = new List<LoadoutData>();

		// Token: 0x040005F5 RID: 1525
		public int selectedLoadoutIndex;
	}
}
