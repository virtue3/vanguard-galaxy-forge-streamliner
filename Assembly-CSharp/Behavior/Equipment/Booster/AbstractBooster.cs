using System;
using Behaviour.Equipment;
using Source.Item;

namespace Behavior.Equipment.Booster
{
	// Token: 0x02000195 RID: 405
	public abstract class AbstractBooster : AbstractEquipment
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x000677F4 File Offset: 0x000659F4
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Booster;
			}
		}
	}
}
