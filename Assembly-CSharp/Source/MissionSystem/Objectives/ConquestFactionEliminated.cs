using System;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C2 RID: 194
	public class ConquestFactionEliminated : SystemsConquered
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0003E46A File Offset: 0x0003C66A
		public override string statusText
		{
			get
			{
				return "Eliminate " + Translation.Translate(this.faction.name, Array.Empty<object>()) + " from the Conquest Sector.";
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0003E490 File Offset: 0x0003C690
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0003E498 File Offset: 0x0003C698
		public override float targetPercentage { get; set; }

		// Token: 0x060007CA RID: 1994 RVA: 0x0003E4A4 File Offset: 0x0003C6A4
		public override bool IsComplete()
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller == null)
			{
				return false;
			}
			ConquestFactionStanding factionStanding = storyteller.GetFactionStanding(this.faction);
			this.currentPercentage = factionStanding.currentConqueredPercentage;
			return this.currentPercentage <= this.targetPercentage;
		}
	}
}
