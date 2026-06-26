using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour;
using Behaviour.Dialogues;
using Behaviour.Equipment;
using Behaviour.Gameplay;
using Behaviour.Managers;
using Behaviour.Spacestation.Docking;
using Behaviour.Travel;
using Behaviour.UI.HUD;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Bar;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Crew;
using Source.Data;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station.Patrons;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation;
using Source.Simulation.World;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000008 RID: 8
public class GameplayManager : MonoBehaviour
{
	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000035 RID: 53 RVA: 0x00003168 File Offset: 0x00001368
	public static Camera camera
	{
		get
		{
			GameplayManager instance = GameplayManager.Instance;
			if (instance == null)
			{
				return null;
			}
			CameraMovement cameraMovement = instance.cameraMovement;
			if (cameraMovement == null)
			{
				return null;
			}
			return cameraMovement.gameCamera;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000036 RID: 54 RVA: 0x00003185 File Offset: 0x00001385
	// (set) Token: 0x06000037 RID: 55 RVA: 0x0000318D File Offset: 0x0000138D
	public CameraMovement cameraMovement { get; private set; }

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000038 RID: 56 RVA: 0x00003196 File Offset: 0x00001396
	// (set) Token: 0x06000039 RID: 57 RVA: 0x0000319E File Offset: 0x0000139E
	public SpaceShip spaceShip { get; private set; }

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600003A RID: 58 RVA: 0x000031A7 File Offset: 0x000013A7
	// (set) Token: 0x0600003B RID: 59 RVA: 0x000031AF File Offset: 0x000013AF
	public List<SpaceShip> fleetSpaceShips { get; private set; } = new List<SpaceShip>();

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x0600003C RID: 60 RVA: 0x000031B8 File Offset: 0x000013B8
	public static bool initialized
	{
		get
		{
			return GameplayManager.Instance && GameplayManager.Instance._initialized;
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000031D4 File Offset: 0x000013D4
	public void ShowItemSaleInfo(Salesman salesman, MissionTrigger? trigger = null)
	{
		ItemSaleInfo itemSaleInfo = UnityEngine.Object.Instantiate<ItemSaleInfo>(this.itemForSalePrefab, Singleton<DialogueManager>.Instance.saleContainer.transform);
		if (trigger != null)
		{
			itemSaleInfo.trigger = new MissionTrigger?(trigger.Value);
		}
		itemSaleInfo.Show(salesman);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x0000321E File Offset: 0x0000141E
	private void Awake()
	{
		GameplayManager.Instance = this;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003228 File Offset: 0x00001428
	private void Start()
	{
		this.moveAction = GameControls.instance.controls.Player.Move;
		this.rotatieAction = GameControls.instance.controls.Player.Rotate;
		Characters.CreateCaptain();
		Characters.CreateShipAI();
		string str = "GameplayManager start, current poi: ";
		MapPointOfInterest currentPointOfInterest = GamePlayer.current.currentPointOfInterest;
		Debug.Log(str + ((currentPointOfInterest != null) ? currentPointOfInterest.ToString() : null));
		this.cameraMovement = Camera.main.GetComponent<CameraMovement>();
		Vector2 spawnLocation = (GamePlayer.current.currentPointOfInterest != null) ? GamePlayer.current.currentPointOfInterest.GetWorldPosition() : Vector2.zero;
		this.GeneratePlayerSpaceship(spawnLocation, 1, "Chisel Mk I SN", false);
		this.GenerateFleet();
		Singleton<BackdropManager>.Instance.SetBackgroundData(GamePlayer.current.currentSystem);
		SidePanel.instance.RefreshIfOpen();
		foreach (Storyteller storyteller in GamePlayer.current.storytellers)
		{
			storyteller.Start();
		}
		foreach (SystemMapData systemMapData in GalaxyMapData.current.allSystems)
		{
			SystemStoryteller storyteller2 = systemMapData.storyteller;
			if (storyteller2 != null)
			{
				storyteller2.Start();
			}
		}
		foreach (MapPointOfInterest mapPointOfInterest in GamePlayer.current.map.allPointsOfInterest)
		{
			SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
			if (spaceStation != null && spaceStation.bar != null)
			{
				spaceStation.bar.CheckUpdatePatrons(false);
			}
		}
		for (int i = 0; i < GamePlayer.current.waypoints.Count; i++)
		{
			if (GamePlayer.current.waypoints[i] == null)
			{
				GamePlayer.current.waypoints.RemoveAt(i);
				i--;
			}
		}
		if (GamePlayer.current.waypoints.Count > 0)
		{
			base.StartCoroutine(this.TravelWhenReady());
		}
		this._initialized = true;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00003464 File Offset: 0x00001664
	private void Update()
	{
		GamePlayer.current.Update();
		Vector2 vector = this.moveAction.ReadValue<Vector2>();
		float num = this.rotatieAction.ReadValue<float>();
		if (GameplayerPrefs.GetManualControl() && this.spaceShip && (vector != Vector2.zero || num != 0f))
		{
			this.spaceShip.ApplyManualMovement(vector, num);
		}
		this.autosaveTimer += Time.deltaTime;
		if (this.autosaveTimer > 300f)
		{
			SaveGame.StoreAutosaveState(null);
			this.autosaveTimer = 0f;
		}
		this.ambientUpdateTimer += Time.deltaTime;
		if (this.ambientUpdateTimer >= 10f)
		{
			base.StartCoroutine(this.DoAmbientUpdate(this.ambientUpdateTimer));
			this.ambientUpdateTimer = 0f;
		}
		this.resetTimer += Time.deltaTime;
		if (GamePlayer.current.autoPlay && this.resetTimer > 28800f && MapPointOfInterest.current is SpaceStation && !SpaceStationInterior.instance)
		{
			this.resetTimer = 0f;
			PersistentSingleton<GameManager>.Instance.Reset();
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x0000358E File Offset: 0x0000178E
	private IEnumerator DoAmbientUpdate(float deltaTime)
	{
		foreach (SystemMapData systemMapData in GamePlayer.current.map.allSystems)
		{
			systemMapData.AmbientUpdate(deltaTime);
			yield return null;
		}
		IEnumerator<SystemMapData> enumerator = null;
		using (IEnumerator<Mission> enumerator2 = GamePlayer.current.allMissions.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				Mission mission = enumerator2.Current;
				MapPointOfInterest dynamicPointOfInterest = mission.dynamicPointOfInterest;
				if (dynamicPointOfInterest != null)
				{
					dynamicPointOfInterest.AmbientUpdate(deltaTime);
				}
			}
			yield break;
		}
		yield break;
		yield break;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000359D File Offset: 0x0000179D
	private IEnumerator TravelWhenReady()
	{
		yield return new WaitUntil(() => this.spaceShip.rigidbody != null && Singleton<TravelManager>.Instance.IsLocalPoiReady());
		TravelManager instance = Singleton<TravelManager>.Instance;
		List<MapPointOfInterest> waypoints = GamePlayer.current.waypoints;
		instance.SetRouteToPOI(waypoints[waypoints.Count - 1]);
		yield break;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000035AC File Offset: 0x000017AC
	public void ReinitPlayerSpaceship()
	{
		SpacestationExteriorManager instance = SpacestationExteriorManager.Instance;
		if (instance.currentDockingOption == null)
		{
			instance.currentDockingOption = SpacestationExteriorManager.Instance.FindDockingOption(this.spaceShip);
		}
		if (this.dockingCoroutine != null)
		{
			base.StopCoroutine(this.dockingCoroutine);
		}
		this.dockingCoroutine = base.StartCoroutine(this.ReinitPlayerSpaceshipRoutine());
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00003603 File Offset: 0x00001803
	private IEnumerator ReinitPlayerSpaceshipRoutine()
	{
		DockingOption currentDockingOption = SpacestationExteriorManager.Instance.currentDockingOption;
		SpaceShipData currentSpaceShip = GamePlayer.current.currentSpaceShip;
		Vector2 vector = Vector2.zero;
		vector = currentDockingOption.dockingTransform.position;
		DockingOption newDockingOption = null;
		if (currentSpaceShip.shipClass.shipRoleType.GetDockingOptionSize() != currentDockingOption.dockingOptionSize)
		{
			newDockingOption = SpacestationExteriorManager.Instance.FindClosestDockingOption(vector, currentSpaceShip.shipClass);
			SpacestationExteriorManager.Instance.currentDockingOption = newDockingOption;
			vector = newDockingOption.dockingTransform.position;
		}
		Debug.Log("Reinit: " + currentSpaceShip.shipClass + ", id: " + this.spaceShip.identifier);
		currentSpaceShip.faction = Faction.player;
		if (this.spaceShip != null)
		{
			bool flag = currentSpaceShip == this.spaceShip.spaceShipData;
		}
		this.GenerateSpaceship(currentSpaceShip, vector, true);
		yield return new WaitForEndOfFrame();
		this.spaceShip.ToggleStateForDocking(false);
		yield return new WaitUntil(() => this.spaceShip.rigidbody);
		if (newDockingOption)
		{
			currentDockingOption.ResetDockingOption();
			newDockingOption.AssignSpaceshipForDocking(this.spaceShip, false);
		}
		else
		{
			currentDockingOption.AssignSpaceshipForDocking(this.spaceShip, true);
		}
		if (!string.IsNullOrEmpty(currentSpaceShip.skillLoadout))
		{
			GamePlayer.current.commander.SetSelectedLoadout(currentSpaceShip.skillLoadout);
		}
		AbilityHud.instance.ResetHud(true);
		SidePanel.instance.RefreshIfOpen();
		yield return null;
		yield break;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003614 File Offset: 0x00001814
	public void GeneratePlayerSpaceship(Vector2 spawnLocation, int level, string ship = "Chisel Mk I SN", bool switchShip = false)
	{
		if (ship == "Chisel Mk I SN" && GamePlayer.current.starterSpaceshipName != "")
		{
			ship = GamePlayer.current.starterSpaceshipName;
		}
		SpaceShipData spaceShipData = GamePlayer.current.currentSpaceShip;
		spaceShipData.faction = Faction.player;
		if (spaceShipData == null || switchShip)
		{
			GamePlayer.current.RemoveSpaceShipData();
			spaceShipData = new SpaceShipData(ship, true, null);
			Debug.Log("Create ship for level: " + level.ToString());
			spaceShipData.LoadDefaultEquipment(level, 0f, ship, null, null, null, false, null);
			spaceShipData.positionData.position = spawnLocation;
		}
		this.GenerateSpaceship(spaceShipData, spawnLocation, false);
	}

	// Token: 0x06000046 RID: 70 RVA: 0x000036DC File Offset: 0x000018DC
	private void GenerateSpaceship(SpaceShipData spaceShipData, Vector2 spawnLocation, bool dockedSpawn = false)
	{
		Vector2 destination = this.spaceShip ? (Vector2)this.spaceShip.transform.position : spawnLocation;
		bool freshSpawn = this.spaceShip == null;
		freshSpawn = false;
		Quaternion rotation = Quaternion.identity;
		if (dockedSpawn)
		{
			Transform dockingTransform = SpacestationExteriorManager.Instance.currentDockingOption.dockingTransform;
			rotation = dockingTransform.rotation;
			if (this.spaceShip.dockSideways)
			{
				rotation = dockingTransform.rotation * Quaternion.Euler(0f, 0f, 90f);
			}
		}
		else if (spaceShipData.positionData != null)
		{
			spawnLocation = (this.spaceShip ? (Vector2)this.spaceShip.transform.position : spaceShipData.positionData.position);
			rotation = (this.spaceShip ? this.spaceShip.transform.rotation : Quaternion.Euler(new Vector3(0f, 0f, spaceShipData.positionData.rotation)));
		}
		this.DestroyCurrentShip();
		this.SetSpaceShipData(spaceShipData);
		this.InstantiateShip(spaceShipData, spawnLocation, rotation, false);
		this.SetDestination(freshSpawn, destination);
		this.UpdateCameraTarget(freshSpawn);
		this.SetupHUD();
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003815 File Offset: 0x00001A15
	private void DestroyCurrentShip()
	{
		if (this.spaceShip != null)
		{
			UnityEngine.Object.Destroy(this.spaceShip.gameObject);
			GamePlayer current = GamePlayer.current;
			if (current == null)
			{
				return;
			}
			current.activeToggles.Clear();
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x0000384C File Offset: 0x00001A4C
	private void ReinitShip(SpaceShipData spaceShipData, Vector2 position)
	{
		Transform dockingTransform = SpacestationExteriorManager.Instance.currentDockingOption.dockingTransform;
		Quaternion rotation = dockingTransform.rotation;
		if (this.spaceShip.dockSideways)
		{
			rotation = dockingTransform.rotation * Quaternion.Euler(0f, 0f, 90f);
		}
		this.spaceShip.transform.SetPositionAndRotation(position, rotation);
		AbstractEquipment[] componentsInChildren = this.spaceShip.GetComponentsInChildren<AbstractEquipment>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.DestroyImmediate(componentsInChildren[i].gameObject);
		}
		this.SetSpaceShipData(spaceShipData);
		this.spaceShip.SetData(spaceShipData, true, true);
		this.spaceShip.InitModules();
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			this.spaceShip.DisableCollisionWith(spaceShip);
		}
		this.UpdateCameraTarget(false);
		this.SetupHUD();
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00003958 File Offset: 0x00001B58
	private void SetSpaceShipData(SpaceShipData spaceShipData)
	{
		GamePlayer.current.SetSpaceShipData(spaceShipData);
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003968 File Offset: 0x00001B68
	private SpaceShip InstantiateShip(SpaceShipData spaceShipData, Vector2 spawnLocation, Quaternion rotation, bool fleet = false)
	{
		string str = "Instantiate ship at: ";
		Vector2 vector = spawnLocation;
		Debug.Log(str + vector.ToString());
		SpaceShip shipClass = spaceShipData.shipClass;
		SpaceShip spaceShip = UnityEngine.Object.Instantiate<SpaceShip>(shipClass, spawnLocation, rotation, base.transform);
		spaceShip.SetDisplayName(shipClass.name);
		spaceShip.SetData(spaceShipData, true, true);
		if (fleet)
		{
			spaceShipData.GiveAmmo(1000000, false);
		}
		spaceShip.InitModules();
		if (!fleet)
		{
			this.spaceShip = spaceShip;
			using (List<SpaceShip>.Enumerator enumerator = this.fleetSpaceShips.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SpaceShip spaceShip2 = enumerator.Current;
					this.spaceShip.DisableCollisionWith(spaceShip2);
				}
				return spaceShip;
			}
		}
		this.fleetSpaceShips.Add(spaceShip);
		this.spaceShip.DisableCollisionWith(spaceShip);
		return spaceShip;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003A4C File Offset: 0x00001C4C
	public bool IsFleetShip(SpaceShip fleetShip)
	{
		return this.fleetSpaceShips.Contains(fleetShip);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00003A5C File Offset: 0x00001C5C
	public void GenerateFleet()
	{
		foreach (SpaceShipData spaceShipData in GamePlayer.current.activeFleet)
		{
			if (spaceShipData.repairTimer <= 0f)
			{
				this.InstantiateShip(spaceShipData, spaceShipData.positionData.position, Quaternion.Euler(new Vector3(0f, 0f, spaceShipData.positionData.rotation)), true);
			}
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003AEC File Offset: 0x00001CEC
	public SpaceShip CreateMercenary(Vector2 spawnPosition, Mercenary mercenary)
	{
		Debug.Log("Creating merc: " + mercenary.ship);
		SpaceShipData spaceShipData = new SpaceShipData(mercenary.ship, false, Faction.player);
		int level = (MapPointOfInterest.current != null) ? MapPointOfInterest.current.level : GamePlayer.current.level;
		spaceShipData.LoadDefaultEquipment(level, 1f, mercenary.seed, new GameplayType?(mercenary.GetGameplayType()), new Rarity?(mercenary.rarity), null, false, null);
		spaceShipData.RemoveModuleOfType(EquipmentSlot.TractorBeam);
		spaceShipData.autoActions = "Wingman";
		spaceShipData.unitRank = mercenary.GetUnitRank();
		spaceShipData.positionData.position = spawnPosition;
		spaceShipData.positionData.rotation = this.spaceShip.transform.rotation.eulerAngles.z;
		spaceShipData.commanderData = mercenary;
		GamePlayer.current.AddToFleet(spaceShipData);
		return this.SpawnFleetShipAtPlayer(spaceShipData);
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003BE1 File Offset: 0x00001DE1
	public void RemoveFleetShip(SpaceShip ship)
	{
		if (ship == null)
		{
			return;
		}
		if (this.fleetSpaceShips.Contains(ship))
		{
			this.fleetSpaceShips.Remove(ship);
		}
		UnityEngine.Object.Destroy(ship.gameObject);
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003C13 File Offset: 0x00001E13
	public void RemoveMercenary()
	{
		if (this.fleetSpaceShips.Count == 0)
		{
			return;
		}
		this.RemoveFleetShip(this.fleetSpaceShips[0]);
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00003C38 File Offset: 0x00001E38
	public SpaceShip SpawnFleetShipAtPlayer(SpaceShipData data)
	{
		Vector2 formationPos = data.shipClass.GetFormationPos(this.spaceShip);
		Vector2 spawnLocation = (Vector2)this.spaceShip.transform.position + formationPos * 20f;
		SpaceShip spaceShip = this.InstantiateShip(data, spawnLocation, Quaternion.identity, true);
		UnitPositionData positionData = this.spaceShip.spaceShipData.positionData;
		Vector2 targetPosition = new Vector2(positionData.position.x + formationPos.x, positionData.position.y + formationPos.y);
		spaceShip.SetCurrentHP(true);
		spaceShip.StartLandNpcAtPoiCoroutine(targetPosition);
		return spaceShip;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00003CD8 File Offset: 0x00001ED8
	public void SetFleetTurretState(bool enabled)
	{
		this.spaceShip.SetTurretState(enabled);
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			spaceShip.SetTurretState(enabled);
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00003D38 File Offset: 0x00001F38
	public void SetFleetActive(bool active, bool forceReset = false)
	{
		this.spaceShip.gameObject.SetActive(active);
		if (this.spaceShip.droneBayModule && forceReset)
		{
			this.spaceShip.droneBayModule.ForceResetDrones();
		}
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			spaceShip.gameObject.SetActive(active);
			if (spaceShip.droneBayModule && forceReset)
			{
				spaceShip.droneBayModule.ForceResetDrones();
			}
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003DE0 File Offset: 0x00001FE0
	public void SetFleetEngineState(bool isEnabled = false, bool thrustersEnabled = true)
	{
		this.spaceShip.SetEngineState(isEnabled, thrustersEnabled);
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			spaceShip.SetEngineState(isEnabled, thrustersEnabled);
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003E40 File Offset: 0x00002040
	public void SetFleetRotation(Quaternion rotation)
	{
		this.spaceShip.transform.rotation = rotation;
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			spaceShip.transform.rotation = rotation;
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00003EA8 File Offset: 0x000020A8
	public void SetFleetMaskInteraction(SpriteMaskInteraction interaction = SpriteMaskInteraction.None)
	{
		this.spaceShip.SetMaskInteraction(interaction);
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			spaceShip.SetMaskInteraction(interaction);
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00003F08 File Offset: 0x00002108
	public void GiveFleetJumpGateImpulse(TravelDirection travelDirection)
	{
		this.spaceShip.GiveJumpGateImpulse(travelDirection);
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			spaceShip.GiveJumpGateImpulse(travelDirection);
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00003F68 File Offset: 0x00002168
	public void SetFleetPosition(Vector2 position)
	{
		this.spaceShip.transform.position = position;
		this.spaceShip.rigidbody.position = position;
		foreach (SpaceShip spaceShip in this.fleetSpaceShips)
		{
			Vector2 relativeFormationPos = (spaceShip.autoActions as WingmanActions).relativeFormationPos;
			spaceShip.transform.position = position + relativeFormationPos;
			spaceShip.rigidbody.position = position + relativeFormationPos;
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004014 File Offset: 0x00002214
	private void SetDestination(bool freshSpawn, Vector2 destination)
	{
		if (freshSpawn)
		{
			this.spaceShip.SetOverrideDestination(destination, true, false, false);
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00004028 File Offset: 0x00002228
	private void UpdateCameraTarget(bool freshSpawn)
	{
		if (!freshSpawn)
		{
			this.cameraMovement.SetTarget(this.spaceShip, true);
			return;
		}
		this.cameraMovement.LoseTarget();
	}

	// Token: 0x0600005A RID: 90 RVA: 0x0000404B File Offset: 0x0000224B
	private void SetupHUD()
	{
		this.SetupEnemyStatusBar();
		this.SetupAbilityHUD();
		this.SetupTurretHUD();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x0000405F File Offset: 0x0000225F
	private void SetupAbilityHUD()
	{
		HudManager.Instance.abilityHud.ResetHud(true);
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00004071 File Offset: 0x00002271
	private void SetupTurretHUD()
	{
		HudManager.Instance.turretControl.ResetHud();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00004082 File Offset: 0x00002282
	private void SetupEnemyStatusBar()
	{
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004084 File Offset: 0x00002284
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04000039 RID: 57
	public const float AmbientUpdateTime = 10f;

	// Token: 0x0400003A RID: 58
	public const float AutoResetTime = 28800f;

	// Token: 0x0400003E RID: 62
	public static GameplayManager Instance;

	// Token: 0x0400003F RID: 63
	private bool _initialized;

	// Token: 0x04000040 RID: 64
	private float autosaveTimer;

	// Token: 0x04000041 RID: 65
	private float ambientUpdateTimer;

	// Token: 0x04000042 RID: 66
	[SerializeField]
	private ItemSaleInfo itemForSalePrefab;

	// Token: 0x04000043 RID: 67
	[SerializeField]
	public TargetableHealthBar healthBarPrefab;

	// Token: 0x04000044 RID: 68
	private float resetTimer;

	// Token: 0x04000045 RID: 69
	private InputAction moveAction;

	// Token: 0x04000046 RID: 70
	private InputAction rotatieAction;

	// Token: 0x04000047 RID: 71
	private bool inputActionUsed;

	// Token: 0x04000048 RID: 72
	private Coroutine dockingCoroutine;
}
