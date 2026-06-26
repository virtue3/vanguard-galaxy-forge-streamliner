using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Bootstrap;
using Behaviour.Crew;
using Behaviour.Item.Usable;
using Behaviour.Travel;
using Behaviour.UI.HUD;
using Behaviour.UI.Missions;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Travel;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation;
using Source.Simulation.World;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using Unity.VisualScripting;
using UnityEngine;

namespace Behaviour.Managers
{
	// Token: 0x02000306 RID: 774
	public class TravelManager : Behaviour.Util.Singleton<TravelManager>
	{
		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x000ACAFD File Offset: 0x000AACFD
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x000ACB05 File Offset: 0x000AAD05
		public MapPointOfInterest targetPoi { get; private set; }

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x000ACB0E File Offset: 0x000AAD0E
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x000ACB16 File Offset: 0x000AAD16
		public MapPointOfInterest localTarget { get; private set; }

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x000ACB1F File Offset: 0x000AAD1F
		// (set) Token: 0x06001C96 RID: 7318 RVA: 0x000ACB27 File Offset: 0x000AAD27
		public Vector2 worldLocationToTravelTo { get; private set; }

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x000ACB30 File Offset: 0x000AAD30
		// (set) Token: 0x06001C98 RID: 7320 RVA: 0x000ACB38 File Offset: 0x000AAD38
		public BasePoiManager localPoiManager { get; private set; }

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x000ACB41 File Offset: 0x000AAD41
		// (set) Token: 0x06001C9A RID: 7322 RVA: 0x000ACB49 File Offset: 0x000AAD49
		public bool usingJumpgate { get; private set; }

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x000ACB52 File Offset: 0x000AAD52
		// (set) Token: 0x06001C9C RID: 7324 RVA: 0x000ACB5A File Offset: 0x000AAD5A
		public float totalDistance { get; private set; }

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x000ACB63 File Offset: 0x000AAD63
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x000ACB6B File Offset: 0x000AAD6B
		public float remainingDistance { get; private set; }

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x000ACB74 File Offset: 0x000AAD74
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x000ACB7C File Offset: 0x000AAD7C
		public bool isWarping { get; private set; }

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x000ACB85 File Offset: 0x000AAD85
		public bool emergencyJumpActive
		{
			get
			{
				return GamePlayer.current.emergencyJump;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001CA2 RID: 7330 RVA: 0x000ACB91 File Offset: 0x000AAD91
		public bool fastLaneTravelActive
		{
			get
			{
				return this.travelMultiplier > 1f;
			}
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000ACBA0 File Offset: 0x000AADA0
		private void Update()
		{
			GameplayManager instance = GameplayManager.Instance;
			UnityEngine.Object exists;
			if (instance == null)
			{
				exists = null;
			}
			else
			{
				SpaceShip spaceShip = instance.spaceShip;
				exists = ((spaceShip != null) ? spaceShip.rigidbody : null);
			}
			if (!exists)
			{
				return;
			}
			if (this.delayTravelAttempt > 0f)
			{
				this.delayTravelAttempt -= Time.deltaTime;
			}
			if (this.isWarping)
			{
				this.CalculateWarpFuel();
				MapPointOfInterest targetPoi = this.targetPoi;
				if (targetPoi != null && !targetPoi.isDynamicPoi)
				{
					this.dynamicEventTimer -= Time.deltaTime;
					if (this.dynamicEventTimer <= 0f)
					{
						this.dynamicEventTimer = 5f;
						if (GamePlayer.current.dynamicEventTimer <= 0f && SeededRandom.Global.RandomBool(0.25f))
						{
							this.TriggerDynamicEvent();
							GamePlayer.current.ResetDynamicEventTimer(-1f);
						}
					}
				}
			}
			if (GamePlayer.current.currentBounty != null)
			{
				this.UpdateBounty(GamePlayer.current.currentBounty);
				return;
			}
			if (GamePlayer.current.currentPatrol != null)
			{
				this.UpdatePatrol(GamePlayer.current.currentPatrol);
				return;
			}
			if (GamePlayer.current.currentIndustry != null)
			{
				this.UpdateIndustry(GamePlayer.current.currentIndustry);
			}
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000ACCD0 File Offset: 0x000AAED0
		private void UpdatePatrol(PatrolMission patrol)
		{
			if ((!patrol.steps[0].IsMissionPoi(this.targetPoi) && !patrol.steps[0].IsMissionPoi(MapPointOfInterest.current)) || patrol.steps[0].GetMissionPoi().leftPoi())
			{
				this.patrolAbandonTimer += Time.deltaTime;
				if (!this.emergencyJumpActive)
				{
					HudManager.Instance.ShowSubtleTimerInfo(this.patrolMaxTimer, Translation.Translate("@PatrolContinue", Array.Empty<object>()), 1);
				}
				if (this.patrolAbandonTimer > this.patrolMaxTimer)
				{
					this.patrolAbandonTimer = 0f;
					Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@PatrolAbandoned", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
					GamePlayer.current.RemoveMission(GamePlayer.current.currentPatrol, false);
					Behaviour.Util.Singleton<FocusedMissionHandler>.Current.ResetFocusedMission();
					return;
				}
			}
			else
			{
				HudManager.Instance.HideSubtleTimerInfo(1);
				this.patrolAbandonTimer = 0f;
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000ACDE4 File Offset: 0x000AAFE4
		private void UpdateBounty(BountyMission bounty)
		{
			if ((!bounty.steps[0].IsMissionPoi(this.targetPoi) && !bounty.steps[0].IsMissionPoi(MapPointOfInterest.current)) || bounty.steps[0].GetMissionPoi().leftPoi())
			{
				if (!this.emergencyJumpActive)
				{
					HudManager.Instance.ShowTimerInfo(10f - this.bountyAbandonTimer, Translation.Translate("@BountyContinue", Array.Empty<object>()));
				}
				this.bountyAbandonTimer += Time.deltaTime;
				if (this.bountyAbandonTimer > 10f)
				{
					this.bountyAbandonTimer = 0f;
					Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@BountyAbandoned", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
					GamePlayer.current.RemoveMission(GamePlayer.current.currentBounty, false);
					Behaviour.Util.Singleton<FocusedMissionHandler>.Current.ResetFocusedMission();
					return;
				}
			}
			else
			{
				HudManager.Instance.HideTimerInfo(0);
			}
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000ACEF4 File Offset: 0x000AB0F4
		private void UpdateIndustry(IndustryMission industry)
		{
			if ((!industry.steps[0].IsMissionPoi(this.targetPoi) && !industry.steps[0].IsMissionPoi(MapPointOfInterest.current)) || industry.steps[0].GetMissionPoi().leftPoi())
			{
				if (!this.emergencyJumpActive && !industry.failed)
				{
					HudManager.Instance.ShowTimerInfo(10f - this.bountyAbandonTimer, Translation.Translate("@IndustryContinue", Array.Empty<object>()));
				}
				this.bountyAbandonTimer += Time.deltaTime;
				if (this.bountyAbandonTimer > 10f || industry.failed)
				{
					this.bountyAbandonTimer = 0f;
					Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@IndustryAbandoned", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
					GamePlayer.current.RemoveMission(industry, false);
					Behaviour.Util.Singleton<FocusedMissionHandler>.Current.ResetFocusedMission();
					return;
				}
			}
			else
			{
				HudManager.Instance.HideTimerInfo(0);
			}
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000AD008 File Offset: 0x000AB208
		public void RegisterLocalPoiManager(BasePoiManager poiManager)
		{
			Debug.Log("Register local poi manager");
			this.localPoiManager = poiManager;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000AD01C File Offset: 0x000AB21C
		public bool CanWeTravel(MapPointOfInterest target = null)
		{
			if (this.usingJumpgate || this.delayTravelAttempt > 0f)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@TravelInJumpTunnel", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
				return false;
			}
			if (MapPointOfInterest.current == target)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@TravelAlreadyAtPOI", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(2f).Show();
				return false;
			}
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.canBeHomeStation && this.emergencyJumpActive)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSRepairsAreRequired", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (GamePlayer.current.currentSpaceShip.cargoUsed > GamePlayer.current.currentSpaceShip.cargoCapacity)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSInventoryFull", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			if (spaceShip.dummyEngine)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoEngine", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (!spaceShip.engine)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoEngine", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (!spaceShip.reactorModule)
			{
				Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoReactor", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			return true;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000AD1EC File Offset: 0x000AB3EC
		public bool IsLocalPoiReady()
		{
			return GamePlayer.current.currentPointOfInterest == null || (this.localPoiManager && this.localPoiManager.initializedAndReady);
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x000AD216 File Offset: 0x000AB416
		public bool AreWeLeaving()
		{
			BasePoiManager localPoiManager = this.localPoiManager;
			return ((localPoiManager != null) ? localPoiManager.poi : null) != this.localTarget;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000AD235 File Offset: 0x000AB435
		public void JumpToPOIFrom(JumpGate poi)
		{
			if (this.travelCoroutine == null)
			{
				base.StartCoroutine(this.JumpToSystem(poi));
				return;
			}
			Debug.Log("Jumping already");
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000AD258 File Offset: 0x000AB458
		public bool SetRouteToPOI(MapPointOfInterest poi)
		{
			Debug.Log("SetRouteToPOi: " + ((poi != null) ? poi.ToString() : null));
			GameplayManager.Instance.spaceShip.SetTurretState(false);
			if (this.travelCoroutine != null)
			{
				this.CancelTravel(null);
			}
			GameplayManager.Instance.spaceShip.manualInputTimer = 0f;
			this.targetPoi = poi;
			if (!(this.targetPoi is SpaceStation))
			{
				foreach (Mission mission in GamePlayer.current.missions)
				{
					if (this.targetPoi == mission.dynamicPointOfInterest)
					{
						Behaviour.Util.Singleton<FocusedMissionHandler>.Instance.SetMission(mission);
						break;
					}
				}
			}
			if (this.IsPOIInSystem(GamePlayer.current.currentSystem, poi))
			{
				GamePlayer.current.waypoints.Clear();
				GamePlayer.current.waypoints.Add(poi);
				this.travelCoroutine = base.StartCoroutine(this.StartTravel(poi));
				return true;
			}
			Debug.Log("Not in system: trying to calculating route...");
			List<MapPointOfInterest> list = this.GenerateShortestRoute(this.targetPoi);
			if (list.Count > 0)
			{
				GamePlayer.current.waypoints = list;
				this.travelCoroutine = base.StartCoroutine(this.StartTravel(GamePlayer.current.waypoints[0]));
				return true;
			}
			Behaviour.Util.Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@TravelLockedSystem", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(6f).Show();
			return false;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x000AD3FC File Offset: 0x000AB5FC
		public List<MapPointOfInterest> GenerateShortestRoute(MapPointOfInterest destination)
		{
			List<MapPointOfInterest> list = new List<MapPointOfInterest>();
			SystemMapData currentSystem = GamePlayer.current.currentSystem;
			Queue<ValueTuple<SystemMapData, List<object>>> queue = new Queue<ValueTuple<SystemMapData, List<object>>>();
			HashSet<string> hashSet = new HashSet<string>();
			queue.Enqueue(new ValueTuple<SystemMapData, List<object>>(currentSystem, new List<object>()));
			while (queue.Count > 0)
			{
				ValueTuple<SystemMapData, List<object>> valueTuple = queue.Dequeue();
				SystemMapData item = valueTuple.Item1;
				List<object> item2 = valueTuple.Item2;
				if (hashSet.Add(item.guid))
				{
					if (this.IsPOIInSystem(item, destination))
					{
						item2.Add(destination);
						list.AddRange(item2);
						return list;
					}
					foreach (JumpGate jumpGate in this.JumpGatesInSystem(item))
					{
						if (jumpGate.canUseJumpGate && jumpGate.targetSystem != null)
						{
							SystemMapData system = GalaxyMapData.current.GetSystem(jumpGate.targetSystemGuid);
							List<object> list2 = new List<object>(item2);
							if (!jumpGate.IsCurrentPoi())
							{
								list2.Add(jumpGate);
							}
							queue.Enqueue(new ValueTuple<SystemMapData, List<object>>(system, list2));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x000AD520 File Offset: 0x000AB720
		private List<JumpGate> JumpGatesInSystem(SystemMapData system)
		{
			List<JumpGate> list = new List<JumpGate>();
			foreach (MapPointOfInterest mapPointOfInterest in system.pointsOfInterest)
			{
				JumpGate jumpGate = mapPointOfInterest as JumpGate;
				if (jumpGate != null)
				{
					list.Add(jumpGate);
				}
			}
			return list;
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x000AD584 File Offset: 0x000AB784
		private bool IsPOIInSystem(SystemMapData system, MapPointOfInterest poi)
		{
			return system == poi.system;
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x000AD590 File Offset: 0x000AB790
		public bool CancelTravel(Vector2? moveToPosition = null)
		{
			if (this.emergencyJumpActive)
			{
				return false;
			}
			Debug.Log("Cancel travel");
			if (this.travelCoroutine != null)
			{
				foreach (Coroutine coroutine in this._prepCoroutines)
				{
					if (coroutine != null)
					{
						base.StopCoroutine(coroutine);
					}
				}
				this._prepCoroutines.Clear();
				HudManager.Instance.SetEchoRemarksStatus(false);
				base.StopCoroutine(this.travelCoroutine);
				if (BasePoiManager.current && BasePoiManager.current.initCoroutine != null)
				{
					BasePoiManager.current.StopCoroutine(BasePoiManager.current.initCoroutine);
				}
				this.travelCoroutine = null;
				this.loadingNextScene = false;
				if (GamePlayer.current.currentPointOfInterest != null)
				{
					GameplayManager.Instance.spaceShip.CancelJumpAway(moveToPosition);
					foreach (SpaceShip spaceShip in GameplayManager.Instance.fleetSpaceShips)
					{
						spaceShip.CancelJumpAway(null);
					}
				}
			}
			if (JumpGateManager.instance)
			{
				TheGate jumpGate = JumpGateManager.instance.jumpGate;
				if (((jumpGate != null) ? jumpGate.jumpingShip : null) == GameplayManager.Instance.spaceShip)
				{
					Debug.Log("Cancel jumpgate travel");
					JumpGateManager.instance.jumpGate.ClearJumpingShip();
					GameplayManager.Instance.spaceShip.jumpingProcedureEngaged = false;
					GameplayManager.Instance.SetFleetMaskInteraction(SpriteMaskInteraction.None);
					GameplayManager.Instance.SetFleetEngineState(true, true);
					this.usingJumpgate = false;
				}
			}
			this.travelMultiplier = 1f;
			GamePlayer.current.waypoints.Clear();
			TravelInfo.instance.ToggleVisible(false);
			return true;
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x000AD76C File Offset: 0x000AB96C
		public bool TravelActive()
		{
			return this.travelCoroutine != null || this.usingJumpgate;
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x000AD780 File Offset: 0x000AB980
		public void TriggerDynamicEvent()
		{
			if (this.emergencyJumpActive || this.totalDistance < 1f || this.remainingDistance / this.spaceShip.unitData.travelSpeed < 8f)
			{
				GamePlayer.current.ResetDynamicEventTimer(8f);
				return;
			}
			SystemStoryteller storyteller = GamePlayer.current.currentSystem.storyteller;
			TravelDynamicEvent travelDynamicEvent = (storyteller != null) ? storyteller.TriggerDynamicEvent() : null;
			foreach (Storyteller storyteller2 in GamePlayer.current.storytellers)
			{
				if (travelDynamicEvent != null)
				{
					break;
				}
				travelDynamicEvent = storyteller2.TriggerDynamicEvent();
			}
			if (travelDynamicEvent != null)
			{
				MapPointOfInterest mapPointOfInterest = travelDynamicEvent.CreateDynamicPOI(GamePlayer.current.currentSystem);
				if (mapPointOfInterest != null)
				{
					mapPointOfInterest.isDynamicPoi = true;
					mapPointOfInterest.timeLeft = 15f;
					GamePlayer.current.currentSystem.pointsOfInterest.Add(mapPointOfInterest);
					TravelInfo.instance.ShowDynamicEvent(mapPointOfInterest, travelDynamicEvent.actionLabel);
				}
			}
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x000AD88C File Offset: 0x000ABA8C
		private IEnumerator StartTravel(MapPointOfInterest target)
		{
			this.CalculateTravelBonusMultiplier();
			if (SceneLoader.IsSceneLoaded("SpacestationInterior"))
			{
				SpaceStationInterior.instance.ExitSpacestation();
			}
			this.screenSize = Behaviour.Util.Singleton<BackdropManager>.Instance.screenSizeGame;
			this.localTarget = target;
			this.spaceShip = GameplayManager.Instance.spaceShip;
			Behaviour.Util.Singleton<BackdropManager>.Instance.cameraMovement.SetTarget(this.spaceShip, true);
			if (SpacestationExteriorManager.Instance != null && SpacestationExteriorManager.Instance.undockingRoutine != null)
			{
				yield return new WaitUntil(() => SpacestationExteriorManager.Instance.undockingRoutine == null);
			}
			if (GamePlayer.current.currentPointOfInterest is JumpGate && ((JumpGate)GamePlayer.current.currentPointOfInterest).targetSystemGuid == target.system.guid)
			{
				JumpGateManager.instance.InitiateTravelThroughGate();
				yield break;
			}
			if (this.localPoiManager)
			{
				this.localPoiManager.StoreLastX();
			}
			this.worldLocationToTravelTo = this.localTarget.GetWorldPositionToTravelTo(null);
			TravelInfo.instance.InitializeTravelInformation(this.targetPoi, this.GetDistanceToTravel(), this.spaceShip.unitData.travelSpeed);
			this.ReturnDronesForFleet();
			yield return this.PrepareAllForInSystemTravel();
			HudManager.Instance.HideSubtleTimerInfo(2);
			HudManager.Instance.RemoveAllHealthBars();
			yield return this.Travel();
			HudManager.Instance.ToggleAllHealthBars(true);
			this.travelCoroutine = null;
			this.travelMultiplier = 1f;
			yield break;
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x000AD8A2 File Offset: 0x000ABAA2
		private IEnumerator PrepareAllForInSystemTravel()
		{
			List<SpaceShip> list = new List<SpaceShip>
			{
				this.spaceShip
			};
			foreach (SpaceShipData spaceShipData in GamePlayer.current.activeFleet)
			{
				Mercenary mercenary = spaceShipData.commanderData as Mercenary;
				if (mercenary == null || !mercenary.repairing)
				{
					SpaceShip spaceShip = spaceShipData.unit as SpaceShip;
					if (spaceShip != null)
					{
						list.Add(spaceShip);
					}
				}
			}
			int pending = list.Count;
			this._prepCoroutines.Clear();
			Action _delegate_1 = null;
			foreach (SpaceShip spaceShip2 in list)
			{
				List<Coroutine> prepCoroutines = this._prepCoroutines;
				IEnumerator coroutine = spaceShip2.PrepareForInSystemTravel(this.worldLocationToTravelTo, this.travelMultiplier > 1f);
				Action onComplete;
				if ((onComplete = _delegate_1) == null)
				{
					onComplete = (_delegate_1 = delegate()
					{
						pending--;
					});
				}
				prepCoroutines.Add(base.StartCoroutine(this.RunAndDecrement(coroutine, onComplete)));
			}
			float timeout = 10f;
			yield return new WaitUntil(() => pending <= 0 || (timeout -= Time.deltaTime) <= 0f);
			this._prepCoroutines.Clear();
			yield break;
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000AD8B1 File Offset: 0x000ABAB1
		private IEnumerator RunAndDecrement(IEnumerator coroutine, Action onComplete)
		{
			yield return coroutine;
			onComplete();
			yield break;
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x000AD8C8 File Offset: 0x000ABAC8
		private void ReturnDronesForFleet()
		{
			foreach (SpaceShipData spaceShipData in GamePlayer.current.activeFleet)
			{
				if (spaceShipData.spaceShip && spaceShipData.spaceShip.droneBayModule)
				{
					spaceShipData.spaceShip.droneBayModule.ReturnDrones();
				}
			}
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x000AD948 File Offset: 0x000ABB48
		private IEnumerator Travel()
		{
			Debug.Log("Travel");
			HudManager.Instance.ToggleDockButton(false);
			HudManager.Instance.SetEchoRemarksStatus(true);
			yield return this.TravelInSystem();
			if (GamePlayer.current == null || !this.spaceShip)
			{
				yield break;
			}
			if (TravelInfo.instance.visible)
			{
				TravelInfo.instance.ShowJumpTransition();
			}
			GamePlayer.current.currentPointOfInterest = this.localTarget;
			this.spaceShip.CompleteTravel();
			foreach (SpaceShipData spaceShipData in GamePlayer.current.activeFleet)
			{
				if (spaceShipData.unit)
				{
					(spaceShipData.unit as SpaceShip).CompleteTravel();
				}
			}
			HudManager.Instance.SetEchoRemarksStatus(false);
			yield return new WaitUntil(delegate()
			{
				BasePoiManager localPoiManager = this.localPoiManager;
				return localPoiManager != null && localPoiManager.initializedAndReady;
			});
			if (this.fastLaneTravelActive && this.targetPoi.system == this.localTarget.system)
			{
				yield return (this.localPoiManager as JumpGateManager).ArriveAtGate();
			}
			GamePlayer.current.waypoints.Remove(this.localTarget);
			this.localPoiManager.SpaceshipHasArrived();
			this.TravelToNextWaypoint();
			yield break;
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x000AD958 File Offset: 0x000ABB58
		public Vector2 GetWorldPositionToTravelTo()
		{
			if (this.localPoiManager && this.localPoiManager.IsPOI(this.localTarget))
			{
				return this.localPoiManager.GetWorldPositionToTravelTo();
			}
			return this.localTarget.GetWorldPositionToTravelTo(null);
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x000AD9A5 File Offset: 0x000ABBA5
		public IEnumerator TravelInSystem()
		{
			this.dynamicEventTimer = 5f;
			this.totalDistance = this.GetDistanceToTravel();
			TravelInfo.instance.InitializeTravelInformation(this.targetPoi, this.totalDistance, this.spaceShip.unitData.travelSpeed);
			float currentTravelTime = 0f;
			string[] array = new string[8];
			array[0] = "Travel in system. Target pos: ";
			int num = 1;
			Vector2 vector = this.localTarget.position;
			array[num] = vector.ToString();
			array[2] = ", current pos: ";
			int num2 = 3;
			vector = GamePlayer.current.mapPosition;
			array[num2] = vector.ToString();
			array[4] = ", spaceship pos";
			array[5] = this.spaceShip.rigidbody.position.ToString();
			array[6] = ", worldTarget: ";
			array[7] = this.worldLocationToTravelTo.ToString();
			Debug.Log(string.Concat(array));
			this.delayTravelAttempt = 3f;
			while (this.spaceShip && this.spaceShip.gameObject && !this.spaceShip.IsNearWorldPosition(this.worldLocationToTravelTo) && GamePlayer.current != null)
			{
				this.worldLocationToTravelTo = this.GetWorldPositionToTravelTo();
				this.isWarping = true;
				this.remainingDistance = this.GetDistanceToTravel();
				float num3 = this.spaceShip.baseWarpAcceleration * this.fuelMultiplier * this.bonusMultiplier;
				float spaceShipMaxWarpSpeed = this.GetSpaceShipMaxWarpSpeed();
				if (this.fastLaneTravelActive)
				{
					this.spaceShip.unitData.travelSpeed = spaceShipMaxWarpSpeed;
				}
				else if (0.5 * (double)num3 * (double)Mathf.Pow(this.spaceShip.unitData.travelSpeed / num3, 2f) >= (double)this.remainingDistance)
				{
					this.spaceShip.unitData.travelSpeed = Mathf.Clamp(this.spaceShip.unitData.travelSpeed - num3 * Time.deltaTime, 0f, spaceShipMaxWarpSpeed);
				}
				else if (this.spaceShip.unitData.travelSpeed < this.spaceShip.baseMaxWarpSpeed * this.fuelMultiplier * this.travelMultiplier * this.bonusMultiplier)
				{
					this.spaceShip.unitData.travelSpeed = Mathf.Clamp(this.spaceShip.unitData.travelSpeed + num3 * Time.deltaTime, 0f, spaceShipMaxWarpSpeed);
				}
				TravelInfo.instance.UpdateTravelInfo(this.remainingDistance, this.spaceShip.unitData.travelSpeed, spaceShipMaxWarpSpeed, num3);
				currentTravelTime += Time.deltaTime;
				this.CheckLocalPoiStatus();
				yield return null;
			}
			if (!this.spaceShip)
			{
				yield break;
			}
			this.isWarping = false;
			if (GamePlayer.current.waypoints.Count <= 1)
			{
				TravelInfo.instance.ToggleVisible(false);
			}
			Resources.UnloadUnusedAssets();
			this.spaceShip.ClearOverrideDestination();
			this.spaceShip.ResetPosition(new Vector2?(this.spaceShip.rigidbody.position));
			this.totalDistance = 0f;
			this.remainingDistance = 0f;
			yield break;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000AD9B4 File Offset: 0x000ABBB4
		public bool SpaceshipTravelPastHalfwayPoint()
		{
			return this.remainingDistance < this.totalDistance / 2f;
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000AD9CC File Offset: 0x000ABBCC
		public float GetSpaceShipMaxWarpSpeed()
		{
			float num = this.spaceShip.baseMaxWarpSpeed * this.fuelMultiplier * this.bonusMultiplier;
			if (this.travelMultiplier > 1f)
			{
				num = this.spaceShip.baseMaxWarpSpeed * this.travelMultiplier;
			}
			if (this.spaceShip.unitData.currentHullHP <= 0.1f)
			{
				num *= (GamePlayer.current.autoPlay ? 0.2f : 0.7f);
			}
			return num;
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000ADA48 File Offset: 0x000ABC48
		private void CalculateWarpFuel()
		{
			if (GamePlayer.current == null || this.travelMultiplier > 1f)
			{
				return;
			}
			if (!GamePlayer.current.currentSpaceShip.cargo.IsWarpFuelAvailable())
			{
				this.fuelMultiplier = 1f;
				return;
			}
			float warpFuelAutopilotMultiplier = TravelManager.GetWarpFuelAutopilotMultiplier();
			if (warpFuelAutopilotMultiplier > 0f)
			{
				this.fuelMultiplier = this.UseWarpFuel(warpFuelAutopilotMultiplier);
				return;
			}
			this.fuelMultiplier = 1f;
		}

		// Token: 0x06001CBD RID: 7357 RVA: 0x000ADAB4 File Offset: 0x000ABCB4
		private void CalculateTravelBonusMultiplier()
		{
			this.bonusMultiplier = 1f;
			if (this.targetPoi is Source.Galaxy.POI.Mining || this.targetPoi is Source.Galaxy.POI.Salvage)
			{
				this.bonusMultiplier *= 1f + SkilltreeNode.industrialTravelSpeedBonus.currentIncrease;
			}
			if (GameplayManager.Instance.spaceShip.shipRoleType.GetGameplayType() == GameplayType.Cargo)
			{
				this.bonusMultiplier *= 1f + SkilltreeNode.economyCargoTravelSpeed.currentIncrease;
			}
		}

		// Token: 0x06001CBE RID: 7358 RVA: 0x000ADB38 File Offset: 0x000ABD38
		private float UseWarpFuel(float autoPlayMultiplier = 1f)
		{
			if (!GamePlayer.current.useWarpFuel)
			{
				return 1f;
			}
			float num = 1f;
			using (IEnumerator<WarpFuelItem> enumerator = GamePlayer.current.currentSpaceShip.cargo.GetAllWarpFuel().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					WarpFuelItem warpFuelItem = enumerator.Current;
					warpFuelItem.UseWarpFuel(this.spaceShip.baseMaxWarpSpeed, Time.deltaTime);
					num = warpFuelItem.multiplier;
				}
			}
			return Mathf.Max(1f, num * autoPlayMultiplier);
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000ADBD0 File Offset: 0x000ABDD0
		public static float GetWarpFuelAutopilotMultiplier()
		{
			if (!GamePlayer.current.autoPlay)
			{
				return 1f;
			}
			if (SkilltreeNode.promptEngineeringT5UseWarpFuel.isActive)
			{
				return 0.8f + GamePlayer.current.commander.GetAutopilotPenaltyReductionModifier();
			}
			if (SkilltreeNode.promptEngineeringT2UseWarpFuel.isActive)
			{
				return 0.4f + GamePlayer.current.commander.GetAutopilotPenaltyReductionModifier();
			}
			return 0f;
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000ADC38 File Offset: 0x000ABE38
		public float GetDistanceToTravel()
		{
			return Vector2.Distance(this.spaceShip.rigidbody.position, this.worldLocationToTravelTo) / MapPointOfInterest.mapToLocalConversion;
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000ADC5C File Offset: 0x000ABE5C
		private void FixedUpdate()
		{
			if (!this.spaceShip || !this.spaceShip.unitData.travelling || this.localTarget == null || this.spaceShip.IsNearWorldPosition(this.worldLocationToTravelTo))
			{
				return;
			}
			this.spaceShip.MoveTowards(this.worldLocationToTravelTo);
			foreach (SpaceShipData spaceShipData in GamePlayer.current.activeFleet)
			{
				if (spaceShipData.unit)
				{
					SpaceShip spaceShip = spaceShipData.unit as SpaceShip;
					Vector2 b = Vector2.zero;
					if (spaceShip.autoActions != null)
					{
						if (spaceShip.autoActions.leaving)
						{
							continue;
						}
						b = (spaceShip.autoActions as WingmanActions).relativeFormationPos;
					}
					spaceShipData.travelSpeed = this.spaceShip.spaceShipData.travelSpeed;
					(spaceShipData.unit as SpaceShip).MoveTowards(this.worldLocationToTravelTo + b);
				}
			}
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000ADD74 File Offset: 0x000ABF74
		private void CheckLocalPoiStatus()
		{
			if (this.localPoiManager && !this.localPoiManager.IsPOI(this.localTarget))
			{
				Vector2 vector = this.spaceShip.transform.position;
				vector.x -= this.screenSize.x / 2f;
				vector.y -= this.screenSize.y / 2f;
				Rect other = new Rect(vector.x, vector.y, this.screenSize.x, this.screenSize.y);
				if (!this.localPoiManager.worldCoordinates.Overlaps(other))
				{
					this.UnloadCurrentScene();
					Behaviour.Util.Singleton<EffectManager>.Instance.KillAllEffects();
					Behaviour.Util.Singleton<SpriteUpdater>.Instance.Reset();
				}
			}
			if ((!this.localPoiManager || !this.localPoiManager.IsPOI(this.localTarget)) && !this.localPoiManager && !this.loadingNextScene)
			{
				this.loadingNextScene = true;
				PersistentSingleton<SceneLoader>.Instance.LoadScene(this.localTarget.sceneName, false);
			}
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000ADEA0 File Offset: 0x000AC0A0
		public void UnloadCurrentScene()
		{
			if (!this.localPoiManager)
			{
				return;
			}
			if (this.localPoiManager != null)
			{
				Debug.Log("Unload " + this.localPoiManager.sceneName);
				PersistentSingleton<SceneLoader>.Current.UnloadScene(this.localPoiManager.sceneName);
			}
			GamePlayer.current.currentPointOfInterest = null;
			this.localPoiManager = null;
			this.loadingNextScene = false;
			if (GameplayManager.Instance.spaceShip)
			{
				GameplayManager.Instance.spaceShip.CleanUpTravel();
			}
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000ADF34 File Offset: 0x000AC134
		public void TravelToNextWaypoint()
		{
			if (GamePlayer.current.waypoints.Count == 0)
			{
				MonoBehaviour.print("Reached destination.");
				if (GamePlayer.current.currentPatrol != null && GamePlayer.current.currentPatrol.dynamicPointOfInterest == MapPointOfInterest.current)
				{
					this.patrolMaxTimer = 20f;
				}
				this.travelCoroutine = null;
				return;
			}
			if (GamePlayer.current.waypoints[0].IsCurrentSystem())
			{
				Debug.Log("Continue travel to waypoint");
				this.travelCoroutine = base.StartCoroutine(this.StartTravel(GamePlayer.current.waypoints[0]));
				return;
			}
			Debug.Log("Travel to next waypoint but no action taken, waypoints: " + GamePlayer.current.waypoints.Count.ToString());
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000ADFFC File Offset: 0x000AC1FC
		public MapPointOfInterest TravelToClosestSpacestationWithFacility(SpaceStationFacility facility, int maxRange = 2)
		{
			MapPointOfInterest nearestSpaceStationWithFacility = SystemMapData.current.GetNearestSpaceStationWithFacility(facility, maxRange, true);
			if (nearestSpaceStationWithFacility != null && this.CanWeTravel(null))
			{
				this.SetRouteToPOI(nearestSpaceStationWithFacility);
			}
			return nearestSpaceStationWithFacility;
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000AE02C File Offset: 0x000AC22C
		private MapPointOfInterest FindClosestStation()
		{
			SystemMapData currentSystem = GamePlayer.current.currentSystem;
			Vector2 mapPosition = GamePlayer.current.mapPosition;
			Queue<ValueTuple<SystemMapData, int, Vector2>> queue = new Queue<ValueTuple<SystemMapData, int, Vector2>>();
			HashSet<string> hashSet = new HashSet<string>();
			MapPointOfInterest mapPointOfInterest = null;
			float num = float.MaxValue;
			int num2 = int.MaxValue;
			queue.Enqueue(new ValueTuple<SystemMapData, int, Vector2>(currentSystem, 0, GamePlayer.current.mapPosition));
			hashSet.Add(currentSystem.guid);
			while (queue.Count > 0)
			{
				ValueTuple<SystemMapData, int, Vector2> valueTuple = queue.Dequeue();
				SystemMapData item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				Vector2 item3 = valueTuple.Item3;
				foreach (MapPointOfInterest mapPointOfInterest2 in item.pointsOfInterest)
				{
					if (!mapPointOfInterest2.hidden)
					{
						SpaceStation spaceStation = mapPointOfInterest2 as SpaceStation;
						if (spaceStation != null && !(mapPointOfInterest2 is IndustryStation) && spaceStation.PlayerIsFriendly() && spaceStation.DockingAvailableFor(GameplayManager.Instance.spaceShip))
						{
							float num3 = (item == currentSystem) ? Vector2.Distance(mapPointOfInterest2.position, mapPosition) : Vector2.Distance(mapPointOfInterest2.position, item3);
							if (item2 < num2 || (item2 == num2 && num3 < num))
							{
								mapPointOfInterest = mapPointOfInterest2;
								num = num3;
								num2 = item2;
							}
						}
					}
				}
				if (mapPointOfInterest != null)
				{
					return mapPointOfInterest;
				}
				foreach (JumpGate jumpGate in item.GetJumpGateList(false))
				{
					if (jumpGate.canUseJumpGate)
					{
						SystemMapData system = GalaxyMapData.current.GetSystem(jumpGate.targetSystemGuid);
						MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest(jumpGate.targetPoiGuid);
						if (system != null && hashSet.Add(system.guid))
						{
							queue.Enqueue(new ValueTuple<SystemMapData, int, Vector2>(system, item2 + 1, pointOfInterest.position));
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000AE224 File Offset: 0x000AC424
		public void TravelToClosestSpacestation()
		{
			if (GamePlayer.current.homeStation != null && this.JumpDistanceToPoi(GamePlayer.current.homeStation) == 0)
			{
				this.SetRouteToPOI(GamePlayer.current.homeStation);
				return;
			}
			MapPointOfInterest mapPointOfInterest = this.FindClosestStation();
			if (mapPointOfInterest != null)
			{
				this.SetRouteToPOI(mapPointOfInterest);
				return;
			}
			this.SetRouteToPOI(GamePlayer.current.lastStation);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000AE288 File Offset: 0x000AC488
		public int JumpDistanceToPoi(MapPointOfInterest destination)
		{
			List<MapPointOfInterest> list = this.GenerateShortestRoute(destination);
			if (list == null || list.Count == 0)
			{
				return -1;
			}
			return list.OfType<JumpGate>().Count<JumpGate>();
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000AE2B5 File Offset: 0x000AC4B5
		protected IEnumerator JumpToSystem(JumpGate jumpGatePoi)
		{
			this.usingJumpgate = true;
			TravelInfo.instance.ShowJumpTransition();
			HudManager.Instance.RemoveAllHealthBars();
			CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			GameplayManager.Instance.SetFleetTurretState(false);
			float boundsX = spaceShip.GetBoundsX();
			yield return new WaitForSeconds(1f);
			Behaviour.Util.Singleton<BackdropManager>.Instance.ShowInJumpGateTravelEffect(jumpGatePoi.GetWorldPosition());
			GameplayManager.Instance.SetFleetActive(false, false);
			yield return new WaitForSeconds(1f);
			Behaviour.Util.Singleton<BackdropManager>.Instance.HideBackground();
			SystemMapData targetSystem = jumpGatePoi.targetSystem;
			if (jumpGatePoi.system.name == "Hermetis" && targetSystem.name == "Canis Majoris")
			{
				GamePlayer.current.TransitionTutorialToSandbox();
				targetSystem = GamePlayer.current.currentSystem;
				jumpGatePoi.targetSystemGuid = targetSystem.guid;
				jumpGatePoi.targetPoiGuid = GamePlayer.current.currentPointOfInterest.guid;
			}
			Debug.Log("Currently in: " + GamePlayer.current.currentSystem.name + " Id: " + GamePlayer.current.currentSystem.guid);
			PersistentSingleton<SceneLoader>.Instance.LoadScene("Travel", false);
			Behaviour.Util.Singleton<BackdropManager>.Instance.DestroyParallax();
			cameraMovement.SetNewPosition(jumpGatePoi.GetTargetPOI().GetWorldPosition());
			yield return new WaitForSeconds(1f);
			GamePlayer.current.currentPointOfInterest = jumpGatePoi.GetTargetPOI();
			GamePlayer.current.currentSystem = targetSystem;
			if (GamePlayer.current.waypoints[0] == jumpGatePoi.GetTargetPOI())
			{
				GamePlayer.current.waypoints.Remove(jumpGatePoi.GetTargetPOI());
			}
			Debug.Log(string.Concat(new string[]
			{
				"Arriving in ",
				GamePlayer.current.currentSystem.name,
				" Id: ",
				GamePlayer.current.currentSystem.guid,
				", Scenename: ",
				GamePlayer.current.currentPointOfInterest.sceneName
			}));
			PersistentSingleton<SceneLoader>.Instance.LoadScene(GamePlayer.current.currentPointOfInterest.sceneName, true);
			yield return new WaitUntil(() => this.localPoiManager != null && this.localPoiManager.initializedAndReady);
			GameplayManager.Instance.SetFleetPosition(((JumpGateManager)this.localPoiManager).GetJumpGateLandingPosition(boundsX));
			cameraMovement.FocusTarget(true);
			Resources.UnloadUnusedAssets();
			Behaviour.Util.Singleton<BackdropManager>.Instance.SetBackgroundData(targetSystem);
			yield return new WaitForSeconds(1f);
			GameplayManager.Instance.SetFleetActive(true, true);
			GameplayManager.Instance.SetFleetEngineState(false, false);
			Behaviour.Util.Singleton<BackdropManager>.Instance.EndJumpgateTravel();
			if (GamePlayer.current.DoFastLaneTravel())
			{
				yield return JumpGateManager.instance.jumpGate.ChargeFastLaneTravelToNextGate(spaceShip, GamePlayer.current.waypoints[0] as JumpGate);
				spaceShip.jumpingProcedureEngaged = false;
				GameplayManager.Instance.SetFleetRotation(spaceShip.GetShipLookRotationForTravel(GamePlayer.current.waypoints[0].GetWorldPosition()));
				this.travelMultiplier = 7f;
			}
			else
			{
				yield return JumpGateManager.instance.ArriveAtGate();
			}
			this.usingJumpgate = false;
			this.TravelToNextWaypoint();
			yield break;
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000AE2CC File Offset: 0x000AC4CC
		public bool TryInitiateTravel(MapPointOfInterest poi)
		{
			if (!this.CanWeTravel(poi))
			{
				return false;
			}
			if (SpacestationExteriorManager.Instance)
			{
				SpacestationExteriorManager.Instance.CancelDocking(GameplayManager.Instance.spaceShip, false);
			}
			if (!this.SetRouteToPOI(poi))
			{
				return false;
			}
			if (HudManager.Instance)
			{
				HudManager.Instance.ToggleHudElements(true);
			}
			return true;
		}

		// Token: 0x040011BD RID: 4541
		public const float dynamicEventDelay = 5f;

		// Token: 0x040011BE RID: 4542
		public const float dynamicEventChance = 0.25f;

		// Token: 0x040011BF RID: 4543
		public const float DeathPenalty = 0.7f;

		// Token: 0x040011C0 RID: 4544
		public const float DeathPenaltyAutopilot = 0.2f;

		// Token: 0x040011C5 RID: 4549
		public bool loadingNextScene;

		// Token: 0x040011C6 RID: 4550
		private Coroutine travelCoroutine;

		// Token: 0x040011C7 RID: 4551
		private readonly List<Coroutine> _prepCoroutines = new List<Coroutine>();

		// Token: 0x040011C9 RID: 4553
		private SpaceShip spaceShip;

		// Token: 0x040011CA RID: 4554
		private Vector2 screenSize;

		// Token: 0x040011CD RID: 4557
		private float fuelMultiplier = 1f;

		// Token: 0x040011CE RID: 4558
		private float travelMultiplier = 1f;

		// Token: 0x040011CF RID: 4559
		private float bonusMultiplier = 1f;

		// Token: 0x040011D0 RID: 4560
		private float dynamicEventTimer;

		// Token: 0x040011D2 RID: 4562
		public float bountyAbandonTimer;

		// Token: 0x040011D3 RID: 4563
		private float patrolAbandonTimer;

		// Token: 0x040011D4 RID: 4564
		public float patrolMaxTimer = 20f;

		// Token: 0x040011D5 RID: 4565
		private float delayTravelAttempt;

		// Token: 0x040011D6 RID: 4566
		public const float WarpFuel_40 = 0.4f;

		// Token: 0x040011D7 RID: 4567
		public const float WarpFuel_60 = 0.8f;

		// Token: 0x040011D8 RID: 4568
		public const float WarpFuel_80 = 0.8f;
	}
}
