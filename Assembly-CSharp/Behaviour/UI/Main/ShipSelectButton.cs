using System;
using Behaviour.Unit;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Main
{
	// Token: 0x0200026B RID: 619
	public class ShipSelectButton : MonoBehaviour
	{
		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060016D1 RID: 5841 RVA: 0x00090F55 File Offset: 0x0008F155
		// (set) Token: 0x060016D2 RID: 5842 RVA: 0x00090F5D File Offset: 0x0008F15D
		public SpaceShip spaceShip { get; private set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060016D3 RID: 5843 RVA: 0x00090F66 File Offset: 0x0008F166
		// (set) Token: 0x060016D4 RID: 5844 RVA: 0x00090F6E File Offset: 0x0008F16E
		public string shipName { get; private set; }

		// Token: 0x060016D5 RID: 5845 RVA: 0x00090F77 File Offset: 0x0008F177
		public void SetShipName(string shipName, Action<SpaceShip> shipSelect)
		{
			this.spaceShip = SpaceShip.Get(shipName);
			this.shipNameText.text = shipName;
			this.shipSelectAction = shipSelect;
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x00090F98 File Offset: 0x0008F198
		public void OnClick()
		{
			Action<SpaceShip> action = this.shipSelectAction;
			if (action == null)
			{
				return;
			}
			action(this.spaceShip);
		}

		// Token: 0x04000E0C RID: 3596
		[SerializeField]
		private TextMeshProUGUI shipNameText;

		// Token: 0x04000E0F RID: 3599
		private Action<SpaceShip> shipSelectAction;
	}
}
