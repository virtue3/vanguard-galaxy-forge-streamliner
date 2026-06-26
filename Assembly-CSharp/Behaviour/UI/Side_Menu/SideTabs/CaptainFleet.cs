using System;
using System.Collections.Generic;
using Source.Player;
using Source.SpaceShip;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu.SideTabs
{
	// Token: 0x020002A4 RID: 676
	public class CaptainFleet : SideTabContent
	{
		// Token: 0x06001930 RID: 6448 RVA: 0x0009C98A File Offset: 0x0009AB8A
		public void GetAllShips()
		{
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x0009C98C File Offset: 0x0009AB8C
		private void Start()
		{
			List<string> list = new List<string>();
			foreach (SpaceShipData spaceShipData in GamePlayer.current.spaceShips)
			{
				list.Add(spaceShipData.shipClass);
			}
			this.shipSelectionDropdown.ClearOptions();
			this.shipSelectionDropdown.AddOptions(list);
			this.shipSelectionDropdown.SetValueWithoutNotify(GamePlayer.current.spaceShips.IndexOf(GamePlayer.current.currentSpaceShip));
			this.ShowShipData();
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x0009CA34 File Offset: 0x0009AC34
		public void ShowShipData()
		{
			SpaceShipData spaceShipData = GamePlayer.current.spaceShips[this.shipSelectionDropdown.value];
			Debug.Log("Showing shipdata: " + spaceShipData.shipClass);
		}

		// Token: 0x04000FA3 RID: 4003
		[SerializeField]
		private TMP_Dropdown shipSelectionDropdown;
	}
}
