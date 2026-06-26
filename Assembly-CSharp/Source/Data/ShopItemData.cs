using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.Data
{
	// Token: 0x0200010B RID: 267
	[Serializable]
	public class ShopItemData
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0004DECB File Offset: 0x0004C0CB
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x0004DED3 File Offset: 0x0004C0D3
		public List<FactionPrerequisites> factionPrereq { get; private set; } = new List<FactionPrerequisites>();

		// Token: 0x0400058A RID: 1418
		[SerializeField]
		public int minAreaLevelRequirement;

		// Token: 0x0400058B RID: 1419
		[SerializeField]
		public int maxAreaLevelRequirement;

		// Token: 0x0400058C RID: 1420
		[SerializeField]
		public int levelRequirement;

		// Token: 0x0400058D RID: 1421
		[SerializeField]
		public bool conquestCommendations;
	}
}
