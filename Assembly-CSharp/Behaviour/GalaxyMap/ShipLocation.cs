using System;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000331 RID: 817
	public class ShipLocation : MonoBehaviour
	{
		// Token: 0x06001EF3 RID: 7923 RVA: 0x000B8DD6 File Offset: 0x000B6FD6
		private void SetSpriteRenderer()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000B8DE4 File Offset: 0x000B6FE4
		public void SetSprite(Sprite sprite)
		{
			if (this.spriteRenderer == null)
			{
				this.SetSpriteRenderer();
			}
			Vector2 vector = sprite.bounds.size;
			if (vector.x > this.maxWidth)
			{
				float num = this.maxWidth / vector.x;
				base.gameObject.transform.localScale = new Vector2(num, num);
			}
			this.spriteRenderer.sprite = sprite;
		}

		// Token: 0x0400128C RID: 4748
		[SerializeField]
		private float maxWidth;

		// Token: 0x0400128D RID: 4749
		private SpriteRenderer spriteRenderer;
	}
}
