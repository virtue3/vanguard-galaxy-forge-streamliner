using System;
using System.Runtime.CompilerServices;
using Behaviour.Transparency;
using Behaviour.UI;
using Behaviour.Util;
using UnityEngine;

namespace Behaviour.Bootstrap
{
	// Token: 0x020002ED RID: 749
	public class Bootstrapper : PersistentSingleton<Bootstrapper>
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001B53 RID: 6995 RVA: 0x000A6CBA File Offset: 0x000A4EBA
		// (set) Token: 0x06001B54 RID: 6996 RVA: 0x000A6CC2 File Offset: 0x000A4EC2
		public AbstractWindow activeWindow { get; private set; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001B55 RID: 6997 RVA: 0x000A6CCB File Offset: 0x000A4ECB
		// (set) Token: 0x06001B56 RID: 6998 RVA: 0x000A6CD3 File Offset: 0x000A4ED3
		public TransparencyMode transparencyMode { get; private set; }

		// Token: 0x06001B57 RID: 6999 RVA: 0x000A6CDC File Offset: 0x000A4EDC
		private void Start()
		{
			AlertPopup.SetPrefab(this.alertPrefab);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000A6CEC File Offset: 0x000A4EEC
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static async void Init()
		{
			await System.Threading.Tasks.Task.CompletedTask;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000A6D1C File Offset: 0x000A4F1C
		public void SetTransparencyMode(TransparencyMode mode)
		{
			Debug.Log("Setting transparency mode: " + mode.ToString());
			this.transparencyMode = mode;
			if (mode == TransparencyMode.Transparent || mode == TransparencyMode.Performant)
			{
				this.activeWindow = this.transparentWindow;
				this.transparentWindow.SetDefaultColorMode();
				if (this.transparentWindow.isActiveAndEnabled)
				{
					this.transparentWindow.SwitchTransparencyMode();
				}
				this.fullScreenWindow.gameObject.SetActive(false);
			}
			else
			{
				TransparentWindow transparentWindow = this.activeWindow as TransparentWindow;
				if (transparentWindow != null)
				{
					transparentWindow.RemoveTransparency();
				}
				this.transparentWindow.gameObject.SetActive(false);
				this.activeWindow = this.fullScreenWindow;
				this.fullScreenWindow.SetWindowed(mode == TransparencyMode.Windowed);
			}
			this.activeWindow.gameObject.SetActive(true);
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000A6DE7 File Offset: 0x000A4FE7
		public float GetInitialMinimumY()
		{
			if (this.activeWindow == null)
			{
				return 0f;
			}
			return this.activeWindow.GetMinimumY();
		}

		// Token: 0x04001125 RID: 4389
		[SerializeField]
		private TransparentWindow transparentWindow;

		// Token: 0x04001126 RID: 4390
		[SerializeField]
		private FullScreenWindow fullScreenWindow;

		// Token: 0x04001127 RID: 4391
		[SerializeField]
		private AlertPopup alertPrefab;

		// Token: 0x0400112A RID: 4394
		private static SteamCommunication steamCommunication;
	}
}
