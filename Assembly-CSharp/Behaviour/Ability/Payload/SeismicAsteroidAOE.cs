using System;
using System.Linq;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D6 RID: 982
	public class SeismicAsteroidAOE : MonoBehaviour
	{
		// Token: 0x0600259B RID: 9627 RVA: 0x000D1B28 File Offset: 0x000CFD28
		private void Start()
		{
			this.CreateDamageData();
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000D1B30 File Offset: 0x000CFD30
		private void CreateDamageData()
		{
			SpaceShip componentInParent = base.GetComponentInParent<SpaceShip>();
			this.damageData = new DamageData(componentInParent.gameObject)
			{
				power = 1.4f * componentInParent.GetNormalizedPower(EquipStat.MiningPower),
				type = DamageType.Heat,
				yield = 0.8f + componentInParent.GetStat(EquipStat.Yield)
			};
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x000D1B84 File Offset: 0x000CFD84
		private void Update()
		{
			this.timer -= Time.deltaTime;
			if (this.timer <= 0f)
			{
				this.PlayImpactEffect();
				this.Explode();
				this.timer = this.damageInterval;
			}
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000D1BBD File Offset: 0x000CFDBD
		private void PlayImpactEffect()
		{
			Singleton<EffectManager>.Instance.PlayShockwaveExplosionEffect(base.transform.position, this.damageRadius / 30f, 0f);
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x000D1BEC File Offset: 0x000CFDEC
		private void Explode()
		{
			if (!this.damageData.source)
			{
				return;
			}
			foreach (Collider2D collider2D in (from collider in Physics2D.OverlapCircleAll(base.transform.position, this.damageRadius)
			orderby (collider.transform.position - base.transform.position).sqrMagnitude
			select collider).ToArray<Collider2D>())
			{
				Asteroid component = collider2D.GetComponent<Asteroid>();
				if (component != null)
				{
					Asteroid asteroid = component;
					DamageData damageData = AreaDamageHelper.CreateNewDamageData(this.damageData, false, base.transform, this.damageRadius, true, collider2D);
					UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(collider2D, damageData, base.name, base.transform);
					if (!asteroid.IsSurfaceOreDepleted())
					{
						asteroid.TakeSurfaceDamage(damageData);
					}
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x040016DF RID: 5855
		[SerializeField]
		private float damageRadius = 8f;

		// Token: 0x040016E0 RID: 5856
		[SerializeField]
		private float damageInterval = 2f;

		// Token: 0x040016E1 RID: 5857
		private DamageData damageData;

		// Token: 0x040016E2 RID: 5858
		private float timer;
	}
}
