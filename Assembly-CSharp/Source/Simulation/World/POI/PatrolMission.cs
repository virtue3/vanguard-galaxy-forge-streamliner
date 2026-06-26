using System;
using Source.Galaxy;
using Source.MissionSystem;

namespace Source.Simulation.World.POI
{
	// Token: 0x02000080 RID: 128
	public class PatrolMission : PoiStoryteller
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x000272F5 File Offset: 0x000254F5
		public PatrolMission(MapPointOfInterest poi) : base(poi)
		{
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00027300 File Offset: 0x00025500
		public override void UpdateActive(float deltaTime)
		{
			this.updateTimer -= deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				if (this.poi.totalEnemyCount == 0)
				{
					MissionObjective.Trigger(MissionTrigger.PatrolWaveFinished, 99, null, false);
				}
				if (this.poi.activeEnemyCount > 0)
				{
					this.waveStarted = true;
					return;
				}
				if (this.waveStarted)
				{
					this.waveStarted = false;
					MissionObjective.Trigger(MissionTrigger.PatrolWaveFinished, null, null, false);
				}
			}
		}

		// Token: 0x04000285 RID: 645
		private float updateTimer;

		// Token: 0x04000286 RID: 646
		private bool waveStarted;
	}
}
