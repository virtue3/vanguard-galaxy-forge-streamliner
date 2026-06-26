using System;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000135 RID: 309
	public class BackgroundSimpleData : IJsonSource
	{
		// Token: 0x06000B9D RID: 2973 RVA: 0x00054454 File Offset: 0x00052654
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"colorOne",
					ColorUtility.ToHtmlStringRGBA(this.colorOne)
				},
				{
					"colorTwo",
					ColorUtility.ToHtmlStringRGBA(this.colorTwo)
				},
				{
					"mainTexture",
					this.mainTexture
				},
				{
					"scale",
					new double?((double)this.scale)
				},
				{
					"colorMixScale",
					new double?((double)this.colorMixScale)
				}
			};
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x000544F4 File Offset: 0x000526F4
		public static BackgroundSimpleData FromJson(JsonValue json)
		{
			BackgroundSimpleData backgroundSimpleData = new BackgroundSimpleData();
			ColorUtility.TryParseHtmlString("#" + json["colorOne"], out backgroundSimpleData.colorOne);
			ColorUtility.TryParseHtmlString("#" + json["colorTwo"], out backgroundSimpleData.colorTwo);
			backgroundSimpleData.mainTexture = json["mainTexture"];
			backgroundSimpleData.scale = (float)json["scale"].AsNumber;
			backgroundSimpleData.colorMixScale = (float)json["colorMixScale"].AsNumber;
			return backgroundSimpleData;
		}

		// Token: 0x04000648 RID: 1608
		public Color colorOne;

		// Token: 0x04000649 RID: 1609
		public Color colorTwo;

		// Token: 0x0400064A RID: 1610
		public float scale;

		// Token: 0x0400064B RID: 1611
		public float colorMixScale;

		// Token: 0x0400064C RID: 1612
		public string mainTexture;
	}
}
