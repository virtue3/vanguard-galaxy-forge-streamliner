using System;
using UnityEngine;

namespace Behaviour.UI.Settings
{
	// Token: 0x02000245 RID: 581
	public class SettingsMenu : MonoBehaviour
	{
		// Token: 0x06001590 RID: 5520 RVA: 0x0008A31C File Offset: 0x0008851C
		private void Start()
		{
			this.GeneralSettings();
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x0008A324 File Offset: 0x00088524
		public void GeneralSettings()
		{
			this.settingsContainer.transform.DestroyChildren();
			this.activeMenu = UnityEngine.Object.Instantiate<GeneralSettingsUI>(this.generalSettingsUI, this.settingsContainer.transform);
			this.activeMenu.SetSettingsMenu(this);
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x0008A35E File Offset: 0x0008855E
		public void AudioSettings()
		{
			this.settingsContainer.transform.DestroyChildren();
			this.activeMenu = UnityEngine.Object.Instantiate<AudioSettingsUI>(this.audioSettingsUI, this.settingsContainer.transform);
			this.activeMenu.SetSettingsMenu(this);
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x0008A398 File Offset: 0x00088598
		public void ControlSettings()
		{
			this.settingsContainer.transform.DestroyChildren();
			this.activeMenu = UnityEngine.Object.Instantiate<ControlSettings>(this.controlSettings, this.settingsContainer.transform);
			this.activeMenu.SetSettingsMenu(this);
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x0008A3D2 File Offset: 0x000885D2
		public void GraphicsSettings()
		{
			this.settingsContainer.transform.DestroyChildren();
			this.activeMenu = UnityEngine.Object.Instantiate<GraphicsSettingsUI>(this.graphicsSettings, this.settingsContainer.transform);
			this.activeMenu.SetSettingsMenu(this);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x0008A40C File Offset: 0x0008860C
		public void SetBackCallback(Action<bool> backCallback)
		{
			this.backCallback = backCallback;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0008A415 File Offset: 0x00088615
		public void BackToSettings()
		{
			UnityEngine.Object.Destroy(this.activeMenu.gameObject);
			this.BackToPreviousMenu();
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x0008A42D File Offset: 0x0008862D
		public void BackToPreviousMenu()
		{
			this.backCallback(true);
		}

		// Token: 0x04000CD4 RID: 3284
		[SerializeField]
		private GeneralSettingsUI generalSettingsUI;

		// Token: 0x04000CD5 RID: 3285
		[SerializeField]
		private AudioSettingsUI audioSettingsUI;

		// Token: 0x04000CD6 RID: 3286
		[SerializeField]
		private ControlSettings controlSettings;

		// Token: 0x04000CD7 RID: 3287
		[SerializeField]
		private GraphicsSettingsUI graphicsSettings;

		// Token: 0x04000CD8 RID: 3288
		[SerializeField]
		private GameObject settingsContainer;

		// Token: 0x04000CD9 RID: 3289
		private SettingsSubMenu activeMenu;

		// Token: 0x04000CDA RID: 3290
		private Action<bool> backCallback;
	}
}
