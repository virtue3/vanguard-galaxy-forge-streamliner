using System;
using Source.Galaxy;
using Source.Util;

namespace Behaviour.Equipment.Builder
{
	// Token: 0x02000372 RID: 882
	public static class ManufacturerExtensions
	{
		// Token: 0x06002206 RID: 8710 RVA: 0x000C5680 File Offset: 0x000C3880
		public static string GetDisplayName(this Manufacturer manufacturer)
		{
			return Translation.TranslateOnly("@ManufacturerName" + manufacturer.ToString(), Array.Empty<object>());
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000C56A4 File Offset: 0x000C38A4
		public static Faction GetFaction(this Manufacturer manufacturer)
		{
			switch (manufacturer)
			{
			case Manufacturer.Gold:
				return Faction.gold;
			case Manufacturer.Red:
				return Faction.red;
			case Manufacturer.Blue:
				return Faction.blue;
			case Manufacturer.Mining:
				return Faction.miningGuild;
			case Manufacturer.Pirate:
				return Faction.marauders;
			case Manufacturer.Police:
				return Faction.policeGuild;
			}
			return null;
		}
	}
}
