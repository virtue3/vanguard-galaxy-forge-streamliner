using System;
using System.Collections.Generic;
using Behaviour.UI.Side_Menu;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.TravelEvents;
using Source.Simulation.World;
using Source.Util;

namespace Source.Simulation.Story
{
	// Token: 0x0200008B RID: 139
	public class Sandbox : Storyteller
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x0002F2D0 File Offset: 0x0002D4D0
		public override int maxPlayerLevel
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0002F2D4 File Offset: 0x0002D4D4
		public override int maxBonusSkillpoints
		{
			get
			{
				return 31;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x0002F2D8 File Offset: 0x0002D4D8
		public override int maxReputation
		{
			get
			{
				return ReputationLevel.Respected.GetReputationThreshold();
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0002F2E0 File Offset: 0x0002D4E0
		public Sandbox(GamePlayer ply) : base(ply)
		{
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0002F2E9 File Offset: 0x0002D4E9
		public override void SetupNewGame()
		{
			SandboxWorld.SetupWorld(this.player);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0002F2F6 File Offset: 0x0002D4F6
		public override void Start()
		{
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0002F2F8 File Offset: 0x0002D4F8
		public override void StoryUpdate(float delta)
		{
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0002F2FC File Offset: 0x0002D4FC
		public override TravelDynamicEvent TriggerDynamicEvent()
		{
			if (SectorMapData.current.quadrant != SectorMapData.quadrantFrontier)
			{
				return null;
			}
			List<Type> list = new List<Type>
			{
				typeof(DistressSalvage),
				typeof(DistressCombat),
				typeof(BigFatAsteroids),
				typeof(Salvage)
			};
			return (TravelDynamicEvent)Activator.CreateInstance(SeededRandom.Global.Choose<Type>(list));
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0002F377 File Offset: 0x0002D577
		public override void Cleanup()
		{
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0002F379 File Offset: 0x0002D579
		public void CleanupStory()
		{
			this._cleanupMissions("DarkspaceMission");
			this._cleanupMissions("UmbralMission");
			if (SidePanel.instance && SidePanel.instance.currentTab == SidePanel.SideTabType.Missions)
			{
				SidePanel.instance.RefreshIfOpen();
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0002F3B4 File Offset: 0x0002D5B4
		private void _cleanupMissions(string prefix)
		{
			foreach (StoryMission storyMission in StoryMission.all)
			{
				if (storyMission.identifier.StartsWith(prefix))
				{
					this.player.missionsArchive.Add(storyMission.identifier);
					for (int i = 0; i < this.player.missions.Count; i++)
					{
						if (this.player.missions[i].storyId == storyMission.identifier)
						{
							this.player.RemoveMission(this.player.missions[i], false);
							i--;
						}
					}
				}
			}
		}

		// Token: 0x040002A8 RID: 680
		public const int MaxLevel = 30;
	}
}
