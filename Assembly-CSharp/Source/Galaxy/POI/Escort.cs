using System;
using Behaviour.GalaxyMap;

namespace Source.Galaxy.POI
{
	// Token: 0x02000154 RID: 340
	public class Escort : Combat
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0005D63E File Offset: 0x0005B83E
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("Escort");
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0005D64A File Offset: 0x0005B84A
		public override bool hasCombatMusic
		{
			get
			{
				return true;
			}
		}
	}
}
