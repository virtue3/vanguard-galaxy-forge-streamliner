using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Unit;
using Source.Crew;
using Source.Data;
using Source.Data.Persistable;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World;
using Source.Simulation.World.System;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Story
{
	// Token: 0x020000B1 RID: 177
	public class UmbralMissions
	{
		// Token: 0x0600072D RID: 1837 RVA: 0x0003B520 File Offset: 0x00039720
		public UmbralMissions()
		{
			this.UmbralMission1();
			this.UmbralMission2();
			this.UmbralMission3();
			this.UmbralMission4();
			this.UmbralMission5();
			this.UmbralMission5b();
			this.UmbralMission6();
			this.UmbralMission7();
			this.UmbralMission8();
			this.UmbralMission9();
			this.UmbralMission10();
			this.UmbralMission11();
			this.UmbralMission12();
			this.UmbralMission13();
			this.UmbralMission14();
			this.UmbralMission14Failed();
			this.UmbralMission15();
			this.UmbralMission16();
			this.UmbralMission17();
			this.UmbralMission18();
			this.UmbralMission19();
			this.UmbralMission20();
			this.UmbralMission21();
			this.UmbralMission22();
			this.UmbralMission22Failed();
			this.UmbralMission23();
			this.DarkspaceMission1();
			this.DarkspaceMission2();
			this.DarkspaceMission3();
			this.DarkspaceMission4();
			this.DarkspaceMission4Failed();
			this.DarkspaceMission5();
			this.DarkspaceMission6();
			this.ConquestUmbral1();
			this.ConquestUmbral2();
			this.ConquestUmbral3();
			this.ConquestUmbral4();
			this.ConquestUmbral5();
			this.ConquestUmbral6();
			this.ConquestUmbral7();
			this.ConquestUmbral8();
			this.ConquestUmbral8b();
			this.ConquestUmbral9();
			this.ConquestUmbral10();
			this.ConquestDarkspace1();
			this.ConquestDarkspace2();
			this.ConquestDarkspace3();
			this.ConquestDarkspace4();
			this.ConquestDarkspace5();
			this.ConquestDarkspace6();
			this.ConquestDarkspace7();
			this.ConquestDarkspace8();
			this.ConquestDarkspace9();
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x0003B671 File Offset: 0x00039871
		private void UmbralMission1()
		{
			StoryMission.Add(new StoryMission("UmbralMission1", delegate(GamePlayer ply)
			{
				MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest("UmbralBeacon1");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@UmbralMission1", Array.Empty<object>());
				mission.description = Translation.Translate("@UmbralMission1Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourcePoi = pointOfInterest;
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = pointOfInterest.system,
					poiHint = pointOfInterest
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = pointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = SystemMapData.current,
					poiHint = pointOfInterest
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.InteractWithUmbralBeacon,
					description = Translation.Translate("@UmbralMissionInteractBeacon", Array.Empty<object>()),
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Source.MissionSystem.Rewards.StoryMission
				{
					missionId = "UmbralMission2"
				});
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 50
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x0003B6A3 File Offset: 0x000398A3
		private void UmbralMission2()
		{
			StoryMission.Add(new StoryMission("UmbralMission2", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@UmbralMission2", Array.Empty<object>());
				mission.description = Translation.Translate("@UmbralMission2Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourcePoi = ply.currentPointOfInterest;
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.VisitUniqueSystem,
					description = Translation.Translate("@UmbralMission2MS1O1", Array.Empty<object>()),
					requiredAmount = 3
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.DockedWithSpaceStation,
					description = Translation.Translate("@UmbralMission2MS1O2", Array.Empty<object>()),
					requiredAmount = 2
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CompleteDynamicMission,
					description = Translation.Translate("@UmbralMission2MS1O3", Array.Empty<object>()),
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission3"
				});
				mission.rewards.Add(new Experience
				{
					amount = 400
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 150
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x0003B6D5 File Offset: 0x000398D5
		private void UmbralMission3()
		{
			StoryMission.Add(new StoryMission("UmbralMission3", delegate(GamePlayer ply)
			{
				MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest("UmbralBeacon1");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@UmbralMission3", Array.Empty<object>());
				mission.description = Translation.Translate("@UmbralMission3Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourcePoi = pointOfInterest;
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = pointOfInterest.system,
					poiHint = pointOfInterest
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = pointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = pointOfInterest.system,
					poiHint = pointOfInterest
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.InteractWithUmbralBeacon,
					description = Translation.Translate("@UmbralMissionInteractBeacon", Array.Empty<object>()),
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission4"
				});
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x0003B707 File Offset: 0x00039907
		private void UmbralMission4()
		{
			StoryMission.Add(new StoryMission("UmbralMission4", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = SectorMapData.current.FindStationForFaction(Faction.gold, new bool?(false));
				Mission mission = new Mission
				{
					name = Translation.Translate("@UmbralMission4", Array.Empty<object>()),
					description = Translation.Translate("@UmbralMission4Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourcePoi = spaceStation,
					sourceFaction = Faction.puppeteers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = spaceStation.guid
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.DockedWithSpaceStation,
					description = Translation.Translate("@DockWith", new object[]
					{
						spaceStation.name
					}),
					requiredAmount = 1
				});
				spaceStation.characters.Add("LuminateCommander");
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.UmbralLuminatePrisoner4,
					description = "Talk to the Luminate",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				Source.Galaxy.POI.Combat combat = this.AddPrisonStation(spaceStation.system);
				MissionStep missionStep2 = new MissionStep
				{
					system = combat.system,
					dynamicPointOfInterest = combat,
					requireAllObjectives = false,
					hidden = true
				};
				missionStep2.objectives.Add(new Source.MissionSystem.Objectives.Reputation
				{
					faction = Faction.gold,
					reputationLevel = ReputationLevel.Cordial
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CombatStationDestroyed,
					description = "Go to the prison and break out the expedition member",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				MissionStep missionStep3 = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation,
					hidden = true
				};
				missionStep3.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral4LuminatePrisonerRelease,
					description = "Talk to " + Characters.luminateCommander.name,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep3);
				SeededRandom random = new SeedGenerator().Add("LuminatePrisoner").CreateRandom();
				mission.rewards.Add(new Source.MissionSystem.Rewards.Crew
				{
					crew = CrewMemberData.CreateCustomCrewMember(Gender.Male, "John", "", "Raythor", CrewIcons.Get("M2Captain"), Profession.Industrial, Rarity.Standard, random, true),
					hidden = true
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission5"
				});
				mission.rewards.Add(new Experience
				{
					amount = 550
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 120
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x0003B728 File Offset: 0x00039928
		private Source.Galaxy.POI.Combat AddPrisonStation(SystemMapData system)
		{
			Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
			system.SetupPOI(combat, null, Faction.gold, 0);
			combat.SetIdentifier("pup4prison");
			combat.name = "Prison Outpost";
			combat.hidden = true;
			this.CreatePrisonStation(combat);
			combat.dangerLevel = "@MapPOIDangerSpaceStation";
			return combat;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0003B784 File Offset: 0x00039984
		private void CreatePrisonStation(MapPointOfInterest poi)
		{
			CombatStationPartData combatStationPartData = new CombatStationPartData("Luminate_Refinery1_2C")
			{
				faction = poi.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationPartData.positionData.rotation = 0f;
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = poi.GetWorldPosition() + new Vector2(15f, 0f);
			combatStationData.AddPart(combatStationPartData);
			CombatStationPartData part = new CombatStationPartData("Luminate_DefenseTurretS1_1C")
			{
				faction = poi.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 1, 1, 0);
			part = new CombatStationPartData("Luminate_Energy1_4C")
			{
				faction = poi.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 0, 2, 2);
			combatStationData.AddPart(new CombatStationPartData("Luminate_Connector_1CSmall")
			{
				faction = poi.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(2, 3, 3, 0);
			combatStationData.AddPart(new CombatStationPartData("Luminate_Connector_1C")
			{
				faction = poi.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(2, 0, 4, 0);
			combatStationData.AddPart(new CombatStationPartData("Luminate_Connector_1CSmall")
			{
				faction = poi.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(2, 1, 5, 0);
			poi.dangerLevel = "@MapPOIDangerSpaceStation";
			poi.AddPersistable(combatStationData);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0003B92C File Offset: 0x00039B2C
		private void UmbralMission5()
		{
			StoryMission.Add(new StoryMission("UmbralMission5", delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@UmbralMission5", Array.Empty<object>()),
					description = Translation.Translate("@UmbralMission5Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.puppeteers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SystemMapData current = SystemMapData.current;
				Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
				combat.system = current;
				current.SetupPOI(combat, null, Faction.gold, 0);
				combat.AddGuards(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 10, 2, 2, null), null);
				MissionStep missionStep = new MissionStep
				{
					system = combat.system,
					dynamicPointOfInterest = combat,
					hidden = true
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = combat.guid
				});
				mission.steps.Add(missionStep);
				SectorMapData sectorMapData = GalaxyMapData.current.allSectors.ElementAt(1);
				MapPointOfInterest mapPointOfInterest = GalaxyMapData.current.allSectors.ElementAt(0).FindJumpgateToSector(GalaxyMapData.current.allSectors.ElementAt(1));
				MissionStep missionStep2 = new MissionStep
				{
					system = mapPointOfInterest.system,
					poiHint = mapPointOfInterest,
					hidden = true
				};
				missionStep2.objectives.Add(new TravelToPOI
				{
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep2);
				SpaceStation spaceStation = sectorMapData.FindStationForFaction(Faction.red, new bool?(false));
				Source.Galaxy.POI.Combat combat2 = new Source.Galaxy.POI.Combat();
				spaceStation.system.SetupPOI(combat2, null, Faction.marauders, 0);
				combat2.AddGuards(combat2.CreateUnitPayload(1f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 0, 1, 5, null), null);
				MissionStep missionStep3 = new MissionStep
				{
					system = combat2.system,
					dynamicPointOfInterest = combat2,
					hidden = true
				};
				missionStep3.objectives.Add(new TravelToPOI
				{
					targetPOI = combat2.guid
				});
				mission.steps.Add(missionStep3);
				Source.Galaxy.POI.Salvage salvage = spaceStation.system.SetupPOI(new Source.Galaxy.POI.Salvage(), null, null, 0) as Source.Galaxy.POI.Salvage;
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2(8f, 2f),
					angle = (float)SeededRandom.Global.RandomRange(0, 360),
					shipTemplate = "Oxlo Mk II"
				};
				salvageData.AddScrapContent(salvage.level, 0.5f, 2);
				salvageData.AddStructuralContent(salvage.level, 2, 1f);
				salvage.AddPersistable(salvageData);
				SpaceShipData spaceShipData = new SpaceShipData("Tugbit", false, Faction.salvageGuild)
				{
					autoActions = "AmbientSalvager"
				};
				spaceShipData.commanderData.SetName("Thundo", "", "Klipz");
				salvage.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				MissionStep missionStep4 = new MissionStep
				{
					system = salvage.system,
					poiHint = salvage,
					hidden = true
				};
				missionStep4.poiHint = salvage;
				missionStep4.dynamicPointOfInterest = salvage;
				missionStep4.objectives.Add(new TravelToPOI
				{
					targetPOI = salvage.guid
				});
				mission.steps.Add(missionStep4);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission5b"
				});
				mission.rewards.Add(new Experience
				{
					amount = 900
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 180
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0003B95E File Offset: 0x00039B5E
		private void UmbralMission5b()
		{
			StoryMission.Add(new StoryMission("UmbralMission5b", delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@UmbralMission5b", Array.Empty<object>()),
					description = Translation.Translate("@UmbralMission5bDescription", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.puppeteers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SectorMapData sectorMapData = GalaxyMapData.current.allSectors.ElementAt(1);
				SpaceStation spaceStation = sectorMapData.FindStationForFaction(Faction.red, new bool?(false));
				spaceStation.characters.Add("ExpeditionKolyatovCaptain");
				MissionStep missionStep = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = spaceStation.guid
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.DockedWithSpaceStation,
					description = "Dock with " + spaceStation.name,
					requiredAmount = 1
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral5KolyatovWelcome,
					description = "See if " + Characters.kolyatovCaptain.name + " is here",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				SpaceStation spaceStation2 = sectorMapData.FindStationForFaction(Faction.salvageGuild, new bool?(false));
				spaceStation2.characters.Add("SteelVultureComputerSalesman");
				MissionStep missionStep2 = new MissionStep
				{
					requireAllObjectives = false,
					hidden = true
				};
				missionStep2.AddPoiHints(new MapPointOfInterest[]
				{
					spaceStation,
					spaceStation2
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral5KolyatovAttack,
					description = string.Concat(new string[]
					{
						"Talk to ",
						Characters.kolyatovCaptain.name,
						" and assist ",
						Translation.Translate(Faction.red.name, Array.Empty<object>()),
						" in assaulting ",
						Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>())
					}),
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.UmbralSteelVultureComputer,
					description = "Go to " + spaceStation2.name + " and buy the Computer",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission6"
				});
				mission.rewards.Add(new Experience
				{
					amount = 700
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 120
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0003B990 File Offset: 0x00039B90
		public static void AttackChoiceSteelVultures()
		{
			Mission mission = GamePlayer.current.missions.FirstOrDefault((Mission m) => m.storyId == "UmbralMission5b");
			MapElement mapElement = GalaxyMapData.current.allSectors.ElementAt(1).FindStationForFaction(Faction.salvageGuild, new bool?(false));
			CombatStation combatStation = new CombatStation();
			mapElement.system.SetupPOI(combatStation, null, Faction.salvageGuild, 0);
			string factionPrefix = combatStation.faction.GetFactionPrefix();
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = combatStation.GetWorldPosition() + new Vector2(15f, 0f);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationPartData.positionData.rotation = 0f;
			combatStationPartData.AddLoot("UmbralComputer", 1);
			combatStationData.AddPart(combatStationPartData);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(0, 3, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Refinery1_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(0, 0, 3, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Refinery1_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(0, 2, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(1, 1, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(2, 1, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(6, 1, 7, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(3, 0, 8, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(4, 0, 9, 0);
			combatStation.AddPersistable(combatStationData);
			List<AbstractUnitData> list = combatStation.CreateUnitPayload(1f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 0, 1, 5, null);
			foreach (AbstractUnitData abstractUnitData in list)
			{
				abstractUnitData.alwaysHostile = true;
				abstractUnitData.noReputationLoss = true;
			}
			combatStation.AddGuards(list, null);
			List<AbstractUnitData> list2 = combatStation.CreateFixedPayload("Kamin", 1, Faction.red, null, UnitRank.Veteran);
			List<AbstractUnitData> list3 = combatStation.CreateFixedPayload("Stilet", 2, Faction.red, null, UnitRank.Rookie);
			SeededRandom global = SeededRandom.Global;
			foreach (AbstractUnitData abstractUnitData2 in list3)
			{
				abstractUnitData2.positionData.position = combatStation.GetWorldPosition() + new Vector2(global.RandomRange(-12f, -25f), global.RandomRange(-9f, 9f));
				combatStation.AddUnit(abstractUnitData2, null, false);
			}
			foreach (AbstractUnitData abstractUnitData3 in list2)
			{
				abstractUnitData3.positionData.position = combatStation.GetWorldPosition() + new Vector2(global.RandomRange(-12f, -25f), global.RandomRange(-9f, 9f));
				combatStation.AddUnit(abstractUnitData3, null, false);
			}
			MissionStep missionStep = new MissionStep
			{
				system = combatStation.system,
				dynamicPointOfInterest = combatStation
			};
			missionStep.objectives.Add(new TravelToPOI
			{
				targetPOI = combatStation.guid
			});
			missionStep.dynamicPointOfInterest = combatStation;
			missionStep.objectives.Add(new TriggerObjective
			{
				trigger = MissionTrigger.CombatStationDestroyed,
				description = "Attack and destroy the Steel Vultures outpost",
				requiredAmount = 1
			});
			missionStep.objectives.Add(new Source.MissionSystem.Objectives.Item
			{
				itemType = InventoryItemType.Get("UmbralComputer"),
				requiredAmount = 1
			});
			mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
			{
				amount = 200,
				faction = Faction.red
			});
			mission.steps.Add(missionStep);
			MissionObjective.Trigger(MissionTrigger.Umbral5KolyatovAttack, 1, null, false);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0003BF74 File Offset: 0x0003A174
		private void UmbralMission6()
		{
			StoryMission.Add(new StoryMission("UmbralMission6", delegate(GamePlayer ply)
			{
				Beacon beacon = GalaxyMapData.current.allSectors.ElementAt(1).GetRandomSystemInRange(0.6f, 1f, false).AddBeacon("UmbralBeacon2", Faction.puppeteers, null);
				beacon.AddCargoContainers(new Vector2(5f, 8f), 2, 1f);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@UmbralMission6", Array.Empty<object>());
				mission.description = Translation.Translate("@UmbralMission6Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = beacon.system,
					poiHint = beacon
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = beacon.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = beacon.system,
					poiHint = beacon
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.InteractWithUmbralBeacon,
					description = Translation.Translate("@UmbralMissionInteractBeacon", Array.Empty<object>()),
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 50
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission7"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0003BFA6 File Offset: 0x0003A1A6
		private void UmbralMission7()
		{
			StoryMission.Add(new StoryMission("UmbralMission7", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = GalaxyMapData.current.allSectors.ElementAt(1).FindStationForFaction(Faction.blue, new bool?(false));
				spaceStation.characters.Add("StellarRepresentative");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@UmbralMission7", Array.Empty<object>());
				mission.description = Translation.Translate("@UmbralMission7Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = spaceStation.system,
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = spaceStation.guid
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral7StellarWelcome,
					description = "Talk to someone from " + Translation.Translate(Faction.blue.name, Array.Empty<object>()),
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					requireAllObjectives = false,
					hidden = true
				};
				missionStep2.objectives.Add(new Source.MissionSystem.Objectives.Reputation
				{
					faction = Faction.blue,
					reputationLevel = ReputationLevel.Friendly
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral7StellarSkirmish,
					description = "Help out " + Translation.Translate(Faction.blue.name, Array.Empty<object>()) + " with their pirate problem",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				MissionStep missionStep3 = new MissionStep
				{
					requireAllObjectives = false,
					system = spaceStation.system,
					poiHint = spaceStation,
					hidden = true
				};
				missionStep3.objectives.Add(new TravelToPOI
				{
					targetPOI = spaceStation.guid
				});
				missionStep3.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral7StellarComplete,
					description = "Talk to " + Characters.stellarRepresentative.name + " to receive the plans.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep3);
				InventoryItemType item = InventoryItemType.Get("UmbralDroneSchematics");
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = item,
					hidden = true
				});
				mission.rewards.Add(new Experience
				{
					amount = 500
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 90
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission8"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0003BFD8 File Offset: 0x0003A1D8
		public static void SkirmishChoiceStellarPirates()
		{
			SystemMapData current = SystemMapData.current;
			SystemMapData systemMapData = SandboxWorld.AddSideContentSystemToSystem(current.sector, current, 4);
			systemMapData.storyteller = SystemStoryteller.Create("FactionSkirmish", systemMapData);
			FactionSkirmish factionSkirmish = systemMapData.storyteller as FactionSkirmish;
			if (factionSkirmish != null)
			{
				factionSkirmish.factionA = Faction.marauders;
				factionSkirmish.factionB = Faction.blue;
			}
			systemMapData.storyteller.SetupSystem();
			systemMapData.GetEntranceJumpgate().name = Translation.Translate("@GateTo", new object[]
			{
				systemMapData.name
			}) + " (" + systemMapData.level.ToString() + ")";
			Mission mission = GamePlayer.current.missions.FirstOrDefault((Mission m) => m.storyId == "UmbralMission7");
			JumpGate jumpGate = null;
			foreach (MapPointOfInterest mapPointOfInterest in systemMapData.allPointsOfInterest)
			{
				JumpGate jumpGate2 = mapPointOfInterest as JumpGate;
				if (jumpGate2 != null)
				{
					jumpGate = jumpGate2;
				}
			}
			InventoryItemType item = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass((JumpGate)GalaxyMapData.current.GetPointOfInterest(jumpGate.targetPoiGuid), null);
			GamePlayer.current.currentSpaceShip.AddCargo(item, 1, false);
			CombatStation poiHint = null;
			foreach (MapPointOfInterest mapPointOfInterest2 in systemMapData.allPointsOfInterest)
			{
				CombatStation combatStation = mapPointOfInterest2 as CombatStation;
				if (combatStation != null && combatStation.faction == Faction.marauders)
				{
					poiHint = combatStation;
				}
			}
			MissionStep missionStep = new MissionStep
			{
				system = jumpGate.system,
				poiHint = poiHint
			};
			missionStep.objectives.Add(new TravelToPOI
			{
				targetPOI = jumpGate.guid
			});
			missionStep.objectives.Add(new TriggerObjective
			{
				trigger = MissionTrigger.CombatStationDestroyed,
				description = "Attack and destroy the Pirate outpost",
				requiredAmount = 1
			});
			mission.steps.Insert(mission.steps.Count - 1, missionStep);
			mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
			{
				amount = 200,
				faction = Faction.blue
			});
			MissionObjective.Trigger(MissionTrigger.Umbral7StellarSkirmish, 1, null, false);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0003C240 File Offset: 0x0003A440
		private void UmbralMission8()
		{
			StoryMission.Add(new StoryMission("UmbralMission8", delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(2).GetRandomSystemInRange(0f, 0.3f, false);
				Beacon beacon = randomSystemInRange.AddBeacon("UmbralBeacon3", Faction.puppeteers, null);
				beacon.AddCargoContainers(new Vector2(5f, 8f), 3, 1f);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@UmbralMission8", Array.Empty<object>());
				mission.description = Translation.Translate("@UmbralMission8Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = randomSystemInRange,
					poiHint = beacon
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = beacon.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = randomSystemInRange,
					poiHint = beacon
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.InteractWithUmbralBeacon,
					description = Translation.Translate("@UmbralMissionInteractBeacon", Array.Empty<object>()),
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 50
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission9"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0003C274 File Offset: 0x0003A474
		private void UmbralMission9()
		{
			string storyMission = "UmbralMission9";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SectorMapData sectorMapData = GalaxyMapData.current.allSectors.ElementAt(2);
				SpaceStation spaceStation = sectorMapData.FindStationForFaction(Faction.salvageGuild, new bool?(true));
				if (spaceStation == null)
				{
					SystemMapData systemMapData = SandboxWorld.AddSideContentSystem(sectorMapData);
					systemMapData.storyteller = SystemStoryteller.Create("DerelictFleet", systemMapData);
					systemMapData.storyteller.SetupSystem();
					systemMapData.GetEntranceJumpgate().name = Translation.Translate("@GateTo", new object[]
					{
						systemMapData.name
					}) + " (" + systemMapData.level.ToString() + ")";
					spaceStation = sectorMapData.FindStationForFaction(Faction.salvageGuild, new bool?(true));
				}
				spaceStation.characters.Add("SteelVulturePocket");
				SystemMapData system = spaceStation.system;
				Source.Galaxy.POI.Salvage salvage = new Source.Galaxy.POI.Salvage();
				system.SetupPOI(salvage, null, null, 0);
				int num = 8;
				for (int i = 0; i < num; i++)
				{
					int value = SeededRandom.Global.RandomRange(6, 11);
					salvage.AddPersistable(new SalvageData
					{
						position = salvage.GetWorldPosition() + new Vector2((float)SeededRandom.Global.RandomRange(0, 20), (float)SeededRandom.Global.RandomRange(-10, 10)),
						shipTemplate = "DroneWreck",
						scrapContent = new Dictionary<InventoryItemType, int>
						{
							{
								"DroneParts",
								value
							}
						}
					});
				}
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				InventoryItemType itemType = InventoryItemType.Get("DroneParts");
				InventoryItemType itemType2 = InventoryItemType.Get("UmbralDroneSchematics");
				MissionStep missionStep = new MissionStep
				{
					system = system,
					poiHint = salvage
				};
				missionStep.dynamicPointOfInterest = salvage;
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Salvage
				{
					itemType = itemType,
					requiredAmount = 20
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = system,
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TradeOffer
				{
					deliverTo = spaceStation,
					itemType = itemType,
					requiredAmount = 20
				});
				missionStep2.objectives.Add(new TradeOffer
				{
					deliverTo = spaceStation,
					itemType = itemType2,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 750
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 180
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission10"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0003C2B0 File Offset: 0x0003A4B0
		private void UmbralMission10()
		{
			string storyMission = "UmbralMission10";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.smugglers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.TryAddCharacter("SmugglerContact");
				spaceStation.characters.Remove("SteelVulturePocket");
				MissionStep missionStep = new MissionStep
				{
					system = SystemMapData.current,
					poiHint = MapPointOfInterest.current
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.DockedWithSpaceStation,
					description = "Dock with the station",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = SystemMapData.current,
					poiHint = MapPointOfInterest.current
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral10Smuggler,
					description = "Talk to the man",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission11"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0003C2EC File Offset: 0x0003A4EC
		private void UmbralMission11()
		{
			string storyMission = "UmbralMission11";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(3).GetRandomSystemInRange(0f, 0.4f, false);
				CombatStation combatStation = new CombatStation();
				randomSystemInRange.SetupPOI(combatStation, null, Faction.smugglers, 0);
				SpaceStation spaceStation = randomSystemInRange.AddSpaceStation(new HashSet<SpaceStationFacility>
				{
					SpaceStationFacility.GeneralShop,
					SpaceStationFacility.Bar,
					SpaceStationFacility.MissionBoard
				}, null, Faction.smugglers, null, null);
				spaceStation.characters.Add("SmugglerSector4");
				string factionPrefix = combatStation.faction.GetFactionPrefix();
				CombatStationData combatStationData = new CombatStationData();
				combatStationData.position = combatStation.GetWorldPosition() + new Vector2(15f, 0f);
				CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
				{
					faction = combatStation.faction
				};
				combatStationPartData.positionData.rotation = 0f;
				combatStationData.AddPart(combatStationPartData);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(0, 1, 1, 1);
				CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_1C")
				{
					faction = combatStation.faction
				};
				combatStationData.AddPart(part);
				combatStationData.ConnectParts(0, 0, 2, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(0, 2, 3, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1C")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(0, 3, 4, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_3C")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(1, 0, 5, 0);
				part = new CombatStationPartData(factionPrefix + "_DefenseTurretS2_2C")
				{
					faction = combatStation.faction
				};
				combatStationData.AddPart(part);
				combatStationData.ConnectParts(5, 1, 6, 1);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(5, 2, 7, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(6, 0, 8, 0);
				combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
				{
					faction = combatStation.faction
				});
				combatStationData.ConnectParts(7, 1, 9, 0);
				combatStation.AddPersistable(combatStationData);
				Faction marauders = Faction.marauders;
				Faction smugglers = Faction.smugglers;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.smugglers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				int num = 0;
				MapPointOfInterest mapPointOfInterest = combatStation;
				float pointsScale = 0.5f;
				Faction f = marauders;
				List<AbstractUnitData> list = mapPointOfInterest.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), f, 0, 0, 1, 5, null);
				combatStation.AddTriggeredSpawn(list, (float)SeededRandom.Global.RandomRange(2, 4), 0, false, true);
				num += list.Count;
				MapPointOfInterest mapPointOfInterest2 = combatStation;
				float pointsScale2 = 1f;
				f = marauders;
				list = mapPointOfInterest2.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), f, 0, 0, 1, 5, null);
				combatStation.AddTriggeredSpawn(list, (float)SeededRandom.Global.RandomRange(20, 15), 0, false, true);
				num += list.Count;
				MissionStep missionStep = new MissionStep
				{
					system = combatStation.system,
					dynamicPointOfInterest = combatStation
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = marauders,
					requiredAmount = num
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral11Smuggler,
					description = "Talk to a smuggler."
				});
				mission.steps.Add(missionStep2);
				InventoryItemType item = InventoryItemType.Get("EscortMissionItem0");
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = item,
					amount = 20,
					hidden = true
				});
				mission.rewards.Add(new Experience
				{
					amount = 500
				});
				mission.rewards.Add(new Credits
				{
					amount = 2000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 250
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission12"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0003C328 File Offset: 0x0003A528
		private void UmbralMission12()
		{
			string storyMission = "UmbralMission12";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = GalaxyMapData.current.allSectors.ElementAt(3).FindStationForFaction(Faction.blue, null);
				spaceStation.characters.Add("StellarStagingPoint");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.smugglers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				InventoryItemType itemType = InventoryItemType.Get("EscortMissionItem0");
				MissionStep missionStep = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = itemType,
					requiredAmount = 20,
					deliverTo = spaceStation
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral12Stellar,
					description = "Talk to the Stellar Representative."
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 250
				});
				mission.rewards.Add(new Credits
				{
					amount = 10000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 200
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission13"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0003C364 File Offset: 0x0003A564
		private void UmbralMission13()
		{
			string storyMission = "UmbralMission13";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = GalaxyMapData.current.allSectors.ElementAt(4).GetRandomSystemInRange(0.2f, 0.5f, false).AddSpaceStation(new HashSet<SpaceStationFacility>
				{
					SpaceStationFacility.GeneralShop,
					SpaceStationFacility.Bar,
					SpaceStationFacility.MissionBoard
				}, null, Faction.smugglers, null, null);
				spaceStation.characters.Add("SmugglerContact");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.smugglers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral13Smuggler,
					description = "Talk to the Smuggler contact."
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Experience
				{
					amount = 100
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission14"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0003C3A0 File Offset: 0x0003A5A0
		private void UmbralMission14()
		{
			string storyMission = "UmbralMission14";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData current = SystemMapData.current;
				MapElement poi = new Escort();
				Faction smugglers = Faction.smugglers;
				MapPointOfInterest mapPointOfInterest = current.SetupPOI(poi, null, smugglers, 0) as MapPointOfInterest;
				mapPointOfInterest.dangerLevel = "@MapPOIDangerPirates";
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.puppeteers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story,
					nextMissionOnFailed = "UmbralMission14Failed"
				};
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				List<AbstractUnitData> list = mapPointOfInterest.CreateFixedPayload("Trundar", 1, Faction.smugglers, null, UnitRank.Rookie);
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.AddCargo(InventoryItemType.Get("EscortMissionItem0"), 10, false);
					abstractUnitData.autoActions = "AmbientEscort";
				}
				mapPointOfInterest.AddGuards(list, null);
				CombatStationFactory.CreateEscortLocation(mapPointOfInterest);
				missionStep.objectives.Add(new ProtectUnit
				{
					enemyFaction = Faction.marauders
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.EscortUnitCargoUnloaded,
					requiredAmount = 10,
					description = "Wait for the cargo ship to unload its cargo"
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = MapPointOfInterest.current,
					hidden = true
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral14SmugglerComplete,
					description = "Talk to James."
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 750
				});
				mission.rewards.Add(new Credits
				{
					amount = 25000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 300
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission15"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0003C3DC File Offset: 0x0003A5DC
		private void UmbralMission14Failed()
		{
			string storyMission = "UmbralMission14Failed";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SpaceStation poiHint = GalaxyMapData.current.allSectors.ElementAt(3).FindStationForFaction(Faction.smugglers, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					poiHint = poiHint
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral14SmugglerFailed,
					description = "Talk to James."
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission14"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0003C418 File Offset: 0x0003A618
		private void UmbralMission15()
		{
			string storyMission = "UmbralMission15";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(4).GetRandomSystemInRange(0.3f, 0.6f, false);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction darkspacers = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, darkspacers, 0) as MapPointOfInterest;
				SpaceShipData spaceShipData = new SpaceShipData("Syladon", false, Faction.darkspacers);
				spaceShipData.commanderData.SetName("Midas", "", "");
				mapPointOfInterest.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Experience
				{
					amount = 200
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 50,
					faction = Faction.darkspacers
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission16"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0003C454 File Offset: 0x0003A654
		private void UmbralMission16()
		{
			string storyMission = "UmbralMission16";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(5).GetRandomSystemInRange(0.3f, 0.6f, false);
				MapElement poi = new CombatStation();
				Faction darkspacers = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, darkspacers, 0) as MapPointOfInterest;
				CombatStationData combatStationData = new CombatStationData();
				combatStationData.position = mapPointOfInterest.GetWorldPosition() + new Vector2(15f, 0f);
				combatStationData = CombatStationFactory.CreateEasyStation1(mapPointOfInterest, null);
				foreach (CombatStationPartData combatStationPartData in combatStationData.stationParts)
				{
					combatStationPartData.alwaysHostile = true;
					combatStationPartData.noReputationLoss = true;
				}
				combatStationData.AddLoot("UmbralMetafiber", 21);
				SpaceShipData spaceShipData = new SpaceShipData("PetaraDRK", false, Faction.darkspacers)
				{
					noReputationLoss = true,
					alwaysHostile = true
				};
				spaceShipData.commanderData.SetName("", "Dark Barry", "");
				mapPointOfInterest.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new KillEnemies
				{
					requiredAmount = mapPointOfInterest.totalEnemyCount,
					enemyFaction = Faction.darkspacers
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CombatStationDestroyed,
					description = "Destroy the station."
				});
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Item
				{
					requiredAmount = 21,
					itemType = InventoryItemType.Get("UmbralMetafiber")
				});
				mission.steps.Add(missionStep);
				SpaceStation spaceStation = GalaxyMapData.current.allSectors.ElementAt(4).FindStationForFaction(Faction.smugglers, null);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TradeOffer
				{
					requiredAmount = 20,
					itemType = InventoryItemType.Get("UmbralMetafiber"),
					deliverTo = spaceStation
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral16Smuggler,
					description = "Talk to James Fleddon"
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = 1800
				});
				mission.rewards.Add(new Credits
				{
					amount = 25000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 400,
					faction = Faction.darkspacers
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 500,
					faction = Faction.smugglers
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission17"
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "DarkspaceMission1"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0003C490 File Offset: 0x0003A690
		private void UmbralMission17()
		{
			string storyMission = "UmbralMission17";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Beacon beacon = GalaxyMapData.current.allSectors.ElementAt(6).GetRandomSystemInRange(0.3f, 0.6f, false).AddBeacon("UmbralBeacon4", Faction.puppeteers, null);
				beacon.AddCargoContainers(new Vector2(5f, 8f), 2, 1f);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = beacon.system,
					poiHint = beacon
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.InteractWithUmbralBeacon,
					description = Translation.Translate("@UmbralMissionInteractBeacon", Array.Empty<object>())
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission18"
				});
				mission.rewards.Add(new Experience
				{
					amount = 250
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0003C4CC File Offset: 0x0003A6CC
		private void UmbralMission18()
		{
			string storyMission = "UmbralMission18";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = GalaxyMapData.current.allSectors.ElementAt(6).FindStationForFaction(Faction.blue, null);
				spaceStation.characters.Add("StellarStagingPoint");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				InventoryItemType itemType = InventoryItemType.Get("UmbralIsolationChamber");
				MissionStep missionStep = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = itemType,
					requiredAmount = 1,
					deliverTo = spaceStation,
					customDescription = "Craft the Isolation Chamber and deliver it to " + spaceStation.name
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral18Stellar,
					description = "Talk to the Stellar spy"
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission19"
				});
				mission.rewards.Add(new Experience
				{
					amount = 2000
				});
				mission.rewards.Add(new Credits
				{
					amount = 25000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0003C508 File Offset: 0x0003A708
		private void UmbralMission19()
		{
			string storyMission = "UmbralMission19";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				MapPointOfInterest pointOfInterest = GalaxyMapData.current.GetPointOfInterest("UmbralBeacon4");
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = pointOfInterest.system,
					poiHint = pointOfInterest
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.InteractWithUmbralBeacon,
					description = Translation.Translate("@UmbralMissionInteractBeacon", Array.Empty<object>())
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission20"
				});
				mission.rewards.Add(new Experience
				{
					amount = 250
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0003C544 File Offset: 0x0003A744
		private void UmbralMission20()
		{
			string storyMission = "UmbralMission20";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = GalaxyMapData.current.allSectors.ElementAt(6).FindStationForFaction(Faction.blue, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					poiHint = spaceStation
				};
				InventoryItemType itemType = InventoryItemType.Get("UmbralComputer");
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = itemType,
					deliverTo = spaceStation,
					requiredAmount = 1
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral20Stellar,
					description = "Talk to Stella"
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission21"
				});
				mission.rewards.Add(new Experience
				{
					amount = 1500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0003C580 File Offset: 0x0003A780
		private void UmbralMission21()
		{
			string storyMission = "UmbralMission21";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					poiHint = MapPointOfInterest.current
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral21Umbral,
					description = "Talk to Umbral."
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission22"
				});
				mission.rewards.Add(new Experience
				{
					amount = 2500
				});
				mission.rewards.Add(new Credits
				{
					amount = 40000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0003C5BC File Offset: 0x0003A7BC
		private void UmbralMission22()
		{
			string storyMission = "UmbralMission22";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(7).GetRandomSystemInRange(0.7f, 0.7f, false);
				MapElement poi = new Escort();
				Faction customFaction = Faction.puppeteers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, customFaction, 0) as MapPointOfInterest;
				mapPointOfInterest.dangerLevel = "@MapPOIDangerPirates";
				MapElement poi2 = new Source.Galaxy.POI.Combat();
				customFaction = Faction.fanatics;
				MapPointOfInterest mapPointOfInterest2 = randomSystemInRange.SetupPOI(poi2, null, customFaction, 0) as MapPointOfInterest;
				mapPointOfInterest2.dangerLevel = "@MapPOIDangerPirates";
				mapPointOfInterest2.AddGuards(mapPointOfInterest2.CreateUnitPayload(0.75f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 0, 3, 5, null), null);
				mapPointOfInterest2.AddTriggeredSpawn(mapPointOfInterest2.CreateUnitPayload(0.75f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 0, 3, 5, null), 30f, 0, false, true);
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.puppeteers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story,
					nextMissionOnFailed = "UmbralMission22Failed"
				};
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				List<AbstractUnitData> list = mapPointOfInterest.CreateUnitPayload(1f, new GameplayType?(GameplayType.Cargo), Faction.puppeteers, 0, 0, 1, 1, null);
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.AddCargo(InventoryItemType.Get("EscortMissionItem0"), 10, false);
					abstractUnitData.autoActions = "AmbientEscort";
				}
				mapPointOfInterest.AddGuards(list, null);
				CombatStationFactory.CreateEscortLocation(mapPointOfInterest);
				missionStep.objectives.Add(new ProtectUnit
				{
					enemyFaction = Faction.fanatics
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.EscortUnitCargoUnloaded,
					requiredAmount = 10,
					description = "Wait for the cargo ship to unload its cargo"
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = mapPointOfInterest2.system,
					dynamicPointOfInterest = mapPointOfInterest2
				};
				missionStep2.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.fanatics,
					requiredAmount = mapPointOfInterest2.totalEnemyCount
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission23"
				});
				mission.rewards.Add(new Experience
				{
					amount = 2500
				});
				mission.rewards.Add(new Credits
				{
					amount = 50000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
		private void UmbralMission22Failed()
		{
			string storyMission = "UmbralMission22Failed";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SpaceStation poiHint = GalaxyMapData.current.allSectors.ElementAt(6).FindStationForFaction(Faction.puppeteers, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					poiHint = poiHint
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.Umbral22Failed,
					description = "Talk to Umbral."
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "UmbralMission22"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0003C634 File Offset: 0x0003A834
		private void UmbralMission23()
		{
			string storyMission = "UmbralMission23";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				MapPointOfInterest mapPointOfInterest = UmbralMissions.CreateConstructionSitePOI();
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Mining
				{
					requiredAmount = 40,
					itemCategory = new ItemCategory?(ItemCategory.Ore),
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Salvage
				{
					requiredAmount = 40,
					itemCategory = new ItemCategory?(ItemCategory.Salvage),
					targetPOI = mapPointOfInterest.guid
				});
				mission.rewards.Add(new Experience
				{
					amount = 2500
				});
				mission.rewards.Add(new Credits
				{
					amount = 250000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0003C670 File Offset: 0x0003A870
		public static MapPointOfInterest CreateConstructionSitePOI()
		{
			SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(7).GetRandomSystemInRange(0.7f, 0.7f, false);
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining
			{
				name = "Potential Jumpgate Site"
			};
			mining.SetAsteroidFieldData(randomSystemInRange.systemOreData, 10);
			SystemMapData systemMapData = randomSystemInRange;
			MapElement poi = mining;
			Faction faction = Faction.puppeteers;
			systemMapData.SetupPOI(poi, null, faction, 0);
			int num = 8;
			string[] list = new string[]
			{
				"Oryne",
				"Firecs",
				"Syladon",
				"Thorne"
			};
			for (int i = 0; i < num; i++)
			{
				SalvageData salvageData = new SalvageData
				{
					position = mining.GetWorldPosition() + new Vector2((float)SeededRandom.Global.RandomRange(-40, -5), (float)SeededRandom.Global.RandomRange(-10, 10)),
					shipTemplate = SeededRandom.Global.Choose<string>(list)
				};
				salvageData.AddItemContent(mining.level, -1, 1f);
				salvageData.AddScrapContent(mining.level, 1f, 2);
				salvageData.AddStructuralContent(mining.level, 2, 1f);
				mining.AddPersistable(salvageData);
			}
			MapPointOfInterest mapPointOfInterest = mining;
			MapPointOfInterest mapPointOfInterest2 = mining;
			float pointsScale = 1f;
			faction = Faction.fanatics;
			mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest2.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null), 20f, 0, true, true);
			MapPointOfInterest mapPointOfInterest3 = mining;
			MapPointOfInterest mapPointOfInterest4 = mining;
			float pointsScale2 = 2f;
			faction = Faction.fanatics;
			mapPointOfInterest3.AddTriggeredSpawn(mapPointOfInterest4.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null), 15f, 0, true, true);
			MapPointOfInterest mapPointOfInterest5 = mining;
			MapPointOfInterest mapPointOfInterest6 = mining;
			float pointsScale3 = 2f;
			faction = Faction.fanatics;
			mapPointOfInterest5.AddTriggeredSpawn(mapPointOfInterest6.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null), 15f, 0, true, true);
			return mining;
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0003C84C File Offset: 0x0003AA4C
		private void DarkspaceMission1()
		{
			string storyMission = "DarkspaceMission1";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(6).GetRandomSystemInRange(0.2f, 0.6f, false);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction darkspacers = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, darkspacers, 0) as MapPointOfInterest;
				SpaceShipData spaceShipData = new SpaceShipData("Syladon", false, Faction.darkspacers);
				spaceShipData.commanderData.SetName("Midas", "", "");
				mapPointOfInterest.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.darkspacers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "DarkspaceMission2"
				});
				mission.rewards.Add(new Experience
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0003C888 File Offset: 0x0003AA88
		private void DarkspaceMission2()
		{
			string storyMission = "DarkspaceMission2";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.darkspacers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SystemMapData current = SystemMapData.current;
				SystemMapData systemMapData = SandboxWorld.AddSideContentSystemToSystem(current.sector, current, 4);
				systemMapData.storyteller = SystemStoryteller.Create("FactionSkirmish", systemMapData);
				FactionSkirmish factionSkirmish = systemMapData.storyteller as FactionSkirmish;
				if (factionSkirmish != null)
				{
					factionSkirmish.factionA = Faction.fanatics;
					factionSkirmish.factionB = Faction.darkspacers;
				}
				systemMapData.storyteller.SetupSystem();
				systemMapData.GetEntranceJumpgate().name = Translation.Translate("@GateTo", new object[]
				{
					systemMapData.name
				}) + " (" + systemMapData.level.ToString() + ")";
				JumpGate jumpGate = null;
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.allPointsOfInterest)
				{
					JumpGate jumpGate2 = mapPointOfInterest as JumpGate;
					if (jumpGate2 != null)
					{
						jumpGate2.UnlockJumpgate();
						jumpGate = (JumpGate)GalaxyMapData.current.GetPointOfInterest(jumpGate2.targetPoiGuid);
					}
				}
				CombatStation combatStation = null;
				foreach (MapPointOfInterest mapPointOfInterest2 in systemMapData.allPointsOfInterest)
				{
					CombatStation combatStation2 = mapPointOfInterest2 as CombatStation;
					if (combatStation2 != null && combatStation2.faction == Faction.fanatics)
					{
						combatStation = combatStation2;
					}
				}
				MissionStep missionStep = new MissionStep
				{
					system = jumpGate.system,
					poiHint = jumpGate
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = jumpGate.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = combatStation.system,
					poiHint = combatStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CombatStationDestroyed,
					description = "Attack and destroy the Meriada's Chosen stronghold.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "DarkspaceMission3"
				});
				mission.rewards.Add(new Experience
				{
					amount = 2000
				});
				mission.rewards.Add(new Credits
				{
					amount = 40000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0003C8C4 File Offset: 0x0003AAC4
		private void DarkspaceMission3()
		{
			string storyMission = "DarkspaceMission3";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(6).GetRandomSystemInRange(0.8f, 1f, false);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction darkspacers = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, darkspacers, 0) as MapPointOfInterest;
				SpaceShipData spaceShipData = new SpaceShipData("Syladon", false, Faction.darkspacers);
				spaceShipData.commanderData.SetName("Midas", "", "");
				mapPointOfInterest.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.darkspacers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "DarkspaceMission4"
				});
				mission.rewards.Add(new Experience
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0003C900 File Offset: 0x0003AB00
		private void DarkspaceMission4()
		{
			string storyMission = "DarkspaceMission4";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(7).GetRandomSystemInRange(0.6f, 0.6f, false);
				MapElement poi = new Escort();
				Faction blue = Faction.blue;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, blue, 0) as MapPointOfInterest;
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.darkspacers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story,
					nextMissionOnFailed = "DarkspaceMission4Failed"
				};
				List<AbstractUnitData> list = mapPointOfInterest.CreateFixedPayload("Oryne", 1, Faction.darkspacers, null, UnitRank.Rookie);
				int num = 1;
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.AddCargo(InventoryItemType.Get("EscortMissionItem0"), num, false);
					abstractUnitData.autoActions = "AmbientEscort";
				}
				mapPointOfInterest.AddGuards(list, null);
				foreach (CombatStationPartData combatStationPartData in CombatStationFactory.CreateEscortLocation(mapPointOfInterest).stationParts)
				{
					combatStationPartData.faction = Faction.blue;
					if (combatStationPartData.partPrefab.partType == CombatStationPartType.DefensePlatform)
					{
						combatStationPartData.alwaysHostile = true;
						combatStationPartData.noReputationLoss = true;
					}
					else
					{
						combatStationPartData.playerFriendly = true;
					}
				}
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new ProtectUnit
				{
					enemyFaction = Faction.blue,
					hostileNoRepLoss = true
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.EscortUnitCargoUnloaded,
					requiredAmount = num,
					description = "Wait for the cargo ship to unload its precious cargo"
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "DarkspaceMission5"
				});
				mission.rewards.Add(new Experience
				{
					amount = 2500
				});
				mission.rewards.Add(new Credits
				{
					amount = 25000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0003C93C File Offset: 0x0003AB3C
		private void DarkspaceMission4Failed()
		{
			string storyMission = "DarkspaceMission4Failed";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(7).GetRandomSystemInRange(0.4f, 0.6f, false);
				MapElement poi = new Escort();
				Faction blue = Faction.blue;
				randomSystemInRange.SetupPOI(poi, null, blue, 0);
				return new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>()),
					completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>()),
					sourceFaction = Faction.darkspacers,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story,
					rewards = 
					{
						new StoryMission
						{
							missionId = "DarkspaceMission4"
						}
					}
				};
			}, null, null));
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0003C978 File Offset: 0x0003AB78
		private void DarkspaceMission5()
		{
			string storyMission = "DarkspaceMission5";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(6).GetRandomSystemInRange(0.7f, 0.9f, false);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction darkspacers = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, darkspacers, 0) as MapPointOfInterest;
				SpaceShipData spaceShipData = new SpaceShipData("Syladon", false, Faction.darkspacers);
				spaceShipData.commanderData.SetName("Midas", "", "");
				mapPointOfInterest.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.darkspacers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "DarkspaceMission6"
				});
				mission.rewards.Add(new Experience
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0003C9B4 File Offset: 0x0003ABB4
		private void DarkspaceMission6()
		{
			string storyMission = "DarkspaceMission6";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				SystemMapData randomSystemInRange = GalaxyMapData.current.allSectors.ElementAt(8).GetRandomSystemInRange(0.2f, 0.4f, false);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction faction = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = randomSystemInRange.SetupPOI(poi, null, faction, 0) as MapPointOfInterest;
				mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest.CreateUnitPayload(1.5f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 0, 1, 5, null), 1f, 0, false, true);
				mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest.CreateFixedPayload("Terravex", 1, null, null, UnitRank.Veteran), 35f, 0, false, true);
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest3 = mapPointOfInterest;
				float pointsScale = 1f;
				faction = Faction.fanatics;
				mapPointOfInterest2.AddTriggeredSpawn(mapPointOfInterest3.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 200, 1, 5, null), 0f, 0, true, true);
				mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest.CreateFixedPayload("Carkalon", 1, Faction.fanatics, null, UnitRank.Standard), 30f, 0, false, true);
				MapPointOfInterest mapPointOfInterest4 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest5 = mapPointOfInterest;
				float pointsScale2 = 3f;
				faction = Faction.fanatics;
				mapPointOfInterest4.AddTriggeredSpawn(mapPointOfInterest5.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 200, 1, 5, null), 5f, 0, true, true);
				MapPointOfInterest mapPointOfInterest6 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest7 = mapPointOfInterest;
				float pointsScale3 = 3f;
				faction = Faction.fanatics;
				mapPointOfInterest6.AddTriggeredSpawn(mapPointOfInterest7.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 200, 1, 5, null), 5f, 0, true, true);
				MapPointOfInterest mapPointOfInterest8 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest9 = mapPointOfInterest;
				float pointsScale4 = 5f;
				faction = Faction.fanatics;
				mapPointOfInterest8.AddTriggeredSpawn(mapPointOfInterest9.CreateUnitPayload(pointsScale4, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 99, 200, 1, 5, null), 5f, 0, true, true);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Description", Array.Empty<object>());
				mission.completionText = Translation.Translate("@UmbralMissionCompletion", Array.Empty<object>());
				mission.sourceFaction = Faction.darkspacers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					system = mapPointOfInterest.system,
					dynamicPointOfInterest = mapPointOfInterest
				};
				int totalEnemyCount = mapPointOfInterest.totalEnemyCount;
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.fanatics,
					requiredAmount = totalEnemyCount
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new Experience
				{
					amount = 5000
				});
				mission.rewards.Add(new Credits
				{
					amount = 50000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x0003C9F0 File Offset: 0x0003ABF0
		private SpaceStation GetUmbralOutpost()
		{
			SpaceStation spaceStation = null;
			foreach (SystemMapData systemMapData in GamePlayer.current.map.allConquestStagingSystems)
			{
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					SpaceStation spaceStation2 = mapPointOfInterest as SpaceStation;
					if (spaceStation2 != null && mapPointOfInterest.faction == Faction.puppeteers)
					{
						spaceStation = spaceStation2;
						spaceStation.hidden = false;
					}
				}
			}
			return spaceStation;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0003CAA0 File Offset: 0x0003ACA0
		private void ConquestUmbral1()
		{
			string storyMission = "ConquestUmbral1";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				SpaceStation umbralOutpost = this.GetUmbralOutpost();
				umbralOutpost.hidden = false;
				umbralOutpost.TryAddCharacter("RealUmbral");
				MissionStep missionStep = new MissionStep
				{
					poiHint = umbralOutpost
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU1TalktoNPC,
					description = "Report to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral2"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0003CAF8 File Offset: 0x0003ACF8
		private void ConquestUmbral2()
		{
			string storyMission = "ConquestUmbral2";
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = Faction.puppeteers;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				InventoryItemType key = InventoryItemType.Get("UmbralUberTool");
				mission.missionItems.Add(key, 1);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.UmbralStationInfected,
					description = "Deploy the hacking tool on a friendly conquest station",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep();
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MissionBoardOpenedWithUmbral,
					description = "Go to the Mission Board of the infected station.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral3"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(1000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0003CB40 File Offset: 0x0003AD40
		private void ConquestUmbral3()
		{
			string storyMission = "ConquestUmbral3";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				this.GetUmbralOutpost().characters.Remove("RealUmbral");
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new StationsInfected
				{
					faction = faction,
					targetAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral4"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(1000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 800
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x0003CB98 File Offset: 0x0003AD98
		private void ConquestUmbral4()
		{
			string storyMission = "ConquestUmbral4";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = faction,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SpaceStation umbralOutpost = this.GetUmbralOutpost();
				MissionStep missionStep = new MissionStep
				{
					poiHint = umbralOutpost
				};
				umbralOutpost.TryAddCharacter("UmbralReachStella");
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU3TalktoNPC,
					description = "Report to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
				SystemMapData system = umbralOutpost.system;
				MapElement poi = combat;
				Faction mercenaryGuild = Faction.mercenaryGuild;
				int level = GamePlayer.current.level;
				system.SetupPOI(poi, null, mercenaryGuild, level);
				MapPointOfInterest mapPointOfInterest = combat;
				float pointsScale = 1f;
				mercenaryGuild = Faction.mercenaryGuild;
				UnitRank? fixedRank = new UnitRank?(UnitRank.Rookie);
				List<AbstractUnitData> list = mapPointOfInterest.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), mercenaryGuild, 0, 0, 3, 5, fixedRank);
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.alwaysHostile = true;
				}
				combat.AddGuards(list, null);
				MissionStep missionStep2 = new MissionStep
				{
					hidden = true,
					dynamicPointOfInterest = combat,
					system = combat.system
				};
				missionStep2.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.mercenaryGuild,
					requiredAmount = list.Count
				});
				mission.steps.Add(missionStep2);
				GamePlayer.current.GetStoryteller<Conquest>().GetEmbassy(faction);
				MissionStep missionStep3 = new MissionStep
				{
					hidden = true,
					poiHint = umbralOutpost
				};
				missionStep3.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU4TalktoNPC,
					description = "Report to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep3);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral5"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(2000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 1200
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0003CBF0 File Offset: 0x0003ADF0
		private void ConquestUmbral5()
		{
			string storyMission = "ConquestUmbral5";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new StationsInfected
				{
					faction = faction,
					targetAmount = 3
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral6"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(400f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(5000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0003CC40 File Offset: 0x0003AE40
		private void ConquestUmbral6()
		{
			string storyMission = "ConquestUmbral6";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				SpaceStation umbralOutpost = this.GetUmbralOutpost();
				MissionStep missionStep = new MissionStep
				{
					poiHint = umbralOutpost
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU5TalktoNPC,
					description = "Report to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				MapPointOfInterest mapPointOfInterest = this.AddResearchStation(Faction.blue, storyteller.GetEmbassy(Faction.blue).system);
				MapPointOfInterest mapPointOfInterest2 = this.AddResearchStation(Faction.red, storyteller.GetEmbassy(Faction.red).system);
				MapPointOfInterest mapPointOfInterest3 = this.AddResearchStation(Faction.gold, storyteller.GetEmbassy(Faction.gold).system);
				MissionStep missionStep2 = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				InventoryItemType itemType = InventoryItemType.Get("UmbralDataSlate");
				missionStep2.objectives.Add(new Source.MissionSystem.Objectives.Item
				{
					itemType = itemType,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				MissionStep missionStep3 = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest2,
					system = mapPointOfInterest2.system
				};
				missionStep3.objectives.Add(new Source.MissionSystem.Objectives.Item
				{
					itemType = itemType,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep3);
				MissionStep missionStep4 = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest3,
					system = mapPointOfInterest3.system
				};
				missionStep4.objectives.Add(new Source.MissionSystem.Objectives.Item
				{
					itemType = itemType,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep4);
				MissionStep missionStep5 = new MissionStep
				{
					poiHint = umbralOutpost,
					hidden = true
				};
				missionStep5.objectives.Add(new TradeOffer
				{
					itemType = itemType,
					deliverTo = umbralOutpost,
					requiredAmount = 3
				});
				missionStep5.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU6TalktoNPC,
					description = "Report to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep5);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral7"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(6000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 1400
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0003CC98 File Offset: 0x0003AE98
		private MapPointOfInterest AddResearchStation(Faction faction, SystemMapData system)
		{
			CombatStation combatStation = new CombatStation();
			MapElement poi = combatStation;
			int level = GamePlayer.current.level;
			system.SetupPOI(poi, null, faction, level);
			string factionPrefix = combatStation.faction.GetFactionPrefix();
			CombatStationData combatStationData = new CombatStationData();
			combatStationData.position = combatStation.GetWorldPosition() + new Vector2(15f, 0f);
			CombatStationPartData combatStationPartData = new CombatStationPartData(factionPrefix + "_Core_4C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationPartData.positionData.rotation = 0f;
			combatStationPartData.AddLoot("UmbralDataSlate", 1);
			combatStationData.AddPart(combatStationPartData);
			CombatStationPartData part = new CombatStationPartData(factionPrefix + "_DefenseDronebaySmall_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(0, 1, 1, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(0, 3, 2, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Refinery1_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(0, 0, 3, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Refinery1_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(0, 2, 4, 1);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(1, 1, 5, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_DockingSmall_2C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(2, 1, 6, 0);
			combatStationData.AddPart(new CombatStationPartData(factionPrefix + "_Connector_1CSmall")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			});
			combatStationData.ConnectParts(6, 1, 7, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(3, 0, 8, 0);
			part = new CombatStationPartData(factionPrefix + "_DefenseTurretS1_1C")
			{
				faction = combatStation.faction,
				alwaysHostile = true,
				noReputationLoss = true
			};
			combatStationData.AddPart(part);
			combatStationData.ConnectParts(4, 0, 9, 0);
			combatStation.AddPersistable(combatStationData);
			List<AbstractUnitData> list = combatStation.CreateUnitPayload(2f, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), null, 0, 0, 1, 5, null);
			foreach (AbstractUnitData abstractUnitData in list)
			{
				abstractUnitData.alwaysHostile = true;
			}
			combatStation.AddGuards(list, null);
			return combatStation;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0003D014 File Offset: 0x0003B214
		private void ConquestUmbral7()
		{
			string storyMission = "ConquestUmbral7";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new StationsInfected
				{
					faction = faction,
					targetAmount = 10
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral8"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(10000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 1000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0003D064 File Offset: 0x0003B264
		private void ConquestUmbral8()
		{
			string storyMission = "ConquestUmbral8";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				SpaceStation umbralOutpost = this.GetUmbralOutpost();
				MissionStep missionStep = new MissionStep
				{
					poiHint = this.GetUmbralOutpost(),
					system = umbralOutpost.system
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU8TalktoNPC,
					description = "Go to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral8b"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0003D0BC File Offset: 0x0003B2BC
		private void ConquestUmbral8b()
		{
			string storyMission = "ConquestUmbral8b";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				SystemMapData systemMapData = SeededRandom.Global.Choose<SystemMapData>(GalaxyMapData.current.allConquestSystems);
				Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
				MapElement poi = combat;
				int level = GamePlayer.current.level;
				systemMapData.SetupPOI(poi, null, null, level);
				combat.faction = Faction.smugglers;
				SpaceShipData spaceShipData = new SpaceShipData("AcolyteVoidweaver", false, Faction.smugglers)
				{
					unitRank = UnitRank.Champion,
					canBeTracked = false,
					alwaysHostile = true,
					canFlee = false
				};
				InventoryItemType inventoryItemType = InventoryItemType.Get("UmbralDataSlate");
				spaceShipData.AddLoot(inventoryItemType, 1);
				combat.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = combat,
					system = combat.system
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.smugglers,
					requiredAmount = 1
				});
				missionStep.objectives.Add(new Source.MissionSystem.Objectives.Item
				{
					itemType = inventoryItemType,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				SpaceStation umbralOutpost = this.GetUmbralOutpost();
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = umbralOutpost,
					hidden = true
				};
				missionStep2.objectives.Add(new TradeOffer
				{
					itemType = inventoryItemType,
					deliverTo = umbralOutpost,
					requiredAmount = 1
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU9TalktoNPC,
					description = "Return to the Umbral outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral9"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(10000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = InventoryItemType.Get("UmbralTrackingBeacon"),
					amount = 5
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = InventoryItemType.Get("UmbralCargoScanner"),
					amount = 5
				});
				mission.rewards.Add(new Ship
				{
					ship = "Eclipse"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 2000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0003D114 File Offset: 0x0003B314
		private void ConquestUmbral9()
		{
			string storyMission = "ConquestUmbral9";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new StationsInfected
				{
					faction = faction,
					targetAmount = 18
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral10"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(300f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(20000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 1000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0003D164 File Offset: 0x0003B364
		private void ConquestUmbral10()
		{
			string storyMission = "ConquestUmbral10";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep
				{
					poiHint = this.GetUmbralOutpost()
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CU10TalktoNPC,
					description = "Go to the Umbral Outpost",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral11"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0003D1BC File Offset: 0x0003B3BC
		private void ConquestUmbral11()
		{
			string storyMission = "ConquestUmbral11";
			Faction faction = Faction.puppeteers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new SystemsConquered
				{
					faction = faction,
					targetPercentage = 0.1f
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral12"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0003D20C File Offset: 0x0003B40C
		private void ConquestDarkspace1()
		{
			string storyMission = "ConquestDarkspace1";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				SystemMapData systemMapData = SeededRandom.Global.Choose<SystemMapData>(GalaxyMapData.current.allConquestStagingSystems);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction darkspacers = Faction.darkspacers;
				MapPointOfInterest mapPointOfInterest = systemMapData.SetupPOI(poi, null, darkspacers, 0) as MapPointOfInterest;
				SpaceShipData spaceShipData = new SpaceShipData("Terravex", false, Faction.darkspacers)
				{
					alwaysFriendly = true,
					unitRank = UnitRank.Legendary
				};
				spaceShipData.commanderData.SetName("Midas", "", "");
				mapPointOfInterest.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				mapPointOfInterest.AddCargoContainers(new Vector2(5f, 8f), 3, 1f);
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = mapPointOfInterest.guid
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace2"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 200
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0003D25C File Offset: 0x0003B45C
		private void ConquestDarkspace2()
		{
			string storyMission = "ConquestDarkspace2";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Faction faction;
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = faction,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SystemMapData systemMapData = SeededRandom.Global.Choose<SystemMapData>(GalaxyMapData.current.allConquestSystems);
				CombatStation combatStation = new CombatStation();
				MapElement poi = combatStation;
				faction = Faction.fanatics;
				int level = GamePlayer.current.level;
				systemMapData.SetupPOI(poi, null, faction, level);
				CombatStationFactory.CreateLargeStation(combatStation);
				MapPointOfInterest mapPointOfInterest = combatStation;
				float pointsScale = 2f;
				faction = Faction.darkspacers;
				List<AbstractUnitData> list = mapPointOfInterest.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null);
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.playerFriendly = true;
				}
				combatStation.AddTriggeredSpawn(list, 1f, 0, false, true);
				MapPointOfInterest mapPointOfInterest2 = combatStation;
				float pointsScale2 = 1.5f;
				faction = Faction.darkspacers;
				List<AbstractUnitData> list2 = mapPointOfInterest2.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null);
				foreach (AbstractUnitData abstractUnitData2 in list2)
				{
					abstractUnitData2.playerFriendly = true;
				}
				combatStation.AddTriggeredSpawn(list2, 30f, 0, false, true);
				MapPointOfInterest mapPointOfInterest3 = combatStation;
				MapPointOfInterest mapPointOfInterest4 = combatStation;
				float pointsScale3 = 1f;
				faction = Faction.fanatics;
				mapPointOfInterest3.AddTriggeredSpawn(mapPointOfInterest4.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null), 0f, 0, true, true);
				MapPointOfInterest mapPointOfInterest5 = combatStation;
				MapPointOfInterest mapPointOfInterest6 = combatStation;
				float pointsScale4 = 2f;
				faction = Faction.fanatics;
				mapPointOfInterest5.AddTriggeredSpawn(mapPointOfInterest6.CreateUnitPayload(pointsScale4, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null), 30f, 0, false, true);
				MapPointOfInterest mapPointOfInterest7 = combatStation;
				MapPointOfInterest mapPointOfInterest8 = combatStation;
				float pointsScale5 = 3f;
				faction = Faction.fanatics;
				mapPointOfInterest7.AddTriggeredSpawn(mapPointOfInterest8.CreateUnitPayload(pointsScale5, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 99, 0, 1, 5, null), 5f, 0, true, true);
				int totalEnemyCount = combatStation.totalEnemyCount;
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = combatStation,
					system = combatStation.system
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CombatStationDestroyed,
					description = "Destroy the Meridia Outpost."
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace3"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 1000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0003D2AC File Offset: 0x0003B4AC
		private void ConquestDarkspace3()
		{
			string storyMission = "ConquestDarkspace3";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
				SystemMapData current = SystemMapData.current;
				MapElement poi = combat;
				Faction fanatics = Faction.fanatics;
				int level = GamePlayer.current.level;
				current.SetupPOI(poi, null, fanatics, level);
				SpaceShipData spaceShipData = new SpaceShipData("Valdrax", false, Faction.fanatics)
				{
					unitRank = UnitRank.Veteran
				};
				spaceShipData.AddLoot(InventoryItemType.Get("Massive Vid-Screen"), 560);
				spaceShipData.AddLoot(InventoryItemType.Get("Dubious Vid-Discs"), 950);
				spaceShipData.AddLoot(InventoryItemType.Get("Simreality Headset"), 400);
				spaceShipData.AddLoot(InventoryItemType.Get("PoiBeacon"), 1);
				combat.AddGuards(new List<AbstractUnitData>
				{
					spaceShipData
				}, null);
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = combat,
					system = combat.system
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.fanatics,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace4"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 500
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0003D2FC File Offset: 0x0003B4FC
		private void ConquestDarkspace4()
		{
			string storyMission = "ConquestDarkspace4";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = faction,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SystemMapData systemMapData = SeededRandom.Global.Choose<SystemMapData>(GalaxyMapData.current.allConquestSystems);
				MapElement poi = new Source.Galaxy.POI.Combat();
				Faction fanatics = Faction.fanatics;
				int level = GamePlayer.current.level;
				MapPointOfInterest mapPointOfInterest = systemMapData.SetupPOI(poi, null, fanatics, level) as MapPointOfInterest;
				mapPointOfInterest.SetAsteroidFieldData(mapPointOfInterest.system.systemOreData, 0);
				mapPointOfInterest.InitializeAsteroids(false, false);
				mapPointOfInterest.AddCargoContainers(new Vector2(50f, 16f), 2, 1f);
				List<AbstractUnitData> list = mapPointOfInterest.CreateUnitPayload(3f, new GameplayType?(GameplayType.Mining), null, 0, 0, 1, 5, null);
				for (int i = 0; i < list.Count; i++)
				{
					list[i].autoActions = "AmbientMiner";
				}
				mapPointOfInterest.AddGuards(list, null);
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest3 = mapPointOfInterest;
				float pointsScale = 1f;
				fanatics = Faction.fanatics;
				mapPointOfInterest2.AddTriggeredSpawn(mapPointOfInterest3.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), fanatics, 0, 0, 1, 5, null), 2f, 0, true, true);
				MapPointOfInterest mapPointOfInterest4 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest5 = mapPointOfInterest;
				float pointsScale2 = 2f;
				fanatics = Faction.fanatics;
				mapPointOfInterest4.AddTriggeredSpawn(mapPointOfInterest5.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), fanatics, 0, 0, 1, 5, null), 30f, 0, false, true);
				MapPointOfInterest mapPointOfInterest6 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest7 = mapPointOfInterest;
				float pointsScale3 = 3f;
				fanatics = Faction.fanatics;
				mapPointOfInterest6.AddTriggeredSpawn(mapPointOfInterest7.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), fanatics, 99, 0, 1, 5, null), 5f, 0, true, true);
				int activeEnemyCount = mapPointOfInterest.activeEnemyCount;
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MinerChasedOff,
					description = "Elimante or chase away Meridia miners",
					requiredAmount = activeEnemyCount,
					gameplayType = GameplayType.Source.Galaxy.POI.Combat
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace5"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(500f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 300
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0003D34C File Offset: 0x0003B54C
		private void ConquestDarkspace5()
		{
			string storyMission = "ConquestDarkspace5";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = faction,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				SystemMapData systemMapData = SeededRandom.Global.Choose<SystemMapData>(GalaxyMapData.current.allConquestSystems);
				MapElement poi = new Source.Galaxy.POI.Salvage();
				Faction fanatics = Faction.fanatics;
				int level = GamePlayer.current.level;
				MapPointOfInterest mapPointOfInterest = systemMapData.SetupPOI(poi, null, fanatics, level) as MapPointOfInterest;
				mapPointOfInterest.AddCargoContainers(new Vector2(50f, 16f), 2, 1f);
				string[] list = new string[]
				{
					"Sparta",
					"Varyag",
					"Grom",
					"Maniple",
					"Kamin"
				};
				for (int i = 0; i < 4; i++)
				{
					SeededRandom.Global.RandomRange(6, 11);
					SalvageData salvageData = new SalvageData
					{
						position = mapPointOfInterest.GetWorldPosition() + new Vector2((float)SeededRandom.Global.RandomRange(-20, 40), (float)SeededRandom.Global.RandomRange(-15, 15)),
						shipTemplate = SeededRandom.Global.Choose<string>(list)
					};
					salvageData.AddScrapContent(mapPointOfInterest.level, 2f, 3);
					salvageData.AddStructuralContent(mapPointOfInterest.level, 3, 2f);
					mapPointOfInterest.AddPersistable(salvageData);
				}
				List<AbstractUnitData> list2 = mapPointOfInterest.CreateUnitPayload(3f, new GameplayType?(GameplayType.Salvage), null, 0, 0, 1, 5, null);
				for (int j = 0; j < list2.Count; j++)
				{
					list2[j].autoActions = "AmbientSalvager";
				}
				mapPointOfInterest.AddGuards(list2, null);
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest3 = mapPointOfInterest;
				float pointsScale = 1f;
				fanatics = Faction.fanatics;
				mapPointOfInterest2.AddTriggeredSpawn(mapPointOfInterest3.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), fanatics, 0, 0, 1, 5, null), 2f, 0, true, true);
				MapPointOfInterest mapPointOfInterest4 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest5 = mapPointOfInterest;
				float pointsScale2 = 2f;
				fanatics = Faction.fanatics;
				mapPointOfInterest4.AddTriggeredSpawn(mapPointOfInterest5.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), fanatics, 0, 0, 1, 5, null), 30f, 0, false, true);
				MapPointOfInterest mapPointOfInterest6 = mapPointOfInterest;
				MapPointOfInterest mapPointOfInterest7 = mapPointOfInterest;
				float pointsScale3 = 3f;
				fanatics = Faction.fanatics;
				mapPointOfInterest6.AddTriggeredSpawn(mapPointOfInterest7.CreateUnitPayload(pointsScale3, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), fanatics, 99, 0, 1, 5, null), 5f, 0, true, true);
				int activeEnemyCount = mapPointOfInterest.activeEnemyCount;
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.SalvagerChasedOff,
					description = "Elimante or chase away Meridia salvagers",
					requiredAmount = activeEnemyCount,
					gameplayType = GameplayType.Source.Galaxy.POI.Combat
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace6"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(500f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 300
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0003D39C File Offset: 0x0003B59C
		private void ConquestDarkspace6()
		{
			string storyMission = "ConquestDarkspace6";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Faction faction;
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = faction,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				int level = GamePlayer.current.level;
				IEnumerable<SystemMapData> allConquestSystems = GalaxyMapData.current.allConquestSystems;
				SystemMapData systemMapData = SeededRandom.Global.Choose<SystemMapData>(allConquestSystems);
				MapElement poi = new Source.Galaxy.POI.Combat();
				faction = Faction.fanatics;
				int forcedLevel = level;
				MapPointOfInterest mapPointOfInterest = systemMapData.SetupPOI(poi, null, faction, forcedLevel) as MapPointOfInterest;
				SpaceShipData item = new SpaceShipData("Carkalon", false, Faction.fanatics)
				{
					unitRank = UnitRank.Veteran
				};
				mapPointOfInterest.AddTriggeredSpawn(new List<AbstractUnitData>
				{
					item
				}, 5f, 0, false, true);
				MissionStep missionStep = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				missionStep.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.fanatics,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				SystemMapData systemMapData2 = SeededRandom.Global.Choose<SystemMapData>(allConquestSystems);
				MapElement poi2 = new Source.Galaxy.POI.Combat();
				faction = Faction.fanatics;
				forcedLevel = level;
				mapPointOfInterest = (systemMapData2.SetupPOI(poi2, null, faction, forcedLevel) as MapPointOfInterest);
				item = new SpaceShipData("Meriavex", false, Faction.fanatics)
				{
					unitRank = UnitRank.Elite
				};
				mapPointOfInterest.AddTriggeredSpawn(new List<AbstractUnitData>
				{
					item
				}, 7f, 0, false, true);
				MissionStep missionStep2 = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				missionStep2.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.fanatics,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				SystemMapData systemMapData3 = SeededRandom.Global.Choose<SystemMapData>(allConquestSystems);
				MapElement poi3 = new Source.Galaxy.POI.Combat();
				faction = Faction.fanatics;
				forcedLevel = level;
				mapPointOfInterest = (systemMapData3.SetupPOI(poi3, null, faction, forcedLevel) as MapPointOfInterest);
				MapPointOfInterest mapPointOfInterest2 = mapPointOfInterest;
				float pointsScale = 1.5f;
				faction = Faction.darkspacers;
				List<AbstractUnitData> list = mapPointOfInterest2.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Source.Galaxy.POI.Combat), faction, 0, 0, 1, 5, null);
				foreach (AbstractUnitData abstractUnitData in list)
				{
					abstractUnitData.playerFriendly = true;
				}
				mapPointOfInterest.AddTriggeredSpawn(list, 2f, 0, false, true);
				item = new SpaceShipData("Meriavex", false, Faction.fanatics)
				{
					unitRank = UnitRank.Champion
				};
				mapPointOfInterest.AddTriggeredSpawn(new List<AbstractUnitData>
				{
					item
				}, 10f, 0, false, true);
				MissionStep missionStep3 = new MissionStep
				{
					dynamicPointOfInterest = mapPointOfInterest,
					system = mapPointOfInterest.system
				};
				missionStep3.objectives.Add(new KillEnemies
				{
					enemyFaction = Faction.fanatics,
					requiredAmount = 1
				});
				mission.steps.Add(missionStep3);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace7"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(1000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0003D3EC File Offset: 0x0003B5EC
		private void ConquestDarkspace7()
		{
			string storyMission = "ConquestDarkspace7";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = faction,
					sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>()),
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				EmbassyStation embassy = GamePlayer.current.GetStoryteller<Conquest>().GetEmbassy(Faction.darkspacers);
				embassy.TryAddCharacter("DarkspaceSmuggler1");
				foreach (JumpGate jumpGate in embassy.system.sector.GetSectorJumpgates())
				{
					if (jumpGate.targetSystem.sector.conquestSector)
					{
						InventoryItemType key = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass((JumpGate)jumpGate.targetSystem.GetPoiWithId(jumpGate.targetPoiGuid), null);
						mission.missionItems.Add(key, 1);
					}
				}
				MissionStep missionStep = new MissionStep
				{
					poiHint = embassy
				};
				missionStep.objectives.Add(new TradeOffer
				{
					deliverTo = embassy,
					itemType = InventoryItemType.Get("CanisterOxide"),
					requiredAmount = 3000
				});
				missionStep.objectives.Add(new TradeOffer
				{
					deliverTo = embassy,
					itemType = InventoryItemType.Get("CanisterSilicon"),
					requiredAmount = 3000
				});
				missionStep.objectives.Add(new TradeOffer
				{
					deliverTo = embassy,
					itemType = InventoryItemType.Get("CanisterPlatinum"),
					requiredAmount = 2000
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CD1TalktoNPC,
					description = "Talk to Midas at the Darkspace Embassy"
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace8"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(200f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Ship
				{
					ship = "Terravex"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = InventoryItemType.Get("CanisterTitanium"),
					amount = 5000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = InventoryItemType.Get("CanisterCarbon"),
					amount = 8000
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0003D43C File Offset: 0x0003B63C
		private void ConquestDarkspace8()
		{
			string storyMission = "ConquestDarkspace8";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new ConquestFactionEliminated
				{
					faction = Faction.fanatics
				});
				mission.steps.Add(missionStep);
				EmbassyStation embassy = GamePlayer.current.GetStoryteller<Conquest>().GetEmbassy(Faction.darkspacers);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = embassy
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CD2TalktoNPC,
					description = "Talk to Midas at the Darkspace Embassy"
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace9"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(5000f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					amount = 100
				});
				return mission;
			}, null, null));
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0003D48C File Offset: 0x0003B68C
		private void ConquestDarkspace9()
		{
			string storyMission = "ConquestDarkspace9";
			Faction faction = Faction.darkspacers;
			StoryMission.Add(new StoryMission(storyMission ?? "", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = faction;
				mission.sourceName = Translation.Translate("@UmbralStory", Array.Empty<object>());
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				EmbassyStation embassy = GamePlayer.current.GetStoryteller<Conquest>().GetEmbassy(Faction.darkspacers);
				MissionStep missionStep = new MissionStep
				{
					poiHint = embassy,
					system = embassy.system
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CD3TalktoNPC,
					description = "Talk to Midas at the Darkspace Embassy"
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace10"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				return mission;
			}, null, null));
		}
	}
}
