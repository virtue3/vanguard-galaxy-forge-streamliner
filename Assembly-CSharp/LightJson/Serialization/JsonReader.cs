using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace LightJson.Serialization
{
	// Token: 0x0200001F RID: 31
	public sealed class JsonReader
	{
		// Token: 0x060001CC RID: 460 RVA: 0x0000B641 File Offset: 0x00009841
		private JsonReader(TextReader reader)
		{
			this.scanner = new TextScanner(reader);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000B655 File Offset: 0x00009855
		private string ReadJsonKey()
		{
			return this.ReadString();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000B660 File Offset: 0x00009860
		private JsonValue ReadJsonValue()
		{
			this.scanner.SkipWhitespace();
			char c = this.scanner.Peek();
			if (char.IsNumber(c))
			{
				return this.ReadNumber();
			}
			if (c > '[')
			{
				if (c <= 'n')
				{
					if (c != 'f')
					{
						if (c != 'n')
						{
							goto IL_90;
						}
						return this.ReadNull();
					}
				}
				else if (c != 't')
				{
					if (c == '{')
					{
						return this.ReadObject();
					}
					goto IL_90;
				}
				return this.ReadBoolean();
			}
			if (c == '"')
			{
				return this.ReadString();
			}
			if (c == '-')
			{
				return this.ReadNumber();
			}
			if (c == '[')
			{
				return this.ReadArray();
			}
			IL_90:
			throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000B70E File Offset: 0x0000990E
		private JsonValue ReadNull()
		{
			this.scanner.Assert("null");
			return JsonValue.Null;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000B728 File Offset: 0x00009928
		private JsonValue ReadBoolean()
		{
			char c = this.scanner.Peek();
			if (c == 'f')
			{
				this.scanner.Assert("false");
				return new bool?(false);
			}
			if (c == 't')
			{
				this.scanner.Assert("true");
				return new bool?(true);
			}
			throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000B794 File Offset: 0x00009994
		private void ReadDigits(StringBuilder builder)
		{
			while (this.scanner.CanRead && char.IsDigit(this.scanner.Peek()))
			{
				builder.Append(this.scanner.Read());
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000B7CC File Offset: 0x000099CC
		private JsonValue ReadNumber()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.scanner.Peek() == '-')
			{
				stringBuilder.Append(this.scanner.Read());
			}
			if (this.scanner.Peek() == '0')
			{
				stringBuilder.Append(this.scanner.Read());
			}
			else
			{
				this.ReadDigits(stringBuilder);
			}
			if (this.scanner.CanRead && this.scanner.Peek() == '.')
			{
				stringBuilder.Append(this.scanner.Read());
				this.ReadDigits(stringBuilder);
			}
			if (this.scanner.CanRead && char.ToLowerInvariant(this.scanner.Peek()) == 'e')
			{
				stringBuilder.Append(this.scanner.Read());
				char c = this.scanner.Peek();
				if (c == '+' || c == '-')
				{
					stringBuilder.Append(this.scanner.Read());
				}
				this.ReadDigits(stringBuilder);
			}
			return new double?(double.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture));
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000B8DC File Offset: 0x00009ADC
		private string ReadString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.scanner.Assert('"');
			for (;;)
			{
				char c = this.scanner.Read();
				if (c == '\\')
				{
					c = this.scanner.Read();
					char c2 = char.ToLower(c);
					if (c2 <= '\\')
					{
						if (c2 != '"' && c2 != '/' && c2 != '\\')
						{
							break;
						}
						stringBuilder.Append(c);
					}
					else if (c2 <= 'f')
					{
						if (c2 != 'b')
						{
							if (c2 != 'f')
							{
								break;
							}
							stringBuilder.Append('\f');
						}
						else
						{
							stringBuilder.Append('\b');
						}
					}
					else
					{
						if (c2 != 'n')
						{
							switch (c2)
							{
							case 'r':
								stringBuilder.Append('\r');
								continue;
							case 't':
								stringBuilder.Append('\t');
								continue;
							case 'u':
								stringBuilder.Append(this.ReadUnicodeLiteral());
								continue;
							}
							break;
						}
						stringBuilder.Append('\n');
					}
				}
				else
				{
					if (c == '"')
					{
						goto IL_126;
					}
					if (char.IsControl(c))
					{
						goto Block_12;
					}
					stringBuilder.Append(c);
				}
			}
			throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
			Block_12:
			throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
			IL_126:
			return stringBuilder.ToString();
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000BA18 File Offset: 0x00009C18
		private int ReadHexDigit()
		{
			switch (char.ToUpper(this.scanner.Read()))
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			}
			throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000BAD5 File Offset: 0x00009CD5
		private char ReadUnicodeLiteral()
		{
			return (char)(0 + this.ReadHexDigit() * 4096 + this.ReadHexDigit() * 256 + this.ReadHexDigit() * 16 + this.ReadHexDigit());
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000BB04 File Offset: 0x00009D04
		private JsonObject ReadObject()
		{
			return this.ReadObject(new JsonObject());
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000BB14 File Offset: 0x00009D14
		private JsonObject ReadObject(JsonObject jsonObject)
		{
			this.scanner.Assert('{');
			this.scanner.SkipWhitespace();
			if (this.scanner.Peek() != '}')
			{
				for (;;)
				{
					this.scanner.SkipWhitespace();
					string key = this.ReadJsonKey();
					if (jsonObject.ContainsKey(key))
					{
						break;
					}
					this.scanner.SkipWhitespace();
					this.scanner.Assert(':');
					this.scanner.SkipWhitespace();
					JsonValue value = this.ReadJsonValue();
					jsonObject.Add(key, value);
					this.scanner.SkipWhitespace();
					char c = this.scanner.Read();
					if (c == '}')
					{
						return jsonObject;
					}
					if (c != ',')
					{
						goto Block_4;
					}
				}
				throw new JsonParseException(JsonParseException.ErrorType.DuplicateObjectKeys, this.scanner.Position);
				Block_4:
				throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
			}
			this.scanner.Read();
			return jsonObject;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		private JsonArray ReadArray()
		{
			return this.ReadArray(new JsonArray());
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000BC00 File Offset: 0x00009E00
		private JsonArray ReadArray(JsonArray jsonArray)
		{
			this.scanner.Assert('[');
			this.scanner.SkipWhitespace();
			if (this.scanner.Peek() != ']')
			{
				char c;
				do
				{
					this.scanner.SkipWhitespace();
					JsonValue value = this.ReadJsonValue();
					jsonArray.Add(value);
					this.scanner.SkipWhitespace();
					c = this.scanner.Read();
					if (c == ']')
					{
						return jsonArray;
					}
				}
				while (c == ',');
				throw new JsonParseException(JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.scanner.Position);
			}
			this.scanner.Read();
			return jsonArray;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000BC90 File Offset: 0x00009E90
		private JsonValue Parse()
		{
			this.scanner.SkipWhitespace();
			return this.ReadJsonValue();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000BCA3 File Offset: 0x00009EA3
		public static JsonValue Parse(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			return new JsonReader(reader).Parse();
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		public static JsonValue Parse(string source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			JsonValue result;
			using (StringReader stringReader = new StringReader(source))
			{
				result = new JsonReader(stringReader).Parse();
			}
			return result;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public static JsonValue ParseFile(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			JsonValue result;
			using (StreamReader streamReader = new StreamReader(path))
			{
				result = new JsonReader(streamReader).Parse();
			}
			return result;
		}

		// Token: 0x040000E2 RID: 226
		private TextScanner scanner;
	}
}
