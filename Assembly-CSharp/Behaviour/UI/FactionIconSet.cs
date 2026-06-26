using System;
using System.Collections.Generic;
using Source.Galaxy;
using UnityEngine;

namespace Behaviour.UI
{
	// Token: 0x020001D7 RID: 471
	public class FactionIconSet : MonoBehaviour
	{
		// Token: 0x170002FE RID: 766
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0007578E File Offset: 0x0007398E
		// (set) Token: 0x060011B2 RID: 4530 RVA: 0x00075796 File Offset: 0x00073996
		public Sprite fullSize { get; private set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x0007579F File Offset: 0x0007399F
		// (set) Token: 0x060011B4 RID: 4532 RVA: 0x000757A7 File Offset: 0x000739A7
		public Sprite tinySize { get; private set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x060011B5 RID: 4533 RVA: 0x000757B0 File Offset: 0x000739B0
		public Sprite mapIcon
		{
			get
			{
				return this.tinySize ?? this.fullSize;
			}
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000757C2 File Offset: 0x000739C2
		private void Awake()
		{
			FactionIconSet.icons[base.gameObject.name] = this;
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x000757DC File Offset: 0x000739DC
		public static FactionIconSet Get(Faction f)
		{
			FactionIconSet result;
			FactionIconSet.icons.TryGetValue(f.identifier, out result);
			return result;
		}

		// Token: 0x040009C1 RID: 2497
		private static Dictionary<string, FactionIconSet> icons = new Dictionary<string, FactionIconSet>();
	}
}
