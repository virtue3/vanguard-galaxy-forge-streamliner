using System;
using System.Collections.Generic;
using Behaviour.Mining;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001C1 RID: 449
	public static class PhysicsInteraction
	{
		// Token: 0x060010A1 RID: 4257 RVA: 0x00070400 File Offset: 0x0006E600
		public static void ApplyShockwaveToNearbyShips(Vector2 center, float scale, float delay = 0.62f)
		{
			foreach (TargetableUnit targetableUnit in PhysicsInteraction.GetUnitsWithinRange(center, PhysicsInteraction.explosionImpactRange, null))
			{
				AbstractUnit abstractUnit = targetableUnit as AbstractUnit;
				if (abstractUnit != null)
				{
					Vector2 vector = abstractUnit.transform.position - center;
					Vector2 force = vector.normalized * (8f - Mathf.Clamp(vector.magnitude, 1f, 8f)) * scale * 1500f;
					abstractUnit.GiveImpulse(force, 0f, delay);
				}
				else if (targetableUnit.damagableByAll)
				{
					targetableUnit.TakeDamage(new DamageData
					{
						damageAmount = 10f
					});
				}
			}
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x000704E4 File Offset: 0x0006E6E4
		public static List<TargetableUnit> GetUnitsWithinRange(Vector2 center, float range, TargetableUnit ignoreUnit = null)
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(center, range);
			List<TargetableUnit> list = new List<TargetableUnit>();
			Collider2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				TargetableUnit targetableUnit;
				if (array2[i].TryGetComponent<TargetableUnit>(out targetableUnit) && targetableUnit != ignoreUnit)
				{
					list.Add(targetableUnit);
				}
			}
			return list;
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0007052C File Offset: 0x0006E72C
		public static List<Asteroid> GetAsteroidsWithinRange(Vector2 center, float range)
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(center, range);
			List<Asteroid> list = new List<Asteroid>();
			Collider2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Asteroid item;
				if (array2[i].TryGetComponent<Asteroid>(out item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x04000929 RID: 2345
		public static float explosionImpactRange = 8f;
	}
}
