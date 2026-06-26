using System;
using Source.Item;

namespace Source.SpaceShip
{
	// Token: 0x0200005C RID: 92
	[Serializable]
	public class SpaceShipModule
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000398 RID: 920 RVA: 0x0001DFFE File Offset: 0x0001C1FE
		// (set) Token: 0x06000399 RID: 921 RVA: 0x0001E006 File Offset: 0x0001C206
		public EquipmentSlot slot { get; private set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0001E00F File Offset: 0x0001C20F
		// (set) Token: 0x0600039B RID: 923 RVA: 0x0001E017 File Offset: 0x0001C217
		public ModuleSize size { get; private set; }
	}
}
