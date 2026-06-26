using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x02000175 RID: 373
	public class StationPolice
	{
		// Token: 0x06000DF6 RID: 3574 RVA: 0x0006549C File Offset: 0x0006369C
		public static string GenerateCanisecStationName()
		{
			string newValue = StationPolice.virtueTerms[StationPolice.rng.Next(StationPolice.virtueTerms.Length)];
			string newValue2 = StationPolice.stationTypes[StationPolice.rng.Next(StationPolice.stationTypes.Length)];
			string newValue3 = StationPolice.modifiers[StationPolice.rng.Next(StationPolice.modifiers.Length)];
			string newValue4 = StationPolice.callSigns[StationPolice.rng.Next(StationPolice.callSigns.Length)];
			return StationPolice.formatPatterns[StationPolice.rng.Next(StationPolice.formatPatterns.Length)].Replace("{virtue}", newValue).Replace("{type}", newValue2).Replace("{mod}", newValue3).Replace("{call}", newValue4);
		}

		// Token: 0x040007C0 RID: 1984
		private static Random rng = new Random();

		// Token: 0x040007C1 RID: 1985
		private static string[] virtueTerms = new string[]
		{
			"Justice",
			"Order",
			"Integrity",
			"Vigil",
			"Pax",
			"Law",
			"Shield",
			"Sentinel",
			"Unity",
			"Guardian"
		};

		// Token: 0x040007C2 RID: 1986
		private static string[] stationTypes = new string[]
		{
			"Node",
			"Spire",
			"Platform",
			"Core",
			"Station",
			"Array",
			"Relay",
			"Hub",
			"Ops",
			"Watchpoint",
			"Bastion"
		};

		// Token: 0x040007C3 RID: 1987
		private static string[] modifiers = new string[]
		{
			"Alpha",
			"Theta",
			"Delta",
			"Sigma",
			"Prime",
			"22",
			"114",
			"IX",
			"7K",
			"Nexus",
			"Echo"
		};

		// Token: 0x040007C4 RID: 1988
		private static string[] callSigns = new string[]
		{
			"Canisec-114",
			"Ops Platform 7",
			"Node-22",
			"Watchpoint 3A",
			"Ops Relay Delta",
			"Sector Node 5"
		};

		// Token: 0x040007C5 RID: 1989
		private static string[] formatPatterns = new string[]
		{
			"{virtue} {type} {mod}",
			"{virtue} {mod}",
			"{call}",
			"{virtue} {type}",
			"{type} {mod}"
		};
	}
}
