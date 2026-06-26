using System;
using UnityEngine;

namespace Behaviour.Crew
{
	// Token: 0x0200039F RID: 927
	public class BonusBadge : MonoBehaviour
	{
		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06002328 RID: 9000 RVA: 0x000C9852 File Offset: 0x000C7A52
		// (set) Token: 0x06002329 RID: 9001 RVA: 0x000C985A File Offset: 0x000C7A5A
		public BonusType bonusType { get; private set; }

		// Token: 0x0600232A RID: 9002 RVA: 0x000C9863 File Offset: 0x000C7A63
		public void SetBonusBadge(BonusType bonusType)
		{
			this.bonusType = bonusType;
		}
	}
}
