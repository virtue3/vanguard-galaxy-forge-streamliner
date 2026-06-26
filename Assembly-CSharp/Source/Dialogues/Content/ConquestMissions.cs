using System;
using System.Collections.Generic;
using Behaviour.UI;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;

namespace Source.Dialogues.Content
{
	// Token: 0x02000102 RID: 258
	public static class ConquestMissions
	{
		// Token: 0x060009C3 RID: 2499 RVA: 0x00049884 File Offset: 0x00047A84
		public static Dialogue CreateGotoConquestDialogue(Character character)
		{
			string mId1 = "StoryMissionSkipToConquest";
			GamePlayer ply = GamePlayer.current;
			if (ply.missionsArchive.Contains(mId1))
			{
				return null;
			}
			if (ply.GetActiveStoryMission(mId1) != null)
			{
				return null;
			}
			Action _delegate_2;
			return new Dialogue
			{
				dialogues = (() => new List<DialogueLine>
				{
					DialogueLine.cDL(character, "Hey, you look like you know your way around a space ship. Interested in an adventure?"),
					DialogueLine.cDL(Characters.captain, "Depends on the kind of adventure. Is there money to be made?"),
					DialogueLine.cDL(character, "Do people even care about doing the right thing anymore? But yes, if you play your cards right, you'll be rich in no time."),
					DialogueLine.cDL(Characters.captain, "Sounds like my kind of job. Spill it."),
					DialogueLine.cDL(character, "Troubling news is coming in from the galactic East. Apparently, the three major corporations have opened a new route to the rest of the galaxy, and they're currently entrenching their positions and preparing for war."),
					DialogueLine.cDL(Characters.captain, "War, huh? I thought they mostly got along just fine."),
					DialogueLine.cDL(character, "By the sound of it, diplomacy has started breaking down, and it's a mad dash to claim as much territory as possible, displacing the native Darkspace Compact. Here, take this Jumpgate Pass, and go check it out.")
				}),
				onComplete = delegate()
				{
					if (GamePlayer.current.missionsArchive.Contains("DarkspaceMission6") && GamePlayer.current.missionsArchive.Contains("UmbralMission23"))
					{
						Sandbox storyteller = ply.GetStoryteller<Sandbox>();
						if (storyteller != null)
						{
							storyteller.CleanupStory();
						}
						ply.AddMissionWithLog(mId1);
						return;
					}
					string msg = "You are about to skip ahead to the next part of the story; Story missions from Umbral Reach and Darkspace Compact you haven't completed yet will become unavailable. \n\nAre you sure you want to continue?";
					string yesLabel = null;
					string noLabel = null;
					Action onYes;
					if ((onYes = _delegate_2) == null)
					{
						onYes = (_delegate_2 = delegate()
						{
							Sandbox storyteller2 = ply.GetStoryteller<Sandbox>();
							if (storyteller2 != null)
							{
								storyteller2.CleanupStory();
							}
							ply.AddMissionWithLog(mId1);
						});
					}
					AlertPopup.ShowQuery(msg, yesLabel, noLabel, onYes, null, null, null);
				}
			};
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0004990C File Offset: 0x00047B0C
		public static List<DialogueLine> CanisecConquest1()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.canisecConquestCommander, string.Concat(new string[]
				{
					"Welcome to ",
					MapPointOfInterest.current.name,
					", Captain ",
					Characters.captain.name,
					"."
				})),
				DialogueLine.cDL(Characters.canisecConquestCommander, "You've been busy. And about to get even more work on your hands..."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "The major factions have gone to war."),
				DialogueLine.cDL(Characters.captain, "All three of them? Just like that?"),
				DialogueLine.cDL(Characters.canisecConquestCommander, "They were always at war with eachother, but had to work together to get here so they played friendly."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "But now... no formal declarations. Probably no diplomacy. We were not informed."),
				DialogueLine.cDL(Characters.captain, "Any idea what started it?"),
				DialogueLine.cDL(Characters.canisecConquestCommander, "It has to be Metafiber... but I'm not sure in what capacity."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "But my job is not to speculate. My job is the law. I deal with criminals."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Canisec was meant to keep the peace between them. That was the agreement, but that's out of the airlock."),
				DialogueLine.cDL(Characters.captain, "So what happens now?"),
				DialogueLine.cDL(Characters.canisecConquestCommander, "My forces will mainly focus on the syndicate, those damn chosen weirdos, and see to the protection of the minor factions."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Well, those that are currently not active in the Conquest sector. Imagine Mindus controlling the entire sector, heh!"),
				DialogueLine.cDL(Characters.captain, "Right... and where does that leave independent captains like me?"),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Wherever you want to be, Captain."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Hmmph... I know that each of the three major factions is recruiting. They need ships to throw at eachother, and they eat through raw resources."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "So you could speak to them. Hear what they have to say."),
				DialogueLine.cDL(Characters.captain, "And pick a side?"),
				DialogueLine.cDL(Characters.canisecConquestCommander, "For now. Nothing's permanent in this sector."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "You can align with any of them. Run their missions. Support their bloodlust. Even by attacking the other factions."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Your reputation will take quite a beating, but that's why they set up embassy stations here in the staging sectors."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Go there if you wish to improve your standing with them. The stations are neutral territory."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "Even the Corsairs have one..."),
				DialogueLine.cDL(Characters.canisecConquestCommander, "But whatever you choose, Captain " + Characters.captain.name + "... remember who's still trying to keep the peace around here."),
				DialogueLine.cDL(Characters.captain, "Understood, Commander.")
			};
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00049B7C File Offset: 0x00047D7C
		public static List<DialogueLine> EmbassyStellar()
		{
			Faction blue = Faction.blue;
			if (blue.IsEnemy(Faction.player))
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Characters.stellarRepresentative, "Captain, you should really improve your standing with us, before we can hire you.")
				};
			}
			if (GamePlayer.current.GetStoryteller<Conquest>().GetHeadquarters(blue) == null)
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Characters.stellarRepresentative, "Captain, we are currently not able to offer you a job position. We are working on retaking a system.")
				};
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.stellarRepresentative, "Ah, Captain. I was hoping we'd meet again."),
				DialogueLine.cDL(Characters.captain, "I'm here for the job offer I saw posted."),
				DialogueLine.cDL(Characters.stellarRepresentative, "Great! Your skills are exactly what we need."),
				DialogueLine.cDL(Characters.stellarRepresentative, "We would like to offer you a position. We offer really good benefits. Access to our fleets... destroyers..."),
				DialogueLine.cDL(Characters.captain, "What are the working hours? Do I at least get a fancy uniform?"),
				DialogueLine.cDL(Characters.stellarRepresentative, "You can work from home, or well spaceship. Uniform is optional, but credits and resources come standard."),
				DialogueLine.cDL(Characters.stellarRepresentative, "By helping us we would gain Fleet Strength, which we can use to push out other factions from surrounding systems."),
				DialogueLine.cDL(Characters.stellarRepresentative, "This in turn will help you, unlocking those perks I mentioned before..."),
				DialogueLine.cDL(Characters.captain, "Interesting, interesting..."),
				DialogueLine.cDL(Characters.stellarRepresentative, "There's a jumpgate linking this system to our headquarters in the Conquest sector."),
				DialogueLine.cDL(Characters.stellarRepresentative, string.Concat(new string[]
				{
					"When you accept, meet up with ",
					Characters.conquestCommanderStellar.description,
					" ",
					Characters.conquestCommanderStellar.name,
					" to get your first assignment."
				}))
			};
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00049D0C File Offset: 0x00047F0C
		public static List<DialogueLine> ConquestStellar1()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Captain. " + Characters.conquestCommanderStellar.name + ". Chief Conquest Officer for this sector."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Time is credits, Captain. So I'll keep this somewhat short."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Stellar Industries sees the value you bring."),
				DialogueLine.cDL(Characters.shipAi, "That goes both ways, faction benefits!"),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Definitely, if the results are positive we will both prosper."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Now, to the matter at hand. Our fleets are concentrated along the frontline systems and require constant reinforcement."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Go to the Conquest mission board to generate Fleet Strength. A wide variety of activities are available."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Direct assaults on enemy stations are also encouraged. This will decrease their Fleet Strength. Though your reputation with them will decline."),
				DialogueLine.cDL(Characters.captain, "The cost of doing business."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Precisely."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "We always play the long-term but for immediate results, we require 10 fleet strength."),
				DialogueLine.cDL(Characters.captain, "I'll get right on that.")
			};
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00049E30 File Offset: 0x00048030
		public static List<DialogueLine> ConquestStellar2()
		{
			ConquestRank conquestRank = Faction.red.GetConquestRank();
			string line = "Yes, once you become " + ConquestRank.Rank3.GetConquestRankTranslation(Faction.blue.identifier) + ", you will gain access to more ships within our fleet catalog.";
			if (conquestRank >= ConquestRankExtension.DestroyerRank())
			{
				line = "You have been working overtime, we have already given you a permit for destroyers.";
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Your recent contributions will improve our frontline positions."),
				DialogueLine.cDL(Characters.captain, "So I'm a profitable investment?"),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "You are yielding acceptable returns."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "Continued improvement will unlock further privileges."),
				DialogueLine.cDL(Characters.shipAi, "Bigger ships!"),
				DialogueLine.cDL(Characters.conquestCommanderStellar, line),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "You can rise further in ranks, but what would really help is you aiding us in owning 30% of the sector."),
				DialogueLine.cDL(Characters.captain, "Well, let me get that sorted for you real quick..."),
				DialogueLine.cDL(Characters.shipAi, "Captain, I received a seemingly encrypted signal. It shows a message: \".tsoptuo ruo ot tropeR. noitnetta ruoy stseuqer larbmU.\"."),
				DialogueLine.cDL(Characters.shipAi, "I tried all decryption methods, but I can't make sense of it."),
				DialogueLine.cDL(Characters.captain, "Yes, I see... a very sophisticated encryption."),
				DialogueLine.cDL(Characters.captain, "Pretty sure that would read \"Umbral requests your attention. Report to our outpost.\"."),
				DialogueLine.cDL(Characters.shipAi, "You are a genius! I will mark the location of the outpost on the map.")
			};
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00049F88 File Offset: 0x00048188
		public static List<DialogueLine> ConquestStellar3()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderStellar, "With your assistance, we now own over 30% of this sector."),
				DialogueLine.cDL(Characters.captain, "Sounds like you're pleased with my performance."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "I do have some feedback, but that can wait."),
				DialogueLine.cDL(Characters.conquestCommanderStellar, "We need to hold this momemtum, and keep growing!"),
				DialogueLine.cDL(Characters.shipAi, "Captain, we may continue supporting Stellar Industries. However, we remain free to assist other factions, including smaller factions."),
				DialogueLine.cDL(Characters.shipAi, "There is also a new message we recieved, it's... someone from the Darkspace Compact. You should know him, it's Midas."),
				DialogueLine.cDL(Characters.shipAi, "He asks that you meet with him. I've marked the location on your map.")
			};
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0004A030 File Offset: 0x00048230
		public static List<DialogueLine> EmbassyKolyatov()
		{
			Faction red = Faction.red;
			if (red.IsEnemy(Faction.player))
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Characters.kolyatovCaptain, "Captain, this will not stand, improve your standing with us before we can work together.")
				};
			}
			if (GamePlayer.current.GetStoryteller<Conquest>().GetHeadquarters(red) == null)
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Characters.kolyatovCaptain, "Captain, we currently have not presence in the Conquest sector. Return to us when we do.")
				};
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.kolyatovCaptain, "Ah, I saw you docking. Thought I'd take a moment to speak with you, Captain."),
				DialogueLine.cDL(Characters.captain, Characters.kolyatovCaptain.name + ", good to see you. I've heard Kolyatov is in an all out war."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "We are, and we need abled bodied captains like yourself."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "You could really make a difference, not for profit, but for the people, the collective."),
				DialogueLine.cDL(Characters.shipAi, "LOOT?"),
				DialogueLine.cDL(Characters.kolyatovCaptain, "..."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "There will be some loot involved."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "Let me return to my script. Ahem... Join us, and you'll be rewarded... with comradeship, solidarity..."),
				DialogueLine.cDL(Characters.shipAi, "..."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "... and access to vessels, such as our destroyers..."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "Your actions can tip the balance here, Captain. Help the collective, and you'll have our full support."),
				DialogueLine.cDL(Characters.kolyatovCaptain, "There's a jumpgate linking this system to our headquarters in the Conquest sector."),
				DialogueLine.cDL(Characters.kolyatovCaptain, string.Concat(new string[]
				{
					"If you would accept, meet up with the ",
					Characters.conquestCommanderKolyatov.description,
					" ",
					Characters.conquestCommanderKolyatov.name,
					" there."
				}))
			};
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0004A1F8 File Offset: 0x000483F8
		public static List<DialogueLine> ConquestKolyatov1()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Ah! Captain! Welcome, welcome!"),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Mikhail Kolyatov. Yes... that Kolyatov. My great-great-great-etc-grandfather started the Collective."),
				DialogueLine.cDL(Characters.shipAi, "Historical records confirm. This is Nikita Kolyatov's offspring."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Ah, yes that was his name."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Anyway. We have situation. Sector is... how you say... contested."),
				DialogueLine.cDL(Characters.captain, "Everyone seems to think they own it."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Exactly! But ownership is outdated concept. Sector should belong to people."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Our fleets push at frontline systems, but we need more strength. Always more strength."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "You help by taking missions in Conquest sector. Build Fleet Strength for us."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "You can also attack enemy stations. They will not like you, but... they do not like us anyway."),
				DialogueLine.cDL(Characters.captain, "Fair point."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "For now, we require 10 Fleet Strength. Small number. Very achievable number."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "You help, yes?"),
				DialogueLine.cDL(Characters.captain, "On it.")
			};
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0004A330 File Offset: 0x00048530
		public static List<DialogueLine> ConquestKolyatov2()
		{
			ConquestRank conquestRank = Faction.red.GetConquestRank();
			string line = "When good Kolyatov friend is " + ConquestRank.Rank3.GetConquestRankTranslation(Faction.red.identifier) + " you may purchase Destroyer ships from our yards.";
			if (conquestRank >= ConquestRankExtension.DestroyerRank())
			{
				line = "Good friend of Mikhail already can purchase destroyer. This is great.";
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Ha, " + Faction.red.GetConquestRank().GetConquestRankTranslation(Faction.red.identifier) + "! I knew you were good investment... I mean, good comrade."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Your actions strengthen the morale greatly."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "When collective grow stronger, we all grow stronger. Even you, Captain."),
				DialogueLine.cDL(Characters.captain, "That sounds almost motivational."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Is family tradition."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Continue working with us and you rise in rank."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, line),
				DialogueLine.cDL(Characters.shipAi, "Large guns!"),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "But, now we also need to liberate systems. 30% we need!"),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "You can do this, yes?"),
				DialogueLine.cDL(Characters.shipAi, "Captain, I received a seemingly encrypted signal. It shows a message: \".tsoptuo ruo ot tropeR. noitnetta ruoy stseuqer larbmU.\"."),
				DialogueLine.cDL(Characters.shipAi, "I tried all decryption methods, but I can't make sense of it."),
				DialogueLine.cDL(Characters.captain, "Yes, I see... a very sophisticated encryption."),
				DialogueLine.cDL(Characters.captain, "Pretty sure that would read \"Umbral requests your attention. Report to our outpost.\"."),
				DialogueLine.cDL(Characters.shipAi, "You are a genius! I will mark the location of the outpost on the map.")
			};
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0004A4D4 File Offset: 0x000486D4
		public static List<DialogueLine> ConquestKolyatov3()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Captain! We control over 30% of sector now! This is good day."),
				DialogueLine.cDL(Characters.captain, "You sound surprised."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "No! Not suprised. I always think greatly of you."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "But 30% is not enough. Sector must be... mine."),
				DialogueLine.cDL(Characters.shipAi, "The people's?"),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Yes, I am also people. You understand spirit of it."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Become better friends with us. More rewards will come. Better ships. Greater trust."),
				DialogueLine.cDL(Characters.conquestCommanderKolyatov, "Together we make sector strong."),
				DialogueLine.cDL(Characters.shipAi, "Captain, we may continue assisting the Kolyatov. However, we remain free to support other factions, including minor factions."),
				DialogueLine.cDL(Characters.shipAi, "There is also a new message we recieved, it's... someone from the Darkspace Compact. You should know him, it's Midas."),
				DialogueLine.cDL(Characters.shipAi, "He asks that you meet with him. I've marked the location on your map.")
			};
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0004A5D0 File Offset: 0x000487D0
		public static List<DialogueLine> EmbassyLuminate()
		{
			Faction gold = Faction.gold;
			if (gold.IsEnemy(Faction.player))
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Characters.luminateCommander, "You are our enemy, improve your standing with us at once!")
				};
			}
			if (GamePlayer.current.GetStoryteller<Conquest>().GetHeadquarters(gold) == null)
			{
				return new List<DialogueLine>
				{
					DialogueLine.cDL(Characters.luminateCommander, "We somehow lost our headquarters. Return when we build another one.")
				};
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.luminateCommander, "The Oracle sensed your arrival, Captain."),
				DialogueLine.cDL(Characters.captain, "Yes, I had to ask for docking clearance."),
				DialogueLine.cDL(Characters.captain, Characters.luminateCommander.name + ", right? I've heard you are looking for reinforcements."),
				DialogueLine.cDL(Characters.luminateCommander, "Yes, we are in need of more Drone Power. And, in order to build more drones, we need resources."),
				DialogueLine.cDL(Characters.luminateCommander, "The Oracle has taken an interest in you."),
				DialogueLine.cDL(Characters.luminateCommander, "It sees potential. Potential to transcend, to align yourself with the Oracle."),
				DialogueLine.cDL(Characters.captain, "So, if I help you, I get... spiritual perks? And other benefits, maybe?"),
				DialogueLine.cDL(Characters.luminateCommander, "Perks of the material plane are secondary, such as the ability to purchase our ships. True reward is comprehension... awareness... joining the path of the Oracle."),
				DialogueLine.cDL(Characters.shipAi, "I like the secondary perks."),
				DialogueLine.cDL(Characters.luminateCommander, "Excellent... Consider the gate to our headquarters open."),
				DialogueLine.cDL(Characters.luminateCommander, "Go through the gate and find our " + Characters.conquestCommanderKolyatov.description + " there."),
				DialogueLine.cDL(Characters.luminateCommander, "May the Oracle find you.")
			};
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0004A75C File Offset: 0x0004895C
		public static List<DialogueLine> ConquestLuminate1()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Welcome, Captain. I am " + Characters.conquestCommanderLuminate.name + "."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "The Oracle has calculated that this sector must be brought into alignment."),
				DialogueLine.cDL(Characters.captain, "Alignment... meaning control?"),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Control is a crude word. We prefer harmony."),
				DialogueLine.cDL(Characters.shipAi, "They mean control."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Our forces press the frontline. When neighboring systems fail their defense, they will be integrated into the Oracle's network."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "You may assist by running missions within the Conquest sector."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Direct assaults on enemy stations are also... suggested. Though your reputation with them will deteriorate quickly."),
				DialogueLine.cDL(Characters.shipAi, "Occupational hazard."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "For now, as a new initiate, can you gather 10 fleet strength for the " + Translation.Translate(Faction.gold.name, Array.Empty<object>()) + "?")
			};
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0004A874 File Offset: 0x00048A74
		public static List<DialogueLine> ConquestLuminate2()
		{
			ConquestRank conquestRank = Faction.red.GetConquestRank();
			string line = "When you are " + ConquestRank.Rank3.GetConquestRankTranslation(Faction.gold.identifier) + ", you may purchase Destroyer type vessels from us.";
			if (conquestRank >= ConquestRankExtension.DestroyerRank())
			{
				line = string.Concat(new string[]
				{
					"I see you have been given a destroyer permit already, but more rewards await you ",
					Faction.gold.GetConquestRank().GetConquestRankTranslation(Faction.gold.identifier),
					" ",
					Characters.captain.name,
					"."
				});
			}
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Your contributions will strengthen our frontline systems."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "With each reinforcement, our probability of successful expansion increases."),
				DialogueLine.cDL(Characters.captain, "I'm getting paid to do it, so happy to."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "We seek to control 30% of the sector, aid us in this effort."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, line),
				DialogueLine.cDL(Characters.captain, "Now that's a blessing I understand."),
				DialogueLine.cDL(Characters.shipAi, "Captain, I received a seemingly encrypted signal. It shows a message: \".tsoptuo ruo ot tropeR. noitnetta ruoy stseuqer larbmU.\"."),
				DialogueLine.cDL(Characters.shipAi, "I tried all decryption methods, but I can't make sense of it."),
				DialogueLine.cDL(Characters.captain, "Yes, I see... a very sophisticated encryption."),
				DialogueLine.cDL(Characters.captain, "Pretty sure that would read \"Umbral requests your attention. Report to our outpost.\""),
				DialogueLine.cDL(Characters.shipAi, "You are a genius! I will mark the location of the outpost on the map.")
			};
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0004A9E8 File Offset: 0x00048BE8
		public static List<DialogueLine> ConquestLuminate3()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "With your assistance, over 30% of this sector now resonates with the Combine."),
				DialogueLine.cDL(Characters.captain, "Resonates. I assume that means we're winning..."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Winning... is temporary. We are seeking to enlight the enemy."),
				DialogueLine.cDL(Characters.conquestCommanderLuminate, "Climb further within our ranks, Captain. Greater rewards, such as a deeper understanding, await you."),
				DialogueLine.cDL(Characters.shipAi, "Captain, we may continue assisting the Luminate. However, we remain free to support any faction we choose. Perhaps we should evaluate the minor factions as well."),
				DialogueLine.cDL(Characters.shipAi, "There is however a new message we recieved, it's... someone from the Darkspace Compact, you should know him, Midas."),
				DialogueLine.cDL(Characters.shipAi, "He asks that you meet with him. I've marked the location on your map.")
			};
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0004AA90 File Offset: 0x00048C90
		public static List<DialogueLine> MercenaryEmbassyIntro()
		{
			return new List<DialogueLine>
			{
				DialogueLine.cDL(Characters.captain, "I'm not sure I have the right spacestation, I'm looking for escort services?"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "You mean mercenaries?"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Let me guess, you spoke to Brenda? We recruited her from... she's new!"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "A good employee... we have been receiving a lot more traffic since she began, not that many new clients though... they left dissapointed."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "But you... you seem in need of a mercenary!"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "We have them in all sizes, the higher your reputation with us the larger the ships we offer."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Looking for some more hull to intimidate your enemies? We have those plenty."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Looking for more of an industrial approach? We have mining and salvage captains available, some flown by real Steel Vultures, truly!"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Don't worry, our agents will try not to steal any loot that belongs to you!"),
				DialogueLine.cDL(Characters.shipAi, "Slight worry detected."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Now, just for some more information as to what our agents can expect."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Have you flown in formation before?"),
				DialogueLine.cDL(Characters.captain, "No."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Do you intend to stay within Canisec controlled space?"),
				DialogueLine.cDL(Characters.captain, "No."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Ok, ok, ok, let's see... will you purposely seek out hostile territories?"),
				DialogueLine.cDL(Characters.captain, "Yes."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Ok, ok... just a silly question this one, really... but we have to ask... Will you be engaging in piracy?"),
				DialogueLine.cDL(Characters.captain, "Possibly."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Right... You have been registered under our highest premium tier! Congratulations."),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "The Recruitment Center found in all Omnitac Agency stations will list the available agents. Happy..."),
				DialogueLine.cDL(Characters.captain, "Happy ending?"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "What? No! Happy browsing!"),
				DialogueLine.cDL(Characters.mercenaryEmbassyIntroduction, "Omnitac protects.")
			};
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0004AC9C File Offset: 0x00048E9C
		public static Dialogue MiningToConquest(Character character)
		{
			return ConquestMissions.CreateConquestDialogue("MiningGoToConquest", Faction.miningGuild, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Welcome to Mindus Holdings. Sorry for the mess, we're still trying to get our operations up and running here."),
				DialogueLine.cDL(Characters.captain, "So I hear there's a big war going on. Is Mindus trying to break in to the conflict as well?"),
				DialogueLine.cDL(character, "Yes. With the faction struggle going on in the Ara Martis, Mindus is looking to exploit the area's natural resources for our own gain. Even without directly engaging with the other combatants, we should be able to carve out an area to conduct our operations."),
				DialogueLine.cDL(Characters.captain, "Sweet, what can I do to help?"),
				DialogueLine.cDL(character, "Complete missions from the mission board here in the Embassy. Once we have gathered enough fleet strength, we will be able to make a concerted push into the Ara Martis and establish a headquarters there."),
				DialogueLine.cDL(Characters.captain, "On it!")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Welcome back to Mindus Holdings. Sorry for the mess, I've been too busy managing fleet logistics to tidy up."),
				DialogueLine.cDL(Characters.captain, "Glad to see my efforts are paying off. Is the fleet ready?"),
				DialogueLine.cDL(character, "Yes, everyone is accounted for and our target is selected. We're just waiting for your signal to kick off the entire operation.")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Welcome back to Mindus Holdings. The fleet is still waiting for your response."),
				DialogueLine.cDL(Characters.captain, "Is everything ready?"),
				DialogueLine.cDL(character, "Yes, at your command we will invade the Ara Martis and claim control over our new headquarters. From there, we will begin to expand our influence. Just say the word.")
			});
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0004ACEC File Offset: 0x00048EEC
		public static Dialogue SalvageToConquest(Character character)
		{
			return ConquestMissions.CreateConquestDialogue("SalvageGoToConquest", Faction.salvageGuild, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hey hey. Did ya hear about the war going on next door?"),
				DialogueLine.cDL(Characters.captain, "Well, yes. It's a bit hard to miss these days. What about it?"),
				DialogueLine.cDL(character, "Ok, here's the deal. War means... salvage! And salvage means profit for the Steel Vultures! Right now we're just sending out small expeditions to salvage whatever they can get their hands on, but with your help we'll be able to build up a fleet and claim a piece of the Ara Martis for ourselves!"),
				DialogueLine.cDL(Characters.captain, "Sounds like I'll be able to profit off this deal as well. What's the plan?"),
				DialogueLine.cDL(character, "Complete missions from the mission board here in the Embassy. Once we have gathered enough fleet strength, we will be able to make a concerted push into the Ara Martis and establish a headquarters there."),
				DialogueLine.cDL(Characters.captain, "On it!")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hey hey. Did ya hear? The fleet is assembled and ready to go!"),
				DialogueLine.cDL(Characters.captain, "Glad to see my hard work is paying off. All the preparations have been made?"),
				DialogueLine.cDL(character, "Yes, everyone is ready and our target is set. The Ara Martis and all of its salvage will be ours! Just say the word, and we'll begin our invasion.")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Hey hey. Ready to start the invasion yet?"),
				DialogueLine.cDL(Characters.captain, "Are all the fleet captains ready?"),
				DialogueLine.cDL(character, "Ready and waiting. Everyone's waiting on your signal to start the invasion. How about it?")
			});
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0004AD3C File Offset: 0x00048F3C
		public static Dialogue MaraudersToConquest(Character character)
		{
			return ConquestMissions.CreateConquestDialogue("MaraudersGoToConquest", Faction.marauders, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Yarr, matey. Are ye ready to become a real pirate?"),
				DialogueLine.cDL(Characters.captain, "Excuse me? Why are you talking like that?"),
				DialogueLine.cDL(character, "Sorry, just trying to make the experience more immersive. But if you're looking to align yourself with us Corsairs, I might be able to unlock a few juicy rewards for you."),
				DialogueLine.cDL(Characters.captain, "Sweet, what can I do?"),
				DialogueLine.cDL(character, "Complete some missions in this here embassy, and we'll be able to gather up enough fleet strength to make a push into the Ara Martis and establish a headquarters there. Loot and eternal glory awaits! Yarrr!"),
				DialogueLine.cDL(Characters.captain, "Yarr.")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Ho there, landlubber. How fares the fleet?"),
				DialogueLine.cDL(Characters.captain, "By the looks of it, you should have enough strength to break through the blockade and establish your base of operations."),
				DialogueLine.cDL(character, "And it will be a moment to be remembered! On your signal, we will begin the assault. Just say the word, matey!")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Ho there, landlubber. The fleet is still waiting for your approval. Did you finish your preparations?"),
				DialogueLine.cDL(Characters.captain, "Is everyone ready?"),
				DialogueLine.cDL(character, "We are all waiting for your signal to begin the assault. A glorious victory awaits!")
			});
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0004AD8C File Offset: 0x00048F8C
		public static Dialogue StrandedToConquest(Character character)
		{
			return ConquestMissions.CreateConquestDialogue("StrandedGoToConquest", Faction.stranded, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Welcome, friend. The Stranded are glad to receive you in our humble Embassy."),
				DialogueLine.cDL(Characters.captain, "I hear there's a war going on. Are the Stranded planning to participate as well?"),
				DialogueLine.cDL(character, "Unfortunately, we lack the means of funding such an expedition, as well as the necessary fleet strength. However..."),
				DialogueLine.cDL(Characters.captain, "Go on."),
				DialogueLine.cDL(character, "Say, someone was to start doing some missions for the Embassy. Perhaps those missions were to generate enough fleet strength to build up a force capable of breaking into the Ara Martis. Well... that could change the course of the Stranded."),
				DialogueLine.cDL(Characters.captain, "Interesting! I'll let you know if I come across someone like that.")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Welcome, friend. The most curious thing happened. We suddenly find ourselves with a powerful fleet! All thanks to a kindly anonymous benefactor."),
				DialogueLine.cDL(Characters.captain, "Oh. Yes, well, I wouldn't know anything about that, surely."),
				DialogueLine.cDL(character, "But on such a fortuitous occasion, we should start off the fleet offensive with a ceremonious order to battle. Please, will you do the honors?")
			}, () => new List<DialogueLine>
			{
				DialogueLine.cDL(character, "Welcome, friend. The fleets are ready, thanks to our benefactor. We are waiting for you to signal the attack."),
				DialogueLine.cDL(Characters.captain, "Do you really want me to take charge of the fleet?"),
				DialogueLine.cDL(character, "No, your role is purely ceremonial. Just give the word, and the fleet will find its own way into the Ara Martis to establish our headquarters.")
			});
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0004ADDC File Offset: 0x00048FDC
		public static Dialogue CreateConquestDialogue(string mId1, Faction f, Func<List<DialogueLine>> introduction, Func<List<DialogueLine>> startMission, Func<List<DialogueLine>> continueMission)
		{
			if (!StoryMission.Get(mId1).IsAvailableFor(GamePlayer.current))
			{
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				if (((storyteller != null) ? storyteller.GetHeadquarters(f) : null) != null)
				{
					return null;
				}
				return new Dialogue
				{
					dialogues = introduction
				};
			}
			else
			{
				if (GamePlayer.current.GetActiveStoryMission(mId1) != null)
				{
					return new Dialogue
					{
						dialogues = continueMission,
						onComplete = delegate()
						{
							ConquestMissions.InitiateJoinConquestMission(mId1, f);
						}
					};
				}
				return new Dialogue
				{
					dialogues = startMission,
					onComplete = delegate()
					{
						ConquestMissions.InitiateJoinConquestMission(mId1, f);
					}
				};
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0004AE94 File Offset: 0x00049094
		private static void InitiateJoinConquestMission(string mId, Faction f)
		{
			GamePlayer ply = GamePlayer.current;
			if (!ply.HasStoryMission(mId))
			{
				ply.AddMissionWithLog(mId);
			}
			AlertPopup.ShowQuery("With your help, " + Translation.Translate(f.name, Array.Empty<object>()) + " will join the faction battle in Ara Martis. \n\nAre you sure you want to continue?", null, null, delegate
			{
				Conquest storyteller = ply.GetStoryteller<Conquest>();
				storyteller.JoinConquestSector(f, storyteller.GetPotentialRejoinHQ(f), true);
				Conquest storyteller2 = ply.GetStoryteller<Conquest>();
				if (storyteller2 != null)
				{
					storyteller2.CleanupStory();
				}
				Mission activeStoryMission = ply.GetActiveStoryMission(mId);
				if (activeStoryMission != null)
				{
					ply.CompleteMission(activeStoryMission, true);
				}
				ply.missionsArchive.Remove(mId);
			}, null, null, null);
		}
	}
}
