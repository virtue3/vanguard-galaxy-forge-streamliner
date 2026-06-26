using System;
using Behaviour.GalaxyMap;
using Behaviour.UI;

namespace Source.Galaxy.POI
{
	// Token: 0x02000155 RID: 341
	public class GreatGate : JumpGate
	{
		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0005D655 File Offset: 0x0005B855
		public override string sceneName
		{
			get
			{
				return "JumpGate";
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0005D65C File Offset: 0x0005B85C
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0005D65F File Offset: 0x0005B85F
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("JumpGate");
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0005D66B File Offset: 0x0005B86B
		public override void AddTooltipInfo(UITooltip tooltip)
		{
		}
	}
}
