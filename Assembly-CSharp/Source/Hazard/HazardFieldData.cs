using System;
using Behaviour.Hazard;
using LightJson;
using Source.Combat;
using Source.Util;

namespace Source.Hazard
{
	// Token: 0x020000FC RID: 252
	public class HazardFieldData : IJsonSource
	{
		// Token: 0x0600095E RID: 2398 RVA: 0x00047DC8 File Offset: 0x00045FC8
		public string GetHazardName()
		{
			return Translation.Translate("@" + this.damageType.ToString(), Array.Empty<object>());
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00047DF0 File Offset: 0x00045FF0
		public static HazardFieldData CreateRandom(float randomChance = 0.4f)
		{
			HazardName random = LocalHazard.GetRandom();
			return new HazardFieldData
			{
				hazardName = random,
				damageType = LocalHazard.GetRandomDamageTypeForHazard(random),
				spawnChance = randomChance
			};
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00047E24 File Offset: 0x00046024
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"generalChance",
					new double?((double)this.spawnChance)
				},
				{
					"hazardName",
					this.hazardName.ToString()
				},
				{
					"damageType",
					this.damageType.ToString()
				}
			};
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00047E9C File Offset: 0x0004609C
		public static HazardFieldData FromJson(JsonValue json)
		{
			return new HazardFieldData
			{
				spawnChance = (float)json["generalChance"].AsNumber,
				hazardName = Enum.Parse<HazardName>(json["hazardName"]),
				damageType = Enum.Parse<DamageType>(json["damageType"])
			};
		}

		// Token: 0x0400052A RID: 1322
		public float spawnChance;

		// Token: 0x0400052B RID: 1323
		public HazardName hazardName;

		// Token: 0x0400052C RID: 1324
		public DamageType damageType;
	}
}
