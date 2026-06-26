using System;
using System.IO;
using System.Text;
using LightJson;
using Source.Galaxy;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000026 RID: 38
	public class BackgroundStorage
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000D987 File Offset: 0x0000BB87
		private static JsonObject SaveCurrentState()
		{
			JsonObject jsonObject = new JsonObject();
			jsonObject["presets"] = BackgroundPresets.current.ToJson();
			return jsonObject;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000D9A3 File Offset: 0x0000BBA3
		public static void LoadState(JsonObject data)
		{
			BackgroundPresets.current = BackgroundPresets.FromJson(data["presets"]);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000D9BA File Offset: 0x0000BBBA
		public static void StoreState()
		{
			BackgroundStorage.Store(BackgroundStorage.SaveCurrentState(), SaveGameFormat.Pretty);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000D9C8 File Offset: 0x0000BBC8
		private static void Store(JsonObject data, SaveGameFormat format = SaveGameFormat.Pretty)
		{
			using (FileStream fileStream = new FileInfo(BackgroundStorage.PresetsDir.FullName + "/backgrounddata.json").Open(FileMode.Create))
			{
				if (format == SaveGameFormat.Compressed)
				{
					throw new NotImplementedException();
				}
				byte[] bytes = Encoding.UTF8.GetBytes(data.ToString(format == SaveGameFormat.Pretty));
				fileStream.Write(bytes, 0, bytes.Length);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000DA3C File Offset: 0x0000BC3C
		public static TextAsset GetFile()
		{
			return Resources.Load<TextAsset>("Background/backgrounddata");
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000DA48 File Offset: 0x0000BC48
		public static bool LoadLatestFile()
		{
			TextAsset file = BackgroundStorage.GetFile();
			if (file != null)
			{
				BackgroundStorage.LoadState(JsonValue.Parse(file.text));
				return true;
			}
			Debug.Log("loading latest backgroundStorage failed");
			return false;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DA86 File Offset: 0x0000BC86
		public static string GetBackgroundTexturePath(Texture2D texture)
		{
			return BackgroundStorage.texturePath + "/" + texture.name;
		}

		// Token: 0x040000F4 RID: 244
		public const string PresetsPathSave = "Assets/Resources/Background";

		// Token: 0x040000F5 RID: 245
		public const string PresetsPath = "Background";

		// Token: 0x040000F6 RID: 246
		public const string DefaultFileName = "backgrounddata";

		// Token: 0x040000F7 RID: 247
		public const string DefaultPresetFile = "backgrounddata.json";

		// Token: 0x040000F8 RID: 248
		private static DirectoryInfo PresetsDir = new DirectoryInfo("Assets/Resources/Background");

		// Token: 0x040000F9 RID: 249
		private static string texturePath = "Background/Textures";
	}
}
