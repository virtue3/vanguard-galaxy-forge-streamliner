using System;
using System.Collections.Generic;
using LightJson;
using Source.MissionSystem;
using Source.Player;
using UnityEngine;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000163 RID: 355
	public class PatrolBoard : IJsonSource
	{
		// Token: 0x06000DA6 RID: 3494 RVA: 0x000627B5 File Offset: 0x000609B5
		public PatrolBoard(SpaceStation parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000627C4 File Offset: 0x000609C4
		public List<PatrolMission> GeneratePatrolMissions()
		{
			SeededRandom seededRandom = new SeedGenerator().Add("PatrolMissions").Add(this.parent.guid).Add(DateTime.Now.DayOfYear).Add(this.patrolCounter).CreateRandom();
			List<PatrolMission> list = new List<PatrolMission>();
			Faction faction = Faction.marauders;
			if (!faction.IsEnemy(Faction.player) || (seededRandom.RandomBool(0.5f) && this.parent.IsEnemyAvailable(Faction.fanatics)))
			{
				faction = Faction.fanatics;
			}
			for (int i = 0; i < 3; i++)
			{
				int num;
				if (i != 1)
				{
					if (i != 2)
					{
						num = this.parent.level;
					}
					else
					{
						num = this.parent.level + 5 + GamePlayer.current.patrolRank;
					}
				}
				else
				{
					num = this.parent.level + 3 + Mathf.CeilToInt((float)GamePlayer.current.patrolRank * 0.5f);
				}
				int missionLevel = num;
				PatrolMission patrolMission = new PatrolMission();
				patrolMission.SetupPatrolWave(this.parent, i, missionLevel, 0, faction, seededRandom, this.patrolCounter);
				list.Add(patrolMission);
			}
			return list;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000628FC File Offset: 0x00060AFC
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"patrolCounter",
					new double?((double)this.patrolCounter)
				}
			};
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00062925 File Offset: 0x00060B25
		public static PatrolBoard FromJson(JsonObject json, SpaceStation parent)
		{
			return new PatrolBoard(parent)
			{
				patrolCounter = json["patrolCounter"]
			};
		}

		// Token: 0x0400076F RID: 1903
		private readonly SpaceStation parent;

		// Token: 0x04000770 RID: 1904
		public int patrolCounter;
	}
}
