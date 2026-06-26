using System;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000179 RID: 377
	public class Amalgam : Faction
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000E04 RID: 3588 RVA: 0x00065EB3 File Offset: 0x000640B3
		public override bool offersMissionsForShip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x00065EB6 File Offset: 0x000640B6
		public Amalgam()
		{
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 1;
		}
	}
}
