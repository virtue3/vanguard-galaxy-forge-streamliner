using System;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003BA RID: 954
	public class StarLayerPerformant : MonoBehaviour
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x000CE8BC File Offset: 0x000CCABC
		// (set) Token: 0x060024A7 RID: 9383 RVA: 0x000CE8C4 File Offset: 0x000CCAC4
		public SpriteRenderer background { get; private set; }

		// Token: 0x060024A8 RID: 9384 RVA: 0x000CE8D0 File Offset: 0x000CCAD0
		private void SetSpriteRenderer()
		{
			if (!this.background)
			{
				this.materialCopy = new Material(this.material);
				this.background = base.GetComponent<SpriteRenderer>();
				this.backgroundColorIdentifier = Shader.PropertyToID("_BackgroundColor");
				this.starColorVarianceIdentifier = Shader.PropertyToID("_ColorVariance");
				this.starAmountIdentifier = Shader.PropertyToID("_StarAmount");
				this.starSizeIdentifier = Shader.PropertyToID("_StarSize");
				this.maskScaleIdentifier = Shader.PropertyToID("_MaskScale");
				this.offsetIdentifier = Shader.PropertyToID("_Offset");
				this.initialBounds = this.background.bounds.size;
			}
		}

		// Token: 0x060024A9 RID: 9385 RVA: 0x000CE988 File Offset: 0x000CCB88
		private void CreateSprite(int width, int height)
		{
			Sprite sprite = SpriteHelper.CreateSpriteFromShader(this.materialCopy, width, height);
			this.background.sprite = sprite;
			base.transform.localScale = this.screenSize / this.background.sprite.bounds.size;
		}

		// Token: 0x060024AA RID: 9386 RVA: 0x000CE9E8 File Offset: 0x000CCBE8
		public void SetScreenSize(Vector2 screenSize, Vector2 offset)
		{
			this.SetSpriteRenderer();
			this.screenSize = screenSize;
			this.offset = offset;
			this.background.transform.localScale = screenSize / this.initialBounds;
			this.materialCopy.SetVector(this.offsetIdentifier, offset);
			if (this.data != null)
			{
				this.SetBackground();
			}
			float num = screenSize.y / screenSize.x;
			int width = Screen.width;
			int height = (int)((float)Screen.width * num);
			this.CreateSprite(width, height);
		}

		// Token: 0x060024AB RID: 9387 RVA: 0x000CEA75 File Offset: 0x000CCC75
		public void SetData(StarLayerPerformantData data)
		{
			this.data = data;
			this.SetSpriteRenderer();
			this.SetBackground();
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x000CEA8C File Offset: 0x000CCC8C
		private void SetBackground()
		{
			this.materialCopy.SetColor(this.backgroundColorIdentifier, this.data.backgroundColor);
			this.materialCopy.SetFloat(this.maskScaleIdentifier, this.data.maskScale);
			this.materialCopy.SetFloat(this.starAmountIdentifier, this.data.starAmount);
			this.materialCopy.SetFloat(this.starSizeIdentifier, this.data.starSize);
			this.materialCopy.SetFloat(this.starColorVarianceIdentifier, this.data.colorVariance);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000CEB28 File Offset: 0x000CCD28
		public StarLayerPerformantData GetData()
		{
			if (this.data == null)
			{
				this.data = new StarLayerPerformantData();
			}
			this.data.backgroundColor = this.materialCopy.GetColor(this.backgroundColorIdentifier);
			this.data.maskScale = this.materialCopy.GetFloat(this.maskScaleIdentifier);
			this.data.starAmount = this.materialCopy.GetFloat(this.starAmountIdentifier);
			this.data.starSize = this.materialCopy.GetFloat(this.starSizeIdentifier);
			this.data.colorVariance = this.materialCopy.GetFloat(this.starColorVarianceIdentifier);
			return this.data;
		}

		// Token: 0x04001635 RID: 5685
		[SerializeField]
		protected Material material;

		// Token: 0x04001637 RID: 5687
		private Vector2 initialBounds;

		// Token: 0x04001638 RID: 5688
		private Vector2 screenSize;

		// Token: 0x04001639 RID: 5689
		private StarLayerPerformantData data;

		// Token: 0x0400163A RID: 5690
		private Vector2 offset;

		// Token: 0x0400163B RID: 5691
		private int backgroundColorIdentifier;

		// Token: 0x0400163C RID: 5692
		private int starColorVarianceIdentifier;

		// Token: 0x0400163D RID: 5693
		private int starAmountIdentifier;

		// Token: 0x0400163E RID: 5694
		private int starSizeIdentifier;

		// Token: 0x0400163F RID: 5695
		private int maskScaleIdentifier;

		// Token: 0x04001640 RID: 5696
		private int offsetIdentifier;

		// Token: 0x04001641 RID: 5697
		protected Material materialCopy;
	}
}
