using System;
using Source.Galaxy;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003AF RID: 943
	public class BackgroundImageOldNoise : MonoBehaviour
	{
		// Token: 0x0600244F RID: 9295 RVA: 0x000CC574 File Offset: 0x000CA774
		private void Awake()
		{
			this.screenSizePixels = new Vector2((float)Screen.width, (float)Screen.height) / 4f;
			this.backgroundSpriteRenderer = base.GetComponent<SpriteRenderer>();
			this.initialBounds = this.backgroundSpriteRenderer.bounds.size;
			this.colorOneIdentifier = Shader.PropertyToID("_ColorOne");
			this.colorTwoIdentifier = Shader.PropertyToID("_ColorTwo");
			this.colorThreeIdentifier = Shader.PropertyToID("_ColorThree");
			this.alphaIdentifier = Shader.PropertyToID("_Alpha");
			this.colorMixScaleIdentifier = Shader.PropertyToID("_ColorMixScale");
			this.scaleIdentifier = Shader.PropertyToID("_Scale");
			this.noiseOffsetIdentifier = Shader.PropertyToID("_NoiseOffset");
			this.noiseTilingIdentifier = Shader.PropertyToID("_NoiseTiling");
			this.noiseTextureIdentifier = Shader.PropertyToID("_MainTex");
			this.maskTextureIdentifier = Shader.PropertyToID("_MaskTex");
			this.mask2TextureIdentifier = Shader.PropertyToID("_Mask2Tex");
			this.colorOverlayTextureIdentifier = Shader.PropertyToID("_ColorLerpTex");
			this.backgroundSpriteRenderer.material.SetColor(this.colorOneIdentifier, this.colorOne);
			this.backgroundSpriteRenderer.material.SetColor(this.colorTwoIdentifier, this.colorTwo);
			this.backgroundSpriteRenderer.material.SetColor(this.colorThreeIdentifier, this.colorThree);
			this.backgroundSpriteRenderer.material.SetFloat(this.colorMixScaleIdentifier, this.colorMixScale);
			this.backgroundSpriteRenderer.material.SetFloat(this.scaleIdentifier, this.noiseScale);
			this.backgroundSpriteRenderer.material.SetVector(this.noiseTilingIdentifier, this.noiseTiling);
			this.backgroundSpriteRenderer.material.SetVector(this.noiseOffsetIdentifier, this.noiseOffset);
			this.backgroundSpriteRenderer.material.SetTexture(this.noiseTextureIdentifier, this.noiseTexture);
			this.backgroundSpriteRenderer.material.SetTexture(this.maskTextureIdentifier, this.maskTexture);
			this.backgroundSpriteRenderer.material.SetTexture(this.mask2TextureIdentifier, this.maskTexture2);
			this.backgroundSpriteRenderer.material.SetTexture(this.colorOverlayTextureIdentifier, this.colorOverlayTexture);
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000CC7CA File Offset: 0x000CA9CA
		private void Start()
		{
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000CC7CC File Offset: 0x000CA9CC
		protected void RenderTextureNoise(BackgroundTextureType type, TextureNoiseData data)
		{
			if (this.noiseRenderer != null)
			{
				UnityEngine.Object.DestroyImmediate(this.noiseRenderer);
			}
			FastNoiseLite fastNoiseLite = new FastNoiseLite(data.seed);
			fastNoiseLite.SetNoiseType(data.noiseType);
			fastNoiseLite.SetFrequency(data.frequency);
			fastNoiseLite.SetFractalLacunarity(data.fractalLacunarity);
			fastNoiseLite.SetFractalGain(data.fractalGain);
			fastNoiseLite.SetFractalOctaves(data.fractalOctaves);
			fastNoiseLite.SetFractalType(data.fractalType);
			fastNoiseLite.SetCellularJitter(data.cellularJitter);
			fastNoiseLite.SetCellularDistanceFunction(data.cellularDistanceFunction);
			fastNoiseLite.SetCellularReturnType(data.cellularReturnType);
			fastNoiseLite.SetFractalWeightedStrength(data.fractalWeighedStrength);
			if (this.screenSizePixels.x == 0f)
			{
				this.screenSizePixels = new Vector2(300f, 300f);
			}
			int num = (int)this.screenSizePixels.x;
			int num2 = (int)this.screenSizePixels.y;
			Texture2D texture2D = new Texture2D(num, num2, TextureFormat.ARGB32, false);
			texture2D.filterMode = FilterMode.Bilinear;
			Color[] pixels = texture2D.GetPixels();
			Color a = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					pixels[j * num + i] = fastNoiseLite.GetNoise((float)i, (float)j) * a;
				}
			}
			texture2D.SetPixels(pixels);
			texture2D.Apply();
			if (this.showNoiseTexture)
			{
				GameObject gameObject = new GameObject();
				gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject, base.transform);
				this.noiseRenderer = gameObject.AddComponent<SpriteRenderer>();
				Sprite sprite = Sprite.Create(texture2D, new Rect(new Vector2(0f, 0f), new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f));
				this.noiseRenderer.sprite = sprite;
			}
			this.SetTextureType(type, texture2D);
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000CC9C0 File Offset: 0x000CABC0
		public void SetTextureType(BackgroundTextureType textureType, Texture2D texture)
		{
			switch (textureType)
			{
			case BackgroundTextureType.Noise:
				this.noiseTexture = texture;
				this.backgroundSpriteRenderer.material.SetTexture(this.noiseTextureIdentifier, this.noiseTexture);
				break;
			case BackgroundTextureType.Mask:
				this.maskTexture = texture;
				this.backgroundSpriteRenderer.material.SetTexture(this.maskTextureIdentifier, this.maskTexture);
				break;
			case BackgroundTextureType.Mask2:
				this.maskTexture2 = texture;
				this.backgroundSpriteRenderer.material.SetTexture(this.mask2TextureIdentifier, this.maskTexture2);
				break;
			case BackgroundTextureType.ColorOverlay:
				this.colorOverlayTexture = texture;
				this.backgroundSpriteRenderer.material.SetTexture(this.colorOverlayTextureIdentifier, this.colorOverlayTexture);
				break;
			}
			if (this.showNoiseTexture && textureType == this.textureType)
			{
				this.showTexture = texture;
			}
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000CCA92 File Offset: 0x000CAC92
		public void SetScreenSize(Vector2 screenSize)
		{
			this.screenSize = screenSize;
			this.backgroundSpriteRenderer.transform.localScale = screenSize / this.initialBounds;
		}

		// Token: 0x040015A8 RID: 5544
		[SerializeField]
		private Color colorOne;

		// Token: 0x040015A9 RID: 5545
		[SerializeField]
		private Color colorTwo;

		// Token: 0x040015AA RID: 5546
		[SerializeField]
		private Color colorThree;

		// Token: 0x040015AB RID: 5547
		[SerializeField]
		private float colorMixScale;

		// Token: 0x040015AC RID: 5548
		[SerializeField]
		private Vector2 noiseTiling;

		// Token: 0x040015AD RID: 5549
		[SerializeField]
		private Vector2 noiseOffset;

		// Token: 0x040015AE RID: 5550
		[SerializeField]
		private float noiseScale;

		// Token: 0x040015AF RID: 5551
		[SerializeField]
		private Texture2D noiseTexture;

		// Token: 0x040015B0 RID: 5552
		[SerializeField]
		private Texture2D maskTexture;

		// Token: 0x040015B1 RID: 5553
		[SerializeField]
		private Texture2D maskTexture2;

		// Token: 0x040015B2 RID: 5554
		[SerializeField]
		private Texture2D colorOverlayTexture;

		// Token: 0x040015B3 RID: 5555
		private int colorOneIdentifier;

		// Token: 0x040015B4 RID: 5556
		private int colorTwoIdentifier;

		// Token: 0x040015B5 RID: 5557
		private int colorThreeIdentifier;

		// Token: 0x040015B6 RID: 5558
		private int alphaIdentifier;

		// Token: 0x040015B7 RID: 5559
		private int colorMixScaleIdentifier;

		// Token: 0x040015B8 RID: 5560
		private int scaleIdentifier;

		// Token: 0x040015B9 RID: 5561
		private int noiseTilingIdentifier;

		// Token: 0x040015BA RID: 5562
		private int noiseOffsetIdentifier;

		// Token: 0x040015BB RID: 5563
		private int noiseTextureIdentifier;

		// Token: 0x040015BC RID: 5564
		private int maskTextureIdentifier;

		// Token: 0x040015BD RID: 5565
		private int mask2TextureIdentifier;

		// Token: 0x040015BE RID: 5566
		private int colorOverlayTextureIdentifier;

		// Token: 0x040015BF RID: 5567
		private Vector2 initialBounds;

		// Token: 0x040015C0 RID: 5568
		private Vector2 screenSize;

		// Token: 0x040015C1 RID: 5569
		private Vector2 screenSizePixels;

		// Token: 0x040015C2 RID: 5570
		private SpriteRenderer backgroundSpriteRenderer;

		// Token: 0x040015C3 RID: 5571
		private SpriteRenderer noiseRenderer;

		// Token: 0x040015C4 RID: 5572
		private BackgroundTextureType textureType;

		// Token: 0x040015C5 RID: 5573
		private bool showNoiseTexture;

		// Token: 0x040015C6 RID: 5574
		private Texture2D showTexture;

		// Token: 0x040015C7 RID: 5575
		private bool generatingNoise;

		// Token: 0x040015C8 RID: 5576
		private TextureNoiseData noiseData;

		// Token: 0x040015C9 RID: 5577
		private BackgroundPresets backgroundDataPresets;

		// Token: 0x040015CA RID: 5578
		private string tempName;
	}
}
