using System;
using Behaviour.Weapons;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003CF RID: 975
	public class DamageOverTime : StackableEffect
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x000D13CB File Offset: 0x000CF5CB
		// (set) Token: 0x0600257C RID: 9596 RVA: 0x000D13D3 File Offset: 0x000CF5D3
		public DamageData damageTemplate { get; private set; }

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x000D13DC File Offset: 0x000CF5DC
		// (set) Token: 0x0600257E RID: 9598 RVA: 0x000D13E4 File Offset: 0x000CF5E4
		public float damagePerTick { get; private set; }

		// Token: 0x0600257F RID: 9599 RVA: 0x000D13F0 File Offset: 0x000CF5F0
		private void Start()
		{
			this.tickTimer = this.tickDelay;
			this.ticksRemaining = this.tickCount;
			if (this.damageTemplate == null)
			{
				this.SetupDamageTemplate(this.CreateDamageTemplate());
			}
			this.target = base.GetComponentInParent<TargetableUnit>();
			this.tempEffect = base.GetComponent<TemporaryEffect>();
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000D1444 File Offset: 0x000CF644
		private void Update()
		{
			this.tickTimer -= Time.deltaTime;
			if (this.tickTimer <= 0f)
			{
				this.Tick();
			}
			if (this.tempEffect)
			{
				this.tempEffect.durationRemaining = (float)this.ticksRemaining * this.tickDelay + this.tickTimer + 0.1f;
			}
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000D14A9 File Offset: 0x000CF6A9
		public void SetupDamageTemplate(DamageData damageData)
		{
			this.damageTemplate = damageData;
			if (damageData != null)
			{
				this.damagePerTick = this.CalculateDamagePerTick();
				return;
			}
			this.damagePerTick = 0f;
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x000D14CD File Offset: 0x000CF6CD
		protected virtual DamageData CreateDamageTemplate()
		{
			return null;
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000D14D0 File Offset: 0x000CF6D0
		protected virtual float CalculateDamagePerTick()
		{
			return this.damageTemplate.totalDamageAmount / (float)this.tickCount;
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000D14E8 File Offset: 0x000CF6E8
		public override void AddStack(StackableEffect other)
		{
			base.AddStack(other);
			DamageOverTime damageOverTime = other as DamageOverTime;
			if (damageOverTime != null)
			{
				this.Add(damageOverTime);
			}
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000D1510 File Offset: 0x000CF710
		public virtual void Add(DamageOverTime dot)
		{
			float num = (float)dot.tickCount * dot.damagePerTick + (float)this.ticksRemaining * this.damagePerTick;
			this.damagePerTick = num / (float)this.tickCount;
			this.ticksRemaining = this.tickCount;
			this.stackSizeAtReset = base.stackSize;
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000D1564 File Offset: 0x000CF764
		public virtual void Tick()
		{
			if (!this.target)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (this.target && !this.damageTemplate.hitTransform)
			{
				this.damageTemplate.hitTransform = this.CreateNewHitTransform(this.damageTemplate.hitCoordinates);
			}
			DamageData copy = this.damageTemplate.GetCopy(this.damagePerTick, this.damageTemplate.hitCoordinates, false);
			copy.targetUnit = this.target;
			copy.hitCoordinates = this.target.transform.position;
			this.target.TakeDamage(copy);
			this.tickTimer = this.tickDelay;
			this.ticksRemaining--;
			base.SetStackSize(Mathf.CeilToInt((float)this.ticksRemaining / (float)this.tickCount * (float)this.stackSizeAtReset));
			if (this.ticksRemaining <= 0)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x000D1666 File Offset: 0x000CF866
		private Transform CreateNewHitTransform(Vector2 localPos)
		{
			return new GameObject("TrackingTarget(DOT)")
			{
				transform = 
				{
					parent = this.target.transform,
					localPosition = localPos
				}
			}.transform;
		}

		// Token: 0x040016CB RID: 5835
		private TargetableUnit target;

		// Token: 0x040016CD RID: 5837
		public float tickDelay;

		// Token: 0x040016CE RID: 5838
		public int tickCount;

		// Token: 0x040016CF RID: 5839
		private int ticksRemaining;

		// Token: 0x040016D0 RID: 5840
		private float tickTimer;

		// Token: 0x040016D1 RID: 5841
		private int stackSizeAtReset = 1;

		// Token: 0x040016D3 RID: 5843
		private TemporaryEffect tempEffect;
	}
}
