using System;

namespace LightJson.Serialization
{
	// Token: 0x0200001E RID: 30
	public sealed class JsonParseException : Exception
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000B5BB File Offset: 0x000097BB
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x0000B5C3 File Offset: 0x000097C3
		public TextPosition Position { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000B5CC File Offset: 0x000097CC
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000B5D4 File Offset: 0x000097D4
		public JsonParseException.ErrorType Type { get; private set; }

		// Token: 0x060001C8 RID: 456 RVA: 0x0000B5DD File Offset: 0x000097DD
		public JsonParseException() : base(JsonParseException.GetDefaultMessage(JsonParseException.ErrorType.Unknown))
		{
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000B5EB File Offset: 0x000097EB
		public JsonParseException(JsonParseException.ErrorType type, TextPosition position) : this(JsonParseException.GetDefaultMessage(type), type, position)
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000B5FB File Offset: 0x000097FB
		public JsonParseException(string message, JsonParseException.ErrorType type, TextPosition position) : base(message)
		{
			this.Type = type;
			this.Position = position;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000B612 File Offset: 0x00009812
		private static string GetDefaultMessage(JsonParseException.ErrorType type)
		{
			switch (type)
			{
			case JsonParseException.ErrorType.IncompleteMessage:
				return "The string ended before a value could be parsed.";
			case JsonParseException.ErrorType.DuplicateObjectKeys:
				return "The parser encountered a JsonObject with duplicate keys.";
			case JsonParseException.ErrorType.InvalidOrUnexpectedCharacter:
				return "The parser encountered an invalid or unexpected character.";
			default:
				return "An error occurred while parsing the JSON message.";
			}
		}

		// Token: 0x02000403 RID: 1027
		public enum ErrorType
		{
			// Token: 0x04001778 RID: 6008
			Unknown,
			// Token: 0x04001779 RID: 6009
			IncompleteMessage,
			// Token: 0x0400177A RID: 6010
			DuplicateObjectKeys,
			// Token: 0x0400177B RID: 6011
			InvalidOrUnexpectedCharacter
		}
	}
}
