using System;
using Source.Galaxy;

namespace Source.Data
{
	// Token: 0x0200010C RID: 268
	[Serializable]
	public class FactionPrerequisites
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0004DEEF File Offset: 0x0004C0EF
		// (set) Token: 0x06000A3A RID: 2618 RVA: 0x0004DEF7 File Offset: 0x0004C0F7
		public ReputationLevel reputationLevel { get; private set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0004DF00 File Offset: 0x0004C100
		// (set) Token: 0x06000A3C RID: 2620 RVA: 0x0004DF08 File Offset: 0x0004C108
		public ConquestRank conquestRank { get; private set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0004DF11 File Offset: 0x0004C111
		// (set) Token: 0x06000A3E RID: 2622 RVA: 0x0004DF19 File Offset: 0x0004C119
		public string faction { get; private set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0004DF22 File Offset: 0x0004C122
		public Faction reqFaction
		{
			get
			{
				if (!string.IsNullOrEmpty(this.faction) && this._factionCached == null)
				{
					this._factionCached = Faction.Get(this.faction);
				}
				return this._factionCached;
			}
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0004DF50 File Offset: 0x0004C150
		public FactionPrerequisites(ReputationLevel reputationLevel, string faction)
		{
			this.reputationLevel = reputationLevel;
			this.faction = faction;
		}

		// Token: 0x04000591 RID: 1425
		private Faction _factionCached;
	}
}
