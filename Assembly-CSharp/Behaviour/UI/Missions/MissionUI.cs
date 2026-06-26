using System;
using System.Collections.Generic;
using Source.MissionSystem;
using Source.Player;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Missions
{
	// Token: 0x0200025D RID: 605
	public class MissionUI : MonoBehaviour
	{
		// Token: 0x0600164E RID: 5710 RVA: 0x0008DF28 File Offset: 0x0008C128
		private void Start()
		{
			this.UpdateMissionList();
		}

		// Token: 0x0600164F RID: 5711 RVA: 0x0008DF30 File Offset: 0x0008C130
		public void UpdateMissionList()
		{
			this.rows.Clear();
			this.missionList.DestroyChildren();
			Mission mission = null;
			float num = 0f;
			foreach (Mission mission2 in GamePlayer.current.allMissions)
			{
				if (mission == null)
				{
					mission = mission2;
				}
				MissionRow missionRow = UnityEngine.Object.Instantiate<MissionRow>(this.rowPrefab, this.missionList);
				missionRow.SetMission(mission2);
				this.rows.Add(missionRow);
				((RectTransform)missionRow.transform).anchoredPosition = new Vector2(0f, num);
				num -= 32f;
			}
			this.missionList.sizeDelta = new Vector2(this.missionList.sizeDelta.x, -num);
			if (mission != null)
			{
				this.ShowMission(mission);
			}
			else
			{
				this.details.NoActiveMission();
			}
			this.SetMissionLimit();
		}

		// Token: 0x06001650 RID: 5712 RVA: 0x0008E028 File Offset: 0x0008C228
		private void SetMissionLimit()
		{
			this.missionLimit.text = string.Format("{0} / {1}", GamePlayer.current.missions.Count, 20);
		}

		// Token: 0x06001651 RID: 5713 RVA: 0x0008E05C File Offset: 0x0008C25C
		public void ShowMission(Mission mission)
		{
			this.details.ShowMission(mission, true);
			foreach (MissionRow missionRow in this.rows)
			{
				missionRow.SetSelected(mission == missionRow.contained);
			}
		}

		// Token: 0x04000D89 RID: 3465
		[SerializeField]
		private RectTransform missionList;

		// Token: 0x04000D8A RID: 3466
		[SerializeField]
		private MissionRow rowPrefab;

		// Token: 0x04000D8B RID: 3467
		[SerializeField]
		private TextMeshProUGUI missionLimit;

		// Token: 0x04000D8C RID: 3468
		[SerializeField]
		private MissionDetails details;

		// Token: 0x04000D8D RID: 3469
		private List<MissionRow> rows = new List<MissionRow>();
	}
}
