using System;
using Behaviour.Equipment;
using Behaviour.Item;
using LightJson;
using Source.Galaxy;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C4 RID: 196
	public class Crafting : MissionObjective
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0003E58C File Offset: 0x0003C78C
		public override string statusText
		{
			get
			{
				string text;
				if (this.requiredEquipment != null)
				{
					text = "equipment";
				}
				else if (this.requiredCategory != null)
				{
					text = Translation.Translate("@ItemCategory" + this.requiredCategory.ToString(), Array.Empty<object>()).ToLower();
				}
				else
				{
					text = "item";
				}
				if (this.requiredRarity != null)
				{
					text = this.requiredRarity.Value.GetDisplayName().ToLower() + " " + text;
				}
				return string.Concat(new string[]
				{
					"Craft ",
					text,
					": ",
					this.currentAmount.ToString(),
					"/",
					this.requiredAmount.ToString()
				});
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0003E65F File Offset: 0x0003C85F
		public override MissionTrigger? triggeredBy
		{
			get
			{
				return new MissionTrigger?(MissionTrigger.CraftItem);
			}
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0003E668 File Offset: 0x0003C868
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x0003E66B File Offset: 0x0003C86B
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0003E66E File Offset: 0x0003C86E
		public override bool IsComplete()
		{
			return this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0003E684 File Offset: 0x0003C884
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			ValueTuple<InventoryItemType, int> valueTuple = (ValueTuple<InventoryItemType, int>)data;
			InventoryItemType item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			if (this.requiredCategory != null)
			{
				ItemCategory itemCategory = item.itemCategory;
				ItemCategory? itemCategory2 = this.requiredCategory;
				if (!(itemCategory == itemCategory2.GetValueOrDefault() & itemCategory2 != null))
				{
					return;
				}
			}
			if (this.requiredRarity != null)
			{
				Rarity rarity = item.rarity;
				Rarity? rarity2 = this.requiredRarity;
				if (rarity < rarity2.GetValueOrDefault() & rarity2 != null)
				{
					return;
				}
			}
			if (this.requiredEquipment != null && item.GetComponent<AbstractEquipment>() != this.requiredEquipment.Value)
			{
				return;
			}
			this.currentAmount = Math.Min(this.currentAmount + item2, this.requiredAmount);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0003E740 File Offset: 0x0003C940
		protected override void DataToJson(JsonObject data)
		{
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			data["currentAmount"] = new double?((double)this.currentAmount);
			if (this.requiredCategory != null)
			{
				data["requiredCategory"] = this.requiredCategory.ToString();
			}
			if (this.requiredRarity != null)
			{
				data["requiredRarity"] = this.requiredRarity.ToString();
			}
			if (this.requiredEquipment != null)
			{
				data["requiredEquipment"] = new bool?(this.requiredEquipment.Value);
			}
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0003E810 File Offset: 0x0003CA10
		protected override void LoadFromJson(JsonObject data)
		{
			this.requiredAmount = data["requiredAmount"];
			this.currentAmount = data["currentAmount"];
			if (data["requiredCategory"].IsString)
			{
				this.requiredCategory = new ItemCategory?(Enum.Parse<ItemCategory>(data["requiredCategory"]));
			}
			if (data["requiredRarity"].IsString)
			{
				this.requiredRarity = new Rarity?(Enum.Parse<Rarity>(data["requiredRarity"]));
			}
			if (!data["requiredEquipment"].IsNull)
			{
				this.requiredEquipment = new bool?(data["requiredEquipment"].AsBoolean);
			}
		}

		// Token: 0x04000456 RID: 1110
		public int requiredAmount = 1;

		// Token: 0x04000457 RID: 1111
		public int currentAmount;

		// Token: 0x04000458 RID: 1112
		public ItemCategory? requiredCategory;

		// Token: 0x04000459 RID: 1113
		public Rarity? requiredRarity;

		// Token: 0x0400045A RID: 1114
		public bool? requiredEquipment;
	}
}
