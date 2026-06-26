using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000396 RID: 918
	public class MissileTrailEffect : TrailEffect
	{
		// Token: 0x060022EA RID: 8938 RVA: 0x000C8E2B File Offset: 0x000C702B
		private void Start()
		{
			base.visualEffect.SetGradient("ColorGradient", this.gradient);
			base.visualEffect.SetFloat("AverageLifetime", this.averageLifetime);
		}

		// Token: 0x040014D3 RID: 5331
		[SerializeField]
		private Gradient gradient;

		// Token: 0x040014D4 RID: 5332
		[SerializeField]
		private float averageLifetime;
	}
}
