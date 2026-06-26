using System;
using System.Collections.Generic;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Mining;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Galaxy.Statics;
using Source.Mining;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.World
{
	// Token: 0x02000076 RID: 118
	public class TutorialWorld
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x000234C8 File Offset: 0x000216C8
		public static void SetupWorld(GamePlayer ply)
		{
			SectorMapData sectorMapData = new SectorMapData(Vector2.zero);
			sectorMapData.name = "Galaxy's Edge";
			ply.map.AddSector(sectorMapData);
			List<SystemMapData> list = new List<SystemMapData>();
			SystemMapData systemMapData = TutorialWorld.CreateSystem(sectorMapData, "Ravon", "B-Type", 1, 0, 2, Faction.stranded);
			Planet planet = systemMapData.AddPlanet("R-1", PlanetType.GasGiant, 20f, new Vector2?(new Vector2(9.78f, 1.34f)), true);
			systemMapData.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet.position)), null, 0f, false, 0.5f);
			Planet planet2 = systemMapData.AddPlanet("R-2", PlanetType.EarthLike, 10f, new Vector2?(new Vector2(7.99f, -1.58f)), true);
			systemMapData.AddPlanet("R-3", PlanetType.Desert, 15f, null, true);
			systemMapData.AddSpaceStation(new HashSet<SpaceStationFacility>(), new Vector2?(TutorialWorld.GetOrbitPosition(planet2.position)), Faction.stranded, "Driftlight Station", null).WithCharacters(new List<string>
			{
				Tutorial.greg.name
			}).WithSize(SpaceStation.StationSize.Small);
			list.Add(systemMapData);
			SystemMapData systemMapData2 = TutorialWorld.CreateSystem(sectorMapData, "Orbitan", "B-Type", 2, 1, 2, Faction.stranded);
			Planet planet3 = systemMapData2.AddPlanet("O-1", PlanetType.GasGiant, 20f, new Vector2?(new Vector2(-5.92f, 0.5f)), true);
			systemMapData2.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet3.position)), null, 0f, false, 0.5f);
			Planet planet4 = systemMapData2.AddPlanet("0-2", PlanetType.EarthLike, 10f, new Vector2?(new Vector2(-3.63f, -2.63f)), true);
			SpaceStation spaceStation = systemMapData2.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MissionBoard,
				SpaceStationFacility.Shipyard
			}, new Vector2?(TutorialWorld.GetOrbitPosition(planet4.position)), null, "Point Station", null).WithCharacters(new List<string>
			{
				Tutorial.virgil.name
			});
			list.Add(systemMapData2);
			SystemMapData systemMapData3 = TutorialWorld.CreateSystem(sectorMapData, "Octantis", "G-Type", 4, 2, 1, Faction.stranded);
			Planet planet5 = systemMapData3.AddPlanet("O-1", PlanetType.GasGiant, 20f, null, true);
			systemMapData3.AddPlanet("O-2", PlanetType.GasGiant, 30f, null, true);
			systemMapData3.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet5.position)), null, 0f, false, 0.5f);
			list.Add(systemMapData3);
			SystemMapData systemMapData4 = TutorialWorld.CreateSystem(sectorMapData, "Balam", "M-Type", 3, 2, 3, Faction.stranded);
			Planet planet6 = systemMapData4.AddPlanet("B-1", PlanetType.GasGiant, 20f, null, true);
			systemMapData4.AddPlanet("B-1A", PlanetType.Rock, 6f, null, true);
			systemMapData4.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet6.position)), null, 0f, false, 0.5f);
			Planet planet7 = systemMapData4.AddPlanet("B-2", PlanetType.Desert, 14f, new Vector2?(new Vector2(-0.54f, 2.6f)), true);
			systemMapData4.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MiningShop,
				SpaceStationFacility.Refinery,
				SpaceStationFacility.MissionBoard,
				SpaceStationFacility.Shipyard
			}, new Vector2?(TutorialWorld.GetOrbitPosition(planet7.position)), null, "Genesis Station", null).WithCharacters(new List<string>
			{
				Tutorial.creed.name
			});
			list.Add(systemMapData4);
			SystemMapData systemMapData5 = TutorialWorld.CreateSystem(sectorMapData, "Sparax", "K-Type", 5, 3, 1, Faction.stranded);
			Planet planet8 = systemMapData5.AddPlanet("S-1", PlanetType.GasGiant, 20f, null, true);
			AsteroidFieldOreSet surface = new AsteroidFieldOreSet("OreRare10", "OreCommon13", new List<OreItemData>
			{
				"OreCommon20",
				"OreRare16"
			});
			AsteroidFieldData customFieldData = new AsteroidFieldData(systemMapData5.systemOreData.amount, systemMapData5.systemOreData.density, systemMapData5.systemOreData.wealth, surface, systemMapData5.systemOreData.coreOres, -1f);
			systemMapData5.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet8.position)), customFieldData, 0f, false, 0.5f);
			Planet planet9 = systemMapData5.AddPlanet("S-2", PlanetType.EarthLike, 15f, null, true);
			systemMapData5.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MiningShop,
				SpaceStationFacility.Refinery,
				SpaceStationFacility.Shipyard,
				SpaceStationFacility.MissionBoard
			}, new Vector2?(TutorialWorld.GetOrbitPosition(planet9.position)), null, null, null);
			list.Add(systemMapData5);
			SystemMapData systemMapData6 = TutorialWorld.CreateSystem(sectorMapData, "Ascella", "K-Type", 4, 3, 3, Faction.stranded);
			Planet planet10 = systemMapData6.AddPlanet("A-2", PlanetType.GasGiant, 20f, null, true);
			systemMapData6.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet10.position)), null, 0f, false, 0.5f);
			Planet planet11 = systemMapData6.AddPlanet("A-3", PlanetType.Rock, 8f, null, true);
			systemMapData6.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MiningShop,
				SpaceStationFacility.Refinery,
				SpaceStationFacility.MissionBoard
			}, new Vector2?(TutorialWorld.GetOrbitPosition(planet11.position)), null, null, null);
			list.Add(systemMapData6);
			SystemMapData systemMapData7 = TutorialWorld.CreateSystem(sectorMapData, "Hermetis", "F-Type", 4, 3, 4, Faction.stranded);
			Planet planet12 = systemMapData7.AddPlanet("E2", PlanetType.GasGiant, 20f, null, true);
			systemMapData7.AddTurorialJumpgatePOI(new Vector2?(new Vector2(18f, 0.5f)));
			systemMapData7.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet12.position)), null, 0f, false, 0.5f);
			list.Add(systemMapData7);
			SystemMapData systemMapData8 = TutorialWorld.CreateSystem(sectorMapData, "Midarion", "O-Type", 7, 4, 0, Faction.stranded);
			Planet planet13 = systemMapData8.AddPlanet("M-1", PlanetType.GasGiant, 20f, null, true);
			systemMapData8.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet13.position)), null, 0f, false, 0.5f);
			list.Add(systemMapData8);
			SystemMapData systemMapData9 = TutorialWorld.CreateSystem(sectorMapData, "Terminus", "G-Type", 6, 4, 2, Faction.stranded);
			Planet planet14 = systemMapData9.AddPlanet("T-1", PlanetType.GasGiant, 20f, null, true);
			systemMapData9.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet14.position)), null, 0f, false, 0.5f);
			systemMapData9.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop
			}, null, null, null, null);
			list.Add(systemMapData9);
			SystemMapData systemMapData10 = TutorialWorld.CreateSystem(sectorMapData, "Thorone", "G-Type", 8, 5, 2, Faction.stranded);
			Planet planet15 = systemMapData10.AddPlanet("T-1", PlanetType.GasGiant, 30f, null, true);
			systemMapData10.AddMiningPoi(Faction.stranded, new Vector2?(TutorialWorld.GetOrbitPosition(planet15.position)), null, 0f, false, 0.5f);
			Planet planet16 = systemMapData10.AddPlanet("T-2", PlanetType.EarthLike, 20f, null, true);
			systemMapData10.AddSpaceStation(new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MiningShop,
				SpaceStationFacility.Refinery,
				SpaceStationFacility.Shipyard,
				SpaceStationFacility.MissionBoard
			}, new Vector2?(TutorialWorld.GetOrbitPosition(planet16.position)), null, null, null);
			list.Add(systemMapData10);
			SystemMapData item = TutorialWorld.CreateSystem(sectorMapData, "Canis Majoris", "G-Type", 5, 20, 4, Faction.stranded);
			list.Add(item);
			ply.currentSystem = systemMapData;
			TutorialWorld.CreateJumpGateLinks(new Dictionary<SystemMapData, List<SystemMapData>>
			{
				{
					systemMapData,
					new List<SystemMapData>
					{
						systemMapData2
					}
				},
				{
					systemMapData2,
					new List<SystemMapData>
					{
						systemMapData3,
						systemMapData4
					}
				},
				{
					systemMapData3,
					new List<SystemMapData>
					{
						systemMapData5
					}
				},
				{
					systemMapData4,
					new List<SystemMapData>
					{
						systemMapData6,
						systemMapData7
					}
				},
				{
					systemMapData5,
					new List<SystemMapData>
					{
						systemMapData8,
						systemMapData9
					}
				},
				{
					systemMapData8,
					new List<SystemMapData>
					{
						systemMapData9
					}
				},
				{
					systemMapData9,
					new List<SystemMapData>
					{
						systemMapData10
					}
				}
			});
			foreach (JumpGate jumpGate in systemMapData3.GetJumpGateList(false))
			{
				if (jumpGate.targetSystem == systemMapData5)
				{
					InventoryItemType item2 = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate, null);
					spaceStation.shopInventory.AddPermanentItem(item2, 1);
				}
			}
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00023E50 File Offset: 0x00022050
		public static void CreateJumpGateLinks(Dictionary<SystemMapData, List<SystemMapData>> systemLinks)
		{
			foreach (KeyValuePair<SystemMapData, List<SystemMapData>> keyValuePair in systemLinks)
			{
				SystemMapData key = keyValuePair.Key;
				foreach (SystemMapData systemMapData in keyValuePair.Value)
				{
					Vector2 xrestrictionForJumpgate = TutorialWorld.GetXRestrictionForJumpgate(systemMapData, key);
					Vector2 yrestrictionForJumpgate = TutorialWorld.GetYRestrictionForJumpgate(systemMapData, key);
					JumpGate jumpGate = new JumpGate
					{
						name = Translation.Translate("@GateTo", new object[]
						{
							systemMapData.name
						}),
						targetSystemGuid = systemMapData.guid
					};
					key.SetupPOI(jumpGate, new Vector2?(key.GetRandomPosition(xrestrictionForJumpgate.x, xrestrictionForJumpgate.y, yrestrictionForJumpgate.x, yrestrictionForJumpgate.y)), null, 0);
					key.pointsOfInterest.Add(jumpGate);
					xrestrictionForJumpgate = TutorialWorld.GetXRestrictionForJumpgate(key, systemMapData);
					yrestrictionForJumpgate = TutorialWorld.GetYRestrictionForJumpgate(key, systemMapData);
					JumpGate jumpGate2 = new JumpGate
					{
						name = Translation.Translate("@GateTo", new object[]
						{
							key.name
						}),
						targetSystemGuid = key.guid
					};
					systemMapData.SetupPOI(jumpGate2, new Vector2?(systemMapData.GetRandomPosition(xrestrictionForJumpgate.x, xrestrictionForJumpgate.y, yrestrictionForJumpgate.x, yrestrictionForJumpgate.y)), null, 0);
					systemMapData.pointsOfInterest.Add(jumpGate2);
					jumpGate.targetPoiGuid = jumpGate2.guid;
					jumpGate2.targetPoiGuid = jumpGate.guid;
				}
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00024038 File Offset: 0x00022238
		private static Vector2 GetYRestrictionForJumpgate(SystemMapData target, SystemMapData originSystem)
		{
			if (target.position.y < originSystem.position.y)
			{
				return new Vector2(-5f, -3f);
			}
			if (target.position.y > originSystem.position.y)
			{
				return new Vector2(3f, 5f);
			}
			return new Vector2(-3f, 3f);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x000240A4 File Offset: 0x000222A4
		private static Vector2 GetXRestrictionForJumpgate(SystemMapData target, SystemMapData originSystem)
		{
			if (target.position.x < originSystem.position.x)
			{
				return new Vector2(-20f, -12f);
			}
			if (target.position.x > originSystem.position.x)
			{
				return new Vector2(12f, 20f);
			}
			return new Vector2(-3f, 3f);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00024110 File Offset: 0x00022310
		private static Vector2 GetOrbitPosition(Vector2 planetPosition)
		{
			float num = 0.15f;
			return new Vector2(SeededRandom.Global.RandomRange(planetPosition.x - num, planetPosition.x + num), SeededRandom.Global.RandomRange(planetPosition.y - num, planetPosition.y + num));
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0002415C File Offset: 0x0002235C
		private static SystemMapData CreateSystem(SectorMapData sector, string name, string starName, int level, int x, int y, Faction faction)
		{
			SystemMapData systemMapData = new SystemMapData(sector, TutorialWorld.AssignPosition(x, y));
			systemMapData.faction = faction;
			systemMapData.name = name;
			systemMapData.storyId = TutorialWorld.systemNameToId[name];
			systemMapData.level = level;
			systemMapData.GenerateSystemOreData(false);
			Star item = new Star
			{
				position = new Vector2(0f, 0f),
				name = name + " (" + starName + ")",
				level = level,
				color = Star.GetRandomColorForType(starName),
				intensity = SeededRandom.Global.RandomRange(2f, 7f)
			};
			systemMapData.statics.Add(item);
			sector.AddSystem(systemMapData);
			return systemMapData;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0002421C File Offset: 0x0002241C
		private static Vector2 AssignPosition(int x, int y)
		{
			float num = (float)x;
			num += UnityEngine.Random.Range(-0.1f, 0.1f);
			float num2 = -30f;
			float num3 = -6f;
			float x2 = num2 + 10f * num;
			num3 += (float)(3 * y);
			return new Vector2(x2, num3);
		}

		// Token: 0x04000257 RID: 599
		public const string RavonId = "tutorial_system_1";

		// Token: 0x04000258 RID: 600
		public const string OrbitanId = "tutorial_system_2";

		// Token: 0x04000259 RID: 601
		public const string OctantisId = "tutorial_system_3";

		// Token: 0x0400025A RID: 602
		public const string BalamId = "tutorial_system_4";

		// Token: 0x0400025B RID: 603
		public const string SparaxId = "tutorial_system_5";

		// Token: 0x0400025C RID: 604
		public const string AscellaId = "tutorial_system_6";

		// Token: 0x0400025D RID: 605
		public const string HermetisId = "tutorial_system_7";

		// Token: 0x0400025E RID: 606
		public const string MidarionId = "tutorial_system_8";

		// Token: 0x0400025F RID: 607
		public const string TerminusId = "tutorial_system_9";

		// Token: 0x04000260 RID: 608
		public const string ThoroneId = "tutorial_system_10";

		// Token: 0x04000261 RID: 609
		public const string CanisMajorisId = "tutorial_system_11";

		// Token: 0x04000262 RID: 610
		public static Dictionary<string, string> systemNameToId = new Dictionary<string, string>
		{
			{
				"Ravon",
				"tutorial_system_1"
			},
			{
				"Orbitan",
				"tutorial_system_2"
			},
			{
				"Octantis",
				"tutorial_system_3"
			},
			{
				"Balam",
				"tutorial_system_4"
			},
			{
				"Sparax",
				"tutorial_system_5"
			},
			{
				"Ascella",
				"tutorial_system_6"
			},
			{
				"Hermetis",
				"tutorial_system_7"
			},
			{
				"Midarion",
				"tutorial_system_8"
			},
			{
				"Terminus",
				"tutorial_system_9"
			},
			{
				"Thorone",
				"tutorial_system_10"
			},
			{
				"Canis Majoris",
				"tutorial_system_11"
			},
			{
				"Ursa Majoris",
				"tutorial_system_11"
			}
		};
	}
}
