using System;
using Behaviour.Managers;
using Behaviour.Tractoring;
using Behaviour.Unit;
using Source.Data;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Player;

namespace Source.Simulation.World.POI
{
	// Token: 0x0200007E RID: 126
	public class HelpMiner : PoiStoryteller
	{
		// Token: 0x06000490 RID: 1168 RVA: 0x000264F5 File Offset: 0x000246F5
		public HelpMiner(MapPointOfInterest poi) : base(poi)
		{
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00026500 File Offset: 0x00024700
		public override void UpdateActive(float deltaTime)
		{
			this.updateTimer -= deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				if (this.supportMission == null)
				{
					foreach (Mission mission in GamePlayer.current.allMissions)
					{
						if (mission.dynamicPointOfInterest == this.poi)
						{
							this.supportMission = mission;
							break;
						}
					}
				}
				if (!this.supportUnit)
				{
					foreach (AbstractUnitData abstractUnitData in this.poi.GetUnits(false))
					{
						if (abstractUnitData.autoActions == "DefenseSubject")
						{
							this.supportUnit = abstractUnitData.unit;
							break;
						}
					}
				}
				if (this.supportUnit && !this.leaving)
				{
					bool flag = false;
					foreach (TractorableItem tractorableItem in BasePoiManager.current.GetComponentsInChildren<TractorableItem>())
					{
						if (tractorableItem && !tractorableItem.isTractored && tractorableItem.data.ownerFaction == this.supportUnit.faction)
						{
							this.supportUnit.SetOverrideDestination(tractorableItem.targetablePosition, false, false, false);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.supportUnit.SetBrakeDestination();
					}
				}
				Mission mission2 = this.supportMission;
				if (mission2 != null && mission2.isComplete)
				{
					this.poi.oreOwnershipOverride = null;
					if (this.supportUnit)
					{
						this.leaving = true;
						this.supportUnit.autoActions.ExitPOI();
					}
				}
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000266D4 File Offset: 0x000248D4
		public override void UpdateAmbient(float deltaTime)
		{
		}

		// Token: 0x04000278 RID: 632
		private Mission supportMission;

		// Token: 0x04000279 RID: 633
		private AbstractUnit supportUnit;

		// Token: 0x0400027A RID: 634
		private bool leaving;

		// Token: 0x0400027B RID: 635
		private float updateTimer;
	}
}
