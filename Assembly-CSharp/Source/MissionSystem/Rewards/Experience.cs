using System;
using LightJson;
using Source.Player;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B5 RID: 181
	public class Experience : MissionReward
	{
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0003DA67 File Offset: 0x0003BC67
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly("@MissionRewardExperience", new object[]
				{
					this.amount
				});
			}
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x0003DA87 File Offset: 0x0003BC87
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x0003DAA5 File Offset: 0x0003BCA5
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x0003DABD File Offset: 0x0003BCBD
		public override void OnComplete(Mission m)
		{
			GamePlayer.current.GainExperience(this.amount);
		}

		// Token: 0x04000442 RID: 1090
		public int amount;
	}
}
