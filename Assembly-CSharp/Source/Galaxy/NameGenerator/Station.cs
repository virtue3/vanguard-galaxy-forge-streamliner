using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x0200016F RID: 367
	public static class Station
	{
		// Token: 0x06000DE5 RID: 3557 RVA: 0x0006450C File Offset: 0x0006270C
		public static string GenerateStationName()
		{
			string text = Station.classicFormats[Station.rng.Next(Station.classicFormats.Length)];
			string newValue = Station.neutralNames[Station.rng.Next(Station.neutralNames.Length)];
			string newValue2 = Station.generalTypes[Station.rng.Next(Station.generalTypes.Length)];
			string newValue3 = Station.modifiers[Station.rng.Next(Station.modifiers.Length)];
			return text.Replace("{name}", newValue).Replace("{type}", newValue2).Replace("{mod}", newValue3);
		}

		// Token: 0x0400079F RID: 1951
		private static Random rng = new Random();

		// Token: 0x040007A0 RID: 1952
		private static string[] neutralNames = new string[]
		{
			"Astra",
			"Novark",
			"Velion",
			"Threxos",
			"Kellan",
			"Zentari",
			"Orun",
			"Miral",
			"Daelis",
			"Vorrin",
			"Selka",
			"Xantor",
			"Tirros"
		};

		// Token: 0x040007A1 RID: 1953
		private static string[] generalTypes = new string[]
		{
			"Station",
			"Platform",
			"Spire",
			"Point",
			"Base",
			"Node",
			"Arc",
			"Tower",
			"Anchor",
			"Ring",
			"Outpost",
			"Array"
		};

		// Token: 0x040007A2 RID: 1954
		private static string[] modifiers = new string[]
		{
			"I",
			"IV",
			"V",
			"IX",
			"X",
			"Prime",
			"Alpha",
			"Beta",
			"Delta",
			"Echelon",
			"Sector 7",
			"Post-44"
		};

		// Token: 0x040007A3 RID: 1955
		private static string[] classicFormats = new string[]
		{
			"{name} {type}",
			"{type} {name}",
			"{name} {type} {mod}",
			"{type} {mod}",
			"{name} {mod}"
		};
	}
}
