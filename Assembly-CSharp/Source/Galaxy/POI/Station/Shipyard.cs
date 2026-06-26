using System;
using System.Collections.Generic;
using LightJson;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000166 RID: 358
	public class Shipyard : IJsonSource
	{
		// Token: 0x06000DB6 RID: 3510 RVA: 0x00062E3C File Offset: 0x0006103C
		public void RemoveShip(string ship)
		{
			for (int i = 0; i < this.spaceShips.Count; i++)
			{
				if (this.spaceShips[i].name == ship)
				{
					this.spaceShips.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00062E85 File Offset: 0x00061085
		public JsonValue ToJson()
		{
			return new JsonObject();
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00062E91 File Offset: 0x00061091
		public static Shipyard FromJson(JsonObject data)
		{
			Shipyard shipyard = new Shipyard();
			shipyard.spaceShips.FromJsonArray(data["ships"], new ClassExtensions.ParseJsonValue<ShipyardShip>(ShipyardShip.FromJson));
			return shipyard;
		}

		// Token: 0x0400077A RID: 1914
		public List<ShipyardShip> spaceShips = new List<ShipyardShip>();
	}
}
