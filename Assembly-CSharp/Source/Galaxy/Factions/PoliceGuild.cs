using System;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000186 RID: 390
	public class PoliceGuild : Faction
	{
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000E22 RID: 3618 RVA: 0x0006688A File Offset: 0x00064A8A
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat;
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000668A0 File Offset: 0x00064AA0
		public PoliceGuild()
		{
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 1;
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("TradeTerminal");
			base.missionTypes.Add("StationBattle");
			base.missionTypes.Add("EscortShip");
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00066921 File Offset: 0x00064B21
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationPolice.GenerateCanisecStationName();
		}
	}
}
