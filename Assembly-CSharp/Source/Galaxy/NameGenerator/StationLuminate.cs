using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000173 RID: 371
	public static class StationLuminate
	{
		// Token: 0x06000DEF RID: 3567 RVA: 0x00064E98 File Offset: 0x00063098
		public static string GenerateLuminateStationName()
		{
			int num = StationLuminate.rng.Next(0, 4);
			string text = StationLuminate.spiritualTechPrefixes[StationLuminate.rng.Next(StationLuminate.spiritualTechPrefixes.Length)];
			string text2 = StationLuminate.technicalTypes[StationLuminate.rng.Next(StationLuminate.technicalTypes.Length)];
			string text3 = StationLuminate.modifiers[StationLuminate.rng.Next(StationLuminate.modifiers.Length)];
			string str = StationLuminate.exaltedWords[StationLuminate.rng.Next(StationLuminate.exaltedWords.Length)];
			switch (num)
			{
			case 0:
				return string.Concat(new string[]
				{
					text,
					" ",
					text2,
					" ",
					text3
				});
			case 1:
				return str + " " + text2;
			case 2:
				return text + " " + text2;
			case 3:
				return str + " " + text3;
			default:
				return "Unnamed Facility";
			}
		}

		// Token: 0x040007B5 RID: 1973
		private static Random rng = new Random();

		// Token: 0x040007B6 RID: 1974
		private static string[] spiritualTechPrefixes = new string[]
		{
			"Sanctum",
			"Concordance",
			"Echelon",
			"Solace",
			"Ascendant",
			"Vault",
			"Chorus",
			"Synchrony",
			"Hymn",
			"Vision",
			"Echo",
			"Divine",
			"Continuum",
			"Halo"
		};

		// Token: 0x040007B7 RID: 1975
		private static string[] technicalTypes = new string[]
		{
			"Node",
			"Core",
			"Spire",
			"Annex",
			"Relay",
			"Array",
			"Hub",
			"Stack",
			"Gate",
			"Platform",
			"Cluster",
			"Terminal"
		};

		// Token: 0x040007B8 RID: 1976
		private static string[] modifiers = new string[]
		{
			"Alpha",
			"Theta",
			"Delta",
			"Sigma",
			"VII",
			"XIV",
			"99",
			"17",
			"08",
			"Prime",
			"Nexus",
			"1A"
		};

		// Token: 0x040007B9 RID: 1977
		private static string[] exaltedWords = new string[]
		{
			"Transcension",
			"Luminae",
			"The Oracle",
			"Unity Pathway",
			"Divine Insight",
			"Mirrored Logic",
			"The Merge",
			"Apotheosis",
			"Glass Revelation",
			"Omnisync"
		};
	}
}
