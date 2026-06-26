using System;

namespace LightJson.Serialization
{
	// Token: 0x02000020 RID: 32
	public sealed class JsonSerializationException : Exception
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000BD58 File Offset: 0x00009F58
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000BD60 File Offset: 0x00009F60
		public JsonSerializationException.ErrorType Type { get; private set; }

		// Token: 0x060001E0 RID: 480 RVA: 0x0000BD69 File Offset: 0x00009F69
		public JsonSerializationException() : base(JsonSerializationException.GetDefaultMessage(JsonSerializationException.ErrorType.Unknown))
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000BD77 File Offset: 0x00009F77
		public JsonSerializationException(JsonSerializationException.ErrorType type) : this(JsonSerializationException.GetDefaultMessage(type), type)
		{
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000BD86 File Offset: 0x00009F86
		public JsonSerializationException(string message, JsonSerializationException.ErrorType type) : base(message)
		{
			this.Type = type;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000BD96 File Offset: 0x00009F96
		private static string GetDefaultMessage(JsonSerializationException.ErrorType type)
		{
			switch (type)
			{
			case JsonSerializationException.ErrorType.InvalidNumber:
				return "The value been serialized contains an invalid number value (NAN, infinity).";
			case JsonSerializationException.ErrorType.InvalidValueType:
				return "The value been serialized contains (or is) an invalid JSON type.";
			case JsonSerializationException.ErrorType.CircularReference:
				return "The value been serialized contains circular references.";
			default:
				return "An error occurred during serialization.";
			}
		}

		// Token: 0x02000404 RID: 1028
		public enum ErrorType
		{
			// Token: 0x0400177D RID: 6013
			Unknown,
			// Token: 0x0400177E RID: 6014
			InvalidNumber,
			// Token: 0x0400177F RID: 6015
			InvalidValueType,
			// Token: 0x04001780 RID: 6016
			CircularReference
		}
	}
}
