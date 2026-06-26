using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000174 RID: 372
	public static class StationMining
	{
		// Token: 0x06000DF1 RID: 3569 RVA: 0x00065158 File Offset: 0x00063358
		public static string GenerateStationName()
		{
			switch (StationMining.rng.Next(0, 3))
			{
			case 0:
				return StationMining.IndustrialCompoundName() + " Station";
			case 1:
				return StationMining.MilitaryCodeName();
			case 2:
				return StationMining.NamedStation();
			default:
				return "Unnamed Station";
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x000651A8 File Offset: 0x000633A8
		private static string IndustrialCompoundName()
		{
			string str = StationMining.industrialPrefixes[StationMining.rng.Next(StationMining.industrialPrefixes.Length)];
			string str2 = StationMining.industrialSuffixes[StationMining.rng.Next(StationMining.industrialSuffixes.Length)];
			return str + str2;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x000651EC File Offset: 0x000633EC
		private static string MilitaryCodeName()
		{
			string arg = StationMining.alphanumericDesignators[StationMining.rng.Next(StationMining.alphanumericDesignators.Length)];
			int num = StationMining.rng.Next(10, 100);
			string arg2 = StationMining.codenames[StationMining.rng.Next(StationMining.codenames.Length)];
			return string.Format("{0}-{1} \"{2}\"", arg, num, arg2);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0006524C File Offset: 0x0006344C
		private static string NamedStation()
		{
			string str = StationMining.IndustrialCompoundName();
			string str2 = StationMining.modifiers[StationMining.rng.Next(StationMining.modifiers.Length)];
			return str + " " + str2;
		}

		// Token: 0x040007BA RID: 1978
		private static Random rng = new Random();

		// Token: 0x040007BB RID: 1979
		private static string[] industrialPrefixes = new string[]
		{
			"Forge",
			"Core",
			"Drill",
			"Crag",
			"Iron",
			"Ore",
			"Grav",
			"Mine",
			"Quarry",
			"Dust",
			"Slag",
			"Rock",
			"Smelt",
			"Crush",
			"Hammer"
		};

		// Token: 0x040007BC RID: 1980
		private static string[] industrialSuffixes = new string[]
		{
			"hold",
			"spire",
			"jack",
			"site",
			"point",
			"hub",
			"pit",
			"base",
			"stack",
			"forge",
			"vault",
			"stone"
		};

		// Token: 0x040007BD RID: 1981
		private static string[] alphanumericDesignators = new string[]
		{
			"MSF",
			"DMS",
			"OPS",
			"RIG",
			"EX",
			"STN",
			"MXR",
			"CMP"
		};

		// Token: 0x040007BE RID: 1982
		private static string[] codenames = new string[]
		{
			"Ironhook",
			"Dustwell",
			"Deepbore",
			"Gravlock",
			"Cragfang",
			"Stonebite",
			"Slaghand",
			"Redvein",
			"Gearcrank",
			"Hardsink"
		};

		// Token: 0x040007BF RID: 1983
		private static string[] modifiers = new string[]
		{
			"Alpha",
			"Beta",
			"Gamma",
			"Delta",
			"Epsilon",
			"IV",
			"VII",
			"IX",
			"Prime",
			"Secundus"
		};
	}
}
