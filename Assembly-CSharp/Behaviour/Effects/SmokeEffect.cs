using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x0200038A RID: 906
	public class SmokeEffect : AbstractEffect
	{
		// Token: 0x060022B5 RID: 8885 RVA: 0x000C82C0 File Offset: 0x000C64C0
		private void Start()
		{
			base.visualEffect.SetFloat("Size", this.size);
			base.visualEffect.SetGradient("Gradient", this.color);
			base.visualEffect.SetFloat("Count", (float)this.count);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000C8310 File Offset: 0x000C6510
		public void SetCombineColor(Gradient combineColor)
		{
			this.color = combineColor;
			base.visualEffect.SetGradient("Gradient", combineColor);
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000C832C File Offset: 0x000C652C
		public void PlayWithArgs(float size, float lifetime, int count)
		{
			this.size = size;
			this.lifetime = lifetime;
			this.count = count;
			base.visualEffect.SetFloat("Size", size);
			base.visualEffect.SetFloat("Lifetime", lifetime);
			base.visualEffect.SetFloat("Count", (float)count);
			base.Play();
		}

		// Token: 0x0400149C RID: 5276
		[SerializeField]
		private float size;

		// Token: 0x0400149D RID: 5277
		[SerializeField]
		private Gradient color;

		// Token: 0x0400149E RID: 5278
		[SerializeField]
		private float lifetime;

		// Token: 0x0400149F RID: 5279
		[SerializeField]
		private int count;
	}
}
