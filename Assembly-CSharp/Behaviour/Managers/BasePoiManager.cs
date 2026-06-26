using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Hazard;
using Behaviour.Mining;
using Behaviour.Spacestation.Docking;
using Behaviour.Travel;
using Behaviour.UI.HUD;
using Behaviour.UI.Main_Menu;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Combat;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Hazard;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World.System;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Managers
{
	// Token: 0x02000301 RID: 769
	public abstract class BasePoiManager : MonoBehaviour
	{
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000AAC0D File Offset: 0x000A8E0D
		public static BasePoiManager current
		{
			get
			{
				TravelManager instance = Singleton<TravelManager>.Instance;
				if (instance == null)
				{
					return null;
				}
				return instance.localPoiManager;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x000AAC1F File Offset: 0x000A8E1F
		// (set) Token: 0x06001C4C RID: 7244 RVA: 0x000AAC27 File Offset: 0x000A8E27
		public Rect worldCoordinates { get; protected set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x000AAC30 File Offset: 0x000A8E30
		// (set) Token: 0x06001C4E RID: 7246 RVA: 0x000AAC38 File Offset: 0x000A8E38
		public MapPointOfInterest poi { get; protected set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x000AAC41 File Offset: 0x000A8E41
		protected virtual float securityPatrolMultiplier
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x000AAC48 File Offset: 0x000A8E48
		public string sceneName
		{
			get
			{
				MapPointOfInterest poi = this.poi;
				return ((poi != null) ? poi.sceneName : null) ?? "Space";
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x000AAC65 File Offset: 0x000A8E65
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x000AAC6D File Offset: 0x000A8E6D
		public bool initializedAndReady { get; protected set; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x000AAC76 File Offset: 0x000A8E76
		public virtual bool xRestrictionOnSpaceship
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000AAC79 File Offset: 0x000A8E79
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x000AAC81 File Offset: 0x000A8E81
		public Coroutine initCoroutine { get; private set; }

		// Token: 0x06001C56 RID: 7254 RVA: 0x000AAC8C File Offset: 0x000A8E8C
		protected virtual void Awake()
		{
			this.screenSize = Singleton<BackdropManager>.Instance.screenSizeGame;
			Singleton<TravelManager>.Instance.RegisterLocalPoiManager(this);
			GameplayManager instance = GameplayManager.Instance;
			bool flag;
			if (instance == null)
			{
				flag = false;
			}
			else
			{
				SpaceShip spaceShip = instance.spaceShip;
				bool? flag2 = (spaceShip != null) ? new bool?(spaceShip.travelling) : null;
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				TravelManager instance2 = Singleton<TravelManager>.Instance;
				if (((instance2 != null) ? instance2.localTarget : null) != null)
				{
					this.poi = Singleton<TravelManager>.Instance.localTarget;
					goto IL_91;
				}
			}
			this.poi = GamePlayer.current.currentPointOfInterest;
			IL_91:
			MapPointOfInterest poi = this.poi;
			this.lastVisitedTimeAtLoad = ((poi != null) ? new float?(poi.lastVisitedTime) : null);
			this.spaceShip = GameplayManager.Instance.spaceShip;
			this.lineRenderer = base.gameObject.AddComponent<LineRenderer>();
			this.lineRenderer.material = Materials.PoiBorder;
			this.lineRenderer.startWidth = 0.2f;
			this.lineRenderer.endWidth = 0.2f;
			this.lineRenderer.positionCount = 4;
			this.lineRenderer.loop = true;
			if (GamePlayer.current.IsInSandBox() && this.poi != null && !this.poi.IsStoryMissionPoi() && !this.poi.HasPatrolUnits())
			{
				if (!(this.poi is Source.Galaxy.POI.Combat) && (this.poi is IndustryStation || Faction.policeGuild.IsEnemy(this.poi.faction)))
				{
					this.hasSecurityPatrol = false;
					return;
				}
				int jumpsToNearestSpaceStation = this.poi.system.GetJumpsToNearestSpaceStation(Faction.policeGuild);
				float num = 0.3f * (1f - (float)jumpsToNearestSpaceStation / 10f);
				num *= this.securityPatrolMultiplier;
				this.hasSecurityPatrol = SeededRandom.Global.RandomBool(Mathf.Clamp(num, 0f, 1f));
				Debug.Log(string.Concat(new string[]
				{
					"Jumps to nearest station: ",
					jumpsToNearestSpaceStation.ToString(),
					", chance for security: ",
					num.ToString(),
					", HasSecurity ",
					this.hasSecurityPatrol.ToString()
				}));
			}
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x000AAECF File Offset: 0x000A90CF
		protected virtual void Start()
		{
			this.initCoroutine = base.StartCoroutine(this.Init());
			if (this.poi != null)
			{
				Register.AddVisitedSystem(this.poi.system.guid);
			}
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x000AAF00 File Offset: 0x000A9100
		protected virtual void Update()
		{
			if (this.poi != null)
			{
				this.updateTimer -= Time.deltaTime;
				if (this.updateTimer < 0f)
				{
					this.StorePositionData();
					this.CheckForIncomingReinforcements();
					this.updateTimer = 0.5f;
				}
				if (this.SpaceShipIsAtPoi())
				{
					this.poi.lastVisitedTime = (float)GamePlayer.current.elapsedTime;
				}
				this.DrawBoundaries();
			}
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000AAF6F File Offset: 0x000A916F
		private IEnumerator Init()
		{
			this.SetScenePosition();
			yield return this.InitializePoi();
			this.SetWorldCoordinates();
			Singleton<BackdropManager>.Instance.InitializeParallax();
			yield return this.InitializationComplete();
			this.initializedAndReady = true;
			this.initCoroutine = null;
			yield break;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000AAF7E File Offset: 0x000A917E
		protected virtual IEnumerator InitializePoi()
		{
			if (this.poi == null)
			{
				yield break;
			}
			foreach (PersistableData data in new List<PersistableData>(this.poi.GetPersistables()))
			{
				this.AddToWorld(data);
				yield return null;
			}
			List<PersistableData>.Enumerator enumerator = default(List<PersistableData>.Enumerator);
			foreach (AbstractUnitData data2 in this.poi.GetUnits(false))
			{
				this.AddToWorld(data2, null, false);
				yield return null;
			}
			IEnumerator<AbstractUnitData> enumerator2 = null;
			if (this.poi is Source.Galaxy.POI.Mining || this.poi.hasAsteroids)
			{
				JumpGate jumpGate = this.poi as JumpGate;
				if (jumpGate != null)
				{
					bool flag = jumpGate.GetTravelDirection() == TravelDirection.Left;
					jumpGate.InitializeAsteroids(!flag, true);
				}
				this.poi.InitializeAsteroids(false, false);
			}
			yield return null;
			yield break;
			yield break;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000AAF90 File Offset: 0x000A9190
		public GameObject AddToWorld(PersistableData data)
		{
			GameObject gameObject = data.AddToWorld(this);
			if (gameObject)
			{
				PersistableUpdater persistableUpdater = gameObject.GetComponent<PersistableUpdater>();
				if (!persistableUpdater)
				{
					persistableUpdater = gameObject.gameObject.AddComponent<PersistableUpdater>();
				}
				persistableUpdater.data = data;
			}
			return gameObject;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000AAFD0 File Offset: 0x000A91D0
		public AbstractUnit AddToWorld(AbstractUnitData data, string seed = null, bool exactLevel = false)
		{
			AbstractUnit abstractUnit = UnityEngine.Object.Instantiate<AbstractUnit>(data.unitDefinition, data.positionData.position, Quaternion.Euler(new Vector3(0f, 0f, data.positionData.rotation)), base.transform);
			if (data.faction == null)
			{
				data.faction = Faction.player;
			}
			data.LoadDefaultEquipment((data.level > 0) ? data.level : this.poi.level, -1f, seed, null, null, null, exactLevel, null);
			abstractUnit.SetData(data, true, true);
			data.GiveAmmo(1000000, false);
			abstractUnit.InitModules();
			return abstractUnit;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000AB098 File Offset: 0x000A9298
		protected virtual void SetWorldCoordinates()
		{
			if (this.poi != null)
			{
				this.worldCoordinates = this.poi.GetWorldBounds();
				return;
			}
			this.worldCoordinates = new Rect(base.transform.position.x - 10f, base.transform.position.y - 10f, 20f, 20f);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000AB100 File Offset: 0x000A9300
		public virtual void SpaceshipHasArrived()
		{
			this.lineRenderer.enabled = true;
			AbstractUnit[] componentsInChildren = base.GetComponentsInChildren<AbstractUnit>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				AutoActions autoActions = componentsInChildren[i].autoActions;
				if (autoActions != null)
				{
					autoActions.SpaceShipHasArrived();
				}
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000AB144 File Offset: 0x000A9344
		private void SetScenePosition()
		{
			MapPointOfInterest poi = this.poi;
			Vector2 v = (poi != null) ? poi.GetWorldPosition() : GameplayManager.Instance.spaceShip.transform.position;
			base.transform.position = v;
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000AB18D File Offset: 0x000A938D
		protected virtual IEnumerator InitializationComplete()
		{
			bool travelling = this.spaceShip.travelling;
			if (this.hasSecurityPatrol)
			{
				this.CreateSecurityPatrol();
			}
			LoadingScreen.Hide(false);
			yield return null;
			yield break;
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000AB19C File Offset: 0x000A939C
		public virtual Vector2 GetWorldPositionToTravelTo()
		{
			return this.poi.GetWorldPositionToTravelTo(null);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000AB1BD File Offset: 0x000A93BD
		public Vector2 GetLocationDifferenceForSpaceship()
		{
			return (Vector2)this.spaceShip.transform.position - this.GetWorldPositionToTravelTo();
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000AB1DF File Offset: 0x000A93DF
		public bool IsPOI(MapPointOfInterest poi)
		{
			return this.poi == poi;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000AB1EC File Offset: 0x000A93EC
		public void StoreLastX()
		{
			if (this.spaceShip != null && this.worldCoordinates.Contains(this.spaceShip.transform.position))
			{
				MapPointOfInterest poi = this.poi;
				if (poi == null)
				{
					return;
				}
				poi.StoreLastX(base.transform.position);
			}
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000AB248 File Offset: 0x000A9448
		public bool SpaceShipIsAtPoi()
		{
			return this.spaceShip != null && this.worldCoordinates.Contains(this.spaceShip.transform.position);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000AB284 File Offset: 0x000A9484
		public void CheckForIncomingReinforcements()
		{
			MapTriggeredPayload nextTriggeredPayload = this.poi.nextTriggeredPayload;
			if (!Singleton<TravelManager>.Instance.TravelActive() && nextTriggeredPayload != null && nextTriggeredPayload.HasEnemyReinforcements() && nextTriggeredPayload.triggerTime > 1.5f)
			{
				HudManager.Instance.ShowSubtleTimerInfo(nextTriggeredPayload.triggerTime, Translation.Translate("@TimerReinforcements", Array.Empty<object>()), 2);
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000AB2E4 File Offset: 0x000A94E4
		public void StorePositionData()
		{
			SpaceShipData currentSpaceShip = GamePlayer.current.currentSpaceShip;
			if (currentSpaceShip != null)
			{
				UnitPositionData positionData = currentSpaceShip.positionData;
				SpaceShipData currentSpaceShip2 = GamePlayer.current.currentSpaceShip;
				positionData.SetDataFromRigidbody((currentSpaceShip2 != null) ? currentSpaceShip2.unit : null);
			}
			foreach (AbstractUnitData abstractUnitData in this.poi.GetUnits(false))
			{
				abstractUnitData.positionData.SetDataFromRigidbody(abstractUnitData.unit);
			}
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000AB374 File Offset: 0x000A9574
		public SpaceShipData CreatePasserbyShipData(Vector2 spawnPosition, DockingOptionSize? size = null)
		{
			Faction faction = this.poi.faction;
			string str = "Faction for creating passerby ship: ";
			Faction faction2 = faction;
			Debug.Log(str + ((faction2 != null) ? faction2.ToString() : null));
			if (faction != Faction.stranded && !faction.IsEnemy(Faction.player) && SeededRandom.Global.RandomBool(0.5f))
			{
				List<Faction> list = new List<Faction>();
				foreach (Faction faction3 in Faction.all)
				{
					if (faction3 != Faction.puppeteers && faction3 != Faction.player && faction3 != faction && !faction3.IsEnemy(faction) && !faction3.IsEnemy(Faction.player))
					{
						list.Add(faction3);
					}
				}
				if (list.Count > 0)
				{
					faction = SeededRandom.Global.Choose<Faction>(list);
				}
			}
			int num = 0;
			SpaceShip randomNPCShipType;
			do
			{
				num++;
				randomNPCShipType = faction.GetRandomNPCShipType(this.poi.level, this.poi.minUnitPoints, this.poi.maxUnitPoints * 3, null);
				if (!(randomNPCShipType == null))
				{
					if (size != null)
					{
						DockingOptionSize dockingOptionSize = randomNPCShipType.shipRoleType.GetDockingOptionSize();
						DockingOptionSize? dockingOptionSize2 = size;
						if (!(dockingOptionSize == dockingOptionSize2.GetValueOrDefault() & dockingOptionSize2 != null))
						{
							goto IL_150;
						}
					}
					if (randomNPCShipType.hasCommander)
					{
						break;
					}
				}
				IL_150:;
			}
			while (num < 50);
			if (randomNPCShipType == null)
			{
				return null;
			}
			Debug.Log("Creating: " + randomNPCShipType);
			SpaceShipData spaceShipData = new SpaceShipData(randomNPCShipType, false, faction);
			spaceShipData.unitRank = UnitRankHelper.GetRandomUnitRankForLevel(this.poi.level, false);
			spaceShipData.positionData.position = spawnPosition;
			if (this.poi is SpaceStation)
			{
				spaceShipData.alwaysFriendly = true;
			}
			return spaceShipData;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000AB548 File Offset: 0x000A9748
		public void CreateSecurityPatrol()
		{
			Faction faction = Faction.policeGuild;
			if (SystemMapData.current.storyteller is ConquestSystem && SeededRandom.Global.RandomBool(0.66f))
			{
				faction = SystemMapData.current.faction;
			}
			MapPointOfInterest poi = this.poi;
			float pointsScale = 1.5f;
			Faction f = faction;
			List<AbstractUnitData> list = poi.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), f, 0, 0, 1, 2, null);
			Vector2 center = this.worldCoordinates.center;
			center.x += (SeededRandom.Global.RandomBool(0.5f) ? (this.worldCoordinates.width / 2f) : (-this.worldCoordinates.width / 2f));
			foreach (AbstractUnitData abstractUnitData in list)
			{
				abstractUnitData.autoActions = "SecurityPatrol";
				abstractUnitData.positionData.position = center;
				if (this.poi is SpaceStation)
				{
					abstractUnitData.alwaysFriendly = true;
				}
				center.x += 4f;
			}
			float spawnDelay = (float)SeededRandom.Global.RandomRange(0, 15);
			this.poi.AddTriggeredSpawn(list, spawnDelay, 0, false, false);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000AB6A8 File Offset: 0x000A98A8
		public void AddDamageInRadiusToNearbyAsteroids(DamageType damageType)
		{
			foreach (Asteroid asteroid in PhysicsInteraction.GetAsteroidsWithinRange(GameplayManager.Instance.spaceShip.transform.position, 10f))
			{
				((DamageInRadius)UnityEngine.Object.Instantiate<LocalHazard>(LocalHazard.Get("DamageInRadius"), asteroid.gameObject.transform)).Init(asteroid.gameObject, new HazardData
				{
					damageType = damageType,
					damageMultiplier = (float)(this.poi.level * 2),
					range = 5f,
					maxDamageFalloffPercentage = 0.5f
				}, 1f);
			}
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000AB778 File Offset: 0x000A9978
		private void DrawBoundaries()
		{
			if (Singleton<TravelManager>.Instance.TravelActive() || !BasePoiManager.current)
			{
				this.lineRenderer.enabled = false;
				return;
			}
			if (this.initializedAndReady && this.lineRenderer && this.spaceShip)
			{
				float distanceFromBorder = this.spaceShip.GetDistanceFromBorder();
				Color color = (distanceFromBorder < 1f) ? Color.red : ColorHelper.steamBlueLight2;
				color.a = Mathf.Lerp(0.05f, 0f, distanceFromBorder / 4f);
				this.lineRenderer.startColor = color;
				this.lineRenderer.endColor = color;
				this.lineRenderer.SetPositions(new Vector3[]
				{
					this.worldCoordinates.min,
					new Vector3(this.worldCoordinates.xMin, this.worldCoordinates.yMax),
					this.worldCoordinates.max,
					new Vector3(this.worldCoordinates.xMax, this.worldCoordinates.yMin)
				});
			}
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000AB8C0 File Offset: 0x000A9AC0
		private void OnDrawGizmos()
		{
			if (this.initializedAndReady)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawLine(this.worldCoordinates.min, new Vector3(this.worldCoordinates.xMin, this.worldCoordinates.yMax));
				Gizmos.DrawLine(new Vector3(this.worldCoordinates.xMin, this.worldCoordinates.yMax), this.worldCoordinates.max);
				Gizmos.DrawLine(this.worldCoordinates.max, new Vector3(this.worldCoordinates.xMax, this.worldCoordinates.yMin));
				Gizmos.DrawLine(new Vector3(this.worldCoordinates.xMax, this.worldCoordinates.yMin), this.worldCoordinates.min);
			}
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000AB9C8 File Offset: 0x000A9BC8
		public void PlayerSetMissionHostile()
		{
			if (this.hostilitiesTriggered)
			{
				return;
			}
			this.hostilitiesTriggered = true;
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (mission.enemyFaction != null && !mission.enemyFaction.IsEnemy(Faction.player) && mission.GetActivePoi(false) == this.poi)
				{
					foreach (AbstractUnitData abstractUnitData in this.poi.GetUnits(true))
					{
						if (abstractUnitData.faction == mission.enemyFaction)
						{
							abstractUnitData.playerHostile = true;
						}
					}
				}
			}
		}

		// Token: 0x04001195 RID: 4501
		protected Vector2 screenSize;

		// Token: 0x04001197 RID: 4503
		protected bool hasSecurityPatrol;

		// Token: 0x04001198 RID: 4504
		protected SpaceShip spaceShip;

		// Token: 0x0400119A RID: 4506
		private LineRenderer lineRenderer;

		// Token: 0x0400119B RID: 4507
		private float updateTimer;

		// Token: 0x0400119C RID: 4508
		protected float? lastVisitedTimeAtLoad = new float?(0f);

		// Token: 0x0400119E RID: 4510
		private bool hostilitiesTriggered;
	}
}
