using System;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200017C RID: 380
	public class Darkspacers : Faction
	{
		// Token: 0x06000E0B RID: 3595 RVA: 0x00066060 File Offset: 0x00064260
		public Darkspacers()
		{
			this.conquestColor = ColorHelper.boringGrey;
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 1;
			base.missionTypes.Add("MineOre");
			base.missionTypes.Add("OreSamples");
			base.missionTypes.Add("DeliverCraftedGoods");
			base.missionTypes.Add("SalvageWreck");
			base.missionTypes.Add("SalvageSamples");
			base.missionTypes.Add("HelpMiner");
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("Courier");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("TradeTerminal");
		}
	}
}
