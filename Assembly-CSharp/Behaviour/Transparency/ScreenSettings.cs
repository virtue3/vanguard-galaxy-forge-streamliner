using System;
using Behaviour.Bootstrap;
using Behaviour.GalaxyMap;
using Behaviour.UI.HUD;
using Behaviour.Util;
using Source.Player;
using UnityEngine;

namespace Behaviour.Transparency
{
	// Token: 0x020002D3 RID: 723
	public class ScreenSettings : MonoBehaviour
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x000A3854 File Offset: 0x000A1A54
		public static int defaultPixelWidth
		{
			get
			{
				return 1152;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x000A385B File Offset: 0x000A1A5B
		public static float nonStackingScreenFactor
		{
			get
			{
				if (!ScreenSettings.doWeStackUI)
				{
					return ScreenSettings.clickableScreenPercentage;
				}
				return ScreenSettings.clickableScreenPercentage / 2f;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001A53 RID: 6739 RVA: 0x000A3875 File Offset: 0x000A1A75
		public static float maxMapWidth
		{
			get
			{
				return (float)(Screen.width - 38) / GameplayerPrefs.GetScaleFactor();
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001A54 RID: 6740 RVA: 0x000A3886 File Offset: 0x000A1A86
		public static float maxMapHeight
		{
			get
			{
				return (float)Screen.height / GameplayerPrefs.GetScaleFactor();
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001A55 RID: 6741 RVA: 0x000A3894 File Offset: 0x000A1A94
		public static float mapHeightGameScreen
		{
			get
			{
				return ScreenSettings.maxMapHeight * ScreenSettings.gameScreenPercentage;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001A56 RID: 6742 RVA: 0x000A38A1 File Offset: 0x000A1AA1
		public static float maxScale
		{
			get
			{
				return ScreenSettings.CalculateScale(ScreenSettings.defaultScale);
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001A57 RID: 6743 RVA: 0x000A38AD File Offset: 0x000A1AAD
		public static float minScale
		{
			get
			{
				return ScreenSettings.defaultMinScale;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001A58 RID: 6744 RVA: 0x000A38B4 File Offset: 0x000A1AB4
		public static float maxScreenPosition
		{
			get
			{
				if (!ScreenSettings.fullscreen)
				{
					return (float)Screen.height - ScreenSettings.clickableScreenPercentage * (float)Screen.height;
				}
				return 0f;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x000A38D6 File Offset: 0x000A1AD6
		public static float minScreenPosition
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x000A38DD File Offset: 0x000A1ADD
		public static bool fullscreen
		{
			get
			{
				return ScreenSettings.gameScreenPercentage == 1f;
			}
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x000A38EC File Offset: 0x000A1AEC
		private static float CalculateScale(float scale)
		{
			return (float)Math.Round((double)Mathf.Min((float)Screen.width / (float)(ScreenSettings.doWeStackUI ? ScreenSettings.defaultPixelWidth : ScreenSettings.pixelWidthStacking) * scale, (float)Screen.height / (float)(ScreenSettings.doWeStackUI ? (ScreenSettings.defaultPixelHeight * 2) : ScreenSettings.defaultPixelHeight) * scale), 2);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x000A3944 File Offset: 0x000A1B44
		private void DetermineStack()
		{
			float scale = GameplayerPrefs.GetScale();
			ScreenSettings.doWeStackUI = ((float)Screen.width < (float)ScreenSettings.pixelWidthStacking * scale / 100f && (float)Screen.height > (float)(ScreenSettings.defaultPixelHeight * 2) * scale / 100f);
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x000A3990 File Offset: 0x000A1B90
		private void Update()
		{
			if (!ScreenSettings.allowScaleUpdate)
			{
				return;
			}
			bool flag = this.lastScreenSize.x != Screen.width || this.lastScreenSize.y != Screen.height;
			this.DetermineStack();
			ScreenSettings.DetermineScale();
			if (flag)
			{
				this.lastScreenSize = new Vector2Int(Screen.width, Screen.height);
				BackdropManager instance = Singleton<BackdropManager>.Instance;
				if (instance != null)
				{
					instance.ResetBackgroundData();
				}
				if (PersistentSingleton<Bootstrapper>.Instance.transparencyMode == TransparencyMode.Windowed)
				{
					GameplayerPrefs.SetWindowedResolution(this.lastScreenSize);
				}
				ScreenSettings.CheckMapSize();
			}
			if (!ScreenSettings.fullscreen && GameplayerPrefs.GetScreenPosition() > ScreenSettings.maxScreenPosition)
			{
				GameplayerPrefs.SetScreenPosition(ScreenSettings.maxScreenPosition);
			}
			else if (!ScreenSettings.fullscreen && GameplayerPrefs.GetScreenPosition() < ScreenSettings.minScreenPosition)
			{
				GameplayerPrefs.SetScreenPosition(ScreenSettings.minScreenPosition);
			}
			ScreenSettings.SetScreenPercentage();
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x000A3A60 File Offset: 0x000A1C60
		public static void CheckMapSize()
		{
			Vector2 galaxyMapSize = GameplayerPrefs.GetGalaxyMapSize();
			if (galaxyMapSize.y > ScreenSettings.maxMapHeight || galaxyMapSize.x > ScreenSettings.maxMapWidth)
			{
				Vector2 vector = new Vector2((galaxyMapSize.x > ScreenSettings.maxMapWidth) ? ScreenSettings.maxMapWidth : galaxyMapSize.x, (galaxyMapSize.y > ScreenSettings.maxMapHeight) ? ScreenSettings.maxMapHeight : galaxyMapSize.y);
				GameplayerPrefs.SetGalaxyMapSize(vector);
				if (AbstractGalaxyMapManager.current && AbstractGalaxyMapManager.current.mapWindow)
				{
					AbstractGalaxyMapManager.current.mapWindow.UpdateSize(vector);
				}
			}
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x000A3AFC File Offset: 0x000A1CFC
		private static void DetermineScale()
		{
			float scale = GameplayerPrefs.GetScale();
			float maxScale = ScreenSettings.maxScale;
			float num = (scale > ScreenSettings.minScale && scale < maxScale) ? scale : GameplayerPrefs.GetAutoScale();
			if (num > maxScale)
			{
				GameplayerPrefs.SetAutoScale(maxScale);
				return;
			}
			if (num < ScreenSettings.minScale)
			{
				if (maxScale < ScreenSettings.minScale)
				{
					GameplayerPrefs.SetAutoScale(maxScale);
					return;
				}
				GameplayerPrefs.SetAutoScale(ScreenSettings.minScale);
				return;
			}
			else
			{
				if (scale > maxScale)
				{
					GameplayerPrefs.SetAutoScale(maxScale);
					return;
				}
				GameplayerPrefs.SetAutoScale(num);
				return;
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x000A3B68 File Offset: 0x000A1D68
		private static void SetMinimumY()
		{
			ScreenSettings.minimumY = (ScreenSettings.fullscreen ? 0f : GameplayerPrefs.GetScreenPosition());
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x000A3B84 File Offset: 0x000A1D84
		public static void SetScreenPercentage()
		{
			ScreenSettings.SetMinimumY();
			float num = GameplayerPrefs.GetAutoScale() / 100f;
			float num2 = (float)(ScreenSettings.doWeStackUI ? (ScreenSettings.defaultPixelHeight * 2) : ScreenSettings.defaultPixelHeight);
			if (num != 1f)
			{
				num2 *= num;
			}
			ScreenSettings.clickableScreenPercentage = num2 / (float)Screen.height;
			if (PersistentSingleton<Bootstrapper>.Instance.transparencyMode == TransparencyMode.FullScreen || PersistentSingleton<Bootstrapper>.Instance.transparencyMode == TransparencyMode.Windowed)
			{
				ScreenSettings.gameScreenPercentage = 1f;
			}
			else
			{
				ScreenSettings.gameScreenPercentage = ScreenSettings.clickableScreenPercentage;
			}
			HudManager instance = HudManager.Instance;
			if ((instance != null) ? instance.seperator : null)
			{
				HudManager.Instance.seperator.SetActive(PersistentSingleton<Bootstrapper>.Instance.transparencyMode == TransparencyMode.Transparent);
			}
		}

		// Token: 0x0400108B RID: 4235
		public static bool doWeStackUI;

		// Token: 0x0400108C RID: 4236
		public static int defaultPixelHeight = 324;

		// Token: 0x0400108D RID: 4237
		public static int pixelWidthStacking = 1920;

		// Token: 0x0400108E RID: 4238
		public static float clickableScreenPercentage = ScreenSettings.doWeStackUI ? 0.6f : 0.3f;

		// Token: 0x0400108F RID: 4239
		public static float gameScreenPercentage = 0.3f;

		// Token: 0x04001090 RID: 4240
		public static float minimumY;

		// Token: 0x04001091 RID: 4241
		public static bool allowScaleUpdate = true;

		// Token: 0x04001092 RID: 4242
		public static float defaultScale = 100f;

		// Token: 0x04001093 RID: 4243
		public static float defaultMinScale = 60f;

		// Token: 0x04001094 RID: 4244
		private Vector2Int lastScreenSize = new Vector2Int(Screen.width, Screen.height);
	}
}
