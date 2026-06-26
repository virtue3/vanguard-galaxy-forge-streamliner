using System;
using System.Collections.Generic;
using System.Linq;
using LightJson;
using Source.Galaxy;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A8 RID: 168
	public class MissionStep : IJsonSource
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000396BE File Offset: 0x000378BE
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x000396C6 File Offset: 0x000378C6
		public List<MissionObjective> objectives { get; private set; } = new List<MissionObjective>();

		// Token: 0x170000DE RID: 222
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x000396CF File Offset: 0x000378CF
		public MapPointOfInterest poiHint
		{
			set
			{
				this.poiHints.Add(value);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x000396DD File Offset: 0x000378DD
		public IEnumerable<MapPointOfInterest> pointsOfInterest
		{
			get
			{
				if (this.dynamicPointOfInterest != null)
				{
					yield return this.dynamicPointOfInterest;
				}
				foreach (MapPointOfInterest mapPointOfInterest in this.poiHints)
				{
					yield return mapPointOfInterest;
				}
				List<MapPointOfInterest>.Enumerator enumerator = default(List<MapPointOfInterest>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x000396ED File Offset: 0x000378ED
		public bool hasPointOfInterest
		{
			get
			{
				return this.dynamicPointOfInterest != null || this.poiHints.Count > 0;
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00039708 File Offset: 0x00037908
		public bool IsMissionPoi(MapPointOfInterest poi)
		{
			using (IEnumerator<MapPointOfInterest> enumerator = this.pointsOfInterest.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == poi)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00039758 File Offset: 0x00037958
		public MapPointOfInterest GetMissionPoi()
		{
			if (this.pointsOfInterest.Count<MapPointOfInterest>() > 0)
			{
				return this.pointsOfInterest.First<MapPointOfInterest>();
			}
			return null;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00039778 File Offset: 0x00037978
		public bool isComplete
		{
			get
			{
				if (this.requireAllObjectives)
				{
					return this.objectives.All((MissionObjective obj) => obj.IsComplete());
				}
				return this.objectives.Any((MissionObjective obj) => obj.IsComplete());
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x000397E4 File Offset: 0x000379E4
		public MissionObjective currentObjective
		{
			get
			{
				foreach (MissionObjective missionObjective in this.objectives)
				{
					if (!missionObjective.IsComplete())
					{
						return missionObjective;
					}
				}
				return null;
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00039840 File Offset: 0x00037A40
		public void AddPoiHints(params MapPointOfInterest[] pois)
		{
			foreach (MapPointOfInterest item in pois)
			{
				this.poiHints.Add(item);
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00039870 File Offset: 0x00037A70
		public Sprite GetIcon()
		{
			foreach (MissionObjective missionObjective in this.objectives)
			{
				if (missionObjective.GetIcon() != null)
				{
					return missionObjective.GetIcon();
				}
			}
			return null;
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x000398D8 File Offset: 0x00037AD8
		public void OnMissionTurnedIn()
		{
			foreach (MissionObjective missionObjective in this.objectives)
			{
				missionObjective.OnMissionTurnedIn();
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00039928 File Offset: 0x00037B28
		public JsonValue ToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (MapPointOfInterest mapPointOfInterest in this.poiHints)
			{
				jsonArray.Add(mapPointOfInterest.guid);
			}
			JsonObject jsonObject = new JsonObject();
			jsonObject.Add("description", this.description);
			jsonObject.Add("objectives", this.objectives.ToJsonArray<MissionObjective>());
			string key = "system";
			SystemMapData systemMapData = this.system;
			jsonObject.Add(key, (systemMapData != null) ? systemMapData.guid : null);
			string key2 = "pointOfInterest";
			MapPointOfInterest mapPointOfInterest2 = this.dynamicPointOfInterest;
			jsonObject.Add(key2, (mapPointOfInterest2 != null) ? mapPointOfInterest2.ToJson() : JsonValue.Null);
			jsonObject.Add("poiHints", jsonArray);
			jsonObject.Add("requireAllObjectives", new bool?(this.requireAllObjectives));
			jsonObject.Add("hidden", new bool?(this.hidden));
			return jsonObject;
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00039A58 File Offset: 0x00037C58
		public static MissionStep FromJson(JsonValue data)
		{
			MissionStep step = new MissionStep
			{
				description = data["description"]
			};
			step.objectives.FromJsonArray(data["objectives"], new ClassExtensions.ParseJsonValue<MissionObjective>(MissionObjective.FromJson));
			if (!data["system"].IsNull)
			{
				GalaxyMapData.current.LoadSystem(data["system"], delegate(SystemMapData system)
				{
					step.system = system;
					if (step.dynamicPointOfInterest != null)
					{
						step.dynamicPointOfInterest.system = system;
					}
				});
				if (!data["pointOfInterest"].IsNull)
				{
					step.dynamicPointOfInterest = MapPointOfInterest.FromJson(data["pointOfInterest"]);
				}
			}
			if (data["poiHint"].IsString)
			{
				GalaxyMapData.current.LoadPointOfInterest(data["poiHint"], delegate(MapPointOfInterest poi)
				{
					step.poiHint = poi;
				});
			}
			if (data["poiHints"].IsJsonArray)
			{
				GalaxyMapData.MapPOILoader _delegate_2;
				foreach (JsonValue jsonValue in data["poiHints"].AsJsonArray)
				{
					GalaxyMapData current = GalaxyMapData.current;
					string guid = jsonValue;
					GalaxyMapData.MapPOILoader toCall;
					if ((toCall = _delegate_2) == null)
					{
						toCall = (_delegate_2 = delegate(MapPointOfInterest poi)
						{
							step.poiHints.Add(poi);
						});
					}
					current.LoadPointOfInterest(guid, toCall);
				}
			}
			if (data["requireAllObjectives"].IsBoolean)
			{
				step.requireAllObjectives = data["requireAllObjectives"].AsBoolean;
			}
			if (data["hidden"].IsBoolean)
			{
				step.hidden = data["hidden"].AsBoolean;
			}
			return step;
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00039C64 File Offset: 0x00037E64
		public bool IsPointOfInterest(MapElement el)
		{
			MapPointOfInterest mapPointOfInterest = el as MapPointOfInterest;
			if (mapPointOfInterest != null)
			{
				MissionObjective currentObjective = this.currentObjective;
				if (((currentObjective != null) ? currentObjective.GetPoi() : null) == mapPointOfInterest)
				{
					return true;
				}
				using (IEnumerator<MapPointOfInterest> enumerator = this.pointsOfInterest.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapPointOfInterest mapPointOfInterest2 = enumerator.Current;
						if (mapPointOfInterest == mapPointOfInterest2)
						{
							return true;
						}
					}
					return false;
				}
			}
			SystemMapData systemMapData = el as SystemMapData;
			if (systemMapData != null)
			{
				using (IEnumerator<MapPointOfInterest> enumerator = systemMapData.allPointsOfInterest.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapPointOfInterest el2 = enumerator.Current;
						if (this.IsPointOfInterest(el2))
						{
							return true;
						}
					}
					return false;
				}
			}
			SectorMapData sectorMapData = el as SectorMapData;
			if (sectorMapData != null)
			{
				foreach (SystemMapData el3 in sectorMapData.allSystems)
				{
					if (this.IsPointOfInterest(el3))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040003C4 RID: 964
		public SystemMapData system;

		// Token: 0x040003C5 RID: 965
		public string description;

		// Token: 0x040003C6 RID: 966
		public MapPointOfInterest dynamicPointOfInterest;

		// Token: 0x040003C7 RID: 967
		private List<MapPointOfInterest> poiHints = new List<MapPointOfInterest>();

		// Token: 0x040003C8 RID: 968
		public bool requireAllObjectives = true;

		// Token: 0x040003C9 RID: 969
		public bool hidden;
	}
}
