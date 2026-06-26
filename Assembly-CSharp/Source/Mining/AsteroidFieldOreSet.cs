using System;
using System.Collections.Generic;
using Behaviour.Mining;
using LightJson;

namespace Source.Mining
{
	// Token: 0x020000E5 RID: 229
	public class AsteroidFieldOreSet : IJsonSource
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00044C64 File Offset: 0x00042E64
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x00044C6C File Offset: 0x00042E6C
		public List<OreItemData> wildcardOres { get; private set; }

		// Token: 0x060008B1 RID: 2225 RVA: 0x00044C75 File Offset: 0x00042E75
		public AsteroidFieldOreSet(OreItemData common, OreItemData rare = null, List<OreItemData> wildcards = null)
		{
			this.commonOre = common;
			this.rareOre = (rare ?? common);
			List<OreItemData> wildcardOres = wildcards;
			if (wildcards == null)
			{
				(wildcardOres = new List<OreItemData>()).Add(rare ?? common);
			}
			this.wildcardOres = wildcardOres;
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00044CAC File Offset: 0x00042EAC
		public OreItemData GetRandomOre(SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			if (random.RandomBool(0.85f))
			{
				return this.commonOre;
			}
			if (random.RandomBool(0.8f))
			{
				return this.rareOre;
			}
			return random.Choose<OreItemData>(this.wildcardOres);
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00044CEC File Offset: 0x00042EEC
		public bool HasOre(OreItemData oreItemData, bool checkWildcard = false)
		{
			return this.commonOre == oreItemData || this.rareOre == oreItemData || (checkWildcard && this.wildcardOres.Contains(oreItemData));
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00044D24 File Offset: 0x00042F24
		public JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (OreItemData oreItemData in this.wildcardOres)
			{
				jsonArray.Add(oreItemData.item.identifier);
			}
			return new JsonObject
			{
				{
					"commonOre",
					this.commonOre.item.identifier
				},
				{
					"rareOre",
					this.rareOre.item.identifier
				},
				{
					"wildcardOres",
					jsonArray
				}
			};
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00044DEC File Offset: 0x00042FEC
		public static AsteroidFieldOreSet FromJson(JsonValue data)
		{
			List<OreItemData> list = new List<OreItemData>();
			foreach (JsonValue jsonValue in data["wildcardOres"].AsJsonArray)
			{
				list.Add(jsonValue.AsString);
			}
			return new AsteroidFieldOreSet(data["commonOre"].AsString, data["rareOre"].AsString, list);
		}

		// Token: 0x0400048E RID: 1166
		public const float CommonChance = 0.85f;

		// Token: 0x0400048F RID: 1167
		public const float RareChance = 0.8f;

		// Token: 0x04000490 RID: 1168
		public OreItemData commonOre;

		// Token: 0x04000491 RID: 1169
		public OreItemData rareOre;
	}
}
