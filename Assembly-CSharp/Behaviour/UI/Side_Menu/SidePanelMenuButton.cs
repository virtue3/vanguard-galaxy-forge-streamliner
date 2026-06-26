using System;
using Behaviour.UI.Tooltip;
using Source.Util;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x0200029C RID: 668
	public class SidePanelMenuButton : BaseMenuButton, ITooltipTitleSource
	{
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x0009B415 File Offset: 0x00099615
		// (set) Token: 0x060018F1 RID: 6385 RVA: 0x0009B41D File Offset: 0x0009961D
		public SidePanel.SideTabType menuTab { get; private set; }

		// Token: 0x060018F2 RID: 6386 RVA: 0x0009B426 File Offset: 0x00099626
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x0009B42E File Offset: 0x0009962E
		protected override void OnClick()
		{
			base.OnClick();
			SidePanel.instance.ToggleTab(this.menuTab);
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x0009B448 File Offset: 0x00099648
		public string GetTooltipTitle()
		{
			string str = "";
			if (this.menuTab == SidePanel.SideTabType.Inventory)
			{
				str = " [I]";
			}
			else if (this.menuTab == SidePanel.SideTabType.Map)
			{
				str = " [M]";
			}
			return Translation.Translate("@SP" + this.menuTab.ToString(), Array.Empty<object>()) + str;
		}
	}
}
