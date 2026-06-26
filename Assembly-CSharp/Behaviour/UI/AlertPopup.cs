using System;
using Behaviour.Transparency;
using Source.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D0 RID: 464
	public class AlertPopup : MonoBehaviour
	{
		// Token: 0x06001180 RID: 4480 RVA: 0x00074318 File Offset: 0x00072518
		private void Awake()
		{
			string transparencyMode = GameplayerPrefs.GetTransparencyMode();
			if (transparencyMode == "Transparent" || transparencyMode == "Performant")
			{
				base.gameObject.AddComponent<WindowMinimumY>();
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x00074351 File Offset: 0x00072551
		private void Start()
		{
			GameManager.Pause();
			if (this.inputParent.gameObject.activeSelf)
			{
				this.inputField.Select();
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x00074375 File Offset: 0x00072575
		private void OnDestroy()
		{
			GameManager.Unpause();
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0007437C File Offset: 0x0007257C
		private void SetText(string label)
		{
			this.label.TL(label, Array.Empty<object>());
			float preferredHeight = this.label.preferredHeight;
			this.inputParent.anchoredPosition = new Vector2(this.inputParent.anchoredPosition.x, this.label.rectTransform.anchoredPosition.y - preferredHeight - 10f);
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x000743E3 File Offset: 0x000725E3
		private void SetInputField(bool inputField, string inputDefault)
		{
			this.inputParent.gameObject.SetActive(inputField);
			if (inputField && inputDefault != null)
			{
				this.inputField.text = inputDefault;
			}
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x00074408 File Offset: 0x00072608
		private void SetButtons(string[] buttons, Action<string>[] buttonActions)
		{
			this.buttonsParent.DestroyChildren();
			float num = 0f;
			for (int i = 0; i < buttons.Length; i++)
			{
				Button button = UnityEngine.Object.Instantiate<Button>(this.buttonPrefab, this.buttonsParent);
				TMP_Text componentInChildren = button.GetComponentInChildren<TMP_Text>();
				componentInChildren.TL(buttons[i], Array.Empty<object>());
				button.onClick.AddListener(new UnityAction(this.DestroyPopup));
				if (buttonActions != null && buttonActions.Length > i && buttonActions[i] != null)
				{
					Action<string> btn = buttonActions[i];
					button.onClick.AddListener(delegate()
					{
						btn(this.inputField.text);
					});
				}
				RectTransform rectTransform = button.transform as RectTransform;
				rectTransform.anchorMin = new Vector2(1f, 0f);
				rectTransform.anchorMax = new Vector2(1f, 0f);
				rectTransform.pivot = new Vector2(1f, 0f);
				rectTransform.sizeDelta = new Vector2(componentInChildren.preferredWidth + 32f, rectTransform.sizeDelta.y);
				rectTransform.anchoredPosition = new Vector2(num, 0f);
				num -= rectTransform.sizeDelta.x + 10f;
				if (i == 0)
				{
					this.submitButton = button;
				}
			}
			float num2 = this.buttonsParent.sizeDelta.y + this.label.preferredHeight + 60f;
			if (this.inputParent.gameObject.activeSelf)
			{
				num2 += this.inputParent.sizeDelta.y + 10f;
			}
			this.alertBox.sizeDelta = new Vector2(this.alertBox.sizeDelta.x, num2);
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x000745CD File Offset: 0x000727CD
		public void OnEndEdit()
		{
			if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
			{
				this.submitButton.onClick.Invoke();
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00074601 File Offset: 0x00072801
		private void DestroyPopup()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x00074610 File Offset: 0x00072810
		public static void ShowMessage(string msg, string buttonLabel = null, Action onConfirm = null)
		{
			AlertPopup.Show(msg, new string[]
			{
				buttonLabel ?? "@UIConfirm"
			}, new Action<string>[]
			{
				delegate(string a)
				{
					Action onConfirm2 = onConfirm;
					if (onConfirm2 == null)
					{
						return;
					}
					onConfirm2();
				}
			}, false, null);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0007465C File Offset: 0x0007285C
		public static void ShowQuery(string msg, string yesLabel = null, string noLabel = null, Action onYes = null, Action onNo = null, string thirdLabel = null, Action onThird = null)
		{
			string[] buttons = new string[]
			{
				yesLabel ?? "@UIYes",
				noLabel ?? "@UINo"
			};
			Action<string>[] buttonActions = new Action<string>[]
			{
				delegate(string a)
				{
					Action onYes2 = onYes;
					if (onYes2 == null)
					{
						return;
					}
					onYes2();
				},
				delegate(string a)
				{
					Action onNo2 = onNo;
					if (onNo2 == null)
					{
						return;
					}
					onNo2();
				}
			};
			if (thirdLabel != null)
			{
				buttons = new string[]
				{
					yesLabel ?? "@UIYes",
					noLabel ?? "@UINo",
					thirdLabel
				};
				buttonActions = new Action<string>[]
				{
					delegate(string a)
					{
						Action onYes2 = onYes;
						if (onYes2 == null)
						{
							return;
						}
						onYes2();
					},
					delegate(string a)
					{
						Action onNo2 = onNo;
						if (onNo2 == null)
						{
							return;
						}
						onNo2();
					},
					delegate(string a)
					{
						Action onThird2 = onThird;
						if (onThird2 == null)
						{
							return;
						}
						onThird2();
					}
				};
			}
			AlertPopup.Show(msg, buttons, buttonActions, false, null);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00074734 File Offset: 0x00072934
		public static void ShowInput(string label, Action<string> onSubmit, string submitLabel = null, string defaultValue = null, bool allowCancel = true, string cancelLabel = null, Action onCancel = null)
		{
			string[] buttons;
			Action<string>[] buttonActions;
			if (allowCancel)
			{
				buttons = new string[]
				{
					submitLabel ?? "@UISubmit",
					cancelLabel ?? "@UICancel"
				};
				buttonActions = new Action<string>[]
				{
					onSubmit,
					delegate(string a)
					{
						Action onCancel2 = onCancel;
						if (onCancel2 == null)
						{
							return;
						}
						onCancel2();
					}
				};
			}
			else
			{
				buttons = new string[]
				{
					submitLabel ?? "@UISubmit"
				};
				buttonActions = new Action<string>[]
				{
					onSubmit
				};
			}
			AlertPopup.Show(label, buttons, buttonActions, true, defaultValue);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000747BA File Offset: 0x000729BA
		public static void Show(string label, string[] buttons, Action<string>[] buttonActions, bool inputField, string inputDefault = null)
		{
			AlertPopup alertPopup = UnityEngine.Object.Instantiate<AlertPopup>(AlertPopup.prefab, UITooltip.tooltipParent);
			alertPopup.SetText(label);
			alertPopup.SetInputField(inputField, inputDefault);
			alertPopup.SetButtons(buttons, buttonActions);
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x000747E2 File Offset: 0x000729E2
		public static void SetPrefab(AlertPopup prefab)
		{
			AlertPopup.prefab = prefab;
		}

		// Token: 0x0400099A RID: 2458
		private static AlertPopup prefab;

		// Token: 0x0400099B RID: 2459
		[SerializeField]
		private RectTransform alertBox;

		// Token: 0x0400099C RID: 2460
		[SerializeField]
		private TMP_Text label;

		// Token: 0x0400099D RID: 2461
		[SerializeField]
		private RectTransform inputParent;

		// Token: 0x0400099E RID: 2462
		[SerializeField]
		private TMP_InputField inputField;

		// Token: 0x0400099F RID: 2463
		[SerializeField]
		private RectTransform buttonsParent;

		// Token: 0x040009A0 RID: 2464
		[SerializeField]
		private Button buttonPrefab;

		// Token: 0x040009A1 RID: 2465
		private Button submitButton;
	}
}
