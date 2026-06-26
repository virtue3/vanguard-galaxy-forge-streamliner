using System;
using Behaviour.GalaxyMap;
using Behaviour.UI;
using Behaviour.Weapons;
using Source.Player;
using Source.Util;

namespace Source.Galaxy.POI
{
	// Token: 0x02000159 RID: 345
	public class Salvage : MapPointOfInterest
	{
		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0005E34B File Offset: 0x0005C54B
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("Salvage");
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000D2B RID: 3371 RVA: 0x0005E357 File Offset: 0x0005C557
		public override string sceneName
		{
			get
			{
				return "Space";
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0005E35E File Offset: 0x0005C55E
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0005E361 File Offset: 0x0005C561
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			if (!GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Salvage, TargetLayer.Both))
			{
				tooltip.AddTextLine("@SalvageNoTurrets", 12, 8f).Text.color = ColorHelper.reddish;
			}
		}
	}
}
