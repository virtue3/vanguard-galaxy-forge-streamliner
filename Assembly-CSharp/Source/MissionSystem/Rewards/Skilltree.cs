using System;
using Behaviour.Crew;
using LightJson;
using Source.Player;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000BC RID: 188
	public class Skilltree : MissionReward
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0003DEA8 File Offset: 0x0003C0A8
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly("@MissionRewardSkilltree", new object[]
				{
					Skilltree.Get(this.treeName).displayName
				});
			}
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x0003DECD File Offset: 0x0003C0CD
		public override void DataToJson(JsonObject data)
		{
			data["treeName"] = this.treeName;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x0003DEE5 File Offset: 0x0003C0E5
		public override void LoadFromJson(JsonObject data)
		{
			this.treeName = data["treeName"];
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0003DEFD File Offset: 0x0003C0FD
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.commander.UnlockSkilltree(this.treeName);
		}

		// Token: 0x0400044B RID: 1099
		public string treeName;
	}
}
