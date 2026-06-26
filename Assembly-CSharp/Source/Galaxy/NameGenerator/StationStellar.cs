using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000177 RID: 375
	public class StationStellar
	{
		// Token: 0x06000DFC RID: 3580 RVA: 0x00065988 File Offset: 0x00063B88
		public static string GenerateStellarStationName()
		{
			string newValue = StationStellar.legacyTerms[StationStellar.rng.Next(StationStellar.legacyTerms.Length)];
			string newValue2 = StationStellar.corpStructures[StationStellar.rng.Next(StationStellar.corpStructures.Length)];
			string newValue3 = StationStellar.corpModifiers[StationStellar.rng.Next(StationStellar.corpModifiers.Length)];
			return StationStellar.formatPatterns[StationStellar.rng.Next(StationStellar.formatPatterns.Length)].Replace("{legacy}", newValue).Replace("{structure}", newValue2).Replace("{modifier}", newValue3);
		}

		// Token: 0x040007CC RID: 1996
		private static Random rng = new Random();

		// Token: 0x040007CD RID: 1997
		private static string[] legacyTerms = new string[]
		{
			"Kepler",
			"Galaxis",
			"Axiom",
			"Celestis",
			"Stellar",
			"Novae",
			"Helios",
			"Peregrine",
			"Astrarch",
			"Primecore",
			"Tarsis"
		};

		// Token: 0x040007CE RID: 1998
		private static string[] corpStructures = new string[]
		{
			"Arcology",
			"Annex",
			"Core",
			"Bastion",
			"Holdings",
			"Tower",
			"Control Stack",
			"Directorate",
			"Node",
			"Spire",
			"Outpost",
			"Command Base",
			"Platform"
		};

		// Token: 0x040007CF RID: 1999
		private static string[] corpModifiers = new string[]
		{
			"Prime",
			"Theta",
			"XIV",
			"09",
			"3B",
			"Omega",
			"Delta",
			"Secundus",
			"Iota",
			"Subsection 4",
			"Unit 7",
			"Nexus",
			"Alpha"
		};

		// Token: 0x040007D0 RID: 2000
		private static string[] formatPatterns = new string[]
		{
			"{legacy} {structure}",
			"{structure} {modifier}",
			"{legacy} {structure} {modifier}",
			"{structure} {legacy}",
			"{legacy} Holdings {modifier}"
		};
	}
}
