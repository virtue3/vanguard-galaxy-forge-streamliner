using System;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x0200013A RID: 314
	public class StarLayerData : IJsonSource
	{
		// Token: 0x06000BB2 RID: 2994 RVA: 0x000554F8 File Offset: 0x000536F8
		public static StarLayerData GenerateFromSystem(SystemMapData system, bool close)
		{
			StarLayerData starLayerData = new StarLayerData();
			SectorMapData sector = system.sector;
			starLayerData.gridTiles = sector.GetStarTiling(close);
			starLayerData.scale = sector.GetStarScale();
			starLayerData.offset = sector.position + system.position / 20f;
			return starLayerData;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0005554C File Offset: 0x0005374C
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"starSize",
					new double?((double)this.starSize)
				},
				{
					"gridTiles",
					JsonUtil.Vector2ToJson(this.gridTiles)
				},
				{
					"scale",
					new double?((double)this.scale)
				},
				{
					"offset",
					JsonUtil.Vector2ToJson(this.offset)
				}
			};
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000555D8 File Offset: 0x000537D8
		public static StarLayerData FromJson(JsonValue json)
		{
			return new StarLayerData
			{
				starSize = (float)json["starSize"].AsNumber,
				gridTiles = JsonUtil.JsonObjectToVector2(json["gridTiles"]),
				scale = (float)json["scale"].AsNumber,
				offset = JsonUtil.JsonObjectToVector2(json["offset"])
			};
		}

		// Token: 0x04000676 RID: 1654
		public float starSize;

		// Token: 0x04000677 RID: 1655
		public Vector2 gridTiles;

		// Token: 0x04000678 RID: 1656
		public float scale;

		// Token: 0x04000679 RID: 1657
		public Vector2 offset;
	}
}
