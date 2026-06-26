using System;

namespace Behaviour.Effects
{
	// Token: 0x02000382 RID: 898
	public class AsteroidExplosionEffect : AbstractEffect
	{
		// Token: 0x06002292 RID: 8850 RVA: 0x000C7BF8 File Offset: 0x000C5DF8
		public void SetShockWaveSize(float size)
		{
			base.visualEffect.SetFloat("ShockwaveSize", size);
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000C7C0B File Offset: 0x000C5E0B
		public void SetDelay(float delay)
		{
			base.visualEffect.SetFloat("Delay", delay);
		}
	}
}
