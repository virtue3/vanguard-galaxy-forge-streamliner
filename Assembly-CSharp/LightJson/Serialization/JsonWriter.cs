using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LightJson.Serialization
{
	// Token: 0x02000021 RID: 33
	public sealed class JsonWriter : IDisposable
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000BDC5 File Offset: 0x00009FC5
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000BDCD File Offset: 0x00009FCD
		public string IndentString { get; set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000BDD6 File Offset: 0x00009FD6
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x0000BDDE File Offset: 0x00009FDE
		public string SpacingString { get; set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000BDE7 File Offset: 0x00009FE7
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x0000BDEF File Offset: 0x00009FEF
		public string NewLineString { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000BE00 File Offset: 0x0000A000
		public bool SortObjects { get; set; }

		// Token: 0x060001EC RID: 492 RVA: 0x0000BE09 File Offset: 0x0000A009
		public JsonWriter() : this(false)
		{
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000BE12 File Offset: 0x0000A012
		public JsonWriter(bool pretty)
		{
			if (pretty)
			{
				this.IndentString = "\t";
				this.SpacingString = " ";
				this.NewLineString = "\n";
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000BE3E File Offset: 0x0000A03E
		private void Initialize()
		{
			this.indent = 0;
			this.isNewLine = true;
			this.writer = new StringWriter();
			this.renderingCollections = new HashSet<IEnumerable<JsonValue>>();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000BE64 File Offset: 0x0000A064
		private void Write(string text)
		{
			if (this.isNewLine)
			{
				this.isNewLine = false;
				this.WriteIndentation();
			}
			this.writer.Write(text);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000BE88 File Offset: 0x0000A088
		private void WriteEncodedJsonValue(JsonValue value)
		{
			switch (value.Type)
			{
			case JsonValueType.Null:
				this.Write("null");
				return;
			case JsonValueType.Boolean:
				this.Write(value.AsString);
				return;
			case JsonValueType.Number:
				this.Write(value.AsNumber.ToString(CultureInfo.InvariantCulture));
				return;
			case JsonValueType.String:
				this.WriteEncodedString(value);
				return;
			case JsonValueType.Object:
				this.Write(string.Format("JsonObject[{0}]", value.AsJsonObject.Count));
				return;
			case JsonValueType.Array:
				this.Write(string.Format("JsonArray[{0}]", value.AsJsonArray.Count));
				return;
			default:
				throw new InvalidOperationException("Invalid value type.");
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000BF50 File Offset: 0x0000A150
		private void WriteEncodedString(string text)
		{
			this.Write("\"");
			int i = 0;
			while (i < text.Length)
			{
				char c = text[i];
				if (c <= '"')
				{
					switch (c)
					{
					case '\b':
						this.writer.Write("\\b");
						break;
					case '\t':
						this.writer.Write("\\t");
						break;
					case '\n':
						this.writer.Write("\\n");
						break;
					case '\v':
						goto IL_E9;
					case '\f':
						this.writer.Write("\\f");
						break;
					case '\r':
						this.writer.Write("\\r");
						break;
					default:
						if (c != '"')
						{
							goto IL_E9;
						}
						this.writer.Write("\\\"");
						break;
					}
				}
				else if (c != '/')
				{
					if (c != '\\')
					{
						goto IL_E9;
					}
					this.writer.Write("\\\\");
				}
				else
				{
					this.writer.Write("\\/");
				}
				IL_F5:
				i++;
				continue;
				IL_E9:
				this.writer.Write(c);
				goto IL_F5;
			}
			this.writer.Write("\"");
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000C074 File Offset: 0x0000A274
		private void WriteIndentation()
		{
			for (int i = 0; i < this.indent; i++)
			{
				this.Write(this.IndentString);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000C09E File Offset: 0x0000A29E
		private void WriteSpacing()
		{
			this.Write(this.SpacingString);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C0AC File Offset: 0x0000A2AC
		private void WriteLine()
		{
			this.Write(this.NewLineString);
			this.isNewLine = true;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000C0C1 File Offset: 0x0000A2C1
		private void WriteLine(string line)
		{
			this.Write(line);
			this.WriteLine();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000C0D0 File Offset: 0x0000A2D0
		private void AddRenderingCollection(IEnumerable<JsonValue> value)
		{
			if (!this.renderingCollections.Add(value))
			{
				throw new JsonSerializationException(JsonSerializationException.ErrorType.CircularReference);
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000C0E7 File Offset: 0x0000A2E7
		private void RemoveRenderingCollection(IEnumerable<JsonValue> value)
		{
			this.renderingCollections.Remove(value);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000C0F8 File Offset: 0x0000A2F8
		private void Render(JsonValue value)
		{
			switch (value.Type)
			{
			case JsonValueType.Null:
			case JsonValueType.Boolean:
			case JsonValueType.Number:
			case JsonValueType.String:
				this.WriteEncodedJsonValue(value);
				return;
			case JsonValueType.Object:
				this.Render(value);
				return;
			case JsonValueType.Array:
				this.Render(value);
				return;
			default:
				throw new JsonSerializationException(JsonSerializationException.ErrorType.InvalidValueType);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000C158 File Offset: 0x0000A358
		private void Render(JsonArray value)
		{
			this.AddRenderingCollection(value);
			this.WriteLine("[");
			this.indent++;
			using (IEnumerator<JsonValue> enumerator = value.GetEnumerator())
			{
				bool flag = enumerator.MoveNext();
				while (flag)
				{
					this.Render(enumerator.Current);
					flag = enumerator.MoveNext();
					if (flag)
					{
						this.WriteLine(",");
					}
					else
					{
						this.WriteLine();
					}
				}
			}
			this.indent--;
			this.Write("]");
			this.RemoveRenderingCollection(value);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000C200 File Offset: 0x0000A400
		private void Render(JsonObject value)
		{
			this.AddRenderingCollection(value);
			this.WriteLine("{");
			this.indent++;
			using (IEnumerator<KeyValuePair<string, JsonValue>> jsonObjectEnumerator = this.GetJsonObjectEnumerator(value))
			{
				bool flag = jsonObjectEnumerator.MoveNext();
				while (flag)
				{
					KeyValuePair<string, JsonValue> keyValuePair = jsonObjectEnumerator.Current;
					this.WriteEncodedString(keyValuePair.Key);
					this.Write(":");
					this.WriteSpacing();
					keyValuePair = jsonObjectEnumerator.Current;
					this.Render(keyValuePair.Value);
					flag = jsonObjectEnumerator.MoveNext();
					if (flag)
					{
						this.WriteLine(",");
					}
					else
					{
						this.WriteLine();
					}
				}
			}
			this.indent--;
			this.Write("}");
			this.RemoveRenderingCollection(value);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000C2D4 File Offset: 0x0000A4D4
		private IEnumerator<KeyValuePair<string, JsonValue>> GetJsonObjectEnumerator(JsonObject jsonObject)
		{
			if (this.SortObjects)
			{
				SortedDictionary<string, JsonValue> sortedDictionary = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);
				foreach (KeyValuePair<string, JsonValue> keyValuePair in jsonObject)
				{
					sortedDictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
				return sortedDictionary.GetEnumerator();
			}
			return jsonObject.GetEnumerator();
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000C350 File Offset: 0x0000A550
		public string Serialize(JsonValue jsonValue)
		{
			this.Initialize();
			this.Render(jsonValue);
			return this.writer.ToString();
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000C36A File Offset: 0x0000A56A
		public void Dispose()
		{
			if (this.writer != null)
			{
				this.writer.Dispose();
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000C37F File Offset: 0x0000A57F
		private static bool IsValidNumber(double number)
		{
			return !double.IsNaN(number) && !double.IsInfinity(number);
		}

		// Token: 0x040000E4 RID: 228
		private int indent;

		// Token: 0x040000E5 RID: 229
		private bool isNewLine;

		// Token: 0x040000E6 RID: 230
		private TextWriter writer;

		// Token: 0x040000E7 RID: 231
		private HashSet<IEnumerable<JsonValue>> renderingCollections;
	}
}
