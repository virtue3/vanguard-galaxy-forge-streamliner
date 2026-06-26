using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Behaviour.Bootstrap;
using Behaviour.Transparency;
using Behaviour.UI.DebugScreen;
using Behaviour.UI.HUD;
using Behaviour.Util;
using Source.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Settings
{
	// Token: 0x02000243 RID: 579
	public class GeneralSettingsUI : SettingsSubMenu
	{
		// Token: 0x06001577 RID: 5495 RVA: 0x00089D66 File Offset: 0x00087F66
		private void Start()
		{
			this.UpdateDisplayDropdown();
			this.SetScaleValue();
			this.SetScreenPositionValue();
			this.SetAlwaysOnTopValue();
			this.SetTransparencyOptions();
			this.SetTravelHintsValue();
			this.SetShowDamageNumbersValue();
			this.SetExperimentalMapValue();
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00089D98 File Offset: 0x00087F98
		public void OnChangeScale(float scaleButton = 0f)
		{
			this.scale = ((scaleButton == 0f) ? float.Parse(this.scaleInput.text) : scaleButton);
			if (this.scale > ScreenSettings.maxScale)
			{
				this.scale = ScreenSettings.maxScale;
			}
			if (this.scale < ScreenSettings.minScale)
			{
				this.scale = ScreenSettings.minScale;
			}
			this.scaleInput.text = this.scale.ToString();
			GameplayerPrefs.SetScale(this.scale);
			GameplayerPrefs.SetAutoScale(this.scale);
			ScreenSettings.SetScreenPercentage();
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00089E27 File Offset: 0x00088027
		private void SetScaleValue()
		{
			this.scale = GameplayerPrefs.GetAutoScale();
			this.scaleInput.text = this.scale.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00089E50 File Offset: 0x00088050
		public void OnChangeScreenPosition(float positionButton = -1f)
		{
			this.screenPosition = ((positionButton >= 0f) ? positionButton : float.Parse(this.screenPositionInput.text));
			if (this.screenPosition > ScreenSettings.maxScreenPosition)
			{
				this.screenPosition = ScreenSettings.maxScreenPosition;
			}
			if (this.screenPosition < ScreenSettings.minScreenPosition)
			{
				this.screenPosition = ScreenSettings.minScreenPosition;
			}
			this.screenPositionInput.text = this.screenPosition.ToString();
			GameplayerPrefs.SetScreenPosition(this.screenPosition);
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00089ECF File Offset: 0x000880CF
		public void SetScreenPositionValue()
		{
			this.screenPosition = GameplayerPrefs.GetScreenPosition();
			this.screenPositionInput.text = this.screenPosition.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00089EF7 File Offset: 0x000880F7
		public void OnChangeAlwaysOnTop()
		{
			GameplayerPrefs.SetAlwaysOnTop(this.alwaysOnTopToggle.isOn);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x00089F09 File Offset: 0x00088109
		private void SetAlwaysOnTopValue()
		{
			this.alwaysOnTopToggle.SetIsOnWithoutNotify(GameplayerPrefs.GetAlwaysOnTop());
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x00089F1B File Offset: 0x0008811B
		public void OnChangeTravelHints()
		{
			GameplayerPrefs.SetTravelHints(this.travelHintsToggle.isOn);
			if (!this.travelHintsToggle.isOn && HudManager.Instance)
			{
				HudManager.Instance.SetEchoRemarksStatus(false);
			}
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x00089F51 File Offset: 0x00088151
		private void SetTravelHintsValue()
		{
			this.travelHintsToggle.SetIsOnWithoutNotify(GameplayerPrefs.GetTravelHints());
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00089F63 File Offset: 0x00088163
		public void OnChangeDamageNumbers()
		{
			GameplayerPrefs.SetShowDamageNumbers(this.showDamageNumbersToggle.isOn);
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00089F75 File Offset: 0x00088175
		private void SetShowDamageNumbersValue()
		{
			this.showDamageNumbersToggle.SetIsOnWithoutNotify(GameplayerPrefs.GetShowDamageNumbers());
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00089F87 File Offset: 0x00088187
		public void OnChangeExperimentalMap()
		{
			GameplayerPrefs.SetShowExperimentalMap(this.experimentalMapToggle.isOn);
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00089F99 File Offset: 0x00088199
		private void SetExperimentalMapValue()
		{
			this.experimentalMapToggle.SetIsOnWithoutNotify(GameplayerPrefs.ShowExperimentalMap());
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00089FAC File Offset: 0x000881AC
		public void OnChangeTransparencyMode()
		{
			string text = this.transparencyModes[this.transparencyDropdown.value];
			GameplayerPrefs.SetTransparencyMode(text);
			PersistentSingleton<Bootstrapper>.Instance.SetTransparencyMode(Enum.Parse<TransparencyMode>(text));
			this.displayDropdown.enabled = (text != TransparencyMode.Windowed.ToString());
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0008A008 File Offset: 0x00088208
		private void SetTransparencyOptions()
		{
			this.transparencyDropdown.ClearOptions();
			string transparencyMode = GameplayerPrefs.GetTransparencyMode();
			if (SteamCommunication.runningOnSteamDeck)
			{
				this.transparencyModes.Add(TransparencyMode.FullScreen.ToString());
			}
			else
			{
				this.transparencyModes = new List<string>();
				foreach (object obj in Enum.GetValues(typeof(TransparencyMode)))
				{
					TransparencyMode transparencyMode2 = (TransparencyMode)obj;
					this.transparencyModes.Add(transparencyMode2.ToString());
				}
			}
			int num = 0;
			int num2 = 0;
			List<string> list = new List<string>();
			foreach (string text in this.transparencyModes)
			{
				num2 = ((transparencyMode == text) ? num : num2);
				string item = text;
				list.Add(item);
				num++;
			}
			this.transparencyDropdown.AddOptions(list);
			this.transparencyDropdown.SetValueWithoutNotify(num2);
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0008A144 File Offset: 0x00088344
		private void UpdateDisplayDropdown()
		{
			this.displayDropdown.ClearOptions();
			Screen.GetDisplayLayout(this.displays);
			this.currentDisplay = Screen.mainWindowDisplayInfo;
			this.currentDisplayIndex = 0;
			int num = 0;
			List<string> list = new List<string>();
			foreach (DisplayInfo displayInfo in this.displays)
			{
				list.Add(displayInfo.name);
				if (this.currentDisplay.Equals(displayInfo))
				{
					this.currentDisplayIndex = num;
				}
				num++;
			}
			this.displayDropdown.AddOptions(list);
			this.displayDropdown.SetValueWithoutNotify(this.currentDisplayIndex);
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0008A204 File Offset: 0x00088404
		private void Update()
		{
			if (this.movingScreen)
			{
				return;
			}
			this.UpdateDisplayDropdown();
			this.SetTransparencyOptions();
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0008A21C File Offset: 0x0008841C
		public void OnChangeDisplay()
		{
			DisplayInfo displayInfo = this.displays[this.displayDropdown.value];
			Singleton<ConsoleScreen>.Instance.AddTextToConsole("Selected display:" + displayInfo.name);
			base.StartCoroutine(this.MoveToDisplay());
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0008A267 File Offset: 0x00088467
		private IEnumerator MoveToDisplay()
		{
			this.movingScreen = true;
			DisplayInfo display = this.displays[this.displayDropdown.value];
			ScreenSettings.allowScaleUpdate = false;
			Debug.Log("Moving window to " + display.name);
			Singleton<ConsoleScreen>.Instance.AddTextToConsole("Also changing resolution to: " + display.width.ToString() + " - " + display.height.ToString());
			Screen.SetResolution(400, 300, FullScreenMode.Windowed);
			yield return new WaitForEndOfFrame();
			Vector2Int vector2Int = new Vector2Int(0, 0);
			ConsoleScreen instance = Singleton<ConsoleScreen>.Instance;
			string str = "Setting target coordinates: ";
			Vector2Int vector2Int2 = vector2Int;
			instance.AddTextToConsole(str + vector2Int2.ToString());
			Task moveOperation = Screen.MoveMainWindowTo(display, vector2Int).AsTask();
			yield return new WaitUntil(() => moveOperation.IsCompleted);
			Singleton<ConsoleScreen>.Instance.AddTextToConsole("Done moving");
			yield return new WaitForEndOfFrame();
			Screen.SetResolution(display.width, display.height, FullScreenMode.FullScreenWindow);
			yield return new WaitForEndOfFrame();
			Singleton<ConsoleScreen>.Instance.AddTextToConsole("Also changed back to fullscreen");
			this.OnChangeScale(0f);
			ScreenSettings.allowScaleUpdate = true;
			this.movingScreen = false;
			yield break;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0008A276 File Offset: 0x00088476
		public void IncreaseScale()
		{
			this.scale += 1f;
			this.OnChangeScale(this.scale);
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x0008A296 File Offset: 0x00088496
		public void DecreaseScale()
		{
			this.scale -= 1f;
			this.OnChangeScale(this.scale);
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0008A2B6 File Offset: 0x000884B6
		public void IncreaseScreenPosition()
		{
			this.screenPosition += 1f;
			this.OnChangeScreenPosition(this.screenPosition);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0008A2D6 File Offset: 0x000884D6
		public void DecreaseScreenPosition()
		{
			this.screenPosition -= 1f;
			this.OnChangeScreenPosition(this.screenPosition);
		}

		// Token: 0x04000CC4 RID: 3268
		[SerializeField]
		private TMP_Dropdown displayDropdown;

		// Token: 0x04000CC5 RID: 3269
		[SerializeField]
		private TMP_InputField scaleInput;

		// Token: 0x04000CC6 RID: 3270
		[SerializeField]
		private TMP_Text scaleText;

		// Token: 0x04000CC7 RID: 3271
		[SerializeField]
		private TMP_Dropdown transparencyDropdown;

		// Token: 0x04000CC8 RID: 3272
		[SerializeField]
		private Toggle alwaysOnTopToggle;

		// Token: 0x04000CC9 RID: 3273
		[SerializeField]
		private TMP_InputField screenPositionInput;

		// Token: 0x04000CCA RID: 3274
		[SerializeField]
		private Toggle travelHintsToggle;

		// Token: 0x04000CCB RID: 3275
		[SerializeField]
		private Toggle showDamageNumbersToggle;

		// Token: 0x04000CCC RID: 3276
		[SerializeField]
		private Toggle experimentalMapToggle;

		// Token: 0x04000CCD RID: 3277
		private List<DisplayInfo> displays = new List<DisplayInfo>();

		// Token: 0x04000CCE RID: 3278
		private List<string> transparencyModes = new List<string>();

		// Token: 0x04000CCF RID: 3279
		private DisplayInfo currentDisplay;

		// Token: 0x04000CD0 RID: 3280
		private int currentDisplayIndex;

		// Token: 0x04000CD1 RID: 3281
		private bool movingScreen;

		// Token: 0x04000CD2 RID: 3282
		private float scale;

		// Token: 0x04000CD3 RID: 3283
		private float screenPosition;
	}
}
