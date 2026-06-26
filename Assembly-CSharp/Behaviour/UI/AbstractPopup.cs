using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F0 RID: 496
	public abstract class AbstractPopup : MonoBehaviour
	{
		// Token: 0x060012C6 RID: 4806 RVA: 0x0007ABEF File Offset: 0x00078DEF
		public virtual void Start()
		{
			if (this.input)
			{
				this.input.Select();
			}
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x0007AC09 File Offset: 0x00078E09
		public virtual void InputEndEdit()
		{
			if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
			{
				this.actionButton.onClick.Invoke();
			}
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x0007AC3D File Offset: 0x00078E3D
		public virtual void Reset()
		{
			base.gameObject.SetActive(false);
			this.actionButton.onClick.RemoveAllListeners();
			this.cancelButton.onClick.RemoveAllListeners();
		}

		// Token: 0x04000A84 RID: 2692
		[SerializeField]
		public Button actionButton;

		// Token: 0x04000A85 RID: 2693
		[SerializeField]
		public Button cancelButton;

		// Token: 0x04000A86 RID: 2694
		[SerializeField]
		protected TextMeshProUGUI title;

		// Token: 0x04000A87 RID: 2695
		[SerializeField]
		protected TMP_Text actionLabel;

		// Token: 0x04000A88 RID: 2696
		[SerializeField]
		public TMP_InputField input;
	}
}
