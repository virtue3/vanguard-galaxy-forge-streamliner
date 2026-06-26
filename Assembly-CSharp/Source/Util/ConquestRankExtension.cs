using System;
using System.Collections.Generic;
using System.Linq;
using Source.Galaxy;
using Source.Player;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200002B RID: 43
	public static class ConquestRankExtension
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0001163B File Offset: 0x0000F83B
		public static string GetConquestRankTranslation(this ConquestRank rank, string factionIdentifier)
		{
			if (rank == ConquestRank.None)
			{
				return Translation.Translate("@RankNone", Array.Empty<object>());
			}
			return Translation.Translate(string.Format("@{0}{1}", factionIdentifier, rank), Array.Empty<object>());
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0001166C File Offset: 0x0000F86C
		public static ConquestRank GetConquestRankLevel(int reputation)
		{
			if (reputation > 0)
			{
				using (IEnumerator<KeyValuePair<ConquestRank, int>> enumerator = (from t in ConquestRankExtension.ConquestRankThresholds
				orderby t.Value descending
				select t).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ConquestRank, int> keyValuePair = enumerator.Current;
						if (reputation >= keyValuePair.Value)
						{
							return keyValuePair.Key;
						}
					}
					return ConquestRank.None;
				}
			}
			foreach (KeyValuePair<ConquestRank, int> keyValuePair2 in from t in ConquestRankExtension.ConquestRankThresholds
			orderby t.Value
			select t)
			{
				if (reputation <= keyValuePair2.Value)
				{
					return keyValuePair2.Key;
				}
			}
			return ConquestRank.None;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0001175C File Offset: 0x0000F95C
		public static Color GetConquestColor(this ConquestRank level)
		{
			Color result;
			switch (level)
			{
			case ConquestRank.None:
				result = ColorHelper.reddish;
				break;
			case ConquestRank.Rank1:
				result = new Color(0.75f, 1f, 0.75f);
				break;
			case ConquestRank.Rank2:
				result = new Color(0.3f, 0.95f, 0.3f);
				break;
			case ConquestRank.Rank3:
				result = new Color(0.75f, 0.9f, 1f);
				break;
			case ConquestRank.Rank4:
				result = new Color(0.3f, 0.65f, 1f);
				break;
			case ConquestRank.Rank5:
				result = new Color(0.9f, 0.75f, 1f);
				break;
			case ConquestRank.Rank6:
				result = new Color(0.7f, 0.3f, 1f);
				break;
			default:
				result = Color.white;
				break;
			}
			return result;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0001182C File Offset: 0x0000FA2C
		public static float GetMissionCommendationsRewardBonus(this ConquestRank level, string factionIdentifier)
		{
			float num;
			switch (level)
			{
			case ConquestRank.Rank1:
				num = 0.1f;
				break;
			case ConquestRank.Rank2:
				num = 0.2f;
				break;
			case ConquestRank.Rank3:
				num = 0.4f;
				break;
			case ConquestRank.Rank4:
				num = 0.6f;
				break;
			case ConquestRank.Rank5:
				num = 0.8f;
				break;
			case ConquestRank.Rank6:
				num = 1f;
				break;
			default:
				num = 0f;
				break;
			}
			float num2 = num;
			if (factionIdentifier == "Puppeteers")
			{
				num2 *= 0.25f;
			}
			return num2;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000118AC File Offset: 0x0000FAAC
		public static float GetReputationBonus(this ConquestRank level)
		{
			float result;
			switch (level)
			{
			case ConquestRank.Rank1:
				result = 0.03f;
				break;
			case ConquestRank.Rank2:
				result = 0.06f;
				break;
			case ConquestRank.Rank3:
				result = 0.1f;
				break;
			case ConquestRank.Rank4:
				result = 0.15f;
				break;
			case ConquestRank.Rank5:
				result = 0.2f;
				break;
			case ConquestRank.Rank6:
				result = 0.25f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00011914 File Offset: 0x0000FB14
		public static float GetFleetStrengthRewardBonus(this ConquestRank level, string factionIdentifier)
		{
			if (factionIdentifier == "Puppeteers")
			{
				return 0f;
			}
			float result;
			switch (level)
			{
			case ConquestRank.Rank1:
				result = 0.1f;
				break;
			case ConquestRank.Rank2:
				result = 0.2f;
				break;
			case ConquestRank.Rank3:
				result = 0.4f;
				break;
			case ConquestRank.Rank4:
				result = 0.6f;
				break;
			case ConquestRank.Rank5:
				result = 0.8f;
				break;
			case ConquestRank.Rank6:
				result = 1f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00011990 File Offset: 0x0000FB90
		public static float GetCreditRewardMultiplier(this ConquestRank level)
		{
			float result;
			switch (level)
			{
			case ConquestRank.Rank1:
				result = 0.04f;
				break;
			case ConquestRank.Rank2:
				result = 0.1f;
				break;
			case ConquestRank.Rank3:
				result = 0.16f;
				break;
			case ConquestRank.Rank4:
				result = 0.24f;
				break;
			case ConquestRank.Rank5:
				result = 0.32f;
				break;
			case ConquestRank.Rank6:
				result = 0.4f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000119F6 File Offset: 0x0000FBF6
		public static bool UnlocksDestroyer(this ConquestRank level, string factionIdentifier)
		{
			return !(factionIdentifier == "Puppeteers") && level == ConquestRankExtension.DestroyerRank();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00011A0F File Offset: 0x0000FC0F
		public static bool HasDestroyerUnlocked(this ConquestRank level, string factionIdentifier)
		{
			return !(factionIdentifier == "Puppeteers") && level >= ConquestRankExtension.DestroyerRank();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00011A2B File Offset: 0x0000FC2B
		public static ConquestRank DestroyerRank()
		{
			return ConquestRank.Rank2;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00011A2E File Offset: 0x0000FC2E
		public static void AddFactionSpecificRewards(this ConquestRank rank, string factionIdentifier)
		{
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00011A30 File Offset: 0x0000FC30
		public static IEnumerable<DecalDefinition> GetDecalUnlocks(this ConquestRank rank, string factionIdentifier)
		{
			return from d in Decals.GetAll()
			where d.conquestUnlock != null && d.conquestUnlock.factionId == factionIdentifier && d.conquestUnlock.minRank == rank
			select d;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00011A68 File Offset: 0x0000FC68
		public static IEnumerable<DecalDefinition> GetAllUnlockedDecals(this ConquestRank rank, string factionIdentifier)
		{
			return from d in Decals.GetAll()
			where d.conquestUnlock != null && d.conquestUnlock.factionId == factionIdentifier && d.conquestUnlock.minRank <= rank
			select d;
		}

		// Token: 0x0400014C RID: 332
		public const int MaxConquestContribution = 4500;

		// Token: 0x0400014D RID: 333
		public static readonly Dictionary<ConquestRank, int> ConquestRankThresholds = new Dictionary<ConquestRank, int>
		{
			{
				ConquestRank.None,
				0
			},
			{
				ConquestRank.Rank1,
				10
			},
			{
				ConquestRank.Rank2,
				150
			},
			{
				ConquestRank.Rank3,
				450
			},
			{
				ConquestRank.Rank4,
				1000
			},
			{
				ConquestRank.Rank5,
				2500
			},
			{
				ConquestRank.Rank6,
				4500
			}
		};
	}
}
