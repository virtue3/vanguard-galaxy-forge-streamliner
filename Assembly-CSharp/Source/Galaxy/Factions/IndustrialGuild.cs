using System;
using Source.Galaxy.NameGenerator;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000180 RID: 384
	public class IndustrialGuild : Faction
	{
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000E12 RID: 3602 RVA: 0x00066282 File Offset: 0x00064482
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat || GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Mining;
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000662AC File Offset: 0x000644AC
		public IndustrialGuild()
		{
			base.missionTypes.Add("MineOre");
			base.missionTypes.Add("OreSamples");
			base.missionTypes.Add("HelpMiner");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("TradeTerminal");
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0006631F File Offset: 0x0006451F
		public override string GenerateStationName(MapPointOfInterest ss)
		{
			return StationIndustry.GenerateForgeIndustryStationName();
		}
	}
}
