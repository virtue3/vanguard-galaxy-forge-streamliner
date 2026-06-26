using System;
using System.Collections;
using Behaviour.Unit;
using Source.Combat;
using UnityEngine;

namespace Behaviour.Mining.AsteroidEffects
{
	// Token: 0x02000300 RID: 768
	public abstract class AsteroidEffect : MonoBehaviour
	{
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x000AAB74 File Offset: 0x000A8D74
		// (set) Token: 0x06001C41 RID: 7233 RVA: 0x000AAB7C File Offset: 0x000A8D7C
		public DamageType damageType { get; protected set; }

		// Token: 0x06001C42 RID: 7234 RVA: 0x000AAB85 File Offset: 0x000A8D85
		protected virtual void Start()
		{
			this.C2D = base.GetComponent<CircleCollider2D>();
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000AAB93 File Offset: 0x000A8D93
		public void SetEffectInterval(float effectInterval)
		{
			this.effectInterval = effectInterval;
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000AAB9C File Offset: 0x000A8D9C
		public void SetDamageType(DamageType damageType)
		{
			this.damageType = damageType;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000AABA5 File Offset: 0x000A8DA5
		public void SetColliderRadius(float radius)
		{
			this.C2D.radius = radius;
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000AABB4 File Offset: 0x000A8DB4
		private void OnTriggerStay2D(Collider2D collision)
		{
			SpaceShip component = collision.gameObject.GetComponent<SpaceShip>();
			if (component && !this.effectTriggered)
			{
				this.EffectTrigger(component);
				this.effectTriggered = true;
			}
		}

		// Token: 0x06001C47 RID: 7239
		protected abstract void EffectTrigger(SpaceShip spaceShip);

		// Token: 0x06001C48 RID: 7240 RVA: 0x000AABEB File Offset: 0x000A8DEB
		protected IEnumerator EffectCooldown()
		{
			yield return new WaitForSeconds(this.effectInterval);
			this.effectTriggered = false;
			yield break;
		}

		// Token: 0x04001190 RID: 4496
		protected CircleCollider2D C2D;

		// Token: 0x04001191 RID: 4497
		protected bool effectTriggered;

		// Token: 0x04001192 RID: 4498
		protected float effectInterval = 1f;
	}
}
