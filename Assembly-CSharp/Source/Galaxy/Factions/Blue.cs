using System;
using Source.Galaxy.NameGenerator;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200017A RID: 378
	public class Blue : Faction
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x00065ECC File Offset: 0x000640CC
		public Blue()
		{
			this.conquestColor = "#0004FF".HexToColor();
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

		// Token: 0x06000E07 RID: 3591 RVA: 0x00065FC6 File Offset: 0x000641C6
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationStellar.GenerateStellarStationName();
		}
	}
}
