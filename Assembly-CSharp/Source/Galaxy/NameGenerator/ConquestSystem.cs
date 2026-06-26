using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x0200016B RID: 363
	public static class ConquestSystem
	{
		// Token: 0x06000DD9 RID: 3545 RVA: 0x00063874 File Offset: 0x00061A74
		public static string GenerateConquestSystemName()
		{
			string newValue = ConquestSystem.violentWords[ConquestSystem.rng.Next(ConquestSystem.violentWords.Length)];
			string newValue2 = ConquestSystem.systemNouns[ConquestSystem.rng.Next(ConquestSystem.systemNouns.Length)];
			string newValue3 = ConquestSystem.tacticalDesignators[ConquestSystem.rng.Next(ConquestSystem.tacticalDesignators.Length)];
			return ConquestSystem.patterns[ConquestSystem.rng.Next(ConquestSystem.patterns.Length)].Replace("{violent}", newValue).Replace("{noun}", newValue2).Replace("{designator}", newValue3);
		}

		// Token: 0x0400078B RID: 1931
		private static Random rng = new Random();

		// Token: 0x0400078C RID: 1932
		private static string[] violentWords = new string[]
		{
			"Ash",
			"Crimson",
			"Broken",
			"Burning",
			"Wounded",
			"Red",
			"Silent",
			"Iron",
			"Black",
			"Veiled",
			"Grave",
			"Fallen",
			"Scorched",
			"Shattered",
			"Blood",
			"Bleak",
			"Pale",
			"Ashen",
			"Lonely",
			"Reclaimed",
			"Redacted",
			"Fallow",
			"Deserted",
			"Isolated",
			"Felled"
		};

		// Token: 0x0400078D RID: 1933
		private static string[] systemNouns = new string[]
		{
			"Drift",
			"Hold",
			"Rift",
			"Gate",
			"Veil",
			"Field",
			"Reach",
			"Front",
			"Stronghold",
			"Hollow",
			"Spine",
			"Cradle",
			"Fall",
			"Zone",
			"Spire",
			"Moor",
			"Spine",
			"Mire",
			"Marsh",
			"Quagmire",
			"Bog"
		};

		// Token: 0x0400078E RID: 1934
		private static string[] tacticalDesignators = new string[]
		{
			"XR",
			"Delta",
			"Kappa",
			"Zone",
			"Tertius",
			"Secundus",
			"Theta",
			"93-B",
			"Prime",
			"IX",
			"7K",
			"Omega",
			"13A",
			"11-2",
			"99",
			"Eterna",
			"Lux",
			"Umbra",
			"Mars",
			"Ares",
			"Sigma",
			"Ultima",
			"Omnis"
		};

		// Token: 0x0400078F RID: 1935
		private static string[] patterns = new string[]
		{
			"{violent} {noun}",
			"The {violent} {noun}",
			"{noun} {designator}",
			"{violent} {noun} {designator}"
		};
	}
}
