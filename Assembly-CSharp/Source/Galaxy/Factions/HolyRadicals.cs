using System;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200017F RID: 383
	public class HolyRadicals : Faction
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000E10 RID: 3600 RVA: 0x00066269 File Offset: 0x00064469
		public override bool offersMissionsForShip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0006626C File Offset: 0x0006446C
		public HolyRadicals()
		{
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 1;
		}
	}
}
