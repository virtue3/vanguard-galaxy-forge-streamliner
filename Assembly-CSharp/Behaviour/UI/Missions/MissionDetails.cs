using System;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Missions
{
	// Token: 0x0200025A RID: 602
	public class MissionDetails : MonoBehaviour
	{
		// Token: 0x06001636 RID: 5686 RVA: 0x0008D2B8 File Offset: 0x0008B4B8
		private void Awake()
		{
			this.sidePanelParent = base.GetComponentInParent<MissionUI>();
			this.spaceStationParent = base.GetComponentInParent<MissionBoard>();
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0008D2D4 File Offset: 0x0008B4D4
		private void Update()
		{
			if (this.currentMission != null)
			{
				this.updateTimer -= Time.deltaTime;
				if (this.updateTimer < 0f)
				{
					this.ShowMission(this.currentMission, false);
					this.updateTimer = 1f;
				}
			}
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0008D320 File Offset: 0x0008B520
		public void NoActiveMission()
		{
			this.currentMission = null;
			this.noActiveMission.gameObject.SetActive(true);
			this.activeMission.gameObject.SetActive(false);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0008D34C File Offset: 0x0008B54C
		public void ShowMission(Mission mission, bool updateRewards = true)
		{
			this.noActiveMission.gameObject.SetActive(false);
			this.activeMission.gameObject.SetActive(true);
			bool flag = mission.CanClaimRewards();
			if (this.sidePanelParent)
			{
				this.interactButton.gameObject.SetActive(flag);
				this.interactLabel.TL("@UIComplete", Array.Empty<object>());
				this.abandonButton.gameObject.SetActive(mission.canAbandon);
				this.trackButton.gameObject.SetActive(true);
				this.locateButton.gameObject.SetActive(mission.GetActivePoi(false) != null);
			}
			else if (SpaceStation.current.missionBoard.availableMissions.Contains(mission))
			{
				this.interactButton.gameObject.SetActive(true);
				this.interactLabel.TL("@UIAccept", Array.Empty<object>());
				this.abandonButton.gameObject.SetActive(false);
				this.trackButton.gameObject.SetActive(false);
				this.locateButton.gameObject.SetActive(false);
			}
			else
			{
				this.interactButton.gameObject.SetActive(flag);
				this.interactLabel.TL("@UIComplete", Array.Empty<object>());
				this.abandonButton.gameObject.SetActive(mission.canAbandon);
				this.trackButton.gameObject.SetActive(true);
				this.locateButton.gameObject.SetActive(false);
			}
			if (GamePlayer.current.missions.Contains(mission))
			{
				this.locateButton.gameObject.SetActive(mission.GetActivePoi(false) != null);
			}
			if (flag)
			{
				this.locateButton.gameObject.SetActive(false);
			}
			if (Singleton<FocusedMissionHandler>.Instance.focusedMission == mission)
			{
				this.trackButton.gameObject.SetActive(false);
			}
			this.currentMission = mission;
			this.missionName.TL(mission.name, Array.Empty<object>());
			this.missionName.color = mission.difficulty.GetColor();
			string text = "[" + Translation.Translate("@" + mission.difficulty.ToString(), Array.Empty<object>()) + "]";
			if (mission.targetLayer != null)
			{
				TargetLayer? targetLayer = mission.targetLayer;
				TargetLayer targetLayer2 = TargetLayer.Core;
				string str = (targetLayer.GetValueOrDefault() == targetLayer2 & targetLayer != null) ? mission.coreName : mission.targetLayer.ToString();
				text = text + " - " + Translation.Translate("@" + str, Array.Empty<object>());
			}
			this.missionDifficulty.text = text;
			this.factionGiver.TL(mission.sourceFaction.name, Array.Empty<object>());
			if (mission.failed)
			{
				this.objectivesText.TL("@UIMissionFailed", Array.Empty<object>());
				this.objectivesText.color = ColorHelper.reddish;
			}
			else
			{
				string text2 = "";
				foreach (MissionStep missionStep in mission.steps)
				{
					if (!missionStep.hidden)
					{
						Color color = (mission.currentStep == missionStep) ? ColorHelper.offWhite : ColorHelper.boringGrey;
						foreach (MissionObjective missionObjective in missionStep.objectives)
						{
							string text3 = " - " + missionObjective.statusText;
							text2 = text2 + text3.HighlightWithColor(missionObjective.IsComplete() ? ColorHelper.greenish : color) + "\n";
						}
						foreach (MapPointOfInterest mapPointOfInterest in missionStep.pointsOfInterest)
						{
							if (mapPointOfInterest != null && mission.currentStep == missionStep)
							{
								MapPointOfInterest mapPointOfInterest2 = missionStep.currentObjective.GetPoi() ?? mapPointOfInterest;
								text2 = text2 + " " + Translation.Translate("@MissionLocation", new object[]
								{
									mapPointOfInterest2.name
								}).HighlightWithColor(ColorHelper.modifierColor) + "\n\n";
								if (mapPointOfInterest.dangerLevel != null)
								{
									text2 = text2 + Translation.Translate(mapPointOfInterest.dangerLevel, Array.Empty<object>()).HighlightWithColor(ColorHelper.reddish) + "\n";
								}
								if (mapPointOfInterest.hazardsDescription != null)
								{
									text2 = text2 + mapPointOfInterest.hazardsDescription.HighlightWithColor(ColorHelper.orange75) + "\n";
								}
							}
						}
					}
				}
				this.objectivesText.TL(Translation.TranslateOnly("@UIMissionObjectives", Array.Empty<object>()).HighlightWithColor(ColorHelper.fadedGrey) + "\n\n" + text2, Array.Empty<object>());
				this.objectivesText.color = Color.white;
			}
			this.description.TL(mission.description, Array.Empty<object>());
			this.description.rectTransform.anchoredPosition = new Vector2(this.description.rectTransform.anchoredPosition.x, this.objectivesText.rectTransform.anchoredPosition.y - this.objectivesText.preferredHeight - 16f);
			this.rewardsHeader.anchoredPosition = new Vector2(this.rewardsHeader.anchoredPosition.x, this.description.rectTransform.anchoredPosition.y - this.description.preferredHeight - 16f);
			if (updateRewards)
			{
				this.rewardsParent.DestroyChildren();
				float num = 0f;
				foreach (MissionReward missionReward in mission.rewards)
				{
					if (!missionReward.hidden)
					{
						MissionRewardRow missionRewardRow = UnityEngine.Object.Instantiate<MissionRewardRow>(this.rewardPrefab, this.rewardsParent);
						missionRewardRow.SetReward(missionReward);
						(missionRewardRow.transform as RectTransform).anchoredPosition = new Vector2(0f, -num);
						num += 20f;
					}
				}
				this.rewardsHeader.sizeDelta = new Vector2(this.rewardsHeader.sizeDelta.x, num);
			}
			this.detailsParent.sizeDelta = new Vector2(this.detailsParent.sizeDelta.x, -this.rewardsHeader.anchoredPosition.y + this.rewardsHeader.sizeDelta.y + 20f);
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x0008DA68 File Offset: 0x0008BC68
		public void ButtonInteract()
		{
			if (this.sidePanelParent)
			{
				GamePlayer.current.CompleteMission(this.currentMission, false);
				return;
			}
			if (SpaceStation.current.missionBoard.availableMissions.Contains(this.currentMission))
			{
				GamePlayer.current.AcceptMission(this.currentMission);
				return;
			}
			GamePlayer.current.CompleteMission(this.currentMission, false);
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0008DAD2 File Offset: 0x0008BCD2
		public void ButtonAbandon()
		{
			AlertPopup.ShowQuery("@UIMissionAbandonQuery", Translation.Translate("@UIYes", Array.Empty<object>()), Translation.Translate("@UINo", Array.Empty<object>()), delegate
			{
				this.AbandonMission(this.currentMission);
			}, null, null, null);
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0008DB0C File Offset: 0x0008BD0C
		private void AbandonMission(Mission mission)
		{
			GamePlayer.current.RemoveMission(mission, false);
			if (SidePanel.instance.currentTab == SidePanel.SideTabType.Missions)
			{
				SidePanel.instance.RefreshIfOpen();
			}
			SpaceStationInterior instance = SpaceStationInterior.instance;
			if (instance != null && instance.currentTab == SpaceStationFacility.MissionBoard)
			{
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.MissionBoard, true);
			}
			if (this.currentMission.trackedOnHud)
			{
				Singleton<FocusedMissionHandler>.Current.ResetFocusedMission();
			}
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x0008DB77 File Offset: 0x0008BD77
		public void ButtonTrack()
		{
			Singleton<FocusedMissionHandler>.Instance.SetMission(this.currentMission);
			this.trackButton.gameObject.SetActive(false);
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0008DB9A File Offset: 0x0008BD9A
		public void ButtonLocate()
		{
			SidePanel.instance.ShowPoiOnMap(this.currentMission.GetActivePoi(false));
		}

		// Token: 0x04000D63 RID: 3427
		[SerializeField]
		private RectTransform detailsParent;

		// Token: 0x04000D64 RID: 3428
		[SerializeField]
		private TMP_Text noActiveMission;

		// Token: 0x04000D65 RID: 3429
		[SerializeField]
		private RectTransform activeMission;

		// Token: 0x04000D66 RID: 3430
		[SerializeField]
		private TMP_Text missionName;

		// Token: 0x04000D67 RID: 3431
		[SerializeField]
		private TMP_Text missionDifficulty;

		// Token: 0x04000D68 RID: 3432
		[SerializeField]
		private TMP_Text factionGiver;

		// Token: 0x04000D69 RID: 3433
		[SerializeField]
		private TMP_Text description;

		// Token: 0x04000D6A RID: 3434
		[SerializeField]
		private TMP_Text objectivesText;

		// Token: 0x04000D6B RID: 3435
		[SerializeField]
		private RectTransform rewardsHeader;

		// Token: 0x04000D6C RID: 3436
		[SerializeField]
		private RectTransform rewardsParent;

		// Token: 0x04000D6D RID: 3437
		[SerializeField]
		private MissionRewardRow rewardPrefab;

		// Token: 0x04000D6E RID: 3438
		[SerializeField]
		private Button interactButton;

		// Token: 0x04000D6F RID: 3439
		[SerializeField]
		private TMP_Text interactLabel;

		// Token: 0x04000D70 RID: 3440
		[SerializeField]
		private Button abandonButton;

		// Token: 0x04000D71 RID: 3441
		[SerializeField]
		private Button trackButton;

		// Token: 0x04000D72 RID: 3442
		[SerializeField]
		private Button locateButton;

		// Token: 0x04000D73 RID: 3443
		private Mission currentMission;

		// Token: 0x04000D74 RID: 3444
		private float updateTimer;

		// Token: 0x04000D75 RID: 3445
		private MissionUI sidePanelParent;

		// Token: 0x04000D76 RID: 3446
		private MissionBoard spaceStationParent;
	}
}
