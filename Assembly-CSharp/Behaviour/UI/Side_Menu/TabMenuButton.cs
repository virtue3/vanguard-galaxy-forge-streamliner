using System;
using Behaviour.UI.Tooltip;
using Source.Util;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x020002A1 RID: 673
	public class TabMenuButton : BaseMenuButton
	{
		// Token: 0x06001919 RID: 6425 RVA: 0x0009BE50 File Offset: 0x0009A050
		protected override void OnClick()
		{
			this.TryOpenContentTab();
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0009BE58 File Offset: 0x0009A058
		public void SetContentTab(SideTabContent content)
		{
			base.SetButtonText(Translation.Translate("@SP" + content.tabName, Array.Empty<object>()));
			this.sideTabContent = content;
			if (!string.IsNullOrEmpty(content.tooltipText))
			{
				if (!this.tooltipSource)
				{
					this.tooltipSource = base.gameObject.AddComponent<TooltipSource>();
				}
				this.tooltipSource.BodyText = content.tooltipText;
			}
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0009BEC8 File Offset: 0x0009A0C8
		private void TryOpenContentTab()
		{
			if (InventoryInteractionManager.Instance.selectedItem != null)
			{
				return;
			}
			SidePanel instance = SidePanel.instance;
			if (!this.isSelected)
			{
				base.StartCoroutine(instance.sideTab.LoadContentTab(this.sideTabContent));
				instance.sideTab.lastTabMenuButton = this;
				base.SelectButton();
			}
		}

		// Token: 0x0600191C RID: 6428 RVA: 0x0009BF20 File Offset: 0x0009A120
		public void SetSelectedButton()
		{
			SidePanel.instance.sideTab.lastTabMenuButton = this;
			base.SelectButton();
		}

		// Token: 0x04000F8B RID: 3979
		public SideTabContent sideTabContent;

		// Token: 0x04000F8C RID: 3980
		public TooltipSource tooltipSource;
	}
}
