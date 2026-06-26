using System;
using System.Collections.Generic;
using Source.Player;
using Steamworks;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B8 RID: 440
	public class SteamStatsManager : MonoBehaviour
	{
		// Token: 0x06000F62 RID: 3938 RVA: 0x0006ADE1 File Offset: 0x00068FE1
		private void Awake()
		{
			if (SteamStatsManager._instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			SteamStatsManager._instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
			this._initStats();
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x0006AE10 File Offset: 0x00069010
		private void _initStats()
		{
			this._addStat(new SteamStat(SteamStatType.Level, new int[]
			{
				10,
				20,
				30
			}));
			this._addStat(new SteamStat(SteamStatType.OreMined, new int[]
			{
				10,
				1000,
				10000,
				100000
			}));
			this._addStat(new SteamStat(SteamStatType.SalvageCollected, new int[]
			{
				10,
				1000,
				10000,
				100000
			}));
			this._addStat(new SteamStat(SteamStatType.FanaticsDestroyed, new int[]
			{
				10,
				1000
			}));
			this._addStat(new SteamStat(SteamStatType.PiratesDestroyed, new int[]
			{
				10,
				1000
			}));
			this._addStat(new SteamStat(SteamStatType.AutopilotTime, new int[]
			{
				28800
			}));
			this._addStat(new SteamStat(SteamStatType.AutopilotTimeTotal, new int[]
			{
				360000
			}));
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x0006AEE6 File Offset: 0x000690E6
		private void _addStat(SteamStat stat)
		{
			this._stats[stat.Stat] = stat;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x0006AEFC File Offset: 0x000690FC
		private void Update()
		{
			foreach (SteamStat steamStat in this._stats.Values)
			{
				steamStat.Update(Time.deltaTime);
			}
			this._statsStoreTimer -= Time.deltaTime;
			if (this._statsStoreTimer < 0f)
			{
				this._statsStoreTimer = 300f;
				SteamUserStats.StoreStats();
			}
			this._updateRichPresenceTimer -= Time.deltaTime;
			if (this._updateRichPresenceTimer < 0f)
			{
				this._updateRichPresenceTimer = 5f;
				this._updateRichPresence();
			}
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x0006AFB8 File Offset: 0x000691B8
		private void OnApplicationQuit()
		{
			if (SteamManager.Initialized)
			{
				SteamUserStats.StoreStats();
			}
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x0006AFC8 File Offset: 0x000691C8
		private void _updateRichPresence()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			string text = this._getRichPresenceText();
			if (text != null && text != this._richPresenceString)
			{
				this._richPresenceString = text;
				SteamFriends.SetRichPresence("steam_display", text);
			}
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x0006B008 File Offset: 0x00069208
		private string _getRichPresenceText()
		{
			return null;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0006B00B File Offset: 0x0006920B
		public static void Init()
		{
			if (!SteamStatsManager._instance)
			{
				new GameObject("SteamStatsManager").AddComponent<SteamStatsManager>();
			}
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x0006B02C File Offset: 0x0006922C
		public static void Add(SteamStatType type, int count)
		{
			if (!SteamStatsManager._instance)
			{
				return;
			}
			SteamStat steamStat;
			if (SteamStatsManager._instance._stats.TryGetValue(type, out steamStat))
			{
				steamStat.Add(count);
			}
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x0006B064 File Offset: 0x00069264
		public static void Set(SteamStatType type, int val)
		{
			if (!SteamStatsManager._instance)
			{
				return;
			}
			SteamStat steamStat;
			if (SteamStatsManager._instance._stats.TryGetValue(type, out steamStat))
			{
				steamStat.Set(val);
			}
		}

		// Token: 0x040008BD RID: 2237
		public const float StatsStoreInterval = 300f;

		// Token: 0x040008BE RID: 2238
		public const float RichPresenceInterval = 5f;

		// Token: 0x040008BF RID: 2239
		private static SteamStatsManager _instance;

		// Token: 0x040008C0 RID: 2240
		private float _statsStoreTimer = 300f;

		// Token: 0x040008C1 RID: 2241
		private float _updateRichPresenceTimer = 5f;

		// Token: 0x040008C2 RID: 2242
		private bool _loaded;

		// Token: 0x040008C3 RID: 2243
		private Dictionary<SteamStatType, SteamStat> _stats = new Dictionary<SteamStatType, SteamStat>();

		// Token: 0x040008C4 RID: 2244
		private string _richPresenceString;
	}
}
