using System;
using Behaviour.Item;
using Behaviour.Managers;
using LightJson;
using Source.Hazard;
using UnityEngine;

namespace Source.Data.Persistable
{
	// Token: 0x0200011A RID: 282
	public abstract class PersistableData : IJsonSource
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x0004F8DB File Offset: 0x0004DADB
		public Quaternion rotation
		{
			get
			{
				return Quaternion.Euler(0f, 0f, this.angle);
			}
		}

		// Token: 0x06000AB3 RID: 2739
		public abstract GameObject AddToWorld(BasePoiManager parent);

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0004F8F2 File Offset: 0x0004DAF2
		public void AddHazardToWorld(GameObject gameObject, float scale)
		{
			if (this.hazardData != null)
			{
				this.hazardData.AddToWorld(gameObject, scale);
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0004F909 File Offset: 0x0004DB09
		public virtual bool ShouldCleanUp()
		{
			return false;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0004F90C File Offset: 0x0004DB0C
		public virtual bool ShouldKeepPoiAlive()
		{
			return false;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x0004F90F File Offset: 0x0004DB0F
		public virtual bool ItemCanBeObtained(InventoryItemType itemType)
		{
			return false;
		}

		// Token: 0x06000AB8 RID: 2744
		public abstract void DataToJson(JsonObject json);

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0004F914 File Offset: 0x0004DB14
		public virtual JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["type"] = base.GetType().Name;
			jsonObject["position"] = JsonUtil.Vector2ToJson(this.position);
			jsonObject["angle"] = new double?((double)this.angle);
			jsonObject["velocity"] = JsonUtil.Vector2ToJson(this.velocity);
			jsonObject["angularVelocity"] = new double?((double)this.angularVelocity);
			if (this.hazardData != null)
			{
				jsonObject["hazard"] = this.hazardData.ToJson();
			}
			this.DataToJson(jsonObject);
			return jsonObject;
		}

		// Token: 0x06000ABA RID: 2746
		public abstract void LoadFromJson(JsonObject json);

		// Token: 0x06000ABB RID: 2747 RVA: 0x0004F9DB File Offset: 0x0004DBDB
		public static PersistableData Create(string name)
		{
			return (PersistableData)Type.GetType("Source.Data.Persistable." + name).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0004FA08 File Offset: 0x0004DC08
		public static PersistableData FromJson(JsonValue json)
		{
			PersistableData persistableData = PersistableData.Create(json["type"]);
			persistableData.position = JsonUtil.JsonObjectToVector2(json["position"]);
			persistableData.angle = (float)json["angle"].AsNumber;
			persistableData.velocity = JsonUtil.JsonObjectToVector2(json["velocity"]);
			persistableData.angularVelocity = (float)json["angularVelocity"].AsNumber;
			if (!json["hazard"].IsNull)
			{
				persistableData.hazardData = HazardData.FromJson(json["hazard"]);
			}
			persistableData.LoadFromJson(json);
			return persistableData;
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0004FAD4 File Offset: 0x0004DCD4
		public virtual void OffsetPosition(Vector2 diff)
		{
			this.position += diff;
		}

		// Token: 0x040005BB RID: 1467
		public Vector2 position;

		// Token: 0x040005BC RID: 1468
		public float angle;

		// Token: 0x040005BD RID: 1469
		public Vector2 velocity;

		// Token: 0x040005BE RID: 1470
		public float angularVelocity;

		// Token: 0x040005BF RID: 1471
		public HazardData hazardData;
	}
}
