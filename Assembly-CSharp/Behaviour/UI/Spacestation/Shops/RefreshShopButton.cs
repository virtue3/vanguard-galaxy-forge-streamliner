using System;
using Behaviour.UI.Timer;
using Behaviour.UI.Tooltip;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Shops
{
	// Token: 0x0200021E RID: 542
	public class RefreshShopButton : MonoBehaviour, ITooltipCustomSource
	{
		// Token: 0x0600141E RID: 5150 RVA: 0x00081B13 File Offset: 0x0007FD13
		public void Init(InventoryShop parent, ReputationLevel reputationLevel)
		{
			this.inventoryShop = parent;
			this.refreshTokens = reputationLevel.GetShopRefreshTokens();
			reputationLevel.CanRefreshShop();
			this.SetTimer();
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x00081B35 File Offset: 0x0007FD35
		private void SetTimer()
		{
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x00081B37 File Offset: 0x0007FD37
		public void TryRefresh()
		{
			if (this.refreshTokens <= 0)
			{
				return;
			}
			this.Refresh();
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x00081B49 File Offset: 0x0007FD49
		public void Refresh()
		{
			this.inventoryShop.RefreshShop();
			this.refreshTokens--;
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00081B64 File Offset: 0x0007FD64
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddTextLine("Refresh Shop", 12, 8f);
			tooltip.AddTextLine("Refresh tokens: " + this.refreshTokens.ToString(), 12, 8f).Text.color = ColorHelper.boringGrey;
			if (this.refreshTokens > 0)
			{
				tooltip.AddTextLine(Translation.Translate("@RepRefreshLeftClick", Array.Empty<object>()), 12, 8f);
			}
		}

		// Token: 0x04000BA0 RID: 2976
		private InventoryShop inventoryShop;

		// Token: 0x04000BA1 RID: 2977
		[SerializeField]
		private CountdownTimer reputationRefresh;

		// Token: 0x04000BA2 RID: 2978
		private int refreshTokens = 1;
	}
}
