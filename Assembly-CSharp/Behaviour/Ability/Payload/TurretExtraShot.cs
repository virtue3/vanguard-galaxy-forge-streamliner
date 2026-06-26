using System;
using Behaviour.Equipment.Turret;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003DB RID: 987
	public class TurretExtraShot : TriggeredPayload
	{
		// Token: 0x060025B2 RID: 9650 RVA: 0x000D22DC File Offset: 0x000D04DC
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			AbstractTurret abstractTurret = source as AbstractTurret;
			if (abstractTurret != null)
			{
				abstractTurret.FireExtraShot();
			}
		}
	}
}
