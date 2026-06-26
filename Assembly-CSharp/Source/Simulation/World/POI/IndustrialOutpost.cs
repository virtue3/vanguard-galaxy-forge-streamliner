using System;
using System.Collections.Generic;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Persistables;
using Behaviour.Salvage;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Simulation.World.POI
{
	// Token: 0x0200007F RID: 127
	public class IndustrialOutpost : PoiStoryteller
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000493 RID: 1171 RVA: 0x000266D6 File Offset: 0x000248D6
		public float repairMax
		{
			get
			{
				return 6000f * GameMath.DamageMultiplier((float)this.startLevel);
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x000266EA File Offset: 0x000248EA
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x000266F2 File Offset: 0x000248F2
		public float ammoAmount { get; private set; } = 1f;

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x000266FB File Offset: 0x000248FB
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00026703 File Offset: 0x00024903
		public float repairAmount { get; private set; } = -1f;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0002670C File Offset: 0x0002490C
		// (set) Token: 0x06000499 RID: 1177 RVA: 0x00026714 File Offset: 0x00024914
		public int currentTurrets { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0002671D File Offset: 0x0002491D
		public int maxTurrets
		{
			get
			{
				return this.turretSlots.Length;
			}
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x00026728 File Offset: 0x00024928
		public IndustrialOutpost(MapPointOfInterest poi) : base(poi)
		{
			this.enemyFaction = Faction.marauders;
			if (this.startLevel == 0)
			{
				this.startLevel = poi.level;
			}
			if (poi.IsEnemyAvailable(Faction.fanatics) && new SeedGenerator().Add("Fanatics").Add(poi.guid).CreateRandom().RandomBool(0.5f))
			{
				this.enemyFaction = Faction.fanatics;
			}
			if (!this.enemyFaction.IsEnemy(Faction.player))
			{
				this.enemyFaction = GamePlayer.current.GetDefaultEnemyFaction();
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0002683C File Offset: 0x00024A3C
		public override void UpdateActive(float deltaTime)
		{
			BasePoiManager current = BasePoiManager.current;
			if (current != null && !current.initializedAndReady)
			{
				return;
			}
			this.lastSpawnTimer += Time.deltaTime;
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.2f;
				IndustryMission currentIndustry = GamePlayer.current.currentIndustry;
				if (currentIndustry == null || currentIndustry.failed || MapPointOfInterest.current != this.poi || !BasePoiManager.current)
				{
					return;
				}
				bool flag = false;
				CombatStationPart[] componentsInChildren = BasePoiManager.current.GetComponentsInChildren<CombatStationPart>();
				foreach (CombatStationPart combatStationPart in componentsInChildren)
				{
					combatStationPart.rigidbody.bodyType = RigidbodyType2D.Kinematic;
					if (combatStationPart.partType == CombatStationPartType.Core)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					currentIndustry.MissionFailed("@IndustryMissionDestroyed");
					IndustryMissionDock componentInChildren = BasePoiManager.current.GetComponentInChildren<IndustryMissionDock>();
					if (componentInChildren)
					{
						UnityEngine.Object.Destroy(componentInChildren.gameObject);
					}
					PersistableData persistableData = null;
					foreach (PersistableData persistableData2 in this.poi.GetPersistables())
					{
						if (persistableData2 != null)
						{
							PersistableData persistableData3 = persistableData2;
							persistableData = persistableData3;
							break;
						}
					}
					if (persistableData != null)
					{
						this.poi.RemovePersistable(persistableData);
					}
					if (SpaceStationInterior.instance)
					{
						SpaceStationInterior.instance.ExitSpacestation();
					}
				}
				SeededRandom global = SeededRandom.Global;
				IndustryStation industryStation = this.poi as IndustryStation;
				int num = industryStation.materialStorage.Remove("IndustrialSupplyPack", 100);
				if (num > 0)
				{
					MissionObjective.Trigger(MissionTrigger.IndustryBoardCraft, num, null, false);
				}
				if (industryStation.materialStorage.Remove("IndustrialAmmoPack", 1) == 1)
				{
					this.ammoAmount += 1f;
				}
				foreach (CombatStationPart combatStationPart2 in componentsInChildren)
				{
					if (combatStationPart2.hardpointSlots.Length != 0)
					{
						foreach (AbstractTurret abstractTurret in combatStationPart2.GetComponentsInChildren<AbstractTurret>())
						{
							if (this.ammoAmount <= 0f)
							{
								abstractTurret.Deactivate();
							}
							else
							{
								abstractTurret.Activate();
							}
						}
					}
				}
				if (this.repairAmount < 0f)
				{
					this.repairAmount = this.repairMax;
				}
				if (industryStation.materialStorage.Remove("IndustrialRepairPack", 1) == 1)
				{
					this.repairAmount += this.repairMax;
				}
				foreach (CombatStationPart combatStationPart3 in componentsInChildren)
				{
					if (this.repairAmount > 1f && combatStationPart3.currentArmorHP < combatStationPart3.maxArmorHP)
					{
						float num2 = Mathf.Min(new float[]
						{
							combatStationPart3.maxArmorHP / 10f,
							combatStationPart3.maxArmorHP - combatStationPart3.currentArmorHP,
							this.repairAmount
						});
						if (num2 > 0f)
						{
							combatStationPart3.unitData.RepairArmorHP(num2);
							this.repairAmount -= num2;
						}
					}
					if (this.repairAmount > 1f && combatStationPart3.currentHullHP < combatStationPart3.maxHullHP)
					{
						float num3 = Mathf.Min(new float[]
						{
							combatStationPart3.maxHullHP / 10f,
							combatStationPart3.maxHullHP - combatStationPart3.currentHullHP,
							this.repairAmount
						});
						if (num3 > 0f)
						{
							combatStationPart3.unitData.RepairHullHp(num3);
							this.repairAmount -= num3;
						}
					}
				}
				this.currentTurrets = 0;
				foreach (Vector2 vector in this.turretSlots)
				{
					Vector2 b = this.poi.GetWorldPosition() + vector;
					bool flag2 = false;
					CombatStationPart[] array = componentsInChildren;
					for (int j = 0; j < array.Length; j++)
					{
						if (Vector2.Distance(array[j].transform.position, b) < 3f)
						{
							int currentTurrets = this.currentTurrets;
							this.currentTurrets = currentTurrets + 1;
							flag2 = true;
							break;
						}
					}
					if (!flag2 && industryStation.materialStorage.Remove("IndustrialTurretPack", 1) == 1)
					{
						CombatStationFactory.CreateGunPlatform(this.poi, Faction.industrialGuild, new Vector2?(vector), (float)global.RandomRange(-45, 45), 1);
						break;
					}
				}
				if (this.poi.totalEnemyCount == 0 || this.lastSpawnTimer > 120f)
				{
					this.lastSpawnTimer = 0f;
					List<AbstractUnitData> list = this.poi.CreateUnitPayload(0.7f + (float)this.waveCount * 0.1f, new GameplayType?(GameplayType.Combat), this.enemyFaction, 0, 0, 1, 5, null);
					foreach (AbstractUnitData abstractUnitData in list)
					{
						abstractUnitData.positionData.position = this.poi.GetWorldPosition() + new Vector2(global.RandomRange(28f, 35f), global.RandomRange(-12f, 12f));
					}
					this.poi.AddTriggeredSpawn(list, (float)((this.waveCount == 0) ? 45 : 15), 0, false, false);
					this.waveCount++;
					this.ammoAmount = Mathf.Max(0f, this.ammoAmount - 0.1f - (float)this.currentTurrets * 0.05f);
				}
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00026DFC File Offset: 0x00024FFC
		public void RefreshPersistables()
		{
			SeededRandom global = SeededRandom.Global;
			float lo = -18f;
			float hi = 18f;
			float hi2 = -2f;
			float lo2 = -95f;
			List<Vector2> list = new List<Vector2>();
			int num = 0;
			int num2 = 0;
			if (MapPointOfInterest.current == this.poi && BasePoiManager.current)
			{
				Vector2 b = GameplayManager.Instance.spaceShip.transform.position;
				foreach (Asteroid asteroid in BasePoiManager.current.GetComponentsInChildren<Asteroid>())
				{
					if (Vector2.Distance(asteroid.transform.position, b) >= 10f)
					{
						if (asteroid.asteroidData.surfaceAmount < 6 || asteroid.asteroidData.innerCoreAmount < 6)
						{
							UnityEngine.Object.Destroy(asteroid.gameObject);
							this.poi.RemovePersistable(asteroid.asteroidData);
						}
						else
						{
							list.Add(asteroid.transform.localPosition);
							num2++;
						}
					}
				}
				foreach (SalvageContainer salvageContainer in BasePoiManager.current.GetComponentsInChildren<SalvageContainer>())
				{
					if (Vector2.Distance(salvageContainer.transform.position, b) >= 10f)
					{
						if (salvageContainer.data.surfaceCount < 6 || salvageContainer.data.structureCount < 6)
						{
							UnityEngine.Object.Destroy(salvageContainer.gameObject);
							this.poi.RemovePersistable(salvageContainer.data);
						}
						else
						{
							list.Add(salvageContainer.transform.localPosition);
							num++;
						}
					}
				}
			}
			List<AsteroidFieldOreSet> collection = new List<AsteroidFieldOreSet>
			{
				new AsteroidFieldOreSet("IndustrialComponent1", null, null),
				new AsteroidFieldOreSet("IndustrialComponent2", null, null),
				new AsteroidFieldOreSet("IndustrialComponent3", null, null)
			};
			List<AsteroidFieldOreSet> list2 = new List<AsteroidFieldOreSet>();
			while (list.Count < 30)
			{
				int num3 = 0;
				Vector2 vector;
				bool flag;
				do
				{
					num3++;
					vector = new Vector2(global.RandomRange(lo2, hi2), global.RandomRange(lo, hi));
					flag = false;
					foreach (Vector2 b2 in list)
					{
						if (Vector2.Distance(vector, b2) < 8f)
						{
							flag = true;
							break;
						}
					}
				}
				while (flag && num3 < 50);
				list.Add(vector);
				if (list2.Count < 2)
				{
					list2.AddRange(collection);
				}
				if (num > num2)
				{
					AsteroidData asteroidData = new AsteroidData("");
					asteroidData.InitAsteroid(this.poi, new AsteroidFieldData(1, 0f, 1.1f, global.ChooseAndRemove<AsteroidFieldOreSet>(list2), global.ChooseAndRemove<AsteroidFieldOreSet>(list2), -1f), this.poi.GetWorldPosition() + vector);
					this.poi.AddPersistable(asteroidData);
					num2++;
				}
				else
				{
					bool flag2 = SeededRandom.Global.RandomBool(0.3f);
					string shipTemplate = flag2 ? "AncientWreck" : "DroneWreck";
					SalvageData salvageData = new SalvageData
					{
						position = this.poi.GetWorldPosition() + vector,
						angle = (float)global.RandomRange(0, 360),
						shipTemplate = shipTemplate,
						initialBattleDamage = 10
					};
					salvageData.scrapContent.Add(global.ChooseAndRemove<AsteroidFieldOreSet>(list2).commonOre.item, global.RandomRange(8, 13) * (flag2 ? 2 : 1));
					salvageData.structuralContent.Add(global.ChooseAndRemove<AsteroidFieldOreSet>(list2).commonOre.item, global.RandomRange(8, 13) * (flag2 ? 2 : 1));
					this.poi.AddPersistable(salvageData);
					num++;
				}
			}
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00027208 File Offset: 0x00025408
		public override void DataFromJson(JsonObject data)
		{
			this.startLevel = data["startLevel"];
			this.waveCount = data["waveCount"];
			this.ammoAmount = (float)data["ammoAmount"].AsNumber;
			this.repairAmount = (float)data["repairAmount"].AsNumber;
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00027278 File Offset: 0x00025478
		public override void DataToJson(JsonObject data)
		{
			data["startLevel"] = new double?((double)this.startLevel);
			data["waveCount"] = new double?((double)this.waveCount);
			data["ammoAmount"] = new double?((double)this.ammoAmount);
			data["repairAmount"] = new double?((double)this.repairAmount);
		}

		// Token: 0x0400027C RID: 636
		private int startLevel;

		// Token: 0x0400027D RID: 637
		private int waveCount;

		// Token: 0x04000280 RID: 640
		private float updateTimer;

		// Token: 0x04000281 RID: 641
		private float lastSpawnTimer;

		// Token: 0x04000282 RID: 642
		private Faction enemyFaction;

		// Token: 0x04000283 RID: 643
		private Vector2[] turretSlots = new Vector2[]
		{
			new Vector2(20f, -5f),
			new Vector2(20f, 5f),
			new Vector2(14f, -10f),
			new Vector2(14f, 10f)
		};
	}
}
