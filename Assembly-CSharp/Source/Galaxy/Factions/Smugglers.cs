using System;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x0200018A RID: 394
	public class Smugglers : Faction
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00066BA2 File Offset: 0x00064DA2
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat || GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Cargo;
			}
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00066BCC File Offset: 0x00064DCC
		public Smugglers()
		{
			this.allowCrossFactionShipUse = false;
			base.missionTypes.Add("Courier");
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("EscortShip");
			base.missionTypes.Add("TradeTerminal");
		}
	}
}
