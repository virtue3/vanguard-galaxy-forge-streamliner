using System;

namespace Source.Galaxy.POI
{
	// Token: 0x0200015B RID: 347
	public static class SpaceStationFacilityExtensions
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x0005E3A0 File Offset: 0x0005C5A0
		public static int GetSortOrder(this SpaceStationFacility facility)
		{
			switch (facility)
			{
			case SpaceStationFacility.GeneralShop:
				return 0;
			case SpaceStationFacility.MiningShop:
				return 0;
			case SpaceStationFacility.TradeTerminal:
				return 1;
			case SpaceStationFacility.BountyBoard:
				return 1;
			case SpaceStationFacility.PoliceBoard:
				return 1;
			case SpaceStationFacility.MissionBoard:
				return 2;
			case SpaceStationFacility.ExitSpacestation:
				return 999;
			case SpaceStationFacility.SalvageShop:
				return 0;
			case SpaceStationFacility.BountyShop:
				return 0;
			case SpaceStationFacility.PatrolShop:
				return 0;
			case SpaceStationFacility.IndustryShop:
				return 0;
			case SpaceStationFacility.ConquestShop:
				return 0;
			case SpaceStationFacility.IndustryBoard:
				return 1;
			case SpaceStationFacility.SalvageWorkshop:
				return 1;
			case SpaceStationFacility.RecruitmentCenter:
				return 1;
			}
			return (int)facility;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0005E458 File Offset: 0x0005C658
		public static bool IsShop(this SpaceStationFacility facility)
		{
			bool result;
			if (facility != SpaceStationFacility.GeneralShop)
			{
				if (facility != SpaceStationFacility.MiningShop)
				{
					switch (facility)
					{
					case SpaceStationFacility.SalvageShop:
						result = true;
						break;
					case SpaceStationFacility.BountyShop:
						result = true;
						break;
					case SpaceStationFacility.PatrolShop:
						result = true;
						break;
					case SpaceStationFacility.IndustryShop:
						result = true;
						break;
					case SpaceStationFacility.ConquestShop:
						result = true;
						break;
					default:
						result = false;
						break;
					}
				}
				else
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
