using System;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x0200013B RID: 315
	public class StarLayerPerformantData : IJsonSource
	{
		// Token: 0x06000BB6 RID: 2998 RVA: 0x00055660 File Offset: 0x00053860
		public static StarLayerPerformantData GenerateForSector(SectorMapData sector)
		{
			SeededRandom seededRandom = new SeedGenerator().Add(sector.backgroundSeed).CreateRandom();
			return new StarLayerPerformantData
			{
				backgroundColor = new Color(seededRandom.RandomRange(0f, 0.1f), seededRandom.RandomRange(0f, 0.05f), seededRandom.RandomRange(0f, 0.15f), 1f),
				maskScale = seededRandom.RandomRange(0.5f, 3f),
				starAmount = seededRandom.RandomRange(0.7f, 0.75f),
				starSize = (float)seededRandom.RandomRange(3000, 30000),
				colorVariance = (float)seededRandom.RandomRange(10, 200)
			};
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00055724 File Offset: 0x00053924
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"backgroundColor",
					ColorUtility.ToHtmlStringRGBA(this.backgroundColor)
				},
				{
					"maskScale",
					new double?((double)this.maskScale)
				},
				{
					"starAmount",
					new double?((double)this.starAmount)
				},
				{
					"starSize",
					new double?((double)this.starSize)
				},
				{
					"colorVariance",
					new double?((double)this.colorVariance)
				}
			};
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x000557CC File Offset: 0x000539CC
		public static StarLayerPerformantData FromJson(JsonValue json)
		{
			StarLayerPerformantData starLayerPerformantData = new StarLayerPerformantData();
			ColorUtility.TryParseHtmlString("#" + json["backgroundColor"], out starLayerPerformantData.backgroundColor);
			starLayerPerformantData.maskScale = (float)json["maskScale"].AsNumber;
			starLayerPerformantData.starAmount = (float)json["starAmount"].AsNumber;
			starLayerPerformantData.starSize = (float)json["starSize"].AsNumber;
			starLayerPerformantData.colorVariance = (float)json["colorVariance"].AsNumber;
			return starLayerPerformantData;
		}

		// Token: 0x0400067A RID: 1658
		public Color backgroundColor;

		// Token: 0x0400067B RID: 1659
		public float maskScale;

		// Token: 0x0400067C RID: 1660
		public float starAmount;

		// Token: 0x0400067D RID: 1661
		public float starSize;

		// Token: 0x0400067E RID: 1662
		public float colorVariance;
	}
}
