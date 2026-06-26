using System;
using Behaviour.AudioSystem;
using Behaviour.Bootstrap;
using Behaviour.Transparency;
using Behaviour.Util;
using Source.Simulation.World;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x02000098 RID: 152
	public static class GameplayerPrefs
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x000358EC File Offset: 0x00033AEC
		public static bool GetAnsweredFullscreen()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.AnsweredFullscreen.ToString(), 0) > 0;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00035914 File Offset: 0x00033B14
		public static void SetAnsweredFullscreen(bool answeredFullscreen)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.AnsweredFullscreen.ToString(), answeredFullscreen ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00035944 File Offset: 0x00033B44
		public static void SetScale(float scale)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.Scale.ToString(), scale);
			PlayerPrefs.Save();
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0003596C File Offset: 0x00033B6C
		public static float GetScale()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.Scale.ToString(), ScreenSettings.defaultScale);
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00035994 File Offset: 0x00033B94
		public static void SetAutoScale(float manualScale)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.AutoScale.ToString(), manualScale);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x000359B8 File Offset: 0x00033BB8
		public static float GetAutoScale()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.AutoScale.ToString(), ScreenSettings.defaultScale);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x000359DE File Offset: 0x00033BDE
		public static float GetScaleFactor()
		{
			return GameplayerPrefs.GetAutoScale() / 100f;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x000359EC File Offset: 0x00033BEC
		public static void SetScreenPosition(float screenPosition)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.ScreenPosition.ToString(), screenPosition);
			PlayerPrefs.Save();
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00035A14 File Offset: 0x00033C14
		public static float GetScreenPosition()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.ScreenPosition.ToString(), PersistentSingleton<Bootstrapper>.Instance.GetInitialMinimumY());
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00035A40 File Offset: 0x00033C40
		public static bool GetAlwaysOnTop()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.AlwaysOnTop.ToString(), 1) > 0;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00035A68 File Offset: 0x00033C68
		public static void SetAlwaysOnTop(bool alwaysOnTop)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.AlwaysOnTop.ToString(), alwaysOnTop ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00035A98 File Offset: 0x00033C98
		public static bool GetTravelHints()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.TravelHints.ToString(), 1) > 0;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00035AC0 File Offset: 0x00033CC0
		public static void SetTravelHints(bool travelHints)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.TravelHints.ToString(), travelHints ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00035AF0 File Offset: 0x00033CF0
		public static bool GetShowDamageNumbers()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.ShowDamageNumbers.ToString(), 1) > 0;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00035B18 File Offset: 0x00033D18
		public static void SetShowDamageNumbers(bool showDamageNumbers)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.ShowDamageNumbers.ToString(), showDamageNumbers ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00035B48 File Offset: 0x00033D48
		public static float GetMusicVolume()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.MusicVolume.ToString(), 0.4f);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00035B70 File Offset: 0x00033D70
		public static void SetMusicVolume(float volume)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.MusicVolume.ToString(), volume);
			PlayerPrefs.Save();
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00035B98 File Offset: 0x00033D98
		public static float GetSoundEffectVolume()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.SoundEffectVolume.ToString(), 1f);
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00035BC0 File Offset: 0x00033DC0
		public static void SetSoundEffectVolume(float volume)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.SoundEffectVolume.ToString(), volume);
			PlayerPrefs.Save();
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00035BE8 File Offset: 0x00033DE8
		public static float GetThrusterVolume()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.ThrusterVolume.ToString(), 0.4f);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00035C10 File Offset: 0x00033E10
		public static void SetThrusterVolume(float volume)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.ThrusterVolume.ToString(), volume);
			PlayerPrefs.Save();
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00035C38 File Offset: 0x00033E38
		public static bool GetAudioMuted()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.AudioMuted.ToString(), 0) > 0;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00035C60 File Offset: 0x00033E60
		public static void SetAudioMuted(bool audioMuted)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.AudioMuted.ToString(), audioMuted ? 1 : 0);
			PersistentSingleton<SoundManager>.Instance.SetGameVolume();
			PersistentSingleton<MusicManager>.Instance.SetMusicVolume();
			PlayerPrefs.Save();
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00035CA4 File Offset: 0x00033EA4
		public static string GetTransparencyMode()
		{
			string text = PlayerPrefs.GetString(PlayerPrefKeys.TransparencyMode.ToString(), GameplayerPrefs.GetDefaultTransparencyMode());
			if (!Enum.IsDefined(typeof(TransparencyMode), text))
			{
				text = GameplayerPrefs.GetDefaultTransparencyMode();
			}
			return text;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00035CE8 File Offset: 0x00033EE8
		private static string GetDefaultTransparencyMode()
		{
			return TransparencyMode.FullScreen.ToString();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00035D04 File Offset: 0x00033F04
		public static void SetTransparencyMode(string mode)
		{
			PlayerPrefs.SetString(PlayerPrefKeys.TransparencyMode.ToString(), mode);
			if (mode != TransparencyMode.FullScreen.ToString())
			{
				PlayerPrefs.SetString(PlayerPrefKeys.TransparencyPreference.ToString(), mode);
			}
			PlayerPrefs.Save();
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00035D5C File Offset: 0x00033F5C
		public static string GetPreferredTransparencyMode()
		{
			return PlayerPrefs.GetString(PlayerPrefKeys.TransparencyPreference.ToString(), GameplayerPrefs.GetDefaultTransparencyMode());
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00035D84 File Offset: 0x00033F84
		public static string GetBackgroundSystemId()
		{
			string text = PlayerPrefs.GetString(PlayerPrefKeys.BackgroundSystem.ToString(), "tutorial_system_1");
			if (!TutorialWorld.systemNameToId.ContainsValue(text))
			{
				text = (TutorialWorld.systemNameToId.ContainsKey(text) ? TutorialWorld.systemNameToId[text] : "tutorial_system_1");
			}
			return text;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00035DDC File Offset: 0x00033FDC
		public static void SetBackgroundSystem(string system)
		{
			PlayerPrefs.SetString(PlayerPrefKeys.BackgroundSystem.ToString(), system);
			PlayerPrefs.Save();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00035E04 File Offset: 0x00034004
		public static bool GetHealthBarsOn()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.HealthBarsOn.ToString(), 1) == 1;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00035E2C File Offset: 0x0003402C
		public static void SetHealthBarsOn(bool on)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.HealthBarsOn.ToString(), on ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00035E5C File Offset: 0x0003405C
		public static Vector2Int GetWindowedResolution()
		{
			return new Vector2Int(PlayerPrefs.GetInt(PlayerPrefKeys.WindowedWidth.ToString(), Screen.width), PlayerPrefs.GetInt(PlayerPrefKeys.WindowedHeight.ToString(), Screen.height));
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00035EA4 File Offset: 0x000340A4
		public static void SetWindowedResolution(Vector2Int resolution)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.WindowedWidth.ToString(), resolution.x);
			PlayerPrefs.SetInt(PlayerPrefKeys.WindowedHeight.ToString(), resolution.y);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00035EEC File Offset: 0x000340EC
		public static float GetZoom()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.Zoom.ToString(), 18f);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00035F14 File Offset: 0x00034114
		public static void SetZoom(float zoom)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.Zoom.ToString(), zoom);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00035F38 File Offset: 0x00034138
		public static bool GetManualControl()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.ManualControl.ToString(), 0) == 1;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00035F60 File Offset: 0x00034160
		public static void SetManualControl(bool on)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.ManualControl.ToString(), on ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00035F90 File Offset: 0x00034190
		public static Vector2 GetSalvageWorkshopPosition()
		{
			return new Vector2(PlayerPrefs.GetFloat(PlayerPrefKeys.SalvageWorkshopX.ToString(), (float)ScreenSettings.defaultPixelWidth), PlayerPrefs.GetFloat(PlayerPrefKeys.SalvageWorkshopY.ToString(), (float)ScreenSettings.defaultPixelHeight));
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00035FD8 File Offset: 0x000341D8
		public static void SetSalvageWorkshopPosition(Vector2 pos)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.SalvageWorkshopX.ToString(), pos.x);
			PlayerPrefs.SetFloat(PlayerPrefKeys.SalvageWorkshopY.ToString(), pos.y);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0003601C File Offset: 0x0003421C
		public static float GetSalvageWorkshopScale()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.SalvageWorkshopScale.ToString(), 1f);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00036044 File Offset: 0x00034244
		public static void SetSalvageWorkshopScale(float scale)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.SalvageWorkshopScale.ToString(), scale);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00036068 File Offset: 0x00034268
		public static bool GetShiftToggleCompare()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.ShiftToggleToCompare.ToString(), 0) > 0;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00036090 File Offset: 0x00034290
		public static void SetShiftToggleCompare(bool shiftToggle)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.ShiftToggleToCompare.ToString(), shiftToggle ? 1 : 0);
			PlayerPrefs.Save();
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000360C0 File Offset: 0x000342C0
		public static Vector2 GetGalaxyMapPosition()
		{
			return new Vector2(PlayerPrefs.GetFloat(PlayerPrefKeys.GalaxyMapX.ToString(), 0f), PlayerPrefs.GetFloat(PlayerPrefKeys.GalaxyMapY.ToString(), 0f));
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00036108 File Offset: 0x00034308
		public static void SetGalaxyMapPosition(Vector2 pos)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.GalaxyMapX.ToString(), pos.x);
			PlayerPrefs.SetFloat(PlayerPrefKeys.GalaxyMapY.ToString(), pos.y);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0003614C File Offset: 0x0003434C
		public static Vector2 GetGalaxyMapSize()
		{
			return new Vector2(PlayerPrefs.GetFloat(PlayerPrefKeys.GalaxyMapWidth.ToString(), ScreenSettings.maxMapWidth), PlayerPrefs.GetFloat(PlayerPrefKeys.GalaxyMapHeight.ToString(), ScreenSettings.mapHeightGameScreen));
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00036194 File Offset: 0x00034394
		public static void SetGalaxyMapSize(Vector2 size)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.GalaxyMapWidth.ToString(), size.x);
			PlayerPrefs.SetFloat(PlayerPrefKeys.GalaxyMapHeight.ToString(), size.y);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x000361D8 File Offset: 0x000343D8
		public static float GetSystemZoomLevel()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.SystemZoomLevel.ToString(), -1f);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00036200 File Offset: 0x00034400
		public static void SetSystemZoomLevel(float zoomLevel)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.SystemZoomLevel.ToString(), zoomLevel);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00036224 File Offset: 0x00034424
		public static float GetSectorZoomLevel()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.SectorZoomLevel.ToString(), -1f);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0003624C File Offset: 0x0003444C
		public static void SetSectorZoomLevel(float zoomLevel)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.SectorZoomLevel.ToString(), zoomLevel);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00036270 File Offset: 0x00034470
		public static float GetGalaxyZoomLevel()
		{
			return PlayerPrefs.GetFloat(PlayerPrefKeys.GalaxyZoomLevel.ToString(), -1f);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00036298 File Offset: 0x00034498
		public static void SetGalaxyZoomLevel(float zoomLevel)
		{
			PlayerPrefs.SetFloat(PlayerPrefKeys.GalaxyZoomLevel.ToString(), zoomLevel);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000362BC File Offset: 0x000344BC
		public static bool ShowExperimentalMap()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.ExperimentalMap.ToString(), 0) == 1;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000362E4 File Offset: 0x000344E4
		public static void SetShowExperimentalMap(bool show)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.ExperimentalMap.ToString(), show ? 1 : 0);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00036310 File Offset: 0x00034510
		public static bool SentSEName()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.SentSEName.ToString(), 0) == 1;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00036338 File Offset: 0x00034538
		public static void SetSentSEName(bool sent)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.SentSEName.ToString(), sent ? 1 : 0);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00036364 File Offset: 0x00034564
		public static bool GetLootBoxSkipAnimations()
		{
			return PlayerPrefs.GetInt(PlayerPrefKeys.LootBoxSkipAnimations.ToString(), 0) == 1;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0003638C File Offset: 0x0003458C
		public static void SetLootBoxSkipAnimations(bool skipAnimations)
		{
			PlayerPrefs.SetInt(PlayerPrefKeys.LootBoxSkipAnimations.ToString(), skipAnimations ? 1 : 0);
			PlayerPrefs.Save();
		}
	}
}
