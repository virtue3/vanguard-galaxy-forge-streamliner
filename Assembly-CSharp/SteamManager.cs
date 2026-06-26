using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x0200000F RID: 15
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060000BE RID: 190 RVA: 0x00005864 File Offset: 0x00003A64
	protected static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x060000BF RID: 191 RVA: 0x00005888 File Offset: 0x00003A88
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005894 File Offset: 0x00003A94
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000589C File Offset: 0x00003A9C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000058AC File Offset: 0x00003AAC
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(new AppId_t(3471800U)))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogWarning("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00005990 File Offset: 0x00003B90
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000059DE File Offset: 0x00003BDE
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005A04 File Offset: 0x00003C04
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		try
		{
			SteamAPI.RunCallbacks();
		}
		catch (InvalidOperationException)
		{
			this.m_bInitialized = false;
		}
	}

	// Token: 0x0400007E RID: 126
	public const uint VanguardGalaxyAppId = 3471800U;

	// Token: 0x0400007F RID: 127
	public const uint SupporterEditionId = 4621360U;

	// Token: 0x04000080 RID: 128
	public const uint SoundtrackDlcId = 4621120U;

	// Token: 0x04000081 RID: 129
	protected static bool s_EverInitialized;

	// Token: 0x04000082 RID: 130
	protected static SteamManager s_instance;

	// Token: 0x04000083 RID: 131
	protected bool m_bInitialized;

	// Token: 0x04000084 RID: 132
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
