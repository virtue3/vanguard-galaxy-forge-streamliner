using System;
using Source.Item;
using UnityEngine;

namespace Behaviour.Equipment.Builder
{
	// Token: 0x02000376 RID: 886
	public class EquipmentBuilderVisual : MonoBehaviour
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600223E RID: 8766 RVA: 0x000C6EA5 File Offset: 0x000C50A5
		// (set) Token: 0x0600223F RID: 8767 RVA: 0x000C6EAD File Offset: 0x000C50AD
		public string displayName { get; protected set; }

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06002240 RID: 8768 RVA: 0x000C6EB6 File Offset: 0x000C50B6
		// (set) Token: 0x06002241 RID: 8769 RVA: 0x000C6EBE File Offset: 0x000C50BE
		public string description { get; protected set; }

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x000C6EC7 File Offset: 0x000C50C7
		// (set) Token: 0x06002243 RID: 8771 RVA: 0x000C6ECF File Offset: 0x000C50CF
		public Sprite icon { get; protected set; }

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x000C6ED8 File Offset: 0x000C50D8
		// (set) Token: 0x06002245 RID: 8773 RVA: 0x000C6EE0 File Offset: 0x000C50E0
		public bool useSuffix { get; protected set; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002246 RID: 8774 RVA: 0x000C6EE9 File Offset: 0x000C50E9
		// (set) Token: 0x06002247 RID: 8775 RVA: 0x000C6EF1 File Offset: 0x000C50F1
		public Manufacturer manufacturer { get; protected set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x000C6EFA File Offset: 0x000C50FA
		// (set) Token: 0x06002249 RID: 8777 RVA: 0x000C6F02 File Offset: 0x000C5102
		public Rarity availableRarity { get; protected set; }
	}
}
