using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000172 RID: 370
	public class StationKolyatov
	{
		// Token: 0x06000DEC RID: 3564 RVA: 0x00064BF8 File Offset: 0x00062DF8
		public static string GenerateKolyatovStationName()
		{
			string newValue = StationKolyatov.ideologicalTerms[StationKolyatov.rng.Next(StationKolyatov.ideologicalTerms.Length)];
			string newValue2 = StationKolyatov.sovietStructures[StationKolyatov.rng.Next(StationKolyatov.sovietStructures.Length)];
			string newValue3 = StationKolyatov.numericalDesignators[StationKolyatov.rng.Next(StationKolyatov.numericalDesignators.Length)];
			string newValue4 = StationKolyatov.sloganPhrases[StationKolyatov.rng.Next(StationKolyatov.sloganPhrases.Length)];
			return StationKolyatov.patterns[StationKolyatov.rng.Next(StationKolyatov.patterns.Length)].Replace("{term}", newValue).Replace("{structure}", newValue2).Replace("{num}", newValue3).Replace("{slogan}", newValue4);
		}

		// Token: 0x040007AF RID: 1967
		private static Random rng = new Random();

		// Token: 0x040007B0 RID: 1968
		private static string[] ideologicalTerms = new string[]
		{
			"Unity",
			"Collective",
			"People's",
			"Worker's",
			"Labor",
			"Red Star",
			"Vanguard",
			"Forward",
			"Progress",
			"Solidarity",
			"Prosperity",
			"Comrade",
			"Reclamation"
		};

		// Token: 0x040007B1 RID: 1969
		private static string[] sovietStructures = new string[]
		{
			"Kolhoz",
			"Complex",
			"Bloc",
			"Node",
			"Platform",
			"Array",
			"Forge",
			"Spire",
			"Station",
			"Arc",
			"Nexus",
			"Command Base",
			"Installation"
		};

		// Token: 0x040007B2 RID: 1970
		private static string[] numericalDesignators = new string[]
		{
			"04",
			"17",
			"87",
			"Delta",
			"Sigma",
			"3K",
			"7B",
			"Zeta",
			"IX",
			"Prime",
			"22",
			"1A",
			"Secundus"
		};

		// Token: 0x040007B3 RID: 1971
		private static string[] sloganPhrases = new string[]
		{
			"Forge of the People",
			"Pillar of Progress",
			"Beacon of Labor",
			"Bastion of Unity",
			"Stronghold of the Vanguard",
			"Citadel of the Future"
		};

		// Token: 0x040007B4 RID: 1972
		private static string[] patterns = new string[]
		{
			"{term} {structure} {num}",
			"{term} {structure}",
			"{structure} {num}",
			"{slogan}"
		};
	}
}
