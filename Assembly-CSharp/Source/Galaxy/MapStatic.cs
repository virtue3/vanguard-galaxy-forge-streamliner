using System;
using Behaviour.GalaxyMap;
using Behaviour.UI;
using LightJson;
using Source.Util;

namespace Source.Galaxy
{
	// Token: 0x02000147 RID: 327
	public abstract class MapStatic : MapElement
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000C6D RID: 3181
		public abstract WorldMapStatic Prefab { get; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000594A0 File Offset: 0x000576A0
		public virtual string typeName
		{
			get
			{
				return "@MapStatic" + base.GetType().Name;
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x000594B7 File Offset: 0x000576B7
		public override float GetLastVisitedTime()
		{
			return 0f;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x000594BE File Offset: 0x000576BE
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			tooltip.AddTextLine("Can't travel to location.", 12, 8f).Text.color = ColorHelper.reddish;
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000594E1 File Offset: 0x000576E1
		public override void DataToJson(JsonObject json)
		{
			json["type"] = base.GetType().Name;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x000594FE File Offset: 0x000576FE
		public static MapStatic Create(string name)
		{
			return (MapStatic)Type.GetType("Source.Galaxy.Statics." + name).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0005952C File Offset: 0x0005772C
		public static MapStatic FromJson(JsonValue val)
		{
			JsonObject asJsonObject = val.AsJsonObject;
			MapStatic mapStatic = MapStatic.Create(asJsonObject["type"]);
			mapStatic.LoadFromJson(asJsonObject);
			return mapStatic;
		}
	}
}
