using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x0200038C RID: 908
	public class SparksEffect : AbstractEffect
	{
		// Token: 0x060022BC RID: 8892 RVA: 0x000C8419 File Offset: 0x000C6619
		public void setSize(float size)
		{
			this.size = size;
			base.visualEffect.SetFloat("Size", size);
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000C8433 File Offset: 0x000C6633
		public void setFrequency(float frequency)
		{
			this.frequency = frequency;
			base.visualEffect.SetFloat("Frequency", frequency);
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000C844D File Offset: 0x000C664D
		public void SetColor(Color color)
		{
			this.color = color;
			base.visualEffect.SetVector4("Color", color);
		}

		// Token: 0x040014A3 RID: 5283
		private float size;

		// Token: 0x040014A4 RID: 5284
		private float frequency;

		// Token: 0x040014A5 RID: 5285
		private Color color;
	}
}
