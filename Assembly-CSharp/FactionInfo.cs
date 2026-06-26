using System;
using System.Collections.Generic;
using Source.Galaxy;

// Token: 0x02000013 RID: 19
public static class FactionInfo
{
	// Token: 0x0600012C RID: 300 RVA: 0x00009A84 File Offset: 0x00007C84
	public static string GetAbbreviation(Faction faction)
	{
		if (faction == null || string.IsNullOrEmpty(faction.identifier))
		{
			return "UNK";
		}
		string result;
		if (FactionInfo.Abbreviations.TryGetValue(faction.identifier, out result))
		{
			return result;
		}
		return "UNK";
	}

	// Token: 0x040000A0 RID: 160
	public static readonly Dictionary<string, string> Abbreviations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
	{
		{
			"Red",
			"KOL"
		},
		{
			"Gold",
			"LUC"
		},
		{
			"Blue",
			"SER"
		},
		{
			"MiningGuild",
			"MIN"
		},
		{
			"TradingGuild",
			"INT"
		},
		{
			"SalvageGuild",
			"SVS"
		},
		{
			"PoliceGuild",
			"CSC"
		},
		{
			"BountyGuild",
			"OSY"
		},
		{
			"IndustrialGuild",
			"FIS"
		},
		{
			"Stranded",
			"STR"
		},
		{
			"Darkspacers",
			"DRK"
		},
		{
			"Smugglers",
			"VOD"
		},
		{
			"Puppeteers",
			"UBR"
		},
		{
			"Marauders",
			"SYN"
		},
		{
			"Fanatics",
			"MRC"
		},
		{
			"Amalgam",
			"AMG"
		},
		{
			"HolyRadicals",
			"HOL"
		},
		{
			"MercenaryGuild",
			"TAC"
		}
	};
}
