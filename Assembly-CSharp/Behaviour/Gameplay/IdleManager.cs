using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Module;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Salvage;
using Behaviour.Tractoring;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.Missions;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Crew;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;

namespace Behaviour.Gameplay
{
	// Token: 0x02000328 RID: 808
	public class IdleManager : Singleton<IdleManager>
	{
		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x000B423C File Offset: 0x000B243C
		private static SpaceShip ship
		{
			get
			{
				return GameplayManager.Instance.spaceShip;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x000B4248 File Offset: 0x000B2448
		// (set) Token: 0x06001E66 RID: 7782 RVA: 0x000B4250 File Offset: 0x000B2450
		public float delayTimer { get; private set; }

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000B4259 File Offset: 0x000B2459
		// (set) Token: 0x06001E68 RID: 7784 RVA: 0x000B4261 File Offset: 0x000B2461
		public float delayTimerBase { get; private set; }

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x000B426A File Offset: 0x000B246A
		// (set) Token: 0x06001E6A RID: 7786 RVA: 0x000B4272 File Offset: 0x000B2472
		public float updateTimer { get; private set; } = 12f;

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x000B427B File Offset: 0x000B247B
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x000B4283 File Offset: 0x000B2483
		public float updateTimerBase { get; private set; } = 12f;

		// Token: 0x06001E6D RID: 7789 RVA: 0x000B428C File Offset: 0x000B248C
		private void Start()
		{
			this.autopilotTree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Engineering));
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000B42A0 File Offset: 0x000B24A0
		private void Update()
		{
			if (GamePlayer.current == null)
			{
				return;
			}
			if (!GamePlayer.current.autoPlayUnlocked)
			{
				return;
			}
			if (!GamePlayer.current.autoPlay)
			{
				return;
			}
			this.durationTimer += Time.deltaTime;
			if (this.durationTimer >= 1f)
			{
				this.durationTimer -= 1f;
				SteamStatsManager.Add(SteamStatType.AutopilotTimeTotal, 1);
				AutopilotSessionStats currentAutopilotSessionStats = GamePlayer.current.currentAutopilotSessionStats;
				if (currentAutopilotSessionStats != null)
				{
					SteamStatsManager.Set(SteamStatType.AutopilotTime, currentAutopilotSessionStats.GetTotalTime());
				}
			}
			if (this.delayTimer <= 0f)
			{
				this.delayTimer = 0f;
				this.delayTimerBase = 0f;
				this.updateTimer -= Time.deltaTime;
				if (this.updateTimer < 0f)
				{
					this.FindActivity();
				}
				return;
			}
			this.delayTimer -= Time.deltaTime;
			if (!SpaceStationInterior.instance)
			{
				this.delayTimer = 0f;
				IdleManager.UpdateActivity("@IdleCalculating", Array.Empty<object>());
				return;
			}
			this.delayTimer -= Time.deltaTime;
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000B43B8 File Offset: 0x000B25B8
		public void FindActivity()
		{
			float num = SkilltreeNode.promptEngineeringEnhancedActivityDelay.isActive ? 8f : (SkilltreeNode.promptEngineeringBasicActivityDelay.isActive ? 10f : 12f);
			this.updateTimer = num;
			this.updateTimerBase = num;
			if (Singleton<TravelManager>.Instance.TravelActive() || GamePlayer.current.waypoints.Count > 0)
			{
				if (this.idleTravelTarget != null && this.idleTravelTarget != Singleton<TravelManager>.instance.targetPoi)
				{
					IdleManager.UpdateActivity(this.rudelyInterrupted ? "@IdleRudelyInterupted" : "@IdleTravelInterupted", new object[]
					{
						GamePlayer.current.commander.lastName
					});
				}
				return;
			}
			IdleManager.UpdateActivity("@IdleCalculating", Array.Empty<object>());
			Mission idleMission = this.GetIdleMission();
			if (idleMission != null && idleMission.GetActivePoi(true) != MapPointOfInterest.current)
			{
				SpaceStation spaceStation = idleMission.GetActivePoi(true) as SpaceStation;
				if (spaceStation != null && spaceStation.DockingAvailableFor(IdleManager.ship))
				{
					Debug.Log("Travel to spacestation for idle mission");
					this.IdleTravelToSpaceStation(null, false);
					return;
				}
			}
			SpaceStation spaceStation2 = MapPointOfInterest.current as SpaceStation;
			if (spaceStation2 != null && spaceStation2.PlayerIsFriendly() && spaceStation2.DockingAvailableFor(IdleManager.ship))
			{
				if (!SpaceStationInterior.instance)
				{
					IdleManager.UpdateActivity("@IdleDockWithSS", Array.Empty<object>());
					SpacestationExteriorManager.Instance.CheckForDocking();
					return;
				}
				if (idleMission != null && idleMission.isComplete && idleMission.GetActivePoi(true) == MapPointOfInterest.current)
				{
					if (GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Mining, TargetLayer.Both))
					{
						GamePlayer.current.lastVisitedMiningPOI = null;
					}
					IdleManager.UpdateActivity("@IdleTurnInMission", new object[]
					{
						idleMission.name
					});
					if (idleMission.category == Translation.Translate("@CourierMission", Array.Empty<object>()))
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Deliveries, 1);
					}
					else
					{
						GamePlayer.current.AddAutopilotStat(IdleStat.Missions, 1);
					}
					idleMission.ClaimRewards(false);
					GamePlayer.RefreshMissionPanel(null);
					return;
				}
				InventoryItemType inventoryItemType = null;
				foreach (Inventory.InventoryItem inventoryItem in GamePlayer.current.currentSpaceShip.cargo.items)
				{
					if (!inventoryItem.item.favouriteItem && (inventoryItem.item.itemCategory != ItemCategory.Ammo || !IdleManager.ship.DoWeUseAmmo(inventoryItem.item)) && (inventoryItem.item.itemCategory != ItemCategory.Usable || inventoryItem.item.storageOverride != StorageOverride.None))
					{
						if (inventoryItem.item.CanGoInMaterials())
						{
							if (!spaceStation2.materialStorage.IsFull(inventoryItem.item.m3))
							{
								inventoryItemType = inventoryItem.item;
								break;
							}
							if (GamePlayer.current.autopilotSettings.autoSell && this.SellItemFromMaterials(spaceStation2, inventoryItem.item.m3))
							{
								return;
							}
							IdleManager.UpdateActivity("@IdleMaterialStorageFull", Array.Empty<object>());
							return;
						}
						else if (inventoryItem.item.CanGoInArmory())
						{
							if (GamePlayer.current.globalInventory.IsFull(inventoryItem.item.m3))
							{
								IdleManager.UpdateActivity("@IdleArmoryStorageFull", Array.Empty<object>());
								return;
							}
							inventoryItemType = inventoryItem.item;
							break;
						}
					}
				}
				if (inventoryItemType != null)
				{
					int amount = 1;
					if (inventoryItemType.itemCategory == ItemCategory.Ammo)
					{
						amount = Mathf.Max(20, InventoryItemType.GetMagSizeForAmmo(inventoryItemType));
					}
					else if (inventoryItemType.itemCategory == ItemCategory.Currency)
					{
						amount = 20;
					}
					int amount2 = GamePlayer.current.currentSpaceShip.cargo.Remove(inventoryItemType, amount);
					this.inventoryFull = false;
					if (inventoryItemType.CanGoInArmory())
					{
						GamePlayer.current.globalInventory.Add(inventoryItemType, amount2, false, false);
					}
					else
					{
						spaceStation2.materialStorage.Add(inventoryItemType, amount2, false, false);
					}
					this.SetQuickerUpdateTimer();
					IdleManager.UpdateActivity("@IdleDepositItem", new object[]
					{
						inventoryItemType.displayName
					});
					return;
				}
				if (IdleManager.ship.currentHullHP < IdleManager.ship.maxHullHP || IdleManager.ship.currentArmorHP < IdleManager.ship.maxArmorHP)
				{
					IdleManager.UpdateActivity("@IdleWaitRepairs", Array.Empty<object>());
					return;
				}
				if (GamePlayer.current.homeStation != null && GamePlayer.current.autopilotSettings.prioritizeHomestation && MapPointOfInterest.current != GamePlayer.current.homeStation && Singleton<TravelManager>.Instance.CanWeTravel(null))
				{
					this.TravelToPoi(GamePlayer.current.homeStation, "@IdleReturnToHomeBase", Array.Empty<object>());
					return;
				}
				this.inventoryFull = false;
				this.FindActivityForEquipment();
			}
			else if (this.inventoryFull)
			{
				Debug.Log("Travel to spacestation, inv full");
				this.IdleTravelToSpaceStation(null, false);
			}
			else
			{
				GameplayManager instance = GameplayManager.Instance;
				bool flag;
				if (instance == null)
				{
					flag = false;
				}
				else
				{
					SpaceShip spaceShip = instance.spaceShip;
					bool? flag2 = (spaceShip != null) ? new bool?(spaceShip.AmmoInCargoForTurrets(false)) : null;
					bool flag3 = false;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					this.IdleTravelToSpaceStation("@IdleTravelForAmmo", true);
				}
				else if (MapPointOfInterest.current is Source.Galaxy.POI.Mining || MapPointOfInterest.current is Source.Galaxy.POI.Salvage || MapPointOfInterest.current is Source.Galaxy.POI.Combat || MapPointOfInterest.current is JumpGate)
				{
					this.IdleActivityInPoi();
				}
				else if (MapPointOfInterest.current == null)
				{
					this.FindActivityForEquipment();
				}
			}
			SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(this.autopilotTree, false);
			if (skillTreeData == null)
			{
				return;
			}
			skillTreeData.AddMasteryXp(10f);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000B4954 File Offset: 0x000B2B54
		private void SetQuickerUpdateTimer()
		{
			this.updateTimer = 400f / GamePlayer.current.currentSpaceShip.cargoCapacity;
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000B4974 File Offset: 0x000B2B74
		private bool SellItemFromMaterials(SpaceStation spaceStation, float m3Needed)
		{
			SpaceStationInterior.instance.GoToLocation(spaceStation.shopInventory.facility, true);
			int num;
			Inventory.InventoryItem cheapestMaterial = spaceStation.materialStorage.GetCheapestMaterial(m3Needed, out num);
			if (cheapestMaterial != null && num > 0)
			{
				InventoryInteractionManager.Instance.SellAmount(cheapestMaterial, num, null);
				IdleManager.UpdateActivity("@IdleSellItem", new object[]
				{
					num,
					cheapestMaterial.item.displayName
				});
				this.SetQuickerUpdateTimer();
				GamePlayer.current.AddAutopilotStat(IdleStat.MaterialsSoldValue, cheapestMaterial.item.sellValue * num);
				return true;
			}
			return false;
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000B4A04 File Offset: 0x000B2C04
		private void IdleTravelToSpaceStation(string messageOverride = null, bool overrideClosests = false)
		{
			Debug.Log("TravelToSpacestation");
			this.SetIdleCombatMode(IdleManager.ship, false, null);
			if (!this.inventoryFull && MapPointOfInterest.current != null)
			{
				foreach (TractorableItem tractorableItem in BasePoiManager.current.GetComponentsInChildren<TractorableItem>())
				{
					if (tractorableItem && tractorableItem.data.ownerFaction == Faction.player)
					{
						TractorModule module = IdleManager.ship.GetModule<TractorModule>();
						if (module)
						{
							module.SetManualTarget(tractorableItem);
							IdleManager.UpdateActivity("@IdleLooting", new object[]
							{
								tractorableItem.data.itemType.displayName
							});
							return;
						}
					}
				}
			}
			if (!Singleton<TravelManager>.Instance.CanWeTravel(null))
			{
				IdleManager.UpdateActivity("@IdleTravelNotPossible", Array.Empty<object>());
				return;
			}
			Mission idleMission = this.GetIdleMission();
			if (((idleMission != null) ? idleMission.GetActivePoi(true) : null) is SpaceStation || ((idleMission != null) ? idleMission.sourcePoi : null) is SpaceStation)
			{
				SpaceStation spaceStation = (SpaceStation)((idleMission.GetActivePoi(true) is SpaceStation) ? idleMission.GetActivePoi(true) : idleMission.sourcePoi);
				if (spaceStation.DockingAvailableFor(IdleManager.ship))
				{
					this.TravelToPoi(spaceStation, messageOverride ?? "@IdleGoToMissionSpaceStation", Array.Empty<object>());
					return;
				}
			}
			if (GamePlayer.current.homeStation != null && !overrideClosests)
			{
				this.TravelToPoi(GamePlayer.current.homeStation, "@IdleReturnToHomeBase", Array.Empty<object>());
				return;
			}
			if (!GamePlayer.current.autopilotSettings.noTravel)
			{
				Singleton<TravelManager>.Instance.TravelToClosestSpacestation();
				IdleManager.UpdateActivity(messageOverride ?? "@IdleGoToSpaceStation", Array.Empty<object>());
				return;
			}
			IdleManager.UpdateActivity("@IdleWantToTravel", Array.Empty<object>());
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000B4BB0 File Offset: 0x000B2DB0
		private bool TravelToPoi(MapPointOfInterest poi, string message, params object[] values)
		{
			if (GamePlayer.current.autopilotSettings.noTravel)
			{
				IdleManager.UpdateActivity("@IdleWantToTravel", Array.Empty<object>());
				return false;
			}
			this.idleTravelTarget = poi;
			this.rudelyInterrupted = SeededRandom.Global.RandomBool(0.3f);
			Singleton<TravelManager>.Instance.SetRouteToPOI(poi);
			IdleManager.UpdateActivity(message, values);
			return true;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000B4C10 File Offset: 0x000B2E10
		private void FindActivityForEquipment()
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.PlayerIsFriendly())
			{
				GameplayManager instance = GameplayManager.Instance;
				bool flag;
				if (instance == null)
				{
					flag = false;
				}
				else
				{
					SpaceShip spaceShip = instance.spaceShip;
					bool? flag2 = (spaceShip != null) ? new bool?(spaceShip.AmmoInCargoForTurrets(true)) : null;
					bool flag3 = false;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair in IdleManager.ship.GetAmmoTypesRequired())
					{
						int value = keyValuePair.Value;
						this.updateTimer = 400f / GamePlayer.current.currentSpaceShip.cargoCapacity;
						if (this.FetchItem(keyValuePair.Key, value, spaceStation))
						{
							return;
						}
					}
					IdleManager.UpdateActivity("@IdleNoAmmo", Array.Empty<object>());
					return;
				}
				if (SkilltreeNode.promptEngineeringT2UseWarpFuel.isActive && !GamePlayer.current.currentSpaceShip.cargo.HasEnoughWarpFuel(2))
				{
					if (this.FetchItem(ItemBuilder.Get("WarpFuel").CreateWarpFuel(Rarity.Enhanced), 2, spaceStation))
					{
						return;
					}
					if (this.FetchItem(ItemBuilder.Get("WarpFuel").CreateWarpFuel(Rarity.HighGrade), 2, spaceStation))
					{
						return;
					}
				}
			}
			if (GamePlayer.current.autopilotSettings.preferMissions && this.DecideMissionActivity())
			{
				return;
			}
			GameplayType preferredGameplayType = IdleManager.ship.GetPreferredGameplayType(false);
			TargetLayer preferredTargetLayer = IdleManager.ship.preferredTargetLayer;
			if (preferredGameplayType != GameplayType.Generic && preferredGameplayType != GameplayType.Cargo)
			{
				if (!GamePlayer.current.currentSpaceShip.HasLoadout(preferredGameplayType, preferredTargetLayer))
				{
					IdleManager.UpdateActivity("@IdleNoSuitableLoadout", new object[]
					{
						preferredGameplayType
					});
					return;
				}
				Debug.Log("preferredMissionType: " + preferredGameplayType.ToString() + ", layer: " + preferredTargetLayer.ToString());
				if (preferredGameplayType == GameplayType.Mining && this.FindMiningActivity(preferredTargetLayer))
				{
					return;
				}
				if (preferredGameplayType == GameplayType.Salvage && this.FindSalvageActivity(preferredTargetLayer))
				{
					return;
				}
				if (preferredGameplayType == GameplayType.Combat && this.FindCombatActivity())
				{
					return;
				}
			}
			if (this.DecideMissionActivity())
			{
				return;
			}
			if (GamePlayer.current.homeStation != null && GamePlayer.current.homeStation != MapPointOfInterest.current)
			{
				this.TravelToPoi(GamePlayer.current.homeStation, "@IdleReturnToHomeBase", Array.Empty<object>());
			}
			IdleManager.UpdateActivity("@IdleNothingToDo", Array.Empty<object>());
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000B4E7C File Offset: 0x000B307C
		private bool FetchItem(InventoryItemType item, int requiredAmount, SpaceStation spaceStation)
		{
			if (GamePlayer.current.globalInventory.HasItem(item, requiredAmount))
			{
				IdleManager.UpdateActivity("@IdleTransferItem", new object[]
				{
					requiredAmount,
					item.displayName
				});
				GamePlayer.current.globalInventory.Remove(item, requiredAmount);
				GamePlayer.current.currentSpaceShip.cargo.Add(item, requiredAmount, false, false);
				return true;
			}
			if (spaceStation.shopInventory != null && spaceStation.shopInventory.HasItem(item, requiredAmount))
			{
				SpaceStationInterior.instance.GoToLocation(spaceStation.shopInventory.facility, true);
				InventoryInteractionManager.Instance.BuyAmount(item, requiredAmount);
				IdleManager.UpdateActivity("@IdleBuyItem", new object[]
				{
					requiredAmount,
					item.displayName
				});
				return true;
			}
			return false;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000B4F4C File Offset: 0x000B314C
		private bool DecideMissionActivity()
		{
			if (!GamePlayer.current.autopilotSettings.runMissions)
			{
				return false;
			}
			Mission mission = null;
			foreach (Mission mission2 in GamePlayer.current.missions)
			{
				if (mission2.failed && mission2.storyId == null)
				{
					mission = mission2;
					break;
				}
			}
			if (mission != null)
			{
				GamePlayer.current.RemoveMission(mission, false);
				if (mission.trackedOnHud)
				{
					Singleton<FocusedMissionHandler>.Current.ResetFocusedMission();
				}
				IdleManager.UpdateActivity("@IdleAbandoningMission", Array.Empty<object>());
				return true;
			}
			Mission mission3 = this.GetIdleMission();
			if (mission3 == null || mission3.GetActivePoi(true) == MapPointOfInterest.current || !Singleton<TravelManager>.Instance.CanWeTravel(null))
			{
				if (mission3 == null)
				{
					SpaceStationInterior instance = SpaceStationInterior.instance;
					if (instance != null && instance.HasMissionBoard(true))
					{
						mission3 = SpaceStationInterior.instance.GetMissionAvailableForAutopilot();
						if (mission3 != null)
						{
							GamePlayer.current.AcceptMission(mission3);
							mission3.idle = true;
							Singleton<FocusedMissionHandler>.Instance.SetMission(mission3);
							IdleManager.UpdateActivity("@IdlePickingUpMission", new object[]
							{
								mission3.name
							});
							return true;
						}
						IdleManager.UpdateActivity("@IdleNegotiateMission", Array.Empty<object>());
						return true;
					}
					else if (GamePlayer.current.homeStation == null || !GamePlayer.current.autopilotSettings.prioritizeHomestation)
					{
						if (GamePlayer.current.autopilotSettings.noTravel)
						{
							IdleManager.UpdateActivity("@IdleWantToTravel", Array.Empty<object>());
							return true;
						}
						if (Singleton<TravelManager>.Instance.TravelToClosestSpacestationWithFacility(SpaceStationFacility.MissionBoard, 3) != null)
						{
							IdleManager.UpdateActivity("@IdleGoToSpaceStationWithMissionBoard", Array.Empty<object>());
							return true;
						}
					}
				}
				return false;
			}
			if (!GamePlayer.current.currentSpaceShip.HasLoadout(mission3.gameplayType, mission3.targetLayer ?? TargetLayer.Both))
			{
				IdleManager.UpdateActivity("@IdleNoSuitableLoadout", new object[]
				{
					mission3.gameplayType
				});
				return true;
			}
			if (this.TravelToPoi(mission3.GetActivePoi(true), "@IdleTravelToMissionLocation", Array.Empty<object>()))
			{
				Singleton<FocusedMissionHandler>.Instance.SetMission(mission3);
			}
			return true;
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000B516C File Offset: 0x000B336C
		private bool FindMiningActivity(TargetLayer targetLayer)
		{
			UnityEngine.Object module = IdleManager.ship.GetModule<MiningModule>();
			DroneBayModule droneBayModule = IdleManager.ship.droneBayModule;
			if (!module && !droneBayModule)
			{
				return false;
			}
			MapPointOfInterest lastVisitedMiningPOI = GamePlayer.current.lastVisitedMiningPOI;
			if (lastVisitedMiningPOI != null && this.IsSuitableForMining(lastVisitedMiningPOI, targetLayer))
			{
				this.TravelToPoi(lastVisitedMiningPOI, "@IdleReturnToMining", new object[]
				{
					lastVisitedMiningPOI.name
				});
				return true;
			}
			foreach (MapPointOfInterest mapPointOfInterest in SystemMapData.current.allPointsOfInterest)
			{
				if (!mapPointOfInterest.hidden)
				{
					Source.Galaxy.POI.Mining mining = mapPointOfInterest as Source.Galaxy.POI.Mining;
					if (mining != null && Singleton<TravelManager>.Instance.CanWeTravel(mapPointOfInterest))
					{
						mining.InitializeAsteroids(false, false);
						if (this.IsSuitableForMining(mapPointOfInterest, targetLayer))
						{
							this.TravelToPoi(mapPointOfInterest, "@IdleGoToMining", new object[]
							{
								mapPointOfInterest.name
							});
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000B526C File Offset: 0x000B346C
		private bool IsSuitableForMining(MapPointOfInterest poi, TargetLayer targetLayer)
		{
			foreach (PersistableData persistableData in poi.GetPersistables())
			{
				AsteroidData asteroidData = persistableData as AsteroidData;
				if (asteroidData != null)
				{
					if (targetLayer == TargetLayer.Surface && asteroidData.surfaceAmount > 0)
					{
						return true;
					}
					if (targetLayer == TargetLayer.Core && asteroidData.innerCoreAmount > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000B52E0 File Offset: 0x000B34E0
		private bool FindSalvageActivity(TargetLayer targetLayer)
		{
			foreach (MapPointOfInterest mapPointOfInterest in SystemMapData.current.allPointsOfInterest)
			{
				if (!mapPointOfInterest.hidden && mapPointOfInterest is Source.Galaxy.POI.Salvage && Singleton<TravelManager>.Instance.CanWeTravel(mapPointOfInterest))
				{
					foreach (PersistableData persistableData in mapPointOfInterest.GetPersistables())
					{
						SalvageData salvageData = persistableData as SalvageData;
						if (salvageData != null && (salvageData.HasItems() || (salvageData.HasScrap() && targetLayer == TargetLayer.Surface) || (salvageData.HasStructuralContent() && targetLayer == TargetLayer.Core)))
						{
							this.TravelToPoi(mapPointOfInterest, "@IdleGoToSalvage", new object[]
							{
								mapPointOfInterest.name
							});
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000B53D4 File Offset: 0x000B35D4
		private bool FindCombatActivity()
		{
			foreach (MapPointOfInterest mapPointOfInterest in SystemMapData.current.allPointsOfInterest)
			{
				if (!mapPointOfInterest.hidden && mapPointOfInterest is Source.Galaxy.POI.Combat && Singleton<TravelManager>.Instance.CanWeTravel(mapPointOfInterest))
				{
					using (IEnumerator<AbstractUnitData> enumerator2 = mapPointOfInterest.GetUnits(false).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.IsPlayerEnemy())
							{
								this.TravelToPoi(mapPointOfInterest, "@IdleGoToCombat", new object[]
								{
									mapPointOfInterest.name
								});
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000B549C File Offset: 0x000B369C
		private void IdleActivityInPoi()
		{
			MiningModule module = IdleManager.ship.GetModule<MiningModule>();
			SalvageModule module2 = IdleManager.ship.GetModule<SalvageModule>();
			CombatModule module3 = IdleManager.ship.GetModule<CombatModule>();
			bool flag = false;
			if (module)
			{
				bool canMineSurface = module.canMineSurface;
				bool canMineCore = module.canMineCore;
				if (module.priorityTarget == null || !module.IsCurrentTargetMineable())
				{
					Asteroid asteroid = null;
					float num = 0f;
					foreach (Asteroid asteroid2 in BasePoiManager.current.GetComponentsInChildren<Asteroid>())
					{
						if (module.IsTargetMineable(asteroid2))
						{
							float num2 = Vector2.Distance(asteroid2.transform.position, module.transform.position);
							if (asteroid == null || num2 < num)
							{
								asteroid = asteroid2;
								num = num2;
							}
						}
					}
					if (asteroid != null)
					{
						IdleManager.ship.SetManualTarget(asteroid);
					}
				}
				TargetableUnit priorityTarget = module.priorityTarget;
				AsteroidData asteroidData;
				if (priorityTarget == null)
				{
					asteroidData = null;
				}
				else
				{
					Asteroid component = priorityTarget.GetComponent<Asteroid>();
					asteroidData = ((component != null) ? component.asteroidData : null);
				}
				AsteroidData asteroidData2 = asteroidData;
				if (asteroidData2 == null)
				{
					TargetableUnit priorityTarget2 = module.priorityTarget;
					if (priorityTarget2 == null || !priorityTarget2.damagableByAll)
					{
						goto IL_187;
					}
				}
				flag = true;
				string text;
				if (module.priorityTarget.damagableByAll)
				{
					text = module.priorityTarget.targetName;
				}
				else if (canMineCore && asteroidData2.innerCoreAmount > 0)
				{
					text = asteroidData2.innerCoreItem.item.displayName;
				}
				else
				{
					text = asteroidData2.surfaceItem.item.displayName;
				}
				IdleManager.UpdateActivity("@IdleMining", new object[]
				{
					text
				});
			}
			IL_187:
			if (module2)
			{
				SalvageContainer salvageContainer = null;
				float num3 = 9999f;
				foreach (SalvageContainer salvageContainer2 in BasePoiManager.current.GetComponentsInChildren<SalvageContainer>())
				{
					if (module2.IsTargetSalvagable(salvageContainer2))
					{
						float num4 = Vector2.Distance(salvageContainer2.transform.position, module2.transform.position);
						if (num4 < num3)
						{
							salvageContainer = salvageContainer2;
							num3 = num4;
						}
					}
				}
				if (module2.priorityTarget == null && salvageContainer != null)
				{
					IdleManager.ship.SetManualTarget(salvageContainer);
					List<SalvageItemData> availableItemContent = salvageContainer.data.availableItemContent;
					if (availableItemContent.Count > 0)
					{
						salvageContainer.data.SetActiveItem(availableItemContent[0].item);
						if (!HudManager.Instance.ShowingSalvageStatus())
						{
							HudManager.Instance.ToggleSalvageStatus(salvageContainer, salvageContainer.data);
						}
					}
				}
				if (!(module2.priorityTarget is SalvageContainer))
				{
					TargetableUnit priorityTarget3 = module2.priorityTarget;
					if (priorityTarget3 == null || !priorityTarget3.damagableByAll)
					{
						goto IL_2B7;
					}
				}
				IdleManager.UpdateActivity("@IdleSalvage", new object[]
				{
					module2.priorityTarget.targetName
				});
				flag = true;
			}
			IL_2B7:
			if (module3)
			{
				AbstractUnit abstractUnit = null;
				float num5 = 9999f;
				foreach (AbstractUnit abstractUnit2 in BasePoiManager.current.GetComponentsInChildren<AbstractUnit>())
				{
					if (abstractUnit2.IsPlayerEnemy() && abstractUnit2.enabled && !(abstractUnit2 is Drone))
					{
						float num6 = Vector2.Distance(abstractUnit2.transform.position, module3.transform.position);
						if (num6 < num5)
						{
							abstractUnit = abstractUnit2;
							num5 = num6;
						}
					}
				}
				if (module3.priorityTarget == null && abstractUnit != null)
				{
					this.SetIdleCombatMode(IdleManager.ship, true, abstractUnit);
				}
				else if (MapPointOfInterest.current.hasPayload && IdleManager.ship.GetPreferredGameplayType(false) == GameplayType.Combat)
				{
					IdleManager.UpdateActivity("@IdleCombatReinforcements", Array.Empty<object>());
					flag = true;
				}
				if (!(module3.priorityTarget is AbstractUnit))
				{
					TargetableUnit priorityTarget4 = module3.priorityTarget;
					if (priorityTarget4 == null || !priorityTarget4.damagableByAll)
					{
						goto IL_3D7;
					}
				}
				IdleManager.UpdateActivity("@IdleCombat", new object[]
				{
					module3.priorityTarget.targetName
				});
				flag = true;
			}
			IL_3D7:
			if (!flag)
			{
				Debug.Log("No valid targets for modules");
				this.IdleTravelToSpaceStation(null, false);
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000B5898 File Offset: 0x000B3A98
		private void SetIdleCombatMode(SpaceShip ship, bool enable, AbstractUnit target = null)
		{
			if (enable)
			{
				CombatActions combatActions = ship.autoActions as CombatActions;
				if (combatActions == null)
				{
					combatActions = new CombatActions(ship);
					ship.SetTemporaryActions(combatActions);
				}
				if (target)
				{
					combatActions.threat.Add(target, 100f * GameMath.DamageMultiplier((float)ship.level));
				}
				combatActions.ChooseCombatTarget();
				return;
			}
			ship.SetTemporaryActions(null);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000B58FC File Offset: 0x000B3AFC
		public Mission GetIdleMission()
		{
			if (!GamePlayer.current.autopilotSettings.runMissions)
			{
				return null;
			}
			FocusedMissionHandler instance = Singleton<FocusedMissionHandler>.Instance;
			Mission mission = (instance != null) ? instance.focusedMission : null;
			if (mission != null && mission.CanBeIdled() && mission != null && mission.CanBeIdleCompleted())
			{
				return Singleton<FocusedMissionHandler>.Instance.focusedMission;
			}
			foreach (Mission mission2 in GamePlayer.current.missions)
			{
				if (mission2.CanBeIdled() && mission2.CanBeIdleCompleted())
				{
					return mission2;
				}
			}
			return null;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000B59AC File Offset: 0x000B3BAC
		public void TriggerInventoryFull()
		{
			if (GamePlayer.current.autoPlay)
			{
				this.inventoryFull = true;
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000B59C1 File Offset: 0x000B3BC1
		public static void UpdateActivity(string text, params object[] values)
		{
			SidePanel.instance.SetIdleStatus(Translation.Translate(text, values));
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000B59D4 File Offset: 0x000B3BD4
		public static void DelayIdleActivities(float delay = 15f)
		{
			Singleton<IdleManager>.Instance.delayTimerBase = delay;
			Singleton<IdleManager>.Instance.delayTimer = delay;
			if (delay != 0f)
			{
				SidePanel.instance.SetIdleStatus(Translation.Translate("@IdlePreparingToLeave", Array.Empty<object>()));
			}
		}

		// Token: 0x04001239 RID: 4665
		public const float ActivityDelay = 12f;

		// Token: 0x0400123A RID: 4666
		public const float BasicActivityDelay = 10f;

		// Token: 0x0400123B RID: 4667
		public const float EnhancedActivityDelay = 8f;

		// Token: 0x0400123C RID: 4668
		public const float ExpertActivityDelay = 6f;

		// Token: 0x0400123D RID: 4669
		public const float ActivitiesPerSecond = 0.0833333358f;

		// Token: 0x0400123E RID: 4670
		public const float LootDropTimescale = 400f;

		// Token: 0x0400123F RID: 4671
		public const float DefaultInputDelay = 15f;

		// Token: 0x04001244 RID: 4676
		private bool inventoryFull;

		// Token: 0x04001245 RID: 4677
		private Skilltree autopilotTree;

		// Token: 0x04001246 RID: 4678
		private float durationTimer;

		// Token: 0x04001247 RID: 4679
		private MapPointOfInterest idleTravelTarget;

		// Token: 0x04001248 RID: 4680
		public bool rudelyInterrupted;
	}
}
