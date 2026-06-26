using System;
using Source.Galaxy;

namespace Behaviour
{
	// Token: 0x020001A5 RID: 421
	[Serializable]
	public class ConquestRankRequirement
	{
		// Token: 0x06000EBA RID: 3770 RVA: 0x00068C70 File Offset: 0x00066E70
		public bool IsMet()
		{
			Faction faction = Faction.Get(this.factionId);
			return faction != null && faction.GetConquestRank() >= this.minRank;
		}

		// Token: 0x04000850 RID: 2128
		public string factionId;

		// Token: 0x04000851 RID: 2129
		public ConquestRank minRank;
	}
}
