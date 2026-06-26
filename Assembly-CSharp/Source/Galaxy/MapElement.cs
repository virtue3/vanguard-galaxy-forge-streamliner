using System;
using Behaviour.UI;
using LightJson;
using Source.Galaxy.NameGenerator;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000145 RID: 325
	public abstract class MapElement : IJsonSource
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000C0D RID: 3085 RVA: 0x000574C5 File Offset: 0x000556C5
		// (set) Token: 0x06000C0E RID: 3086 RVA: 0x000574CD File Offset: 0x000556CD
		public string guid { get; protected set; } = Guid.NewGuid().ToString();

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C0F RID: 3087 RVA: 0x000574D6 File Offset: 0x000556D6
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x000574F2 File Offset: 0x000556F2
		public string name
		{
			get
			{
				if (this._name == null)
				{
					this._name = this.GenerateDefaultName();
				}
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x000574FB File Offset: 0x000556FB
		// (set) Token: 0x06000C12 RID: 3090 RVA: 0x00057503 File Offset: 0x00055703
		public virtual Faction faction { get; set; }

		// Token: 0x06000C13 RID: 3091 RVA: 0x0005750C File Offset: 0x0005570C
		protected virtual string GenerateDefaultName()
		{
			return EmptySpace.GenerateEmptySpaceName();
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00057513 File Offset: 0x00055713
		public void SetIdentifier(string guid)
		{
			this.guid = guid;
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0005751C File Offset: 0x0005571C
		public virtual Color GetColor()
		{
			return Color.white;
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00057523 File Offset: 0x00055723
		public virtual float GetSize()
		{
			return 1f;
		}

		// Token: 0x06000C17 RID: 3095
		public abstract float GetLastVisitedTime();

		// Token: 0x06000C18 RID: 3096
		public abstract void ActiveUpdate(float delta);

		// Token: 0x06000C19 RID: 3097 RVA: 0x0005752A File Offset: 0x0005572A
		public virtual void AmbientUpdate(float delta)
		{
		}

		// Token: 0x06000C1A RID: 3098
		public abstract void AddTooltipInfo(UITooltip tooltip);

		// Token: 0x06000C1B RID: 3099 RVA: 0x0005752C File Offset: 0x0005572C
		public virtual void LoadFromJson(JsonObject data)
		{
			this.guid = data["guid"];
			this.position = JsonUtil.JsonObjectToVector2(data["position"]);
			this.name = data["name"];
			this.level = data["level"].AsInteger;
			if (data.ContainsKey("faction"))
			{
				this.faction = Faction.Get(data["faction"]);
			}
			if (data.ContainsKey("systemName"))
			{
				GalaxyMapData.current.LoadSystem(data["systemName"], delegate(SystemMapData sys)
				{
					this.system = sys;
				});
			}
		}

		// Token: 0x06000C1C RID: 3100
		public abstract void DataToJson(JsonObject data);

		// Token: 0x06000C1D RID: 3101 RVA: 0x000575F4 File Offset: 0x000557F4
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject
			{
				{
					"guid",
					this.guid
				},
				{
					"position",
					JsonUtil.Vector2ToJson(this.position)
				},
				{
					"name",
					this.name
				},
				{
					"level",
					new double?((double)this.level)
				}
			};
			if (this.system != null)
			{
				jsonObject["systemName"] = this.system.guid;
			}
			if (this.faction != null)
			{
				jsonObject["faction"] = this.faction.identifier;
			}
			this.DataToJson(jsonObject);
			return jsonObject;
		}

		// Token: 0x040006D6 RID: 1750
		public SystemMapData system;

		// Token: 0x040006D7 RID: 1751
		private string _name;

		// Token: 0x040006D8 RID: 1752
		public Vector2 position;

		// Token: 0x040006D9 RID: 1753
		public int level;
	}
}
