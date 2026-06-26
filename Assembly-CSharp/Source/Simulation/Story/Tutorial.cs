using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Dialogues;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.Equipment.Turret.Salvage;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Crew;
using Source.Data.Persistable;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.TravelEvents;
using Source.Simulation.World;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.Story
{
	// Token: 0x0200008C RID: 140
	public class Tutorial : Storyteller
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0002F484 File Offset: 0x0002D684
		public override int maxPlayerLevel
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x0002F488 File Offset: 0x0002D688
		public override int maxReputation
		{
			get
			{
				return ReputationLevel.Respected.GetReputationThreshold();
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0002F490 File Offset: 0x0002D690
		public override int maxBonusSkillpoints
		{
			get
			{
				return 11;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0002F494 File Offset: 0x0002D694
		public static Character captain
		{
			get
			{
				return Characters.captain;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x0002F49B File Offset: 0x0002D69B
		public static Character shipAi
		{
			get
			{
				return Characters.shipAi;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x0002F4A2 File Offset: 0x0002D6A2
		public static Character greg
		{
			get
			{
				return Characters.greg;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x0002F4A9 File Offset: 0x0002D6A9
		public static Character virgil
		{
			get
			{
				return Characters.virgil;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0002F4B0 File Offset: 0x0002D6B0
		public static Character elena
		{
			get
			{
				return Characters.elena;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x0002F4B7 File Offset: 0x0002D6B7
		public static Character creed
		{
			get
			{
				return Characters.creed;
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0002F4BE File Offset: 0x0002D6BE
		public Tutorial(GamePlayer ply) : base(ply)
		{
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0002F4C8 File Offset: 0x0002D6C8
		public override void SetupNewGame()
		{
			TutorialWorld.SetupWorld(this.player);
			this.player.SetMapUsage(false);
			this.player.SetAutoPlayUsage(false);
			this.startDialogue = true;
			Mining mining = this.player.currentSystem.AddTutorialPOI(new Vector2?(new Vector2(4.22f, -3.43f)));
			mining.AddPersistable(new SalvageData
			{
				position = mining.GetWorldPosition() + new Vector2(-5f, -3f),
				shipTemplate = "Pickaxe LM",
				scrapContent = new Dictionary<InventoryItemType, int>
				{
					{
						"SalvageTitanium",
						22
					},
					{
						"SalvageOxide",
						20
					}
				},
				itemContent = new List<SalvageItemData>()
			});
			mining.AddPersistable(new SalvageData
			{
				position = mining.GetWorldPosition() + new Vector2(10f, 2f),
				shipTemplate = "Chisel Mk I SN",
				scrapContent = new Dictionary<InventoryItemType, int>
				{
					{
						"SalvageTitanium",
						10
					},
					{
						"SalvageOxide",
						10
					}
				},
				itemContent = new List<SalvageItemData>
				{
					new SalvageItemData(InventoryItemType.Get("Salvage Power I"), 1f, false)
				}
			});
			mining.AddPersistable(new MoveToAreaData
			{
				position = mining.GetWorldPosition() + new Vector2(-4.5f, 0f)
			});
			LootContainerData lootContainerData = new LootContainerData
			{
				position = mining.GetWorldPosition() + new Vector2(4f, -2f),
				name = "@LCNameOldCargoContainer"
			};
			lootContainerData.AddLoot(InventoryItemType.Get("Titanium Plate"), 1);
			lootContainerData.AddLoot(InventoryItemType.Get("Gatling Ammo"), 300);
			mining.AddPersistable(lootContainerData);
			LootContainerData lootContainerData2 = new LootContainerData
			{
				position = mining.GetWorldPosition() + new Vector2(12.3f, -3.2f),
				name = "@LCNameOldCargoContainer"
			};
			lootContainerData2.AddLoot(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f), 1);
			lootContainerData2.AddLoot(InventoryItemType.Get("OreCommon1"), 1);
			mining.AddPersistable(lootContainerData2);
			LootContainerData lootContainerData3 = new LootContainerData
			{
				position = mining.GetWorldPosition() + new Vector2(-3f, -6.1f),
				name = "@LCNameOldCargoContainer"
			};
			lootContainerData3.AddLoot(InventoryItemType.Get("OreCommon1"), 3);
			lootContainerData3.AddLoot(InventoryItemType.Get("Gatling Ammo"), 600);
			mining.AddPersistable(lootContainerData3);
			this.player.currentPointOfInterest = mining;
			this.player.currentSpaceShip.positionData.position = this.player.currentPointOfInterest.GetWorldPosition() - new Vector2(15f, 0f);
			this.player.currentSpaceShip.ApplyRandomBattleDamage(5, 20);
			this.GenerateTutorialMissions();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0002F7CF File Offset: 0x0002D9CF
		public override void Start()
		{
			if (this.startDialogue)
			{
				this.startDialogue = false;
				GameplayManager.Instance.StartCoroutine(this.StartWarpIn());
				GameplayManager.Instance.StartCoroutine(this.StartInitDialogue());
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0002F802 File Offset: 0x0002DA02
		private IEnumerator StartWarpIn()
		{
			yield return new WaitUntil(() => GameplayManager.Instance.spaceShip);
			Behaviour.Unit.SpaceShip spaceship = GameplayManager.Instance.spaceShip;
			spaceship.GiveImpulse(new Vector2(spaceship.rigidbody.mass * (float)SeededRandom.Global.RandomRange(3, 4), 0f), spaceship.rigidbody.inertia, 0f);
			yield return new WaitForSeconds(0.5f);
			spaceship.SetBrakeDestination();
			yield break;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0002F80A File Offset: 0x0002DA0A
		private IEnumerator StartInitDialogue()
		{
			yield return new WaitForSeconds(2f);
			Singleton<DialogueManager>.Instance.StartDialogue(this.Tutorial_1a_Start(), null);
			yield break;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0002F819 File Offset: 0x0002DA19
		public override void StoryUpdate(float delta)
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0002F81C File Offset: 0x0002DA1C
		public override void OnStoryMissionComplete(string storyId)
		{
			if (storyId == "tutorial_combat_1")
			{
				Singleton<DialogueManager>.Instance.StartDialogue(this.Tutorial_1a_Complete(), null);
				return;
			}
			if (storyId == "tutorial_1")
			{
				Singleton<DialogueManager>.Instance.StartDialogue(this.Tutorial_1a_Complete(), null);
				return;
			}
			if (storyId == "tutorial_salvage_1")
			{
				Singleton<DialogueManager>.Instance.StartDialogue(this.Tutorial_1a_Complete(), null);
				return;
			}
			if (!(storyId == "tutorial_1b"))
			{
				if (!(storyId == "tutorial_8"))
				{
					return;
				}
				this.player.SetAutoPlayUsage(true);
				SidePanel.instance.NotifyAutopilot();
				return;
			}
			else
			{
				Singleton<DialogueManager>.Instance.StartDialogue(this.Tutorial_1b_Complete(), null);
				SidePanel.instance.NotifyTab(SidePanel.SideTabType.Map, null);
				TractorModule componentInChildren = GameplayManager.Instance.spaceShip.GetComponentInChildren<TractorModule>();
				if (componentInChildren == null)
				{
					return;
				}
				componentInChildren.DisableAutoTargeting(false);
				return;
			}
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x0002F8F0 File Offset: 0x0002DAF0
		public List<DialogueLine> Tutorial_1a_Start()
		{
			Tutorial.IsCombat();
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Captain!"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Captain " + Tutorial.captain.name + ", respond!"));
			list.Add(DialogueLine.cDL(Tutorial.captain, "What in the... " + Tutorial.shipAi.name + ", run a full ship diagnostic."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Hull integrity is compromised. Several systems are offline. My AI core has sustained damage."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Repairs will require external support, preferably a station."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "You don't look too good either. Your learning implant seems to be... reset."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "And to top it off, we've lost the cargo... including all the Plasma Cells we had."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Well... What about navigation? Can we set a course?"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Negative. Navigation is down. I'm attempting a reboot."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "In the meantime, see if engines, targeting and tractoring are still functional.").WithTrigger(delegate
			{
				TractorModule componentInChildren = GameplayManager.Instance.spaceShip.GetComponentInChildren<TractorModule>();
				if (componentInChildren == null)
				{
					return;
				}
				componentInChildren.DisableAutoTargeting(true);
			}));
			return list;
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0002FA28 File Offset: 0x0002DC28
		public List<DialogueLine> Tutorial_1a_Complete()
		{
			string str = ", though Basic Scanning could help identify asteroids.";
			if (Tutorial.IsCombat())
			{
				str = ".";
			}
			else if (Tutorial.IsSalvage())
			{
				str = ".";
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Tutorial.shipAi, "Captain, engines are operational. Tractor module is operational. Auto-targeting needs calibration."),
				DialogueLine.cDL(Tutorial.shipAi, "I managed to recover one skill point."),
				DialogueLine.cDL(Tutorial.shipAi, "Use it as you wish" + str),
				DialogueLine.cDL(Tutorial.shipAi, "Navigation's still down. I'm working on it. Finish calibrating the tractor in the meantime.")
			};
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0002FAB8 File Offset: 0x0002DCB8
		public List<DialogueLine> Tutorial_1b_Complete()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Tractor module auto-targeting is back online, Captain."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Navigation is partially restored. I've located an outpost called Driftlight Station."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Good. Can you set a course now?"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Negative. Manual input required. Tap the map icon on your panel, or press 'M' on your control pad.").WithTrigger(delegate
			{
				GamePlayer.current.SetMapUsage(true);
			}));
			return list;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0002FB44 File Offset: 0x0002DD44
		public static List<DialogueLine> Greg_Default(Character greg)
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(greg, "It's a good thing you ended up here, Captain."),
				DialogueLine.cDL(greg, "Others aren't so lucky. Some end up into the lower systems, pirate territory."),
				DialogueLine.cDL(greg, "Out there, it's join up or get scrapped for parts."),
				DialogueLine.cDL(greg, "And that working ECHO of yours? That'd light up every scanner in the sector.")
			};
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0002FB9C File Offset: 0x0002DD9C
		public static List<DialogueLine> Tutorial_2_Welcome()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.greg, "Another one, huh... I'm " + Tutorial.greg.name + ". What do you want?"));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Captain " + Tutorial.captain.name + ". This... isn't the exit point out of the Great Gate, is it?"));
			list.Add(DialogueLine.cDL(Tutorial.greg, "The Great Gate? That's what they're calling it now?"));
			list.Add(DialogueLine.cDL(Tutorial.greg, "No, " + Tutorial.captain.name + "... this is Ravon on the galaxy's edge."));
			list.Add(DialogueLine.cDL(Tutorial.greg, "By the looks of your ship, I'm guessing you're here for repairs."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "It's not just the ship. My ECHO system took a beating too."));
			list.Add(DialogueLine.cDL(Tutorial.greg, "An ECHO? You still have a working one?!"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Affirmative... However, my AI Core was damaged during the jump."));
			list.Add(DialogueLine.cDL(Tutorial.greg, "That is quite rare... first things first: head over to your personal hangar to fix your ship a bit quicker. The auto-repairs will do the job, but it seems like you are in a hurry."));
			list.Add(DialogueLine.cDL(Tutorial.greg, "While you're waiting for repairs... install this new turret. Yours isn't up to par. Once you're done, come talk to me again.").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Navigation Online");
				if (GameplayManager.Instance.spaceShip.currentHullHP == GameplayManager.Instance.spaceShip.maxHullHP)
				{
					MissionObjective.Trigger(MissionTrigger.PersonalHangarRepair, null, null, false);
				}
			}));
			return list;
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0002FCE0 File Offset: 0x0002DEE0
		public static List<DialogueLine> Tutorial_3_Complete()
		{
			string line = Tutorial.IsCombat() ? "I need you to head over to a location and get rid of some pesky Autonomous Drones, left there by some pirate scum." : (Tutorial.IsSalvage() ? "I'll give you the location of a derelict ship, if you can bring back some salvaged materials we'll call it even. You can keep the rest you can scrape off of it." : "It's simple. I just need some ores to send off for refinement in the next cycle. Gather them, and we'll be good.");
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.captain, "Thanks. Any chance we could take a look at " + Tutorial.shipAi.name + "'s core now?"));
			list.Add(DialogueLine.cDL(Tutorial.greg, "I could take a look, but I won't be much help. You'll need a engineer for that, and there aren't any around here."));
			list.Add(DialogueLine.cDL(Tutorial.greg, "I do have an idea of where you might find one, but first, you'll need to return the favor."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Alright, I'm sure we can manage. Well... I can manage."));
			list.Add(DialogueLine.cDL(Tutorial.greg, line));
			list.Add(DialogueLine.cDL(Tutorial.greg, "I'll even give this plasma cell and a jumpgate pass to get you to the next system."));
			list.Add(DialogueLine.cDL(Tutorial.greg, "Oh, and also... you can use the armory (global) and material storage (local) to make room in your cargo hold.").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Personal Hangar");
			}));
			return list;
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0002FDDC File Offset: 0x0002DFDC
		public static List<DialogueLine> Tutorial_4_Complete()
		{
			string missionName = Tutorial.IsCombat() ? "Defeat The Drones" : (Tutorial.IsSalvage() ? "Salvage The Wreckage" : "Calibrate New Equipment");
			string line = "Captain, I see you managed to bring back the ores. Nice work.";
			if (Tutorial.IsCombat())
			{
				line = "Captain, great work. Wasn't much of a challenge it seems.";
			}
			else if (Tutorial.IsSalvage())
			{
				line = "Captain, great work. That new laser will cut them up nicely.";
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Tutorial.greg, line),
				DialogueLine.cDL(Tutorial.captain, "Haven't done this much manual work in years. With ECHO and all..."),
				DialogueLine.cDL(Tutorial.greg, "Oh, very exhausting indeed... Sitting in the captain's chair, pushing buttons all day..."),
				DialogueLine.cDL(Tutorial.greg, "Well, let's get you moving along with your journey."),
				DialogueLine.cDL(Tutorial.greg, "You should be familiar with the jumpgate system, it's the same as what we use back in our galaxy."),
				DialogueLine.cDL(Tutorial.greg, "Luckily, for us... a lot of colony ships ended up here carrying the gate constructors."),
				DialogueLine.cDL(Tutorial.greg, "So just jettison the pass at the gate to Orbitan, and it should unlock for you."),
				DialogueLine.cDL(Tutorial.captain, "Got it. Jettison the pass at the gate and head to Orbitan."),
				DialogueLine.cDL(Tutorial.shipAi, "Captain, I will remind you, just check the mission overview."),
				DialogueLine.cDL(Tutorial.greg, "Right, Once you're in the next system, head over to Point Station. They might be able to help you out. Good luck.").WithTrigger(delegate
				{
					GamePlayer.current.CompleteMission(missionName);
				})
			};
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0002FF1C File Offset: 0x0002E11C
		public static List<DialogueLine> Tutorial_5_Welcome()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.virgil, "Howdy?"));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Hey there, I'm looking for an ECHO engineer."));
			list.Add(DialogueLine.cDL(Tutorial.virgil, "I see, ain't nobody of that caliber 'round here."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Of course, ECHOs do seem pretty rare here..."));
			list.Add(DialogueLine.cDL(Tutorial.virgil, "Yeah... can't help you with that. I gotta go now. Bye.").WithTrigger(delegate
			{
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.characters.Remove("Virgil");
				SpaceStationInterior instance = SpaceStationInterior.instance;
				instance.SetupCharacters(spaceStation.characters);
				instance.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Uh, okay... that was... abrupt."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Captain, I have located another station we should visit, but it is in the next system."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "We should run some missions from the Mission Board to purchase a pass to the next system."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "We can use the material (local) and armory (global) storage to help manage our items. You can access it through the inventory overview."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "In the sidepanel, I got it. Let's get to it.").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Using The Gatesystem");
			}));
			return list;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00030048 File Offset: 0x0002E248
		public static List<DialogueLine> Creed_Default(Character character)
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(character, "I was part of The Second Wave, serving as a senior mechanic aboard the colony ship Orbitan of the 19th Expeditionary Fleet."),
				DialogueLine.cDL(character, "We were... I guess... just one of the unlucky ones, ending up here."),
				DialogueLine.cDL(character, "Time passed. More ships from other fleets started showing up.."),
				DialogueLine.cDL(character, "Contracted captains, freelancers, outlaws... haven't seen a lot of major factions here, funnily enough."),
				DialogueLine.cDL(character, "Well, not all of them were friendly, but most were. If we wanted to survive, we had to work together."),
				DialogueLine.cDL(character, "And that's why I am helping you, so we can help eachother.")
			};
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000300C0 File Offset: 0x0002E2C0
		public static List<DialogueLine> Tutorial_6_Welcome()
		{
			string line = Tutorial.IsSalvage() ? "Seems like you know your way around scrap, so this shouldn't be a problem for you." : "Here's an old Salvage Laser you can use, no need to return it.";
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.creed, "Hey there..."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Hello. It's a long shot, but do you happen to have an ECHO engineer around here?"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Really? What for?"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "My AI Core was damaged during the journey through the Great Gate."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "What?! They don't get damaged... they get destroyed!"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Elena, get over here.").WithTrigger(delegate
			{
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.characters.Add("Elena");
				SpaceStationInterior instance = SpaceStationInterior.instance;
				instance.SetupCharacters(spaceStation.characters);
				instance.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Creed? What's going on. Oh my..."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Let me see... hmm... yes... what... how...?"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Uh, could you... maybe stop?"));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Sorry, this is just... very exciting."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "I guess... are you able to repair the core?"));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Creed? Give them the nav coordinates."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Yeah... we won't get this opportunity again."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "We just have to put our faith in you, stranger... Here you go."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "There's an old derelict corvette out there with a Damaged AI Core. Salvage it and bring it back here. We should be able to use that one to craft a new one."));
			list.Add(DialogueLine.cDL(Tutorial.creed, line));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Got it, thank you."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Wait! Captain, if you don't mind, I'd love to join your crew..."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Sure, I guess... We'll take all the help we can get."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "This is great! Finally, we might actually get out of here.").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Get To The Balam System");
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.characters.Remove("Elena");
				SpaceStationInterior.instance.SetupCharacters(spaceStation.characters);
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			return list;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000302D0 File Offset: 0x0002E4D0
		public static List<DialogueLine> Tutorial_7_Complete()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.creed, "Good, you're back. How did it go?"));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Creed, we managed to salvage the damaged AI Core!"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "That's excellent news! We still have the knowledge to craft AI Cores, just no working ECHOs to place them in... until now"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "I've added the Industrial skill tree to your learning implant, it looks like you didn't have one before."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "I won't say no to that.").WithTrigger(delegate
			{
				GamePlayer.current.commander.UnlockSkilltree(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Industrial));
			}));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Industrial skill tree has been activated, Captain."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "It's not necessary right now, but there are some useful upgrades in it if you have the skill points to spare."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Head over to the forge, craft the AI Core, and come back to me as soon as you can."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Got it. I'll head over there now.").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Salvage The Derelict");
			}));
			return list;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000303E8 File Offset: 0x0002E5E8
		public static List<DialogueLine> Tutorial_8_Complete()
		{
			string line = Tutorial.IsCombat() ? "Shouldn't be a problem, I see you already have a combat vessel." : "You could slap a combat turret on your current vessel, or see if the shipyard has a gunship lying around.";
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.creed, "Alright, now... " + Tutorial.elena.name + ", it's your turn."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Captain, could you let " + Tutorial.elena.name + " access your ECHO to replace the core."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Alright, sure... go ahead."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Go ahead, " + Tutorial.elena.name + ". I've given temporary access."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Let's see..."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Yes, this is a perfect fit..."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Alright, done."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Captain?"));
			list.Add(DialogueLine.cDL(Tutorial.captain, Tutorial.shipAi.name + "?"));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Id fytz lyk a glob."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Uh oh..."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Just kidding. Systems are stable. I've reallocated 4% of my processing power to humor.").WithTrigger(delegate
			{
				GamePlayer.current.commander.UnlockSkilltree(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Engineering));
			}));
			list.Add(DialogueLine.cDL(Tutorial.creed, "You gave me quite a scare there... Well, good job, " + Tutorial.elena.name + "."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Yes!!"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "So, here's the deal, Captain... "));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Your ECHO is the only one who can communicate with the constructor unit in the Hermetis system."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "It's the gate that connects this sector to the rest of the galaxy."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "So, once we repair the gate, we can finally leave this place!"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Yes, " + Tutorial.elena.name + ", let us hope. There's just one problem... getting to Hermetis."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "It's most likely being guarded by at least one pirate. They are trying to block us from entering that system. They are not in a rush to let us leave this sector..."));
			list.Add(DialogueLine.cDL(Tutorial.creed, line));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Once you take care of them, head back over here."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Sounds good, thank you."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Captain, I can now take over certain tasks. You can toggle me on or off in your sidepanel.").WithTrigger(delegate
			{
				GamePlayer.current.currentSpaceShip.RemoveCargo(InventoryItemType.Get("AI Core"), 1, false);
				GamePlayer.current.globalInventory.Remove(InventoryItemType.Get("AI Core"), 1);
				((SpaceStation)GamePlayer.current.currentPointOfInterest).materialStorage.Remove(InventoryItemType.Get("AI Core"), 1);
				GamePlayer.current.CompleteMission("Craft The Core");
			}));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "It would also be helpful if you learn " + Translation.Translate("@PromptEngineeringBasicMissionRunnerName", Array.Empty<object>()) + " in the Autopilot skilltree. It will let me negotiate better rewards whenever I carry out missions."));
			list.Add(DialogueLine.cDL(Tutorial.shipAi, "Go grab something to drink, read a book, whatever you want really. I can handle the more basic tasks, just not at maximum efficiency... yet."));
			return list;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x00030700 File Offset: 0x0002E900
		public static List<DialogueLine> Tutorial_9_Complete()
		{
			List<DialogueLine> list = new List<DialogueLine>
			{
				DialogueLine.cDL(Tutorial.creed, "Welcome back, Captain."),
				DialogueLine.cDL(Tutorial.captain, Tutorial.creed.name + ", the problem's taken care of."),
				DialogueLine.cDL(Tutorial.creed, "Knew you could handle it."),
				DialogueLine.cDL(Tutorial.creed, "Not much stands in our way now, just the gate repairs."),
				DialogueLine.cDL(Tutorial.elena, "Well, we still need the materials, " + Tutorial.creed.name + "."),
				DialogueLine.cDL(Tutorial.creed, "Right, good point...")
			};
			if (Tutorial.IsCombat())
			{
				list.Add(DialogueLine.cDL(Tutorial.creed, "You could mine or salvage the materials, but seeing as you are quite capable in combat..."));
				list.Add(DialogueLine.cDL(Tutorial.creed, "The pirates stole the materials we needed for the repairs"));
				list.Add(DialogueLine.cDL(Tutorial.creed, "They are quite formidable, so having a ship with a shield generator would help quite a bit."));
				list.Add(DialogueLine.cDL(Tutorial.creed, "I'll give you the location of these pirate groups. Defeat them and we'll collect the materials they've stolen from us.").WithTrigger(delegate
				{
					GamePlayer.current.CompleteMission("Defeat The Pirate");
				}));
				list.Add(DialogueLine.cDL(Tutorial.creed, "We attuned your ship with the gate leading to Octantis, that's where the pirates are holding up. It should be unlocked now."));
			}
			else
			{
				list.Add(DialogueLine.cDL(Tutorial.creed, "Captain, it'd be a huge help if you could track down and craft what we need."));
				list.Add(DialogueLine.cDL(Tutorial.creed, "Some of the raw stuff's more common in the lower systems.Take this gate pass from Octantis to Sparax."));
				list.Add(DialogueLine.cDL(Tutorial.captain, "Understood. Let's put ECHO to work."));
				list.Add(DialogueLine.cDL(Tutorial.shipAi, "Just give the order, Captain.").WithTrigger(delegate
				{
					GamePlayer.current.CompleteMission("Defeat The Pirate");
				}));
			}
			return list;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x000308C8 File Offset: 0x0002EAC8
		public static List<DialogueLine> Tutorial_10_Combat_Complete()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.creed, "Captain!"));
			list.Add(DialogueLine.cDL(Tutorial.captain, Tutorial.creed.name + ", that were a lot of pirates."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Well, you dealt quite a blow to their presence, so you have my utmost thanks."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "It was very close, " + Tutorial.creed.name));
			list.Add(DialogueLine.cDL(Tutorial.creed, "We already send someone to bring most of the materials to the gate."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "I will transfer the Signal Array to your cargo, only your ECHO can link this to the gate."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Should be the last thing to do, and we can all get out of this sector.").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Recover The Materials");
			}));
			return list;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000309B0 File Offset: 0x0002EBB0
		public static List<DialogueLine> Tutorial_10_Complete()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Tutorial.creed, "Give me the good news, Captain."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "We've repaired the gate!"));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Or... " + Tutorial.elena.name + " can give me the news."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "You don't know how much you've helped us, thank you so much."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "I'd like to stay with the Captain permanently, if that's okay with you, " + Tutorial.creed.name + "."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Of course. You make a good team. Goodbye, " + Tutorial.elena.name + "."));
			list.Add(DialogueLine.cDL(Tutorial.elena, "Maybe we'll meet again. Goodbye, " + Tutorial.creed.name + "."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "Captain, I've heard there are many possibilities waiting for you in this new galaxy."));
			list.Add(DialogueLine.cDL(Tutorial.creed, "You might even make it all the way into Darkspace with your ECHO... if it's real. Anyway, good luck."));
			list.Add(DialogueLine.cDL(Tutorial.captain, "Thanks, and good luck to you too, " + Tutorial.creed.name + ".").WithTrigger(delegate
			{
				GamePlayer.current.CompleteMission("Repair The Gate");
			}));
			return list;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00030B1C File Offset: 0x0002ED1C
		public void GenerateTutorialMissions()
		{
			string id = "tutorial_1";
			if (Tutorial.IsCombat())
			{
				id = "tutorial_combat_1";
			}
			else if (Tutorial.IsSalvage())
			{
				id = "tutorial_salvage_1";
			}
			GamePlayer.current.AddMissionWithLog(StoryMission.Get(GamePlayer.current, id));
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00030B60 File Offset: 0x0002ED60
		public override void AddItemsToShop(SpaceStation parent, ShopInventory shop)
		{
			foreach (JumpGate jumpGate in parent.system.GetJumpGateList(false))
			{
				if (!jumpGate.canUseJumpGate && jumpGate.targetSystemGuid != null)
				{
					InventoryItemType item = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate, null);
					shop.Add(item, 1, false, false);
				}
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00030BDC File Offset: 0x0002EDDC
		public override bool ItemIsPlayerAvailable(EquipmentBuilder item)
		{
			AbstractEquipment component = item.prefab.GetComponent<AbstractEquipment>();
			if (component.size != ModuleSize.Small)
			{
				return false;
			}
			AbstractTurret abstractTurret = component as AbstractTurret;
			if (abstractTurret != null)
			{
				return abstractTurret is MiningTurret || abstractTurret is MiningCoreTurret || abstractTurret is AutoCannonTurret || abstractTurret is GatlingTurret || abstractTurret is SalvageLaserTurret;
			}
			return !(component is DroneBayModule);
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00030C57 File Offset: 0x0002EE57
		public override bool ShipIsPlayerAvailable(Behaviour.Unit.SpaceShip ss)
		{
			return ss.shipRoleType.GetTypeSize() < 3 && !ss.HasModuleSlot(EquipmentSlot.DroneBay);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00030C74 File Offset: 0x0002EE74
		public override TravelDynamicEvent TriggerDynamicEvent()
		{
			if (Register.HasFlag("FirstTutorialEvent", false))
			{
				List<Type> list = new List<Type>
				{
					typeof(DistressSalvage),
					typeof(DistressCombat),
					typeof(BigFatAsteroids),
					typeof(Source.Simulation.TravelEvents.Salvage)
				};
				return (TravelDynamicEvent)Activator.CreateInstance(SeededRandom.Global.Choose<Type>(list));
			}
			Register.SetFlag("FirstTutorialEvent", true);
			TravelDynamicEvent result;
			switch (GamePlayer.current.commander.personalHistory)
			{
			case PersonalHistory.Miner:
				result = new BigFatAsteroids();
				break;
			case PersonalHistory.NavyCaptain:
				result = new DistressCombat();
				break;
			case PersonalHistory.Salvaging:
				result = new Source.Simulation.TravelEvents.Salvage();
				break;
			default:
				result = new DistressSalvage();
				break;
			}
			return result;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00030D38 File Offset: 0x0002EF38
		private static bool IsCombat()
		{
			return GamePlayer.current.starterSpecialization == 8;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00030D47 File Offset: 0x0002EF47
		private static bool IsSalvage()
		{
			return GamePlayer.current.starterSpecialization == 6;
		}

		// Token: 0x040002A9 RID: 681
		private bool startDialogue;
	}
}
