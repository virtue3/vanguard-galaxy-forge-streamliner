using System;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200017B RID: 379
	public class BountyGuild : Faction
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000E08 RID: 3592 RVA: 0x00065FCD File Offset: 0x000641CD
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat;
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00065FE4 File Offset: 0x000641E4
		public BountyGuild()
		{
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("TradeTerminal");
			base.missionTypes.Add("StationBattle");
			base.missionTypes.Add("EscortShip");
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00066057 File Offset: 0x00064257
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationBounty.GenerateOrsanonStationName();
		}
	}
}
