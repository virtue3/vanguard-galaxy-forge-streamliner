using System;
using Behaviour.Unit;
using Source.Util;

namespace Source.Galaxy.Factions
{
	// Token: 0x02000181 RID: 385
	public class Marauders : Faction
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00066326 File Offset: 0x00064526
		public override bool offersMissionsForShip
		{
			get
			{
				return GameplayManager.Instance.spaceShip.GetPreferredGameplayType(false) == GameplayType.Combat;
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x0006633C File Offset: 0x0006453C
		public Marauders()
		{
			this.conquestColor = "#AF0017".HexToColor();
			this.allowCrossFactionShipUse = false;
			this.minShipVariety = 2;
			base.missionTypes.Add("BountyHunt");
			base.missionTypes.Add("ClearAsteroidField");
			base.missionTypes.Add("ClearSalvageField");
			base.missionTypes.Add("MineOre");
			base.missionTypes.Add("SalvageWreck");
			base.missionTypes.Add("TradeTerminal");
			base.missionTypes.Add("StationBattle");
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000663DD File Offset: 0x000645DD
		public override bool IsNPCShipAvailable(int level, Behaviour.Unit.SpaceShip ship, GameplayType? activity)
		{
			return level < 20 || base.IsNPCShipAvailable(level, ship, activity);
		}
	}
}
