using System;
using Behaviour.Equipment.Module;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.Unit;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000CF RID: 207
	public class TradeOffer : MissionObjective
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x0003F806 File Offset: 0x0003DA06
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x0003F80E File Offset: 0x0003DA0E
		public int currentAmount { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0003F817 File Offset: 0x0003DA17
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x0003F81F File Offset: 0x0003DA1F
		public int displayedAmount { get; private set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x0003F828 File Offset: 0x0003DA28
		public override string statusText
		{
			get
			{
				return Translation.TranslateOnly("@MissionObjectiveTradeOffer", new object[]
				{
					this.itemType.displayName,
					this.deliverTo.name,
					Mathf.Clamp(this.displayedAmount, 0, this.requiredAmount),
					this.requiredAmount,
					this.itemType.m3 * (float)this.requiredAmount
				});
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x0003F8A4 File Offset: 0x0003DAA4
		public float cargoSpaceRequired
		{
			get
			{
				return (float)this.requiredAmount * this.itemType.m3;
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0003F8B9 File Offset: 0x0003DAB9
		public override bool IsComplete()
		{
			this.UpdateCurrentCount();
			return this.currentAmount >= this.requiredAmount;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0003F8D2 File Offset: 0x0003DAD2
		public override void OnMissionTurnedIn()
		{
			SpaceStation spaceStation = this.deliverTo;
			if (spaceStation == null)
			{
				return;
			}
			spaceStation.ConsumeAvailableItems(this.itemType, this.requiredAmount);
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0003F8F4 File Offset: 0x0003DAF4
		public void UpdateCurrentCount()
		{
			GamePlayer current = GamePlayer.current;
			if (((current != null) ? current.currentSpaceShip : null) == null)
			{
				return;
			}
			SpaceStation spaceStation = this.deliverTo;
			this.currentAmount = ((spaceStation != null) ? spaceStation.CountAvailableItems(this.itemType) : 0);
			this.displayedAmount = this.currentAmount;
			if (MapPointOfInterest.current != this.deliverTo)
			{
				this.displayedAmount += GamePlayer.current.currentSpaceShip.cargo.GetCount(this.itemType);
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0003F974 File Offset: 0x0003DB74
		public override MapPointOfInterest GetPoiForEcho()
		{
			if (this.displayedAmount < this.requiredAmount)
			{
				this.fetchPoi = GamePlayer.current.currentSystem.GetNearestPoiForItem(this.itemType, 1);
				if (this.fetchPoi != null)
				{
					return this.fetchPoi;
				}
			}
			return this.deliverTo;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0003F9C0 File Offset: 0x0003DBC0
		public override MapPointOfInterest GetPoi()
		{
			if (this.displayedAmount < this.requiredAmount)
			{
				if (!this.fetchPoiFetched)
				{
					this.fetchPoi = GamePlayer.current.currentSystem.GetNearestPoiForItem(this.itemType, 1);
					this.fetchPoiFetched = true;
				}
				if (this.fetchPoi != null)
				{
					return this.fetchPoi;
				}
			}
			return this.deliverTo;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0003FA1C File Offset: 0x0003DC1C
		public override bool LoadoutCanRetrieveItem(MapPointOfInterest poi)
		{
			if (this.displayedAmount < this.requiredAmount)
			{
				OreItemData oreItemData;
				if (this.itemType.itemCategory == ItemCategory.Ore && this.itemType.TryGetComponent<OreItemData>(out oreItemData))
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
				if (this.itemType.itemCategory == ItemCategory.Salvage)
				{
					GameplayManager instance2 = GameplayManager.Instance;
					UnityEngine.Object x;
					if (instance2 == null)
					{
						x = null;
					}
					else
					{
						SpaceShip spaceShip2 = instance2.spaceShip;
						x = ((spaceShip2 != null) ? spaceShip2.GetModule<SalvageModule>() : null);
					}
					return x != null;
				}
			}
			return this.displayedAmount >= this.requiredAmount;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0003FAD0 File Offset: 0x0003DCD0
		public override int ItemCountRequired(InventoryItemType item)
		{
			if (this.itemType == item)
			{
				return this.requiredAmount;
			}
			return 0;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0003FAE8 File Offset: 0x0003DCE8
		protected override void DataToJson(JsonObject data)
		{
			data["itemType"] = this.itemType.identifier;
			data["requiredAmount"] = new double?((double)this.requiredAmount);
			string key = "deliverTo";
			SpaceStation spaceStation = this.deliverTo;
			data[key] = ((spaceStation != null) ? spaceStation.guid : null);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0003FB50 File Offset: 0x0003DD50
		protected override void LoadFromJson(JsonObject data)
		{
			this.itemType = data["itemType"].AsString;
			this.requiredAmount = data["requiredAmount"];
			if (data["deliverTo"].IsString)
			{
				GalaxyMapData.current.LoadPointOfInterest(data["deliverTo"], delegate(MapPointOfInterest poi)
				{
					this.deliverTo = (poi as SpaceStation);
				});
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0003FBCC File Offset: 0x0003DDCC
		public override Sprite GetIcon()
		{
			return this.itemType.icon;
		}

		// Token: 0x04000476 RID: 1142
		public InventoryItemType itemType;

		// Token: 0x04000477 RID: 1143
		public int requiredAmount;

		// Token: 0x04000478 RID: 1144
		public SpaceStation deliverTo;

		// Token: 0x04000479 RID: 1145
		private MapPointOfInterest fetchPoi;

		// Token: 0x0400047A RID: 1146
		private bool fetchPoiFetched;
	}
}
