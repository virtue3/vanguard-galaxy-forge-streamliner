using System;

namespace Source.Crew
{
	// Token: 0x02000128 RID: 296
	[Flags]
	public enum Profession
	{
		// Token: 0x04000615 RID: 1557
		None = 0,
		// Token: 0x04000616 RID: 1558
		Mining = 1,
		// Token: 0x04000617 RID: 1559
		Engineering = 4,
		// Token: 0x04000618 RID: 1560
		Salvaging = 16,
		// Token: 0x04000619 RID: 1561
		Combat = 32,
		// Token: 0x0400061A RID: 1562
		Industrial = 64
	}
}
