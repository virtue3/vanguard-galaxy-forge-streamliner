using System;
using System.Collections.Generic;
using System.Diagnostics;
using LightJson.Serialization;

namespace LightJson
{
	// Token: 0x0200001C RID: 28
	[DebuggerDisplay("{ToString(),nq}", Type = "JsonValue({Type})")]
	[DebuggerTypeProxy(typeof(JsonValue.JsonValueDebugView))]
	public struct JsonValue
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000ADBF File Offset: 0x00008FBF
		public JsonValueType Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000ADC7 File Offset: 0x00008FC7
		public bool IsNull
		{
			get
			{
				return this.Type == JsonValueType.Null;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000ADD2 File Offset: 0x00008FD2
		public bool IsBoolean
		{
			get
			{
				return this.Type == JsonValueType.Boolean;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public bool IsInteger
		{
			get
			{
				if (!this.IsNumber)
				{
					return false;
				}
				double num = this.value;
				return num >= -2147483648.0 && num <= 2147483647.0 && num % 1.0 == 0.0;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000AE2E File Offset: 0x0000902E
		public bool IsNumber
		{
			get
			{
				return this.Type == JsonValueType.Number;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000AE39 File Offset: 0x00009039
		public bool IsString
		{
			get
			{
				return this.Type == JsonValueType.String;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000AE44 File Offset: 0x00009044
		public bool IsJsonObject
		{
			get
			{
				return this.Type == JsonValueType.Object;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000AE4F File Offset: 0x0000904F
		public bool IsJsonArray
		{
			get
			{
				return this.Type == JsonValueType.Array;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000AE5C File Offset: 0x0000905C
		public bool IsDateTime
		{
			get
			{
				return this.AsDateTime != null;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000AE78 File Offset: 0x00009078
		public bool AsBoolean
		{
			get
			{
				switch (this.Type)
				{
				case JsonValueType.Boolean:
					return this.value == 1.0;
				case JsonValueType.Number:
					return this.value != 0.0;
				case JsonValueType.String:
					return (string)this.reference != "";
				case JsonValueType.Object:
				case JsonValueType.Array:
					return true;
				default:
					return false;
				}
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000AEEC File Offset: 0x000090EC
		public int AsInteger
		{
			get
			{
				double asNumber = this.AsNumber;
				if (asNumber >= 2147483647.0)
				{
					return int.MaxValue;
				}
				if (asNumber <= -2147483648.0)
				{
					return int.MinValue;
				}
				return (int)asNumber;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000AF28 File Offset: 0x00009128
		public long AsLong
		{
			get
			{
				double asNumber = this.AsNumber;
				if (asNumber >= 9.2233720368547758E+18)
				{
					return long.MaxValue;
				}
				if (asNumber <= -9.2233720368547758E+18)
				{
					return long.MinValue;
				}
				return (long)asNumber;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000AF6C File Offset: 0x0000916C
		public double AsNumber
		{
			get
			{
				switch (this.Type)
				{
				case JsonValueType.Boolean:
					return (double)((this.value == 1.0) ? 1 : 0);
				case JsonValueType.Number:
					return this.value;
				case JsonValueType.String:
				{
					double result;
					if (double.TryParse((string)this.reference, out result))
					{
						return result;
					}
					break;
				}
				}
				return 0.0;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000AFD4 File Offset: 0x000091D4
		public string AsString
		{
			get
			{
				switch (this.Type)
				{
				case JsonValueType.Boolean:
					if (this.value != 1.0)
					{
						return "false";
					}
					return "true";
				case JsonValueType.Number:
					return this.value.ToString();
				case JsonValueType.String:
					return (string)this.reference;
				default:
					return null;
				}
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000B037 File Offset: 0x00009237
		public JsonObject AsJsonObject
		{
			get
			{
				if (!this.IsJsonObject)
				{
					return null;
				}
				return (JsonObject)this.reference;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000B04E File Offset: 0x0000924E
		public JsonArray AsJsonArray
		{
			get
			{
				if (!this.IsJsonArray)
				{
					return null;
				}
				return (JsonArray)this.reference;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000B068 File Offset: 0x00009268
		public DateTime? AsDateTime
		{
			get
			{
				DateTime dateTime;
				if (this.IsString && DateTime.TryParse((string)this.reference, out dateTime))
				{
					return new DateTime?(dateTime);
				}
				return null;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000B0A4 File Offset: 0x000092A4
		public object AsObject
		{
			get
			{
				JsonValueType jsonValueType = this.Type;
				if (jsonValueType - JsonValueType.Boolean <= 1)
				{
					return this.value;
				}
				if (jsonValueType - JsonValueType.String > 2)
				{
					return null;
				}
				return this.reference;
			}
		}

		// Token: 0x17000038 RID: 56
		public JsonValue this[string key]
		{
			get
			{
				if (this.IsJsonObject)
				{
					return ((JsonObject)this.reference)[key];
				}
				throw new InvalidOperationException("This value does not represent a JsonObject.");
			}
			set
			{
				if (this.IsJsonObject)
				{
					((JsonObject)this.reference)[key] = value;
					return;
				}
				throw new InvalidOperationException("This value does not represent a JsonObject.");
			}
		}

		// Token: 0x17000039 RID: 57
		public JsonValue this[int index]
		{
			get
			{
				if (this.IsJsonArray)
				{
					return ((JsonArray)this.reference)[index];
				}
				throw new InvalidOperationException("This value does not represent a JsonArray.");
			}
			set
			{
				if (this.IsJsonArray)
				{
					((JsonArray)this.reference)[index] = value;
					return;
				}
				throw new InvalidOperationException("This value does not represent a JsonArray.");
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B174 File Offset: 0x00009374
		private JsonValue(JsonValueType type, double value, object reference)
		{
			this.type = type;
			this.value = value;
			this.reference = reference;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000B18B File Offset: 0x0000938B
		public JsonValue(bool? value)
		{
			if (value != null)
			{
				this.reference = null;
				this.type = JsonValueType.Boolean;
				this.value = (double)(value.Value ? 1 : 0);
				return;
			}
			this = JsonValue.Null;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B1C4 File Offset: 0x000093C4
		public JsonValue(double? value)
		{
			if (value != null)
			{
				this.reference = null;
				this.type = JsonValueType.Number;
				this.value = value.Value;
				return;
			}
			this = JsonValue.Null;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B1F6 File Offset: 0x000093F6
		public JsonValue(string value)
		{
			if (value != null)
			{
				this.value = 0.0;
				this.type = JsonValueType.String;
				this.reference = value;
				return;
			}
			this = JsonValue.Null;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B224 File Offset: 0x00009424
		public JsonValue(JsonObject value)
		{
			if (value != null)
			{
				this.value = 0.0;
				this.type = JsonValueType.Object;
				this.reference = value;
				return;
			}
			this = JsonValue.Null;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000B252 File Offset: 0x00009452
		public JsonValue(JsonArray value)
		{
			if (value != null)
			{
				this.value = 0.0;
				this.type = JsonValueType.Array;
				this.reference = value;
				return;
			}
			this = JsonValue.Null;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000B280 File Offset: 0x00009480
		public static implicit operator JsonValue(bool? value)
		{
			return new JsonValue(value);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000B288 File Offset: 0x00009488
		public static implicit operator JsonValue(double? value)
		{
			return new JsonValue(value);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000B290 File Offset: 0x00009490
		public static implicit operator JsonValue(string value)
		{
			return new JsonValue(value);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000B298 File Offset: 0x00009498
		public static implicit operator JsonValue(JsonObject value)
		{
			return new JsonValue(value);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B2A0 File Offset: 0x000094A0
		public static implicit operator JsonValue(JsonArray value)
		{
			return new JsonValue(value);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000B2A8 File Offset: 0x000094A8
		public static implicit operator JsonValue(DateTime? value)
		{
			if (value == null)
			{
				return JsonValue.Null;
			}
			return new JsonValue(value.Value.ToString("o"));
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000B2DD File Offset: 0x000094DD
		public static implicit operator int(JsonValue jsonValue)
		{
			if (jsonValue.IsInteger)
			{
				return jsonValue.AsInteger;
			}
			return 0;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B2F4 File Offset: 0x000094F4
		public static implicit operator int?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			return new int?(jsonValue);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B31F File Offset: 0x0000951F
		public static implicit operator bool(JsonValue jsonValue)
		{
			return jsonValue.IsBoolean && jsonValue.value == 1.0;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000B340 File Offset: 0x00009540
		public static implicit operator bool?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			return new bool?(jsonValue);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000B36B File Offset: 0x0000956B
		public static implicit operator double(JsonValue jsonValue)
		{
			if (jsonValue.IsNumber)
			{
				return jsonValue.value;
			}
			return double.NaN;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B388 File Offset: 0x00009588
		public static implicit operator double?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			return new double?(jsonValue);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public static implicit operator string(JsonValue jsonValue)
		{
			if (jsonValue.IsString || jsonValue.IsNull)
			{
				return jsonValue.reference as string;
			}
			return null;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B3D5 File Offset: 0x000095D5
		public static implicit operator JsonObject(JsonValue jsonValue)
		{
			if (jsonValue.IsJsonObject || jsonValue.IsNull)
			{
				return jsonValue.reference as JsonObject;
			}
			return null;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B3F6 File Offset: 0x000095F6
		public static implicit operator JsonArray(JsonValue jsonValue)
		{
			if (jsonValue.IsJsonArray || jsonValue.IsNull)
			{
				return jsonValue.reference as JsonArray;
			}
			return null;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B418 File Offset: 0x00009618
		public static implicit operator DateTime(JsonValue jsonValue)
		{
			DateTime? asDateTime = jsonValue.AsDateTime;
			if (asDateTime != null)
			{
				return asDateTime.Value;
			}
			return DateTime.MinValue;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B444 File Offset: 0x00009644
		public static implicit operator DateTime?(JsonValue jsonValue)
		{
			if (jsonValue.IsDateTime || jsonValue.IsNull)
			{
				return jsonValue.AsDateTime;
			}
			return null;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B474 File Offset: 0x00009674
		public static bool operator ==(JsonValue a, JsonValue b)
		{
			return a.Type == b.Type && a.value == b.value && object.Equals(a.reference, b.reference);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000B4A7 File Offset: 0x000096A7
		public static bool operator !=(JsonValue a, JsonValue b)
		{
			return !(a == b);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000B4B3 File Offset: 0x000096B3
		public static JsonValue Parse(string text)
		{
			return JsonReader.Parse(text);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000B4BC File Offset: 0x000096BC
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return this.IsNull;
			}
			JsonValue? jsonValue = obj as JsonValue?;
			return jsonValue != null && this == jsonValue.Value;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000B4FC File Offset: 0x000096FC
		public override int GetHashCode()
		{
			if (this.IsNull)
			{
				return this.Type.GetHashCode();
			}
			return this.Type.GetHashCode() ^ this.value.GetHashCode() ^ EqualityComparer<object>.Default.GetHashCode(this.reference);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000B55A File Offset: 0x0000975A
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000B564 File Offset: 0x00009764
		public string ToString(bool pretty)
		{
			string result;
			using (JsonWriter jsonWriter = new JsonWriter(pretty))
			{
				result = jsonWriter.Serialize(this);
			}
			return result;
		}

		// Token: 0x040000D5 RID: 213
		private readonly JsonValueType type;

		// Token: 0x040000D6 RID: 214
		private readonly object reference;

		// Token: 0x040000D7 RID: 215
		private readonly double value;

		// Token: 0x040000D8 RID: 216
		public static readonly JsonValue Null = new JsonValue(JsonValueType.Null, 0.0, null);

		// Token: 0x02000402 RID: 1026
		private class JsonValueDebugView
		{
			// Token: 0x170005DF RID: 1503
			// (get) Token: 0x0600266E RID: 9838 RVA: 0x000D4CAF File Offset: 0x000D2EAF
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonObject ObjectView
			{
				get
				{
					if (this.jsonValue.IsJsonObject)
					{
						return (JsonObject)this.jsonValue.reference;
					}
					return null;
				}
			}

			// Token: 0x170005E0 RID: 1504
			// (get) Token: 0x0600266F RID: 9839 RVA: 0x000D4CD0 File Offset: 0x000D2ED0
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonArray ArrayView
			{
				get
				{
					if (this.jsonValue.IsJsonArray)
					{
						return (JsonArray)this.jsonValue.reference;
					}
					return null;
				}
			}

			// Token: 0x170005E1 RID: 1505
			// (get) Token: 0x06002670 RID: 9840 RVA: 0x000D4CF1 File Offset: 0x000D2EF1
			public JsonValueType Type
			{
				get
				{
					return this.jsonValue.Type;
				}
			}

			// Token: 0x170005E2 RID: 1506
			// (get) Token: 0x06002671 RID: 9841 RVA: 0x000D4D00 File Offset: 0x000D2F00
			public object Value
			{
				get
				{
					if (this.jsonValue.IsJsonObject)
					{
						return (JsonObject)this.jsonValue.reference;
					}
					if (this.jsonValue.IsJsonArray)
					{
						return (JsonArray)this.jsonValue.reference;
					}
					return this.jsonValue;
				}
			}

			// Token: 0x06002672 RID: 9842 RVA: 0x000D4D54 File Offset: 0x000D2F54
			public JsonValueDebugView(JsonValue jsonValue)
			{
				this.jsonValue = jsonValue;
			}

			// Token: 0x04001776 RID: 6006
			private JsonValue jsonValue;
		}
	}
}
