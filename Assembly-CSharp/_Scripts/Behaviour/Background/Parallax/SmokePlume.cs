using System;
using UnityEngine;

namespace _Scripts.Behaviour.Background.Parallax
{
	// Token: 0x020001A0 RID: 416
	public class SmokePlume : MonoBehaviour
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x000689B4 File Offset: 0x00066BB4
		// (set) Token: 0x06000EA9 RID: 3753 RVA: 0x000689BC File Offset: 0x00066BBC
		public Color color { get; private set; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000EAA RID: 3754 RVA: 0x000689C5 File Offset: 0x00066BC5
		// (set) Token: 0x06000EAB RID: 3755 RVA: 0x000689CD File Offset: 0x00066BCD
		public int tile { get; private set; }

		// Token: 0x06000EAC RID: 3756 RVA: 0x000689D6 File Offset: 0x00066BD6
		private void Awake()
		{
			this.renderer = base.GetComponent<SpriteRenderer>();
			this.colorIdentifier = Shader.PropertyToID("_Color");
			this.tileIdentifier = Shader.PropertyToID("_TexIndex");
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00068A04 File Offset: 0x00066C04
		private void OnEnable()
		{
			this.renderer.material.SetColor(this.colorIdentifier, this.color);
			this.renderer.material.SetInt(this.tileIdentifier, this.tile);
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00068A3E File Offset: 0x00066C3E
		public void SetColor(Color color)
		{
			this.color = color;
			SpriteRenderer spriteRenderer = this.renderer;
			if (spriteRenderer == null)
			{
				return;
			}
			spriteRenderer.material.SetColor(this.colorIdentifier, color);
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00068A63 File Offset: 0x00066C63
		public void SetTile(int tile)
		{
			this.tile = tile;
			SpriteRenderer spriteRenderer = this.renderer;
			if (spriteRenderer == null)
			{
				return;
			}
			spriteRenderer.material.SetInt(this.tileIdentifier, tile);
		}

		// Token: 0x04000846 RID: 2118
		private SpriteRenderer renderer;

		// Token: 0x04000847 RID: 2119
		private int colorIdentifier;

		// Token: 0x04000848 RID: 2120
		private int tileIdentifier;
	}
}
