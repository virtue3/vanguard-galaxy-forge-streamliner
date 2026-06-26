using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Util;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000139 RID: 313
	public class SmokeBackgroundEffectData
	{
		// Token: 0x06000BAF RID: 2991 RVA: 0x000552BC File Offset: 0x000534BC
		public static SmokeBackgroundEffectData GenerateForSystem(SystemMapData system, int distance = 0)
		{
			SmokeBackgroundEffectData smokeBackgroundEffectData = new SmokeBackgroundEffectData();
			float num = system.sector.backgroundSeed / (system.position.x + system.position.y);
			SeededRandom seededRandom = new SeedGenerator().Add(num).CreateRandom();
			smokeBackgroundEffectData.distance = (float)((distance == 0) ? seededRandom.RandomRange(13, 18) : distance);
			smokeBackgroundEffectData.size = seededRandom.RandomRange(20f, 40f);
			smokeBackgroundEffectData.amount = seededRandom.RandomRange(30, 40);
			smokeBackgroundEffectData.seed = num;
			smokeBackgroundEffectData.colors = SmokeBackgroundEffectData.CreateForBackground(seededRandom, distance);
			return smokeBackgroundEffectData;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0005535C File Offset: 0x0005355C
		private static Gradient CreateForBackground(SeededRandom random, int distance)
		{
			Gradient gradient = new Gradient();
			Gradient smokeColorsAlpha = Singleton<BackdropManager>.Instance.GetSmokeColorsAlpha();
			gradient.SetKeys(smokeColorsAlpha.colorKeys, smokeColorsAlpha.alphaKeys);
			Color col = (distance == 1) ? Singleton<BackdropManager>.Instance.GetSmokeColor(random.RandomRange(0f, 1f)) : Singleton<BackdropManager>.Instance.GetNebulaColor(random.RandomRange(0f, 1f));
			Color col2 = (distance == 1) ? Singleton<BackdropManager>.Instance.GetSmokeColor(random.RandomRange(0f, 1f)) : Singleton<BackdropManager>.Instance.GetNebulaColor(random.RandomRange(0f, 1f));
			List<GradientColorKey> list = gradient.colorKeys.ToList<GradientColorKey>();
			GradientColorKey value = list[1];
			value.time = 0.9f;
			list[1] = value;
			list.Add(new GradientColorKey(col, 0.95f));
			list.Add(new GradientColorKey(col2, 1f));
			gradient.SetColorKeys(list.ToArray());
			if (distance == 1)
			{
				List<GradientAlphaKey> list2 = gradient.alphaKeys.ToList<GradientAlphaKey>();
				GradientAlphaKey value2 = list2[1];
				value2.time = 0.9f;
				list2[1] = value2;
				list2.Add(new GradientAlphaKey(random.RandomRange(0.1f, 0.3f), 0.95f));
				list2.Add(new GradientAlphaKey(random.RandomRange(0.1f, 0.3f), 1f));
				gradient.SetAlphaKeys(list2.ToArray());
			}
			return gradient;
		}

		// Token: 0x04000671 RID: 1649
		public float distance;

		// Token: 0x04000672 RID: 1650
		public Gradient colors;

		// Token: 0x04000673 RID: 1651
		public float size;

		// Token: 0x04000674 RID: 1652
		public int amount;

		// Token: 0x04000675 RID: 1653
		public float seed;
	}
}
