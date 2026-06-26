using System;
using System.Globalization;
using Source.Galaxy;
using Source.Player;
using Source.Simulation;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000030 RID: 48
	public class GameMath
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00011BCC File Offset: 0x0000FDCC
		public static int maxLevel
		{
			get
			{
				if (GamePlayer.current == null)
				{
					return 100;
				}
				int num = 0;
				foreach (Storyteller storyteller in GamePlayer.current.storytellers)
				{
					num = Math.Max(num, storyteller.maxPlayerLevel);
				}
				return num;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00011C38 File Offset: 0x0000FE38
		public static int maxReputation
		{
			get
			{
				if (GamePlayer.current == null || GamePlayer.current.storytellers.Count == 0)
				{
					return ReputationLevel.Exalted.GetReputationThreshold();
				}
				int num = 0;
				foreach (Storyteller storyteller in GamePlayer.current.storytellers)
				{
					num = Math.Max(num, storyteller.maxReputation);
				}
				return num;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00011CB8 File Offset: 0x0000FEB8
		public static int MaxBonusSkillPoints
		{
			get
			{
				if (GamePlayer.current == null || GamePlayer.current.storytellers.Count == 0)
				{
					return 0;
				}
				int num = 0;
				foreach (Storyteller storyteller in GamePlayer.current.storytellers)
				{
					num = Math.Max(num, storyteller.maxBonusSkillpoints);
				}
				return num;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00011D34 File Offset: 0x0000FF34
		public static int MaxLootBoxSkillPoints
		{
			get
			{
				if (GamePlayer.current == null || GamePlayer.current.storytellers.Count == 0)
				{
					return 0;
				}
				int num = 0;
				foreach (Storyteller storyteller in GamePlayer.current.storytellers)
				{
					num = Math.Max(num, storyteller.maxLootBoxSkillPoints);
				}
				return num;
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00011DB0 File Offset: 0x0000FFB0
		public static float ConquestHealthMultiplier(int playerLevel)
		{
			return 1f + Mathf.Clamp01((float)((playerLevel - 30) / 30));
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00011DC5 File Offset: 0x0000FFC5
		public static float GetMaxExperienceForLevel(int level)
		{
			return 1000f * Mathf.Pow(2f, (float)level / 5f);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00011DE0 File Offset: 0x0000FFE0
		public static int GetLevelFromExperience(float xp)
		{
			if (xp <= 0f)
			{
				return 0;
			}
			int b = Mathf.FloorToInt(5f * Mathf.Log(xp / 1000f, 2f));
			return Mathf.Max(0, b);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00011E1B File Offset: 0x0001001B
		public static int GetCreditsValue(float baseAmount, int level)
		{
			return Mathf.RoundToInt(baseAmount * 100f * GameMath.CostMultiplier(level));
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00011E30 File Offset: 0x00010030
		public static int GetExperienceRewardValue(float baseAmount, int targetLevel)
		{
			int level = GamePlayer.current.commander.level;
			float num = 5f * baseAmount;
			if (level > targetLevel)
			{
				int num2 = level - targetLevel;
				if (num2 > 3)
				{
					num = 1E-05f;
				}
				else
				{
					num *= 1f - 0.2f * (float)num2;
				}
			}
			else if (targetLevel > level)
			{
				int num3 = Mathf.Min(3, targetLevel - level);
				num *= 1f + 0.1f * (float)num3;
			}
			return Mathf.CeilToInt(num * Mathf.Pow(2f, (float)level / 9f));
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00011EB4 File Offset: 0x000100B4
		public static float DamageMultiplier(float level)
		{
			return Mathf.Pow(2f, level / 10f);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00011EC7 File Offset: 0x000100C7
		public static float CostMultiplier(int level)
		{
			return Mathf.Pow(2f, (float)level / 14f);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00011EDC File Offset: 0x000100DC
		public static string FormatNumber(float num, int decimals = -1)
		{
			if (num == float.PositiveInfinity)
			{
				Debug.LogError("Een infinite doen we niet aan, ga toch weg!");
				return "";
			}
			if (num >= 1000f)
			{
				decimals = 0;
			}
			if (decimals == -1)
			{
				decimals = ((num > 0f && num < 1f) ? 1 : 0);
			}
			string str;
			if (num >= 10000f)
			{
				int num2 = 0;
				do
				{
					num /= 1000f;
					num2++;
				}
				while (num >= 1000f);
				switch (num2)
				{
				case 1:
					str = "K";
					break;
				case 2:
					str = "M";
					break;
				case 3:
					str = "B";
					break;
				case 4:
					str = "T";
					break;
				case 5:
					str = "Qa";
					break;
				case 6:
					str = "Qi";
					break;
				case 7:
					str = "Sx";
					break;
				case 8:
					str = "Sp";
					break;
				case 9:
					str = "Oc";
					break;
				default:
					str = "Xx";
					break;
				}
				if (num < 1f)
				{
					decimals = 3;
				}
				else if (num < 10f)
				{
					decimals = 2;
				}
				else if (num < 100f)
				{
					decimals = 1;
				}
				else
				{
					decimals = 0;
				}
			}
			else
			{
				str = "";
			}
			return num.ToString("N" + decimals.ToString(), new CultureInfo(Translation.CurrentLocale)) + str;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00012020 File Offset: 0x00010220
		public static string FormatTime(int tSec, bool shortFormat = false)
		{
			bool flag = tSec < 0;
			if (flag)
			{
				tSec *= -1;
			}
			int num = tSec % 60;
			int num2 = tSec % 3600 / 60;
			int num3 = tSec / 3600;
			if (shortFormat)
			{
				return string.Concat(new string[]
				{
					flag ? "-" : "",
					num3.ToString(),
					":",
					num2.ToString().PadLeft(2, '0'),
					":",
					num.ToString().PadLeft(2, '0')
				});
			}
			return (flag ? "-" : "") + Translation.TranslateOnly((num3 > 0) ? "@TimeFormatHMS" : "@TimeFormatMS", new object[]
			{
				num3,
				num2,
				num
			});
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000120FC File Offset: 0x000102FC
		public static string FormatPercentage(float percentage, FormatPercentageMode mode = FormatPercentageMode.Default, int decimals = 1)
		{
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(new CultureInfo(Translation.CurrentLocale));
			instance.PercentPositivePattern = 1;
			string text;
			if (mode == FormatPercentageMode.Default)
			{
				text = percentage.ToString("P" + decimals.ToString(), instance);
			}
			else
			{
				percentage -= 1f;
				text = ((percentage < 0f) ? "" : "+") + percentage.ToString("P" + decimals.ToString(), instance);
			}
			if (text.EndsWith(".0%"))
			{
				text = text.Substring(0, text.Length - 3) + "%";
			}
			return text;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x000121A4 File Offset: 0x000103A4
		public static bool LineIntersects(Vector2 lineOneA, Vector2 lineOneB, Vector2 lineTwoA, Vector2 lineTwoB)
		{
			return (lineTwoB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x) > (lineTwoA.y - lineOneA.y) * (lineTwoB.x - lineOneA.x) != (lineTwoB.y - lineOneB.y) * (lineTwoA.x - lineOneB.x) > (lineTwoA.y - lineOneB.y) * (lineTwoB.x - lineOneB.x) && (lineTwoA.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoA.x - lineOneA.x) != (lineTwoB.y - lineOneA.y) * (lineOneB.x - lineOneA.x) > (lineOneB.y - lineOneA.y) * (lineTwoB.x - lineOneA.x);
		}

		// Token: 0x0400014E RID: 334
		public const float BaseDamage = 5f;

		// Token: 0x0400014F RID: 335
		public const float BaseValue = 100f;

		// Token: 0x04000150 RID: 336
		public const float ShipBaseValue = 65000f;

		// Token: 0x04000151 RID: 337
		public const float ShipBaseCommendationValue = 3.5f;

		// Token: 0x04000152 RID: 338
		public const float ShipSellValue = 0.4f;

		// Token: 0x04000153 RID: 339
		public const float ShipHPScale = 150f;

		// Token: 0x04000154 RID: 340
		public const float ShipArmorScale = 300f;

		// Token: 0x04000155 RID: 341
		public const float ShipShieldScale = 300f;

		// Token: 0x04000156 RID: 342
		public const float FastLaneTravelMultiplier = 7f;

		// Token: 0x04000157 RID: 343
		public const float LootBoxOpenPrice = 50f;

		// Token: 0x04000158 RID: 344
		public const float LootBoxChanceSalvage = 0.01f;

		// Token: 0x04000159 RID: 345
		public const float LootBoxChanceSalvageLastHit = 0.05f;

		// Token: 0x0400015A RID: 346
		public const float CoreDamageMultiplier = 1.2f;

		// Token: 0x0400015B RID: 347
		public const float BaseOreHealth = 40f;

		// Token: 0x0400015C RID: 348
		public const float OreValueMultiplier = 3f;

		// Token: 0x0400015D RID: 349
		public const float IdleCreditRewardFactor = 0.33f;

		// Token: 0x0400015E RID: 350
		public const float IdleExperienceRewardFactor = 0.5f;

		// Token: 0x0400015F RID: 351
		public const float IdleReputationRewardFactor = 0.1f;

		// Token: 0x04000160 RID: 352
		public const int IdleAmmoPurchaseDuration = 180;

		// Token: 0x04000161 RID: 353
		private const float DamageDoublingLevels = 10f;

		// Token: 0x04000162 RID: 354
		private const float CostDoublingLevels = 14f;

		// Token: 0x04000163 RID: 355
		private const float ExperienceDoublingLevels = 5f;

		// Token: 0x04000164 RID: 356
		private const float ExperienceRewardDoublingLevels = 9f;

		// Token: 0x04000165 RID: 357
		public static int minReputation = ReputationLevel.AbsoluteThreat.GetReputationThreshold();
	}
}
