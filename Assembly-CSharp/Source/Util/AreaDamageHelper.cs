using System;
using Behaviour.Weapons;
using UnityEngine;

namespace Source.Util
{
	// Token: 0x02000024 RID: 36
	public static class AreaDamageHelper
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0000C4D8 File Offset: 0x0000A6D8
		public static DamageData CreateNewDamageData(DamageData damageData, bool canMineCore, Transform transform, float damageRadius, bool surface = true, Collider2D collider = null)
		{
			if (!damageData.source)
			{
				return null;
			}
			float num = 1f;
			if (collider)
			{
				num = AreaDamageHelper.CalculateDropoff(transform, damageRadius, collider);
			}
			return new DamageData(damageData.source)
			{
				damageAmount = num * ((canMineCore && surface) ? (damageData.damageAmount / 1.2f) : damageData.damageAmount),
				power = damageData.power,
				yield = damageData.yield * (surface ? 0.2f : 1f)
			};
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000C564 File Offset: 0x0000A764
		public static DamageData CreateNewDamageData(DamageData damageData, Transform transform, float damageRadius, Collider2D collider = null, bool dropOff = true)
		{
			if (!damageData.source)
			{
				return null;
			}
			float num = 1f;
			if (collider && dropOff)
			{
				num = AreaDamageHelper.CalculateDropoff(transform, damageRadius, collider);
			}
			return new DamageData(damageData.source)
			{
				damageAmount = num * damageData.damageAmount,
				power = damageData.power,
				type = damageData.type
			};
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		public static float CalculateDropoff(Transform transform, float damageRadius, Collider2D collider = null)
		{
			float result = 1f;
			if (collider)
			{
				float num = Mathf.Clamp01(Vector2.Distance(transform.position, collider.transform.position) / damageRadius);
				result = Mathf.Max(1f - num, 0.1f);
			}
			return result;
		}
	}
}
