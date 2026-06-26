using System;
using System.Collections.Generic;
using Behaviour.Util;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x0200013C RID: 316
	public class SystemBackgroundCompositeData : IJsonSource
	{
		// Token: 0x06000BBA RID: 3002 RVA: 0x0005587B File Offset: 0x00053A7B
		public SystemBackgroundCompositeData(string systemId)
		{
			this.systemId = systemId;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x000558B8 File Offset: 0x00053AB8
		public static SystemBackgroundCompositeData CreateForSystem(SystemMapData system)
		{
			SystemBackgroundCompositeData systemBackgroundCompositeData = new SystemBackgroundCompositeData(system.storyId);
			systemBackgroundCompositeData.sunlightColor = Singleton<BackdropManager>.Instance.GetStarColor(system.GetStar().color);
			systemBackgroundCompositeData.sunlightIntensity = system.GetStar().intensity;
			systemBackgroundCompositeData.globalColor = system.sector.globalLightColor;
			systemBackgroundCompositeData.globalLightIntensity = system.sector.globalLightIntensity;
			systemBackgroundCompositeData.starLayers.Add(StarLayerData.GenerateFromSystem(system, true));
			systemBackgroundCompositeData.starLayers.Add(StarLayerData.GenerateFromSystem(system, false));
			systemBackgroundCompositeData.starLayerPerformantData = StarLayerPerformantData.GenerateForSector(system.sector);
			for (int i = 0; i < 3; i++)
			{
				systemBackgroundCompositeData.smokeBackgroundEffectData.Add(SmokeBackgroundEffectData.GenerateForSystem(system, (i == 0) ? 1 : 0));
			}
			SeededRandom seededRandom = new SeedGenerator().Add(system.sector.backgroundSeed).CreateRandom();
			int num = seededRandom.RandomRange(1, 3);
			Vector2 position = new Vector2(seededRandom.RandomRange(-19f, 19f), seededRandom.RandomRange(-3f, 3f));
			float num2 = (float)seededRandom.RandomRange(0, 360);
			for (int j = 0; j < num; j++)
			{
				systemBackgroundCompositeData.nebulae.Add(NebulaData.GenerateForSystem(system, position, num2));
				num2 += (float)seededRandom.RandomRange(45 * j, 270);
			}
			return systemBackgroundCompositeData;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x00055A18 File Offset: 0x00053C18
		public JsonValue ToJson()
		{
			JsonValue value = JsonValue.Null;
			if (this.starLayerPerformantData != null)
			{
				value = this.starLayerPerformantData.ToJson();
			}
			return new JsonObject
			{
				{
					"systemId",
					this.systemId
				},
				{
					"globalColor",
					ColorUtility.ToHtmlStringRGBA(this.globalColor)
				},
				{
					"globalLightIntensity",
					new double?((double)this.globalLightIntensity)
				},
				{
					"sunlightColor",
					ColorUtility.ToHtmlStringRGBA(this.sunlightColor)
				},
				{
					"sunlightIntensity",
					new double?((double)this.sunlightIntensity)
				},
				{
					"starLayerPerformant",
					value
				},
				{
					"starLayers",
					this.starLayers.ToJsonArray<StarLayerData>()
				},
				{
					"nebulae",
					this.nebulae.ToJsonArray<NebulaData>()
				},
				{
					"smokeBackgrounds",
					this.smokeBackgrounds.ToJsonArray<SmokeBackgroundData>()
				}
			};
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x00055B34 File Offset: 0x00053D34
		public static SystemBackgroundCompositeData FromJson(JsonValue json)
		{
			SystemBackgroundCompositeData systemBackgroundCompositeData = new SystemBackgroundCompositeData(json["systemId"]);
			ColorUtility.TryParseHtmlString("#" + json["globalColor"], out systemBackgroundCompositeData.globalColor);
			systemBackgroundCompositeData.globalLightIntensity = (float)json["globalLightIntensity"].AsNumber;
			ColorUtility.TryParseHtmlString("#" + json["sunlightColor"], out systemBackgroundCompositeData.sunlightColor);
			systemBackgroundCompositeData.sunlightIntensity = (float)json["sunlightIntensity"].AsNumber;
			if (json["starLayerPerformant"].IsJsonObject)
			{
				systemBackgroundCompositeData.starLayerPerformantData = StarLayerPerformantData.FromJson(json["starLayerPerformant"]);
			}
			systemBackgroundCompositeData.nebulae.FromJsonArray(json["nebulae"], new ClassExtensions.ParseJsonValue<NebulaData>(NebulaData.FromJson));
			systemBackgroundCompositeData.smokeBackgrounds.FromJsonArray(json["smokeBackgrounds"], new ClassExtensions.ParseJsonValue<SmokeBackgroundData>(SmokeBackgroundData.FromJson));
			systemBackgroundCompositeData.starLayers.FromJsonArray(json["starLayers"], new ClassExtensions.ParseJsonValue<StarLayerData>(StarLayerData.FromJson));
			return systemBackgroundCompositeData;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00055C82 File Offset: 0x00053E82
		public static SystemBackgroundCompositeData FromJson(string key, JsonValue value)
		{
			return SystemBackgroundCompositeData.FromJson(value);
		}

		// Token: 0x0400067F RID: 1663
		public string systemId;

		// Token: 0x04000680 RID: 1664
		public Color globalColor;

		// Token: 0x04000681 RID: 1665
		public float globalLightIntensity;

		// Token: 0x04000682 RID: 1666
		public Color sunlightColor;

		// Token: 0x04000683 RID: 1667
		public float sunlightIntensity;

		// Token: 0x04000684 RID: 1668
		public StarLayerPerformantData starLayerPerformantData;

		// Token: 0x04000685 RID: 1669
		public List<NebulaData> nebulae = new List<NebulaData>();

		// Token: 0x04000686 RID: 1670
		public List<SmokeBackgroundData> smokeBackgrounds = new List<SmokeBackgroundData>();

		// Token: 0x04000687 RID: 1671
		public List<SmokeBackgroundEffectData> smokeBackgroundEffectData = new List<SmokeBackgroundEffectData>();

		// Token: 0x04000688 RID: 1672
		public List<StarLayerData> starLayers = new List<StarLayerData>();
	}
}
