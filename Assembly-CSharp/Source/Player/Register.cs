using System;
using System.Collections.Generic;
using System.Linq;
using LightJson;
using Source.MissionSystem;

namespace Source.Player
{
	// Token: 0x0200009A RID: 154
	public class Register
	{
		// Token: 0x0600064E RID: 1614 RVA: 0x00036622 File Offset: 0x00034822
		public Register()
		{
			this.Flags = new Dictionary<string, bool>();
			this.Counters = new Dictionary<string, int>();
			this.Data = new Dictionary<string, string>();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00036656 File Offset: 0x00034856
		public static bool FlagExists(string name)
		{
			GamePlayer current = GamePlayer.current;
			return current != null && current.register.Flags.ContainsKey(name);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00036674 File Offset: 0x00034874
		public static bool HasFlag(string name, bool defaultValue = false)
		{
			bool result = defaultValue;
			GamePlayer current = GamePlayer.current;
			if (current != null)
			{
				current.register.Flags.TryGetValue(name, out result);
			}
			return result;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000366A2 File Offset: 0x000348A2
		public static void SetFlag(string name, bool value = true)
		{
			GamePlayer.current.register.Flags[name] = value;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x000366BA File Offset: 0x000348BA
		public static bool CounterExists(string name)
		{
			GamePlayer current = GamePlayer.current;
			return current != null && current.register.Counters.ContainsKey(name);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000366D8 File Offset: 0x000348D8
		public static int GetCounter(string name, int defaultValue = 0)
		{
			int result = defaultValue;
			GamePlayer current = GamePlayer.current;
			if (current != null)
			{
				current.register.Counters.TryGetValue(name, out result);
			}
			return result;
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00036706 File Offset: 0x00034906
		public static void SetCounter(string name, int value)
		{
			GamePlayer.current.register.Counters[name] = value;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00036720 File Offset: 0x00034920
		public static int AddCounter(string name, int add = 1, int defaultValue = 0)
		{
			int num;
			if (!GamePlayer.current.register.Counters.TryGetValue(name, out num))
			{
				num = defaultValue;
			}
			num += add;
			Register.SetCounter(name, num);
			return num;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00036754 File Offset: 0x00034954
		public static bool DataExists(string name)
		{
			GamePlayer current = GamePlayer.current;
			return current != null && current.register.Data.ContainsKey(name);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00036774 File Offset: 0x00034974
		public static string GetData(string name, string defaultValue = null)
		{
			string result = defaultValue;
			GamePlayer current = GamePlayer.current;
			if (current != null)
			{
				current.register.Data.TryGetValue(name, out result);
			}
			return result;
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x000367A2 File Offset: 0x000349A2
		public static void SetData(string name, string value)
		{
			GamePlayer.current.register.Data[name] = value;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x000367BA File Offset: 0x000349BA
		public static bool SystemVisited(string systemGuid)
		{
			GamePlayer current = GamePlayer.current;
			return current != null && current.register.visitedSystems.Contains(systemGuid);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x000367D7 File Offset: 0x000349D7
		public static void AddVisitedSystem(string systemGuid)
		{
			GamePlayer current = GamePlayer.current;
			if (current != null && current.register.visitedSystems.Add(systemGuid))
			{
				MissionObjective.Trigger(MissionTrigger.VisitUniqueSystem, null, null, false);
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00036801 File Offset: 0x00034A01
		public static void ClearVisitedSystems()
		{
			GamePlayer current = GamePlayer.current;
			if (current == null)
			{
				return;
			}
			current.register.visitedSystems.Clear();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x0003681C File Offset: 0x00034A1C
		public static int GetVisitedStationsCount()
		{
			GamePlayer current = GamePlayer.current;
			if (current == null)
			{
				return 0;
			}
			return current.register.visitedSystems.Count;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00036838 File Offset: 0x00034A38
		public JsonObject ToJson()
		{
			JsonObject jsonObject = new JsonObject();
			foreach (KeyValuePair<string, bool> keyValuePair in this.Flags)
			{
				jsonObject[keyValuePair.Key] = new bool?(keyValuePair.Value);
			}
			JsonObject jsonObject2 = new JsonObject();
			foreach (KeyValuePair<string, int> keyValuePair2 in this.Counters)
			{
				jsonObject2[keyValuePair2.Key] = new double?((double)keyValuePair2.Value);
			}
			Register.SetData("VisitedSystems", string.Join(",", this.visitedSystems));
			JsonObject jsonObject3 = new JsonObject();
			foreach (KeyValuePair<string, string> keyValuePair3 in this.Data)
			{
				jsonObject3[keyValuePair3.Key] = keyValuePair3.Value;
			}
			JsonObject jsonObject4 = new JsonObject();
			jsonObject4["Flags"] = jsonObject;
			jsonObject4["Counters"] = jsonObject2;
			jsonObject4["Data"] = jsonObject3;
			return jsonObject4;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x000369BC File Offset: 0x00034BBC
		public static Register FromJson(JsonObject data)
		{
			Register register = new Register();
			foreach (KeyValuePair<string, JsonValue> keyValuePair in data["Flags"].AsJsonObject)
			{
				register.Flags[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (KeyValuePair<string, JsonValue> keyValuePair2 in data["Counters"].AsJsonObject)
			{
				register.Counters[keyValuePair2.Key] = keyValuePair2.Value;
			}
			foreach (KeyValuePair<string, JsonValue> keyValuePair3 in data["Data"].AsJsonObject)
			{
				register.Data[keyValuePair3.Key] = keyValuePair3.Value;
			}
			string text;
			if (register.Data.TryGetValue("VisitedSystems", out text))
			{
				register.visitedSystems = new HashSet<string>(from s in text.Split(',', StringSplitOptions.None)
				where !string.IsNullOrEmpty(s)
				select s);
			}
			else
			{
				register.visitedSystems = new HashSet<string>();
			}
			return register;
		}

		// Token: 0x04000353 RID: 851
		private Dictionary<string, bool> Flags;

		// Token: 0x04000354 RID: 852
		private Dictionary<string, int> Counters;

		// Token: 0x04000355 RID: 853
		private Dictionary<string, string> Data;

		// Token: 0x04000356 RID: 854
		private HashSet<string> visitedSystems = new HashSet<string>();

		// Token: 0x04000357 RID: 855
		public const string OresMined = "OresMined";

		// Token: 0x04000358 RID: 856
		public const string SurfaceOreMined = "SurfaceOreMined";

		// Token: 0x04000359 RID: 857
		public const string CoreOreMined = "CoreOreMined";

		// Token: 0x0400035A RID: 858
		public const string SurfaceOreYieldMax = "SurfaceOreYieldMax";

		// Token: 0x0400035B RID: 859
		public const string CoreOreYieldMax = "CoreOreYieldMax";

		// Token: 0x0400035C RID: 860
		public const string OreStolen = "OreStolen";

		// Token: 0x0400035D RID: 861
		public const string SalvagingScrap = "SalvagingScrap";

		// Token: 0x0400035E RID: 862
		public const string SalvagingScrapYieldMax = "SalvagingScrapYieldMax";

		// Token: 0x0400035F RID: 863
		public const string SalvagingItemsRetrieved = "SalvagingItemsRetrieved";

		// Token: 0x04000360 RID: 864
		public const string SalvagingItemMaxYield = "SalvagingItemMaxYield";

		// Token: 0x04000361 RID: 865
		public const string SalvagingCreditsRetrieved = "SalvagingCreditsRetrieved";

		// Token: 0x04000362 RID: 866
		public const string SalvagingLootboxRetrieved = "SalvagingLootboxRetrieved";

		// Token: 0x04000363 RID: 867
		public const string EmergencyJumps = "EmergencyJumps";

		// Token: 0x04000364 RID: 868
		public const string StationVisited = "StationsVisited";

		// Token: 0x04000365 RID: 869
		public const string CreditsGained = "CreditsGained";

		// Token: 0x04000366 RID: 870
		public const string LootBoxesGained = "LootBoxesGained";

		// Token: 0x04000367 RID: 871
		public const string LootBoxSkillPointsGained = "LootboxSkillPointsGained";

		// Token: 0x04000368 RID: 872
		public const string WorkshopCreditGained = "WorkshopCreditsGained";

		// Token: 0x04000369 RID: 873
		public const string FoundCargoWithCargoScanner = "CargoScannerHit";

		// Token: 0x0400036A RID: 874
		public const string CargoScannerValue = "CargoScannerValue";

		// Token: 0x0400036B RID: 875
		public const string TrackerPlaced = "TrackerPlaced";

		// Token: 0x0400036C RID: 876
		public const string DecoyTransponderUsed = "DecoyTransponderUsed";
	}
}
