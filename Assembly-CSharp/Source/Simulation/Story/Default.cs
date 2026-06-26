using System;
using System.Collections.Generic;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.SpaceShip;

namespace Source.Simulation.Story
{
	// Token: 0x02000088 RID: 136
	public class Default : Storyteller
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x00029C8F File Offset: 0x00027E8F
		public Default(GamePlayer ply) : base(ply)
		{
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00029C98 File Offset: 0x00027E98
		public override void SetupNewGame()
		{
			this.SetupFactions();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00029CA0 File Offset: 0x00027EA0
		public void SetupFactions()
		{
			this.SetupEnemyFaction(Faction.marauders);
			this.SetupEnemyFaction(Faction.fanatics);
			this.SetupEnemyFaction(Faction.amalgam);
			this.SetupEnemyFaction(Faction.holyRadicals);
			this.SetupCriminalFaction(Faction.smugglers);
			this.SetupCriminalFaction(Faction.darkspacers);
			this.SetupCriminalFaction(Faction.puppeteers);
			Faction.gold.ChangeFactionReputation(Faction.blue, -1000);
			Faction.gold.ChangeFactionReputation(Faction.red, -1000);
			Faction.blue.ChangeFactionReputation(Faction.red, -1000);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00029D38 File Offset: 0x00027F38
		private void SetupEnemyFaction(Faction f)
		{
			foreach (Faction faction in Faction.all)
			{
				if (f != faction)
				{
					faction.ChangeFactionReputation(f, -6000);
				}
			}
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00029D90 File Offset: 0x00027F90
		private void SetupCriminalFaction(Faction f)
		{
			Faction.gold.ChangeFactionReputation(f, -1000);
			Faction.red.ChangeFactionReputation(f, -1000);
			Faction.blue.ChangeFactionReputation(f, -1000);
			Faction.policeGuild.ChangeFactionReputation(f, -1000);
			Faction.bountyGuild.ChangeFactionReputation(f, -1000);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00029DED File Offset: 0x00027FED
		public override void Start()
		{
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00029DEF File Offset: 0x00027FEF
		public override void StoryUpdate(float delta)
		{
			GamePlayer.current.currentSystem.ActiveUpdate(delta);
			MapPointOfInterest currentPointOfInterest = GamePlayer.current.currentPointOfInterest;
			if (currentPointOfInterest == null)
			{
				return;
			}
			currentPointOfInterest.ActiveUpdate(delta);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00029E18 File Offset: 0x00028018
		public override bool ItemIsPlayerAvailable(EquipmentBuilder item)
		{
			AbstractEquipment component = item.prefab.GetComponent<AbstractEquipment>();
			if (component.size == ModuleSize.Medium)
			{
				return this.player.currentSystem.level >= 8;
			}
			return component.size != ModuleSize.Large || (this.player.currentSystem.sector.quadrant > 1 && this.player.currentSystem.level >= 45);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00029E8D File Offset: 0x0002808D
		public override void Cleanup()
		{
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00029E90 File Offset: 0x00028090
		public void RemoveSkillPointsFromShops()
		{
			InventoryItemType y = InventoryItemType.Get("BonusSkillPointTemplate");
			List<Inventory.InventoryItem> list = new List<Inventory.InventoryItem>();
			foreach (MapPointOfInterest mapPointOfInterest in GalaxyMapData.current.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null)
				{
					ShopInventory shopInventory = spaceStation.shopInventory;
					if (shopInventory != null)
					{
						list.Clear();
						foreach (Inventory.InventoryItem inventoryItem in shopInventory.items)
						{
							if (inventoryItem.item == y)
							{
								list.Add(inventoryItem);
							}
						}
						if (shopInventory.permanentItems != null)
						{
							foreach (Inventory.InventoryItem inventoryItem2 in shopInventory.permanentItems)
							{
								if (inventoryItem2.item == y)
								{
									list.Add(inventoryItem2);
								}
							}
						}
						foreach (Inventory.InventoryItem item in list)
						{
							shopInventory.Remove(item, 1);
						}
					}
				}
			}
		}
	}
}
