using System;
using System.Collections.Generic;
using System.Reflection;
using Behaviour.UI;
using Source.Dialogues.Content;
using Source.Galaxy;
using Source.Galaxy.POI.Station.Patrons;
using Source.MissionSystem;
using Source.MissionSystem.Story;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using UnityEngine;

namespace Source.Dialogues
{
	// Token: 0x020000FF RID: 255
	public static class Characters
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00048378 File Offset: 0x00046578
		public static Character _captain
		{
			get
			{
				return Characters.captain;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0004837F File Offset: 0x0004657F
		public static Character _shipAi
		{
			get
			{
				return Characters.shipAi;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x00048386 File Offset: 0x00046586
		public static Character virgil
		{
			get
			{
				Character result;
				if ((result = Characters._virgil) == null)
				{
					result = (Characters._virgil = Characters.Virgil());
				}
				return result;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0004839C File Offset: 0x0004659C
		public static Character elena
		{
			get
			{
				Character result;
				if ((result = Characters._elena) == null)
				{
					result = (Characters._elena = Characters.Elena());
				}
				return result;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x000483B2 File Offset: 0x000465B2
		public static Character creed
		{
			get
			{
				Character result;
				if ((result = Characters._creed) == null)
				{
					result = (Characters._creed = Characters.Creed());
				}
				return result;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x000483C8 File Offset: 0x000465C8
		public static Character greg
		{
			get
			{
				Character result;
				if ((result = Characters._greg) == null)
				{
					result = (Characters._greg = Characters.Greg());
				}
				return result;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x000483DE File Offset: 0x000465DE
		public static Character umbralContact
		{
			get
			{
				Character result;
				if ((result = Characters._umbralContact) == null)
				{
					result = (Characters._umbralContact = Characters.Umbral());
				}
				return result;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x000483F4 File Offset: 0x000465F4
		public static Character realUmbralContact
		{
			get
			{
				Character result;
				if ((result = Characters._realUmbralContact) == null)
				{
					result = (Characters._realUmbralContact = Characters.RealUmbral());
				}
				return result;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600097D RID: 2429 RVA: 0x0004840A File Offset: 0x0004660A
		public static Character luminateCommander
		{
			get
			{
				Character result;
				if ((result = Characters._luminateCommander) == null)
				{
					result = (Characters._luminateCommander = Characters.LuminateCommander());
				}
				return result;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x00048420 File Offset: 0x00046620
		public static Character prisonGuard
		{
			get
			{
				Character result;
				if ((result = Characters._prisonGuard) == null)
				{
					result = (Characters._prisonGuard = Characters.PrisonGuard());
				}
				return result;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x00048436 File Offset: 0x00046636
		public static Character raythor
		{
			get
			{
				Character result;
				if ((result = Characters._raythor) == null)
				{
					result = (Characters._raythor = Characters.ExpeditionCaptainRaythor());
				}
				return result;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0004844C File Offset: 0x0004664C
		public static Character kolyatovCaptain
		{
			get
			{
				Character result;
				if ((result = Characters._kolyatovCaptain) == null)
				{
					result = (Characters._kolyatovCaptain = Characters.ExpeditionKolyatovCaptain());
				}
				return result;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000981 RID: 2433 RVA: 0x00048462 File Offset: 0x00046662
		public static Character steelVultureComputerSalesman
		{
			get
			{
				Character result;
				if ((result = Characters._steelVultureComputerSalesman) == null)
				{
					result = (Characters._steelVultureComputerSalesman = Characters.SteelVultureComputerSalesman());
				}
				return result;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x00048478 File Offset: 0x00046678
		public static Character stellarRepresentative
		{
			get
			{
				Character result;
				if ((result = Characters._stellarRepresentative) == null)
				{
					result = (Characters._stellarRepresentative = Characters.StellarRepresentative());
				}
				return result;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000983 RID: 2435 RVA: 0x0004848E File Offset: 0x0004668E
		public static Character steelVulturePocket
		{
			get
			{
				Character result;
				if ((result = Characters._steelVulturePocket) == null)
				{
					result = (Characters._steelVulturePocket = Characters.SteelVulturePocket());
				}
				return result;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x000484A4 File Offset: 0x000466A4
		public static Character smugglerContact
		{
			get
			{
				Character result;
				if ((result = Characters._smugglerContact) == null)
				{
					result = (Characters._smugglerContact = Characters.SmugglerContact());
				}
				return result;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000985 RID: 2437 RVA: 0x000484BA File Offset: 0x000466BA
		public static Character smugglerSector4
		{
			get
			{
				Character result;
				if ((result = Characters._smugglerSector4) == null)
				{
					result = (Characters._smugglerSector4 = Characters.SmugglerSector4());
				}
				return result;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x000484D0 File Offset: 0x000466D0
		public static Character stellarStagingPoint
		{
			get
			{
				Character result;
				if ((result = Characters._stellarStagingPoint) == null)
				{
					result = (Characters._stellarStagingPoint = Characters.StellarStagingPoint());
				}
				return result;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000987 RID: 2439 RVA: 0x000484E6 File Offset: 0x000466E6
		public static Character darkspaceSmuggler1
		{
			get
			{
				Character result;
				if ((result = Characters._darkspaceSmuggler1) == null)
				{
					result = (Characters._darkspaceSmuggler1 = Characters.DarkspaceSmuggler1());
				}
				return result;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x000484FC File Offset: 0x000466FC
		public static Character canisecConquestCommander
		{
			get
			{
				Character result;
				if ((result = Characters._canisecConquestCommander) == null)
				{
					result = (Characters._canisecConquestCommander = Characters.CanisecConquestCommander());
				}
				return result;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x00048512 File Offset: 0x00046712
		public static Character conquestCommanderStellar
		{
			get
			{
				Character result;
				if ((result = Characters._conquestCommanderStellar) == null)
				{
					result = (Characters._conquestCommanderStellar = Characters.ConquestStellarCommander());
				}
				return result;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x00048528 File Offset: 0x00046728
		public static Character conquestCommanderKolyatov
		{
			get
			{
				Character result;
				if ((result = Characters._conquestCommanderKolyatov) == null)
				{
					result = (Characters._conquestCommanderKolyatov = Characters.ConquestKolyatovCommander());
				}
				return result;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0004853E File Offset: 0x0004673E
		public static Character conquestCommanderLuminate
		{
			get
			{
				Character result;
				if ((result = Characters._conquestCommanderLuminate) == null)
				{
					result = (Characters._conquestCommanderLuminate = Characters.ConquestLuminateCommander());
				}
				return result;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x00048554 File Offset: 0x00046754
		public static Character mercenaryEmbassyIntroduction
		{
			get
			{
				Character result;
				if ((result = Characters._mercenaryEmbassyIntroduction) == null)
				{
					result = (Characters._mercenaryEmbassyIntroduction = Characters.MercenaryEmbassyIntroduction());
				}
				return result;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0004856A File Offset: 0x0004676A
		public static Character umbralReachStella
		{
			get
			{
				Character result;
				if ((result = Characters._umbralReachStella) == null)
				{
					result = (Characters._umbralReachStella = Characters.UmbralReachStella());
				}
				return result;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x00048580 File Offset: 0x00046780
		public static Character melloy
		{
			get
			{
				Character result;
				if ((result = Characters._melloy) == null)
				{
					result = (Characters._melloy = Characters.Melloy());
				}
				return result;
			}
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00048598 File Offset: 0x00046798
		public static Character GetCharacter(string name)
		{
			MethodInfo method = typeof(Characters).GetMethod(name);
			if (method != null && method.ReturnType == typeof(Character))
			{
				return (Character)method.Invoke(null, null);
			}
			Debug.LogWarning("Character '" + name + "' not found or does not return a Character.");
			return null;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x000485FC File Offset: 0x000467FC
		public static void CreateCaptain()
		{
			Characters.captain = Character.CreateCharacter(string.IsNullOrEmpty(GamePlayer.current.commander.callsign) ? GamePlayer.current.commander.firstName : GamePlayer.current.commander.callsign).WithDescription("Your player character").WithPortret(GamePlayer.current.commander.sprite);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00048667 File Offset: 0x00046867
		public static void CreateShipAI()
		{
			Characters.shipAi = Character.CreateCharacter("ECHO").WithDescription("Your personal AI - Experimental Command & Helmsman Operator").WithPortret(Resources.Load<Sprite>("Sprites/NPC/AI"));
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00048694 File Offset: 0x00046894
		public static Character Greg()
		{
			Character character = Character.CreateCharacter("Greg").WithDescription("Outpost Chief").WithPortret(Resources.Load<Sprite>("Sprites/NPC/Greg"));
			character.AddDialogue(MissionTrigger.Tutorial2Welcome, new Func<List<DialogueLine>>(Tutorial.Tutorial_2_Welcome), null, false);
			character.AddDialogue(MissionTrigger.Tutorial3Complete, new Func<List<DialogueLine>>(Tutorial.Tutorial_3_Complete), null, false);
			character.AddDialogue(MissionTrigger.Tutorial4Complete, new Func<List<DialogueLine>>(Tutorial.Tutorial_4_Complete), null, false);
			character.AddDefaultDialogue(Tutorial.Greg_Default(character));
			return character;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00048712 File Offset: 0x00046912
		public static Character Elena()
		{
			return Character.CreateCharacter("Elena").WithDescription("Mechanic").WithPortret(Resources.Load<Sprite>("Sprites/NPC/Elena"));
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00048738 File Offset: 0x00046938
		public static Character Creed()
		{
			Character character = Character.CreateCharacter("Creed").WithDescription("Station Commander").WithPortret(Resources.Load<Sprite>("Sprites/NPC/Creed"));
			character.AddDialogue(MissionTrigger.Tutorial6Welcome, new Func<List<DialogueLine>>(Tutorial.Tutorial_6_Welcome), null, false);
			character.AddDialogue(MissionTrigger.Tutorial7Complete, new Func<List<DialogueLine>>(Tutorial.Tutorial_7_Complete), null, false);
			character.AddDialogue(MissionTrigger.Tutorial8Complete, new Func<List<DialogueLine>>(Tutorial.Tutorial_8_Complete), null, false);
			character.AddDialogue(MissionTrigger.Tutorial9Complete, new Func<List<DialogueLine>>(Tutorial.Tutorial_9_Complete), null, false);
			character.AddDialogue(MissionTrigger.Tutorial10CombatComplete, new Func<List<DialogueLine>>(Tutorial.Tutorial_10_Combat_Complete), null, false);
			character.AddDialogue(MissionTrigger.Tutorial10Complete, new Func<List<DialogueLine>>(Tutorial.Tutorial_10_Complete), null, false);
			character.AddDefaultDialogue(Tutorial.Creed_Default(character));
			return character;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000487F8 File Offset: 0x000469F8
		public static Character Virgil()
		{
			Character character = Character.CreateCharacter("Virgil").WithDescription("Outpost Chief?").WithPortret(Resources.Load<Sprite>("Sprites/NPC/Virgil"));
			character.AddDialogue(MissionTrigger.Tutorial5Welcome, new Func<List<DialogueLine>>(Tutorial.Tutorial_5_Welcome), null, false);
			return character;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00048833 File Offset: 0x00046A33
		public static Character Umbral()
		{
			return Character.CreateCharacter("Unknown Contact").WithDescription("Your mysterious employer").WithPortret(Resources.Load<Sprite>("Sprites/NPC/Umbral"));
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00048858 File Offset: 0x00046A58
		public static Character RealUmbral()
		{
			Character character = Character.CreateCharacter("Umbral").WithDescription("Umbral Reach Contact").WithPortret(Resources.Load<Sprite>("Sprites/NPC/Umbral"));
			character.AddDialogue(MissionTrigger.Umbral21Umbral, new Func<List<DialogueLine>>(Puppeteers.Umbral21Completed), null, false);
			character.AddDialogue(MissionTrigger.Umbral22Failed, new Func<List<DialogueLine>>(Puppeteers.Umbral22Failed), null, false);
			character.AddDialogue(MissionTrigger.CU1TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest1), null, false);
			return character;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000488CC File Offset: 0x00046ACC
		public static Character LuminateCommander()
		{
			Character character = Character.CreateCharacter("Arle").WithDescription("Luminate Commander").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M2Luminate"));
			character.AddDialogue(MissionTrigger.UmbralLuminatePrisoner4, new Func<List<DialogueLine>>(Puppeteers.UmbralMission4LuminateStation), null, false);
			character.AddDialogue(MissionTrigger.Umbral4LuminatePrisonerRelease, new Func<List<DialogueLine>>(Puppeteers.Umbral4LuminatePrisonerRelease), null, false);
			character.AddDialogue(MissionTrigger.ConquestLuminateEmbassy, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.EmbassyLuminate), delegate
			{
				Characters.AlignWithMajorFaction(Faction.gold);
			}, true);
			return character;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0004895C File Offset: 0x00046B5C
		public static Character PrisonGuard()
		{
			return Character.CreateCharacter("Prison Guard").WithDescription("Prison Guard").WithPortret(Resources.Load<Sprite>("Sprites/NPC/AI"));
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00048981 File Offset: 0x00046B81
		public static Character ExpeditionCaptainRaythor()
		{
			return Character.CreateCharacter("John Raythor").WithDescription("Industrial").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M2Captain"));
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x000489A8 File Offset: 0x00046BA8
		public static Character ExpeditionKolyatovCaptain()
		{
			Character character = Character.CreateCharacter("Keril").WithDescription("Kolyatov Conscript").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M3Kolyatov"));
			character.AddDialogue(MissionTrigger.Umbral5KolyatovWelcome, new Func<List<DialogueLine>>(Puppeteers.UmbralLuminate5bKolyatovWelcome), null, false);
			character.AddDialogue(MissionTrigger.Umbral5KolyatovAttack, new Func<List<DialogueLine>>(Puppeteers.Umbral5bKolyatovAttack), new Action(Characters.AttackSteelVultures), true);
			character.AddDialogue(MissionTrigger.ConquestKolyatovEmbassy, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.EmbassyKolyatov), delegate
			{
				Characters.AlignWithMajorFaction(Faction.red);
			}, true);
			return character;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00048A44 File Offset: 0x00046C44
		public static Character SteelVultureComputerSalesman()
		{
			Character character = Character.CreateCharacter("Thundo Klipz").WithDescription("Steel Vuture Salvager").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M3SteelVulture"));
			character.AddDialogue(MissionTrigger.UmbralSteelVultureComputer, new Func<List<DialogueLine>>(Puppeteers.UmbralReachLuminate5SteelVultureSalesman), new Action(Characters.BuyComputer), true);
			return character;
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x00048A98 File Offset: 0x00046C98
		public static Character StellarRepresentative()
		{
			Character character = Character.CreateCharacter("Adeline Lorentz").WithDescription("Station Commander").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M4Stellar"));
			character.AddDialogue(MissionTrigger.Umbral7StellarWelcome, new Func<List<DialogueLine>>(Puppeteers.Umbral7StellarWelcome), null, false);
			character.AddDialogue(MissionTrigger.Umbral7StellarSkirmish, new Func<List<DialogueLine>>(Puppeteers.Umbral7StellarSkirmish), new Action(Characters.AttackPiratesSkirmish), true);
			character.AddDialogue(MissionTrigger.Umbral7StellarComplete, new Func<List<DialogueLine>>(Puppeteers.Umbral7StellarComplete), null, false);
			character.AddDialogue(MissionTrigger.ConquestStellarEmbassy, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.EmbassyStellar), delegate
			{
				Characters.AlignWithMajorFaction(Faction.blue);
			}, true);
			return character;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x00048B49 File Offset: 0x00046D49
		public static Character SteelVulturePocket()
		{
			return Character.CreateCharacter("Mick Flank").WithDescription("Salvager").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M5SteelVulture"));
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x00048B70 File Offset: 0x00046D70
		public static Character SmugglerContact()
		{
			Character character = Character.CreateCharacter("James Fleddon").WithDescription("Smuggler").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M6Smuggler"));
			character.AddDialogue(MissionTrigger.Umbral10Smuggler, new Func<List<DialogueLine>>(Puppeteers.Umbral10Smuggler), null, false);
			character.AddDialogue(MissionTrigger.Umbral13Smuggler, new Func<List<DialogueLine>>(Puppeteers.Umbral13Smuggler), null, false);
			character.AddDialogue(MissionTrigger.Umbral14SmugglerFailed, new Func<List<DialogueLine>>(Puppeteers.Umbral14SmugglerFailed), null, false);
			character.AddDialogue(MissionTrigger.Umbral14SmugglerComplete, new Func<List<DialogueLine>>(Puppeteers.Umbral14SmugglerComplete), null, false);
			character.AddDialogue(MissionTrigger.Umbral16Smuggler, new Func<List<DialogueLine>>(Puppeteers.Umbral16Smuggler), null, false);
			return character;
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00048C0E File Offset: 0x00046E0E
		public static Character SmugglerSector4()
		{
			Character character = Character.CreateCharacter("Claude").WithDescription("Smuggler").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M7Smuggler"));
			character.AddDialogue(MissionTrigger.Umbral11Smuggler, new Func<List<DialogueLine>>(Puppeteers.Umbral11Smuggler), null, false);
			return character;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00048C4C File Offset: 0x00046E4C
		public static Character StellarStagingPoint()
		{
			Character character = Character.CreateCharacter("Stella Chion").WithDescription("Stellar Manager").WithPortret(Resources.Load<Sprite>("Sprites/NPC/MP1Stellar"));
			character.AddDialogue(MissionTrigger.Umbral12Stellar, new Func<List<DialogueLine>>(Puppeteers.Umbral12Stellar), null, false);
			character.AddDialogue(MissionTrigger.Umbral18Stellar, new Func<List<DialogueLine>>(Puppeteers.Umbral18Stellar), null, false);
			character.AddDialogue(MissionTrigger.Umbral20Stellar, new Func<List<DialogueLine>>(Puppeteers.Umbral20Completed), null, false);
			return character;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00048CC0 File Offset: 0x00046EC0
		public static Character UmbralReachStella()
		{
			Character character = Character.CreateCharacter("Stella Chion").WithDescription("Umbral Reach Spymaster").WithPortret(Resources.Load<Sprite>("Sprites/NPC/MP1StellarSpymaster"));
			character.AddDialogue(MissionTrigger.CU3TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest4), null, false);
			character.AddDialogue(MissionTrigger.CU4TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest4b), null, false);
			character.AddDialogue(MissionTrigger.CU5TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest6), null, false);
			character.AddDialogue(MissionTrigger.CU6TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest7), null, false);
			character.AddDialogue(MissionTrigger.CU8TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest9), null, false);
			character.AddDialogue(MissionTrigger.CU9TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.UmbralConquest10), null, false);
			character.AddDefaultDialogue(Puppeteers.Stella_Default(character));
			return character;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00048D80 File Offset: 0x00046F80
		public static Character DarkspaceSmuggler1()
		{
			Character character = Character.CreateCharacter("Midas").WithDescription("Darkspace Smuggler").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M8Smuggler"));
			character.AddDialogue(MissionTrigger.Umbral15Darkspacers, new Func<List<DialogueLine>>(Puppeteers.Umbral15Darkspacers), null, false);
			character.AddDialogue(MissionTrigger.Umbral15Darkspacers, new Func<List<DialogueLine>>(Puppeteers.Umbral15Darkspacers), null, false);
			character.AddDialogue(MissionTrigger.CD1TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.DarkspaceConquest7), null, false);
			character.AddDialogue(MissionTrigger.CD2TalktoNPC, new Func<List<DialogueLine>>(Puppeteers.DarkspaceConquest8), null, false);
			character.AddDefaultDialogue(Puppeteers.Midas_Default(character));
			return character;
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00048E14 File Offset: 0x00047014
		public static Character Melloy()
		{
			return Character.CreateCharacter("Melloy").WithDescription("Funny Guy").WithPortret(Resources.Load<Sprite>("Sprites/NPC/M8Smuggler"));
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00048E3C File Offset: 0x0004703C
		public static void BuyComputer()
		{
			Salesman salesman = new Salesman(null);
			salesman._name = Characters.steelVultureComputerSalesman.name;
			salesman._isMale = true;
			salesman._icon = Characters.steelVultureComputerSalesman.portretSprite;
			salesman.description = Characters.steelVultureComputerSalesman.description;
			salesman.itemForSale = "UmbralComputer";
			salesman.itemCost = salesman.itemForSale.cost * 189;
			GameplayManager.Instance.ShowItemSaleInfo(salesman, new MissionTrigger?(MissionTrigger.UmbralSteelVultureComputer));
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00048EC0 File Offset: 0x000470C0
		public static void AttackSteelVultures()
		{
			AlertPopup.ShowQuery("@UmbralM5AttackSteelVultures", "Attack", "Cancel", delegate
			{
				UmbralMissions.AttackChoiceSteelVultures();
			}, null, null, null);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00048EF8 File Offset: 0x000470F8
		public static void AttackPiratesSkirmish()
		{
			AlertPopup.ShowQuery("Will you help Stellar Industries defeat the pirates?", "Attack", "Cancel", delegate
			{
				UmbralMissions.SkirmishChoiceStellarPirates();
			}, null, null, null);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00048F30 File Offset: 0x00047130
		public static Character QuestgiverSkilltreeMining()
		{
			Character character = Character.CreateCharacter("Elias McIntire").WithDescription("Mining Specialist").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeMining"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateMiningDialogue);
			character.missionIds.Add("SkilltreeMissionMining");
			character.missionIds.Add("SkilltreeMissionMining2");
			return character;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00048F94 File Offset: 0x00047194
		public static Character QuestgiverSkilltreeCombat()
		{
			Character character = Character.CreateCharacter("Olga Skarsgard").WithDescription("Combat Veteran").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeCombat"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateCombatDialogue);
			character.missionIds.Add("SkilltreeMissionCombat");
			character.missionIds.Add("SkilltreeMissionCombat2");
			return character;
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00048FF8 File Offset: 0x000471F8
		public static Character QuestgiverSkilltreeSalvage()
		{
			Character character = Character.CreateCharacter("Alice Okono").WithDescription("Scrapper").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeSalvage"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateSalvageDialogue);
			character.missionIds.Add("SkilltreeMissionSalvage");
			character.missionIds.Add("SkilltreeMissionSalvage2");
			return character;
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0004905C File Offset: 0x0004725C
		public static Character QuestgiverSkilltreeIndustrial()
		{
			Character character = Character.CreateCharacter("Brandon Wallace").WithDescription("Forgemaster").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeIndustrial"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateIndustrialDialogue);
			character.missionIds.Add("SkilltreeMissionIndustrial");
			character.missionIds.Add("SkilltreeMissionIndustrial2");
			return character;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x000490C0 File Offset: 0x000472C0
		public static Character QuestgiverSkilltreeDrones()
		{
			Character character = Character.CreateCharacter("Oron").WithDescription("Machine Shepherd").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeDrones"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateDronesDialogue);
			character.missionIds.Add("SkilltreeMissionDrones");
			character.missionIds.Add("SkilltreeMissionDrones2");
			return character;
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00049124 File Offset: 0x00047324
		public static Character QuestgiverSkilltreeDefense()
		{
			Character character = Character.CreateCharacter("Sergeant Ogolin").WithDescription("Defense Specialist").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeDefense"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateDefenseDialogue);
			character.missionIds.Add("SkilltreeMissionDefense");
			character.missionIds.Add("SkilltreeMissionDefense2");
			return character;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x00049188 File Offset: 0x00047388
		public static Character QuestgiverSkilltreeEconomy()
		{
			Character character = Character.CreateCharacter("Margot Cash").WithDescription("Veteran Economist").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkilltreeEconomy"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SkilltreeMissions.CreateEconomyDialogue);
			character.missionIds.Add("SkilltreeMissionEconomy");
			character.missionIds.Add("SkilltreeMissionEconomy2");
			return character;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x000491EC File Offset: 0x000473EC
		public static Character QuestgiverPatrol()
		{
			Character character = Character.CreateCharacter("Etienne Briggs").WithDescription("Canisec Secretary").WithPortret(Resources.Load<Sprite>("Sprites/NPC/PatrolGiver"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SideMissions.CreatePatrolDialogue);
			character.missionIds.Add("SideMissionPatrol");
			return character;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00049240 File Offset: 0x00047440
		public static Character QuestgiverBounty()
		{
			Character character = Character.CreateCharacter("Amalia Rodriguez").WithDescription("Bounty Hunter").WithPortret(Resources.Load<Sprite>("Sprites/NPC/BountyGiver"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SideMissions.CreateBountyDialogue);
			character.missionIds.Add("SideMissionBounty");
			return character;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00049294 File Offset: 0x00047494
		public static Character QuestgiverGoToConquest()
		{
			Character character = Character.CreateCharacter("Gabriel Ramos").WithDescription("Security Liaison").WithPortret(Resources.Load<Sprite>("Sprites/NPC/ConquestStart"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.ConquestMissions.CreateGotoConquestDialogue);
			character.missionIds.Add("StoryMissionSkipToConquest");
			return character;
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x000492E8 File Offset: 0x000474E8
		public static Character QuestgiverFastLaneTravel()
		{
			Character character = Character.CreateCharacter("Edar Thopter").WithDescription("Trade Director").WithPortret(Resources.Load<Sprite>("Sprites/NPC/TradeMission"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SideMissions.CreateFastLaneDialogue);
			character.missionIds.Add("SideMissionFastLane");
			return character;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0004933C File Offset: 0x0004753C
		public static Character QuestgiverSkilltreeUnlockTier2()
		{
			Character character = Character.CreateCharacter("Sergio Weisartz").WithDescription("Doctor?").WithPortret(Resources.Load<Sprite>("Sprites/NPC/SkillsDoctor"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SideMissions.CreateSkilltreeUnlockTier2Dialogue);
			character.missionIds.Add("SkilltreeUnlockTier2");
			return character;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00049390 File Offset: 0x00047590
		public static Character QuestgiverMercenaryIntroduction()
		{
			Character character = Character.CreateCharacter("Brenda Diamond").WithDescription("Satisfaction Officer").WithPortret(Resources.Load<Sprite>("Sprites/NPC/MercWoman"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.SideMissions.CreateMercenaryMission);
			character.missionIds.Add("MercenaryIntroduction");
			return character;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x000493E2 File Offset: 0x000475E2
		public static Character MercenaryEmbassyIntroduction()
		{
			Character character = Character.CreateCharacter("Anton Havel").WithDescription("Omnitac Sales Agent").WithPortret(Resources.Load<Sprite>("Sprites/NPC/MercMan"));
			character.AddDialogue(MissionTrigger.MercIntroEmbassyTalkToNPC, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.MercenaryEmbassyIntro), null, false);
			return character;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x00049420 File Offset: 0x00047620
		public static Character CanisecConquestCommander()
		{
			Character character = Character.CreateCharacter(Translation.Translate("@ConquestCanisecCommander", Array.Empty<object>())).WithDescription("Canisec Commander").WithPortret(Resources.Load<Sprite>("Sprites/NPC/ConquestCanisec"));
			character.AddDialogue(MissionTrigger.ConquestCanisec1, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.CanisecConquest1), null, false);
			return character;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x00049470 File Offset: 0x00047670
		public static Character ConquestStellarCommander()
		{
			Character character = Character.CreateCharacter(Translation.Translate("@ConquestStellarCommander", Array.Empty<object>())).WithDescription(Translation.Translate("@ConquestStellarCommanderTitle", Array.Empty<object>())).WithPortret(Resources.Load<Sprite>("Sprites/NPC/ConquestStellar"));
			character.AddDialogue(MissionTrigger.CSHQIntro, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestStellar1), null, false);
			character.AddDialogue(MissionTrigger.CS2, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestStellar2), null, false);
			character.AddDialogue(MissionTrigger.CS3, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestStellar3), null, false);
			return character;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x000494F8 File Offset: 0x000476F8
		public static Character ConquestKolyatovCommander()
		{
			Character character = Character.CreateCharacter(Translation.Translate("@ConquestKolyatovCommander", Array.Empty<object>())).WithDescription(Translation.Translate("@ConquestKolyatovCommanderTitle", Array.Empty<object>())).WithPortret(Resources.Load<Sprite>("Sprites/NPC/ConquestKolyatov"));
			character.AddDialogue(MissionTrigger.CKHQIntro, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestKolyatov1), null, false);
			character.AddDialogue(MissionTrigger.CK2, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestKolyatov2), null, false);
			character.AddDialogue(MissionTrigger.CK3, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestKolyatov3), null, false);
			return character;
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x00049580 File Offset: 0x00047780
		public static Character ConquestLuminateCommander()
		{
			Character character = Character.CreateCharacter(Translation.Translate("@ConquestLuminateCommander", Array.Empty<object>())).WithDescription(Translation.Translate("@ConquestLuminateCommanderTitle", Array.Empty<object>())).WithPortret(Resources.Load<Sprite>("Sprites/NPC/ConquestLuminate"));
			character.AddDialogue(MissionTrigger.CLHQIntro, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestLuminate1), null, false);
			character.AddDialogue(MissionTrigger.CL2, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestLuminate2), null, false);
			character.AddDialogue(MissionTrigger.CL3, new Func<List<DialogueLine>>(Source.Dialogues.Content.ConquestMissions.ConquestLuminate3), null, false);
			return character;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x00049608 File Offset: 0x00047808
		private static void AlignWithMajorFaction(Faction faction)
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (faction.IsEnemy(Faction.player))
			{
				AlertPopup.ShowQuery(Translation.Translate("@ConquestFactionEnemy", new object[]
				{
					Translation.Translate(faction.name, Array.Empty<object>())
				}), Translation.Translate("@UIYes", Array.Empty<object>()), null, null, null, null, null);
				return;
			}
			if (storyteller.GetHeadquarters(faction) == null)
			{
				AlertPopup.ShowQuery(Translation.Translate("@ConquestMajorFactionNotActive", Array.Empty<object>()), Translation.Translate("@ConquestIllBeBack", Array.Empty<object>()), null, null, null, null, null);
				return;
			}
			AlertPopup.ShowQuery(Translation.Translate("@ConquestAlignWithMajorFaction", Array.Empty<object>()), Translation.Translate("@UIYes", Array.Empty<object>()), Translation.Translate("@UINo", Array.Empty<object>()), delegate
			{
				Source.MissionSystem.Story.ConquestMissions.FactionAlignmentChoice(faction);
			}, null, null, null);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000496FC File Offset: 0x000478FC
		public static Character QuestgiverMiningToConquest()
		{
			Character character = Character.CreateCharacter("Waldo Everson").WithDescription("Frontier Operations").WithPortret(Resources.Load<Sprite>("Sprites/NPC/ToConquestMining"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.ConquestMissions.MiningToConquest);
			character.missionIds.Add("MiningGoToConquest");
			return character;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x00049750 File Offset: 0x00047950
		public static Character QuestgiverSalvageToConquest()
		{
			Character character = Character.CreateCharacter("Lynn Bree").WithDescription("Salvage Opportunist").WithPortret(Resources.Load<Sprite>("Sprites/NPC/ToConquestSalvage"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.ConquestMissions.SalvageToConquest);
			character.missionIds.Add("SalvageGoToConquest");
			return character;
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x000497A4 File Offset: 0x000479A4
		public static Character QuestgiverMaraudersToConquest()
		{
			Character character = Character.CreateCharacter("Horace the Red").WithDescription("Corsair Scout").WithPortret(Resources.Load<Sprite>("Sprites/NPC/ToConquestMarauders"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.ConquestMissions.MaraudersToConquest);
			character.missionIds.Add("MaraudersGoToConquest");
			return character;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x000497F8 File Offset: 0x000479F8
		public static Character QuestgiverStrandedToConquest()
		{
			Character character = Character.CreateCharacter("Mirthe Coman").WithDescription("Opportunity Analyst").WithPortret(Resources.Load<Sprite>("Sprites/NPC/ToConquestStranded"));
			character.createDialogue = new Func<Character, Dialogue>(Source.Dialogues.Content.ConquestMissions.StrandedToConquest);
			character.missionIds.Add("StrandedGoToConquest");
			return character;
		}

		// Token: 0x04000538 RID: 1336
		public static Character captain;

		// Token: 0x04000539 RID: 1337
		public static Character shipAi;

		// Token: 0x0400053A RID: 1338
		private static Character _virgil;

		// Token: 0x0400053B RID: 1339
		private static Character _elena;

		// Token: 0x0400053C RID: 1340
		private static Character _creed;

		// Token: 0x0400053D RID: 1341
		private static Character _greg;

		// Token: 0x0400053E RID: 1342
		private static Character _umbralContact;

		// Token: 0x0400053F RID: 1343
		private static Character _realUmbralContact;

		// Token: 0x04000540 RID: 1344
		private static Character _luminateCommander;

		// Token: 0x04000541 RID: 1345
		private static Character _prisonGuard;

		// Token: 0x04000542 RID: 1346
		private static Character _raythor;

		// Token: 0x04000543 RID: 1347
		private static Character _kolyatovCaptain;

		// Token: 0x04000544 RID: 1348
		private static Character _steelVultureComputerSalesman;

		// Token: 0x04000545 RID: 1349
		private static Character _stellarRepresentative;

		// Token: 0x04000546 RID: 1350
		private static Character _steelVulturePocket;

		// Token: 0x04000547 RID: 1351
		private static Character _smugglerContact;

		// Token: 0x04000548 RID: 1352
		private static Character _smugglerSector4;

		// Token: 0x04000549 RID: 1353
		private static Character _stellarStagingPoint;

		// Token: 0x0400054A RID: 1354
		private static Character _darkspaceSmuggler1;

		// Token: 0x0400054B RID: 1355
		private static Character _canisecConquestCommander;

		// Token: 0x0400054C RID: 1356
		private static Character _conquestCommanderStellar;

		// Token: 0x0400054D RID: 1357
		private static Character _conquestCommanderKolyatov;

		// Token: 0x0400054E RID: 1358
		private static Character _conquestCommanderLuminate;

		// Token: 0x0400054F RID: 1359
		private static Character _mercenaryEmbassyIntroduction;

		// Token: 0x04000550 RID: 1360
		private static Character _umbralReachStella;

		// Token: 0x04000551 RID: 1361
		private static Character _melloy;
	}
}
