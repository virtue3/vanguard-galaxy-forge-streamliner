using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.Util
{
	// Token: 0x020001B1 RID: 433
	public class MissionIcons : MonoBehaviour
	{
		// Token: 0x06000F3C RID: 3900 RVA: 0x00069F3C File Offset: 0x0006813C
		private void Awake()
		{
			MissionIcons.instance = this;
			foreach (Sprite sprite in this.icons)
			{
				this.dictIcons[sprite.name] = sprite;
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00069FA0 File Offset: 0x000681A0
		public static Sprite Get(string id)
		{
			return MissionIcons.instance.dictIcons[id];
		}

		// Token: 0x0400089C RID: 2204
		private static MissionIcons instance;

		// Token: 0x0400089D RID: 2205
		[SerializeField]
		private List<Sprite> icons;

		// Token: 0x0400089E RID: 2206
		private Dictionary<string, Sprite> dictIcons = new Dictionary<string, Sprite>();
	}
}
