using System;
using System.Collections.Generic;
using Source.Crew;
using Source.Player;

namespace Source.Util
{
	// Token: 0x02000038 RID: 56
	public static class NameGenerator
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0001581C File Offset: 0x00013A1C
		public static void GiveCrewMemberRandomName(CrewMemberData cmd, SeededRandom random, string faction = null)
		{
			CommanderData commander = GamePlayer.current.commander;
			do
			{
				cmd.SetName(random.Choose<string>((cmd.gender == Gender.Male) ? NameGenerator.maleNames : NameGenerator.femaleNames), (faction != null) ? NameGenerator.GetFactionCallsign(faction, random) : NameGenerator.GetRandomCallsign(random), random.Choose<string>(NameGenerator.lastNames));
			}
			while (cmd.firstName == commander.firstName || cmd.callsign == commander.callsign || cmd.lastName == commander.lastName);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000158AA File Offset: 0x00013AAA
		public static string GetRandomFirstName(bool male, SeededRandom random = null)
		{
			return (random ?? SeededRandom.Global).Choose<string>(male ? NameGenerator.maleNames : NameGenerator.femaleNames);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000158CC File Offset: 0x00013ACC
		public static string GetRandomCallsign(SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			IReadOnlyList<string> anyCallsigns = SupporterCallsigns.AnyCallsigns;
			int hi = NameGenerator.callSigns.Count + anyCallsigns.Count;
			int num = random.RandomRange(0, hi);
			if (num >= NameGenerator.callSigns.Count)
			{
				return anyCallsigns[num - NameGenerator.callSigns.Count];
			}
			return NameGenerator.callSigns[num];
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00015930 File Offset: 0x00013B30
		public static string GetFactionCallsign(string faction, SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			IReadOnlyList<string> factionCallsigns = SupporterCallsigns.GetFactionCallsigns(faction);
			if (factionCallsigns != null)
			{
				float num = (float)(factionCallsigns.Count - 1) / 9f;
				if (num > 1f)
				{
					num = 1f;
				}
				float chanceOfTrue = 0.1f + num * 0.6f;
				if (random.RandomBool(chanceOfTrue))
				{
					return factionCallsigns[random.RandomRange(0, factionCallsigns.Count)];
				}
			}
			return NameGenerator.GetRandomCallsign(random);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000159A0 File Offset: 0x00013BA0
		public static string GetRandomLastName(SeededRandom random = null)
		{
			return (random ?? SeededRandom.Global).Choose<string>(NameGenerator.lastNames);
		}

		// Token: 0x04000171 RID: 369
		private static List<string> maleNames = new List<string>
		{
			"Alex",
			"Blake",
			"Casey",
			"Drew",
			"Evan",
			"Finley",
			"Gray",
			"Harper",
			"Jordan",
			"Kai",
			"Karl",
			"Logan",
			"Morgan",
			"Quinn",
			"Reese",
			"Sawyer",
			"Tatum",
			"Taylor",
			"Riley",
			"Charlie",
			"Samuel",
			"Emerson",
			"Hayden",
			"Peyton",
			"Avery",
			"Rowan",
			"Spencer",
			"Aiden",
			"Hunter",
			"Cameron",
			"Jesse",
			"Shane",
			"Sloan",
			"Zane",
			"Rory",
			"Cody",
			"Reed",
			"Jim",
			"Alexis",
			"Sloane",
			"Jaden",
			"Micah",
			"Parker",
			"Reagan",
			"Remy",
			"Rene",
			"Amos",
			"Remington",
			"Presley",
			"Shawn",
			"Tanner",
			"Aiden",
			"Tristan",
			"Dylan",
			"Emery",
			"Bobbie",
			"Thomas",
			"Andrew",
			"Horace",
			"Winston",
			"Paul",
			"Lenar",
			"Fedmahn",
			"Martin",
			"Sol",
			"Brawne",
			"Gaius",
			"William",
			"Lee",
			"Saul",
			"Felix"
		};

		// Token: 0x04000172 RID: 370
		private static List<string> femaleNames = new List<string>
		{
			"Alex",
			"Alice",
			"Casey",
			"Gray",
			"Harper",
			"Jordan",
			"Morgan",
			"Quinn",
			"Taylor",
			"Bailey",
			"Riley",
			"Charlie",
			"Dakota",
			"Skyler",
			"Avery",
			"Rowan",
			"Spencer",
			"Anastasia",
			"Cameron",
			"Jesse",
			"Phoenix",
			"Sage",
			"Rory",
			"Reed",
			"Julie",
			"Kara",
			"Blair",
			"Alexis",
			"Parker",
			"Remy",
			"Rene",
			"Presley",
			"Emery",
			"Camina",
			"Bobbie",
			"Michelle",
			"Eloise",
			"Sonja",
			"Emily",
			"Elizabeth",
			"Lenar",
			"Sol",
			"Brawne",
			"Lee",
			"Sharon",
			"Felix",
			"Cally",
			"Laura",
			"Helena"
		};

		// Token: 0x04000173 RID: 371
		private static readonly List<string> callSigns = new List<string>
		{
			"Viper",
			"Ghost",
			"Hawk",
			"Falcon",
			"Wolf",
			"Eagle",
			"Raven",
			"Phoenix",
			"Tiger",
			"Lion",
			"Panther",
			"Jaguar",
			"Cobra",
			"Scorpion",
			"Dragon",
			"Blade",
			"Shadow",
			"Raptor",
			"Maverick",
			"Rogue",
			"Hunter",
			"Bandit",
			"Vandal",
			"Venom",
			"Storm",
			"Blaze",
			"Bullet",
			"Thunder",
			"Lightning",
			"Tornado",
			"Reaper",
			"Warrior",
			"Valkyrie",
			"Nightmare",
			"Frost",
			"Rage",
			"Inferno",
			"Vortex",
			"Rampage",
			"Tempest",
			"Titan",
			"Sentinel",
			"Cyclone",
			"Gunner",
			"Sharpshooter",
			"Sniper",
			"Juggernaut",
			"Specter",
			"Saber",
			"Spartan",
			"Iceman",
			"Nebula",
			"Starbuck",
			"Apollo",
			"Boomer",
			"Archer",
			"Crossbow",
			"Longshot",
			"Marksman",
			"Assassin",
			"Gladiator",
			"Knight",
			"Warlock",
			"Phantom",
			"Ghost",
			"Salamander",
			"Predator",
			"Razor",
			"Sphinx",
			"Hydra",
			"Viper",
			"Scorpion",
			"Shadow",
			"Vortex",
			"Tempest",
			"Inferno"
		};

		// Token: 0x04000174 RID: 372
		private static List<string> lastNames = new List<string>
		{
			"Adama",
			"Armstrong",
			"Bennett",
			"Carson",
			"Davidson",
			"Ellis",
			"Foster",
			"Garrett",
			"Harrison",
			"Iverson",
			"Jenkins",
			"Drummer",
			"Draper",
			"Holden",
			"Burton",
			"Mao",
			"Nagata",
			"Harrington",
			"Baltar",
			"Agathon",
			"Kendrick",
			"Langley",
			"Mitchell",
			"Nolan",
			"Owens",
			"Preston",
			"Quinn",
			"Robinson",
			"Sawyer",
			"Turner",
			"Underwood",
			"Vaughn",
			"Walker",
			"Xander",
			"Young",
			"Zimmerman",
			"Anderson",
			"Brown",
			"Clark",
			"Davis",
			"Edwards",
			"Franklin",
			"Graham",
			"Hamilton",
			"Ingram",
			"Johnson",
			"King",
			"Lewis",
			"Miller",
			"Nelson",
			"Parker",
			"Reed",
			"Scott",
			"Taylor",
			"Upton",
			"Vance",
			"Wright",
			"Xenos",
			"Yates",
			"Zane",
			"Valerii",
			"Allen",
			"Brooks",
			"Carter",
			"Diaz",
			"Evans",
			"Fisher",
			"Green",
			"Hughes",
			"Jackson",
			"Lee",
			"Tyrol",
			"Moore",
			"Peterson",
			"Price",
			"Roberts",
			"Sanders",
			"Stewart",
			"Tucker",
			"Warren",
			"Wood",
			"Young",
			"Truman",
			"Theisman",
			"Henke",
			"Pritchart",
			"Hemphill",
			"LaFollet",
			"Harkness",
			"Alexander",
			"Winton",
			"Churchill",
			"Duré",
			"Hoyt",
			"Kassad",
			"Silenus",
			"Weintraub",
			"Lamia",
			"Thrace",
			"Tigh",
			"Gaeta",
			"Roslin",
			"Anders",
			"Dualla",
			"Cain"
		};
	}
}
