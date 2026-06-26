using System;
using System.Collections.Generic;
using Behavior.UI.GalaxyMap;
using Behaviour.Background;
using Behaviour.Bootstrap;
using Behaviour.GalaxyMap.Util;
using Behaviour.UI.Side_Menu;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.World.System;
using Source.Simulation.World.Util;
using Source.Util;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Scripts.Behaviour.Background;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000329 RID: 809
	public abstract class AbstractGalaxyMapManager : MonoBehaviour
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000B5A2B File Offset: 0x000B3C2B
		// (set) Token: 0x06001E83 RID: 7811 RVA: 0x000B5A33 File Offset: 0x000B3C33
		public SystemMapData currentSystem { get; protected set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x000B5A3C File Offset: 0x000B3C3C
		// (set) Token: 0x06001E85 RID: 7813 RVA: 0x000B5A44 File Offset: 0x000B3C44
		public SectorMapData currentSector { get; protected set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x000B5A4D File Offset: 0x000B3C4D
		// (set) Token: 0x06001E87 RID: 7815 RVA: 0x000B5A55 File Offset: 0x000B3C55
		public int zoomLevel { get; protected set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000B5A5E File Offset: 0x000B3C5E
		// (set) Token: 0x06001E89 RID: 7817 RVA: 0x000B5A66 File Offset: 0x000B3C66
		public int currentQuadrant { get; protected set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x000B5A6F File Offset: 0x000B3C6F
		public virtual Rect xyBounds
		{
			get
			{
				return Rect.zero;
			}
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x000B5A76 File Offset: 0x000B3C76
		protected virtual void Awake()
		{
			AbstractGalaxyMapManager.current = this;
			SidePanel instance = SidePanel.instance;
			this.focusPointOfInterest = ((instance != null) ? instance.focussedPoi : null);
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x000B5A95 File Offset: 0x000B3C95
		protected virtual void Start()
		{
			this.InitGalaxyMap();
			this.galaxyBackground.SetScreenSize(this.mapSize);
			this.galaxyBackground.gameObject.SetActive(false);
			this.SetMapLocation();
			this.currentQuadrant = SectorMapData.current.quadrant;
		}

		// Token: 0x06001E8D RID: 7821
		protected abstract void InitGalaxyMap();

		// Token: 0x06001E8E RID: 7822 RVA: 0x000B5AD8 File Offset: 0x000B3CD8
		protected virtual void Update()
		{
			if (Input.GetKeyUp(KeyCode.Alpha1) && this.zoomLevel != 2)
			{
				this.ShowGalaxyMap(-1);
				return;
			}
			if (Input.GetKeyUp(KeyCode.Alpha2) && this.zoomLevel != 1)
			{
				this.ShowSectorMap(SectorMapData.current);
				return;
			}
			if (Input.GetKeyUp(KeyCode.Alpha3) && this.zoomLevel != 0)
			{
				this.ShowSystemMap(SystemMapData.current);
			}
		}

		// Token: 0x06001E8F RID: 7823
		protected abstract void SetMapLocation();

		// Token: 0x06001E90 RID: 7824
		protected abstract void SetShipLocation();

		// Token: 0x06001E91 RID: 7825 RVA: 0x000B5B3C File Offset: 0x000B3D3C
		public void CreateShipLocation(Transform parent, Vector2 position)
		{
			Sprite sprite = GamePlayer.current.currentSpaceShip.GetSprite();
			this.shipLocation = UnityEngine.Object.Instantiate<ShipLocation>(this.shipLocationPrefab, parent);
			this.shipLocation.SetSprite(sprite);
			this.shipLocation.transform.localPosition = position;
		}

		// Token: 0x06001E92 RID: 7826
		public abstract void ShowGalaxyMap(int quadrant = -1);

		// Token: 0x06001E93 RID: 7827
		public abstract void ShowSectorMap(SectorMapData smd);

		// Token: 0x06001E94 RID: 7828
		public abstract void ShowSystemMap(SystemMapData systemMapData);

		// Token: 0x06001E95 RID: 7829
		public abstract void RefreshGalaxyMap(int quadrant);

		// Token: 0x06001E96 RID: 7830
		public abstract void RefreshSectorMap(SectorMapData currentSector);

		// Token: 0x06001E97 RID: 7831 RVA: 0x000B5B90 File Offset: 0x000B3D90
		protected void DrawConquestPolygons(SectorMapData smd, List<WorldMapStatic> systems)
		{
			List<Vector2> list = new List<Vector2>();
			bool flag = GameplayerPrefs.ShowExperimentalMap();
			foreach (SystemMapData systemMapData in smd.allSystems)
			{
				list.Add(flag ? (smd.mapPosition + systemMapData.position) : systemMapData.position);
			}
			Rect boundsExpanded = VoronoiCells.ExpandedBounds(list, 2f);
			List<List<Vector2>> list2 = VoronoiCells.ComputeBoundedVoronoi(list, boundsExpanded);
			int num = 0;
			foreach (List<Vector2> list3 in list2)
			{
				Vector3[] array = new Vector3[list3.Count];
				for (int i = 0; i < list3.Count; i++)
				{
					array[i] = list3[i];
				}
				Faction faction = systems[num].content.faction;
				ConquestSystem conquestSystem = ((SystemMapData)systems[num].content).storyteller as ConquestSystem;
				LineRenderer lineRenderer = UnityEngine.Object.Instantiate<LineRenderer>(this.conquestBorderPrefab, this._sectorContent.transform);
				lineRenderer.positionCount = array.Length;
				lineRenderer.SetPositions(array);
				lineRenderer.startColor = faction.conquestColor;
				lineRenderer.endColor = faction.conquestColor;
				lineRenderer.transform.position = new Vector3(0f, 0f, -0.04f);
				lineRenderer.gameObject.SetActive(true);
				bool flag2 = conquestSystem != null && conquestSystem.headquarters;
				Material material = UnityEngine.Object.Instantiate<Material>(this.conquestAreaMaterial);
				material.color = faction.conquestColor.WithAlpha(flag2 ? 0.1f : 0.03f);
				GameObject gameObject = PolygonMeshBuilder.CreateRegionObject(list3, material, this._sectorContent.transform, "conquestArea");
				gameObject.layer = LayerMask.NameToLayer("GalaxyMap");
				gameObject.transform.SetParent(systems[num].transform);
				Collider2D component = systems[num].GetComponent<Collider2D>();
				if (component)
				{
					UnityEngine.Object.Destroy(component);
				}
				List<Vector2> list4 = new List<Vector2>();
				foreach (Vector2 vector in list3)
				{
					list4.Add(new Vector2(vector.x / systems[num].transform.localScale.x + gameObject.transform.localPosition.x, vector.y / systems[num].transform.localScale.y + gameObject.transform.localPosition.y));
				}
				PolygonCollider2D polygonCollider2D = systems[num].gameObject.AddComponent<PolygonCollider2D>();
				polygonCollider2D.pathCount = 1;
				polygonCollider2D.SetPath(0, list4);
				num++;
			}
		}

		// Token: 0x06001E98 RID: 7832
		public abstract void RefreshSystemMap(SystemMapData currentSystem);

		// Token: 0x06001E99 RID: 7833 RVA: 0x000B5EC8 File Offset: 0x000B40C8
		public static void ChangeZoomLevel(int zoomLevel)
		{
			zoomLevel = Mathf.Clamp(zoomLevel, 0, 2);
			if (!AbstractGalaxyMapManager.current)
			{
				AbstractGalaxyMapManager.LoadGalaxyMap();
				return;
			}
			if (AbstractGalaxyMapManager.current.zoomLevel == zoomLevel)
			{
				return;
			}
			if (zoomLevel == 0)
			{
				AbstractGalaxyMapManager.current.ShowSystemMap(AbstractGalaxyMapManager.current.currentSystem ?? SystemMapData.current);
				return;
			}
			if (zoomLevel == 1)
			{
				AbstractGalaxyMapManager abstractGalaxyMapManager = AbstractGalaxyMapManager.current;
				SystemMapData currentSystem = AbstractGalaxyMapManager.current.currentSystem;
				abstractGalaxyMapManager.ShowSectorMap(((currentSystem != null) ? currentSystem.sector : null) ?? SectorMapData.current);
				return;
			}
			if (zoomLevel == 2)
			{
				AbstractGalaxyMapManager.current.ShowGalaxyMap(-1);
			}
		}

		// Token: 0x06001E9A RID: 7834
		public abstract void StoreCurrentZoom();

		// Token: 0x06001E9B RID: 7835
		public abstract void StartResize();

		// Token: 0x06001E9C RID: 7836
		public abstract void SetAspect(float aspect, float zoomFactor);

		// Token: 0x06001E9D RID: 7837 RVA: 0x000B5F5D File Offset: 0x000B415D
		public static void ToggleGalaxyMap()
		{
			if (AbstractGalaxyMapManager.current)
			{
				AbstractGalaxyMapManager.UnloadGalaxyMap();
				return;
			}
			AbstractGalaxyMapManager.LoadGalaxyMap();
		}

		// Token: 0x06001E9E RID: 7838 RVA: 0x000B5F78 File Offset: 0x000B4178
		public static void UnloadGalaxyMap()
		{
			SidePanel.instance.ShowSectorMapButtons(false);
			if (AbstractGalaxyMapManager.current)
			{
				if (SceneLoader.IsSceneLoaded("GalaxyMap"))
				{
					SceneManager.UnloadSceneAsync("GalaxyMap");
				}
				if (SceneLoader.IsSceneLoaded("GalaxyMapOld"))
				{
					SceneManager.UnloadSceneAsync("GalaxyMapOld");
				}
			}
		}

		// Token: 0x06001E9F RID: 7839 RVA: 0x000B5FCA File Offset: 0x000B41CA
		public static void LoadGalaxyMap()
		{
			string sceneName = GameplayerPrefs.ShowExperimentalMap() ? "GalaxyMap" : "GalaxyMapOld";
			if (!GameplayerPrefs.ShowExperimentalMap())
			{
				SidePanel.instance.ShowSectorMapButtons(true);
			}
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}

		// Token: 0x06001EA0 RID: 7840 RVA: 0x000B5FF8 File Offset: 0x000B41F8
		public static bool IsShowing()
		{
			return SceneLoader.IsSceneLoaded("GalaxyMap") || SceneLoader.IsSceneLoaded("GalaxyMapOld");
		}

		// Token: 0x06001EA1 RID: 7841 RVA: 0x000B6012 File Offset: 0x000B4212
		public void FilterByFaction(Faction faction)
		{
			this.factionSearch.FilterByFaction(faction);
		}

		// Token: 0x06001EA2 RID: 7842 RVA: 0x000B6020 File Offset: 0x000B4220
		public WorldMapStatic[] GetWorldMapStatics()
		{
			return this._sectorContent.GetComponentsInChildren<WorldMapStatic>();
		}

		// Token: 0x04001249 RID: 4681
		public static AbstractGalaxyMapManager current;

		// Token: 0x0400124A RID: 4682
		[SerializeField]
		protected WorldMapLayer _galaxyContent;

		// Token: 0x0400124B RID: 4683
		[SerializeField]
		protected WorldMapLayer _sectorContent;

		// Token: 0x0400124C RID: 4684
		[SerializeField]
		protected WorldMapLayer _systemContent;

		// Token: 0x0400124D RID: 4685
		[SerializeField]
		protected ShipLocation shipLocationPrefab;

		// Token: 0x0400124E RID: 4686
		[SerializeField]
		protected StarLayer starLayerPrefab;

		// Token: 0x0400124F RID: 4687
		[SerializeField]
		protected SystemBackgroundComposite systemBackgroundPrefab;

		// Token: 0x04001250 RID: 4688
		[SerializeField]
		protected SectorBackgroundComposite sectorBackgroundPrefab;

		// Token: 0x04001251 RID: 4689
		[SerializeField]
		protected AbstractGalaxyBackground galaxyBackground;

		// Token: 0x04001252 RID: 4690
		[SerializeField]
		public Camera mapCamera;

		// Token: 0x04001253 RID: 4691
		[SerializeField]
		protected DrawJumpgateLine jumpgateLinePrefab;

		// Token: 0x04001254 RID: 4692
		[SerializeField]
		protected LineRenderer conquestBorderPrefab;

		// Token: 0x04001255 RID: 4693
		[SerializeField]
		protected Material conquestAreaMaterial;

		// Token: 0x04001256 RID: 4694
		[SerializeField]
		protected FactionSearch factionSearch;

		// Token: 0x04001257 RID: 4695
		public GalaxyMapWindow mapWindow;

		// Token: 0x04001258 RID: 4696
		protected ShipLocation shipLocation;

		// Token: 0x0400125B RID: 4699
		protected MapPointOfInterest focusPointOfInterest;

		// Token: 0x0400125C RID: 4700
		protected Vector2 screenSize;

		// Token: 0x0400125D RID: 4701
		protected float sectorPosMultiplier = 1.5f;

		// Token: 0x04001260 RID: 4704
		protected AbstractGalaxyMapBackground currentBackground;

		// Token: 0x04001261 RID: 4705
		protected WorldMapLayer currentWorldMapLayer;

		// Token: 0x04001262 RID: 4706
		public bool tweening;

		// Token: 0x04001263 RID: 4707
		protected float currentCamAspect;

		// Token: 0x04001264 RID: 4708
		protected bool showingConquestSector;

		// Token: 0x04001265 RID: 4709
		protected Vector2 mapSize;
	}
}
