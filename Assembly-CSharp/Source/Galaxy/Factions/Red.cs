using System;
using Source.Galaxy.NameGenerator;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000188 RID: 392
	public class Red : Faction
	{
		// Token: 0x06000E2A RID: 3626 RVA: 0x00066A0C File Offset: 0x00064C0C
		public Red()
		{
			this.conquestColor = "#FF0400".HexToColor();
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

		// Token: 0x06000E2B RID: 3627 RVA: 0x00066B06 File Offset: 0x00064D06
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationKolyatov.GenerateKolyatovStationName();
		}
	}
}
