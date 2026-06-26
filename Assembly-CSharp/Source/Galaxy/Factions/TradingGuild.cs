using System;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200018C RID: 396
	public class TradingGuild : Faction
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x00066CFB File Offset: 0x00064EFB
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat || GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Cargo;
			}
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00066D28 File Offset: 0x00064F28
		public TradingGuild()
		{
			base.missionTypes.Add("Courier");
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("EscortShip");
			base.missionTypes.Add("TradeMaterials");
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00066D7B File Offset: 0x00064F7B
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationTrade.GenerateStationName();
		}
	}
}
