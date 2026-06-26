using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.MissionSystem;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F6 RID: 502
	public class SkillTreeBadge : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ITooltipCustomSource
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x060012E1 RID: 4833 RVA: 0x0007B6BC File Offset: 0x000798BC
		// (set) Token: 0x060012E2 RID: 4834 RVA: 0x0007B6C4 File Offset: 0x000798C4
		public Behaviour.Crew.Skilltree skillTree { get; private set; }

		// Token: 0x060012E3 RID: 4835 RVA: 0x0007B6D0 File Offset: 0x000798D0
		public void SetSkillTree(Behaviour.Crew.Skilltree tree)
		{
			this.skillTree = tree;
			this.name.TL(tree.displayName, Array.Empty<object>());
			if (this.skillTree.IsLocked())
			{
				this.name.color = ColorHelper.boringGrey;
			}
			this.icon.sprite = Resources.Load<Sprite>("Sprites/Crew/SkillTrees/" + tree.name);
			this.masteryProgressionImage.fillAmount = (float)(tree.GetMasteryLevel() / 100);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0007B74C File Offset: 0x0007994C
		public void SetSelected(bool sel)
		{
			this.highlight.gameObject.SetActive(sel);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0007B760 File Offset: 0x00079960
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.skillTree.IsLocked())
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				base.GetComponentInParent<CaptainSkillTree>().SelectSkillTree(this.skillTree);
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right && this.skillTree.IsLocked() && this.skilltreeMission != null)
			{
				SidePanel.instance.ShowPoiOnMap(this.skilltreeMission.GetActivePoi(false));
			}
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0007B7CC File Offset: 0x000799CC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@SkillTree" + this.skillTree.name, Array.Empty<object>()), 14, 8f);
			if (!this.skillTree.IsLocked())
			{
				tooltip.AddTextLine(string.Format("{0}: {1}/{2}", Translation.Translate("@SkillTreePointsInvested", Array.Empty<object>()), this.skillTree.GetInvestedSkillPoints(), this.skillTree.maxPoints), 12, 8f).Text.color = ColorHelper.boringGrey;
				List<string> list = new List<string>();
				foreach (CrewMemberData crewMemberData in GamePlayer.current.currentSpaceShip.crewMembers)
				{
					if (crewMemberData != null)
					{
						foreach (SkilltreeNode skilltreeNode in crewMemberData.unlockedNodes)
						{
							SkilltreeNode node = this.skillTree.GetNode(skilltreeNode.identifier);
							if (skilltreeNode == node)
							{
								list.Add(skilltreeNode.displayName);
							}
						}
					}
				}
				if (list.Count > 0)
				{
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
					tooltip.AddTextLine(Translation.Translate("@SkillTreeCrewBonusSkills", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
					foreach (string text in list)
					{
						tooltip.AddTextLine("+1 " + Translation.Translate(text, Array.Empty<object>()), 12, 8f);
					}
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
				}
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@Locked", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
			if (GamePlayer.current.IsPrologueActive())
			{
				return;
			}
			this.skilltreeMission = null;
			bool flag = false;
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (this.skilltreeMission != null)
				{
					break;
				}
				foreach (MissionReward missionReward in mission.rewards)
				{
					Source.MissionSystem.Rewards.Skilltree skilltree = missionReward as Source.MissionSystem.Rewards.Skilltree;
					if (skilltree != null && skilltree.treeName == this.skillTree.identifier)
					{
						this.skilltreeMission = mission;
						flag = true;
						break;
					}
				}
			}
			if (flag && this.skilltreeMission != null)
			{
				tooltip.AddTextLine(Translation.Translate("@MissionGivesSkilltreeReward", new object[]
				{
					this.skilltreeMission.name
				}), 12, 8f).Text.color = ColorHelper.boringGrey;
				return;
			}
			tooltip.AddTextLine(Translation.Translate("@MissionSkilltreeGiver", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
		}

		// Token: 0x04000A9C RID: 2716
		[SerializeField]
		private Image icon;

		// Token: 0x04000A9D RID: 2717
		[SerializeField]
		private new TextMeshProUGUI name;

		// Token: 0x04000A9E RID: 2718
		[SerializeField]
		private RectTransform highlight;

		// Token: 0x04000A9F RID: 2719
		[SerializeField]
		private Image masteryProgressionImage;

		// Token: 0x04000AA1 RID: 2721
		private Mission skilltreeMission;
	}
}
