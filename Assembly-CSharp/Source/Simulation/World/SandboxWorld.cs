using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Source.Galaxy;
using Source.Galaxy.NameGenerator;
using Source.Galaxy.POI;
using Source.Galaxy.Statics;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.World
{
	// Token: 0x02000074 RID: 116
	public class SandboxWorld
	{
		// Token: 0x06000435 RID: 1077 RVA: 0x000213EC File Offset: 0x0001F5EC
		public static void SetupWorld(GamePlayer ply)
		{
			SandboxWorld.random = SeededRandom.Global;
			SandboxWorld.map = ply.map;
			int num = 9;
			List<Vector2> list = new List<Vector2>();
			List<MapElement> list2 = new List<MapElement>();
			for (int i = 0; i < num; i++)
			{
				list.Add(GalaxyMapData.GetRandomPosition(list, -38f, 38f, -6f, 6f, 6f));
			}
			list.Sort((Vector2 a, Vector2 b) => a.x.CompareTo(b.x));
			List<string> list3 = new List<string>();
			int count = list.Count;
			while (list3.Count < count)
			{
				string item = Sector.GenerateSubsectorName();
				if (!list3.Contains(item))
				{
					list3.Add(item);
				}
			}
			int num2 = 0;
			foreach (Vector2 pos in list)
			{
				string name = list3[num2++];
				SectorMapData sectorMapData = SandboxWorld.CreateSector(pos, name);
				SandboxWorld.map.AddSector(sectorMapData);
				list2.Add(sectorMapData);
			}
			int num3 = 0;
			foreach (MapElement mapElement in list2)
			{
				int num4 = SandboxWorld.random.RandomRange(7, 10);
				List<MapElement> list4 = new List<MapElement>();
				List<Vector2> list5 = new List<Vector2>();
				for (int j = 0; j < num4; j++)
				{
					list5.Add(GalaxyMapData.GetRandomPosition(list5, -38f, 38f, -6f, 6f, 9f));
				}
				list5.Sort((Vector2 a, Vector2 b) => a.x.CompareTo(b.x));
				int num5 = Math.Max(ply.level, num3 * 3 + 1);
				mapElement.level = num5;
				foreach (Vector2 pos2 in list5)
				{
					SystemMapData item2 = SandboxWorld.CreateEmptySystem(mapElement as SectorMapData, num5, SandboxWorld.random.Choose<Faction>(SandboxWorld.majorFactions), pos2, false);
					list4.Add(item2);
					num5++;
				}
				num3++;
				SandboxWorld.CreateJumpgateLinks(list4, 2, 3);
			}
			SandboxWorld.CreateJumpgateLinks(list2, 2, 3);
			bool flag = true;
			foreach (MapElement mapElement2 in list2)
			{
				SectorMapData sectorMapData2 = mapElement2 as SectorMapData;
				List<SystemMapData> list6 = new List<SystemMapData>(sectorMapData2.allSystems);
				Dictionary<Faction, int> dictionary = SandboxWorld.majorFactions.ToDictionary((Faction f) => f, (Faction f) => 0);
				List<SystemMapData> list7 = new List<SystemMapData>(list6);
				SandboxWorld.random.Shuffle<SystemMapData>(list7);
				foreach (SystemMapData systemMapData in list7)
				{
					int minCount = dictionary.Values.Min();
					List<Faction> list8 = (from kv in dictionary
					where kv.Value == minCount
					select kv.Key).ToList<Faction>();
					Faction faction = SandboxWorld.random.Choose<Faction>(list8);
					Dictionary<Faction, int> dictionary2 = dictionary;
					Faction key = faction;
					int num6 = dictionary2[key];
					dictionary2[key] = num6 + 1;
					systemMapData.faction = faction;
					systemMapData.AddSpaceStation(SandboxWorld.GetStationFacilities(faction, systemMapData.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), faction, null, null);
					if (SandboxWorld.random.RandomBool(0.5f))
					{
						systemMapData.AddMiningPoi(faction, new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), null, 0f, true, 0.5f);
					}
				}
				SystemMapData systemMapData2 = list6[0];
				if (flag)
				{
					list6.RemoveAt(0);
					systemMapData2.AddSpaceStation(SandboxWorld.GetStationFacilities(Faction.miningGuild, systemMapData2.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData2)), Faction.miningGuild, null, null);
					if (systemMapData2.GetPointOfInterest<Mining>() == null)
					{
						systemMapData2.AddMiningPoi(Faction.miningGuild, new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData2)), null, 0f, false, 0.5f);
					}
				}
				foreach (Faction faction2 in SandboxWorld.guilds)
				{
					SystemMapData systemMapData3 = SandboxWorld.random.ChooseAndRemove<SystemMapData>(list6);
					systemMapData3.AddSpaceStation(SandboxWorld.GetStationFacilities(faction2, systemMapData3.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData3)), faction2, null, null);
					if (faction2 == Faction.miningGuild && systemMapData3.GetPointOfInterest<Mining>() == null)
					{
						systemMapData3.AddMiningPoi(faction2, new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData3)), null, 0f, false, 0.5f);
					}
					else if (faction2 == Faction.salvageGuild)
					{
						systemMapData3.AddAncientWreckPoi(faction2, SandboxWorld.GetFreeOrbit(systemMapData3));
					}
				}
				int num7 = SandboxWorld.random.RandomRange(2, 4);
				List<SystemMapData> list9 = new List<SystemMapData>();
				for (int k = 0; k < num7; k++)
				{
					list9.Add(SandboxWorld.AddSideContentSystem(sectorMapData2));
				}
				List<string> list10 = new List<string>
				{
					"PirateHideout",
					"Motherlode",
					"DerelictFleet",
					"FactionSkirmish"
				};
				SandboxWorld.random.Shuffle<string>(list10);
				for (int l = 0; l < list9.Count; l++)
				{
					list9[l].storyteller = SystemStoryteller.Create(list10[l % list10.Count], list9[l]);
					list9[l].storyteller.SetupSystem();
					list9[l].GetEntranceJumpgate().name = Translation.Translate("@GateTo", new object[]
					{
						list9[l].name
					});
				}
				if (flag)
				{
					GreatGate greatGate = new GreatGate
					{
						name = "@MapPOIGreatGate"
					};
					greatGate.SetIdentifier("GreatGatePOI");
					systemMapData2.SetupPOI(greatGate, new Vector2?(new Vector2(-18f, 0f)), Faction.policeGuild, 0);
					CombatStationFactory.CreateGunPlatform(greatGate, Faction.policeGuild, null, 0f, 1);
					systemMapData2.pointsOfInterest.Add(greatGate);
					ply.currentPointOfInterest = greatGate;
					ply.currentSystem = systemMapData2;
					ply.currentSpaceShip.positionData.position = ply.currentPointOfInterest.GetWorldPosition() - new Vector2(5f, 0f);
					flag = false;
					SandboxWorld.AddCanisecStation(systemMapData2, greatGate);
				}
			}
			List<SectorMapData> list11 = new List<SectorMapData>();
			foreach (MapElement mapElement3 in list2)
			{
				list11.Add(mapElement3 as SectorMapData);
			}
			SandboxWorld.AddSidequests(list11);
			SandboxWorld.AddIndustrialGuild(list11);
			SandboxWorld.AddMercenaryGuild(list11);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00021BF0 File Offset: 0x0001FDF0
		public static void AddSidequests(List<SectorMapData> sectors)
		{
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeMining", sectors, 0, (SpaceStation poi) => poi.faction == Faction.miningGuild);
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeCombat", sectors, 0, (SpaceStation poi) => poi.faction == Faction.bountyGuild);
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeSalvage", sectors, 0, (SpaceStation poi) => poi.faction == Faction.salvageGuild);
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeIndustrial", sectors, 1, (SpaceStation poi) => poi.forge != null);
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeDrones", sectors, 1, (SpaceStation poi) => poi.faction == Faction.gold);
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeDefense", sectors, 1, (SpaceStation poi) => poi.faction == Faction.policeGuild);
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeEconomy", sectors, 3, (SpaceStation poi) => poi.faction == Faction.tradingGuild);
			SandboxWorld.AddSidequestCharacter("QuestgiverBounty", sectors, 0, (SpaceStation poi) => poi.bountyBoard != null);
			SandboxWorld.AddSidequestCharacter("QuestgiverPatrol", sectors, 0, (SpaceStation poi) => poi.patrolBoard != null);
			int preferredSector = Mathf.Max(sectors.IndexOf(SectorMapData.current), 4);
			SandboxWorld.AddSidequestCharacter("QuestgiverFastLaneTravel", sectors, preferredSector, (SpaceStation poi) => poi.faction == Faction.tradingGuild);
			List<SectorMapData> list = GalaxyMapData.current.GetQuadrant(1).ToList<SectorMapData>();
			if (list.Count == 0)
			{
				list = GalaxyMapData.current.allSectors.ToList<SectorMapData>();
			}
			SandboxWorld.AddSidequestCharacter("QuestgiverSkilltreeUnlockTier2", sectors, list.Count - 3, (SpaceStation poi) => poi.faction == Faction.red);
			SandboxWorld.AddSidequestCharacter("QuestgiverGoToConquest", sectors, sectors.Count - 1, (SpaceStation poi) => poi.faction == Faction.policeGuild);
			SandboxWorld.AddSidequestCharacter("QuestgiverMercenaryIntroduction", sectors, 3, (SpaceStation poi) => poi.faction == Faction.policeGuild);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00021E78 File Offset: 0x00020078
		private static void AddSidequestCharacter(string character, List<SectorMapData> sectors, int preferredSector, SectorMapData.PointOfInterestFilter<SpaceStation> filter)
		{
			foreach (SectorMapData sectorMapData in sectors)
			{
				foreach (SystemMapData systemMapData in sectorMapData.allSystems)
				{
					using (IEnumerator<SpaceStation> enumerator3 = systemMapData.GetPointsOfInterest<SpaceStation>().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.characters.Contains(character))
							{
								return;
							}
						}
					}
				}
			}
			bool flag = true;
			SectorMapData.PointOfInterestFilter<SpaceStation> _delegate_0;
			while (preferredSector < sectors.Count)
			{
				SectorMapData sectorMapData2 = sectors[preferredSector];
				SectorMapData.PointOfInterestFilter<SpaceStation> filter2;
				if ((filter2 = _delegate_0) == null)
				{
					filter2 = (_delegate_0 = ((SpaceStation poi) => !poi.system.pocketSystem && poi.characters.Count == 0 && filter(poi)));
				}
				SpaceStation spaceStation = sectorMapData2.RandomPointOfInterest<SpaceStation>(filter2, null);
				if (spaceStation != null)
				{
					spaceStation.characters.Add(character);
					return;
				}
				if (flag)
				{
					flag = false;
					preferredSector = 0;
				}
				else
				{
					preferredSector++;
				}
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00021FA8 File Offset: 0x000201A8
		public static HashSet<SpaceStationFacility> GetStationFacilities(Faction owner, int level)
		{
			HashSet<SpaceStationFacility> hashSet = new HashSet<SpaceStationFacility>
			{
				SpaceStationFacility.GeneralShop,
				SpaceStationFacility.MissionBoard
			};
			if (SandboxWorld.random.RandomBool(0.75f))
			{
				hashSet.Add(SpaceStationFacility.Shipyard);
			}
			if (SandboxWorld.random.RandomBool(0.5f))
			{
				hashSet.Add(SpaceStationFacility.Bar);
			}
			hashSet.Add(SpaceStationFacility.PersonalHangar);
			if (SandboxWorld.majorFactions.Contains(owner))
			{
				if (SandboxWorld.random.RandomBool(0.75f))
				{
					hashSet.Add(SpaceStationFacility.Bar);
				}
				if (SandboxWorld.random.RandomBool(0.5f))
				{
					hashSet.Add(SpaceStationFacility.Refinery);
				}
			}
			else if (owner == Faction.miningGuild)
			{
				hashSet.Remove(SpaceStationFacility.GeneralShop);
				hashSet.Add(SpaceStationFacility.MiningShop);
				if (SandboxWorld.random.RandomBool(0.75f))
				{
					hashSet.Add(SpaceStationFacility.Refinery);
				}
			}
			else if (owner == Faction.policeGuild)
			{
				hashSet.Remove(SpaceStationFacility.Shipyard);
				hashSet.Remove(SpaceStationFacility.Bar);
				if (level >= 10)
				{
					hashSet.Add(SpaceStationFacility.PoliceBoard);
					hashSet.Add(SpaceStationFacility.PatrolShop);
					hashSet.Remove(SpaceStationFacility.GeneralShop);
				}
			}
			else if (owner == Faction.tradingGuild)
			{
				hashSet.Add(SpaceStationFacility.TradeTerminal);
			}
			else if (owner == Faction.bountyGuild)
			{
				hashSet.Add(SpaceStationFacility.Bar);
				if (level >= 10)
				{
					hashSet.Add(SpaceStationFacility.BountyBoard);
					hashSet.Add(SpaceStationFacility.BountyShop);
					hashSet.Remove(SpaceStationFacility.GeneralShop);
				}
			}
			else if (owner == Faction.salvageGuild)
			{
				hashSet.Remove(SpaceStationFacility.GeneralShop);
				hashSet.Add(SpaceStationFacility.SalvageShop);
				hashSet.Add(SpaceStationFacility.SalvageStation);
				if (level >= 15)
				{
					hashSet.Add(SpaceStationFacility.SalvageWorkshop);
				}
				if (SandboxWorld.random.RandomBool(0.75f))
				{
					hashSet.Add(SpaceStationFacility.Refinery);
				}
			}
			else if (owner == Faction.industrialGuild)
			{
				hashSet.Remove(SpaceStationFacility.GeneralShop);
				hashSet.Remove(SpaceStationFacility.Shipyard);
				hashSet.Remove(SpaceStationFacility.MissionBoard);
				hashSet.Add(SpaceStationFacility.IndustryShop);
				hashSet.Add(SpaceStationFacility.Refinery);
				hashSet.Add(SpaceStationFacility.IndustryBoard);
			}
			else if (owner == Faction.mercenaryGuild)
			{
				hashSet.Add(SpaceStationFacility.RecruitmentCenter);
			}
			return hashSet;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000221AC File Offset: 0x000203AC
		public static SystemMapData AddSideContentSystem(SectorMapData sector)
		{
			Vector2 pos = default(Vector2);
			float num = 0f;
			for (float num2 = -33f; num2 <= 33f; num2 += 2f)
			{
				for (float num3 = -4f; num3 <= 4f; num3 += 2f)
				{
					Vector2 vector = new Vector2(num2, num3);
					float num4 = 99f;
					foreach (SystemMapData systemMapData in sector.allSystems)
					{
						num4 = Mathf.Min(num4, Vector2.Distance(vector, systemMapData.position));
					}
					if (num4 > num)
					{
						pos = vector;
						num = num4;
					}
				}
			}
			SystemMapData systemMapData2 = SandboxWorld.CreateEmptySystem(sector, sector.level + 10, Faction.marauders, pos, false);
			systemMapData2.pocketSystem = true;
			MapElement mapElement = null;
			float num5 = 0f;
			foreach (SystemMapData systemMapData3 in sector.allSystems)
			{
				if (systemMapData3 != systemMapData2)
				{
					float num6 = Vector2.Distance(systemMapData2.position, systemMapData3.position);
					if (mapElement == null || num6 < num5)
					{
						mapElement = systemMapData3;
						num5 = num6;
					}
				}
			}
			if (mapElement != null)
			{
				SandboxWorld.CreateJumpgatePoi(mapElement, systemMapData2, false);
				systemMapData2.level = mapElement.level + 10;
			}
			return systemMapData2;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00022320 File Offset: 0x00020520
		public static SystemMapData AddSideContentSystemToSystem(SectorMapData sector, SystemMapData preferredSystem, int addLevel = 10)
		{
			Vector2 position = preferredSystem.position;
			float num = 3f;
			float num2 = 6f;
			Vector2 pos = position;
			bool flag = false;
			for (float num3 = -33f; num3 <= 33f; num3 += 1f)
			{
				for (float num4 = -4f; num4 <= 4f; num4 += 1f)
				{
					Vector2 vector = new Vector2(num3, num4);
					float num5 = Vector2.Distance(vector, preferredSystem.position);
					if (num5 >= num && num5 <= num2)
					{
						bool flag2 = false;
						foreach (SystemMapData systemMapData in sector.allSystems)
						{
							if (Vector2.Distance(vector, systemMapData.position) < 2f)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							pos = vector;
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			SystemMapData systemMapData2 = SandboxWorld.CreateEmptySystem(sector, preferredSystem.level + addLevel, Faction.marauders, pos, false);
			systemMapData2.pocketSystem = true;
			SandboxWorld.CreateJumpgatePoi(preferredSystem, systemMapData2, false);
			return systemMapData2;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00022440 File Offset: 0x00020640
		public static void CreateJumpgateLinks(List<MapElement> elements, int minLinks, int maxLinks)
		{
			List<ValueTuple<MapElement, MapElement>> list = new List<ValueTuple<MapElement, MapElement>>();
			foreach (MapElement mapElement in elements)
			{
				int num = SeededRandom.Global.RandomRange(minLinks, maxLinks);
				int num2 = 0;
				using (List<ValueTuple<MapElement, MapElement>>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ValueTuple<MapElement, MapElement> valueTuple = enumerator2.Current;
						if (valueTuple.Item1 == mapElement || valueTuple.Item2 == mapElement)
						{
							num2++;
						}
					}
					goto IL_7B;
				}
				goto IL_6D;
				IL_7B:
				if (num2 >= num)
				{
					continue;
				}
				IL_6D:
				SandboxWorld.AddJumpgateLink(mapElement, elements, list);
				num2++;
				goto IL_7B;
			}
			while (SandboxWorld.CloseJumpgateGap(elements, list))
			{
			}
			foreach (ValueTuple<MapElement, MapElement> valueTuple2 in list)
			{
				SandboxWorld.CreateJumpgatePoi(valueTuple2.Item1, valueTuple2.Item2, true);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00022558 File Offset: 0x00020758
		private static bool HasPath(MapElement from, MapElement to, List<ValueTuple<MapElement, MapElement>> jumpgateLinks)
		{
			if (from == to)
			{
				return true;
			}
			HashSet<MapElement> hashSet = new HashSet<MapElement>();
			Queue<MapElement> queue = new Queue<MapElement>();
			queue.Enqueue(from);
			while (queue.Count > 0)
			{
				MapElement mapElement = queue.Dequeue();
				hashSet.Add(mapElement);
				foreach (ValueTuple<MapElement, MapElement> valueTuple in jumpgateLinks)
				{
					if ((valueTuple.Item1 == mapElement && valueTuple.Item2 == to) || (valueTuple.Item2 == mapElement && valueTuple.Item1 == to))
					{
						return true;
					}
					if (valueTuple.Item1 == mapElement && !queue.Contains(valueTuple.Item2) && !hashSet.Contains(valueTuple.Item2))
					{
						queue.Enqueue(valueTuple.Item2);
					}
					else if (valueTuple.Item2 == mapElement && !queue.Contains(valueTuple.Item1) && !hashSet.Contains(valueTuple.Item1))
					{
						queue.Enqueue(valueTuple.Item1);
					}
				}
			}
			return false;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00022680 File Offset: 0x00020880
		private static bool CloseJumpgateGap(List<MapElement> systems, List<ValueTuple<MapElement, MapElement>> jumpgateLinks)
		{
			MapElement mapElement = null;
			MapElement item = null;
			float num = 0f;
			foreach (MapElement mapElement2 in systems)
			{
				foreach (MapElement mapElement3 in systems)
				{
					if (!SandboxWorld.HasPath(mapElement2, mapElement3, jumpgateLinks))
					{
						float num2 = Vector2.Distance(mapElement2.position, mapElement3.position);
						if (mapElement == null || num2 < num)
						{
							mapElement = mapElement2;
							item = mapElement3;
							num = num2;
						}
					}
				}
			}
			if (mapElement != null)
			{
				jumpgateLinks.Add(new ValueTuple<MapElement, MapElement>(mapElement, item));
				return true;
			}
			return false;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00022750 File Offset: 0x00020950
		private static void AddJumpgateLink(MapElement system, List<MapElement> systems, List<ValueTuple<MapElement, MapElement>> jumpgateLinks)
		{
			MapElement mapElement = null;
			float num = 0f;
			foreach (MapElement mapElement2 in systems)
			{
				if (mapElement2 != system)
				{
					bool flag = false;
					foreach (ValueTuple<MapElement, MapElement> valueTuple in jumpgateLinks)
					{
						if ((valueTuple.Item1 == system && valueTuple.Item2 == mapElement2) || (valueTuple.Item1 == mapElement2 && valueTuple.Item2 == system))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						float num2 = Vector2.Distance(system.position, mapElement2.position);
						if (mapElement == null || num2 < num)
						{
							mapElement = mapElement2;
							num = num2;
						}
					}
				}
			}
			if (mapElement != null)
			{
				jumpgateLinks.Add(new ValueTuple<MapElement, MapElement>(system, mapElement));
			}
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00022848 File Offset: 0x00020A48
		private static Vector2 GetYRestrictionForJumpgate(SystemMapData target, SystemMapData originSystem)
		{
			Vector2 sectorPosition = target.sectorPosition;
			Vector2 sectorPosition2 = originSystem.sectorPosition;
			if (sectorPosition.y < sectorPosition2.y)
			{
				return new Vector2(-5f, -3f);
			}
			if (sectorPosition.y > sectorPosition2.y)
			{
				return new Vector2(3f, 5f);
			}
			return new Vector2(-3f, 3f);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000228B0 File Offset: 0x00020AB0
		private static Vector2 GetXRestrictionForJumpgate(SystemMapData target, SystemMapData originSystem)
		{
			Vector2 sectorPosition = target.sectorPosition;
			Vector2 sectorPosition2 = originSystem.sectorPosition;
			if (sectorPosition.x < sectorPosition2.x)
			{
				return new Vector2(-20f, -12f);
			}
			if (sectorPosition.x > sectorPosition2.x)
			{
				return new Vector2(12f, 20f);
			}
			return new Vector2(-3f, 3f);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00022916 File Offset: 0x00020B16
		private static void CreateJumpgatePoi(MapElement a, MapElement b, bool isUnlocked)
		{
			if (a is SystemMapData)
			{
				SandboxWorld.CreateJumpgatePoi(a as SystemMapData, b as SystemMapData, false, isUnlocked);
				return;
			}
			SandboxWorld.CreateJumpgatePoi(a as SectorMapData, b as SectorMapData);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00022948 File Offset: 0x00020B48
		private static void CreateJumpgatePoi(SystemMapData a, SystemMapData b, bool isSectorJumpgate, bool isUnlocked)
		{
			Vector2 xrestrictionForJumpgate = SandboxWorld.GetXRestrictionForJumpgate(a, b);
			Vector2 yrestrictionForJumpgate = SandboxWorld.GetYRestrictionForJumpgate(a, b);
			JumpGate jumpGate = new JumpGate
			{
				name = Translation.Translate("@GateTo", new object[]
				{
					isSectorJumpgate ? a.sector.name : a.name
				}),
				targetSystemGuid = a.guid,
				sectorJumpgate = isSectorJumpgate
			};
			b.SetupPOI(jumpGate, new Vector2?(b.GetRandomPosition(xrestrictionForJumpgate.x, xrestrictionForJumpgate.y, yrestrictionForJumpgate.x, yrestrictionForJumpgate.y)), null, 0);
			b.pointsOfInterest.Add(jumpGate);
			xrestrictionForJumpgate = SandboxWorld.GetXRestrictionForJumpgate(b, a);
			yrestrictionForJumpgate = SandboxWorld.GetYRestrictionForJumpgate(b, a);
			JumpGate jumpGate2 = new JumpGate
			{
				name = Translation.Translate("@GateTo", new object[]
				{
					isSectorJumpgate ? b.sector.name : b.name
				}),
				targetSystemGuid = b.guid,
				sectorJumpgate = isSectorJumpgate
			};
			a.SetupPOI(jumpGate2, new Vector2?(a.GetRandomPosition(xrestrictionForJumpgate.x, xrestrictionForJumpgate.y, yrestrictionForJumpgate.x, yrestrictionForJumpgate.y)), null, 0);
			a.pointsOfInterest.Add(jumpGate2);
			jumpGate.targetPoiGuid = jumpGate2.guid;
			jumpGate2.targetPoiGuid = jumpGate.guid;
			if (isUnlocked)
			{
				jumpGate.UnlockJumpgate();
				jumpGate2.UnlockJumpgate();
			}
			if (isSectorJumpgate)
			{
				SandboxWorld.AddCanisecStation(a, jumpGate2);
				SandboxWorld.AddCanisecStation(b, jumpGate);
			}
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00022AC0 File Offset: 0x00020CC0
		private static void AddCanisecStation(SystemMapData sys, MapPointOfInterest nearby)
		{
			sys.AddSpaceStation(SandboxWorld.GetStationFacilities(Faction.policeGuild, sys.level), new Vector2?(sys.GetRandomPosition(Mathf.Max(-20f, nearby.position.x - 4f), Mathf.Min(20f, nearby.position.x + 4f), Mathf.Max(-5f, nearby.position.y - 4f), Mathf.Min(5f, nearby.position.y + 4f))), Faction.policeGuild, null, null);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00022B64 File Offset: 0x00020D64
		private static void CreateJumpgatePoi(SectorMapData a, SectorMapData b)
		{
			float num = a.position.x * 100f + (float)(10000 * a.quadrant);
			float num2 = a.position.y * 100f;
			float num3 = b.position.x * 100f + (float)(10000 * b.quadrant);
			float num4 = b.position.y * 100f;
			SystemMapData systemMapData = null;
			SystemMapData b2 = null;
			float num5 = 0f;
			foreach (SystemMapData systemMapData2 in a.allSystems)
			{
				if (!systemMapData2.pocketSystem && !SandboxWorld.HasSectorJumpgate(systemMapData2))
				{
					foreach (SystemMapData systemMapData3 in b.allSystems)
					{
						if (!systemMapData3.pocketSystem && !SandboxWorld.HasSectorJumpgate(systemMapData3))
						{
							float num6 = Vector2.Distance(new Vector2(systemMapData2.position.x + num, systemMapData2.position.y + num2), new Vector2(systemMapData3.position.x + num3, systemMapData3.position.y + num4));
							if (systemMapData == null || num6 < num5)
							{
								systemMapData = systemMapData2;
								b2 = systemMapData3;
								num5 = num6;
							}
						}
					}
				}
			}
			if (systemMapData != null)
			{
				SandboxWorld.CreateJumpgatePoi(systemMapData, b2, true, false);
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00022D00 File Offset: 0x00020F00
		private static bool HasSectorJumpgate(SystemMapData system)
		{
			foreach (MapPointOfInterest mapPointOfInterest in system.pointsOfInterest)
			{
				JumpGate jumpGate = mapPointOfInterest as JumpGate;
				if (jumpGate != null && jumpGate.sectorJumpgate)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00022D64 File Offset: 0x00020F64
		public static Vector2 GetFreeOrbit(SystemMapData system)
		{
			foreach (MapStatic mapStatic in system.statics)
			{
				if (mapStatic is Planet)
				{
					bool flag = true;
					using (List<MapPointOfInterest>.Enumerator enumerator2 = system.pointsOfInterest.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (Vector2.Distance(enumerator2.Current.position, mapStatic.position) < 1.5f)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						return mapStatic.position + new Vector2(SandboxWorld.random.RandomRange(-0.15f, 0.15f), SandboxWorld.random.RandomRange(-0.15f, 0.15f));
					}
				}
			}
			return system.GetRandomPosition(-20f, 20f, -5f, 5f);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00022E74 File Offset: 0x00021074
		private static SectorMapData CreateSector(Vector2 pos, string name)
		{
			return new SectorMapData(pos)
			{
				name = name,
				quadrant = SectorMapData.quadrantFrontier
			};
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00022E90 File Offset: 0x00021090
		public static bool IsSystemNameTaken(string name)
		{
			using (IEnumerator<SystemMapData> enumerator = GalaxyMapData.current.allSystems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.name == name)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00022EF0 File Offset: 0x000210F0
		public static string EnsureUniqueName(Func<string> generator)
		{
			string text = null;
			for (int i = 0; i < 5; i++)
			{
				text = generator();
				if (!SandboxWorld.IsSystemNameTaken(text))
				{
					return text;
				}
			}
			List<string> list = new List<string>(SandboxWorld.nameSuffixes);
			SandboxWorld.random.Shuffle<string>(list);
			foreach (string str in list)
			{
				string text2 = text + " " + str;
				if (!SandboxWorld.IsSystemNameTaken(text2))
				{
					return text2;
				}
			}
			int num = 2;
			string text3;
			do
			{
				text3 = text + " " + num++.ToString();
			}
			while (SandboxWorld.IsSystemNameTaken(text3));
			return text3;
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00022FB8 File Offset: 0x000211B8
		public static SystemMapData CreateEmptySystem(SectorMapData sector, int level, Faction owner, Vector2 pos, bool mainMenu = false)
		{
			if (SandboxWorld.random == null)
			{
				SandboxWorld.random = SeededRandom.Global;
			}
			SystemMapData systemMapData = new SystemMapData(sector, pos)
			{
				level = level,
				faction = owner
			};
			sector.AddSystem(systemMapData);
			string text = mainMenu ? "BogusMenusicum" : SandboxWorld.EnsureUniqueName(new Func<string>(Source.Galaxy.NameGenerator.Star.GenerateStarName));
			string text2 = SandboxWorld.random.Choose<string>(Source.Galaxy.Statics.Star.StarType);
			systemMapData.name = text;
			Source.Galaxy.Statics.Star item = new Source.Galaxy.Statics.Star
			{
				position = new Vector2(0f, 0f),
				name = text + " (" + text2 + ")",
				level = level,
				color = Source.Galaxy.Statics.Star.GetRandomColorForType(text2),
				intensity = SeededRandom.Global.RandomRange(2f, 5f)
			};
			systemMapData.statics.Add(item);
			systemMapData.GenerateSystemOreData(true);
			systemMapData.storyId = "tutorial_system_" + level.ToString();
			int num = mainMenu ? SandboxWorld.random.RandomRange(1, 2) : SandboxWorld.random.RandomRange(4, 8);
			for (int i = 0; i < num; i++)
			{
				systemMapData.AddPlanet("TODO", SandboxWorld.random.ChooseEnum<PlanetType>(0), SeededRandom.Global.RandomRange(6f, 30f), null, !mainMenu);
			}
			return systemMapData;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00023120 File Offset: 0x00021320
		public static void AddIndustrialGuild(List<SectorMapData> sectors)
		{
			if (SandboxWorld.random == null)
			{
				SandboxWorld.random = SeededRandom.Global;
			}
			foreach (SectorMapData sectorMapData in sectors)
			{
				if (sectorMapData.sectorLevel.Item1 >= 10)
				{
					SystemMapData systemMapData;
					do
					{
						systemMapData = SandboxWorld.random.Choose<SystemMapData>(sectorMapData.allSystems);
					}
					while (systemMapData.pocketSystem);
					systemMapData.AddSpaceStation(SandboxWorld.GetStationFacilities(Faction.industrialGuild, systemMapData.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), Faction.industrialGuild, null, null);
				}
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000231CC File Offset: 0x000213CC
		public static void AddMercenaryGuild(List<SectorMapData> sectors)
		{
			if (SandboxWorld.random == null)
			{
				SandboxWorld.random = SeededRandom.Global;
			}
			for (int i = 3; i < sectors.Count; i++)
			{
				SectorMapData sectorMapData = sectors[i];
				if (!sectorMapData.allSystems.Any((SystemMapData s) => s.GetPointsOfInterest<SpaceStation>().Any((SpaceStation ss) => ss.faction == Faction.mercenaryGuild)))
				{
					List<SystemMapData> list = (from s in sectorMapData.allSystems
					where s.GetPointOfInterest<SpaceStation>() == null && !s.pocketSystem
					select s).ToList<SystemMapData>();
					if (list.Count == 0)
					{
						list = (from s in sectorMapData.allSystems
						where !s.pocketSystem
						select s).ToList<SystemMapData>();
					}
					if (list.Count == 0)
					{
						list = new List<SystemMapData>(sectorMapData.allSystems);
					}
					SystemMapData systemMapData = SandboxWorld.random.Choose<SystemMapData>(list);
					systemMapData.AddSpaceStation(SandboxWorld.GetStationFacilities(Faction.mercenaryGuild, systemMapData.level), new Vector2?(SandboxWorld.GetFreeOrbit(systemMapData)), Faction.mercenaryGuild, null, null);
				}
			}
		}

		// Token: 0x04000249 RID: 585
		public const float SectorMinX = -38f;

		// Token: 0x0400024A RID: 586
		public const float SectorMaxX = 38f;

		// Token: 0x0400024B RID: 587
		public const float SectorMinY = -6f;

		// Token: 0x0400024C RID: 588
		public const float SectorMaxY = 6f;

		// Token: 0x0400024D RID: 589
		public const float PoiMinX = -20f;

		// Token: 0x0400024E RID: 590
		public const float PoiMaxX = 20f;

		// Token: 0x0400024F RID: 591
		public const float PoiMinY = -6f;

		// Token: 0x04000250 RID: 592
		public const float PoiMaxY = 6f;

		// Token: 0x04000251 RID: 593
		private static SeededRandom random;

		// Token: 0x04000252 RID: 594
		private static GalaxyMapData map;

		// Token: 0x04000253 RID: 595
		private static List<Faction> majorFactions = new List<Faction>
		{
			Faction.red,
			Faction.blue,
			Faction.gold
		};

		// Token: 0x04000254 RID: 596
		private static List<Faction> guilds = new List<Faction>
		{
			Faction.miningGuild,
			Faction.tradingGuild,
			Faction.salvageGuild,
			Faction.bountyGuild
		};

		// Token: 0x04000255 RID: 597
		private static readonly string[] nameSuffixes = new string[]
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
	}
}
