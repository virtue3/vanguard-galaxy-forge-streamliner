using System;
using Behaviour.UI;
using Behaviour.UI.Spacestation.Bar;
using LightJson;
using UnityEngine;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x0200015F RID: 351
	public abstract class BarPatron : IJsonSource
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x000618B0 File Offset: 0x0005FAB0
		public string identifier
		{
			get
			{
				return base.GetType().Name;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000D78 RID: 3448
		public abstract string seed { get; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000D79 RID: 3449
		public abstract string name { get; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000D7A RID: 3450
		public abstract bool isMale { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000D7B RID: 3451
		public abstract Sprite icon { get; }

		// Token: 0x06000D7C RID: 3452
		public abstract void InteractWithPatron(BarUI ui);

		// Token: 0x06000D7D RID: 3453
		public abstract void AddTooltipContent(UITooltip tooltip);

		// Token: 0x06000D7E RID: 3454
		public abstract void DataFromJson(JsonObject data);

		// Token: 0x06000D7F RID: 3455
		public abstract void DataToJson(JsonObject data);

		// Token: 0x06000D80 RID: 3456 RVA: 0x000618BD File Offset: 0x0005FABD
		public BarPatron(SpaceStation ss)
		{
			this.spaceStation = ss;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x000618CC File Offset: 0x0005FACC
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.InitializeData();
			this.initialized = true;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000618E4 File Offset: 0x0005FAE4
		public virtual bool ConflictsWith(BarPatron other)
		{
			return this.name == other.name || this.icon == other.icon;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0006190C File Offset: 0x0005FB0C
		protected virtual void InitializeData()
		{
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x0006190E File Offset: 0x0005FB0E
		public static BarPatron Create(string id, SpaceStation ss)
		{
			return (BarPatron)Type.GetType("Source.Galaxy.POI.Station.Patrons." + id).GetConstructor(new Type[]
			{
				ss.GetType()
			}).Invoke(new object[]
			{
				ss
			});
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00061948 File Offset: 0x0005FB48
		public static BarPatron FromJson(JsonValue data, SpaceStation ss)
		{
			BarPatron barPatron = BarPatron.Create(data["identifier"], ss);
			barPatron.seat = data["seat"];
			barPatron.DataFromJson(data);
			return barPatron;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00061984 File Offset: 0x0005FB84
		public JsonValue ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			this.DataToJson(jsonObject);
			jsonObject["identifier"] = this.identifier;
			jsonObject["seat"] = new double?((double)this.seat);
			return jsonObject;
		}

		// Token: 0x0400075E RID: 1886
		public int seat;

		// Token: 0x0400075F RID: 1887
		protected SpaceStation spaceStation;

		// Token: 0x04000760 RID: 1888
		private bool initialized;
	}
}
