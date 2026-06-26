using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Source.Crew;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station;
using Source.Hazard;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.MissionSystem.Story;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World;
using Source.Simulation.World.System;
using Source.SpaceShip;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000032 RID: 50
	public static class GamePatch
	{
		// Token: 0x0600025E RID: 606 RVA: 0x000122B0 File Offset: 0x000104B0
		public static void OldDemoToEA(GamePlayer player)
		{
			Debug.Log("Oude save game naar Early Access converteren");
			foreach (SystemMapData systemMapData in player.map.allSystems)
			{
				if (systemMapData.faction == Faction.miningGuild)
				{
					systemMapData.faction = Faction.stranded;
				}
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					if (mapPointOfInterest.faction == Faction.miningGuild)
					{
						mapPointOfInterest.faction = Faction.stranded;
					}
				}
			}
			int reputation = Faction.player.GetReputation(Faction.miningGuild);
			Faction.player.SetReputation(Faction.miningGuild, 0);
			Faction.player.SetReputation(Faction.stranded, reputation);
			SpaceStation spaceStation = player.currentPointOfInterest as SpaceStation;
			if (spaceStation != null)
			{
				spaceStation.ClearUnits();
			}
			player.waypoints.Clear();
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000123C4 File Offset: 0x000105C4
		public static void CheckFactions(GamePlayer player)
		{
			if (player.factionData.GetReputation(Faction.player, Faction.fanatics) >= -1000)
			{
				player.GetStoryteller<Default>().SetupFactions();
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000123ED File Offset: 0x000105ED
		public static void CheckEconomy(GamePlayer player)
		{
			if (player.GetStoryteller<Economy>() == null && player.IsInSandBox())
			{
				player.AddStoryteller(new Economy(player), true);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0001240C File Offset: 0x0001060C
		public static void ResetBountyGuild(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				if (mapPointOfInterest.faction == Faction.bountyGuild)
				{
					SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
					if (spaceStation != null)
					{
						if (mapPointOfInterest.level < 10)
						{
							spaceStation.bountyBoard = null;
						}
						else
						{
							spaceStation.generalShopInventory = null;
							spaceStation.bountyShopInventory = new ShopInventory(spaceStation)
							{
								facility = SpaceStationFacility.BountyShop
							};
						}
					}
				}
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0001249C File Offset: 0x0001069C
		public static void ResetPoliceGuild(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				if (mapPointOfInterest.faction == Faction.policeGuild)
				{
					SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
					if (spaceStation != null && mapPointOfInterest.level >= 10)
					{
						spaceStation.generalShopInventory = null;
						spaceStation.patrolShopInventory = new ShopInventory(spaceStation)
						{
							facility = SpaceStationFacility.PatrolShop
						};
					}
				}
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00012524 File Offset: 0x00010724
		public static void ResetDemoPosition(GamePlayer player)
		{
			MapPointOfInterest currentPointOfInterest = player.currentPointOfInterest;
			string a;
			if (currentPointOfInterest == null)
			{
				a = null;
			}
			else
			{
				SystemMapData system = currentPointOfInterest.system;
				a = ((system != null) ? system.name : null);
			}
			if (a == "Canis Majoris")
			{
				JumpGate pointOfInterest = player.currentSystem.GetPointOfInterest<JumpGate>();
				player.currentSystem = pointOfInterest.targetSystem;
				player.currentPointOfInterest = pointOfInterest.GetTargetPOI();
				player.currentSpaceShip.positionData.position = player.currentPointOfInterest.GetWorldPosition() + new Vector2(-8f, 0f);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000125AE File Offset: 0x000107AE
		public static void AddSidequests(GamePlayer player)
		{
			if (player.GetStoryteller<Sandbox>() != null)
			{
				SandboxWorld.AddSidequests(new List<SectorMapData>(player.map.allSectors));
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000125CD File Offset: 0x000107CD
		public static void AddConquestSidequests(GamePlayer player)
		{
			if (player.GetStoryteller<Conquest>() != null)
			{
				ConquestWorld.AddSidequests(new List<SectorMapData>(player.map.allSectors));
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000125EC File Offset: 0x000107EC
		public static void FixTutorialGateDescription(GamePlayer player)
		{
			Mission mission = player.missions.FirstOrDefault((Mission m) => m.storyId == "tutorial_10");
			if (mission != null)
			{
				foreach (MissionStep missionStep in mission.steps)
				{
					foreach (MissionObjective missionObjective in missionStep.objectives)
					{
						TriggerObjective triggerObjective = missionObjective as TriggerObjective;
						if (triggerObjective != null && triggerObjective.description == "Deliver Structural Components to the gate")
						{
							triggerObjective.description = "Deliver Skeleton Frames to the gate";
						}
					}
				}
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x000126C8 File Offset: 0x000108C8
		public static void EnsureFactionMissionBoard(GamePlayer player)
		{
			if (player.GetStoryteller<Sandbox>() == null)
			{
				return;
			}
			foreach (SectorMapData sectorMapData in player.map.allSectors)
			{
				foreach (SystemMapData systemMapData in sectorMapData.allSystems)
				{
					foreach (SpaceStation spaceStation in systemMapData.GetPointsOfInterest<SpaceStation>())
					{
						if ((spaceStation.faction == Faction.red || spaceStation.faction == Faction.blue || spaceStation.faction == Faction.gold) && spaceStation.missionBoard == null)
						{
							spaceStation.missionBoard = new MissionBoard(spaceStation);
						}
					}
				}
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x000127C4 File Offset: 0x000109C4
		public static void AddMercenaryStationsPatch(GamePlayer player)
		{
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 6) && player.GetStoryteller<Sandbox>() != null)
			{
				SandboxWorld.AddMercenaryGuild(new List<SectorMapData>(player.map.allSectors));
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x000127F4 File Offset: 0x000109F4
		public static void EnsurePersistablesAreWithinBounds(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				Vector2 vector = Vector2.zero;
				foreach (PersistableData persistableData in new List<PersistableData>(mapPointOfInterest.GetPersistables()))
				{
					if (Vector2.Distance(persistableData.position, mapPointOfInterest.GetWorldPosition()) > 150f)
					{
						if (vector == Vector2.zero)
						{
							vector = mapPointOfInterest.GetWorldPosition() - persistableData.position;
						}
						persistableData.position += vector;
					}
				}
				vector = Vector2.zero;
				foreach (AbstractUnitData unitData in new List<AbstractUnitData>(mapPointOfInterest.GetUnits(false)))
				{
					vector = GamePatch.FixUnit(unitData, mapPointOfInterest, vector);
				}
				vector = Vector2.zero;
				foreach (MapTriggeredPayload mapTriggeredPayload in new List<MapTriggeredPayload>(mapPointOfInterest.GetTriggeredPayloads()))
				{
					if (!mapTriggeredPayload.spawnAtPlayer)
					{
						foreach (AbstractUnitData unitData2 in mapTriggeredPayload.units)
						{
							vector = GamePatch.FixUnit(unitData2, mapPointOfInterest, vector);
						}
					}
				}
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000129F8 File Offset: 0x00010BF8
		private static Vector2 FixUnit(AbstractUnitData unitData, MapPointOfInterest poi, Vector2 diff)
		{
			if (Vector2.Distance(unitData.positionData.position, poi.GetWorldPosition()) > 150f)
			{
				if (diff == Vector2.zero)
				{
					diff = poi.GetWorldPosition() - unitData.positionData.position;
				}
				unitData.positionData.position += diff;
			}
			return diff;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00012A60 File Offset: 0x00010C60
		public static void GenerateEnemyMiners()
		{
			foreach (MapPointOfInterest mapPointOfInterest in GalaxyMapData.current.allPointsOfInterest)
			{
				Source.Galaxy.POI.Mining mining = mapPointOfInterest as Source.Galaxy.POI.Mining;
				if (mining != null && !MiningClaimItem.IsMiningClaim(mining) && SeededRandom.Global.RandomBool(0.3f) && !(mapPointOfInterest.name == "Potential Jumpgate Site"))
				{
					List<AbstractUnitData> list = mapPointOfInterest.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), Faction.marauders, 0, 0, 1, 5, null);
					for (int i = 0; i < list.Count; i++)
					{
						list[i].autoActions = "AmbientMiner";
						list[i].positionData.position = mapPointOfInterest.GetWorldPosition() + new Vector2(SeededRandom.Global.RandomRange(12f, 80f), SeededRandom.Global.RandomRange(-9f, 9f));
					}
					mapPointOfInterest.AddTriggeredSpawn(list, 5f, 0, false, false);
				}
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00012BA4 File Offset: 0x00010DA4
		public static void AddStructuralSalvage()
		{
			foreach (MapPointOfInterest mapPointOfInterest in GalaxyMapData.current.allPointsOfInterest)
			{
				foreach (PersistableData persistableData in mapPointOfInterest.GetPersistables())
				{
					SalvageData salvageData = persistableData as SalvageData;
					if (salvageData != null)
					{
						salvageData.AddStructuralContent(mapPointOfInterest.level, 2, 1f);
					}
				}
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00012C40 File Offset: 0x00010E40
		public static void FixAsteroidFieldsAmount()
		{
			foreach (SystemMapData systemMapData in GalaxyMapData.current.allSystems)
			{
				int num = 0;
				foreach (MapPointOfInterest mapPointOfInterest in (from p in systemMapData.pointsOfInterest
				orderby p.lastVisitedTime descending
				select p).ToList<MapPointOfInterest>())
				{
					Source.Galaxy.POI.Mining mining = mapPointOfInterest as Source.Galaxy.POI.Mining;
					if (mining != null && !MiningClaimItem.IsMiningClaim(mining))
					{
						num++;
						if (num > 3 && mapPointOfInterest != MapPointOfInterest.currentOrNext)
						{
							systemMapData.pointsOfInterest.Remove(mapPointOfInterest);
						}
					}
				}
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00012D2C File Offset: 0x00010F2C
		public static void AddIndustrialGuild(GamePlayer player)
		{
			if (player.GetStoryteller<Sandbox>() != null)
			{
				foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
				{
					SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
					if (spaceStation != null && spaceStation.faction == Faction.industrialGuild)
					{
						return;
					}
				}
				SandboxWorld.AddIndustrialGuild(new List<SectorMapData>(player.map.allSectors));
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00012DAC File Offset: 0x00010FAC
		public static void RefreshInfiniteShops(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null)
				{
					if (spaceStation.bountyShopInventory != null && spaceStation.bountyShopInventory.count > 0)
					{
						spaceStation.GenerateBountyShopInventory();
					}
					else if (spaceStation.patrolShopInventory != null && spaceStation.patrolShopInventory.count > 0)
					{
						spaceStation.GeneratePatrolShopInventory();
					}
				}
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00012E3C File Offset: 0x0001103C
		public static void AddSalvageWorkshopsToWorld(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				if (mapPointOfInterest.level >= 15)
				{
					SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
					if (spaceStation != null && mapPointOfInterest.faction == Faction.salvageGuild && spaceStation.salvageWorkshop == null)
					{
						spaceStation.salvageWorkshop = new SalvageWorkshop(spaceStation);
					}
				}
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00012EBC File Offset: 0x000110BC
		public static void FixClearConstructionSiteMission(GamePlayer player)
		{
			Mission mission = null;
			foreach (Mission mission2 in player.missions)
			{
				if (mission2.storyId == "UmbralMission23")
				{
					mission = mission2;
					break;
				}
			}
			if (mission == null)
			{
				return;
			}
			if (mission.currentStep.pointsOfInterest.Count<MapPointOfInterest>() == 0)
			{
				MapPointOfInterest mapPointOfInterest = UmbralMissions.CreateConstructionSitePOI();
				foreach (MissionStep missionStep in mission.steps)
				{
					missionStep.system = mapPointOfInterest.system;
					missionStep.dynamicPointOfInterest = mapPointOfInterest;
					foreach (MissionObjective missionObjective in missionStep.objectives)
					{
						Source.MissionSystem.Objectives.Salvage salvage = missionObjective as Source.MissionSystem.Objectives.Salvage;
						if (salvage != null)
						{
							salvage.targetPOI = mapPointOfInterest.guid;
						}
						Source.MissionSystem.Objectives.Mining mining = missionObjective as Source.MissionSystem.Objectives.Mining;
						if (mining != null)
						{
							mining.targetPOI = mapPointOfInterest.guid;
						}
					}
				}
				return;
			}
			foreach (MapPointOfInterest mapPointOfInterest2 in mission.currentStep.pointsOfInterest)
			{
				foreach (AbstractUnitData abstractUnitData in mapPointOfInterest2.GetUnits(false).ToList<AbstractUnitData>())
				{
					GameplayType? gameplayType = abstractUnitData.gameplayType;
					GameplayType gameplayType2 = GameplayType.Mining;
					if (gameplayType.GetValueOrDefault() == gameplayType2 & gameplayType != null)
					{
						mapPointOfInterest2.RemoveUnit(abstractUnitData);
					}
				}
				foreach (MapTriggeredPayload mapTriggeredPayload in mapPointOfInterest2.GetTriggeredPayloads().ToList<MapTriggeredPayload>())
				{
					if (mapTriggeredPayload.units.Count > 0)
					{
						foreach (AbstractUnitData abstractUnitData2 in mapTriggeredPayload.units)
						{
							GameplayType? gameplayType = abstractUnitData2.gameplayType;
							GameplayType gameplayType2 = GameplayType.Mining;
							if (gameplayType.GetValueOrDefault() == gameplayType2 & gameplayType != null)
							{
								mapPointOfInterest2.RemovePayload(mapTriggeredPayload);
								break;
							}
						}
					}
				}
				bool flag = false;
				using (List<MissionObjective>.Enumerator enumerator3 = mission.currentStep.objectives.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current is Source.MissionSystem.Objectives.Mining)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					break;
				}
				foreach (PersistableData persistableData in mapPointOfInterest2.GetPersistables().ToList<PersistableData>())
				{
					AsteroidData asteroidData = persistableData as AsteroidData;
					if (asteroidData != null)
					{
						mapPointOfInterest2.RemovePersistable(asteroidData);
					}
				}
				mapPointOfInterest2.SetAsteroidFieldData(mapPointOfInterest2.system.systemOreData, 0);
				mapPointOfInterest2.asteroidsInitialized = false;
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0001329C File Offset: 0x0001149C
		public static void SetCriticalCrewmembers(GamePlayer player)
		{
			foreach (CrewMemberData crewMemberData in player.crewMembers)
			{
				string fullName = crewMemberData.GetFullName();
				if (fullName == "Elena Scott" || fullName == "John Raythor")
				{
					crewMemberData.SetCritical(true);
				}
			}
			foreach (Mission mission in player.missions)
			{
				foreach (MissionReward missionReward in mission.rewards)
				{
					Crew crew = missionReward as Crew;
					if (crew != null)
					{
						string fullName2 = crew.crew.GetFullName();
						if (fullName2 == "Elena Scott" || fullName2 == "John Raythor")
						{
							crew.crew.SetCritical(true);
						}
					}
				}
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000133C8 File Offset: 0x000115C8
		public static void AddSectorQuadrant(GamePlayer ply)
		{
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				sectorMapData.quadrant = SectorMapData.quadrantFrontier;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0001341C File Offset: 0x0001161C
		public static void CleanupJumpGates(GamePlayer ply)
		{
			foreach (MapPointOfInterest mapPointOfInterest in ply.map.allPointsOfInterest)
			{
				JumpGate jumpGate = mapPointOfInterest as JumpGate;
				if (jumpGate != null && jumpGate.lastVisitedTime == 0f)
				{
					jumpGate.CleanupWindowDressing();
				}
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00013484 File Offset: 0x00011684
		public static void ClearSpacestationPayloads(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && !(mapPointOfInterest is IndustryStation))
				{
					spaceStation.ClearPayloads();
				}
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x000134E8 File Offset: 0x000116E8
		public static void CleanupSpaceShipPng(GamePlayer player)
		{
			InventoryItemType item = "SpaceShipPng";
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && spaceStation.materialStorage.count > 0)
				{
					int count = spaceStation.materialStorage.GetCount(item);
					spaceStation.materialStorage.Remove(item, count);
					player.globalInventory.Add(item, count, false, false);
				}
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00013580 File Offset: 0x00011780
		public static void TweakClearingConstructionSiteMission(GamePlayer player)
		{
			foreach (Mission mission in player.missions)
			{
				if (mission.storyId == "UmbralMission23")
				{
					foreach (MissionObjective missionObjective in mission.currentStep.objectives)
					{
						Source.MissionSystem.Objectives.Mining mining = missionObjective as Source.MissionSystem.Objectives.Mining;
						if (mining != null)
						{
							mining.requiredAmount = 40;
						}
					}
				}
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00013630 File Offset: 0x00011830
		public static void AddAsteroidsClearingConstructionSiteMission(GamePlayer player)
		{
			foreach (Mission mission in player.missions)
			{
				if (mission.storyId == "UmbralMission23")
				{
					foreach (MapPointOfInterest mapPointOfInterest in mission.currentStep.pointsOfInterest)
					{
						bool flag = false;
						using (List<MissionObjective>.Enumerator enumerator3 = mission.currentStep.objectives.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current is Source.MissionSystem.Objectives.Mining)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return;
						}
						foreach (PersistableData persistableData in mapPointOfInterest.GetPersistables().ToList<PersistableData>())
						{
							AsteroidData asteroidData = persistableData as AsteroidData;
							if (asteroidData != null)
							{
								mapPointOfInterest.RemovePersistable(asteroidData);
							}
						}
						mapPointOfInterest.SetAsteroidFieldData(mapPointOfInterest.system.systemOreData, 10);
						mapPointOfInterest.asteroidsInitialized = false;
					}
				}
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000137CC File Offset: 0x000119CC
		public static void ClearMercenaries(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && mapPointOfInterest.faction == Faction.mercenaryGuild)
				{
					RecruitmentCenter recruitmentCenter = spaceStation.recruitmentCenter;
					if (recruitmentCenter != null)
					{
						recruitmentCenter.ClearMercs();
					}
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00013840 File Offset: 0x00011A40
		public static void AdjustStationSizes(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null)
				{
					spaceStation.stationSize = SystemMapData.GetStationSize(spaceStation.GetFacilities().Count<SpaceStationFacility>());
				}
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x000138AC File Offset: 0x00011AAC
		public static void SetConquestStationSeed(GamePlayer player)
		{
			foreach (SectorMapData sectorMapData in player.map.allSectors)
			{
				if (sectorMapData.conquestSector)
				{
					foreach (SystemMapData systemMapData in sectorMapData.allSystems)
					{
						foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
						{
							SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
							if (spaceStation != null)
							{
								spaceStation.SetStationSeed();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00013980 File Offset: 0x00011B80
		public static void AddConquestShop(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				EmbassyStation embassyStation = mapPointOfInterest as EmbassyStation;
				if (embassyStation != null && (embassyStation.generalShopInventory != null || embassyStation.miningShopInventory != null || embassyStation.salvageShopInventory != null))
				{
					embassyStation.SwapConquestShop(true);
				}
				else
				{
					ConquestStation conquestStation = mapPointOfInterest as ConquestStation;
					if (conquestStation != null)
					{
						ConquestSystem conquestSystem = conquestStation.system.storyteller as ConquestSystem;
						if (conquestSystem != null && conquestSystem.headquarters)
						{
							conquestSystem.SetHeadquarters(true);
						}
					}
				}
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00013A28 File Offset: 0x00011C28
		public static void AddConquestRefinery(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && mapPointOfInterest.system.sector.quadrant == 2 && (mapPointOfInterest.faction == Faction.miningGuild || mapPointOfInterest.faction == Faction.salvageGuild))
				{
					if (spaceStation.forge == null)
					{
						spaceStation.forge = new Forge(spaceStation);
					}
					if (spaceStation.refinery == null)
					{
						spaceStation.refinery = new Refinery(spaceStation);
					}
				}
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00013AD4 File Offset: 0x00011CD4
		public static void AddConquestIntroMission(GamePlayer player)
		{
			if (player.missionsArchive.Contains("StoryMissionSkipToConquest"))
			{
				string text = "ConquestThreeFactions";
				if (!player.missionsArchive.Contains(text))
				{
					Mission mission = Source.MissionSystem.StoryMission.Get(GamePlayer.current, text);
					GamePlayer.current.AddMissionWithLog(mission);
					return;
				}
			}
			else
			{
				foreach (Mission mission2 in GamePlayer.current.missions)
				{
					if (mission2.storyId == "StoryMissionSkipToConquest")
					{
						mission2.rewards.Add(new Source.MissionSystem.Rewards.StoryMission
						{
							missionId = "ConquestThreeFactions"
						});
					}
				}
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00013B90 File Offset: 0x00011D90
		public static void AddEmbassyRepresentatives(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				EmbassyStation embassyStation = mapPointOfInterest as EmbassyStation;
				if (embassyStation != null)
				{
					if (embassyStation.faction == Faction.gold)
					{
						string character = "LuminateCommander";
						embassyStation.TryAddCharacter(character);
					}
					else if (embassyStation.faction == Faction.red)
					{
						string character2 = "ExpeditionKolyatovCaptain";
						embassyStation.TryAddCharacter(character2);
					}
					else if (embassyStation.faction == Faction.blue)
					{
						string character3 = "StellarRepresentative";
						embassyStation.TryAddCharacter(character3);
					}
					else if (embassyStation.faction == Faction.policeGuild)
					{
						string character4 = "CanisecConquestCommander";
						embassyStation.TryAddCharacter(character4);
					}
				}
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00013C60 File Offset: 0x00011E60
		public static void AddConquestCommanders(GamePlayer player)
		{
			foreach (MapPointOfInterest mapPointOfInterest in player.map.allPointsOfInterest)
			{
				ConquestStation conquestStation = mapPointOfInterest as ConquestStation;
				if (conquestStation != null)
				{
					ConquestSystem conquestSystem = conquestStation.system.storyteller as ConquestSystem;
					if (conquestSystem != null && conquestSystem.headquarters)
					{
						conquestStation.SwapConquestCommander(true);
					}
				}
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00013CD8 File Offset: 0x00011ED8
		public static void SetTier2Unlocked(GamePlayer player)
		{
			if (player.GetStoryteller<Conquest>() != null)
			{
				player.SetSkilltreeTier2Unlocked(true);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00013CEC File Offset: 0x00011EEC
		public static void AddUmbralOutpost(GamePlayer player)
		{
			if (player.HasStoryteller<Conquest>())
			{
				SectorMapData sectorMapData = null;
				foreach (SectorMapData sectorMapData2 in player.map.allSectors)
				{
					if (sectorMapData2.quadrant == 2)
					{
						sectorMapData = sectorMapData2;
						if (sectorMapData2.name == "Lux Magna")
						{
							break;
						}
					}
				}
				if (sectorMapData != null)
				{
					ConquestWorld.AddUmbralOutpost(SeededRandom.Global.Choose<SystemMapData>(sectorMapData.allSystems));
				}
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00013D78 File Offset: 0x00011F78
		public static void TryAddUmbralMission(GamePlayer player)
		{
			if (player.HasStoryteller<Conquest>())
			{
				if (player.missionsArchive.Contains("ConquestStellar1") || player.missionsArchive.Contains("ConquestKolyatov1") || player.missionsArchive.Contains("ConquestLuminate1"))
				{
					string text = "ConquestUmbral1";
					if (!player.missionsArchive.Contains(text))
					{
						Mission mission = Source.MissionSystem.StoryMission.Get(GamePlayer.current, text);
						GamePlayer.current.AddMissionWithLog(mission);
						return;
					}
				}
				else
				{
					foreach (Mission mission2 in GamePlayer.current.missions)
					{
						if (mission2.storyId == "ConquestStellar1" || mission2.storyId == "ConquestKolyatov1" || mission2.storyId == "ConquestLuminate1")
						{
							mission2.rewards.Add(new Source.MissionSystem.Rewards.StoryMission
							{
								missionId = "ConquestUmbral1"
							});
						}
					}
				}
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00013E8C File Offset: 0x0001208C
		public static void TryAddDarkspaceMission(GamePlayer player)
		{
			if (player.HasStoryteller<Conquest>())
			{
				if (player.missionsArchive.Contains("ConquestStellar2") || player.missionsArchive.Contains("ConquestKolyatov2") || player.missionsArchive.Contains("ConquestLuminate2"))
				{
					string text = "ConquestDarkspace1";
					if (!player.missionsArchive.Contains(text))
					{
						Mission mission = Source.MissionSystem.StoryMission.Get(GamePlayer.current, text);
						GamePlayer.current.AddMissionWithLog(mission);
						return;
					}
				}
				else
				{
					foreach (Mission mission2 in GamePlayer.current.missions)
					{
						if (mission2.storyId == "ConquestStellar2" || mission2.storyId == "ConquestKolyatov2" || mission2.storyId == "ConquestLuminate2")
						{
							mission2.rewards.Add(new Source.MissionSystem.Rewards.StoryMission
							{
								missionId = "ConquestDarkspace1"
							});
						}
					}
				}
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00013FA0 File Offset: 0x000121A0
		public static void FixUmbralMission10Character(GamePlayer player)
		{
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (mission.storyId == "UmbralMission10")
				{
					SpaceStation spaceStation = mission.currentStep.GetMissionPoi() as SpaceStation;
					if (spaceStation != null)
					{
						spaceStation.TryAddCharacter("SmugglerContact");
						spaceStation.characters.Remove("SteelVulturePocket");
					}
				}
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00014034 File Offset: 0x00012234
		public static void GrantPersonalHistoryTitle(GamePlayer player)
		{
			CommanderData commander = player.commander;
			TitleDefinition titleDefinition;
			switch (commander.personalHistory)
			{
			case PersonalHistory.Miner:
				titleDefinition = Titles.Miner;
				break;
			case PersonalHistory.NavyCaptain:
				titleDefinition = Titles.NavyCaptain;
				break;
			case PersonalHistory.Salvaging:
				titleDefinition = Titles.Salvager;
				break;
			case PersonalHistory.Hauler:
				titleDefinition = Titles.Hauler;
				break;
			case PersonalHistory.BountyHunter:
				titleDefinition = Titles.BountyHunter;
				break;
			case PersonalHistory.Pirate:
				titleDefinition = Titles.Pirate;
				break;
			default:
				titleDefinition = null;
				break;
			}
			TitleDefinition titleDefinition2 = titleDefinition;
			if (titleDefinition2 == null)
			{
				return;
			}
			player.GrantTitle(titleDefinition2.identifier);
			if (string.IsNullOrEmpty(commander.selectedTitle))
			{
				commander.SetTitle(titleDefinition2.GetDisplayName(), titleDefinition2.color);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x000140D4 File Offset: 0x000122D4
		public static void FixDuplicateSystemNames(GamePlayer player)
		{
			Dictionary<string, List<SystemMapData>> dictionary = new Dictionary<string, List<SystemMapData>>();
			foreach (SystemMapData systemMapData in player.map.allSystems)
			{
				if (!string.IsNullOrEmpty(systemMapData.name))
				{
					List<SystemMapData> list;
					if (!dictionary.TryGetValue(systemMapData.name, out list))
					{
						list = new List<SystemMapData>();
						dictionary[systemMapData.name] = list;
					}
					list.Add(systemMapData);
				}
			}
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			HashSet<string> hashSet = new HashSet<string>(dictionary.Keys);
			foreach (KeyValuePair<string, List<SystemMapData>> keyValuePair in dictionary)
			{
				string text;
				List<SystemMapData> list2;
				keyValuePair.Deconstruct(out text, out list2);
				List<SystemMapData> list3 = list2;
				if (list3.Count > 1)
				{
					for (int i = 1; i < list3.Count; i++)
					{
						string name = list3[i].name;
						string text2 = GamePatch.PatchMakeSystemNameUnique(name, hashSet);
						dictionary2[list3[i].guid] = text2;
						list3[i].name = text2;
						hashSet.Add(text2);
						Debug.Log(string.Concat(new string[]
						{
							"[Patch 0.8.0.6] Renamed duplicate system '",
							name,
							"' -> '",
							text2,
							"', guid: ",
							list3[i].guid
						}));
					}
				}
			}
			if (dictionary2.Count == 0)
			{
				return;
			}
			foreach (SystemMapData systemMapData2 in player.map.allSystems)
			{
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData2.pointsOfInterest)
				{
					JumpGate jumpGate = mapPointOfInterest as JumpGate;
					if (jumpGate != null)
					{
						SystemMapData system = player.map.GetSystem(jumpGate.targetSystemGuid);
						if (system != null && dictionary2.ContainsKey(system.guid))
						{
							jumpGate.name = (jumpGate.sectorJumpgate ? Translation.Translate("@GateTo", new object[]
							{
								system.sector.name
							}) : Translation.Translate("@GateTo", new object[]
							{
								system.name
							}));
						}
					}
				}
			}
			List<Inventory> list4 = new List<Inventory>
			{
				player.globalInventory
			};
			foreach (SpaceShipData spaceShipData in player.spaceShips)
			{
				list4.Add(spaceShipData.cargo);
			}
			foreach (Inventory inventory in list4)
			{
				foreach (Inventory.InventoryItem inventoryItem in inventory.items)
				{
					GamePatch.PatchUpdateJumpgatePass(inventoryItem.item, player.map, dictionary2);
				}
			}
			foreach (MapPointOfInterest mapPointOfInterest2 in player.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest2 as SpaceStation;
				if (spaceStation != null)
				{
					ShopInventory shopInventory = spaceStation.shopInventory;
					if (shopInventory != null)
					{
						foreach (Inventory.InventoryItem inventoryItem2 in shopInventory.items)
						{
							GamePatch.PatchUpdateJumpgatePass(inventoryItem2.item, player.map, dictionary2);
						}
						if (shopInventory.permanentItems != null)
						{
							foreach (Inventory.InventoryItem inventoryItem3 in shopInventory.permanentItems)
							{
								GamePatch.PatchUpdateJumpgatePass(inventoryItem3.item, player.map, dictionary2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00014568 File Offset: 0x00012768
		private static void PatchUpdateJumpgatePass(InventoryItemType item, GalaxyMapData map, Dictionary<string, string> renamedSystems)
		{
			if (item == null)
			{
				return;
			}
			JumpgatePassItem jumpgatePassItem;
			if (!item.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem))
			{
				return;
			}
			SystemMapData system = map.GetSystem(jumpgatePassItem.systemGuid);
			if (system == null)
			{
				return;
			}
			JumpGate jumpGate = system.GetPoiWithId(jumpgatePassItem.jumpgateGuid) as JumpGate;
			if (jumpGate == null)
			{
				return;
			}
			SystemMapData system2 = map.GetSystem(jumpGate.targetSystemGuid);
			if (system2 == null)
			{
				return;
			}
			if (!renamedSystems.ContainsKey(system.guid) && !renamedSystems.ContainsKey(system2.guid))
			{
				return;
			}
			string text = jumpGate.sectorJumpgate ? "@ItemNameSectorPass" : "@ItemNameJumpgatePass";
			string text2 = jumpGate.sectorJumpgate ? system.sector.name : system.name;
			string text3 = jumpGate.sectorJumpgate ? system2.sector.name : system2.name;
			Debug.Log(string.Concat(new string[]
			{
				"[Patch 0.8.0.6] Renamed pass for systems '",
				item.displayName,
				"' -> '",
				Translation.Translate(text, new object[]
				{
					text2,
					text3
				}),
				"'"
			}));
			item.SetDisplayName(Translation.Translate(text, new object[]
			{
				text2,
				text3
			}));
			item.SetDescription(Translation.Translate("@JumpgatePassDescription", new object[]
			{
				system.name
			}));
		}

		// Token: 0x06000289 RID: 649 RVA: 0x000146B8 File Offset: 0x000128B8
		private static string PatchMakeSystemNameUnique(string baseName, HashSet<string> takenNames)
		{
			List<string> list = new List<string>
			{
				"Cygnus",
				"Lyra",
				"Auriga",
				"Vega",
				"Rigel",
				"Capella",
				"Antares",
				"Spica",
				"Deneb",
				"Altair",
				"Castor",
				"Procyon",
				"Achernar",
				"Canopus",
				"Arcturus"
			};
			SeededRandom.Global.Shuffle<string>(list);
			foreach (string str in list)
			{
				string text = baseName + " " + str;
				if (!takenNames.Contains(text))
				{
					return text;
				}
			}
			int num = 2;
			string text2;
			do
			{
				text2 = baseName + " " + num++.ToString();
			}
			while (takenNames.Contains(text2));
			return text2;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00014800 File Offset: 0x00012A00
		public static void AddConquestMiningGuildPOIs(GamePlayer player)
		{
			if (!player.HasStoryteller<Conquest>())
			{
				return;
			}
			foreach (SectorMapData sectorMapData in player.map.allSectors)
			{
				if (sectorMapData.quadrant == SectorMapData.quadrantConquest)
				{
					foreach (SystemMapData systemMapData in sectorMapData.allSystems)
					{
						bool flag = false;
						foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
						{
							if (mapPointOfInterest is SpaceStation && !(mapPointOfInterest is ConquestStation) && mapPointOfInterest.faction == Faction.miningGuild)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							int num = 0;
							foreach (MapPointOfInterest mapPointOfInterest2 in systemMapData.pointsOfInterest)
							{
								Source.Galaxy.POI.Mining mining = mapPointOfInterest2 as Source.Galaxy.POI.Mining;
								if (mining != null && !MiningClaimItem.IsMiningClaim(mining))
								{
									num++;
								}
							}
							for (int i = num; i < 3; i++)
							{
								Source.Galaxy.POI.Mining mining2 = systemMapData.AddMiningPoi(Faction.miningGuild, new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), null, 0f, false, 0.5f);
								mining2.hazardFieldData = HazardFieldData.CreateRandom(0.4f);
								mining2.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mining2.hazardFieldData.GetHazardName();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00014A0C File Offset: 0x00012C0C
		public static void AddConquestSalvageGuildPOIs(GamePlayer player)
		{
			if (!player.HasStoryteller<Conquest>())
			{
				return;
			}
			foreach (SectorMapData sectorMapData in player.map.allSectors)
			{
				if (sectorMapData.quadrant == SectorMapData.quadrantConquest)
				{
					foreach (SystemMapData systemMapData in sectorMapData.allSystems)
					{
						bool flag = false;
						foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
						{
							if (mapPointOfInterest is SpaceStation && !(mapPointOfInterest is ConquestStation) && mapPointOfInterest.faction == Faction.salvageGuild)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							int num = 0;
							foreach (MapPointOfInterest mapPointOfInterest2 in systemMapData.pointsOfInterest)
							{
								Source.Galaxy.POI.Salvage salvage = mapPointOfInterest2 as Source.Galaxy.POI.Salvage;
								if (salvage != null && !SalvageClaimItem.IsSalvageClaim(salvage))
								{
									num++;
								}
							}
							for (int i = num; i < 3; i++)
							{
								systemMapData.AddAncientWreckPoi(Faction.salvageGuild, SandboxWorld.GetFreeOrbit(systemMapData));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public static void RegenerateSystemOreData(GamePlayer player)
		{
			foreach (SystemMapData systemMapData in player.map.allSystems)
			{
				if (!systemMapData.pocketSystem)
				{
					systemMapData.systemOreData = null;
					systemMapData.GenerateSystemOreData(false);
				}
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00014C2C File Offset: 0x00012E2C
		public static void FixStackedEquipmentItems(GamePlayer player)
		{
			List<Inventory> list = new List<Inventory>
			{
				player.globalInventory
			};
			foreach (SpaceShipData spaceShipData in player.spaceShips)
			{
				list.Add(spaceShipData.cargo);
			}
			foreach (Inventory inventory in list)
			{
				foreach (Inventory.InventoryItem inventoryItem in inventory.items)
				{
					if (inventoryItem.count > 1 && inventoryItem.item.equipmentBuilder)
					{
						inventoryItem.SpecialAddCount(1 - inventoryItem.count);
					}
				}
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00014D34 File Offset: 0x00012F34
		public static void FixStoryMissionUnitFactions(GamePlayer player)
		{
			Dictionary<string, Dictionary<string, Faction>> dictionary = new Dictionary<string, Dictionary<string, Faction>>();
			string key = "UmbralMission5";
			Dictionary<string, Faction> dictionary2 = new Dictionary<string, Faction>();
			dictionary2["Tugbit"] = Faction.salvageGuild;
			dictionary[key] = dictionary2;
			string key2 = "UmbralMission15";
			Dictionary<string, Faction> dictionary3 = new Dictionary<string, Faction>();
			dictionary3["Syladon"] = Faction.darkspacers;
			dictionary[key2] = dictionary3;
			string key3 = "UmbralMission16";
			Dictionary<string, Faction> dictionary4 = new Dictionary<string, Faction>();
			dictionary4["PetaraDRK"] = Faction.darkspacers;
			dictionary[key3] = dictionary4;
			string key4 = "DarkspaceMission1";
			Dictionary<string, Faction> dictionary5 = new Dictionary<string, Faction>();
			dictionary5["Syladon"] = Faction.darkspacers;
			dictionary[key4] = dictionary5;
			string key5 = "DarkspaceMission3";
			Dictionary<string, Faction> dictionary6 = new Dictionary<string, Faction>();
			dictionary6["Syladon"] = Faction.darkspacers;
			dictionary[key5] = dictionary6;
			string key6 = "DarkspaceMission5";
			Dictionary<string, Faction> dictionary7 = new Dictionary<string, Faction>();
			dictionary7["Syladon"] = Faction.darkspacers;
			dictionary[key6] = dictionary7;
			string key7 = "ConquestUmbral8b";
			Dictionary<string, Faction> dictionary8 = new Dictionary<string, Faction>();
			dictionary8["AcolyteVoidweaver"] = Faction.smugglers;
			dictionary[key7] = dictionary8;
			string key8 = "ConquestDarkspace1";
			Dictionary<string, Faction> dictionary9 = new Dictionary<string, Faction>();
			dictionary9["Terravex"] = Faction.darkspacers;
			dictionary[key8] = dictionary9;
			string key9 = "ConquestDarkspace3";
			Dictionary<string, Faction> dictionary10 = new Dictionary<string, Faction>();
			dictionary10["Valdrax"] = Faction.fanatics;
			dictionary[key9] = dictionary10;
			string key10 = "ConquestDarkspace6";
			Dictionary<string, Faction> dictionary11 = new Dictionary<string, Faction>();
			dictionary11["Carkalon"] = Faction.fanatics;
			dictionary11["Meriavex"] = Faction.fanatics;
			dictionary[key10] = dictionary11;
			Dictionary<string, Dictionary<string, Faction>> dictionary12 = dictionary;
			foreach (Mission mission in player.missions)
			{
				Dictionary<string, Faction> dictionary13;
				if (mission.difficulty == MissionDifficulty.Story && mission.storyId != null && dictionary12.TryGetValue(mission.storyId, out dictionary13))
				{
					foreach (MissionStep missionStep in mission.steps)
					{
						foreach (MapPointOfInterest mapPointOfInterest in missionStep.pointsOfInterest)
						{
							foreach (AbstractUnitData abstractUnitData in mapPointOfInterest.GetUnits(true))
							{
								if (abstractUnitData.faction == Faction.player || abstractUnitData.faction == null)
								{
									SpaceShipData spaceShipData = abstractUnitData as SpaceShipData;
									Faction faction;
									if (spaceShipData != null && dictionary13.TryGetValue(spaceShipData.shipClass.identifier, out faction))
									{
										abstractUnitData.faction = faction;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00015030 File Offset: 0x00013230
		public static void FixEliminateMeridiaMission(GamePlayer player)
		{
			if (player.HasStoryteller<Conquest>())
			{
				foreach (Mission mission in GamePlayer.current.missions)
				{
					if (mission.storyId == "ConquestDarkspace8")
					{
						if (mission.steps[0].objectives[1] != null)
						{
							mission.steps[0].objectives.Remove(mission.steps[0].objectives[1]);
						}
						EmbassyStation embassy = GamePlayer.current.GetStoryteller<Conquest>().GetEmbassy(Faction.darkspacers);
						MissionStep missionStep = new MissionStep
						{
							poiHint = embassy
						};
						missionStep.objectives.Add(new TriggerObjective
						{
							trigger = MissionTrigger.CD2TalktoNPC,
							description = "Talk to Midas at the Darkspace Embassy"
						});
						mission.steps.Add(missionStep);
					}
				}
			}
		}
	}
}
