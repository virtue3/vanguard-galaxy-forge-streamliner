using System;

namespace Source.Dialogues
{
	// Token: 0x02000101 RID: 257
	[Serializable]
	public class DialogueLine
	{
		// Token: 0x060009C0 RID: 2496 RVA: 0x00049859 File Offset: 0x00047A59
		public DialogueLine(Character character, string text)
		{
			this.character = character;
			this.text = text;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0004986F File Offset: 0x00047A6F
		public DialogueLine WithTrigger(Action trigger)
		{
			this.trigger = trigger;
			return this;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x00049879 File Offset: 0x00047A79
		public static DialogueLine cDL(Character character, string line)
		{
			return new DialogueLine(character, line);
		}

		// Token: 0x04000556 RID: 1366
		public Character character;

		// Token: 0x04000557 RID: 1367
		public string text;

		// Token: 0x04000558 RID: 1368
		public Action trigger;
	}
}
