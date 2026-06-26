using System;
using System.Collections;
using Behaviour.Bootstrap;
using Behaviour.UI.MainMenu;
using Behaviour.UI.Settings;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001E2 RID: 482
	public class MainMenuUI : MonoBehaviour
	{
		// Token: 0x0600124C RID: 4684 RVA: 0x0007883D File Offset: 0x00076A3D
		private void Awake()
		{
			MainMenuUI.instance = this;
			if (!SaveGame.HasSaveGames())
			{
				this.continueGame.interactable = false;
			}
			base.StartCoroutine(this.FadeOut());
			this.versionNumber.text = GameVersion.GetVersion();
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00078878 File Offset: 0x00076A78
		private void Start()
		{
			if (!SteamCommunication.runningOnSteamDeck && !GameplayerPrefs.GetAnsweredFullscreen())
			{
				AlertPopup.ShowQuery("@UIFullscreenQuestion", "@UIBottomscreen", "@UIWindowedMode", new Action(this.SetBottomScreen), new Action(this.SetWindowedMode), "@UIFullscreen", new Action(this.SetFullscreen));
			}
			if (SteamManager.Initialized)
			{
				SteamStatsManager.Init();
			}
			if (SteamCommunication.supporterEdition)
			{
				this.supporterEditionPanel.gameObject.SetActive(true);
				return;
			}
			this.nonSupporterEditionPanel.gameObject.SetActive(true);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00078908 File Offset: 0x00076B08
		public void SetFullscreen()
		{
			GameplayerPrefs.SetTransparencyMode(TransparencyMode.FullScreen.ToString());
			PersistentSingleton<Bootstrapper>.Instance.SetTransparencyMode(TransparencyMode.FullScreen);
			GameplayerPrefs.SetAnsweredFullscreen(true);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0007893C File Offset: 0x00076B3C
		public void SetBottomScreen()
		{
			GameplayerPrefs.SetTransparencyMode(TransparencyMode.Transparent.ToString());
			PersistentSingleton<Bootstrapper>.Instance.SetTransparencyMode(TransparencyMode.Transparent);
			GameplayerPrefs.SetAnsweredFullscreen(true);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00078970 File Offset: 0x00076B70
		public void SetWindowedMode()
		{
			GameplayerPrefs.SetTransparencyMode(TransparencyMode.Windowed.ToString());
			PersistentSingleton<Bootstrapper>.Instance.SetTransparencyMode(TransparencyMode.Windowed);
			GameplayerPrefs.SetAnsweredFullscreen(true);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x000789A2 File Offset: 0x00076BA2
		public void Continue()
		{
			PersistentSingleton<GameManager>.Instance.LoadLatestSaveFile();
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x000789AE File Offset: 0x00076BAE
		public void CancelLoadGame()
		{
			UnityEngine.Object.Destroy(this.loadGameUI.gameObject);
			base.gameObject.SetActive(true);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000789CC File Offset: 0x00076BCC
		public void BackToMainMenu(bool active)
		{
			UnityEngine.Object.Destroy(this.settingsMenu.gameObject);
			base.gameObject.SetActive(active);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000789EA File Offset: 0x00076BEA
		public void StartGame()
		{
			PersistentSingleton<SceneLoader>.Instance.LoadScene("Start - New Game", false);
			PersistentSingleton<SceneLoader>.Instance.UnloadScene("Main Menu");
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00078A0C File Offset: 0x00076C0C
		public void LoadGame()
		{
			this.loadGameUI = UnityEngine.Object.Instantiate<LoadGameUI>(this.loadGameUIPrefab, base.transform.parent);
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00078A38 File Offset: 0x00076C38
		public void Settings()
		{
			this.settingsMenu = UnityEngine.Object.Instantiate<SettingsMenu>(this.settingsMenuPrefab, base.transform.parent);
			this.settingsMenu.SetBackCallback(new Action<bool>(this.BackToMainMenu));
			base.gameObject.SetActive(false);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00078A84 File Offset: 0x00076C84
		public void ExitGame()
		{
			Application.Quit();
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00078A8B File Offset: 0x00076C8B
		private IEnumerator FadeOut()
		{
			float elapsedTime = 0f;
			while (elapsedTime < this.fadeDuration)
			{
				elapsedTime += Time.deltaTime;
				float alpha = 1f - Mathf.Clamp01(elapsedTime / this.fadeDuration);
				this.SetAlpha(alpha);
				yield return null;
			}
			this.fadeImage.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00078A9A File Offset: 0x00076C9A
		private void SetAlpha(float t)
		{
			this.fadeImage.color = this.fadeImage.color.WithAlpha(t);
		}

		// Token: 0x04000A26 RID: 2598
		public static MainMenuUI instance;

		// Token: 0x04000A27 RID: 2599
		[SerializeField]
		private LoadGameUI loadGameUIPrefab;

		// Token: 0x04000A28 RID: 2600
		[SerializeField]
		private SettingsMenu settingsMenuPrefab;

		// Token: 0x04000A29 RID: 2601
		private LoadGameUI loadGameUI;

		// Token: 0x04000A2A RID: 2602
		private SettingsMenu settingsMenu;

		// Token: 0x04000A2B RID: 2603
		[SerializeField]
		private SupporterEditionPanel supporterEditionPanel;

		// Token: 0x04000A2C RID: 2604
		[SerializeField]
		private GameObject nonSupporterEditionPanel;

		// Token: 0x04000A2D RID: 2605
		[SerializeField]
		private Button continueGame;

		// Token: 0x04000A2E RID: 2606
		[SerializeField]
		private Image fadeImage;

		// Token: 0x04000A2F RID: 2607
		private readonly float fadeDuration = 0.4f;

		// Token: 0x04000A30 RID: 2608
		[SerializeField]
		private TMP_Text versionNumber;
	}
}
