using System;
using LightJson;
using Source.Player;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000BB RID: 187
	public class Skillpoint : MissionReward
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0003DDDC File Offset: 0x0003BFDC
		public override string rewardText
		{
			get
			{
				if (this.amount <= 1)
				{
					return Translation.TranslateOnly("@MissionRewardSkillpoint", new object[]
					{
						this.amount
					});
				}
				return Translation.TranslateOnly("@MissionRewardSkillpoints", new object[]
				{
					this.amount
				});
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0003DE2F File Offset: 0x0003C02F
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0003DE4D File Offset: 0x0003C04D
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0003DE65 File Offset: 0x0003C065
		public override void OnComplete(Mission m)
		{
			if (!GamePlayer.current.commander.TryGiveBonusSkillPoints(this.amount, false))
			{
				GamePlayer.current.currentSpaceShip.AddCargo("BonusSkillPointTemplate", this.amount, true);
			}
		}

		// Token: 0x0400044A RID: 1098
		public int amount;
	}
}
