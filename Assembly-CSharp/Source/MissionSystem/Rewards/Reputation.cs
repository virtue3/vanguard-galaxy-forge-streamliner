using System;
using LightJson;
using Source.Galaxy;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B9 RID: 185
	public class Reputation : MissionReward
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0003DC61 File Offset: 0x0003BE61
		public override string rewardText
		{
			get
			{
				string text = "@MissionRewardReputation";
				object[] array = new object[2];
				array[0] = this.amount;
				int num = 1;
				Faction faction = this.faction;
				array[num] = (((faction != null) ? faction.name : null) ?? "");
				return Translation.TranslateOnly(text, array);
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0003DCA0 File Offset: 0x0003BEA0
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
			if (this.faction != null)
			{
				data["faction"] = this.faction.identifier;
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0003DCEC File Offset: 0x0003BEEC
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
			if (data.ContainsKey("faction"))
			{
				this.faction = Faction.Get(data["faction"]);
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0003DD2C File Offset: 0x0003BF2C
		public override void OnComplete(Mission m)
		{
			(this.faction ?? m.sourceFaction).ChangePlayerReputation(this.amount);
		}

		// Token: 0x04000447 RID: 1095
		public int amount;

		// Token: 0x04000448 RID: 1096
		public Faction faction;
	}
}
