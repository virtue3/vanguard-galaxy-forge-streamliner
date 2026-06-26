using System;
using System.Collections.Generic;
using Behaviour.Weapons;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000360 RID: 864
	public class CargoScoopModule : AbstractTargetingModule
	{
		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x000BFE1C File Offset: 0x000BE01C
		protected override TargetingPriority baseTargetPriority
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000BFE23 File Offset: 0x000BE023
		public override void SetManualTarget(TargetableUnit manualTarget)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000BFE2A File Offset: 0x000BE02A
		public override void UpdateAvailableTargets(IEnumerable<TargetableUnit> targets)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000BFE31 File Offset: 0x000BE031
		public override MainStat GetMainStat()
		{
			return new MainStat("Power", 0f);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000BFE42 File Offset: 0x000BE042
		protected override void SetMainSubStats()
		{
		}
	}
}
