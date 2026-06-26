using System;
using System.Collections.Generic;
using LightJson;

namespace Source.Player
{
	// Token: 0x0200008F RID: 143
	public class AutopilotSessionStats : IJsonSource
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x0003120C File Offset: 0x0002F40C
		public bool DoWeKeepThis()
		{
			return this.stats.Count != 0 && GamePlayer.current.elapsedTime - (double)this.startTime >= 300.0;
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0003123D File Offset: 0x0002F43D
		public int GetTotalTime()
		{
			return (int)(((this.endTime > 0f) ? this.endTime : ((float)GamePlayer.current.elapsedTime)) - this.startTime);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00031268 File Offset: 0x0002F468
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<IdleStat, int> keyValuePair in this.stats)
			{
				jsonObject[keyValuePair.Key.ToString()] = new double?((double)keyValuePair.Value);
			}
			return new JsonObject
			{
				{
					"shipName",
					this.shipName
				},
				{
					"shipActivity",
					this.shipActivity
				},
				{
					"startTime",
					new double?((double)this.startTime)
				},
				{
					"endTime",
					new double?((double)this.endTime)
				},
				{
					"stats",
					jsonObject
				}
			};
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0003136C File Offset: 0x0002F56C
		public static AutopilotSessionStats FromJson(JsonValue json)
		{
			AutopilotSessionStats autopilotSessionStats = new AutopilotSessionStats();
			autopilotSessionStats.shipName = json["shipName"];
			autopilotSessionStats.startTime = (float)json["startTime"].AsNumber;
			autopilotSessionStats.endTime = (float)json["endTime"].AsNumber;
			if (json["stats"].AsJsonObject != null)
			{
				foreach (KeyValuePair<string, JsonValue> keyValuePair in json["stats"].AsJsonObject)
				{
					autopilotSessionStats.stats[Enum.Parse<IdleStat>(keyValuePair.Key)] = (int)keyValuePair.Value.AsNumber;
				}
			}
			if (json["shipActivity"].IsString)
			{
				autopilotSessionStats.shipActivity = json["shipActivity"];
			}
			return autopilotSessionStats;
		}

		// Token: 0x040002B5 RID: 693
		public const int KeepStatsAfterSeconds = 300;

		// Token: 0x040002B6 RID: 694
		public string shipName;

		// Token: 0x040002B7 RID: 695
		public string shipActivity;

		// Token: 0x040002B8 RID: 696
		public float startTime;

		// Token: 0x040002B9 RID: 697
		public float endTime;

		// Token: 0x040002BA RID: 698
		public Dictionary<IdleStat, int> stats = new Dictionary<IdleStat, int>();
	}
}
