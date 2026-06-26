using System;
using UnityEngine;

namespace Behaviour.Effects
{
	// Token: 0x0200038F RID: 911
	public class InJumpGateTunnelEffect : AbstractEffect
	{
		// Token: 0x060022D1 RID: 8913 RVA: 0x000C87C4 File Offset: 0x000C69C4
		protected override void Awake()
		{
			base.Awake();
			base.visualEffect.SetVector2("ScreenSize", this.screenSize);
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000C87E2 File Offset: 0x000C69E2
		public void SetStartSize(Vector2 screenSize)
		{
			this.screenSize = screenSize;
			base.visualEffect.SetVector2("ScreenSize", screenSize);
		}

		// Token: 0x040014B2 RID: 5298
		[SerializeField]
		private Vector2 screenSize;
	}
}
