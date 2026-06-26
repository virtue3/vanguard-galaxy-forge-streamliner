using System;
using LightJson;
using Source.Player;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000BD RID: 189
	public class StoryMission : MissionReward
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0003DF1C File Offset: 0x0003C11C
		public override string rewardText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0003DF23 File Offset: 0x0003C123
		public override void DataToJson(JsonObject data)
		{
			data["missionId"] = this.missionId;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x0003DF3B File Offset: 0x0003C13B
		public override void LoadFromJson(JsonObject data)
		{
			this.missionId = data["missionId"];
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0003DF54 File Offset: 0x0003C154
		public override void OnComplete(Mission m)
		{
			if (this.missionId == "tutorial_salvage_10")
			{
				this.missionId = "tutorial_10";
			}
			Mission mission = StoryMission.Get(GamePlayer.current, this.missionId);
			GamePlayer.current.AddMissionWithLog(mission);
		}

		// Token: 0x0400044C RID: 1100
		public string missionId;
	}
}
