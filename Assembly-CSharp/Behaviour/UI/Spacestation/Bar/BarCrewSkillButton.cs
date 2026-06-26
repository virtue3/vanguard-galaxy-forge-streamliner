using System;
using Behaviour.Crew;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Bar
{
	// Token: 0x02000238 RID: 568
	public class BarCrewSkillButton : MonoBehaviour, ITooltipTitleSource, ITooltipCustomSource
	{
		// Token: 0x0600153E RID: 5438 RVA: 0x00088FF4 File Offset: 0x000871F4
		public void SetSkill(SkilltreeNode skill, bool unlocked)
		{
			this.contained = skill;
			this.unlocked = unlocked;
			this.icon.sprite = ((skill != null) ? skill.icon : null);
			this.icon.material = (unlocked ? Materials.Default : Materials.Grayscale75);
			this.icon.color = (unlocked ? Color.white : Color.gray);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0008905A File Offset: 0x0008725A
		public string GetTooltipTitle()
		{
			return this.contained.displayName;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x00089068 File Offset: 0x00087268
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			CrewSkillNodeItem.AddSkillDescriptionToTooltip(tooltip, this.contained, 0, 0, 0, false, false);
			if (this.contained.parent != null)
			{
				tooltip.AddTextLine(Translation.Translate(this.contained.parent.displayName, Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
			}
			if (!this.unlocked)
			{
				tooltip.AddTextLine(Translation.Translate("@UIBarCrewmemberLevelReq", new object[]
				{
					this.contained.crewLevelRequired
				}), 12, 8f).Text.color = Color.red;
			}
		}

		// Token: 0x04000C8F RID: 3215
		[SerializeField]
		private Image icon;

		// Token: 0x04000C90 RID: 3216
		private SkilltreeNode contained;

		// Token: 0x04000C91 RID: 3217
		private bool unlocked;
	}
}
