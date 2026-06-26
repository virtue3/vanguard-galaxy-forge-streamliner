using System;
using Behaviour.Spacestation.Docking;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.Util;
using UnityEngine;

namespace Source.SpaceShip.Auto
{
	// Token: 0x0200006E RID: 110
	public class TrackingTargetActions : AutoActions
	{
		// Token: 0x060003F7 RID: 1015 RVA: 0x0001F75A File Offset: 0x0001D95A
		public TrackingTargetActions(AbstractUnit parent) : base(parent)
		{
			this.undockTimer = SeededRandom.Global.RandomRange(2f, 8f);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001F780 File Offset: 0x0001D980
		public override void Update(float delta)
		{
			base.Update(delta);
			this.undockTimer -= Time.deltaTime;
			if (this.undockTimer < 0f)
			{
				this.undockTimer = 0.5f;
				DockingState? dockingState = this.spaceShip.spaceShipData.dockingState;
				DockingState dockingState2 = DockingState.Docked;
				if (dockingState.GetValueOrDefault() == dockingState2 & dockingState != null)
				{
					DockingOption dockingOption = SpacestationExteriorManager.Instance.FindDockingOption(this.spaceShip);
					if (dockingOption)
					{
						dockingOption.StartUndockCoroutine();
					}
				}
				if (!base.leaving)
				{
					base.ExitPOI();
				}
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001F814 File Offset: 0x0001DA14
		protected override void RemoveUnit()
		{
			Combat combat = new Combat();
			SystemMapData.current.SetupPOI(combat, null, null, 0);
			Vector3 v = this.parent.transform.rotation * new Vector2(4f, 0f);
			combat.position = SystemMapData.current.GetClearedPosition(MapPointOfInterest.current.position + v);
			combat.name = "@UmbralTrackingBeaconTarget";
			combat.timeLeft = 1800f;
			this.spaceShip.spaceShipData.autoActions = null;
			this.spaceShip.spaceShipData.positionData.position = combat.GetWorldPosition() + new Vector2(12f, -4f);
			this.spaceShip.spaceShipData.alwaysFriendly = false;
			this.spaceShip.spaceShipData.playerFriendly = false;
			this.spaceShip.spaceShipData.travelling = false;
			combat.AddUnit(this.spaceShip.spaceShipData, null, false);
			if (this.spaceShip.spaceShipData.HasLoadout(GameplayType.Mining, TargetLayer.Both))
			{
				AsteroidFieldData systemOreData = SystemMapData.current.systemOreData;
				combat.SetAsteroidFieldData(new AsteroidFieldData(2, 1f, 2f, systemOreData.surfaceOres, systemOreData.coreOres, -1f), 0);
			}
			else if (this.spaceShip.spaceShipData.HasLoadout(GameplayType.Salvage, TargetLayer.Both))
			{
				SalvageData salvageData = new SalvageData
				{
					position = combat.GetWorldPosition() + new Vector2(12f, 2f),
					angle = (float)SeededRandom.Global.RandomRange(0, 360),
					shipTemplate = combat.FindSalvageShipTemplate(this.spaceShip.faction.RandomEnemyFaction(null))
				};
				salvageData.AddScrapContent(combat.level, 0.5f, 2);
				salvageData.AddStructuralContent(combat.level, 2, 1f);
				combat.AddPersistable(salvageData);
			}
			else
			{
				MapPointOfInterest mapPointOfInterest = combat;
				MapPointOfInterest mapPointOfInterest2 = combat;
				float pointsScale = 1f;
				Faction f = this.spaceShip.faction.RandomEnemyFaction(null);
				int maxPointsPerUnit = this.spaceShip.pointValue / 2;
				mapPointOfInterest.AddGuards(mapPointOfInterest2.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), f, 0, maxPointsPerUnit, 1, 1, null), null);
			}
			if (SeededRandom.Global.RandomBool(0.5f))
			{
				this.spaceShip.spaceShipData.autoActions = "CallForReinforcements";
			}
			SystemMapData.current.pointsOfInterest.Add(combat);
			base.RemoveUnit();
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001FAA1 File Offset: 0x0001DCA1
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001FAA4 File Offset: 0x0001DCA4
		public override void OnDamageTaken(DamageData data)
		{
		}

		// Token: 0x0400023D RID: 573
		private float undockTimer;
	}
}
