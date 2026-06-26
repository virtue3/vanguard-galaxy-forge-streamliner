using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Story
{
	// Token: 0x020000AD RID: 173
	public class SideMissions
	{
		// Token: 0x06000704 RID: 1796 RVA: 0x0003AA03 File Offset: 0x00038C03
		public SideMissions()
		{
			this.SideMissionPatrol();
			this.SideMissionBounty();
			this.SideMissionFastLane();
			this.UnlockTier2Skilltrees();
			this.MercenaryIntroduction();
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0003AA29 File Offset: 0x00038C29
		private void SideMissionPatrol()
		{
			StoryMission.Add(new StoryMission("SideMissionPatrol", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SideMissionPatrol", Array.Empty<object>());
				mission.description = Translation.Translate("@SideMissionPatrolDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SideMissionPatrolComplete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.turnIn = MapPointOfInterest.current;
				mission.sourceFaction = Faction.policeGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.iconName = "Combat";
				mission.canBeIdled = false;
				mission.dynamicLevel = true;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = 1,
					trigger = MissionTrigger.CompletePatrol,
					description = "Complete a Patrol"
				});
				mission.steps.Add(missionStep);
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
					faction = Faction.policeGuild,
					amount = 400
				});
				return mission;
			}, null, "Canisec Patrol Roster"));
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0003AA5F File Offset: 0x00038C5F
		private void SideMissionBounty()
		{
			StoryMission.Add(new StoryMission("SideMissionBounty", delegate(GamePlayer ply)
			{
				Mission mission = new Mission();
				mission.name = Translation.Translate("@SideMissionBounty", Array.Empty<object>());
				mission.description = Translation.Translate("@SideMissionBountyDesc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@SideMissionBountyComplete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.turnIn = MapPointOfInterest.current;
				mission.sourceFaction = Faction.bountyGuild;
				mission.trackedOnHud = true;
				mission.difficulty = MissionDifficulty.Story;
				mission.iconName = "Combat";
				mission.canBeIdled = false;
				mission.dynamicLevel = true;
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = 1,
					trigger = MissionTrigger.BountyTargetKilled,
					description = "Kill a Bounty target"
				});
				mission.steps.Add(missionStep);
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
					faction = Faction.bountyGuild,
					amount = 400
				});
				return mission;
			}, null, "Orsanon Bounty Board"));
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0003AA95 File Offset: 0x00038C95
		private void SideMissionFastLane()
		{
			StoryMission.Add(new StoryMission("SideMissionFastLane", delegate(GamePlayer ply)
			{
				InventoryItemType inventoryItemType = "FastLaneMissionItem";
				int num = 40;
				SpaceStation spaceStationForFactionAtDistance = SectorMapData.current.GetSpaceStationForFactionAtDistance(Faction.tradingGuild, 2);
				Debug.Log(spaceStationForFactionAtDistance);
				Mission mission = new Mission
				{
					name = Translation.Translate("@SideMissionFastLane", Array.Empty<object>()),
					description = Translation.Translate("@SideMissionFastLaneDesc", new object[]
					{
						inventoryItemType.displayName,
						spaceStationForFactionAtDistance.name,
						spaceStationForFactionAtDistance.system.name
					}),
					completionText = Translation.Translate("@SideMissionFastLaneComplete", Array.Empty<object>()),
					sourcePoi = MapPointOfInterest.current,
					turnIn = spaceStationForFactionAtDistance,
					sourceFaction = Faction.tradingGuild,
					trackedOnHud = true,
					difficulty = MissionDifficulty.Story,
					iconName = "Map_Poi_Location_Jumpgate",
					canBeIdled = false,
					dynamicLevel = true
				};
				SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
				spaceStationForFactionAtDistance.characters.Add(spaceStation.characters[0]);
				mission.missionItems.Add(inventoryItemType, num);
				MissionStep missionStep = new MissionStep();
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = inventoryItemType,
					requiredAmount = num,
					deliverTo = spaceStationForFactionAtDistance,
					gameplayType = GameplayType.Cargo
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep();
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.FastTravelTalkToNPC,
					description = "Talk to Edar Thopter",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(100f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(50f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.tradingGuild,
					amount = 400
				});
				return mission;
			}, null, "Unlock Fast Lane travel"));
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0003AACC File Offset: 0x00038CCC
		private void UnlockTier2Skilltrees()
		{
			string storyId = "SkilltreeUnlockTier2";
			StoryMission.Add(new StoryMission(storyId ?? "", delegate(GamePlayer ply)
			{
				SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyId, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyId + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyId + "Complete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.turnIn = spaceStation;
				mission.trackedOnHud = true;
				mission.sourceFaction = Faction.red;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				mission.dynamicLevel = true;
				mission.canAbandon = true;
				mission.autoComplete = true;
				MissionStep missionStep = new MissionStep();
				InventoryItemType itemType = InventoryItemType.Get("Artificial Organs");
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = itemType,
					requiredAmount = 3,
					deliverTo = spaceStation,
					gameplayType = GameplayType.Cargo
				});
				InventoryItemType itemType2 = InventoryItemType.Get("Synthblood Pack");
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = itemType2,
					requiredAmount = 8,
					deliverTo = spaceStation,
					gameplayType = GameplayType.Cargo
				});
				InventoryItemType itemType3 = InventoryItemType.Get("Blank Android Shell");
				missionStep.objectives.Add(new TradeOffer
				{
					itemType = itemType3,
					requiredAmount = 1,
					deliverTo = spaceStation,
					gameplayType = GameplayType.Cargo
				});
				mission.steps.Add(missionStep);
				MissionStep missionStep2 = new MissionStep
				{
					poiHint = spaceStation
				};
				missionStep2.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.SkillTier2TalkToNPC,
					description = "Talk to the Doctor",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep2);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(50f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Skillpoint
				{
					amount = 2
				});
				return mission;
			}, (GamePlayer player) => player.level >= 15, "Skilltree Learning Implant"));
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0003AB34 File Offset: 0x00038D34
		private void MercenaryIntroduction()
		{
			string storyId = "MercenaryIntroduction";
			StoryMission.Add(new StoryMission(storyId ?? "", delegate(GamePlayer ply)
			{
				MapPointOfInterest mapPointOfInterest = null;
				if (GamePlayer.current.GetStoryteller<Sandbox>() != null)
				{
					SectorMapData sectorMapData = GamePlayer.current.map.GetQuadrant(SectorMapData.quadrantFrontier).ToList<SectorMapData>().ElementAtOrDefault(3);
					if (sectorMapData != null)
					{
						foreach (SystemMapData systemMapData in sectorMapData.allSystems)
						{
							foreach (SpaceStation spaceStation in systemMapData.GetPointsOfInterest<SpaceStation>())
							{
								if (spaceStation.faction == Faction.mercenaryGuild)
								{
									mapPointOfInterest = spaceStation;
									break;
								}
							}
							if (mapPointOfInterest != null)
							{
								break;
							}
						}
						if (mapPointOfInterest == null)
						{
							SystemMapData systemMapData2 = (from s in sectorMapData.allSystems
							where !s.pocketSystem
							orderby s.GetPointsOfInterest<SpaceStation>().Count<SpaceStation>()
							select s).FirstOrDefault<SystemMapData>();
							if (systemMapData2 != null)
							{
								SystemMapData systemMapData3 = systemMapData2;
								HashSet<SpaceStationFacility> stationFacilities = SandboxWorld.GetStationFacilities(Faction.mercenaryGuild, systemMapData2.level);
								Faction mercenaryGuild = Faction.mercenaryGuild;
								mapPointOfInterest = systemMapData3.AddSpaceStation(stationFacilities, null, mercenaryGuild, null, null);
							}
						}
					}
				}
				SpaceStation spaceStation2 = mapPointOfInterest as SpaceStation;
				if (spaceStation2 != null)
				{
					spaceStation2.TryAddCharacter("MercenaryEmbassyIntroduction");
				}
				Mission mission = new Mission();
				mission.name = Translation.Translate("@" + storyId, Array.Empty<object>());
				mission.description = Translation.Translate("@" + storyId + "Desc", Array.Empty<object>());
				mission.completionText = Translation.Translate("@" + storyId + "Complete", Array.Empty<object>());
				mission.sourcePoi = MapPointOfInterest.current;
				mission.turnIn = mapPointOfInterest;
				mission.trackedOnHud = true;
				mission.sourceFaction = Faction.mercenaryGuild;
				mission.difficulty = MissionDifficulty.Story;
				mission.canBeIdled = false;
				mission.dynamicLevel = true;
				mission.autoComplete = true;
				MissionStep missionStep = new MissionStep
				{
					poiHint = mapPointOfInterest
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					trigger = MissionTrigger.MercIntroEmbassyTalkToNPC,
					description = "Go to the station and talk to the associate",
					requiredAmount = 1
				});
				mission.steps.Add(missionStep);
				mission.rewards.Add(new Experience
				{
					amount = GameMath.GetExperienceRewardValue(50f, GamePlayer.current.level)
				});
				mission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.mercenaryGuild,
					amount = 1000
				});
				return mission;
			}, null, "Escort Services?"));
		}
	}
}
