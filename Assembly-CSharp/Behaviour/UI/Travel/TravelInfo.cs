using System;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Travel
{
	// Token: 0x020001FF RID: 511
	public class TravelInfo : MonoBehaviour
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001327 RID: 4903 RVA: 0x0007DCBD File Offset: 0x0007BEBD
		// (set) Token: 0x06001328 RID: 4904 RVA: 0x0007DCC5 File Offset: 0x0007BEC5
		public TravelETAInfo travelETAInfo { get; private set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001329 RID: 4905 RVA: 0x0007DCCE File Offset: 0x0007BECE
		// (set) Token: 0x0600132A RID: 4906 RVA: 0x0007DCD6 File Offset: 0x0007BED6
		public bool visible { get; private set; }

		// Token: 0x0600132B RID: 4907 RVA: 0x0007DCE0 File Offset: 0x0007BEE0
		private void Awake()
		{
			TravelInfo.instance = this;
			this.remainingDistanceTL = Translation.Translate("@TravelRemainingDistance", Array.Empty<object>());
			this.travelUnitsTL = Translation.Translate("@TravelUnits", Array.Empty<object>());
			this.travelSecondTL = Translation.Translate("@TravelSecond", Array.Empty<object>());
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0007DD32 File Offset: 0x0007BF32
		private void Update()
		{
			this.DynamicEventCountdown();
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0007DD3C File Offset: 0x0007BF3C
		private void DynamicEventCountdown()
		{
			if (this.dynamicEventTimer > 0f)
			{
				this.dynamicEventTimer -= Time.deltaTime;
				if (this.dynamicEventTimer <= 0f)
				{
					this.dynamicEventParent.gameObject.SetActive(false);
				}
				this.dynamicEventProgress.localScale = new Vector3(this.dynamicEventTimer / 5f, 1f, 1f);
			}
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0007DDAC File Offset: 0x0007BFAC
		public void InitializeTravelInformation(MapPointOfInterest target, float initialDistance, float unitsPerSecond)
		{
			this.ToggleVisible(true);
			this.destinationInfo.Init(target);
			this.travelSpeedInfo.Init(unitsPerSecond);
			this.travelWarpFuelInfo.Init();
			this.travelETAInfo.Init(initialDistance, unitsPerSecond);
			this.distanceText.text = string.Format("{0}: {1:0} {2}", this.remainingDistanceTL, initialDistance * 100f, this.travelUnitsTL);
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0007DE20 File Offset: 0x0007C020
		public void ShowJumpTransition()
		{
			this.distanceText.text = string.Concat(new string[]
			{
				this.remainingDistanceTL,
				": ",
				"--".HighlightWithColor(ColorHelper.fadedGrey),
				" ",
				this.travelUnitsTL
			});
			this.travelSpeedInfo.ShowTransition();
			this.travelETAInfo.ShowTransition();
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0007DE90 File Offset: 0x0007C090
		public void UpdateTravelInfo(float remainingDistance, float unitsPerSecond, float maxWarpSpeed, float maxWarpAcceleration)
		{
			remainingDistance *= 100f;
			this.distanceText.text = string.Format("{0}: {1:0} {2}", this.remainingDistanceTL, remainingDistance, this.travelUnitsTL);
			this.travelWarpFuelInfo.UpdateTravelInfo();
			float num = unitsPerSecond * 100f;
			string speed = GameMath.FormatNumber(num, 1);
			this.travelSpeedInfo.UpdateTravelInfo(speed);
			this.travelETAInfo.UpdateETA(remainingDistance, maxWarpSpeed, num, maxWarpAcceleration);
			this.emergencyActive.gameObject.SetActive(Singleton<TravelManager>.Instance.emergencyJumpActive);
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0007DF1E File Offset: 0x0007C11E
		public void ToggleVisible(bool toggle)
		{
			this.visible = toggle;
			this.UpdateVisibility();
		}

		// Token: 0x06001332 RID: 4914 RVA: 0x0007DF2D File Offset: 0x0007C12D
		public void ToggleHideUI(bool hide)
		{
			this.uiHidden = hide;
			this.UpdateVisibility();
		}

		// Token: 0x06001333 RID: 4915 RVA: 0x0007DF3C File Offset: 0x0007C13C
		private void UpdateVisibility()
		{
			this.container.gameObject.SetActive(this.visible && !this.uiHidden);
			if (!this.visible)
			{
				this.dynamicEventParent.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001334 RID: 4916 RVA: 0x0007DF7C File Offset: 0x0007C17C
		public void ShowDynamicEvent(MapPointOfInterest poi, string actionLabel)
		{
			this.dynamicEventPoi = poi;
			this.dynamicEventLabel.TL(poi.name, Array.Empty<object>());
			this.dynamicEventButton.TL(actionLabel, Array.Empty<object>());
			this.dynamicEventParent.gameObject.SetActive(true);
			this.dynamicEventSubLabel.gameObject.SetActive(GamePlayer.current.autoPlay);
			this.dynamicEventTimer = 5f;
		}

		// Token: 0x06001335 RID: 4917 RVA: 0x0007DFF0 File Offset: 0x0007C1F0
		public void VisitDynamicEvent()
		{
			this.dynamicEventParent.gameObject.SetActive(false);
			if (this.dynamicEventPoi != null)
			{
				if (GamePlayer.current.autoPlay)
				{
					GamePlayer.current.EndAutopilotSession();
				}
				Singleton<TravelManager>.Instance.SetRouteToPOI(this.dynamicEventPoi);
			}
		}

		// Token: 0x04000ACD RID: 2765
		public const float dynamicEventDuration = 5f;

		// Token: 0x04000ACE RID: 2766
		public static TravelInfo instance;

		// Token: 0x04000ACF RID: 2767
		[SerializeField]
		private RectTransform container;

		// Token: 0x04000AD0 RID: 2768
		[SerializeField]
		private TextMeshProUGUI distanceText;

		// Token: 0x04000AD1 RID: 2769
		[SerializeField]
		private TextMeshProUGUI etaText;

		// Token: 0x04000AD2 RID: 2770
		[SerializeField]
		private TravelDestinationInfo destinationInfo;

		// Token: 0x04000AD3 RID: 2771
		[SerializeField]
		private TravelSpeedInfo travelSpeedInfo;

		// Token: 0x04000AD4 RID: 2772
		[SerializeField]
		private TravelWaypointInfo gateInfo;

		// Token: 0x04000AD5 RID: 2773
		[SerializeField]
		private TravelWarpFuelInfo travelWarpFuelInfo;

		// Token: 0x04000AD6 RID: 2774
		[SerializeField]
		private TravelDistressInfo travelDistressInfo;

		// Token: 0x04000AD8 RID: 2776
		[SerializeField]
		private RectTransform dynamicEventParent;

		// Token: 0x04000AD9 RID: 2777
		[SerializeField]
		private RectTransform dynamicEventProgress;

		// Token: 0x04000ADA RID: 2778
		[SerializeField]
		private TMP_Text dynamicEventLabel;

		// Token: 0x04000ADB RID: 2779
		[SerializeField]
		private TMP_Text dynamicEventSubLabel;

		// Token: 0x04000ADC RID: 2780
		[SerializeField]
		private TMP_Text dynamicEventButton;

		// Token: 0x04000ADD RID: 2781
		[SerializeField]
		private TMP_Text emergencyActive;

		// Token: 0x04000ADE RID: 2782
		public const float DISTANCE_MULTIPLIER = 100f;

		// Token: 0x04000ADF RID: 2783
		private string remainingDistanceTL;

		// Token: 0x04000AE0 RID: 2784
		public string travelUnitsTL;

		// Token: 0x04000AE1 RID: 2785
		public string travelSecondTL;

		// Token: 0x04000AE2 RID: 2786
		private float dynamicEventTimer;

		// Token: 0x04000AE3 RID: 2787
		private MapPointOfInterest dynamicEventPoi;

		// Token: 0x04000AE5 RID: 2789
		private bool uiHidden;
	}
}
