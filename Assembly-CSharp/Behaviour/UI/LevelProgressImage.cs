using System;
using Behaviour.UI.Tooltip;
using Source.Crew;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D3 RID: 467
	public class LevelProgressImage : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x0600119E RID: 4510 RVA: 0x000752BA File Offset: 0x000734BA
		private void Awake()
		{
			this.originalMaterial = this.image.material;
			this.colorIdentifier = Shader.PropertyToID("Color");
			this.tooltip = base.GetComponent<TooltipSource>();
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x000752EC File Offset: 0x000734EC
		public void SetCrewMember(AbstractCrewData crewMemberData)
		{
			this.crewMemberData = crewMemberData;
			this.fillAmountGoal = (this.fillAmount = this.GetNewFillAmount());
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00075318 File Offset: 0x00073518
		private void Update()
		{
			if (this.crewMemberData != null)
			{
				float newFillAmount = this.GetNewFillAmount();
				if (!this.fillAmountGoal.ApproximatelyEqual(newFillAmount))
				{
					this.fillAmountGoal = newFillAmount;
				}
				if (!this.fillAmountGoal.ApproximatelyEqual(this.fillAmount))
				{
					float num = this.fillAmountGoal - this.image.fillAmount;
					this.fillAmount += (((double)num < 0.001) ? num : (num * Time.deltaTime * 3f));
				}
				else
				{
					this.image.material = this.originalMaterial;
				}
				this.image.fillAmount = this.fillAmount;
				UITooltip.Refresh(this.tooltip);
			}
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x000753CB File Offset: 0x000735CB
		private float GetNewFillAmount()
		{
			return (float)Math.Round((double)(this.crewMemberData.experience / this.crewMemberData.maxExperience), 2);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x000753EC File Offset: 0x000735EC
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (this.crewMemberData.IsMaxLevel())
			{
				tooltip.AddTextLine(Translation.Translate("@UIMaxLevelReached", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.greenish;
			}
			else
			{
				string text = GameMath.FormatNumber(this.crewMemberData.experience, -1) + " / " + GameMath.FormatNumber(this.crewMemberData.maxExperience, -1);
				tooltip.AddTextLine(Translation.Translate("@Experience", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.detailsColor), 12, 8f).Text.color = ColorHelper.offWhite;
			}
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(Translation.Translate("@UIOnlyLevel", Array.Empty<object>()) + ": " + this.crewMemberData.level.ToString().HighlightWithColor(ColorHelper.lightCyan), 12, 8f).Text.color = ColorHelper.offWhite;
		}

		// Token: 0x040009AF RID: 2479
		[SerializeField]
		private Material shaderMaterial;

		// Token: 0x040009B0 RID: 2480
		[SerializeField]
		private Image image;

		// Token: 0x040009B1 RID: 2481
		private AbstractCrewData crewMemberData;

		// Token: 0x040009B2 RID: 2482
		private Material originalMaterial;

		// Token: 0x040009B3 RID: 2483
		private TooltipSource tooltip;

		// Token: 0x040009B4 RID: 2484
		private int colorIdentifier;

		// Token: 0x040009B5 RID: 2485
		private float fillAmount;

		// Token: 0x040009B6 RID: 2486
		private float fillAmountGoal;
	}
}
