using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.GalaxyMap;
using Behaviour.Hazard;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.Util;
using LightJson;
using Source.Combat;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.Story
{
	// Token: 0x02000087 RID: 135
	public class Conquest : Sandbox
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00027959 File Offset: 0x00025B59
		public override int maxLootBoxSkillPoints
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0002795D File Offset: 0x00025B5D
		public override int maxPlayerLevel
		{
			get
			{
				return 60;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00027961 File Offset: 0x00025B61
		public override int maxBonusSkillpoints
		{
			get
			{
				return 61;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00027965 File Offset: 0x00025B65
		public override int maxReputation
		{
			get
			{
				return ReputationLevel.Distinguished.GetReputationThreshold();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0002796E File Offset: 0x00025B6E
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00027976 File Offset: 0x00025B76
		public List<ConquestSystem> systems { get; private set; }

		// Token: 0x060004B7 RID: 1207 RVA: 0x0002797F File Offset: 0x00025B7F
		public Conquest(GamePlayer ply) : base(ply)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00027994 File Offset: 0x00025B94
		public override void SetupNewGame()
		{
			ConquestWorld.SetupWorld(this.player);
			SeededRandom global = SeededRandom.Global;
			Faction conquestFaction = ConquestWorld.sectorOwners[0];
			Faction conquestFaction2 = ConquestWorld.sectorOwners[1];
			Faction conquestFaction3 = ConquestWorld.sectorOwners[2];
			global.RandomRange(-20, 0);
			global.RandomRange(-20, 0);
			SystemMapData systemMapData = this.FindClosestSystem(new Vector2(global.RandomRange(-10f, 10f), 20f));
			SystemMapData systemMapData2 = this.FindClosestSystem(new Vector2(-40f, 0f));
			SystemMapData systemMapData3 = this.FindClosestSystem(new Vector2(global.RandomRange(-10f, 10f), -20f));
			SystemMapData systemMapData4 = this.FindClosestSystem(new Vector2(40f, 20f));
			SystemMapData systemMapData5 = null;
			foreach (SystemMapData systemMapData6 in ConquestWorld.conquestSector.allSystems)
			{
				foreach (JumpGate jumpGate in systemMapData6.GetPointsOfInterest<JumpGate>())
				{
					if (jumpGate.sectorJumpgate && jumpGate.targetSystem.level == 60)
					{
						systemMapData5 = systemMapData6;
						(systemMapData6.storyteller as ConquestSystem).SetHeadquarters(true);
						break;
					}
				}
			}
			systemMapData.SetConquestFaction(conquestFaction);
			systemMapData2.SetConquestFaction(conquestFaction2);
			systemMapData3.SetConquestFaction(conquestFaction3);
			systemMapData4.SetConquestFaction(Faction.fanatics);
			systemMapData5.SetConquestFaction(Faction.darkspacers);
			this.SetupStartSystem(systemMapData, 30f);
			this.SetupStartSystem(systemMapData2, 30f);
			this.SetupStartSystem(systemMapData3, 30f);
			this.SetupStartSystem(systemMapData4, 10f);
			this.SetupStartSystem(systemMapData5, 10f);
			this.player.GetStoryteller<Economy>().GenerateEconomy();
			this.PopulateSystems();
			for (int i = 0; i < 9; i++)
			{
				this.DoConquestTick(true);
			}
			this.DoConquestTick(false);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00027BB8 File Offset: 0x00025DB8
		private void SetupStartSystem(SystemMapData system, float initialStrength)
		{
			ConquestSystem conquestSystem = system.storyteller as ConquestSystem;
			conquestSystem.combatStrength += initialStrength;
			conquestSystem.controlLevel = 2;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00027BDC File Offset: 0x00025DDC
		private SystemMapData FindClosestSystem(Vector2 pos)
		{
			SystemMapData systemMapData = null;
			float num = 0f;
			foreach (SystemMapData systemMapData2 in ConquestWorld.conquestSector.allSystems)
			{
				float num2 = Vector2.Distance(pos, systemMapData2.position);
				if (systemMapData == null || num2 < num)
				{
					systemMapData = systemMapData2;
					num = num2;
				}
			}
			return systemMapData;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00027C4C File Offset: 0x00025E4C
		public override void Start()
		{
			this.PopulateSystems();
			if (this.conquestTickTime <= 0f)
			{
				this.conquestTickTime = 3600f;
			}
			int num = 0;
			int num2 = this.lastConquestTick.DayOfYear;
			int num3 = 0;
			while (num3 < 5 && num2 != DateTime.Now.DayOfYear)
			{
				num++;
				num2++;
				num3++;
			}
			if (num == 0 && this.patchConquestTick)
			{
				num = 1;
			}
			if (num > 0)
			{
				this.CleanupEvents();
				for (int i = 1; i < num; i++)
				{
					this.DoConquestTick(true);
				}
				this.DoConquestTick(false);
			}
			foreach (Faction faction in Conquest.playerPopulatingFactions)
			{
				EmbassyStation embassy = this.GetEmbassy(faction);
				string text = null;
				if (faction == Faction.miningGuild)
				{
					text = "QuestgiverMiningToConquest";
				}
				else if (faction == Faction.salvageGuild)
				{
					text = "QuestgiverSalvageToConquest";
				}
				else if (faction == Faction.marauders)
				{
					text = "QuestgiverMaraudersToConquest";
				}
				else if (faction == Faction.stranded)
				{
					text = "QuestgiverStrandedToConquest";
				}
				if (embassy != null && text != null)
				{
					this.EnsureEmbassyCharacter(embassy, text);
				}
			}
			EmbassyStation embassy2 = this.GetEmbassy(Faction.policeGuild);
			if (embassy2 != null)
			{
				embassy2.shipyard = null;
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00027D9C File Offset: 0x00025F9C
		private void EnsureEmbassyCharacter(EmbassyStation embassy, string character)
		{
			if (!embassy.characters.Contains(character))
			{
				embassy.characters.Add(character);
			}
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00027DB8 File Offset: 0x00025FB8
		private void PopulateSystems()
		{
			this.systems = new List<ConquestSystem>();
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				if (sectorMapData.quadrant == SectorMapData.quadrantConquest && sectorMapData.conquestSector)
				{
					this.conquestSector = sectorMapData;
					break;
				}
			}
			if (this.conquestSector != null)
			{
				foreach (SystemMapData systemMapData in this.conquestSector.allSystems)
				{
					ConquestSystem conquestSystem = systemMapData.storyteller as ConquestSystem;
					if (conquestSystem != null)
					{
						this.systems.Add(conquestSystem);
						if (conquestSystem.connectedSystems == null)
						{
							conquestSystem.Start();
						}
					}
				}
			}
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00027E94 File Offset: 0x00026094
		public override void StoryUpdate(float delta)
		{
			this.conquestTickTime -= delta;
			if (this.conquestTickTime < 0f)
			{
				this.DoConquestTick(false);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00027EB8 File Offset: 0x000260B8
		public void DoConquestTick(bool inSetup = false)
		{
			SeededRandom global = SeededRandom.Global;
			this.lastConquestTick = DateTime.Now;
			this.conquestTickTime = 3600f;
			global.Shuffle<ConquestSystem>(this.systems);
			foreach (ConquestSystem conquestSystem in this.systems)
			{
				global.Shuffle<ConquestSystem>(conquestSystem.connectedSystems);
			}
			Dictionary<ConquestSystem, ConquestSystem> dictionary = new Dictionary<ConquestSystem, ConquestSystem>();
			foreach (ConquestSystem conquestSystem2 in this.systems)
			{
				ConquestSystem conquestSystem3 = null;
				foreach (ConquestSystem conquestSystem4 in conquestSystem2.connectedSystems)
				{
					if (conquestSystem2.faction != conquestSystem4.faction && conquestSystem2.combatStrength > conquestSystem4.combatStrength && !dictionary.ContainsValue(conquestSystem4) && (conquestSystem3 == null || conquestSystem3.combatStrength > conquestSystem4.combatStrength))
					{
						conquestSystem3 = conquestSystem4;
					}
				}
				if (conquestSystem3 != null && (!conquestSystem3.headquarters || conquestSystem3.combatStrength <= 0f))
				{
					dictionary.Add(conquestSystem2, conquestSystem3);
				}
			}
			foreach (KeyValuePair<ConquestSystem, ConquestSystem> keyValuePair in dictionary)
			{
				if (keyValuePair.Key.combatStrength > keyValuePair.Value.combatStrength)
				{
					if (keyValuePair.Value.playerControlLevel > 0 || SystemMapData.current == keyValuePair.Value.system)
					{
						keyValuePair.Value.playerControlLevel = Mathf.Max(0, keyValuePair.Value.playerControlLevel - 1);
						keyValuePair.Key.combatStrength -= keyValuePair.Value.combatStrength / global.RandomRange(2f, 2.5f);
						keyValuePair.Value.combatStrength -= keyValuePair.Value.combatStrength / global.RandomRange(1.5f, 2.5f);
					}
					else if (keyValuePair.Value.controlLevel > 0)
					{
						keyValuePair.Value.controlLevel--;
						keyValuePair.Key.combatStrength -= keyValuePair.Value.combatStrength / global.RandomRange(2f, 2.5f);
						keyValuePair.Value.combatStrength -= keyValuePair.Value.combatStrength / global.RandomRange(1.5f, 2.5f);
					}
					else
					{
						keyValuePair.Key.combatStrength -= keyValuePair.Value.combatStrength / global.RandomRange(1f, 1.5f);
						if (keyValuePair.Value.headquarters)
						{
							this.GetFactionStanding(keyValuePair.Value.faction).rejoinConquestCooldown = 5;
						}
						keyValuePair.Value.system.SetConquestFaction(keyValuePair.Key.faction);
						keyValuePair.Value.SetHeadquarters(false);
						keyValuePair.Value.controlLevel = 1;
						keyValuePair.Value.combatStrength = keyValuePair.Key.combatStrength / 2f;
						keyValuePair.Key.combatStrength = keyValuePair.Value.combatStrength;
					}
				}
			}
			foreach (ConquestSystem conquestSystem5 in this.systems)
			{
				if (conquestSystem5.controlLevel < 2)
				{
					bool flag = true;
					using (List<ConquestSystem>.Enumerator enumerator2 = conquestSystem5.connectedSystems.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.faction != conquestSystem5.faction)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag || dictionary.ContainsKey(conquestSystem5))
					{
						conquestSystem5.controlLevel++;
					}
				}
			}
			foreach (ConquestSystem conquestSystem6 in this.systems)
			{
				conquestSystem6.baseReinforcements = Mathf.Clamp(conquestSystem6.baseReinforcements + (float)(global.RandomBool(0.5f) ? 1 : -1), 0f, 4f);
				conquestSystem6.combatStrength += conquestSystem6.totalReinforcements;
				if (!conquestSystem6.headquarters && !dictionary.ContainsKey(conquestSystem6) && !dictionary.ContainsValue(conquestSystem6))
				{
					conquestSystem6.combatStrength -= (float)Mathf.FloorToInt(conquestSystem6.combatStrength / 10f);
				}
			}
			foreach (ConquestSystem conquestSystem7 in this.systems)
			{
				if (conquestSystem7.controlLevel >= 2)
				{
					foreach (ConquestSystem conquestSystem8 in conquestSystem7.connectedSystems)
					{
						if (conquestSystem8.faction == conquestSystem7.faction && conquestSystem8.combatStrength < conquestSystem7.combatStrength)
						{
							float combatStrength = (conquestSystem8.combatStrength + conquestSystem7.combatStrength) / 2f;
							conquestSystem8.combatStrength = combatStrength;
							conquestSystem7.combatStrength = combatStrength;
						}
					}
				}
			}
			foreach (SectorMapData sectorMapData in this.player.map.allSectors)
			{
				if (sectorMapData.conquestSector)
				{
					foreach (SystemMapData systemMapData in sectorMapData.allSystems)
					{
						foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
						{
							SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
							if (spaceStation != null && MapPointOfInterest.current != mapPointOfInterest)
							{
								mapPointOfInterest.CleanupConquestStation();
								spaceStation.conquestStationInitialized = false;
							}
						}
					}
				}
			}
			bool flag2 = SeededRandom.Global.RandomBool(0.5f);
			foreach (ConquestSystem conquestSystem9 in this.systems)
			{
				if (conquestSystem9.umbralControlLevel == 1f && !flag2)
				{
					using (List<ConquestSystem>.Enumerator enumerator2 = conquestSystem9.connectedSystems.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ConquestSystem conquestSystem10 = enumerator2.Current;
							if (conquestSystem10.umbralControlLevel == 0f)
							{
								conquestSystem10.umbralControlLevel = 0.05f;
								flag2 = true;
								break;
							}
						}
						continue;
					}
				}
				if (conquestSystem9.umbralControlLevel > 0f)
				{
					conquestSystem9.umbralControlLevel = Mathf.Clamp01(conquestSystem9.umbralControlLevel + global.RandomRange(0.05f, 0.1f));
				}
			}
			foreach (Faction faction in Conquest.conquestFactions)
			{
				bool flag3 = false;
				ConquestSystem conquestSystem11 = null;
				int num = 0;
				foreach (ConquestSystem conquestSystem12 in this.systems)
				{
					if (conquestSystem12.faction == faction)
					{
						num++;
						if (conquestSystem12.headquarters && flag3)
						{
							conquestSystem12.SetHeadquarters(false);
						}
						else if (conquestSystem12.headquarters)
						{
							flag3 = true;
						}
						else if (conquestSystem11 == null || conquestSystem12.combatStrength > conquestSystem11.combatStrength)
						{
							conquestSystem11 = conquestSystem12;
						}
					}
				}
				if (!flag3 && conquestSystem11 != null)
				{
					conquestSystem11.SetHeadquarters(true);
				}
				ConquestFactionStanding conquestFactionStanding = this.GetFactionStanding(faction);
				conquestFactionStanding.currentConquestArea = num;
				conquestFactionStanding.maxConquestArea = Mathf.Max(conquestFactionStanding.maxConquestArea, num);
				conquestFactionStanding.currentConqueredPercentage = (float)num / (float)this.systems.Count;
			}
			if (!inSetup)
			{
				foreach (KeyValuePair<ConquestSystem, ConquestSystem> keyValuePair2 in dictionary)
				{
					if (keyValuePair2.Key.faction != keyValuePair2.Value.faction)
					{
						for (int i = 0; i < 3; i++)
						{
							if (global.RandomBool(0.5f))
							{
								this.AddFactionSkirmish(keyValuePair2.Value.system, keyValuePair2.Key.faction, keyValuePair2.Value.faction);
							}
						}
					}
				}
				foreach (ConquestSystem sys in this.systems)
				{
					if (global.RandomBool(0.5f))
					{
						this.AddRandomEncounter(sys, global);
					}
				}
				int num2 = 0;
				foreach (ConquestSystem conquestSystem13 in this.systems)
				{
					num2 = Mathf.Max(num2, conquestSystem13.system.level);
				}
				foreach (Faction faction2 in Conquest.conquestFactions)
				{
					EmbassyStation embassy = this.GetEmbassy(faction2);
					ConquestSystem headquarters = this.GetHeadquarters(faction2);
					if (embassy != null)
					{
						embassy.level = Mathf.Max(embassy.level, num2 - 1);
					}
					bool flag4 = Conquest.autoPopulatingFactions.Contains(faction2);
					if (flag4 && embassy != null && headquarters == null)
					{
						embassy.combatStrength += 10f;
					}
					if (headquarters == null && embassy != null && embassy.combatStrength > 0f && flag4)
					{
						ConquestSystem potentialRejoinHQ = this.GetPotentialRejoinHQ(faction2);
						if (potentialRejoinHQ != null)
						{
							this.JoinConquestSector(faction2, potentialRejoinHQ, false);
						}
					}
				}
				foreach (ConquestSystem conquestSystem14 in this.systems)
				{
					EmbassyJumpgate pointOfInterest = conquestSystem14.system.GetPointOfInterest<EmbassyJumpgate>();
					if (pointOfInterest != null && conquestSystem14.headquarters)
					{
						EmbassyJumpgate embassyJumpgate = this.GetEmbassyJumpgate(conquestSystem14.system.faction);
						if (embassyJumpgate != null)
						{
							embassyJumpgate.name = Translation.Translate("@GateToEmbassy", new object[]
							{
								conquestSystem14.system.faction.name
							});
							embassyJumpgate.faction = conquestSystem14.system.faction;
							embassyJumpgate.LinkJumpgate(pointOfInterest, true);
						}
					}
					else if (pointOfInterest != null)
					{
						conquestSystem14.system.RemovePointOfInterest(pointOfInterest);
					}
				}
				foreach (Faction faction3 in Conquest.conquestFactions)
				{
					ConquestSystem headquarters2 = this.GetHeadquarters(faction3);
					EmbassyJumpgate embassyJumpgate2 = this.GetEmbassyJumpgate(faction3);
					if (headquarters2 != null)
					{
						if (headquarters2.system.GetPointOfInterest<EmbassyJumpgate>() == null && embassyJumpgate2 != null)
						{
							ConquestStation pointOfInterest2 = headquarters2.system.GetPointOfInterest<ConquestStation>();
							EmbassyJumpgate embassyJumpgate3 = new EmbassyJumpgate
							{
								system = headquarters2.system,
								faction = faction3,
								level = headquarters2.system.level,
								name = Translation.Translate("@GateToEmbassy", new object[]
								{
									headquarters2.system.faction.name
								}),
								position = headquarters2.system.GetRandomPosition(pointOfInterest2.position.x - 4f, pointOfInterest2.position.x - 3f, pointOfInterest2.position.y - 2f, pointOfInterest2.position.y + 2f),
								hidden = true
							};
							headquarters2.system.pointsOfInterest.Add(embassyJumpgate3);
							embassyJumpgate3.LinkJumpgate(embassyJumpgate2, true);
						}
					}
					else if (embassyJumpgate2 != null)
					{
						embassyJumpgate2.hidden = true;
						embassyJumpgate2.LockGate();
					}
				}
				foreach (ConquestSystem conquestSystem15 in this.systems)
				{
					int num3 = GamePlayer.current.level - conquestSystem15.system.level;
					if (num3 > 3)
					{
						conquestSystem15.system.UpdateLevel(conquestSystem15.system.level + 2);
					}
					else if (num3 > 0)
					{
						conquestSystem15.system.UpdateLevel(conquestSystem15.system.level + 1);
					}
					else if (num3 > -4 && SeededRandom.Global.RandomBool(0.5f))
					{
						conquestSystem15.system.UpdateLevel(conquestSystem15.system.level + 1);
					}
				}
				AbstractGalaxyMapManager current = AbstractGalaxyMapManager.current;
				if (current && !current.tweening && current.zoomLevel == 1 && current.currentSector == this.conquestSector)
				{
					current.RefreshSectorMap(this.conquestSector);
				}
				if (GamePlayer.current.waypoints.Count > 0 && Singleton<TravelManager>.Instance && Singleton<TravelManager>.Instance.targetPoi != null)
				{
					bool flag5 = false;
					foreach (MapPointOfInterest mapPointOfInterest2 in GamePlayer.current.waypoints)
					{
						JumpGate jumpGate = mapPointOfInterest2 as JumpGate;
						if (jumpGate != null && !jumpGate.canUseJumpGate)
						{
							flag5 = true;
						}
					}
					Debug.Log("reroute:" + flag5.ToString());
					if (flag5)
					{
						Singleton<TravelManager>.Instance.SetRouteToPOI(Singleton<TravelManager>.Instance.targetPoi);
					}
				}
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00028F14 File Offset: 0x00027114
		public ConquestSystem GetPotentialRejoinHQ(Faction faction)
		{
			ConquestSystem result = null;
			EmbassyStation embassy = this.GetEmbassy(faction);
			foreach (ConquestSystem conquestSystem in this.systems)
			{
				if (conquestSystem.combatStrength < embassy.combatStrength / 2f)
				{
					result = conquestSystem;
					break;
				}
			}
			return result;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00028F84 File Offset: 0x00027184
		public void JoinConquestSector(Faction faction, ConquestSystem hqSystem, bool force = false)
		{
			ConquestFactionStanding conquestFactionStanding = this.GetFactionStanding(faction);
			EmbassyStation embassy = this.GetEmbassy(faction);
			if (conquestFactionStanding.rejoinConquestCooldown > 0 && !force)
			{
				conquestFactionStanding.rejoinConquestCooldown--;
				return;
			}
			hqSystem.system.SetConquestFaction(faction);
			hqSystem.SetHeadquarters(true);
			hqSystem.combatStrength = embassy.combatStrength;
			embassy.combatStrength = 0f;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00028FE8 File Offset: 0x000271E8
		private void AddRandomEncounter(ConquestSystem sys, SeededRandom random)
		{
			int num = random.RandomRange(0, 3);
			if (num == 1)
			{
				this.AddRogueAsteroid(sys.system);
				return;
			}
			if (num == 2)
			{
				this.AddBattleWreckage(sys.system);
				return;
			}
			this.AddFactionSkirmish(sys.system, sys.faction, random.RandomBool(0.5f) ? Faction.marauders : Faction.fanatics);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0002904C File Offset: 0x0002724C
		private void AddRogueAsteroid(SystemMapData system)
		{
			AsteroidFieldData asteroidFieldData = new AsteroidFieldData(SeededRandom.Global.RandomRange(4, 6), 0.5f, 80f, AsteroidFieldData.CreateOreSet(system.level, true), AsteroidFieldData.CreateOreSet(system.level, false), 1f);
			Faction faction = system.faction;
			AsteroidFieldData customFieldData = asteroidFieldData;
			Mining mining = system.AddMiningPoi(faction, null, customFieldData, 0f, false, 0f);
			mining.name = Translation.Translate("@ConquestEventAsteroid", Array.Empty<object>());
			mining.timeLeft = SeededRandom.Global.RandomRange(2700f, 3240f);
			mining.ClearPersistables();
			mining.InitializeAsteroids(false, false);
			mining.AddGuards(mining.CreateUnitPayload(0.65f, new GameplayType?(GameplayType.Mining), Faction.miningGuild, 0, 0, 1, 5, null), null).ForEach(delegate(AbstractUnitData ship)
			{
				ship.autoActions = "AmbientMiner";
			});
			float pointsScale = 0.7f;
			Faction faction2 = system.faction;
			mining.AddTriggeredSpawn(mining.CreateUnitPayload(pointsScale, new GameplayType?(GameplayType.Combat), faction2, 0, 0, 1, 5, null), 8f, 0, false, true);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00029178 File Offset: 0x00027378
		private void AddBattleWreckage(SystemMapData system)
		{
			SeededRandom global = SeededRandom.Global;
			Salvage salvage = new Salvage
			{
				name = Translation.Translate("@ConquestEventSalvage", Array.Empty<object>()),
				timeLeft = SeededRandom.Global.RandomRange(2700f, 3240f)
			};
			system.SetupPOI(salvage, null, null, 0);
			Faction faction = system.faction.RandomEnemyFaction(global);
			SalvageData salvageData = new SalvageData
			{
				position = salvage.GetWorldPosition() + new Vector2(global.RandomRange(9f, 14f), global.RandomRange(-12f, 12f)),
				angle = (float)global.RandomRange(0, 360),
				shipTemplate = faction.GetRandomNPCShipType(system.level, 250, 500, new GameplayType?(GameplayType.Combat)),
				initialBattleDamage = 80,
				hazardData = salvage.CreateHazardData(HazardName.DamageInRadius, DamageType.Radiation)
			};
			salvageData.AddItemContent(salvage.level, 3, 2f);
			salvageData.AddScrapContent(salvage.level, 2f, 2);
			salvageData.AddStructuralContent(salvage.level, 2, 1f);
			salvage.AddPersistable(salvageData);
			for (int i = 0; i < SeededRandom.Global.RandomRange(5, 8); i++)
			{
				Vector2 normalized = new Vector2(global.RandomRange(-1f, 1f), global.RandomRange(-1f, 1f)).normalized;
				SalvageData salvageData2 = new SalvageData
				{
					position = salvageData.position + normalized * global.RandomRange(12f, 18f),
					angle = (float)global.RandomRange(0, 360),
					initialBattleDamage = 40,
					shipTemplate = system.faction.GetRandomNPCShipType(system.level, 50, 200, new GameplayType?(GameplayType.Combat))
				};
				salvageData2.AddScrapContent(salvage.level, 2f, 2);
				salvageData2.AddStructuralContent(salvage.level, 2, 1f);
				salvage.AddPersistable(salvageData2);
			}
			salvage.AddGuards(salvage.CreateUnitPayload(0.65f, new GameplayType?(GameplayType.Salvage), Faction.salvageGuild, 0, 0, 1, 5, null), null);
			salvage.hazardsDescription = Translation.Translate("@MapPOIDangerRadiation", Array.Empty<object>());
			system.pointsOfInterest.Add(salvage);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000293EC File Offset: 0x000275EC
		private void AddFactionSkirmish(SystemMapData system, Faction fA, Faction fB)
		{
			if (fA == fB || !fA.IsEnemy(fB))
			{
				fB = ((fA == Faction.marauders) ? Faction.fanatics : Faction.marauders);
			}
			Combat combat = new Combat();
			system.SetupPOI(combat, null, null, 0);
			combat.name = Translation.Translate("@ConquestEventSkirmish", new object[]
			{
				fB.name
			});
			system.pointsOfInterest.Add(combat);
			combat.timeLeft = SeededRandom.Global.RandomRange(2700f, 3240f);
			this.AddSkirmishToPoi(combat, fA, fB);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00029484 File Offset: 0x00027684
		private void AddSkirmishToPoi(MapPointOfInterest poi, Faction fA, Faction fB)
		{
			Vector2 worldPosition = poi.GetWorldPosition();
			List<AbstractUnitData> list = poi.CreateUnitPayload(1f, new GameplayType?(GameplayType.Combat), fA, 0, 0, 1, 5, null);
			List<AbstractUnitData> list2 = poi.CreateUnitPayload(1.5f, new GameplayType?(GameplayType.Combat), fB, 0, 0, 1, 5, null);
			float num = -5f;
			foreach (AbstractUnitData abstractUnitData in list)
			{
				abstractUnitData.positionData.position = worldPosition + new Vector2(-10f, num);
				num += 5f;
				poi.AddUnit(abstractUnitData, null, false);
			}
			num = -5f;
			foreach (AbstractUnitData abstractUnitData2 in list2)
			{
				abstractUnitData2.positionData.position = worldPosition + new Vector2(10f, num);
				num += 5f;
				poi.AddUnit(abstractUnitData2, null, false);
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000295BC File Offset: 0x000277BC
		public EmbassyStation GetEmbassy(Faction f)
		{
			if (f == Faction.fanatics && !GamePlayer.current.missionsArchive.Contains("ConquestDarkspace8"))
			{
				if (!GamePlayer.current.missions.Any((Mission m) => m.storyId == "ConquestDarkspace8"))
				{
					return new EmbassyStation
					{
						combatStrength = 40f
					};
				}
			}
			foreach (SectorMapData sectorMapData in GalaxyMapData.current.allSectors)
			{
				if (sectorMapData.quadrant == SectorMapData.quadrantConquest)
				{
					foreach (SystemMapData systemMapData in sectorMapData.allSystems)
					{
						if (systemMapData.faction == f)
						{
							EmbassyStation pointOfInterest = systemMapData.GetPointOfInterest<EmbassyStation>();
							if (pointOfInterest != null)
							{
								return pointOfInterest;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000296C8 File Offset: 0x000278C8
		public EmbassyJumpgate GetEmbassyJumpgate(Faction f)
		{
			EmbassyStation embassy = this.GetEmbassy(f);
			if (embassy != null && embassy.system != null)
			{
				return embassy.system.GetPointOfInterest<EmbassyJumpgate>();
			}
			return null;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x000296F8 File Offset: 0x000278F8
		public ConquestSystem GetHeadquarters(Faction f)
		{
			foreach (ConquestSystem conquestSystem in this.systems)
			{
				if (conquestSystem.faction == f && conquestSystem.headquarters)
				{
					return conquestSystem;
				}
			}
			return null;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0002975C File Offset: 0x0002795C
		public void CleanupEvents()
		{
			List<MapPointOfInterest> list = new List<MapPointOfInterest>();
			foreach (SystemMapData systemMapData in this.conquestSector.allSystems)
			{
				list.Clear();
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					if (mapPointOfInterest.timeLeft > 0f && !PoiBeaconItem.HasPoiBeacon(mapPointOfInterest))
					{
						list.Add(mapPointOfInterest);
					}
				}
				foreach (MapPointOfInterest poi in list)
				{
					systemMapData.RemovePointOfInterest(poi);
				}
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00029854 File Offset: 0x00027A54
		public ConquestFactionStanding GetFactionStanding(Faction f)
		{
			ConquestFactionStanding conquestFactionStanding;
			this.factionStanding.TryGetValue(f, out conquestFactionStanding);
			if (conquestFactionStanding == null)
			{
				conquestFactionStanding = new ConquestFactionStanding(f);
				this.factionStanding.Add(f, conquestFactionStanding);
			}
			return conquestFactionStanding;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00029888 File Offset: 0x00027A88
		public override void DataToJson(JsonObject data)
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<Faction, ConquestFactionStanding> keyValuePair in this.factionStanding)
			{
				jsonObject[keyValuePair.Key.identifier] = keyValuePair.Value.ToJson();
			}
			data["lastConquestTick"] = this.lastConquestTick.Ticks.ToString();
			data["conquestTickTime"] = new double?((double)this.conquestTickTime);
			data["factionStanding"] = jsonObject;
			data["umbralContribution"] = new double?((double)this.umbralContribution);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00029964 File Offset: 0x00027B64
		public override void DataFromJson(JsonObject data)
		{
			this.lastConquestTick = new DateTime(long.Parse(data["lastConquestTick"]));
			this.conquestTickTime = (float)data["conquestTickTime"].AsNumber;
			this.umbralContribution = data["umbralContribution"];
			if (data["factionStanding"].IsJsonObject)
			{
				foreach (KeyValuePair<string, JsonValue> keyValuePair in data["factionStanding"].AsJsonObject)
				{
					Faction faction = Faction.Get(keyValuePair.Key);
					this.factionStanding[faction] = ConquestFactionStanding.FromJson(faction, keyValuePair.Value);
				}
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00029A44 File Offset: 0x00027C44
		public void UpdateSkillPointPrice()
		{
			InventoryItemType y = InventoryItemType.Get("BonusSkillPointTemplate");
			foreach (MapPointOfInterest mapPointOfInterest in GalaxyMapData.current.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && spaceStation.conquestShopInventory != null)
				{
					foreach (Inventory.InventoryItem inventoryItem in spaceStation.conquestShopInventory.items)
					{
						if (inventoryItem.item == y && inventoryItem.costItem != null)
						{
							inventoryItem.costCount = BonusSkillPoint.GetConquestValue();
						}
					}
				}
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00029B10 File Offset: 0x00027D10
		public float GetUmbralControlPercentage(bool fullControl = true)
		{
			return (float)(this.GetUmbralControlledStations(fullControl) / this.systems.Count);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00029B28 File Offset: 0x00027D28
		public int GetUmbralControlledStations(bool fullControl = true)
		{
			float num = fullControl ? 1f : 0.05f;
			int num2 = 0;
			using (List<ConquestSystem>.Enumerator enumerator = this.systems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.station.umbralControlLevel >= num)
					{
						num2++;
					}
				}
			}
			return num2;
		}

		// Token: 0x0400028B RID: 651
		public const float TickDelay = 3600f;

		// Token: 0x0400028C RID: 652
		public const int RejoinConquestTicks = 5;

		// Token: 0x0400028D RID: 653
		public const float UmbralControlForMissions = 0.05f;

		// Token: 0x0400028E RID: 654
		public const float UmbralControlForShop = 0.5f;

		// Token: 0x0400028F RID: 655
		public new const int MaxLevel = 60;

		// Token: 0x04000290 RID: 656
		public const float ReinforcementsMin = 0f;

		// Token: 0x04000291 RID: 657
		public const float ReinforcementsMax = 4f;

		// Token: 0x04000292 RID: 658
		public static HashSet<Faction> conquestFactions = new HashSet<Faction>
		{
			Faction.red,
			Faction.blue,
			Faction.gold,
			Faction.miningGuild,
			Faction.salvageGuild,
			Faction.marauders,
			Faction.fanatics,
			Faction.darkspacers,
			Faction.stranded
		};

		// Token: 0x04000293 RID: 659
		public static HashSet<Faction> autoPopulatingFactions = new HashSet<Faction>
		{
			Faction.red,
			Faction.blue,
			Faction.gold,
			Faction.darkspacers
		};

		// Token: 0x04000294 RID: 660
		public static HashSet<Faction> playerPopulatingFactions = new HashSet<Faction>
		{
			Faction.miningGuild,
			Faction.salvageGuild,
			Faction.marauders,
			Faction.stranded
		};

		// Token: 0x04000295 RID: 661
		public float conquestTickTime;

		// Token: 0x04000296 RID: 662
		public DateTime lastConquestTick;

		// Token: 0x04000297 RID: 663
		private Dictionary<Faction, ConquestFactionStanding> factionStanding = new Dictionary<Faction, ConquestFactionStanding>();

		// Token: 0x04000298 RID: 664
		public int umbralContribution;

		// Token: 0x0400029A RID: 666
		private SectorMapData conquestSector;

		// Token: 0x0400029B RID: 667
		public bool patchConquestTick;
	}
}
