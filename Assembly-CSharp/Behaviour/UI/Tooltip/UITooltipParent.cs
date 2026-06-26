using System;
using UnityEngine;

namespace Behaviour.UI.Tooltip
{
	// Token: 0x0200020D RID: 525
	public class UITooltipParent : MonoBehaviour
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001377 RID: 4983 RVA: 0x0007EA5F File Offset: 0x0007CC5F
		// (set) Token: 0x06001378 RID: 4984 RVA: 0x0007EA66 File Offset: 0x0007CC66
		public static UITooltip TooltipPrefab { get; private set; }

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001379 RID: 4985 RVA: 0x0007EA6E File Offset: 0x0007CC6E
		// (set) Token: 0x0600137A RID: 4986 RVA: 0x0007EA75 File Offset: 0x0007CC75
		public static UITooltip ItemTooltipPrefab { get; private set; }

		// Token: 0x0600137B RID: 4987 RVA: 0x0007EA7D File Offset: 0x0007CC7D
		private void Awake()
		{
			UITooltipParent.TooltipPrefab = this._tooltipPrefab;
			UITooltipParent.ItemTooltipPrefab = this._itemTooltipPrefab;
			UITooltip.SetupTooltipContext(base.transform as RectTransform);
		}

		// Token: 0x04000B20 RID: 2848
		[SerializeField]
		private UITooltip _tooltipPrefab;

		// Token: 0x04000B21 RID: 2849
		[SerializeField]
		private UITooltip _itemTooltipPrefab;
	}
}
