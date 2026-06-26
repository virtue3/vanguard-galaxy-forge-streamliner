using System;

namespace Source.Util.NameGen
{
	// Token: 0x0200004F RID: 79
	public static class CrimeGen
	{
		// Token: 0x06000321 RID: 801 RVA: 0x00019628 File Offset: 0x00017828
		public static string GenerateCrime(SeededRandom random)
		{
			int num = random.RandomRange(0, 100);
			string str;
			if (num < 50)
			{
				str = random.Choose<string>(CrimeGen.minorCrimes);
			}
			else if (num < 85)
			{
				str = random.Choose<string>(CrimeGen.seriousCrimes);
			}
			else
			{
				str = random.Choose<string>(CrimeGen.extremeCrimes);
			}
			return random.Choose<string>(CrimeGen.crimePrefixes) + " " + str;
		}

		// Token: 0x040001B5 RID: 437
		private static readonly string[] minorCrimes = new string[]
		{
			"illegal salvage operations",
			"unregistered drone deployment",
			"unauthorized warp jumps",
			"tampering with beacon channels",
			"transporting contraband bio-material",
			"identity masking violations",
			"pattern-spoofing in restricted lanes",
			"the crime of murder",
			"speeding",
			"hurling asteroids towards planets"
		};

		// Token: 0x040001B6 RID: 438
		private static readonly string[] seriousCrimes = new string[]
		{
			"acts of orbital piracy",
			"multiple counts of ship hijacking",
			"substance contamination offences",
			"smuggling restricted xenotech",
			"attacking civilian convoys",
			"unauthorized boarding",
			"subverting planetary authority",
			"conducting forbidden research",
			"looking at an official the wrong way"
		};

		// Token: 0x040001B7 RID: 439
		private static readonly string[] extremeCrimes = new string[]
		{
			"indoctrination attempts",
			"bio-mechanical corruption spreading",
			"terror operations in populated sectors",
			"hostile takeover of relay outposts",
			"genetic tampering with sentient species",
			"attempted assimilation of government officials"
		};

		// Token: 0x040001B8 RID: 440
		private static readonly string[] crimePrefixes = new string[]
		{
			"For",
			"Due to",
			"As punishment for",
			"Following accusations of",
			"After documented involvement in"
		};
	}
}
