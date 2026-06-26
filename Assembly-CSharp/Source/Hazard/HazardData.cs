using System;
using Behaviour.Hazard;
using Behaviour.Managers;
using LightJson;
using Source.Combat;
using Source.Data.Persistable;
using UnityEngine;

namespace Source.Hazard
{
	// Token: 0x020000FB RID: 251
	public class HazardData : IJsonSource
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00047B8A File Offset: 0x00045D8A
		public virtual bool attachToObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00047B8D File Offset: 0x00045D8D
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x00047B95 File Offset: 0x00045D95
		public PersistableData persistable { get; protected set; }

		// Token: 0x06000955 RID: 2389 RVA: 0x00047B9E File Offset: 0x00045D9E
		public virtual void AddToWorld(GameObject source, float rangeModifier)
		{
			UnityEngine.Object.Instantiate<LocalHazard>(LocalHazard.Get(this.hazard.name), source.transform).Init(source, this, rangeModifier);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00047BC4 File Offset: 0x00045DC4
		public virtual GameObject AddToWorld(BasePoiManager poiManager, PersistableData data)
		{
			LocalHazard localHazard = UnityEngine.Object.Instantiate<LocalHazard>(LocalHazard.Get(this.hazard.name), data.position, Quaternion.identity, poiManager.transform);
			localHazard.Init(null, this, 1f);
			this.persistable = data;
			return localHazard.gameObject;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00047C15 File Offset: 0x00045E15
		public virtual PersistableData CreatePersistableData()
		{
			return null;
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00047C18 File Offset: 0x00045E18
		public virtual void DataToJson(JsonObject json)
		{
			json["hazard"] = this.hazard.name;
			json["damageType"] = this.damageType.ToString();
			json["damageMultiplier"] = new double?((double)this.damageMultiplier);
			json["maxDamageFalloff"] = new double?((double)this.maxDamageFalloffPercentage);
			json["range"] = new double?((double)this.range);
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x00047CB5 File Offset: 0x00045EB5
		public static HazardData Create(string name)
		{
			return (HazardData)Type.GetType("Source.Hazard." + name + "HazardData").GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00047CE7 File Offset: 0x00045EE7
		public static HazardData FromJson(JsonValue json)
		{
			HazardData hazardData = HazardData.Create(json["hazard"]);
			hazardData.LoadFromJson(json);
			return hazardData;
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00047D0C File Offset: 0x00045F0C
		public virtual void LoadFromJson(JsonObject json)
		{
			this.hazard = LocalHazard.Get(json["hazard"]);
			this.damageType = Enum.Parse<DamageType>(json["damageType"]);
			this.damageMultiplier = (float)json["damageMultiplier"].AsNumber;
			this.maxDamageFalloffPercentage = (float)json["maxDamageFalloff"].AsNumber;
			this.range = (float)json["range"].AsNumber;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00047DA0 File Offset: 0x00045FA0
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			this.DataToJson(jsonObject);
			return jsonObject;
		}

		// Token: 0x04000524 RID: 1316
		public LocalHazard hazard;

		// Token: 0x04000525 RID: 1317
		public DamageType damageType;

		// Token: 0x04000526 RID: 1318
		public float damageMultiplier;

		// Token: 0x04000527 RID: 1319
		public float range;

		// Token: 0x04000528 RID: 1320
		public float maxDamageFalloffPercentage;
	}
}
