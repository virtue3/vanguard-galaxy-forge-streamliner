using System;
using LightJson;
using Source.Galaxy;

namespace Source.Simulation.World
{
	// Token: 0x02000075 RID: 117
	public abstract class SystemStoryteller
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x000233EC File Offset: 0x000215EC
		public string identifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000233F9 File Offset: 0x000215F9
		public SystemStoryteller(SystemMapData system)
		{
			this.system = system;
		}

		// Token: 0x06000451 RID: 1105
		public abstract void SetupSystem();

		// Token: 0x06000452 RID: 1106 RVA: 0x00023408 File Offset: 0x00021608
		public virtual TravelDynamicEvent TriggerDynamicEvent()
		{
			return null;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0002340B File Offset: 0x0002160B
		public virtual void Start()
		{
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0002340D File Offset: 0x0002160D
		public virtual void UpdateActive(float deltaTime)
		{
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0002340F File Offset: 0x0002160F
		public virtual void UpdateAmbient(float deltaTime)
		{
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00023411 File Offset: 0x00021611
		public virtual void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00023413 File Offset: 0x00021613
		public virtual void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00023415 File Offset: 0x00021615
		public static SystemStoryteller Create(string id, SystemMapData system)
		{
			return (SystemStoryteller)Type.GetType("Source.Simulation.World.System." + id).GetConstructor(new Type[]
			{
				typeof(SystemMapData)
			}).Invoke(new object[]
			{
				system
			});
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00023454 File Offset: 0x00021654
		public static SystemStoryteller FromJson(JsonValue data, SystemMapData system)
		{
			SystemStoryteller systemStoryteller = SystemStoryteller.Create(data["identifier"], system);
			if (data.IsJsonObject)
			{
				systemStoryteller.DataFromJson(data);
			}
			return systemStoryteller;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00023490 File Offset: 0x00021690
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			this.DataToJson(jsonObject);
			jsonObject["identifier"] = this.identifier;
			return jsonObject;
		}

		// Token: 0x04000256 RID: 598
		public readonly SystemMapData system;
	}
}
