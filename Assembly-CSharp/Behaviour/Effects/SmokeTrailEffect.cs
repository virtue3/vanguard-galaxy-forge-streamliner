using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x0200038B RID: 907
	public class SmokeTrailEffect : TrailEffect
	{
		// Token: 0x060022B9 RID: 8889 RVA: 0x000C8390 File Offset: 0x000C6590
		private void Start()
		{
			base.visualEffect.SetFloat("Size", this.size);
			base.visualEffect.SetGradient("Gradient", this.color);
			base.visualEffect.SetFloat("AverageLifetime", this.averageLifetime);
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x000C83DF File Offset: 0x000C65DF
		public void PlayWithSize(float size, float delay, float spawnRate)
		{
			this.size = size;
			base.visualEffect.SetFloat("Size", size);
			base.visualEffect.SetFloat("SpawnRate", spawnRate);
			base.Play(delay);
		}

		// Token: 0x040014A0 RID: 5280
		[SerializeField]
		private float size;

		// Token: 0x040014A1 RID: 5281
		[SerializeField]
		private float averageLifetime;

		// Token: 0x040014A2 RID: 5282
		[SerializeField]
		private Gradient color;
	}
}
