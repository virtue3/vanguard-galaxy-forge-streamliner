using System;
using System.Collections.Generic;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Settings
{
	// Token: 0x02000241 RID: 577
	public class ControlSettings : SettingsSubMenu
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x00089A6B File Offset: 0x00087C6B
		private void Start()
		{
			this.SetManualControlValue();
			this.CreateHotkeyRows();
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x00089A79 File Offset: 0x00087C79
		public void OnManualControlToggle()
		{
			GameplayerPrefs.SetManualControl(this.manualControlToggle.isOn);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00089A8B File Offset: 0x00087C8B
		private void SetManualControlValue()
		{
			this.manualControlToggle.SetIsOnWithoutNotify(GameplayerPrefs.GetManualControl());
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00089AA0 File Offset: 0x00087CA0
		private void CreateHotkeyRows()
		{
			foreach (string text in this.hotkeys)
			{
				TMP_Text tmp_Text = UnityEngine.Object.Instantiate<TMP_Text>(this.textPrefab, this.hotkeyLabes);
				tmp_Text.text = Translation.Translate(text, Array.Empty<object>());
				tmp_Text.rectTransform.anchoredPosition = new Vector2(0f, -this.currentHeight);
				TMP_Text tmp_Text2 = UnityEngine.Object.Instantiate<TMP_Text>(this.textPrefab, this.hotkeyDescriptions);
				tmp_Text2.text = Translation.Translate(text + "Use", Array.Empty<object>());
				tmp_Text2.rectTransform.anchoredPosition = new Vector2(0f, -this.currentHeight);
				this.currentHeight += this.rowHeight;
			}
			this.content.sizeDelta = new Vector2(400f, this.currentHeight);
		}

		// Token: 0x04000CBA RID: 3258
		[SerializeField]
		private TMP_Text textPrefab;

		// Token: 0x04000CBB RID: 3259
		[SerializeField]
		private Toggle manualControlToggle;

		// Token: 0x04000CBC RID: 3260
		[SerializeField]
		private RectTransform hotkeyLabes;

		// Token: 0x04000CBD RID: 3261
		[SerializeField]
		private RectTransform hotkeyDescriptions;

		// Token: 0x04000CBE RID: 3262
		[SerializeField]
		private RectTransform content;

		// Token: 0x04000CBF RID: 3263
		private float rowHeight = 40f;

		// Token: 0x04000CC0 RID: 3264
		private float currentHeight = 20f;

		// Token: 0x04000CC1 RID: 3265
		private List<string> hotkeys = new List<string>
		{
			"@SettingsLeftClick",
			"@SettingsLeftClickHold",
			"@SettingsCtrlLeftClick",
			"@SettingsRightClick",
			"@SettingsRightClickHold",
			"@SettingsShiftRightClick",
			"@SettingsF",
			"@SettingsShiftB",
			"@SettingsTab",
			"@SettingsM",
			"@SettingsI",
			"@SettingsEsc",
			"@SettingsScrollWheel",
			"@SettingsShiftZ",
			"@SettingsH",
			"@SettingsT",
			"@SettingsCtrlT",
			"@SettingsCtrlM"
		};
	}
}
