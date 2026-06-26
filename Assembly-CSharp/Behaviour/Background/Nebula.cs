using System;
using Source.Galaxy;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B4 RID: 948
	public class Nebula : MonoBehaviour
	{
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002465 RID: 9317 RVA: 0x000CCFB1 File Offset: 0x000CB1B1
		// (set) Token: 0x06002466 RID: 9318 RVA: 0x000CCFB9 File Offset: 0x000CB1B9
		public SpriteRenderer spriteRenderer { get; private set; }

		// Token: 0x06002467 RID: 9319 RVA: 0x000CCFC2 File Offset: 0x000CB1C2
		private void Awake()
		{
			this.SetSpriteRenderer();
			if (this.data != null)
			{
				this.RenderNebula();
			}
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x000CCFD8 File Offset: 0x000CB1D8
		private void SetSpriteRenderer()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			this.secondTextureIdentifier = Shader.PropertyToID("_SecondTex");
			this.colorOneIdentifier = Shader.PropertyToID("_ColorOne");
			this.colorTwoIdentifier = Shader.PropertyToID("_ColorTwo");
			this.distortionIdentifier = Shader.PropertyToID("_Distortion");
			this.noiseScaleIdentifier = Shader.PropertyToID("_NoiseScale");
			this.mainTilingIdentifier = Shader.PropertyToID("_MainTiling");
			this.mainOffsetIdentifier = Shader.PropertyToID("_MainOffset");
			this.maskTextureIdentifier = Shader.PropertyToID("_MaskTex");
			this.maskDistortionIdentifier = Shader.PropertyToID("_MaskDistortion");
			this.maskNoiseScaleIdentifier = Shader.PropertyToID("_MaskNoiseScale");
			this.maskTilingIdentifier = Shader.PropertyToID("_MaskTiling");
			this.maskOffsetIdentifier = Shader.PropertyToID("_MaskOffset");
			this.highlightMaskIdentifier = Shader.PropertyToID("_HighlightMask");
			this.highlightDistortionIdentifier = Shader.PropertyToID("_HighlightMaskDistortion");
			this.highlightColorIdentifier = Shader.PropertyToID("_HighlightColor");
			this.highlightNoiseScaleIdentifier = Shader.PropertyToID("_HighlightNoiseScale");
			this.highlightTilingIdentifier = Shader.PropertyToID("_highlightTiling");
			this.highlightOffsetIdentifier = Shader.PropertyToID("_highlightOffset");
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x000CD111 File Offset: 0x000CB311
		public void SetData(NebulaData data)
		{
			if (!this.spriteRenderer)
			{
				this.SetSpriteRenderer();
			}
			this.data = data;
			this.RenderNebula();
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x000CD134 File Offset: 0x000CB334
		private void RenderNebula()
		{
			base.transform.localPosition = this.data.position;
			base.transform.localScale = this.data.scale;
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, this.data.rotation));
			this.spriteRenderer.color = this.data.spriteColor;
			this.spriteRenderer.material.SetTexture(this.secondTextureIdentifier, this.GetTextureForIndex(this.data.secondTextureIndex));
			this.spriteRenderer.material.SetColor(this.colorOneIdentifier, this.data.colorOne);
			this.spriteRenderer.material.SetColor(this.colorTwoIdentifier, this.data.colorTwo);
			this.spriteRenderer.material.SetFloat(this.distortionIdentifier, this.data.distortion);
			this.spriteRenderer.material.SetFloat(this.noiseScaleIdentifier, this.data.noiseScale);
			this.spriteRenderer.material.SetVector(this.mainTilingIdentifier, this.data.mainTiling);
			this.spriteRenderer.material.SetVector(this.mainOffsetIdentifier, this.data.mainOffset);
			this.spriteRenderer.material.SetTexture(this.maskTextureIdentifier, this.GetTextureForIndex(this.data.maskTextureIndex));
			this.spriteRenderer.material.SetFloat(this.maskDistortionIdentifier, this.data.maskDistortion);
			this.spriteRenderer.material.SetFloat(this.maskNoiseScaleIdentifier, this.data.maskNoiseScale);
			this.spriteRenderer.material.SetVector(this.maskTilingIdentifier, this.data.maskTiling);
			this.spriteRenderer.material.SetVector(this.maskOffsetIdentifier, this.data.maskOffset);
			this.spriteRenderer.material.SetTexture(this.highlightMaskIdentifier, this.GetTextureForIndex(this.data.highlightTextureIndex));
			this.spriteRenderer.material.SetColor(this.highlightColorIdentifier, this.data.colorHighlight);
			this.spriteRenderer.material.SetFloat(this.highlightDistortionIdentifier, this.data.highlightDistortion);
			this.spriteRenderer.material.SetFloat(this.highlightNoiseScaleIdentifier, this.data.hightlightNoiseScale);
			this.spriteRenderer.material.SetVector(this.highlightTilingIdentifier, this.data.highlightTiling);
			this.spriteRenderer.material.SetVector(this.highlightOffsetIdentifier, this.data.highlightOffset);
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x000CD439 File Offset: 0x000CB639
		public Texture2D GetTextureForIndex(int index)
		{
			return (Texture2D)Resources.Load("Background/Textures/" + this.data.GetNebulaTextureName(index));
		}

		// Token: 0x040015DF RID: 5599
		private NebulaData data;

		// Token: 0x040015E0 RID: 5600
		private int secondTextureIdentifier;

		// Token: 0x040015E1 RID: 5601
		private int colorOneIdentifier;

		// Token: 0x040015E2 RID: 5602
		private int colorTwoIdentifier;

		// Token: 0x040015E3 RID: 5603
		private int distortionIdentifier;

		// Token: 0x040015E4 RID: 5604
		private int noiseScaleIdentifier;

		// Token: 0x040015E5 RID: 5605
		private int mainTilingIdentifier;

		// Token: 0x040015E6 RID: 5606
		private int mainOffsetIdentifier;

		// Token: 0x040015E7 RID: 5607
		private int maskTextureIdentifier;

		// Token: 0x040015E8 RID: 5608
		private int maskDistortionIdentifier;

		// Token: 0x040015E9 RID: 5609
		private int maskNoiseScaleIdentifier;

		// Token: 0x040015EA RID: 5610
		private int maskTilingIdentifier;

		// Token: 0x040015EB RID: 5611
		private int maskOffsetIdentifier;

		// Token: 0x040015EC RID: 5612
		private int highlightColorIdentifier;

		// Token: 0x040015ED RID: 5613
		private int highlightMaskIdentifier;

		// Token: 0x040015EE RID: 5614
		private int highlightDistortionIdentifier;

		// Token: 0x040015EF RID: 5615
		private int highlightNoiseScaleIdentifier;

		// Token: 0x040015F0 RID: 5616
		private int highlightTilingIdentifier;

		// Token: 0x040015F1 RID: 5617
		private int highlightOffsetIdentifier;
	}
}
