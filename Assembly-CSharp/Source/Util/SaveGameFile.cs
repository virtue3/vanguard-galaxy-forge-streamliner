using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Behaviour.Bootstrap;
using Behaviour.UI;
using Behaviour.UI.Main_Menu;
using Behaviour.Util;
using LightJson;
using Source.Player;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000040 RID: 64
	public class SaveGameFile
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x00017AC5 File Offset: 0x00015CC5
		public DateTime Timestamp
		{
			get
			{
				return this.File.LastWriteTime;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00017AD2 File Offset: 0x00015CD2
		public SaveGameFile(FileInfo file)
		{
			this.Name = file.Name.Replace(".save", "");
			this.File = file;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00017AFC File Offset: 0x00015CFC
		public void LoadSaveGame()
		{
			try
			{
				SaveGame.LoadState(this.Recall());
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				LoadingScreen.Hide(true);
				AlertPopup.ShowMessage("@ELLoadGameError", null, delegate
				{
					GamePlayer current = GamePlayer.current;
					if (current != null)
					{
						current.Cleanup();
					}
					PersistentSingleton<SceneLoader>.Instance.StartMenu();
				});
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00017B60 File Offset: 0x00015D60
		public JsonObject Recall()
		{
			byte[] array = null;
			using (FileStream fileStream = this.File.OpenRead())
			{
				try
				{
					using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							gzipStream.CopyTo(memoryStream);
							array = memoryStream.ToArray();
						}
					}
				}
				catch (IOException)
				{
				}
			}
			using (FileStream fileStream2 = this.File.OpenRead())
			{
				if (array == null)
				{
					array = new byte[fileStream2.Length];
					fileStream2.Read(array, 0, array.Length);
				}
			}
			return JsonValue.Parse(Encoding.UTF8.GetString(array));
		}

		// Token: 0x04000186 RID: 390
		public readonly FileInfo File;

		// Token: 0x04000187 RID: 391
		public readonly string Name;
	}
}
