using System;
using Behaviour.Transparency;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B9 RID: 953
	public class StarLayerOld : AbstractBackgroundLayer, ICameraTrackable
	{
		// Token: 0x06002499 RID: 9369 RVA: 0x000CE4DC File Offset: 0x000CC6DC
		protected override void Awake()
		{
			base.Awake();
			this.gridTilingIdentifier = Shader.PropertyToID("_TilingVector");
			this.starSizeIdentifier = Shader.PropertyToID("_StarSize");
			this.scaleIdentifier = Shader.PropertyToID("_Scale");
			this.offsetIdentifier = Shader.PropertyToID("_Offset");
			this.debugIdentifier = Shader.PropertyToID("_Debug");
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000CE53F File Offset: 0x000CC73F
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

		// Token: 0x0600249B RID: 9371 RVA: 0x000CE568 File Offset: 0x000CC768
		protected override void SetSpriteRenderer()
		{
			this.spriteRenderer = this.starSquare.GetComponent<SpriteRenderer>();
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x000CE57B File Offset: 0x000CC77B
		public override void SetCamera()
		{
			this.cameraTracker.SetCamera(Camera.main);
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x000CE590 File Offset: 0x000CC790
		public override void SetScreenSize(Vector2 screenSize, Vector2 screenSizeGame)
		{
			base.SetScreenSize(screenSize, screenSizeGame);
			float num = this.gridTiles.y / this.gridTiles.x;
			this.scaleWithFactor = new Vector2(screenSize.x, screenSize.x * num);
			this.starSquare.transform.localScale = this.scaleWithFactor;
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x000CE5F4 File Offset: 0x000CC7F4
		private void SetPropertiesOnMaterial()
		{
			this.materialCopy.SetVector(this.gridTilingIdentifier, this.gridTiles);
			this.materialCopy.SetFloat(this.starSizeIdentifier, this.starSize);
			this.materialCopy.SetFloat(this.scaleIdentifier, this.scale);
			this.materialCopy.SetVector(this.offsetIdentifier, this.offset);
			this.materialCopy.SetInt(this.debugIdentifier, 0);
			this.starSquare.transform.localScale = this.gridTiles;
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000CE694 File Offset: 0x000CC894
		private void SetOffset(Vector2 offset)
		{
			this.offset = offset;
			this.materialCopy.SetVector(this.offsetIdentifier, offset);
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000CE6B4 File Offset: 0x000CC8B4
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
			base.gameObject.transform.position = newPosition;
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000CE781 File Offset: 0x000CC981
		public void SetLayer(int layer)
		{
			base.gameObject.layer = layer;
			this.starSquare.layer = layer;
			this.trackCamera = false;
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000CE7A4 File Offset: 0x000CC9A4
		public void SetData(StarLayerData data, bool map, Vector2 screenSize)
		{
			this.data = data;
			this.initialOffset = data.offset;
			this.scale = data.scale;
			this.gridTiles = data.gridTiles;
			if (!map && ScreenSettings.fullscreen && !(this.gridTiles.y / this.gridTiles.x).ApproximatelyEqual(screenSize.y / screenSize.x))
			{
				this.gridTiles.y = screenSize.y / screenSize.x * this.gridTiles.x;
			}
			this.SetPropertiesOnMaterial();
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000CE83B File Offset: 0x000CCA3B
		public StarLayerData GetData()
		{
			if (this.data == null)
			{
				this.data = new StarLayerData();
			}
			this.SetPropertiesOnData();
			return this.data;
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000CE85C File Offset: 0x000CCA5C
		private void SetPropertiesOnData()
		{
			this.data.starSize = this.starSize;
			this.data.gridTiles = this.gridTiles;
			this.data.scale = this.scale;
			this.data.offset = this.initialOffset;
		}

		// Token: 0x04001627 RID: 5671
		[SerializeField]
		private GameObject starSquare;

		// Token: 0x04001628 RID: 5672
		[SerializeField]
		private float starSize;

		// Token: 0x04001629 RID: 5673
		[SerializeField]
		private Vector2 gridTiles;

		// Token: 0x0400162A RID: 5674
		[SerializeField]
		private float scale;

		// Token: 0x0400162B RID: 5675
		[SerializeField]
		private Vector2 initialOffset;

		// Token: 0x0400162C RID: 5676
		private Vector2 offset;

		// Token: 0x0400162D RID: 5677
		private int gridTilingIdentifier;

		// Token: 0x0400162E RID: 5678
		private int starSizeIdentifier;

		// Token: 0x0400162F RID: 5679
		private int scaleIdentifier;

		// Token: 0x04001630 RID: 5680
		private int offsetIdentifier;

		// Token: 0x04001631 RID: 5681
		private int debugIdentifier;

		// Token: 0x04001632 RID: 5682
		private Vector2 totalCameraPositionChange;

		// Token: 0x04001633 RID: 5683
		private StarLayerData data;

		// Token: 0x04001634 RID: 5684
		private bool trackCamera = true;
	}
}
