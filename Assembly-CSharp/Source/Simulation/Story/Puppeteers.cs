using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Dialogues;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Travel;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Data.Persistable;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.Story
{
	// Token: 0x0200008A RID: 138
	public class Puppeteers : Storyteller
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0002ABC7 File Offset: 0x00028DC7
		public static Character captain
		{
			get
			{
				return Characters.captain;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0002ABCE File Offset: 0x00028DCE
		public static Character shipAi
		{
			get
			{
				return Characters.shipAi;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0002ABD5 File Offset: 0x00028DD5
		public static Character umbralContact
		{
			get
			{
				return Characters.umbralContact;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0002ABDC File Offset: 0x00028DDC
		public static Character realUmbralContact
		{
			get
			{
				return Characters.realUmbralContact;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0002ABE3 File Offset: 0x00028DE3
		public static Character luminateCommander
		{
			get
			{
				return Characters.luminateCommander;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x0002ABEA File Offset: 0x00028DEA
		public static Character raythor
		{
			get
			{
				return Characters.raythor;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0002ABF1 File Offset: 0x00028DF1
		public static Character kolyatovCaptain
		{
			get
			{
				return Characters.kolyatovCaptain;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0002ABF8 File Offset: 0x00028DF8
		public static Character steelVutureComputerSalesman
		{
			get
			{
				return Characters.steelVultureComputerSalesman;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0002ABFF File Offset: 0x00028DFF
		public static Character stellarRepresentative
		{
			get
			{
				return Characters.stellarRepresentative;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x0002AC06 File Offset: 0x00028E06
		public static Character steelVulturePocket
		{
			get
			{
				return Characters.steelVulturePocket;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x0002AC0D File Offset: 0x00028E0D
		public static Character smugglerContact
		{
			get
			{
				return Characters.smugglerContact;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0002AC14 File Offset: 0x00028E14
		public static Character smugglerSector4
		{
			get
			{
				return Characters.smugglerSector4;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0002AC1B File Offset: 0x00028E1B
		public static Character stellarStagingPoint
		{
			get
			{
				return Characters.stellarStagingPoint;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0002AC22 File Offset: 0x00028E22
		public static Character darkspaceSmuggler1
		{
			get
			{
				return Characters.darkspaceSmuggler1;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0002AC29 File Offset: 0x00028E29
		public static Character umbralReachStella
		{
			get
			{
				return Characters.umbralReachStella;
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0002AC30 File Offset: 0x00028E30
		public Puppeteers(GamePlayer ply) : base(ply)
		{
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0002AC39 File Offset: 0x00028E39
		public override void SetupNewGame()
		{
			this.SetupBeaconPoi();
			this.startDialogue = true;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0002AC48 File Offset: 0x00028E48
		private void SetupBeaconPoi()
		{
			Beacon beacon = this.player.currentSystem.AddBeacon("UmbralBeacon1", Faction.puppeteers, null);
			beacon.AddCargoContainers(new Vector2(5f, 8f), 2, 1f);
			LootContainerData lootContainerData = new LootContainerData
			{
				position = beacon.GetWorldPosition() + new Vector2(SeededRandom.Global.RandomRange(0f, beacon.position.x), SeededRandom.Global.RandomRange(-beacon.position.y / 2f, beacon.position.y / 2f)),
				name = "@LCNameOldCargoContainer"
			};
			int level = GamePlayer.current.level;
			lootContainerData.AddLoot(EquipmentBuilder.Get("SmallAutocannon").CreateItemType(Rarity.Standard, level + 1, true, "umbralhelp", false, false), 1);
			lootContainerData.AddLoot(InventoryItemType.Get("AutoCannon Ammo"), 400);
			lootContainerData.AddLoot(EquipmentBuilder.Get("SmallSalvageLaser").CreateItemType(Rarity.Standard, level + 1, true, "umbralhelp", false, false), 1);
			lootContainerData.AddLoot(EquipmentBuilder.Get("SmallMiningLaser").CreateItemType(Rarity.Standard, level + 1, true, "umbralhelp", false, false), 1);
			beacon.AddPersistable(lootContainerData);
			this.GenerateUmbralMissions();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0002AD96 File Offset: 0x00028F96
		public override void Start()
		{
			if (this.startDialogue)
			{
				this.startDialogue = false;
				GameplayManager.Instance.StartCoroutine(this.StartWarpIn());
				GameplayManager.Instance.StartCoroutine(this.StartInitDialogue());
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0002ADC9 File Offset: 0x00028FC9
		private IEnumerator StartWarpIn()
		{
			yield return new WaitUntil(() => GameplayManager.Instance.spaceShip);
			SpaceShip spaceship = GameplayManager.Instance.spaceShip;
			if (spaceship.jumpingProcedureEngaged)
			{
				yield break;
			}
			spaceship.SetMaskInteraction(SpriteMaskInteraction.VisibleOutsideMask);
			spaceship.SetEngineState(false, false);
			yield return new WaitUntil(() => JumpGateManager.instance);
			yield return new WaitUntil(() => JumpGateManager.instance.initializedAndReady);
			spaceship.jumpingProcedureEngaged = true;
			MapPointOfInterest current = MapPointOfInterest.current;
			JumpGate gate = current as JumpGate;
			if (gate != null)
			{
				gate.UnlockJumpgate();
				yield return JumpGateManager.instance.ArriveAtGate();
				gate.LockGate();
			}
			spaceship.SetEngineState(true, true);
			spaceship.SetOverrideDestination(new Vector2(spaceship.transform.position.x + 8f, spaceship.transform.position.y), true, false, false);
			yield break;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0002ADD1 File Offset: 0x00028FD1
		private IEnumerator StartInitDialogue()
		{
			yield return new WaitForSeconds(10f);
			Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralMission1Start(), null);
			yield break;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0002ADD9 File Offset: 0x00028FD9
		public override void StoryUpdate(float delta)
		{
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0002ADDC File Offset: 0x00028FDC
		public override void OnStoryMissionComplete(string storyId)
		{
			uint num = (uint)storyId.GetHashCode();
			if (num <= 1762841078U)
			{
				if (num <= 420761275U)
				{
					if (num <= 67399070U)
					{
						if (num != 33843832U)
						{
							if (num != 50621451U)
							{
								if (num != 67399070U)
								{
									return;
								}
								if (!(storyId == "UmbralMission20"))
								{
									return;
								}
								MapPointOfInterest.current.faction = Faction.puppeteers;
								return;
							}
							else
							{
								if (!(storyId == "UmbralMission23"))
								{
									return;
								}
								Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.Umbral23Completed(), null);
								return;
							}
						}
						else
						{
							if (!(storyId == "UmbralMission22"))
							{
								return;
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.Umbral22Completed(), null);
							return;
						}
					}
					else if (num <= 336873180U)
					{
						if (num != 84029594U)
						{
							if (num != 336873180U)
							{
								return;
							}
							if (!(storyId == "UmbralMission8"))
							{
								return;
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralMission8Completed(), null);
							return;
						}
						else
						{
							if (!(storyId == "UmbralMission19"))
							{
								return;
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.Umbral19Completed(), null);
							return;
						}
					}
					else if (num != 353650799U)
					{
						if (num != 420761275U)
						{
							return;
						}
						if (!(storyId == "UmbralMission5"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralMission5Completed(), null);
						return;
					}
					else
					{
						if (!(storyId == "UmbralMission9"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.Umbral9Complete(), null);
						return;
					}
				}
				else if (num <= 612194146U)
				{
					if (num <= 487871751U)
					{
						if (num != 437538894U)
						{
							if (num != 487871751U)
							{
								return;
							}
							if (!(storyId == "UmbralMission1"))
							{
								return;
							}
							for (int i = 0; i < Register.GetVisitedStationsCount(); i++)
							{
								MissionObjective.Trigger(MissionTrigger.VisitUniqueSystem, null, null, false);
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralMission1Completed(), null);
							return;
						}
						else
						{
							if (!(storyId == "UmbralMission6"))
							{
								return;
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralMission6Completed(), null);
							return;
						}
					}
					else if (num != 521426989U)
					{
						if (num != 612194146U)
						{
							return;
						}
						if (!(storyId == "DarkspaceMission4Failed"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission4Failed(), null);
						return;
					}
					else
					{
						if (!(storyId == "UmbralMission3"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralMission3Completed(), null);
						return;
					}
				}
				else if (num <= 1729285840U)
				{
					if (num != 1408769947U)
					{
						if (num != 1729285840U)
						{
							return;
						}
						if (!(storyId == "ConquestDarkspace7"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest7(), null);
						return;
					}
					else
					{
						if (!(storyId == "UmbralMission5b"))
						{
							return;
						}
						GalaxyMapData.current.allSectors.ElementAt(1).FindStationForFaction(Faction.salvageGuild, new bool?(false));
						return;
					}
				}
				else if (num != 1746063459U)
				{
					if (num != 1762841078U)
					{
						return;
					}
					if (!(storyId == "ConquestDarkspace5"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest5(), null);
					return;
				}
				else
				{
					if (!(storyId == "ConquestDarkspace6"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest6(), null);
					return;
				}
			}
			else if (num <= 2467578399U)
			{
				if (num <= 1813173935U)
				{
					if (num != 1779618697U)
					{
						if (num != 1796396316U)
						{
							if (num != 1813173935U)
							{
								return;
							}
							if (!(storyId == "ConquestDarkspace2"))
							{
								return;
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest2(), null);
							return;
						}
						else
						{
							if (!(storyId == "ConquestDarkspace3"))
							{
								return;
							}
							Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest3(), null);
							return;
						}
					}
					else
					{
						if (!(storyId == "ConquestDarkspace4"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest4(), null);
						return;
					}
				}
				else if (num <= 2417245542U)
				{
					if (num != 1829951554U)
					{
						if (num != 2417245542U)
						{
							return;
						}
						if (!(storyId == "ConquestUmbral2"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralConquest2(), null);
						return;
					}
					else
					{
						if (!(storyId == "ConquestDarkspace1"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceConquest1(), null);
						return;
					}
				}
				else if (num != 2434023161U)
				{
					if (num != 2467578399U)
					{
						return;
					}
					if (!(storyId == "ConquestUmbral5"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralConquest5(), null);
					return;
				}
				else
				{
					if (!(storyId == "ConquestUmbral3"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.UmbralConquest3(), null);
					return;
				}
			}
			else if (num <= 3376537636U)
			{
				if (num <= 3342982398U)
				{
					if (num != 3309427160U)
					{
						if (num != 3342982398U)
						{
							return;
						}
						if (!(storyId == "DarkspaceMission3"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission3(), null);
						return;
					}
					else
					{
						if (!(storyId == "DarkspaceMission1"))
						{
							return;
						}
						Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission1(), null);
						return;
					}
				}
				else if (num != 3359760017U)
				{
					if (num != 3376537636U)
					{
						return;
					}
					if (!(storyId == "DarkspaceMission5"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission5Completed(), null);
					return;
				}
				else
				{
					if (!(storyId == "DarkspaceMission2"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission2(), null);
					return;
				}
			}
			else if (num <= 3426870493U)
			{
				if (num != 3393315255U)
				{
					if (num != 3426870493U)
					{
						return;
					}
					if (!(storyId == "DarkspaceMission6"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission6Complete(), null);
					return;
				}
				else
				{
					if (!(storyId == "DarkspaceMission4"))
					{
						return;
					}
					Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.DarkspaceMission4Completed(), null);
					return;
				}
			}
			else if (num != 4144110224U)
			{
				if (num != 4177665462U)
				{
					return;
				}
				if (!(storyId == "UmbralMission15"))
				{
					return;
				}
				Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.Umbral15Darkspacers(), null);
				return;
			}
			else
			{
				if (!(storyId == "UmbralMission17"))
				{
					return;
				}
				Singleton<DialogueManager>.Instance.StartDialogue(Puppeteers.Umbral17Completed(), null);
				return;
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0002B3D5 File Offset: 0x000295D5
		public void GenerateUmbralMissions()
		{
			GamePlayer.current.AddMissionWithLog(StoryMission.Get(GamePlayer.current, "UmbralMission1"));
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0002B3F0 File Offset: 0x000295F0
		public static List<DialogueLine> UmbralMission1Start()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "We have made it, captain!"));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "Captain, we're receiving a hail."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "ECHO, put it through."));
			list.Add(DialogueLine.cDL(Puppeteers.umbralContact, "Captain " + Puppeteers.captain.name + ", you took your time..."));
			list.Add(DialogueLine.cDL(Puppeteers.umbralContact, "This channel isn't secure... come to the rally point."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "This seems to be our contact profile, Captain. I've marked the Rally Point on your map.").WithTrigger(delegate
			{
				SidePanel.instance.NotifyTab(SidePanel.SideTabType.Map, null);
			}));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Understood, thank you ECHO."));
			return list;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0002B4D0 File Offset: 0x000296D0
		public static List<DialogueLine> UmbralMission1Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralContact, "Captain, I've been waiting for quite a bit."),
				DialogueLine.cDL(Puppeteers.captain, "I... got here as fast as I could. Who am I communicating with?"),
				DialogueLine.cDL(Puppeteers.umbralContact, "...Have you made contact with any of the others?"),
				DialogueLine.cDL(Puppeteers.captain, "No, I haven't seen anyone since before the jump. I got thrown off course and ended up in a closed sector."),
				DialogueLine.cDL(Puppeteers.umbralContact, "I should have known it wouldn't go flawlessly... quite a botched operation already. But... we'll still make use of you."),
				DialogueLine.cDL(Puppeteers.umbralContact, "I'll contact you when we know more."),
				DialogueLine.cDL(Puppeteers.umbralContact, "In the meantime, get acquainted with your surrounding systems. Maybe do a job or two. It's quite peaceful around there... if you ignore the outlaws."),
				DialogueLine.cDL(Puppeteers.umbralContact, "If you need the items in the containers, take them."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, we can install new hardware at the Personal Hangar in any spacestation.")
			};
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0002B5A0 File Offset: 0x000297A0
		public static List<DialogueLine> UmbralMission3Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralContact, "Captain, urgent news."),
				DialogueLine.cDL(Puppeteers.umbralContact, "One of your expedition members has been captured by the " + Translation.Translate(Faction.gold.name, Array.Empty<object>()) + "."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Find him and get him out. He's carrying critical information I need."),
				DialogueLine.cDL(Puppeteers.umbralContact, "The location of the " + Translation.Translate("@UComputer", Array.Empty<object>()) + ".")
			};
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0002B640 File Offset: 0x00029840
		public static List<DialogueLine> UmbralMission4LuminateStation()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "Welcome to " + MapPointOfInterest.current.name + "."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, string.Concat(new string[]
			{
				"I'm ",
				Puppeteers.luminateCommander.name,
				" of the ",
				Translation.Translate(Faction.gold.name, Array.Empty<object>()),
				"."
			})));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "We've already encountered two others of your kind."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Our kind?"));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "Disruptors... "));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "I... don't know how to respond to that. Are they nearby? Can I speak to them?"));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "They broke several laws. One escaped. The other is in custody."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "So no, you cannot speak to them."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Laws? I'm sure they weren't that bad... Our group suffered a major jump malfunction. Ending up here wasn't their intention."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "... I might be willing to release him, if your reputation with us were higher..."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "This is all I can offer."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "May the oracle find you."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "... And you?"));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "Captain, I've scanned the system and found a prison outpost, the only one within range.").WithTrigger(delegate
			{
				Puppeteers.UnhidePrisonPOI();
			}));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Great, let's go break him out..."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "... Or we could improve relations with the " + Translation.Translate(Faction.gold.name, Array.Empty<object>()) + ", they will release him as a favor."));
			return list;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0002B83C File Offset: 0x00029A3C
		public static List<DialogueLine> Umbral4LuminatePrisonerRelease()
		{
			Mission mission = GamePlayer.current.missions.FirstOrDefault((Mission m) => m.storyId == "UmbralMission4");
			bool flag = false;
			if (mission.steps.ElementAt(1).objectives.ElementAt(1).IsComplete())
			{
				flag = true;
			}
			List<DialogueLine> list = new List<DialogueLine>();
			if (flag)
			{
				list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, flag ? "Captain, your behaviour is as foretold..." : "Captain, you have done us a great service helping us.."));
				list.Add(DialogueLine.cDL(Puppeteers.captain, "I... "));
				list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "No need to apologize, Captain. You did us a favor."));
			}
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "We will release your companion, we have been holding him here."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "Untill next time, Captain."));
			list.Add(DialogueLine.cDL(Puppeteers.luminateCommander, "May the oracle find you."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Thank you?"));
			if (flag)
			{
				list.Add(DialogueLine.cDL(Puppeteers.captain, "Echo, what just happened? We blasted their outpost, but they didn't seem to care..."));
				list.Add(DialogueLine.cDL(Puppeteers.shipAi, "It is unclear, I will have to run more analysis."));
			}
			list.Add(DialogueLine.cDL(Puppeteers.raythor, "Thank you for getting me released."));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, "It's you! " + Puppeteers.captain.name + ", right? We haven't properly met but I've seen your name on the mission manifest."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "I present to you... Captain " + Puppeteers.captain.name + "."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Thank you, " + Puppeteers.shipAi.name + ". Good to meet you...? How'd you end up here?"));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, Puppeteers.raythor.name + ". My ship got a jump malfunction, ended up some systems away. I found another member, " + Puppeteers.kolyatovCaptain.name + "."));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, "We were making our way back to the rally point, but got intercepted by the " + Translation.Translate(Faction.gold.name, Array.Empty<object>()) + "."));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, Puppeteers.kolyatovCaptain.name + "'s ship is carrying the " + Translation.Translate("@UComputer", Array.Empty<object>()) + ", so we prioritized his escape."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Why would they target you?"));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, "I don't know, maybe it was sacred space for them we unknowingly entered..."));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, "Anyway, we should really go after " + Puppeteers.kolyatovCaptain.name + ". We could track down his warp trail.."));
			list.Add(DialogueLine.cDL(Puppeteers.raythor, Puppeteers.captain.name + ", since my ship is a goner. Can I join your crew?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Definitely, we'll take all the help we can get."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "Captain, I'll analyze his trail, and send you the coordinates."));
			return list;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0002BB3B File Offset: 0x00029D3B
		public static void UnhidePrisonPOI()
		{
			GalaxyMapData.current.GetPointOfInterest("pup4prison").hidden = false;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0002BB54 File Offset: 0x00029D54
		public static List<DialogueLine> UmbralMission5Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Wow wow wow... wow. What are you doing here?"),
				DialogueLine.cDL(Puppeteers.raythor, "Captain, be careful... these are " + Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>()) + "."),
				DialogueLine.cDL(Puppeteers.raythor, "It seems we are disturbing their feast, so to speak."),
				DialogueLine.cDL(Puppeteers.raythor, "Wait a second, that's " + Puppeteers.kolyatovCaptain.name + "'s ship!"),
				DialogueLine.cDL(Puppeteers.raythor, "Ahem, salvage captain, this ship belongs to Captain " + Puppeteers.kolyatovCaptain.name + "."),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Wrong! This is our ship."),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Now you get outta here, before we call in backup."),
				DialogueLine.cDL(Puppeteers.captain, "We understand... but do you know where this captain went? Is he alive?"),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Of course... you think we are savages?! No, we're salvagers."),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "I reckon he went to a " + Translation.Translate(Faction.red.name, Array.Empty<object>()) + " station."),
				DialogueLine.cDL(Puppeteers.captain, "And did you find a sort of compu...?"),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "I said get outta here. We're busy.")
			};
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0002BCC8 File Offset: 0x00029EC8
		public static List<DialogueLine> UmbralLuminate5bKolyatovWelcome()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "Hello there..."),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, Puppeteers.raythor.name + "! You made it"),
				DialogueLine.cDL(Puppeteers.raythor, Puppeteers.kolyatovCaptain.name + ", yes, thanks to " + Puppeteers.captain.name + " here"),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "I wanted to come back but my new boss wouldn't allow it."),
				DialogueLine.cDL(Puppeteers.raythor, "I see you are wearing " + Translation.Translate(Faction.red.name, Array.Empty<object>()) + " colors."),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "I felt obligated to join, seeing as they took me in after I almost got scrapped alive by the " + Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>())),
				DialogueLine.cDL(Puppeteers.captain, Puppeteers.kolyatovCaptain.name + ", did you manage to bring back the Computer?"),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "You know of the Computer? I... could not retrieve it, I suspect the " + Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>()) + " have it."),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "We are actually about to attack one of their outposts."),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "Let me know if you want to join in, I'll be here when you are ready."),
				DialogueLine.cDL(Puppeteers.raythor, "Captain, we could try and go to the " + Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>()) + " and see if we can strike a deal without bloodshed?")
			};
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0002BE70 File Offset: 0x0002A070
		public static List<DialogueLine> Umbral5bKolyatovAttack()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, string.Concat(new string[]
				{
					"Captain ",
					Puppeteers.captain.name,
					", are you ready to assault the ",
					Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>()),
					" outpost?"
				})),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "Don't worry about your standing, there won't be any evidence of your involvement."),
				DialogueLine.cDL(Puppeteers.kolyatovCaptain, "They'll all be dead.")
			};
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0002BF08 File Offset: 0x0002A108
		public static List<DialogueLine> UmbralReachLuminate5SteelVultureSalesman()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Hello there... Oh it's you."),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Can I help ye?"),
				DialogueLine.cDL(Puppeteers.captain, "We do meet again, I am wondering if you extracted some sort of computer from that ship, a Command Co... "),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, let's not make it seem that valuable.."),
				DialogueLine.cDL(Puppeteers.captain, "Command... console? Game console! For playing games."),
				DialogueLine.cDL(Puppeteers.shipAi, "Nice save?"),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "Let me see..."),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "... This one? Can't seem to get it to play games..."),
				DialogueLine.cDL(Puppeteers.steelVutureComputerSalesman, "You can take it off our hands for a price.")
			};
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0002BFD8 File Offset: 0x0002A1D8
		public static List<DialogueLine> UmbralMission6Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralContact, "Captain, you found the computer, thankfully this mission is not lost..."),
				DialogueLine.cDL(Puppeteers.umbralContact, "You are turning out to be quite reliable."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Time to let you in a bit more."),
				DialogueLine.cDL(Puppeteers.umbralContact, "What we are trying to do is to get a foothold in this \"new\" galaxy."),
				DialogueLine.cDL(Puppeteers.umbralContact, "The major factions all are racing to get deeper and deeper into the so-called Darkspace."),
				DialogueLine.cDL(Puppeteers.umbralContact, "We need to get there before them, or now at least around the same time."),
				DialogueLine.cDL(Puppeteers.umbralContact, "They are already building a gate, but seeing as they are doing it together, with all the hostilities, it's not going as fast."),
				DialogueLine.cDL(Puppeteers.umbralContact, "We, however, have a plan. The old constructor fleet."),
				DialogueLine.cDL(Puppeteers.umbralContact, "There aren't many operational, so our best bet is to get the schematics from " + Translation.Translate(Faction.blue.name, Array.Empty<object>()) + "."),
				DialogueLine.cDL(Puppeteers.umbralContact, "I'm in the progress of finding crafting materials, I'll let you know once you come back after you acquired the schematics.")
			};
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0002C0DC File Offset: 0x0002A2DC
		public static List<DialogueLine> Umbral7StellarWelcome()
		{
			bool flag = ReputationLevelExtensions.IsLevelOrHigher(Faction.blue.GetReputationLevel(Faction.player), ReputationLevel.Friendly);
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "State your business."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Hello? We are looking to purchase some schematics... the constructor drone schematics?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "For... constructing a mining operation?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "Right, if you would have said for constructing a gate I wouldn't have continued this conversation..."));
			if (flag)
			{
				list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "It seems you have been quite helpful to us lately."));
				list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "I'll go and look for the schematics, come find me in a little bit."));
			}
			else
			{
				list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "We can give you a blueprint-copy, if you help us out?"));
				list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "We have a pirate problem. As always."));
				list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "Our forces here are limited. Most of the navy's out defending higher-priority systems."));
				list.Add(DialogueLine.cDL(Puppeteers.stellarRepresentative, "If you can assist us in clearing a pirate system, we'll trade you the schematics."));
			}
			return list;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0002C1DB File Offset: 0x0002A3DB
		public static List<DialogueLine> Umbral7StellarSkirmish()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.stellarRepresentative, "Captain, are you ready to attack?"),
				DialogueLine.cDL(Puppeteers.stellarRepresentative, "We'll provide you with the gatepass to the pirate system.")
			};
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0002C20C File Offset: 0x0002A40C
		public static List<DialogueLine> Umbral7StellarComplete()
		{
			Mission mission = GamePlayer.current.missions.FirstOrDefault((Mission m) => m.storyId == "UmbralMission7");
			bool flag = false;
			if (mission.steps.ElementAt(1).objectives.ElementAt(1).IsComplete())
			{
				flag = true;
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.stellarRepresentative, flag ? ("Captain " + Puppeteers.captain.name + "... I saw the report, your impact was felt throughout the system.") : string.Concat(new string[]
				{
					"Captain ",
					Puppeteers.captain.name,
					", there you are. Always a pleasure to help a friend of ",
					Translation.Translate(Faction.blue.name, Array.Empty<object>()),
					"."
				})),
				DialogueLine.cDL(Puppeteers.stellarRepresentative, "Here is a blueprint-copy of the schematics, good luck trying to craft them..."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, these are indeed the schematics we need."),
				DialogueLine.cDL(Puppeteers.captain, "Thanks, let's bring them to " + Puppeteers.umbralContact.name + " and see what we need to do next.")
			};
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0002C338 File Offset: 0x0002A538
		public static List<DialogueLine> UmbralMission8Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralContact, "So we have the schematics, and the computer."),
				DialogueLine.cDL(Puppeteers.umbralContact, "I've sourced the location of the materials, our best bet is to use the old drone parts. That will cut crafting time by quite a bit."),
				DialogueLine.cDL(Puppeteers.umbralContact, "There is a secluded system owned by the " + Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>()) + ". I'm in the process of striking a deal with them. Gain access to their system and start salvaging."),
				DialogueLine.cDL(Puppeteers.umbralContact, "You should be able to purchase a gate pass directly from their station."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Bring them some materials and the schematics, and they should take over the larger operation.")
			};
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0002C3D4 File Offset: 0x0002A5D4
		public static List<DialogueLine> Umbral9Complete()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.steelVulturePocket, "Oye, you Captain " + Puppeteers.captain.name + "?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Sure am, you are?"));
			list.Add(DialogueLine.cDL(Puppeteers.steelVulturePocket, Puppeteers.steelVulturePocket.name + " be the name."));
			list.Add(DialogueLine.cDL(Puppeteers.steelVulturePocket, "We'll take the collectin' of the drone bits over from 'ere. We gettin' payed good money."));
			list.Add(DialogueLine.cDL(Puppeteers.steelVulturePocket, "You can leave the schematics with me, I'll give 'em to someone who can read."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "So what am I supposed to do?"));
			list.Add(DialogueLine.cDL(Puppeteers.steelVulturePocket, "I'm not the boss. Not tellin' ya how to spend yer time. See ya."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Alright, right..."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "Captain, there is someone hailing us.").WithTrigger(delegate
			{
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.characters.Add("SmugglerContact");
				spaceStation.characters.Remove("SteelVulturePocket");
			}));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "You there, incoming ship."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Talk to me when you land, I'll be waiting in the airlock."));
			return list;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0002C514 File Offset: 0x0002A714
		public static List<DialogueLine> SmugglerContact_Default()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, we have to wait until a game update releases for this mission to continue.")
			};
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0002C530 File Offset: 0x0002A730
		public static List<DialogueLine> Umbral10Smuggler()
		{
			SectorMapData sectorMapData = GalaxyMapData.current.allSectors.ElementAt(3);
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Captain, let me introduce myself..."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, string.Concat(new string[]
			{
				"I'm ",
				Puppeteers.smugglerContact.name,
				", you could call me a member of the ",
				Translation.Translate(Faction.smugglers.name, Array.Empty<object>()),
				"."
			})));
			list.Add(DialogueLine.cDL(Puppeteers.captain, Puppeteers.captain.name + ", pleased to meet you."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "I've got a job for you in the next sector."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Since I can't really leave here, we'll fork over some good credits."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "I like credits."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Who doesn't right..."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Anyways, I lost contact with an associate of mine, and I want you to do his job."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Go to one of our stations in Sector " + sectorMapData.name + "."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "...one second, Captain."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Sure..."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Some pirates are attacking one of our outposts, can you rush over there and help them out?"));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Afterwards go to one of our stations, and they'll give you more info about our missing associate.").WithTrigger(delegate
			{
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.characters.Remove("SmugglerContact");
				SpaceStationInterior instance = SpaceStationInterior.instance;
				instance.SetupCharacters(spaceStation.characters);
				instance.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			return list;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0002C6EC File Offset: 0x0002A8EC
		public static List<DialogueLine> Umbral11Smuggler()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.smugglerSector4, "Captain " + Puppeteers.captain.name + ", thank you!"),
				DialogueLine.cDL(Puppeteers.smugglerSector4, "Wouldn't have our outpost anymore if it wasn't for you."),
				DialogueLine.cDL(Puppeteers.smugglerSector4, "Let's get you your next mission. The one that pays quite a bit."),
				DialogueLine.cDL(Puppeteers.captain, "Now we're talking!"),
				DialogueLine.cDL(Puppeteers.smugglerSector4, "We have a " + Translation.Translate(Faction.blue.name, Array.Empty<object>()) + " spy who needs help."),
				DialogueLine.cDL(Puppeteers.smugglerSector4, "Just some... regular cargo we need you to bring over there."),
				DialogueLine.cDL(Puppeteers.smugglerSector4, "Nothing too fancy, just don't go snooping around the cargo.")
			};
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0002C7C4 File Offset: 0x0002A9C4
		public static List<DialogueLine> Umbral12Stellar()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Hey... What's the secret passphrase?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Oh, come on!"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Correct."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "What?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Good thing you knew, I was about to terminate you."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Me too... I have brought some \"items\" with me, I'll unload them and leave them in your care."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Sure, you brought some construction materials for " + Translation.Translate(Faction.blue.name, Array.Empty<object>()) + " I see, with some wares stuck in between..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Captain Smuggler!"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Wooo.. not so loud. I mean, if it pays it pays, right?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "What are the construction materials for?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, Translation.Translate(Faction.blue.name, Array.Empty<object>()) + " and the other major factions are constructing a new jumpgate into the Darkspace."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "What happened to the old one?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "It got destroyed by the " + Translation.Translate(Faction.darkspacers.name, Array.Empty<object>()) + ". They don't need that gate, they discovered or created wormholes to enter these sectors."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "How do you know this?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "We do quite a bit of trading with them. So, from time to time, they grant us access through their wormholes."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "What do you trade with them?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "New tech, blueprints, faction weaponry, high-grade crafting materials. You know, the normal stuff."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "And what do you get in return?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Ha, good question...."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "We have a new job for you, helping with some more important cargo now."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Go to the provided waypoint and talk to your contact. I'll provide the contact details. Goodbye, Captain.").WithTrigger(delegate
			{
				SpaceStation spaceStation = (SpaceStation)GamePlayer.current.currentPointOfInterest;
				spaceStation.characters.Remove("StellarStagingPoint");
				SpaceStationInterior instance = SpaceStationInterior.instance;
				instance.SetupCharacters(spaceStation.characters);
				instance.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			return list;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0002CA08 File Offset: 0x0002AC08
		public static List<DialogueLine> Umbral13Smuggler()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.smugglerContact, "Captain, you have arrived."),
				DialogueLine.cDL(Puppeteers.captain, Puppeteers.smugglerContact.name + "! Thought you couldn't travel around and was stuck at that station."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "Well, I can, but I just can't be that visible, so I have to take my time."),
				DialogueLine.cDL(Puppeteers.captain, "So you are here to give me another job, right?"),
				DialogueLine.cDL(Puppeteers.smugglerContact, "I am, " + Puppeteers.captain.name + "... We have a shipment ready, but we fear the plans have been leaked to some pirates."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "We would appreciate your assistance, escorting one of our cargo vessels."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "We think the pirates are waiting at the cargo depot. Hurry over there!")
			};
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0002CAD0 File Offset: 0x0002ACD0
		public static List<DialogueLine> Umbral14SmugglerComplete()
		{
			GalaxyMapData.current.allSectors.ElementAt(3).FindStationForFaction(Faction.smugglers, null).characters.Remove("SmugglerSector4");
			SpaceStation.current.characters.Add("SmugglerSector4");
			SpaceStationInterior instance = SpaceStationInterior.instance;
			instance.SetupCharacters(SpaceStation.current.characters);
			instance.GoToLocation(SpaceStationFacility.Airlock, true);
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "Well done, Captain."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Claude, fancy meeting you here."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "What do you mean well done, Claude?"));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, Puppeteers.smugglerSector4.name + ", we opened the boxes but there was no metafi..."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "No, James... Ah, I guess you've earned our trust, Captain."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "You were protecting a shipment full of Metafiber, at least that should have been in there."));
			list.Add(DialogueLine.cDL(Puppeteers.shipAi, "Metafiber, that's not a substance known to me Captain."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "... metafiber? What would that be?"));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "Well, that would require more time to explain. Right now, we need to get after the missing shipment."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, Puppeteers.stellarStagingPoint.name + " will have told you about the " + Translation.Translate(Faction.darkspacers.name, Array.Empty<object>()) + ", correct?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "She mentioned their name. That's all."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "I see.. well you're going to meet them."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "They are our suppliers, so our best bet is to see what they know."));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "Head over to one of their hideouts, and ask where the shipment is."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, Puppeteers.smugglerSector4.name + ", are we in any danger going there?"));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "I know what you are thinking, and yes that's possible... but you are more than capable from what we've seen."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Is there at least a secret passphrase I should know of?"));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerContact, "Passphrase...? Yes, you should use \"Hello, I'm looking for the missing Metafiber shipment we paid you for!!\" "));
			list.Add(DialogueLine.cDL(Puppeteers.smugglerSector4, "James, relax. Captain, I believe you'll manage, bring the shipment back here to James.").WithTrigger(delegate
			{
				SpaceStation.current.characters.Remove("SmugglerSector4");
				SpaceStationInterior instance2 = SpaceStationInterior.instance;
				instance2.SetupCharacters(SpaceStation.current.characters);
				instance2.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			return list;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0002CD3F File Offset: 0x0002AF3F
		public static List<DialogueLine> Umbral14SmugglerFailed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.smugglerContact, "This didn't go as planned Captain, luckily that was one of the decoy freighters."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "The real one is coming in now, go there and escort it to the cargo depot.")
			};
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0002CD70 File Offset: 0x0002AF70
		public static List<DialogueLine> Umbral15Darkspacers()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, string.Concat(new string[]
				{
					"Incoming ship, this is ",
					Puppeteers.darkspaceSmuggler1.name,
					" of the ",
					Translation.Translate(Faction.darkspacers.name, Array.Empty<object>()),
					"."
				})),
				DialogueLine.cDL(Puppeteers.captain, "Hello, I'm looking for the missing Metafiber shipment we paid you for!!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "What? Are you... alright?"),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain..."),
				DialogueLine.cDL(Puppeteers.captain, string.Concat(new string[]
				{
					"I... hello there. ",
					Puppeteers.smugglerSector4.name,
					" of the ",
					Translation.Translate(Faction.smugglers.name, Array.Empty<object>()),
					" send me."
				})),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Yup, we were expecting someone."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Listen... someone went rogue within our outfit and we suspect he took the Metafiber."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We will give you his location since we don't need any more enemies."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "It's difficult as is with the current factions knocking at our doors. And not to mention... ah nevermind."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "...so we wouldn't mind you taking out these bastards, you'd gain some goodwill with us as well."),
				DialogueLine.cDL(Puppeteers.captain, "I'll see what I can do... Before I go, can you tell me more about this Metafiber?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You are joking?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You don't know the reason why you are here? And why your galaxy invaded ours?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "To get your hands on Metafiber, to gain control over it."),
				DialogueLine.cDL(Puppeteers.captain, "What, no I'm a freelancer... contracted to aid on a mission, only heard of Metafiber quite recently... And I'm in search for credits? Fame, power?"),
				DialogueLine.cDL(Puppeteers.shipAi, "Loot!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Well, you most definitely are involved, even if you don't know it."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Listen, Metafiber is probably our most valuable commodity, and judging by the return of your galaxy to ours, it's also yours."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Metafiber is mainly used in cybernetics, which in turn prolongs our lives quite significantly."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "It has more uses, but I won't get into it here."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Go to the location I just send you, and best of luck, milky.")
			};
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0002CFC4 File Offset: 0x0002B1C4
		public static List<DialogueLine> Umbral16Smuggler()
		{
			SectorMapData sectorMapData = GalaxyMapData.current.allSectors.ElementAt(6);
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.smugglerContact, "Captain, you are quite something."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "How'd you get all that Metafiber back?"),
				DialogueLine.cDL(Puppeteers.captain, "Well, I just followed the mission objectives."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "That makes sense... You did bring 1 too many Metafiber boxes with you."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "As a gesture of good will we'll let you keep that one."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "That's worth a lot more then some measly credits, just so you know."),
				DialogueLine.cDL(Puppeteers.captain, "Thank you " + Puppeteers.smugglerContact.name + "."),
				DialogueLine.cDL(Puppeteers.smugglerContact, Puppeteers.darkspaceSmuggler1.name + ", the Darkspacer you met, asked if you could meet them again over in Sector " + sectorMapData.name),
				DialogueLine.cDL(Puppeteers.smugglerContact, "Guessing they rather keep you as a friend, then an enemy."),
				DialogueLine.cDL(Puppeteers.captain, "That's good to hear, I can inquire more about this Metafiber."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "Captain, just some advice... don't go telling everyone you have it, it's highly illegal and highly sought after."),
				DialogueLine.cDL(Puppeteers.smugglerContact, "Take care now, thanks again."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, we received a message from our mysterious employer. I'll play it back."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Captain " + Puppeteers.captain.name + ", the constructor fleet is about to be finished. Meet us at the location to be briefed on our next steps."),
				DialogueLine.cDL(Puppeteers.shipAi, "I've added the location to your mission overview. You can choose which mission we will do first.")
			};
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0002D160 File Offset: 0x0002B360
		public static List<DialogueLine> Umbral17Completed()
		{
			GalaxyMapData.current.allSectors.ElementAt(6);
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralContact, "Captain, good of you to show up."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Something tells me you found something of value."),
				DialogueLine.cDL(Puppeteers.captain, "How'd you know?"),
				DialogueLine.cDL(Puppeteers.umbralContact, "It's our job to know, Captain..."),
				DialogueLine.cDL(Puppeteers.umbralContact, "And whenever Metafiber comes into play, we work overtime."),
				DialogueLine.cDL(Puppeteers.umbralContact, "We have been trying to get our hands on it since we got here."),
				DialogueLine.cDL(Puppeteers.umbralContact, "We want to study it, run some experiments on it."),
				DialogueLine.cDL(Puppeteers.captain, "You don't know what it is? It's a sort of bridge between flesh and machine, right?"),
				DialogueLine.cDL(Puppeteers.umbralContact, "Well, yes.. But it's a lot more, we suspect."),
				DialogueLine.cDL(Puppeteers.umbralContact, "I want you to craft a specialized chamber to study this Metafiber."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Don't worry, we'll only use a small portion of it, it'll stay yours."),
				DialogueLine.cDL(Puppeteers.umbralContact, "Then bring the item to the location attached in the mission.")
			};
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0002D280 File Offset: 0x0002B480
		public static List<DialogueLine> Umbral18Stellar()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.captain, "It's you again!"),
				DialogueLine.cDL(Puppeteers.stellarStagingPoint, "We meet again, " + Puppeteers.captain.name + "."),
				DialogueLine.cDL(Puppeteers.captain, "You're not with the " + Translation.Translate(Faction.smugglers.name, Array.Empty<object>()) + " anymore?"),
				DialogueLine.cDL(Puppeteers.stellarStagingPoint, "I'm with anyone who pays well."),
				DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Start unloading the Chamber, and we'll install it here."),
				DialogueLine.cDL(Puppeteers.stellarStagingPoint, "I'll take a small sample of the Metafiber as well..."),
				DialogueLine.cDL(Puppeteers.captain, "What happens now?"),
				DialogueLine.cDL(Puppeteers.stellarStagingPoint, "They'll run some tests, meanwhile I've been told you need to meet our employer again.")
			};
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0002D36C File Offset: 0x0002B56C
		public static List<DialogueLine> Umbral19Completed()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.umbralContact, "Captain, I have to say.. We are very pleased with your help."));
			list.Add(DialogueLine.cDL(Puppeteers.umbralContact, "Even though this work is still somewhat within your contract."));
			list.Add(DialogueLine.cDL(Puppeteers.umbralContact, "It's time to fully entrust you with who we are, and what we are doing here."));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "We are the Umbral Reach.").WithTrigger(delegate
			{
				Register.SetFlag("PuppeteersNameChange", true);
			}));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "From now on, you can refer to me as just " + Puppeteers.realUmbralContact.name + "."));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "So " + Puppeteers.captain.name + ", it is time for us to establish a foothold from which we can grow, don't you think?"));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Well, I didn't go through all this trouble to stop here."));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "So... Remember that Command Cortex you got back from the " + Translation.Translate(Faction.salvageGuild.name, Array.Empty<object>()) + "?"));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "Bring it to " + Puppeteers.stellarStagingPoint.name + ", she already set the plan in motion to use it."));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "The Command Cortex will take over the station's AI, which will give us total command."));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "No hostilities should arise from this, " + Translation.Translate(Faction.blue.name, Array.Empty<object>()) + " won't know it immediatly."));
			list.Add(DialogueLine.cDL(Puppeteers.realUmbralContact, "We do not want to make any enemies at this stage."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "Alright " + Puppeteers.realUmbralContact.name + ", I'm heading over there now."));
			return list;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0002D540 File Offset: 0x0002B740
		public static List<DialogueLine> Umbral20Completed()
		{
			List<DialogueLine> list = new List<DialogueLine>();
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Captain, I've already ensured they won't detect anything."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "What about the station crew?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "They've already switched allegiances, seeing as " + Translation.Translate(Faction.puppeteers.name, Array.Empty<object>()) + " pays quite a bit more."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Activating the Command Cortex."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "5%... 15%..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "25%..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Everything is nominal..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "60%..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "90%..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "99%..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "99%... uh..."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "100%... Done!"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Looks like it worked, quite effectively as well."));
			list.Add(DialogueLine.cDL(Puppeteers.captain, "So that's it?"));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "Yes, that's it. The Command Cortex has it's energy fully depleted."));
			list.Add(DialogueLine.cDL(Puppeteers.stellarStagingPoint, "I'm setting up a beacon here so you can contact " + Puppeteers.realUmbralContact.name + " in this station.").WithTrigger(delegate
			{
				SpaceStation.current.characters.Add("RealUmbral");
				SpaceStationInterior instance = SpaceStationInterior.instance;
				instance.SetupCharacters(SpaceStation.current.characters);
				instance.GoToLocation(SpaceStationFacility.Airlock, true);
			}));
			return list;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0002D6F8 File Offset: 0x0002B8F8
		public static List<DialogueLine> Umbral21Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Captain, we meet not in the cold of space but within our new station."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Let's tell you about the experiments we ran."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Some most disturbing but yet very exciting finds."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "I won't bore you with all the details, but this underlined the importance of pushing further."),
				DialogueLine.cDL(Puppeteers.shipAi, "He really hasn't given you any details, Captain."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "I've already ordered the construction fleet to move out. They are moving towards a system to construct a gate into the Darkspace."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "We are however getting reports of drones being wiped out halting our efforts."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Not your mere pirates, this seems to be a different faction altogether."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "I need you to assist the system in their preparations.")
			};
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0002D7C7 File Offset: 0x0002B9C7
		public static List<DialogueLine> Umbral22Failed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "The supplies were lost under your watch, Captain."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "A fresh batch is on its way. This time, don't give our enemies a reason to celebrate.")
			};
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0002D7F8 File Offset: 0x0002B9F8
		public static List<DialogueLine> Umbral22Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.shipAi, "Incoming message, Captain."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Captain, good job."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "These Meridia's Chosen, we've tried contacting them, offering them good credits, but to no avail."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "They seem true fanatics, not easily swayed."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Won't be the last of them, I'm afraid."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "However... the system is now secure enough for us to bring in the main construction force and start constructing the gate."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "We still need to find a appropriate spot. We need you to go ahead and clear a potential location."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "It is full with asteroids and debris."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Hostile forces are expected."),
				DialogueLine.cDL(Puppeteers.shipAi, "Message ended.")
			};
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0002D8DC File Offset: 0x0002BADC
		public static List<DialogueLine> Umbral23Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, " + Puppeteers.realUmbralContact.name + " is hailing us."),
				DialogueLine.cDL(Puppeteers.captain, "Put it through, " + Puppeteers.shipAi.name + "."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Great job, Captain " + Puppeteers.captain.name + "."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Your efforts have made it so we can finally start the construction of the gate."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "And your work has come to an end, as stated in the contract."),
				DialogueLine.cDL(Puppeteers.captain, "This was all stated in the contract?!"),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Yes..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Sort of..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "...Anyways, the construction will take some time, but I look forward to your aid when we enter the Darkspace."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "We'll have to write up a new contract, of course."),
				DialogueLine.cDL(Puppeteers.shipAi, "I suggest negotiating a better deal on our part, more loot for instance."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "In the mean time, upgrade your ships and crew for the venture into the Darkspace."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "The upcoming conflict will not be for the weak..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Oh, and let's not forget your credit payment."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Until your next employment, Captain."),
				DialogueLine.cDL(Puppeteers.captain, "See you next time, " + Puppeteers.realUmbralContact.name + ".")
			};
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0002DA90 File Offset: 0x0002BC90
		public static List<DialogueLine> DarkspaceMission1()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Milky, you are here."),
				DialogueLine.cDL(Puppeteers.captain, "I am..."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "May it be coincidence or not, but your assault against the traitors seemed to have awoken a larger presence of the fanatics."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Meridia's Chosen they call themselfs. What a joke."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Complete filth who are seeking to corrupt, like they did with the traitors."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "They are numerous, growing every cycle, converting more and more warriors for the upcoming conflict."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "They do not seek to use Metafiber like we do, only to corrupt it."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "..."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "One of their warbands is trying to set up a stronghold."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I would like to use your power to help us destroy them."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "It would mean a great deal to us if you succeed."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, it seems like we should stay out of their way, instead of going towards them."),
				DialogueLine.cDL(Puppeteers.captain, Puppeteers.shipAi.name + ", but what about the loot?"),
				DialogueLine.cDL(Puppeteers.shipAi, "You have convinced me."),
				DialogueLine.cDL(Puppeteers.captain, Puppeteers.darkspaceSmuggler1.name + ", I'll go there immediatly."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "The jumpgate has already been unlocked by my forces."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I strongly advise you not to attack their station directly."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Weaken the enemy forces around the system first. It'll reduce their reinforcements when you strike the station."),
				DialogueLine.cDL(Puppeteers.captain, "Noted, off I go.")
			};
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0002DC50 File Offset: 0x0002BE50
		public static List<DialogueLine> DarkspaceMission2()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, we are receiving a message."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Well done milky, we destroyed their advances in this sector."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "It will have slowed them down, but nothing more."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "They are an inevitable plague upon our sectors."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Let's meet up at the following coordinates for our next course of action.")
			};
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0002DCCC File Offset: 0x0002BECC
		public static List<DialogueLine> DarkspaceMission3()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You arrived."),
				DialogueLine.cDL(Puppeteers.shipAi, "We know that."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "..."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Meridia's Chosen, may the meteors of Uron rain upon them, are not our only problem."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Your factions are knocking at our sectors. They are building their own gate, since we destroyed the old ones."),
				DialogueLine.cDL(Puppeteers.captain, "So wouldn't it be time to retreat to your Darkspace, bolster defenses?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You do not understand. Our armadas are massive, enough to wipe out every warship in this quadrant."),
				DialogueLine.cDL(Puppeteers.captain, "Then why hasn't that happened?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Sadly, they've been distracted, fighting on another front."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "So... we need to sabotage this construction as much as we can."),
				DialogueLine.cDL(Puppeteers.captain, "I see " + Puppeteers.darkspaceSmuggler1.name + ". And what do you need me for?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "They require massive amounts of construction materials for their construction."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, maybe don't mention you helped with hauling construction materials."),
				DialogueLine.cDL(Puppeteers.captain, "I see, " + Puppeteers.darkspaceSmuggler1.name + "... interesting... interesting..."),
				DialogueLine.cDL(Puppeteers.shipAi, "Nice."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "... Right. We've found a location for one of their cargo depots, it appears to be the least guarded."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Escort one of our ships to the depot. We'll give them a little electromagnetic surprise."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We'll wait until you are clear of the zone, you've earned that.")
			};
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0002DE80 File Offset: 0x0002C080
		public static List<DialogueLine> DarkspaceMission4Failed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You failed, unusual."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Let us try again.")
			};
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0002DEB1 File Offset: 0x0002C0B1
		public static List<DialogueLine> DarkspaceMission4Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.shipAi, "Now, get out of here! Back to " + Puppeteers.darkspaceSmuggler1.name + ".")
			};
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0002DEE4 File Offset: 0x0002C0E4
		public static List<DialogueLine> DarkspaceMission5Completed()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "The bomb shut down their construction, temporarily."),
				DialogueLine.cDL(Puppeteers.shipAi, "There seem to be a lot of temporarily solutions lately."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Our focus on the invading factions also did us harm."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Meridia's Filth has taken over our wormhole entry point into this quadrant."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I've gathered the remaining forces to repel them, and I would like your strength added to them."),
				DialogueLine.cDL(Puppeteers.captain, "You can count on me, " + Puppeteers.darkspaceSmuggler1.name),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Once you enter the location, we'll warp in."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Let us fight."),
				DialogueLine.cDL(Puppeteers.shipAi, "LET US FIGHT!")
			};
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		public static List<DialogueLine> DarkspaceMission6Complete()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Corrupted beings, they are destroyed."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "But more will take their place soon."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Thank you, Captain."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I don't know how, but the filth collapsed the wormhole. It will take us time before we can reestablish a new connection with our home."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I will contact you when that time arises."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I feel like a reward is in place."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Take this learning implant."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Save warp travels, Captain."),
				DialogueLine.cDL(Puppeteers.captain, "Till we meet again, " + Puppeteers.darkspaceSmuggler1.name + ".")
			};
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0002E0A8 File Offset: 0x0002C2A8
		public static List<DialogueLine> UmbralConquest1()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Captain. You are finally here."),
				DialogueLine.cDL(Puppeteers.captain, "What is here?"),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "We set up this small outpost, to act as a base of operations."),
				DialogueLine.cDL(Puppeteers.captain, "So how's things going?"),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "... Everything is going fine, we have expanded our Metafiber experiments."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "New opportunities have arrived with the new war."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "This is where we need you."),
				DialogueLine.cDL(Puppeteers.captain, "Of course."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "The stations within the Conquest Sector were assembled rather crudely. Little attention was given to security."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Their software are not getting any security updates."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "That is where we found our in."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "I will provide you with a specialized infiltration tool. A hacking tool if you will."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Use this on any friendly station within the Conquest Sector."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "It will infect their systems... and persist, even if they attempt repairs or complete reconstruction."),
				DialogueLine.cDL(Puppeteers.shipAi, "How?"),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "How? Probably the cloud, I don't know... Not that important."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "It's all safe for you. You will not suffer any reputation loss with the station you choose, should really only be benefitial."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Once successful, I will make contact."),
				DialogueLine.cDL(Puppeteers.captain, "On it!")
			};
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0002E24C File Offset: 0x0002C44C
		public static List<DialogueLine> UmbralConquest2()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Checking signal..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "One second.. checking..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "All clear. None the wiser."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Let's see... yes..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Alright, we have set up the Umbral Reach board, there you will be able to find a mission which will infect their systems further."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Once we reach full infection in the station, we will seek contact again."),
				DialogueLine.cDL(Puppeteers.shipAi, "Oh! A new button Captain, let's press it.")
			};
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0002E2F4 File Offset: 0x0002C4F4
		public static List<DialogueLine> UmbralConquest3()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Systems are... fully infected..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "That was a lot quicker then anticipated.."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Head back to the outpost, as I heard Stella has arrived there from the Frontier."),
				DialogueLine.cDL(Puppeteers.captain, "Is there a passphrase?"),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "No."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "She will be your liason going forward."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "And she will make a fine agent out of you yet.")
			};
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0002E3B0 File Offset: 0x0002C5B0
		public static List<DialogueLine> UmbralConquest4()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Captain " + Puppeteers.captain.name + ", how are you?"),
				DialogueLine.cDL(Puppeteers.captain, "No."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "No... Ok? Sure... I'm fine thank you."),
				DialogueLine.cDL(Puppeteers.captain, "Stella? You changed."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "I have to fit my new role, you understand."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "I have been tasked overseeing your efforts in this new sector."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "We don't need to take over by force, if we can just control the opposition."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "But sometimes... force is required, the destruction of the enemy! Eliminate them! Vaporize them! Shred them down to their beams!"),
				DialogueLine.cDL(Puppeteers.shipAi, "Other then that, are you alright?"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "... yes... We discovered some wannabe spies closeby, surely trying to gather intel on our new presence here."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Deal with them."),
				DialogueLine.cDL(Puppeteers.captain, "Yes ma'am."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Right, I forgot... make sure you bring one of those Decoy Transponders with you from the shop, if you value your reputation with whoever you will encounter."),
				DialogueLine.cDL(Puppeteers.shipAi, "Interesting. Captain, the spoofed transponder signature will persist only until your next station docking.")
			};
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0002E4FC File Offset: 0x0002C6FC
		public static List<DialogueLine> UmbralConquest4b()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.captain, "We eliminated them, Stella. They were mercenaries of the Omnitac Agency."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "I knew it!"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "It was the Omnitac Agency all along!"),
				DialogueLine.cDL(Puppeteers.shipAi, "An esteemed escort service would probably not operate without someone hiring them."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Right, good point."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "This does not help us any further, we are dealing with someone trying to cover their warp signature."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "We'll have to let our agents infiltrate further and gather information."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "In the meantime, your task is to infect more stations."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Check out the shop if you need more hacking tools."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Return to me once you have reach the target."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, hacking other stations will work the same as you did with the first station, but the conquest station you are trying to infect has to be adjecent to an already infected station's system.")
			};
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0002E5F8 File Offset: 0x0002C7F8
		public static List<DialogueLine> UmbralConquest5()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Our reach is expanding, Captain."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Keep up the good work."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Stella is waiting for you at the outpost. Report to her at once.")
			};
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0002E64C File Offset: 0x0002C84C
		public static List<DialogueLine> UmbralConquest6()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Captain " + Puppeteers.captain.name + "."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Let's get right to it. We don't know anything about who hired the Omnitac mercs, yet."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "What we did discover is the location of several research facilities."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "One for each of the major factions. They are, like us, experimenting on Metafiber."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "We need their data slates, it would speed up our research by quite a bit."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Expect resistance, it should be heavily guarded."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "But, all we need are the data slates, which are most likely in the Core part of the station."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "I could be an quick and easy snatch job, acquire them however you see fit."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Return them to me after acquiring them."),
				DialogueLine.cDL(Puppeteers.shipAi, "Just a friendly reminder captain, Decoy Transponders are available in the Umbral Shops.")
			};
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0002E744 File Offset: 0x0002C944
		public static List<DialogueLine> UmbralConquest7()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralReachStella, "These are valuable. I'll send them to our research station at once."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "There are not many tasks I can give you, other than the task of further expanding our influence."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Get back to me once more stations have been infected.")
			};
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0002E798 File Offset: 0x0002C998
		public static List<DialogueLine> UmbralConquest8()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Captain, you are going at a great pace!"),
				DialogueLine.cDL(Puppeteers.captain, "Can't you hire more personnel? I feel like I'm carrying this team, and I'm the only one in this team."),
				DialogueLine.cDL(Puppeteers.shipAi, "Hey! Rude..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "..."),
				DialogueLine.cDL(Puppeteers.realUmbralContact, "Report to Stella at once.")
			};
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0002E814 File Offset: 0x0002CA14
		public static List<DialogueLine> UmbralConquest9()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Captain."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "We have located another Metafiber Data Slate, or at least know of it."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Our agents gathered it will be transported soon."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "By a Void Drifter ship."),
				DialogueLine.cDL(Puppeteers.captain, "I thought you were allies with them?"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Allies? With all of them? Do you know how much that would cost?"),
				DialogueLine.cDL(Puppeteers.captain, "Probably a lot of commendations!"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Yes, you would need special Void Drifter Commendations, and those do not exist, thank the oracle."),
				DialogueLine.cDL(Puppeteers.captain, "The oracle?"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "It's just a saying..."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Ahem... so, let's get back to the mission."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Our agents have placed a tracker on the ship, so we know where it is."),
				DialogueLine.cDL(Puppeteers.captain, "Tracker?"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Yes, to track ships? How else will you covertly steal their cargo? They are available in most Umbral Shops. Pairs great with Cargo Scanners."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "But this ship has already been tagged. Go to the location, and eliminate the ship."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Bring back the data, Captain.")
			};
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0002E978 File Offset: 0x0002CB78
		public static List<DialogueLine> UmbralConquest10()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Thank you for delivering the slate, good job as always."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "I'll send it off to get experimented upon."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "This should be the missing piece, we have everything we need."),
				DialogueLine.cDL(Puppeteers.captain, "What are you trying to achieve, anyways?"),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "That would be not for me to spill, Captain..."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Let's distract you with a reward."),
				DialogueLine.cDL(Puppeteers.captain, "I am distracted."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "There is something waiting for you in your hangar."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "It's an old design. A stripped down Eclipse, high travel speed, decent turrets but... somewhat lower protection values."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Even has a small dronebay. You know, for drones."),
				DialogueLine.cDL(Puppeteers.umbralReachStella, "Get back to infecting stations, Captain."),
				DialogueLine.cDL(Puppeteers.captain, "First, I'm checking out the hangar!")
			};
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0002EA88 File Offset: 0x0002CC88
		public static List<DialogueLine> Stella_Default(Character character)
		{
			bool flag = false;
			using (List<Mission>.Enumerator enumerator = GamePlayer.current.missions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.storyId == "ConquestUmbral10")
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Puppeteers.shipAi, "We will have to wait for a game update to progress this mission, Captain.")
				};
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.captain, "Stella?"),
				DialogueLine.cDL(character, "Ah!! You startled me. Don't sneak up on me like that."),
				DialogueLine.cDL(Puppeteers.captain, "Isn't that what I'm supposed to do, being all sneaky like?"),
				DialogueLine.cDL(character, "Yes, but don't try it out on me! Get going!")
			};
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0002EB60 File Offset: 0x0002CD60
		public static List<DialogueLine> DarkspaceConquest1()
		{
			List<DialogueLine> list = new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Captain!"),
				DialogueLine.cDL(Puppeteers.captain, "Midas? Nice ship."),
				DialogueLine.cDL(Puppeteers.shipAi, "It is a Terravex-class Destroyer, captain. Shiny!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Thank you. As you may have spotted, Meridia has upped its efforts, expanding further and further."),
				DialogueLine.cDL(Puppeteers.captain, "Yes yes, I need to halt their expansion, and I will get rewarded. Right?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Correct."),
				DialogueLine.cDL(Puppeteers.captain, "Good enough for me."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Even if they don't hold any territory, these filths endlessly try to gather more souls for their overlord."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We will send you on a small campaign, disrupting them the best we can."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "So... your task... we know where some prominent political figures are being transported. If we can terminate them, we deal a great blow to their propaganda machine."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "They will be traveling to an outpost. Destroy it! Their departure point is logged at the station they are inbound from."),
				DialogueLine.cDL(Puppeteers.captain, "Will I be getting assistance?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I'll see what forces I can spare. But you seem quite capable."),
				DialogueLine.cDL(Puppeteers.shipAi, "Always the same story.")
			};
			if (Faction.darkspacers.GetReputation(Faction.player) < ReputationLevel.Wary.GetReputationThreshold())
			{
				list.Add(DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Oh, just so you know... your reputation with the Compact here is not important to me. I'd rather you fix it, but my task is the protection of the core systems from the threat out of Meridia."));
				list.Add(DialogueLine.cDL(Puppeteers.captain, "Yes, well... sorry about that."));
				list.Add(DialogueLine.cDL(Puppeteers.shipAi, "Not really sorry."));
			}
			return list;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0002ECF0 File Offset: 0x0002CEF0
		public static List<DialogueLine> DarkspaceConquest2()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "That's it! Freeze in the nothingness you cowards!"),
				DialogueLine.cDL(Puppeteers.shipAi, "Wraaaaaaah!"),
				DialogueLine.cDL(Puppeteers.captain, "What? Yes!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Good job, now get over to the next location and continue your crusade!"),
				DialogueLine.cDL(Puppeteers.captain, "I feel like it's more your crusade... but yes, on it!")
			};
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0002ED6C File Offset: 0x0002CF6C
		public static List<DialogueLine> DarkspaceConquest3()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Finally! The damage those monsters would've done would be immense."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "It dropped some of their discs as well."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I suggest not viewing the discs, just sell them at an Intertrade Trade Terminal."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We want to eliminate more figures within their hierarchy."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "If you could draw them out, by let's say sabotaging their economy... their mining and salvage operations..."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "... that would garner attention from some more elite forces, and dealing with them would hurt their morale quite a bit."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I've marked the location of a mining operation on your map. Your first target. Eliminate the miners."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Could be a quick hit and run, but they could send reinforcements."),
				DialogueLine.cDL(Puppeteers.shipAi, "Captain, amongst the loot I also scanned a beacon... handy, in this situation.")
			};
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0002EE3C File Offset: 0x0002D03C
		public static List<DialogueLine> DarkspaceConquest4()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Good. The miners have been dealt with."),
				DialogueLine.cDL(Puppeteers.shipAi, "Easy targets. Just how I like them."),
				DialogueLine.cDL(Puppeteers.captain, "Hey, that's my line!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Right.. next up is a salvaging operation, same idea."),
				DialogueLine.cDL(Puppeteers.captain, "No explanation needed, going there right away! Let me check the map first... what?! All the way over there...")
			};
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0002EEB8 File Offset: 0x0002D0B8
		public static List<DialogueLine> DarkspaceConquest5()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Yes!"),
				DialogueLine.cDL(Puppeteers.captain, "Easy targets. Just how I like them."),
				DialogueLine.cDL(Puppeteers.shipAi, "Hey, that's my line!"),
				DialogueLine.cDL(Puppeteers.captain, "No it's not, I'll talk to you later about it ECHO!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I... ok... Well, I've picked up a signal that the Meridia commanders have been deployed."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "My forces have managed to put trackers on them. Very convenient for you, Captain."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "A slight warning, these are not ordinary captains, so be prepared for quite a fight."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We've marked the location on your map as to where they can be intercepted."),
				DialogueLine.cDL(Puppeteers.captain, "Got it, we'll go in prepared."),
				DialogueLine.cDL(Puppeteers.shipAi, "Or not, we'll wing it."),
				DialogueLine.cDL(Puppeteers.captain, "ECHO!!")
			};
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0002EFB4 File Offset: 0x0002D1B4
		public static List<DialogueLine> DarkspaceConquest6()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Let me guess... Easy targets. Just how you like them?"),
				DialogueLine.cDL(Puppeteers.captain, "They were definitely not easy!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I'm joking... this is quite the accomplishment. With them gone, it will bring internal chaos. Even more than is standard for them."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You have helped us out greatly."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "I have another request, and it is a big one... it's something which needs to happen before we can proceed further towards Meridia."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "But let's take a breather... you do seem to be entitled to a reward, at this stage."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We can't give it to you completely for free, so let's make it a trade."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We need certain materials. Bring them to the Darkspace Embassy."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Here's the gate pass to our embassy sector."),
				DialogueLine.cDL(Puppeteers.captain, "But what are we trading for? Just materials?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "No, and a ship of course!"),
				DialogueLine.cDL(Puppeteers.shipAi, "Exciting! Captain, Intertrade stations have missions that exchange materials, in case we're missing something specific.")
			};
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0002F0C4 File Offset: 0x0002D2C4
		public static List<DialogueLine> DarkspaceConquest7()
		{
			float currentConqueredPercentage = GamePlayer.current.GetStoryteller<Conquest>().GetFactionStanding(Faction.fanatics).currentConqueredPercentage;
			bool flag = false;
			if (currentConqueredPercentage >= 0f)
			{
				flag = true;
			}
			string text = "I really should check the map more often... It seems you have already eliminated them. Well, the reward stands!";
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Material transfer completed. Have fun, Captain! You'll find the ship in your hangar."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You'll need to install modules and turrets for the coming fight, or well... fights."),
				DialogueLine.cDL(Puppeteers.captain, "Yes, we need to calibrate the targeting of the new ship."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "We will entrust you with the following objective: eliminate Meridia from the conquest sector."),
				DialogueLine.cDL(Puppeteers.captain, "Well, that is quite a bit of calibrating..."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, flag ? text : "We will reward you handsomely, we need to make sure they do not pose a threat here anymore, before we continue."),
				DialogueLine.cDL(Puppeteers.captain, flag ? "Great! I like rewards." : "I understand, I'm on it, Midas!")
			};
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0002F1A4 File Offset: 0x0002D3A4
		public static List<DialogueLine> DarkspaceConquest8()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "You have done it, Captain."),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "It's something which none of us could achieve, but you are a different breed. Not NPC's like us."),
				DialogueLine.cDL(Puppeteers.captain, "Don't say that! ECHO is an NPC and did most of the work!"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "Non Powerful Captain, I meant. What are you on about?"),
				DialogueLine.cDL(Puppeteers.darkspaceSmuggler1, "... give me some time to come up with a new task for you.")
			};
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0002F220 File Offset: 0x0002D420
		public static List<DialogueLine> Midas_Default(Character character)
		{
			bool flag = false;
			using (List<Mission>.Enumerator enumerator = GamePlayer.current.missions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.storyId == "ConquestDarkspace9")
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Puppeteers.shipAi, "We will have to wait for a game update to progress this mission, Captain.")
				};
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Puppeteers.captain, "Midas!"),
				DialogueLine.cDL(character, "Not right now Captain, I'm busy.")
			};
		}

		// Token: 0x040002A3 RID: 675
		public const string UmbralBeacon1 = "UmbralBeacon1";

		// Token: 0x040002A4 RID: 676
		public const string UmbralBeacon2 = "UmbralBeacon2";

		// Token: 0x040002A5 RID: 677
		public const string UmbralBeacon3 = "UmbralBeacon3";

		// Token: 0x040002A6 RID: 678
		public const string UmbralBeacon4 = "UmbralBeacon4";

		// Token: 0x040002A7 RID: 679
		private bool startDialogue;
	}
}
