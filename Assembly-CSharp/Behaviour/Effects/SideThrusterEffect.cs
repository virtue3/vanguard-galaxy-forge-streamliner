using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000388 RID: 904
	public class SideThrusterEffect : ThrusterEffect
	{
		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x060022B1 RID: 8881 RVA: 0x000C8282 File Offset: 0x000C6482
		public override bool isMain
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000C8285 File Offset: 0x000C6485
		protected override void Start()
		{
			base.Start();
			base.visualEffect.SetGradient("Gradient", this.gradient);
			this.maxPower = 250f;
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000C82AE File Offset: 0x000C64AE
		public Side GetSide()
		{
			return this.side;
		}

		// Token: 0x04001495 RID: 5269
		[SerializeField]
		private Gradient gradient;

		// Token: 0x04001496 RID: 5270
		[SerializeField]
		private Side side;
	}
}
