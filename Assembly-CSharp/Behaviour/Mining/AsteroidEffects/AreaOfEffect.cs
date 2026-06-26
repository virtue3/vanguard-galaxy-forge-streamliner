using System;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Combat;

namespace Behaviour.Mining.AsteroidEffects
{
	// Token: 0x020002FF RID: 767
	public class AreaOfEffect : AsteroidEffect
	{
		// Token: 0x06001C3D RID: 7229 RVA: 0x000AAB19 File Offset: 0x000A8D19
		protected override void Start()
		{
			base.Start();
			base.damageType = DamageType.Corrosion;
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000AAB28 File Offset: 0x000A8D28
		protected override void EffectTrigger(SpaceShip spaceShip)
		{
			DamageData data = new DamageData(base.gameObject)
			{
				damageAmount = 1f,
				type = base.damageType
			};
			spaceShip.TakeDamage(data);
			base.StartCoroutine(base.EffectCooldown());
		}
	}
}
