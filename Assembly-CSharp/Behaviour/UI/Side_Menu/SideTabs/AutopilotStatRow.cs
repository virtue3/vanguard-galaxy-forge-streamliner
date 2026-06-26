using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002BB RID: 699
	public class AutopilotStatRow : MonoBehaviour
	{
		// Token: 0x060019C7 RID: 6599 RVA: 0x000A0393 File Offset: 0x0009E593
		public void SetInfo(string name, string value)
		{
			this.field.text = name;
			this.stat.text = value;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000A03AD File Offset: 0x0009E5AD
		public void SetPosition(Vector2 position)
		{
			(base.transform as RectTransform).anchoredPosition = position;
		}

		// Token: 0x04001032 RID: 4146
		[SerializeField]
		private Image icon;

		// Token: 0x04001033 RID: 4147
		[SerializeField]
		private TMP_Text field;

		// Token: 0x04001034 RID: 4148
		[SerializeField]
		private TMP_Text stat;
	}
}
