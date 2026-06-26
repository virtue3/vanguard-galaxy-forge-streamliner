using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x0200009F RID: 159
	public static class Titles
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x00036D7C File Offset: 0x00034F7C
		public static TitleDefinition Get(string identifier)
		{
			TitleDefinition result;
			Titles._all.TryGetValue(identifier, out result);
			return result;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00036D98 File Offset: 0x00034F98
		public static IEnumerable<TitleDefinition> GetAll()
		{
			return Titles._all.Values;
		}

		// Token: 0x04000382 RID: 898
		public static readonly Color colorGold = new Color(1f, 0.84f, 0f);

		// Token: 0x04000383 RID: 899
		public static readonly Color colorSilver = new Color(0.75f, 0.75f, 0.75f);

		// Token: 0x04000384 RID: 900
		public static readonly Color colorRare = new Color(0.4f, 0.85f, 1f);

		// Token: 0x04000385 RID: 901
		public static readonly TitleDefinition SupporterCombat = new TitleDefinition("supporter_combat", "@TitleSupporterCombat", Titles.colorGold);

		// Token: 0x04000386 RID: 902
		public static readonly TitleDefinition SupporterMining = new TitleDefinition("supporter_mining", "@TitleSupporterMining", Titles.colorGold);

		// Token: 0x04000387 RID: 903
		public static readonly TitleDefinition SupporterSalvaging = new TitleDefinition("supporter_salvaging", "@TitleSupporterSalvaging", Titles.colorGold);

		// Token: 0x04000388 RID: 904
		public static readonly TitleDefinition SupporterTrading = new TitleDefinition("supporter_trading", "@TitleSupporterTrading", Titles.colorGold);

		// Token: 0x04000389 RID: 905
		public static readonly TitleDefinition Miner = new TitleDefinition("miner", "@TitleMiner", Titles.colorSilver);

		// Token: 0x0400038A RID: 906
		public static readonly TitleDefinition NavyCaptain = new TitleDefinition("navycaptain", "@TitleNavyCaptain", Titles.colorSilver);

		// Token: 0x0400038B RID: 907
		public static readonly TitleDefinition Salvager = new TitleDefinition("salvager", "@TitleSalvager", Titles.colorSilver);

		// Token: 0x0400038C RID: 908
		public static readonly TitleDefinition Hauler = new TitleDefinition("hauler", "@TitleHauler", Titles.colorSilver);

		// Token: 0x0400038D RID: 909
		public static readonly TitleDefinition BountyHunter = new TitleDefinition("bountyhunter", "@TitleBountyHunter", Titles.colorSilver);

		// Token: 0x0400038E RID: 910
		public static readonly TitleDefinition Pirate = new TitleDefinition("pirate", "@TitlePirate", Titles.colorSilver);

		// Token: 0x0400038F RID: 911
		private static readonly Dictionary<string, TitleDefinition> _all = new Dictionary<string, TitleDefinition>
		{
			{
				Titles.SupporterCombat.identifier,
				Titles.SupporterCombat
			},
			{
				Titles.SupporterMining.identifier,
				Titles.SupporterMining
			},
			{
				Titles.SupporterSalvaging.identifier,
				Titles.SupporterSalvaging
			},
			{
				Titles.SupporterTrading.identifier,
				Titles.SupporterTrading
			},
			{
				Titles.Miner.identifier,
				Titles.Miner
			},
			{
				Titles.NavyCaptain.identifier,
				Titles.NavyCaptain
			},
			{
				Titles.Salvager.identifier,
				Titles.Salvager
			},
			{
				Titles.Hauler.identifier,
				Titles.Hauler
			},
			{
				Titles.BountyHunter.identifier,
				Titles.BountyHunter
			},
			{
				Titles.Pirate.identifier,
				Titles.Pirate
			}
		};
	}
}
