using System;
using System.Collections.Generic;
using Behaviour.UI.Missions;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Timer;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001E3 RID: 483
	public class MissionBoard : MonoBehaviour
	{
		// Token: 0x0600125B RID: 4699 RVA: 0x00078ACB File Offset: 0x00076CCB
		private void Awake()
		{
			this.spacestation = (GamePlayer.current.currentPointOfInterest as SpaceStation);
			SpaceStation spaceStation = this.spacestation;
			if (spaceStation == null)
			{
				return;
			}
			spaceStation.missionBoard.SetRefreshCallback(new Action(this.ShowMissionBoard));
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00078B03 File Offset: 0x00076D03
		private void Start()
		{
			this.ShowMissionBoard();
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00078B0B File Offset: 0x00076D0B
		private void Update()
		{
			this.timer.timer = this.spacestation.missionBoard.remainingTime;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00078B28 File Offset: 0x00076D28
		private void OnDestroy()
		{
			this.spacestation.missionBoard.SetRefreshCallback(null);
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00078B3C File Offset: 0x00076D3C
		public void ShowMission(Mission m, bool userClicked = false)
		{
			this.detailsParent.ShowMission(m, true);
			if (userClicked && this.spacestation.missionBoard.remainingTime < 20f)
			{
				this.spacestation.missionBoard.ProgressTimer(-30f);
				this.timer.timer = this.spacestation.missionBoard.remainingTime;
			}
			foreach (MissionRow missionRow in this.rows)
			{
				missionRow.SetSelected(m == missionRow.contained);
			}
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00078BF0 File Offset: 0x00076DF0
		public void ShowMissionBoard()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.rows.Clear();
			this.listParent.DestroyChildren();
			Mission mission = null;
			Mission mission2 = null;
			bool flag = false;
			float num = 0f;
			foreach (Mission mission3 in this.GetVisibleMissions())
			{
				if (mission2 == null)
				{
					mission2 = mission3;
				}
				if (mission == null && this.IsImportantMission(mission3))
				{
					mission = mission3;
				}
				if (mission3 == MissionBoard.preselectMission)
				{
					flag = true;
				}
				MissionRow missionRow = UnityEngine.Object.Instantiate<MissionRow>(this.rowPrefab, this.listParent);
				missionRow.SetMission(mission3);
				this.rows.Add(missionRow);
				((RectTransform)missionRow.transform).anchoredPosition = new Vector2(0f, num);
				num -= 34f;
			}
			this.listParent.sizeDelta = new Vector2(this.listParent.sizeDelta.x, -num);
			if (flag)
			{
				this.ShowMission(MissionBoard.preselectMission, false);
			}
			else if (mission != null)
			{
				this.ShowMission(mission, false);
			}
			else if (mission2 != null)
			{
				this.ShowMission(mission2, false);
			}
			else
			{
				this.detailsParent.NoActiveMission();
			}
			this.timer.timer = this.spacestation.missionBoard.remainingTime;
			MissionBoard.preselectMission = null;
			bool flag2 = this.spacestation.umbralControlLevel >= 0.05f;
			if (flag2 && mission2 == null)
			{
				this.SwitchToUmbralMission();
				flag2 = false;
			}
			if (flag2)
			{
				MissionObjective.Trigger(MissionTrigger.MissionBoardOpenedWithUmbral, null, null, false);
			}
			this.umbralButton.gameObject.SetActive(flag2);
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00078D98 File Offset: 0x00076F98
		private bool IsImportantMission(Mission mission)
		{
			return this.spacestation.missionBoard.availableMissions.Contains(mission) || (mission.isComplete && mission.turnIn == this.spacestation);
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00078DCD File Offset: 0x00076FCD
		private IEnumerable<Mission> GetVisibleMissions()
		{
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (mission.turnIn == this.spacestation)
				{
					yield return mission;
				}
			}
			List<Mission>.Enumerator enumerator = default(List<Mission>.Enumerator);
			foreach (Mission mission2 in this.spacestation.missionBoard.availableMissions)
			{
				yield return mission2;
			}
			enumerator = default(List<Mission>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00078DDD File Offset: 0x00076FDD
		public void GenerateNewMissions()
		{
			this.spacestation.missionBoard.RegenerateMissions(6);
			this.ShowMissionBoard();
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00078DF6 File Offset: 0x00076FF6
		public void SwitchToUmbralMission()
		{
			UnityEngine.Object.Instantiate<UmbralBoard>(this.umbralBoard, base.transform.parent);
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04000A31 RID: 2609
		[SerializeField]
		public RectTransform listParent;

		// Token: 0x04000A32 RID: 2610
		[SerializeField]
		public MissionRow rowPrefab;

		// Token: 0x04000A33 RID: 2611
		[SerializeField]
		public MissionDetails detailsParent;

		// Token: 0x04000A34 RID: 2612
		[SerializeField]
		private CountdownTimer timer;

		// Token: 0x04000A35 RID: 2613
		[SerializeField]
		private Button umbralButton;

		// Token: 0x04000A36 RID: 2614
		[SerializeField]
		private UmbralBoard umbralBoard;

		// Token: 0x04000A37 RID: 2615
		private const float UserInterruptTime = 30f;

		// Token: 0x04000A38 RID: 2616
		public static Mission preselectMission;

		// Token: 0x04000A39 RID: 2617
		private SpaceStation spacestation;

		// Token: 0x04000A3A RID: 2618
		private List<MissionRow> rows = new List<MissionRow>();
	}
}
