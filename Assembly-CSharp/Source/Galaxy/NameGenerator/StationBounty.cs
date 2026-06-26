using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000170 RID: 368
	public class StationBounty
	{
		// Token: 0x06000DE7 RID: 3559 RVA: 0x0006473C File Offset: 0x0006293C
		public static string GenerateOrsanonStationName()
		{
			string newValue = StationBounty.roughTerms[StationBounty.rng.Next(StationBounty.roughTerms.Length)];
			string newValue2 = StationBounty.structureTypes[StationBounty.rng.Next(StationBounty.structureTypes.Length)];
			string newValue3 = StationBounty.modifiers[StationBounty.rng.Next(StationBounty.modifiers.Length)];
			return StationBounty.patterns[StationBounty.rng.Next(StationBounty.patterns.Length)].Replace("{term}", newValue).Replace("{structure}", newValue2).Replace("{modifier}", newValue3);
		}

		// Token: 0x040007A4 RID: 1956
		private static Random rng = new Random();

		// Token: 0x040007A5 RID: 1957
		private static string[] roughTerms = new string[]
		{
			"Gunhold",
			"Crashpoint",
			"Dropgate",
			"Fringe",
			"Deadpoint",
			"Bullet Haven",
			"Dustlock",
			"Cutpoint",
			"Outrider",
			"Warrant Base"
		};

		// Token: 0x040007A6 RID: 1958
		private static string[] structureTypes = new string[]
		{
			"Hub",
			"Post",
			"Stack",
			"Node",
			"Platform",
			"Depot",
			"Outpost",
			"Base",
			"Arc",
			"Point"
		};

		// Token: 0x040007A7 RID: 1959
		private static string[] modifiers = new string[]
		{
			"Alpha",
			"Delta",
			"Echo",
			"ZK-13",
			"7B",
			"IX",
			"3A",
			"Prime",
			"Unit 22",
			"03",
			"Theta"
		};

		// Token: 0x040007A8 RID: 1960
		private static string[] patterns = new string[]
		{
			"{structure} {modifier}",
			"{term}",
			"ORS {structure} {modifier}",
			"{term} {structure}",
			"{term} {modifier}"
		};
	}
}
