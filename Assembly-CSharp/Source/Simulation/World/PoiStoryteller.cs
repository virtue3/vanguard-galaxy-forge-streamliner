using System;
using LightJson;
using Source.Galaxy;

namespace Source.Simulation.World
{
	// Token: 0x02000073 RID: 115
	public abstract class PoiStoryteller
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00021314 File Offset: 0x0001F514
		public string identifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00021321 File Offset: 0x0001F521
		public PoiStoryteller(MapPointOfInterest poi)
		{
			this.poi = poi;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00021330 File Offset: 0x0001F530
		public virtual void UpdateActive(float deltaTime)
		{
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00021332 File Offset: 0x0001F532
		public virtual void UpdateAmbient(float deltaTime)
		{
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00021334 File Offset: 0x0001F534
		public virtual void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00021336 File Offset: 0x0001F536
		public virtual void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00021338 File Offset: 0x0001F538
		public static PoiStoryteller Create(string id, MapPointOfInterest poi)
		{
			return (PoiStoryteller)Type.GetType("Source.Simulation.World.POI." + id).GetConstructor(new Type[]
			{
				typeof(MapPointOfInterest)
			}).Invoke(new object[]
			{
				poi
			});
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00021378 File Offset: 0x0001F578
		public static PoiStoryteller FromJson(JsonValue data, MapPointOfInterest poi)
		{
			PoiStoryteller poiStoryteller = PoiStoryteller.Create(data["identifier"], poi);
			if (data.IsJsonObject)
			{
				poiStoryteller.DataFromJson(data);
			}
			return poiStoryteller;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000213B4 File Offset: 0x0001F5B4
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			this.DataToJson(jsonObject);
			jsonObject["identifier"] = this.identifier;
			return jsonObject;
		}

		// Token: 0x04000248 RID: 584
		public readonly MapPointOfInterest poi;
	}
}
