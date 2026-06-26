using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Behaviour.UI.Main_Menu;
using Behaviour.Util;
using Source.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Behaviour.Bootstrap
{
	// Token: 0x020002EF RID: 751
	public class SceneLoader : PersistentSingleton<SceneLoader>
	{
		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001B5C RID: 7004 RVA: 0x000A6E10 File Offset: 0x000A5010
		// (set) Token: 0x06001B5D RID: 7005 RVA: 0x000A6E18 File Offset: 0x000A5018
		public string CurrentScene { get; private set; }

		// Token: 0x06001B5E RID: 7006 RVA: 0x000A6E21 File Offset: 0x000A5021
		public void Init(string currentScene)
		{
			this.CurrentScene = currentScene;
			if (this.CurrentScene == "SpacestationInterior")
			{
				SceneLoader.LoadSceneIfNotLoaded("Spacestation");
			}
			SceneLoader.LoadSceneIfNotLoaded(currentScene);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000A6E50 File Offset: 0x000A5050
		public void SplashScreen()
		{
			List<string> excludedScenes = new List<string>
			{
				"Bootstrapper",
				"Camera"
			};
			base.StartCoroutine(this.UnloadAllScenesExcept(excludedScenes, delegate
			{
				SceneLoader.LoadSceneIfNotLoaded("Camera");
				SceneLoader.LoadSceneIfNotLoaded("SplashScreen");
				GamePlayer.current = null;
			}));
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x000A6EA8 File Offset: 0x000A50A8
		public void StartMenu()
		{
			LoadingScreen.Hide(true);
			List<string> excludedScenes = new List<string>
			{
				"Bootstrapper",
				"Camera",
				"Main Menu",
				"Backdrop"
			};
			base.StartCoroutine(this.UnloadAllScenesExcept(excludedScenes, delegate
			{
				SceneLoader.LoadSceneIfNotLoaded("Camera");
				SceneLoader.LoadSceneIfNotLoaded("Main Menu");
				SceneLoader.LoadSceneIfNotLoaded("Backdrop");
				GamePlayer.current = null;
			}));
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x000A6F1C File Offset: 0x000A511C
		public void UnloadScenesForLoadGame(Action onComplete)
		{
			List<string> excludedScenes = new List<string>
			{
				"Bootstrapper",
				"Backdrop",
				"Camera"
			};
			base.StartCoroutine(this.UnloadAllScenesExcept(excludedScenes, onComplete));
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x000A6F5F File Offset: 0x000A515F
		private IEnumerator UnloadAllScenesExcept(List<string> excludedScenes, Action onComplete)
		{
			int num;
			for (int i = SceneManager.sceneCount - 1; i >= 0; i = num - 1)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (!excludedScenes.Contains(sceneAt.name) && sceneAt.isLoaded)
				{
					yield return SceneManager.UnloadSceneAsync(sceneAt);
				}
				num = i;
			}
			this.CurrentScene = null;
			if (onComplete != null)
			{
				onComplete();
			}
			yield break;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x000A6F7C File Offset: 0x000A517C
		public static bool IsSceneLoaded(string sceneName)
		{
			return SceneManager.GetSceneByName(sceneName).isLoaded;
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x000A6F9C File Offset: 0x000A519C
		public void ToggleSpaceStationInterior(bool enable, bool addSpaceStation = true)
		{
			Debug.Log("Toggle Space Station: " + SceneManager.GetActiveScene().name);
			if (!enable)
			{
				this.UnloadScene("SpacestationInterior");
				this.CurrentScene = "Spacestation";
				this.LoadNeededScenes();
				if (addSpaceStation)
				{
					SceneLoader.LoadSceneIfNotLoaded("Spacestation");
					return;
				}
			}
			else
			{
				SceneLoader.LoadSceneIfNotLoaded("SpacestationInterior");
				if (addSpaceStation)
				{
					SceneLoader.LoadSceneIfNotLoaded("Spacestation");
				}
				this.CurrentScene = "SpacestationInterior";
			}
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x000A7018 File Offset: 0x000A5218
		public void LoadScenesOnStartGame()
		{
			this.UnloadCurrentScene();
			if (SceneLoader.IsSceneLoaded("Main Menu"))
			{
				this.UnloadScene("Main Menu");
			}
			this.LoadNeededScenes();
			if (GamePlayer.current.currentPointOfInterest != null)
			{
				this.Init(GamePlayer.current.currentPointOfInterest.sceneName);
				return;
			}
			if (GamePlayer.current.waypoints.Count == 0 || !GamePlayer.current.currentSpaceShip.travelling)
			{
				this.Init("Space");
			}
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x000A7099 File Offset: 0x000A5299
		public void LoadNeededScenes()
		{
			SceneLoader.LoadSceneIfNotLoaded("Camera");
			SceneLoader.LoadSceneIfNotLoaded("UI - Sidepanel");
			SceneLoader.LoadSceneIfNotLoaded("UI - HUD");
			SceneLoader.LoadSceneIfNotLoaded("Backdrop");
			SceneLoader.LoadSceneIfNotLoaded("Gameplay");
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x000A70D4 File Offset: 0x000A52D4
		public static async Task LoadSceneIfNotLoaded(string sceneName)
		{
			if (!IsSceneLoaded(sceneName))
			{
				AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				while (op != null && !op.isDone)
					await System.Threading.Tasks.Task.Yield();
			}
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x000A7118 File Offset: 0x000A5318
		public async void LoadScene(string sceneName, bool force = false)
		{
			if (force || !IsSceneLoaded(sceneName))
			{
				AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
				while (op != null && !op.isDone)
					await System.Threading.Tasks.Task.Yield();
			}
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x000A7160 File Offset: 0x000A5360
		public async Task UnloadScene(string sceneName)
		{
			if (IsSceneLoaded(sceneName))
			{
				AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
				while (op != null && !op.isDone)
					await System.Threading.Tasks.Task.Yield();
			}
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x000A71AC File Offset: 0x000A53AC
		public async Task UnloadCurrentScene()
		{
			if (this.CurrentScene != null && IsSceneLoaded(this.CurrentScene))
			{
				AsyncOperation op = SceneManager.UnloadSceneAsync(this.CurrentScene);
				while (op != null && !op.isDone)
					await System.Threading.Tasks.Task.Yield();
			}
		}

		// Token: 0x04001130 RID: 4400
		public const string SPACE = "Space";

		// Token: 0x04001131 RID: 4401
		public const string COMBAT = "Combat";

		// Token: 0x04001132 RID: 4402
		public const string MINING = "Mining";

		// Token: 0x04001133 RID: 4403
		public const string TRAVEL = "Travel";

		// Token: 0x04001134 RID: 4404
		public const string SPACESTATION = "Spacestation";

		// Token: 0x04001135 RID: 4405
		public const string SPACESTATION_INTERIOR = "SpacestationInterior";

		// Token: 0x04001136 RID: 4406
		public const string JUMPGATE = "JumpGate";

		// Token: 0x04001137 RID: 4407
		public const string UI_HUD = "UI - HUD";

		// Token: 0x04001138 RID: 4408
		public const string UI_SIDEPANEL = "UI - Sidepanel";

		// Token: 0x04001139 RID: 4409
		public const string GAMEPLAY = "Gameplay";

		// Token: 0x0400113A RID: 4410
		public const string BACKDROP = "Backdrop";

		// Token: 0x0400113B RID: 4411
		public const string BOOTSTRAPPER = "Bootstrapper";

		// Token: 0x0400113C RID: 4412
		public const string CAMERA = "Camera";

		// Token: 0x0400113D RID: 4413
		public const string SPLASHSCREEN = "SplashScreen";

		// Token: 0x0400113E RID: 4414
		public const string MAIN_MENU = "Main Menu";

		// Token: 0x0400113F RID: 4415
		public const string NEW_GAME = "Start - New Game";
	}
}
