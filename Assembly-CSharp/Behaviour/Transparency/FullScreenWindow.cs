using System;
using Source.Player;
using UnityEngine;

namespace Behaviour.Transparency
{
	// Token: 0x020002D1 RID: 721
	public class FullScreenWindow : AbstractWindow
	{
		// Token: 0x06001A4A RID: 6730 RVA: 0x000A37BB File Offset: 0x000A19BB
		protected override void Awake()
		{
			base.Awake();
			this.fullScreenCamera.backgroundColor = Color.black;
			base.isClickThrough = false;
			ScreenSettings.SetScreenPercentage();
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x000A37DF File Offset: 0x000A19DF
		protected override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x000A37E8 File Offset: 0x000A19E8
		public void SetWindowed(bool windowed)
		{
			if (!windowed)
			{
				Screen.SetResolution(Screen.mainWindowDisplayInfo.width, Screen.mainWindowDisplayInfo.height, true);
				return;
			}
			Vector2Int windowedResolution = GameplayerPrefs.GetWindowedResolution();
			Screen.SetResolution(windowedResolution.x, windowedResolution.y, false);
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000A382D File Offset: 0x000A1A2D
		public override float GetMinimumY()
		{
			return 0f;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x000A3834 File Offset: 0x000A1A34
		public override void ToggleSteamOverlay(bool enabled)
		{
			Debug.Log("Steamoverlay while fullscreen? :) " + enabled.ToString());
		}
	}
}
