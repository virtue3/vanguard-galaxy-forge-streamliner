using System;
using Source.Crew;
using Source.Item;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200003C RID: 60
	public static class RarityExtensions
	{
		// Token: 0x060002AF RID: 687 RVA: 0x00016840 File Offset: 0x00014A40
		public static Color GetColor(this Rarity rarity)
		{
			Color result;
			switch (rarity)
			{
			case Rarity.Standard:
				result = ColorHelper.RarityStandard;
				break;
			case Rarity.Enhanced:
				result = ColorHelper.RarityEnhanced;
				break;
			case Rarity.HighGrade:
				result = ColorHelper.RarityHighGrade;
				break;
			case Rarity.Exotic:
				result = ColorHelper.RarityExotic;
				break;
			case Rarity.Legendary:
				result = ColorHelper.RarityLegendary;
				break;
			default:
				result = ColorHelper.RarityStandard;
				break;
			}
			return result;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00016898 File Offset: 0x00014A98
		public static Color GetBackgroundColor(this Rarity rarity)
		{
			Color result;
			switch (rarity)
			{
			case Rarity.Standard:
				result = ColorHelper.RarityStandardBackground;
				break;
			case Rarity.Enhanced:
				result = ColorHelper.RarityEnhancedBackground;
				break;
			case Rarity.HighGrade:
				result = ColorHelper.RarityHighGradeBackground;
				break;
			case Rarity.Exotic:
				result = ColorHelper.RarityExoticBackground;
				break;
			case Rarity.Legendary:
				result = ColorHelper.RarityLegendaryBackground;
				break;
			default:
				result = ColorHelper.RarityStandardBackground;
				break;
			}
			return result;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000168F0 File Offset: 0x00014AF0
		public static string GetDisplayName(this Rarity rarity)
		{
			switch (rarity)
			{
			case Rarity.Standard:
				return "Standard";
			case Rarity.Enhanced:
				return "Enhanced";
			case Rarity.HighGrade:
				return "High Grade";
			case Rarity.Exotic:
				return "Exotic";
			case Rarity.Legendary:
				return "Legendary";
			default:
				return "";
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0001693C File Offset: 0x00014B3C
		public static string GetDisplayName(this Profession prof)
		{
			return prof.ToString();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0001694C File Offset: 0x00014B4C
		public static float GetPowerMultiplier(this Rarity rarity)
		{
			float result;
			switch (rarity)
			{
			case Rarity.Standard:
				result = 1f;
				break;
			case Rarity.Enhanced:
				result = 1.1f;
				break;
			case Rarity.HighGrade:
				result = 1.2f;
				break;
			case Rarity.Exotic:
				result = 1.35f;
				break;
			case Rarity.Legendary:
				result = 1.5f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000169A4 File Offset: 0x00014BA4
		public static float GetCostMultiplier(this Rarity rarity)
		{
			float result;
			switch (rarity)
			{
			case Rarity.Standard:
				result = 1f;
				break;
			case Rarity.Enhanced:
				result = 1.6f;
				break;
			case Rarity.HighGrade:
				result = 2.2f;
				break;
			case Rarity.Exotic:
				result = 3.4f;
				break;
			case Rarity.Legendary:
				result = 4.5f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000169FC File Offset: 0x00014BFC
		public static Rarity GetShopRarity(int level)
		{
			if (level > 5 && SeededRandom.Global.RandomBool(0.2f))
			{
				return Rarity.Enhanced;
			}
			return Rarity.Standard;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00016A18 File Offset: 0x00014C18
		public static int GetStatCount(this Rarity rarity)
		{
			int result;
			switch (rarity)
			{
			case Rarity.Standard:
				result = 0;
				break;
			case Rarity.Enhanced:
				result = 1;
				break;
			case Rarity.HighGrade:
				result = 2;
				break;
			case Rarity.Exotic:
				result = 3;
				break;
			case Rarity.Legendary:
				result = 3;
				break;
			default:
				result = 2;
				break;
			}
			return result;
		}
	}
}
