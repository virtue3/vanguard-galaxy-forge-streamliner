using System;
using System.Collections.Generic;
using Source.MissionSystem;

namespace Source.Dialogues
{
	// Token: 0x02000100 RID: 256
	public class Dialogue
	{
		// Token: 0x04000552 RID: 1362
		public MissionTrigger trigger;

		// Token: 0x04000553 RID: 1363
		public Func<List<DialogueLine>> dialogues;

		// Token: 0x04000554 RID: 1364
		public Action onComplete;

		// Token: 0x04000555 RID: 1365
		public bool conditionalTrigger = true;
	}
}
