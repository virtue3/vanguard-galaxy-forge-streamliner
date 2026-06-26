using System;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C6 RID: 198
	public class DroneTrigger : TriggerObjective
	{
		// Token: 0x060007E3 RID: 2019 RVA: 0x0003E9FC File Offset: 0x0003CBFC
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			if (!GameplayManager.Instance.spaceShip.droneBayModule)
			{
				return;
			}
			base.ProcessMissionTrigger(trigger, data);
		}
	}
}
