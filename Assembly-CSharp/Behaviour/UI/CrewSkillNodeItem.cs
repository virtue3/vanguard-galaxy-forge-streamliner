using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D2 RID: 466
	public class CrewSkillNodeItem : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, ITooltipCustomSource
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x000748E1 File Offset: 0x00072AE1
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x000748E9 File Offset: 0x00072AE9
		public SkilltreeNode node { get; protected set; }

		// Token: 0x06001196 RID: 4502 RVA: 0x000748F2 File Offset: 0x00072AF2
		public void SetSkillNode(SkilltreeNode skillNode, Action redrawCallback)
		{
			this.callback = redrawCallback;
			this.SetSkillNode(skillNode);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00074904 File Offset: 0x00072B04
		public void SetSkillNode(SkilltreeNode skillNode)
		{
			this.node = skillNode;
			float num = (float)this.node.currentPoints;
			float num2 = (float)this.node.CurrentCommanderPoints();
			float num3 = (float)this.node.maxSkillPoints;
			this.skillIcon.sprite = this.node.icon;
			this.skillBorderIcon.gameObject.SetActive(true);
			this.rank.text = string.Format("{0}/{1}", num, num3);
			if (num == 0f)
			{
				this.skillIcon.color = ColorHelper.boringGrey;
				if (this.node.tier > 3 && !GamePlayer.current.skilltreeTier2Unlocked)
				{
					this.skillBorderIcon.color = ColorHelper.skilltreeBorderLockedRed;
					return;
				}
				this.skillBorderIcon.color = (this.node.CanInvestSkillPoints() ? ColorHelper.skilltreeBorderRank : ColorHelper.boringGrey);
				return;
			}
			else
			{
				if (num2 >= num3)
				{
					this.rankImage.color = ColorHelper.skilltreeMaxRank;
					this.skillBorderIcon.color = ColorHelper.skilltreeMaxRank;
					return;
				}
				this.skillBorderIcon.color = (this.node.CanInvestSkillPoints() ? ColorHelper.skilltreeBorderMaxRank : ColorHelper.skilltreeBorderMaxRank);
				return;
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00074A38 File Offset: 0x00072C38
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && this.node.CanInvestSkillPoints())
			{
				int count = 1;
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					count = Mathf.Clamp(this.node.maxSkillPoints - this.node.investedPoints, 1, GamePlayer.current.commander.GetRemainingSkillPoints());
				}
				this.node.InvestSkillPoints(count);
				this.callback();
				this.SetSkillNode(this.node);
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Right && this.node.CanRemoveSkillPoint(true))
			{
				int count2 = 1;
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					count2 = this.node.investedPoints;
				}
				this.node.RemoveSkillPoints(count2);
				this.callback();
				this.SetSkillNode(this.node);
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00074B28 File Offset: 0x00072D28
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.node.displayName, 16, 8f);
			int currentPoints = this.node.currentPoints;
			int investedPoints = this.node.investedPoints;
			int maxSkillPoints = this.node.maxSkillPoints;
			List<string> list = new List<string>();
			foreach (CrewMemberData crewMemberData in GamePlayer.current.currentSpaceShip.crewMembers)
			{
				if (crewMemberData != null)
				{
					using (IEnumerator<SkilltreeNode> enumerator = crewMemberData.unlockedNodes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == this.node)
							{
								list.Add(Translation.Translate("@SkillAddedByCrew", new object[]
								{
									crewMemberData.GetFullName()
								}));
							}
						}
					}
				}
			}
			Color orange = ColorHelper.orange75;
			string arg = currentPoints.ToString();
			string str = string.Format("{0}/{1}", arg, maxSkillPoints);
			if (list.Count > 0)
			{
				str = string.Format("{0}/{1} ({2} {3})", new object[]
				{
					currentPoints,
					maxSkillPoints,
					list.Count,
					Translation.Translate("@FromCrew", Array.Empty<object>())
				});
			}
			UITooltipText uitooltipText = tooltip.AddTextLine(Translation.Translate("@Rank", Array.Empty<object>()) + " " + str, 12, 8f);
			if (this.node.IsActivatedAbility())
			{
				tooltip.AddTextLine(Translation.Translate("@SkillActivatedAbility", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.purpleBlueish;
			}
			else if (this.node.IsTriggeredAbility())
			{
				tooltip.AddTextLine(Translation.Translate("@SkillTriggeredAbility", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.lightCyan;
			}
			else
			{
				tooltip.AddTextLine(Translation.Translate("@SkillPassive", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			}
			if (this.node.exclusiveNodes.Count > 0)
			{
				tooltip.AddTextLine(Translation.Translate("@SkillExclusive", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.modifierColor;
			}
			foreach (string text in list)
			{
				tooltip.AddTextLine(text, 12, 8f).Text.color = orange;
			}
			uitooltipText.Text.rectTransform.offsetMax = new Vector2(0f, 0f);
			if (this.node.tier > 3 && !GamePlayer.current.skilltreeTier2Unlocked)
			{
				tooltip.AddTextLine(Translation.Translate("@SkillTier2Locked", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
			}
			else if (this.node.conquestLocked && GamePlayer.current.GetStoryteller<Conquest>() == null)
			{
				tooltip.AddTextLine(Translation.Translate("@SkillLocked", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
			}
			else
			{
				int requiredPointsInTree = this.node.requiredPointsInTree;
				if (requiredPointsInTree > this.node.parent.GetInvestedSkillPoints())
				{
					tooltip.AddTextLine(Translation.Translate("@SkillReqRank", new object[]
					{
						requiredPointsInTree,
						"@SkillTree" + this.node.parent.identifier
					}), 12, 8f).Text.color = ColorHelper.reddish;
				}
				int requiredPointsTotal = this.node.requiredPointsTotal;
				if (requiredPointsTotal > GamePlayer.current.commander.GetInvestedSkillPoints())
				{
					tooltip.AddTextLine(Translation.Translate("@SkillReqTotal", new object[]
					{
						requiredPointsTotal
					}), 12, 8f).Text.color = ColorHelper.reddish;
				}
				using (List<SkilltreeNode>.Enumerator enumerator3 = this.node.exclusiveNodes.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current.investedPoints > 0)
						{
							tooltip.AddTextLine(Translation.Translate("@ExclusiveNodeAlreadyActive", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.reddish;
						}
					}
				}
				SkilltreeNode requiredNode = this.node.requiredNode;
				if (requiredNode && requiredNode.investedPoints < requiredNode.maxSkillPoints)
				{
					tooltip.AddTextLine(Translation.Translate("@SkillReqPrevious", new object[]
					{
						"@" + requiredNode.identifier + "Name"
					}), 12, 8f).Text.color = ColorHelper.reddish;
				}
			}
			CrewSkillNodeItem.AddSkillDescriptionToTooltip(tooltip, this.node, investedPoints, currentPoints, list.Count, true, true);
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00075050 File Offset: 0x00073250
		public static void AddSkillDescriptionToTooltip(UITooltip tooltip, SkilltreeNode node, int investedPoints, int currentPoints, int crewRanks = 0, bool showAssign = true, bool showRemove = true)
		{
			ValueTuple<string, string> valueTuple = node.CreateSkillDescription(currentPoints, crewRanks);
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			int maxSkillPoints = node.maxSkillPoints;
			tooltip.AddTextLine(item, 12, 8f).Text.color = ColorHelper.detailsColor;
			if (currentPoints > 0 && maxSkillPoints != 1 && investedPoints < maxSkillPoints)
			{
				tooltip.AddTextLine(Translation.Translate("@NextRank", Array.Empty<object>()) + ":", 12, 8f);
				tooltip.AddTextLine(item2, 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (showAssign && investedPoints < maxSkillPoints && GamePlayer.current.commander.GetRemainingSkillPoints() > 0 && node.CanInvestSkillPoints())
			{
				tooltip.AddTextLine(Translation.Translate("@SkillClickToGain", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
				if (maxSkillPoints > 1)
				{
					tooltip.AddTextLine(Translation.Translate("@SkillShiftClickToGain", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
				}
			}
			if (showRemove && node.CanRemoveSkillPoint(false))
			{
				tooltip.AddTextLine("@SkillClickToRemove", 12, 8f).Text.color = ColorHelper.orange75;
				if (maxSkillPoints > 1)
				{
					tooltip.AddTextLine(Translation.Translate("@SkillShiftClickToRemove", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.orange75;
					return;
				}
			}
			else if (investedPoints == maxSkillPoints && showRemove && !node.CanRemoveSkillPoint(false))
			{
				tooltip.AddTextLine("@SkillClickToRemoveSS", 12, 8f).Text.color = ColorHelper.orange75;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x000751F4 File Offset: 0x000733F4
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.node.CurrentCommanderPoints() >= this.node.maxSkillPoints)
			{
				return;
			}
			if (!this.node.CanInvestSkillPoints())
			{
				return;
			}
			this.savedBorderColor = this.skillBorderIcon.color;
			this.rankImage.color = ColorHelper.boringGrey;
			this.skillBorderIcon.color = ColorHelper.boringGrey;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0007525C File Offset: 0x0007345C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.node.CurrentCommanderPoints() >= this.node.maxSkillPoints)
			{
				return;
			}
			if (!this.node.CanInvestSkillPoints())
			{
				return;
			}
			this.rankImage.color = this.savedBorderColor;
			this.skillBorderIcon.color = this.savedBorderColor;
		}

		// Token: 0x040009A6 RID: 2470
		[SerializeField]
		private TextMeshProUGUI rank;

		// Token: 0x040009A7 RID: 2471
		[SerializeField]
		private Image rankImage;

		// Token: 0x040009A8 RID: 2472
		[SerializeField]
		private Image skillIcon;

		// Token: 0x040009A9 RID: 2473
		[SerializeField]
		private Image skillBorderIcon;

		// Token: 0x040009AA RID: 2474
		[SerializeField]
		private Color activeColor;

		// Token: 0x040009AB RID: 2475
		[SerializeField]
		private Color selectableColor;

		// Token: 0x040009AD RID: 2477
		private Action callback;

		// Token: 0x040009AE RID: 2478
		private Color savedBorderColor;
	}
}
