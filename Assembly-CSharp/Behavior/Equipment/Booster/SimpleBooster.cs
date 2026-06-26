using System;
using System.Collections.Generic;
using Behaviour.Equipment;
using Source.Item;
using Source.Util;

namespace Behavior.Equipment.Booster
{
	// Token: 0x02000196 RID: 406
	public class SimpleBooster : AbstractBooster
	{
		// Token: 0x06000E70 RID: 3696 RVA: 0x00067800 File Offset: 0x00065A00
		public override MainStat GetMainStat()
		{
			using (List<EquipStatLine>.Enumerator enumerator = base.stats.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					EquipStatLine equipStatLine = enumerator.Current;
					return new MainStat(equipStatLine.stat.GetDisplayName(), equipStatLine.ToString(false));
				}
			}
			return null;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x0006786C File Offset: 0x00065A6C
		protected override void SetMainSubStats()
		{
		}
	}
}
