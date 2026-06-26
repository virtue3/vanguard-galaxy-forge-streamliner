using System;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B4 RID: 436
	public class Singleton<T> : MonoBehaviour where T : Component
	{
		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0006A185 File Offset: 0x00068385
		public static bool HasInstance
		{
			get
			{
				return Singleton<T>.instance != null;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0006A197 File Offset: 0x00068397
		public static T Current
		{
			get
			{
				return Singleton<T>.instance;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0006A19E File Offset: 0x0006839E
		public static T Instance
		{
			get
			{
				if (Singleton<T>.instance == null)
				{
					Singleton<T>.instance = UnityEngine.Object.FindFirstObjectByType<T>();
				}
				return Singleton<T>.instance;
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0006A1C1 File Offset: 0x000683C1
		protected virtual void Awake()
		{
			this.InitializeSingleton();
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0006A1CC File Offset: 0x000683CC
		protected virtual void InitializeSingleton()
		{
			if (this.UnparentOnAwake)
			{
				base.transform.SetParent(null);
			}
			if (Singleton<T>.instance == null)
			{
				Singleton<T>.instance = (this as T);
				return;
			}
			if (this != Singleton<T>.instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x040008A3 RID: 2211
		[Tooltip("If this is true, this singleton will auto detach if it finds itself parented on awake")]
		public bool UnparentOnAwake = true;

		// Token: 0x040008A4 RID: 2212
		protected static T instance;
	}
}
