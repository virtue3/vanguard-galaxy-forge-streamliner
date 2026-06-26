using System;
using System.Linq;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Ability;
using Source.Combat;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Module
{
	// Token: 0x0200035F RID: 863
	public class ArmorModule : AbstractModule
	{
		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000BFBA9 File Offset: 0x000BDDA9
		public float maxArmor
		{
			get
			{
				return base.parent.maxArmorHP;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x000BFBB8 File Offset: 0x000BDDB8
		// (set) Token: 0x060020CB RID: 8395 RVA: 0x000BFBE0 File Offset: 0x000BDDE0
		public float currentArmor
		{
			get
			{
				AbstractUnit parent = base.parent;
				if (parent != null)
				{
					return parent.unitData.currentArmorHP;
				}
				return 0f;
			}
			set
			{
				AbstractUnit parent = base.parent;
				if (parent != null)
				{
					parent.unitData.currentArmorHP = value;
				}
			}
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000BFC04 File Offset: 0x000BDE04
		protected override void Update()
		{
			base.Update();
			float stat = this.GetStat(EquipStat.ArmorRegen);
			if (stat > 0f)
			{
				this.currentArmor = Mathf.Min(this.currentArmor + stat * Time.deltaTime, base.parent.maxArmorHP);
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x000BFC4C File Offset: 0x000BDE4C
		public override EquipmentSlot slot
		{
			get
			{
				return EquipmentSlot.Armor;
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000BFC50 File Offset: 0x000BDE50
		public void TakeDamage(DamageData damageData)
		{
			if (this.weakTypes.Contains(damageData.type))
			{
				damageData.damageAmount *= 1f + this.weakAmount;
			}
			else
			{
				damageData.damageAmount *= 1f - this.resistAmount;
			}
			float damageAmount = damageData.damageAmount;
			if (damageAmount > 0f)
			{
				base.parent.CheckTriggerAbility(AbilityTrigger.OnArmorDamageTaken, damageData, null);
			}
			if (this.currentArmor > damageData.damageAmount)
			{
				this.currentArmor -= damageData.damageAmount;
				damageData.absorbedByArmor += damageData.damageAmount;
				damageData.damageAmount = 0f;
			}
			else
			{
				damageData.absorbedByArmor += this.currentArmor;
				damageData.damageAmount -= this.currentArmor;
				this.currentArmor = 0f;
			}
			if (base.parent == GameplayManager.Instance.spaceShip)
			{
				float exp = Mathf.Min(damageAmount, base.parent.maxArmorHP);
				base.parent.AddDefenseMasteryXp(exp);
			}
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000BFD6C File Offset: 0x000BDF6C
		public override MainStat GetMainStat()
		{
			EquipStatLine? equipStatLine = base.GetStatLine(EquipStat.ArmorHP);
			float percentage = (equipStatLine != null) ? equipStatLine.GetValueOrDefault().multiplier : 1f;
			return new MainStat("Armor HP", GameMath.FormatPercentage(percentage, FormatPercentageMode.Offset, 0));
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x000BFDB0 File Offset: 0x000BDFB0
		protected override void SetMainSubStats()
		{
			this.mainSubStats.AddMainSubStat("Damage resistance", GameMath.FormatPercentage(this.resistAmount, FormatPercentageMode.Default, 1));
			this.mainSubStats.AddMainSubStat(string.Join<DamageType>('/', this.weakTypes) + " vulnerability", GameMath.FormatPercentage(this.weakAmount, FormatPercentageMode.Default, 1));
		}

		// Token: 0x0400138D RID: 5005
		public DamageType[] weakTypes;

		// Token: 0x0400138E RID: 5006
		public float resistAmount;

		// Token: 0x0400138F RID: 5007
		public float weakAmount;

		// Token: 0x04001390 RID: 5008
		public float overrideBaseCapacity = -1f;
	}
}
