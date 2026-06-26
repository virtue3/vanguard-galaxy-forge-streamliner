using System;
using LightJson;
using Source.Crew;
using Source.Player;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B4 RID: 180
	public class Crew : MissionReward
	{
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0003D9F5 File Offset: 0x0003BBF5
		public override string rewardText
		{
			get
			{
				return this.crew.GetFullName();
			}
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x0003DA02 File Offset: 0x0003BC02
		public override void DataToJson(JsonObject data)
		{
			data["crew"] = this.crew.ToJson();
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0003DA1A File Offset: 0x0003BC1A
		public override void LoadFromJson(JsonObject data)
		{
			this.crew = CrewMemberData.FromJson(data["crew"]);
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0003DA32 File Offset: 0x0003BC32
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.crewMembers.Add(this.crew);
			GamePlayer.current.currentSpaceShip.AssignCrewMember(this.crew);
		}

		// Token: 0x04000441 RID: 1089
		public CrewMemberData crew;
	}
}
