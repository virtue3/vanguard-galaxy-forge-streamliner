using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.Util;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000064 RID: 100
	public class CallForReinforcementsActions : CombatActions
	{
		// Token: 0x060003BD RID: 957 RVA: 0x0001E824 File Offset: 0x0001CA24
		public CallForReinforcementsActions(AbstractUnit parent) : base(parent)
		{
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001E82D File Offset: 0x0001CA2D
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0001E830 File Offset: 0x0001CA30
		public override void OnDamageTaken(DamageData data)
		{
			MapPointOfInterest current = MapPointOfInterest.current;
			if (current == null)
			{
				return;
			}
			if (!this.called && data.sourceUnit && data.sourceUnit.IsPlayer(false))
			{
				this.called = true;
				this.parent.unitData.autoActions = "Combat";
				MapPointOfInterest mapPointOfInterest = current;
				MapPointOfInterest mapPointOfInterest2 = current;
				float pointsScale = 0.6f;
				Faction faction = this.parent.faction;
				mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest2.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), faction, 0, 0, 1, 5, null), 8f, 0, false, true);
			}
		}

		// Token: 0x04000222 RID: 546
		public bool called;
	}
}
