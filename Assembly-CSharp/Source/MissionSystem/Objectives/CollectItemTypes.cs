using System;
using System.Collections.Generic;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C1 RID: 193
	public class CollectItemTypes : MissionObjective
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0003E19C File Offset: 0x0003C39C
		public int currentAmount
		{
			get
			{
				return this.currentTypes.Count;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x0003E1A9 File Offset: 0x0003C3A9
		// (set) Token: 0x060007BD RID: 1981 RVA: 0x0003E1B1 File Offset: 0x0003C3B1
		public override GameplayType gameplayType { get; set; } = GameplayType.Mining;

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0003E1BC File Offset: 0x0003C3BC
		public override string statusText
		{
			get
			{
				string text;
				if (this.itemCategory != null)
				{
					text = Translation.Translate("@ItemCategory" + this.itemCategory.ToString(), Array.Empty<object>());
				}
				else
				{
					text = "Item";
				}
				return string.Concat(new string[]
				{
					text,
					" types found: ",
					this.currentAmount.ToString(),
					"/",
					this.requiredAmount.ToString()
				});
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0003E242 File Offset: 0x0003C442
		public override bool IsComplete()
		{
			return this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060007C0 RID: 1984 RVA: 0x0003E255 File Offset: 0x0003C455
		public override MissionTrigger? triggeredBy
		{
			get
			{
				return new MissionTrigger?(MissionTrigger.ItemCollected);
			}
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0003E25D File Offset: 0x0003C45D
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0003E260 File Offset: 0x0003C460
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			ValueTuple<TractorableItemData, AbstractUnitData> valueTuple = (ValueTuple<TractorableItemData, AbstractUnitData>)data;
			TractorableItemData item = valueTuple.Item1;
			if (valueTuple.Item2.faction != Faction.player)
			{
				return;
			}
			if (this.itemCategory != null)
			{
				ItemCategory itemCategory = item.itemType.itemCategory;
				ItemCategory? itemCategory2 = this.itemCategory;
				if (!(itemCategory == itemCategory2.GetValueOrDefault() & itemCategory2 != null))
				{
					return;
				}
			}
			this.currentTypes.Add(item.itemType.identifier);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0003E2D6 File Offset: 0x0003C4D6
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0003E2DC File Offset: 0x0003C4DC
		protected override void DataToJson(JsonObject data)
		{
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			JsonArray jsonArray = new JsonArray();
			foreach (string value in this.currentTypes)
			{
				jsonArray.Add(value);
			}
			data["currentTypes"] = jsonArray;
			if (this.itemCategory != null)
			{
				data["itemCategory"] = this.itemCategory.Value.ToString();
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0003E3A0 File Offset: 0x0003C5A0
		protected override void LoadFromJson(JsonObject data)
		{
			this.requiredAmount = data["requiredAmount"];
			foreach (JsonValue jsonValue in data["currentTypes"].AsJsonArray)
			{
				this.currentTypes.Add(jsonValue);
			}
			if (data["itemCategory"].IsString)
			{
				this.itemCategory = new ItemCategory?(Enum.Parse<ItemCategory>(data["itemCategory"]));
			}
		}

		// Token: 0x04000450 RID: 1104
		public ItemCategory? itemCategory;

		// Token: 0x04000451 RID: 1105
		public int requiredAmount;

		// Token: 0x04000452 RID: 1106
		private HashSet<string> currentTypes = new HashSet<string>();
	}
}
