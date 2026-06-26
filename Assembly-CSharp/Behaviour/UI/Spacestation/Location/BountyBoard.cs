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
	// Token: 0x0200021F RID: 543
	public class BountyBoard : MonoBehaviour
	{
		// Token: 0x06001424 RID: 5156 RVA: 0x00081BEC File Offset: 0x0007FDEC
		private void Start()
		{
			this.levelText.TL("@BountyLvl", new object[]
			{
				GamePlayer.current.bountyRank + 1
			});
			if (GamePlayer.current.bountyRank > 0)
			{
				this.levelText.rectTransform.anchoredPosition = new Vector2(-86f, 0f);
				this.retireButton.gameObject.SetActive(true);
			}
			List<BountyMission> list = SpaceStation.current.bountyBoard.GenerateBountyMissions();
			this.missionRows = new List<BountyMissionRow>();
			this.missionsParent.DestroyChildren();
			float num = 0f;
			foreach (BountyMission mission in list)
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

		// Token: 0x06001425 RID: 5157 RVA: 0x00081D28 File Offset: 0x0007FF28
		public void LaunchClicked()
		{
			if (!GameplayManager.Instance.spaceShip.AmmoInCargoForTurrets(false))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoAmmoForTurret", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			SpaceStation.current.bountyBoard.bountyCounter++;
			GamePlayer.current.currentBounty = this.selectedMission;
			Singleton<FocusedMissionHandler>.Instance.SetMission(this.selectedMission);
			GamePlayer.current.EndAutopilotSession();
			Singleton<TravelManager>.Instance.bountyAbandonTimer = 0f;
			Singleton<TravelManager>.Instance.TryInitiateTravel(this.selectedMission.steps[0].dynamicPointOfInterest);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00081DE4 File Offset: 0x0007FFE4
		public void ShowMission(BountyMission contained)
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

		// Token: 0x06001427 RID: 5159 RVA: 0x00081FD0 File Offset: 0x000801D0
		public void DoRetire()
		{
			int bountyRank = GamePlayer.current.bountyRank;
			int commendationCount = Mathf.RoundToInt(5f * Mathf.Pow((float)bountyRank, 1.2f));
			InventoryItemType commendation = "BountyCurrency";
			AlertPopup.ShowQuery(Translation.TranslateOnly("@BountyMissionRetireDesc", new object[]
			{
				commendationCount,
				commendation.displayName
			}), null, null, delegate
			{
				GamePlayer.current.bountyRank = 0;
				GamePlayer.current.currentSpaceShip.AddCargo(commendation, commendationCount, false);
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.BountyBoard, true);
			}, null, null, null);
		}

		// Token: 0x04000BA3 RID: 2979
		[SerializeField]
		private TMP_Text levelText;

		// Token: 0x04000BA4 RID: 2980
		[SerializeField]
		private Button retireButton;

		// Token: 0x04000BA5 RID: 2981
		[SerializeField]
		private RectTransform missionsParent;

		// Token: 0x04000BA6 RID: 2982
		[SerializeField]
		private BountyMissionRow missionPrefab;

		// Token: 0x04000BA7 RID: 2983
		[SerializeField]
		private TMP_Text missionLevel;

		// Token: 0x04000BA8 RID: 2984
		[SerializeField]
		private TMP_Text missionName;

		// Token: 0x04000BA9 RID: 2985
		[SerializeField]
		private TMP_Text missionDesc;

		// Token: 0x04000BAA RID: 2986
		[SerializeField]
		private TMP_Text rewardsLabel;

		// Token: 0x04000BAB RID: 2987
		[SerializeField]
		private RectTransform rewardsParent;

		// Token: 0x04000BAC RID: 2988
		[SerializeField]
		private MissionRewardRow rewardPrefab;

		// Token: 0x04000BAD RID: 2989
		private List<BountyMissionRow> missionRows;

		// Token: 0x04000BAE RID: 2990
		private BountyMission selectedMission;
	}
}
