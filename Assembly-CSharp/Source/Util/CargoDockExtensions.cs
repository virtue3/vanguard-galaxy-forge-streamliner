using System;
using Behaviour.Spacestation.Cargo;
using Source.Item;

namespace Source.Util
{
	// Token: 0x02000027 RID: 39
	public static class CargoDockExtensions
	{
		// Token: 0x06000227 RID: 551 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		public static CargoBox.CargoBoxColor GetCargoBoxColor(this ItemCategory type)
		{
			switch (type)
			{
			case ItemCategory.Ore:
			case ItemCategory.Crystal:
				return CargoBox.CargoBoxColor.Blue;
			case ItemCategory.Ammo:
			case ItemCategory.Torpedo:
				return CargoBox.CargoBoxColor.Red;
			case ItemCategory.Turret:
			case ItemCategory.DefensiveTurret:
				return CargoBox.CargoBoxColor.Grey;
			case ItemCategory.Module:
			case ItemCategory.Booster:
			case ItemCategory.Usable:
				return CargoBox.CargoBoxColor.Green;
			case ItemCategory.Junk:
				return CargoBox.CargoBoxColor.Brown;
			case ItemCategory.RefinedProduct:
				return CargoBox.CargoBoxColor.Teal;
			case ItemCategory.TradeGoods:
				return CargoBox.CargoBoxColor.Red;
			case ItemCategory.Salvage:
				return CargoBox.CargoBoxColor.Brown;
			}
			return CargoBox.CargoBoxColor.Grey;
		}
	}
}
