using System;

namespace Source.Galaxy.NameGenerator
{
	// Token: 0x0200016E RID: 366
	public static class Star
	{
		// Token: 0x06000DDF RID: 3551 RVA: 0x00064144 File Offset: 0x00062344
		public static string GenerateStarName()
		{
			switch (Star.rng.Next(0, 4))
			{
			case 0:
				return Star.GreekDesignationName();
			case 1:
				return Star.PrefixSuffixName();
			case 2:
				return Star.WithPostfixName();
			case 3:
				return Star.HybridAlphaNum();
			default:
				return "Unnamed Star";
			}
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00064194 File Offset: 0x00062394
		private static string PrefixSuffixName()
		{
			string str = Star.prefixes[Star.rng.Next(Star.prefixes.Length)];
			string str2 = Star.suffixes[Star.rng.Next(Star.suffixes.Length)];
			return str + str2;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x000641D8 File Offset: 0x000623D8
		private static string GreekDesignationName()
		{
			string str = Star.greekLetters[Star.rng.Next(Star.greekLetters.Length)];
			string str2 = Star.PrefixSuffixName();
			return str + " " + str2;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00064210 File Offset: 0x00062410
		private static string WithPostfixName()
		{
			string str = Star.PrefixSuffixName();
			string str2 = Star.postfixes[Star.rng.Next(Star.postfixes.Length)];
			return str + " " + str2;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00064248 File Offset: 0x00062448
		private static string HybridAlphaNum()
		{
			return Star.prefixes[Star.rng.Next(Star.prefixes.Length)] + "-" + Star.rng.Next(1, 100).ToString();
		}

		// Token: 0x0400079A RID: 1946
		private static Random rng = new Random();

		// Token: 0x0400079B RID: 1947
		private static string[] prefixes = new string[]
		{
			"Zor",
			"Kel",
			"Vor",
			"Tan",
			"Eri",
			"Xan",
			"Lum",
			"Ael",
			"Nex",
			"Sar",
			"Thal",
			"Yel",
			"Vex",
			"Mar",
			"Gra",
			"Nor",
			"Zel",
			"Quan"
		};

		// Token: 0x0400079C RID: 1948
		private static string[] suffixes = new string[]
		{
			"os",
			"ar",
			"on",
			"an",
			"is",
			"ex",
			"ix",
			"ir",
			"en",
			"us",
			"or",
			"ae"
		};

		// Token: 0x0400079D RID: 1949
		private static string[] greekLetters = new string[]
		{
			"Alpha",
			"Beta",
			"Gamma",
			"Delta",
			"Epsilon",
			"Zeta",
			"Eta",
			"Theta",
			"Iota",
			"Kappa",
			"Lambda",
			"Mu",
			"Nu",
			"Xi",
			"Omicron",
			"Pi",
			"Rho",
			"Sigma",
			"Tau",
			"Upsilon",
			"Phi",
			"Chi",
			"Psi",
			"Omega"
		};

		// Token: 0x0400079E RID: 1950
		private static string[] postfixes = new string[]
		{
			"Prime",
			"Secundus",
			"Major",
			"Minor",
			"Nova",
			"Rex",
			"V",
			"IV",
			"IX",
			"VII",
			"VII",
			"X",
			"Ultima"
		};
	}
}
