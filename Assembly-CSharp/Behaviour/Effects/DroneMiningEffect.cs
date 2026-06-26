using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x02000383 RID: 899
	public class DroneMiningEffect : AbstractEffect
	{
		// Token: 0x06002295 RID: 8853 RVA: 0x000C7C28 File Offset: 0x000C5E28
		private void Start()
		{
			base.visualEffect.SetFloat("Size", this.size);
			base.visualEffect.SetGradient("Color", this.color);
			base.visualEffect.SetFloat("Frequency", this.frequency);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000C7C77 File Offset: 0x000C5E77
		public void PlayWithSize(float size, float frequency)
		{
			base.visualEffect.SetFloat("Size", size);
			base.visualEffect.SetFloat("Frequency", frequency);
			base.Play();
		}

		// Token: 0x04001477 RID: 5239
		[SerializeField]
		private float size;

		// Token: 0x04001478 RID: 5240
		[SerializeField]
		private Gradient color;

		// Token: 0x04001479 RID: 5241
		[SerializeField]
		private float frequency;
	}
}
