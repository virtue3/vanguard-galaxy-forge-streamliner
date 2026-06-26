using System;
using Behaviour.Weapons;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x02000206 RID: 518
	public class TargetableHealthBar : UITooltipContent
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001358 RID: 4952 RVA: 0x0007E774 File Offset: 0x0007C974
		public override float Height
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0007E77B File Offset: 0x0007C97B
		public void SetTargetableUnit(TargetableUnit targetableUnit, TargetLayer targetLayer, bool textToggle)
		{
			this.targetable = targetableUnit;
			this.targetLayer = targetLayer;
			this.textVisible = textToggle;
			this.Update();
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0007E798 File Offset: 0x0007C998
		private void Update()
		{
			float value = (this.targetLayer == TargetLayer.Surface) ? ((float)this.targetable.currentSurfaceHealth / (float)this.targetable.maxSurfaceHealth) : ((float)this.targetable.currentCoreHealth / (float)this.targetable.maxCoreHealth);
			this.remainingHealthText.gameObject.SetActive(this.textVisible);
			if (this.textVisible)
			{
				this.remainingHealthText.text = GameMath.FormatNumber((float)((this.targetLayer == TargetLayer.Surface) ? this.targetable.currentSurfaceHealth : this.targetable.currentCoreHealth), -1);
			}
			this.fill.anchorMax = new Vector2(Mathf.Clamp01(value), 1f);
		}

		// Token: 0x04000B12 RID: 2834
		[SerializeField]
		private RectTransform fill;

		// Token: 0x04000B13 RID: 2835
		[SerializeField]
		private TextMeshProUGUI remainingHealthText;

		// Token: 0x04000B14 RID: 2836
		private TargetableUnit targetable;

		// Token: 0x04000B15 RID: 2837
		private TargetLayer targetLayer;

		// Token: 0x04000B16 RID: 2838
		private bool textVisible = true;
	}
}
