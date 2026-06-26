using System;
using Source.Player;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.Transparency
{
	// Token: 0x020002D0 RID: 720
	public class CanvasScaleUpdater : MonoBehaviour
	{
		// Token: 0x06001A47 RID: 6727 RVA: 0x000A370C File Offset: 0x000A190C
		private void Start()
		{
			this.canvasScaler = base.GetComponent<CanvasScaler>();
			this.currentScale = GameplayerPrefs.GetScaleFactor();
			this.canvasScaler.scaleFactor = this.currentScale;
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x000A3738 File Offset: 0x000A1938
		private void Update()
		{
			if (!this.canvasScaler)
			{
				Debug.Log("No canvasScaler in : " + base.name);
				return;
			}
			if (!ScreenSettings.clickableScreenPercentage.ApproximatelyEqual(this.currentScreenPercentage) || !this.currentScale.ApproximatelyEqual(GameplayerPrefs.GetScaleFactor()))
			{
				this.currentScale = GameplayerPrefs.GetScaleFactor();
				this.canvasScaler.scaleFactor = this.currentScale;
				this.currentScreenPercentage = ScreenSettings.clickableScreenPercentage;
			}
		}

		// Token: 0x04001088 RID: 4232
		private float currentScreenPercentage;

		// Token: 0x04001089 RID: 4233
		private float currentScale;

		// Token: 0x0400108A RID: 4234
		private CanvasScaler canvasScaler;
	}
}
