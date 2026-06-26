using System;
using Behaviour.Util;
using Source.MissionSystem;
using Source.Player;

namespace Behaviour.UI.Missions
{
	// Token: 0x02000259 RID: 601
	public class FocusedMissionHandler : Singleton<FocusedMissionHandler>
	{
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x0008D0CD File Offset: 0x0008B2CD
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x0008D0D5 File Offset: 0x0008B2D5
		public Mission focusedMission { get; private set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600162F RID: 5679 RVA: 0x0008D0E0 File Offset: 0x0008B2E0
		// (remove) Token: 0x06001630 RID: 5680 RVA: 0x0008D118 File Offset: 0x0008B318
		public event Action<Mission> OnFocusedMissionChanged;

		// Token: 0x06001631 RID: 5681 RVA: 0x0008D14D File Offset: 0x0008B34D
		private void Start()
		{
			this.SetupFocusedMission();
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0008D158 File Offset: 0x0008B358
		private void SetupFocusedMission()
		{
			if (GamePlayer.current == null)
			{
				return;
			}
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				if (mission.trackedOnHud)
				{
					this.SetMission(mission);
				}
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0008D1BC File Offset: 0x0008B3BC
		public void SetMission(Mission mission)
		{
			this.focusedMission = mission;
			if (mission != null)
			{
				mission.trackedOnHud = true;
			}
			foreach (Mission mission2 in GamePlayer.current.missions)
			{
				mission2.trackedOnHud = (mission2 == mission);
			}
			Action<Mission> onFocusedMissionChanged = this.OnFocusedMissionChanged;
			if (onFocusedMissionChanged == null)
			{
				return;
			}
			onFocusedMissionChanged(mission);
		}

		// Token: 0x06001634 RID: 5684 RVA: 0x0008D23C File Offset: 0x0008B43C
		public void ResetFocusedMission()
		{
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (mission.difficulty == MissionDifficulty.Tutorial || mission.difficulty == MissionDifficulty.Story)
				{
					this.SetMission(mission);
					return;
				}
			}
			this.SetMission(null);
		}
	}
}
