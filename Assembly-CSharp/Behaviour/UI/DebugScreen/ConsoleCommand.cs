using System;

namespace Behaviour.UI.DebugScreen
{
	// Token: 0x02000297 RID: 663
	public struct ConsoleCommand
	{
		// Token: 0x06001895 RID: 6293 RVA: 0x0009A6C1 File Offset: 0x000988C1
		public ConsoleCommand(string cmd, string shortCmd, Action<string> a, string desc = "")
		{
			this.command = cmd;
			this.shortCommand = shortCmd;
			this.action = a;
			this.description = desc;
		}

		// Token: 0x04000F46 RID: 3910
		public string command;

		// Token: 0x04000F47 RID: 3911
		public string shortCommand;

		// Token: 0x04000F48 RID: 3912
		public Action<string> action;

		// Token: 0x04000F49 RID: 3913
		public string description;
	}
}
