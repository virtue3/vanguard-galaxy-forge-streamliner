using System;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B8 RID: 952
	public class StarLayer : AbstractBackgroundLayer, ICameraTrackable
	{
		// Token: 0x0600248A RID: 9354 RVA: 0x000CE074 File Offset: 0x000CC274
		protected override void Awake()
		{
			base.Awake();
			this.gridTilingIdentifier = Shader.PropertyToID("_TilingVector");
			this.starSizeIdentifier = Shader.PropertyToID("_StarSize");
			this.scaleIdentifier = Shader.PropertyToID("_Scale");
			this.offsetIdentifier = Shader.PropertyToID("_Offset");
			this.debugIdentifier = Shader.PropertyToID("_Debug");
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000CE0D7 File Offset: 0x000CC2D7
		protected void OnValidate()
		{
			if (this.data == null)
			{
				return;
			}
			this.SetPropertiesOnData();
			this.SetPropertiesOnMaterial();
			this.SetScreenSize(this.screenSize, this.screenSizeGame);
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000CE100 File Offset: 0x000CC300
		private void OnDestroy()
		{
			RenderTexture renderTexture = this.renderTexture;
			if (renderTexture == null)
			{
				return;
			}
			renderTexture.Release();
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000CE112 File Offset: 0x000CC312
		protected override void SetSpriteRenderer()
		{
			this.spriteRenderer = this.starSquare.GetComponent<SpriteRenderer>();
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000CE125 File Offset: 0x000CC325
		public override void SetCamera()
		{
			this.cameraTracker.SetCamera(Camera.main);
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000CE138 File Offset: 0x000CC338
		public override void SetScreenSize(Vector2 screenSize, Vector2 screenSizeGame)
		{
			base.SetScreenSize(screenSize, screenSizeGame);
			float num = screenSize.y / screenSize.x;
			float num2 = this.gridTiles.y / this.gridTiles.x;
			this.scaleWithFactor = new Vector2(screenSize.x, screenSize.x * num2);
			int width = Screen.width;
			int height = (int)((float)Screen.width * num2);
			this.CreateSprite(width, height);
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000CE1A4 File Offset: 0x000CC3A4
		private void CreateSprite(int width, int height)
		{
			Sprite sprite = SpriteHelper.CreateSpriteFromShader(this.materialCopy, width, height);
			this.spriteRenderer.sprite = sprite;
			base.transform.localScale = this.screenSize / this.spriteRenderer.sprite.bounds.size;
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000CE204 File Offset: 0x000CC404
		private void SetPropertiesOnMaterial()
		{
			this.materialCopy.SetVector(this.gridTilingIdentifier, this.gridTiles);
			this.materialCopy.SetFloat(this.starSizeIdentifier, this.starSize);
			this.materialCopy.SetFloat(this.scaleIdentifier, this.scale);
			this.materialCopy.SetVector(this.offsetIdentifier, this.offset);
			this.materialCopy.SetInt(this.debugIdentifier, 0);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000CE289 File Offset: 0x000CC489
		private void SetOffset(Vector2 offset)
		{
			this.offset = offset;
			this.materialCopy.SetVector(this.offsetIdentifier, offset);
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000CE2AC File Offset: 0x000CC4AC
		public void SetPositionDelta(Vector2 delta, Vector2 newPosition)
		{
			if (!this.trackCamera)
			{
				return;
			}
			Vector2 vector = Vector2.zero;
			if (GamePlayer.current != null)
			{
				vector = GamePlayer.current.mapPosition / 300f;
			}
			Vector2 vector2 = vector;
			if (GameplayManager.Instance && !GameplayManager.Instance.spaceShip.travelling)
			{
				this.totalCameraPositionChange += delta;
				vector2 += this.totalCameraPositionChange / 10000f;
			}
			vector2.x = this.scaleWithFactor.y / this.scaleWithFactor.x * vector2.x;
			this.SetOffset(this.initialOffset + vector2);
			newPosition -= vector2 * (1f / this.scale) * 10f;
			base.gameObject.transform.position = newPosition;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000CE39D File Offset: 0x000CC59D
		public void SetLayer(int layer)
		{
			base.gameObject.layer = layer;
			this.starSquare.layer = layer;
			this.trackCamera = false;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000CE3C0 File Offset: 0x000CC5C0
		public void SetData(StarLayerData data, Vector2 screenSize)
		{
			this.data = data;
			this.initialOffset = data.offset;
			this.offset = data.offset;
			this.scale = data.scale;
			this.gridTiles = data.gridTiles;
			if (!(this.gridTiles.y / this.gridTiles.x).ApproximatelyEqual(screenSize.y / screenSize.x))
			{
				this.gridTiles.y = screenSize.y / screenSize.x * this.gridTiles.x;
			}
			this.SetPropertiesOnMaterial();
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x000CE459 File Offset: 0x000CC659
		public StarLayerData GetData()
		{
			if (this.data == null)
			{
				this.data = new StarLayerData();
			}
			this.SetPropertiesOnData();
			return this.data;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000CE47C File Offset: 0x000CC67C
		private void SetPropertiesOnData()
		{
			this.data.starSize = this.starSize;
			this.data.gridTiles = this.gridTiles;
			this.data.scale = this.scale;
			this.data.offset = this.initialOffset;
		}

		// Token: 0x04001618 RID: 5656
		[SerializeField]
		private GameObject starSquare;

		// Token: 0x04001619 RID: 5657
		[SerializeField]
		private float starSize;

		// Token: 0x0400161A RID: 5658
		[SerializeField]
		private Vector2 gridTiles;

		// Token: 0x0400161B RID: 5659
		[SerializeField]
		private float scale;

		// Token: 0x0400161C RID: 5660
		[SerializeField]
		private Vector2 initialOffset;

		// Token: 0x0400161D RID: 5661
		private Vector2 offset;

		// Token: 0x0400161E RID: 5662
		private int gridTilingIdentifier;

		// Token: 0x0400161F RID: 5663
		private int starSizeIdentifier;

		// Token: 0x04001620 RID: 5664
		private int scaleIdentifier;

		// Token: 0x04001621 RID: 5665
		private int offsetIdentifier;

		// Token: 0x04001622 RID: 5666
		private int debugIdentifier;

		// Token: 0x04001623 RID: 5667
		private Vector2 totalCameraPositionChange;

		// Token: 0x04001624 RID: 5668
		private StarLayerData data;

		// Token: 0x04001625 RID: 5669
		private bool trackCamera = true;

		// Token: 0x04001626 RID: 5670
		private RenderTexture renderTexture;
	}
}
