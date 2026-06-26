using System;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200017D RID: 381
	public class Fanatics : Faction
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0006613C File Offset: 0x0006433C
		public override bool offersMissionsForShip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0006613F File Offset: 0x0006433F
		public Fanatics()
		{
			this.conquestColor = "#7F00CE".HexToColor();
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 1;
		}
	}
}
