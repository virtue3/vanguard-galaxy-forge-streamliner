using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000176 RID: 374
	public class StationSalvage
	{
		// Token: 0x06000DF9 RID: 3577 RVA: 0x00065704 File Offset: 0x00063904
		public static string GenerateVulturesStationName()
		{
			string newValue = StationSalvage.junkyDescriptors[StationSalvage.rng.Next(StationSalvage.junkyDescriptors.Length)];
			string newValue2 = StationSalvage.coreTerms[StationSalvage.rng.Next(StationSalvage.coreTerms.Length)];
			string newValue3 = StationSalvage.modifiers[StationSalvage.rng.Next(StationSalvage.modifiers.Length)];
			string newValue4 = StationSalvage.nicknames[StationSalvage.rng.Next(StationSalvage.nicknames.Length)];
			return StationSalvage.patterns[StationSalvage.rng.Next(StationSalvage.patterns.Length)].Replace("{descriptor}", newValue).Replace("{core}", newValue2).Replace("{modifier}", newValue3).Replace("{nickname}", newValue4);
		}

		// Token: 0x040007C6 RID: 1990
		private static Random rng = new Random();

		// Token: 0x040007C7 RID: 1991
		private static string[] coreTerms = new string[]
		{
			"Nest",
			"Roost",
			"Rig",
			"Spire",
			"Bastion",
			"Hold",
			"Hangar",
			"Stack",
			"Arc",
			"Hub",
			"Platform",
			"Core",
			"Heap"
		};

		// Token: 0x040007C8 RID: 1992
		private static string[] modifiers = new string[]
		{
			"7K",
			"09-B",
			"XIII",
			"Delta",
			"Prime",
			"Echo",
			"Ultima",
			"Zero",
			"Zeta",
			"4R",
			"IX",
			"13"
		};

		// Token: 0x040007C9 RID: 1993
		private static string[] junkyDescriptors = new string[]
		{
			"Scrap",
			"Rust",
			"Drift",
			"Junk",
			"Crush",
			"Grime",
			"Wire",
			"Crack",
			"Weld",
			"Clank"
		};

		// Token: 0x040007CA RID: 1994
		private static string[] nicknames = new string[]
		{
			"Vulture's Roost",
			"The Rattlehome",
			"Last Chance Hub",
			"Grinder's Rest",
			"Fort Opportunity",
			"The Hollow Fang"
		};

		// Token: 0x040007CB RID: 1995
		private static string[] patterns = new string[]
		{
			"{descriptor} {core}",
			"{core} {modifier}",
			"{nickname}",
			"{descriptor} {core} {modifier}",
			"{descriptor} {modifier}"
		};
	}
}
