using System;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200018B RID: 395
	public class Stranded : Faction
	{
		// Token: 0x06000E31 RID: 3633 RVA: 0x00066C28 File Offset: 0x00064E28
		public Stranded()
		{
			this.conquestColor = "#689900".HexToColor();
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
