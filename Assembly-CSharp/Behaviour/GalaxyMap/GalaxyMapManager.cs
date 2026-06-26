using System;
using System.Collections.Generic;
using Behavior.UI.GalaxyMap;
using Behaviour.UI.DebugScreen;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x0200032D RID: 813
	public class GalaxyMapManager : AbstractGalaxyMapManager
	{
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x000B672D File Offset: 0x000B492D
		public override Rect xyBounds
		{
			get
			{
				return this.currentWorldMapLayer.GetXYBounds();
			}
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x000B673A File Offset: 0x000B493A
		protected override void Awake()
		{
			base.Awake();
			this.cameraMovement = this.mapCamera.GetComponent<GalaxyMapCameraMovement>();
			this.zoomBeforeDrag = this.mapCamera.orthographicSize;
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000B6764 File Offset: 0x000B4964
		protected override void InitGalaxyMap()
		{
			this.mapSize = this.GetMapSize();
			float num = this.mapCamera.orthographicSize * 2f;
			this.screenSize = new Vector2(this.mapCamera.aspect * num, num);
			this.currentBackground = null;
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000B67B0 File Offset: 0x000B49B0
		protected override void Update()
		{
			base.Update();
			if (Input.GetKeyUp(KeyCode.F))
			{
				bool follow = !this.cameraMovement.followTarget;
				this.cameraMovement.SetTarget(this.shipLocation.gameObject, follow);
			}
			this.currentLocationTimer -= Time.deltaTime;
			if (this.currentLocationTimer < 0f)
			{
				this.currentLocationTimer = 0.1f;
				if (base.zoomLevel != 2)
				{
					WorldMapElement worldMapElement = this.CheckCurrentLocation();
					if (worldMapElement != null)
					{
						WorldMapStatic worldMapStatic = worldMapElement as WorldMapStatic;
						if (worldMapStatic != null)
						{
							SystemMapData systemMapData = worldMapStatic.content as SystemMapData;
							if (systemMapData != null)
							{
								this.SetCurrentSystem(systemMapData);
							}
							else if (worldMapStatic.content is SectorMapData)
							{
								this.SetCurrentSystem(worldMapStatic.content.system);
							}
						}
						else
						{
							WorldMapPOI worldMapPOI = worldMapElement as WorldMapPOI;
							if (worldMapPOI != null)
							{
								this.SetCurrentSystem(worldMapPOI.content.system);
							}
						}
					}
				}
				if (base.zoomLevel == 0)
				{
					this.DrawVisibleSystems();
				}
			}
			this.SetShipLocation();
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000B68AC File Offset: 0x000B4AAC
		private void SetCurrentSystem(SystemMapData system)
		{
			base.currentSystem = system;
			base.currentSector = ((system != null) ? system.sector : null);
			this.mapWidget.ShowSystem(system, base.zoomLevel);
			this.mapWidget.ShowSector((system != null) ? system.sector : null, base.zoomLevel);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000B6904 File Offset: 0x000B4B04
		private WorldMapElement CheckCurrentLocation()
		{
			Vector3 position = this.mapCamera.transform.position;
			this.physicsResults = new List<Collider2D>();
			Physics2D.OverlapCircle(position, 10f, ContactFilter2D.noFilter, this.physicsResults);
			CircleCollider2D circleCollider2D = null;
			float num = float.MaxValue;
			foreach (Collider2D collider2D in this.physicsResults)
			{
				if (collider2D is CircleCollider2D)
				{
					float num2 = Vector2.SqrMagnitude(collider2D.transform.position - position);
					if (num2 < num)
					{
						num = num2;
						circleCollider2D = (CircleCollider2D)collider2D;
					}
				}
			}
			WorldMapElement result = null;
			if (circleCollider2D)
			{
				result = circleCollider2D.GetComponent<WorldMapElement>();
			}
			return result;
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000B69DC File Offset: 0x000B4BDC
		public override void StartResize()
		{
			this.zoomBeforeDrag = this.mapCamera.orthographicSize / this.zoomFactor;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000B69F6 File Offset: 0x000B4BF6
		public override void SetAspect(float aspect, float zoomFactor)
		{
			this.zoomFactor = zoomFactor;
			this.mapCamera.aspect = aspect;
			this.mapCamera.orthographicSize = zoomFactor * this.zoomBeforeDrag;
			this.cameraMovement.maxZoom = zoomFactor * this.GetMaxZoom();
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000B6A34 File Offset: 0x000B4C34
		private float GetMaxZoom()
		{
			float result;
			switch (base.zoomLevel)
			{
			case 0:
				result = this.maxZoomSystem;
				break;
			case 1:
				result = this.maxZoomSector;
				break;
			case 2:
				result = this.maxZoomGalaxy;
				break;
			default:
				result = this.maxZoomSystem;
				break;
			}
			return result;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000B6A80 File Offset: 0x000B4C80
		protected override void SetMapLocation()
		{
			if (this.focusPointOfInterest == null)
			{
				this.ShowSystemMap(SystemMapData.current);
				return;
			}
			base.currentSystem = this.focusPointOfInterest.system;
			base.currentSector = this.focusPointOfInterest.system.sector;
			if (!this.focusPointOfInterest.system.sector.IsUnlocked())
			{
				this.ShowGalaxyMap(this.focusPointOfInterest.system.sector.quadrant);
				return;
			}
			if (!this.focusPointOfInterest.system.isUnlocked)
			{
				this.ShowSectorMap(this.focusPointOfInterest.system.sector);
				return;
			}
			this.ShowSystemMap(this.focusPointOfInterest.system);
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000B6B38 File Offset: 0x000B4D38
		private Vector2 GetMapSize()
		{
			float num = 2f;
			return new Vector2((38f + num) * 2f, (6f + num) * 2f);
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000B6B6C File Offset: 0x000B4D6C
		protected override void SetShipLocation()
		{
			Quaternion rotation = Quaternion.identity;
			Vector2 vector = this.shipLocation ? this.shipLocation.transform.position : Vector2.zero;
			Vector2 vector2;
			if (base.zoomLevel == 0)
			{
				rotation = GameplayManager.Instance.spaceShip.transform.rotation;
				vector2 = SystemMapData.current.mapPosition + GamePlayer.current.mapPosition;
			}
			else if (base.zoomLevel == 1)
			{
				vector2 = SectorMapData.current.mapPosition + SystemMapData.current.position;
			}
			else
			{
				vector2 = SectorMapData.current.mapPositionGalaxy + SectorMapData.current.position * this.sectorPosMultiplier;
			}
			if (!this.shipLocation)
			{
				base.CreateShipLocation(base.transform, vector2);
			}
			if (this.shipLocation)
			{
				this.shipLocation.transform.position = vector2;
				this.shipLocation.transform.rotation = rotation;
			}
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000B6C7C File Offset: 0x000B4E7C
		public override void ShowGalaxyMap(int quadrant = -1)
		{
			if (quadrant < 0)
			{
				quadrant = base.currentQuadrant;
			}
			else
			{
				base.currentQuadrant = quadrant;
			}
			base.zoomLevel = 2;
			this.RefreshGalaxyMap(quadrant);
			this.mapCamera.transform.position = this.GetCameraPosition(base.currentSector.mapPositionGalaxy);
			this.showingConquestSector = false;
			this.mapWidget.ShowMapContent(null);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000B6CE0 File Offset: 0x000B4EE0
		public override void ShowSectorMap(SectorMapData smd)
		{
			bool flag = ConsoleScreen.DebugModifier();
			if (!smd.IsUnlocked() && !flag)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@GalaxyMapSystemLocked", Array.Empty<object>())).WithColor(ColorHelper.reddish).Show();
				return;
			}
			base.zoomLevel = 1;
			this.RefreshSectorMap(smd);
			this.mapCamera.transform.position = this.GetCameraPosition(base.currentSector.mapPosition);
			this.showingConquestSector = smd.conquestSector;
			this.mapWidget.ShowMapContent(smd);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000B6D70 File Offset: 0x000B4F70
		public override void ShowSystemMap(SystemMapData systemMapData)
		{
			bool flag = ConsoleScreen.DebugModifier();
			if (GamePlayer.current.currentSystem != systemMapData && !systemMapData.jumpgateOpen && !flag)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@GalaxyMapSystemLocked", Array.Empty<object>())).WithColor(ColorHelper.reddish).Show();
				return;
			}
			base.zoomLevel = 0;
			this.RefreshSystemMap(systemMapData);
			this.mapCamera.transform.position = this.GetCameraPosition(systemMapData.mapPosition);
			this.showingConquestSector = false;
			this.mapWidget.ShowMapContent(systemMapData);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000B6E04 File Offset: 0x000B5004
		private Vector3 GetCameraPosition(Vector2 position)
		{
			return new Vector3(position.x, position.y, -20f);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000B6E1C File Offset: 0x000B501C
		public override void RefreshGalaxyMap(int quadrant)
		{
			this._galaxyContent.ClearContent();
			this.cameraMovement.maxZoom = this.maxZoomGalaxy;
			if (!this.SetCurrentZoom() && this.mapCamera.orthographicSize > this.maxZoomGalaxy)
			{
				this.mapCamera.orthographicSize = this.maxZoomGalaxy;
			}
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				Vector3 v = sectorMapData.mapPositionGalaxy;
				WorldMapStatic worldMapStatic = UnityEngine.Object.Instantiate<WorldMapStatic>(WorldMapStatic.GetPrefab("Sector"), this._galaxyContent.transform);
				worldMapStatic.transform.position = v + (Vector3)(sectorMapData.position * this.sectorPosMultiplier);
				worldMapStatic.SetContent(sectorMapData);
				this._galaxyContent.AddSpriteRenderers(worldMapStatic.GetSpriteRenderers());
				this._galaxyContent.CheckPosition(worldMapStatic.transform.position);
				foreach (SystemMapData systemMapData in sectorMapData.allSystems)
				{
					foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
					{
						if (!mapPointOfInterest.hidden)
						{
							JumpGate jumpGate = mapPointOfInterest as JumpGate;
							if (jumpGate != null && jumpGate.sectorJumpgate && jumpGate.targetSystemGuid != null)
							{
								SystemMapData system = GalaxyMapData.current.GetSystem(jumpGate.targetSystemGuid);
								if (system.level <= systemMapData.level || system.sector.quadrant != systemMapData.sector.quadrant)
								{
									DrawJumpgateLine drawJumpgateLine = UnityEngine.Object.Instantiate<DrawJumpgateLine>(this.jumpgateLinePrefab, this._galaxyContent.transform);
									drawJumpgateLine.SetMouseInteraction(false);
									Vector2 objB = system.sector.mapPositionGalaxy + system.sector.position * this.sectorPosMultiplier;
									drawJumpgateLine.SetPositions(worldMapStatic.transform.position, objB, jumpGate.canUseJumpGate ? (jumpGate.sectorLine ? ColorHelper.discBlueLight2 : ColorHelper.greenish) : ColorHelper.reddish, this.cameraMovement, 0.3f);
									this._galaxyContent.AddLineRenderer(drawJumpgateLine.lineRenderer);
								}
							}
						}
					}
				}
				SectorMapData sectorMapData2 = sectorMapData;
				MapPointOfInterest focusPointOfInterest = this.focusPointOfInterest;
				object obj;
				if (focusPointOfInterest == null)
				{
					obj = null;
				}
				else
				{
					SystemMapData system2 = focusPointOfInterest.system;
					obj = ((system2 != null) ? system2.sector : null);
				}
				if (sectorMapData2 == obj)
				{
					worldMapStatic.highlightMouse = true;
				}
			}
			this.SwitchToMapLayer(this._galaxyContent);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000B7130 File Offset: 0x000B5330
		public override void RefreshSectorMap(SectorMapData currentSector)
		{
			this._sectorContent.ClearContent();
			base.currentSector = currentSector;
			this.cameraMovement.maxZoom = this.maxZoomSector;
			this.drawnJumpGateLines = new List<string>();
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				Vector2 vector = Vector2.zero;
				List<WorldMapStatic> list = new List<WorldMapStatic>();
				Vector2 mapPosition = sectorMapData.mapPosition;
				this._sectorContent.CheckPosition(sectorMapData.mapPosition);
				MapElementBackground mapElementBackground = null;
				if (!sectorMapData.conquestSector)
				{
					mapElementBackground = UnityEngine.Object.Instantiate<MapElementBackground>(this.mapElementBackgroundPrefab, this._sectorContent.transform);
					mapElementBackground.transform.localPosition = new Vector3(mapPosition.x, mapPosition.y, 1f);
					mapElementBackground.SetLabel(sectorMapData.name);
				}
				foreach (SystemMapData systemMapData in sectorMapData.allSystems)
				{
					WorldMapStatic worldMapStatic = UnityEngine.Object.Instantiate<WorldMapStatic>(systemMapData.prefab, this._sectorContent.transform);
					worldMapStatic.transform.localPosition = mapPosition + systemMapData.position;
					vector = this.StretchMinMax(systemMapData.position, vector);
					worldMapStatic.SetContent(systemMapData);
					this._sectorContent.AddSpriteRenderers(worldMapStatic.GetSpriteRenderers());
					list.Add(worldMapStatic);
					foreach (MapPointOfInterest poi in systemMapData.pointsOfInterest)
					{
						this.DrawJumpGateLine(poi, worldMapStatic, this._sectorContent, true);
					}
					SystemMapData systemMapData2 = systemMapData;
					MapPointOfInterest focusPointOfInterest = this.focusPointOfInterest;
					if (systemMapData2 == ((focusPointOfInterest != null) ? focusPointOfInterest.system : null))
					{
						worldMapStatic.highlightMouse = true;
					}
				}
				if (sectorMapData.conquestSector)
				{
					base.DrawConquestPolygons(sectorMapData, list);
				}
				else
				{
					mapElementBackground.SetSize(vector);
					if (sectorMapData.faction != null)
					{
						mapElementBackground.SetColor(sectorMapData.faction.conquestColor);
					}
				}
				if (sectorMapData == currentSector && !this.SetCurrentZoom())
				{
					this.UpdateCameraZoom(vector);
				}
			}
			this.SwitchToMapLayer(this._sectorContent);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000B73B8 File Offset: 0x000B55B8
		private void DrawJumpGateLine(MapPointOfInterest poi, WorldMapElement fromObject, WorldMapLayer parent, bool sector = true)
		{
			if (poi.hidden)
			{
				return;
			}
			JumpGate jumpGate = poi as JumpGate;
			if (jumpGate != null && jumpGate.targetSystemGuid != null)
			{
				SystemMapData system = GalaxyMapData.current.GetSystem(jumpGate.targetSystemGuid);
				if (this.drawnJumpGateLines.Contains(poi.system.name + system.name))
				{
					return;
				}
				if (jumpGate is EmbassyJumpgate || jumpGate.GetTargetPOI() is EmbassyJumpgate)
				{
					return;
				}
				DrawJumpgateLine drawJumpgateLine = UnityEngine.Object.Instantiate<DrawJumpgateLine>(this.jumpgateLinePrefab, parent.transform);
				Vector2 objB = sector ? (system.sector.mapPosition + system.position) : (system.mapPosition + jumpGate.GetTargetPOI().position);
				drawJumpgateLine.SetMouseInteraction(!sector || jumpGate.sectorLine);
				drawJumpgateLine.SetPositions(fromObject.transform.position, objB, jumpGate.canUseJumpGate ? (jumpGate.sectorLine ? ColorHelper.discBlueLight2 : ColorHelper.greenish) : ColorHelper.reddish, this.cameraMovement, (sector && !jumpGate.sectorLine) ? 0.5f : 1f);
				drawJumpgateLine.nameA = (sector ? poi.system.sector.name : poi.system.name);
				drawJumpgateLine.nameB = (sector ? system.sector.name : system.name);
				drawJumpgateLine.jumpToA = (sector ? poi.system.sector.mapPosition : poi.system.mapPosition);
				drawJumpgateLine.jumpToB = (sector ? system.sector.mapPosition : system.mapPosition);
				parent.AddLineRenderer(drawJumpgateLine.lineRenderer);
				this.drawnJumpGateLines.Add(poi.system.name + system.name);
				this.drawnJumpGateLines.Add(system.name + poi.system.name);
			}
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x000B75B8 File Offset: 0x000B57B8
		public override void RefreshSystemMap(SystemMapData currentSystem)
		{
			this._systemContent.ClearContent();
			base.currentSystem = currentSystem;
			this.cameraMovement.maxZoom = this.maxZoomSystem;
			if (base.currentSector == null)
			{
				base.currentSector = currentSystem.sector;
			}
			this.drawnJumpGateLines = new List<string>();
			this.drawnSystems = new List<SystemMapData>();
			this.DrawVisibleSystems();
			this.SwitchToMapLayer(this._systemContent);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000B7624 File Offset: 0x000B5824
		private void DrawVisibleSystems()
		{
			foreach (SystemMapData systemMapData in GalaxyMapData.current.allSystems)
			{
				this.DrawSystem(systemMapData);
				this._systemContent.CheckPosition(systemMapData.mapPosition);
			}
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000B7688 File Offset: 0x000B5888
		private void DrawSystem(SystemMapData system)
		{
			if (this.drawnSystems.Contains(system))
			{
				return;
			}
			if (Vector2.Distance(system.mapPosition, this.mapCamera.transform.position) > this.mapCamera.orthographicSize * 3f)
			{
				return;
			}
			Vector3 vector = system.mapPosition;
			MapElementBackground mapElementBackground = UnityEngine.Object.Instantiate<MapElementBackground>(this.mapElementBackgroundPrefab, this._systemContent.transform);
			mapElementBackground.transform.localPosition = new Vector3(vector.x, vector.y, 1f);
			mapElementBackground.SetLabel(system.name);
			Vector2 vector2 = new Vector2(10f, 5f);
			foreach (MapStatic mapStatic in system.statics)
			{
				WorldMapStatic worldMapStatic = UnityEngine.Object.Instantiate<WorldMapStatic>(mapStatic.Prefab, this._systemContent.transform);
				worldMapStatic.transform.localPosition = vector + (Vector3)mapStatic.position;
				worldMapStatic.SetContent(mapStatic);
				this._systemContent.AddSpriteRenderers(worldMapStatic.GetSpriteRenderers());
			}
			if (system.isUnlocked)
			{
				foreach (MapPointOfInterest mapPointOfInterest in system.pointsOfInterest)
				{
					if (!mapPointOfInterest.hidden)
					{
						WorldMapPOI worldMapPOI = UnityEngine.Object.Instantiate<WorldMapPOI>(mapPointOfInterest.Prefab, this._systemContent.transform);
						worldMapPOI.highlightMouse = (mapPointOfInterest == this.focusPointOfInterest);
						worldMapPOI.transform.localPosition = vector + new Vector3(mapPointOfInterest.position.x, mapPointOfInterest.position.y, -1f);
						vector2 = this.StretchMinMax(mapPointOfInterest.position, vector2);
						worldMapPOI.SetContent(mapPointOfInterest);
						JumpGate jumpGate = mapPointOfInterest as JumpGate;
						if (jumpGate != null)
						{
							if (!jumpGate.canUseJumpGate)
							{
								worldMapPOI.spriteRenderer.color = ColorHelper.reddish;
							}
							else
							{
								worldMapPOI.spriteRenderer.color = Color.white;
							}
							this.DrawJumpGateLine(mapPointOfInterest, worldMapPOI, this._systemContent, false);
						}
						this._systemContent.AddSpriteRenderers(worldMapPOI.GetSpriteRenderers());
					}
				}
				foreach (Mission mission in GamePlayer.current.allMissions)
				{
					MapPointOfInterest dynamicPointOfInterest = mission.dynamicPointOfInterest;
					if (((dynamicPointOfInterest != null) ? dynamicPointOfInterest.system : null) == system && dynamicPointOfInterest.system == system)
					{
						WorldMapPOI worldMapPOI2 = UnityEngine.Object.Instantiate<WorldMapPOI>(dynamicPointOfInterest.Prefab, this._systemContent.transform);
						worldMapPOI2.transform.localPosition = vector + new Vector3(dynamicPointOfInterest.position.x, dynamicPointOfInterest.position.y, -1f);
						vector2 = this.StretchMinMax(dynamicPointOfInterest.position, vector2);
						worldMapPOI2.SetContent(dynamicPointOfInterest);
						worldMapPOI2.SetMissionPoi();
						worldMapPOI2.highlightMouse = (dynamicPointOfInterest == this.focusPointOfInterest);
						this._systemContent.AddSpriteRenderers(worldMapPOI2.GetSpriteRenderers());
					}
				}
			}
			mapElementBackground.SetSize(vector2);
			mapElementBackground.SetColor(system.GetStar().GetColor());
			if (base.currentSystem == system && !this.SetCurrentZoom())
			{
				this.UpdateCameraZoom(vector2);
			}
			this.drawnSystems.Add(system);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000B7A34 File Offset: 0x000B5C34
		private void UpdateCameraZoom(Vector2 maxPositions)
		{
			if (maxPositions.x / maxPositions.y > this.mapCamera.aspect)
			{
				this.mapCamera.orthographicSize = maxPositions.x * 1.2f;
				return;
			}
			this.mapCamera.orthographicSize = maxPositions.y * 2f;
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000B7A8C File Offset: 0x000B5C8C
		private Vector2 StretchMinMax(Vector2 position, Vector2 minMax)
		{
			if (Mathf.Abs(position.x) > minMax.x)
			{
				minMax.x = Mathf.Abs(position.x);
			}
			if (Mathf.Abs(position.y) > minMax.y)
			{
				minMax.y = Mathf.Abs(position.y);
			}
			return minMax;
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000B7AE4 File Offset: 0x000B5CE4
		private void SwitchToMapLayer(WorldMapLayer newLayer)
		{
			if (this.currentWorldMapLayer)
			{
				this.currentWorldMapLayer.ClearContent();
				this.currentWorldMapLayer.gameObject.SetActive(false);
			}
			this.currentWorldMapLayer = newLayer;
			this.currentWorldMapLayer.gameObject.SetActive(true);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000B7B34 File Offset: 0x000B5D34
		public override void StoreCurrentZoom()
		{
			switch (base.zoomLevel)
			{
			case 0:
				GameplayerPrefs.SetSystemZoomLevel(this.mapCamera.orthographicSize);
				return;
			case 1:
				GameplayerPrefs.SetSectorZoomLevel(this.mapCamera.orthographicSize);
				return;
			case 2:
				GameplayerPrefs.SetGalaxyZoomLevel(this.mapCamera.orthographicSize);
				return;
			default:
				return;
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000B7B90 File Offset: 0x000B5D90
		public bool SetCurrentZoom()
		{
			float num = -1f;
			switch (base.zoomLevel)
			{
			case 0:
				num = GameplayerPrefs.GetSystemZoomLevel();
				break;
			case 1:
				num = GameplayerPrefs.GetSectorZoomLevel();
				break;
			case 2:
				num = GameplayerPrefs.GetGalaxyZoomLevel();
				break;
			}
			if (num > 0f)
			{
				this.mapCamera.orthographicSize = num;
				return true;
			}
			return false;
		}

		// Token: 0x04001279 RID: 4729
		[SerializeField]
		private float maxZoomGalaxy = 20f;

		// Token: 0x0400127A RID: 4730
		[SerializeField]
		private float maxZoomSector = 30f;

		// Token: 0x0400127B RID: 4731
		[SerializeField]
		private float maxZoomSystem = 30f;

		// Token: 0x0400127C RID: 4732
		[SerializeField]
		private MapElementBackground mapElementBackgroundPrefab;

		// Token: 0x0400127D RID: 4733
		[SerializeField]
		private MapWidget mapWidget;

		// Token: 0x0400127E RID: 4734
		private GalaxyMapCameraMovement cameraMovement;

		// Token: 0x0400127F RID: 4735
		private float currentLocationTimer;

		// Token: 0x04001280 RID: 4736
		private float zoomBeforeDrag;

		// Token: 0x04001281 RID: 4737
		private float zoomFactor;

		// Token: 0x04001282 RID: 4738
		private List<string> drawnJumpGateLines = new List<string>();

		// Token: 0x04001283 RID: 4739
		private List<SystemMapData> drawnSystems = new List<SystemMapData>();

		// Token: 0x04001284 RID: 4740
		private List<Collider2D> physicsResults;
	}
}
