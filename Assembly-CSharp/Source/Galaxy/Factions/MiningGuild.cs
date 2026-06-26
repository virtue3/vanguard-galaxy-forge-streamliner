using System;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000184 RID: 388
	public class MiningGuild : Faction
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x000667DE File Offset: 0x000649DE
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat || GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Mining;
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00066808 File Offset: 0x00064A08
		public MiningGuild()
		{
			this.conquestColor = "#3FAD1D".HexToColor();
			base.missionTypes.Add("MineOre");
			base.missionTypes.Add("OreSamples");
			base.missionTypes.Add("HelpMiner");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("TradeTerminal");
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0006687B File Offset: 0x00064A7B
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationMining.GenerateStationName();
		}
	}
}
