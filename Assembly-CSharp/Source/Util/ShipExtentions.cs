using System;
using Behaviour.Spacestation.Docking;
using Source.SpaceShip;

namespace Source.Util
{
	// Token: 0x02000042 RID: 66
	public static class ShipExtentions
	{
		// Token: 0x060002EB RID: 747 RVA: 0x00017C4C File Offset: 0x00015E4C
		public static int GetShipSizeCategory(this SpaceShipType spaceshipType)
		{
			switch (spaceshipType)
			{
			case SpaceShipType.Size1:
				return 1;
			case SpaceShipType.Size2:
				return 2;
			case SpaceShipType.Size3:
				return 3;
			case SpaceShipType.Size4:
				return 4;
			case SpaceShipType.Size5:
				return 5;
			case SpaceShipType.Size6:
				return 6;
			case SpaceShipType.Size7:
				return 7;
			case SpaceShipType.Size8:
				return 8;
			default:
				return 0;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00017C8C File Offset: 0x00015E8C
		public static DockingOptionSize GetDockingSize(this SpaceShipType type)
		{
			DockingOptionSize result;
			switch (type)
			{
			case SpaceShipType.Drone:
				result = DockingOptionSize.small;
				break;
			case SpaceShipType.Size1:
				result = DockingOptionSize.small;
				break;
			case SpaceShipType.Size2:
				result = DockingOptionSize.small;
				break;
			case SpaceShipType.Size3:
				result = DockingOptionSize.medium;
				break;
			case SpaceShipType.Size4:
				result = DockingOptionSize.large;
				break;
			case SpaceShipType.Size5:
				result = DockingOptionSize.large;
				break;
			case SpaceShipType.Size6:
				result = DockingOptionSize.large;
				break;
			case SpaceShipType.Size7:
				result = DockingOptionSize.large;
				break;
			case SpaceShipType.Size8:
				result = DockingOptionSize.large;
				break;
			default:
				result = DockingOptionSize.small;
				break;
			}
			return result;
		}
	}
}
