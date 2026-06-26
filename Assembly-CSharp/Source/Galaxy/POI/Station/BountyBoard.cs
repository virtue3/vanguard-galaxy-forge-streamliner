using System;
using System.Collections.Generic;
using Behaviour.Unit;
using LightJson;
using Source.Data;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using Source.Util.NameGen;
using UnityEngine;

namespace Source.Galaxy.POI.Station
{
	// Token: 0x02000160 RID: 352
	public class BountyBoard : IJsonSource
	{
		// Token: 0x06000D87 RID: 3463 RVA: 0x000619D6 File Offset: 0x0005FBD6
		public BountyBoard(SpaceStation parent)
		{
			this.parent = parent;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000619E8 File Offset: 0x0005FBE8
		public List<BountyMission> GenerateBountyMissions()
		{
			SeededRandom seededRandom = new SeedGenerator().Add("BountyMissions").Add(this.parent.guid).Add(DateTime.Now.DayOfYear).Add(this.bountyCounter).CreateRandom();
			Faction faction = Faction.marauders;
			if (!faction.IsEnemy(Faction.player) || (seededRandom.RandomBool(0.5f) && this.parent.IsEnemyAvailable(Faction.fanatics)))
			{
				faction = Faction.fanatics;
			}
			List<BountyMission> list = new List<BountyMission>();
			for (int i = 0; i < 3; i++)
			{
				int num;
				if (i != 1)
				{
					if (i != 2)
					{
						num = this.parent.level;
					}
					else
					{
						num = this.parent.level + 5 + GamePlayer.current.bountyRank;
					}
				}
				else
				{
					num = this.parent.level + 3 + Mathf.CeilToInt((float)GamePlayer.current.bountyRank * 0.5f);
				}
				int num2 = num;
				string text;
				if (i != 1)
				{
					if (i != 2)
					{
						text = "@BountyDiffNormal";
					}
					else
					{
						text = "@BountyDiffExtreme";
					}
				}
				else
				{
					text = "@BountyDiffElevated";
				}
				string text2 = text;
				string text3 = SpacePirateNames.GeneratePirateName(seededRandom);
				Faction faction2 = seededRandom.Choose<Faction>(BountyBoard.bountyFactions);
				string text4 = CrimeGen.GenerateCrime(seededRandom);
				string description = Translation.TranslateOnly("@BountyMissionDesc", new object[]
				{
					text4,
					text3,
					faction2.name
				});
				if (faction == Faction.fanatics)
				{
					text3 = CaptainFanaticNames.GenerateChosenName(seededRandom);
					description = Translation.TranslateOnly("@BountyMissionFanDesc", new object[]
					{
						text4,
						text3,
						faction2.name
					});
				}
				BountyMission bountyMission = new BountyMission
				{
					name = Translation.Translate("@BountyMissionName", new object[]
					{
						text3
					}),
					description = description,
					category = Translation.Translate(text2, new object[]
					{
						num2 - this.parent.level
					}),
					completionText = Translation.Translate("@BountyMissionCompleted", Array.Empty<object>()),
					pirateName = text3,
					bountyLevel = i,
					difficulty = MissionDifficulty.Hard,
					sourcePoi = this.parent,
					sourceFaction = this.parent.faction,
					autoComplete = true,
					iconName = "Combat"
				};
				Combat combat = new Combat();
				this.parent.system.SetupPOI(combat, null, faction, 0);
				combat.name = ((faction == Faction.marauders) ? "@BountyMissionPOI" : "@BountyMissionFanPOI");
				combat.level = num2;
				combat.GeneratePoiPersistables(faction2);
				combat.AddGuards(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), seededRandom);
				combat.AddTriggeredSpawn(combat.CreateUnitPayload(0.8f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null), 5f, 0, true, true);
				MapPointOfInterest mapPointOfInterest = combat;
				MapPointOfInterest mapPointOfInterest2 = combat;
				float pointsScale = 1f;
				GameplayType? gType = new GameplayType?(GameplayType.Combat);
				Faction f = null;
				int minPointsPerUnit = 0;
				int maxPointsPerUnit = 0;
				int minUnits = 1;
				int maxUnits = 5;
				UnitRank? fixedRank = null;
				mapPointOfInterest.AddTriggeredSpawn(mapPointOfInterest2.CreateUnitPayload(pointsScale, gType, f, minPointsPerUnit, maxPointsPerUnit, minUnits, maxUnits, fixedRank), 5f, 1, true, true);
				int num3 = (num2 < 20) ? 40 : 50;
				num3 = ((num2 > 32) ? 140 : num3);
				UnitRank randomUnitRankForLevel = UnitRankHelper.GetRandomUnitRankForLevel(num2, true);
				MapPointOfInterest mapPointOfInterest3 = combat;
				float pointsScale2 = 2.5f;
				num = num3;
				int maxPointsPerUnit2 = num3 * 2;
				fixedRank = new UnitRank?(randomUnitRankForLevel);
				List<AbstractUnitData> list2 = mapPointOfInterest3.CreateUnitPayload(pointsScale2, new GameplayType?(GameplayType.Combat), null, num, maxPointsPerUnit2, 1, 1, fixedRank);
				SpaceShipData spaceShipData = list2[0] as SpaceShipData;
				spaceShipData.commanderData.SetName(text3, null, null);
				spaceShipData.deathTrigger = new MissionTrigger?(MissionTrigger.BountyTargetKilled);
				list2.AddRange(combat.CreateUnitPayload(0.5f, new GameplayType?(GameplayType.Combat), null, 0, 0, 1, 5, null));
				combat.AddTriggeredSpawn(list2, 5f, 2, true, true);
				MissionStep missionStep = new MissionStep
				{
					system = this.parent.system,
					dynamicPointOfInterest = combat
				};
				missionStep.objectives.Add(new TriggerObjective
				{
					requiredAmount = 1,
					trigger = MissionTrigger.BountyTargetKilled,
					description = Translation.Translate("@BountyMissionObjective", new object[]
					{
						faction.name
					})
				});
				bountyMission.steps.Add(missionStep);
				new SeedGenerator().Add("BountyMissionRewards").Add(i).Add(this.parent.guid).Add(DateTime.Now.DayOfYear).Add(this.bountyCounter).CreateRandom();
				float num4;
				if (i != 0)
				{
					if (i != 2)
					{
						num4 = 1f;
					}
					else
					{
						num4 = 1.3f;
					}
				}
				else
				{
					num4 = 0.7f;
				}
				float num5 = num4;
				bountyMission.rewards.Add(new Source.MissionSystem.Rewards.Item
				{
					item = "BountyCurrency",
					amount = Mathf.RoundToInt(seededRandom.RandomRange(7f, 8f) * GameMath.CostMultiplier(num2) * num5)
				});
				bountyMission.rewards.Add(new Credits
				{
					amount = GameMath.GetCreditsValue(25f, num2)
				});
				bountyMission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = Faction.bountyGuild,
					amount = 175
				});
				bountyMission.rewards.Add(new Source.MissionSystem.Rewards.Reputation
				{
					faction = faction2,
					amount = 350
				});
				list.Add(bountyMission);
			}
			return list;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00061FA1 File Offset: 0x000601A1
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"bountyCounter",
					new double?((double)this.bountyCounter)
				}
			};
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00061FCA File Offset: 0x000601CA
		public static BountyBoard FromJson(JsonObject json, SpaceStation parent)
		{
			return new BountyBoard(parent)
			{
				bountyCounter = json["bountyCounter"]
			};
		}

		// Token: 0x04000761 RID: 1889
		private static List<Faction> bountyFactions = new List<Faction>
		{
			Faction.gold,
			Faction.blue,
			Faction.red,
			Faction.miningGuild,
			Faction.tradingGuild
		};

		// Token: 0x04000762 RID: 1890
		private readonly SpaceStation parent;

		// Token: 0x04000763 RID: 1891
		public int bountyCounter;
	}
}
