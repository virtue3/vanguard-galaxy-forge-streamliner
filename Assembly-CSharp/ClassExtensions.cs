using System;
using System.Collections.Generic;
using LightJson;
using Source.Data;
using Source.Util;
using TMPro;
using UnityEngine;

// Token: 0x02000012 RID: 18
public static class ClassExtensions
{
	// Token: 0x06000119 RID: 281 RVA: 0x000093C8 File Offset: 0x000075C8
	public static void DestroyChildren(this Transform t)
	{
		foreach (object obj in t)
		{
			Transform transform = (Transform)obj;
			transform.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0000942C File Offset: 0x0000762C
	public static void DestroyActiveChildren(this Transform t)
	{
		foreach (object obj in t)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.activeSelf)
			{
				transform.gameObject.SetActive(false);
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x0600011B RID: 283 RVA: 0x000094A0 File Offset: 0x000076A0
	public static void SetLayerRecursively(this GameObject o, int layer)
	{
		o.layer = layer;
		foreach (object obj in o.transform)
		{
			((Transform)obj).gameObject.SetLayerRecursively(layer);
		}
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00009504 File Offset: 0x00007704
	public static Transform FindRecursive(this Transform t, string name)
	{
		foreach (object obj in t)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.name == name)
			{
				return transform;
			}
			Transform transform2 = transform.FindRecursive(name);
			if (transform2)
			{
				return transform2;
			}
		}
		return null;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00009584 File Offset: 0x00007784
	public static void Z(this Transform t, ZIndex z)
	{
		t.position = new Vector3(t.position.x, t.position.y, z.GetIndex());
	}

	// Token: 0x0600011E RID: 286 RVA: 0x000095B0 File Offset: 0x000077B0
	public static JsonArray ToJsonArray<T>(this IEnumerable<T> list) where T : IJsonSource
	{
		JsonArray jsonArray = new JsonArray();
		foreach (T t in list)
		{
			jsonArray.Add((t != null) ? t.ToJson() : JsonValue.Null);
		}
		return jsonArray;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000961C File Offset: 0x0000781C
	public static JsonArray ToJsonArray(this IEnumerable<string> list)
	{
		JsonArray jsonArray = new JsonArray();
		foreach (string value in list)
		{
			jsonArray.Add(value);
		}
		return jsonArray;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00009674 File Offset: 0x00007874
	public static JsonObject ToJsonObject<T>(this Dictionary<string, T> dict) where T : IJsonSource
	{
		JsonObject jsonObject = new JsonObject();
		foreach (KeyValuePair<string, T> keyValuePair in dict)
		{
			JsonObject jsonObject2 = jsonObject;
			string key = keyValuePair.Key;
			T value = keyValuePair.Value;
			jsonObject2[key] = value.ToJson();
		}
		return jsonObject;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x000096E4 File Offset: 0x000078E4
	public static JsonObject ToJsonObject<E, T>(this Dictionary<E, T> dict) where E : struct, IConvertible where T : IJsonSource
	{
		if (!typeof(E).IsEnum)
		{
			throw new ArgumentException("E must be an enum!");
		}
		JsonObject jsonObject = new JsonObject();
		foreach (KeyValuePair<E, T> keyValuePair in dict)
		{
			JsonObject jsonObject2 = jsonObject;
			E key = keyValuePair.Key;
			string key2 = key.ToString();
			T value = keyValuePair.Value;
			jsonObject2[key2] = value.ToJson();
		}
		return jsonObject;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00009780 File Offset: 0x00007980
	public static void FromJsonArray<T>(this List<T> list, JsonArray data, ClassExtensions.ParseJsonValue<T> parser)
	{
		if (data == null)
		{
			return;
		}
		foreach (JsonValue data2 in data)
		{
			T t = parser(data2);
			if (t != null)
			{
				list.Add(t);
			}
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000097DC File Offset: 0x000079DC
	public static void FromJsonArray<T>(this T[] array, JsonArray data, ClassExtensions.ParseJsonValue<T> parser)
	{
		if (data == null)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (data[i].IsNull ? default(T) : parser(data[i]));
		}
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000982C File Offset: 0x00007A2C
	public static void FromJsonObject<T>(this Dictionary<string, T> dict, JsonObject data, ClassExtensions.ParseJsonValueDict<T> parser)
	{
		if (data == null)
		{
			return;
		}
		foreach (KeyValuePair<string, JsonValue> keyValuePair in data)
		{
			T t = parser(keyValuePair.Key, keyValuePair.Value);
			if (t != null)
			{
				dict[keyValuePair.Key] = t;
			}
		}
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000989C File Offset: 0x00007A9C
	public static void FromJsonObject<E, T>(this Dictionary<E, T> dict, JsonObject data, ClassExtensions.ParseJsonValueDict<T> parser) where E : struct, IConvertible
	{
		if (!typeof(E).IsEnum)
		{
			throw new ArgumentException("E must be an enum!");
		}
		if (data == null)
		{
			return;
		}
		foreach (KeyValuePair<string, JsonValue> keyValuePair in data)
		{
			T t = parser(keyValuePair.Key, keyValuePair.Value);
			if (t != null)
			{
				dict[Enum.Parse<E>(keyValuePair.Key)] = t;
			}
		}
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00009930 File Offset: 0x00007B30
	public static void TL(this TMP_Text t, string s, params object[] values)
	{
		t.text = Translation.Translate(s, values);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000993F File Offset: 0x00007B3F
	public static bool TryGetComponentInParent<T>(this Component component, out T result) where T : Component
	{
		result = component.GetComponentInParent<T>();
		return result != null;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00009960 File Offset: 0x00007B60
	public static bool TryGetComponentInImmediateParent<T>(this Component component, out T result) where T : Component
	{
		result = default(T);
		if (component.transform.parent == null)
		{
			return false;
		}
		result = component.transform.parent.GetComponent<T>();
		return result != null;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000099B0 File Offset: 0x00007BB0
	public static bool TryGetComponentOrInImmediateParent<T>(this Component component, out T result) where T : Component
	{
		result = component.GetComponent<T>();
		if (result != null)
		{
			return true;
		}
		if (component.transform.parent == null)
		{
			return false;
		}
		result = component.transform.parent.GetComponent<T>();
		return result != null;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00009A1C File Offset: 0x00007C1C
	public static Color HexToColor(this string s)
	{
		Color result;
		ColorUtility.TryParseHtmlString(s, out result);
		return result;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00009A34 File Offset: 0x00007C34
	public static List<AbstractUnitData> AlwaysHostile(this List<AbstractUnitData> payload)
	{
		foreach (AbstractUnitData abstractUnitData in payload)
		{
			abstractUnitData.alwaysHostile = true;
		}
		return payload;
	}

	// Token: 0x020003F9 RID: 1017
	// (Invoke) Token: 0x06002607 RID: 9735
	public delegate T ParseJsonValue<T>(JsonValue data);

	// Token: 0x020003FA RID: 1018
	// (Invoke) Token: 0x0600260B RID: 9739
	public delegate T ParseJsonValueDict<T>(string id, JsonValue data);
}
