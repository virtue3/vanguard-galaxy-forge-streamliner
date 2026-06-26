using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Background;
using Behaviour.Managers;
using Behaviour.Transparency;
using Behaviour.UI;
using Behaviour.UI.DebugScreen;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;
using _Scripts.Behaviour.Background;

namespace Behaviour.GalaxyMap
{
	// Token: 0x0200032E RID: 814
	public class GalaxyMapManagerOld : AbstractGalaxyMapManager
	{
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000B7C2A File Offset: 0x000B5E2A
		// (set) Token: 0x06001EDA RID: 7898 RVA: 0x000B7C32 File Offset: 0x000B5E32
		public SystemBackgroundComposite systemBackground { get; private set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x000B7C3B File Offset: 0x000B5E3B
		// (set) Token: 0x06001EDC RID: 7900 RVA: 0x000B7C43 File Offset: 0x000B5E43
		public SectorBackgroundComposite sectorBackground { get; private set; }

		// Token: 0x06001EDD RID: 7901 RVA: 0x000B7C4C File Offset: 0x000B5E4C
		protected override void InitGalaxyMap()
		{
			this.mapSize = this.GetMapSize();
			this.systemBackground = UnityEngine.Object.Instantiate<SystemBackgroundComposite>(this.systemBackgroundPrefab, base.transform);
			this.sectorBackground = UnityEngine.Object.Instantiate<SectorBackgroundComposite>(this.sectorBackgroundPrefab, base.transform);
			float num = this.mapCamera.orthographicSize * 2f;
			this.screenSize = new Vector2(this.mapCamera.aspect * num, num);
			this.systemBackground.SetScreenSize(this.mapSize, Vector2.zero);
			this.sectorBackground.SetScreenSize(this.mapSize, Vector2.zero);
			this.currentBackground = null;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000B7CF4 File Offset: 0x000B5EF4
		protected override void Update()
		{
			base.Update();
			float y = Input.mouseScrollDelta.y;
			if (y < 0f)
			{
				if (base.zoomLevel < 2)
				{
					if (base.zoomLevel == 0)
					{
						SystemMapData currentSystem = base.currentSystem;
						this.ShowSectorMap(((currentSystem != null) ? currentSystem.sector : null) ?? SectorMapData.current);
					}
					else
					{
						this.ShowGalaxyMap(-1);
					}
				}
				else if (base.currentQuadrant > SectorMapData.quadrantPrologue && GamePlayer.current.HasAccessToQuadrant(SectorMapData.quadrantConquest))
				{
					this.ShowGalaxyMap((base.currentQuadrant == SectorMapData.quadrantFrontier) ? SectorMapData.quadrantConquest : SectorMapData.quadrantFrontier);
				}
			}
			if (y > 0f && base.zoomLevel > 0)
			{
				GameObject mouseOverGameObject = UIHelper.GetMouseOverGameObject(this.mapCamera);
				WorldMapStatic worldMapStatic = (mouseOverGameObject != null) ? mouseOverGameObject.GetComponent<WorldMapStatic>() : null;
				if (base.zoomLevel == 2)
				{
					if (worldMapStatic)
					{
						SectorMapData sectorMapData = worldMapStatic.content as SectorMapData;
						if (sectorMapData != null)
						{
							this.ShowSectorMap(sectorMapData);
							goto IL_141;
						}
					}
					SystemMapData currentSystem2 = base.currentSystem;
					this.ShowSectorMap(((currentSystem2 != null) ? currentSystem2.sector : null) ?? SectorMapData.current);
				}
				else
				{
					if (worldMapStatic)
					{
						SystemMapData systemMapData = worldMapStatic.content as SystemMapData;
						if (systemMapData != null)
						{
							this.ShowSystemMap(systemMapData);
							goto IL_141;
						}
					}
					this.ShowSystemMap(base.currentSystem ?? SystemMapData.current);
				}
			}
			IL_141:
			this.SetShipLocation();
			if (!this.mapCamera.aspect.ApproximatelyEqual(this.currentCamAspect))
			{
				this.currentCamAspect = this.mapCamera.aspect;
				this.AspectChanged();
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x000B7E78 File Offset: 0x000B6078
		protected override void SetMapLocation()
		{
			if (this.focusPointOfInterest == null)
			{
				this.ShowSystemMap(SystemMapData.current);
				return;
			}
			base.currentSystem = this.focusPointOfInterest.system;
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

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000B7F18 File Offset: 0x000B6118
		private void AspectChanged()
		{
			float num = this.showingConquestSector ? (this.mapSize.y * 3f) : this.mapSize.y;
			float num2 = this.mapSize.x / num;
			float num3 = this.mapSize.x / this.currentCamAspect / 2f;
			if (num2 < this.currentCamAspect)
			{
				num3 = num / 2f;
			}
			this.mapCamera.orthographicSize = num3 * 1.05f;
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x000B7F94 File Offset: 0x000B6194
		private Vector2 GetMapSize()
		{
			float num = 2f;
			return new Vector2((38f + num) * 2f, (6f + num) * 2f);
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x000B7FC8 File Offset: 0x000B61C8
		protected override void SetShipLocation()
		{
			if (this.tweening)
			{
				if (this.shipLocation)
				{
					UnityEngine.Object.Destroy(this.shipLocation.gameObject);
				}
				return;
			}
			Transform transform = null;
			Quaternion rotation = Quaternion.identity;
			Vector2 vector = Vector2.zero;
			if (base.zoomLevel == 0)
			{
				if (base.currentSystem == GamePlayer.current.currentSystem)
				{
					transform = this._systemContent.transform;
					rotation = GameplayManager.Instance.spaceShip.transform.rotation;
					vector = GamePlayer.current.mapPosition;
				}
			}
			else if (base.zoomLevel == 1)
			{
				if (base.currentSector == GamePlayer.current.currentSystem.sector)
				{
					transform = this._sectorContent.transform;
					vector = SystemMapData.current.position;
				}
			}
			else if (base.currentQuadrant == GamePlayer.current.currentSystem.sector.quadrant)
			{
				transform = this._galaxyContent.transform;
				vector = SectorMapData.current.position;
			}
			if (transform)
			{
				if (!this.shipLocation && !Singleton<TravelManager>.Instance.usingJumpgate)
				{
					base.CreateShipLocation(transform, vector);
				}
				if (this.shipLocation)
				{
					this.shipLocation.transform.position = vector;
					this.shipLocation.transform.rotation = rotation;
					return;
				}
			}
			else if (this.shipLocation)
			{
				UnityEngine.Object.Destroy(this.shipLocation.gameObject);
			}
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000B813C File Offset: 0x000B633C
		public override void ShowGalaxyMap(int quadrant = -1)
		{
			if (this.tweening)
			{
				return;
			}
			if (quadrant < 0)
			{
				quadrant = base.currentQuadrant;
			}
			else
			{
				base.currentQuadrant = quadrant;
			}
			bool flag = base.zoomLevel != 2;
			base.zoomLevel = 2;
			this.RefreshGalaxyMap(quadrant);
			if (flag)
			{
				WorldMapLayer galaxyContent = this._galaxyContent;
				AbstractGalaxyMapBackground galaxyBackground = this.galaxyBackground;
				bool zoomIn = false;
				SectorMapData currentSector = base.currentSector;
				this.tweenCoroutine = base.StartCoroutine(this.TweenNewBackground(galaxyContent, galaxyBackground, zoomIn, (currentSector != null) ? (-currentSector.position) : Vector2.zero, 3f, 1f, base.currentQuadrant == SectorMapData.quadrantConquest));
			}
			this.showingConquestSector = false;
			SidePanel.instance.ShowMapContent(null);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000B81E8 File Offset: 0x000B63E8
		public override void ShowSectorMap(SectorMapData smd)
		{
			bool flag = ConsoleScreen.DebugModifier();
			if (this.tweening)
			{
				return;
			}
			if (!smd.IsUnlocked() && !flag)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@GalaxyMapSystemLocked", Array.Empty<object>())).WithColor(ColorHelper.reddish).Show();
				return;
			}
			bool flag2 = base.zoomLevel == 2;
			Vector2 zoomPosition = flag2 ? smd.position : (-base.currentSystem.position);
			base.zoomLevel = 1;
			this.sectorBackground.gameObject.SetActive(true);
			this.sectorBackground.LoadBackgroundData(smd, false);
			this.sectorBackground.SetLayer(base.gameObject.layer);
			this.RefreshSectorMap(smd);
			this.showingConquestSector = smd.conquestSector;
			this.tweenCoroutine = base.StartCoroutine(this.TweenNewBackground(this._sectorContent, this.sectorBackground, flag2, zoomPosition, (float)(flag2 ? 15 : 1), flag2 ? 8.5f : 1f, smd.conquestSector));
			SidePanel.instance.ShowMapContent(smd);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x000B82F8 File Offset: 0x000B64F8
		public override void ShowSystemMap(SystemMapData systemMapData)
		{
			bool flag = ConsoleScreen.DebugModifier();
			if (this.tweening)
			{
				return;
			}
			if (GamePlayer.current.currentSystem != systemMapData && !systemMapData.jumpgateOpen && !flag)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@GalaxyMapSystemLocked", Array.Empty<object>())).WithColor(ColorHelper.reddish).Show();
				return;
			}
			base.zoomLevel = 0;
			this.systemBackground.gameObject.SetActive(true);
			this.systemBackground.LoadBackgroundData(systemMapData, true);
			this.systemBackground.SetLayer(base.gameObject.layer);
			this.RefreshSystemMap(systemMapData);
			this.tweenCoroutine = base.StartCoroutine(this.TweenNewBackground(this._systemContent, this.systemBackground, true, systemMapData.position, 2f, 2f, systemMapData.sector.conquestSector));
			this.showingConquestSector = false;
			SidePanel.instance.ShowMapContent(systemMapData);
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000B83E8 File Offset: 0x000B65E8
		private IEnumerator TweenNewBackground(WorldMapLayer newLayer, AbstractGalaxyMapBackground newBackground, bool zoomIn, Vector2 zoomPosition, float zoomFactor = 1f, float distanceFactor = 2f, bool fullScreenMap = false)
		{
			float cameraHeight = ScreenSettings.nonStackingScreenFactor;
			if (fullScreenMap)
			{
				cameraHeight = 1f;
			}
			SidePanel.instance.SetMapWidgetPosition(fullScreenMap);
			Rect mapRect = this.mapCamera.rect;
			if (this.currentBackground == null)
			{
				this.currentBackground = newBackground;
				this.currentWorldMapLayer = newLayer;
				this.currentWorldMapLayer.gameObject.SetActive(true);
				this.currentBackground.gameObject.SetActive(true);
				mapRect.height = cameraHeight;
				yield break;
			}
			this.tweening = true;
			newLayer.gameObject.SetActive(true);
			zoomPosition *= distanceFactor;
			newLayer.transform.localScale = Vector3.one;
			this.currentWorldMapLayer.transform.localScale = Vector3.one;
			newBackground.gameObject.SetActive(true);
			this.currentBackground.SetBackgroundAlpha(1f);
			newBackground.SetBackgroundAlpha(0f);
			newLayer.SetAlpha(0f);
			float totalTime = 0f;
			float alphaDiffPerTick = 2f / this.maxTweenTime * this.tweenDeltaTime;
			float zoomLevel = 0.5f;
			float zoomDiffPerTick = zoomLevel * zoomFactor / this.maxTweenTime * this.tweenDeltaTime;
			float alphaDiffIn = 0f;
			float alphaDiffOut = 0f;
			float zoomDiff = 0f;
			float num = Vector2.Distance(this.currentWorldMapLayer.transform.position, -zoomPosition);
			float distancePerSecond = num / this.maxTweenTime;
			float cameraDiffPerSecond = (cameraHeight - mapRect.height) / this.maxTweenTime;
			while (totalTime < this.maxTweenTime)
			{
				if (totalTime > this.maxTweenTime * 0.5f)
				{
					alphaDiffIn += alphaDiffPerTick;
					newBackground.SetBackgroundAlpha(0f + alphaDiffIn);
					newLayer.SetAlpha(0f + alphaDiffIn);
				}
				else if (totalTime < this.maxTweenTime * 0.5f)
				{
					alphaDiffOut += alphaDiffPerTick;
					this.currentBackground.SetBackgroundAlpha(1f - alphaDiffOut);
					this.currentWorldMapLayer.SetAlpha(1f - alphaDiffOut);
				}
				zoomDiff += zoomDiffPerTick;
				mapRect.height += cameraDiffPerSecond * this.tweenDeltaTime;
				this.mapCamera.rect = mapRect;
				this.currentWorldMapLayer.transform.localScale = (zoomIn ? (Vector3.one + new Vector3(zoomDiff, zoomDiff, 0f)) : (Vector3.one * zoomLevel * zoomFactor - new Vector3(zoomDiff, zoomDiff, 0f)));
				totalTime += this.tweenDeltaTime;
				Vector3 position = Vector2.MoveTowards(this.currentWorldMapLayer.transform.position, -zoomPosition, distancePerSecond * this.tweenDeltaTime);
				this.currentWorldMapLayer.transform.position = position;
				yield return new WaitForSeconds(this.tweenDeltaTime);
			}
			newLayer.transform.localScale = Vector3.one;
			this.currentWorldMapLayer.transform.localScale = Vector3.one;
			this.currentWorldMapLayer.transform.position = Vector2.zero;
			mapRect.height = cameraHeight;
			this.mapCamera.rect = mapRect;
			this.currentBackground.gameObject.SetActive(false);
			this.currentBackground = newBackground;
			this.currentWorldMapLayer.gameObject.SetActive(false);
			this.currentWorldMapLayer = newLayer;
			this.tweening = false;
			yield break;
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000B8438 File Offset: 0x000B6638
		public override void RefreshGalaxyMap(int quadrant)
		{
			this._galaxyContent.ClearContent();
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				if (sectorMapData.quadrant == quadrant)
				{
					float d = (quadrant == SectorMapData.quadrantConquest) ? 1.25f : 1f;
					WorldMapStatic worldMapStatic = UnityEngine.Object.Instantiate<WorldMapStatic>(WorldMapStatic.GetPrefab("Sector"), this._galaxyContent.transform);
					worldMapStatic.transform.position = sectorMapData.position * d;
					worldMapStatic.SetContent(sectorMapData);
					this._galaxyContent.AddSpriteRenderers(worldMapStatic.GetSpriteRenderers());
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
										Vector2 position = system.sector.position;
										if (system.sector.quadrant > systemMapData.sector.quadrant)
										{
											position.x += 100f;
										}
										else if (system.sector.quadrant < systemMapData.sector.quadrant)
										{
											position.x -= 100f;
										}
										drawJumpgateLine.SetPositions(worldMapStatic.transform.position, position * d, jumpGate.canUseJumpGate ? ColorHelper.greenish : ColorHelper.reddish, null, 0.3f);
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
					if (quadrant == SectorMapData.quadrantConquest)
					{
						float num = sectorMapData.conquestSector ? 0.5f : 0f;
						worldMapStatic.transform.localScale = new Vector3(1f + num, 1f + num, 1f);
					}
				}
			}
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x000B8754 File Offset: 0x000B6954
		public override void RefreshSectorMap(SectorMapData sector)
		{
			this._sectorContent.ClearContent();
			base.currentSector = sector;
			List<WorldMapStatic> list = new List<WorldMapStatic>();
			List<string> list2 = new List<string>();
			foreach (SystemMapData systemMapData in sector.allSystems)
			{
				WorldMapStatic worldMapStatic = UnityEngine.Object.Instantiate<WorldMapStatic>(systemMapData.prefab, this._sectorContent.transform);
				worldMapStatic.transform.localPosition = systemMapData.position;
				worldMapStatic.SetContent(systemMapData);
				this._sectorContent.AddSpriteRenderers(worldMapStatic.GetSpriteRenderers());
				list.Add(worldMapStatic);
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					if (!mapPointOfInterest.hidden)
					{
						JumpGate jumpGate = mapPointOfInterest as JumpGate;
						if (jumpGate != null && jumpGate.targetSystemGuid != null)
						{
							SystemMapData system = GalaxyMapData.current.GetSystem(jumpGate.targetSystemGuid);
							if (!list2.Contains(systemMapData.name + " " + system.name) && !(jumpGate is EmbassyJumpgate) && !(jumpGate.GetTargetPOI() is EmbassyJumpgate))
							{
								DrawJumpgateLine drawJumpgateLine = UnityEngine.Object.Instantiate<DrawJumpgateLine>(this.jumpgateLinePrefab, this._sectorContent.transform);
								Vector2 position = system.position;
								if (systemMapData.sector != system.sector)
								{
									position.x += (system.sector.position.x - systemMapData.sector.position.x) * 100f;
									position.y += (system.sector.position.y - systemMapData.sector.position.y) * 100f;
									if (system.sector.quadrant > systemMapData.sector.quadrant)
									{
										position.x += 10000f;
									}
									else if (system.sector.quadrant < systemMapData.sector.quadrant)
									{
										position.x -= 10000f;
									}
								}
								drawJumpgateLine.SetPositions(worldMapStatic.transform.position, position, jumpGate.canUseJumpGate ? ColorHelper.greenish : ColorHelper.reddish, null, 0.3f);
								this._sectorContent.AddLineRenderer(drawJumpgateLine.lineRenderer);
								list2.Add(systemMapData.name + " " + system.name);
								list2.Add(system.name + " " + systemMapData.name);
							}
						}
					}
				}
				SystemMapData systemMapData2 = systemMapData;
				MapPointOfInterest focusPointOfInterest = this.focusPointOfInterest;
				if (systemMapData2 == ((focusPointOfInterest != null) ? focusPointOfInterest.system : null))
				{
					worldMapStatic.highlightMouse = true;
				}
			}
			if (sector.conquestSector)
			{
				base.DrawConquestPolygons(sector, list);
			}
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x000B8A80 File Offset: 0x000B6C80
		public override void RefreshSystemMap(SystemMapData system)
		{
			this._systemContent.ClearContent();
			base.currentSystem = system;
			if (base.currentSector == null)
			{
				base.currentSector = system.sector;
			}
			foreach (MapStatic mapStatic in system.statics)
			{
				WorldMapStatic worldMapStatic = UnityEngine.Object.Instantiate<WorldMapStatic>(mapStatic.Prefab, this._systemContent.transform);
				worldMapStatic.transform.localPosition = mapStatic.position;
				worldMapStatic.SetContent(mapStatic);
				this._systemContent.AddSpriteRenderers(worldMapStatic.GetSpriteRenderers());
			}
			foreach (MapPointOfInterest mapPointOfInterest in system.pointsOfInterest)
			{
				if (!mapPointOfInterest.hidden)
				{
					WorldMapPOI worldMapPOI = UnityEngine.Object.Instantiate<WorldMapPOI>(mapPointOfInterest.Prefab, this._systemContent.transform);
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
					}
					worldMapPOI.highlightMouse = (mapPointOfInterest == this.focusPointOfInterest);
					worldMapPOI.transform.localPosition = new Vector3(mapPointOfInterest.position.x, mapPointOfInterest.position.y, -1f);
					worldMapPOI.SetContent(mapPointOfInterest);
					this._systemContent.AddSpriteRenderers(worldMapPOI.GetSpriteRenderers());
				}
			}
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				MapPointOfInterest dynamicPointOfInterest = mission.dynamicPointOfInterest;
				if (((dynamicPointOfInterest != null) ? dynamicPointOfInterest.system : null) == system && dynamicPointOfInterest.system == system)
				{
					WorldMapPOI worldMapPOI2 = UnityEngine.Object.Instantiate<WorldMapPOI>(dynamicPointOfInterest.Prefab, this._systemContent.transform);
					worldMapPOI2.transform.localPosition = new Vector3(dynamicPointOfInterest.position.x, dynamicPointOfInterest.position.y, -1f);
					worldMapPOI2.SetContent(dynamicPointOfInterest);
					worldMapPOI2.SetMissionPoi();
					worldMapPOI2.highlightMouse = (dynamicPointOfInterest == this.focusPointOfInterest);
					this._systemContent.AddSpriteRenderers(worldMapPOI2.GetSpriteRenderers());
				}
			}
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000B8D14 File Offset: 0x000B6F14
		public override void StoreCurrentZoom()
		{
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000B8D16 File Offset: 0x000B6F16
		public override void StartResize()
		{
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000B8D18 File Offset: 0x000B6F18
		public override void SetAspect(float aspect, float zoomFactor)
		{
		}

		// Token: 0x04001285 RID: 4741
		[SerializeField]
		private float maxTweenTime;

		// Token: 0x04001286 RID: 4742
		[SerializeField]
		private float tweenDeltaTime;

		// Token: 0x04001289 RID: 4745
		public Coroutine tweenCoroutine;
	}
}
