using System;
using LightJson;
using Source.Player;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B7 RID: 183
	public class MissionFollowUp : MissionReward
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0003DBE0 File Offset: 0x0003BDE0
		public override string rewardText
		{
			get
			{
				return "";
			}
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x0003DBE7 File Offset: 0x0003BDE7
		public override void DataToJson(JsonObject data)
		{
			data["mission"] = this.followUpMission.ToJson();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x0003DBFF File Offset: 0x0003BDFF
		public override void LoadFromJson(JsonObject data)
		{
			this.followUpMission = Mission.FromJson(data["mission"]);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x0003DC17 File Offset: 0x0003BE17
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.AddMissionWithLog(this.followUpMission);
		}

		// Token: 0x04000445 RID: 1093
		public Mission followUpMission;
	}
}
