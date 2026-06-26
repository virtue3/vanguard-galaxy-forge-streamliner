using System;
using LightJson;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000167 RID: 359
	public class ShipyardShip : IJsonSource
	{
		// Token: 0x06000DBA RID: 3514 RVA: 0x00062ED2 File Offset: 0x000610D2
		public ShipyardShip(string name)
		{
			this.name = name;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x00062EE1 File Offset: 0x000610E1
		public ShipyardShip(string name, int amount)
		{
			this.name = name;
			this.amount = amount;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00062EF7 File Offset: 0x000610F7
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"shipName",
					this.name
				},
				{
					"amount",
					new double?((double)this.amount)
				}
			};
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00062F37 File Offset: 0x00061137
		public static ShipyardShip FromJson(JsonValue json)
		{
			return new ShipyardShip(json["shipName"], json["amount"]);
		}

		// Token: 0x0400077B RID: 1915
		public string name;

		// Token: 0x0400077C RID: 1916
		public int amount;
	}
}
