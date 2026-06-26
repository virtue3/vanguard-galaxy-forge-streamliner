using System;
using Behaviour.GalaxyMap;
using Behaviour.UI;

namespace Source.Galaxy.POI
{
	// Token: 0x0200014E RID: 334
	public class Beacon : MapPointOfInterest
	{
		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000CE1 RID: 3297 RVA: 0x0005CF7C File Offset: 0x0005B17C
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("Beacon");
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x0005CF88 File Offset: 0x0005B188
		public override string sceneName
		{
			get
			{
				return "Space";
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x0005CF8F File Offset: 0x0005B18F
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0005CF92 File Offset: 0x0005B192
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			tooltip.AddTextLine("Your mysterious employer...", 12, 8f);
		}
	}
}
