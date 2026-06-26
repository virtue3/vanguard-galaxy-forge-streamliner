using System;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B3 RID: 435
	public class PersistentSingleton<T> : MonoBehaviour where T : Component
	{
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0006A0A8 File Offset: 0x000682A8
		public static bool HasInstance
		{
			get
			{
				return PersistentSingleton<T>.instance != null;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0006A0BA File Offset: 0x000682BA
		public static T Current
		{
			get
			{
				return PersistentSingleton<T>.instance;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0006A0C1 File Offset: 0x000682C1
		public static T Instance
		{
			get
			{
				if (PersistentSingleton<T>.instance == null)
				{
					PersistentSingleton<T>.instance = UnityEngine.Object.FindFirstObjectByType<T>();
				}
				return PersistentSingleton<T>.instance;
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0006A0F5 File Offset: 0x000682F5
		protected virtual void Awake()
		{
			if (PersistentSingleton<T>.instance == null)
			{
				this.InitializeSingleton();
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0006A110 File Offset: 0x00068310
		protected virtual void InitializeSingleton()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (PersistentSingleton<T>.instance == null)
			{
				PersistentSingleton<T>.instance = (this as T);
				base.enabled = true;
				return;
			}
			if (this != PersistentSingleton<T>.instance)
			{
				Debug.LogError(base.name + " - Initialising PersistentSingleton that already exists");
			}
		}

		// Token: 0x040008A1 RID: 2209
		[Tooltip("if this is true, this singleton will auto detach if it finds itself parented on awake")]
		public bool UnparentOnAwake = true;

		// Token: 0x040008A2 RID: 2210
		protected static T instance;
	}
}
