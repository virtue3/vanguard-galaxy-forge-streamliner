using System;
using Behaviour.GalaxyMap;
using Behaviour.UI;

namespace Source.Galaxy.POI
{
	// Token: 0x0200015D RID: 349
	public class TutorialJumpgate : MapPointOfInterest
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x000615EB File Offset: 0x0005F7EB
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("JumpGate");
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x000615F7 File Offset: 0x0005F7F7
		public override string sceneName
		{
			get
			{
				return "Space";
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x000615FE File Offset: 0x0005F7FE
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00061601 File Offset: 0x0005F801
		public override void AddTooltipInfo(UITooltip tooltip)
		{
		}
	}
}
