using System;
using Behaviour.Background;
using Behaviour.Bootstrap;
using Behaviour.Effects;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.World;
using Source.Util;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x02000009 RID: 9
public class BackdropManager : Singleton<BackdropManager>
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x06000062 RID: 98 RVA: 0x000040D2 File Offset: 0x000022D2
	// (set) Token: 0x06000063 RID: 99 RVA: 0x000040DA File Offset: 0x000022DA
	public CameraMovement cameraMovement { get; private set; }

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000064 RID: 100 RVA: 0x000040E3 File Offset: 0x000022E3
	// (set) Token: 0x06000065 RID: 101 RVA: 0x000040EB File Offset: 0x000022EB
	public Vector2 screenSize { get; private set; }

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000066 RID: 102 RVA: 0x000040F4 File Offset: 0x000022F4
	// (set) Token: 0x06000067 RID: 103 RVA: 0x000040FC File Offset: 0x000022FC
	public Vector2 screenSizeGame { get; private set; }

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000068 RID: 104 RVA: 0x00004105 File Offset: 0x00002305
	// (set) Token: 0x06000069 RID: 105 RVA: 0x0000410D File Offset: 0x0000230D
	public SystemBackgroundComposite systemBackgroundComposite { get; private set; }

	// Token: 0x0600006A RID: 106 RVA: 0x00004118 File Offset: 0x00002318
	private void Start()
	{
		this.mainCamera = Camera.main;
		this.systemBackgroundComposite = UnityEngine.Object.Instantiate<SystemBackgroundComposite>(this.systemBackgroundCompositePrefab, this.mainCamera.transform);
		this.systemBackgroundComposite.GetComponent<CameraTracker>().SetCamera(this.mainCamera);
		this.cameraMovement = this.mainCamera.GetComponent<CameraMovement>();
		this.gameCamera = this.cameraMovement.gameCamera;
		this.systemBackgroundComposite.SetGameCamera(this.gameCamera);
		this.fullScreenSize = new Vector2((float)Screen.width, (float)Screen.height);
		this.currentAspect = this.mainCamera.aspect;
		this.dustEffect = UnityEngine.Object.Instantiate<GameObject>(this.spaceDustEffectPrefab, base.transform).GetComponent<DustEffect>();
		this.dustEffect.GetComponent<CameraTracker>().SetCamera(this.gameCamera);
		this.SetScreenSize();
		if (SceneLoader.IsSceneLoaded("Main Menu"))
		{
			this.SetMainMenuState();
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00004208 File Offset: 0x00002408
	public void SetMainMenuState()
	{
		SectorMapData sectorMapData = new SectorMapData(Vector2.zero);
		SystemMapData systemMapData = SandboxWorld.CreateEmptySystem(sectorMapData, 1, Faction.blue, Vector2.zero, true);
		sectorMapData.AddSystem(systemMapData);
		this.LoadBackgroundData(systemMapData);
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00004240 File Offset: 0x00002440
	private void Update()
	{
		if (!this.mainCamera)
		{
			return;
		}
		bool flag = this.fullScreenSize != Vector2.zero && this.fullScreenSize != new Vector2((float)Screen.width, (float)Screen.height);
		bool flag2 = !this.mainCamera.aspect.ApproximatelyEqual(this.currentAspect);
		if (flag || flag2)
		{
			this.SetScreenSize();
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000042B0 File Offset: 0x000024B0
	private void SetScreenSize()
	{
		this.fullScreenSize = new Vector2((float)Screen.width, (float)Screen.height);
		this.currentAspect = this.mainCamera.aspect;
		float num = this.mainCamera.orthographicSize * 2f * (1f + this.backgroundMargin);
		float num2 = CameraMovement.maxZoom * 2f;
		this.screenSize = new Vector2(this.mainCamera.aspect * num, num);
		this.screenSizeGame = new Vector2(this.gameCamera.aspect * num2, num2);
		this.dustEffect.SetArea(this.screenSizeGame);
		this.systemBackgroundComposite.SetScreenSize(this.screenSize, this.screenSizeGame);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000436A File Offset: 0x0000256A
	public Color GetStarColor(float time)
	{
		return this.starColors.Evaluate(time);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00004378 File Offset: 0x00002578
	public Color GetNebulaColor(float time)
	{
		return this.nebulaColors.Evaluate(time);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004386 File Offset: 0x00002586
	public Gradient GetNebulaColors()
	{
		return this.nebulaColors;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000438E File Offset: 0x0000258E
	public Color GetSmokeColor(float time)
	{
		return this.smokeColors.Evaluate(time);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000439C File Offset: 0x0000259C
	public Gradient GetSmokeColorsAlpha()
	{
		return this.smokeColorsAlpha;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x000043A4 File Offset: 0x000025A4
	public void SetSunlight(SystemBackgroundCompositeData data)
	{
		this.sunlight.color = data.sunlightColor;
		this.sunlight.intensity = data.sunlightIntensity;
		this.sunlightPlanets.color = data.sunlightColor;
		this.sunlightPlanets.intensity = data.sunlightIntensity;
		this.globalLight.color = data.globalColor;
		this.globalLight.intensity = data.globalLightIntensity;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004418 File Offset: 0x00002618
	public void ShowInJumpGateTravelEffect(Vector2 position)
	{
		this.dustEffect.gameObject.SetActive(false);
		this.inJumpgateEffect = UnityEngine.Object.Instantiate<GameObject>(this.inJumpgateEffectPrefab, position, Quaternion.identity, base.transform).GetComponent<InJumpGateTunnelEffect>();
		this.inJumpgateEffect.SetStartSize(this.screenSizeGame);
		this.inJumpgateEffect.Play();
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00004479 File Offset: 0x00002679
	public void EndJumpgateTravel()
	{
		this.inJumpgateEffect.Stop();
		UnityEngine.Object.Destroy(this.inJumpgateEffect.gameObject);
		this.dustEffect.gameObject.SetActive(true);
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000044A7 File Offset: 0x000026A7
	public void HideBackground()
	{
		this.systemBackgroundComposite.gameObject.SetActive(false);
	}

	// Token: 0x06000077 RID: 119 RVA: 0x000044BA File Offset: 0x000026BA
	public void SetBackgroundData(SystemMapData data)
	{
		this.SetScreenSize();
		this.LoadBackgroundData(data);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x000044C9 File Offset: 0x000026C9
	private void LoadBackgroundData(SystemMapData data)
	{
		this.systemBackgroundComposite.gameObject.SetActive(true);
		this.systemBackgroundComposite.LoadBackgroundData(data, false);
		this.InitializeParallax();
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000044EF File Offset: 0x000026EF
	public void InitializeParallax()
	{
		SystemBackgroundComposite systemBackgroundComposite = this.systemBackgroundComposite;
		if (systemBackgroundComposite == null)
		{
			return;
		}
		systemBackgroundComposite.InitializeParallaxBackground();
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004501 File Offset: 0x00002701
	public void DestroyParallax()
	{
		SystemBackgroundComposite systemBackgroundComposite = this.systemBackgroundComposite;
		if (systemBackgroundComposite == null)
		{
			return;
		}
		systemBackgroundComposite.DestroyParallaxBackground();
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00004513 File Offset: 0x00002713
	public void ResetBackgroundData()
	{
		this.SetScreenSize();
		if (GamePlayer.current != null)
		{
			this.SetBackgroundData(GamePlayer.current.currentSystem);
			return;
		}
		this.SetMainMenuState();
	}

	// Token: 0x04000049 RID: 73
	[SerializeField]
	private Gradient starColors;

	// Token: 0x0400004A RID: 74
	[SerializeField]
	private Gradient nebulaColors;

	// Token: 0x0400004B RID: 75
	[SerializeField]
	private Gradient smokeColors;

	// Token: 0x0400004C RID: 76
	[SerializeField]
	private Gradient smokeColorsAlpha;

	// Token: 0x0400004D RID: 77
	[SerializeField]
	private SystemBackgroundComposite systemBackgroundCompositePrefab;

	// Token: 0x0400004E RID: 78
	[SerializeField]
	private GameObject inJumpgateEffectPrefab;

	// Token: 0x0400004F RID: 79
	[SerializeField]
	private GameObject spaceDustEffectPrefab;

	// Token: 0x04000050 RID: 80
	[SerializeField]
	private Light2D globalLight;

	// Token: 0x04000051 RID: 81
	[SerializeField]
	private Light2D sunlight;

	// Token: 0x04000052 RID: 82
	[SerializeField]
	private Light2D sunlightPlanets;

	// Token: 0x04000053 RID: 83
	[SerializeField]
	private float backgroundMargin;

	// Token: 0x04000054 RID: 84
	private InJumpGateTunnelEffect inJumpgateEffect;

	// Token: 0x04000055 RID: 85
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000056 RID: 86
	private Camera mainCamera;

	// Token: 0x04000057 RID: 87
	private Camera gameCamera;

	// Token: 0x0400005B RID: 91
	private float orthographicSize;

	// Token: 0x0400005C RID: 92
	private Vector2 fullScreenSize;

	// Token: 0x0400005D RID: 93
	private float currentAspect;

	// Token: 0x0400005F RID: 95
	private DustEffect dustEffect;
}
