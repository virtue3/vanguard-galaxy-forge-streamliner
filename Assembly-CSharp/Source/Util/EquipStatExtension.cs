using System;
using Source.Item;

namespace Source.Util
{
	// Token: 0x0200002D RID: 45
	public static class EquipStatExtension
	{
		// Token: 0x06000248 RID: 584 RVA: 0x00011B04 File Offset: 0x0000FD04
		public static string GetDisplayName(this EquipStat equipStat)
		{
			return Translation.Translate("@EquipStat" + equipStat.ToString(), Array.Empty<object>());
		}
	}
}
