using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Managers;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Galaxy;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.Player;

namespace Source.SpaceShip.Auto
{
	// Token: 0x02000068 RID: 104
	public class DefenseSubjectActions : AutoActions
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0001EE88 File Offset: 0x0001D088
		protected override bool automaticallyLeave
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001EE8B File Offset: 0x0001D08B
		public DefenseSubjectActions(AbstractUnit parent) : base(parent)
		{
			this.SetMission();
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0001EE9C File Offset: 0x0001D09C
		private void SetMission()
		{
			MapPointOfInterest poi = Singleton<TravelManager>.Instance.targetPoi;
			if (poi == null)
			{
				poi = MapPointOfInterest.current;
			}
			this.mission = GamePlayer.current.missions.FirstOrDefault(delegate(Mission m)
			{
				MissionStep currentStep = m.currentStep;
				if (((currentStep != null) ? currentStep.dynamicPointOfInterest : null) != poi)
				{
					return false;
				}
				List<MissionObjective> objectives = m.currentStep.objectives;
				if (objectives == null)
				{
					return false;
				}
				return objectives.Any((MissionObjective o) => o is ProtectUnit);
			});
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001EEF3 File Offset: 0x0001D0F3
		public override bool DoWeRespawn()
		{
			return false;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0001EEF8 File Offset: 0x0001D0F8
		public override void OnDamageTaken(DamageData data)
		{
			if (this.parent.currentHullHP <= 0f)
			{
				if (!GamePlayer.current.missions.Contains(this.mission))
				{
					return;
				}
				if (this.mission.nextMissionOnFailed == null)
				{
					this.mission.MissionFailed("@UIMissionFailDied");
					return;
				}
				GamePlayer.current.missions.Remove(this.mission);
				Mission mission = StoryMission.Get(GamePlayer.current, this.mission.nextMissionOnFailed);
				GamePlayer.current.AddMissionWithLog(mission);
			}
		}

		// Token: 0x0400022B RID: 555
		protected Mission mission;
	}
}
