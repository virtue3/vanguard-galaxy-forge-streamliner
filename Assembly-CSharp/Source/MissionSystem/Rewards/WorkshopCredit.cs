using System;
using LightJson;
using Source.Player;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000BF RID: 191
	public class WorkshopCredit : MissionReward
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0003E08A File Offset: 0x0003C28A
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly("@MissionRewardWorkshopCredit", new object[]
				{
					this.amount
				});
			}
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0003E0AA File Offset: 0x0003C2AA
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0003E0C8 File Offset: 0x0003C2C8
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0003E0E0 File Offset: 0x0003C2E0
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.AddSalvageShopCredit(this.amount, 0, false);
			Register.AddCounter("CreditsGained", this.amount, 0);
		}

		// Token: 0x0400044E RID: 1102
		public int amount;
	}
}
