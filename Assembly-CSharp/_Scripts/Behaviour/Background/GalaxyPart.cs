using System;
using UnityEngine;

namespace _Scripts.Behaviour.Background
{
	// Token: 0x0200019D RID: 413
	public class GalaxyPart : MonoBehaviour
	{
		// Token: 0x06000E94 RID: 3732 RVA: 0x0006828C File Offset: 0x0006648C
		private void Start()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			this.spriteRenderer.material.SetInt("_GradientNumber", this.gradientNumber);
		}

		// Token: 0x04000830 RID: 2096
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000831 RID: 2097
		[SerializeField]
		private int gradientNumber;
	}
}
