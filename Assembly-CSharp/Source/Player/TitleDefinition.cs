using System;
using Source.Util;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x0200009E RID: 158
	public class TitleDefinition
	{
		// Token: 0x0600066D RID: 1645 RVA: 0x00036D4C File Offset: 0x00034F4C
		public TitleDefinition(string identifier, string translationKey, Color color)
		{
			this.identifier = identifier;
			this.translationKey = translationKey;
			this.color = color;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00036D69 File Offset: 0x00034F69
		public string GetDisplayName()
		{
			return Translation.Translate(this.translationKey, Array.Empty<object>());
		}

		// Token: 0x0400037F RID: 895
		public readonly string identifier;

		// Token: 0x04000380 RID: 896
		public readonly string translationKey;

		// Token: 0x04000381 RID: 897
		public readonly Color color;
	}
}
