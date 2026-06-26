using System;
using System.Collections.Generic;
using LightJson;
using UnityEngine;

namespace Source.Galaxy
{
	// Token: 0x02000134 RID: 308
	public class BackgroundPresets : IJsonSource
	{
		// Token: 0x06000B97 RID: 2967 RVA: 0x00054343 File Offset: 0x00052543
		public void AddPreset(SystemBackgroundCompositeData background)
		{
			this.presetsData[background.systemId] = background;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00054357 File Offset: 0x00052557
		public SystemBackgroundCompositeData GetBySystemKey(string key)
		{
			if (!this.presetsData.ContainsKey(key))
			{
				return null;
			}
			return this.presetsData[key];
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00054375 File Offset: 0x00052575
		public SystemBackgroundCompositeData GetRandom()
		{
			return SeededRandom.Global.Choose<SystemBackgroundCompositeData>(this.presetsData.Values);
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0005438C File Offset: 0x0005258C
		public JsonValue ToJson()
		{
			return this.presetsData.ToJsonObject<SystemBackgroundCompositeData>();
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x000543A0 File Offset: 0x000525A0
		public static BackgroundPresets FromJson(JsonValue json)
		{
			BackgroundPresets backgroundPresets = new BackgroundPresets();
			backgroundPresets.presetsData.FromJsonObject(json, new ClassExtensions.ParseJsonValueDict<SystemBackgroundCompositeData>(SystemBackgroundCompositeData.FromJson));
			Debug.Log("Loading " + backgroundPresets.presetsData.Count.ToString() + " systems");
			int num = 0;
			foreach (KeyValuePair<string, SystemBackgroundCompositeData> keyValuePair in backgroundPresets.presetsData)
			{
				num++;
			}
			return backgroundPresets;
		}

		// Token: 0x04000646 RID: 1606
		public static BackgroundPresets current;

		// Token: 0x04000647 RID: 1607
		private Dictionary<string, SystemBackgroundCompositeData> presetsData = new Dictionary<string, SystemBackgroundCompositeData>();
	}
}
