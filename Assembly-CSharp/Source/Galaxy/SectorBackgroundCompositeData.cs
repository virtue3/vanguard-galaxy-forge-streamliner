using System;
using System.Collections.Generic;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000137 RID: 311
	public class SectorBackgroundCompositeData : IJsonSource
	{
		// Token: 0x06000BA6 RID: 2982 RVA: 0x00054E0C File Offset: 0x0005300C
		public SectorBackgroundCompositeData(string sectorId)
		{
			this.sectorId = sectorId;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00054E28 File Offset: 0x00053028
		public static SectorBackgroundCompositeData CreateForSector(SectorMapData sector)
		{
			SectorBackgroundCompositeData sectorBackgroundCompositeData = new SectorBackgroundCompositeData(sector.name);
			sectorBackgroundCompositeData.starLayerPerformantData = StarLayerPerformantData.GenerateForSector(sector);
			SeededRandom seededRandom = new SeedGenerator().Add(sector.backgroundSeed).CreateRandom();
			int num = seededRandom.RandomRange(1, 3);
			Vector2 position = new Vector2(seededRandom.RandomRange(-19f, 19f), seededRandom.RandomRange(-3f, 3f));
			float num2 = (float)seededRandom.RandomRange(0, 360);
			for (int i = 0; i < num; i++)
			{
				sectorBackgroundCompositeData.nebulae.Add(NebulaData.GenerateForSector(sector, position, num2));
				num2 += (float)seededRandom.RandomRange(45 * i, 270);
			}
			return sectorBackgroundCompositeData;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00054EE4 File Offset: 0x000530E4
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"globalColor",
					ColorUtility.ToHtmlStringRGBA(this.globalColor)
				},
				{
					"globalLightIntensity",
					new double?((double)this.globalLightIntensity)
				},
				{
					"nebulae",
					this.nebulae.ToJsonArray<NebulaData>()
				}
			};
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00054F50 File Offset: 0x00053150
		public static SystemBackgroundCompositeData FromJson(JsonValue json)
		{
			SystemBackgroundCompositeData systemBackgroundCompositeData = new SystemBackgroundCompositeData(json["systemId"]);
			ColorUtility.TryParseHtmlString("#" + json["globalColor"], out systemBackgroundCompositeData.globalColor);
			systemBackgroundCompositeData.globalLightIntensity = (float)json["globalLightIntensity"].AsNumber;
			systemBackgroundCompositeData.nebulae.FromJsonArray(json["nebulae"], new ClassExtensions.ParseJsonValue<NebulaData>(NebulaData.FromJson));
			return systemBackgroundCompositeData;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00054FDF File Offset: 0x000531DF
		public static SystemBackgroundCompositeData FromJson(string key, JsonValue value)
		{
			return SectorBackgroundCompositeData.FromJson(value);
		}

		// Token: 0x04000664 RID: 1636
		public string sectorId;

		// Token: 0x04000665 RID: 1637
		public Color globalColor;

		// Token: 0x04000666 RID: 1638
		public float globalLightIntensity;

		// Token: 0x04000667 RID: 1639
		public List<NebulaData> nebulae = new List<NebulaData>();

		// Token: 0x04000668 RID: 1640
		public StarLayerPerformantData starLayerPerformantData;
	}
}
