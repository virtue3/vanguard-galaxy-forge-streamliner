using System;
using Source.Item;

namespace Behaviour.Mining
{
	// Token: 0x020002FE RID: 766
	[Serializable]
	public class OreRefinementProduct
	{
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x000AAACA File Offset: 0x000A8CCA
		// (set) Token: 0x06001C37 RID: 7223 RVA: 0x000AAAD2 File Offset: 0x000A8CD2
		public RefinedMaterial product { get; private set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x000AAADB File Offset: 0x000A8CDB
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x000AAAE3 File Offset: 0x000A8CE3
		public float customYield { get; private set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x000AAAEC File Offset: 0x000A8CEC
		public float yield
		{
			get
			{
				if (this.customYield <= 0f)
				{
					return this.dynamicYield;
				}
				return this.customYield;
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x000AAB08 File Offset: 0x000A8D08
		public void UpdateDynamicYield(float yield)
		{
			this.dynamicYield = yield;
		}

		// Token: 0x0400118F RID: 4495
		private float dynamicYield;
	}
}
