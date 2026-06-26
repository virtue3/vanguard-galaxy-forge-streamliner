using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Source.Galaxy;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200003D RID: 61
	public static class ReputationLevelExtensions
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x00016A58 File Offset: 0x00014C58
		public static ReputationLevel GetReputationLevel(int reputation)
		{
			if (reputation > 0)
			{
				using (IEnumerator<KeyValuePair<ReputationLevel, int>> enumerator = (from t in ReputationLevelExtensions.ReputationThresholds
				orderby t.Value descending
				select t).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ReputationLevel, int> keyValuePair = enumerator.Current;
						if (reputation >= keyValuePair.Value)
						{
							return keyValuePair.Key;
						}
					}
					return ReputationLevel.Neutral;
				}
			}
			foreach (KeyValuePair<ReputationLevel, int> keyValuePair2 in from t in ReputationLevelExtensions.ReputationThresholds
			orderby t.Value
			select t)
			{
				if (reputation <= keyValuePair2.Value)
				{
					return keyValuePair2.Key;
				}
			}
			return ReputationLevel.Neutral;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00016B48 File Offset: 0x00014D48
		public static string GetReputationCategory(this ReputationLevel level)
		{
			string result;
			switch (level)
			{
			case ReputationLevel.AbsoluteThreat:
			case ReputationLevel.Hated:
			case ReputationLevel.Despised:
			case ReputationLevel.Hostile:
				result = "Negative";
				break;
			case ReputationLevel.Wary:
			case ReputationLevel.Neutral:
				result = "Neutral";
				break;
			case ReputationLevel.Cordial:
			case ReputationLevel.Friendly:
			case ReputationLevel.Respected:
			case ReputationLevel.Distinguished:
			case ReputationLevel.Exalted:
				result = "Positive";
				break;
			default:
				throw new ArgumentOutOfRangeException("level", string.Format("Unknown reputation level: {0}", level));
			}
			return result;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00016BC0 File Offset: 0x00014DC0
		public static int GetReputationThreshold(this ReputationLevel level)
		{
			int result;
			if (ReputationLevelExtensions.ReputationThresholds.TryGetValue(level, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("level", string.Format("No threshold defined for reputation level: {0}", level));
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00016BF8 File Offset: 0x00014DF8
		public static Color GetReputationColor(this ReputationLevel level)
		{
			Color result;
			switch (level)
			{
			case ReputationLevel.AbsoluteThreat:
				result = new Color(0.5f, 0f, 0f);
				break;
			case ReputationLevel.Hated:
				result = Color.red;
				break;
			case ReputationLevel.Despised:
				result = new Color(1f, 0.27f, 0f);
				break;
			case ReputationLevel.Hostile:
				result = new Color(1f, 0.65f, 0f);
				break;
			case ReputationLevel.Wary:
				result = new Color(0.85f, 0.65f, 0.13f);
				break;
			case ReputationLevel.Neutral:
				result = Color.gray;
				break;
			case ReputationLevel.Cordial:
				result = new Color(0.56f, 0.93f, 0.56f);
				break;
			case ReputationLevel.Friendly:
				result = Color.green;
				break;
			case ReputationLevel.Respected:
				result = new Color(0.13f, 0.55f, 0.13f);
				break;
			case ReputationLevel.Distinguished:
				result = new Color(0.24f, 0.7f, 0.44f);
				break;
			case ReputationLevel.Exalted:
				result = new Color(0f, 0.39f, 0f);
				break;
			default:
				throw new ArgumentOutOfRangeException("level", string.Format("No color defined for reputation level: {0}", level));
			}
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00016D38 File Offset: 0x00014F38
		public static int GetNextLevelRequirement(int reputation)
		{
			foreach (KeyValuePair<ReputationLevel, int> keyValuePair in from t in ReputationLevelExtensions.ReputationThresholds
			orderby t.Value
			select t)
			{
				if (reputation < keyValuePair.Value)
				{
					return keyValuePair.Value - reputation;
				}
			}
			return 0;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00016DBC File Offset: 0x00014FBC
		public static int GetCurrentLevelProgress(int reputation)
		{
			foreach (KeyValuePair<ReputationLevel, int> keyValuePair in (from t in ReputationLevelExtensions.ReputationThresholds
			orderby t.Value descending
			select t).ToList<KeyValuePair<ReputationLevel, int>>())
			{
				if (reputation >= keyValuePair.Value)
				{
					return reputation - keyValuePair.Value;
				}
			}
			return 0;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00016E4C File Offset: 0x0001504C
		public static int GetCurrentLevelRange(int reputation)
		{
			List<KeyValuePair<ReputationLevel, int>> list = (from t in ReputationLevelExtensions.ReputationThresholds
			orderby t.Value descending
			select t).ToList<KeyValuePair<ReputationLevel, int>>();
			for (int i = 0; i < list.Count - 1; i++)
			{
				if (reputation >= list[i].Value)
				{
					return list[i].Value - list[i + 1].Value;
				}
			}
			return list.Last<KeyValuePair<ReputationLevel, int>>().Value - ReputationLevelExtensions.ReputationThresholds[ReputationLevel.AbsoluteThreat];
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00016EEC File Offset: 0x000150EC
		public static int GetNextLevelRange(int reputation)
		{
			List<KeyValuePair<ReputationLevel, int>> list = (from t in ReputationLevelExtensions.ReputationThresholds
			orderby t.Value
			select t).ToList<KeyValuePair<ReputationLevel, int>>();
			for (int i = 0; i < list.Count - 1; i++)
			{
				if (reputation < list[i].Value)
				{
					return list[i].Value - list[i - 1].Value;
				}
			}
			return list.Last<KeyValuePair<ReputationLevel, int>>().Value - ReputationLevelExtensions.ReputationThresholds[ReputationLevel.AbsoluteThreat];
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00016F8C File Offset: 0x0001518C
		public static int GetNextLevelThreshold(int reputation)
		{
			List<KeyValuePair<ReputationLevel, int>> list = (from t in ReputationLevelExtensions.ReputationThresholds
			orderby t.Value
			select t).ToList<KeyValuePair<ReputationLevel, int>>();
			foreach (KeyValuePair<ReputationLevel, int> keyValuePair in list)
			{
				if (reputation < keyValuePair.Value)
				{
					return keyValuePair.Value;
				}
			}
			return list.Last<KeyValuePair<ReputationLevel, int>>().Value - ReputationLevelExtensions.ReputationThresholds[ReputationLevel.AbsoluteThreat];
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00017034 File Offset: 0x00015234
		public static float GetTotalReputationProgress(int reputation)
		{
			int num = ReputationLevelExtensions.ReputationThresholds[ReputationLevel.AbsoluteThreat];
			int num2 = ReputationLevelExtensions.ReputationThresholds[ReputationLevel.Exalted];
			reputation = Math.Clamp(reputation, num, num2);
			return (float)(reputation - num) / (float)(num2 - num);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0001706D File Offset: 0x0001526D
		public static bool IsLevelOrHigher(ReputationLevel current, ReputationLevel target)
		{
			return current.GetReputationThreshold() >= target.GetReputationThreshold();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00017080 File Offset: 0x00015280
		public static ValueTuple<int, int> GetAbsoluteProgressToTarget(int reputation, ReputationLevel targetLevel)
		{
			int num = ReputationLevelExtensions.ReputationThresholds[targetLevel];
			return new ValueTuple<int, int>(Math.Min(reputation, num), num);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000170A6 File Offset: 0x000152A6
		public static float GetShopDiscount(int reputation)
		{
			return ReputationLevelExtensions.GetReputationLevel(reputation).GetShopDiscount();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000170B4 File Offset: 0x000152B4
		public static float GetShopDiscount(this ReputationLevel level)
		{
			float result;
			switch (level)
			{
			case ReputationLevel.Cordial:
				result = 0.02f;
				break;
			case ReputationLevel.Friendly:
				result = 0.04f;
				break;
			case ReputationLevel.Respected:
				result = 0.06f;
				break;
			case ReputationLevel.Distinguished:
				result = 0.08f;
				break;
			case ReputationLevel.Exalted:
				result = 0.1f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0001710E File Offset: 0x0001530E
		public static bool CanRefreshShop(this ReputationLevel level)
		{
			return level >= ReputationLevel.Friendly;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00017118 File Offset: 0x00015318
		public static int GetShopRefreshTokens(this ReputationLevel level)
		{
			int result;
			switch (level)
			{
			case ReputationLevel.Friendly:
				result = 1;
				break;
			case ReputationLevel.Respected:
				result = 1;
				break;
			case ReputationLevel.Distinguished:
				result = 2;
				break;
			case ReputationLevel.Exalted:
				result = 2;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00017154 File Offset: 0x00015354
		public static float GetMissionRewardMultiplier(this ReputationLevel level)
		{
			float result;
			switch (level)
			{
			case ReputationLevel.Cordial:
				result = 0.05f;
				break;
			case ReputationLevel.Friendly:
				result = 0.1f;
				break;
			case ReputationLevel.Respected:
				result = 0.2f;
				break;
			case ReputationLevel.Distinguished:
				result = 0.3f;
				break;
			case ReputationLevel.Exalted:
				result = 0.4f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x000171B0 File Offset: 0x000153B0
		public static int GetBonusMissionAmount(this ReputationLevel level)
		{
			int result;
			switch (level)
			{
			case ReputationLevel.Friendly:
				result = 1;
				break;
			case ReputationLevel.Respected:
				result = 2;
				break;
			case ReputationLevel.Distinguished:
				result = 2;
				break;
			case ReputationLevel.Exalted:
				result = 3;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000171EA File Offset: 0x000153EA
		public static bool CanRefreshBoard(this ReputationLevel level)
		{
			return level >= ReputationLevel.Cordial;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000171F4 File Offset: 0x000153F4
		public static float GetMissionBoardRefreshTimer(this ReputationLevel level)
		{
			float result;
			switch (level)
			{
			case ReputationLevel.Friendly:
				result = 55f;
				break;
			case ReputationLevel.Respected:
				result = 50f;
				break;
			case ReputationLevel.Distinguished:
				result = 45f;
				break;
			case ReputationLevel.Exalted:
				result = 40f;
				break;
			default:
				result = 60f;
				break;
			}
			return result;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00017244 File Offset: 0x00015444
		public static float GetRepairSpeedMultiplier(this ReputationLevel level)
		{
			float result;
			switch (level)
			{
			case ReputationLevel.Cordial:
				result = 0.9f;
				break;
			case ReputationLevel.Friendly:
				result = 0.6f;
				break;
			case ReputationLevel.Respected:
				result = 0.4f;
				break;
			case ReputationLevel.Distinguished:
				result = 0.3f;
				break;
			case ReputationLevel.Exalted:
				result = 0.2f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x000172A0 File Offset: 0x000154A0
		public static float GetRepairCostDiscount(this ReputationLevel level)
		{
			float result;
			switch (level)
			{
			case ReputationLevel.Friendly:
				result = 0.2f;
				break;
			case ReputationLevel.Respected:
				result = 0.4f;
				break;
			case ReputationLevel.Distinguished:
				result = 0.6f;
				break;
			case ReputationLevel.Exalted:
				result = 0.8f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x000172F0 File Offset: 0x000154F0
		public static float GetShipyardDiscount(this ReputationLevel level)
		{
			float result;
			switch (level)
			{
			case ReputationLevel.Friendly:
				result = 0.04f;
				break;
			case ReputationLevel.Respected:
				result = 0.08f;
				break;
			case ReputationLevel.Distinguished:
				result = 0.12f;
				break;
			case ReputationLevel.Exalted:
				result = 0.2f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x04000175 RID: 373
		public static readonly Dictionary<ReputationLevel, int> ReputationThresholds = new Dictionary<ReputationLevel, int>
		{
			{
				ReputationLevel.AbsoluteThreat,
				-50000
			},
			{
				ReputationLevel.Hated,
				-30000
			},
			{
				ReputationLevel.Despised,
				-15000
			},
			{
				ReputationLevel.Hostile,
				-5000
			},
			{
				ReputationLevel.Wary,
				-500
			},
			{
				ReputationLevel.Neutral,
				0
			},
			{
				ReputationLevel.Cordial,
				1500
			},
			{
				ReputationLevel.Friendly,
				5000
			},
			{
				ReputationLevel.Respected,
				15000
			},
			{
				ReputationLevel.Distinguished,
				30000
			},
			{
				ReputationLevel.Exalted,
				50000
			}
		};
	}
}
