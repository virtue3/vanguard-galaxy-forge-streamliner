using System;
using System.Collections.Generic;
using LightJson;
using Source.Data.Persistable;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World.POI;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000161 RID: 353
	public class IndustryBoard : IJsonSource
	{
		// Token: 0x06000D8C RID: 3468 RVA: 0x00062036 File Offset: 0x00060236
		public IndustryBoard(SpaceStation parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00062045 File Offset: 0x00060245
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"industryCounter",
					new double?((double)this.industryCounter)
				}
			};
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0006206E File Offset: 0x0006026E
		public static IndustryBoard FromJson(JsonObject json, SpaceStation parent)
		{
			return new IndustryBoard(parent)
			{
				industryCounter = json["industryCounter"]
			};
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0006208C File Offset: 0x0006028C
		public List<IndustryMission> GenerateIndustryMissions()
		{
			SeededRandom seededRandom = new SeedGenerator().Add("IndustryMissions").Add(this.parent.guid).Add(DateTime.Now.DayOfYear).Add(this.industryCounter).CreateRandom();
			List<IndustryMission> list = new List<IndustryMission>();
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
						num = this.parent.level + 5 + GamePlayer.current.industryRank;
					}
				}
				else
				{
					num = this.parent.level + 3 + Mathf.CeilToInt((float)GamePlayer.current.industryRank * 0.5f);
				}
				int level = num;
				IndustryStation industryStation = new IndustryStation();
				this.parent.system.SetupPOI(industryStation, null, Faction.industrialGuild, 0);
				IndustrialOutpost storyteller = new IndustrialOutpost(industryStation);
				industryStation.storyteller = storyteller;
				industryStation.name = "@IndustryMissionPOI";
				industryStation.level = level;
				industryStation.forge = new Forge(industryStation);
				industryStation.AddPersistable(new PrefabData
				{
					prefabName = "IndustryDockingArea",
					position = industryStation.GetWorldPosition() + new Vector2(5f, 1f)
				});
				CombatStationFactory.CreateEasyStation1(industryStation, new Vector2?(new Vector2(11f, 0f)));
				CombatStationFactory.CreateGunPlatform(industryStation, Faction.industrialGuild, new Vector2?(new Vector2(20f, 5f)), (float)seededRandom.RandomRange(-45, 45), 1);
				IndustryMission item = IndustryMission.Create(this.parent, industryStation, i, 0);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x04000764 RID: 1892
		private readonly SpaceStation parent;

		// Token: 0x04000765 RID: 1893
		public int industryCounter;
	}
}
