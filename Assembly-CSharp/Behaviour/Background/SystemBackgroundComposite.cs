using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Galaxy;
using Source.Galaxy.Statics;
using Source.Player;
using UnityEngine;
using _Scripts.Behaviour.Background;
using _Scripts.Behaviour.Background.Parallax;

namespace Behaviour.Background
{
	// Token: 0x020003BB RID: 955
	public class SystemBackgroundComposite : AbstractGalaxyMapBackground, ICameraTrackable
	{
		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000CEBE2 File Offset: 0x000CCDE2
		// (set) Token: 0x060024B0 RID: 9392 RVA: 0x000CEBEA File Offset: 0x000CCDEA
		public ParallaxLayer parallaxLayer { get; protected set; }

		// Token: 0x060024B1 RID: 9393 RVA: 0x000CEBF3 File Offset: 0x000CCDF3
		private void Update()
		{
			this.smokeCheckTimer -= Time.deltaTime;
			if (this.smokeCheckTimer > 0f)
			{
				return;
			}
			this.smokeCheckTimer = 0.1f;
			this.UpdateLocalSmokeEffect();
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000CEC28 File Offset: 0x000CCE28
		private void UpdateLocalSmokeEffect()
		{
			if (!this.localeSmokeEffect || this.isGalaxyMap)
			{
				return;
			}
			BasePoiManager current = BasePoiManager.current;
			bool flag = current != null && current.SpaceShipIsAtPoi();
			if (flag && !this.localeSmokeEffect.tileWarpEnabled)
			{
				this.localeSmokeEffect.SetTileWarpEnabled(true);
				return;
			}
			if (!flag && this.localeSmokeEffect.tileWarpEnabled)
			{
				this.localeSmokeEffect.SetTileWarpEnabled(false);
			}
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000CEC98 File Offset: 0x000CCE98
		public void LoadBackgroundData(SystemMapData systemData, bool isGalaxyMap = false)
		{
			base.transform.DestroyChildren();
			this.spriteRenderers.Clear();
			this.isGalaxyMap = isGalaxyMap;
			if (GamePlayer.current != null)
			{
				this.systemData = systemData;
				this.data = SystemBackgroundCompositeData.CreateForSystem(systemData);
			}
			else
			{
				this.systemData = systemData;
				this.data = SystemBackgroundCompositeData.CreateForSystem(systemData);
			}
			if (this.setPresetCoroutine != null)
			{
				base.StopCoroutine(this.setPresetCoroutine);
			}
			this.setPresetCoroutine = base.StartCoroutine(this.SetPresetForSystem());
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000CED17 File Offset: 0x000CCF17
		public void SetGameCamera(Camera camera)
		{
			this.gameCamera = camera;
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000CED20 File Offset: 0x000CCF20
		public override void SetBackgroundAlpha(float alpha)
		{
			foreach (SpriteRenderer spriteRenderer in this.spriteRenderers)
			{
				Color color = spriteRenderer.color;
				color.a = alpha;
				spriteRenderer.color = color;
			}
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000CED80 File Offset: 0x000CCF80
		private IEnumerator SetPresetForSystem()
		{
			if (this.data != null)
			{
				yield return this.SetBackground();
			}
			yield break;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000CED8F File Offset: 0x000CCF8F
		public void SetMainMenuState()
		{
			this.CreateDefaultStarLayers();
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000CED97 File Offset: 0x000CCF97
		private IEnumerator SetBackground()
		{
			base.transform.DestroyChildren();
			foreach (NebulaData nebulaData in this.data.nebulae)
			{
				Nebula nebula = this.InstantiateNebula();
				nebula.SetData(nebulaData);
				nebula.transform.localScale = new Vector2(1f, 1f);
				this.spriteRenderers.Add(nebula.spriteRenderer);
			}
			yield return null;
			if (this.data.starLayerPerformantData != null)
			{
				this.InstantiateStarBackground();
				this.starBackground.SetData(this.data.starLayerPerformantData);
				this.spriteRenderers.Add(this.starBackground.background);
			}
			yield return null;
			if (!this.isGalaxyMap)
			{
				this.CreatePlanets();
				yield return null;
				this.CreateSmokeLayers();
				yield return null;
				yield return this.CreateStarLayers();
				this.SetSunlight();
			}
			yield break;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000CEDA6 File Offset: 0x000CCFA6
		public void SetSunlight()
		{
			Singleton<BackdropManager>.Instance.SetSunlight(this.data);
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000CEDB8 File Offset: 0x000CCFB8
		private void CreateSmokeLayers()
		{
			foreach (SmokeBackgroundEffectData smokeBackgroundEffectData in this.data.smokeBackgroundEffectData)
			{
				SmokeBackgroundEffect smokeBackgroundEffect = UnityEngine.Object.Instantiate<SmokeBackgroundEffect>(this.smokeBackgroundEffectPrefab, base.transform);
				smokeBackgroundEffect.GetComponent<CameraTracker>().SetCamera(this.gameCamera);
				smokeBackgroundEffect.SetData(smokeBackgroundEffectData, this.screenSizeGame);
				if (smokeBackgroundEffectData.distance == 1f)
				{
					this.localeSmokeEffect = smokeBackgroundEffect;
				}
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000CEE50 File Offset: 0x000CD050
		private Nebula InstantiateNebula()
		{
			return UnityEngine.Object.Instantiate<Nebula>(this.nebulaPrefab, base.transform);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000CEE64 File Offset: 0x000CD064
		private void InstantiateStarBackground()
		{
			this.starBackground = UnityEngine.Object.Instantiate<StarLayerPerformant>(this.starLayerPerformantPrefab, base.transform);
			StarLayerPerformant starLayerPerformant = this.starBackground;
			Vector2 vector = this.screenSize;
			SystemMapData systemMapData = this.systemData;
			starLayerPerformant.SetScreenSize(vector, (systemMapData != null) ? systemMapData.position : Vector2.zero);
			this.starBackground.gameObject.layer = base.gameObject.layer;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000CEECA File Offset: 0x000CD0CA
		private StarLayer InstantiateStarLayer(StarLayerData data)
		{
			StarLayer starLayer = UnityEngine.Object.Instantiate<StarLayer>(this.starLayerPrefab, base.transform);
			starLayer.SetData(data, this.screenSize);
			starLayer.SetScreenSize(this.screenSize, this.screenSizeGame);
			return starLayer;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000CEEFC File Offset: 0x000CD0FC
		private IEnumerator CreateStarLayers()
		{
			foreach (StarLayerData starLayerData in this.data.starLayers)
			{
				this.InstantiateStarLayer(starLayerData);
				yield return null;
			}
			List<StarLayerData>.Enumerator enumerator = default(List<StarLayerData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000CEF0C File Offset: 0x000CD10C
		public void CreateDefaultStarLayers()
		{
			Vector2 vector = (GamePlayer.current != null) ? GamePlayer.current.currentSystem.position : Vector2.zero;
			this.InstantiateStarLayer(new StarLayerData
			{
				gridTiles = new Vector2(20f, 5f) / 2f,
				scale = 2f,
				offset = vector
			});
			this.InstantiateStarLayer(new StarLayerData
			{
				gridTiles = new Vector2(20f, 5f) / 4f,
				scale = 1.5f,
				offset = vector + new Vector2(-50.3f, 30.4f)
			});
			this.InstantiateStarLayer(new StarLayerData
			{
				gridTiles = new Vector2(20f, 5f) / 8f,
				scale = 1f,
				offset = vector + new Vector2(50.3f, -25.9f)
			});
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000CF018 File Offset: 0x000CD218
		public void CreatePlanets()
		{
			if (this.systemData == null)
			{
				return;
			}
			foreach (MapStatic mapStatic in this.systemData.statics)
			{
				Source.Galaxy.Statics.Planet planet = mapStatic as Source.Galaxy.Statics.Planet;
				if (planet != null)
				{
					UnityEngine.Object.Instantiate<Planet>(this.planetPrefab, planet.position * MapPointOfInterest.mapToLocalConversion, Quaternion.identity, base.transform).SetPlanetData(planet);
				}
			}
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000CF0AC File Offset: 0x000CD2AC
		public void SetScreenSize(Vector2 screenSize, Vector2 screenSizeGame)
		{
			this.screenSize = screenSize;
			this.screenSizeGame = screenSizeGame;
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000CF0BC File Offset: 0x000CD2BC
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			base.transform.position = newPosition;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000CF0D0 File Offset: 0x000CD2D0
		public void SetLayer(int layer)
		{
			base.gameObject.layer = layer;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).gameObject.layer = layer;
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000CF118 File Offset: 0x000CD318
		public void InitializeParallaxBackground()
		{
			string str = "Initialize parallax called, poi:";
			MapPointOfInterest current = MapPointOfInterest.current;
			string str2 = (current != null) ? current.ToString() : null;
			string str3 = ", basePoi: ";
			BasePoiManager current2 = BasePoiManager.current;
			Debug.Log(str + str2 + str3 + ((current2 != null) ? current2.ToString() : null));
			if (!BasePoiManager.current || BasePoiManager.current.poi == null)
			{
				return;
			}
			Debug.Log("Refreshing parallax");
			this.parallaxLayer = UnityEngine.Object.Instantiate<ParallaxLayer>(this.parallaxLayerPrefab, base.transform);
			this.parallaxLayer.Init(BasePoiManager.current.poi, BasePoiManager.current.worldCoordinates);
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000CF1B4 File Offset: 0x000CD3B4
		public void DestroyParallaxBackground()
		{
			UnityEngine.Object.Destroy(this.parallaxLayer.gameObject);
		}

		// Token: 0x04001642 RID: 5698
		[SerializeField]
		private Nebula nebulaPrefab;

		// Token: 0x04001643 RID: 5699
		[SerializeField]
		private StarLayer starLayerPrefab;

		// Token: 0x04001644 RID: 5700
		[SerializeField]
		private StarLayerOld starLayerOldPrefab;

		// Token: 0x04001645 RID: 5701
		[SerializeField]
		private GameObject spaceDustEffectPrefab;

		// Token: 0x04001646 RID: 5702
		[SerializeField]
		private SmokeBackgroundEffect smokeBackgroundEffectPrefab;

		// Token: 0x04001647 RID: 5703
		[SerializeField]
		private StarLayerPerformant starLayerPerformantPrefab;

		// Token: 0x04001648 RID: 5704
		[SerializeField]
		private ParallaxLayer parallaxLayerPrefab;

		// Token: 0x04001649 RID: 5705
		[SerializeField]
		private Planet planetPrefab;

		// Token: 0x0400164B RID: 5707
		private Vector2 screenSize;

		// Token: 0x0400164C RID: 5708
		private Vector2 screenSizeGame;

		// Token: 0x0400164D RID: 5709
		public SystemBackgroundCompositeData data;

		// Token: 0x0400164E RID: 5710
		public SystemMapData systemData;

		// Token: 0x0400164F RID: 5711
		private StarLayerPerformant starBackground;

		// Token: 0x04001650 RID: 5712
		private bool isGalaxyMap;

		// Token: 0x04001651 RID: 5713
		private Coroutine setPresetCoroutine;

		// Token: 0x04001652 RID: 5714
		private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

		// Token: 0x04001653 RID: 5715
		private SmokeBackgroundEffect localeSmokeEffect;

		// Token: 0x04001654 RID: 5716
		private Camera gameCamera;

		// Token: 0x04001655 RID: 5717
		private float smokeCheckTimer;
	}
}
