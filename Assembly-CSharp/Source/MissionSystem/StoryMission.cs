using System;
using System.Collections.Generic;
using Source.MissionSystem.Story;
using Source.Player;

namespace Source.MissionSystem
{
	// Token: 0x020000AB RID: 171
	public class StoryMission
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0003A586 File Offset: 0x00038786
		public static IEnumerable<StoryMission> all
		{
			get
			{
				return StoryMission.allMissions.Values;
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0003A592 File Offset: 0x00038792
		static StoryMission()
		{
			new TutorialMissions();
			new UmbralMissions();
			new SkilltreeMissions();
			new SideMissions();
			new ConquestMissions();
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0003A5BC File Offset: 0x000387BC
		public StoryMission(string id, StoryMission.CreateMission gen, Func<GamePlayer, bool> available = null, string pickupHint = null)
		{
			this.identifier = id;
			this.generator = gen;
			this.checkAvailable = available;
			this.pickupHint = pickupHint;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0003A5E1 File Offset: 0x000387E1
		public bool IsAvailableFor(GamePlayer player)
		{
			Func<GamePlayer, bool> func = this.checkAvailable;
			return func == null || func(player);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0003A5F5 File Offset: 0x000387F5
		public static Mission Get(GamePlayer player, string id)
		{
			Mission mission = StoryMission.allMissions[id].generator(player);
			mission.storyId = id;
			return mission;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0003A614 File Offset: 0x00038814
		public static StoryMission Get(string id)
		{
			return StoryMission.allMissions[id];
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0003A621 File Offset: 0x00038821
		public static bool IsAvailable(GamePlayer player, string id)
		{
			return StoryMission.allMissions[id].IsAvailableFor(player);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x0003A634 File Offset: 0x00038834
		public static void Add(StoryMission m)
		{
			StoryMission.allMissions[m.identifier] = m;
		}

		// Token: 0x0400043A RID: 1082
		private static Dictionary<string, StoryMission> allMissions = new Dictionary<string, StoryMission>();

		// Token: 0x0400043B RID: 1083
		public readonly string identifier;

		// Token: 0x0400043C RID: 1084
		public string pickupHint;

		// Token: 0x0400043D RID: 1085
		private StoryMission.CreateMission generator;

		// Token: 0x0400043E RID: 1086
		private Func<GamePlayer, bool> checkAvailable;

		// Token: 0x0200043A RID: 1082
		// (Invoke) Token: 0x06002761 RID: 10081
		public delegate Mission CreateMission(GamePlayer owner);
	}
}
