using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Unit;
using Source.Item;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200004A RID: 74
	public static class UnitRankHelper
	{
		// Token: 0x06000313 RID: 787 RVA: 0x00018B34 File Offset: 0x00016D34
		public static UnitRank GetRandomUnitRankForLevel(int level, bool isBoss = false)
		{
			int num = 6;
			int num2 = 12;
			int num3 = 40;
			int num4 = 70;
			int num5 = 90;
			Dictionary<UnitRank, float> source = new Dictionary<UnitRank, float>();
			if (!isBoss)
			{
				float value = 60f * Mathf.Exp(-0.02f * (float)level);
				float value2 = (level < num) ? 0f : (35f * Mathf.Exp(-0.012f * (float)(level - num)));
				float value3 = (level < num2) ? 0f : (5f + 0.45f * (float)level);
				source = new Dictionary<UnitRank, float>
				{
					{
						UnitRank.Rookie,
						value
					},
					{
						UnitRank.Standard,
						value2
					},
					{
						UnitRank.Veteran,
						value3
					}
				};
			}
			else
			{
				float value4 = Mathf.Max(40f - (float)(level - 1) * 0.3f, 5f);
				float value5 = (level < num3) ? 0f : Mathf.Min(30f, 5f + (float)(level - 40) * 0.25f);
				float value6 = (level < num4) ? 0f : Mathf.Min(15f, 5f + (float)(level - 70) * 0.3f);
				float value7 = (level < num5) ? 0f : Mathf.Min(15f, (float)(level - 90) * 0.2f);
				source = new Dictionary<UnitRank, float>
				{
					{
						UnitRank.Elite,
						value4
					},
					{
						UnitRank.Champion,
						value5
					},
					{
						UnitRank.Commander,
						value6
					},
					{
						UnitRank.Legendary,
						value7
					}
				};
			}
			List<KeyValuePair<UnitRank, float>> list = (from kv in source
			where kv.Value > 0f
			select kv).ToList<KeyValuePair<UnitRank, float>>();
			float num6 = list.Sum((KeyValuePair<UnitRank, float> kv) => kv.Value);
			if (num6 > 0f)
			{
				float num7 = SeededRandom.Global.RandomRange(0f, num6);
				float num8 = 0f;
				foreach (KeyValuePair<UnitRank, float> keyValuePair in list)
				{
					num8 += keyValuePair.Value;
					if (num7 <= num8)
					{
						return keyValuePair.Key;
					}
				}
				return list.Last<KeyValuePair<UnitRank, float>>().Key;
			}
			if (!isBoss)
			{
				return UnitRank.Rookie;
			}
			return UnitRank.Elite;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00018D7C File Offset: 0x00016F7C
		public static string GetRankName(UnitRank rank)
		{
			string result;
			if (!UnitRankHelper.displayNames.TryGetValue(rank, out result))
			{
				return rank.ToString();
			}
			return result;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00018DA8 File Offset: 0x00016FA8
		public static Color GetColor(UnitRank rank)
		{
			Color result;
			if (!UnitRankHelper.rankColors.TryGetValue(rank, out result))
			{
				return Color.white;
			}
			return result;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x00018DCC File Offset: 0x00016FCC
		public static float GetStatMultiplier(this UnitRank rank)
		{
			float result;
			if (!UnitRankHelper.statMultiplier.TryGetValue(rank, out result))
			{
				return 0.7f;
			}
			return result;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00018DF0 File Offset: 0x00016FF0
		public static float GetStatAddition(this UnitRank rank)
		{
			float result;
			if (!UnitRankHelper.additionMultiplier.TryGetValue(rank, out result))
			{
				return -0.3f;
			}
			return result;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00018E14 File Offset: 0x00017014
		public static float GetAdjustedPoints(this UnitRank rank)
		{
			float result;
			if (UnitRankHelper.pointMultiplier.TryGetValue(rank, out result))
			{
				return result;
			}
			return 1f;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00018E38 File Offset: 0x00017038
		public static float ApplyRankMultiplier(UnitRank rank, EquipStat idx, float baseValue)
		{
			float num = baseValue;
			float num2;
			if (idx.IsPercentageStat() && UnitRankHelper.statMultiplier.TryGetValue(rank, out num2))
			{
				return num * num2;
			}
			float num3;
			if (UnitRankHelper.statMultiplier.TryGetValue(rank, out num3))
			{
				num *= num3;
			}
			return num;
		}

		// Token: 0x0400019E RID: 414
		private static readonly Dictionary<UnitRank, string> displayNames = new Dictionary<UnitRank, string>
		{
			{
				UnitRank.Rookie,
				"Rookie"
			},
			{
				UnitRank.Standard,
				"Regular"
			},
			{
				UnitRank.Veteran,
				"Veteran"
			},
			{
				UnitRank.Elite,
				"Elite"
			},
			{
				UnitRank.Champion,
				"Champion"
			},
			{
				UnitRank.Commander,
				"Commander"
			},
			{
				UnitRank.Legendary,
				"Legendary"
			}
		};

		// Token: 0x0400019F RID: 415
		private static readonly Dictionary<UnitRank, Color> rankColors = new Dictionary<UnitRank, Color>
		{
			{
				UnitRank.Rookie,
				new Color(0.7f, 0.7f, 0.7f, 1f)
			},
			{
				UnitRank.Standard,
				new Color(0.5f, 0.7f, 1f, 1f)
			},
			{
				UnitRank.Veteran,
				new Color(0.2f, 0.8f, 0.2f, 1f)
			},
			{
				UnitRank.Elite,
				new Color(1f, 0.85f, 0.2f, 1f)
			},
			{
				UnitRank.Champion,
				new Color(1f, 0.6f, 0.2f, 1f)
			},
			{
				UnitRank.Commander,
				new Color(0.6f, 0.3f, 1f, 1f)
			},
			{
				UnitRank.Legendary,
				new Color(1f, 0.2f, 0.2f, 1f)
			}
		};

		// Token: 0x040001A0 RID: 416
		private static readonly Dictionary<UnitRank, float> statMultiplier = new Dictionary<UnitRank, float>
		{
			{
				UnitRank.Rookie,
				0.7f
			},
			{
				UnitRank.Standard,
				0.9f
			},
			{
				UnitRank.Veteran,
				1.1f
			},
			{
				UnitRank.Elite,
				1.3f
			},
			{
				UnitRank.Champion,
				1.6f
			},
			{
				UnitRank.Commander,
				2f
			},
			{
				UnitRank.Legendary,
				2.4f
			}
		};

		// Token: 0x040001A1 RID: 417
		private static readonly Dictionary<UnitRank, float> additionMultiplier = new Dictionary<UnitRank, float>
		{
			{
				UnitRank.Rookie,
				-0.3f
			},
			{
				UnitRank.Standard,
				-0.1f
			},
			{
				UnitRank.Veteran,
				0.1f
			},
			{
				UnitRank.Elite,
				0.15f
			},
			{
				UnitRank.Champion,
				0.2f
			},
			{
				UnitRank.Commander,
				0.3f
			},
			{
				UnitRank.Legendary,
				0.3f
			}
		};

		// Token: 0x040001A2 RID: 418
		private static readonly Dictionary<UnitRank, float> pointMultiplier = new Dictionary<UnitRank, float>
		{
			{
				UnitRank.Rookie,
				1f
			},
			{
				UnitRank.Standard,
				1.2f
			},
			{
				UnitRank.Veteran,
				1.3f
			},
			{
				UnitRank.Elite,
				1.5f
			},
			{
				UnitRank.Champion,
				1.6f
			},
			{
				UnitRank.Commander,
				2f
			},
			{
				UnitRank.Legendary,
				2.4f
			}
		};
	}
}
