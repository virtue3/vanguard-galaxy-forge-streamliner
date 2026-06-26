using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000178 RID: 376
	public static class StationTrade
	{
		// Token: 0x06000DFF RID: 3583 RVA: 0x00065BC0 File Offset: 0x00063DC0
		public static string GenerateStationName()
		{
			switch (StationTrade.rng.Next(0, 3))
			{
			case 0:
				return StationTrade.CommercialTitleName();
			case 1:
				return StationTrade.NamedFreeport();
			case 2:
				return StationTrade.NexusWithDesignator();
			default:
				return "Unnamed Hub";
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00065C08 File Offset: 0x00063E08
		private static string CommercialTitleName()
		{
			string str = StationTrade.commercialPrefixes[StationTrade.rng.Next(StationTrade.commercialPrefixes.Length)];
			string str2 = StationTrade.names[StationTrade.rng.Next(StationTrade.names.Length)];
			return str + " " + str2;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00065C50 File Offset: 0x00063E50
		private static string NamedFreeport()
		{
			string str = StationTrade.names[StationTrade.rng.Next(StationTrade.names.Length)];
			string str2 = StationTrade.commercialSuffixes[StationTrade.rng.Next(StationTrade.commercialSuffixes.Length)];
			return str + " " + str2;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00065C98 File Offset: 0x00063E98
		private static string NexusWithDesignator()
		{
			string str = StationTrade.commercialPrefixes[StationTrade.rng.Next(StationTrade.commercialPrefixes.Length)];
			string str2 = StationTrade.designators[StationTrade.rng.Next(StationTrade.designators.Length)];
			return str + " " + str2;
		}

		// Token: 0x040007D1 RID: 2001
		private static Random rng = new Random();

		// Token: 0x040007D2 RID: 2002
		private static string[] commercialPrefixes = new string[]
		{
			"Freeport",
			"Exchange",
			"Nexus",
			"Trade",
			"Market",
			"Port",
			"Bazaar",
			"Spindle",
			"Arc",
			"Waystation",
			"Concord",
			"Spire",
			"Node",
			"Nebula"
		};

		// Token: 0x040007D3 RID: 2003
		private static string[] commercialSuffixes = new string[]
		{
			"Prime",
			"Arc",
			"Terminal",
			"Spire",
			"Hub",
			"Ring",
			"Haven",
			"Delta",
			"Station",
			"Platform",
			"Concourse",
			"Port"
		};

		// Token: 0x040007D4 RID: 2004
		private static string[] names = new string[]
		{
			"Halix",
			"Tarkeen",
			"Venton",
			"Zharos",
			"Mirael",
			"Nyros",
			"Veltran",
			"Yalara",
			"Dekari",
			"Xenith",
			"Damaris",
			"Kelvon"
		};

		// Token: 0x040007D5 RID: 2005
		private static string[] designators = new string[]
		{
			"Alpha",
			"Beta",
			"Theta",
			"Sigma",
			"Gamma",
			"IV",
			"VI",
			"IX",
			"XII",
			"Omega"
		};
	}
}
