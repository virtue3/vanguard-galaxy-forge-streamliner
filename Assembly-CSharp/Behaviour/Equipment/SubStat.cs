using System;
using Source.Util;

namespace Behaviour.Equipment
{
	// Token: 0x0200033E RID: 830
	public class SubStat
	{
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000BB116 File Offset: 0x000B9316
		// (set) Token: 0x06001F8E RID: 8078 RVA: 0x000BB11E File Offset: 0x000B931E
		public string mainSubStatName { get; private set; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000BB127 File Offset: 0x000B9327
		// (set) Token: 0x06001F90 RID: 8080 RVA: 0x000BB12F File Offset: 0x000B932F
		public string mainSubStatAmount { get; private set; }

		// Token: 0x06001F91 RID: 8081 RVA: 0x000BB138 File Offset: 0x000B9338
		public SubStat(string name, float amount)
		{
			this.mainSubStatName = name;
			this.mainSubStatAmount = GameMath.FormatNumber(amount, -1);
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000BB154 File Offset: 0x000B9354
		public SubStat(string name, string amount)
		{
			this.mainSubStatName = name;
			this.mainSubStatAmount = amount;
		}
	}
}
