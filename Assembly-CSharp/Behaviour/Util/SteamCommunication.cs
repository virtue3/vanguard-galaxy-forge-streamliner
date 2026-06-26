using System;
using Behaviour.Bootstrap;
using Steamworks;

namespace Behaviour.Util
{
	// Token: 0x020001B7 RID: 439
	public class SteamCommunication
	{
		// Token: 0x06000F5F RID: 3935 RVA: 0x0006AD24 File Offset: 0x00068F24
		public void InitSteam()
		{
			if (SteamManager.Initialized)
			{
				this.overlayActivatedCallback = Callback<GameOverlayActivated_t>.Create(new Callback<GameOverlayActivated_t>.DispatchDelegate(this.OnOverlayActivated));
				SteamCommunication.runningOnSteamDeck = SteamUtils.IsSteamRunningOnSteamDeck();
				SteamCommunication.supporterEdition = SteamApps.BIsDlcInstalled(new AppId_t(4621360U));
				SteamCommunication.soundtrackDlc = SteamApps.BIsDlcInstalled(new AppId_t(4621120U));
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x0006AD84 File Offset: 0x00068F84
		protected void OnOverlayActivated(GameOverlayActivated_t callback)
		{
			bool flag = callback.m_bActive == 1;
			uint appId = callback.m_nAppID.m_AppId;
			if (!flag || appId == 0U || appId == 3471800U)
			{
				SteamCommunication.overlayActivated = flag;
				PersistentSingleton<Bootstrapper>.Current.activeWindow.ToggleSteamOverlay(flag);
				if (flag)
				{
					GameManager.Pause();
					return;
				}
				GameManager.Unpause();
			}
		}

		// Token: 0x040008B8 RID: 2232
		public static bool runningOnSteamDeck;

		// Token: 0x040008B9 RID: 2233
		public static bool overlayActivated;

		// Token: 0x040008BA RID: 2234
		public static bool supporterEdition;

		// Token: 0x040008BB RID: 2235
		public static bool soundtrackDlc;

		// Token: 0x040008BC RID: 2236
		protected Callback<GameOverlayActivated_t> overlayActivatedCallback;
	}
}
