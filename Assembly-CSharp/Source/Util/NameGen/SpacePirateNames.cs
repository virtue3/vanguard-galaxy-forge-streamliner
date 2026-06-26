using System;

namespace Source.Util.NameGen
{
	// Token: 0x02000052 RID: 82
	public static class SpacePirateNames
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0001BAA4 File Offset: 0x00019CA4
		public static string GeneratePirateName(SeededRandom random)
		{
			int num = random.RandomRange(0, 3);
			string text = random.Choose<string>(SpacePirateNames.firstNames);
			string text2 = random.Choose<string>(SpacePirateNames.surnames);
			string text3 = random.Choose<string>(SpacePirateNames.pirateTitles);
			string text4 = random.Choose<string>(SpacePirateNames.nicknames);
			string str = random.Choose<string>(SpacePirateNames.regionsOrClaims);
			switch (num)
			{
			case 0:
				return string.Concat(new string[]
				{
					text3,
					" ",
					text,
					" \"",
					text4,
					"\" ",
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
				return "Unnamed Corsair";
			}
		}

		// Token: 0x040001D1 RID: 465
		private static string[] firstNames = new string[]
		{
			"Vorn",
			"Ash",
			"Jex",
			"Talon",
			"Rex",
			"Nyra",
			"Kess",
			"Mira",
			"Silas",
			"Drake",
			"Varga",
			"Straxx",
			"Thorne",
			"Vel",
			"Cors",
			"Laz",
			"Kelso",
			"Juno",
			"Xen"
		};

		// Token: 0x040001D2 RID: 466
		private static string[] surnames = new string[]
		{
			"Kass",
			"Drake",
			"Raine",
			"Mord",
			"Tessar",
			"Flint",
			"Grimm",
			"Sable",
			"Tark",
			"Hale",
			"Rook",
			"Vox",
			"Kael",
			"Zarrin"
		};

		// Token: 0x040001D3 RID: 467
		private static string[] pirateTitles = new string[]
		{
			"Captain",
			"Commodore",
			"Corsair",
			"Privateer",
			"Raider",
			"Baron",
			"Warlord"
		};

		// Token: 0x040001D4 RID: 468
		private static string[] nicknames = new string[]
		{
			"Voidbeard",
			"Deadspin",
			"Red Sun",
			"Gravetooth",
			"Starburn",
			"Iron Widow",
			"Darkwake",
			"Pulsefang",
			"Jetscar",
			"Black Thruster",
			"Gutterwind",
			"Screamline"
		};

		// Token: 0x040001D5 RID: 469
		private static string[] regionsOrClaims = new string[]
		{
			"of the Kuros Verge",
			"of Delta-13",
			"from the Ebon Drift",
			"of the Starlight Maw",
			"from Sector Theta",
			"of the Outer Spur",
			"from Voidgate",
			"of Helion Wastes",
			"from Titan Break",
			"of the Anax Line"
		};
	}
}
