using System;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x020003A4 RID: 932
	public class CrewIcon
	{
		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06002340 RID: 9024 RVA: 0x000C9FA7 File Offset: 0x000C81A7
		public string identifier
		{
			get
			{
				return this.sprite.name;
			}
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000C9FB4 File Offset: 0x000C81B4
		public CrewIcon(Sprite icon, bool male)
		{
			this.sprite = icon;
			this.isMale = male;
		}

		// Token: 0x0400151A RID: 5402
		public Sprite sprite;

		// Token: 0x0400151B RID: 5403
		public bool isMale;

		// Token: 0x0400151C RID: 5404
		public bool hidden;
	}
}
