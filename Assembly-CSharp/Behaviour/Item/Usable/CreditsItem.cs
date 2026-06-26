using System;
using Behaviour.UI;
using Behaviour.UI.Spacestation;
using LightJson;
using Source.Player;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200030D RID: 781
	public class CreditsItem : UsableItem
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x000AFDA8 File Offset: 0x000ADFA8
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x000AFDB0 File Offset: 0x000ADFB0
		public int amount { get; private set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x000AFDB9 File Offset: 0x000ADFB9
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000AFDBC File Offset: 0x000ADFBC
		public override void DataFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000AFDD4 File Offset: 0x000ADFD4
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x06001D5E RID: 7518 RVA: 0x000AFDF4 File Offset: 0x000ADFF4
		public override bool OnUse()
		{
			GamePlayer.current.credits += (long)this.amount;
			Register.AddCounter("CreditsGained", this.amount, 0);
			GamePlayer.current.AddAutopilotStat(IdleStat.Credits, this.amount);
			if (!SpaceStationInterior.instance)
			{
				UIInfoTextParent instance = UIInfoTextParent.instance;
				if (instance != null)
				{
					instance.ShowPickupText("@UICredits", this.amount);
				}
			}
			return true;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000AFE64 File Offset: 0x000AE064
		public void SetCredits(int amount)
		{
			this.amount = amount;
		}
	}
}
