using System;
using TMPro;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001D4 RID: 468
	public class SkillRequirementText : MonoBehaviour
	{
		// Token: 0x060011A4 RID: 4516 RVA: 0x0007550A File Offset: 0x0007370A
		public void SetText(string text)
		{
			this.textfield.text = text;
		}

		// Token: 0x040009B7 RID: 2487
		[SerializeField]
		private TextMeshProUGUI textfield;
	}
}
