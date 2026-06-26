using System;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B0 RID: 944
	public class BackgroundImageSimple : MonoBehaviour
	{
		// Token: 0x06002455 RID: 9301 RVA: 0x000CCAC4 File Offset: 0x000CACC4
		private void Awake()
		{
			this.SetSpriteRenderer();
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000CCACC File Offset: 0x000CACCC
		private void SetSpriteRenderer()
		{
			if (!this.background)
			{
				this.background = base.GetComponent<SpriteRenderer>();
				this.colorOneIdentifier = Shader.PropertyToID("_ColorOne");
				this.colorTwoIdentifier = Shader.PropertyToID("_ColorTwo");
				this.scaleIdentifier = Shader.PropertyToID("_Scale");
				this.colorMixScaleIdentifier = Shader.PropertyToID("_ColorMixScale");
				this.initialBounds = this.background.bounds.size;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000CCB50 File Offset: 0x000CAD50
		public void SetData(BackgroundSimpleData data)
		{
			this.data = data;
			this.SetSpriteRenderer();
			this.SetBackground();
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000CCB68 File Offset: 0x000CAD68
		private void SetBackground()
		{
			if (this.data.mainTexture != null)
			{
				Texture2D texture2D = (Texture2D)Resources.Load(this.data.mainTexture);
				this.background.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
			}
			this.background.material.SetFloat(this.scaleIdentifier, this.data.scale);
			this.background.material.SetColor(this.colorOneIdentifier, this.data.colorOne);
			this.background.material.SetColor(this.colorTwoIdentifier, this.data.colorTwo);
			this.background.material.SetFloat(this.colorMixScaleIdentifier, this.data.colorMixScale);
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000CCC5C File Offset: 0x000CAE5C
		public BackgroundSimpleData GetData()
		{
			if (this.data == null)
			{
				this.data = new BackgroundSimpleData();
			}
			if (this.background.sprite.texture.name != "Square")
			{
				Texture2D texture = this.background.sprite.texture;
				this.data.mainTexture = BackgroundStorage.GetBackgroundTexturePath(texture);
			}
			this.data.colorOne = this.background.material.GetColor(this.colorOneIdentifier);
			this.data.colorTwo = this.background.material.GetColor(this.colorTwoIdentifier);
			this.data.scale = this.background.material.GetFloat(this.scaleIdentifier);
			this.data.colorMixScale = this.background.material.GetFloat(this.colorMixScaleIdentifier);
			return this.data;
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000CCD49 File Offset: 0x000CAF49
		public void SetScreenSize(Vector2 screenSize)
		{
			this.SetSpriteRenderer();
			this.screenSize = screenSize;
			this.background.transform.localScale = screenSize / this.initialBounds;
			if (this.data != null)
			{
				this.SetBackground();
			}
		}

		// Token: 0x040015CB RID: 5579
		public BackgroundSimpleData data;

		// Token: 0x040015CC RID: 5580
		private int scaleIdentifier;

		// Token: 0x040015CD RID: 5581
		private int colorOneIdentifier;

		// Token: 0x040015CE RID: 5582
		private int colorTwoIdentifier;

		// Token: 0x040015CF RID: 5583
		private int colorMixScaleIdentifier;

		// Token: 0x040015D0 RID: 5584
		private SpriteRenderer background;

		// Token: 0x040015D1 RID: 5585
		private Vector2 screenSize;

		// Token: 0x040015D2 RID: 5586
		private Vector2 initialBounds;

		// Token: 0x040015D3 RID: 5587
		private Vector2 cameraPositionChange;
	}
}
