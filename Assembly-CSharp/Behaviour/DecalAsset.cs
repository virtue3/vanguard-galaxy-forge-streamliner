using System;
using UnityEngine;

namespace Behaviour
{
	// Token: 0x020001A6 RID: 422
	[CreateAssetMenu(fileName = "decal_new", menuName = "Game/Decal")]
	public class DecalAsset : ScriptableObject
	{
		// Token: 0x04000852 RID: 2130
		public string displayName;

		// Token: 0x04000853 RID: 2131
		public Sprite sprite;

		// Token: 0x04000854 RID: 2132
		public bool isSupporter;

		// Token: 0x04000855 RID: 2133
		public bool isSoundtrackDlc;

		// Token: 0x04000856 RID: 2134
		public bool isDefault;

		// Token: 0x04000857 RID: 2135
		public bool supportsColorTint;

		// Token: 0x04000858 RID: 2136
		public ConquestRankRequirement conquestUnlock;
	}
}
