using System;
using Behaviour.Effects;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B2 RID: 946
	public class FogEffect : AbstractEffect
	{
		// Token: 0x0600245F RID: 9311 RVA: 0x000CCE30 File Offset: 0x000CB030
		private void Start()
		{
			base.visualEffect.SetVector2("Area", this.area);
			base.visualEffect.SetFloat("FogAmount", this.fogAmount);
			base.visualEffect.SetVector2("AverageFogSpeed", this.averageFogSpeed);
			base.visualEffect.SetFloat("FogSize", this.fogSize);
			base.visualEffect.SetVector4("FogColor", this.fogColor);
			base.visualEffect.SetGradient("FogGradient", this.fogGradient);
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000CCEC6 File Offset: 0x000CB0C6
		public void SetArea(Vector2 area)
		{
			this.area = area;
			base.visualEffect.SetVector2("Area", area);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000CCEE0 File Offset: 0x000CB0E0
		public void SetFogSpeed(Vector2 speed)
		{
			this.fogSpeed = speed;
			base.visualEffect.SetFloat("MinFogSpeed", this.fogSpeed.x);
			base.visualEffect.SetFloat("MaxFogSpeed", this.fogSpeed.y);
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000CCF20 File Offset: 0x000CB120
		public void SetFogOptions(Color fogColor, Gradient fogGradient, float size, Vector2 averageSpeed)
		{
			this.fogColor = fogColor;
			this.fogGradient = fogGradient;
			this.fogSize = size;
			this.averageFogSpeed = averageSpeed;
			base.visualEffect.SetVector4("FogColor", fogColor);
			base.visualEffect.SetGradient("FogGradient", fogGradient);
			base.visualEffect.SetFloat("FogSize", size);
			base.visualEffect.SetVector2("AverageFogSpeed", averageSpeed);
			base.visualEffect.SetFloat("CameraCorrectionFactor", 30f);
		}

		// Token: 0x040015D8 RID: 5592
		[SerializeField]
		private Vector2 area;

		// Token: 0x040015D9 RID: 5593
		[SerializeField]
		private Color fogColor;

		// Token: 0x040015DA RID: 5594
		[SerializeField]
		private Vector2 averageFogSpeed;

		// Token: 0x040015DB RID: 5595
		[SerializeField]
		private float fogAmount;

		// Token: 0x040015DC RID: 5596
		[SerializeField]
		private float fogSize;

		// Token: 0x040015DD RID: 5597
		[SerializeField]
		private Gradient fogGradient;

		// Token: 0x040015DE RID: 5598
		[SerializeField]
		private Vector2 fogSpeed;
	}
}
