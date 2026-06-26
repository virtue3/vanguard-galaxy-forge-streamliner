using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.GalaxyMap;
using Behaviour.Hazard;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Util;
using LightJson;
using Source.Combat;
using Source.Data;
using Source.Data.Persistable;
using Source.Hazard;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Simulation.World;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000146 RID: 326
	public abstract class MapPointOfInterest : MapElement
	{
		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x000576F9 File Offset: 0x000558F9
		public static MapPointOfInterest current
		{
			get
			{
				GamePlayer current = GamePlayer.current;
				if (current == null)
				{
					return null;
				}
				return current.currentPointOfInterest;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x0005770B File Offset: 0x0005590B
		public static MapPointOfInterest currentOrNext
		{
			get
			{
				MapPointOfInterest result;
				if ((result = MapPointOfInterest.current) == null)
				{
					TravelManager instance = Singleton<TravelManager>.Instance;
					if (instance == null)
					{
						return null;
					}
					result = instance.localTarget;
				}
				return result;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C22 RID: 3106
		public abstract WorldMapPOI Prefab { get; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C23 RID: 3107
		public abstract string sceneName { get; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C24 RID: 3108
		public abstract bool storeLastX { get; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00057728 File Offset: 0x00055928
		public MapTriggeredPayload nextTriggeredPayload
		{
			get
			{
				float num = 0f;
				MapTriggeredPayload result = null;
				foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
				{
					if (mapTriggeredPayload.triggerTime > 0f && (mapTriggeredPayload.triggerTime < num || num == 0f))
					{
						num = mapTriggeredPayload.triggerTime;
						result = mapTriggeredPayload;
					}
				}
				return result;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x000577A4 File Offset: 0x000559A4
		public bool hasPayload
		{
			get
			{
				return this.payloads.Count > 0;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x000577B4 File Offset: 0x000559B4
		public virtual string typeName
		{
			get
			{
				return "@MapPOI" + base.GetType().Name;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x000577CB File Offset: 0x000559CB
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x000577D3 File Offset: 0x000559D3
		public bool hasAsteroids { get; protected set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x000577DC File Offset: 0x000559DC
		public AsteroidFieldData asteroidFieldData
		{
			get
			{
				return this.customFieldData ?? this.system.systemOreData;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x000577F3 File Offset: 0x000559F3
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x000577FB File Offset: 0x000559FB
		public AsteroidFieldData customFieldData { get; protected set; }

		// Token: 0x06000C2D RID: 3117 RVA: 0x00057804 File Offset: 0x00055A04
		public bool IsStoryMissionPoi()
		{
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				if (mission.GetActivePoi(false) == this && mission.storyId != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00057868 File Offset: 0x00055A68
		public virtual int pointsValue
		{
			get
			{
				return Math.Max(34, this.level * 4);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00057879 File Offset: 0x00055A79
		public int minUnitPoints
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0005787D File Offset: 0x00055A7D
		public int maxUnitPoints
		{
			get
			{
				return Math.Max(20, this.level * 4);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00057890 File Offset: 0x00055A90
		public int activeEnemyCount
		{
			get
			{
				int num = 0;
				using (List<AbstractUnitData>.Enumerator enumerator = this.units.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsPlayerEnemy())
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000578EC File Offset: 0x00055AEC
		public int totalEnemyCount
		{
			get
			{
				int num = this.activeEnemyCount;
				foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
				{
					using (List<AbstractUnitData>.Enumerator enumerator2 = mapTriggeredPayload.units.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.IsPlayerEnemy())
							{
								num++;
							}
						}
					}
				}
				return num;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x00057984 File Offset: 0x00055B84
		public int activeUnitCount
		{
			get
			{
				return this.units.Count;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00057994 File Offset: 0x00055B94
		public int totalUnitCount
		{
			get
			{
				int num = this.activeUnitCount;
				foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
				{
					num += mapTriggeredPayload.units.Count;
				}
				return num;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000C35 RID: 3125 RVA: 0x000579F8 File Offset: 0x00055BF8
		public virtual bool hasCombatMusic
		{
			get
			{
				foreach (AbstractUnitData abstractUnitData in this.units)
				{
					if (abstractUnitData.IsPlayerEnemy() && abstractUnitData.IsCombatant())
					{
						return true;
					}
				}
				foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
				{
					foreach (AbstractUnitData abstractUnitData2 in mapTriggeredPayload.units)
					{
						if (abstractUnitData2.IsPlayerEnemy() && abstractUnitData2.IsCombatant())
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00057AEC File Offset: 0x00055CEC
		public override void ActiveUpdate(float delta)
		{
			PoiStoryteller poiStoryteller = this.storyteller;
			if (poiStoryteller != null)
			{
				poiStoryteller.UpdateActive(delta);
			}
			if (Singleton<TravelManager>.HasInstance && Singleton<TravelManager>.Instance.TravelActive())
			{
				return;
			}
			for (int i = 0; i < this.payloads.Count; i++)
			{
				if (this.payloads[i].Update(delta))
				{
					this.payloads.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00057B5C File Offset: 0x00055D5C
		public override void AmbientUpdate(float delta)
		{
			base.AmbientUpdate(delta);
			PoiStoryteller poiStoryteller = this.storyteller;
			if (poiStoryteller != null)
			{
				poiStoryteller.UpdateAmbient(delta);
			}
			if (MapPointOfInterest.current != this)
			{
				foreach (AbstractUnitData abstractUnitData in this.units)
				{
					if (abstractUnitData.faction.IsEnemy(Faction.player))
					{
						if (abstractUnitData.maxShieldHP > 0f)
						{
							abstractUnitData.currentShieldHP = abstractUnitData.maxShieldHP;
						}
						if (abstractUnitData.maxArmorHP > 0f)
						{
							abstractUnitData.currentArmorHP = Mathf.Min(abstractUnitData.currentArmorHP + abstractUnitData.maxArmorHP / 10f, abstractUnitData.maxArmorHP);
						}
						if (abstractUnitData.maxHullHP > 0f)
						{
							abstractUnitData.currentHullHP = Mathf.Min(abstractUnitData.currentHullHP + abstractUnitData.maxHullHP / 10f, abstractUnitData.maxHullHP);
						}
					}
				}
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00057C60 File Offset: 0x00055E60
		public override float GetLastVisitedTime()
		{
			return this.lastVisitedTime;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00057C68 File Offset: 0x00055E68
		public bool leftPoi()
		{
			return this.lastVisitedTime > 0f && MapPointOfInterest.currentOrNext != this;
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00057C84 File Offset: 0x00055E84
		public virtual bool CanTravelHere()
		{
			return true;
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x00057C87 File Offset: 0x00055E87
		public bool IsCurrentSystem()
		{
			return this.system == GamePlayer.current.currentSystem;
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00057C9B File Offset: 0x00055E9B
		public bool IsCurrentPoi()
		{
			return MapPointOfInterest.current == this;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00057CA5 File Offset: 0x00055EA5
		public void StoreLastX(Vector2 poiPosition)
		{
			if (this.storeLastX)
			{
				GameplayManager instance = GameplayManager.Instance;
				if (((instance != null) ? instance.spaceShip : null) != null)
				{
					this.lastVisitedX = GameplayManager.Instance.spaceShip.GetCurrentX() - poiPosition.x;
				}
			}
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00057CE4 File Offset: 0x00055EE4
		public virtual Vector2 GetLocalOffset()
		{
			if (this.storeLastX)
			{
				return new Vector2(this.lastVisitedX, 0f);
			}
			return Vector2.zero;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x00057D04 File Offset: 0x00055F04
		public Vector2 GetWorldPosition()
		{
			return this.position * MapPointOfInterest.mapToLocalConversion;
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x00057D18 File Offset: 0x00055F18
		public void UpdateWorldPosition(Vector2 world)
		{
			Vector2 worldPosition = this.GetWorldPosition();
			this.position = world / MapPointOfInterest.mapToLocalConversion;
			Vector2 vector = world - worldPosition;
			foreach (AbstractUnitData abstractUnitData in this.units)
			{
				abstractUnitData.positionData.position += vector;
			}
			foreach (PersistableData persistableData in this.persistables)
			{
				persistableData.OffsetPosition(vector);
			}
			foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
			{
				if (!mapTriggeredPayload.spawnAtPlayer)
				{
					foreach (AbstractUnitData abstractUnitData2 in mapTriggeredPayload.units)
					{
						abstractUnitData2.positionData.position += vector;
					}
				}
			}
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x00057E70 File Offset: 0x00056070
		public void UpdateLocalPosition(Vector2 pos)
		{
			this.UpdateWorldPosition(pos * MapPointOfInterest.mapToLocalConversion);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00057E84 File Offset: 0x00056084
		public Vector2 GetWorldPositionToTravelTo(Vector2? formationOffset = null)
		{
			Vector2 vector = this.GetWorldPosition() + this.GetLocalOffset() + (formationOffset ?? Vector2.zero);
			Rect worldBounds = this.GetWorldBounds();
			return new Vector2(Mathf.Clamp(vector.x, worldBounds.xMin - 10f, worldBounds.xMax + 10f), vector.y);
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00057EF8 File Offset: 0x000560F8
		public GameObject AddPersistable(PersistableData persistable)
		{
			this.persistables.Add(persistable);
			if (MapPointOfInterest.currentOrNext == this && !this.initializingPersistables)
			{
				return Singleton<TravelManager>.Instance.localPoiManager.AddToWorld(persistable);
			}
			return null;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00057F28 File Offset: 0x00056128
		public void RemovePersistable(PersistableData persistable)
		{
			this.persistables.Remove(persistable);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00057F38 File Offset: 0x00056138
		public bool PersistablesContainRetrievableItem(InventoryItemType itemType)
		{
			using (List<PersistableData>.Enumerator enumerator = this.persistables.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ItemCanBeObtained(itemType))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00057F94 File Offset: 0x00056194
		public virtual bool UnvisitedAndHasRetrievableItem(InventoryItemType itemType, bool overrideVisited = false)
		{
			return false;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00057F97 File Offset: 0x00056197
		public AbstractUnit AddUnit(AbstractUnitData unit, string seed = null, bool exactLevel = false)
		{
			this.units.Add(unit);
			if (MapPointOfInterest.currentOrNext == this)
			{
				return BasePoiManager.current.AddToWorld(unit, seed, exactLevel);
			}
			return null;
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00057FBC File Offset: 0x000561BC
		public void RemoveUnit(AbstractUnitData unit)
		{
			this.units.Remove(unit);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00057FCB File Offset: 0x000561CB
		public void AddPayload(MapTriggeredPayload payload)
		{
			this.payloads.Add(payload);
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x00057FD9 File Offset: 0x000561D9
		public IReadOnlyList<PersistableData> GetPersistables()
		{
			return this.persistables;
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00057FE4 File Offset: 0x000561E4
		public Dictionary<OreItemData, int> GetOreTotalsInField(bool scanned = false)
		{
			Dictionary<OreItemData, int> dictionary = new Dictionary<OreItemData, int>();
			foreach (PersistableData persistableData in this.GetPersistables())
			{
				AsteroidData asteroidData = persistableData as AsteroidData;
				if (asteroidData != null)
				{
					if (asteroidData.surfaceItem != null && asteroidData.surfaceAmount > 0)
					{
						this.AddOre(dictionary, asteroidData.surfaceItem, asteroidData.surfaceAmount);
					}
					if (asteroidData.innerCoreItem != null && asteroidData.innerCoreAmount > 0)
					{
						this.AddOre(dictionary, asteroidData.innerCoreItem, asteroidData.innerCoreAmount);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00058090 File Offset: 0x00056290
		private void AddOre(Dictionary<OreItemData, int> dict, OreItemData ore, int amount)
		{
			int num;
			if (dict.TryGetValue(ore, out num))
			{
				dict[ore] = num + amount;
				return;
			}
			dict.Add(ore, amount);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000580BB File Offset: 0x000562BB
		public IEnumerable<AbstractUnitData> GetUnits(bool includeInactive = false)
		{
			foreach (AbstractUnitData abstractUnitData in this.units)
			{
				yield return abstractUnitData;
			}
			List<AbstractUnitData>.Enumerator enumerator = default(List<AbstractUnitData>.Enumerator);
			if (includeInactive)
			{
				foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
				{
					foreach (AbstractUnitData abstractUnitData2 in mapTriggeredPayload.units)
					{
						yield return abstractUnitData2;
					}
					enumerator = default(List<AbstractUnitData>.Enumerator);
				}
				List<MapTriggeredPayload>.Enumerator enumerator2 = default(List<MapTriggeredPayload>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000580D4 File Offset: 0x000562D4
		public bool HasUnitsForFaction(Faction f)
		{
			using (List<AbstractUnitData>.Enumerator enumerator = this.units.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.faction == f)
					{
						return true;
					}
				}
			}
			foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
			{
				using (List<AbstractUnitData>.Enumerator enumerator = mapTriggeredPayload.units.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.faction == f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x000581B0 File Offset: 0x000563B0
		public bool HasPatrolUnits()
		{
			using (List<AbstractUnitData>.Enumerator enumerator = this.units.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.autoActions == "SecurityPatrol")
					{
						return true;
					}
				}
			}
			foreach (MapTriggeredPayload mapTriggeredPayload in this.payloads)
			{
				using (List<AbstractUnitData>.Enumerator enumerator = mapTriggeredPayload.units.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.autoActions == "SecurityPatrol")
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x000582A0 File Offset: 0x000564A0
		public IEnumerable<MapTriggeredPayload> GetTriggeredPayloads()
		{
			return this.payloads;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000582A8 File Offset: 0x000564A8
		public void RemovePayload(MapTriggeredPayload payload)
		{
			this.payloads.Remove(payload);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000582B7 File Offset: 0x000564B7
		public List<AbstractUnitData> AddPirateTurrets(int count, SeededRandom random = null, Faction f = null)
		{
			return this.AddGuards(this.CreateTurretPayload(count, f, random), random);
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000582CC File Offset: 0x000564CC
		public List<AbstractUnitData> AddGuards(List<AbstractUnitData> guards, SeededRandom random = null)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			List<PersistableData> list = new List<PersistableData>();
			for (int i = 0; i < guards.Count; i++)
			{
				if (list.Count == 0)
				{
					list.AddRange(this.GetPersistables());
				}
				Vector2 b = new Vector2(random.RandomRange(-1f, 1f), random.RandomRange(-1f, 1f)).normalized * 3f;
				if (list.Count > 0)
				{
					PersistableData persistableData = random.Choose<PersistableData>(list);
					list.Remove(persistableData);
					guards[i].positionData.position = persistableData.position + b;
				}
				else
				{
					guards[i].positionData.position = this.GetWorldPosition() + new Vector2(random.RandomRange(0f, 15f), random.RandomRange(-10f, 10f));
				}
				this.AddUnit(guards[i], null, false);
			}
			return guards;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x000583DC File Offset: 0x000565DC
		public List<AbstractUnitData> CreateFixedPayload(string fixedUnit, int unitCount = 1, Faction f = null, GameplayType? loadout = null, UnitRank rank = UnitRank.Rookie)
		{
			if (f == null)
			{
				f = this.faction;
			}
			List<AbstractUnitData> list = new List<AbstractUnitData>();
			for (int i = 0; i < unitCount; i++)
			{
				SpaceShipData spaceShipData = new SpaceShipData(fixedUnit, false, f);
				spaceShipData.unitRank = rank;
				spaceShipData.LoadFactionData(f);
				spaceShipData.LoadDefaultEquipment(this.level, -1f, null, loadout, null, null, false, null);
				list.Add(spaceShipData);
			}
			return list;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00058454 File Offset: 0x00056654
		public List<AbstractUnitData> CreateUnitPayload(float pointsScale = 1f, GameplayType? gType = (GameplayType)1, Faction f = null, int minPointsPerUnit = 0, int maxPointsPerUnit = 0, int minUnits = 1, int maxUnits = 5, UnitRank? fixedRank = null)
		{
			if (f == null)
			{
				f = this.faction;
			}
			if (minPointsPerUnit == 0)
			{
				minPointsPerUnit = this.minUnitPoints;
			}
			if (maxPointsPerUnit == 0)
			{
				maxPointsPerUnit = Mathf.Max(minPointsPerUnit + 10, this.maxUnitPoints);
				if (pointsScale > 1f)
				{
					maxPointsPerUnit = Mathf.RoundToInt((float)maxPointsPerUnit * Mathf.Sqrt(pointsScale));
				}
			}
			minPointsPerUnit = Mathf.Max(minPointsPerUnit, Mathf.RoundToInt((float)this.pointsValue * pointsScale / 16f));
			FactionShipCollection npcshipTypes = f.GetNPCShipTypes(this.level, minPointsPerUnit, maxPointsPerUnit, gType);
			minPointsPerUnit = npcshipTypes.minPointsPerUnit;
			maxPointsPerUnit = Mathf.Max(minPointsPerUnit + 10, maxPointsPerUnit);
			int num = Mathf.Max(Mathf.RoundToInt((float)this.pointsValue * pointsScale), minPointsPerUnit * minUnits);
			List<AbstractUnitData> list = new List<AbstractUnitData>();
			int i = SeededRandom.Global.RandomRange(minUnits, maxUnits + 1);
			int num2 = num;
			while (i > 0)
			{
				SpaceShip unit = npcshipTypes.GetUnit(num2, i);
				i--;
				if (unit != null)
				{
					SpaceShipData spaceShipData = new SpaceShipData(unit, false, f);
					spaceShipData.unitRank = (fixedRank ?? UnitRankHelper.GetRandomUnitRankForLevel(this.level, false));
					list.Add(spaceShipData);
					float f2 = (float)unit.pointValue * spaceShipData.unitRank.GetAdjustedPoints();
					num2 -= Mathf.RoundToInt(f2);
				}
			}
			foreach (AbstractUnitData abstractUnitData in list)
			{
				SpaceShipData spaceShipData2 = abstractUnitData as SpaceShipData;
				if (spaceShipData2 != null)
				{
					spaceShipData2.LoadFactionData(f);
				}
				abstractUnitData.LoadDefaultEquipment(this.level, -1f, null, gType, null, null, false, null);
			}
			if (list.Count == 0)
			{
				Debug.LogWarning("No units in CreateUnitPayload!");
			}
			return list;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x00058620 File Offset: 0x00056820
		public List<AbstractUnitData> CreateTurretPayload(int count, Faction f = null, SeededRandom random = null)
		{
			List<AbstractUnitData> list = new List<AbstractUnitData>();
			for (int i = 0; i < count; i++)
			{
				list.Add(new DefensiveTurretData("DepMissileTurret")
				{
					faction = (f ?? this.faction)
				});
			}
			return list;
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00058668 File Offset: 0x00056868
		public List<AbstractUnitData> AddTriggeredSpawn(List<AbstractUnitData> toSpawn, float spawnDelay, int triggerSequence = 0, bool waitForNoEnemies = false, bool spawnAtPlayer = true)
		{
			MapTriggeredPayload mapTriggeredPayload = new MapTriggeredPayload(this)
			{
				triggerTime = spawnDelay,
				spawnAtPlayer = spawnAtPlayer,
				triggerSequence = triggerSequence,
				waitForNoEnemies = waitForNoEnemies
			};
			float num = SeededRandom.Global.RandomRange(-8f, 8f);
			float y = (float)(SeededRandom.Global.RandomBool(0.5f) ? -20 : 20);
			foreach (AbstractUnitData abstractUnitData in toSpawn)
			{
				if (spawnAtPlayer)
				{
					abstractUnitData.positionData.position = new Vector2(num, y);
				}
				else
				{
					abstractUnitData.positionData.position += new Vector2(num, y);
				}
				num += 4f;
				mapTriggeredPayload.AddUnit(abstractUnitData);
			}
			this.AddPayload(mapTriggeredPayload);
			return toSpawn;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00058754 File Offset: 0x00056954
		public bool CanSpawnHazard()
		{
			return this.hazardFieldData != null && SeededRandom.Global.RandomBool(this.hazardFieldData.spawnChance);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00058775 File Offset: 0x00056975
		public HazardData CreateHazardData()
		{
			return LocalHazard.Get(this.hazardFieldData.hazardName.ToString()).CreateData(this.level, this.hazardFieldData.damageType);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x000587A8 File Offset: 0x000569A8
		public HazardData CreateHazardData(HazardName hazardName, DamageType damageType)
		{
			return LocalHazard.Get(hazardName.ToString()).CreateData(this.level, damageType);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x000587C8 File Offset: 0x000569C8
		public void AddCargoContainers(Vector2 poiSize, int count, float spawnChance = 0.4f)
		{
			for (int i = 0; i < count; i++)
			{
				if (SeededRandom.Global.RandomBool(spawnChance))
				{
					LootContainerData lootContainerData = new LootContainerData
					{
						position = this.GetWorldPosition() + new Vector2(SeededRandom.Global.RandomRange(0f, poiSize.x), SeededRandom.Global.RandomRange(-poiSize.y / 2f, poiSize.y / 2f)),
						name = "@LCNameOldCargoContainer"
					};
					if (SeededRandom.Global.RandomBool(0.5f))
					{
						if (SeededRandom.Global.RandomBool(0.05f))
						{
							lootContainerData.AddLoot(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f), 1);
						}
						else
						{
							lootContainerData.AddLoot(ItemBuilder.Get("WarpFuel").CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f), 1);
						}
					}
					List<InventoryItemType> refinedItemsWithDropChance = InventoryItemType.GetRefinedItemsWithDropChance(this.level + 5, null);
					if (SeededRandom.Global.RandomBool(0.3f))
					{
						lootContainerData.AddLoot(SeededRandom.Global.Choose<InventoryItemType>(refinedItemsWithDropChance), SeededRandom.Global.RandomRange(20, 31));
					}
					if (SeededRandom.Global.RandomBool(0.7f))
					{
						lootContainerData.AddLoot(ItemBuilder.Get("Credits").CreateCreditsItem(GameMath.GetCreditsValue(SeededRandom.Global.RandomRange(2.5f, 3.25f), this.level)), 1);
					}
					if (SeededRandom.Global.RandomBool(0.05f) && GamePlayer.current.IsInSandBox())
					{
						lootContainerData.AddLoot(InventoryItemType.Get("LockedContainerKey"), 1);
					}
					if (lootContainerData.loot.Count == 0 || SeededRandom.Global.RandomBool(0.4f))
					{
						string[] array = new string[6];
						array[0] = "Create ore: ";
						int num = 1;
						SystemMapData system = this.system;
						array[num] = ((system != null) ? system.ToString() : null);
						array[2] = ", level: ";
						array[3] = this.system.level.ToString();
						array[4] = ", oreData: ";
						int num2 = 5;
						SystemMapData system2 = this.system;
						AsteroidFieldData asteroidFieldData = (system2 != null) ? system2.systemOreData : null;
						array[num2] = ((asteroidFieldData != null) ? asteroidFieldData.ToString() : null);
						Debug.Log(string.Concat(array));
						lootContainerData.AddLoot(this.system.systemOreData.GetRandomOre(true, null).item, SeededRandom.Global.RandomRange(2, 9));
					}
					this.AddPersistable(lootContainerData);
				}
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00058A2C File Offset: 0x00056C2C
		public string FindSalvageShipTemplate(Faction f = null)
		{
			if (f == null)
			{
				f = this.faction;
			}
			return f.GetRandomNPCShipType(this.level, this.maxUnitPoints / 3, this.maxUnitPoints * 2, null);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x00058A6E File Offset: 0x00056C6E
		public void SetAsteroidFieldData(AsteroidFieldData asteroidFieldData, int amount = 0)
		{
			this.hasAsteroids = true;
			if (amount != 0)
			{
				asteroidFieldData.SetAmount(amount);
			}
			this.customFieldData = asteroidFieldData;
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00058A88 File Offset: 0x00056C88
		public void InitializeAsteroids(bool reverse = false, bool keepMiddleClear = false)
		{
			if (!this.asteroidsInitialized)
			{
				MiningPoiHelper.InitializeAsteroids(this, this.asteroidFieldData, reverse, keepMiddleClear);
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00058AA0 File Offset: 0x00056CA0
		public virtual void CleanupPersistables()
		{
			if (this.lastVisitedTime == 0f)
			{
				return;
			}
			if (Singleton<TravelManager>.Instance.targetPoi == this)
			{
				return;
			}
			for (int i = 0; i < this.persistables.Count; i++)
			{
				if (this.persistables[i].ShouldCleanUp())
				{
					this.persistables.RemoveAt(i);
					i--;
				}
			}
			this.asteroidsInitialized = false;
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x00058B0C File Offset: 0x00056D0C
		public void CleanupConquestStation()
		{
			for (int i = 0; i < this.persistables.Count; i++)
			{
				if (this.persistables[i] is CombatStationData)
				{
					this.persistables.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x00058B52 File Offset: 0x00056D52
		public void ClearUnits()
		{
			this.units.Clear();
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00058B5F File Offset: 0x00056D5F
		public void ClearPayloads()
		{
			this.payloads.Clear();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00058B6C File Offset: 0x00056D6C
		public void ClearPersistables()
		{
			this.persistables.Clear();
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00058B79 File Offset: 0x00056D79
		public static MapPointOfInterest Create(string name)
		{
			return (MapPointOfInterest)Type.GetType("Source.Galaxy.POI." + name).GetConstructor(new Type[0]).Invoke(new object[0]);
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00058BA8 File Offset: 0x00056DA8
		public static MapPointOfInterest FromJson(JsonValue val)
		{
			JsonObject asJsonObject = val.AsJsonObject;
			MapPointOfInterest poi = MapPointOfInterest.Create(asJsonObject["type"]);
			poi.LoadFromJson(asJsonObject);
			poi.dangerLevel = asJsonObject["dangerLevel"];
			poi.hazardsDescription = asJsonObject["hazardsDescription"];
			poi.lastVisitedTime = (float)asJsonObject["lastVisitedTime"].AsNumber;
			if (poi.storeLastX)
			{
				poi.lastVisitedX = (float)asJsonObject["lastVisitedX"].AsNumber;
			}
			if (asJsonObject.ContainsKey("timeLeft"))
			{
				poi.timeLeft = (float)asJsonObject["timeLeft"].AsNumber;
			}
			if (asJsonObject.ContainsKey("persistables"))
			{
				poi.persistables.FromJsonArray(asJsonObject["persistables"], new ClassExtensions.ParseJsonValue<PersistableData>(PersistableData.FromJson));
			}
			if (asJsonObject.ContainsKey("units"))
			{
				poi.units.FromJsonArray(asJsonObject["units"], new ClassExtensions.ParseJsonValue<AbstractUnitData>(AbstractUnitData.FromJson));
			}
			if (asJsonObject.ContainsKey("hidden"))
			{
				poi.hidden = asJsonObject["hidden"].AsBoolean;
			}
			if (asJsonObject.ContainsKey("payloads"))
			{
				poi.payloads.FromJsonArray(asJsonObject["payloads"], (JsonValue data) => MapTriggeredPayload.FromJson(poi, data));
			}
			if (!asJsonObject["hazardFieldData"].IsNull)
			{
				poi.hazardFieldData = HazardFieldData.FromJson(asJsonObject["hazardFieldData"]);
			}
			if (!asJsonObject["storyteller"].IsNull)
			{
				poi.storyteller = PoiStoryteller.FromJson(asJsonObject["storyteller"], poi);
			}
			if (asJsonObject.ContainsKey("backgroundSeed"))
			{
				poi.backgroundSeed = (ulong)asJsonObject["backgroundSeed"].AsInteger;
			}
			else
			{
				poi.backgroundSeed = (ulong)SeededRandom.Global.RandomInt();
			}
			if (!asJsonObject["asteroidsInitialized"].IsNull)
			{
				poi.asteroidsInitialized = asJsonObject["asteroidsInitialized"];
			}
			if (!asJsonObject["hasAsteroids"].IsNull)
			{
				poi.hasAsteroids = asJsonObject["hasAsteroids"];
			}
			if (!asJsonObject["customFieldData"].IsNull && asJsonObject["customFieldData"].IsJsonObject)
			{
				poi.customFieldData = AsteroidFieldData.FromJson(asJsonObject["customFieldData"]);
			}
			if (!asJsonObject["oreOwnershipOverride"].IsNull && asJsonObject["oreOwnershipOverride"].IsString)
			{
				poi.oreOwnershipOverride = Faction.Get(asJsonObject["oreOwnershipOverride"]);
			}
			return poi;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00058F0C File Offset: 0x0005710C
		public override void DataToJson(JsonObject data)
		{
			data["type"] = base.GetType().Name;
			data["dangerLevel"] = this.dangerLevel;
			data["hazardsDescription"] = this.hazardsDescription;
			data["lastVisitedTime"] = new double?((double)this.lastVisitedTime);
			data["backgroundSeed"] = new double?(this.backgroundSeed);
			if (this.storeLastX)
			{
				data["lastVisitedX"] = new double?((double)this.lastVisitedX);
			}
			if (this.timeLeft > 0f)
			{
				data["timeLeft"] = new double?((double)this.timeLeft);
			}
			if (this.persistables.Count > 0)
			{
				data["persistables"] = this.persistables.ToJsonArray<PersistableData>();
			}
			if (this.units.Count > 0)
			{
				data["units"] = this.units.ToJsonArray<AbstractUnitData>();
			}
			if (this.payloads.Count > 0)
			{
				data["payloads"] = this.payloads.ToJsonArray<MapTriggeredPayload>();
			}
			if (this.hidden)
			{
				data["hidden"] = new bool?(this.hidden);
			}
			if (this.hazardFieldData != null)
			{
				data["hazardFieldData"] = this.hazardFieldData.ToJson();
			}
			if (this.storyteller != null)
			{
				data["storyteller"] = this.storyteller.ToJson();
			}
			data["asteroidsInitialized"] = new bool?(this.asteroidsInitialized);
			data["hasAsteroids"] = new bool?(this.hasAsteroids);
			if (this.customFieldData != null)
			{
				data["customFieldData"] = this.customFieldData.ToJson();
			}
			if (this.oreOwnershipOverride != null)
			{
				data["oreOwnershipOverride"] = this.oreOwnershipOverride.identifier;
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00059138 File Offset: 0x00057338
		public virtual Rect GetWorldBounds()
		{
			Vector2 worldPosition = this.GetWorldPosition();
			float num = worldPosition.x - 10f;
			float num2 = worldPosition.x + 10f;
			foreach (PersistableData persistableData in this.GetPersistables())
			{
				if (num > persistableData.position.x)
				{
					num = persistableData.position.x;
				}
				if (num2 < persistableData.position.x)
				{
					num2 = persistableData.position.x;
				}
			}
			foreach (AbstractUnitData abstractUnitData in this.GetUnits(false))
			{
				if (num > abstractUnitData.positionData.position.x)
				{
					num = abstractUnitData.positionData.position.x;
				}
				if (num2 < abstractUnitData.positionData.position.x)
				{
					num2 = abstractUnitData.positionData.position.x;
				}
			}
			Vector2 screenSizeGame = Singleton<BackdropManager>.Instance.screenSizeGame;
			return new Rect(num - 10f, worldPosition.y - screenSizeGame.y / 2f, num2 + 20f - num, screenSizeGame.y);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000592A0 File Offset: 0x000574A0
		public bool IsEnemyAvailable(Faction f)
		{
			if (f == Faction.marauders)
			{
				return true;
			}
			if (f == Faction.fanatics && this.system != null)
			{
				if (SectorMapData.current.quadrant > 1)
				{
					return true;
				}
				if (GalaxyMapData.current.GetQuadrant(1).TakeLast(3).Contains(this.system.sector))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x000592FC File Offset: 0x000574FC
		public void GeneratePoiPersistables(Faction secondFaction)
		{
			SeededRandom global = SeededRandom.Global;
			if (global.RandomBool(0.3f))
			{
				this.SetAsteroidFieldData(this.system.systemOreData, global.RandomRange(3, 8));
			}
			if (global.RandomBool(0.3f))
			{
				int num = global.RandomRange(1, 3);
				for (int i = 0; i < num; i++)
				{
					string shipTemplate = this.FindSalvageShipTemplate(secondFaction);
					SalvageData salvageData = new SalvageData
					{
						position = this.GetWorldPosition() + new Vector2((float)global.RandomRange(-40, -5), (float)global.RandomRange(-10, 10)),
						shipTemplate = shipTemplate
					};
					salvageData.AddScrapContent(this.level, 1f, 2);
					salvageData.AddStructuralContent(this.level, 2, 1f);
					this.AddPersistable(salvageData);
				}
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000593CC File Offset: 0x000575CC
		public bool IsMissionRelevant()
		{
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				using (List<MissionStep>.Enumerator enumerator2 = mission.steps.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.IsMissionPoi(this))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x040006DB RID: 1755
		public static float mapToLocalConversion = 250f;

		// Token: 0x040006DC RID: 1756
		public float timeLeft = -999f;

		// Token: 0x040006DD RID: 1757
		public float lastVisitedX;

		// Token: 0x040006DE RID: 1758
		public float lastVisitedTime;

		// Token: 0x040006DF RID: 1759
		public string dangerLevel;

		// Token: 0x040006E0 RID: 1760
		public string hazardsDescription;

		// Token: 0x040006E1 RID: 1761
		protected List<PersistableData> persistables = new List<PersistableData>();

		// Token: 0x040006E2 RID: 1762
		protected bool initializingPersistables;

		// Token: 0x040006E3 RID: 1763
		protected List<AbstractUnitData> units = new List<AbstractUnitData>();

		// Token: 0x040006E4 RID: 1764
		protected List<MapTriggeredPayload> payloads = new List<MapTriggeredPayload>();

		// Token: 0x040006E5 RID: 1765
		public bool hidden;

		// Token: 0x040006E6 RID: 1766
		public bool asteroidsInitialized;

		// Token: 0x040006E9 RID: 1769
		public HazardFieldData hazardFieldData;

		// Token: 0x040006EA RID: 1770
		public Faction oreOwnershipOverride;

		// Token: 0x040006EB RID: 1771
		public PoiStoryteller storyteller;

		// Token: 0x040006EC RID: 1772
		public bool isDynamicPoi;

		// Token: 0x040006ED RID: 1773
		public ulong backgroundSeed;
	}
}
