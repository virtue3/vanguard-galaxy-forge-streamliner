using System;
using Source.Crew;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000183 RID: 387
	public class ExtraMercenaryPreset
	{
		// Token: 0x06000E1D RID: 3613 RVA: 0x000667B9 File Offset: 0x000649B9
		public ExtraMercenaryPreset(string callsign, string[] battlecries, Gender gender, string seed)
		{
			this.callsign = callsign;
			this.battlecries = battlecries;
			this.gender = gender;
			this.seed = seed;
		}

		// Token: 0x040007D7 RID: 2007
		public string callsign;

		// Token: 0x040007D8 RID: 2008
		public string[] battlecries;

		// Token: 0x040007D9 RID: 2009
		public Gender gender;

		// Token: 0x040007DA RID: 2010
		public string seed;
	}
}
