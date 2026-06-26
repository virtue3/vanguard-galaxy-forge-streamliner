using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x0200016C RID: 364
	public static class EmptySpace
	{
		// Token: 0x06000DDB RID: 3547 RVA: 0x00063BBC File Offset: 0x00061DBC
		public static string GenerateEmptySpaceName()
		{
			string newValue = EmptySpace.adjectives[EmptySpace.rng.Next(EmptySpace.adjectives.Length)];
			string newValue2 = EmptySpace.nouns[EmptySpace.rng.Next(EmptySpace.nouns.Length)];
			return EmptySpace.patterns[EmptySpace.rng.Next(EmptySpace.patterns.Length)].Replace("{adj}", newValue).Replace("{noun}", newValue2);
		}

		// Token: 0x04000790 RID: 1936
		private static Random rng = new Random();

		// Token: 0x04000791 RID: 1937
		private static string[] adjectives = new string[]
		{
			"Cold",
			"Dark",
			"Lonely",
			"Silent",
			"Empty",
			"Desolate",
			"Echoing",
			"Forgotten",
			"Null",
			"Endless",
			"Wailing",
			"Veiled",
			"Flickering",
			"Hollow",
			"Fractured",
			"Lost",
			"Drifting",
			"Uncharted",
			"Quiet",
			"Bleak"
		};

		// Token: 0x04000792 RID: 1938
		private static string[] nouns = new string[]
		{
			"Void",
			"Reach",
			"Abyss",
			"Sector",
			"Frontier",
			"Wake",
			"Locus",
			"Fold",
			"Channel",
			"Expanse",
			"Rift",
			"Anomaly",
			"Field",
			"Bend",
			"Corridor",
			"Stretch",
			"Shell",
			"Margin",
			"Scar",
			"Depths"
		};

		// Token: 0x04000793 RID: 1939
		private static string[] patterns = new string[]
		{
			"{adj} {noun}",
			"Uncharted {noun}"
		};
	}
}
