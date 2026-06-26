using System;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000138 RID: 312
	public class SmokeBackgroundData : IJsonSource
	{
		// Token: 0x06000BAB RID: 2987 RVA: 0x00054FE8 File Offset: 0x000531E8
		public static SmokeBackgroundData GenerateForSystem(SystemMapData system, int distance)
		{
			SmokeBackgroundData smokeBackgroundData = new SmokeBackgroundData();
			float num = (system.sector.backgroundSeed + system.position.x + system.position.y) * (float)distance;
			SeededRandom seededRandom = new SeedGenerator().Add(num).CreateRandom();
			smokeBackgroundData.seed = num;
			smokeBackgroundData.noiseTileFactor = seededRandom.RandomRange(0.05f, 1f);
			smokeBackgroundData.noiseScale = (float)seededRandom.RandomRange((distance == 1) ? 40 : 10, (distance == 1) ? 100 : 30);
			smokeBackgroundData.maskScale = (float)seededRandom.RandomRange(2, 10);
			smokeBackgroundData.colorPower = seededRandom.RandomRange(0.5f, 2f);
			smokeBackgroundData.distance = distance;
			smokeBackgroundData.color = Color.Lerp(new Color(0.4f, 0.4f, 0.4f), new Color(0.2f, 0.2f, 0.2f), seededRandom.RandomRange(0f, 1f));
			smokeBackgroundData.sortingLayerName = ((distance == 1) ? "BackgroundLit" : "Planets");
			return smokeBackgroundData;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x00055100 File Offset: 0x00053300
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"color",
					ColorUtility.ToHtmlStringRGBA(this.color)
				},
				{
					"noiseTileFactor",
					new double?((double)this.noiseTileFactor)
				},
				{
					"noiseScale",
					new double?((double)this.noiseScale)
				},
				{
					"maskScale",
					new double?((double)this.maskScale)
				},
				{
					"colorPower",
					new double?((double)this.colorPower)
				},
				{
					"distance",
					new double?((double)this.distance)
				},
				{
					"sortingLayerName",
					this.sortingLayerName
				}
			};
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x000551DC File Offset: 0x000533DC
		public static SmokeBackgroundData FromJson(JsonValue json)
		{
			SmokeBackgroundData smokeBackgroundData = new SmokeBackgroundData();
			ColorUtility.TryParseHtmlString("#" + json["color"], out smokeBackgroundData.color);
			smokeBackgroundData.noiseTileFactor = (float)json["noiseTileFactor"].AsNumber;
			smokeBackgroundData.noiseScale = (float)json["noiseScale"].AsNumber;
			smokeBackgroundData.maskScale = (float)json["maskScale"].AsNumber;
			smokeBackgroundData.colorPower = (float)json["colorPower"].AsNumber;
			smokeBackgroundData.distance = json["distance"].AsInteger;
			smokeBackgroundData.sortingLayerName = json["sortingLayerName"];
			return smokeBackgroundData;
		}

		// Token: 0x04000669 RID: 1641
		public float noiseTileFactor;

		// Token: 0x0400066A RID: 1642
		public float noiseScale;

		// Token: 0x0400066B RID: 1643
		public float maskScale;

		// Token: 0x0400066C RID: 1644
		public float colorPower;

		// Token: 0x0400066D RID: 1645
		public int distance;

		// Token: 0x0400066E RID: 1646
		public Color color;

		// Token: 0x0400066F RID: 1647
		public string sortingLayerName;

		// Token: 0x04000670 RID: 1648
		public float seed;
	}
}
