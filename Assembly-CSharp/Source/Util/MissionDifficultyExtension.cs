using System;
using System.Collections.Generic;
using Source.MissionSystem;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000037 RID: 55
	public static class MissionDifficultyExtension
	{
		// Token: 0x0600029F RID: 671 RVA: 0x000155A8 File Offset: 0x000137A8
		public static MissionDifficulty GetMissionDifficulty(int level, bool normalUnlocked, bool hardUnlocked, bool skullUnlocked, bool insaneUnlocked, SeededRandom random)
		{
			float a = normalUnlocked ? 0.5f : 1f;
			float a2 = normalUnlocked ? 0.3f : 0f;
			float a3 = hardUnlocked ? 0.2f : 0f;
			float a4 = skullUnlocked ? 0.2f : 0f;
			float a5 = insaneUnlocked ? 0.1f : 0f;
			float t = Mathf.Clamp((float)level / 100f, 0f, 1f);
			return random.Choose<MissionDifficulty>(new Dictionary<MissionDifficulty, float>
			{
				{
					MissionDifficulty.Easy,
					Mathf.Lerp(a, 0.1f, t)
				},
				{
					MissionDifficulty.Normal,
					normalUnlocked ? Mathf.Lerp(a2, 0.3f, t) : 0f
				},
				{
					MissionDifficulty.Hard,
					hardUnlocked ? Mathf.Lerp(a3, 0.3f, t) : 0f
				},
				{
					MissionDifficulty.Skull,
					skullUnlocked ? Mathf.Lerp(a4, 0.2f, t) : 0f
				},
				{
					MissionDifficulty.Insane,
					insaneUnlocked ? Mathf.Lerp(a5, 0.2f, t) : 0f
				}
			});
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000156BC File Offset: 0x000138BC
		public static float GetObjectiveAmountMultiplier(this MissionDifficulty missionDifficulty)
		{
			float result;
			switch (missionDifficulty)
			{
			case MissionDifficulty.Easy:
				result = 1f;
				break;
			case MissionDifficulty.Normal:
				result = 1.3f;
				break;
			case MissionDifficulty.Hard:
				result = 1.6f;
				break;
			case MissionDifficulty.Skull:
				result = 2.1f;
				break;
			case MissionDifficulty.Insane:
				result = 2.5f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00015714 File Offset: 0x00013914
		public static float GetRewardMultiplier(this MissionDifficulty missionDifficulty)
		{
			float result;
			switch (missionDifficulty)
			{
			case MissionDifficulty.Easy:
				result = 1f;
				break;
			case MissionDifficulty.Normal:
				result = 1.8f;
				break;
			case MissionDifficulty.Hard:
				result = 2.8f;
				break;
			case MissionDifficulty.Skull:
				result = 3.6f;
				break;
			case MissionDifficulty.Insane:
				result = 6f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0001576C File Offset: 0x0001396C
		public static Color GetColor(this MissionDifficulty missionDifficulty)
		{
			Color result;
			switch (missionDifficulty)
			{
			case MissionDifficulty.Easy:
				result = ColorHelper.RarityStandard;
				break;
			case MissionDifficulty.Normal:
				result = ColorHelper.RarityEnhanced;
				break;
			case MissionDifficulty.Hard:
				result = ColorHelper.RarityHighGrade;
				break;
			case MissionDifficulty.Skull:
				result = ColorHelper.RarityExotic;
				break;
			case MissionDifficulty.Insane:
				result = ColorHelper.RarityLegendary;
				break;
			default:
				result = ColorHelper.RaritySpecial;
				break;
			}
			return result;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000157C4 File Offset: 0x000139C4
		public static Color GetBackgroundColor(this MissionDifficulty missionDifficulty)
		{
			Color result;
			switch (missionDifficulty)
			{
			case MissionDifficulty.Easy:
				result = ColorHelper.RarityStandardBackground;
				break;
			case MissionDifficulty.Normal:
				result = ColorHelper.RarityEnhancedBackground;
				break;
			case MissionDifficulty.Hard:
				result = ColorHelper.RarityHighGradeBackground;
				break;
			case MissionDifficulty.Skull:
				result = ColorHelper.RarityExoticBackground;
				break;
			case MissionDifficulty.Insane:
				result = ColorHelper.RarityLegendaryBackground;
				break;
			default:
				result = ColorHelper.RaritySpecialBackground;
				break;
			}
			return result;
		}
	}
}
