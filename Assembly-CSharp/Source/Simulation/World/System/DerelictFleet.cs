using System;
using System.Collections.Generic;
using Behaviour.Item.Builder;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Util;

namespace Source.Simulation.World.System
{
	// Token: 0x02000079 RID: 121
	public class DerelictFleet : SystemStoryteller
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x000248C5 File Offset: 0x00022AC5
		public DerelictFleet(SystemMapData system) : base(system)
		{
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000248D0 File Offset: 0x00022AD0
		public override void SetupSystem()
		{
			SeededRandom random = SeededRandom.Global;
			this.system.pocketSystem = true;
			this.system.name = SandboxWorld.EnsureUniqueName(() => Translation.Translate("@PocketSystemDerelictWreck", new object[]
			{
				random.Choose<string>(DerelictFleet.systemNames)
			}));
			this.system.faction = Faction.salvageGuild;
			this.system.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.Refinery,
				SpaceStationFacility.SalvageShop,
				SpaceStationFacility.MissionBoard,
				SpaceStationFacility.Bar,
				SpaceStationFacility.Shipyard,
				SpaceStationFacility.SalvageWorkshop
			}, null, null, null, null).salvageShopInventory.AddPermanentItem("BonusSkillPointTemplate", 1);
			for (int i = 0; i < 3; i++)
			{
				this.system.AddDerelictFleetPoi(this.system.faction, null, "AncientWreck", 0.5f);
			}
			JumpGate entranceJumpgate = this.system.GetEntranceJumpgate();
			foreach (SystemMapData systemMapData in this.system.sector.allSystems)
			{
				if (systemMapData.jumpgateOpen)
				{
					foreach (MapPointOfInterest mapPointOfInterest in systemMapData.allPointsOfInterest)
					{
						if (mapPointOfInterest.faction == Faction.salvageGuild)
						{
							SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
							if (spaceStation != null)
							{
								spaceStation.salvageShopInventory.AddPermanentItem(ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(entranceJumpgate, null), 1);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400026C RID: 620
		private static string[] systemNames = new string[]
		{
			"Ancient",
			"Venerable",
			"Desecrated",
			"Looted",
			"Bone-picked",
			"Abandoned",
			"Treacherous",
			"Eternal",
			"Isolated",
			"Forlorn",
			"Forsaken",
			"Ravaged",
			"Haunted",
			"Blighted",
			"Ruined",
			"Wretched",
			"Pillaged",
			"Corroded"
		};
	}
}
