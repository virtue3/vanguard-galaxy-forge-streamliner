using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000393 RID: 915
	public class FlashEffect : AbstractEffect
	{
		// Token: 0x060022E3 RID: 8931 RVA: 0x000C8D89 File Offset: 0x000C6F89
		private void Start()
		{
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000C8D8B File Offset: 0x000C6F8B
		public void SetSize(float size)
		{
			this.size = size;
			base.visualEffect.SetFloat("Size", size);
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000C8DA5 File Offset: 0x000C6FA5
		public void SetColor(Color color)
		{
			this.color = color;
			base.visualEffect.SetVector4("Color", color);
		}

		// Token: 0x040014CC RID: 5324
		private float size;

		// Token: 0x040014CD RID: 5325
		private Color color;

		// Token: 0x040014CE RID: 5326
		private int sizeIdentifier;

		// Token: 0x040014CF RID: 5327
		private int colorIdentifier;
	}
}
