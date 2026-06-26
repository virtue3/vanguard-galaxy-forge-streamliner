using System;
using UnityEngine;
using _Scripts.Behaviour.Background;

namespace Behaviour.Background
{
	// Token: 0x020003AD RID: 941
	public class AbstractGalaxyBackground : AbstractGalaxyMapBackground
	{
		// Token: 0x0600244B RID: 9291 RVA: 0x000CC550 File Offset: 0x000CA750
		private void Awake()
		{
			this.background = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x000CC55E File Offset: 0x000CA75E
		public void SetScreenSize(Vector2 screenSize)
		{
			this.screenSize = screenSize;
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000CC567 File Offset: 0x000CA767
		public override void SetBackgroundAlpha(float alpha)
		{
		}

		// Token: 0x040015A0 RID: 5536
		private SpriteRenderer background;

		// Token: 0x040015A1 RID: 5537
		private Vector2 initialBounds;

		// Token: 0x040015A2 RID: 5538
		private Vector2 screenSize;
	}
}
