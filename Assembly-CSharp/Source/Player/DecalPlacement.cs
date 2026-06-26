using System;
using LightJson;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x02000092 RID: 146
	public class DecalPlacement
	{
		// Token: 0x06000587 RID: 1415 RVA: 0x00031704 File Offset: 0x0002F904
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"decalId",
					this.decalId
				},
				{
					"opacity",
					new double?((double)this.opacity)
				},
				{
					"position",
					JsonUtil.Vector2ToJson(this.position)
				},
				{
					"scale",
					new double?((double)this.scale)
				},
				{
					"rotation",
					new double?((double)this.rotation)
				},
				{
					"color",
					"#" + ColorUtility.ToHtmlStringRGB(this.color)
				}
			};
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000317CC File Offset: 0x0002F9CC
		public static DecalPlacement FromJson(JsonValue json)
		{
			DecalPlacement decalPlacement = new DecalPlacement
			{
				decalId = json["decalId"].AsString,
				opacity = (json["opacity"].IsNull ? 0.75f : ((float)json["opacity"].AsNumber)),
				position = (json["position"].IsJsonObject ? JsonUtil.JsonObjectToVector2(json["position"].AsJsonObject) : Vector2.zero),
				scale = (json["scale"].IsNull ? 1f : ((float)json["scale"].AsNumber)),
				rotation = (json["rotation"].IsNull ? 0f : ((float)json["rotation"].AsNumber)),
				color = Color.white
			};
			string asString = json["color"].AsString;
			Color color;
			if (!string.IsNullOrEmpty(asString) && ColorUtility.TryParseHtmlString(asString, out color))
			{
				decalPlacement.color = color;
			}
			return decalPlacement;
		}

		// Token: 0x040002D3 RID: 723
		public string decalId;

		// Token: 0x040002D4 RID: 724
		public float opacity = 0.75f;

		// Token: 0x040002D5 RID: 725
		public Vector2 position;

		// Token: 0x040002D6 RID: 726
		public float scale = 1f;

		// Token: 0x040002D7 RID: 727
		public float rotation;

		// Token: 0x040002D8 RID: 728
		public Color color = Color.white;
	}
}
