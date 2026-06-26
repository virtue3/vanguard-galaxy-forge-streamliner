using System;
using Source.Item;
using Source.Util;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000363 RID: 867
	public class HullModule : AbstractModule
	{
		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06002136 RID: 8502 RVA: 0x000C242C File Offset: 0x000C062C
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Hull;
			}
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000C2430 File Offset: 0x000C0630
		public override MainStat GetMainStat()
		{
			EquipStatLine? equipStatLine = base.GetStatLine(EquipStat.HullHP);
			float percentage = (equipStatLine != null) ? equipStatLine.GetValueOrDefault().multiplier : 1f;
			return new MainStat("Hull HP", GameMath.FormatPercentage(percentage, FormatPercentageMode.Offset, 0));
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000C2474 File Offset: 0x000C0674
		protected override void SetMainSubStats()
		{
		}
	}
}
