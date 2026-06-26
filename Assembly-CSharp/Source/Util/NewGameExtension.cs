using System;
using Source.Crew;

namespace Source.Util
{
	// Token: 0x02000039 RID: 57
	public static class NewGameExtension
	{
		// Token: 0x060002AA RID: 682 RVA: 0x000166DC File Offset: 0x000148DC
		public static string GetDisplayName(this PersonalHistory rarity)
		{
			switch (rarity)
			{
			case PersonalHistory.Miner:
				return "Prospector";
			case PersonalHistory.NavyCaptain:
				return "Navy Officer";
			case PersonalHistory.Salvaging:
				return "Wreckage Explorer";
			case PersonalHistory.Hauler:
				return "Cargo Runner";
			case PersonalHistory.BountyHunter:
				return "Contract Specialist";
			case PersonalHistory.Pirate:
				return "Raider";
			default:
				return "";
			}
		}
	}
}
