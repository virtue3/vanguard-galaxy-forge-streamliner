using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.UI;
using Behaviour.Util;
using LightJson;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000149 RID: 329
	public class SectorMapData : MapElement
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00059B3B File Offset: 0x00057D3B
		public static SectorMapData current
		{
			get
			{
				SystemMapData current = SystemMapData.current;
				if (current == null)
				{
					return null;
				}
				return current.sector;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x00059B4D File Offset: 0x00057D4D
		public IEnumerable<SystemMapData> allSystems
		{
			get
			{
				return this.systems;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00059B58 File Offset: 0x00057D58
		public Vector2 mapPosition
		{
			get
			{
				return new Vector2(this.position.x * 15f + (float)(this.quadrant * 1000), this.position.y * 7.5f + (float)(this.quadrant * 50));
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x00059BA5 File Offset: 0x00057DA5
		public Vector2 mapPositionGalaxy
		{
			get
			{
				return new Vector2((float)(this.quadrant * 75), (float)(this.quadrant * 10));
			}
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00059BC0 File Offset: 0x00057DC0
		public SectorMapData()
		{
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00059BD3 File Offset: 0x00057DD3
		public SectorMapData(Vector2 pos)
		{
			this.position = pos;
			this.SetBackgroundProperties();
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00059BF4 File Offset: 0x00057DF4
		public void SetBackgroundProperties()
		{
			this.backgroundSeed = SeededRandom.Global.RandomInt();
			this.globalLightColor = Color.Lerp(Color.white, this.GetBackgroundColor(), 0.5f);
			this.globalLightIntensity = SeededRandom.Global.RandomRange(0.3f, 0.5f);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00059C48 File Offset: 0x00057E48
		public bool IsUnlocked()
		{
			if (SectorMapData.current == this)
			{
				return true;
			}
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					JumpGate jumpGate = mapPointOfInterest as JumpGate;
					if (jumpGate != null && jumpGate.sectorJumpgate && jumpGate.canUseJumpGate)
					{
						return true;
					}
					EmbassyJumpgate embassyJumpgate = mapPointOfInterest as EmbassyJumpgate;
					if (embassyJumpgate != null && embassyJumpgate.canUseJumpGate)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00059D10 File Offset: 0x00057F10
		public Color GetBackgroundColor()
		{
			return Singleton<BackdropManager>.Instance.GetNebulaColor(new SeedGenerator().Add(this.backgroundSeed).CreateRandom().RandomRange(0f, 1f));
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00059D48 File Offset: 0x00057F48
		public Vector2 GetStarTiling(bool close)
		{
			SeededRandom seededRandom = new SeedGenerator().Add(this.backgroundSeed).CreateRandom();
			return new Vector2((float)seededRandom.RandomRange(close ? 5 : 30, close ? 15 : 80), (float)seededRandom.RandomRange(close ? 1 : 7, close ? 4 : 20));
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x00059DA3 File Offset: 0x00057FA3
		public float GetStarScale()
		{
			return (float)new SeedGenerator().Add(this.backgroundSeed).CreateRandom().RandomRange(1, 3);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00059DC7 File Offset: 0x00057FC7
		public void AddSystem(SystemMapData system)
		{
			this.systems.Add(system);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00059DD8 File Offset: 0x00057FD8
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			bool flag = false;
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (!mission.isComplete || mission.turnIn == null || mission.turnIn.system.sector != this)
				{
					MissionStep currentStep = mission.currentStep;
					if (currentStep == null || !currentStep.IsPointOfInterest(this))
					{
						continue;
					}
				}
				if (!flag)
				{
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
					tooltip.AddTextLine("@MapPOIMissions", 12, 8f);
					flag = true;
				}
				tooltip.AddTextLine(mission.name, 12, 8f).Text.color = mission.difficulty.GetColor();
			}
			bool flag2 = false;
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				foreach (string text in systemMapData.GetAvailableMissionHints(null))
				{
					if (!flag2)
					{
						tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
						tooltip.AddTextLine("@MapPOIAvailableMission", 12, 8f);
						flag2 = true;
					}
					tooltip.AddTextLine(text, 12, 8f).Text.color = ColorHelper.detailsColor;
				}
			}
			List<string> list = new List<string>();
			float num = 0f;
			foreach (SystemMapData systemMapData2 in this.systems)
			{
				foreach (SpaceStation spaceStation in systemMapData2.GetPointsOfInterest<SpaceStation>())
				{
					if (spaceStation.materialStorage.count > 0)
					{
						foreach (Inventory.InventoryItem inventoryItem in spaceStation.materialStorage.items)
						{
							num += inventoryItem.item.m3 * (float)inventoryItem.count;
							list.Add(Translation.Translate(inventoryItem.item.displayName, Array.Empty<object>()));
						}
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
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0005A12C File Offset: 0x0005832C
		public override float GetLastVisitedTime()
		{
			float num = 0f;
			foreach (SystemMapData systemMapData in this.systems)
			{
				num = ((num < systemMapData.GetLastVisitedTime()) ? systemMapData.GetLastVisitedTime() : num);
			}
			return num;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0005A194 File Offset: 0x00058394
		public override void ActiveUpdate(float delta)
		{
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0005A196 File Offset: 0x00058396
		public bool Contains(SystemMapData currentSystem)
		{
			return this.systems.Contains(currentSystem);
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0005A1A4 File Offset: 0x000583A4
		public IEnumerable<JumpGate> GetSectorJumpgates()
		{
			foreach (SystemMapData systemMapData in this.systems)
			{
				foreach (JumpGate jumpGate in systemMapData.GetJumpGateList(false))
				{
					if (jumpGate.sectorJumpgate)
					{
						yield return jumpGate;
					}
				}
				IEnumerator<JumpGate> enumerator2 = null;
			}
			List<SystemMapData>.Enumerator enumerator = default(List<SystemMapData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0005A1B4 File Offset: 0x000583B4
		public SpaceStation FindStationForFaction(Faction owner, bool? pocketSystem = null)
		{
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				if (pocketSystem == null || systemMapData.pocketSystem == pocketSystem.Value)
				{
					foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
					{
						SpaceStation spaceStation = mapPointOfInterest as SpaceStation;
						if (spaceStation != null && mapPointOfInterest.faction == owner)
						{
							return spaceStation;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0005A26C File Offset: 0x0005846C
		public MapPointOfInterest FindJumpgateToSector(SectorMapData sectorMapData)
		{
			foreach (SystemMapData systemMapData in this.systems)
			{
				foreach (MapPointOfInterest mapPointOfInterest in systemMapData.pointsOfInterest)
				{
					JumpGate gate = mapPointOfInterest as JumpGate;
					if (gate != null && sectorMapData.systems.FirstOrDefault((SystemMapData s) => s == gate.targetSystem) != null)
					{
						return mapPointOfInterest;
					}
				}
			}
			return null;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0005A330 File Offset: 0x00058530
		public SystemMapData GetRandomSystemInRange(float lowPercent = 0f, float highPercent = 1f, bool pocketSystem = false)
		{
			lowPercent = Mathf.Clamp01(lowPercent);
			highPercent = Mathf.Clamp01(highPercent);
			if (lowPercent > highPercent)
			{
				float num = highPercent;
				float num2 = lowPercent;
				lowPercent = num;
				highPercent = num2;
			}
			List<SystemMapData> list = (from s in this.allSystems
			where pocketSystem || !s.pocketSystem
			select s).ToList<SystemMapData>();
			if (list.Count == 0)
			{
				return null;
			}
			int num3 = Mathf.FloorToInt((float)list.Count * lowPercent);
			int num4 = Mathf.CeilToInt((float)list.Count * highPercent);
			num3 = Mathf.Clamp(num3, 0, list.Count - 1);
			num4 = Mathf.Clamp(num4, num3 + 1, list.Count);
			List<SystemMapData> list2 = list.Skip(num3).Take(num4 - num3).ToList<SystemMapData>();
			if (list2.Count == 0)
			{
				return null;
			}
			return list2[SeededRandom.Global.RandomRange(0, list2.Count)];
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0005A408 File Offset: 0x00058608
		public string GetNameLabel()
		{
			ValueTuple<int, int> sectorLevel = this.sectorLevel;
			if (sectorLevel.Item2 == 0)
			{
				return base.name + " (" + sectorLevel.Item1.ToString() + "+)";
			}
			return string.Concat(new string[]
			{
				base.name,
				" (",
				sectorLevel.Item1.ToString(),
				"-",
				sectorLevel.Item2.ToString(),
				")"
			});
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0005A490 File Offset: 0x00058690
		public ValueTuple<int, int> sectorLevel
		{
			get
			{
				int num = 9999;
				int num2 = 0;
				foreach (SystemMapData systemMapData in this.allSystems)
				{
					num = Math.Min(num, systemMapData.level);
					if (systemMapData.storyteller == null)
					{
						num2 = Math.Max(num2, systemMapData.level);
					}
				}
				return new ValueTuple<int, int>(num, num2);
			}
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0005A508 File Offset: 0x00058708
		public T RandomPointOfInterest<T>(SectorMapData.PointOfInterestFilter<T> filter, SeededRandom random = null) where T : MapPointOfInterest
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			List<T> list = new List<T>();
			foreach (SystemMapData systemMapData in this.allSystems)
			{
				foreach (T t in systemMapData.GetPointsOfInterest<T>())
				{
					if (filter(t))
					{
						list.Add(t);
					}
				}
			}
			return random.Choose<T>(list);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0005A5A8 File Offset: 0x000587A8
		public SpaceStation GetSpaceStationForFactionAtDistance(Faction faction, int sectorDistance)
		{
			Queue<ValueTuple<SectorMapData, int>> queue = new Queue<ValueTuple<SectorMapData, int>>();
			queue.Enqueue(new ValueTuple<SectorMapData, int>(this, 0));
			int num = 0;
			SpaceStation spaceStation = null;
			while (queue.Count > 0 && num <= sectorDistance)
			{
				ValueTuple<SectorMapData, int> valueTuple = queue.Dequeue();
				SectorMapData item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				num = item2;
				foreach (JumpGate jumpGate in item.GetSectorJumpgates())
				{
					if (jumpGate.canUseJumpGate && jumpGate.targetSystem.sector != this)
					{
						queue.Enqueue(new ValueTuple<SectorMapData, int>(jumpGate.targetSystem.sector, item2 + 1));
						if (item2 < sectorDistance)
						{
							spaceStation = null;
						}
						else
						{
							SpaceStation spaceStation2 = item.FindStationForFaction(faction, null);
							if (spaceStation2 != null && item2 == sectorDistance)
							{
								spaceStation = spaceStation2;
							}
						}
					}
				}
				if (spaceStation != null)
				{
					return spaceStation;
				}
			}
			return null;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0005A698 File Offset: 0x00058898
		public override void DataToJson(JsonObject data)
		{
			data["systems"] = this.systems.ToJsonArray<SystemMapData>();
			data["globalLightColor"] = ColorUtility.ToHtmlStringRGBA(this.globalLightColor);
			data["globalLightIntensity"] = new double?((double)this.globalLightIntensity);
			data["backgroundSeed"] = this.backgroundSeed.ToString();
			data["quadrant"] = new double?((double)this.quadrant);
			if (this.conquestSector)
			{
				data["conquestSector"] = new bool?(true);
			}
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0005A74C File Offset: 0x0005894C
		public override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data2);
			this.systems.FromJsonArray(data2["systems"], (JsonValue data) => SystemMapData.FromJson(this, data));
			ColorUtility.TryParseHtmlString("#" + data2["globalLightColor"], out this.globalLightColor);
			this.globalLightIntensity = (float)data2["globalLightIntensity"].AsNumber;
			this.backgroundSeed = (uint)data2["backgroundSeed"].AsInteger;
			this.quadrant = data2["quadrant"];
			this.conquestSector = data2["conquestSector"];
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0005A80C File Offset: 0x00058A0C
		public static SectorMapData FromJson(JsonValue data)
		{
			SectorMapData sectorMapData = new SectorMapData();
			sectorMapData.LoadFromJson(data);
			return sectorMapData;
		}

		// Token: 0x040006F5 RID: 1781
		public static int quadrantPrologue = 0;

		// Token: 0x040006F6 RID: 1782
		public static int quadrantFrontier = 1;

		// Token: 0x040006F7 RID: 1783
		public static int quadrantConquest = 2;

		// Token: 0x040006F8 RID: 1784
		public int quadrant;

		// Token: 0x040006F9 RID: 1785
		private List<SystemMapData> systems = new List<SystemMapData>();

		// Token: 0x040006FA RID: 1786
		public Color globalLightColor;

		// Token: 0x040006FB RID: 1787
		public float globalLightIntensity;

		// Token: 0x040006FC RID: 1788
		public uint backgroundSeed;

		// Token: 0x040006FD RID: 1789
		public bool conquestSector;

		// Token: 0x020004C7 RID: 1223
		// (Invoke) Token: 0x060029C2 RID: 10690
		public delegate bool PointOfInterestFilter<T>(T t) where T : MapPointOfInterest;
	}
}
