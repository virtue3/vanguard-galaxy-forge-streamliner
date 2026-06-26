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
	// Token: 0x02000222 RID: 546
	public class PatrolBoard : MonoBehaviour
	{
		// Token: 0x06001435 RID: 5173 RVA: 0x00082604 File Offset: 0x00080804
		private void Start()
		{
			this.levelText.TL("@PatrolLvl", new object[]
			{
				GamePlayer.current.patrolRank + 1
			});
			if (GamePlayer.current.patrolRank > 0)
			{
				this.levelText.rectTransform.anchoredPosition = new Vector2(-86f, 0f);
				this.retireButton.gameObject.SetActive(true);
			}
			List<PatrolMission> list = SpaceStation.current.patrolBoard.GeneratePatrolMissions();
			this.missionRows = new List<BountyMissionRow>();
			this.missionsParent.DestroyChildren();
			float num = 0f;
			foreach (PatrolMission mission in list)
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

		// Token: 0x06001436 RID: 5174 RVA: 0x00082740 File Offset: 0x00080940
		public void LaunchClicked()
		{
			if (!GameplayManager.Instance.spaceShip.AmmoInCargoForTurrets(false))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoAmmoForTurret", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			SpaceStation.current.patrolBoard.patrolCounter++;
			GamePlayer.current.currentPatrol = this.selectedMission;
			Singleton<FocusedMissionHandler>.Instance.SetMission(this.selectedMission);
			GamePlayer.current.EndAutopilotSession();
			Singleton<TravelManager>.Instance.TryInitiateTravel(this.selectedMission.steps[0].dynamicPointOfInterest);
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x000827EC File Offset: 0x000809EC
		public void ShowMission(PatrolMission contained)
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

		// Token: 0x06001438 RID: 5176 RVA: 0x000829D8 File Offset: 0x00080BD8
		public void DoRetire()
		{
			int patrolRank = GamePlayer.current.patrolRank;
			int commendationCount = Mathf.RoundToInt(5f * Mathf.Pow((float)patrolRank, 1.2f));
			InventoryItemType commendation = "PatrolCurrency";
			AlertPopup.ShowQuery(Translation.TranslateOnly("@PatrolMissionRetire", new object[]
			{
				commendationCount,
				commendation.displayName
			}), null, null, delegate
			{
				GamePlayer.current.patrolRank = 0;
				GamePlayer.current.currentSpaceShip.AddCargo(commendation, commendationCount, false);
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.PoliceBoard, true);
			}, null, null, null);
		}

		// Token: 0x04000BC2 RID: 3010
		[SerializeField]
		private TMP_Text levelText;

		// Token: 0x04000BC3 RID: 3011
		[SerializeField]
		private Button retireButton;

		// Token: 0x04000BC4 RID: 3012
		[SerializeField]
		private RectTransform missionsParent;

		// Token: 0x04000BC5 RID: 3013
		[SerializeField]
		private BountyMissionRow missionPrefab;

		// Token: 0x04000BC6 RID: 3014
		[SerializeField]
		private TMP_Text missionLevel;

		// Token: 0x04000BC7 RID: 3015
		[SerializeField]
		private TMP_Text missionName;

		// Token: 0x04000BC8 RID: 3016
		[SerializeField]
		private TMP_Text missionDesc;

		// Token: 0x04000BC9 RID: 3017
		[SerializeField]
		private TMP_Text rewardsLabel;

		// Token: 0x04000BCA RID: 3018
		[SerializeField]
		private RectTransform rewardsParent;

		// Token: 0x04000BCB RID: 3019
		[SerializeField]
		private MissionRewardRow rewardPrefab;

		// Token: 0x04000BCC RID: 3020
		private List<BountyMissionRow> missionRows;

		// Token: 0x04000BCD RID: 3021
		private PatrolMission selectedMission;
	}
}
