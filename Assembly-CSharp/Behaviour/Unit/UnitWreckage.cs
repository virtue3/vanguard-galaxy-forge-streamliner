using System;
using Behaviour.Space;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001CA RID: 458
	public class UnitWreckage : Debris
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x0007348C File Offset: 0x0007168C
		protected override void FadeAway()
		{
			base.FadeAway();
			base.FadeSpriteRenderer(this.core);
		}

		// Token: 0x0400097E RID: 2430
		public SpriteRenderer surface;

		// Token: 0x0400097F RID: 2431
		public SpriteRenderer core;

		// Token: 0x04000980 RID: 2432
		public Rigidbody2D rigidbody2D;
	}
}
