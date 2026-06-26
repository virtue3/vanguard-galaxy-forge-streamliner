using System;
using System.Collections.Generic;
using Source.Galaxy;
using Source.Galaxy.NameGenerator;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.Player;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.World
{
	// Token: 0x02000072 RID: 114
	public static class ConquestWorld
	{
		// Token: 0x0600041E RID: 1054 RVA: 0x000200D4 File Offset: 0x0001E2D4
		public static void SetupWorld(GamePlayer ply)
		{
			ConquestWorld.random = SeededRandom.Global;
			ConquestWorld.map = ply.map;
			new List<MapElement>();
			List<SectorMapData> list = ConquestWorld.CreateStagingArea();
			ConquestWorld.conquestSector = ConquestWorld.CreateConquestArea();
			List<SectorMapData> list2 = ConquestWorld.CreateDarkspaceArea();
			SectorMapData item = null;
			foreach (SectorMapData sectorMapData in ConquestWorld.map.allSectors)
			{
				if (sectorMapData.quadrant == SectorMapData.quadrantFrontier)
				{
					item = sectorMapData;
				}
			}
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				item,
				list[1]
			}, 1, 1);
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				list[1],
				ConquestWorld.conquestSector
			}, 1, 1);
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				ConquestWorld.conquestSector,
				list2[0]
			}, 1, 1);
			List<SectorMapData> list3 = new List<SectorMapData>();
			foreach (SectorMapData sectorMapData2 in GamePlayer.current.map.allSectors)
			{
				if (sectorMapData2.quadrant == SectorMapData.quadrantConquest)
				{
					list3.Add(sectorMapData2);
				}
			}
			ConquestWorld.AddSidequests(list3);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00020238 File Offset: 0x0001E438
		private static List<SectorMapData> CreateStagingArea()
		{
			List<List<Vector2>> list = new List<List<Vector2>>();
			List<SectorMapData> list2 = new List<SectorMapData>
			{
				ConquestWorld.CreateSector(new Vector2(ConquestWorld.random.RandomRange(-18f, -8f), ConquestWorld.random.RandomRange(4f, 5f)), "Lux Arctos"),
				ConquestWorld.CreateSector(new Vector2(ConquestWorld.random.RandomRange(-18f, -8f), 0f), "Lux Magna"),
				ConquestWorld.CreateSector(new Vector2(ConquestWorld.random.RandomRange(-18f, -8f), ConquestWorld.random.RandomRange(-5f, -4f)), "Lux Australis")
			};
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(new List<Vector2>());
				int num = ConquestWorld.random.RandomRange(5, 8);
				for (int j = 0; j < num; j++)
				{
					list[i].Add(GalaxyMapData.GetRandomPosition(list[i], -38f, 38f, -6f, 6f, 9f));
				}
				list[i].Sort((Vector2 a, Vector2 b) => a.x.CompareTo(b.x));
			}
			Vector2 pos = list[1][0];
			list[1].RemoveAt(0);
			SandboxWorld.CreateEmptySystem(list2[1], 31, Faction.policeGuild, pos, false);
			ConquestWorld.sectorOwners = new List<Faction>
			{
				Faction.blue,
				Faction.red,
				Faction.gold
			};
			ConquestWorld.random.Shuffle<Faction>(ConquestWorld.sectorOwners);
			for (int k = 0; k < 3; k++)
			{
				SandboxWorld.CreateEmptySystem(list2[k], 33, ConquestWorld.sectorOwners[k], ConquestWorld.random.ChooseAndRemove<Vector2>(list[k]), false);
			}
			List<Faction> list3 = new List<Faction>
			{
				Faction.salvageGuild,
				Faction.miningGuild,
				Faction.tradingGuild,
				Faction.bountyGuild,
				Faction.industrialGuild,
				Faction.mercenaryGuild,
				Faction.stranded
			};
			ConquestWorld.random.Shuffle<Faction>(list3);
			int num2 = 1;
			int num3 = 31;
			while (list3.Count > 0)
			{
				num3++;
				num2 = (num2 + 1) % 3;
				SandboxWorld.CreateEmptySystem(list2[num2], num3, list3[0], ConquestWorld.random.ChooseAndRemove<Vector2>(list[num2]), false);
				list3.RemoveAt(0);
			}
			foreach (SectorMapData sectorMapData in list2)
			{
				foreach (SystemMapData system in sectorMapData.allSystems)
				{
					ConquestWorld.AddEmbassyStation(system);
				}
			}
			Vector2 vector = ConquestWorld.random.ChooseAndRemove<Vector2>(list[0]);
			for (int l = 0; l < 3; l++)
			{
				while (list[l].Count > 0)
				{
					SystemMapData systemMapData = SandboxWorld.CreateEmptySystem(list2[l], ConquestWorld.random.RandomRange(31, 41), ConquestWorld.sectorOwners[l], list[l][0], false);
					systemMapData.AddSpaceStation(ConquestWorld.GetStationFacilities(systemMapData.faction, systemMapData.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), null, null, null);
					list[l].RemoveAt(0);
				}
			}
			foreach (SectorMapData sectorMapData2 in list2)
			{
				SandboxWorld.CreateJumpgateLinks(new List<MapElement>(sectorMapData2.allSystems), 2, 3);
			}
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				list2[0],
				list2[1]
			}, 1, 1);
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				list2[1],
				list2[2]
			}, 1, 1);
			SystemMapData systemMapData2 = SandboxWorld.CreateEmptySystem(list2[0], 40, Faction.marauders, vector, false);
			ConquestWorld.AddEmbassyStation(systemMapData2);
			SystemMapData systemMapData3 = null;
			float num4 = -1f;
			foreach (SystemMapData systemMapData4 in list2[0].allSystems)
			{
				if (systemMapData4 != systemMapData2)
				{
					float num5 = Vector2.Distance(vector, systemMapData4.position);
					if (systemMapData3 == null || num5 < num4)
					{
						systemMapData3 = systemMapData4;
						num4 = num5;
					}
				}
			}
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				systemMapData2,
				systemMapData3
			}, 1, 1);
			List<Faction> list4 = new List<Faction>
			{
				Faction.miningGuild,
				Faction.tradingGuild,
				Faction.salvageGuild,
				Faction.bountyGuild,
				Faction.industrialGuild,
				Faction.policeGuild,
				Faction.mercenaryGuild
			};
			foreach (SectorMapData sectorMapData3 in list2)
			{
				List<SystemMapData> list5 = new List<SystemMapData>(sectorMapData3.allSystems);
				foreach (SystemMapData system2 in list5)
				{
					ConquestWorld.AddSideContent(system2);
				}
				foreach (Faction faction in list4)
				{
					if (list5.Count == 0)
					{
						break;
					}
					bool flag = true;
					SystemMapData systemMapData5 = ConquestWorld.random.ChooseAndRemove<SystemMapData>(list5);
					if (systemMapData5.faction != Faction.marauders)
					{
						foreach (MapPointOfInterest mapPointOfInterest in systemMapData5.pointsOfInterest)
						{
							if (mapPointOfInterest is SpaceStation && mapPointOfInterest.faction == faction)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							systemMapData5.AddSpaceStation(ConquestWorld.GetStationFacilities(faction, systemMapData5.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData5)), faction, null, null);
							ConquestWorld.AddGuildContent(faction, systemMapData5);
						}
					}
				}
			}
			ConquestWorld.AddUmbralOutpost(ConquestWorld.random.Choose<SystemMapData>(list2[1].allSystems));
			return list2;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000209A0 File Offset: 0x0001EBA0
		public static void AddUmbralOutpost(SystemMapData umbralSystem)
		{
			HashSet<SpaceStationFacility> spaceStationFacilities = new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MissionBoard,
				SpaceStationFacility.PersonalHangar
			};
			umbralSystem.AddSpaceStation(spaceStationFacilities, new Vector2?(SandboxWorld.GetFreeOrbit(umbralSystem)), Faction.puppeteers, "Unblinking Gaze", new EmbassyStation()).hidden = true;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x000209F4 File Offset: 0x0001EBF4
		private static void AddEmbassyStation(SystemMapData system)
		{
			HashSet<SpaceStationFacility> stationFacilities = ConquestWorld.GetStationFacilities(system.faction, system.level);
			if (system.faction != Faction.policeGuild)
			{
				stationFacilities.Add(SpaceStationFacility.Shipyard);
			}
			if (stationFacilities.Remove(SpaceStationFacility.GeneralShop) || stationFacilities.Remove(SpaceStationFacility.MiningShop) || stationFacilities.Remove(SpaceStationFacility.SalvageShop))
			{
				stationFacilities.Add(SpaceStationFacility.ConquestShop);
			}
			SpaceStation spaceStation = system.AddSpaceStation(stationFacilities, new Vector2?(SandboxWorld.GetFreeOrbit(system)), null, null, new EmbassyStation());
			if (spaceStation.faction == Faction.gold)
			{
				spaceStation.characters.Add("LuminateCommander");
			}
			else if (spaceStation.faction == Faction.red)
			{
				spaceStation.characters.Add("ExpeditionKolyatovCaptain");
			}
			else if (spaceStation.faction == Faction.blue)
			{
				spaceStation.characters.Add("StellarRepresentative");
			}
			else if (spaceStation.faction == Faction.policeGuild)
			{
				spaceStation.characters.Add("CanisecConquestCommander");
			}
			EmbassyJumpgate item = new EmbassyJumpgate
			{
				system = system,
				faction = system.faction,
				level = system.level,
				name = Translation.Translate("@GateToHQ", new object[]
				{
					system.faction.name
				}),
				position = system.GetRandomPosition(spaceStation.position.x + 3f, spaceStation.position.x + 4f, spaceStation.position.y - 2f, spaceStation.position.y + 2f),
				hidden = true
			};
			system.pointsOfInterest.Add(item);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00020B8F File Offset: 0x0001ED8F
		public static void AddSidequests(List<SectorMapData> sectors)
		{
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00020B94 File Offset: 0x0001ED94
		private static void AddSideContent(SystemMapData system)
		{
			if (ConquestWorld.random.RandomBool(0.5f))
			{
				system.AddMiningPoi(Faction.miningGuild, new Vector2?(SandboxWorld.GetFreeOrbit(system)), null, 0f, true, 0.5f);
			}
			if (ConquestWorld.random.RandomBool(0.5f))
			{
				system.AddAncientWreckPoi(Faction.salvageGuild, SandboxWorld.GetFreeOrbit(system));
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00020BF8 File Offset: 0x0001EDF8
		private static void AddGuildContent(Faction guild, SystemMapData guildSystem)
		{
			if (guild == Faction.miningGuild)
			{
				int num = 0;
				using (List<MapPointOfInterest>.Enumerator enumerator = guildSystem.pointsOfInterest.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Mining)
						{
							num++;
						}
					}
				}
				for (int i = num; i < 3; i++)
				{
					Mining mining = guildSystem.AddMiningPoi(guild, new Vector2?(SandboxWorld.GetFreeOrbit(guildSystem)), null, 0f, false, 0.5f);
					mining.hazardFieldData = HazardFieldData.CreateRandom(0.4f);
					mining.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining.hazardFieldData.GetHazardName();
				}
				return;
			}
			if (guild == Faction.salvageGuild)
			{
				int num2 = 0;
				using (List<MapPointOfInterest>.Enumerator enumerator = guildSystem.pointsOfInterest.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Salvage)
						{
							num2++;
						}
					}
				}
				for (int j = num2; j < 3; j++)
				{
					guildSystem.AddAncientWreckPoi(guild, SandboxWorld.GetFreeOrbit(guildSystem));
				}
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00020D34 File Offset: 0x0001EF34
		private static SectorMapData CreateConquestArea()
		{
			SectorMapData sectorMapData = ConquestWorld.CreateSector(new Vector2(0f, 0f), "Ara Martis");
			sectorMapData.conquestSector = true;
			List<Vector2> list = new List<Vector2>();
			int num = ConquestWorld.random.RandomRange(75, 89);
			for (int i = 0; i < num; i++)
			{
				list.Add(GalaxyMapData.GetRandomPosition(list, -38f, 38f, -18f, 18f, 4f));
			}
			List<MapElement> list2 = new List<MapElement>();
			foreach (Vector2 pos in list)
			{
				list2.Add(ConquestWorld.CreateConquestAreaSystem(sectorMapData, pos));
			}
			SandboxWorld.CreateJumpgateLinks(list2, 3, ConquestWorld.random.RandomBool(0.5f) ? 5 : 4);
			return sectorMapData;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00020E1C File Offset: 0x0001F01C
		private static SystemMapData CreateConquestAreaSystem(SectorMapData sector, Vector2 pos)
		{
			int level = ConquestWorld.random.RandomRange(32, 37);
			SystemMapData systemMapData = SandboxWorld.CreateEmptySystem(sector, ConquestWorld.random.RandomRange(30, 60), Faction.darkspacers, pos, false);
			systemMapData.name = SandboxWorld.EnsureUniqueName(new Func<string>(Source.Galaxy.NameGenerator.ConquestSystem.GenerateConquestSystemName));
			systemMapData.storyteller = new Source.Simulation.World.System.ConquestSystem(systemMapData);
			systemMapData.storyteller.SetupSystem();
			systemMapData.UpdateLevel(level);
			ConquestWorld.AddSideContent(systemMapData);
			systemMapData.AddSpaceStation(ConquestWorld.GetStationFacilities(Faction.darkspacers, level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), null, null, new ConquestStation());
			if (ConquestWorld.random.RandomBool(0.6f))
			{
				Faction faction = ConquestWorld.conquestGuilds[0];
				ConquestWorld.conquestGuilds.RemoveAt(0);
				ConquestWorld.conquestGuilds.Add(faction);
				systemMapData.AddSpaceStation(ConquestWorld.GetStationFacilities(faction, level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), faction, null, null);
				ConquestWorld.AddGuildContent(faction, systemMapData);
			}
			if (ConquestWorld.random.RandomBool(0.2f))
			{
				ConquestWorld.CreateCombatStation(ConquestWorld.random.RandomBool(0.5f) ? Faction.marauders : Faction.fanatics, systemMapData);
			}
			return systemMapData;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00020F40 File Offset: 0x0001F140
		private static void CreateCombatStation(Faction f, SystemMapData system)
		{
			Combat combat = system.AddCombatStation(f, null, true, 0);
			combat.name = ((f == Faction.marauders) ? "Corsair" : "Meridian") + " Hideout";
			combat.dangerLevel = "@MapPOIDangerSpaceStation";
			CombatStationFactory.CreateLargeStation(combat);
			combat.AddTriggeredSpawn(combat.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), 8f, 0, false, true);
			combat.AddTriggeredSpawn(combat.CreateUnitPayload(1.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), 80f, 0, false, true);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00020FF0 File Offset: 0x0001F1F0
		private static List<SectorMapData> CreateDarkspaceArea()
		{
			List<List<Vector2>> list = new List<List<Vector2>>();
			List<SectorMapData> list2 = new List<SectorMapData>
			{
				ConquestWorld.CreateSector(new Vector2(ConquestWorld.random.RandomRange(11f, 15f), ConquestWorld.random.RandomRange(-4f, -3f)), "Penumbra"),
				ConquestWorld.CreateSector(new Vector2(ConquestWorld.random.RandomRange(11f, 15f), ConquestWorld.random.RandomRange(3f, 4f)), "Lucifer")
			};
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(new List<Vector2>());
				int num = ConquestWorld.random.RandomRange(3, 6);
				for (int j = 0; j < num; j++)
				{
					list[i].Add(GalaxyMapData.GetRandomPosition(list[i], -38f, 38f, -6f, 6f, 9f));
				}
				list[i].Sort((Vector2 a, Vector2 b) => a.x.CompareTo(b.x));
			}
			SystemMapData system = SandboxWorld.CreateEmptySystem(list2[0], 60, Faction.darkspacers, list[0][0], false);
			list[0].RemoveAt(0);
			ConquestWorld.AddEmbassyStation(system);
			for (int k = 0; k < 2; k++)
			{
				while (list[k].Count > 0)
				{
					SystemMapData systemMapData = SandboxWorld.CreateEmptySystem(list2[k], 60, (k == 0) ? Faction.darkspacers : Faction.fanatics, list[k][0], false);
					systemMapData.AddSpaceStation(ConquestWorld.GetStationFacilities(systemMapData.faction, systemMapData.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), null, null, null);
					list[k].RemoveAt(0);
				}
			}
			foreach (SectorMapData sectorMapData in list2)
			{
				SandboxWorld.CreateJumpgateLinks(new List<MapElement>(sectorMapData.allSystems), 2, 3);
			}
			SandboxWorld.CreateJumpgateLinks(new List<MapElement>
			{
				list2[0],
				list2[1]
			}, 1, 1);
			return list2;
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0002124C File Offset: 0x0001F44C
		private static SectorMapData CreateSector(Vector2 pos, string name)
		{
			SectorMapData sectorMapData = new SectorMapData(pos);
			sectorMapData.name = name;
			sectorMapData.quadrant = SectorMapData.quadrantConquest;
			ConquestWorld.map.AddSector(sectorMapData);
			return sectorMapData;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00021280 File Offset: 0x0001F480
		public static HashSet<SpaceStationFacility> GetStationFacilities(Faction owner, int level)
		{
			HashSet<SpaceStationFacility> stationFacilities = SandboxWorld.GetStationFacilities(owner, level);
			if (owner == Faction.miningGuild || owner == Faction.salvageGuild)
			{
				stationFacilities.Add(SpaceStationFacility.Refinery);
			}
			return stationFacilities;
		}

		// Token: 0x04000243 RID: 579
		private static SeededRandom random;

		// Token: 0x04000244 RID: 580
		private static GalaxyMapData map;

		// Token: 0x04000245 RID: 581
		public static List<Faction> sectorOwners;

		// Token: 0x04000246 RID: 582
		public static SectorMapData conquestSector;

		// Token: 0x04000247 RID: 583
		private static List<Faction> conquestGuilds = new List<Faction>
		{
			Faction.miningGuild,
			Faction.industrialGuild,
			Faction.salvageGuild,
			Faction.tradingGuild,
			Faction.bountyGuild,
			Faction.mercenaryGuild,
			Faction.policeGuild
		};
	}
}
