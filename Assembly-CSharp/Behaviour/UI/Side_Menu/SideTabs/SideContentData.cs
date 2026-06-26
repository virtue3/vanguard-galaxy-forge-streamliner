using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002C0 RID: 704
	public class SideContentData : MonoBehaviour
	{
		// Token: 0x060019E7 RID: 6631 RVA: 0x000A167B File Offset: 0x0009F87B
		public List<SideTabContent> GetData()
		{
			return this.content;
		}

		// Token: 0x0400104C RID: 4172
		[SerializeField]
		private List<SideTabContent> content;
	}
}
