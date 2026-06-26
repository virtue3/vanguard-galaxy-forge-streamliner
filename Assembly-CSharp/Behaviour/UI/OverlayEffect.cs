using System;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F9 RID: 505
	public class OverlayEffect : MonoBehaviour
	{
		// Token: 0x060012F9 RID: 4857 RVA: 0x0007CFF8 File Offset: 0x0007B1F8
		private void Start()
		{
			this.image = base.GetComponent<Image>();
			this.shaderMaterial = this.image.material;
			this.colorIdentifier = Shader.PropertyToID("Color");
			this.shaderMaterial.SetColor(this.colorIdentifier, this.overlayColor);
			this.shaderMaterial.SetFloat("_Alpha", 0.7f);
		}

		// Token: 0x04000AB3 RID: 2739
		[SerializeField]
		private Color overlayColor;

		// Token: 0x04000AB4 RID: 2740
		private Image image;

		// Token: 0x04000AB5 RID: 2741
		private int colorIdentifier;

		// Token: 0x04000AB6 RID: 2742
		private Material shaderMaterial;

		// Token: 0x04000AB7 RID: 2743
		private float currentScannerProgress;

		// Token: 0x04000AB8 RID: 2744
		private float maxProgress;

		// Token: 0x04000AB9 RID: 2745
		private bool fadeout;

		// Token: 0x04000ABA RID: 2746
		private float fadeTime;
	}
}
