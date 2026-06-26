using System;
using System.Collections.Generic;
using Source.Crew;
using Source.Galaxy;

namespace Source.Player
{
	// Token: 0x02000099 RID: 153
	public class PersonalHistoryData
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x000363BA File Offset: 0x000345BA
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x000363C2 File Offset: 0x000345C2
		public int starterCredits { get; private set; } = 1;

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x000363CB File Offset: 0x000345CB
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x000363D3 File Offset: 0x000345D3
		public CommanderSpecialization starterSpecialization { get; private set; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x000363DC File Offset: 0x000345DC
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x000363E4 File Offset: 0x000345E4
		public List<string> starterShips { get; private set; } = new List<string>();

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x000363ED File Offset: 0x000345ED
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x000363F5 File Offset: 0x000345F5
		public List<Faction> starterReputations { get; private set; } = new List<Faction>();

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x000363FE File Offset: 0x000345FE
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00036406 File Offset: 0x00034606
		public int starterCrewMembersAmount { get; private set; }

		// Token: 0x0600064D RID: 1613 RVA: 0x00036410 File Offset: 0x00034610
		public PersonalHistoryData(PersonalHistory personalHistory)
		{
			switch (personalHistory)
			{
			case PersonalHistory.Miner:
				this.starterCredits = 5000;
				this.starterSpecialization = CommanderSpecialization.Mining;
				this.starterShips.Add("Chisel Mk I");
				this.starterShips.Add("Garnil");
				this.starterCrewMembersAmount = 0;
				this.description = "@NGMiningStart";
				this.locked = false;
				return;
			case PersonalHistory.NavyCaptain:
				this.starterCredits = 4000;
				this.starterSpecialization = CommanderSpecialization.Offense;
				this.starterShips.Add("Margil");
				this.starterShips.Add("Raptor");
				this.startWithAmmo = true;
				this.starterCrewMembersAmount = 0;
				this.description = "@NGCombatStart";
				this.locked = false;
				return;
			case PersonalHistory.Salvaging:
				this.starterCredits = 5000;
				this.starterSpecialization = CommanderSpecialization.Salvaging;
				this.starterShips.Add("Tugbit");
				this.starterShips.Add("Chisel Mk I SN");
				this.starterCrewMembersAmount = 0;
				this.description = "@NGSalvageStart";
				this.locked = false;
				return;
			case PersonalHistory.Hauler:
				this.starterCredits = 10000;
				this.starterSpecialization = CommanderSpecialization.Economy;
				this.starterShips.Add("Oxlo Mk I");
				this.starterCrewMembersAmount = 1;
				this.description = "@NGNotAvailable";
				this.locked = true;
				return;
			case PersonalHistory.BountyHunter:
				this.starterCredits = 5000;
				this.starterSpecialization = CommanderSpecialization.Drones;
				this.starterShips.Add("Acolyte AC-1");
				this.startWithAmmo = true;
				this.starterCrewMembersAmount = 0;
				this.description = "@NGNotAvailable";
				this.locked = true;
				return;
			case PersonalHistory.Pirate:
				this.starterCredits = 1000;
				this.starterSpecialization = CommanderSpecialization.Offense;
				this.starterShips.Add("Blood Dredger");
				this.startWithAmmo = true;
				this.starterCrewMembersAmount = 0;
				this.description = "@NGNotAvailable";
				this.locked = true;
				return;
			default:
				this.starterSpecialization = CommanderSpecialization.Leadership;
				return;
			}
		}

		// Token: 0x0400034F RID: 847
		public string starterShipName = "";

		// Token: 0x04000350 RID: 848
		public bool startWithAmmo;

		// Token: 0x04000351 RID: 849
		public string description;

		// Token: 0x04000352 RID: 850
		public bool locked;
	}
}
