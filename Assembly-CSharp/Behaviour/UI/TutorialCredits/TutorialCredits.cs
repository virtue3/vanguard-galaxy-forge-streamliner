using System;
using Behaviour.Bootstrap;
using Behaviour.Util;
using Source.Player;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.TutorialCredits
{
	// Token: 0x02000298 RID: 664
	public class TutorialCredits : Singleton<TutorialCredits>
	{
		// Token: 0x06001896 RID: 6294 RVA: 0x0009A6E0 File Offset: 0x000988E0
		public void TriggerTutorialCredits()
		{
			this.ActivateCredits(true);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x0009A6E9 File Offset: 0x000988E9
		public void MainMenu()
		{
			if (GamePlayer.current != null)
			{
				GamePlayer.current.Cleanup();
			}
			PersistentSingleton<SceneLoader>.Instance.StartMenu();
			this.ActivateCredits(false);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x0009A70D File Offset: 0x0009890D
		public void GoBack()
		{
			GamePlayer.current.Cleanup();
			PersistentSingleton<GameManager>.Instance.LoadLatestSaveFile();
			this.ActivateCredits(false);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0009A72A File Offset: 0x0009892A
		public void GoSteamWishlist()
		{
			Application.OpenURL("https://store.steampowered.com/app/3471800/Vanguard_Galaxy/#game_area_purchase");
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0009A736 File Offset: 0x00098936
		private void ActivateCredits(bool activate)
		{
			this.credits.SetActive(activate);
		}

		// Token: 0x04000F4A RID: 3914
		[SerializeField]
		private GameObject credits;

		// Token: 0x04000F4B RID: 3915
		[SerializeField]
		private TextMeshProUGUI thankYou;
	}
}
