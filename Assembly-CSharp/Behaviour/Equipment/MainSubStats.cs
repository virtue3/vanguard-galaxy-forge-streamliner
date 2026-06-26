using System;
using System.Collections.Generic;

namespace Behaviour.Equipment
{
	// Token: 0x0200033F RID: 831
	public class MainSubStats
	{
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x000BB16A File Offset: 0x000B936A
		// (set) Token: 0x06001F94 RID: 8084 RVA: 0x000BB172 File Offset: 0x000B9372
		public List<SubStat> subStatsList { get; private set; }

		// Token: 0x06001F95 RID: 8085 RVA: 0x000BB17B File Offset: 0x000B937B
		public MainSubStats()
		{
			this.subStatsList = new List<SubStat>();
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000BB18E File Offset: 0x000B938E
		public void AddMainSubStat(string name, float amount)
		{
			this.subStatsList.Add(new SubStat(name, amount));
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000BB1A2 File Offset: 0x000B93A2
		public void AddMainSubStat(string name, string amount)
		{
			this.subStatsList.Add(new SubStat(name, amount));
		}
	}
}
