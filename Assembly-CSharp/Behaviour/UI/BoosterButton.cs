using System;
using Source.Item;
using Source.SpaceShip;
using Source.Util;

namespace Behaviour.UI
{
	// Token: 0x020001EA RID: 490
	public class BoosterButton : EquipmentButton
	{
		// Token: 0x0600129F RID: 4767 RVA: 0x00079D4B File Offset: 0x00077F4B
		protected override void SetHoveringButton(bool toggle)
		{
			InventoryInteractionManager.Instance.hoveringBoosterButton = (toggle ? this : null);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x00079D60 File Offset: 0x00077F60
		protected override void AssignSlot<T>(T slot)
		{
			SpaceShipBooster spaceShipBooster = slot as SpaceShipBooster;
			if (spaceShipBooster != null)
			{
				this.spaceShipBooster = spaceShipBooster;
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00079D83 File Offset: 0x00077F83
		public override string GetSlotText<T>(T slot)
		{
			return Translation.Translate("@SPEquipEmpty", new object[]
			{
				EquipmentSlot.Booster
			});
		}

		// Token: 0x04000A6B RID: 2667
		public SpaceShipBooster spaceShipBooster;

		// Token: 0x04000A6C RID: 2668
		public int index;
	}
}
