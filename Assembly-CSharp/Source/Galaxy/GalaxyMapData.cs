using System;
using System.Collections.Generic;
using LightJson;
using Source.MissionSystem;
using Source.Player;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000144 RID: 324
	public class GalaxyMapData : IJsonSource
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00056E4C File Offset: 0x0005504C
		public static GalaxyMapData current
		{
			get
			{
				GamePlayer current = GamePlayer.current;
				if (current == null)
				{
					return null;
				}
				return current.map;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00056E5E File Offset: 0x0005505E
		public IEnumerable<SectorMapData> allSectors
		{
			get
			{
				return this.sectors;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00056E66 File Offset: 0x00055066
		public IEnumerable<SystemMapData> allSystems
		{
			get
			{
				foreach (SectorMapData sectorMapData in this.sectors)
				{
					foreach (SystemMapData systemMapData in sectorMapData.allSystems)
					{
						yield return systemMapData;
					}
					IEnumerator<SystemMapData> enumerator2 = null;
				}
				List<SectorMapData>.Enumerator enumerator = default(List<SectorMapData>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00056E76 File Offset: 0x00055076
		public IEnumerable<SystemMapData> allConquestSystems
		{
			get
			{
				foreach (SectorMapData sectorMapData in this.sectors)
				{
					if (sectorMapData.conquestSector)
					{
						foreach (SystemMapData systemMapData in sectorMapData.allSystems)
						{
							yield return systemMapData;
						}
						IEnumerator<SystemMapData> enumerator2 = null;
					}
				}
				List<SectorMapData>.Enumerator enumerator = default(List<SectorMapData>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00056E86 File Offset: 0x00055086
		public IEnumerable<SystemMapData> allConquestStagingSystems
		{
			get
			{
				foreach (SectorMapData sectorMapData in this.sectors)
				{
					if (sectorMapData.quadrant != 2 || !sectorMapData.conquestSector)
					{
						foreach (SystemMapData systemMapData in sectorMapData.allSystems)
						{
							yield return systemMapData;
						}
						IEnumerator<SystemMapData> enumerator2 = null;
					}
				}
				List<SectorMapData>.Enumerator enumerator = default(List<SectorMapData>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00056E96 File Offset: 0x00055096
		public IEnumerable<SystemMapData> allFrontierSystems
		{
			get
			{
				foreach (SectorMapData sectorMapData in this.sectors)
				{
					if (sectorMapData.quadrant == 1)
					{
						foreach (SystemMapData systemMapData in sectorMapData.allSystems)
						{
							yield return systemMapData;
						}
						IEnumerator<SystemMapData> enumerator2 = null;
					}
				}
				List<SectorMapData>.Enumerator enumerator = default(List<SectorMapData>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00056EA6 File Offset: 0x000550A6
		public IEnumerable<MapPointOfInterest> allPointsOfInterest
		{
			get
			{
				foreach (SystemMapData systemMapData in this.allSystems)
				{
					foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
					{
						yield return mapPointOfInterest;
					}
					List<MapPointOfInterest>.Enumerator enumerator2 = default(List<MapPointOfInterest>.Enumerator);
				}
				IEnumerator<SystemMapData> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00056EB6 File Offset: 0x000550B6
		public IEnumerable<SystemMapData> currentSector
		{
			get
			{
				return GamePlayer.current.currentSystem.sector.allSystems;
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00056ECC File Offset: 0x000550CC
		public void ClearSectors()
		{
			this.sectors.Clear();
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x00056ED9 File Offset: 0x000550D9
		public void RemoveSector(SectorMapData smd)
		{
			this.sectors.Remove(smd);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00056EE8 File Offset: 0x000550E8
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"sectors",
					this.sectors.ToJsonArray<SectorMapData>()
				}
			};
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00056F10 File Offset: 0x00055110
		public void AddSector(SectorMapData system)
		{
			this.sectors.Add(system);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00056F1E File Offset: 0x0005511E
		public IEnumerable<SectorMapData> GetQuadrant(int quadrant)
		{
			foreach (SectorMapData sectorMapData in this.sectors)
			{
				if (sectorMapData.quadrant == quadrant)
				{
					yield return sectorMapData;
				}
			}
			List<SectorMapData>.Enumerator enumerator = default(List<SectorMapData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00056F38 File Offset: 0x00055138
		public SystemMapData GetSystem(string guid)
		{
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				if (systemMapData.guid == guid)
				{
					return systemMapData;
				}
			}
			return null;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00056F94 File Offset: 0x00055194
		public SystemMapData GetSystemByStoryId(string id)
		{
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				if (systemMapData.storyId == id)
				{
					return systemMapData;
				}
			}
			return null;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00056FF0 File Offset: 0x000551F0
		public MapPointOfInterest GetPointOfInterest(string guid)
		{
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					if (mapPointOfInterest.guid == guid)
					{
						return mapPointOfInterest;
					}
				}
			}
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				foreach (MissionStep missionStep in mission.steps)
				{
					string a;
					if (missionStep == null)
					{
						a = null;
					}
					else
					{
						MapPointOfInterest dynamicPointOfInterest = missionStep.dynamicPointOfInterest;
						a = ((dynamicPointOfInterest != null) ? dynamicPointOfInterest.guid : null);
					}
					if (a == guid)
					{
						return missionStep.dynamicPointOfInterest;
					}
				}
			}
			return null;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0005712C File Offset: 0x0005532C
		public void FromJson(JsonValue val)
		{
			if (val["systems"].IsJsonArray)
			{
				val["guid"] = Guid.NewGuid().ToString();
				val["position"] = JsonUtil.Vector2ToJson(Vector2.zero);
				SectorMapData sectorMapData = SectorMapData.FromJson(val);
				sectorMapData.SetBackgroundProperties();
				sectorMapData.name = "Galaxy's Edge";
				sectorMapData.level = 1;
				this.AddSector(sectorMapData);
			}
			else
			{
				this.sectors.FromJsonArray(val["sectors"], new ClassExtensions.ParseJsonValue<SectorMapData>(SectorMapData.FromJson));
			}
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				this.RegisterSystem(systemMapData);
				foreach (MapPointOfInterest poi in systemMapData.pointsOfInterest)
				{
					this.RegisterPointOfInterest(poi);
				}
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00057268 File Offset: 0x00055468
		private void RegisterPointOfInterest(MapPointOfInterest poi)
		{
			if (this.poiLoaders.ContainsKey(poi.guid))
			{
				foreach (GalaxyMapData.MapPOILoader mapPOILoader in this.poiLoaders[poi.guid])
				{
					mapPOILoader(poi);
				}
				this.poiLoaders.Remove(poi.guid);
			}
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000572EC File Offset: 0x000554EC
		private void RegisterSystem(SystemMapData system)
		{
			if (this.systemLoaders.ContainsKey(system.guid))
			{
				foreach (GalaxyMapData.MapSystemLoader mapSystemLoader in this.systemLoaders[system.guid])
				{
					mapSystemLoader(system);
				}
				this.systemLoaders.Remove(system.guid);
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00057370 File Offset: 0x00055570
		public void LoadPointOfInterest(string guid, GalaxyMapData.MapPOILoader toCall)
		{
			MapPointOfInterest pointOfInterest = this.GetPointOfInterest(guid);
			if (pointOfInterest != null)
			{
				toCall(pointOfInterest);
				return;
			}
			List<GalaxyMapData.MapPOILoader> list;
			if (!this.poiLoaders.TryGetValue(guid, out list))
			{
				list = new List<GalaxyMapData.MapPOILoader>();
				this.poiLoaders[guid] = list;
			}
			list.Add(toCall);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000573BC File Offset: 0x000555BC
		public void LoadSystem(string guid, GalaxyMapData.MapSystemLoader toCall)
		{
			SystemMapData system = this.GetSystem(guid);
			if (system != null)
			{
				toCall(system);
				return;
			}
			List<GalaxyMapData.MapSystemLoader> list;
			if (!this.systemLoaders.TryGetValue(guid, out list))
			{
				list = new List<GalaxyMapData.MapSystemLoader>();
				this.systemLoaders[guid] = list;
			}
			list.Add(toCall);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00057408 File Offset: 0x00055608
		public static Vector2 GetRandomPosition(List<Vector2> exclude, float minX, float maxX, float minY, float maxY, float minDistance)
		{
			Vector2 vector = default(Vector2);
			int num = 0;
			for (;;)
			{
				num++;
				vector = new Vector2(SeededRandom.Global.RandomRange(minX, maxX), SeededRandom.Global.RandomRange(minY, maxY));
				bool flag = true;
				foreach (Vector2 b in exclude)
				{
					if (Vector2.Distance(vector, b) < minDistance)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				if (num >= 50)
				{
					return vector;
				}
			}
			return vector;
		}

		// Token: 0x040006D2 RID: 1746
		private List<SectorMapData> sectors = new List<SectorMapData>();

		// Token: 0x040006D3 RID: 1747
		private Dictionary<string, List<GalaxyMapData.MapPOILoader>> poiLoaders = new Dictionary<string, List<GalaxyMapData.MapPOILoader>>();

		// Token: 0x040006D4 RID: 1748
		private Dictionary<string, List<GalaxyMapData.MapSystemLoader>> systemLoaders = new Dictionary<string, List<GalaxyMapData.MapSystemLoader>>();

		// Token: 0x020004BD RID: 1213
		// (Invoke) Token: 0x06002972 RID: 10610
		public delegate void MapPOILoader(MapPointOfInterest poi);

		// Token: 0x020004BE RID: 1214
		// (Invoke) Token: 0x06002976 RID: 10614
		public delegate void MapSystemLoader(SystemMapData system);
	}
}
