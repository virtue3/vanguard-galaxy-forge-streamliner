using System;
using Behaviour.Bootstrap;
using Behaviour.UI.MainMenu;
using Behaviour.UI.Settings;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002B6 RID: 694
	public class SideMenuOptions : SideTabContent
	{
		// Token: 0x0600199F RID: 6559 RVA: 0x0009F858 File Offset: 0x0009DA58
		private void Awake()
		{
			this.versionNumber.text = GameVersion.GetVersion();
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0009F86A File Offset: 0x0009DA6A
		public void MainMenu()
		{
			Source.Util.SaveGame.StoreAutosaveState("autosave-rtt");
			GamePlayer.current.Cleanup();
			PersistentSingleton<SceneLoader>.Instance.StartMenu();
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0009F88A File Offset: 0x0009DA8A
		public void SaveGame()
		{
			this.CreateSubMenu(this.saveGameUI);
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0009F898 File Offset: 0x0009DA98
		public void LoadGame()
		{
			this.CreateSubMenu(this.loadGameUI);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0009F8A8 File Offset: 0x0009DAA8
		private void CreateSubMenu(SaveGameUI newMenu)
		{
			this.SetMenuActive(false);
			SaveGameUI saveGameUI = UnityEngine.Object.Instantiate<SaveGameUI>(newMenu, base.transform);
			saveGameUI.SetSideMenuOptions(this);
			this.currentInstance = saveGameUI.gameObject;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0009F8DC File Offset: 0x0009DADC
		public void GeneralSettings()
		{
			this.SetMenuActive(false);
			SettingsMenu settingsMenu = UnityEngine.Object.Instantiate<SettingsMenu>(this.settingsMenu, base.transform);
			this.currentInstance = settingsMenu.gameObject;
			settingsMenu.SetBackCallback(new Action<bool>(this.SetMenuActive));
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0009F920 File Offset: 0x0009DB20
		public void SetMenuActive(bool showMenu)
		{
			if (this.currentInstance)
			{
				UnityEngine.Object.Destroy(this.currentInstance);
			}
			this.menu.SetActive(showMenu);
			this.discord.SetActive(showMenu);
			this.fullscreenToggle.SetActive(showMenu);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0009F95E File Offset: 0x0009DB5E
		public void ExitGame()
		{
			Source.Util.SaveGame.StoreAutosaveState(null);
			Application.Quit();
		}

		// Token: 0x04001011 RID: 4113
		[SerializeField]
		private SaveGameUI saveGameUI;

		// Token: 0x04001012 RID: 4114
		[SerializeField]
		private LoadGameUI loadGameUI;

		// Token: 0x04001013 RID: 4115
		[SerializeField]
		private SettingsMenu settingsMenu;

		// Token: 0x04001014 RID: 4116
		[SerializeField]
		private GameObject menu;

		// Token: 0x04001015 RID: 4117
		[SerializeField]
		private GameObject discord;

		// Token: 0x04001016 RID: 4118
		[SerializeField]
		private GameObject fullscreenToggle;

		// Token: 0x04001017 RID: 4119
		[SerializeField]
		private TMP_Text versionNumber;

		// Token: 0x04001018 RID: 4120
		private GameObject currentInstance;
	}
}
