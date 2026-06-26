using System;
using UnityEngine;

namespace Behaviour.UI.Settings
{
	// Token: 0x02000246 RID: 582
	public abstract class SettingsSubMenu : MonoBehaviour
	{
		// Token: 0x06001599 RID: 5529 RVA: 0x0008A443 File Offset: 0x00088643
		public void SetSettingsMenu(SettingsMenu settingsMenu)
		{
			this.settingsMenu = settingsMenu;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0008A44C File Offset: 0x0008864C
		public void BackToSettings()
		{
			this.settingsMenu.BackToSettings();
		}

		// Token: 0x04000CDB RID: 3291
		protected SettingsMenu settingsMenu;
	}
}
