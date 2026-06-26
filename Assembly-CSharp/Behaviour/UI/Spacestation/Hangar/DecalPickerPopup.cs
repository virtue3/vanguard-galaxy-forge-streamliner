using System;
using System.Linq;
using Behaviour.Util;
using Source.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Hangar
{
	// Token: 0x02000233 RID: 563
	public class DecalPickerPopup : MonoBehaviour
	{
		// Token: 0x0600150F RID: 5391 RVA: 0x000880E8 File Offset: 0x000862E8
		private void Awake()
		{
			this.closeButton.onClick.AddListener(delegate()
			{
				base.gameObject.SetActive(false);
			});
			this.clearButton.onClick.AddListener(new UnityAction(this.OnClear));
			this.opacitySlider.minValue = 0.1f;
			this.opacitySlider.maxValue = 1f;
			this.opacitySlider.onValueChanged.AddListener(delegate(float _)
			{
				this.FireCallback();
			});
			this.scaleSlider.minValue = 0.5f;
			this.scaleSlider.maxValue = 3f;
			this.scaleSlider.onValueChanged.AddListener(delegate(float _)
			{
				this.FireCallback();
			});
			this.rotationSlider.minValue = -36f;
			this.rotationSlider.maxValue = 36f;
			this.rotationSlider.wholeNumbers = true;
			this.rotationSlider.onValueChanged.AddListener(delegate(float _)
			{
				this.FireCallback();
			});
			if (this.hueSlider)
			{
				this.hueSlider.minValue = 0f;
				this.hueSlider.maxValue = 1f;
				this.hueSlider.onValueChanged.AddListener(delegate(float _)
				{
					this.OnHSVChanged();
				});
			}
			if (this.saturationSlider)
			{
				this.saturationSlider.minValue = 0f;
				this.saturationSlider.maxValue = 1f;
				this.saturationSlider.onValueChanged.AddListener(delegate(float _)
				{
					this.OnHSVChanged();
				});
			}
			if (this.valueSlider)
			{
				this.valueSlider.minValue = 0f;
				this.valueSlider.maxValue = 1f;
				this.valueSlider.onValueChanged.AddListener(delegate(float _)
				{
					this.OnHSVChanged();
				});
			}
			if (this.hexInput)
			{
				this.hexInput.characterLimit = 6;
				this.hexInput.onValueChanged.AddListener(new UnityAction<string>(this.SanitizeHexInput));
				this.hexInput.onEndEdit.AddListener(new UnityAction<string>(this.OnHexInputChanged));
			}
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x0008831C File Offset: 0x0008651C
		public void Open(int placementIndex, string currentDecalId, float currentOpacity, float currentScale, float currentRotation, Color currentColor, Action<int, string, float, float, float, Color> onPicked)
		{
			this._placementIndex = placementIndex;
			this._currentDecalId = currentDecalId;
			this._currentColor = currentColor;
			this._onPicked = onPicked;
			this._selectedButton = null;
			this.opacitySlider.SetValueWithoutNotify(currentOpacity);
			this.scaleSlider.SetValueWithoutNotify(currentScale);
			this.rotationSlider.SetValueWithoutNotify(Mathf.Round(currentRotation / 5f));
			this.ApplyColorToUI(this._currentColor);
			base.gameObject.SetActive(true);
			this.Populate(currentDecalId);
			this.RefreshColorSection();
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x000883A4 File Offset: 0x000865A4
		private void ApplyColorToUI(Color c)
		{
			float valueWithoutNotify;
			float num;
			float valueWithoutNotify2;
			Color.RGBToHSV(c, out valueWithoutNotify, out num, out valueWithoutNotify2);
			if (num < 0.01f)
			{
				num = 1f;
			}
			if (this.hueSlider)
			{
				this.hueSlider.SetValueWithoutNotify(valueWithoutNotify);
			}
			if (this.saturationSlider)
			{
				this.saturationSlider.SetValueWithoutNotify(num);
			}
			if (this.valueSlider)
			{
				this.valueSlider.SetValueWithoutNotify(valueWithoutNotify2);
			}
			if (this.colorSwatch)
			{
				this.colorSwatch.color = c;
			}
			if (this.hexInput)
			{
				this.hexInput.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGB(c));
			}
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00088450 File Offset: 0x00086650
		private void OnHSVChanged()
		{
			float h = this.hueSlider ? this.hueSlider.value : 0f;
			float s = this.saturationSlider ? this.saturationSlider.value : 1f;
			float v = this.valueSlider ? this.valueSlider.value : 1f;
			this._currentColor = Color.HSVToRGB(h, s, v);
			if (this.colorSwatch)
			{
				this.colorSwatch.color = this._currentColor;
			}
			if (this.hexInput)
			{
				this.hexInput.SetTextWithoutNotify(ColorUtility.ToHtmlStringRGB(this._currentColor));
			}
			this.FireCallback();
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00088514 File Offset: 0x00086714
		private void SanitizeHexInput(string value)
		{
			string text = new string((from c in value.ToUpper()
			where (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F')
			select c).ToArray<char>());
			if (text != value)
			{
				this.hexInput.SetTextWithoutNotify(text);
			}
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x0008856C File Offset: 0x0008676C
		private void OnHexInputChanged(string hex)
		{
			if (hex.Length != 6)
			{
				this.ApplyColorToUI(this._currentColor);
				return;
			}
			Color currentColor;
			if (ColorUtility.TryParseHtmlString("#" + hex, out currentColor))
			{
				this._currentColor = currentColor;
				this.ApplyColorToUI(this._currentColor);
				this.FireCallback();
			}
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000885BC File Offset: 0x000867BC
		private static bool IsDecalUnlocked(DecalDefinition def)
		{
			if (def.isSupporter)
			{
				return SteamCommunication.supporterEdition;
			}
			if (def.isSoundtrackDlc)
			{
				return SteamCommunication.soundtrackDlc;
			}
			if (def.isDefault)
			{
				return true;
			}
			ConquestRankRequirement conquestUnlock = def.conquestUnlock;
			if (!string.IsNullOrEmpty((conquestUnlock != null) ? conquestUnlock.factionId : null))
			{
				return def.conquestUnlock.IsMet();
			}
			GamePlayer current = GamePlayer.current;
			return current != null && current.HasDecal(def.identifier);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x0008862C File Offset: 0x0008682C
		private void Populate(string currentDecalId)
		{
			this.grid.DestroyChildren();
			foreach (DecalDefinition decalDefinition in Decals.GetAll().Where(new Func<DecalDefinition, bool>(DecalPickerPopup.IsDecalUnlocked)))
			{
				string capturedId = decalDefinition.identifier;
				DecalSlotButton btn = UnityEngine.Object.Instantiate<DecalSlotButton>(this.decalButtonPrefab, this.grid);
				btn.Setup(0, capturedId, delegate(int _)
				{
					this.Pick(capturedId, btn);
				});
				btn.SetHighlight(capturedId == currentDecalId);
				if (capturedId == currentDecalId)
				{
					this._selectedButton = btn;
				}
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00088714 File Offset: 0x00086914
		private void Pick(string decalId, DecalSlotButton btn)
		{
			if (this._selectedButton)
			{
				this._selectedButton.SetHighlight(false);
			}
			this._selectedButton = btn;
			btn.SetHighlight(true);
			this._currentDecalId = decalId;
			DecalDefinition decalDefinition = Decals.Get(decalId);
			if (decalDefinition == null || !decalDefinition.supportsColorTint)
			{
				this._currentColor = Color.white;
				this.ApplyColorToUI(this._currentColor);
			}
			this.RefreshColorSection();
			this.FireCallback();
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00088784 File Offset: 0x00086984
		private void RefreshColorSection()
		{
			if (!this.colorSection)
			{
				return;
			}
			DecalDefinition decalDefinition = string.IsNullOrEmpty(this._currentDecalId) ? null : Decals.Get(this._currentDecalId);
			this.colorSection.SetActive(decalDefinition != null && decalDefinition.supportsColorTint);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000887D4 File Offset: 0x000869D4
		private void FireCallback()
		{
			if (!string.IsNullOrEmpty(this._currentDecalId))
			{
				Action<int, string, float, float, float, Color> onPicked = this._onPicked;
				if (onPicked == null)
				{
					return;
				}
				onPicked(this._placementIndex, this._currentDecalId, this.opacitySlider.value, this.scaleSlider.value, this.rotationSlider.value * 5f, this._currentColor);
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00088838 File Offset: 0x00086A38
		private void OnClear()
		{
			Action<int, string, float, float, float, Color> onPicked = this._onPicked;
			if (onPicked != null)
			{
				onPicked(this._placementIndex, null, this.opacitySlider.value, this.scaleSlider.value, this.rotationSlider.value * 5f, this._currentColor);
			}
			base.gameObject.SetActive(false);
		}

		// Token: 0x04000C66 RID: 3174
		[SerializeField]
		private Transform grid;

		// Token: 0x04000C67 RID: 3175
		[SerializeField]
		private DecalSlotButton decalButtonPrefab;

		// Token: 0x04000C68 RID: 3176
		[SerializeField]
		private Button clearButton;

		// Token: 0x04000C69 RID: 3177
		[SerializeField]
		private Button closeButton;

		// Token: 0x04000C6A RID: 3178
		[SerializeField]
		private Slider opacitySlider;

		// Token: 0x04000C6B RID: 3179
		[SerializeField]
		private Slider scaleSlider;

		// Token: 0x04000C6C RID: 3180
		[SerializeField]
		private Slider rotationSlider;

		// Token: 0x04000C6D RID: 3181
		[SerializeField]
		private Slider hueSlider;

		// Token: 0x04000C6E RID: 3182
		[SerializeField]
		private Slider saturationSlider;

		// Token: 0x04000C6F RID: 3183
		[SerializeField]
		private Slider valueSlider;

		// Token: 0x04000C70 RID: 3184
		[SerializeField]
		private TMP_InputField hexInput;

		// Token: 0x04000C71 RID: 3185
		[SerializeField]
		private Image colorSwatch;

		// Token: 0x04000C72 RID: 3186
		[SerializeField]
		private GameObject colorSection;

		// Token: 0x04000C73 RID: 3187
		private int _placementIndex;

		// Token: 0x04000C74 RID: 3188
		private string _currentDecalId;

		// Token: 0x04000C75 RID: 3189
		private Color _currentColor = Color.white;

		// Token: 0x04000C76 RID: 3190
		private DecalSlotButton _selectedButton;

		// Token: 0x04000C77 RID: 3191
		private Action<int, string, float, float, float, Color> _onPicked;
	}
}
