using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x0200009B RID: 155
	public class SteamAchievement
	{
		// Token: 0x0600065F RID: 1631 RVA: 0x00036B50 File Offset: 0x00034D50
		public static void Trigger(string name)
		{
			if (!SteamAchievement._triggered.Add(name))
			{
				return;
			}
			Debug.Log("Trigger Steam achievement: " + name);
			if (!SteamManager.Initialized)
			{
				return;
			}
			bool flag;
			SteamUserStats.GetAchievement(name, out flag);
			if (!flag)
			{
				SteamUserStats.SetAchievement(name);
				SteamUserStats.StoreStats();
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00036B9C File Offset: 0x00034D9C
		public static void Clear(string name = null)
		{
			if (SteamManager.Initialized)
			{
				if (name == null)
				{
					SteamUserStats.ResetAllStats(true);
				}
				else
				{
					SteamUserStats.ClearAchievement(name);
				}
				SteamUserStats.StoreStats();
			}
		}

		// Token: 0x0400036D RID: 877
		private static HashSet<string> _triggered = new HashSet<string>();
	}
}
