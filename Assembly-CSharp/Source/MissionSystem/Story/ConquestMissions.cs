using System;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;

namespace Source.MissionSystem.Story
{
	// Token: 0x020000AC RID: 172
	public class ConquestMissions
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x0003A648 File Offset: 0x00038848
		public ConquestMissions()
		{
			this.GotoConquestMission();
			this.ConquestThreeFactions();
			this.ConquestStellar1();
			this.ConquestStellar2();
			this.ConquestKolyatov1();
			this.ConquestKolyatov2();
			this.ConquestLuminate1();
			this.ConquestLuminate2();
			this.CreateGoToConquestMission("MiningGoToConquest", Faction.miningGuild);
			this.CreateGoToConquestMission("SalvageGoToConquest", Faction.salvageGuild);
			this.CreateGoToConquestMission("MaraudersGoToConquest", Faction.marauders);
			this.CreateGoToConquestMission("StrandedGoToConquest", Faction.stranded);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0003A6CC File Offset: 0x000388CC
		private void GotoConquestMission()
		{
			StoryMission.Add(new StoryMission("StoryMissionSkipToConquest", delegate(GamePlayer ply)
			{
				if (ply.GetStoryteller<Conquest>() == null)
				{
					ply.AddStoryteller(new Conquest(ply), true);
				}
				JumpGate jumpGate = null;
				JumpGate jumpGate2 = null;
				foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
				{
					if (sectorMapData.quadrant == 2)
					{
						foreach (SystemMapData systemMapData in sectorMapData.allSystems)
						{
							foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
							{
								JumpGate jumpGate3 = mapPointOfInterest as JumpGate;
								if (jumpGate3 != null && jumpGate3.sectorJumpgate)
								{
									JumpGate jumpGate4 = jumpGate3.GetTargetPOI() as JumpGate;
									if (jumpGate4 != null && jumpGate4.system.sector.quadrant == 1)
									{
										jumpGate = jumpGate3;
										jumpGate2 = jumpGate4;
										break;
									}
								}
							}
						}
					}
				}
				InventoryItemType key = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate2, null);
				Mission mission = new Mission();
				mission.name = Translation.Translate("@StoryMissionSkipToConquest", Array.Empty<object>());
				mission.description = Translation.Translate("@StoryMissionSkipToConquestDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@StoryMissionSkipToConquestComplete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.sourceFaction = Faction.policeGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				mission.autoComplete = true;
				mission.missionItems[key] = 1;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = jumpGate2.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep();
				missionStep2.objectives.Add(new TravelToPOI
				{
					targetPOI = jumpGate.guid
				});
				mission.steps.Add(missionStep2);
				EmbassyStation embassy = GamePlayer.current.GetStoryteller<Conquest>().GetEmbassy(Faction.policeGuild);
				MissionStep missionStep3 = new MissionStep
				{
					system = jumpGate.system,
					poiHint = embassy
				};
				missionStep3.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ConquestCanisec1,
					description = "Talk to the Canisec Commander",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep3);
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.policeGuild,
					amount = 300
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestThreeFactions"
				});
				return mission;
			}, (GamePlayer player) => player.level >= 28, "Continue your galactic journey"));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0003A72C File Offset: 0x0003892C
		private void ConquestThreeFactions()
		{
			string storyMission = "ConquestThreeFactions";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Mission mission = new Mission
				{
					name = Translation.Translate("@" + storyMission, Array.Empty<object>()),
					description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>()),
					completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>()),
					sourceFaction = Faction.policeGuild,
					canAbandon = false,
					trackedOnHud = true,
					autoComplete = true,
					difficulty = MissionDifficulty.Story
				};
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				EmbassyStation embassy = storyteller.GetEmbassy(Faction.blue);
				EmbassyStation embassy2 = storyteller.GetEmbassy(Faction.red);
				EmbassyStation embassy3 = storyteller.GetEmbassy(Faction.gold);
				MissionStep missionStep = new MissionStep
				{
					requireAllObjectives = false,
					description = "Align yourself with a faction"
				};
				missionStep.AddPoiHints(new MapPointOfInterest[]
				{
					embassy2,
					embassy,
					embassy3
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ConquestStellarEmbassy,
					description = "Find the Stellar Embassy.",
					requiredAmount = 1
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ConquestKolyatovEmbassy,
					description = "Find the Kolyatov Embassy.",
					requiredAmount = 1
				});
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.ConquestLuminateEmbassy,
					description = "Find the Luminate Embassy.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				return mission;
			}, null, null));
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0003A768 File Offset: 0x00038968
		public static void FactionAlignmentChoice(Faction faction)
		{
			Mission mission;
			if (faction == Faction.blue)
			{
				mission = StoryMission.Get(GamePlayer.current, "ConquestStellar1");
				MissionObjective.Trigger(MissionTrigger.ConquestStellarEmbassy, 1, null, false);
			}
			else if (faction == Faction.red)
			{
				mission = StoryMission.Get(GamePlayer.current, "ConquestKolyatov1");
				MissionObjective.Trigger(MissionTrigger.ConquestKolyatovEmbassy, 1, null, false);
			}
			else
			{
				mission = StoryMission.Get(GamePlayer.current, "ConquestLuminate1");
				MissionObjective.Trigger(MissionTrigger.ConquestLuminateEmbassy, 1, null, false);
			}
			GamePlayer.current.AddMissionWithLog(mission);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x0003A7F4 File Offset: 0x000389F4
		private void ConquestStellar1()
		{
			string storyMission = "ConquestStellar1";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Faction blue = Faction.blue;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = blue;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				ConquestStation station = storyteller.GetHeadquarters(blue).station;
				EmbassyJumpgate embassyJumpgate = storyteller.GetEmbassyJumpgate(blue);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = embassyJumpgate.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = station.system,
					poiHint = station
				};
				missionStep2.objectives.Add(new TravelToPOI
				{
					targetPOI = station.guid
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CSHQIntro,
					description = "Talk to Victor Hale.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				MissionStep missionStep3 = new MissionStep
				{
					hidden = true
				};
				missionStep3.objectives.Add(new ConquestFleetStrength
				{
					trigger = MissionTrigger.EarnCombatStrengthForFaction,
					faction = blue,
					description = "Increase Fleet Strength",
					requiredAmount = 10
				});
				mission.steps.Add(missionStep3);
				MissionStep missionStep4 = new MissionStep
				{
					hidden = true
				};
				missionStep4.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CS2,
					description = "Talk to Victor Hale",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep4);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestStellar2"
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral1"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "ConquestCurrency",
					amount = 60
				});
				mission.rewards.Add(new Credits
				{
					amount = 2000000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0003A830 File Offset: 0x00038A30
		private void ConquestStellar2()
		{
			string storyMission = "ConquestStellar2";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Faction blue = Faction.blue;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = blue;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				ConquestStation station = storyteller.GetHeadquarters(blue).station;
				storyteller.GetEmbassyJumpgate(blue);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new SystemsConquered
				{
					faction = blue,
					targetPercentage = 0.3f
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					hidden = true
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CS3,
					description = "Go to the Stellar HQ and talk to Victor Hale.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace1"
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "ConquestCurrency",
					amount = 400
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new Credits
				{
					amount = 10000000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0003A86C File Offset: 0x00038A6C
		private void ConquestKolyatov1()
		{
			string storyMission = "ConquestKolyatov1";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Faction red = Faction.red;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = red;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				ConquestStation station = storyteller.GetHeadquarters(red).station;
				EmbassyJumpgate embassyJumpgate = storyteller.GetEmbassyJumpgate(red);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = embassyJumpgate.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = station.system,
					poiHint = station
				};
				missionStep2.objectives.Add(new TravelToPOI
				{
					targetPOI = station.guid
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CKHQIntro,
					description = "Talk to Mikhail Kolyatov.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				MissionStep missionStep3 = new MissionStep
				{
					hidden = true
				};
				missionStep3.objectives.Add(new ConquestFleetStrength
				{
					trigger = MissionTrigger.EarnCombatStrengthForFaction,
					faction = red,
					description = "Increase Fleet Strength",
					requiredAmount = 10
				});
				mission.steps.Add(missionStep3);
				MissionStep missionStep4 = new MissionStep
				{
					hidden = true
				};
				missionStep4.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CK2,
					description = "Talk to Mikhail Kolyatov",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep4);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestKolyatov2"
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral1"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "ConquestCurrency",
					amount = 60
				});
				mission.rewards.Add(new Credits
				{
					amount = 2000000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0003A8A8 File Offset: 0x00038AA8
		private void ConquestKolyatov2()
		{
			string storyMission = "ConquestKolyatov2";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Faction red = Faction.red;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = red;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				ConquestStation station = storyteller.GetHeadquarters(red).station;
				storyteller.GetEmbassyJumpgate(red);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new SystemsConquered
				{
					faction = red,
					targetPercentage = 0.3f
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					hidden = true
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CK3,
					description = "Go to the Kolyatov HQ and talk to Mikhail Kolyatov.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "ConquestCurrency",
					amount = 400
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new Credits
				{
					amount = 10000000
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace1"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0003A8E4 File Offset: 0x00038AE4
		private void ConquestLuminate1()
		{
			string storyMission = "ConquestLuminate1";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Faction gold = Faction.gold;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = gold;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				ConquestStation station = storyteller.GetHeadquarters(gold).station;
				EmbassyJumpgate embassyJumpgate = storyteller.GetEmbassyJumpgate(gold);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TravelToPOI
				{
					targetPOI = embassyJumpgate.guid
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					system = station.system,
					poiHint = station
				};
				missionStep2.objectives.Add(new TravelToPOI
				{
					targetPOI = station.guid
				});
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CLHQIntro,
					description = "Talk to Triane Solis.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				MissionStep missionStep3 = new MissionStep
				{
					hidden = true
				};
				missionStep3.objectives.Add(new ConquestFleetStrength
				{
					trigger = MissionTrigger.EarnCombatStrengthForFaction,
					faction = gold,
					description = "Increase Fleet Strength",
					requiredAmount = 10
				});
				mission.steps.Add(missionStep3);
				MissionStep missionStep4 = new MissionStep
				{
					hidden = true
				};
				missionStep4.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CL2,
					description = "Talk to Triane Solis",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep4);
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestLuminate2"
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestUmbral1"
				});
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "ConquestCurrency",
					amount = 60
				});
				mission.rewards.Add(new Credits
				{
					amount = 2000000
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x0003A920 File Offset: 0x00038B20
		private void ConquestLuminate2()
		{
			string storyMission = "ConquestLuminate2";
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Faction gold = Faction.gold;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = gold;
				mission.canAbandon = false;
				mission.trackedOnHud = true;
				mission.autoComplete = true;
				mission.difficulty = MissionDifficulty.Story;
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				ConquestStation station = storyteller.GetHeadquarters(gold).station;
				storyteller.GetEmbassyJumpgate(gold);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new SystemsConquered
				{
					faction = gold,
					targetPercentage = 0.3f
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					hidden = true
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.CL3,
					description = "Go to the Luminate HQ and talk to Triane Solis.",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "ConquestCurrency",
					amount = 400
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 1
				});
				mission.rewards.Add(new Credits
				{
					amount = 10000000
				});
				mission.rewards.Add(new StoryMission
				{
					missionId = "ConquestDarkspace1"
				});
				return mission;
			}, null, null));
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0003A95C File Offset: 0x00038B5C
		private void CreateGoToConquestMission(string storyMission, Faction f)
		{
			StoryMission.Add(new StoryMission(storyMission, delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyMission, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyMission + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyMission + "Complete", Array.Empty<object>());
				mission.sourceFaction = f;
				mission.trackedOnHud = false;
				mission.difficulty = MissionDifficulty.Story;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.None,
					description = "@" + storyMission + "Hint",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = f,
					amount = 800
				});
				return mission;
			}, (GamePlayer ply) => this.GoToConquestAvailable(f, ply), "@" + storyMission + "Hint"));
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x0003A9C4 File Offset: 0x00038BC4
		private bool GoToConquestAvailable(Faction f, GamePlayer ply)
		{
			Conquest storyteller = ply.GetStoryteller<Conquest>();
			EmbassyStation embassyStation = (storyteller != null) ? storyteller.GetEmbassy(f) : null;
			return storyteller != null && embassyStation != null && storyteller.GetHeadquarters(f) == null && storyteller.GetPotentialRejoinHQ(f) != null;
		}
	}
}
