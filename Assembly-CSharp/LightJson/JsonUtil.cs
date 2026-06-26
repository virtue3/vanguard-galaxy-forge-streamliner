using System;
using UnityEngine;

namespace LightJson
{
	// Token: 0x0200001B RID: 27
	public class JsonUtil
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000AA1C File Offset: 0x00008C1C
		public static JsonObject GradientToJson(Gradient gradient)
		{
			JsonArray jsonArray = new JsonArray();
			foreach (GradientAlphaKey gradientAlphaKey in gradient.alphaKeys)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"alpha",
						new double?((double)gradientAlphaKey.alpha)
					},
					{
						"time",
						new double?((double)gradientAlphaKey.time)
					}
				});
			}
			JsonArray jsonArray2 = new JsonArray();
			foreach (GradientColorKey gradientColorKey in gradient.colorKeys)
			{
				jsonArray2.Add(new JsonObject
				{
					{
						"color",
						ColorUtility.ToHtmlStringRGBA(gradientColorKey.color)
					},
					{
						"time",
						new double?((double)gradientColorKey.time)
					}
				});
			}
			return new JsonObject
			{
				{
					"alphaKeys",
					jsonArray
				},
				{
					"colorKeys",
					jsonArray2
				}
			};
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000AB38 File Offset: 0x00008D38
		public static Gradient JsonObjectToGradient(JsonObject json)
		{
			Gradient gradient = new Gradient();
			GradientAlphaKey[] array = new GradientAlphaKey[json["alphaKeys"].AsJsonArray.Count];
			int num = 0;
			foreach (JsonValue jsonValue in json["alphaKeys"].AsJsonArray)
			{
				JsonObject jsonObject = jsonValue;
				array[num] = new GradientAlphaKey((float)jsonObject["alpha"], (float)jsonObject["time"]);
				num++;
			}
			GradientColorKey[] array2 = new GradientColorKey[json["colorKeys"].AsJsonArray.Count];
			num = 0;
			foreach (JsonValue jsonValue2 in json["colorKeys"].AsJsonArray)
			{
				JsonObject jsonObject2 = jsonValue2;
				Color col;
				ColorUtility.TryParseHtmlString("#" + json["colorOne"], out col);
				array2[num] = new GradientColorKey(col, (float)jsonObject2["time"]);
				num++;
			}
			gradient.alphaKeys = array;
			gradient.colorKeys = array2;
			return gradient;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000ACB8 File Offset: 0x00008EB8
		public static JsonObject Vector2ToJson(Vector2 vector)
		{
			return new JsonObject
			{
				{
					"x",
					new double?((double)vector.x)
				},
				{
					"y",
					new double?((double)vector.y)
				}
			};
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000AD04 File Offset: 0x00008F04
		public static Vector2 JsonObjectToVector2(JsonObject json)
		{
			return new Vector2((float)json["x"].AsNumber, (float)json["y"].AsNumber);
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000AD40 File Offset: 0x00008F40
		public static JsonValue ArrayToJson(float[] arr)
		{
			JsonArray jsonArray = new JsonArray();
			for (int i = 0; i < arr.Length; i++)
			{
				jsonArray.Add(new double?((double)arr[i]));
			}
			return jsonArray;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000AD7C File Offset: 0x00008F7C
		public static float[] FloatArrayFromJson(JsonArray data)
		{
			float[] array = new float[data.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (float)data[i].AsNumber;
			}
			return array;
		}
	}
}
