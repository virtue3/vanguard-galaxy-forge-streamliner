using System;
using System.IO;

namespace LightJson.Serialization
{
	// Token: 0x02000023 RID: 35
	public sealed class TextScanner
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000C394 File Offset: 0x0000A594
		public TextPosition Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000C39C File Offset: 0x0000A59C
		public bool CanRead
		{
			get
			{
				return this.reader.Peek() != -1;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000C3AF File Offset: 0x0000A5AF
		public TextScanner(TextReader reader)
		{
			this.reader = reader;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000C3BE File Offset: 0x0000A5BE
		public char Peek()
		{
			int num = this.reader.Peek();
			if (num == -1)
			{
				throw new JsonParseException(JsonParseException.ErrorType.IncompleteMessage, this.position);
			}
			return (char)num;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000C3E0 File Offset: 0x0000A5E0
		public char Read()
		{
			int num = this.reader.Read();
			if (num == -1)
			{
				throw new JsonParseException(JsonParseException.ErrorType.IncompleteMessage, this.position);
			}
			if (num != 10)
			{
				if (num != 13)
				{
					this.position.column = this.position.column + 1L;
					return (char)num;
				}
				if (this.reader.Peek() == 10)
				{
					this.reader.Read();
				}
			}
			this.position.line = this.position.line + 1L;
			this.position.column = 0L;
			return '\n';
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000C463 File Offset: 0x0000A663
		public void SkipWhitespace()
		{
			while (char.IsWhiteSpace(this.Peek()))
			{
				this.Read();
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000C47B File Offset: 0x0000A67B
		public void Assert(char next)
		{
			if (this.Peek() == next)
			{
				this.Read();
				return;
			}
			throw new JsonParseException(string.Format("Parser expected '{0}'", next), JsonParseException.ErrorType.InvalidOrUnexpectedCharacter, this.position);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C4AC File Offset: 0x0000A6AC
		public void Assert(string next)
		{
			for (int i = 0; i < next.Length; i++)
			{
				this.Assert(next[i]);
			}
		}

		// Token: 0x040000EE RID: 238
		private TextReader reader;

		// Token: 0x040000EF RID: 239
		private TextPosition position;
	}
}
