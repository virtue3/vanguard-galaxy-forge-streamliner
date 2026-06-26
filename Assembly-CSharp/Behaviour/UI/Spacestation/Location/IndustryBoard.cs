using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.UI.Missions;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000221 RID: 545
	public class IndustryBoard : MonoBehaviour
	{
		// Token: 0x06001430 RID: 5168 RVA: 0x00082198 File Offset: 0x00080398
		private void Start()
		{
			this.levelText.TL("@IndustryLevel", new object[]
			{
				GamePlayer.current.industryRank + 1
			});
			if (GamePlayer.current.industryRank > 0)
			{
				this.levelText.rectTransform.anchoredPosition = new Vector2(-86f, 0f);
				this.retireButton.gameObject.SetActive(true);
			}
			List<IndustryMission> list = SpaceStation.current.industryBoard.GenerateIndustryMissions();
			this.missionRows = new List<BountyMissionRow>();
			this.missionsParent.DestroyChildren();
			float num = 0f;
			foreach (IndustryMission mission in list)
			{
				BountyMissionRow bountyMissionRow = UnityEngine.Object.Instantiate<BountyMissionRow>(this.missionPrefab, this.missionsParent);
				bountyMissionRow.SetMission(mission);
				RectTransform rectTransform = (RectTransform)bountyMissionRow.transform;
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, num);
				num -= 80f;
				this.missionRows.Add(bountyMissionRow);
			}
			this.ShowMission(list[0]);
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x000822D4 File Offset: 0x000804D4
		public void LaunchClicked()
		{
			if (!GameplayManager.Instance.spaceShip.AmmoInCargoForTurrets(false))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoAmmoForTurret", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			SpaceStation.current.industryBoard.industryCounter++;
			GamePlayer.current.currentIndustry = this.selectedMission;
			Singleton<FocusedMissionHandler>.Instance.SetMission(this.selectedMission);
			GamePlayer.current.EndAutopilotSession();
			Singleton<TravelManager>.Instance.TryInitiateTravel(this.selectedMission.steps[0].dynamicPointOfInterest);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x00082380 File Offset: 0x00080580
		public void ShowMission(IndustryMission contained)
		{
			this.selectedMission = contained;
			this.missionLevel.TL("@TooltipItemLevel", new object[]
			{
				contained.level
			});
			this.missionName.text = contained.name;
			this.missionDesc.TL(contained.description, Array.Empty<object>());
			this.rewardsLabel.rectTransform.anchoredPosition = new Vector2(this.rewardsLabel.rectTransform.anchoredPosition.x, this.missionDesc.rectTransform.anchoredPosition.y - this.missionDesc.preferredHeight - 10f);
			this.rewardsParent.anchoredPosition = new Vector2(this.rewardsParent.anchoredPosition.x, this.rewardsLabel.rectTransform.anchoredPosition.y - 20f);
			this.rewardsParent.DestroyChildren();
			float num = 0f;
			float num2 = 0f;
			foreach (MissionReward reward in contained.rewards)
			{
				MissionRewardRow missionRewardRow = UnityEngine.Object.Instantiate<MissionRewardRow>(this.rewardPrefab, this.rewardsParent);
				missionRewardRow.SetReward(reward);
				((RectTransform)missionRewardRow.transform).anchoredPosition = new Vector2(num, num2);
				num2 -= 24f;
				if (num == 0f && num2 < -24f)
				{
					num = this.rewardsParent.rect.width * 0.7f;
					num2 = 0f;
				}
			}
			foreach (BountyMissionRow bountyMissionRow in this.missionRows)
			{
				bountyMissionRow.UpdateSelectedMission(contained);
			}
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0008256C File Offset: 0x0008076C
		public void DoRetire()
		{
			int industryRank = GamePlayer.current.industryRank;
			int commendationCount = Mathf.RoundToInt(5f * Mathf.Pow((float)industryRank, 1.2f));
			InventoryItemType commendation = "IndustryCurrency";
			AlertPopup.ShowQuery(Translation.TranslateOnly("@IndustryMissionRetire", new object[]
			{
				commendationCount,
				commendation.displayName
			}), null, null, delegate
			{
				GamePlayer.current.industryRank = 0;
				GamePlayer.current.currentSpaceShip.AddCargo(commendation, commendationCount, false);
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.IndustryBoard, true);
			}, null, null, null);
		}

		// Token: 0x04000BB6 RID: 2998
		[SerializeField]
		private TMP_Text levelText;

		// Token: 0x04000BB7 RID: 2999
		[SerializeField]
		private Button retireButton;

		// Token: 0x04000BB8 RID: 3000
		[SerializeField]
		private RectTransform missionsParent;

		// Token: 0x04000BB9 RID: 3001
		[SerializeField]
		private BountyMissionRow missionPrefab;

		// Token: 0x04000BBA RID: 3002
		[SerializeField]
		private TMP_Text missionLevel;

		// Token: 0x04000BBB RID: 3003
		[SerializeField]
		private TMP_Text missionName;

		// Token: 0x04000BBC RID: 3004
		[SerializeField]
		private TMP_Text missionDesc;

		// Token: 0x04000BBD RID: 3005
		[SerializeField]
		private TMP_Text rewardsLabel;

		// Token: 0x04000BBE RID: 3006
		[SerializeField]
		private RectTransform rewardsParent;

		// Token: 0x04000BBF RID: 3007
		[SerializeField]
		private MissionRewardRow rewardPrefab;

		// Token: 0x04000BC0 RID: 3008
		private List<BountyMissionRow> missionRows;

		// Token: 0x04000BC1 RID: 3009
		private IndustryMission selectedMission;
	}
}
