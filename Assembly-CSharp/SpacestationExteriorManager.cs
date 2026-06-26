using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior.Spacestation;
using Behaviour.Bootstrap;
using Behaviour.GalaxyMap;
using Behaviour.Managers;
using Behaviour.Spacestation.Cargo;
using Behaviour.Spacestation.Docking;
using Behaviour.UI.HUD;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World.System;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class SpacestationExteriorManager : BasePoiManager
{
	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000083 RID: 131 RVA: 0x0000483E File Offset: 0x00002A3E
	// (set) Token: 0x06000084 RID: 132 RVA: 0x00004845 File Offset: 0x00002A45
	public static SpacestationExteriorManager Instance { get; private set; }

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000085 RID: 133 RVA: 0x0000484D File Offset: 0x00002A4D
	// (set) Token: 0x06000086 RID: 134 RVA: 0x00004855 File Offset: 0x00002A55
	public Spacestation spacestation { get; private set; }

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000087 RID: 135 RVA: 0x0000485E File Offset: 0x00002A5E
	// (set) Token: 0x06000088 RID: 136 RVA: 0x00004866 File Offset: 0x00002A66
	public Vector2 landingPosition { get; private set; }

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000089 RID: 137 RVA: 0x0000486F File Offset: 0x00002A6F
	// (set) Token: 0x0600008A RID: 138 RVA: 0x00004877 File Offset: 0x00002A77
	public Coroutine undockingRoutine { get; private set; }

	// Token: 0x0600008B RID: 139 RVA: 0x00004880 File Offset: 0x00002A80
	protected override void Awake()
	{
		base.Awake();
		SpacestationExteriorManager.Instance = this;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0000488E File Offset: 0x00002A8E
	protected override void Update()
	{
		base.Update();
		if (this.autoActionsTimer >= 0f)
		{
			this.autoActionsTimer -= Time.deltaTime;
			if (Time.timeScale == 0f)
			{
				this.autoActionsTimer = -1f;
			}
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000048CC File Offset: 0x00002ACC
	protected override void SetWorldCoordinates()
	{
		float num = 250f;
		float num2 = CameraMovement.maxZoom * 4f;
		base.worldCoordinates = new Rect(base.transform.position.x - num / 2f, base.transform.position.y - num2 / 2f, num, num2);
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00004928 File Offset: 0x00002B28
	public override Vector2 GetWorldPositionToTravelTo()
	{
		if (base.initializedAndReady)
		{
			return this.landingPosition;
		}
		return base.GetWorldPositionToTravelTo();
	}

	// Token: 0x0600008F RID: 143 RVA: 0x0000493F File Offset: 0x00002B3F
	protected override IEnumerator InitializePoi()
	{
		yield return base.InitializePoi();
		SpaceStation spaceStation = base.poi as SpaceStation;
		if (spaceStation != null && !spaceStation.conquestStationInitialized)
		{
			this.InitializeConquestStation(spaceStation);
		}
		yield return new WaitForEndOfFrame();
		this.SetDockingOptions();
		this.SetLandingPosition();
		if (this.spaceShip && !this.spaceShip.travelling && this.PlayerIsFriendly())
		{
			this.AssignClosestDockingOption(this.spaceShip, true);
			this.CheckForSpaceStationEnter();
		}
		this.autoActionsTimer = 0.2f;
		foreach (AbstractUnitData abstractUnitData in base.poi.GetUnits(false))
		{
			SpacestationExteriorManager.ClosureClass30_0 _locals_1 = new SpacestationExteriorManager.ClosureClass30_0();
			_locals_1._thisRef = this;
			AbstractUnit unit = abstractUnitData.unit;
			_locals_1.spaceship = (unit as SpaceShip);
			if (_locals_1.spaceship != null && !abstractUnitData.IsEnemy(Faction.player))
			{
				yield return new WaitUntil(() => _locals_1.spaceship.autoActions != null || _locals_1._thisRef.autoActionsTimer < 0f);
				_locals_1.spaceship.InitSpacestationAutoActions();
			}
			_locals_1 = null;
		}
		IEnumerator<AbstractUnitData> enumerator = null;
		yield return new WaitForEndOfFrame();
		if (base.poi.system.sector.conquestSector)
		{
			CargoDockManager component = base.GetComponent<CargoDockManager>();
			component.enabled = true;
			component.SetDockTypes();
			component.SetDocks();
		}
		yield return null;
		yield break;
		yield break;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x0000494E File Offset: 0x00002B4E
	public bool PlayerIsFriendly()
	{
		return (base.poi as SpaceStation).PlayerIsFriendly();
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00004960 File Offset: 0x00002B60
	public void CheckForSpaceStationEnter()
	{
		if (!GamePlayer.current.autoPlay && !GamePlayer.current.hasUmbralTransponder)
		{
			float? lastVisitedTimeAtLoad = this.lastVisitedTimeAtLoad;
			float num = 0f;
			if ((lastVisitedTimeAtLoad.GetValueOrDefault() > num & lastVisitedTimeAtLoad != null) && !SpaceStationInterior.instance && GamePlayer.current.waypoints.Count == 0)
			{
				this.EnterSpacestation();
			}
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000049CC File Offset: 0x00002BCC
	private Spacestation SetNewStation(SpaceStation data)
	{
		if (data.system.sector.conquestSector)
		{
			this.InitializeConquestStation(data);
		}
		SpaceStation.StationVariants randomVariant = SeededRandom.Global.ChooseEnum<SpaceStation.StationVariants>(0);
		Spacestation result = this.spacestationVariants.FirstOrDefault((Spacestation s) => s.variant == randomVariant) ?? this.spacestationVariants[0];
		data.stationVariant = randomVariant;
		return result;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00004A3C File Offset: 0x00002C3C
	private void InitializeConquestStation(SpaceStation data)
	{
		if (data.stationSeed == null)
		{
			data.SetStationSeed();
		}
		CombatStationData combatStationData;
		if (data.faction == Faction.puppeteers && data.system.sector.quadrant == 2)
		{
			combatStationData = CombatStationFactory.CreateUmbralOutpost(data);
		}
		else
		{
			combatStationData = CombatStationFactory.CreateStationVariant(data.stationSeed, data);
		}
		data.conquestStationInitialized = true;
		if (data is EmbassyStation)
		{
			foreach (CombatStationPartData combatStationPartData in combatStationData.stationParts)
			{
				combatStationPartData.alwaysFriendly = true;
			}
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00004ADC File Offset: 0x00002CDC
	private void OnDestroy()
	{
		foreach (SpaceShip spaceShip in base.GetComponentsInChildren<SpaceShip>())
		{
			base.poi.RemoveUnit(spaceShip.spaceShipData);
		}
		MapPointOfInterest poi = base.poi;
		if (poi == null)
		{
			return;
		}
		poi.ClearPayloads();
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00004B24 File Offset: 0x00002D24
	public override void SpaceshipHasArrived()
	{
		base.SpaceshipHasArrived();
		if (!this.PlayerIsFriendly())
		{
			return;
		}
		GamePlayer.current.lastStation = (base.poi as SpaceStation);
		HudManager.Instance.ToggleDockButton(!AbstractGalaxyMapManager.IsShowing());
		MissionObjective.Trigger(MissionTrigger.ArrivedAtSpaceStation, null, null, false);
		SpaceStation current = SpaceStation.current;
		if (current != null)
		{
			current.RefreshShopsIfNecessary(true);
		}
		CargoDockManager instance = CargoDockManager.Instance;
		if (instance != null)
		{
			instance.DistributeAndResetTimer();
		}
		this.CheckForDocking();
		this.CheckForSpaceStationEnter();
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00004BA0 File Offset: 0x00002DA0
	public void CheckForDocking()
	{
		foreach (DockingOption dockingOption in this.dockingOptions)
		{
			if (GameplayManager.Instance.spaceShip == dockingOption.dockingSpaceship)
			{
				this.CheckForSpaceStationEnter();
				return;
			}
		}
		if (GamePlayer.current.waypoints.Count == 0 && PersistentSingleton<SceneLoader>.Instance.CurrentScene != "SpacestationInterior")
		{
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSDocking", Array.Empty<object>())).WithColor(ColorHelper.greenish).WithCustomTime(2f).Show();
			this.AssignClosestDockingOption(GameplayManager.Instance.spaceShip, false);
			this.CheckForSpaceStationEnter();
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00004C80 File Offset: 0x00002E80
	protected override IEnumerator InitializationComplete()
	{
		yield return base.InitializationComplete();
		if (this.PlayerIsFriendly())
		{
			base.StartCoroutine(this.NPCSpawner());
		}
		else
		{
			base.StartCoroutine(this.ReinforcementsSpawner());
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00004C90 File Offset: 0x00002E90
	public DockingOption GetDockingOption(SpaceShip ship)
	{
		foreach (DockingOption dockingOption in this.dockingOptions)
		{
			if (dockingOption.dockingSpaceship == ship)
			{
				return dockingOption;
			}
		}
		return null;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00004CF4 File Offset: 0x00002EF4
	public bool CancelDocking(SpaceShip ship, bool notify = true)
	{
		DockingOption dockingOption = this.GetDockingOption(ship);
		if (dockingOption != null)
		{
			DockingState? dockingState = ship.spaceShipData.dockingState;
			DockingState dockingState2 = DockingState.Docked;
			if (!(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null))
			{
				dockingState = ship.spaceShipData.dockingState;
				dockingState2 = DockingState.Undocking;
				if (!(dockingState.GetValueOrDefault() == dockingState2 & dockingState != null))
				{
					ship.SetEngineState(true, true);
					ship.ToggleStateForDocking(true);
					ship.spaceShipData.dockingState = null;
					ship.forceWorldAngle = null;
					dockingOption.ResetDockingOption();
					ship.spaceShipData.dockingState = null;
					if (notify)
					{
						Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSDockingCancel", Array.Empty<object>())).WithColor(ColorHelper.reddish).WithCustomTime(2f).Show();
					}
					return true;
				}
			}
			return false;
		}
		ship.spaceShipData.dockingState = null;
		return true;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00004DE7 File Offset: 0x00002FE7
	public void StartUndocking()
	{
		this.enteringSpaceStation = false;
		if (this.undockingRoutine != null)
		{
			return;
		}
		this.undockingRoutine = base.StartCoroutine(this.UndockSpaceship());
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00004E0B File Offset: 0x0000300B
	private IEnumerator UndockSpaceship()
	{
		DockingOption dockingOption = this.FindDockingOption(GameplayManager.Instance.spaceShip);
		yield return new WaitUntil(() => GameplayManager.Instance.spaceShip.rigidbody != null);
		if (dockingOption == null)
		{
			this.undockingRoutine = null;
			yield break;
		}
		yield return dockingOption.AssignSpaceshipForUnDocking(GameplayManager.Instance.spaceShip);
		this.undockingRoutine = null;
		yield break;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00004E1C File Offset: 0x0000301C
	private void SetDockingOptions()
	{
		this.dockingOptions.Clear();
		foreach (DockingOption item in base.GetComponentsInChildren<DockingOption>())
		{
			this.dockingOptions.Add(item);
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00004E59 File Offset: 0x00003059
	private void SetLandingPosition()
	{
		this.landingPosition = this.GetLandingPosition(GameplayManager.Instance.spaceShip);
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00004E74 File Offset: 0x00003074
	public Vector2 GetLandingPosition(SpaceShip spaceShip)
	{
		Vector2 result;
		if (spaceShip.rigidbody.position.x > base.poi.GetWorldPosition().x)
		{
			result = base.transform.position;
			result.x += 40f;
		}
		else
		{
			result = base.transform.position;
			result.x -= 30f;
		}
		result.y += (float)UnityEngine.Random.Range(-10, 10);
		result.x += (float)UnityEngine.Random.Range(-4, 4);
		return result;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00004F14 File Offset: 0x00003114
	public DockingOption FindDockingOption(SpaceShip spaceShip)
	{
		foreach (DockingOption dockingOption in this.dockingOptions)
		{
			if (dockingOption.dockingSpaceship == spaceShip)
			{
				return dockingOption;
			}
		}
		return this.FindClosestDockingOption(spaceShip);
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00004F7C File Offset: 0x0000317C
	public bool AssignClosestDockingOption(SpaceShip spaceship, bool init = false)
	{
		DockingOption dockingOption = this.FindClosestDockingOption(spaceship);
		if (dockingOption != null)
		{
			dockingOption.AssignSpaceshipForDocking(spaceship, init && Vector2.Distance(dockingOption.transform.position, spaceship.transform.position) < 1f);
			return true;
		}
		return false;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00004FD8 File Offset: 0x000031D8
	private DockingOption FindClosestDockingOption(SpaceShip spaceship)
	{
		DockingOption result = null;
		float num = float.MaxValue;
		foreach (DockingOption dockingOption in this.dockingOptions)
		{
			if (!dockingOption.occupied && dockingOption.CanDock(spaceship))
			{
				float num2 = Vector2.Distance(spaceship.transform.position, dockingOption.transform.position);
				if (num2 < num)
				{
					num = num2;
					result = dockingOption;
				}
			}
		}
		return result;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00005070 File Offset: 0x00003270
	public DockingOption FindClosestDockingOption(Vector2 position, SpaceShip ship)
	{
		DockingOption dockingOption = null;
		float num = float.MaxValue;
		foreach (DockingOption dockingOption2 in this.dockingOptions)
		{
			if (dockingOption2.CanDock(ship))
			{
				float num2 = Vector2.Distance(position, dockingOption2.transform.position);
				if (num2 < num)
				{
					num = num2;
					dockingOption = dockingOption2;
				}
			}
		}
		if (dockingOption.occupied)
		{
			UnityEngine.Object.Destroy(dockingOption.dockingSpaceship.gameObject);
		}
		return dockingOption;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005108 File Offset: 0x00003308
	public DockingOption FindDockingOptionAtPosition(Vector2 position, SpaceShip ship)
	{
		foreach (DockingOption dockingOption in this.dockingOptions)
		{
			if (!dockingOption.occupied && dockingOption.CanDock(ship) && Vector2.Distance(position, dockingOption.transform.position) < 0.5f)
			{
				return dockingOption;
			}
		}
		return null;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0000518C File Offset: 0x0000338C
	private IEnumerator NPCSpawner()
	{
		for (;;)
		{
			yield return new WaitForSeconds((float)UnityEngine.Random.Range(5, 15));
			if (base.poi.GetUnits(false).Count<AbstractUnitData>() + base.poi.GetTriggeredPayloads().Count<MapTriggeredPayload>() < this.dockingOptions.Count - 1)
			{
				for (int i = 0; i < 10; i++)
				{
					DockingOptionSize size = SeededRandom.Global.Choose<DockingOptionSize>(SpacestationExteriorManager.visitingShipSizes);
					if (this.DockingOptionAvailableForSize(size) && this.CreatePasserbyShip(size))
					{
						break;
					}
				}
			}
			yield return new WaitForSeconds((float)UnityEngine.Random.Range(20, 45));
		}
		yield break;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000519B File Offset: 0x0000339B
	private IEnumerator ReinforcementsSpawner()
	{
		ConquestSystem conquest = SystemMapData.current.storyteller as ConquestSystem;
		bool isSystemFaction = SystemMapData.current.faction == base.poi.faction;
		if (conquest != null)
		{
			while (!isSystemFaction || conquest.combatStrength > 0f)
			{
				if (base.poi.totalEnemyCount == 0)
				{
					if (this.extraReinforcements > 0f && isSystemFaction)
					{
						int num = Mathf.RoundToInt(this.extraReinforcements);
						if ((float)num > conquest.combatStrength)
						{
							num = Mathf.RoundToInt(conquest.combatStrength);
							conquest.combatStrength = 0f;
						}
						else
						{
							conquest.combatStrength -= (float)num;
						}
						if (conquest.combatStrength > 0f)
						{
							Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@DestroyedStationReinforcements", new object[]
							{
								-num
							})).WithColor(ColorHelper.greenish).WithCustomTime(5f).Show();
						}
						else
						{
							conquest.controlLevel = 0;
							Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@ReinforcementsReducedToZero", Array.Empty<object>())).WithColor(ColorHelper.greenish).WithCustomTime(5f).Show();
						}
					}
					base.poi.AddTriggeredSpawn(base.poi.CreateUnitPayload(2f + this.extraReinforcements, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), 8f, 0, false, true);
					this.extraReinforcements += 1f;
				}
				yield return new WaitForSeconds(1f);
			}
		}
		yield break;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x000051AC File Offset: 0x000033AC
	public bool DockingOptionAvailableForSize(DockingOptionSize size)
	{
		return this.dockingOptions.Count((DockingOption option) => !option.occupied && option.CanDock(size)) > ((this.spaceShip.shipRoleType.GetDockingOptionSize() == size && !this.IsSpaceShipDocking(this.spaceShip)) ? 1 : 0);
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000520C File Offset: 0x0000340C
	private bool IsSpaceShipDocking(SpaceShip spaceShip)
	{
		using (List<DockingOption>.Enumerator enumerator = this.dockingOptions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.dockingSpaceship == spaceShip)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000526C File Offset: 0x0000346C
	public bool CreatePasserbyShip(DockingOptionSize size)
	{
		if (base.poi.faction == Faction.puppeteers && base.poi.system.sector.quadrant == 2)
		{
			return false;
		}
		Vector2 spawnPosition;
		if (SeededRandom.Global.RandomBool(0.5f))
		{
			spawnPosition = this.GetLeftSpawnPosition();
		}
		else
		{
			spawnPosition = this.GetRightSpawnPosition();
		}
		spawnPosition.y += (float)(SeededRandom.Global.RandomBool(0.5f) ? -30 : 30);
		spawnPosition.x += (float)UnityEngine.Random.Range(-5, 5);
		SpaceShipData spaceShipData = base.CreatePasserbyShipData(spawnPosition, new DockingOptionSize?(size));
		if (spaceShipData == null)
		{
			Debug.Log("Failed to create passerby ship, no suitable unit found!");
			return false;
		}
		spaceShipData.autoActions = "SpaceStation";
		spaceShipData.dockingState = new DockingState?(DockingState.Arriving);
		MapTriggeredPayload mapTriggeredPayload = new MapTriggeredPayload(base.poi);
		mapTriggeredPayload.units.Add(spaceShipData);
		mapTriggeredPayload.spawnAtPlayer = false;
		base.poi.AddPayload(mapTriggeredPayload);
		return true;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00005360 File Offset: 0x00003560
	public Vector2 GetClosestWarpInPosition(SpaceShip spaceShip)
	{
		Vector2 result;
		if (spaceShip.rigidbody.position.x < base.transform.position.x)
		{
			result = base.transform.position;
			result.x -= 20f;
		}
		else
		{
			result = base.transform.position;
			result.x += 30f;
		}
		result.y += (float)SeededRandom.Global.RandomRange(-8, 8);
		return result;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x000053F0 File Offset: 0x000035F0
	public Vector2 GetFittingExitPosition(SpaceShip spaceShip)
	{
		Vector2 result;
		if (spaceShip.rigidbody.position.x < base.transform.position.x)
		{
			result = this.GetLeftSpawnPosition();
			result.x -= 100f;
		}
		else
		{
			result = this.GetRightSpawnPosition();
			result.x += 100f;
		}
		return result;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00005450 File Offset: 0x00003650
	private Vector2 GetRightSpawnPosition()
	{
		Vector2 result = base.transform.position;
		result.x += 30f;
		return result;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00005480 File Offset: 0x00003680
	private Vector2 GetLeftSpawnPosition()
	{
		Vector2 result = base.transform.position;
		result.x -= 30f;
		return result;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x000054B0 File Offset: 0x000036B0
	public DockingPad GetDockedSpaceship(SpaceShip ship)
	{
		foreach (DockingOption dockingOption in this.dockingOptions)
		{
			DockingPad dockingPad = dockingOption as DockingPad;
			if (dockingPad != null && dockingPad.dockingSpaceship == ship)
			{
				return dockingPad;
			}
		}
		return null;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000551C File Offset: 0x0000371C
	public void EnterSpacestation()
	{
		if (this.enteringSpaceStation || SceneLoader.IsSceneLoaded("SpacestationInterior"))
		{
			return;
		}
		PersistentSingleton<SceneLoader>.Instance.ToggleSpaceStationInterior(true, true);
		GamePlayer.current.emergencyJump = false;
		this.enteringSpaceStation = true;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00005551 File Offset: 0x00003751
	public void TryRemoveDockingOption(DockingOption option)
	{
		if (this.dockingOptions.Contains(option))
		{
			this.dockingOptions.Remove(option);
		}
	}

	// Token: 0x04000064 RID: 100
	[SerializeField]
	private List<Spacestation> spacestationVariants;

	// Token: 0x04000065 RID: 101
	[SerializeField]
	private Spacestation outpostPrefab;

	// Token: 0x04000066 RID: 102
	[SerializeField]
	private Vector2 spacestationPosition = new Vector2(20f, 0f);

	// Token: 0x04000068 RID: 104
	[SerializeField]
	private List<Transform> travelLocations = new List<Transform>();

	// Token: 0x04000069 RID: 105
	private List<DockingOption> dockingOptions = new List<DockingOption>();

	// Token: 0x0400006C RID: 108
	private static Dictionary<DockingOptionSize, float> visitingShipSizes = new Dictionary<DockingOptionSize, float>
	{
		{
			DockingOptionSize.small,
			2f
		},
		{
			DockingOptionSize.medium,
			1.5f
		},
		{
			DockingOptionSize.large,
			0.5f
		}
	};

	// Token: 0x0400006D RID: 109
	private float autoActionsTimer = -1f;

	// Token: 0x0400006E RID: 110
	public DockingOption currentDockingOption;

	// Token: 0x0400006F RID: 111
	public bool enteringSpaceStation;

	// Token: 0x04000070 RID: 112
	private float extraReinforcements;

	private sealed class ClosureClass30_0
	{
		public SpacestationExteriorManager _thisRef;
		public Behaviour.Unit.SpaceShip spaceship;
	}
}
