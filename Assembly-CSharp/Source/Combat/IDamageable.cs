using System;
using Behaviour.Unit;
using Behaviour.Weapons;

namespace Source.Combat
{
	// Token: 0x02000133 RID: 307
	public interface IDamageable
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000B94 RID: 2964
		bool enabled { get; }

		// Token: 0x06000B95 RID: 2965
		void TakeDamage(DamageData damageData);

		// Token: 0x06000B96 RID: 2966
		bool IsEnemy(AbstractUnit unit);
	}
}
