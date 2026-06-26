using System;
using LightJson;
using Source.Player;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B3 RID: 179
	public class Credits : MissionReward
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0003D95A File Offset: 0x0003BB5A
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly("@MissionRewardCredits", new object[]
				{
					this.amount
				});
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0003D97A File Offset: 0x0003BB7A
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0003D998 File Offset: 0x0003BB98
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0003D9B0 File Offset: 0x0003BBB0
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.credits += (long)this.amount;
			Register.AddCounter("CreditsGained", this.amount, 0);
			GamePlayer.current.AddAutopilotStat(IdleStat.Credits, this.amount);
		}

		// Token: 0x04000440 RID: 1088
		public int amount;
	}
}
