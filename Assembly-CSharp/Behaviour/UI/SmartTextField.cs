using System;
using Behaviour.Gameplay;
using Behaviour.Spacestation;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Behaviour.UI
{
	// Token: 0x020001F7 RID: 503
	public class SmartTextField : MonoBehaviour
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x0007BB44 File Offset: 0x00079D44
		private void Awake()
		{
			this.self = base.GetComponent<TMP_InputField>();
			this.self.onSubmit.AddListener(new UnityAction<string>(this.OnSubmit));
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0007BB6E File Offset: 0x00079D6E
		private void Update()
		{
			if (this.self.isFocused)
			{
				GameControls.Delay();
				SpacestationControls.Delay();
				SmartTextField.current = this;
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0007BB8D File Offset: 0x00079D8D
		public void Tab()
		{
			if (this.nextTextField)
			{
				this.nextTextField.Select();
				return;
			}
			if (this.self.isFocused)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0007BBC0 File Offset: 0x00079DC0
		public void OnSubmit(string input)
		{
			this.EndFocus();
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0007BBC8 File Offset: 0x00079DC8
		public void EndFocus()
		{
			((RectTransform)base.transform.root.GetChild(0)).anchoredPosition = Vector2.zero;
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0007BBEC File Offset: 0x00079DEC
		public void FocusTextfield()
		{
			Rect rect = ((RectTransform)base.transform).rect;
			if (SteamManager.Initialized && SteamUtils.ShowFloatingGamepadTextInput(EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, Mathf.RoundToInt(rect.xMin), Mathf.RoundToInt(rect.yMin), Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height)))
			{
				RectTransform rectTransform = (RectTransform)base.transform.root.GetChild(0);
				AlertPopup alertPopup;
				if (!rectTransform.TryGetComponent<AlertPopup>(out alertPopup))
				{
					rectTransform.anchoredPosition = new Vector2(0f, (float)(Screen.height / 2) / base.transform.root.GetComponent<Canvas>().scaleFactor);
				}
			}
		}

		// Token: 0x04000AA2 RID: 2722
		public static SmartTextField current;

		// Token: 0x04000AA3 RID: 2723
		public static int controlDelay;

		// Token: 0x04000AA4 RID: 2724
		private TMP_InputField self;

		// Token: 0x04000AA5 RID: 2725
		[SerializeField]
		private TMP_InputField nextTextField;
	}
}
