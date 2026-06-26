using System;
using Behaviour.UI.Tooltip;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x0200029B RID: 667
	public class SidePanelInventoryButton : MonoBehaviour, ITooltipTextSource
	{
		// Token: 0x060018EE RID: 6382 RVA: 0x0009B3CD File Offset: 0x000995CD
		public string GetTooltipText()
		{
			return Translation.TranslateOnly("@SPInventoryDesc", new object[]
			{
				GamePlayer.current.currentSpaceShip.cargoUsed,
				GamePlayer.current.currentSpaceShip.cargoCapacity
			});
		}
	}
}
