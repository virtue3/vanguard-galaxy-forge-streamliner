using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x0200016A RID: 362
	public static class CombatArea
	{
		// Token: 0x06000DD7 RID: 3543 RVA: 0x00063664 File Offset: 0x00061864
		public static string GenerateCombatAreaName()
		{
			string newValue = CombatArea.combatAdjectives[CombatArea.rng.Next(CombatArea.combatAdjectives.Length)];
			string newValue2 = CombatArea.combatNouns[CombatArea.rng.Next(CombatArea.combatNouns.Length)];
			string newValue3 = CombatArea.modifiers[CombatArea.rng.Next(CombatArea.modifiers.Length)];
			return CombatArea.patterns[CombatArea.rng.Next(CombatArea.patterns.Length)].Replace("{adj}", newValue).Replace("{noun}", newValue2).Replace("{mod}", newValue3);
		}

		// Token: 0x04000786 RID: 1926
		private static Random rng = new Random();

		// Token: 0x04000787 RID: 1927
		private static string[] combatAdjectives = new string[]
		{
			"Burning",
			"Red",
			"Wrecked",
			"Bleeding",
			"Fractured",
			"Dead",
			"Hostile",
			"Iron",
			"Lost",
			"Shadow"
		};

		// Token: 0x04000788 RID: 1928
		private static string[] combatNouns = new string[]
		{
			"Zone",
			"Vector",
			"Field",
			"Orbit",
			"Grid",
			"Sector",
			"Line",
			"Crossroads",
			"Gate",
			"Corridor",
			"Wreckfield",
			"Killzone",
			"Maw",
			"Trap"
		};

		// Token: 0x04000789 RID: 1929
		private static string[] modifiers = new string[]
		{
			"Alpha",
			"Theta",
			"Echo",
			"IX",
			"7K",
			"13",
			"Omega",
			"ZK",
			"22",
			"Delta",
			"404"
		};

		// Token: 0x0400078A RID: 1930
		private static string[] patterns = new string[]
		{
			"{adj} {noun} {mod}",
			"{adj} {noun}",
			"{noun} {mod}"
		};
	}
}
