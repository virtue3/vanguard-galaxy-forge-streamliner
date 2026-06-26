using System;
using System.Linq;
using Behaviour.Managers;
using Behaviour.UI.HUD;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Ability;
using Source.Combat;
using Source.Data;
using Source.Drone;
using Source.SpaceShip;
using Source.Util;
using UnityEngine;

namespace Behaviour.Unit
{
	// Token: 0x020001C8 RID: 456
	public class Torpedo : AbstractUnit
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x00073051 File Offset: 0x00071251
		// (set) Token: 0x06001146 RID: 4422 RVA: 0x00073059 File Offset: 0x00071259
		public AbstractUnit torpedoCommander { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x00073062 File Offset: 0x00071262
		// (set) Token: 0x06001148 RID: 4424 RVA: 0x0007306A File Offset: 0x0007126A
		public TorpedoData torpedoData { get; private set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x00073073 File Offset: 0x00071273
		public override string targetName
		{
			get
			{
				return "@Torpedo";
			}
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x0007307C File Offset: 0x0007127C
		protected override void Start()
		{
			base.Start();
			base.engine.SetNoSlowdown();
			CircleCollider2D component = base.GetComponent<CircleCollider2D>();
			if (component)
			{
				this.hitboxOffset = component.offset.x;
				this.hitboxSize = component.radius;
				return;
			}
			this.hitboxSize = 0.1f;
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x000730D2 File Offset: 0x000712D2
		protected override void Awake()
		{
			base.Awake();
			UnityEngine.Object.Destroy(base.gameObject, 20f);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x000730EA File Offset: 0x000712EA
		private void OnEnable()
		{
			if (!base.surfaceSprite)
			{
				base.CloneBaseSprite();
			}
		}

		// Token: 0x0600114D RID: 4429 RVA: 0x00073100 File Offset: 0x00071300
		protected override void Update()
		{
			base.Update();
			if (this.target)
			{
				base.SetOverrideDestination(this.target.transform.position, false, false, false);
			}
			else
			{
				this.PlayImpactEffect(true);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			foreach (Collider2D collider2D in Physics2D.OverlapCircleAll(base.transform.position + base.transform.forward * this.hitboxOffset, this.hitboxSize))
			{
				if (collider2D.gameObject.GetComponent<AbstractUnit>() && this.IsTarget(collider2D))
				{
					this.damageData.CalculateDamage(false);
					this.damageData.hitCoordinates = base.transform.position;
					this.damageData.CreateHitTransform(collider2D.transform);
					IDamageable component = collider2D.GetComponent<IDamageable>();
					if (component != null)
					{
						component.TakeDamage(this.damageData);
					}
					this.PlayImpactEffect(true);
					this.Explode(this.damageData);
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x0600114E RID: 4430 RVA: 0x0007322C File Offset: 0x0007142C
		private bool IsTarget(Collider2D collider)
		{
			IDamageable damageable;
			return collider.TryGetComponent<IDamageable>(out damageable) && damageable.enabled && damageable.IsEnemy(this.damageData.sourceUnit);
		}

		// Token: 0x0600114F RID: 4431 RVA: 0x0007325E File Offset: 0x0007145E
		protected override AutoActions CreateAutoActions()
		{
			return null;
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00073261 File Offset: 0x00071461
		public override void CheckTriggerAbility(AbilityTrigger trigger, object source, AbstractUnit triggeredBySubordinate)
		{
			this.torpedoCommander.CheckTriggerAbility(trigger, source, triggeredBySubordinate);
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00073274 File Offset: 0x00071474
		public override void SetData(AbstractUnitData abstractUnitData, bool setUnitRef = true, bool applyBattleDamage = false)
		{
			base.SetData(abstractUnitData, setUnitRef, applyBattleDamage);
			TorpedoData torpedoData = abstractUnitData as TorpedoData;
			if (torpedoData != null)
			{
				this.torpedoData = torpedoData;
			}
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x0007329B File Offset: 0x0007149B
		public void SetCommander(AbstractUnit spaceship)
		{
			this.torpedoCommander = spaceship;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000732A4 File Offset: 0x000714A4
		public void SetTarget(TargetableUnit target)
		{
			this.target = target;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000732AD File Offset: 0x000714AD
		public void SetDamageData(DamageData damageData)
		{
			this.damageData = damageData;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x000732B6 File Offset: 0x000714B6
		public void Initialize(Vector2 initialSpeed)
		{
			base.rigidbody.linearVelocity = initialSpeed;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x000732C4 File Offset: 0x000714C4
		private void PlayImpactEffect(bool normal = true)
		{
			if (!Singleton<EffectManager>.Instance || this.damageData == null)
			{
				return;
			}
			Singleton<EffectManager>.Instance.PlayExplosionEffect(base.transform.position, Vector2.zero, normal ? 0.8f : 0.2f, ColorHelper.red90, 0f);
			Singleton<EffectManager>.Instance.PlaySmokeEffect(base.transform.position, this.damageData.IsCriticalHit() ? 0.2f : 0.13f, new DamageType?(this.damageData.type), 1f, 15);
			PhysicsInteraction.ApplyShockwaveToNearbyShips(base.transform.position, 0.6f, 0f);
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00073388 File Offset: 0x00071588
		private void Explode(DamageData newDamageData)
		{
			if (!newDamageData.source)
			{
				return;
			}
			foreach (Collider2D collider2D in (from collider in Physics2D.OverlapCircleAll(base.transform.position, this.explosionRadius)
			orderby (collider.transform.position - base.transform.position).sqrMagnitude
			select collider).ToArray<Collider2D>())
			{
				AbstractUnit abstractUnit;
				if (collider2D.TryGetComponent<AbstractUnit>(out abstractUnit) && this.IsTarget(collider2D))
				{
					DamageData damageData = AreaDamageHelper.CreateNewDamageData(newDamageData, base.transform, this.explosionRadius, collider2D, true);
					UnityEngine.Object obj = AsteroidHelper.SetTrackingTargetData(collider2D, damageData, base.name, base.transform);
					abstractUnit.TakeDamage(damageData);
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00073431 File Offset: 0x00071631
		protected override void OnDestroy()
		{
			base.OnDestroy();
			HudManager instance = HudManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.RemoveHealthBar(this);
		}

		// Token: 0x04000971 RID: 2417
		public TargetableUnit target;

		// Token: 0x04000972 RID: 2418
		protected float hitboxOffset;

		// Token: 0x04000973 RID: 2419
		protected float hitboxSize;

		// Token: 0x04000974 RID: 2420
		private DamageData damageData;

		// Token: 0x04000975 RID: 2421
		[SerializeField]
		private float explosionRadius = 6f;
	}
}
