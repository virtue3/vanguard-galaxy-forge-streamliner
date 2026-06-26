using System;
using Behaviour.Equipment.Module;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.Unit;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C9 RID: 201
	public class Mining : MissionObjective
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0003EC99 File Offset: 0x0003CE99
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0003ECA1 File Offset: 0x0003CEA1
		public int currentAmount { get; protected set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0003ECAA File Offset: 0x0003CEAA
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0003ECB2 File Offset: 0x0003CEB2
		public override GameplayType gameplayType { get; set; } = GameplayType.Mining;

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0003ECBC File Offset: 0x0003CEBC
		public override string statusText
		{
			get
			{
				string str;
				if (this.itemType)
				{
					str = Translation.Translate(this.itemType.displayName, Array.Empty<object>());
				}
				else if (this.itemCategory != null)
				{
					str = Translation.Translate("@ItemCategory" + this.itemCategory.ToString(), Array.Empty<object>());
				}
				else
				{
					str = "Items";
				}
				string text = str + " collected";
				if (this.miningFaction != null && this.miningFaction != Faction.player)
				{
					text = text + " by " + Translation.Translate(this.miningFaction.name, Array.Empty<object>());
				}
				return string.Concat(new string[]
				{
					text,
					": ",
					this.currentAmount.ToString(),
					"/",
					this.requiredAmount.ToString()
				});
			}
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0003EDA9 File Offset: 0x0003CFA9
		public override bool IsComplete()
		{
			return this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0003EDBC File Offset: 0x0003CFBC
		public override MissionTrigger? triggeredBy
		{
			get
			{
				return new MissionTrigger?(MissionTrigger.ItemCollected);
			}
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0003EDC4 File Offset: 0x0003CFC4
		public override Sprite GetIcon()
		{
			InventoryItemType inventoryItemType = this.itemType;
			if (inventoryItemType == null)
			{
				return null;
			}
			return inventoryItemType.icon;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0003EDD8 File Offset: 0x0003CFD8
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			ValueTuple<TractorableItemData, AbstractUnitData> valueTuple = (ValueTuple<TractorableItemData, AbstractUnitData>)data;
			TractorableItemData item = valueTuple.Item1;
			AbstractUnitData item2 = valueTuple.Item2;
			if (this.itemType && item.itemType != this.itemType)
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
			if (this.miningFaction == null && item2.faction != Faction.player)
			{
				return;
			}
			if (this.miningFaction != null && item2.faction != this.miningFaction)
			{
				return;
			}
			if (this.targetPOI == null || this.targetPOI == GamePlayer.current.currentPointOfInterest.guid)
			{
				this.currentAmount = Math.Min(this.currentAmount + item.itemAmount, this.requiredAmount);
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0003EEBB File Offset: 0x0003D0BB
		public override MapPointOfInterest GetPoi()
		{
			return GalaxyMapData.current.GetPointOfInterest(this.targetPOI);
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0003EED0 File Offset: 0x0003D0D0
		public override bool LoadoutCanRetrieveItem(MapPointOfInterest poi)
		{
			OreItemData oreItemData;
			if (!this.IsComplete() && this.itemType.TryGetComponent<OreItemData>(out oreItemData))
			{
				GameplayManager instance = GameplayManager.Instance;
				object obj;
				if (instance == null)
				{
					obj = null;
				}
				else
				{
					SpaceShip spaceShip = instance.spaceShip;
					obj = ((spaceShip != null) ? spaceShip.GetModule<MiningModule>() : null);
				}
				object obj2 = obj;
				return obj2 != null && obj2.CanMineItemFromFieldData(poi.asteroidFieldData, this.itemType);
			}
			return this.IsComplete();
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0003EF30 File Offset: 0x0003D130
		protected override void DataToJson(JsonObject data)
		{
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			data["currentAmount"] = new double?((double)this.currentAmount);
			data["targetPOI"] = this.targetPOI;
			if (this.itemType)
			{
				data["itemType"] = this.itemType.identifier;
			}
			if (this.itemCategory != null)
			{
				data["itemCategory"] = this.itemCategory.Value.ToString();
			}
			if (this.miningFaction != null)
			{
				data["miningFaction"] = this.miningFaction.identifier;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0003F00C File Offset: 0x0003D20C
		protected override void LoadFromJson(JsonObject data)
		{
			this.requiredAmount = data["requiredAmount"];
			this.currentAmount = data["currentAmount"];
			if (data["itemType"].IsString)
			{
				this.itemType = data["itemType"].AsString;
			}
			if (data["itemCategory"].IsString)
			{
				this.itemCategory = new ItemCategory?(Enum.Parse<ItemCategory>(data["itemCategory"]));
			}
			if (data["targetPOI"] != JsonValue.Null)
			{
				this.targetPOI = data["targetPOI"];
			}
			if (data["miningFaction"] != JsonValue.Null)
			{
				this.miningFaction = Faction.Get(data["miningFaction"]);
			}
		}

		// Token: 0x04000462 RID: 1122
		public InventoryItemType itemType;

		// Token: 0x04000463 RID: 1123
		public ItemCategory? itemCategory;

		// Token: 0x04000464 RID: 1124
		public int requiredAmount;

		// Token: 0x04000466 RID: 1126
		public Faction miningFaction;

		// Token: 0x04000467 RID: 1127
		public string targetPOI;
	}
}
