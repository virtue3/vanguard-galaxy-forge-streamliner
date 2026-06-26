using System;
using Behaviour.Managers;
using Behaviour.Util;
using Source.Galaxy;
using Source.Player;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D9 RID: 985
	public class SalvageCargoLootboxBot : SalvageCreditDrone
	{
		// Token: 0x060025AD RID: 9645 RVA: 0x000D21B8 File Offset: 0x000D03B8
		protected override void GainItem()
		{
			Singleton<LootManager>.Instance.CreateLootBox(MapPointOfInterest.current.level, base.transform.position);
			this.target.data.lootboxExtracted++;
			Register.AddCounter("SalvagingLootboxRetrieved", 1, 0);
		}
	}
}
