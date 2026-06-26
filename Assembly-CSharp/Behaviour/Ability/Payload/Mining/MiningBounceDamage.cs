using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Mining;
using Behaviour.Weapons;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Behaviour.Ability.Payload.Mining
{
	// Token: 0x020003DE RID: 990
	public class MiningBounceDamage : TriggeredPayload
	{
		// Token: 0x060025B9 RID: 9657 RVA: 0x000D23D3 File Offset: 0x000D05D3
		public override bool ShouldTrigger(object source)
		{
			return ((DamageData)source).targetUnit is Asteroid;
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x000D23EC File Offset: 0x000D05EC
		public override void PayloadTriggered(object source, int stackSize = 1)
		{
			DamageData damageData = (DamageData)source;
			DamageData copy = damageData.GetCopy(damageData.totalDamageAmount * this.damagePercentage, damageData.hitCoordinates, false);
			copy.type = DamageType.Radiation;
			List<Collider2D> list = (from collider in Physics2D.OverlapCircleAll(base.transform.position, this.damageRadius)
			orderby (collider.transform.position - base.transform.position).sqrMagnitude
			select collider).ToList<Collider2D>();
			int num = 0;
			foreach (Collider2D collider2D in list)
			{
				Asteroid component = collider2D.GetComponent<Asteroid>();
				if (component != null && component != damageData.targetUnit)
				{
					copy.targetUnit = component;
					UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(collider2D, copy, base.name, base.transform);
					if (!component.IsSurfaceOreDepleted())
					{
						component.TakeSurfaceDamage(copy);
						num++;
					}
					UnityEngine.Object.Destroy(obj);
					if (num == this.bounces)
					{
						break;
					}
				}
			}
		}

		// Token: 0x040016F2 RID: 5874
		[SerializeField]
		private float damagePercentage = 0.2f;

		// Token: 0x040016F3 RID: 5875
		[SerializeField]
		private float damageRadius = 8f;

		// Token: 0x040016F4 RID: 5876
		[SerializeField]
		private int bounces = 1;
	}
}
