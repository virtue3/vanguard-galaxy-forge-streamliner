using System;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x02000263 RID: 611
	public class PlayerIcon : MonoBehaviour
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x0008F017 File Offset: 0x0008D217
		// (set) Token: 0x06001684 RID: 5764 RVA: 0x0008F01F File Offset: 0x0008D21F
		public Image icon { get; private set; }

		// Token: 0x06001685 RID: 5765 RVA: 0x0008F028 File Offset: 0x0008D228
		private void Awake()
		{
			this.icon = base.GetComponent<Image>();
		}
	}
}
