using System;
using Source.Crew;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B2 RID: 690
	public class OfficerRenamePopup : MonoBehaviour
	{
		// Token: 0x06001990 RID: 6544 RVA: 0x0009F591 File Offset: 0x0009D791
		private void Start()
		{
			GameManager.Pause();
			this.firstNameInput.Select();
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0009F5A3 File Offset: 0x0009D7A3
		private void OnDestroy()
		{
			GameManager.Unpause();
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0009F5AC File Offset: 0x0009D7AC
		public void Open(CrewMemberData crew, Action<string, string, string> onConfirm)
		{
			this.onConfirm = onConfirm;
			this.firstNameInput.text = crew.firstName;
			this.callsignInput.text = crew.callsign;
			this.lastNameInput.text = crew.lastName;
			this.confirmButton.onClick.RemoveAllListeners();
			this.confirmButton.onClick.AddListener(new UnityAction(this.Confirm));
			this.cancelButton.onClick.RemoveAllListeners();
			this.cancelButton.onClick.AddListener(new UnityAction(this.Close));
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0009F64C File Offset: 0x0009D84C
		private void Confirm()
		{
			if (string.IsNullOrWhiteSpace(this.firstNameInput.text))
			{
				return;
			}
			Action<string, string, string> action = this.onConfirm;
			if (action != null)
			{
				action(this.firstNameInput.text, this.callsignInput.text, this.lastNameInput.text);
			}
			this.Close();
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0009F6A4 File Offset: 0x0009D8A4
		private void Close()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x04001008 RID: 4104
		[SerializeField]
		private TMP_InputField firstNameInput;

		// Token: 0x04001009 RID: 4105
		[SerializeField]
		private TMP_InputField callsignInput;

		// Token: 0x0400100A RID: 4106
		[SerializeField]
		private TMP_InputField lastNameInput;

		// Token: 0x0400100B RID: 4107
		[SerializeField]
		private Button confirmButton;

		// Token: 0x0400100C RID: 4108
		[SerializeField]
		private Button cancelButton;

		// Token: 0x0400100D RID: 4109
		private Action<string, string, string> onConfirm;
	}
}
