using System;
using LightJson;
using Source.Player;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200030E RID: 782
	public class CrewPodItem : UsableItem
	{
		// Token: 0x06001D61 RID: 7521 RVA: 0x000AFE78 File Offset: 0x000AE078
		public override bool OnUse()
		{
			int num = GamePlayer.current.currentSpaceShip.AddCrew(this.crewType, this.crewAmount);
			if (num > 0)
			{
				this.crewAmount = num;
				return false;
			}
			return true;
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000AFEAF File Offset: 0x000AE0AF
		public override void DataToJson(JsonObject data)
		{
			data["crewType"] = this.crewType.ToString();
			data["crewAmount"] = new double?((double)this.crewAmount);
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000AFEE8 File Offset: 0x000AE0E8
		public override void DataFromJson(JsonObject data)
		{
			if (!data["crewType"].IsNull)
			{
				this.crewType = data["crewType"];
			}
			this.crewAmount = data["crewAmount"];
		}

		// Token: 0x040011FD RID: 4605
		public string crewType;

		// Token: 0x040011FE RID: 4606
		public int crewAmount;
	}
}
