using System;
using Behaviour.Equipment.Turret;
using Behaviour.Tractoring;
using Behaviour.Weapons;
using Source.Data.Persistable;

namespace Behaviour.Persistables
{
	// Token: 0x020002F5 RID: 757
	public class LootBox : TractorableItem
	{
		// Token: 0x06001BA0 RID: 7072 RVA: 0x000A8158 File Offset: 0x000A6358
		public void Init(LootBoxData item)
		{
			base.data = item;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000A8161 File Offset: 0x000A6361
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return false;
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000A8164 File Offset: 0x000A6364
		public override void TakeDamage(DamageData data)
		{
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x000A8166 File Offset: 0x000A6366
		public override string targetName { get; }
	}
}
