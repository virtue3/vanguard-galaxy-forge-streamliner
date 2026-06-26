using System;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003AC RID: 940
	public abstract class AbstractBackgroundLayer : MonoBehaviour
	{
		// Token: 0x06002446 RID: 9286 RVA: 0x000CC4A4 File Offset: 0x000CA6A4
		protected virtual void Awake()
		{
			this.SetSpriteRenderer();
			this.initialBounds = this.spriteRenderer.bounds.size;
			this.cameraTracker = base.GetComponent<CameraTracker>();
			this.materialCopy = new Material(this.material);
			this.SetCamera();
			if (this.screenSize != Vector2.zero)
			{
				Debug.Log("Calling setScreenSize from Awake");
				this.SetScreenSize(this.screenSize, this.screenSizeGame);
			}
		}

		// Token: 0x06002447 RID: 9287
		protected abstract void SetSpriteRenderer();

		// Token: 0x06002448 RID: 9288
		public abstract void SetCamera();

		// Token: 0x06002449 RID: 9289 RVA: 0x000CC526 File Offset: 0x000CA726
		public virtual void SetScreenSize(Vector2 screenSize, Vector2 screenSizeGame)
		{
			this.SetSpriteRenderer();
			this.screenSize = screenSize;
			this.screenSizeGame = screenSizeGame;
			this.scaleWithFactor = this.screenSize;
		}

		// Token: 0x04001598 RID: 5528
		protected Vector2 screenSize;

		// Token: 0x04001599 RID: 5529
		protected Vector2 screenSizeGame;

		// Token: 0x0400159A RID: 5530
		protected Vector2 scaleWithFactor;

		// Token: 0x0400159B RID: 5531
		protected Vector2 initialBounds;

		// Token: 0x0400159C RID: 5532
		protected SpriteRenderer spriteRenderer;

		// Token: 0x0400159D RID: 5533
		[SerializeField]
		protected Material material;

		// Token: 0x0400159E RID: 5534
		protected Material materialCopy;

		// Token: 0x0400159F RID: 5535
		protected CameraTracker cameraTracker;
	}
}
