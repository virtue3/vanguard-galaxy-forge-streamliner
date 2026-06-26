using System;

namespace Source.Item
{
	// Token: 0x020000F4 RID: 244
	public static class ItemCategoryExtensions
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x00047671 File Offset: 0x00045871
		public static string GetDisplayName(this ItemCategory cat)
		{
			return "@ItemCategory" + cat.ToString();
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0004768C File Offset: 0x0004588C
		public static bool CanBeEquiped(this ItemCategory cat)
		{
			bool result;
			switch (cat)
			{
			case ItemCategory.Turret:
				result = true;
				break;
			case ItemCategory.Module:
				result = true;
				break;
			case ItemCategory.Booster:
				result = true;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x000476C0 File Offset: 0x000458C0
		public static float GetSellValueMultiplier(this ItemCategory cat)
		{
			switch (cat)
			{
			case ItemCategory.Ore:
				return 0.8f;
			case ItemCategory.Ammo:
				return 0.8f;
			case ItemCategory.Turret:
				return 0.12f;
			case ItemCategory.Module:
				return 0.12f;
			case ItemCategory.Booster:
				return 0.12f;
			case ItemCategory.Junk:
				return 0.8f;
			case ItemCategory.Drone:
				return 0.12f;
			case ItemCategory.RefinedProduct:
				return 0.8f;
			case ItemCategory.Torpedo:
				return 0.12f;
			case ItemCategory.JumpgatePass:
				return 0.12f;
			case ItemCategory.TradeGoods:
				return 0.5f;
			case ItemCategory.Usable:
				return 0.12f;
			case ItemCategory.DefensiveTurret:
				return 0.12f;
			case ItemCategory.Salvage:
				return 0.8f;
			case ItemCategory.Currency:
				return 0.05f;
			case ItemCategory.Crystal:
				return 0.8f;
			}
			return 0f;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000477A8 File Offset: 0x000459A8
		public static bool HasBuybackValue(this ItemCategory cat)
		{
			bool result;
			switch (cat)
			{
			case ItemCategory.Turret:
				result = true;
				break;
			case ItemCategory.Module:
				result = true;
				break;
			case ItemCategory.Booster:
				result = true;
				break;
			default:
				switch (cat)
				{
				case ItemCategory.JumpgatePass:
					return true;
				case ItemCategory.Usable:
					return true;
				case ItemCategory.DefensiveTurret:
					return true;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
