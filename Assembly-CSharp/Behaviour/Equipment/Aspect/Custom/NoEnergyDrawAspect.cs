using System;

namespace Behaviour.Equipment.Aspect.Custom
{
	// Token: 0x0200037D RID: 893
	public class NoEnergyDrawAspect : EquipAspect
	{
		// Token: 0x06002266 RID: 8806 RVA: 0x000C7395 File Offset: 0x000C5595
		public override void Initialize(AbstractEquipment equipment)
		{
			equipment.capacityCost = 0f;
		}
	}
}
