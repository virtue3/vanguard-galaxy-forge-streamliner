using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200003E RID: 62
	public static class SalvageHelper
	{
		// Token: 0x060002CF RID: 719 RVA: 0x000173D9 File Offset: 0x000155D9
		public static string BuildScrapItemName(string material, int tier)
		{
			if (tier > 1)
			{
				return string.Format("Salvage{0}Tier{1}", material, tier);
			}
			return "Salvage" + material;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000173FC File Offset: 0x000155FC
		public static int RollTier(int level)
		{
			return SalvageHelper.RollWeighted(SalvageHelper.GetScrapTierWeights(level));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0001740C File Offset: 0x0001560C
		private static Dictionary<int, float> GetScrapTierWeights(int level)
		{
			Dictionary<int, float> dictionary = new Dictionary<int, float>();
			foreach (KeyValuePair<int, float> keyValuePair in SalvageHelper.scrapTierCenters)
			{
				float num = Mathf.Abs((float)level - keyValuePair.Value);
				float value = Mathf.Clamp01(1f - num / 40f);
				dictionary[keyValuePair.Key] = value;
			}
			return dictionary;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00017494 File Offset: 0x00015694
		private static int RollWeighted(Dictionary<int, float> weights)
		{
			float num = weights.Values.Sum();
			float num2 = SeededRandom.Global.RandomFloat() * num;
			foreach (KeyValuePair<int, float> keyValuePair in weights)
			{
				num2 -= keyValuePair.Value;
				if (num2 <= 0f)
				{
					return keyValuePair.Key;
				}
			}
			return 1;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00017518 File Offset: 0x00015718
		public static float GetQuantityMultiplier(int level, int tier)
		{
			float num = SalvageHelper.scrapTierCenters[tier];
			if ((float)level <= num)
			{
				return 1f;
			}
			float num2 = ((float)level - num) * 0.02f;
			return 1f + num2;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0001754E File Offset: 0x0001574E
		public static int GetScrapAmount(int level, int tier, int size)
		{
			return Mathf.CeilToInt((float)Mathf.CeilToInt((float)SeededRandom.Global.RandomRange(6, 12) * SalvageHelper.GetQuantityMultiplier(level, tier)) * SalvageHelper.GetSizeMultiplier(size));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00017578 File Offset: 0x00015778
		public static float GetSizeMultiplier(int size)
		{
			if (size < 3)
			{
				return 1f;
			}
			if (size < 5)
			{
				return 1.5f;
			}
			return 2f;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00017593 File Offset: 0x00015793
		public static int GetSizeMultiplierStructure(int size)
		{
			if (size < 3)
			{
				return 1;
			}
			if (size < 5)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x04000176 RID: 374
		private static readonly Dictionary<int, float> scrapTierCenters = new Dictionary<int, float>
		{
			{
				1,
				10f
			},
			{
				2,
				30f
			},
			{
				3,
				50f
			},
			{
				4,
				80f
			},
			{
				5,
				110f
			},
			{
				6,
				140f
			}
		};

		// Token: 0x04000177 RID: 375
		public static readonly List<string> surfaceMaterials = new List<string>
		{
			"Titanium",
			"Oxide",
			"Tungsten",
			"Silicon",
			"Carbon"
		};

		// Token: 0x04000178 RID: 376
		public static readonly List<string> structuralMaterials = new List<string>
		{
			"Platinum",
			"Iridium",
			"Astatine"
		};

		// Token: 0x04000179 RID: 377
		private const int StructuralAmountPenalty = 6;

		// Token: 0x0400017A RID: 378
		private const float ScrapSpread = 40f;

		// Token: 0x0400017B RID: 379
		private const int MinScrapAmount = 6;

		// Token: 0x0400017C RID: 380
		private const int MaxScrapAmount = 12;
	}
}
