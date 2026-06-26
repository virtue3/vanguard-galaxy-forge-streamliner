using System;
using Behaviour.Tractoring;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.Player;

namespace Behaviour.Travel
{
	// Token: 0x020002CE RID: 718
	public class TutorialJumpgateTractor : JumpgateTractor
	{
		// Token: 0x06001A3B RID: 6715 RVA: 0x000A34CC File Offset: 0x000A16CC
		protected override bool CanTractorItem(TractorableItem tItem)
		{
			foreach (Mission mission in GamePlayer.current.missions)
			{
				MissionStep currentStep = mission.currentStep;
				if (currentStep == null)
				{
					return false;
				}
				foreach (MissionObjective missionObjective in currentStep.objectives)
				{
					TriggerObjective triggerObjective = missionObjective as TriggerObjective;
					if (triggerObjective != null && !triggerObjective.IsComplete())
					{
						string identifier = tItem.data.itemType.identifier;
						if ((triggerObjective.trigger == MissionTrigger.TutorialJumpgateStructure && identifier == "Structural Component") || (triggerObjective.trigger == MissionTrigger.TutorialJumpgatePlates && identifier == "Titanium Plate") || (triggerObjective.trigger == MissionTrigger.TutorialJumpgateConduit && identifier == "Conduit Cell") || (triggerObjective.trigger == MissionTrigger.TutorialJumpgateBeacon && identifier == "Signal Array"))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x000A3608 File Offset: 0x000A1808
		public override void ItemTractored(TractorableItem item)
		{
			string identifier = item.data.itemType.identifier;
			if (identifier == "Structural Component")
			{
				MissionObjective.Trigger(MissionTrigger.TutorialJumpgateStructure, item.data.itemAmount, null, false);
			}
			if (identifier == "Titanium Plate")
			{
				MissionObjective.Trigger(MissionTrigger.TutorialJumpgatePlates, item.data.itemAmount, null, false);
			}
			if (identifier == "Conduit Cell")
			{
				MissionObjective.Trigger(MissionTrigger.TutorialJumpgateConduit, item.data.itemAmount, null, false);
			}
			if (identifier == "Signal Array")
			{
				MissionObjective.Trigger(MissionTrigger.TutorialJumpgateBeacon, item.data.itemAmount, null, false);
			}
		}
	}
}
