using System;
using Behaviour.AudioSystem;
using Behaviour.Bootstrap;
using Behaviour.Mining;
using Behaviour.Persistables;
using Behaviour.Transparency;
using Behaviour.Travel;
using Behaviour.UI.Main_Menu;
using Behaviour.Util;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour
{
	// Token: 0x020001A7 RID: 423
	public class GameManager : PersistentSingleton<GameManager>
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00068CAF File Offset: 0x00066EAF
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x00068CB7 File Offset: 0x00066EB7
		public Transform equipmentBuilderRoot { get; private set; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00068CC0 File Offset: 0x00066EC0
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00068CC8 File Offset: 0x00066EC8
		public Transform itemBuilderRoot { get; private set; }

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00068CD1 File Offset: 0x00066ED1
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x00068CD9 File Offset: 0x00066ED9
		public Asteroid asteroidPrefab { get; private set; }

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00068CE2 File Offset: 0x00066EE2
		// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x00068CEA File Offset: 0x00066EEA
		public TutorialJumpgate tutorialJumpgatePrefab { get; private set; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00068CF3 File Offset: 0x00066EF3
		// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x00068CFB File Offset: 0x00066EFB
		public EscortDestination escortDestinationPrefab { get; private set; }

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00068D04 File Offset: 0x00066F04
		// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x00068D0C File Offset: 0x00066F0C
		public MoveToArea moveToAreaPrefab { get; private set; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00068D15 File Offset: 0x00066F15
		// (set) Token: 0x06000ECA RID: 3786 RVA: 0x00068D1D File Offset: 0x00066F1D
		public LootContainer lootContainerPrefab { get; private set; }

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00068D26 File Offset: 0x00066F26
		// (set) Token: 0x06000ECC RID: 3788 RVA: 0x00068D2E File Offset: 0x00066F2E
		public Beacon beaconPrefab { get; private set; }

		// Token: 0x06000ECD RID: 3789 RVA: 0x00068D37 File Offset: 0x00066F37
		private void Start()
		{
			GamePlayer.current = null;
			Application.targetFrameRate = 120;
			Application.runInBackground = true;
			QualitySettings.vSyncCount = 0;
			this.SelectCursor();
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00068D58 File Offset: 0x00066F58
		private void Update()
		{
			this.TrySetCursor();
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00068D60 File Offset: 0x00066F60
		private void TrySetCursor()
		{
			if (ScreenSettings.fullscreen)
			{
				return;
			}
			this.SelectCursor();
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x00068D70 File Offset: 0x00066F70
		public void SelectCursor()
		{
			AbstractWindow activeWindow = PersistentSingleton<Bootstrapper>.Instance.activeWindow;
			if (activeWindow == null || !activeWindow.isClickThrough)
			{
				if (this.cursorState != CursorState.On)
				{
					Cursor.SetCursor(this.cursorTexture, this.hotspot, this.cursorMode);
					this.cursorState = CursorState.On;
					return;
				}
			}
			else if (this.cursorState != CursorState.Off)
			{
				Cursor.SetCursor(null, this.hotspot, this.cursorMode);
				this.cursorState = CursorState.Off;
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00068DE1 File Offset: 0x00066FE1
		public void ContinueGame()
		{
			if (GamePlayer.current == null && !SaveGame.LoadLatestSave())
			{
				Debug.Log("Not finding save");
				GamePlayer.StartNewGame();
			}
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00068E00 File Offset: 0x00067000
		public void Reset()
		{
			SaveGame.StoreAutosaveState("autosave-reset");
			this.LoadGame(SaveGame.GetLatestSave());
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x00068E17 File Offset: 0x00067017
		public void LoadLatestSaveFile()
		{
			this.LoadGame(SaveGame.GetLatestSave());
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x00068E24 File Offset: 0x00067024
		public void LoadGame(SaveGameFile saveFile)
		{
			LoadingScreen.Show(null);
			if (GamePlayer.current != null)
			{
				GamePlayer.current.Cleanup();
			}
			this.equipmentBuilderRoot.DestroyChildren();
			this.itemBuilderRoot.DestroyChildren();
			this.currentSaveGameFile = saveFile;
			PersistentSingleton<SceneLoader>.Instance.UnloadScenesForLoadGame(new Action(this.LoadCurrentSaveGameFile));
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x00068E7B File Offset: 0x0006707B
		public void LoadCurrentSaveGameFile()
		{
			this.currentSaveGameFile.LoadSaveGame();
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00068E88 File Offset: 0x00067088
		public void StartNewGame()
		{
			GamePlayer.StartNewGame();
			PersistentSingleton<SceneLoader>.Instance.LoadScenesOnStartGame();
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x00068E99 File Offset: 0x00067099
		// (set) Token: 0x06000ED8 RID: 3800 RVA: 0x00068EA0 File Offset: 0x000670A0
		public static bool isQuitting { get; private set; }

		// Token: 0x06000ED9 RID: 3801 RVA: 0x00068EA8 File Offset: 0x000670A8
		private void OnApplicationQuit()
		{
			GameManager.isQuitting = true;
			if (GamePlayer.current != null)
			{
				Debug.Log("Quiting, save!");
				SaveGame.StoreAutosaveState(null);
			}
			PlayerPrefs.Save();
			GamePlayer.current = null;
			Translation.Clear();
			GameManager.pauseCount = 0;
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00068EDD File Offset: 0x000670DD
		public static bool isPaused
		{
			get
			{
				return GameManager.pauseCount > 0;
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00068EE7 File Offset: 0x000670E7
		public static void Pause()
		{
			PersistentSingleton<SoundManager>.instance.OnPause(true);
			Time.timeScale = 0f;
			GameManager.pauseCount++;
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00068F0A File Offset: 0x0006710A
		public static void Unpause()
		{
			GameManager.pauseCount--;
			if (GameManager.pauseCount <= 0)
			{
				PersistentSingleton<SoundManager>.instance.OnPause(false);
				GameManager.pauseCount = 0;
				Time.timeScale = 1f;
			}
		}

		// Token: 0x04000861 RID: 2145
		[SerializeField]
		public Gradient backgroundColors;

		// Token: 0x04000862 RID: 2146
		private SaveGameFile currentSaveGameFile;

		// Token: 0x04000863 RID: 2147
		private static int pauseCount;

		// Token: 0x04000864 RID: 2148
		public CursorState cursorState;

		// Token: 0x04000865 RID: 2149
		public Texture2D cursorTexture;

		// Token: 0x04000866 RID: 2150
		public Vector2 hotspot = Vector2.zero;

		// Token: 0x04000867 RID: 2151
		public CursorMode cursorMode;
	}
}
