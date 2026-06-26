using System;
using Source.Galaxy.NameGenerator;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200017E RID: 382
	public class Gold : Faction
	{
		// Token: 0x06000E0E RID: 3598 RVA: 0x00066168 File Offset: 0x00064368
		public Gold()
		{
			this.conquestColor = "#FFC61E".HexToColor();
			this.allowCrossFactionShipUse = false;
			base.missionTypes.Add("MineOre");
			base.missionTypes.Add("OreSamples");
			base.missionTypes.Add("DeliverCraftedGoods");
			base.missionTypes.Add("SalvageWreck");
			base.missionTypes.Add("SalvageSamples");
			base.missionTypes.Add("HelpMiner");
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("StationBattle");
			base.missionTypes.Add("Courier");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("EscortShip");
			base.missionTypes.Add("TradeTerminal");
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00066262 File Offset: 0x00064462
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationLuminate.GenerateLuminateStationName();
		}
	}
}
