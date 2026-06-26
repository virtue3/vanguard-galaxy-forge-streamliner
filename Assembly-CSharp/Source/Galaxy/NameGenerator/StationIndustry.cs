using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000171 RID: 369
	public static class StationIndustry
	{
		// Token: 0x06000DEA RID: 3562 RVA: 0x00064940 File Offset: 0x00062B40
		public static string GenerateForgeIndustryStationName()
		{
			string newValue = StationIndustry.industrialNouns[StationIndustry.rng.Next(StationIndustry.industrialNouns.Length)];
			string newValue2 = StationIndustry.materials[StationIndustry.rng.Next(StationIndustry.materials.Length)];
			string newValue3 = StationIndustry.modifiers[StationIndustry.rng.Next(StationIndustry.modifiers.Length)];
			string newValue4 = StationIndustry.brandedPrefixes[StationIndustry.rng.Next(StationIndustry.brandedPrefixes.Length)];
			return StationIndustry.patterns[StationIndustry.rng.Next(StationIndustry.patterns.Length)].Replace("{noun}", newValue).Replace("{material}", newValue2).Replace("{mod}", newValue3).Replace("{prefix}", newValue4);
		}

		// Token: 0x040007A9 RID: 1961
		private static Random rng = new Random();

		// Token: 0x040007AA RID: 1962
		private static string[] industrialNouns = new string[]
		{
			"Refinery",
			"Smelter",
			"Foundry",
			"Processing",
			"Forge",
			"Crucible",
			"Core",
			"Anchor",
			"Stack",
			"Arc",
			"Platform",
			"Node",
			"Bastion",
			"Spire"
		};

		// Token: 0x040007AB RID: 1963
		private static string[] materials = new string[]
		{
			"Iron",
			"Steel",
			"Copper",
			"Ferron",
			"Titan",
			"Alloy",
			"Ore",
			"Carbon",
			"Cobalt",
			"Voltan",
			"Nickel",
			"Plasteel"
		};

		// Token: 0x040007AC RID: 1964
		private static string[] modifiers = new string[]
		{
			"Alpha",
			"Delta",
			"Zeta",
			"IX",
			"Prime",
			"08",
			"22",
			"3K",
			"Gamma",
			"Sigma",
			"Nexus",
			"Echo",
			"Theta",
			"114"
		};

		// Token: 0x040007AD RID: 1965
		private static string[] brandedPrefixes = new string[]
		{
			"FI-",
			"Forge-",
			"FRG-",
			"INDX-",
			"Stack-",
			"Iron-",
			"OPS-"
		};

		// Token: 0x040007AE RID: 1966
		private static string[] patterns = new string[]
		{
			"{material} {noun}",
			"{noun} {mod}",
			"{noun} of {material}",
			"{prefix}{noun} {mod}",
			"{material} {mod}",
			"{material} {noun} {mod}"
		};
	}
}
