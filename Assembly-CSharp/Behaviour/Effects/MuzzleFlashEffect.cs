using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000397 RID: 919
	public class MuzzleFlashEffect : AbstractEffect
	{
		// Token: 0x060022EC RID: 8940 RVA: 0x000C8E64 File Offset: 0x000C7064
		protected override void Awake()
		{
			base.Awake();
			base.visualEffect.SetFloat("Frequency", this.frequency);
			base.visualEffect.SetFloat("Size", this.size);
			base.visualEffect.SetGradient("Gradient", this.color);
			base.visualEffect.SetFloat("Amount", 0f);
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000C8ECE File Offset: 0x000C70CE
		public override void Play(float delay)
		{
			base.visualEffect.SetFloat("Amount", this.amount);
			base.Play(delay);
		}

		// Token: 0x040014D5 RID: 5333
		[SerializeField]
		private float frequency;

		// Token: 0x040014D6 RID: 5334
		[SerializeField]
		private float size;

		// Token: 0x040014D7 RID: 5335
		[SerializeField]
		private Gradient color;

		// Token: 0x040014D8 RID: 5336
		[SerializeField]
		private float amount;
	}
}
