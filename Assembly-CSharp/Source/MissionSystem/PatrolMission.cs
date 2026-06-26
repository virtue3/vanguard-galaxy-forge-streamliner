using System;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.UI.Missions;
using Behaviour.Util;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem.Objectives;
using Source.MissionSystem.Rewards;
using Source.Player;
using Source.Simulation.World.POI;
using Source.Util;
using Source.Util.NameGen;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000AA RID: 170
	public class PatrolMission : Mission
	{
		// Token: 0x060006E6 RID: 1766 RVA: 0x00039DAC File Offset: 0x00037FAC
		public override void OnMissionAbandoned()
		{
			base.OnMissionAbandoned();
			base.steps[0].GetMissionPoi().ClearUnits();
			base.steps[0].GetMissionPoi().ClearPayloads();
			if (this.patrolLevel == 2 && (this.wave == 0 || this.wave > 4))
			{
				GamePlayer.current.patrolRank = Math.Max(0, GamePlayer.current.patrolRank - 1);
			}
			if (BasePoiManager.current)
			{
				foreach (TravelPortal travelPortal in BasePoiManager.current.GetComponentsInChildren<TravelPortal>())
				{
					if (travelPortal.portalDesc == "@PatrolPortalNext")
					{
						UnityEngine.Object.Destroy(travelPortal.gameObject);
					}
				}
			}
			MapPointOfInterest currentOrNext = MapPointOfInterest.currentOrNext;
			if (currentOrNext != null)
			{
				foreach (PersistableData persistableData in currentOrNext.GetPersistables())
				{
					PatrolPortalData patrolPortalData = persistableData as PatrolPortalData;
					if (patrolPortalData != null && patrolPortalData.isForwardPortal)
					{
						currentOrNext.RemovePersistable(persistableData);
					}
				}
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00039ED0 File Offset: 0x000380D0
		public void SetupPatrolWave(SpaceStation parent, int missionIndex, int missionLevel, int wave, Faction faction, SeededRandom random = null, int patrolCounter = 0)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			string text;
			if (missionIndex != 1)
			{
				if (missionIndex != 2)
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
			SystemMapData system = parent.system;
			Faction faction2 = random.Choose<Faction>(PatrolMission.patrolFactions);
			this.wave = wave;
			this.name = Translation.Translate("@PatrolMissionName", new object[]
			{
				system.name
			});
			string text3 = PatrolReasonGen.GenerateReason(random);
			string text4 = PatrolReasonGen.GenerateAidRequest(random);
			string description = Translation.TranslateOnly("@PatrolMissionDesc", new object[]
			{
				faction.name,
				text3,
				faction2.name,
				text4
			});
			this.description = description;
			this.category = Translation.Translate(text2, new object[]
			{
				missionLevel - parent.level
			});
			this.completionText = Translation.Translate("@PatrolMissionCompleted", Array.Empty<object>());
			this.patrolLevel = missionIndex;
			this.difficulty = MissionDifficulty.Hard;
			this.sourcePoi = parent;
			this.sourceFaction = parent.faction;
			this.autoComplete = true;
			this.iconName = "Combat";
			Combat combat = new Combat();
			system.SetupPOI(combat, null, faction, 0);
			combat.name = "@PatrolMissionPOI";
			combat.level = missionLevel;
			combat.storyteller = new PatrolMission(combat);
			combat.AddCargoContainers(new Vector2(30f, 16f), 1, 0.4f);
			for (int i = 0; i < 5; i++)
			{
				this.AddWave(combat, i, random);
			}
			combat.GeneratePoiPersistables(faction2);
			MissionStep missionStep = new MissionStep
			{
				system = system,
				dynamicPointOfInterest = combat
			};
			missionStep.objectives.Add(new TriggerObjective
			{
				requiredAmount = 5,
				trigger = MissionTrigger.PatrolWaveFinished,
				description = Translation.Translate("@PatrolMissionObjective", Array.Empty<object>())
			});
			base.steps.Add(missionStep);
			if (random != SeededRandom.Global)
			{
				random = new SeedGenerator().Add("PatrolMissionRewards").Add(missionIndex).Add(parent.guid).Add(DateTime.Now.DayOfYear).Add(patrolCounter).CreateRandom();
			}
			float num;
			if (missionIndex != 0)
			{
				if (missionIndex != 2)
				{
					num = 1f;
				}
				else
				{
					num = 1.3f;
				}
			}
			else
			{
				num = 0.7f;
			}
			float num2 = num;
			base.rewards.Add(new Source.MissionSystem.Rewards.Item
			{
				item = "PatrolCurrency",
				amount = Mathf.RoundToInt(random.RandomRange(6.5f, 7.2f) * GameMath.CostMultiplier(missionLevel) * num2)
			});
			base.rewards.Add(new Credits
			{
				amount = GameMath.GetCreditsValue(15f, missionLevel)
			});
			base.rewards.Add(new Source.MissionSystem.Rewards.Reputation
			{
				faction = Faction.policeGuild,
				amount = 125
			});
			base.rewards.Add(new Source.MissionSystem.Rewards.Reputation
			{
				faction = faction2,
				amount = 250
			});
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x0003A1FC File Offset: 0x000383FC
		private void AddWave(MapPointOfInterest poi, int wave, SeededRandom random)
		{
			float num = random.RandomRange(0f, 0.5f);
			List<AbstractUnitData> list = poi.CreateUnitPayload(0.5f + num + (float)wave * 0.1f, new GameplayType?(GameplayType.Combat), null, 20, 0, 1, 5, null);
			if (wave == 0)
			{
				poi.AddGuards(list, random);
				return;
			}
			poi.AddTriggeredSpawn(list, 5f, wave - 1, true, true);
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0003A268 File Offset: 0x00038468
		public override bool CanClaimRewards()
		{
			MapPointOfInterest missionPoi = base.mostRecentStep.GetMissionPoi();
			if (MapPointOfInterest.current != missionPoi)
			{
				return false;
			}
			using (IEnumerator<AbstractUnitData> enumerator = MapPointOfInterest.current.GetUnits(false).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsPlayerEnemy())
					{
						return false;
					}
				}
			}
			return base.CanClaimRewards();
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0003A2DC File Offset: 0x000384DC
		public override void ClaimRewards(bool force = false)
		{
			base.ClaimRewards(force);
			MissionObjective.Trigger(MissionTrigger.CompletePatrol, null, null, false);
			MapPointOfInterest missionPoi = base.mostRecentStep.GetMissionPoi();
			GamePlayer.current.maxPatrolLevel = Math.Max(GamePlayer.current.maxPatrolLevel, base.level);
			if (this.patrolLevel == 2 && this.wave % 2 == 0)
			{
				GamePlayer.current.patrolRank++;
			}
			SpaceStation parent = this.sourcePoi as SpaceStation;
			PatrolMission patrolMission = new PatrolMission();
			patrolMission.SetupPatrolWave(parent, this.patrolLevel, MapPointOfInterest.current.level + 1, this.wave + 1, missionPoi.faction, null, 0);
			GamePlayer.current.currentPatrol = patrolMission;
			Singleton<FocusedMissionHandler>.Instance.SetMission(patrolMission);
			missionPoi.AddPersistable(new PatrolPortalData
			{
				position = GameplayManager.Instance.spaceShip.transform.position + new Vector3(10f, SeededRandom.Global.RandomRange(-5f, 5f)),
				portalName = "@PatrolPortal",
				portalDesc = "@PatrolPortalNext",
				isForwardPortal = true,
				targetPoi = patrolMission.GetActivePoi(false)
			});
			missionPoi.AddPersistable(new PatrolPortalData
			{
				position = GameplayManager.Instance.spaceShip.transform.position + new Vector3(-10f, SeededRandom.Global.RandomRange(-5f, 5f)),
				portalName = "@PatrolPortal",
				portalDesc = "@PatrolPortalBack",
				isForwardPortal = false,
				targetPoi = this.sourcePoi
			});
			Singleton<TravelManager>.Instance.patrolMaxTimer = 60f;
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x0003A49C File Offset: 0x0003869C
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			result["patrolLevel"] = new double?((double)this.patrolLevel);
			result["wave"] = new double?((double)this.wave);
			return result;
		}

		// Token: 0x060006EC RID: 1772 RVA: 0x0003A4EB File Offset: 0x000386EB
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			this.patrolLevel = data["patrolLevel"];
			this.wave = data["wave"];
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0003A520 File Offset: 0x00038720
		public static PatrolMission FromJson(JsonObject data)
		{
			PatrolMission patrolMission = new PatrolMission();
			patrolMission.DataFromJson(data);
			return patrolMission;
		}

		// Token: 0x04000437 RID: 1079
		private static List<Faction> patrolFactions = new List<Faction>
		{
			Faction.gold,
			Faction.blue,
			Faction.red,
			Faction.miningGuild,
			Faction.tradingGuild
		};

		// Token: 0x04000438 RID: 1080
		public int patrolLevel;

		// Token: 0x04000439 RID: 1081
		public int wave;
	}
}
