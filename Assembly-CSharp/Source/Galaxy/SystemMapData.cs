using System;
using System.Collections.Generic;
using Behaviour.Equipment.Builder;
using Behaviour.GalaxyMap;
using Behaviour.Hazard;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.Util;
using LightJson;
using Source.Combat;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy.POI;
using Source.Galaxy.Statics;
using Source.Hazard;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World;
using Source.Simulation.World.System;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x0200014A RID: 330
	public class SystemMapData : MapElement
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0005A83C File Offset: 0x00058A3C
		public static SystemMapData current
		{
			get
			{
				GamePlayer current = GamePlayer.current;
				if (current == null)
				{
					return null;
				}
				return current.currentSystem;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0005A84E File Offset: 0x00058A4E
		public WorldMapStatic prefab
		{
			get
			{
				return WorldMapStatic.GetPrefab("System");
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0005A85C File Offset: 0x00058A5C
		public Vector2 sectorPosition
		{
			get
			{
				return new Vector2(this.position.x + this.sector.position.x * 100f + (float)(this.sector.quadrant * 10000), this.position.y + this.sector.position.y * 100f);
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0005A8C8 File Offset: 0x00058AC8
		public Vector2 mapPosition
		{
			get
			{
				return new Vector2(this.position.x * 20f + this.sector.position.x * 150f + (float)(this.sector.quadrant * 10000), this.position.y * 20f + this.sector.position.y * 150f);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0005A940 File Offset: 0x00058B40
		public IEnumerable<Faction> factions
		{
			get
			{
				HashSet<Faction> hashSet = new HashSet<Faction>();
				foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
				{
					hashSet.Add(mapPointOfInterest.faction);
				}
				return hashSet;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x0005A9A0 File Offset: 0x00058BA0
		public IEnumerable<MapPointOfInterest> allPointsOfInterest
		{
			get
			{
				foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
				{
					yield return mapPointOfInterest;
				}
				List<MapPointOfInterest>.Enumerator enumerator = default(List<MapPointOfInterest>.Enumerator);
				foreach (Mission mission in GamePlayer.current.allMissions)
				{
					MapPointOfInterest dynamicPointOfInterest = mission.dynamicPointOfInterest;
					if (((dynamicPointOfInterest != null) ? dynamicPointOfInterest.system : null) == this)
					{
						yield return dynamicPointOfInterest;
					}
				}
				IEnumerator<Mission> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0005A9B0 File Offset: 0x00058BB0
		public bool isUnlocked
		{
			get
			{
				if (SystemMapData.current == this)
				{
					return true;
				}
				if (!this.sector.IsUnlocked())
				{
					return false;
				}
				foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
				{
					JumpGate jumpGate = mapPointOfInterest as JumpGate;
					if (jumpGate != null && jumpGate.canUseJumpGate)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000CA3 RID: 3235 RVA: 0x0005AA2C File Offset: 0x00058C2C
		public bool jumpgateOpen
		{
			get
			{
				foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
				{
					JumpGate jumpGate = mapPointOfInterest as JumpGate;
					if (jumpGate != null && jumpGate.canUseJumpGate)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0005AA90 File Offset: 0x00058C90
		public Star GetStar()
		{
			foreach (MapStatic mapStatic in this.statics)
			{
				Star star = mapStatic as Star;
				if (star != null)
				{
					return star;
				}
			}
			return null;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0005AAEC File Offset: 0x00058CEC
		public override Color GetColor()
		{
			return this.GetStar().GetColor();
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0005AAF9 File Offset: 0x00058CF9
		public override float GetSize()
		{
			return this.GetStar().GetSize();
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0005AB06 File Offset: 0x00058D06
		public SystemMapData(SectorMapData parent, Vector2 pos)
		{
			this.sector = parent;
			this.position = pos;
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0005AB34 File Offset: 0x00058D34
		public void SetConquestFaction(Faction f)
		{
			Faction faction = this.faction;
			this.faction = f;
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (mapPointOfInterest.faction == faction && (mapPointOfInterest is ConquestStation || !(mapPointOfInterest is SpaceStation)))
				{
					mapPointOfInterest.faction = f;
				}
			}
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0005ABB0 File Offset: 0x00058DB0
		public override float GetLastVisitedTime()
		{
			float num = 0f;
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				num = ((num < mapPointOfInterest.GetLastVisitedTime()) ? mapPointOfInterest.GetLastVisitedTime() : num);
			}
			return num;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0005AC18 File Offset: 0x00058E18
		public override void ActiveUpdate(float delta)
		{
			if (this.storyteller != null)
			{
				this.storyteller.UpdateActive(delta);
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0005AC30 File Offset: 0x00058E30
		public override void AmbientUpdate(float delta)
		{
			if (this.storyteller != null)
			{
				this.storyteller.UpdateAmbient(delta);
			}
			for (int i = 0; i < this.pointsOfInterest.Count; i++)
			{
				MapPointOfInterest mapPointOfInterest = this.pointsOfInterest[i];
				mapPointOfInterest.AmbientUpdate(delta);
				if (mapPointOfInterest.timeLeft > 0f)
				{
					mapPointOfInterest.timeLeft -= delta;
					if (mapPointOfInterest.timeLeft < delta * 2f && MapPointOfInterest.currentOrNext == mapPointOfInterest)
					{
						mapPointOfInterest.timeLeft += delta;
					}
					else if (mapPointOfInterest.timeLeft <= 0f)
					{
						if (mapPointOfInterest.IsMissionRelevant())
						{
							mapPointOfInterest.timeLeft += 30f;
						}
						else
						{
							PoiBeaconItem.RemovePoiBeacon(mapPointOfInterest);
							this.pointsOfInterest.RemoveAt(i);
							i--;
						}
					}
				}
			}
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0005AD04 File Offset: 0x00058F04
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			if (this.faction != null)
			{
				string text = "@SystemControlledBy";
				Color c = this.faction.IsEnemy(Faction.player) ? ColorHelper.reddish : ColorHelper.greenish;
				using (List<MapPointOfInterest>.Enumerator enumerator = this.pointsOfInterest.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is EmbassyStation)
						{
							text = "@SystemControlledEmbassy";
							c = ColorHelper.greenish;
						}
					}
				}
				tooltip.AddTextLine(Translation.Highlight(text, c, new object[]
				{
					this.faction.name
				}), 12, 8f);
			}
			bool flag = false;
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null)
				{
					if (!flag)
					{
						tooltip.AddSeparator(null);
						tooltip.AddTextLine("@SystemSpaceStations", 12, 8f);
						flag = true;
					}
					tooltip.AddTextLine(spaceStation.faction.name, 12, 8f).Text.color = spaceStation.faction.relationColor;
				}
			}
			bool flag2 = false;
			foreach (JumpGate jumpGate in this.GetJumpGateList(true))
			{
				if (!flag2)
				{
					tooltip.AddSeparator(null);
					tooltip.AddTextLine("@SystemSectorJumpgate", 12, 8f);
					flag2 = true;
				}
				SystemMapData targetSystem = jumpGate.targetSystem;
				tooltip.AddTextLine(targetSystem.sector.GetNameLabel(), 12, 8f).Text.color = targetSystem.faction.relationColor;
			}
			bool flag3 = false;
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (!mission.isComplete || mission.turnIn == null || mission.turnIn.system != this)
				{
					MissionStep currentStep = mission.currentStep;
					if (currentStep == null || !currentStep.IsPointOfInterest(this))
					{
						continue;
					}
				}
				if (!flag3)
				{
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
					tooltip.AddTextLine("@MapPOIMissions", 12, 8f);
					flag3 = true;
				}
				tooltip.AddTextLine(mission.name, 12, 8f).Text.color = mission.difficulty.GetColor();
			}
			bool flag4 = false;
			foreach (string text2 in this.GetAvailableMissionHints(null))
			{
				if (!flag4)
				{
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
					tooltip.AddTextLine("@MapPOIAvailableMission", 12, 8f);
					flag4 = true;
				}
				tooltip.AddTextLine(text2, 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			List<string> list = new List<string>();
			float num = 0f;
			foreach (SpaceStation spaceStation2 in this.GetPointsOfInterest<SpaceStation>())
			{
				if (spaceStation2.materialStorage.count > 0)
				{
					foreach (Inventory.InventoryItem inventoryItem in spaceStation2.materialStorage.items)
					{
						num += inventoryItem.item.m3 * (float)inventoryItem.count;
						list.Add(Translation.Translate(inventoryItem.item.displayName, Array.Empty<object>()));
					}
				}
			}
			if (list.Count > 0)
			{
				tooltip.AddSeparator(null);
				tooltip.AddTextLine(Translation.Translate("@SSMaterialsStored", new object[]
				{
					num
				}), 12, 8f);
				if (list.Count > 3)
				{
					while (list.Count > 3)
					{
						list.RemoveAt(3);
					}
					list.Add("...");
				}
				tooltip.AddTextLine(string.Join(", ", list), 12, 8f);
			}
			if (this.sector.conquestSector)
			{
				ConquestSystem conquestSystem = this.storyteller as ConquestSystem;
				if (conquestSystem != null)
				{
					tooltip.AddSeparator(null);
					if (conquestSystem.umbralControlLevel > 0f)
					{
						tooltip.AddTextLine(Translation.Highlight("@UmbralControlLevel", ColorHelper.umbralColor, new object[]
						{
							GameMath.FormatPercentage(conquestSystem.umbralControlLevel, FormatPercentageMode.Default, 1)
						}), 12, 8f);
						tooltip.AddSeparator(null);
					}
					if (conquestSystem.headquarters)
					{
						tooltip.AddTextLine("@ConquestSystemHeader2", 12, 8f);
					}
					else
					{
						tooltip.AddTextLine("@ConquestSystemHeader", 12, 8f);
					}
					tooltip.AddTextLine(Translation.Translate("@ConquestSystemStrength", new object[]
					{
						Mathf.RoundToInt(conquestSystem.combatStrength)
					}), 12, 8f);
					tooltip.AddTextLine(Translation.Translate("@ConquestSystemReinforcements", new object[]
					{
						Mathf.RoundToInt(conquestSystem.totalReinforcements)
					}), 12, 8f);
					tooltip.AddTextLine(Translation.Translate("@ConquestSystemStatus", new object[]
					{
						"@ConquestSystemStatus" + conquestSystem.controlLevel.ToString()
					}), 12, 8f);
				}
			}
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0005B31C File Offset: 0x0005951C
		public AsteroidFieldData GenerateSystemOreData(bool hazards = false)
		{
			if (this.systemOreData != null)
			{
				return this.systemOreData;
			}
			float num = Mathf.Lerp(260f, 130f, Mathf.Clamp01((float)this.level / 40f));
			float wealth = 0.7f + (float)this.level / 65f + SeededRandom.Global.RandomFloat() * ((float)this.level / num);
			this.systemOreData = new AsteroidFieldData(SeededRandom.Global.RandomRange(24, 36), 1f, wealth, AsteroidFieldData.CreateOreSet(this.level, true), AsteroidFieldData.CreateOreSet(this.level, false), -1f);
			return this.systemOreData;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0005B3C5 File Offset: 0x000595C5
		public void RemovePointOfInterest(MapPointOfInterest poi)
		{
			this.pointsOfInterest.Remove(poi);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0005B3D4 File Offset: 0x000595D4
		public bool IsStaticPOI(MapPointOfInterest poi)
		{
			return this.pointsOfInterest.Contains(poi);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0005B3E4 File Offset: 0x000595E4
		public MapPointOfInterest GetMiningClaimPOI()
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (mapPointOfInterest.name.StartsWith("Source.Galaxy.POI.Mining Claim"))
				{
					return mapPointOfInterest;
				}
			}
			return null;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0005B44C File Offset: 0x0005964C
		public MapPointOfInterest GetSalvageClaimPOI()
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (mapPointOfInterest.name.StartsWith("Salvage Claim"))
				{
					return mapPointOfInterest;
				}
			}
			return null;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0005B4B4 File Offset: 0x000596B4
		public SpaceStation AddSpaceStation(HashSet<SpaceStationFacility> spaceStationFacilities, Vector2? customPosition = null, Faction customFaction = null, string stationName = null, SpaceStation spaceStation = null)
		{
			if (spaceStation == null)
			{
				spaceStation = new SpaceStation();
			}
			this.SetupPOI(spaceStation, customPosition, customFaction, 0);
			spaceStation.name = (stationName ?? spaceStation.faction.GenerateStationName(spaceStation));
			spaceStation.SetFacilities(spaceStationFacilities);
			if (spaceStationFacilities.Count < 4)
			{
				spaceStation.stationSize = SpaceStation.StationSize.Small;
			}
			else if (spaceStationFacilities.Count < 6)
			{
				spaceStation.stationSize = SpaceStation.StationSize.Medium;
				spaceStation.stationVariant = SeededRandom.Global.ChooseEnum<SpaceStation.StationVariants>(0);
			}
			else
			{
				spaceStation.stationSize = SpaceStation.StationSize.Large;
				spaceStation.stationVariant = SeededRandom.Global.ChooseEnum<SpaceStation.StationVariants>(0);
			}
			this.pointsOfInterest.Add(spaceStation);
			return spaceStation;
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0005B55C File Offset: 0x0005975C
		public static SpaceStation.StationSize GetStationSize(int facilityCount)
		{
			if (facilityCount < 4)
			{
				return SpaceStation.StationSize.Small;
			}
			if (facilityCount < 6)
			{
				return SpaceStation.StationSize.Medium;
			}
			return SpaceStation.StationSize.Large;
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0005B56C File Offset: 0x0005976C
		public Source.Galaxy.POI.Mining AddTutorialPOI(Vector2? customPosition = null)
		{
			new SeedGenerator().Add("tutorial").CreateRandom();
			AsteroidFieldData asteroidFieldData = new AsteroidFieldData(4, 1f, 0.6f, new AsteroidFieldOreSet("OreCommon1", null, null), AsteroidFieldData.CreateOreSet(this.level, false), -1f);
			asteroidFieldData.excludeSizes = new List<AsteroidSize>
			{
				AsteroidSize.Tiny
			};
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining
			{
				name = "@Unknown",
				hidden = true
			};
			mining.SetAsteroidFieldData(asteroidFieldData, 0);
			this.SetupPOI(mining, customPosition, Faction.miningGuild, 0);
			this.pointsOfInterest.Add(mining);
			return mining;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0005B610 File Offset: 0x00059810
		public MapPointOfInterest AddTurorialJumpgatePOI(Vector2? customPosition = null)
		{
			new SeedGenerator().Add("tutorialjumpgate").CreateRandom();
			TutorialJumpgate tutorialJumpgate = new TutorialJumpgate
			{
				name = "@TutorialJumpgate"
			};
			tutorialJumpgate.SetIdentifier("TutorialJumpgatePOI");
			this.SetupPOI(tutorialJumpgate, customPosition, Faction.miningGuild, 0);
			tutorialJumpgate.AddPersistable(new TutorialJumpgateData
			{
				position = tutorialJumpgate.GetWorldPosition() + new Vector2(6f, 3f)
			});
			this.pointsOfInterest.Add(tutorialJumpgate);
			return tutorialJumpgate;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0005B698 File Offset: 0x00059898
		public Source.Galaxy.POI.Mining AddMiningPoi(Faction faction, Vector2? customPosition = null, AsteroidFieldData customFieldData = null, float hazardLevel = 0f, bool pirateChance = false, float friendlyMinerChance = 0.5f)
		{
			Source.Galaxy.POI.Mining mining = new Source.Galaxy.POI.Mining
			{
				name = string.Format("Asteroid Field T-{0}", SeededRandom.Global.RandomRange(3, 177))
			};
			mining.SetAsteroidFieldData(customFieldData, 0);
			this.SetupPOI(mining, customPosition, faction, 0);
			bool flag = false;
			if (SeededRandom.Global.RandomBool(friendlyMinerChance))
			{
				List<AbstractUnitData> list = mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), Faction.miningGuild, 0, 0, 1, 1, null);
				list.ForEach(delegate(AbstractUnitData a)
				{
					a.autoActions = "AmbientMiner";
				});
				mining.AddTriggeredSpawn(list, (float)SeededRandom.Global.RandomRange(10, 40), 0, false, true);
				flag = true;
			}
			if (pirateChance && SeededRandom.Global.RandomBool(0.3f))
			{
				List<AbstractUnitData> list2 = mining.CreateUnitPayload(1f, new GameplayType?(GameplayType.Mining), Faction.marauders, 0, 0, 1, 5, null);
				for (int i = 0; i < list2.Count; i++)
				{
					list2[i].autoActions = "AmbientMiner";
					list2[i].positionData.position = mining.GetWorldPosition() + new Vector2(SeededRandom.Global.RandomRange(12f, 80f), SeededRandom.Global.RandomRange(-9f, 9f));
				}
				mining.AddTriggeredSpawn(list2, 5f, 0, false, false);
			}
			if (SeededRandom.Global.RandomBool(0.6f) && !flag)
			{
				SpaceShipData spaceShipData = new SpaceShipData("Chisel Mk II", false, Faction.miningGuild)
				{
					autoActions = "AmbientMiner"
				};
				if (SeededRandom.Global.RandomBool(0.5f))
				{
					EquipmentBuilder equipmentBuilder = EquipmentBuilder.Get("SmallMiningCoreExplosiveLauncher");
					spaceShipData.EquipItem(equipmentBuilder.CreateItemType(Rarity.Standard, this.level, false, null, false, false), 0);
				}
				mining.AddTriggeredSpawn(new List<AbstractUnitData>
				{
					spaceShipData
				}, (float)SeededRandom.Global.RandomRange(30, 60), 0, false, true);
			}
			mining.AddCargoContainers(new Vector2(120f, 16f), 4, 0.4f);
			if (hazardLevel > 0f)
			{
				HazardName random = LocalHazard.GetRandom();
				mining.hazardFieldData = new HazardFieldData
				{
					hazardName = random,
					damageType = LocalHazard.GetRandomDamageTypeForHazard(random),
					spawnChance = 0.5f
				};
			}
			this.pointsOfInterest.Add(mining);
			return mining;
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0005B91C File Offset: 0x00059B1C
		public Salvage AddDerelictFleetPoi(Faction faction, Vector2? customPosition = null, string shipTemplate = "AncientWreck", float hazardChance = 0.5f)
		{
			SeededRandom global = SeededRandom.Global;
			Salvage salvage = new Salvage
			{
				name = Translation.Translate("@SalvagePOIShipyard", Array.Empty<object>())
			};
			this.SetupPOI(salvage, customPosition, faction, 0);
			int num = salvage.level / 20;
			int num2 = 3 + num * 2;
			int num3 = Mathf.Max(num2, salvage.level / 5 + global.RandomRange(0, num + 1));
			int num4 = global.RandomRange(num2, num3 + 1);
			float num5 = 0f;
			for (int i = 0; i < num4; i++)
			{
				num5 += global.RandomRange(9f, 14f);
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2(num5, global.RandomRange(-12f, 12f)),
					angle = (float)global.RandomRange(0, 360),
					shipTemplate = shipTemplate,
					initialBattleDamage = 80,
					hazardData = (global.RandomBool(hazardChance) ? salvage.CreateHazardData(HazardName.DamageInRadius, DamageType.Radiation) : null)
				};
				salvageData.AddItemContent(salvage.level, 3, 2f);
				salvageData.AddScrapContent(salvage.level, 2f, 2);
				salvageData.AddStructuralContent(salvage.level, 2, 1f);
				salvage.AddPersistable(salvageData);
			}
			salvage.hazardsDescription = Translation.Translate("@MapPOIDangerRadiation", Array.Empty<object>());
			salvage.AddCargoContainers(new Vector2(30f, 16f), 1, 0.4f);
			this.pointsOfInterest.Add(salvage);
			return salvage;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0005BAAC File Offset: 0x00059CAC
		public Salvage AddAncientWreckPoi(Faction f, Vector2 pos)
		{
			SeededRandom global = SeededRandom.Global;
			Salvage salvage = this.SetupPOI(new Salvage(), new Vector2?(pos), f, 0) as Salvage;
			salvage.name = Translation.Translate("@SalvagePOIShipyard", Array.Empty<object>());
			int num = salvage.level / 20;
			int num2 = 3 + num * 2;
			int num3 = Mathf.Max(num2, salvage.level / 5 + global.RandomRange(0, num + 1));
			int num4 = global.RandomRange(num2, num3 + 1);
			float num5 = 0f;
			for (int i = 0; i < num4; i++)
			{
				num5 += global.RandomRange(9f, 14f);
				SalvageData salvageData = new SalvageData
				{
					position = salvage.GetWorldPosition() + new Vector2(num5, global.RandomRange(-12f, 12f)),
					angle = (float)global.RandomRange(0, 360),
					shipTemplate = "AncientWreck",
					initialBattleDamage = 80
				};
				salvageData.AddScrapContent(salvage.level, 4f, 3);
				salvageData.AddStructuralContent(salvage.level, 3, 1f);
				salvage.AddPersistable(salvageData);
			}
			salvage.AddCargoContainers(new Vector2(30f, 16f), 1, 0.4f);
			this.pointsOfInterest.Add(salvage);
			return salvage;
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0005BC04 File Offset: 0x00059E04
		public Source.Galaxy.POI.Combat AddCombat(Faction faction = null, Vector2? customPosition = null, int forcedLevel = 0)
		{
			Source.Galaxy.POI.Combat combat = new Source.Galaxy.POI.Combat();
			this.SetupPOI(combat, customPosition, faction, forcedLevel);
			this.pointsOfInterest.Add(combat);
			return combat;
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0005BC30 File Offset: 0x00059E30
		public Source.Galaxy.POI.Combat AddCombatStation(Faction faction = null, Vector2? customPosition = null, bool addToSystem = true, int forcedLevel = 0)
		{
			CombatStation combatStation = new CombatStation();
			this.SetupPOI(combatStation, customPosition, faction, forcedLevel);
			if (addToSystem)
			{
				this.pointsOfInterest.Add(combatStation);
			}
			return combatStation;
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0005BC60 File Offset: 0x00059E60
		public Planet AddPlanet(string planetName, PlanetType type, float scale, Vector2? customPosition = null, bool addPoi = true)
		{
			Planet planet = new Planet
			{
				name = planetName,
				pType = type,
				scale = scale,
				rotationSpeed = 0.01f
			};
			if (addPoi)
			{
				this.SetupPOI(planet, customPosition, null, 0);
			}
			else
			{
				planet.position = new Vector2(SeededRandom.Global.RandomRange(-1f, 1f), SeededRandom.Global.RandomRange(-0.25f, 0.25f));
			}
			this.statics.Add(planet);
			return planet;
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0005BCE8 File Offset: 0x00059EE8
		public Beacon AddBeacon(string guid = null, Faction faction = null, Vector2? customPosition = null)
		{
			Beacon beacon = new Beacon
			{
				faction = faction
			};
			beacon.name = Translation.Translate("@Beacon", Array.Empty<object>());
			if (guid != null)
			{
				beacon.SetIdentifier(guid);
			}
			this.SetupPOI(beacon, customPosition, faction, 0);
			BeaconData persistable = new BeaconData
			{
				position = beacon.GetWorldPosition() + new Vector2(1f, 0f)
			};
			beacon.AddPersistable(persistable);
			this.pointsOfInterest.Add(beacon);
			return beacon;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0005BD68 File Offset: 0x00059F68
		public MapElement SetupPOI(MapElement poi, Vector2? customPosition = null, Faction customFaction = null, int forcedLevel = 0)
		{
			poi.system = this;
			poi.level = ((forcedLevel > 0) ? forcedLevel : this.level);
			poi.faction = (customFaction ?? (this.faction ?? Faction.player));
			poi.position = (customPosition ?? this.GetRandomPosition(-20f, 20f, -5f, 5f));
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (mapPointOfInterest != poi && Vector2.Distance(mapPointOfInterest.position, poi.position) < 2f)
				{
					Vector2 normalized = (mapPointOfInterest.position - poi.position).normalized;
					mapPointOfInterest.UpdateLocalPosition(poi.position + 2f * normalized);
				}
			}
			return poi;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0005BE74 File Offset: 0x0005A074
		public Vector2 GetDynamicMissionPosition()
		{
			Vector2 v = Singleton<TravelManager>.Instance.localTarget.position - GamePlayer.current.mapPosition;
			float z = (float)SeededRandom.Global.RandomRange(-45, 45);
			v = Quaternion.Euler(new Vector3(0f, 0f, z)) * v;
			return GamePlayer.current.mapPosition + v.normalized * 3f;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0005BEF8 File Offset: 0x0005A0F8
		public Vector2 GetRandomPosition(float minX = -20f, float maxX = 20f, float minY = -5f, float maxY = 5f)
		{
			Vector2 vector = default(Vector2);
			int num = 0;
			float num2 = 2.5f;
			for (;;)
			{
				num++;
				num2 -= 0.03f;
				vector = new Vector2(SeededRandom.Global.RandomRange(minX, maxX), SeededRandom.Global.RandomRange(minY, maxY));
				bool flag = true;
				foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
				{
					if (Vector2.Distance(vector, mapPointOfInterest.position) < num2)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					foreach (MapStatic mapStatic in this.statics)
					{
						if (Vector2.Distance(vector, mapStatic.position) < num2)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						foreach (Mission mission in GamePlayer.current.allMissions)
						{
							foreach (MissionStep missionStep in mission.steps)
							{
								MapPointOfInterest dynamicPointOfInterest = missionStep.dynamicPointOfInterest;
								if (((dynamicPointOfInterest != null) ? dynamicPointOfInterest.system : null) == this && Vector2.Distance(vector, missionStep.dynamicPointOfInterest.position) < num2)
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (num >= 50)
				{
					return vector;
				}
			}
			return vector;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0005C0AC File Offset: 0x0005A2AC
		public Vector2 GetClearedPosition(Vector2 pos)
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (Vector2.Distance(mapPointOfInterest.position, pos) < 2f)
				{
					Vector2 normalized = (mapPointOfInterest.position - pos).normalized;
					pos += 2f * normalized;
				}
			}
			return pos;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0005C134 File Offset: 0x0005A334
		public IEnumerable<string> GetAvailableMissionHints(MapPointOfInterest poiToCheck = null)
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.allPointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && (poiToCheck == null || poiToCheck == spaceStation))
				{
					string availableMissionHint = spaceStation.GetAvailableMissionHint();
					if (availableMissionHint != null)
					{
						yield return availableMissionHint;
					}
				}
			}
			IEnumerator<MapPointOfInterest> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0005C14C File Offset: 0x0005A34C
		public override void DataToJson(JsonObject data)
		{
			data["position"] = JsonUtil.Vector2ToJson(this.position);
			data["storyId"] = this.storyId;
			data["pointsOfInterest"] = this.pointsOfInterest.ToJsonArray<MapPointOfInterest>();
			data["statics"] = this.statics.ToJsonArray<MapStatic>();
			data["systemOreData"] = this.systemOreData.ToJson();
			data["pocketSystem"] = new bool?(this.pocketSystem);
			if (this.storyteller != null)
			{
				data["storyteller"] = this.storyteller.ToJson();
			}
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0005C210 File Offset: 0x0005A410
		public static SystemMapData FromJson(SectorMapData parent, JsonValue val)
		{
			SystemMapData systemMapData = new SystemMapData(parent, JsonUtil.JsonObjectToVector2(val["position"]));
			systemMapData.pointsOfInterest.FromJsonArray(val["pointsOfInterest"], new ClassExtensions.ParseJsonValue<MapPointOfInterest>(MapPointOfInterest.FromJson));
			foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
			{
				mapPointOfInterest.system = systemMapData;
			}
			systemMapData.statics.FromJsonArray(val["statics"], new ClassExtensions.ParseJsonValue<MapStatic>(MapStatic.FromJson));
			foreach (MapStatic mapStatic in systemMapData.statics)
			{
				mapStatic.system = systemMapData;
			}
			systemMapData.systemOreData = AsteroidFieldData.FromJson(val["systemOreData"]);
			systemMapData.storyId = val["storyId"];
			if (!val["pocketSystem"].IsNull)
			{
				systemMapData.pocketSystem = val["pocketSystem"];
			}
			if (!val["storyteller"].IsNull)
			{
				systemMapData.storyteller = SystemStoryteller.FromJson(val["storyteller"], systemMapData);
			}
			systemMapData.LoadFromJson(val);
			return systemMapData;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x0005C3A4 File Offset: 0x0005A5A4
		public MapPointOfInterest GetPoiWithId(string guid)
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (mapPointOfInterest.guid == guid)
				{
					return mapPointOfInterest;
				}
			}
			return null;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x0005C408 File Offset: 0x0005A608
		public IEnumerable<JumpGate> GetJumpGateList(bool sectorOnly = false)
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				JumpGate jumpGate = mapPointOfInterest as JumpGate;
				if (jumpGate != null && (jumpGate.sectorJumpgate || !sectorOnly))
				{
					yield return jumpGate;
				}
			}
			List<MapPointOfInterest>.Enumerator enumerator = default(List<MapPointOfInterest>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x0005C41F File Offset: 0x0005A61F
		public IEnumerable<SystemMapData> GetAdjacentSystems()
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				JumpGate jumpGate = mapPointOfInterest as JumpGate;
				if (jumpGate != null)
				{
					yield return jumpGate.targetSystem;
				}
			}
			List<MapPointOfInterest>.Enumerator enumerator = default(List<MapPointOfInterest>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0005C430 File Offset: 0x0005A630
		public MapPointOfInterest GetNearestPoiForItem(InventoryItemType itemType, int depth = 1)
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				if (mapPointOfInterest.PersistablesContainRetrievableItem(itemType) || mapPointOfInterest.UnvisitedAndHasRetrievableItem(itemType, true))
				{
					return mapPointOfInterest;
				}
			}
			depth--;
			if (depth <= 0)
			{
				return null;
			}
			foreach (JumpGate jumpGate in this.GetJumpGateList(false))
			{
				if (jumpGate.canUseJumpGate)
				{
					SystemMapData targetSystem = jumpGate.targetSystem;
					MapPointOfInterest mapPointOfInterest2 = (targetSystem != null) ? targetSystem.GetNearestPoiForItem(itemType, depth) : null;
					if (mapPointOfInterest2 != null)
					{
						return mapPointOfInterest2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x0005C508 File Offset: 0x0005A708
		public MapPointOfInterest GetNearestSpaceStationWithFacility(SpaceStationFacility facility, int depth = 3, bool excludeCurrent = true)
		{
			Debug.Log(string.Concat(new string[]
			{
				base.name,
				" checking SS with facility: ",
				facility.ToString(),
				" (",
				depth.ToString(),
				")"
			}));
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
				if (spaceStation != null && spaceStation.PlayerIsFriendly() && spaceStation.HasFacility(facility) && (!excludeCurrent || mapPointOfInterest != MapPointOfInterest.current) && (facility != SpaceStationFacility.MissionBoard || spaceStation.faction.offersMissionsForShip))
				{
					Debug.Log("Found it: " + mapPointOfInterest.name);
					return mapPointOfInterest;
				}
			}
			depth--;
			if (depth <= 0)
			{
				return null;
			}
			foreach (JumpGate jumpGate in this.GetJumpGateList(false))
			{
				if (jumpGate.canUseJumpGate)
				{
					SystemMapData targetSystem = jumpGate.targetSystem;
					MapPointOfInterest mapPointOfInterest2 = (targetSystem != null) ? targetSystem.GetNearestSpaceStationWithFacility(facility, depth, true) : null;
					if (mapPointOfInterest2 != null)
					{
						return mapPointOfInterest2;
					}
				}
			}
			return null;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0005C66C File Offset: 0x0005A86C
		public int GetJumpsToNearestSpaceStation(Faction faction)
		{
			if (this.CheckForSpaceStationOfFaction(this, faction, true))
			{
				return 0;
			}
			Queue<ValueTuple<SystemMapData, int>> queue = new Queue<ValueTuple<SystemMapData, int>>();
			queue.Enqueue(new ValueTuple<SystemMapData, int>(this, 0));
			int num = 0;
			while (queue.Count > 0 && num < 8)
			{
				ValueTuple<SystemMapData, int> valueTuple = queue.Dequeue();
				SystemMapData item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				num = item2;
				foreach (JumpGate jumpGate in item.GetJumpGateList(false))
				{
					if (jumpGate.canUseJumpGate)
					{
						if (this.CheckForSpaceStationOfFaction(item, faction, false))
						{
							return item2;
						}
						queue.Enqueue(new ValueTuple<SystemMapData, int>(jumpGate.targetSystem, item2 + 1));
					}
				}
			}
			return 8;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0005C734 File Offset: 0x0005A934
		private bool CheckForSpaceStationOfFaction(SystemMapData system, Faction faction, bool excludeCurrent = false)
		{
			foreach (MapPointOfInterest mapPointOfInterest in system.pointsOfInterest)
			{
				if (!excludeCurrent || mapPointOfInterest != MapPointOfInterest.current)
				{
					SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
					if (spaceStation != null && spaceStation.faction == faction)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0005C7A8 File Offset: 0x0005A9A8
		public T GetPointOfInterest<T>() where T : MapPointOfInterest
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				T t = mapPointOfInterest as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0005C818 File Offset: 0x0005AA18
		public IEnumerable<T> GetPointsOfInterest<T>() where T : MapPointOfInterest
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				T t = mapPointOfInterest as T;
				if (t != null)
				{
					yield return t;
				}
			}
			List<MapPointOfInterest>.Enumerator enumerator = default(List<MapPointOfInterest>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0005C828 File Offset: 0x0005AA28
		public bool GetPointOfInterest<T>(out T val) where T : MapPointOfInterest
		{
			val = this.GetPointOfInterest<T>();
			return val != null;
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x0005C844 File Offset: 0x0005AA44
		public JumpGate GetEntranceJumpgate()
		{
			foreach (MapPointOfInterest mapPointOfInterest in this.allPointsOfInterest)
			{
				JumpGate jumpGate = mapPointOfInterest as JumpGate;
				if (jumpGate != null && jumpGate.targetSystem.level < this.level)
				{
					return jumpGate.GetTargetPOI() as JumpGate;
				}
			}
			return null;
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0005C8B8 File Offset: 0x0005AAB8
		public void UpdateLevel(int level)
		{
			this.level = level;
			foreach (MapPointOfInterest mapPointOfInterest in this.pointsOfInterest)
			{
				mapPointOfInterest.level = level;
			}
		}

		// Token: 0x040006FE RID: 1790
		public bool pocketSystem;

		// Token: 0x040006FF RID: 1791
		public string storyId;

		// Token: 0x04000700 RID: 1792
		public const int MAX_X = 20;

		// Token: 0x04000701 RID: 1793
		public const int MIN_JUMPGATE_X = 12;

		// Token: 0x04000702 RID: 1794
		public const int MIN_JUMPGATE_Y = 3;

		// Token: 0x04000703 RID: 1795
		public const int MAX_Y = 5;

		// Token: 0x04000704 RID: 1796
		public SectorMapData sector;

		// Token: 0x04000705 RID: 1797
		public SystemStoryteller storyteller;

		// Token: 0x04000706 RID: 1798
		public List<MapPointOfInterest> pointsOfInterest = new List<MapPointOfInterest>();

		// Token: 0x04000707 RID: 1799
		public List<MapStatic> statics = new List<MapStatic>();

		// Token: 0x04000708 RID: 1800
		public AsteroidFieldData systemOreData;
	}
}
