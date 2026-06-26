using System;

namespace Source.Item
{
	// Token: 0x020000F6 RID: 246
	public static class RarityExtentions
	{
		// Token: 0x06000945 RID: 2373 RVA: 0x00047800 File Offset: 0x00045A00
		public static string GetAlternateNames(this Rarity spaceshipType)
		{
			switch (spaceshipType)
			{
			case Rarity.Standard:
				return "whitecommon";
			case Rarity.Enhanced:
				return "greenuncommon";
			case Rarity.HighGrade:
				return "bluerare";
			case Rarity.Exotic:
				return "purpleepic";
			case Rarity.Legendary:
				return "orange";
			default:
				return "";
			}
		}
	}
}
