using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Main
{
	// Token: 0x0200025E RID: 606
	public class CustomShipName : MonoBehaviour
	{
		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06001653 RID: 5715 RVA: 0x0008E0D7 File Offset: 0x0008C2D7
		public string shipName
		{
			get
			{
				return this.shipNameInputField.text;
			}
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x0008E0E4 File Offset: 0x0008C2E4
		private void OnEnable()
		{
			base.StartCoroutine(this.SelectText());
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x0008E0F3 File Offset: 0x0008C2F3
		private IEnumerator SelectText()
		{
			yield return null;
			this.shipNameInputField.ActivateInputField();
			yield return null;
			this.shipNameInputField.MoveToEndOfLine(false, false);
			yield break;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0008E102 File Offset: 0x0008C302
		public void UpdateShipName(string name)
		{
			this.shipNameInputField.text = name;
		}

		// Token: 0x04000D8E RID: 3470
		[SerializeField]
		private TMP_InputField shipNameInputField;
	}
}
