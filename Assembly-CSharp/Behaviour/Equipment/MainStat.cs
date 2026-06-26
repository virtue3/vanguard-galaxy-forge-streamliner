using System;
using Source.Util;

namespace Behaviour.Equipment
{
	// Token: 0x0200033D RID: 829
	public class MainStat
	{
		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x000BB0C2 File Offset: 0x000B92C2
		// (set) Token: 0x06001F88 RID: 8072 RVA: 0x000BB0CA File Offset: 0x000B92CA
		public string mainStatName { get; private set; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x000BB0D3 File Offset: 0x000B92D3
		// (set) Token: 0x06001F8A RID: 8074 RVA: 0x000BB0DB File Offset: 0x000B92DB
		public string mainStatAmount { get; private set; }

		// Token: 0x06001F8B RID: 8075 RVA: 0x000BB0E4 File Offset: 0x000B92E4
		public MainStat(string name, float amount)
		{
			this.mainStatName = name;
			this.mainStatAmount = GameMath.FormatNumber(amount, -1);
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x000BB100 File Offset: 0x000B9300
		public MainStat(string name, string amount)
		{
			this.mainStatName = name;
			this.mainStatAmount = amount;
		}
	}
}
