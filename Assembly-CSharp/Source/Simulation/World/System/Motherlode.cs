using System;
using System.Collections.Generic;
using Behaviour.Item.Builder;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.Mining;
using Source.Util;

namespace Source.Simulation.World.System
{
	// Token: 0x0200007B RID: 123
	public class Motherlode : SystemStoryteller
	{
		// Token: 0x06000484 RID: 1156 RVA: 0x00025BB5 File Offset: 0x00023DB5
		public Motherlode(SystemMapData system) : base(system)
		{
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00025BC0 File Offset: 0x00023DC0
		public override void SetupSystem()
		{
			SeededRandom random = SeededRandom.Global;
			this.system.pocketSystem = true;
			this.system.name = SandboxWorld.EnsureUniqueName(() => Translation.Translate("@PocketSystemMotherlode", new object[]
			{
				random.Choose<string>(Motherlode.systemNames)
			}));
			this.system.faction = Faction.miningGuild;
			this.system.systemOreData = new AsteroidFieldData(random.RandomRange(9, 19), 1f, 1.3f, AsteroidFieldData.CreateOreSet(this.system.level, true), AsteroidFieldData.CreateOreSet(this.system.level, false), -1f);
			this.system.systemOreData.SwapCommonRare();
			this.system.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.Refinery,
				SpaceStationFacility.MiningShop,
				SpaceStationFacility.MissionBoard,
				SpaceStationFacility.Bar,
				SpaceStationFacility.Shipyard
			}, null, null, null, null).miningShopInventory.AddPermanentItem("BonusSkillPointTemplate", 1);
			for (int i = 0; i < 3; i++)
			{
				Mining mining = this.system.AddMiningPoi(Faction.miningGuild, null, null, 0f, false, 0.5f);
				mining.hazardFieldData = HazardFieldData.CreateRandom(0.4f);
				mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
			}
			JumpGate entranceJumpgate = this.system.GetEntranceJumpgate();
			foreach (SystemMapData systemMapData in this.system.sector.allSystems)
			{
				if (systemMapData.jumpgateOpen)
				{
					foreach (MapPointOfInterest mapPointOfInterest in systemMapData.allPointsOfInterest)
					{
						if (mapPointOfInterest.faction == Faction.miningGuild)
						{
							SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
							if (spaceStation != null)
							{
								spaceStation.miningShopInventory.AddPermanentItem(ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(entranceJumpgate, null), 1);
							}
						}
					}
				}
			}
		}

		// Token: 0x04000275 RID: 629
		private static string[] systemNames = new string[]
		{
			"Amber-9",
			"Sapphire-13",
			"Ruby-6",
			"Aurum-117",
			"Opal-121",
			"Amethyst-3A",
			"Triassic-11",
			"Jura-14",
			"Emerald-9",
			"Argentum-5",
			"Topaz-7",
			"Garnet-22",
			"Onyx-8",
			"Citrine-4B",
			"Cobalt-19",
			"Obsidian-7",
			"Ferrum-2",
			"Devonian-3"
		};
	}
}
