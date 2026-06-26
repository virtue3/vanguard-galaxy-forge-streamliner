using System;
using Behaviour.Hazard;
using LightJson;
using Source.Data.Persistable;
using Source.Galaxy;
using UnityEngine;

namespace Source.Hazard
{
	// Token: 0x020000FD RID: 253
	public class MineHazardData : HazardData
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x00047F09 File Offset: 0x00046109
		public override bool attachToObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00047F0C File Offset: 0x0004610C
		public override void AddToWorld(GameObject source, float rangeModifier)
		{
			for (int i = 0; i < this.amount; i++)
			{
				Vector2 vector = new Vector2(source.transform.position.x + (float)((SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * SeededRandom.Global.RandomRange(i, i * 2)), source.transform.position.y + (float)((SeededRandom.Global.RandomBool(0.5f) ? -1 : 1) * SeededRandom.Global.RandomRange(i, i * 2)));
				vector = source.GetComponent<Collider2D>().ClosestPoint(vector);
				UnityEngine.Object.Instantiate<LocalHazard>(LocalHazard.Get(this.hazard.name), vector, Quaternion.identity, source.transform).Init(source, this, rangeModifier);
			}
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00047FDE File Offset: 0x000461DE
		public override PersistableData CreatePersistableData()
		{
			base.persistable = new MineData();
			base.persistable.hazardData = this;
			return base.persistable;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00048000 File Offset: 0x00046200
		public override void DataToJson(JsonObject json)
		{
			base.DataToJson(json);
			json["acquiringRange"] = new double?((double)this.acquiringRange);
			json["homingSpeed"] = new double?((double)this.homingSpeed);
			json["amount"] = new double?((double)this.amount);
			json["faction"] = this.faction.identifier;
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00048084 File Offset: 0x00046284
		public override void LoadFromJson(JsonObject json)
		{
			base.LoadFromJson(json);
			this.acquiringRange = (float)json["acquiringRange"].AsNumber;
			this.homingSpeed = (float)json["homingSpeed"].AsNumber;
			this.amount = json["amount"].AsInteger;
			this.faction = Faction.Get(json["faction"]);
		}

		// Token: 0x0400052D RID: 1325
		public float acquiringRange;

		// Token: 0x0400052E RID: 1326
		public float homingSpeed;

		// Token: 0x0400052F RID: 1327
		public int amount;

		// Token: 0x04000530 RID: 1328
		public Faction faction;
	}
}
