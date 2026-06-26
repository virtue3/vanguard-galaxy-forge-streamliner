using System;
using UnityEngine;

namespace Behaviour.Ability
{
	// Token: 0x020003C2 RID: 962
	public class StackableEffect : MonoBehaviour
	{
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x000D051E File Offset: 0x000CE71E
		// (set) Token: 0x0600252A RID: 9514 RVA: 0x000D0526 File Offset: 0x000CE726
		public int maxStackSize { get; private set; }

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x000D052F File Offset: 0x000CE72F
		// (set) Token: 0x0600252C RID: 9516 RVA: 0x000D0537 File Offset: 0x000CE737
		public int stackSize { get; private set; } = 1;

		// Token: 0x0600252D RID: 9517 RVA: 0x000D0540 File Offset: 0x000CE740
		public void SetStackSize(int stackSize)
		{
			this.stackSize = stackSize;
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x000D054C File Offset: 0x000CE74C
		public virtual void AddStack(StackableEffect other)
		{
			int stackSize = this.stackSize;
			this.stackSize += other.stackSize;
			if (this.maxStackSize > 0 && this.stackSize > this.maxStackSize)
			{
				this.stackSize = stackSize;
			}
		}
	}
}
