using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Mining;
using LightJson;
using Source.Item;
using UnityEngine;

namespace Source.Mining
{
	// Token: 0x020000E4 RID: 228
	public class AsteroidFieldData : IJsonSource
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x0004464A File Offset: 0x0004284A
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x00044652 File Offset: 0x00042852
		public int amount { get; private set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0004465B File Offset: 0x0004285B
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00044663 File Offset: 0x00042863
		public float density { get; private set; } = 1f;

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0004466C File Offset: 0x0004286C
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00044674 File Offset: 0x00042874
		public float wealth { get; private set; }

		// Token: 0x060008A3 RID: 2211 RVA: 0x00044680 File Offset: 0x00042880
		public AsteroidFieldData(int amount, float density, float wealth, AsteroidFieldOreSet surface, AsteroidFieldOreSet core, float coreChanceOverride = -1f)
		{
			this.amount = amount;
			this.density = density;
			this.wealth = wealth;
			this.coreChanceOverride = coreChanceOverride;
			this.surfaceOres = surface;
			this.coreOres = core;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000446E1 File Offset: 0x000428E1
		public void SetAmount(int amount)
		{
			this.amount = amount;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x000446EA File Offset: 0x000428EA
		public OreItemData GetRandomOre(bool surface, SeededRandom random = null)
		{
			return (surface ? this.surfaceOres : this.coreOres).GetRandomOre(random);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00044704 File Offset: 0x00042904
		public List<OreItemData> GetAllOresInField()
		{
			List<OreItemData> list = new List<OreItemData>();
			list.Add(this.surfaceOres.commonOre);
			list.Add(this.surfaceOres.rareOre);
			list.Add(this.coreOres.commonOre);
			list.Add(this.coreOres.rareOre);
			foreach (OreItemData item in this.surfaceOres.wildcardOres)
			{
				list.Add(item);
			}
			foreach (OreItemData item2 in this.coreOres.wildcardOres)
			{
				list.Add(item2);
			}
			return list.Distinct<OreItemData>().ToList<OreItemData>();
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x000447F8 File Offset: 0x000429F8
		public OreItemData GetMajorSurfaceOre()
		{
			return this.surfaceOres.commonOre;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00044808 File Offset: 0x00042A08
		public List<OreItemData> GetMajorOres()
		{
			return new List<OreItemData>
			{
				this.surfaceOres.commonOre,
				this.surfaceOres.rareOre,
				this.coreOres.commonOre,
				this.coreOres.rareOre
			};
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00044860 File Offset: 0x00042A60
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject
			{
				{
					"amount",
					new double?((double)this.amount)
				},
				{
					"density",
					new double?((double)this.density)
				},
				{
					"wealth",
					new double?((double)this.wealth)
				},
				{
					"surfaceOres",
					this.surfaceOres.ToJson()
				},
				{
					"coreOres",
					this.coreOres.ToJson()
				}
			};
			if (this.coreChanceOverride >= 0f)
			{
				jsonObject["coreChanceOverride"] = new double?((double)this.coreChanceOverride);
			}
			return jsonObject;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00044928 File Offset: 0x00042B28
		public static AsteroidFieldData FromJson(JsonValue json)
		{
			AsteroidFieldData asteroidFieldData = new AsteroidFieldData(json["amount"].AsInteger, (float)json["density"].AsNumber, (float)json["wealth"].AsNumber, AsteroidFieldOreSet.FromJson(json["surfaceOres"]), AsteroidFieldOreSet.FromJson(json["coreOres"]), -1f);
			if (json["coreChanceOverride"].IsNumber)
			{
				asteroidFieldData.coreChanceOverride = (float)json["coreChanceOverride"].AsNumber;
			}
			return asteroidFieldData;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000449D4 File Offset: 0x00042BD4
		public static float CalculateWealth(int level, float multiplier = 1f)
		{
			float num = Mathf.Lerp(260f, 130f, Mathf.Clamp01((float)level / 40f));
			return (0.7f + (float)level / 65f + SeededRandom.Global.RandomFloat() * ((float)level / num)) * multiplier;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00044A20 File Offset: 0x00042C20
		public static AsteroidFieldOreSet CreateOreSet(int level, bool surface)
		{
			if (!surface)
			{
				level += 10;
			}
			AsteroidFieldData.OreProbabilitySet oreProbabilitySet = new AsteroidFieldData.OreProbabilitySet();
			AsteroidFieldData.OreProbabilitySet oreProbabilitySet2 = new AsteroidFieldData.OreProbabilitySet();
			AsteroidFieldData.OreProbabilitySet oreProbabilitySet3 = new AsteroidFieldData.OreProbabilitySet();
			foreach (OreItemData oreItemData in OreItemData.regularOres)
			{
				int num = oreItemData.item.itemLevel - level;
				if (num < 6 && num > -4 && oreItemData.item.rarity <= Rarity.Enhanced)
				{
					oreProbabilitySet.Add(oreItemData, 1f);
				}
				else if (num < 15 && num > -10 && oreItemData.item.rarity >= Rarity.Enhanced)
				{
					oreProbabilitySet2.Add(oreItemData, oreItemData.occurrence * Mathf.Pow(0.9f, (float)Mathf.Abs(num)));
				}
				else if (num > 0 && num < 30)
				{
					oreProbabilitySet3.Add(oreItemData, oreItemData.occurrence);
				}
			}
			OreItemData common = oreProbabilitySet.ChooseAndRemove() ?? "OreCommon1";
			OreItemData rare = oreProbabilitySet2.ChooseAndRemove() ?? "OreRare1";
			if (level == 1 && surface)
			{
				common = "OreCommon1";
				rare = "OreRare1";
			}
			List<OreItemData> list = new List<OreItemData>();
			for (int i = 0; i < 4; i++)
			{
				OreItemData oreItemData2 = oreProbabilitySet3.ChooseAndRemove();
				if (oreItemData2 != null)
				{
					list.Add(oreItemData2);
				}
			}
			return new AsteroidFieldOreSet(common, rare, list);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00044BA4 File Offset: 0x00042DA4
		public void SwapCommonRare()
		{
			OreItemData commonOre = this.surfaceOres.commonOre;
			this.surfaceOres.commonOre = this.surfaceOres.rareOre;
			this.surfaceOres.rareOre = commonOre;
			OreItemData commonOre2 = this.coreOres.commonOre;
			this.coreOres.commonOre = this.coreOres.rareOre;
			this.coreOres.rareOre = commonOre2;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00044C10 File Offset: 0x00042E10
		public float GetCoreChance(AsteroidSize size)
		{
			if (this.coreChanceOverride >= 0f)
			{
				return this.coreChanceOverride;
			}
			float result;
			switch (size)
			{
			case AsteroidSize.Tiny:
				result = 0.25f;
				break;
			case AsteroidSize.Small:
				result = 0.33f;
				break;
			case AsteroidSize.Medium:
				result = 0.5f;
				break;
			default:
				result = 1f;
				break;
			}
			return result;
		}

		// Token: 0x04000486 RID: 1158
		public const int CoreLevelBoost = 10;

		// Token: 0x0400048A RID: 1162
		public AsteroidFieldOreSet surfaceOres;

		// Token: 0x0400048B RID: 1163
		public AsteroidFieldOreSet coreOres;

		// Token: 0x0400048C RID: 1164
		public List<AsteroidSize> excludeSizes = new List<AsteroidSize>();

		// Token: 0x0400048D RID: 1165
		public float coreChanceOverride = -1f;

		// Token: 0x0200047B RID: 1147
		private class OreProbabilitySet
		{
			// Token: 0x06002822 RID: 10274 RVA: 0x000E3C26 File Offset: 0x000E1E26
			public void Add(OreItemData ore, float chance)
			{
				this.set.Add(ore, chance);
			}

			// Token: 0x06002823 RID: 10275 RVA: 0x000E3C38 File Offset: 0x000E1E38
			public OreItemData ChooseAndRemove()
			{
				OreItemData oreItemData = SeededRandom.Global.Choose<OreItemData>(this.set);
				if (oreItemData != null)
				{
					this.set.Remove(oreItemData);
					return oreItemData;
				}
				using (Dictionary<OreItemData, float>.KeyCollection.Enumerator enumerator = this.set.Keys.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
				return null;
			}

			// Token: 0x040018E8 RID: 6376
			private Dictionary<OreItemData, float> set = new Dictionary<OreItemData, float>();
		}
	}
}
