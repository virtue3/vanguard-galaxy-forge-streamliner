using System;
using Behaviour.Crew;
using Behaviour.Equipment.Aspect;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F5 RID: 501
	public class MasteryBadge : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x0007B319 File Offset: 0x00079519
		// (set) Token: 0x060012DD RID: 4829 RVA: 0x0007B321 File Offset: 0x00079521
		public Skilltree skillTree { get; private set; }

		// Token: 0x060012DE RID: 4830 RVA: 0x0007B32C File Offset: 0x0007952C
		public void SetSkillTree(Skilltree tree)
		{
			this.skillTree = tree;
			int masteryLevel = tree.GetMasteryLevel();
			this.masteryXpText.text = string.Format("{0} {1}", Translation.Translate("@Mastery", Array.Empty<object>()), masteryLevel);
			this.masteryProgressionImage.fillAmount = (float)(masteryLevel / 100);
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0007B384 File Offset: 0x00079584
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(Translation.Translate("@SkillTree" + this.skillTree.identifier, Array.Empty<object>()) + " " + Translation.Translate("@Mastery", Array.Empty<object>()), 14, 8f);
			string text = GameMath.FormatNumber(this.skillTree.GetMasteryXp(), -1) + " / " + GameMath.FormatNumber(GameMath.GetMaxExperienceForLevel(this.skillTree.GetMasteryLevel()), -1);
			tooltip.AddTextLine(Translation.Translate("@Experience", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.detailsColor), 12, 8f).Text.color = ColorHelper.offWhite;
			string str = Translation.Translate("@Mastery", Array.Empty<object>());
			string str2 = " ";
			int i = this.skillTree.GetMasteryLevel();
			tooltip.AddTextLine(str + str2 + i.ToString().HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@Bonus", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.green50;
			GameObject[] passiveMasteryBonusses = this.skillTree.passiveMasteryBonusses;
			for (i = 0; i < passiveMasteryBonusses.Length; i++)
			{
				BoostStat component = passiveMasteryBonusses[i].GetComponent<BoostStat>();
				component.SetStackSize(this.skillTree.GetMasteryLevel());
				foreach (EquipStatLine equipStatLine in component.GetStats())
				{
					tooltip.AddTextLine(equipStatLine.ToReadableString(false), 12, 8f).Text.color = ColorHelper.greenish;
				}
			}
			if (this.skillTree == Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Industrial)))
			{
				if (this.skillTree.GetMasteryLevel() <= 0)
				{
					return;
				}
				tooltip.AddTextLine("+" + (0.5f * (float)this.skillTree.GetMasteryLevel()).ToString() + "% Refine/Crafting Speed", 12, 8f).Text.color = ColorHelper.greenish;
				return;
			}
			else
			{
				if (!(this.skillTree == Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Engineering))))
				{
					if (this.skillTree == Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Economy)))
					{
						if (this.skillTree.GetMasteryLevel() <= 0)
						{
							return;
						}
						tooltip.AddTextLine("+" + (1f * (float)this.skillTree.GetMasteryLevel()).ToString() + "% Terminal Supply Increase", 12, 8f).Text.color = ColorHelper.greenish;
					}
					return;
				}
				if (this.skillTree.GetMasteryLevel() <= 0)
				{
					return;
				}
				tooltip.AddTextLine("+" + (0.1f * (float)this.skillTree.GetMasteryLevel()).ToString() + "% Penalty Reduction", 12, 8f).Text.color = ColorHelper.greenish;
				return;
			}
		}

		// Token: 0x04000A99 RID: 2713
		[SerializeField]
		private TextMeshProUGUI masteryXpText;

		// Token: 0x04000A9A RID: 2714
		[SerializeField]
		private Image masteryProgressionImage;
	}
}
