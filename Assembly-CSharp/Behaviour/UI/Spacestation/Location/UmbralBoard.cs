using System;
using Behaviour.UI.Missions;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x0200022D RID: 557
	public class UmbralBoard : MonoBehaviour
	{
		// Token: 0x060014F2 RID: 5362 RVA: 0x00087854 File Offset: 0x00085A54
		private void Start()
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null)
			{
				this.currentMission = spaceStation.missionBoard.GetUmbralMission(false);
			}
			if (this.currentMission != null)
			{
				this.ShowMission(this.currentMission);
			}
			else
			{
				this.ShowNoMission();
			}
			this.UpdateHeaderText();
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x000878A4 File Offset: 0x00085AA4
		private void UpdateHeaderText()
		{
			float percentage = 0f;
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null)
			{
				percentage = spaceStation.umbralControlLevel;
			}
			this.headerText.text = Translation.Highlight("@UmbralMissionHeader", ColorHelper.umbralColor, new object[]
			{
				GameMath.FormatPercentage(percentage, FormatPercentageMode.Default, 1)
			});
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x000878F7 File Offset: 0x00085AF7
		public void AcceptClicked()
		{
			GamePlayer.current.AcceptMission(this.currentMission);
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x0008790C File Offset: 0x00085B0C
		public void ShowMission(Mission contained)
		{
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
				if (num == 0f && num2 < -48f)
				{
					num = this.rewardsParent.rect.width * 0.7f;
					num2 = 0f;
				}
			}
			this.missionContent.gameObject.SetActive(true);
			this.noMissionContent.gameObject.SetActive(false);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00087AD0 File Offset: 0x00085CD0
		public void ShowNoMission()
		{
			this.missionContent.gameObject.SetActive(false);
			this.noMissionContent.gameObject.SetActive(true);
		}

		// Token: 0x04000C41 RID: 3137
		[SerializeField]
		private TMP_Text headerText;

		// Token: 0x04000C42 RID: 3138
		[SerializeField]
		private RectTransform missionContent;

		// Token: 0x04000C43 RID: 3139
		[SerializeField]
		private RectTransform noMissionContent;

		// Token: 0x04000C44 RID: 3140
		[SerializeField]
		private TMP_Text missionLevel;

		// Token: 0x04000C45 RID: 3141
		[SerializeField]
		private TMP_Text missionName;

		// Token: 0x04000C46 RID: 3142
		[SerializeField]
		private TMP_Text missionDesc;

		// Token: 0x04000C47 RID: 3143
		[SerializeField]
		private TMP_Text rewardsLabel;

		// Token: 0x04000C48 RID: 3144
		[SerializeField]
		private RectTransform rewardsParent;

		// Token: 0x04000C49 RID: 3145
		[SerializeField]
		private MissionRewardRow rewardPrefab;

		// Token: 0x04000C4A RID: 3146
		private Mission currentMission;
	}
}
