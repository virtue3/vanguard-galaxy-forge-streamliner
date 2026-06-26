using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Behaviour.Bootstrap;
using Behaviour.Managers;
using Behaviour.UI;
using Behaviour.Util;
using LightJson;
using Source.Player;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x0200003F RID: 63
	public class SaveGame
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x0001766F File Offset: 0x0001586F
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00017676 File Offset: 0x00015876
		public static string loadedVersion { get; private set; }

		// Token: 0x060002DA RID: 730 RVA: 0x0001767E File Offset: 0x0001587E
		static SaveGame()
		{
			if (!SaveGame.SavesDir.Exists)
			{
				SaveGame.SavesDir.Create();
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x000176B9 File Offset: 0x000158B9
		public static JsonObject SaveCurrentState()
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["Version"] = Application.version;
			jsonObject["Player"] = GamePlayer.current.ToJson();
			return jsonObject;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x000176EC File Offset: 0x000158EC
		public static void LoadState(JsonObject data)
		{
			SaveGame.loadedVersion = data["Version"];
			Debug.Log("Loading savegame, version: " + SaveGame.loadedVersion + ", Game version: " + Application.version);
			if (GameVersion.IsFuture(SaveGame.loadedVersion))
			{
				AlertPopup.ShowMessage("@UILoadGameTooNew", null, delegate
				{
					GamePlayer current = GamePlayer.current;
					if (current != null)
					{
						current.Cleanup();
					}
					PersistentSingleton<SceneLoader>.Instance.StartMenu();
				});
				return;
			}
			GamePlayer.current = GamePlayer.FromJson(data["Player"]);
			PersistentSingleton<SceneLoader>.Instance.LoadScenesOnStartGame();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00017782 File Offset: 0x00015982
		public static void StoreAutosaveState(string saveFile = null)
		{
			SaveGame.Store(SaveGame.SaveCurrentState(), saveFile ?? SaveGame.GetAutosaveSlot(), SaveGameFormat.Compressed, 0);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001779A File Offset: 0x0001599A
		public static void DoSave(string saveFile)
		{
			SaveGame.Store(SaveGame.SaveCurrentState(), saveFile, SaveGameFormat.Compressed, 0);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000177AC File Offset: 0x000159AC
		private static string GetAutosaveSlot()
		{
			List<SaveGameFile> list = new List<SaveGameFile>();
			for (int i = 0; i < 3; i++)
			{
				string text = "autosave-" + i.ToString();
				SaveGameFile saveGame = SaveGame.GetSaveGame(text);
				if (saveGame == null)
				{
					return text;
				}
				list.Add(saveGame);
			}
			list.Sort((SaveGameFile a, SaveGameFile b) => a.File.LastWriteTime.Ticks.CompareTo(b.File.LastWriteTime.Ticks));
			return list[0].Name;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00017824 File Offset: 0x00015A24
		public static void Store(JsonObject data, string saveName, SaveGameFormat format = SaveGameFormat.Compressed, int attempt = 0)
		{
			SaveGame._saves = null;
			FileInfo fileInfo = new FileInfo(SaveGame.SavesDir.FullName + "/" + saveName + ".save");
			try
			{
				string s = data.ToString(format == SaveGameFormat.Pretty);
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				using (FileStream fileStream = fileInfo.Open(FileMode.Create))
				{
					if (format == SaveGameFormat.Compressed)
					{
						using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
						{
							gzipStream.Write(bytes, 0, bytes.Length);
							goto IL_77;
						}
					}
					fileStream.Write(bytes, 0, bytes.Length);
					IL_77:;
				}
				if (Singleton<EventLogManager>.Instance)
				{
					Singleton<EventLogManager>.Instance.NewEvent("SaveGame", Translation.Translate("@ELSaveGame", new object[]
					{
						saveName
					}));
				}
			}
			catch (Exception exception)
			{
				if (attempt < 5)
				{
					SaveGame.Store(data, saveName, format, attempt + 1);
				}
				else
				{
					if (Singleton<EventLogManager>.Instance && saveName.Contains("autosave"))
					{
						Singleton<EventLogManager>.Instance.NewEvent("SaveGameError", Translation.Translate("@ELSaveGameErrorAutosave", new object[]
						{
							saveName
						}));
					}
					else
					{
						AlertPopup.ShowMessage("@ELSaveGameError", null, null);
					}
					if (fileInfo.Exists)
					{
						fileInfo.Delete();
					}
					Debug.LogException(exception);
				}
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00017984 File Offset: 0x00015B84
		public static List<SaveGameFile> GetSaveGames()
		{
			if (SaveGame._saves == null)
			{
				SaveGame._saves = new List<SaveGameFile>();
				foreach (FileInfo file in SaveGame.SavesDir.GetFiles("*.save"))
				{
					SaveGame._saves.Add(new SaveGameFile(file));
				}
			}
			return SaveGame._saves;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000179DC File Offset: 0x00015BDC
		public static SaveGameFile GetSaveGame(string name)
		{
			foreach (SaveGameFile saveGameFile in SaveGame.GetSaveGames())
			{
				if (saveGameFile.Name == name)
				{
					return saveGameFile;
				}
			}
			return null;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00017A3C File Offset: 0x00015C3C
		public static SaveGameFile GetLatestSave()
		{
			List<SaveGameFile> saveGames = SaveGame.GetSaveGames();
			saveGames.Sort((SaveGameFile a, SaveGameFile b) => b.File.LastWriteTime.Ticks.CompareTo(a.File.LastWriteTime.Ticks));
			if (saveGames.Count > 0)
			{
				return saveGames[0];
			}
			return null;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00017A88 File Offset: 0x00015C88
		public static bool LoadLatestSave()
		{
			SaveGameFile latestSave = SaveGame.GetLatestSave();
			if (latestSave != null)
			{
				latestSave.LoadSaveGame();
				return true;
			}
			Debug.Log("loading latest save failed");
			return false;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00017AB1 File Offset: 0x00015CB1
		public static bool HasSaveGames()
		{
			return SaveGame.GetLatestSave() != null;
		}

		// Token: 0x0400017D RID: 381
		public const string DefaultSaveFile = "autosave";

		// Token: 0x0400017E RID: 382
		public const int AutosaveSlots = 3;

		// Token: 0x0400017F RID: 383
		public const float AutosaveFrequency = 300f;

		// Token: 0x04000180 RID: 384
		public static string SavesPath = Application.persistentDataPath + "/Saves";

		// Token: 0x04000181 RID: 385
		public const SaveGameFormat DefaultSaveFormat = SaveGameFormat.Compressed;

		// Token: 0x04000182 RID: 386
		public const string SaveFileExtension = ".save";

		// Token: 0x04000183 RID: 387
		private static DirectoryInfo SavesDir = new DirectoryInfo(SaveGame.SavesPath);

		// Token: 0x04000184 RID: 388
		private static List<SaveGameFile> _saves;
	}
}
