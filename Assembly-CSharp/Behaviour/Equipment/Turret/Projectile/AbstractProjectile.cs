using System;
using Behaviour.Managers;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret.Projectile
{
	// Token: 0x0200034D RID: 845
	public abstract class AbstractProjectile : MonoBehaviour
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x000BDE8B File Offset: 0x000BC08B
		protected virtual bool destroyOnHit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000BDE90 File Offset: 0x000BC090
		protected virtual void Awake()
		{
			this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			CircleCollider2D component = base.GetComponent<CircleCollider2D>();
			if (component)
			{
				this.hitboxOffset = component.offset.x;
				this.hitboxSize = component.radius;
				return;
			}
			this.hitboxSize = 0.1f;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000BDEE1 File Offset: 0x000BC0E1
		protected virtual void Start()
		{
			if (this.destroyOnHit)
			{
				UnityEngine.Object.Destroy(base.gameObject, this.lifetime);
			}
			base.transform.Z(ZIndex.Projectile);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000BDF08 File Offset: 0x000BC108
		public void Initialize(DamageData damageData, float projectileSpeed)
		{
			this.speed = projectileSpeed + this.speedBonus;
			this.damageData = damageData;
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000BDF20 File Offset: 0x000BC120
		protected virtual void Update()
		{
			base.transform.Translate(Vector3.right * this.speed * Time.deltaTime);
			if (!this.damageData.sourceUnit)
			{
				return;
			}
			foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(base.transform.position + base.transform.forward * this.hitboxOffset, this.hitboxSize))
			{
				if (this.IsTarget(collider2D))
				{
					RaycastHit2D hit = Physics2D.Raycast(base.transform.position, base.transform.right, 1f);
					if (hit)
					{
						base.transform.position = hit.point;
					}
					this.damageData.hitCoordinates = base.transform.position;
					this.damageData.CreateHitTransform(collider2D.transform);
					this.OnHit(collider2D);
					Singleton<EffectManager>.Instance.PlayFlashEffect(base.transform.position, this.damageData.effectColor, (float)(this.damageData.IsCriticalHit() ? 20 : 15));
					Singleton<EffectManager>.Instance.PlaySmokeEffect(base.transform.position, this.damageData.IsCriticalHit() ? 0.2f : 0.13f, new DamageType?(this.damageData.type), 1f, 15);
					if (this.destroyOnHit)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					return;
				}
			}
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000BE0D4 File Offset: 0x000BC2D4
		protected virtual bool IsTarget(Collider2D collider)
		{
			IDamageable damageable;
			return collider.TryGetComponent<IDamageable>(out damageable) && damageable.enabled && damageable.IsEnemy(this.damageData.sourceUnit);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000BE106 File Offset: 0x000BC306
		public void SetTargetUnit(TargetableUnit target)
		{
			this.damageData.targetUnit = target;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000BE114 File Offset: 0x000BC314
		protected virtual void OnDestroy()
		{
			TrailRenderer componentInChildren = base.GetComponentInChildren<TrailRenderer>();
			if (componentInChildren && BasePoiManager.current)
			{
				Vector3 position = componentInChildren.transform.position;
				componentInChildren.transform.parent = BasePoiManager.current.transform;
				componentInChildren.transform.position = position;
				UnityEngine.Object.Destroy(componentInChildren.gameObject, componentInChildren.time + 0.1f);
			}
		}

		// Token: 0x06002053 RID: 8275
		protected abstract void OnHit(Collider2D collision);

		// Token: 0x04001345 RID: 4933
		[SerializeField]
		protected float speedBonus;

		// Token: 0x04001346 RID: 4934
		[SerializeField]
		protected float lifetime = 5f;

		// Token: 0x04001347 RID: 4935
		protected float speed;

		// Token: 0x04001348 RID: 4936
		protected SpriteRenderer spriteRenderer;

		// Token: 0x04001349 RID: 4937
		protected DamageData damageData;

		// Token: 0x0400134A RID: 4938
		protected float hitboxOffset;

		// Token: 0x0400134B RID: 4939
		protected float hitboxSize;
	}
}
