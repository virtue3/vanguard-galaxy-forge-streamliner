using System;
using Behaviour.Effects;
using Behaviour.Effects.Data;
using Behaviour.Weapons;
using Source.Ability;
using Source.Galaxy;
using Source.Item;
using Source.MissionSystem;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x02000366 RID: 870
	public class ShieldGeneratorModule : AbstractModule
	{
		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x000C2A30 File Offset: 0x000C0C30
		// (set) Token: 0x06002151 RID: 8529 RVA: 0x000C2A38 File Offset: 0x000C0C38
		public float regen { get; protected set; }

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x000C2A41 File Offset: 0x000C0C41
		public float modifiedRechargeRate
		{
			get
			{
				return this.rechargeRate * (1f + this.GetStat(EquipStat.ShieldRechargeRate));
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002153 RID: 8531 RVA: 0x000C2A58 File Offset: 0x000C0C58
		public float modifiedRechargeDelay
		{
			get
			{
				return this.rechargeDelay / (1f + this.GetStat(EquipStat.ShieldRechargeDelay));
			}
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06002154 RID: 8532 RVA: 0x000C2A6F File Offset: 0x000C0C6F
		public float maxShieldCapacity
		{
			get
			{
				return base.parent.maxShieldHP;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06002155 RID: 8533 RVA: 0x000C2A7C File Offset: 0x000C0C7C
		// (set) Token: 0x06002156 RID: 8534 RVA: 0x000C2A89 File Offset: 0x000C0C89
		public float currentShieldCapacity
		{
			get
			{
				return base.parent.currentShieldHP;
			}
			set
			{
				base.parent.currentShieldHP = value;
			}
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06002157 RID: 8535 RVA: 0x000C2A97 File Offset: 0x000C0C97
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.ShieldGenerator;
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000C2A9C File Offset: 0x000C0C9C
		protected override void Start()
		{
			base.Start();
			if (base.parent.inShipYard)
			{
				return;
			}
			SpriteRenderer spriteRenderer = base.parent.spriteRenderer;
			this.InitShieldEffect(spriteRenderer);
			Vector2 v = base.transform.localPosition;
			v = (Vector2)spriteRenderer.sprite.bounds.extents - spriteRenderer.sprite.pivot / spriteRenderer.sprite.pixelsPerUnit * base.transform.localScale.y;
			base.transform.localPosition = v;
			this.ResetDelayTimer();
			this.regen = this.maxShieldCapacity * 0.001f;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000C2B58 File Offset: 0x000C0D58
		protected override void Update()
		{
			if (base.parent.inShipYard)
			{
				return;
			}
			base.Update();
			if (this.rechargeDelayTimer > 0f)
			{
				this.rechargeDelayTimer -= Time.deltaTime;
			}
			if (this.rechargeDelayTimer <= 0f)
			{
				this.rechargeTickTimer -= Time.deltaTime;
				if (this.rechargeTickTimer <= 0f)
				{
					this.TickShieldRecharge();
					this.rechargeTickTimer = 0.25f;
				}
			}
			float stat = this.GetStat(EquipStat.ShieldRegen);
			if (stat > 0f)
			{
				this.currentShieldCapacity = Mathf.Min(this.currentShieldCapacity + stat * Time.deltaTime, this.maxShieldCapacity);
			}
			this.effectRespawnPosition = Mathf.Clamp(this.effectRespawnPosition + Time.deltaTime * this.effectRespawnDirection / 3f, 0f, 1f);
			this.shieldEffect.SetShieldTime(this.effectRespawnPosition, false);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000C2C48 File Offset: 0x000C0E48
		private void InitShieldEffect(SpriteRenderer parentRenderer)
		{
			this.shieldEffect = UnityEngine.Object.Instantiate<ShieldEffect>(this.shieldEffect, base.transform);
			this.shieldEffect.SetMainTexture(parentRenderer.sprite.texture);
			this.shieldEffect.SetScaleFactor(base.parent.transform.localScale / 5f);
			this.shieldEffect.Play();
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000C2CB8 File Offset: 0x000C0EB8
		private void TickShieldRecharge()
		{
			if (this.currentShieldCapacity == 0f)
			{
				this.effectRespawnDirection = 1f;
				this.shieldEffect.SetShieldTime(this.effectRespawnPosition, true);
			}
			float num = Mathf.Max(0f, this.maxShieldCapacity * 0.005f * this.modifiedRechargeRate);
			this.currentShieldCapacity = Mathf.Min(this.currentShieldCapacity + num, this.maxShieldCapacity);
			if (this.currentShieldCapacity == this.maxShieldCapacity && base.parent.faction != Faction.player && !base.parent.faction.IsEnemy(Faction.player))
			{
				MissionObjective.Trigger(MissionTrigger.FriendlyRepaired, null, null, false);
			}
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000C2D67 File Offset: 0x000C0F67
		public void ResetDelayTimer()
		{
			this.rechargeDelayTimer = this.rechargeDelay;
			this.rechargeTickTimer = 0.25f;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000C2D80 File Offset: 0x000C0F80
		public void StartRechargeImmediate()
		{
			this.rechargeDelayTimer = 0f;
			this.rechargeTickTimer = 0f;
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000C2D98 File Offset: 0x000C0F98
		public void TakeDamage(DamageData damageData)
		{
			float damageAmount = damageData.damageAmount;
			if (damageAmount > 0f)
			{
				base.parent.CheckTriggerAbility(AbilityTrigger.OnShieldDamageTaken, damageData, null);
			}
			if (this.currentShieldCapacity > damageAmount)
			{
				this.currentShieldCapacity -= damageData.damageAmount;
				damageData.absorbedByShield += damageData.damageAmount;
				damageData.damageAmount = 0f;
			}
			else
			{
				if (!this.isRecharging)
				{
					this.effectRespawnDirection = -1f;
				}
				damageData.absorbedByShield += this.currentShieldCapacity;
				damageData.damageAmount -= this.currentShieldCapacity;
				this.currentShieldCapacity = 0f;
			}
			if (damageAmount > 0f && damageData.IsCriticalHit())
			{
				base.parent.CheckTriggerAbility(AbilityTrigger.OnShieldCriticalHitTaken, damageData, null);
			}
			Vector2 position = base.transform.InverseTransformPoint(damageData.hitCoordinates);
			if (this.shieldEffect)
			{
				this.shieldEffect.SetHitsData(new ShieldHitData(position, this.maxDuration, damageData.effectColor, damageAmount / 2f, base.parent.GetShipSize()));
			}
			if (base.parent == GameplayManager.Instance.spaceShip)
			{
				float exp = Mathf.Min(damageAmount, base.parent.maxShieldHP);
				base.parent.AddDefenseMasteryXp(exp);
			}
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000C2EF4 File Offset: 0x000C10F4
		public override MainStat GetMainStat()
		{
			EquipStatLine? equipStatLine;
			float percentage = (base.GetStatLine(EquipStat.ShieldHP) != null) ? equipStatLine.GetValueOrDefault().multiplier : 1f;
			return new MainStat("Shield HP", GameMath.FormatPercentage(percentage, FormatPercentageMode.Offset, 0));
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000C2F38 File Offset: 0x000C1138
		protected override void SetMainSubStats()
		{
			if (this.rechargeRate != 1f)
			{
				this.mainSubStats.AddMainSubStat("Recharge Rate", GameMath.FormatPercentage(this.rechargeRate, FormatPercentageMode.Offset, 1));
			}
			this.mainSubStats.AddMainSubStat("Recharge Delay", this.rechargeDelay.ToString() + "s");
		}

		// Token: 0x040013C7 RID: 5063
		public const float tickDelay = 0.25f;

		// Token: 0x040013C8 RID: 5064
		public const float rechargePerTick = 0.005f;

		// Token: 0x040013C9 RID: 5065
		public float rechargeRate;

		// Token: 0x040013CA RID: 5066
		public float rechargeDelay;

		// Token: 0x040013CC RID: 5068
		private float effectRespawnPosition;

		// Token: 0x040013CD RID: 5069
		private float effectRespawnDirection = 1f;

		// Token: 0x040013CE RID: 5070
		private float maxDuration = 1f;

		// Token: 0x040013CF RID: 5071
		private bool isRecharging;

		// Token: 0x040013D0 RID: 5072
		private float rechargeDelayTimer;

		// Token: 0x040013D1 RID: 5073
		private float rechargeTickTimer;

		// Token: 0x040013D2 RID: 5074
		public float overrideBaseCapacity = -1f;

		// Token: 0x040013D3 RID: 5075
		[SerializeField]
		private Material shieldMaterial;

		// Token: 0x040013D4 RID: 5076
		[SerializeField]
		private ShieldEffect shieldEffect;
	}
}
