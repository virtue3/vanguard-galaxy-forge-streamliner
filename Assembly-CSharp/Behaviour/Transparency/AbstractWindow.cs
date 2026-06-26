using System;
using System.Collections;
using Behaviour.Util;
using UnityEngine;

namespace Behaviour.Transparency
{
	// Token: 0x020002CF RID: 719
	public abstract class AbstractWindow : PersistentSingleton<AbstractWindow>
	{
		// Token: 0x06001A3E RID: 6718 RVA: 0x000A36C4 File Offset: 0x000A18C4
		protected virtual void OnEnable()
		{
			base.StartCoroutine(this.SwitchScreenPercentage());
			this.windowEnabled = true;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x000A36DA File Offset: 0x000A18DA
		private void OnDisable()
		{
			this.windowEnabled = false;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x000A36E3 File Offset: 0x000A18E3
		private IEnumerator SwitchScreenPercentage()
		{
			yield return new WaitForEndOfFrame();
			ScreenSettings.SetScreenPercentage();
			BackdropManager instance = Singleton<BackdropManager>.Instance;
			if (instance != null)
			{
				instance.ResetBackgroundData();
			}
			yield break;
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001A41 RID: 6721 RVA: 0x000A36EB File Offset: 0x000A18EB
		// (set) Token: 0x06001A42 RID: 6722 RVA: 0x000A36F3 File Offset: 0x000A18F3
		public bool isClickThrough { get; protected set; }

		// Token: 0x06001A43 RID: 6723 RVA: 0x000A36FC File Offset: 0x000A18FC
		protected IEnumerator ResolutionThereAndBackAgain()
		{
			DisplayInfo currentDisplay = Screen.mainWindowDisplayInfo;
			ScreenSettings.allowScaleUpdate = false;
			yield return new WaitForSeconds(0.05f);
			Screen.SetResolution(currentDisplay.width, currentDisplay.height, FullScreenMode.Windowed);
			Debug.Log("RTABA - Change to 400 x 300");
			yield return new WaitForSeconds(0.05f);
			Screen.SetResolution(currentDisplay.width, currentDisplay.height, FullScreenMode.FullScreenWindow);
			Debug.Log("RTABA - Change to " + currentDisplay.width.ToString() + " x " + currentDisplay.height.ToString());
			yield return new WaitForSeconds(0.05f);
			ScreenSettings.allowScaleUpdate = true;
			Debug.Log("Done with resolution change");
			yield break;
		}

		// Token: 0x06001A44 RID: 6724
		public abstract void ToggleSteamOverlay(bool enabled);

		// Token: 0x06001A45 RID: 6725
		public abstract float GetMinimumY();

		// Token: 0x04001085 RID: 4229
		public Camera fullScreenCamera;

		// Token: 0x04001086 RID: 4230
		protected bool windowEnabled;
	}
}
