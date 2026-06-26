using System;
using Source.Util;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C7 RID: 199
	public class Item : Mining
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0003EA28 File Offset: 0x0003CC28
		public override string statusText
		{
			get
			{
				return string.Concat(new string[]
				{
					Translation.Translate(this.itemType.displayName, Array.Empty<object>()),
					" gained: ",
					base.currentAmount.ToString(),
					"/",
					this.requiredAmount.ToString()
				});
			}
		}
	}
}
