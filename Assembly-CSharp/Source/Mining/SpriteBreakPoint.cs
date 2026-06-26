using System;
using System.Collections.Generic;
using Behaviour.Util;
using LightJson;
using UnityEngine;

namespace Source.Mining
{
	// Token: 0x020000E6 RID: 230
	public class SpriteBreakPoint : IJsonSource
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00044E90 File Offset: 0x00043090
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x00044E98 File Offset: 0x00043098
		public Vector2 position { get; private set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00044EA1 File Offset: 0x000430A1
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x00044EA9 File Offset: 0x000430A9
		public int size { get; private set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00044EB2 File Offset: 0x000430B2
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00044EBA File Offset: 0x000430BA
		public bool core { get; private set; }

		// Token: 0x060008BC RID: 2236 RVA: 0x00044EC3 File Offset: 0x000430C3
		public SpriteBreakPoint(Vector2 position, int size, bool core = false)
		{
			this.position = position;
			this.size = size;
			this.core = core;
			this.brokenPixels = new List<Vector2Int>();
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00044EEC File Offset: 0x000430EC
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"position",
					JsonUtil.Vector2ToJson(this.position)
				},
				{
					"size",
					new double?((double)this.size)
				},
				{
					"core",
					new bool?(this.core)
				}
			};
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00044F58 File Offset: 0x00043158
		public static SpriteBreakPoint FromJson(JsonValue json)
		{
			return new SpriteBreakPoint(JsonUtil.JsonObjectToVector2(json["position"]), json["size"].AsInteger, json["core"].AsBoolean);
		}

		// Token: 0x04000496 RID: 1174
		public List<Vector2Int> brokenPixels;

		// Token: 0x04000497 RID: 1175
		public BreakDelayedSprite surfaceDelayedSprite;
	}
}
