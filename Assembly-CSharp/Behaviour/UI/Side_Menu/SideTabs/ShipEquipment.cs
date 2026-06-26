using System;
using System.Collections;
using Behaviour.UI.Main;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002BE RID: 702
	public class ShipEquipment : SideTabContent
	{
		// Token: 0x060019D6 RID: 6614 RVA: 0x000A0CB4 File Offset: 0x0009EEB4
		private void Start()
		{
			this.ShowShips();
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x000A0CBC File Offset: 0x0009EEBC
		private void ShowShips()
		{
			this.shipSelect.SetPlayerShip();
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x000A0CC9 File Offset: 0x0009EEC9
		public IEnumerator ShowCrewTab()
		{
			yield return new WaitForSeconds(0.01f);
			this.shipSelect.ShowCrewTab();
			yield break;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x000A0CD8 File Offset: 0x0009EED8
		public void RefreshCrewOverview()
		{
			if (this.shipSelect.crewViewOpen)
			{
				this.shipSelect.PopulateCrewView();
			}
		}

		// Token: 0x04001043 RID: 4163
		[SerializeField]
		private Behaviour.UI.Main.ShipCarousel shipSelect;
	}
}
