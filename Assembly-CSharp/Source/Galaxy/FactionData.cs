using System;
using System.Collections.Generic;
using LightJson;
using Source.Player;
using Source.Util;

namespace Source.Galaxy
{
	// Token: 0x02000141 RID: 321
	public class FactionData : IJsonSource
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x0005688F File Offset: 0x00054A8F
		public static FactionData current
		{
			get
			{
				GamePlayer current = GamePlayer.current;
				if (current == null)
				{
					return null;
				}
				return current.factionData;
			}
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x000568A4 File Offset: 0x00054AA4
		public void ChangeReputation(Faction self, Faction other, int change)
		{
			if (self == other)
			{
				return;
			}
			int reputation = this.GetFactionPair(self, other).reputation;
			int num = Math.Min(reputation + change, GameMath.maxReputation);
			num = Math.Max(num, GameMath.minReputation);
			int num2 = num - reputation;
			if (self == Faction.player && num2 != 0)
			{
				if (num2 > 0)
				{
					GamePlayer.current.AddAutopilotStat(IdleStat.ReputationGained, num2);
				}
				else
				{
					GamePlayer.current.AddAutopilotStat(IdleStat.ReputationLost, num2);
				}
			}
			this.GetFactionPair(self, other).reputation = num;
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0005691C File Offset: 0x00054B1C
		public bool IsEnemy(Faction self, Faction other)
		{
			if (self == other)
			{
				return false;
			}
			if (self == Faction.player || other == Faction.player)
			{
				Faction item = (self == Faction.player) ? other : self;
				if (GamePlayer.current.atWar.Contains(item))
				{
					return true;
				}
			}
			return (float)this.GetReputation(self, other) < -500f;
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x00056970 File Offset: 0x00054B70
		public int GetReputation(Faction self, Faction other)
		{
			if (self == null || other == null)
			{
				return -9999;
			}
			return this.GetFactionPair(self, other).reputation;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0005698B File Offset: 0x00054B8B
		public void SetReputation(Faction self, Faction other, int setTo)
		{
			this.GetFactionPair(self, other).reputation = setTo;
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0005699C File Offset: 0x00054B9C
		private FactionData.FactionPair GetFactionPair(Faction self, Faction other)
		{
			foreach (FactionData.FactionPair factionPair in this.factionPairs)
			{
				if ((factionPair.f1 == self && factionPair.f2 == other) || (factionPair.f1 == other && factionPair.f2 == self))
				{
					return factionPair;
				}
			}
			FactionData.FactionPair factionPair2 = new FactionData.FactionPair(self, other, 0);
			this.factionPairs.Add(factionPair2);
			return factionPair2;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00056A28 File Offset: 0x00054C28
		public JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (FactionData.FactionPair factionPair in this.factionPairs)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"f1",
						factionPair.f1.identifier
					},
					{
						"f2",
						factionPair.f2.identifier
					},
					{
						"reputation",
						new double?((double)factionPair.reputation)
					}
				});
			}
			return jsonArray;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00056AE8 File Offset: 0x00054CE8
		public static FactionData FromJson(JsonValue data)
		{
			FactionData factionData = new FactionData();
			foreach (JsonValue jsonValue in data.AsJsonArray)
			{
				int num = Math.Min(jsonValue["reputation"].AsInteger, GameMath.maxReputation);
				num = Math.Max(num, GameMath.minReputation);
				Faction faction = Faction.Get(jsonValue["f1"]);
				Faction faction2 = Faction.Get(jsonValue["f2"]);
				if (faction != faction2)
				{
					factionData.factionPairs.Add(new FactionData.FactionPair(faction, faction2, num));
				}
			}
			return factionData;
		}

		// Token: 0x040006BE RID: 1726
		public const float foeReputation = -500f;

		// Token: 0x040006BF RID: 1727
		private List<FactionData.FactionPair> factionPairs = new List<FactionData.FactionPair>();

		// Token: 0x020004BC RID: 1212
		private class FactionPair
		{
			// Token: 0x06002970 RID: 10608 RVA: 0x000E8B2F File Offset: 0x000E6D2F
			public FactionPair(Faction f1, Faction f2, int rep = 0)
			{
				this.f1 = f1;
				this.f2 = f2;
				this.reputation = rep;
			}

			// Token: 0x040019C5 RID: 6597
			public readonly Faction f1;

			// Token: 0x040019C6 RID: 6598
			public readonly Faction f2;

			// Token: 0x040019C7 RID: 6599
			public int reputation;
		}
	}
}
