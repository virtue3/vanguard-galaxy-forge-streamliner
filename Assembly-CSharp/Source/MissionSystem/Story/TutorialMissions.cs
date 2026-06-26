using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Unit;
using Source.Crew;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.World;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Story
{
	// Token: 0x020000B0 RID: 176
	public class TutorialMissions
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x0003AFCC File Offset: 0x000391CC
		private static SpaceStation GetSpaceStation(GamePlayer ply, int level)
		{
			foreach (SystemMapData systemMapData in ply.map.allSystems)
			{
				if (systemMapData.level == level)
				{
					return TutorialMissions.GetSpaceStation(systemMapData);
				}
			}
			return null;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0003B02C File Offset: 0x0003922C
		private static SpaceStation GetSpaceStation(GamePlayer ply, string storyId)
		{
			foreach (SystemMapData systemMapData in ply.map.allSystems)
			{
				if (systemMapData.storyId == storyId)
				{
					return TutorialMissions.GetSpaceStation(systemMapData);
				}
			}
			return null;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0003B094 File Offset: 0x00039294
		private static SpaceStation GetSpaceStation(SystemMapData system)
		{
			foreach (MapPointOfInterest mapPointOfInterest in system.pointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null)
				{
					return spaceStation;
				}
			}
			return null;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0003B0F0 File Offset: 0x000392F0
		public TutorialMissions()
		{
			this.Tutorial_1();
			this.Tutorial_1b();
			this.Tutorial_2();
			this.Tutorial_3();
			this.Tutorial_4();
			this.Tutorial_5();
			this.Tutorial_6();
			this.Tutorial_7();
			this.Tutorial_8();
			this.Tutorial_9();
			this.Tutorial_10();
			this.Tutorial_11();
			this.Tutorial_Combat_1();
			this.Tutorial_Combat_4();
			this.Tutorial_Combat_10();
			this.Tutorial_Salvage_1();
			this.Tutorial_Salvage_4();
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0003B169 File Offset: 0x00039369
		private void Tutorial_Combat_1()
		{
			StoryMission.Add(new StoryMission("tutorial_combat_1", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = "Systems Check";
				mission.description = "Ensure thrusters, scanners, and equipment are still operational whilst your AI is running a diagnostic on your ship's systems. ";
				mission.completionText = "Systems seem to be working somewhat decently, could be worse.";
				mission.sourcePoi = ply.currentPointOfInterest;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MoveToArea,
					description = "Hold left-click to move to the designated area",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep();
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MoveCamera,
					description = "Hold right-click to move the camera",
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.LootContainerOpened,
					description = "Left-click to target and shoot open the cargo container",
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ItemCollected,
					description = "Left-click on an item to use the tractor module",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_1b"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0003B19B File Offset: 0x0003939B
		private void Tutorial_Salvage_1()
		{
			StoryMission.Add(new StoryMission("tutorial_salvage_1", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = "Systems Check";
				mission.description = "Ensure thrusters, scanners, and equipment are still operational whilst your AI is running a diagnostic on your ship's systems. ";
				mission.completionText = "Systems seem to be working somewhat decently, could be worse.";
				mission.sourcePoi = ply.currentPointOfInterest;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MoveToArea,
					description = "Hold left-click to move to the designated area",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep();
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MoveCamera,
					description = "Hold right-click to move the camera",
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.TargetWreckage,
					description = "Left-click to target wreckage (derelict ship)",
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ItemCollected,
					description = "Left-click on an item to use the tractor module",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_1b"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0003B1CD File Offset: 0x000393CD
		private void Tutorial_1()
		{
			StoryMission.Add(new StoryMission("tutorial_1", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = "Systems Check";
				mission.description = "Ensure thrusters, scanners, and mining equipment are still operational whilst your AI is running a diagnostic on your ship's systems. ";
				mission.completionText = "Systems seem to be working somewhat decently, could be worse.";
				mission.sourcePoi = ply.currentPointOfInterest;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MoveToArea,
					description = "Hold left-click to move to the designated area",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep();
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MoveCamera,
					description = "Hold right-click to move the camera",
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.TargetAsteroid,
					description = "Left-click to target an asteroid and begin mining",
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ItemCollected,
					description = "Left-click on an item to use the tractor module",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_1b"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0003B1FF File Offset: 0x000393FF
		private void Tutorial_1b()
		{
			StoryMission.Add(new StoryMission("tutorial_1b", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = "Tractor More Items";
				mission.description = "Get some more loot in our cargo hold.";
				mission.completionText = "Such hard work, manually targeting...";
				mission.sourcePoi = ply.currentPointOfInterest;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ItemCollected,
					description = "Use the tractor beam to pick up 2 items",
					requiredAmount = 2
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_2"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0003B231 File Offset: 0x00039431
		private void Tutorial_2()
		{
			StoryMission.Add(new StoryMission("tutorial_2", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, 1);
				Mission mission = new Mission();
				mission.name = "Navigation Online";
				mission.description = "Navigation is back online, I have found what appears to be a small outpost, " + spaceStation.name + ". Let's head over there and see if we can get some help.";
				mission.completionText = "My ECHO is not destroyed. That's a good thing it seems.";
				mission.sourcePoi = ply.currentPointOfInterest;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ArrivedAtSpaceStation,
					description = "Use the map [M] to fly to " + spaceStation.name,
					requiredAmount = 1
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.DockedWithSpaceStation,
					description = "Dock with the spacestation",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial2Welcome,
					description = "Talk to someone",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 150
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_3"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0003B263 File Offset: 0x00039463
		private void Tutorial_3()
		{
			StoryMission.Add(new StoryMission("tutorial_3", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_1");
				Mission mission = new Mission
				{
					name = "Personal Hangar",
					description = "Install a new turret, because your old one is pretty busted up. Right-Click on the turret in your inventory, or left-click and drag the turret on the turret hardpoint.",
					completionText = "The ship is looking a lot better.",
					sourcePoi = spaceStation,
					sourceFaction = Faction.player,
					sourceName = "Tutorial",
					canAbandon = false,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Tutorial,
					turnIn = spaceStation
				};
				InventoryItemType key = EquipmentBuilder.Get("SmallMiningLaser").CreateItemType(Rarity.Standard, 2, true, "tml", false, false);
				if (GamePlayer.current.starterSpecialization == 8)
				{
					key = EquipmentBuilder.Get("SmallGatlingTurret").CreateItemType(Rarity.Standard, 2, true, "tml", false, false);
					mission.missionItems.Add(InventoryItemType.Get("Gatling Ammo"), 400);
				}
				else if (GamePlayer.current.starterSpecialization == 6)
				{
					key = EquipmentBuilder.Get("SmallSalvageLaser").CreateItemType(Rarity.Standard, 2, true, "tml", false, false);
				}
				mission.missionItems.Add(key, 1);
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.PersonalHangarRepair,
					description = "Go to your hangar and repair your ship",
					requiredAmount = 1
				});
				MissionTrigger trigger = MissionTrigger.InstallMiningLaser;
				if (GamePlayer.current.starterSpecialization == 8)
				{
					trigger = MissionTrigger.InstallCombatTurret;
				}
				else if (GamePlayer.current.starterSpecialization == 6)
				{
					trigger = MissionTrigger.InstallSalvageLaser;
				}
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = trigger,
					description = "Install a new turret",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial3Complete,
					description = "Talk to Greg",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 150
				});
				string missionId = "tutorial_4";
				if (GamePlayer.current.starterSpecialization == 8)
				{
					missionId = "tutorial_combat_4";
				}
				else if (GamePlayer.current.starterSpecialization == 6)
				{
					missionId = "tutorial_salvage_4";
				}
				mission.rewards.Add(new StoryMission
				{
					missionId = missionId
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0003B295 File Offset: 0x00039495
		private void Tutorial_4()
		{
			StoryMission.Add(new StoryMission("tutorial_4", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_1");
				InventoryItemType inventoryItemType = "OreCommon1";
				Mission mission = new Mission();
				mission.name = "Calibrate New Equipment";
				mission.description = "Well, let's test out that new laser, let's get you out there and bring me back some fine " + Translation.Translate(inventoryItemType.displayName, Array.Empty<object>()) + ". I'll give you some credits and I'll trade you a plasma cell, and the gate to Orbitan.";
				mission.completionText = "A much better laser!";
				mission.sourcePoi = spaceStation;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				mission.turnIn = spaceStation;
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = inventoryItemType,
					requiredAmount = 10,
					deliverTo = spaceStation
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial4Complete,
					description = "Talk to Greg",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 250
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100,
					faction = Faction.stranded
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f)
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_5"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0003B2C7 File Offset: 0x000394C7
		private void Tutorial_Combat_4()
		{
			StoryMission.Add(new StoryMission("tutorial_combat_4", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_1");
				"OreCommon1";
				Mission mission = new Mission();
				mission.name = "Defeat The Drones";
				mission.description = "Let's test out that new turret, some rogue drones need to be dealt with. Open your map and travel to the POI. I'll give you some credits and I'll trade you a plasma cell, and the gate to Orbitan.";
				mission.completionText = "Got rid of the drones.";
				mission.sourcePoi = spaceStation;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				mission.turnIn = spaceStation;
				Combat combat = spaceStation.system.AddCombat(Faction.marauders, null, 0);
				combat.AddTriggeredSpawn(combat.CreateFixedPayload("PirateDrone", 2, null, null, UnitRank.Rookie), 0f, 0, false, true);
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = combat
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.marauders,
					requiredAmount = 2
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial4Complete,
					description = "Talk to Greg",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 250
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100,
					faction = Faction.stranded
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f)
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_5"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0003B2F9 File Offset: 0x000394F9
		private void Tutorial_Salvage_4()
		{
			StoryMission.Add(new StoryMission("tutorial_salvage_4", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_1");
				"OreCommon1";
				SystemMapData systemByStoryId = GalaxyMapData.current.GetSystemByStoryId("tutorial_system_1");
				Mission mission = new Mission();
				mission.name = "Salvage The Wreckage";
				mission.description = "Let's test out that new turret, should speed up salvaging quite a bit. Open your map and travel to the POI. When you're back I'll trade you some credits and I'll trade you a plasma cell, and the gate to Orbitan.";
				mission.completionText = "Nothing but scraps left.";
				mission.sourcePoi = spaceStation;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				mission.turnIn = spaceStation;
				Source.Galaxy.POI.Salvage salvage = new Source.Galaxy.POI.Salvage
				{
					name = "Derelict Oxlo",
					level = systemByStoryId.level,
					hidden = false
				};
				systemByStoryId.SetupPOI(salvage, null, Faction.stranded, 0);
				EquipmentBuilder.GetItemsForGeneralShop(salvage.level);
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2(4f, 2f),
					shipTemplate = "Oxlo Mk I",
					scrapContent = new Dictionary<InventoryItemType, int>
					{
						{
							"SalvageSilicon",
							15
						},
						{
							"SalvageCarbon",
							15
						}
					}
				};
				salvageData.AddItemContent(salvage.level, 3, 1f);
				salvage.AddPersistable(salvageData);
				MissionStep missionStep = new MissionStep
				{
					system = salvage.system,
					dynamicPointOfInterest = salvage
				};
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = "SalvageSilicon",
					requiredAmount = 4,
					deliverTo = spaceStation
				});
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = "SalvageCarbon",
					requiredAmount = 4,
					deliverTo = spaceStation
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial4Complete,
					description = "Talk to Greg",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 250
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100,
					faction = Faction.stranded
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f)
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_5"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0003B32B File Offset: 0x0003952B
		private void Tutorial_5()
		{
			StoryMission.Add(new StoryMission("tutorial_5", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_1");
				SpaceStation spaceStation2 = null;
				InventoryItemType key = null;
				JumpGate jumpGate = null;
				using (IEnumerator<JumpGate> enumerator = spaceStation.system.GetJumpGateList(false).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						JumpGate jumpGate2 = enumerator.Current;
						key = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate2, null);
						spaceStation2 = TutorialMissions.GetSpaceStation(jumpGate2.GetTargetPOI().system);
						jumpGate = jumpGate2;
					}
				}
				Mission mission = new Mission
				{
					name = "Using The Gatesystem",
					description = "Go to the next system and get help.",
					completionText = "Used the jump gate, but no engineer here.",
					sourcePoi = spaceStation,
					sourceFaction = Faction.player,
					sourceName = "Tutorial",
					canAbandon = false,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Tutorial,
					turnIn = spaceStation2
				};
				mission.missionItems.Add(key, 1);
				if (!jumpGate.canUseJumpGate)
				{
					MissionStep missionStep = new MissionStep
					{
						system = jumpGate.system,
						poiHint = jumpGate
					};
					missionStep.objectives.Add(new TriggerObjective
					{
						trigger = MissionTrigger.UnlockJumpgateOrbitan,
						description = "Jettisson the jumpgatepass at the gate.",
						requiredAmount = 1
					});
					mission.steps.Add(missionStep);
				}
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation2.system,
					poiHint = spaceStation2
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.DockedWithSpaceStation,
					description = "Dock with " + spaceStation2.name,
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial5Welcome,
					description = "Talk to someone at " + spaceStation2.name + " in " + spaceStation2.system.name,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 150
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_6"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0003B35D File Offset: 0x0003955D
		private void Tutorial_6()
		{
			StoryMission.Add(new StoryMission("tutorial_6", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_2");
				Mission mission = new Mission
				{
					name = "Get To The Balam System",
					description = "We can use the mission board now. Let's see what they have on offer. If we make enough credits we can buy a Jumpgate Pass to Balam.",
					completionText = "Elena joined the crew.",
					sourcePoi = spaceStation,
					sourceFaction = Faction.player,
					sourceName = "Tutorial",
					canAbandon = false,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Tutorial
				};
				SpaceStation spaceStation2 = null;
				JumpGate jumpGate = null;
				foreach (JumpGate jumpGate2 in spaceStation.system.GetJumpGateList(false))
				{
					if (!(GalaxyMapData.current.GetSystem(jumpGate2.targetSystemGuid).storyId != "tutorial_system_4"))
					{
						spaceStation2 = TutorialMissions.GetSpaceStation(jumpGate2.GetTargetPOI().system);
						jumpGate = jumpGate2;
						break;
					}
				}
				if (!jumpGate.canUseJumpGate)
				{
					MissionStep missionStep = new MissionStep
					{
						system = jumpGate.system,
						poiHint = jumpGate
					};
					missionStep.objectives.Add(new TriggerObjective
					{
						trigger = MissionTrigger.UnlockJumpgateBalam,
						description = "Buy a pass and unlock the gate to " + spaceStation2.system.name,
						requiredAmount = 1
					});
					mission.steps.Add(missionStep);
				}
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation2.system,
					poiHint = spaceStation2
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial6Welcome,
					description = "Talk to someone at " + spaceStation2.name + " in " + spaceStation2.system.name,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 350
				});
				SeededRandom random = new SeedGenerator().Add("Mechanic").CreateRandom();
				mission.rewards.Add(new Source.MissionSystem.Rewards.Crew
				{
					crew = CrewMemberData.CreateCustomCrewMember(Gender.Female, "Elena", "", "Scott", CrewIcons.Get("Elena"), Profession.Engineering, Rarity.Standard, random, true),
					hidden = true
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_7"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x0003B38F File Offset: 0x0003958F
		private void Tutorial_7()
		{
			StoryMission.Add(new StoryMission("tutorial_7", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_4");
				SystemMapData systemByStoryId = GalaxyMapData.current.GetSystemByStoryId("tutorial_system_4");
				Mission mission = new Mission
				{
					name = "Salvage The Derelict",
					description = "We know of a salvage location here in the Balam system where a broken down UF Navy Corvette got abandoned. Equip a (cheap) salvage laser and salvage it for an AI core. Return it to " + spaceStation.name + " in " + spaceStation.system.name,
					completionText = "Unlocked Industrial skilltree.",
					sourcePoi = spaceStation,
					sourceFaction = Faction.player,
					sourceName = "Tutorial",
					canAbandon = false,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Tutorial
				};
				if (GamePlayer.current.starterSpecialization != 6)
				{
					InventoryItemType key = EquipmentBuilder.Get("JunkSalvageLaser").CreateItemType(Rarity.Standard, 3, true, "tml", false, false);
					mission.missionItems.Add(key, 1);
				}
				Source.Galaxy.POI.Salvage salvage = new Source.Galaxy.POI.Salvage
				{
					name = "Derelict UF Navy Corvette",
					level = systemByStoryId.level,
					hidden = false
				};
				systemByStoryId.SetupPOI(salvage, null, Faction.gold, 0);
				EquipmentBuilder.GetItemsForGeneralShop(salvage.level);
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2(8f, 2f),
					shipTemplate = "Fanatic",
					scrapContent = new Dictionary<InventoryItemType, int>
					{
						{
							"SalvageTitanium",
							2
						},
						{
							"SalvageTungsten",
							4
						}
					},
					itemContent = new List<SalvageItemData>
					{
						new SalvageItemData(InventoryItemType.Get("DamagedAiCore"), 9999f, true)
					}
				};
				salvageData.AddItemContent(salvage.level, 3, 1f);
				salvage.AddPersistable(salvageData);
				MissionStep missionStep = new MissionStep
				{
					system = salvage.system,
					dynamicPointOfInterest = salvage
				};
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
				{
					itemType = "DamagedAiCore",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial7Complete,
					description = "Return to Creed",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 350
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_8"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0003B3C1 File Offset: 0x000395C1
		private void Tutorial_8()
		{
			StoryMission.Add(new StoryMission("tutorial_8", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_4");
				Mission mission = new Mission();
				mission.name = "Craft The Core";
				mission.description = "Go to the forge and craft a AI Core from the salvaged Damaged AI core to hopefully reactivate ECHO's autopilot.";
				mission.completionText = "Unlocked Prompt Engineering skilltree.";
				mission.sourcePoi = spaceStation;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CraftAICore,
					description = "Use the Forge to craft the AI core.",
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial8Complete,
					description = "Return to Creed",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 150
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_9"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0003B3F3 File Offset: 0x000395F3
		private void Tutorial_9()
		{
			StoryMission.Add(new StoryMission("tutorial_9", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_4");
				JumpGate jumpGate = null;
				foreach (JumpGate jumpGate2 in GalaxyMapData.current.GetSystemByStoryId("tutorial_system_4").GetJumpGateList(false))
				{
					if (jumpGate2.targetSystem == GalaxyMapData.current.GetSystemByStoryId("tutorial_system_7"))
					{
						jumpGate = jumpGate2;
					}
				}
				Mission mission = new Mission
				{
					name = "Defeat The Pirate",
					description = "We need to defeat a pirate ship guarding the gate to Hermetis.",
					completionText = "Got rid of the hostile ship.",
					sourcePoi = spaceStation,
					sourceFaction = Faction.player,
					sourceName = "Tutorial",
					canAbandon = false,
					trackedOnHud = true,
					turnIn = spaceStation,
					difficulty = MissionDifficulty.Tutorial
				};
				InventoryItemType lootItem = null;
				foreach (JumpGate jumpGate3 in GalaxyMapData.current.GetSystemByStoryId("tutorial_system_4").GetJumpGateList(false))
				{
					if (jumpGate3.targetSystem == GalaxyMapData.current.GetSystemByStoryId("tutorial_system_7"))
					{
						lootItem = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate3, null);
						break;
					}
				}
				if (ply.commander.personalHistory == PersonalHistory.NavyCaptain)
				{
					MapPointOfInterest mapPointOfInterest = jumpGate;
					MapPointOfInterest mapPointOfInterest2 = jumpGate;
					float pointsScale = 1f;
					Faction marauders = Faction.marauders;
					UnitRank? fixedRank = new UnitRank?(UnitRank.Rookie);
					mapPointOfInterest.AddGuards(mapPointOfInterest2.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), marauders, 20, 20, 1, 1, fixedRank), null)[0].AddLoot(lootItem, 1);
				}
				else
				{
					List<AbstractUnitData> list = jumpGate.AddGuards(jumpGate.CreateFixedPayload("Rockhound", 1, Faction.marauders, null, UnitRank.Rookie), null);
					list[0].EquipItem(EquipmentBuilder.Get("JunkGattlingTurret").CreateItemType(Rarity.Standard, 2, false, null, false, false), 0);
					list[0].AddLoot(lootItem, 1);
				}
				MissionStep missionStep = new MissionStep
				{
					system = jumpGate.system,
					poiHint = jumpGate
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.marauders,
					requiredAmount = 1
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial9Complete,
					description = "Return to Creed",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 500
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.stranded,
					amount = 75
				});
				string missionId = "tutorial_10";
				if (GamePlayer.current.starterSpecialization == 8)
				{
					missionId = "tutorial_combat_10";
				}
				mission.rewards.Add(new StoryMission
				{
					missionId = missionId
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0003B425 File Offset: 0x00039625
		private void Tutorial_Combat_10()
		{
			StoryMission.Add(new StoryMission("tutorial_combat_10", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_4");
				SystemMapData systemByStoryId = GalaxyMapData.current.GetSystemByStoryId("tutorial_system_3");
				InventoryItemType key = null;
				foreach (JumpGate jumpGate in systemByStoryId.GetJumpGateList(false))
				{
					if (jumpGate.targetSystem == GalaxyMapData.current.GetSystemByStoryId("tutorial_system_5"))
					{
						key = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate, null);
						break;
					}
				}
				foreach (JumpGate jumpGate2 in GalaxyMapData.current.GetSystemByStoryId("tutorial_system_2").GetJumpGateList(false))
				{
					if (jumpGate2.targetSystem == systemByStoryId)
					{
						jumpGate2.UnlockJumpgate();
					}
				}
				MapPointOfInterest mapPointOfInterest = systemByStoryId.SetupPOI(new Combat(), null, null, 0) as MapPointOfInterest;
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				float pointsScale = 0.5f;
				Faction marauders = Faction.marauders;
				UnitRank? fixedRank = new UnitRank?(UnitRank.Rookie);
				List<AbstractUnitData> list = mapPointOfInterest2.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), marauders, 0, 0, 1, 5, fixedRank);
				mapPointOfInterest.AddGuards(list, null);
				MapPointOfInterest mapPointOfInterest3 = systemByStoryId.SetupPOI(new Combat(), null, null, 0) as MapPointOfInterest;
				MapPointOfInterest mapPointOfInterest4 = mapPointOfInterest;
				float pointsScale2 = 1f;
				marauders = Faction.marauders;
				fixedRank = new UnitRank?(UnitRank.Rookie);
				List<AbstractUnitData> list2 = mapPointOfInterest4.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Combat), marauders, 0, 0, 1, 5, fixedRank);
				mapPointOfInterest3.AddGuards(list2, null);
				MapPointOfInterest mapPointOfInterest5 = GalaxyMapData.current.GetSystemByStoryId("tutorial_system_5").SetupPOI(new Combat(), null, null, 0) as MapPointOfInterest;
				MapPointOfInterest mapPointOfInterest6 = mapPointOfInterest;
				float pointsScale3 = 1f;
				marauders = Faction.marauders;
				fixedRank = new UnitRank?(UnitRank.Rookie);
				List<AbstractUnitData> list3 = mapPointOfInterest6.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Combat), marauders, 20, 0, 1, 5, fixedRank);
				mapPointOfInterest5.AddGuards(list3, null);
				Mission mission = new Mission();
				mission.name = "Recover The Materials";
				mission.description = "We know where the materials are, but we need you to defeat the pirates guarding them. Don't worry about bringing it back, we will send someone to pick it up.";
				mission.completionText = "Completed.";
				mission.sourcePoi = spaceStation;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.turnIn = spaceStation;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				mission.missionItems.Add(key, 1);
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.marauders,
					requiredAmount = list.Count
				});
				MissionStep missionStep2 = new MissionStep
				{
					system = mapPointOfInterest3.system,
					dynamicPointOfInterest = mapPointOfInterest3
				};
				missionStep2.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.marauders,
					requiredAmount = list2.Count
				});
				MissionStep missionStep3 = new MissionStep
				{
					system = mapPointOfInterest5.system,
					dynamicPointOfInterest = mapPointOfInterest5
				};
				missionStep3.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.marauders,
					requiredAmount = list3.Count
				});
				MissionStep missionStep4 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep4.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial10CombatComplete,
					description = "Return to Creed",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.steps.Add(missionStep2);
				mission.steps.Add(missionStep3);
				mission.steps.Add(missionStep4);
				mission.rewards.Add(new Experience
				{
					amount = 1000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = InventoryItemType.Get("Signal Array"),
					amount = 1
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_10"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0003B457 File Offset: 0x00039657
		private void Tutorial_10()
		{
			StoryMission.Add(new StoryMission("tutorial_10", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(ply, "tutorial_system_4");
				SystemMapData systemByStoryId = GalaxyMapData.current.GetSystemByStoryId("tutorial_system_3");
				InventoryItemType key = null;
				foreach (JumpGate jumpGate in systemByStoryId.GetJumpGateList(false))
				{
					if (jumpGate.targetSystem == GalaxyMapData.current.GetSystemByStoryId("tutorial_system_5"))
					{
						key = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate, null);
						break;
					}
				}
				MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest("TutorialJumpgatePOI");
				Mission mission = new Mission
				{
					name = "Repair The Gate",
					description = "Deliver the required materials to the Jumpgate construction site, then return to " + spaceStation.name + ". Just throw them out of your cargo hold, and the construction unit should pick them up.",
					completionText = "Completed.",
					sourcePoi = spaceStation,
					sourceFaction = Faction.player,
					sourceName = "Tutorial",
					turnIn = spaceStation,
					canAbandon = false,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Tutorial
				};
				if (GamePlayer.current.starterSpecialization != 8)
				{
					mission.missionItems.Add(key, 1);
				}
				if (GamePlayer.current.starterSpecialization != 8)
				{
					MissionStep missionStep = new MissionStep
					{
						poiHint = pointOfInterest
					};
					missionStep.objectives.Add(new TriggerObjective
					{
						trigger = MissionTrigger.TutorialJumpgateStructure,
						description = "Deliver Skeleton Frames to the gate",
						requiredAmount = 4
					});
					mission.steps.Add(missionStep);
					MissionStep missionStep2 = new MissionStep
					{
						poiHint = pointOfInterest
					};
					missionStep2.objectives.Add(new TriggerObjective
					{
						trigger = MissionTrigger.TutorialJumpgatePlates,
						description = "Deliver Titanium Plates to the gate",
						requiredAmount = 8
					});
					mission.steps.Add(missionStep2);
					MissionStep missionStep3 = new MissionStep
					{
						poiHint = pointOfInterest
					};
					missionStep3.objectives.Add(new TriggerObjective
					{
						trigger = MissionTrigger.TutorialJumpgateConduit,
						description = "Deliver Conduit Cells to the gate",
						requiredAmount = 8
					});
					mission.steps.Add(missionStep3);
				}
				MissionStep missionStep4 = new MissionStep
				{
					poiHint = pointOfInterest
				};
				missionStep4.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.TutorialJumpgateBeacon,
					description = "Deliver a Signal Array to the gate",
					requiredAmount = 1
				});
				MissionStep missionStep5 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep5.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Tutorial10Complete,
					description = "Return to Creed",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep4);
				mission.steps.Add(missionStep5);
				mission.rewards.Add(new Experience
				{
					amount = 800
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "tutorial_11"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0003B489 File Offset: 0x00039689
		private void Tutorial_11()
		{
			StoryMission.Add(new StoryMission("tutorial_11", delegate(GamePlayer ply)
			{
				SystemMapData systemMapData;
				SystemMapData systemMapData2;
				MapPointOfInterest mapPointOfInterest = TutorialMissions.CreateJumpGateLinkToSandbox(out systemMapData, out systemMapData2);
				SpaceStation spaceStation = TutorialMissions.GetSpaceStation(MapPointOfInterest.current.system);
				MapPointOfInterest poiHint = null;
				foreach (JumpGate jumpGate in systemMapData.GetJumpGateList(false))
				{
					if (jumpGate.targetSystem == systemMapData2)
					{
						poiHint = jumpGate;
						jumpGate.UnlockJumpgate();
						jumpGate.position = mapPointOfInterest.position;
					}
				}
				Mission mission = new Mission();
				mission.name = "One Way Trip";
				mission.description = "Jump through the gate and start your adventure.";
				mission.completionText = "Completed the tutorial!";
				mission.sourcePoi = spaceStation;
				mission.sourceFaction = Faction.player;
				mission.sourceName = "Tutorial";
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Tutorial;
				MissionStep missionStep = new MissionStep
				{
					poiHint = poiHint
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.TutorialLastJump,
					description = "Go through the gate to leave this sector",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				return mission;
			}, null, null));
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x0003B4BC File Offset: 0x000396BC
		public static MapPointOfInterest CreateJumpGateLinkToSandbox(out SystemMapData sourceSystem, out SystemMapData targetSystem)
		{
			MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest("TutorialJumpgatePOI");
			sourceSystem = pointOfInterest.system;
			targetSystem = GalaxyMapData.current.GetSystemByStoryId("tutorial_system_11");
			pointOfInterest.system.RemovePointOfInterest(pointOfInterest);
			TutorialWorld.CreateJumpGateLinks(new Dictionary<SystemMapData, List<SystemMapData>>
			{
				{
					sourceSystem,
					new List<SystemMapData>
					{
						targetSystem
					}
				}
			});
			return pointOfInterest;
		}
	}
}
