using System;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Travel
{
	// Token: 0x02000200 RID: 512
	public class TravelSpeedInfo : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x06001337 RID: 4919 RVA: 0x0007E048 File Offset: 0x0007C248
		private void Awake()
		{
			this.spdText = Translation.Translate("@TravelSpeed", Array.Empty<object>());
			this.speedFullText = Translation.Translate("@TravelSpeedFull", Array.Empty<object>());
			this.accelarationText = Translation.Translate("@TravelAcceleration", Array.Empty<object>());
			this.maxSpeedText = Translation.Translate("@TravelBaseMaxSpeed", Array.Empty<object>());
			this.tooltip = base.GetComponent<TooltipSource>();
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x0007E0B5 File Offset: 0x0007C2B5
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.1f;
				UITooltip.Refresh(this.tooltip);
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0007E0EC File Offset: 0x0007C2EC
		public void Init(float unitsPerSecond)
		{
			this.speed = GameMath.FormatNumber(unitsPerSecond * 100f, 1);
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			this.accelaration = spaceShip.baseWarpAcceleration;
			this.maxSpeed = spaceShip.baseMaxWarpSpeed;
			this.speedUnit = TravelInfo.instance.travelUnitsTL + "/" + TravelInfo.instance.travelSecondTL;
			this.UpdateTravelInfo(this.speed);
		}

		// Token: 0x0600133A RID: 4922 RVA: 0x0007E160 File Offset: 0x0007C360
		public void UpdateTravelInfo(string speed)
		{
			this.speed = speed;
			this.speedText.text = string.Concat(new string[]
			{
				this.spdText,
				": ",
				speed.HighlightWithColor(ColorHelper.detailsColor),
				" ",
				this.speedUnit
			});
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0007E1BA File Offset: 0x0007C3BA
		public void ShowTransition()
		{
			this.speedText.text = this.spdText + ": " + "--".HighlightWithColor(ColorHelper.fadedGrey);
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0007E1E8 File Offset: 0x0007C3E8
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(string.Concat(new string[]
			{
				this.speedFullText,
				": ",
				this.speed.HighlightWithColor(ColorHelper.detailsColor),
				" ",
				this.speedUnit
			}), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddSeparator(null);
			tooltip.AddTextLine(string.Format("{0}: {1} {2}", this.maxSpeedText, this.maxSpeed * 100f, this.speedUnit), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(string.Format("{0}: {1} {2}", this.accelarationText, this.accelaration * 100f, this.speedUnit), 12, 8f).Text.color = ColorHelper.offWhite;
		}

		// Token: 0x04000AE6 RID: 2790
		[SerializeField]
		private TextMeshProUGUI speedText;

		// Token: 0x04000AE7 RID: 2791
		private TooltipSource tooltip;

		// Token: 0x04000AE8 RID: 2792
		private string speed;

		// Token: 0x04000AE9 RID: 2793
		private string spdText;

		// Token: 0x04000AEA RID: 2794
		private string speedFullText;

		// Token: 0x04000AEB RID: 2795
		private string accelarationText;

		// Token: 0x04000AEC RID: 2796
		private string maxSpeedText;

		// Token: 0x04000AED RID: 2797
		private float accelaration;

		// Token: 0x04000AEE RID: 2798
		private float maxSpeed;

		// Token: 0x04000AEF RID: 2799
		private string speedUnit;

		// Token: 0x04000AF0 RID: 2800
		private float updateTimer;
	}
}
