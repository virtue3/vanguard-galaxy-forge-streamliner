using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using Source.Crew;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002AC RID: 684
	public class CaptainSkillTree : SideTabContent
	{
		// Token: 0x06001962 RID: 6498 RVA: 0x0009E050 File Offset: 0x0009C250
		private void Start()
		{
			this.SetCommander();
			this.pointsText.text = Translation.Translate("@SPPoints", Array.Empty<object>()) + ":";
			if (SpaceStationInterior.instance == null)
			{
				this.resetSkilltrees.gameObject.SetActive(false);
				this.loadoutContainer.SetActive(false);
				return;
			}
			this.resetSkilltrees.gameObject.SetActive(true);
			this.loadoutContainer.SetActive(true);
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0009E0D0 File Offset: 0x0009C2D0
		public void SetCommander()
		{
			this.crewContainer.DestroyChildren();
			this.badgesContainer.DestroyChildren();
			this.elCapitan = GamePlayer.current.commander;
			this.progressImage.SetCrewMember(this.elCapitan);
			this.SetSkillTrees();
			this.SetLoadouts();
			this.UpdatePoints();
			if (this.elCapitan.selectedLoadout != null && SpaceStationInterior.instance)
			{
				this.loadoutDropdown.SetValueWithoutNotify(this.elCapitan.selectedLoadoutIndex);
				this.SelectLoadout();
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0009E15C File Offset: 0x0009C35C
		private void SetSkillTrees()
		{
			foreach (SkillTreeData skillTreeData in this.elCapitan.skillTrees)
			{
				SkillTreeBadge skillTreeBadge = UnityEngine.Object.Instantiate<SkillTreeBadge>(this.skillTreeBadgePrefab, this.badgesContainer);
				skillTreeBadge.SetSkillTree(skillTreeData.skilltree);
				this.skillTreeBadges.Add(skillTreeBadge);
				if (this.selectedSkillTree == null)
				{
					this.SelectSkillTree(skillTreeData.skilltree);
					skillTreeBadge.SetSelected(true);
				}
			}
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0009E1F8 File Offset: 0x0009C3F8
		public void SelectSkillTree(Skilltree skillTree)
		{
			this.crewContainer.DestroyChildren();
			SkillTreeRowItem skillTreeRowItem = UnityEngine.Object.Instantiate<SkillTreeRowItem>(this.skillTreeRowItem, this.crewContainer);
			skillTreeRowItem.SetSkillTree(skillTree, new Action(this.UpdateSkillSelection));
			this.masteryBadge.SetSkillTree(skillTree);
			this.selectedSkillTree = skillTree;
			foreach (SkillTreeBadge skillTreeBadge in this.skillTreeBadges)
			{
				skillTreeBadge.SetSelected(skillTreeBadge.skillTree == this.selectedSkillTree);
			}
			this.crewContainer.sizeDelta = new Vector2(skillTreeRowItem.width, this.crewContainer.sizeDelta.y);
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0009E2C4 File Offset: 0x0009C4C4
		private void UpdateSkillSelection()
		{
			this.UpdateLoadout();
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0009E2CC File Offset: 0x0009C4CC
		private void Update()
		{
			if (this.elCapitan != null)
			{
				this.UpdatePoints();
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0009E2DC File Offset: 0x0009C4DC
		private void UpdatePoints()
		{
			Color color = this.skillpointsAvailable.color;
			int remainingSkillPoints = this.elCapitan.GetRemainingSkillPoints();
			if (remainingSkillPoints > 0)
			{
				color = ColorHelper.greenish;
			}
			this.skillpointsAvailable.text = remainingSkillPoints.ToString().HighlightWithColor(color);
			this.levelText.text = Translation.Translate("@SPLevel", Array.Empty<object>()) + " " + this.elCapitan.level.ToString();
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0009E35A File Offset: 0x0009C55A
		private void UpdateLoadout()
		{
			if (this.elCapitan.loadouts.Count == 0)
			{
				return;
			}
			this.elCapitan.UpdateLoadout();
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0009E37C File Offset: 0x0009C57C
		private void SetLoadouts()
		{
			List<string> list = new List<string>();
			int num = 0;
			foreach (LoadoutData loadoutData in this.elCapitan.loadouts)
			{
				list.Add(loadoutData.name);
				num++;
			}
			this.loadoutDropdown.AddOptions(list);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0009E3F4 File Offset: 0x0009C5F4
		public void SelectLoadout()
		{
			foreach (SkillTreeData skillTreeData in this.elCapitan.skillTrees)
			{
				skillTreeData.RefundSkillpoints();
			}
			this.elCapitan.SetSelectedLoadout(this.loadoutDropdown.value);
			this.SelectSkillTree(this.selectedSkillTree);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0009E46C File Offset: 0x0009C66C
		public void AddLoadout()
		{
			AlertPopup.ShowInput("@SkillLoadoutEnterName", new Action<string>(this.CreateLoadout), null, null, true, null, null);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0009E48C File Offset: 0x0009C68C
		public void CreateLoadout(string name)
		{
			if (this.elCapitan.GetLoadoutWithName(name) != null)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SkillLoadoutNameExists", new object[]
				{
					name
				})).WithColor(ColorHelper.red90).Show();
				AlertPopup.ShowInput("@SkillLoadoutEnterName", new Action<string>(this.CreateLoadout), null, name, true, null, null);
				return;
			}
			this.elCapitan.AddLoadout(name);
			this.loadoutDropdown.options.Add(new TMP_Dropdown.OptionData(name));
			this.loadoutDropdown.value = this.loadoutDropdown.options.Count - 1;
			this.loadoutDropdown.RefreshShownValue();
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0009E53C File Offset: 0x0009C73C
		public void EditLoadout()
		{
			if (this.loadoutDropdown.options.Count == 0)
			{
				return;
			}
			AlertPopup.ShowInput("@SkillLoadoutEnterName", new Action<string>(this.EditLoadoutName), null, this.elCapitan.loadouts[this.loadoutDropdown.value].name, true, null, null);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0009E598 File Offset: 0x0009C798
		public void EditLoadoutName(string name)
		{
			string name2 = this.elCapitan.selectedLoadout.name;
			this.elCapitan.loadouts[this.loadoutDropdown.value].name = name;
			this.loadoutDropdown.options[this.loadoutDropdown.value].text = name;
			this.loadoutDropdown.RefreshShownValue();
			this.EditLoadoutNamesOnShips(name2, name);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0009E60C File Offset: 0x0009C80C
		public void DeleteLoadout()
		{
			if (this.loadoutDropdown.options.Count == 0)
			{
				return;
			}
			AlertPopup.ShowQuery(Translation.Translate("@SkillLoadoutDeleteSure", new object[]
			{
				this.elCapitan.selectedLoadout.name
			}), null, null, new Action(this.DefinitelyDeleteLoadout), null, null, null);
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0009E668 File Offset: 0x0009C868
		public void DefinitelyDeleteLoadout()
		{
			string name = this.elCapitan.selectedLoadout.name;
			this.elCapitan.loadouts.Remove(this.elCapitan.selectedLoadout);
			this.loadoutDropdown.options.RemoveAt(this.elCapitan.selectedLoadoutIndex);
			if (this.loadoutDropdown.options.Count > 0)
			{
				this.loadoutDropdown.value = this.loadoutDropdown.options.Count - 1;
			}
			this.loadoutDropdown.RefreshShownValue();
			this.EditLoadoutNamesOnShips(name, null);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0009E700 File Offset: 0x0009C900
		private void EditLoadoutNamesOnShips(string oldName, string newName = null)
		{
			foreach (SpaceShipData spaceShipData in GamePlayer.current.spaceShips)
			{
				if (spaceShipData.skillLoadout == oldName)
				{
					spaceShipData.skillLoadout = newName;
				}
			}
		}

		// Token: 0x04000FD1 RID: 4049
		[SerializeField]
		private RectTransform crewContainer;

		// Token: 0x04000FD2 RID: 4050
		[SerializeField]
		private SkillTreeRowItem skillTreeRowItem;

		// Token: 0x04000FD3 RID: 4051
		[SerializeField]
		private LevelProgressImage progressImage;

		// Token: 0x04000FD4 RID: 4052
		[SerializeField]
		private TextMeshProUGUI pointsText;

		// Token: 0x04000FD5 RID: 4053
		[SerializeField]
		private SkilltreePoints points;

		// Token: 0x04000FD6 RID: 4054
		[SerializeField]
		private TextMeshProUGUI skillpointsAvailable;

		// Token: 0x04000FD7 RID: 4055
		[SerializeField]
		private TextMeshProUGUI levelText;

		// Token: 0x04000FD8 RID: 4056
		[SerializeField]
		private Transform badgesContainer;

		// Token: 0x04000FD9 RID: 4057
		[SerializeField]
		private SkillTreeBadge skillTreeBadgePrefab;

		// Token: 0x04000FDA RID: 4058
		[SerializeField]
		private MasteryBadge masteryBadge;

		// Token: 0x04000FDB RID: 4059
		[SerializeField]
		private ResetSkilltrees resetSkilltrees;

		// Token: 0x04000FDC RID: 4060
		[SerializeField]
		private TMP_Dropdown loadoutDropdown;

		// Token: 0x04000FDD RID: 4061
		[SerializeField]
		private Button addButton;

		// Token: 0x04000FDE RID: 4062
		[SerializeField]
		private Button editButton;

		// Token: 0x04000FDF RID: 4063
		[SerializeField]
		private Button removeButton;

		// Token: 0x04000FE0 RID: 4064
		[SerializeField]
		private GameObject loadoutContainer;

		// Token: 0x04000FE1 RID: 4065
		private List<SkillTreeBadge> skillTreeBadges = new List<SkillTreeBadge>();

		// Token: 0x04000FE2 RID: 4066
		private CommanderData elCapitan;

		// Token: 0x04000FE3 RID: 4067
		private Skilltree selectedSkillTree;
	}
}
