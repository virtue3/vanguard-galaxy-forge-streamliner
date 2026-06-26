using System;
using System.Collections.Generic;
using LightJson;
using LightJson.Serialization;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000047 RID: 71
	public static class SupporterCallsigns
	{
		// Token: 0x060002F5 RID: 757 RVA: 0x0001829C File Offset: 0x0001649C
		public static List<SupporterCallsigns.Entry> ParseJson(string json)
		{
			List<SupporterCallsigns.Entry> list = new List<SupporterCallsigns.Entry>();
			if (string.IsNullOrWhiteSpace(json))
			{
				return list;
			}
			JsonArray asJsonArray = JsonReader.Parse(json).AsJsonArray;
			if (asJsonArray == null)
			{
				return list;
			}
			foreach (JsonValue jsonValue in asJsonArray)
			{
				JsonObject asJsonObject = jsonValue.AsJsonObject;
				if (asJsonObject != null)
				{
					list.Add(new SupporterCallsigns.Entry
					{
						steamId = (asJsonObject["steamId"].AsString ?? ""),
						name = (asJsonObject["name"].AsString ?? ""),
						faction = (asJsonObject["faction"].AsString ?? "")
					});
				}
			}
			return list;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00018394 File Offset: 0x00016594
		public static string SerializeJson(IEnumerable<SupporterCallsigns.Entry> entries, bool pretty = true)
		{
			JsonArray jsonArray = new JsonArray();
			foreach (SupporterCallsigns.Entry entry in entries)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"name",
						entry.name
					},
					{
						"faction",
						entry.faction
					}
				});
			}
			return jsonArray.ToString(pretty);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00018424 File Offset: 0x00016624
		public static string SerializeJsonFull(IEnumerable<SupporterCallsigns.Entry> entries, bool pretty = true)
		{
			JsonArray jsonArray = new JsonArray();
			foreach (SupporterCallsigns.Entry entry in entries)
			{
				jsonArray.Add(new JsonObject
				{
					{
						"steamId",
						entry.steamId
					},
					{
						"name",
						entry.name
					},
					{
						"faction",
						entry.faction
					}
				});
			}
			return jsonArray.ToString(pretty);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000184C8 File Offset: 0x000166C8
		public static void LoadAll()
		{
			SupporterCallsigns.EnsureLoaded();
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000184CF File Offset: 0x000166CF
		public static void Invalidate()
		{
			SupporterCallsigns._all = null;
			SupporterCallsigns._anyCallsigns = null;
			SupporterCallsigns._byFaction = null;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000184E4 File Offset: 0x000166E4
		private static void EnsureLoaded()
		{
			if (SupporterCallsigns._all != null)
			{
				return;
			}
			SupporterCallsigns._all = new List<SupporterCallsigns.Entry>();
			SupporterCallsigns._anyCallsigns = new List<string>();
			SupporterCallsigns._byFaction = new Dictionary<string, List<string>>();
			TextAsset textAsset = Resources.Load<TextAsset>("SupporterCallsigns");
			if (textAsset == null)
			{
				return;
			}
			SupporterCallsigns._all = SupporterCallsigns.ParseJson(textAsset.text);
			foreach (SupporterCallsigns.Entry entry in SupporterCallsigns._all)
			{
				if (string.IsNullOrEmpty(entry.faction) || entry.faction == "Any")
				{
					SupporterCallsigns._anyCallsigns.Add(entry.name);
				}
				else
				{
					List<string> list;
					if (!SupporterCallsigns._byFaction.TryGetValue(entry.faction, out list))
					{
						list = new List<string>();
						SupporterCallsigns._byFaction[entry.faction] = list;
					}
					list.Add(entry.name);
				}
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002FB RID: 763 RVA: 0x000185E4 File Offset: 0x000167E4
		public static IReadOnlyList<string> AnyCallsigns
		{
			get
			{
				SupporterCallsigns.EnsureLoaded();
				return SupporterCallsigns._anyCallsigns;
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000185F0 File Offset: 0x000167F0
		public static IReadOnlyList<string> GetFactionCallsigns(string faction)
		{
			SupporterCallsigns.EnsureLoaded();
			List<string> list;
			if (!string.IsNullOrEmpty(faction) && SupporterCallsigns._byFaction.TryGetValue(faction, out list) && list.Count > 0)
			{
				return list;
			}
			return null;
		}

		// Token: 0x0400018E RID: 398
		private static List<SupporterCallsigns.Entry> _all;

		// Token: 0x0400018F RID: 399
		private static List<string> _anyCallsigns;

		// Token: 0x04000190 RID: 400
		private static Dictionary<string, List<string>> _byFaction;

		// Token: 0x0200040C RID: 1036
		public struct Entry
		{
			// Token: 0x04001799 RID: 6041
			public string steamId;

			// Token: 0x0400179A RID: 6042
			public string name;

			// Token: 0x0400179B RID: 6043
			public string faction;
		}
	}
}
