using System;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000189 RID: 393
	public class SalvageGuild : Faction
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00066B0D File Offset: 0x00064D0D
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat || GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Salvage;
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00066B38 File Offset: 0x00064D38
		public SalvageGuild()
		{
			this.conquestColor = "#FFA830".HexToColor();
			base.missionTypes.Add("SalvageWreck");
			base.missionTypes.Add("SalvageSamples");
			base.missionTypes.Add("TradeTerminal");
			base.missionTypes.Add("ClearSalvageField");
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00066B9B File Offset: 0x00064D9B
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationSalvage.GenerateVulturesStationName();
		}
	}
}
