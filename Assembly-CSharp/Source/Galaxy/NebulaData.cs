using System;
using Behaviour.Util;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000136 RID: 310
	public class NebulaData : IJsonSource
	{
		// Token: 0x06000BA0 RID: 2976 RVA: 0x000545AB File Offset: 0x000527AB
		public string GetNebulaTextureName(int index)
		{
			return this.nebulaTextures[index];
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x000545B8 File Offset: 0x000527B8
		public static NebulaData GenerateForSystem(SystemMapData system, Vector2 position, float rotation)
		{
			NebulaData nebulaData = NebulaData.GenerateForSector(system.sector, position, rotation);
			position.x -= system.position.x / 2f;
			position.y -= system.position.y / 2f;
			nebulaData.position = position;
			return nebulaData;
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00054614 File Offset: 0x00052814
		public static NebulaData GenerateForSector(SectorMapData sector, Vector2 position, float rotation)
		{
			SeededRandom seededRandom = new SeedGenerator().Add(sector.backgroundSeed).CreateRandom();
			SeededRandom seededRandom2 = new SeedGenerator().Add(sector.backgroundSeed / 2U).CreateRandom();
			SeededRandom seededRandom3 = new SeedGenerator().Add(sector.backgroundSeed / 3U).CreateRandom();
			NebulaData nebulaData = new NebulaData();
			nebulaData.position = position;
			nebulaData.rotation = rotation;
			nebulaData.scale = new Vector2(seededRandom.RandomRange(0.5f, 2f), seededRandom.RandomRange(0.5f, 1f));
			int hi = nebulaData.nebulaTextures.Length - 1;
			nebulaData.maskTextureIndex = seededRandom.RandomRange(0, hi);
			nebulaData.secondTextureIndex = seededRandom2.RandomRange(0, hi);
			nebulaData.highlightTextureIndex = seededRandom3.RandomRange(0, hi);
			nebulaData.spriteColor = Color.white;
			nebulaData.spriteColor.a = seededRandom.RandomRange(0.05f, 0.2f);
			nebulaData.colorOne = sector.globalLightColor;
			Color nebulaColor = Singleton<BackdropManager>.Instance.GetNebulaColor(seededRandom.RandomRange(0f, 1f));
			nebulaData.colorTwo = Color.Lerp(Color.white, nebulaColor, seededRandom.RandomRange(0.5f, 1f));
			Color nebulaColor2 = Singleton<BackdropManager>.Instance.GetNebulaColor(seededRandom2.RandomRange(0f, 1f));
			nebulaData.colorHighlight = Color.Lerp(Color.white, nebulaColor2, seededRandom2.RandomRange(0.5f, 1f));
			nebulaData.distortion = seededRandom.RandomRange(-0.5f, 0.5f);
			nebulaData.maskDistortion = seededRandom.RandomRange(-0.3f, 0.3f);
			nebulaData.highlightDistortion = seededRandom.RandomRange(-1f, 1f);
			nebulaData.mainTiling = new Vector2(seededRandom.RandomRange(0.1f, 5f), seededRandom.RandomRange(0.11f, 5f));
			nebulaData.maskTiling = new Vector2(seededRandom.RandomRange(0.1f, 5f), seededRandom.RandomRange(0.11f, 5f));
			nebulaData.highlightTiling = new Vector2(seededRandom.RandomRange(0.1f, 5f), seededRandom.RandomRange(0.11f, 5f));
			nebulaData.mainOffset = new Vector2(seededRandom.RandomRange(0.1f, 500f), seededRandom.RandomRange(0.11f, 500f));
			nebulaData.maskOffset = new Vector2(seededRandom.RandomRange(0.1f, 500f), seededRandom.RandomRange(0.11f, 500f));
			nebulaData.highlightOffset = new Vector2(seededRandom.RandomRange(0.1f, 500f), seededRandom.RandomRange(0.11f, 500f));
			nebulaData.noiseScale = seededRandom.RandomRange(-10f, 10f);
			nebulaData.maskNoiseScale = seededRandom.RandomRange(-5f, 5f);
			nebulaData.hightlightNoiseScale = seededRandom.RandomRange(-8f, 8f);
			return nebulaData;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00054920 File Offset: 0x00052B20
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"position",
					JsonUtil.Vector2ToJson(this.position)
				},
				{
					"scale",
					JsonUtil.Vector2ToJson(this.scale)
				},
				{
					"rotation",
					new double?((double)this.rotation)
				},
				{
					"spriteColor",
					ColorUtility.ToHtmlStringRGBA(this.spriteColor)
				},
				{
					"colorOne",
					ColorUtility.ToHtmlStringRGBA(this.colorOne)
				},
				{
					"colorTwo",
					ColorUtility.ToHtmlStringRGBA(this.colorTwo)
				},
				{
					"colorHighlight",
					ColorUtility.ToHtmlStringRGBA(this.colorHighlight)
				},
				{
					"secondTexture",
					new double?((double)this.secondTextureIndex)
				},
				{
					"maskTexture",
					new double?((double)this.maskTextureIndex)
				},
				{
					"highlightTexture",
					new double?((double)this.highlightTextureIndex)
				},
				{
					"distortion",
					new double?((double)this.distortion)
				},
				{
					"noiseScale",
					new double?((double)this.noiseScale)
				},
				{
					"maskDistortion",
					new double?((double)this.maskDistortion)
				},
				{
					"maskNoiseScale",
					new double?((double)this.maskNoiseScale)
				},
				{
					"highlightDistortion",
					new double?((double)this.highlightDistortion)
				},
				{
					"highlightNoiseScale",
					new double?((double)this.hightlightNoiseScale)
				}
			};
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00054B04 File Offset: 0x00052D04
		public static NebulaData FromJson(JsonValue json)
		{
			NebulaData nebulaData = new NebulaData();
			nebulaData.position = JsonUtil.JsonObjectToVector2(json["position"]);
			nebulaData.scale = JsonUtil.JsonObjectToVector2(json["scale"]);
			nebulaData.rotation = (float)json["rotation"].AsNumber;
			ColorUtility.TryParseHtmlString("#" + json["colorOne"], out nebulaData.colorOne);
			ColorUtility.TryParseHtmlString("#" + json["colorTwo"], out nebulaData.colorTwo);
			ColorUtility.TryParseHtmlString("#" + json["colorHighlight"], out nebulaData.colorHighlight);
			if (json["spriteColor"].IsString)
			{
				ColorUtility.TryParseHtmlString("#" + json["spriteColor"], out nebulaData.spriteColor);
			}
			else
			{
				nebulaData.spriteColor = Color.white;
			}
			nebulaData.secondTextureIndex = json["secondTexture"];
			nebulaData.maskTextureIndex = json["maskTexture"];
			nebulaData.highlightTextureIndex = json["highlightTexture"];
			nebulaData.distortion = (float)json["distortion"].AsNumber;
			nebulaData.noiseScale = (float)json["noiseScale"].AsNumber;
			nebulaData.maskDistortion = (float)json["maskDistortion"].AsNumber;
			nebulaData.maskNoiseScale = (float)json["maskNoiseScale"].AsNumber;
			nebulaData.highlightDistortion = (float)json["highlightDistortion"].AsNumber;
			nebulaData.hightlightNoiseScale = (float)json["highlightNoiseScale"].AsNumber;
			return nebulaData;
		}

		// Token: 0x0400064D RID: 1613
		public Vector2 position;

		// Token: 0x0400064E RID: 1614
		public Vector2 scale;

		// Token: 0x0400064F RID: 1615
		public float rotation;

		// Token: 0x04000650 RID: 1616
		public Color spriteColor;

		// Token: 0x04000651 RID: 1617
		public Color colorOne;

		// Token: 0x04000652 RID: 1618
		public Color colorTwo;

		// Token: 0x04000653 RID: 1619
		public Color colorHighlight;

		// Token: 0x04000654 RID: 1620
		public int secondTextureIndex;

		// Token: 0x04000655 RID: 1621
		public int maskTextureIndex;

		// Token: 0x04000656 RID: 1622
		public int highlightTextureIndex;

		// Token: 0x04000657 RID: 1623
		public float distortion;

		// Token: 0x04000658 RID: 1624
		public float noiseScale;

		// Token: 0x04000659 RID: 1625
		public Vector2 mainTiling;

		// Token: 0x0400065A RID: 1626
		public Vector2 mainOffset;

		// Token: 0x0400065B RID: 1627
		public float maskDistortion;

		// Token: 0x0400065C RID: 1628
		public float maskNoiseScale;

		// Token: 0x0400065D RID: 1629
		public Vector2 maskTiling;

		// Token: 0x0400065E RID: 1630
		public Vector2 maskOffset;

		// Token: 0x0400065F RID: 1631
		public float highlightDistortion;

		// Token: 0x04000660 RID: 1632
		public float hightlightNoiseScale;

		// Token: 0x04000661 RID: 1633
		public Vector2 highlightTiling;

		// Token: 0x04000662 RID: 1634
		public Vector2 highlightOffset;

		// Token: 0x04000663 RID: 1635
		private string[] nebulaTextures = new string[]
		{
			"mask-1",
			"mask-2",
			"mask-3",
			"mask-4",
			"mask-greg-boom",
			"mask-greg-cloud",
			"mask-greg-explosion",
			"mask-greg-gas",
			"mask-greg-lines",
			"mask-greg-smoke-twirl2",
			"mask-greg-smoketwirl",
			"mask-greg-smokey",
			"mask-greg-super",
			"mask-greg-supernova",
			"mask-greg-swirl",
			"mask-greg-swirl2",
			"mask-greg-swirl3",
			"Noise01",
			"Noise02",
			"Noise03",
			"Noise04",
			"Noise05",
			"perlin_noise",
			"perlin_noise2",
			"mask-square"
		};
	}
}
