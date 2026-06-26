using System;
using Behaviour.Unit;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class ZoomSwitchFiltering : MonoBehaviour
{
	// Token: 0x06000031 RID: 49 RVA: 0x00002FF4 File Offset: 0x000011F4
	private void Start()
	{
		this.sprites = base.GetComponentsInChildren<SpriteRenderer>();
		SpaceShip spaceShip;
		if (base.TryGetComponent<SpaceShip>(out spaceShip) && spaceShip.inShipYard)
		{
			this.SetFilterMode(FilterMode.Point);
			return;
		}
		if (GameplayManager.Instance && GameplayManager.Instance.cameraMovement)
		{
			GameplayManager.Instance.cameraMovement.AddZoomSwitchFiltering(this);
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003054 File Offset: 0x00001254
	public void SetFilterMode(FilterMode mode)
	{
		for (int i = 0; i < this.sprites.Length; i++)
		{
			if (this.sprites[i] && this.sprites[i].sprite)
			{
				this.sprites[i].sprite.texture.filterMode = mode;
				if (this.sprites[i].sprite.GetSecondaryTextureCount() == 1)
				{
					this.sprites[i].sprite.GetSecondaryTextures(this.normalMap);
					this.normalMap[0].texture.filterMode = mode;
				}
			}
		}
	}

	// Token: 0x0400002F RID: 47
	private SpriteRenderer[] sprites;

	// Token: 0x04000030 RID: 48
	private SecondarySpriteTexture[] normalMap = new SecondarySpriteTexture[1];
}
