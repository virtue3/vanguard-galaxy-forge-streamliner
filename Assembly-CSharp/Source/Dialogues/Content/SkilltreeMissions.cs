using System;
using System.Collections.Generic;
using Source.MissionSystem;
using Source.Player;

namespace Source.Dialogues.Content
{
	// Token: 0x02000104 RID: 260
	public class SkilltreeMissions
	{
		// Token: 0x060009DE RID: 2526 RVA: 0x0004B370 File Offset: 0x00049570
		public static Dialogue CreateMiningDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionMining";
			string mId2 = "SkilltreeMissionMining2";
			if (!GamePlayer.current.commander.HasSkilltree("Mining"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Hey, look who's back with a fat stack of cash. Ready to get started?"));
							list.Add(DialogueLine.cDL(Characters.captain, "You bet!"));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Alright, let me just flick this switch here, and... done!");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Huh, I don't feel any smarter."));
							list.Add(DialogueLine.cDL(character, "You seem like a hard-working fellow. Interested in doing some surveying work for us? Mindus will make it worth your time."));
							list.Add(DialogueLine.cDL(Characters.captain, "Sure, I'm always up for a challenge."));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Ready to enroll in Mindus Academy yet?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Hey, welcome to Mindus, the galaxy's foremost asteroid mining corporation. What can I do for you?"),
							DialogueLine.cDL(Characters.captain, "I was looking to specialize further into asteroid mining, any way you can help me out?"),
							DialogueLine.cDL(character, "Sure thing, but you'll have to show me your skills first. Go out there and collect some ore, then we can talk. Oh, and don't forget about the application fee.")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Wow, what's all that dust on your uniform? Looks like you'll make a proper miner after all!"));
							list.Add(DialogueLine.cDL(Characters.captain, "Please... No more..."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Don't worry, you can get back to relaxing after this. Here's your reward.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "How's that survey coming along?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Hey, you look like you know a thing or two about asteroid mining. How about you do some work for us?"),
						DialogueLine.cDL(Characters.captain, "Why do I feel this 'work' is going to be the end of me?"),
						DialogueLine.cDL(character, "Don't worry about it. We're conducting a survey of the area. You won't even have to go out of your way for it, just log the types and quantities of asteroid ore you collect."),
						DialogueLine.cDL(Characters.captain, "I'll think about it.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0004B538 File Offset: 0x00049738
		public static Dialogue CreateCombatDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionCombat";
			string mId2 = "SkilltreeMissionCombat2";
			if (!GamePlayer.current.commander.HasSkilltree("Combat - Offensive"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Unbelievable. Even a pathetic creature like you can follow orders."));
							list.Add(DialogueLine.cDL(Characters.captain, "Ugh, can we skip the insults?"));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "But insulting the new recruits is the only entertainment I get these days! Anyway, here's your contractor license.");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Excellent, that should help me get an upper hand over my enemies."));
							list.Add(DialogueLine.cDL(character, "Now that you're one of us, how about some friendly competition? There are plenty of pirates all throughout the sector, let's see if you can top the leaderboards!"));
							list.Add(DialogueLine.cDL(Characters.captain, "Challenge accepted!"));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Have you completed your assignment, worm?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Unbelievable. Is this the quality of new recruits these days?"),
							DialogueLine.cDL(Characters.captain, "I'm sorry?"),
							DialogueLine.cDL(character, "Yeah, you better be. You just set foot onto a bastion of Orsanon Security, but you look like you sauntered into a supermarket. Shape up, recruit!"),
							DialogueLine.cDL(Characters.captain, "Err, sure. Nice to meet you too."),
							DialogueLine.cDL(character, "You want combat training? Prove to me that you're not some lowly worm-creature first. There's some autonomous drones flying around in this system. Destroy them, then return to me, along with your application fee."),
							DialogueLine.cDL(Characters.captain, "Combat training does sound like a good idea...")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "I can't believe it. Looks like we've got a new chief pirate hunter!"));
							list.Add(DialogueLine.cDL(Characters.captain, "Please tell me there's a reward for all that work."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Good job, recruit. You've earned this skill point fair and square.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "How's the pirate hunt going?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "You look like you've fired a few guns in your time. How about some friendly competition?"),
						DialogueLine.cDL(Characters.captain, "Why do I feel like I'm being roped into some kind of scheme?"),
						DialogueLine.cDL(character, "Orsanon Security is keeping leaderboards for pirate eliminations. Sign up, and you may even win a prize!"),
						DialogueLine.cDL(Characters.captain, "I'll think about it.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0004B700 File Offset: 0x00049900
		public static Dialogue CreateSalvageDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionSalvage";
			string mId2 = "SkilltreeMissionSalvage2";
			if (!GamePlayer.current.commander.HasSkilltree("Salvaging"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Hey, you're back! How did your scrap hunt go?"));
							list.Add(DialogueLine.cDL(Characters.captain, "Does any of this rusted metal look valuable to you?"));
							list.Add(DialogueLine.cDL(character, "No, but I'm just the middleman. Some scientist-looking folks go crazy for the stuff."));
							list.Add(DialogueLine.cDL(Characters.captain, "So, can you teach me about salvaging now?"));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Of course, as promised. Oops, did I forget to mention the training fee?");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "This training better be worth it. I worked hard for those credits, you know."));
							list.Add(DialogueLine.cDL(character, "Now that you know a thing or two about salvaging, why not spread the word of your new best friends, the Steel Vultures? If folks see a charmer like you at work in a salvage field, they'll surely come around to us."));
							list.Add(DialogueLine.cDL(Characters.captain, "That sounds like a lot of work, I'll think about it."));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Hey, sugar! How's that salvage coming along?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "You're brave. You actually decided to disembark on board a Steel Vultures station. For all you know, my people could be scrapping your ride right now."),
							DialogueLine.cDL(Characters.captain, "I doubt it, that'd be bad for business."),
							DialogueLine.cDL(character, "Haha, too true! You're all right. A lot of people have some strange hangups when it comes to salvaging. Mostly we just take stuff that was already abandoned. Mostly."),
							DialogueLine.cDL(Characters.captain, "Huh, so even old abandoned rust can still be valuable?"),
							DialogueLine.cDL(character, "You bet! And to prove it, here's coordinates to an ancient wreckage floating about in this system. Go extract some of its components, and in return I'll teach you about salvaging."),
							DialogueLine.cDL(Characters.captain, "On it!")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Wow, you actually did it! Good job collecting all that salvage, couldn't have been easy."));
							list.Add(DialogueLine.cDL(Characters.captain, "Please tell me there's a reward for all that work."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Good job, recruit. You've earned this skill point fair and square.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Heya! Salvage anything good lately?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Hey, sweetie! When was the last time you ever had a salvage laser bite into a derelict wreck?"),
						DialogueLine.cDL(Characters.captain, "What's with this strange pickup line?"),
						DialogueLine.cDL(character, "Just kidding. But as you know, this sector is full of scrap ripe for the picking. Prove that you're a reliable salvager, and the Steel Vultures will scrounge up some rewards for ya."),
						DialogueLine.cDL(Characters.captain, "I'll think about it.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0004B8C8 File Offset: 0x00049AC8
		public static Dialogue CreateIndustrialDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionIndustrial";
			string mId2 = "SkilltreeMissionIndustrial2";
			if (!GamePlayer.current.commander.HasSkilltree("Industrial"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "I don't believe it, the rookie actually managed to throw some plates together. Not bad!"));
							list.Add(DialogueLine.cDL(Characters.captain, "All I did was program the automated systems, though."));
							list.Add(DialogueLine.cDL(character, "Shush, don't tell anyone! If the rabble knew what that forge was capable of, I'd be out of a job!"));
							list.Add(DialogueLine.cDL(Characters.captain, "So, can you teach me about advanced industrial processes now?"));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Of course, as promised. Here ya go, kid. I hope this knowledge serves you well.");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(character, "Now that you're a bona fide expert, how about you hone your skills some more? The shops around here are always looking for more refined materials, and you might even make something useful for yourself."));
							list.Add(DialogueLine.cDL(Characters.captain, "That sounds like a lot of work, I'll think about it."));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "You done at the forge yet?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "I don't believe it. You want to know what it's like to be a Forgemaster? You, with those flabby arms? I doubt you've ever even held a hammer before!"),
							DialogueLine.cDL(Characters.captain, "As if modern manufacturing techniques even involve hammers."),
							DialogueLine.cDL(character, "Har har! You're all right, kid. Say, if you want to learn more about industrial processes, I do have some time to take on a new student. Interested?"),
							DialogueLine.cDL(Characters.captain, "Judging by that glint in your eye, you're not just going to tell me what I want to know."),
							DialogueLine.cDL(character, "Damn straight! Show me what you're capable of first. Then, for the right amount of credits, I might teach you a thing or two..."),
							DialogueLine.cDL(Characters.captain, "I'll get to work.")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "I don't believe it. You actually did all that? I didn't think you outsiders were actually capable of doing work."));
							list.Add(DialogueLine.cDL(Characters.captain, "Believe me, it wasn't easy. I think a reward is in order."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Of course. From now on, you may officially call yourself my apprentice! Please bow in gratitude.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks, I guess..."));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "How's the forge treating you?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Have I seen you around here before?"),
						DialogueLine.cDL(Characters.captain, "Depends, am I in trouble? Did anything expensive break?"),
						DialogueLine.cDL(character, "You just have that look about you. Like you belong at the forge controls. Say, why don't you do some crafting here? We can always use some extra hands."),
						DialogueLine.cDL(Characters.captain, "I'll think about it.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0004BA90 File Offset: 0x00049C90
		public static Dialogue CreateDronesDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionDrones";
			string mId2 = "SkilltreeMissionDrones2";
			if (!GamePlayer.current.commander.HasSkilltree("Drones"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Good to see you, initiate. I see you have taken to the life of a Drone whisperer."));
							list.Add(DialogueLine.cDL(Characters.captain, "Erm, yeah, sure. Drones are great and all that."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Wonderful news. Now, as soon as you make your donation to the Combine, we can begin.");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Is this going to take long?"));
							list.Add(DialogueLine.cDL(character, "It is already done."));
							list.Add(DialogueLine.cDL(character, "Now, show the galaxy how a true Drone master operates. Once you have ascended and become one with your Drones, meet me back here, and I will reward you."));
							list.Add(DialogueLine.cDL(Characters.captain, "Why does it sound like I've just joined a cult?"));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Greetings, initiate. Are you ready to be anointed?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Machine's blessings upon you, stranger. Have you received the word of the Oracle yet?"),
							DialogueLine.cDL(Characters.captain, "Umm, no. I don't think so."),
							DialogueLine.cDL(character, "How unfortunate. Our blessed Oracle has deemed you worthy of receiving training in the art of Drone control."),
							DialogueLine.cDL(Characters.captain, "Drones, huh? That does sound interesting. Sign me up!"),
							DialogueLine.cDL(character, "One moment, please. You will first have to demonstrate ownership of a Drone-capable space craft. And, of course, your tithe to the Luminate Combine is required."),
							DialogueLine.cDL(Characters.captain, "I'll get to work.")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Marvelous work, brother. The number of devotees to this temple has tripled since your arrival!"));
							list.Add(DialogueLine.cDL(Characters.captain, "Hell yeah. Of course, that's all thanks to me."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "The Luminate Combine will forever be in your debt. Here, take this.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "You're very welcome."));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Greetings, brother. How are your Drones feeling today?"),
							DialogueLine.cDL(Characters.captain, "They are, uh, doing just fine. I guess.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Welcome, brother. Do you seek to join the Oracle in prayer?"),
						DialogueLine.cDL(Characters.captain, "No, actually, I heard you're looking for drone pilots."),
						DialogueLine.cDL(character, "Ah, yes. Show everyone the magnificence of a drone-based loadout, and we'll reward you."),
						DialogueLine.cDL(Characters.captain, "I'll get on it.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0004BC58 File Offset: 0x00049E58
		public static Dialogue CreateDefenseDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionDefense";
			string mId2 = "SkilltreeMissionDefense2";
			if (!GamePlayer.current.commander.HasSkilltree("Defense"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Welcome back, I heard that ship managed to make it back to station. The captain was very grateful for his rescue."));
							list.Add(DialogueLine.cDL(Characters.captain, "No problem. At least, as long as you hold up your end of the deal."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Canisec always keeps its word. Here, I've granted you access to our Defense resources.");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
							list.Add(DialogueLine.cDL(character, "Damn those pirates. I wish someone would just show those bastards that we'll never give up the fight. Show that Canisec and the rest of the galaxy will never yield to their threat."));
							list.Add(DialogueLine.cDL(Characters.captain, "That sounds dangerous, I'll think about it."));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Hey, did you meet with the damaged ship?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Hi, welcome to the Canisec Defense Academy. Sorry for the lack of fanfare, we're kind of all hands on deck today."),
							DialogueLine.cDL(Characters.captain, "What's going on?"),
							DialogueLine.cDL(character, "A big pirate raid disrupted most of the system, and we're still dealing with the fallout. We'd be grateful if you could help out."),
							DialogueLine.cDL(Characters.captain, "Sure, tell me what to do."),
							DialogueLine.cDL(character, "Buy a Shield Pylon from the general shop and meet one of the ships that was raided. They're a sitting duck out there. In exchange, I'll give you a discount on our standard enrollment fee."),
							DialogueLine.cDL(Characters.captain, "Sign me up!")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Unbelievable, every pirate we've interrogated lately has told legends of your unyielding battle tactics."));
							list.Add(DialogueLine.cDL(Characters.captain, "No mercy for lawless scum."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Here's your reward, it's more than deserved. Thanks for defending the sector.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Anything for you."));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Hey, meet with any pirates lately?"),
							DialogueLine.cDL(Characters.captain, "Just the usual.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Did you hear? Pirates raided another base in the system. They must think we're a joke."),
						DialogueLine.cDL(Characters.captain, "Sounds like something needs to be done."),
						DialogueLine.cDL(character, "I wish someone would just show those bastards that we'll never give up the fight. Show that Canisec and the rest of the galaxy will never yield to their threat."),
						DialogueLine.cDL(Characters.captain, "That sounds dangerous, I'll think about it.")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0004BE20 File Offset: 0x0004A020
		public static Dialogue CreateEconomyDialogue(Character character)
		{
			string mId1 = "SkilltreeMissionEconomy";
			string mId2 = "SkilltreeMissionEconomy2";
			if (!GamePlayer.current.commander.HasSkilltree("Economy"))
			{
				Mission mission1 = GamePlayer.current.GetActiveStoryMission(mId1);
				if (mission1 != null && mission1.CanClaimRewards())
				{
					Action _delegate_2 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Well I'll be damned, you actually managed to save up some money. Did you have to starve yourself? Go without fuel for months?"));
							list.Add(DialogueLine.cDL(Characters.captain, "Actually, it was easy. Even without your snooty high-class lady charm I was able to do this much. I'm beginning to doubt if I need your lessons at all."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "Oh, you have guts! I like you. Here, as promised, my 100-part video course on day trading.");
							Action trigger;
							if ((trigger = _delegate_2) == null)
							{
								trigger = (_delegate_2 = delegate()
								{
									GamePlayer.current.CompleteMission(mission1, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
							list.Add(DialogueLine.cDL(character, "Now, if you're up for a challenge... there's an ongoing contest here at Intertrade, where the one who makes the most money using the Trade Terminal gets a big fat bonus. Interested?"));
							list.Add(DialogueLine.cDL(Characters.captain, "You had me at bonus. Sign me up!"));
							return list;
						},
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId2);
						}
					};
				}
				if (mission1 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Oh, the pauper is back. How's your first million credits coming along?"),
							DialogueLine.cDL(Characters.captain, "Working on it.")
						})
					};
				}
				if (mission1 == null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Out of my way, peasant. My important business waits for no one."),
							DialogueLine.cDL(Characters.captain, "Excuse me? I can trade here, just like everybody else."),
							DialogueLine.cDL(character, "You? A trader? please, don't make me laugh. I doubt you even know what a million-credit bill looks like in person."),
							DialogueLine.cDL(Characters.captain, "Oh yeah? Well, I'll prove you wrong! I can buy high and sell low like the best of them!"),
							DialogueLine.cDL(character, "Fine, fine. Have it your way. If you show me a million credits in your possession, I might be willing to teach you a thing or two about the real economy that keeps this galaxy afloat. After paying my modest coaching fee, of course."),
							DialogueLine.cDL(Characters.captain, "I'll hold you to that!")
						}),
						onComplete = delegate()
						{
							GamePlayer.current.AddMissionWithLog(mId1);
						}
					};
				}
				return null;
			}
			else
			{
				Mission mission2 = GamePlayer.current.GetActiveStoryMission(mId2);
				if (mission2 != null && mission2.CanClaimRewards())
				{
					Action _delegate_7 = null;
					return new Dialogue
					{
						dialogues = delegate()
						{
							List<DialogueLine> list = new List<DialogueLine>();
							list.Add(DialogueLine.cDL(character, "Wow, I just checked the leaderboard. Looks like you won this round of the Intertrade Challenge! Congratulations!"));
							list.Add(DialogueLine.cDL(Characters.captain, "Of course, it was all skill. No luck or gambling involved."));
							DialogueLine dialogueLine = DialogueLine.cDL(character, "The Trade Master has tasked me with handing out your reward. Here you go, well deserved.");
							Action trigger;
							if ((trigger = _delegate_7) == null)
							{
								trigger = (_delegate_7 = delegate()
								{
									GamePlayer.current.CompleteMission(mission2, false);
								});
							}
							list.Add(dialogueLine.WithTrigger(trigger));
							list.Add(DialogueLine.cDL(Characters.captain, "Thanks!"));
							return list;
						}
					};
				}
				if (mission2 != null)
				{
					return new Dialogue
					{
						dialogues = (() => new List<DialogueLine>
						{
							DialogueLine.cDL(character, "Back already? I'm not seeing your name atop the leaderboard just yet. How's your trading career coming along?"),
							DialogueLine.cDL(Characters.captain, "I'm working on it.")
						})
					};
				}
				return new Dialogue
				{
					dialogues = (() => new List<DialogueLine>
					{
						DialogueLine.cDL(character, "Greetings, trader. Are you here to take on the Intertrade Challenge?."),
						DialogueLine.cDL(Characters.captain, "What's that?"),
						DialogueLine.cDL(character, "Well... Okay, I'll tell you, even though we're competitors now. There's an ongoing contest here at Intertrade, where the one who makes the most money using the Trade Terminal gets a big fat bonus. Interested?"),
						DialogueLine.cDL(Characters.captain, "You had me at bonus. Sign me up!")
					}),
					onComplete = delegate()
					{
						GamePlayer.current.AddMissionWithLog(mId2);
					}
				};
			}
		}
	}
}
