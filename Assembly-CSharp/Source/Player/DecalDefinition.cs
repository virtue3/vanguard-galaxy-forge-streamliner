using System;
using Behaviour;
using UnityEngine;

namespace Source.Player
{
	// Token: 0x02000093 RID: 147
	public class DecalDefinition
	{
		// Token: 0x0600058A RID: 1418 RVA: 0x00031940 File Offset: 0x0002FB40
		internal DecalDefinition(DecalAsset asset)
		{
			this.identifier = asset.name;
			this.displayName = asset.displayName;
			this.isSupporter = asset.isSupporter;
			this.isSoundtrackDlc = asset.isSoundtrackDlc;
			this.isDefault = asset.isDefault;
			this.supportsColorTint = asset.supportsColorTint;
			this.conquestUnlock = asset.conquestUnlock;
			this._sprite = asset.sprite;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000319B3 File Offset: 0x0002FBB3
		public Sprite GetSprite()
		{
			return this._sprite;
		}

		// Token: 0x040002D9 RID: 729
		public readonly string identifier;

		// Token: 0x040002DA RID: 730
		public readonly string displayName;

		// Token: 0x040002DB RID: 731
		public readonly bool isSupporter;

		// Token: 0x040002DC RID: 732
		public readonly bool isSoundtrackDlc;

		// Token: 0x040002DD RID: 733
		public readonly bool isDefault;

		// Token: 0x040002DE RID: 734
		public readonly bool supportsColorTint;

		// Token: 0x040002DF RID: 735
		public readonly ConquestRankRequirement conquestUnlock;

		// Token: 0x040002E0 RID: 736
		private readonly Sprite _sprite;
	}
}
