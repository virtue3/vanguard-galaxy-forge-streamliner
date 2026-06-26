using System;
using Behaviour.Effects;
using UnityEngine;

namespace Behaviour.Background
{
	// Token: 0x020003B1 RID: 945
	public class DustEffect : AbstractEffect
	{
		// Token: 0x0600245C RID: 9308 RVA: 0x000CCD8F File Offset: 0x000CAF8F
		public void SetArea(Vector2 area)
		{
			this.area = area;
			base.visualEffect.Reinit();
			base.visualEffect.SetVector2("Area", area);
			base.visualEffect.Play();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000CCDC0 File Offset: 0x000CAFC0
		private void Start()
		{
			base.visualEffect.SetFloat("DustAmount", this.dustAmount);
			base.visualEffect.SetVector2("Area", this.area);
			base.visualEffect.SetVector2("AverageDustSpeed", this.averageDustSpeed);
			base.visualEffect.SetVector2("DustSize", this.dustSize);
		}

		// Token: 0x040015D4 RID: 5588
		[SerializeField]
		private Vector2 area;

		// Token: 0x040015D5 RID: 5589
		[SerializeField]
		private float dustAmount;

		// Token: 0x040015D6 RID: 5590
		[SerializeField]
		private Vector2 averageDustSpeed;

		// Token: 0x040015D7 RID: 5591
		[SerializeField]
		private Vector2 dustSize;
	}
}
