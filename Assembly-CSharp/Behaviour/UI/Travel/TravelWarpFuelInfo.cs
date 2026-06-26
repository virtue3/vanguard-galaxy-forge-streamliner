using System;
using Behaviour.Managers;
using Behaviour.UI.Tooltip;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.Travel
{
	// Token: 0x02000201 RID: 513
	public class TravelWarpFuelInfo : MonoBehaviour, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x0600133E RID: 4926 RVA: 0x0007E2F0 File Offset: 0x0007C4F0
		private void Awake()
		{
			this.warpFuel = Translation.Translate("@WarpFuel", Array.Empty<object>());
			this.unavailableText = Translation.Translate("@WarpFuelUnavailable", Array.Empty<object>());
			this.enabledText = Translation.Translate("@WarpFuelEnabled", Array.Empty<object>());
			this.disabledText = Translation.Translate("@WarpFuelDisabled", Array.Empty<object>());
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0007E351 File Offset: 0x0007C551
		public void Init()
		{
			this.SetWarpFuelStatus();
		}

		// Token: 0x06001340 RID: 4928 RVA: 0x0007E359 File Offset: 0x0007C559
		public void UpdateTravelInfo()
		{
			this.SetWarpFuelStatus();
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0007E364 File Offset: 0x0007C564
		public void SetWarpFuelStatus()
		{
			bool useWarpFuel = GamePlayer.current.useWarpFuel;
			bool flag = GamePlayer.current.currentSpaceShip.cargo.IsWarpFuelAvailable();
			this.warpFuelActive = useWarpFuel;
			this.warpFuelAvailable = flag;
			string text = "";
			if (!flag)
			{
				this.warpFuelStatus = this.unavailableText;
				this.warpFuelStatusColor = Color.gray;
			}
			else if (useWarpFuel)
			{
				this.warpFuelStatus = this.enabledText;
				this.warpFuelStatusColor = ColorHelper.greenish;
				if (GamePlayer.current.autoPlay)
				{
					text = "(Autopilot Efficiency: " + GameMath.FormatPercentage(TravelManager.GetWarpFuelAutopilotMultiplier(), FormatPercentageMode.Default, 1) + ")";
				}
			}
			else
			{
				this.warpFuelStatus = this.disabledText;
				this.warpFuelStatusColor = ColorHelper.reddish;
			}
			this.warpfuelText.text = string.Concat(new string[]
			{
				this.warpFuel,
				": ",
				this.warpFuelStatus.HighlightWithColor(this.warpFuelStatusColor),
				" ",
				text.HighlightWithColor(ColorHelper.boringGrey)
			});
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0007E470 File Offset: 0x0007C670
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine(this.warpFuel + ": " + this.warpFuelStatus.HighlightWithColor(this.warpFuelStatusColor), 12, 8f).Text.color = ColorHelper.offWhite;
			if (!this.warpFuelAvailable)
			{
				return;
			}
			if (this.warpFuelActive)
			{
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@WarpFuelEfficiency", Array.Empty<object>()) + ": " + GameMath.FormatPercentage(TravelManager.GetWarpFuelAutopilotMultiplier(), FormatPercentageMode.Default, 1), 12, 8f).Text.color = ColorHelper.offWhite;
				tooltip.AddTextLine("@UIWarpFuelDisable", 12, 8f);
				return;
			}
			tooltip.AddTextLine("@UIWarpFuelEnable", 12, 8f);
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0007E543 File Offset: 0x0007C743
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				GamePlayer.current.useWarpFuel = !GamePlayer.current.useWarpFuel;
				UITooltip.Refresh();
			}
		}

		// Token: 0x04000AF1 RID: 2801
		[SerializeField]
		private TextMeshProUGUI warpfuelText;

		// Token: 0x04000AF2 RID: 2802
		private string warpFuel;

		// Token: 0x04000AF3 RID: 2803
		private string unavailableText;

		// Token: 0x04000AF4 RID: 2804
		private string enabledText;

		// Token: 0x04000AF5 RID: 2805
		private string disabledText;

		// Token: 0x04000AF6 RID: 2806
		private string warpFuelStatus;

		// Token: 0x04000AF7 RID: 2807
		private Color warpFuelStatusColor;

		// Token: 0x04000AF8 RID: 2808
		private bool warpFuelAvailable;

		// Token: 0x04000AF9 RID: 2809
		private bool warpFuelActive;
	}
}
