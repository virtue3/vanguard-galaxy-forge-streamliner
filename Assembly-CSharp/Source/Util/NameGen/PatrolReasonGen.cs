using System;

namespace Source.Util.NameGen
{
	// Token: 0x02000050 RID: 80
	public static class PatrolReasonGen
	{
		// Token: 0x06000323 RID: 803 RVA: 0x000197B4 File Offset: 0x000179B4
		public static string GenerateReason(SeededRandom random)
		{
			int num = random.RandomRange(0, 100);
			string str;
			if (num < 50)
			{
				str = random.Choose<string>(PatrolReasonGen.minorReasons);
			}
			else if (num < 85)
			{
				str = random.Choose<string>(PatrolReasonGen.seriousReasons);
			}
			else
			{
				str = random.Choose<string>(PatrolReasonGen.extremeReasons);
			}
			return random.Choose<string>(PatrolReasonGen.prefixes) + " " + str;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00019813 File Offset: 0x00017A13
		public static string GenerateAidRequest(SeededRandom random)
		{
			return random.Choose<string>(PatrolReasonGen.aidRequest);
		}

		// Token: 0x040001B9 RID: 441
		private static readonly string[] minorReasons = new string[]
		{
			"harassing civilian transports",
			"interfering with local patrols",
			"violating border protocols",
			"running illegal scan operations",
			"shadowing trade convoys",
			"provoking disputes with settlers"
		};

		// Token: 0x040001BA RID: 442
		private static readonly string[] seriousReasons = new string[]
		{
			"raiding trade routes",
			"blockading civilian stations",
			"illegally seizing cargo vessels",
			"conducting covert military drills",
			"harassing diplomatic envoys"
		};

		// Token: 0x040001BB RID: 443
		private static readonly string[] extremeReasons = new string[]
		{
			"disrupting sector-wide communications",
			"deploying corruption anomalies",
			"attempting a hostile takeover of stations",
			"staging coordinated attacks on outposts",
			"forcibly conscripting civilians",
			"sabotaging planetary infrastructure"
		};

		// Token: 0x040001BC RID: 444
		private static readonly string[] prefixes = new string[]
		{
			"forces have been",
			"forces are currently",
			"forces were recently",
			"forces have once again begun",
			"forces seem to be escalating their activities by"
		};

		// Token: 0x040001BD RID: 445
		private static readonly string[] aidRequest = new string[]
		{
			"requires immediate aid.",
			"is requesting urgent assistance.",
			"asks that we lend our support.",
			"has called for reinforcements.",
			"seeks our intervention.",
			"demands rapid tactical support.",
			"urgently requests help.",
			"is calling for immediate backup.",
			"requests that we act swiftly.",
			"implores us to assist in this matter."
		};
	}
}
