using System;
using System.Collections.Generic;
using Source.Item;

namespace Behaviour.Unit
{
	// Token: 0x020001C0 RID: 448
	public interface IEquipStatSource
	{
		// Token: 0x0600109E RID: 4254
		IEnumerable<EquipStatLine> GetStats();

		// Token: 0x0600109F RID: 4255
		EquipStatLine? GetStatLine(EquipStat stat);

		// Token: 0x060010A0 RID: 4256
		string GetName();
	}
}
