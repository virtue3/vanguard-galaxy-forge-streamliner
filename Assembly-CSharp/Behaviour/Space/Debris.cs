using System;
using UnityEngine;

namespace Behaviour.Space
{
	// Token: 0x020002EB RID: 747
	public class Debris : MonoBehaviour
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001B48 RID: 6984 RVA: 0x000A6B27 File Offset: 0x000A4D27
		// (set) Token: 0x06001B49 RID: 6985 RVA: 0x000A6B2F File Offset: 0x000A4D2F
		public PolygonCollider2D collider { get; protected set; }

		// Token: 0x06001B4A RID: 6986 RVA: 0x000A6B38 File Offset: 0x000A4D38
		private void Awake()
		{
			this.collider = base.GetComponent<PolygonCollider2D>();
			this.lifetime = SeededRandom.Global.RandomRange(this.averageLifetime - this.averageLifetime / 2f, this.averageLifetime + this.averageLifetime / 2f);
			this.fadeRemaining = this.fadeDuration;
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000A6B94 File Offset: 0x000A4D94
		private void Update()
		{
			this.lifetime -= Time.deltaTime;
			if (this.lifetime < 0f)
			{
				this.fading = true;
				this.DisableColliders();
				UnityEngine.Object.Destroy(base.gameObject, this.fadeDuration);
			}
			if (this.fading)
			{
				this.FadeAway();
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000A6BEC File Offset: 0x000A4DEC
		protected virtual void DisableColliders()
		{
			UnityEngine.Object.Destroy(this.collider);
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000A6BFC File Offset: 0x000A4DFC
		protected virtual void FadeAway()
		{
			this.fadeRemaining -= Time.deltaTime;
			base.transform.localScale = Vector3.one * Mathf.Clamp(this.fadeRemaining / this.fadeDuration, 0f, 1f);
			this.FadeSpriteRenderer(this.spriteRenderer);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x000A6C58 File Offset: 0x000A4E58
		protected void FadeSpriteRenderer(SpriteRenderer renderer)
		{
			Color color = renderer.color;
			color.a = Mathf.Clamp(this.fadeRemaining / this.fadeDuration, 0f, 1f);
			renderer.color = color;
		}

		// Token: 0x0400111E RID: 4382
		[SerializeField]
		private float averageLifetime;

		// Token: 0x0400111F RID: 4383
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04001121 RID: 4385
		[SerializeField]
		private float fadeDuration;

		// Token: 0x04001122 RID: 4386
		private float lifetime;

		// Token: 0x04001123 RID: 4387
		private bool fading;

		// Token: 0x04001124 RID: 4388
		protected float fadeRemaining;
	}
}
