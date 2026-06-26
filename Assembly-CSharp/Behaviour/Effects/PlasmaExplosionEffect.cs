using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000385 RID: 901
	public class PlasmaExplosionEffect : AbstractEffect
	{
		// Token: 0x0600229A RID: 8858 RVA: 0x000C7CD8 File Offset: 0x000C5ED8
		public void SetGradient(Gradient gradient)
		{
			base.visualEffect.SetGradient("Color", gradient);
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x000C7CEB File Offset: 0x000C5EEB
		public void SetSize(float size)
		{
			base.visualEffect.SetFloat("Size", size);
		}

		// Token: 0x0400147A RID: 5242
		public Gradient gradient;

		// Token: 0x0400147B RID: 5243
		public float size;
	}
}
