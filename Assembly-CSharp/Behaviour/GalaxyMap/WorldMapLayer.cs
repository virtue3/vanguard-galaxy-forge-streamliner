using System;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviour.GalaxyMap
{
	// Token: 0x02000334 RID: 820
	public class WorldMapLayer : MonoBehaviour
	{
		// Token: 0x06001F19 RID: 7961 RVA: 0x000B9276 File Offset: 0x000B7476
		public void AddSpriteRenderers(List<SpriteRenderer> spriteRenderers)
		{
			this.spriteRenderers.AddRange(spriteRenderers);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000B9284 File Offset: 0x000B7484
		public void CheckPosition(Vector2 position)
		{
			if (position.x < this.minX)
			{
				this.minX = position.x;
			}
			else if (position.x > this.maxX)
			{
				this.maxX = position.x;
			}
			if (position.y < this.minY)
			{
				this.minY = position.y;
				return;
			}
			if (position.y > this.maxY)
			{
				this.maxY = position.y;
			}
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000B92FC File Offset: 0x000B74FC
		public void AddLineRenderer(LineRenderer lineRenderer)
		{
			this.lines.Add(lineRenderer);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000B930C File Offset: 0x000B750C
		public void SetAlpha(float alpha)
		{
			foreach (SpriteRenderer spriteRenderer in this.spriteRenderers)
			{
				Color color = spriteRenderer.color;
				color.a = alpha;
				spriteRenderer.color = color;
			}
			foreach (LineRenderer lineRenderer in this.lines)
			{
				Color startColor = lineRenderer.startColor;
				startColor.a = Mathf.Pow(alpha, 4f);
				lineRenderer.startColor = startColor;
				Color endColor = lineRenderer.endColor;
				endColor.a = Mathf.Pow(alpha, 4f);
				lineRenderer.endColor = endColor;
			}
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000B93E8 File Offset: 0x000B75E8
		public void ClearContent()
		{
			this.spriteRenderers.Clear();
			this.lines.Clear();
			base.transform.DestroyChildren();
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000B940B File Offset: 0x000B760B
		public Rect GetXYBounds()
		{
			return new Rect(this.minX - this.margin, this.maxX + this.margin, this.minY - this.margin, this.maxY + this.margin);
		}

		// Token: 0x040012A2 RID: 4770
		private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

		// Token: 0x040012A3 RID: 4771
		private List<LineRenderer> lines = new List<LineRenderer>();

		// Token: 0x040012A4 RID: 4772
		[SerializeField]
		private Camera mapCamera;

		// Token: 0x040012A5 RID: 4773
		public float padding = 0.95f;

		// Token: 0x040012A6 RID: 4774
		public float margin = 4f;

		// Token: 0x040012A7 RID: 4775
		public float minX;

		// Token: 0x040012A8 RID: 4776
		public float maxX;

		// Token: 0x040012A9 RID: 4777
		public float minY;

		// Token: 0x040012AA RID: 4778
		public float maxY;
	}
}
