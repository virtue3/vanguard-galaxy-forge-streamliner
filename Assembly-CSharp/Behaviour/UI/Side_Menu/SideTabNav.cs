using System;
using UnityEngine;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x020002A0 RID: 672
	public class SideTabNav : MonoBehaviour
	{
		// Token: 0x06001915 RID: 6421 RVA: 0x0009BD8C File Offset: 0x00099F8C
		public TabMenuButton CreateButton(SideTabContent sideTabContent, bool selected = false, bool deactivated = false)
		{
			TabMenuButton tabMenuButton = this.InitializeButton(sideTabContent, deactivated);
			sideTabContent.tabMenuButton = tabMenuButton;
			if (selected)
			{
				tabMenuButton.SetSelectedButton();
			}
			return tabMenuButton;
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0009BDB4 File Offset: 0x00099FB4
		private TabMenuButton InitializeButton(SideTabContent content, bool deactivated)
		{
			TabMenuButton tabMenuButton = UnityEngine.Object.Instantiate<TabMenuButton>(this.tabMenuButton, base.transform);
			tabMenuButton.SetContentTab(content);
			tabMenuButton.SetDeactivated(deactivated);
			if (deactivated)
			{
				tabMenuButton.SetButtonColor();
			}
			return tabMenuButton;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0009BDEC File Offset: 0x00099FEC
		public void ClearButtons()
		{
			foreach (object obj in base.transform)
			{
				UnityEngine.Object.Destroy(((Transform)obj).gameObject);
			}
		}

		// Token: 0x04000F8A RID: 3978
		[SerializeField]
		private TabMenuButton tabMenuButton;
	}
}
