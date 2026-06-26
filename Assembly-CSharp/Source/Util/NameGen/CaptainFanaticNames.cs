using System;

namespace Source.Util.NameGen
{
	// Token: 0x0200004E RID: 78
	public static class CaptainFanaticNames
	{
		// Token: 0x0600031D RID: 797 RVA: 0x000191C8 File Offset: 0x000173C8
		public static string GenerateChosenName(SeededRandom random)
		{
			int num = random.RandomRange(0, 3);
			string text = random.Choose<string>(CaptainFanaticNames.firstNames);
			string text2 = random.Choose<string>(CaptainFanaticNames.surnames);
			string text3 = random.Choose<string>(CaptainFanaticNames.fanaticTitles);
			string text4 = random.Choose<string>(CaptainFanaticNames.corruptNicknames);
			string str = random.Choose<string>(CaptainFanaticNames.originClaims);
			switch (num)
			{
			case 0:
				return string.Concat(new string[]
				{
					text3,
					" ",
					text,
					" ",
					text2
				});
			case 1:
				return string.Concat(new string[]
				{
					text,
					" \"",
					text4,
					"\" ",
					text2
				});
			case 2:
				return text4 + " " + str;
			default:
				return "Unbound Chosen";
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00019295 File Offset: 0x00017495
		public static string GetRandomFirstName(SeededRandom random = null)
		{
			return (random ?? SeededRandom.Global).Choose<string>(CaptainFanaticNames.firstNames);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000192AB File Offset: 0x000174AB
		public static string GetRandomLastName(SeededRandom random = null)
		{
			return (random ?? SeededRandom.Global).Choose<string>(CaptainFanaticNames.surnames);
		}

		// Token: 0x040001B0 RID: 432
		private static readonly string[] firstNames = new string[]
		{
			"Serin",
			"Malek",
			"Yvora",
			"Dreyl",
			"Ostren",
			"Vaelis",
			"Kyran",
			"Nerith",
			"Aural",
			"Vesk",
			"Jhyra",
			"Osryn",
			"Thalec",
			"Merun",
			"Sorynn",
			"Variel",
			"Tareth",
			"Nylae",
			"Astrix",
			"Zyren",
			"Kaelis",
			"Lyric",
			"Orynn",
			"Tavrel",
			"Calyth",
			"Veyra",
			"Jorik",
			"Sylas",
			"Ravyn",
			"Xyler",
			"Tavik",
			"Althar",
			"Zerith"
		};

		// Token: 0x040001B1 RID: 433
		private static readonly string[] surnames = new string[]
		{
			"Kaelor",
			"Vireth",
			"Solkar",
			"Nemeris",
			"Draxil",
			"Ythar",
			"Selvek",
			"Morath",
			"Eslin",
			"Tharan",
			"Velkor",
			"Drayen",
			"Sarvak",
			"Zoryn",
			"Kavros",
			"Tyrinth",
			"Veldor",
			"Xarven",
			"Dravik",
			"Morys",
			"Soryn",
			"Thalven",
			"Ravok",
			"Calyth",
			"Voryn",
			"Zerik",
			"Jorvik"
		};

		// Token: 0x040001B2 RID: 434
		private static readonly string[] fanaticTitles = new string[]
		{
			"Ascendant",
			"Harbinger",
			"Voice of Meridia",
			"Purifier",
			"Chosen Vessel",
			"High Conduit",
			"Transmutant",
			"Sanctifier",
			"Bearer of the Fiber",
			"Prime Initiate"
		};

		// Token: 0x040001B3 RID: 435
		private static readonly string[] corruptNicknames = new string[]
		{
			"The Woven",
			"Fiberbound",
			"Mindsplice",
			"Hollowlight",
			"Soul-Threaded",
			"Latticeborn",
			"The Knitted Flesh",
			"Iron-Veined",
			"Bound to Meridia",
			"The Resonant",
			"Pulse-Silenced",
			"Silkheart",
			"Graft-Bearer"
		};

		// Token: 0x040001B4 RID: 436
		private static readonly string[] originClaims = new string[]
		{
			"of the Lost Meridian Verge",
			"from the Silent Cradle",
			"of the Machine Choir",
			"from the Broken Expanse",
			"of the Forsaken Lineage",
			"from the Biolith Chambers",
			"of the First Threading",
			"from Meridian Exile",
			"of the Lattice Communion",
			"from the Sanctum of Threads"
		};
	}
}
