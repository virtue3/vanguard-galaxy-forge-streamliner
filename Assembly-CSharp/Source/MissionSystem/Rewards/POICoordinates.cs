using System;
using LightJson;
using Source.Galaxy;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B8 RID: 184
	public class POICoordinates : MissionReward
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0003DC31 File Offset: 0x0003BE31
		public override string rewardText
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0003DC38 File Offset: 0x0003BE38
		public override void DataToJson(JsonObject data)
		{
			data["POICoordinates"] = this.poi.ToJson();
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x0003DC50 File Offset: 0x0003BE50
		public override void LoadFromJson(JsonObject data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x0003DC57 File Offset: 0x0003BE57
		public override void OnComplete(Mission m)
		{
		}

		// Token: 0x04000446 RID: 1094
		public MapPointOfInterest poi;
	}
}
