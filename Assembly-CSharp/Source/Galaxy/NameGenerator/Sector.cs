using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x0200016D RID: 365
	public static class Sector
	{
		// Token: 0x06000DDD RID: 3549 RVA: 0x00063DC8 File Offset: 0x00061FC8
		public static string GenerateSubsectorName()
		{
			switch (Sector.rng.Next(0, 4))
			{
			case 0:
				return Sector.greekLetters[Sector.rng.Next(Sector.greekLetters.Length)] + " " + Sector.astroRegions[Sector.rng.Next(Sector.astroRegions.Length)];
			case 1:
				return Sector.scientificTerms[Sector.rng.Next(Sector.scientificTerms.Length)] + " " + Sector.astroRegions[Sector.rng.Next(Sector.astroRegions.Length)];
			case 2:
			{
				string arg = Sector.alphanumPrefixes[Sector.rng.Next(Sector.alphanumPrefixes.Length)];
				int num = Sector.rng.Next(10, 999);
				return string.Format("{0}-{1} {2}", arg, num, Sector.astroRegions[Sector.rng.Next(Sector.astroRegions.Length)]);
			}
			case 3:
				return Sector.techNames[Sector.rng.Next(Sector.techNames.Length)] + " " + Sector.astroRegions[Sector.rng.Next(Sector.astroRegions.Length)];
			default:
				return "Unnamed Subsector";
			}
		}

		// Token: 0x04000794 RID: 1940
		private static Random rng = new Random();

		// Token: 0x04000795 RID: 1941
		private static string[] greekLetters = new string[]
		{
			"Alpha",
			"Beta",
			"Gamma",
			"Delta",
			"Epsilon",
			"Zeta",
			"Theta",
			"Lambda",
			"Mu",
			"Omicron",
			"Sigma",
			"Tau",
			"Omega"
		};

		// Token: 0x04000796 RID: 1942
		private static string[] scientificTerms = new string[]
		{
			"Graviton",
			"Neutrino",
			"Helion",
			"Quasar",
			"Cepheid",
			"Pulsar",
			"Plasma",
			"Fusion",
			"Singularity",
			"Flux",
			"Spectra",
			"Magnetron",
			"Chroniton"
		};

		// Token: 0x04000797 RID: 1943
		private static string[] astroRegions = new string[]
		{
			"Field",
			"Expanse",
			"Drift",
			"Cluster",
			"Sector",
			"Reach",
			"Array",
			"Belt",
			"Zone",
			"Corridor",
			"Spur",
			"Rift",
			"Periphery"
		};

		// Token: 0x04000798 RID: 1944
		private static string[] alphanumPrefixes = new string[]
		{
			"XR",
			"UNS",
			"RSV",
			"ZK",
			"CY",
			"TRN",
			"AE",
			"OB",
			"GT",
			"LZ",
			"SYN"
		};

		// Token: 0x04000799 RID: 1945
		private static string[] techNames = new string[]
		{
			"Veltrax",
			"Xeradon",
			"Polaris",
			"Cenaris",
			"Orbex",
			"Norex",
			"Tarsys",
			"Meridius",
			"Axalon",
			"Kernis"
		};
	}
}
