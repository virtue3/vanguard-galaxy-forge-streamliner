using System;
using Source.Util;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x0200032B RID: 811
	public class FactionIcon : MonoBehaviour
	{
		// Token: 0x06001EB3 RID: 7859 RVA: 0x000B6413 File Offset: 0x000B4613
		public void SetFactionIcon(Sprite icon)
		{
			this.factionIcon.sprite = icon;
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000B6421 File Offset: 0x000B4621
		public void SetEmbassy()
		{
			this.factionBorder.gameObject.SetActive(true);
			this.factionBorder.color = ColorHelper.gold;
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000B6444 File Offset: 0x000B4644
		public void SetIsHeadquarters()
		{
			this.factionBorder.gameObject.SetActive(true);
			this.factionBorder.color = ColorHelper.silver;
		}

		// Token: 0x04001271 RID: 4721
		[SerializeField]
		private SpriteRenderer factionIcon;

		// Token: 0x04001272 RID: 4722
		[SerializeField]
		private SpriteRenderer factionBorder;
	}
}
