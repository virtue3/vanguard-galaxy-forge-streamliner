using System;
using System.Collections.Generic;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.Player;

namespace Source.Dialogues.Content
{
	// Token: 0x02000103 RID: 259
	public class SideMissions
	{
		// Token: 0x060009D8 RID: 2520 RVA: 0x0004AF20 File Offset: 0x00049120
		public static Dialogue CreatePatrolDialogue(Character character)
		{
			string mId1 = "SideMissionPatrol";
			if (GamePlayer.current.missionsArchive.Contains(mId1))
			{
				return null;
			}
			Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
			if (mission1 != null && mission1.CanClaimRewards())
			{
				Action _delegate_1;
				return new Dialogue
				{
					dialogues = delegate()
					{
						List<DialogueLine> list = new List<DialogueLine>();
						list.Add(DialogueLine.cDL(character, "Welcome back, recruit. System tells me you completed your patrol, well done!"));
						list.Add(DialogueLine.cDL(Characters.captain, "Man, those pirates weren't kidding. Almost like they were trying to kill me!"));
						DialogueLine dialogueLine = DialogueLine.cDL(character, "Nobody said this would be easy. Luckily for you, that's why the rewards are quite substantial.");
						Action trigger;
						if ((trigger = _delegate_1) == null)
						{
							trigger = (_delegate_1 = delegate()
							{
								GamePlayer.current.CompleteMission(mission1, false);
							});
						}
						list.Add(dialogueLine.WithTrigger(trigger));
						list.Add(DialogueLine.cDL(Characters.captain, "Thanks! That almost makes all this hassle worth it."));
						list.Add(DialogueLine.cDL(character, "That's what you say now. Soon, you'll be back here day after day, measuring yourself against the most hardy foes this Galaxy has to offer."));
						list.Add(DialogueLine.cDL(Characters.captain, "Great, I can't wait..."));
						return list;
					}
				};
			}
			if (mission1 != null)
			{
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "How's your patrol coming along, recruit?"),
						DialogueLine.cDL(Characters.captain, "Working on it.")
					})
				};
			}
			return new Dialogue
			{
				dialogues = (() => new List<DialogueLine>
				{
					DialogueLine.cDL(character, "Welcome to Canisec, your most trustworthy defense partner in the Galaxy. What can I do for you?"),
					DialogueLine.cDL(Characters.captain, "Hey, I heard you were recruiting contractors to conduct patrols of the sector. What's that all about?"),
					DialogueLine.cDL(character, "Simple, you go to any point of interest that we post on the patrol board, and eliminate any hostile forces that you encounter. Then you collect your reward, rinse, and repeat. Interested?"),
					DialogueLine.cDL(Characters.captain, "Sure, sign me up!"),
					DialogueLine.cDL(character, "Excellent. We do have a one-time sign up bonus for new recruits, so check out our offer in your mission log. See you back soon!")
				}),
				onComplete = delegate()
				{
					GamePlayer.current.AddMissionWithLog(mId1);
				}
			};
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0004AFEC File Offset: 0x000491EC
		public static Dialogue CreateBountyDialogue(Character character)
		{
			string mId1 = "SideMissionBounty";
			if (GamePlayer.current.missionsArchive.Contains(mId1))
			{
				return null;
			}
			Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
			if (mission1 != null && mission1.CanClaimRewards())
			{
				Action _delegate_1;
				return new Dialogue
				{
					dialogues = delegate()
					{
						List<DialogueLine> list = new List<DialogueLine>();
						list.Add(DialogueLine.cDL(character, "You did it! One less piece of pirate scum polluting our galaxy."));
						list.Add(DialogueLine.cDL(Characters.captain, "That was quite the challenge! Pirates sure aren't messing around in these parts. Now where's my reward?"));
						DialogueLine dialogueLine = DialogueLine.cDL(character, "Here you go, well deserved. Get some rest, and come back any time you're in the mood for more pirate hunting!");
						Action trigger;
						if ((trigger = _delegate_1) == null)
						{
							trigger = (_delegate_1 = delegate()
							{
								GamePlayer.current.CompleteMission(mission1, false);
							});
						}
						list.Add(dialogueLine.WithTrigger(trigger));
						list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
						return list;
					}
				};
			}
			if (mission1 != null)
			{
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Hey, how's your hunt coming along?"),
						DialogueLine.cDL(Characters.captain, "Working on it.")
					})
				};
			}
			return new Dialogue
			{
				dialogues = (() => new List<DialogueLine>
				{
					DialogueLine.cDL(character, "Hey, big shot. You look ready for a real challenge."),
					DialogueLine.cDL(Characters.captain, "You're talking to me?"),
					DialogueLine.cDL(character, "Of course! I can spot a real pirate hunter from miles away. And you're in luck, there's plenty of pirate scum for all of us to fight in this sector."),
					DialogueLine.cDL(Characters.captain, "Pirates do seem to be popping up all over the place. Was it always this bad? And what did they do to deserve this treatment?"),
					DialogueLine.cDL(character, "Bah, irrelevant. All you need to know is that there's a bounty on their head, and they probably did something to deserve that. Less talking, more hunting. And there's a big reward in it for you."),
					DialogueLine.cDL(Characters.captain, "Now you're talking my language!")
				}),
				onComplete = delegate()
				{
					GamePlayer.current.AddMissionWithLog(mId1);
				}
			};
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0004B0B8 File Offset: 0x000492B8
		public static Dialogue CreateFastLaneDialogue(Character character)
		{
			MissionObjective.Trigger(MissionTrigger.FastTravelTalkToNPC, null, null, false);
			string mId1 = "SideMissionFastLane";
			if (GamePlayer.current.missionsArchive.Contains(mId1) && GamePlayer.current.fastLaneTravelUnlocked)
			{
				return null;
			}
			Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
			if (mission1 != null && mission1.CanClaimRewards())
			{
				Action _delegate_1;
				return new Dialogue
				{
					dialogues = delegate()
					{
						List<DialogueLine> list = new List<DialogueLine>();
						list.Add(DialogueLine.cDL(character, "Well, that took you long enough. After you left I had a shower, read the news while enjoying two coffees and juggled my goldfish for a bit of fun. And then I traveled this way as well. Just to prove a point, I guess."));
						list.Add(DialogueLine.cDL(Characters.captain, "Ehhh, how...? Are the goldfish ok?"));
						list.Add(DialogueLine.cDL(character, "Fast Lane travel, my friend. You have to see it to believe it. And yeah, they're fine."));
						list.Add(DialogueLine.cDL(Characters.captain, "Very impressive, how does it work?"));
						list.Add(DialogueLine.cDL(character, "Well, you pick up the aquarium, and... Oh wait, you mean the travel."));
						DialogueLine dialogueLine = DialogueLine.cDL(character, "The technical details are secret. But, for your effort, we'll give you exclusive access. Happy jumping!");
						Action trigger;
						if ((trigger = _delegate_1) == null)
						{
							trigger = (_delegate_1 = delegate()
							{
								GamePlayer.current.CompleteMission(mission1, false);
							});
						}
						list.Add(dialogueLine.WithTrigger(trigger));
						list.Add(DialogueLine.cDL(Characters.shipAi, "Upgrade installed, Captain. Oh wow..."));
						list.Add(DialogueLine.cDL(Characters.captain, "Ok cool, thanks. I guess we'll go try it right now.").WithTrigger(delegate
						{
							GamePlayer.current.SetFastLaneTravelUnlocked(true);
							SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
							if (spaceStation == null)
							{
								return;
							}
							spaceStation.characters.Remove("QuestgiverFastLaneTravel");
						}));
						return list;
					}
				};
			}
			if (mission1 != null)
			{
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Hey, still here? Those goods don't transport themselves you know."),
						DialogueLine.cDL(Characters.shipAi, "Indeed, that is usually how it works.")
					})
				};
			}
			return new Dialogue
			{
				dialogues = (() => new List<DialogueLine>
				{
					DialogueLine.cDL(character, "Oh hey, you looking for work?"),
					DialogueLine.cDL(Characters.captain, "Well that depends, what kind of work?"),
					DialogueLine.cDL(character, "Just a little bit of trading, with a very interesting reward if I say so myself."),
					DialogueLine.cDL(Characters.captain, "Ok, sounds compelling, tell me more!"),
					DialogueLine.cDL(character, "Let's not spoil it right away shall we? Let's just say that Intertrade Network has access some special Jump Gate tech."),
					DialogueLine.cDL(Characters.captain, "Jump Gate tech you say? Ok I'm in."),
					DialogueLine.cDL(character, "Great, deliver the goods to the indicated space station and we'll see about that reward.")
				}),
				onComplete = delegate()
				{
					GamePlayer.current.AddMissionWithLog(mId1);
				}
			};
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0004B198 File Offset: 0x00049398
		public static Dialogue CreateSkilltreeUnlockTier2Dialogue(Character character)
		{
			string mId1 = "SkilltreeUnlockTier2";
			if (GamePlayer.current.missionsArchive.Contains(mId1))
			{
				return null;
			}
			Mission activeStoryMission = GamePlayer.current.GetActiveStoryMission(mId1);
			bool flag = false;
			if (activeStoryMission != null)
			{
				foreach (MissionObjective missionObjective in activeStoryMission.currentStep.objectives)
				{
					TriggerObjective triggerObjective = missionObjective as TriggerObjective;
					if (triggerObjective != null && triggerObjective.trigger == MissionTrigger.SkillTier2TalkToNPC)
					{
						flag = true;
					}
				}
			}
			if (activeStoryMission != null && flag)
			{
				return new Dialogue
				{
					dialogues = delegate()
					{
						List<DialogueLine> list = new List<DialogueLine>();
						list.Add(DialogueLine.cDL(character, "You are back! I have prepared everything!"));
						list.Add(DialogueLine.cDL(character, "Did they look at you funny? Walking away with these items?"));
						list.Add(DialogueLine.cDL(Characters.captain, "Well, I think it just got transported to my ship, so I didn't see anyone."));
						list.Add(DialogueLine.cDL(character, "Yes, but what would your crew think of you hah!"));
						list.Add(DialogueLine.cDL(Characters.captain, "Well, they know we are doing this for you, so..."));
						list.Add(DialogueLine.cDL(character, "Yes, well sure... fine... payment time, the agreed upon amount."));
						list.Add(DialogueLine.cDL(Characters.captain, "The amount being one, and payment being you upgrading my learning implant?"));
						list.Add(DialogueLine.cDL(character, "Yes, yes! I will do this now."));
						list.Add(DialogueLine.cDL(Characters.captain, "Go ahead... Should I lie down?"));
						list.Add(DialogueLine.cDL(character, "No... hmmm..."));
						list.Add(DialogueLine.cDL(character, "Done! Humans are just so easy to work with."));
						list.Add(DialogueLine.cDL(Characters.captain, "That... ok, I feel like I could have done that."));
						list.Add(DialogueLine.cDL(character, "Probably, very easy... Now, please... Go! I will work on my project, yes!"));
						list.Add(DialogueLine.cDL(Characters.captain, "Thank you, I hope you succeed... I think?"));
						list.Add(DialogueLine.cDL(Characters.shipAi, "Oh what are wonderful person."));
						list.Add(DialogueLine.cDL(Characters.captain, "I would not say that, but I'm happy my learning implant got upgraded.").WithTrigger(delegate
						{
							GamePlayer.current.SetSkilltreeTier2Unlocked(true);
							SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
							if (spaceStation != null)
							{
								spaceStation.characters.Remove("QuestgiverSkilltreeUnlockTier2");
							}
							MissionObjective.Trigger(MissionTrigger.SkillTier2TalkToNPC, null, null, false);
						}));
						return list;
					}
				};
			}
			if (activeStoryMission != null)
			{
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "What? I was not doing anything."),
						DialogueLine.cDL(Characters.captain, "I, just... where do I get these items again?"),
						DialogueLine.cDL(character, "In-ter-trade! When you land, find one of their Trade Terminals and procure the items.")
					})
				};
			}
			if (GamePlayer.current.level > 15)
			{
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Yes! Oh look at you. Hey there..."),
						DialogueLine.cDL(character, "Poor Captain, hello? Hello! How are you?"),
						DialogueLine.cDL(Characters.captain, "Uh... Hello? I... am fine."),
						DialogueLine.cDL(character, "It... You... Oh, you I can fix!"),
						DialogueLine.cDL(Characters.captain, "I need fixing?"),
						DialogueLine.cDL(character, "You poor... Yes! Upgrading you need!"),
						DialogueLine.cDL(character, "I will enhance your learning capabilities."),
						DialogueLine.cDL(Characters.captain, "For free?"),
						DialogueLine.cDL(character, "Oh you stupi... yes! No! No not for free. But not that costly either."),
						DialogueLine.cDL(character, "You see, I am working on a little side project."),
						DialogueLine.cDL(character, "Just some android tinkering, nothing illegal, no!"),
						DialogueLine.cDL(character, "It's just that I have run out of supplies."),
						DialogueLine.cDL(character, "I need several more Artificial Organs, some packs of Synthblood and another Blank Android Shell."),
						DialogueLine.cDL(character, "But... I am banned from Intertrade. You understand, right?"),
						DialogueLine.cDL(Characters.captain, "I'm not banned, so no?"),
						DialogueLine.cDL(character, "See! Great, you can bring me these items."),
						DialogueLine.cDL(Characters.shipAi, "Captain, why not help this noble and great doctor?"),
						DialogueLine.cDL(Characters.captain, "You are on his side? Wait... what do you think he's doing here?"),
						DialogueLine.cDL(Characters.shipAi, "..."),
						DialogueLine.cDL(Characters.captain, "Alright " + character.name + ", I'll bring you these items, be right back."),
						DialogueLine.cDL(character, "Ah! So exciting! Yes, I go prepare.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId1);
					}
				};
			}
			return new Dialogue
			{
				dialogues = (() => new List<DialogueLine>
				{
					DialogueLine.cDL(character, "No, you go away."),
					DialogueLine.cDL(Characters.captain, "Can I come back later?"),
					DialogueLine.cDL(character, "You need a bit more experience for me to take interest in you.")
				})
			};
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0004B2D4 File Offset: 0x000494D4
		public static Dialogue CreateMercenaryMission(Character character)
		{
			string mId1 = "MercenaryIntroduction";
			if (GamePlayer.current.missionsArchive.Contains(mId1))
			{
				return null;
			}
			if (GamePlayer.current.GetActiveStoryMission(mId1) != null)
			{
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(Characters.captain, "So, it seems I was not paying attention, where do I go again?"),
						DialogueLine.cDL(character, "The Omnitac Space Station. I send the map marker to your info panel.")
					})
				};
			}
			return new Dialogue
			{
				dialogues = (() => new List<DialogueLine>
				{
					DialogueLine.cDL(character, "Greetings Captain. " + character.name + ", nice to meet you."),
					DialogueLine.cDL(Characters.captain, "Well met. I haven't seen your insignia, which faction are you aligned to?"),
					DialogueLine.cDL(character, "Aligned to none, aligned to all one could say."),
					DialogueLine.cDL(character, "We just need to right incentives... cold hard digital credits..."),
					DialogueLine.cDL(Characters.shipAi, "Ah, Mercs!"),
					DialogueLine.cDL(character, "You shut your mou... your voice apparatus.. no! Bad robot!"),
					DialogueLine.cDL(character, "Omnitac Agency is an esteemed escort service!"),
					DialogueLine.cDL(Characters.captain, "I... sure... am confused. I am interested."),
					DialogueLine.cDL(character, "We offer a wide variety of different personnel."),
					DialogueLine.cDL(character, "We have set up several operations across the frontier, but we have a Space Station relatively closeby."),
					DialogueLine.cDL(character, "Meet up with an associate of mine, they will be able to assist you further."),
					DialogueLine.cDL(Characters.captain, "Heading over there now!"),
					DialogueLine.cDL(character, "Omnitac protects."),
					DialogueLine.cDL(Characters.captain, "That's good to know. Protection is important.")
				}),
				onComplete = delegate()
				{
					GamePlayer.current.AddMissionWithLog(mId1);
				}
			};
		}
	}
}
