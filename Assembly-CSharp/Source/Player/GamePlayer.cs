using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Dialogues;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.Missions;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using Behaviour.Util;
using LightJson;
using Source.Crew;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.Factions;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station;
using Source.Hazard;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.MissionSystem.Objectives;
using Source.Simulation;
using Source.Simulation.Story;
using Source.SpaceShip;
using Source.SpaceShip.Auto;
using Source.Util;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x02000095 RID: 149
	public class GamePlayer : IJsonSource
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00031A42 File Offset: 0x0002FC42
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00031A4A File Offset: 0x0002FC4A
		public SpaceShipData currentSpaceShip { get; private set; }

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00031A53 File Offset: 0x0002FC53
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00031A5B File Offset: 0x0002FC5B
		public GalaxyMapData map { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00031A64 File Offset: 0x0002FC64
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00031A6C File Offset: 0x0002FC6C
		public CommanderData commander { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00031A75 File Offset: 0x0002FC75
		public int level
		{
			get
			{
				return this.commander.level;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00031A82 File Offset: 0x0002FC82
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x00031A8A File Offset: 0x0002FC8A
		public FactionData factionData { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00031A93 File Offset: 0x0002FC93
		public Mission infiniteMission
		{
			get
			{
				BountyMission result;
				if ((result = this.currentBounty) == null)
				{
					result = (this.currentPatrol ?? this.currentIndustry);
				}
				return result;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00031AAF File Offset: 0x0002FCAF
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00031AB7 File Offset: 0x0002FCB7
		public Inventory globalInventory { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00031AC0 File Offset: 0x0002FCC0
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x00031AC8 File Offset: 0x0002FCC8
		public Register register { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x00031AD1 File Offset: 0x0002FCD1
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00031AD9 File Offset: 0x0002FCD9
		public double elapsedTime { get; private set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00031AE2 File Offset: 0x0002FCE2
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x00031AEA File Offset: 0x0002FCEA
		public bool mapUnlocked { get; private set; } = true;

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00031AF3 File Offset: 0x0002FCF3
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00031AFB File Offset: 0x0002FCFB
		public bool autoPlayUnlocked { get; private set; } = true;

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00031B04 File Offset: 0x0002FD04
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00031B0C File Offset: 0x0002FD0C
		public bool fastLaneTravelUnlocked { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00031B15 File Offset: 0x0002FD15
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00031B1D File Offset: 0x0002FD1D
		public bool skilltreeTier2Unlocked { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00031B26 File Offset: 0x0002FD26
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00031B2E File Offset: 0x0002FD2E
		public long salvageWorkshopTotalSpent { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00031B37 File Offset: 0x0002FD37
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x00031B3F File Offset: 0x0002FD3F
		public int salvageWorkshopTotalModified { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00031B48 File Offset: 0x0002FD48
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00031B50 File Offset: 0x0002FD50
		public long salvageWorkshopShopCredit { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00031B59 File Offset: 0x0002FD59
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x00031B61 File Offset: 0x0002FD61
		public bool salvageWorkshopTradeInMaterialsUnlocked { get; private set; } = true;

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00031B6C File Offset: 0x0002FD6C
		public Mercenary hiredMercenary
		{
			get
			{
				foreach (SpaceShipData spaceShipData in this.activeFleet)
				{
					Mercenary mercenary = spaceShipData.commanderData as Mercenary;
					if (mercenary != null && !mercenary.isExtra)
					{
						return mercenary;
					}
				}
				return null;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00031BD4 File Offset: 0x0002FDD4
		public IEnumerable<Mission> allMissions
		{
			get
			{
				if (this.currentBounty != null)
				{
					yield return this.currentBounty;
				}
				if (this.currentPatrol != null)
				{
					yield return this.currentPatrol;
				}
				if (this.currentIndustry != null)
				{
					yield return this.currentIndustry;
				}
				foreach (Mission mission in this.missions)
				{
					yield return mission;
				}
				List<Mission>.Enumerator enumerator = default(List<Mission>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00031BE4 File Offset: 0x0002FDE4
		public bool IsInSandBox()
		{
			using (List<Storyteller>.Enumerator enumerator = this.storytellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Sandbox)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00031C40 File Offset: 0x0002FE40
		public bool HasStoryteller<T>()
		{
			using (List<Storyteller>.Enumerator enumerator = this.storytellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is default(T))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00031C9C File Offset: 0x0002FE9C
		public bool HasAccessToQuadrant(int quadrant)
		{
			if (quadrant == SectorMapData.quadrantPrologue)
			{
				return this.GetStoryteller<Tutorial>() != null;
			}
			if (quadrant == SectorMapData.quadrantFrontier)
			{
				return this.GetStoryteller<Sandbox>() != null;
			}
			return quadrant == SectorMapData.quadrantConquest && this.GetStoryteller<Conquest>() != null;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00031CD8 File Offset: 0x0002FED8
		public GamePlayer()
		{
			this.factionData = new FactionData();
			this.register = new Register();
			this.map = new GalaxyMapData();
			this.globalInventory = new Inventory(false);
			this.autopilotSettings = new AutopilotSettings();
			this.ResetDynamicEventTimer(-1f);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00031E4D File Offset: 0x0003004D
		public GamePlayer(PersonalHistoryData personalHistoryData, bool tutorial) : this()
		{
			this.credits = (long)personalHistoryData.starterCredits;
			this.starterSpaceshipName = personalHistoryData.starterShipName;
			this.starterSpecialization = (int)personalHistoryData.starterSpecialization;
			this.commander = CommanderData.CreateCommander(personalHistoryData.starterSpecialization, tutorial);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00031E8C File Offset: 0x0003008C
		public T GetStoryteller<T>() where T : Storyteller
		{
			foreach (Storyteller storyteller in this.storytellers)
			{
				T t = storyteller as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00031EFC File Offset: 0x000300FC
		public bool CanAfford(float amount)
		{
			return this.credits >= (long)((int)amount);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00031F0C File Offset: 0x0003010C
		public int RequiredItemCountForMissions(InventoryItemType item)
		{
			int num = 0;
			foreach (Mission mission in this.allMissions)
			{
				foreach (MissionStep missionStep in mission.steps)
				{
					foreach (MissionObjective missionObjective in missionStep.objectives)
					{
						num += missionObjective.ItemCountRequired(item);
					}
				}
			}
			return num;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00031FD4 File Offset: 0x000301D4
		public bool CanFit(float amount)
		{
			return amount < this.currentSpaceShip.cargoCapacity - this.currentSpaceShip.cargoUsed;
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00031FF0 File Offset: 0x000301F0
		public void RemoveCredits(float amount)
		{
			if (amount < 0f)
			{
				Debug.LogWarning("Cannot remove a negative credit amount");
				return;
			}
			this.AddAutopilotStat(IdleStat.CreditsSpent, (int)amount);
			this.credits -= (long)((int)amount);
			if (this.credits < 0L)
			{
				this.credits = 0L;
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00032030 File Offset: 0x00030230
		public void Update()
		{
			this.elapsedTime += (double)Time.deltaTime;
			if (this.dynamicEventTimer > 0f)
			{
				this.dynamicEventTimer -= Time.deltaTime;
			}
			foreach (Storyteller storyteller in this.storytellers)
			{
				storyteller.StoryUpdate(Time.deltaTime);
			}
			for (int i = 0; i < this.missions.Count; i++)
			{
				this.missions[i].Update(Time.deltaTime);
			}
			BountyMission bountyMission = this.currentBounty;
			if (bountyMission != null)
			{
				bountyMission.Update(Time.deltaTime);
			}
			PatrolMission patrolMission = this.currentPatrol;
			if (patrolMission != null)
			{
				patrolMission.Update(Time.deltaTime);
			}
			IndustryMission industryMission = this.currentIndustry;
			if (industryMission != null)
			{
				industryMission.Update(Time.deltaTime);
			}
			this.UpdateExtraMercenarySpawn();
			SpaceShipData currentSpaceShip = this.currentSpaceShip;
			UnityEngine.Object exists;
			if (currentSpaceShip == null)
			{
				exists = null;
			}
			else
			{
				AbstractUnit unit = currentSpaceShip.unit;
				exists = ((unit != null) ? unit.rigidbody : null);
			}
			if (exists)
			{
				this.mapPosition = this.currentSpaceShip.unit.rigidbody.position / MapPointOfInterest.mapToLocalConversion;
			}
			this.UpdateMerc();
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x0003217C File Offset: 0x0003037C
		private void UpdateExtraMercenarySpawn()
		{
			if (this.level < 15)
			{
				return;
			}
			if (this.currentSpaceShip == null || this.currentSpaceShip.unit == null)
			{
				return;
			}
			this.extraMercenarySpawnCheckTimer -= Time.deltaTime;
			this.extraMercenarySpawnTimer -= Time.deltaTime;
			float num = this.currentSpaceShip.DamageTakenPercentage();
			if (num < 0.5f)
			{
				return;
			}
			if (this.elapsedTime - this.currentSpaceShip.unit.lastDamageTime > 10.0)
			{
				return;
			}
			if (this.extraMercenarySpawnCheckTimer <= 0f)
			{
				this.extraMercenarySpawnCheckTimer = 10f;
				if (this.extraMercenarySpawnTimer > 0f)
				{
					return;
				}
				foreach (SpaceShipData spaceShipData in this.activeFleet)
				{
					Mercenary mercenary = spaceShipData.commanderData as Mercenary;
					if (mercenary != null && mercenary.isExtra)
					{
						return;
					}
				}
				MapPointOfInterest mapPointOfInterest = MapPointOfInterest.current;
				int num2 = (mapPointOfInterest != null) ? mapPointOfInterest.activeEnemyCount : 1;
				float chanceOfTrue = 0.01f * num * (float)num2;
				if (this.hiredMercenary != null && this.hiredMercenary.repairing)
				{
					chanceOfTrue = 0.25f;
				}
				if (SeededRandom.Global.RandomBool(chanceOfTrue))
				{
					this.SpawnExtraMercenary();
					this.extraMercenarySpawnTimer = 600f;
				}
			}
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x000322E4 File Offset: 0x000304E4
		private void SpawnExtraMercenary()
		{
			List<ExtraMercenaryPreset> extraMercenaryPresets = MercenaryGuild.extraMercenaryPresets;
			if (extraMercenaryPresets == null || extraMercenaryPresets.Count == 0)
			{
				return;
			}
			ExtraMercenaryPreset extraMercenaryPreset = SeededRandom.Global.Choose<ExtraMercenaryPreset>(extraMercenaryPresets);
			Mercenary mercenary = new Mercenary(extraMercenaryPreset.seed, new Gender?(extraMercenaryPreset.gender), extraMercenaryPreset.callsign, null);
			if (extraMercenaryPreset.battlecries != null && extraMercenaryPreset.battlecries.Length != 0)
			{
				mercenary.battlecry = SeededRandom.Global.Choose<string>(extraMercenaryPreset.battlecries);
			}
			mercenary.isExtra = true;
			mercenary.rarity = Rarity.Legendary;
			mercenary.timeLeft = 300f;
			mercenary.autoExtend = false;
			MapPointOfInterest mapPointOfInterest = MapPointOfInterest.current;
			int num = (mapPointOfInterest != null) ? mapPointOfInterest.level : this.level;
			List<SpaceShip> source = SpaceShip.GetNPCShipTypes(this.level, num - 5, num * 4, new GameplayType?(GameplayType.Combat)).ToList<SpaceShip>();
			HashSet<string> allowedShipNames = new HashSet<string>();
			foreach (KeyValuePair<ValueTuple<SpaceShipRole, SpaceShipType>, List<string>> keyValuePair in RecruitmentCenter.combatShips)
			{
				foreach (string item in keyValuePair.Value)
				{
					allowedShipNames.Add(item);
				}
			}
			List<SpaceShip> list = (from s in source
			where allowedShipNames.Contains(s.identifier)
			select s).ToList<SpaceShip>();
			if (list.Count > 0)
			{
				SpaceShip spaceShip = SeededRandom.Global.Choose<SpaceShip>(list);
				mercenary.ship = spaceShip.name;
				Vector2 spawnPosition = GameplayManager.Instance.spaceShip.transform.position;
				UnityEngine.Object x = GameplayManager.Instance.CreateMercenary(spawnPosition, mercenary);
				HudManager.Instance.ToggleWingman(true);
				if (x != null)
				{
					mercenary.pendingNpcMessage = mercenary.battlecry;
				}
			}
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x000324D0 File Offset: 0x000306D0
		private void UpdateMerc()
		{
			for (int i = this.activeFleet.Count - 1; i >= 0; i--)
			{
				SpaceShipData spaceShipData = this.activeFleet[i];
				Mercenary mercenary = spaceShipData.commanderData as Mercenary;
				if (mercenary != null)
				{
					if (mercenary.repairing)
					{
						spaceShipData.repairTimer -= Time.deltaTime;
						if (spaceShipData.repairTimer < 0f && this.waypoints.Count == 0)
						{
							GameplayManager.Instance.SpawnFleetShipAtPlayer(spaceShipData);
							spaceShipData.repairTimer = 0f;
							mercenary.repairing = false;
						}
					}
					else
					{
						mercenary.timeLeft -= Time.deltaTime;
						if (mercenary.timeLeft < 0f)
						{
							if (mercenary.autoExtend && this.CanAfford((float)mercenary.creditCost))
							{
								this.RemoveCredits((float)mercenary.creditCost);
								mercenary.AddMercTime(1);
							}
							else
							{
								SpaceShip spaceShip = spaceShipData.unit as SpaceShip;
								if (spaceShip != null)
								{
									WingmanActions wingmanActions = spaceShip.autoActions as WingmanActions;
									if (wingmanActions != null)
									{
										wingmanActions.LeavePlayer();
									}
								}
								this.activeFleet.RemoveAt(i);
								HudManager.Instance.ToggleWingman(true);
							}
						}
						if (!mercenary.isExtra)
						{
							mercenary.repUpdateTimer -= Time.deltaTime;
							if (mercenary.repUpdateTimer < 0f)
							{
								Faction.mercenaryGuild.ChangePlayerReputation(mercenary.GetRarityCostMultiplier());
								mercenary.repUpdateTimer = 60f;
							}
						}
					}
				}
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00032643 File Offset: 0x00030843
		public bool IsMapUsageUnlocked()
		{
			return this.mapUnlocked;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0003264B File Offset: 0x0003084B
		public void SetMapUsage(bool toggle)
		{
			this.mapUnlocked = toggle;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00032654 File Offset: 0x00030854
		public void SetAutoPlayUsage(bool toggle)
		{
			this.autoPlayUnlocked = toggle;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0003265D File Offset: 0x0003085D
		public void SetFastLaneTravelUnlocked(bool unlocked)
		{
			this.fastLaneTravelUnlocked = unlocked;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00032666 File Offset: 0x00030866
		public void SetSkilltreeTier2Unlocked(bool unlocked)
		{
			this.skilltreeTier2Unlocked = unlocked;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0003266F File Offset: 0x0003086F
		public void SetSalvageWorkshopTradeinUnlocked(bool unlocked)
		{
			this.salvageWorkshopTradeInMaterialsUnlocked = unlocked;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00032678 File Offset: 0x00030878
		public bool AutopilotCanBeEnabled()
		{
			return this.autoPlayUnlocked && this.infiniteMission == null;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00032690 File Offset: 0x00030890
		public void EndAutopilotSession()
		{
			this.autoPlay = false;
			GameplayManager.Instance.spaceShip.ResetAutoActions();
			if (this.currentAutopilotSessionStats != null && this.currentAutopilotSessionStats.DoWeKeepThis())
			{
				this.currentAutopilotSessionStats.endTime = (float)this.elapsedTime;
				this.autopilotSessionStats.Add(this.currentAutopilotSessionStats);
				if (this.autopilotSessionStats.Count > 10)
				{
					this.autopilotSessionStats.RemoveAt(0);
				}
				this.currentAutopilotSessionStats = null;
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Ship, 3);
			}
			this.currentAutopilotSessionStats = null;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00032720 File Offset: 0x00030920
		public void StartAutopilotSession()
		{
			this.currentAutopilotSessionStats = new AutopilotSessionStats();
			this.currentAutopilotSessionStats.shipName = this.currentSpaceShip.name;
			AutopilotSessionStats autopilotSessionStats = this.currentAutopilotSessionStats;
			SpaceShip spaceShip = this.currentSpaceShip.unit as SpaceShip;
			autopilotSessionStats.shipActivity = ((spaceShip != null) ? spaceShip.GetPreferredActivityName(false) : null);
			this.currentAutopilotSessionStats.startTime = (float)this.elapsedTime;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00032788 File Offset: 0x00030988
		public void AddAutopilotStat(IdleStat stat, int amount)
		{
			if (this.autoPlay)
			{
				if (this.currentAutopilotSessionStats == null)
				{
					this.StartAutopilotSession();
				}
				if (this.currentAutopilotSessionStats.stats.ContainsKey(stat))
				{
					Dictionary<IdleStat, int> stats = this.currentAutopilotSessionStats.stats;
					stats[stat] += amount;
					return;
				}
				this.currentAutopilotSessionStats.stats.Add(stat, amount);
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x000327EF File Offset: 0x000309EF
		public float GetLastX()
		{
			if (this.currentPointOfInterest != null)
			{
				return this.currentPointOfInterest.lastVisitedX;
			}
			return 0f;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0003280A File Offset: 0x00030A0A
		public bool UseLastShipPosition()
		{
			return this.currentPointOfInterest == null || this.currentPointOfInterest.storeLastX;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00032821 File Offset: 0x00030A21
		public bool nextWaypointIsSystem(string guid)
		{
			return this.waypoints.Count != 0 && this.waypoints[0].system.guid == guid;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0003284E File Offset: 0x00030A4E
		public bool HasWaypointsAfterCurrent()
		{
			return this.waypoints.Count > 1;
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00032860 File Offset: 0x00030A60
		public int CountAvailableItems(InventoryItemType type)
		{
			int num = this.globalInventory.GetCount(type) + this.currentSpaceShip.cargo.GetCount(type);
			SpaceStation spaceStation = this.currentPointOfInterest as SpaceStation;
			if (spaceStation != null)
			{
				num += spaceStation.materialStorage.GetCount(type);
			}
			return num;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000328AC File Offset: 0x00030AAC
		public bool ConsumeAvailableItems(InventoryItemType type, int count)
		{
			if (this.CountAvailableItems(type) < count)
			{
				return false;
			}
			SpaceStation spaceStation = this.currentPointOfInterest as SpaceStation;
			int num;
			if (spaceStation != null)
			{
				num = spaceStation.materialStorage.Remove(type, count);
				count -= num;
				if (count <= 0)
				{
					return true;
				}
			}
			num = this.globalInventory.Remove(type, count);
			count -= num;
			if (count <= 0)
			{
				return true;
			}
			num = this.currentSpaceShip.cargo.Remove(type, count);
			count -= num;
			return count <= 0;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00032928 File Offset: 0x00030B28
		public int CountAllItems(InventoryItemType type)
		{
			int num = this.globalInventory.GetCount(type);
			foreach (SpaceShipData spaceShipData in this.spaceShips)
			{
				num += spaceShipData.cargo.GetCount(type);
			}
			foreach (MapPointOfInterest mapPointOfInterest in this.map.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null)
				{
					num += spaceStation.materialStorage.GetCount(type);
				}
			}
			return num;
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000329E8 File Offset: 0x00030BE8
		public void ReloadDefensiveTurrets()
		{
			using (IEnumerator<Inventory.InventoryItem> enumerator = this.currentSpaceShip.cargo.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DefensiveTurretItem defensiveTurretItem;
					if (enumerator.Current.item.TryGetComponent<DefensiveTurretItem>(out defensiveTurretItem))
					{
						defensiveTurretItem.currentHealth = 1f;
						defensiveTurretItem.currentAmmo = defensiveTurretItem.maxAmmo;
						ShieldPylonItem shieldPylonItem = defensiveTurretItem as ShieldPylonItem;
						if (shieldPylonItem != null)
						{
							shieldPylonItem.SetCharge(1f);
						}
					}
				}
			}
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00032A74 File Offset: 0x00030C74
		public void RepairFleet()
		{
			foreach (SpaceShipData spaceShipData in this.activeFleet)
			{
				spaceShipData.RepairHullHp(spaceShipData.maxHullHP);
				spaceShipData.RepairArmorHP(spaceShipData.maxArmorHP);
				spaceShipData.RepairShieldHp(spaceShipData.maxShieldHP);
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00032AE4 File Offset: 0x00030CE4
		public void AddToFleet(SpaceShipData spaceShipData)
		{
			this.activeFleet.Add(spaceShipData);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00032AF2 File Offset: 0x00030CF2
		public void RemoveFromFleet(SpaceShipData spaceShipData)
		{
			this.activeFleet.Remove(spaceShipData);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00032B04 File Offset: 0x00030D04
		public MissionTrigger? GetCurrentDialogueTrigger(Character character)
		{
			if (character == null)
			{
				return null;
			}
			foreach (Mission mission in this.missions)
			{
				if (mission.currentStep != null && (mission.difficulty == MissionDifficulty.Tutorial || mission.difficulty == MissionDifficulty.Story) && mission.currentStep != null)
				{
					foreach (MissionObjective missionObjective in mission.currentStep.objectives)
					{
						TriggerObjective triggerObjective = missionObjective as TriggerObjective;
						if (triggerObjective != null && !triggerObjective.IsComplete())
						{
							foreach (Source.Dialogues.Dialogue dialogue in character.dialogues)
							{
								if (triggerObjective.trigger == dialogue.trigger)
								{
									return new MissionTrigger?(triggerObjective.trigger);
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00032C4C File Offset: 0x00030E4C
		public void AcceptMission(Mission mission)
		{
			if (!this.IsMissionsLimitExceeded())
			{
				this.AddMissionWithLog(mission);
				SpaceStation.current.missionBoard.AcceptMission(mission);
				GamePlayer.RefreshMissionPanel(mission);
				return;
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UIMaxMissions", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00032CA8 File Offset: 0x00030EA8
		public static void RefreshMissionPanel(Mission mission = null)
		{
			SidePanel instance = SidePanel.instance;
			if (instance.currentTab == SidePanel.SideTabType.Missions)
			{
				instance.RefreshIfOpen();
			}
			SpaceStationInterior instance2 = SpaceStationInterior.instance;
			if (instance2 != null && instance2.currentTab == SpaceStationFacility.MissionBoard)
			{
				if (mission != null)
				{
					Behaviour.UI.MissionBoard.preselectMission = mission;
				}
				SpaceStationInterior.instance.GoToLocation(SpaceStationFacility.MissionBoard, true);
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00032CF7 File Offset: 0x00030EF7
		public void ArchiveMission(string id, bool allowDuplicate = false)
		{
			if (allowDuplicate || !this.missionsArchive.Contains(id))
			{
				this.missionsArchive.Add(id);
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00032D18 File Offset: 0x00030F18
		public void RemoveMission(Mission mission, bool completed)
		{
			if (this.currentBounty == mission)
			{
				this.currentBounty = null;
			}
			else if (this.currentPatrol == mission)
			{
				HudManager.Instance.HideSubtleTimerInfo(2);
				this.currentPatrol = null;
			}
			else if (this.currentIndustry == mission)
			{
				this.currentIndustry = null;
			}
			else
			{
				this.missions.Remove(mission);
			}
			if (!completed)
			{
				mission.OnMissionAbandoned();
				return;
			}
			if (mission.storyId != null)
			{
				this.ArchiveMission(mission.storyId, true);
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00032D94 File Offset: 0x00030F94
		public void CompleteMission(string name)
		{
			Mission mission = null;
			foreach (Mission mission2 in this.missions)
			{
				if (mission2.name == name)
				{
					mission = mission2;
				}
			}
			if (mission != null)
			{
				this.CompleteMission(mission, false);
				return;
			}
			Debug.LogWarning("Mission not active: " + name);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00032E10 File Offset: 0x00031010
		public void CompleteMission(Mission m, bool force = false)
		{
			m.ClaimRewards(force);
			SidePanel instance = SidePanel.instance;
			if (instance.currentTab == SidePanel.SideTabType.Missions)
			{
				instance.RefreshIfOpen();
			}
			if (SpaceStationInterior.instance && SpaceStationInterior.instance.currentTab == SpaceStationFacility.MissionBoard)
			{
				SpaceStationInterior.instance.Refresh();
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00032E5D File Offset: 0x0003105D
		public bool IsMissionCompleted(string storyId)
		{
			return this.missionsArchive.Contains(storyId);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00032E6C File Offset: 0x0003106C
		public Mission GetMission(string storyId)
		{
			foreach (Mission mission in this.missions)
			{
				if (mission.storyId == storyId)
				{
					return mission;
				}
			}
			return null;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00032ED0 File Offset: 0x000310D0
		public void CleanupSaveData()
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.map.allPointsOfInterest.ToList<MapPointOfInterest>())
			{
				if (mapPointOfInterest.lastVisitedTime != 0f && mapPointOfInterest != MapPointOfInterest.currentOrNext && Singleton<TravelManager>.Instance.targetPoi != mapPointOfInterest)
				{
					float num = (float)this.elapsedTime - mapPointOfInterest.lastVisitedTime;
					SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
					if (spaceStation != null)
					{
						if (num > 300f)
						{
							Source.Galaxy.POI.Station.MissionBoard missionBoard = spaceStation.missionBoard;
							if (missionBoard != null)
							{
								missionBoard.availableMissions.Clear();
							}
						}
						if (num > 3600f)
						{
							spaceStation.ClearShopInventory();
							mapPointOfInterest.CleanupConquestStation();
							spaceStation.conquestStationInitialized = false;
						}
					}
					if (num > 3600f)
					{
						Source.Galaxy.POI.Mining mining = mapPointOfInterest as Source.Galaxy.POI.Mining;
						if (mining != null && !MiningClaimItem.IsMiningClaim(mining) && mapPointOfInterest.name != "Potential Jumpgate Site")
						{
							mapPointOfInterest.system.RemovePointOfInterest(mapPointOfInterest);
							Source.Galaxy.POI.Mining mining2 = mapPointOfInterest.system.AddMiningPoi(mapPointOfInterest.faction, null, null, 0f, true, 0.5f);
							if (mapPointOfInterest.hazardFieldData != null)
							{
								mining2.hazardFieldData = HazardFieldData.CreateRandom(0.4f);
								mining2.hazardsDescription = Translation.Translate("@MapPOIHazard", Array.Empty<object>()) + ": " + mapPointOfInterest.hazardFieldData.GetHazardName();
							}
						}
						else
						{
							Source.Galaxy.POI.Salvage salvage = mapPointOfInterest as Source.Galaxy.POI.Salvage;
							if (salvage != null && !SalvageClaimItem.IsSalvageClaim(salvage))
							{
								mapPointOfInterest.system.RemovePointOfInterest(mapPointOfInterest);
								if (!salvage.system.pocketSystem)
								{
									mapPointOfInterest.system.AddAncientWreckPoi(mapPointOfInterest.faction, mapPointOfInterest.system.GetRandomPosition(-20f, 20f, -5f, 5f));
								}
								else
								{
									mapPointOfInterest.system.AddDerelictFleetPoi(mapPointOfInterest.faction, null, "AncientWreck", 0.5f);
								}
							}
							else
							{
								JumpGate jumpGate = mapPointOfInterest as JumpGate;
								if (jumpGate != null)
								{
									jumpGate.CleanupWindowDressing();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00033110 File Offset: 0x00031310
		public void GrantTitle(string identifier)
		{
			this.unlockedTitles.Add(identifier);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0003311F File Offset: 0x0003131F
		public bool HasTitle(string identifier)
		{
			return this.unlockedTitles.Contains(identifier);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0003312D File Offset: 0x0003132D
		public void GrantDecal(string identifier)
		{
			this.unlockedDecals.Add(identifier);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0003313C File Offset: 0x0003133C
		public bool HasDecal(string identifier)
		{
			return this.unlockedDecals.Contains(identifier);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0003314C File Offset: 0x0003134C
		public void AddSalvageCreditsSpent(int amount)
		{
			this.salvageWorkshopTotalSpent += (long)amount;
			int salvageWorkshopTotalModified = this.salvageWorkshopTotalModified;
			this.salvageWorkshopTotalModified = salvageWorkshopTotalModified + 1;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00033178 File Offset: 0x00031378
		public void AddSalvageShopCredit(int creditValue, int itemAmount, bool fromItems = true)
		{
			this.salvageWorkshopShopCredit += (long)creditValue;
			if (fromItems)
			{
				Register.AddCounter("SalWorShopCreditItemAmount", itemAmount, 0);
				Register.AddCounter("SalWorShopCreditValueFromItems", creditValue, 0);
			}
			Register.AddCounter("WorkshopCreditsGained", creditValue, 0);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000331B3 File Offset: 0x000313B3
		public void RemoveSalvageShopCredit(int creditValue)
		{
			this.salvageWorkshopShopCredit -= (long)creditValue;
			if (this.salvageWorkshopShopCredit < 0L)
			{
				this.salvageWorkshopShopCredit = 0L;
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000331D6 File Offset: 0x000313D6
		public void ResetDynamicEventTimer(float baseTime = -1f)
		{
			if (baseTime <= 0f)
			{
				baseTime = 450f;
			}
			this.dynamicEventTimer = SeededRandom.Global.RandomRange(baseTime, baseTime * 3f);
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00033200 File Offset: 0x00031400
		public bool ConsumeRefinedMaterial(RefinedMaterial mat, float amt)
		{
			if (this.refinedStorage[(int)mat] < amt)
			{
				return false;
			}
			this.refinedStorage[(int)mat] -= amt;
			return true;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0003322E File Offset: 0x0003142E
		public void AddRefinedMaterial(RefinedMaterial mat, float amt)
		{
			this.refinedStorage[(int)mat] += amt;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00033244 File Offset: 0x00031444
		public void FillRefinery(float amt)
		{
			for (int i = 0; i < this.refinedStorage.Length; i++)
			{
				this.refinedStorage[i] = amt;
			}
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0003326D File Offset: 0x0003146D
		public float CountRefinedMaterial(RefinedMaterial material)
		{
			return this.refinedStorage[(int)material];
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00033278 File Offset: 0x00031478
		public bool DoFastLaneTravel()
		{
			if (this.waypoints.Count > 0)
			{
				JumpGate jumpGate = this.waypoints[0] as JumpGate;
				if (jumpGate != null && jumpGate.canUseJumpGate)
				{
					return this.fastLaneTravelUnlocked;
				}
			}
			return false;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000332B8 File Offset: 0x000314B8
		public JsonValue ToJson()
		{
			this.CleanupSaveData();
			JsonArray jsonArray = new JsonArray();
			foreach (MapPointOfInterest mapPointOfInterest in this.waypoints)
			{
				jsonArray.Add(mapPointOfInterest.guid);
			}
			JsonArray jsonArray2 = new JsonArray();
			foreach (Faction faction in this.atWar)
			{
				jsonArray2.Add(faction.identifier);
			}
			JsonObject jsonObject = new JsonObject();
			jsonObject.Add("factionData", this.factionData.ToJson());
			jsonObject.Add("map", this.map.ToJson());
			jsonObject.Add("spaceShips", this.spaceShips.ToJsonArray<SpaceShipData>());
			jsonObject.Add("activeFleet", this.activeFleet.ToJsonArray<SpaceShipData>());
			jsonObject.Add("starterSpaceshipName", this.starterSpaceshipName);
			jsonObject.Add("starterSpecialization", new double?((double)this.starterSpecialization));
			jsonObject.Add("commander", this.commander.ToJson());
			jsonObject.Add("crewMembers", this.crewMembers.ToJsonArray<CrewMemberData>());
			jsonObject.Add("currentSpaceShip", this.currentSpaceShip.guid);
			jsonObject.Add("missions", this.missions.ToJsonArray<Mission>());
			jsonObject.Add("missionsArchive", this.missionsArchive.ToJsonArray());
			jsonObject.Add("globalInventory", this.globalInventory.ToJson());
			jsonObject.Add("register", this.register.ToJson());
			jsonObject.Add("credits", this.credits.ToString());
			jsonObject.Add("currentSystem", this.currentSystem.guid);
			string key = "currentPointOfInterest";
			MapPointOfInterest mapPointOfInterest2 = this.currentPointOfInterest;
			jsonObject.Add(key, (mapPointOfInterest2 != null) ? mapPointOfInterest2.guid : null);
			jsonObject.Add("elapsedTime", new double?(this.elapsedTime));
			jsonObject.Add("dynamicEventTimer", new double?((double)this.dynamicEventTimer));
			jsonObject.Add("extraMercenarySpawnTimer", new double?((double)this.extraMercenarySpawnTimer));
			jsonObject.Add("waypoints", jsonArray);
			jsonObject.Add("mapPosition", JsonUtil.Vector2ToJson(this.mapPosition));
			jsonObject.Add("autoPlay", new bool?(this.autoPlay));
			jsonObject.Add("autopilotSettings", this.autopilotSettings.ToJson());
			string key2 = "currentAutopilotSessionStats";
			AutopilotSessionStats autopilotSessionStats = this.currentAutopilotSessionStats;
			jsonObject.Add(key2, (autopilotSessionStats != null) ? autopilotSessionStats.ToJson() : JsonValue.Null);
			jsonObject.Add("autopilotSessionStats", this.autopilotSessionStats.ToJsonArray<AutopilotSessionStats>());
			jsonObject.Add("forgeDepositInCargo", new bool?(this.forgeDepositInCargo));
			string key3 = "lastMiningPOI";
			MapPointOfInterest mapPointOfInterest3 = this.lastVisitedMiningPOI;
			jsonObject.Add(key3, (mapPointOfInterest3 != null) ? mapPointOfInterest3.guid : null);
			string key4 = "homeStation";
			SpaceStation spaceStation = this.homeStation;
			jsonObject.Add(key4, (spaceStation != null) ? spaceStation.guid : null);
			string key5 = "lastStation";
			SpaceStation spaceStation2 = this.lastStation;
			jsonObject.Add(key5, (spaceStation2 != null) ? spaceStation2.guid : null);
			jsonObject.Add("mapUnlocked", new bool?(this.mapUnlocked));
			jsonObject.Add("autoPlayUnlocked", new bool?(this.autoPlayUnlocked));
			jsonObject.Add("fastLaneTravelUnlocked", new bool?(this.fastLaneTravelUnlocked));
			jsonObject.Add("storytellers", this.storytellers.ToJsonArray<Storyteller>());
			jsonObject.Add("patrolRank", new double?((double)this.patrolRank));
			jsonObject.Add("bountyRank", new double?((double)this.bountyRank));
			jsonObject.Add("industryRank", new double?((double)this.industryRank));
			jsonObject.Add("maxBountyLevel", new double?((double)this.maxBountyLevel));
			jsonObject.Add("maxPatrolLevel", new double?((double)this.maxPatrolLevel));
			jsonObject.Add("maxIndustryLevel", new double?((double)this.maxIndustryLevel));
			string key6 = "currentBounty";
			BountyMission bountyMission = this.currentBounty;
			jsonObject.Add(key6, (bountyMission != null) ? bountyMission.ToJson() : JsonValue.Null);
			string key7 = "currentPatrol";
			PatrolMission patrolMission = this.currentPatrol;
			jsonObject.Add(key7, (patrolMission != null) ? patrolMission.ToJson() : JsonValue.Null);
			string key8 = "currentIndustry";
			IndustryMission industryMission = this.currentIndustry;
			jsonObject.Add(key8, (industryMission != null) ? industryMission.ToJson() : JsonValue.Null);
			jsonObject.Add("refinedStorage", JsonUtil.ArrayToJson(this.refinedStorage));
			jsonObject.Add("blueprints", this.blueprints.ToJsonArray());
			jsonObject.Add("useWarpFuel", new bool?(this.useWarpFuel));
			jsonObject.Add("emergencyJump", new bool?(this.emergencyJump));
			jsonObject.Add("salworcrtotal", new double?((double)this.salvageWorkshopTotalSpent));
			jsonObject.Add("salwortotmod", new double?((double)this.salvageWorkshopTotalModified));
			jsonObject.Add("salworshopcredit", new double?((double)this.salvageWorkshopShopCredit));
			jsonObject.Add("salwordaily", this.salvageWorkshopDailyItem);
			jsonObject.Add("salworDailyLastUpdateTime", this.salvageWorkshopLastUpdateTime.ToString());
			jsonObject.Add("salworTradeInUnlocked", new bool?(this.salvageWorkshopTradeInMaterialsUnlocked));
			jsonObject.Add("abilitySlots", this.abilitySlots.ToJsonArray());
			jsonObject.Add("unlockedTitles", this.unlockedTitles.ToJsonArray());
			jsonObject.Add("unlockedDecals", this.unlockedDecals.ToJsonArray());
			jsonObject.Add("abilityCooldowns", this.AbilityCooldownsToJson());
			jsonObject.Add("activeEffects", this.ActiveEffectsToJson());
			jsonObject.Add("activeToggles", this.ActivetogglesToJson());
			jsonObject.Add("atWar", jsonArray2);
			jsonObject.Add("skilltreeTier2Unlocked", new bool?(this.skilltreeTier2Unlocked));
			jsonObject.Add("hasUmbralTransponder", new bool?(this.hasUmbralTransponder));
			return jsonObject;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00033A24 File Offset: 0x00031C24
		private JsonArray ActiveEffectsToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (KeyValuePair<string, ActiveEffectData> keyValuePair in new Dictionary<string, ActiveEffectData>(this.activeEffects))
			{
				if (keyValuePair.Key != null && keyValuePair.Value != null)
				{
					jsonArray.Add(new JsonObject
					{
						{
							"id",
							keyValuePair.Key
						},
						{
							"dur",
							new double?((double)keyValuePair.Value.durationRemaining)
						},
						{
							"stacks",
							new double?((double)keyValuePair.Value.stackSize)
						}
					});
				}
			}
			return jsonArray;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00033B04 File Offset: 0x00031D04
		private JsonArray ActivetogglesToJson()
		{
			JsonArray jsonArray = new JsonArray();
			foreach (string value in new HashSet<string>(this.activeToggles))
			{
				jsonArray.Add(value);
			}
			return jsonArray;
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00033B6C File Offset: 0x00031D6C
		private JsonObject AbilityCooldownsToJson()
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<string, float> keyValuePair in new Dictionary<string, float>(this.abilityCooldowns))
			{
				if (keyValuePair.Value > 0f)
				{
					jsonObject[keyValuePair.Key] = new double?((double)keyValuePair.Value);
				}
			}
			return jsonObject;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00033BF4 File Offset: 0x00031DF4
		public static GamePlayer FromJson(JsonValue val)
		{
			GamePlayer gamePlayer = GamePlayer.current;
			GamePlayer player = new GamePlayer();
			GamePlayer.current = player;
			player.recreateItems = GameVersion.IsBelow(SaveGame.loadedVersion, 0, 6, 0, 0);
			player.factionData = FactionData.FromJson(val2["factionData"]);
			player.starterSpaceshipName = val2["starterSpaceshipName"];
			player.starterSpecialization = ((val2["starterSpecialization"] == JsonValue.Null || val2["starterSpecialization"] == new double?(0.0)) ? 2 : val2["starterSpecialization"]);
			player.commander = CommanderData.FromJson(val2["commander"]);
			player.crewMembers.FromJsonArray(val2["crewMembers"], new ClassExtensions.ParseJsonValue<CrewMemberData>(CrewMemberData.FromJson));
			player.spaceShips.FromJsonArray(val2["spaceShips"], (JsonValue data) => SpaceShipData.FromJson(data, true));
			if (!val2["activeFleet"].IsNull)
			{
				player.activeFleet.FromJsonArray(val2["activeFleet"], (JsonValue data) => SpaceShipData.FromJson(data, false));
			}
			player.map.FromJson(val2["map"]);
			player.currentSpaceShip = player.GetSpaceShipData(val2["currentSpaceShip"]);
			player.missions.FromJsonArray(val2["missions"], new ClassExtensions.ParseJsonValue<Mission>(Mission.FromJson));
			player.missionsArchive.FromJsonArray(val2["missionsArchive"], (JsonValue val) => val.AsString);
			player.globalInventory = Inventory.FromJson(val2["globalInventory"], null, false);
			player.register = Register.FromJson(val2["register"]);
			if (val2["credits"].IsString)
			{
				player.credits = long.Parse(val2["credits"]);
			}
			else
			{
				player.credits = (long)val2["credits"];
			}
			player.currentSystem = player.map.GetSystem(val2["currentSystem"]);
			player.mapPosition = JsonUtil.JsonObjectToVector2(val2["mapPosition"]);
			player.elapsedTime = val2["elapsedTime"].AsNumber;
			player.dynamicEventTimer = (float)val2["dynamicEventTimer"].AsNumber;
			player.extraMercenarySpawnTimer = (float)val2["extraMercenarySpawnTimer"].AsNumber;
			player.patrolRank = val2["patrolRank"];
			player.bountyRank = val2["bountyRank"];
			player.industryRank = val2["industryRank"];
			player.maxBountyLevel = val2["maxBountyLevel"];
			player.maxPatrolLevel = val2["maxPatrolLevel"];
			player.maxIndustryLevel = val2["maxIndustryLevel"];
			player.useWarpFuel = val2["useWarpFuel"];
			player.emergencyJump = val2["emergencyJump"];
			player.hasUmbralTransponder = val2["hasUmbralTransponder"];
			if (val2["lastMiningPOI"] != JsonValue.Null)
			{
				player.lastVisitedMiningPOI = player.map.GetPointOfInterest(val2["lastMiningPOI"]);
			}
			if (val2["homeStation"] != JsonValue.Null)
			{
				player.homeStation = (player.map.GetPointOfInterest(val2["homeStation"]) as SpaceStation);
			}
			if (val2["lastStation"] != JsonValue.Null)
			{
				player.lastStation = (player.map.GetPointOfInterest(val2["lastStation"]) as SpaceStation);
			}
			if (val2["mapUnlocked"] != JsonValue.Null)
			{
				player.mapUnlocked = val2["mapUnlocked"].AsBoolean;
			}
			if (val2["autoPlayUnlocked"] != JsonValue.Null)
			{
				player.autoPlayUnlocked = val2["autoPlayUnlocked"].AsBoolean;
			}
			if (val2["fastLaneTravelUnlocked"] != JsonValue.Null)
			{
				player.fastLaneTravelUnlocked = val2["fastLaneTravelUnlocked"].AsBoolean;
			}
			if (val2["storytellers"] != JsonValue.Null)
			{
				player.storytellers.FromJsonArray(val2["storytellers"], (JsonValue data) => Storyteller.FromJson(data, player));
			}
			else
			{
				player.AddStoryteller(new Default(player), true);
				player.AddStoryteller(new Tutorial(player), false);
			}
			if (val2["currentBounty"] != JsonValue.Null)
			{
				player.currentBounty = BountyMission.FromJson(val2["currentBounty"]);
			}
			if (val2["currentPatrol"] != JsonValue.Null)
			{
				player.currentPatrol = PatrolMission.FromJson(val2["currentPatrol"]);
			}
			if (val2["currentIndustry"] != JsonValue.Null)
			{
				player.currentIndustry = IndustryMission.FromJson(val2["currentIndustry"]);
			}
			if (val2["currentPointOfInterest"] != JsonValue.Null)
			{
				player.currentPointOfInterest = player.map.GetPointOfInterest(val2["currentPointOfInterest"]);
			}
			if (val2["refinedStorage"].IsJsonArray)
			{
				player.refinedStorage = JsonUtil.FloatArrayFromJson(val2["refinedStorage"]);
			}
			if (val2["waypoints"].IsJsonArray)
			{
				foreach (JsonValue jsonValue in val2["waypoints"].AsJsonArray)
				{
					player.waypoints.Add(player.map.GetPointOfInterest(jsonValue));
				}
			}
			player.autoPlay = val2["autoPlay"];
			if (val2["autopilotSettings"].IsJsonObject)
			{
				player.autopilotSettings = AutopilotSettings.FromJson(val2["autopilotSettings"]);
			}
			else
			{
				player.autopilotSettings = new AutopilotSettings();
			}
			if (val2["currentAutopilotSessionStats"].IsJsonObject)
			{
				player.currentAutopilotSessionStats = AutopilotSessionStats.FromJson(val2["currentAutopilotSessionStats"]);
			}
			if (val2["autopilotSessionStats"].IsJsonArray)
			{
				player.autopilotSessionStats.FromJsonArray(val2["autopilotSessionStats"], new ClassExtensions.ParseJsonValue<AutopilotSessionStats>(AutopilotSessionStats.FromJson));
			}
			if (val2["forgeDepositInCargo"].IsBoolean)
			{
				player.forgeDepositInCargo = val2["forgeDepositInCargo"];
			}
			else
			{
				player.forgeDepositInCargo = true;
			}
			if (val2["blueprints"].IsJsonArray)
			{
				player.blueprints.FromJsonArray(val2["blueprints"], (JsonValue val) => val.AsString);
			}
			if (val2["salworcrtotal"] != JsonValue.Null)
			{
				long asLong = val2["salworcrtotal"].AsLong;
				player.salvageWorkshopTotalSpent = ((asLong < 0L) ? 0L : asLong);
			}
			if (val2["salwortotmod"] != JsonValue.Null)
			{
				player.salvageWorkshopTotalModified = val2["salwortotmod"];
			}
			if (val2["salworshopcredit"] != JsonValue.Null)
			{
				long asLong2 = val2["salworshopcredit"].AsLong;
				player.salvageWorkshopShopCredit = ((asLong2 < 0L) ? 0L : asLong2);
			}
			if (val2["salwordaily"] != JsonValue.Null)
			{
				player.salvageWorkshopDailyItem = val2["salwordaily"];
			}
			if (val2["salworDailyLastUpdateTime"] != JsonValue.Null)
			{
				player.salvageWorkshopLastUpdateTime = long.Parse(val2["salworDailyLastUpdateTime"]);
			}
			if (val2["salworTradeInUnlocked"] != JsonValue.Null)
			{
				player.salvageWorkshopTradeInMaterialsUnlocked = val2["salworTradeInUnlocked"].AsBoolean;
			}
			if (val2["abilitySlots"].IsJsonArray)
			{
				player.abilitySlots.FromJsonArray(val2["abilitySlots"], (JsonValue val) => val.AsString);
			}
			if (val2["unlockedTitles"].IsJsonArray)
			{
				foreach (JsonValue jsonValue2 in val2["unlockedTitles"].AsJsonArray)
				{
					player.unlockedTitles.Add(jsonValue2.AsString);
				}
			}
			if (val2["unlockedDecals"].IsJsonArray)
			{
				foreach (JsonValue jsonValue3 in val2["unlockedDecals"].AsJsonArray)
				{
					player.unlockedDecals.Add(jsonValue3.AsString);
				}
			}
			if (val2["abilityCooldowns"].IsJsonObject)
			{
				foreach (KeyValuePair<string, JsonValue> keyValuePair in val2["abilityCooldowns"].AsJsonObject)
				{
					player.abilityCooldowns[keyValuePair.Key] = (float)keyValuePair.Value.AsNumber;
				}
			}
			if (val2["activeEffects"].IsJsonArray)
			{
				foreach (JsonValue jsonValue4 in val2["activeEffects"].AsJsonArray)
				{
					player.activeEffects[jsonValue4["id"].AsString] = new ActiveEffectData
					{
						durationRemaining = (float)jsonValue4["dur"].AsNumber,
						stackSize = (int)jsonValue4["stacks"].AsNumber
					};
				}
			}
			if (val2["activeToggles"].IsJsonArray)
			{
				foreach (JsonValue jsonValue5 in val2["activeToggles"].AsJsonArray)
				{
					player.activeToggles.Add(jsonValue5.AsString);
				}
			}
			if (val2["atWar"].IsJsonArray)
			{
				foreach (JsonValue jsonValue6 in val2["atWar"].AsJsonArray)
				{
					player.atWar.Add(Faction.Get(jsonValue6));
				}
			}
			if (val2["skilltreeTier2Unlocked"] != JsonValue.Null)
			{
				player.skilltreeTier2Unlocked = val2["skilltreeTier2Unlocked"].AsBoolean;
			}
			Refinery.autoSell = Register.HasFlag("AutoSell", false);
			GamePatch.CheckFactions(player);
			if (player.GetStoryteller<Default>() == null)
			{
				player.AddStoryteller(new Default(player), true);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 6, 0, 0))
			{
				GamePatch.OldDemoToEA(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 6, 6, 2))
			{
				GamePatch.ResetBountyGuild(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 6, 7, 0))
			{
				GamePatch.AddSidequests(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 6, 9, 2))
			{
				GamePatch.AddSidequests(player);
				GamePatch.ResetPoliceGuild(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 0, 5))
			{
				GamePatch.FixTutorialGateDescription(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 0, 6))
			{
				GamePatch.EnsureFactionMissionBoard(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 0, 10))
			{
				GamePatch.EnsurePersistablesAreWithinBounds(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 1, 0))
			{
				GamePatch.GenerateEnemyMiners();
				GamePatch.AddStructuralSalvage();
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 1, 2))
			{
				GamePatch.FixAsteroidFieldsAmount();
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 1, 3))
			{
				GamePatch.AddIndustrialGuild(player);
				GamePatch.RefreshInfiniteShops(player);
				GamePatch.AddSalvageWorkshopsToWorld(player);
				player.useWarpFuel = true;
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 4, 6))
			{
				GamePatch.FixClearConstructionSiteMission(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 5, 7))
			{
				GamePatch.SetCriticalCrewmembers(player);
				GamePatch.ClearSpacestationPayloads(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 5, 10))
			{
				GamePatch.CleanupSpaceShipPng(player);
				GamePatch.TweakClearingConstructionSiteMission(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 6, 0))
			{
				GamePatch.CleanupJumpGates(player);
				if (player.GetStoryteller<Sandbox>() != null)
				{
					GamePatch.AddSidequests(player);
					GamePatch.AddSectorQuadrant(player);
				}
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 6, 3))
			{
				GamePatch.AddAsteroidsClearingConstructionSiteMission(player);
				GamePatch.ClearMercenaries(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 6, 5))
			{
				GamePatch.AdjustStationSizes(player);
				GamePatch.SetConquestStationSeed(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 7, 0))
			{
				GamePatch.CheckEconomy(player);
				GamePatch.AddConquestShop(player);
				GamePatch.AddConquestRefinery(player);
				Conquest storyteller = player.GetStoryteller<Conquest>();
				if (storyteller != null)
				{
					storyteller.patchConquestTick = true;
				}
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 7, 1))
			{
				GamePatch.AddConquestIntroMission(player);
				GamePatch.AddEmbassyRepresentatives(player);
				if (player.GetStoryteller<Sandbox>() != null)
				{
					GamePatch.AddSidequests(player);
				}
				Conquest storyteller2 = player.GetStoryteller<Conquest>();
				if (storyteller2 != null)
				{
					GamePatch.SetTier2Unlocked(player);
					storyteller2.patchConquestTick = true;
				}
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 7, 2))
			{
				GamePatch.AddConquestCommanders(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 7, 3))
			{
				GamePatch.AddUmbralOutpost(player);
				GamePatch.TryAddUmbralMission(player);
				if (player.GetStoryteller<Conquest>() != null)
				{
					GamePatch.AddConquestSidequests(player);
				}
				GamePatch.ClearMercenaries(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 7, 7, 5))
			{
				GamePatch.AddSidequests(player);
				GamePatch.TryAddDarkspaceMission(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 3))
			{
				GamePatch.FixUmbralMission10Character(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 4))
			{
				GamePatch.FixEliminateMeridiaMission(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 6))
			{
				GamePatch.AddSidequests(player);
				GamePatch.AddMercenaryStationsPatch(player);
				GamePatch.GrantPersonalHistoryTitle(player);
				GamePatch.FixDuplicateSystemNames(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 9))
			{
				GamePatch.AddConquestMiningGuildPOIs(player);
				GamePatch.AddConquestSalvageGuildPOIs(player);
				GamePatch.RegenerateSystemOreData(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 10))
			{
				GamePatch.FixStackedEquipmentItems(player);
			}
			if (GameVersion.IsBelow(SaveGame.loadedVersion, 0, 8, 0, 12))
			{
				GamePatch.FixStoryMissionUnitFactions(player);
			}
			GamePatch.ResetDemoPosition(player);
			GamePlayer.current = gamePlayer;
			return player;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00034F3C File Offset: 0x0003313C
		public void TransitionTutorialToSandbox()
		{
			this.missions.Clear();
			this.map.ClearSectors();
			Register.ClearVisitedSystems();
			this.homeStation = null;
			this.lastVisitedMiningPOI = null;
			this.lastStation = null;
			for (int i = 0; i < this.refinedStorage.Length; i++)
			{
				this.refinedStorage[i] = 0f;
			}
			this.RemoveStoryteller<Tutorial>();
			this.DiscardClaimItemsFromInventory(Inventory.global);
			foreach (SpaceShipData spaceShipData in this.spaceShips)
			{
				this.DiscardClaimItemsFromInventory(spaceShipData.cargo);
			}
			this.AddStoryteller(new Sandbox(this), true);
			this.AddStoryteller(new Source.Simulation.Story.Puppeteers(this), true);
			this.AddStoryteller(new Economy(this), true);
			JumpGate jumpGate = new JumpGate();
			jumpGate.UnlockJumpgate();
			jumpGate.hasGate = false;
			jumpGate.timeLeft = 5f;
			this.currentSystem.SetupPOI(jumpGate, null, null, 0);
			this.currentSystem.pointsOfInterest.Add(jumpGate);
			this.currentPointOfInterest = jumpGate;
			this.waypoints.Clear();
			this.waypoints.Add(jumpGate);
			SteamAchievement.Trigger("CompletePrologue");
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0003508C File Offset: 0x0003328C
		private void DiscardClaimItemsFromInventory(Inventory inventory)
		{
			foreach (Inventory.InventoryItem inventoryItem in inventory.items)
			{
				if (inventoryItem.item.GetComponent<MiningClaimItem>() || inventoryItem.item.GetComponent<SalvageClaimItem>() || inventoryItem.item.GetComponent<JumpgatePassItem>())
				{
					this.currentSpaceShip.cargo.Remove(inventoryItem, inventoryItem.count);
				}
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00035120 File Offset: 0x00033320
		public void AddStoryteller(Storyteller st, bool setup = true)
		{
			this.storytellers.Add(st);
			if (setup)
			{
				st.SetupNewGame();
			}
			if (GameplayManager.initialized)
			{
				st.Start();
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00035144 File Offset: 0x00033344
		public void RemoveStoryteller<T>() where T : Storyteller
		{
			for (int i = 0; i < this.storytellers.Count; i++)
			{
				if (this.storytellers[i] is default(T))
				{
					this.storytellers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0003518C File Offset: 0x0003338C
		public SpaceShipData GetSpaceShipData(string guid)
		{
			foreach (SpaceShipData spaceShipData in this.spaceShips)
			{
				if (spaceShipData.guid == guid)
				{
					return spaceShipData;
				}
			}
			return null;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000351F0 File Offset: 0x000333F0
		public CrewMemberData GetCrewMember(string guid)
		{
			foreach (CrewMemberData crewMemberData in this.crewMembers)
			{
				if (crewMemberData.guid == guid)
				{
					return crewMemberData;
				}
			}
			return null;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00035254 File Offset: 0x00033454
		public void RemoveHiredMercenary(bool dismissed = false)
		{
			Mercenary hiredMercenary = this.hiredMercenary;
			if (hiredMercenary == null)
			{
				return;
			}
			bool flag = false;
			if (!dismissed && hiredMercenary.timeLeft > 0f && hiredMercenary.repairing)
			{
				this.activeFleet[0].StartFleetRepair(hiredMercenary.repairTime);
			}
			else
			{
				HudManager.Instance.ToggleWingman(true);
				flag = true;
			}
			Debug.Log("Active Fleet: " + this.activeFleet.Count.ToString());
			for (int i = this.activeFleet.Count - 1; i >= 0; i--)
			{
				Mercenary mercenary = this.activeFleet[i].commanderData as Mercenary;
				string[] array = new string[6];
				array[0] = "Check active fleet: ";
				array[1] = this.activeFleet[i].name;
				array[2] = ". merc: ";
				int num = 3;
				Mercenary mercenary2 = mercenary;
				array[num] = ((mercenary2 != null) ? mercenary2.ToString() : null);
				array[4] = ", extra: ";
				array[5] = ((mercenary != null) ? new bool?(mercenary.isExtra) : null).ToString();
				Debug.Log(string.Concat(array));
				Mercenary mercenary3 = this.activeFleet[i].commanderData as Mercenary;
				if (mercenary3 != null && !mercenary3.isExtra)
				{
					SpaceShip spaceShip = this.activeFleet[i].unit as SpaceShip;
					if (spaceShip != null)
					{
						GameplayManager.Instance.RemoveFleetShip(spaceShip);
					}
					if (flag)
					{
						this.activeFleet.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000353DB File Offset: 0x000335DB
		public static void CreateNewGamePlayer(PersonalHistoryData personalHistoryData = null, bool tutorial = false)
		{
			if (personalHistoryData != null)
			{
				GamePlayer.current = new GamePlayer(personalHistoryData, tutorial);
				return;
			}
			GamePlayer.current = new GamePlayer();
			GamePlayer.current.commander = CommanderData.CreateCommander(CommanderSpecialization.Leadership, tutorial);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00035408 File Offset: 0x00033608
		public static void StartNewGame()
		{
			if (GamePlayer.current == null)
			{
				GamePlayer.CreateNewGamePlayer(null, false);
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00035418 File Offset: 0x00033618
		public void SetSpaceShipData(SpaceShipData data)
		{
			this.currentSpaceShip = data;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00035421 File Offset: 0x00033621
		public void RemoveSpaceShipData()
		{
			this.currentSpaceShip = null;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0003542C File Offset: 0x0003362C
		public void UpdateFleetPosition(Vector2 position)
		{
			this.currentSpaceShip.positionData.position = position;
			foreach (SpaceShipData spaceShipData in this.activeFleet)
			{
				spaceShipData.positionData.position = position;
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00035494 File Offset: 0x00033694
		public void GainExperience(int amount)
		{
			this.commander.GiveExperience((float)amount);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000354A4 File Offset: 0x000336A4
		public void AddMissionWithLog(Mission mission)
		{
			foreach (MissionStep missionStep in mission.steps)
			{
				if (missionStep.dynamicPointOfInterest != null)
				{
					missionStep.dynamicPointOfInterest.UpdateLocalPosition(missionStep.dynamicPointOfInterest.system.GetRandomPosition(-20f, 20f, -5f, 5f));
				}
			}
			if (mission.storyId != null)
			{
				this.missions.Insert(0, mission);
			}
			else
			{
				this.missions.Add(mission);
			}
			mission.OnMissionStart();
			if (mission.trackedOnHud && Singleton<FocusedMissionHandler>.Instance)
			{
				Singleton<FocusedMissionHandler>.Instance.SetMission(mission);
			}
			if (mission.enemyFaction != null && !Faction.player.IsEnemy(mission.enemyFaction))
			{
				AlertPopup.ShowQuery(Translation.TranslateOnly("@MissionAtWarDesc", new object[]
				{
					mission.enemyFaction.name
				}), null, null, delegate
				{
					this.atWar.Add(mission.enemyFaction);
				}, null, null, null);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00035604 File Offset: 0x00033804
		public void AddMissionWithLog(string storyMissionId)
		{
			this.AddMissionWithLog(StoryMission.Get(this, storyMissionId));
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00035613 File Offset: 0x00033813
		public bool IsMissionsLimitExceeded()
		{
			return this.missions.Count >= 20;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00035628 File Offset: 0x00033828
		public void Cleanup()
		{
			DialogueManager instance = Singleton<DialogueManager>.Instance;
			if (instance != null)
			{
				instance.Cleanup();
			}
			NotificationManager instance2 = Singleton<NotificationManager>.Instance;
			if (instance2 != null)
			{
				instance2.Cleanup();
			}
			foreach (Storyteller storyteller in this.storytellers)
			{
				storyteller.Cleanup();
			}
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00035698 File Offset: 0x00033898
		public Mission GetActiveStoryMission(string missionId)
		{
			foreach (Mission mission in this.allMissions)
			{
				if (mission.storyId == missionId)
				{
					return mission;
				}
			}
			return null;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000356F4 File Offset: 0x000338F4
		public bool HasStoryMission(string missionId)
		{
			if (missionId == null)
			{
				return false;
			}
			if (this.missionsArchive.Contains(missionId))
			{
				return true;
			}
			using (IEnumerator<Mission> enumerator = this.allMissions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.storyId == missionId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00035764 File Offset: 0x00033964
		public bool AddBlueprint(string blueprint)
		{
			if (this.blueprints.Contains(blueprint))
			{
				return false;
			}
			this.blueprints.Add(blueprint);
			return true;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00035784 File Offset: 0x00033984
		public List<Behaviour.Unit.Drone> GetUnlockedDrones()
		{
			List<Behaviour.Unit.Drone> list = new List<Behaviour.Unit.Drone>();
			list.Add(Behaviour.Unit.Drone.Get("Combat Laser Behaviour.Unit.Drone"));
			list.Add(Behaviour.Unit.Drone.Get("Salvage Behaviour.Unit.Drone Laser"));
			list.Add(Behaviour.Unit.Drone.Get("Mining Behaviour.Unit.Drone Laser"));
			if (SkilltreeNode.dronesUnlockDrones1.isActive)
			{
				list.Add(Behaviour.Unit.Drone.Get("Combat Missile Behaviour.Unit.Drone"));
				list.Add(Behaviour.Unit.Drone.Get("Salvage Behaviour.Unit.Drone Structure"));
				list.Add(Behaviour.Unit.Drone.Get("Mining Behaviour.Unit.Drone Driller"));
			}
			return (from d in list
			orderby d.displayName
			select d).ToList<Behaviour.Unit.Drone>();
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00035830 File Offset: 0x00033A30
		public bool IsPrologueActive()
		{
			using (List<Storyteller>.Enumerator enumerator = this.storytellers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current is Tutorial)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0003588C File Offset: 0x00033A8C
		public Faction GetDefaultEnemyFaction()
		{
			if (Faction.marauders.IsEnemy(Faction.player))
			{
				return Faction.marauders;
			}
			if (Faction.fanatics.IsEnemy(Faction.player))
			{
				return Faction.fanatics;
			}
			if (Faction.holyRadicals.IsEnemy(Faction.player))
			{
				return Faction.holyRadicals;
			}
			return Faction.amalgam;
		}

		// Token: 0x040002E2 RID: 738
		public const float dynamicEventDelay = 450f;

		// Token: 0x040002E3 RID: 739
		public const int MissionLimit = 20;

		// Token: 0x040002E4 RID: 740
		public static GamePlayer current;

		// Token: 0x040002E5 RID: 741
		public readonly List<Storyteller> storytellers = new List<Storyteller>();

		// Token: 0x040002E6 RID: 742
		public bool autoPlay;

		// Token: 0x040002E7 RID: 743
		public AutopilotSettings autopilotSettings;

		// Token: 0x040002E8 RID: 744
		public AutopilotSessionStats currentAutopilotSessionStats;

		// Token: 0x040002E9 RID: 745
		public readonly List<AutopilotSessionStats> autopilotSessionStats = new List<AutopilotSessionStats>();

		// Token: 0x040002EA RID: 746
		public bool forgeDepositInCargo = true;

		// Token: 0x040002EB RID: 747
		public long credits = 10000L;

		// Token: 0x040002EC RID: 748
		public string starterSpaceshipName = "";

		// Token: 0x040002ED RID: 749
		public int starterSpecialization;

		// Token: 0x040002F0 RID: 752
		public readonly List<SpaceShipData> spaceShips = new List<SpaceShipData>();

		// Token: 0x040002F1 RID: 753
		public readonly List<CrewMemberData> crewMembers = new List<CrewMemberData>();

		// Token: 0x040002F2 RID: 754
		public CrewData unassignedCrew = new CrewData();

		// Token: 0x040002F5 RID: 757
		public readonly HashSet<Faction> atWar = new HashSet<Faction>();

		// Token: 0x040002F6 RID: 758
		public readonly List<Mission> missions = new List<Mission>();

		// Token: 0x040002F7 RID: 759
		public BountyMission currentBounty;

		// Token: 0x040002F8 RID: 760
		public PatrolMission currentPatrol;

		// Token: 0x040002F9 RID: 761
		public IndustryMission currentIndustry;

		// Token: 0x040002FA RID: 762
		public readonly List<string> missionsArchive = new List<string>();

		// Token: 0x040002FC RID: 764
		public readonly List<string> blueprints = new List<string>();

		// Token: 0x040002FE RID: 766
		public SystemMapData currentSystem;

		// Token: 0x040002FF RID: 767
		public Vector2 mapPosition;

		// Token: 0x04000300 RID: 768
		public MapPointOfInterest currentPointOfInterest;

		// Token: 0x04000301 RID: 769
		public List<MapPointOfInterest> waypoints = new List<MapPointOfInterest>();

		// Token: 0x04000302 RID: 770
		public MapPointOfInterest lastVisitedMiningPOI;

		// Token: 0x04000303 RID: 771
		public SpaceStation homeStation;

		// Token: 0x04000304 RID: 772
		public SpaceStation lastStation;

		// Token: 0x04000306 RID: 774
		public float dynamicEventTimer;

		// Token: 0x04000307 RID: 775
		public float extraMercenarySpawnTimer;

		// Token: 0x04000308 RID: 776
		public float extraMercenarySpawnCheckTimer = 10f;

		// Token: 0x0400030D RID: 781
		public int bountyRank;

		// Token: 0x0400030E RID: 782
		public int patrolRank;

		// Token: 0x0400030F RID: 783
		public int industryRank;

		// Token: 0x04000310 RID: 784
		public int maxBountyLevel;

		// Token: 0x04000311 RID: 785
		public int maxPatrolLevel;

		// Token: 0x04000312 RID: 786
		public int maxIndustryLevel;

		// Token: 0x04000317 RID: 791
		public long salvageWorkshopLastUpdateTime;

		// Token: 0x04000318 RID: 792
		public string salvageWorkshopDailyItem;

		// Token: 0x04000319 RID: 793
		public bool recreateItems;

		// Token: 0x0400031A RID: 794
		public bool holdPosition;

		// Token: 0x0400031B RID: 795
		public bool useWarpFuel = true;

		// Token: 0x0400031C RID: 796
		public bool emergencyJump;

		// Token: 0x0400031D RID: 797
		public bool hasUmbralTransponder;

		// Token: 0x0400031E RID: 798
		public readonly List<SpaceShipData> activeFleet = new List<SpaceShipData>();

		// Token: 0x0400031F RID: 799
		private float[] refinedStorage = new float[Enum.GetValues(typeof(RefinedMaterial)).Length];

		// Token: 0x04000320 RID: 800
		public readonly List<string> abilitySlots = new List<string>();

		// Token: 0x04000321 RID: 801
		public HashSet<string> unlockedTitles = new HashSet<string>();

		// Token: 0x04000322 RID: 802
		public HashSet<string> unlockedDecals = new HashSet<string>();

		// Token: 0x04000323 RID: 803
		public Dictionary<string, float> abilityCooldowns = new Dictionary<string, float>();

		// Token: 0x04000324 RID: 804
		public Dictionary<string, ActiveEffectData> activeEffects = new Dictionary<string, ActiveEffectData>();

		// Token: 0x04000325 RID: 805
		public HashSet<string> activeToggles = new HashSet<string>();
	}
}
