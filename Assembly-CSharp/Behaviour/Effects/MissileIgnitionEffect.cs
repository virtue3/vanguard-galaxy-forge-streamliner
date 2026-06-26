using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000395 RID: 917
	public class MissileIgnitionEffect : AbstractEffect
	{
		// Token: 0x060022E8 RID: 8936 RVA: 0x000C8DD4 File Offset: 0x000C6FD4
		private void Start()
		{
			base.visualEffect.SetFloat("Frequency", this.frequency);
			base.visualEffect.SetFloat("Size", this.size);
			base.visualEffect.SetGradient("Color", this.color);
		}

		// Token: 0x040014D0 RID: 5328
		[SerializeField]
		private float frequency;

		// Token: 0x040014D1 RID: 5329
		[SerializeField]
		private float size;

		// Token: 0x040014D2 RID: 5330
		[SerializeField]
		private Gradient color;
	}
}
