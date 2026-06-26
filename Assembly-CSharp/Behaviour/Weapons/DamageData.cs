using System;
using Behaviour.Crew;
using Behaviour.Equipment.Turret;
using Behaviour.Unit;
using Source.Ability;
using Source.Combat;
using Source.Item;
using Source.SpaceShip.Auto;
using UnityEngine;

namespace Behaviour.Weapons
{
	// Token: 0x020001A9 RID: 425
	public class DamageData
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x00069176 File Offset: 0x00067376
		public float totalDamageAmount
		{
			get
			{
				return this.CalculateDamage(false).originalAmount;
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00069184 File Offset: 0x00067384
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x00069192 File Offset: 0x00067392
		public float damageAmount
		{
			get
			{
				return this.CalculateDamage(false).damageAmount;
			}
			set
			{
				this.CalculateDamage(false).damageAmount = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x000691A1 File Offset: 0x000673A1
		public int critCount
		{
			get
			{
				return this.CalculateDamage(false).critCount;
			}
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x000691AF File Offset: 0x000673AF
		public DamageData(AbstractTurret turret) : this(turret.gameObject)
		{
			this.sourceTurret = turret;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000691C4 File Offset: 0x000673C4
		public DamageData(GameObject source)
		{
			this.yield = 0.6f;
			this.damageForceMovement = true;
			this.source = source;
			this.sourceUnit = source.GetComponentInParent<AbstractUnit>();
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x000691F1 File Offset: 0x000673F1
		public DamageData(AbstractUnit parent)
		{
			this.yield = 0.6f;
			this.damageForceMovement = true;
			this.source = parent.gameObject;
			this.sourceUnit = parent;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0006921E File Offset: 0x0006741E
		public DamageData()
		{
			this.yield = 0.6f;
			this.damageForceMovement = true;
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00069238 File Offset: 0x00067438
		public DamageData GetCopy(float damage, Vector2 hitCoordinates, bool includeTurret = true)
		{
			DamageData damageData;
			if (includeTurret && this.sourceTurret != null)
			{
				damageData = new DamageData(this.sourceTurret);
			}
			else if (this.source)
			{
				damageData = new DamageData(this.source);
			}
			else
			{
				damageData = new DamageData();
			}
			damageData.power = this.power;
			damageData.type = this.type;
			damageData.yield = this.yield;
			damageData.criticalChance = this.criticalChance;
			damageData.targetUnit = this.targetUnit;
			damageData.hitTransform = this.hitTransform;
			damageData.hitCoordinates = hitCoordinates;
			damageData.effectColor = this.effectColor;
			damageData.isDamageOverTime = this.isDamageOverTime;
			damageData.isCoreDamage = this.isCoreDamage;
			damageData.OverrideDamageAmount(damage);
			return damageData;
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00069300 File Offset: 0x00067500
		public DamageData.DamageCalculation CalculateDamage(bool recalc = false)
		{
			if (this.calculation == null || recalc)
			{
				float num = this.power / 5f;
				num *= SeededRandom.Global.RandomRange(0.8f, 1.25f);
				if (this.sourceTurret != null)
				{
					num /= this.sourceTurret.defaultAttacksPerSecond;
				}
				int num2 = 0;
				float num3 = this.criticalChance;
				while (SeededRandom.Global.RandomBool(num3))
				{
					num2++;
					num3 *= 0.5f;
					if (num2 > SkilltreeNode.combatMegaCrit.currentPoints)
					{
						break;
					}
				}
				if (num2 > 0)
				{
					float num4 = 2f;
					if (this.sourceTurret != null)
					{
						num4 += this.sourceTurret.GetStat(EquipStat.CriticalDamage);
					}
					else if (this.sourceUnit != null)
					{
						num4 += this.sourceUnit.GetStat(EquipStat.CriticalDamage);
					}
					num *= Mathf.Pow(num4, (float)num2);
				}
				EquipStat damageBoostStat = this.type.GetDamageBoostStat();
				if (this.sourceTurret != null)
				{
					num *= 1f + this.sourceTurret.GetStat(damageBoostStat) + this.sourceTurret.GetStat(EquipStat.Damage);
				}
				else if (this.sourceUnit != null)
				{
					num *= 1f + this.sourceUnit.GetStat(damageBoostStat) + this.sourceUnit.GetStat(EquipStat.Damage);
				}
				this.calculation = new DamageData.DamageCalculation(num, num2);
			}
			return this.calculation;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0006944A File Offset: 0x0006764A
		public void OverrideDamageAmount(float dmg)
		{
			this.CalculateDamage(false).OverrideDamageAmount(dmg);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x00069459 File Offset: 0x00067659
		public bool IsCriticalHit()
		{
			return this.critCount > 0;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00069464 File Offset: 0x00067664
		public void PreDamageEvents()
		{
			if (this.sourceUnit)
			{
				this.sourceUnit.CheckTriggerAbility(AbilityTrigger.BeforeDamageDealt, this, null);
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00069484 File Offset: 0x00067684
		public void PostDamageEvents()
		{
			if (this.sourceUnit)
			{
				this.sourceUnit.CheckTriggerAbility(AbilityTrigger.OnDamageDealt, this, null);
				if (this.IsCriticalHit())
				{
					this.sourceUnit.CheckTriggerAbility(AbilityTrigger.OnCriticalHit, this, null);
				}
				CombatActions combatActions = this.sourceUnit.autoActions as CombatActions;
				if (combatActions != null)
				{
					AbstractUnit abstractUnit = this.targetUnit as AbstractUnit;
					if (abstractUnit != null)
					{
						combatActions.threat.Add(abstractUnit, this.totalDamageAmount / 10f);
					}
				}
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00069500 File Offset: 0x00067700
		public void CreateHitTransform(Transform hit)
		{
			GameObject gameObject = new GameObject("Hit Target");
			gameObject.transform.parent = hit;
			gameObject.transform.position = this.hitCoordinates;
			UnityEngine.Object.Destroy(gameObject, 5f);
			this.hitTransform = gameObject.transform;
		}

		// Token: 0x0400086C RID: 2156
		public const float PowerPerDamage = 5f;

		// Token: 0x0400086D RID: 2157
		public readonly AbstractUnit sourceUnit;

		// Token: 0x0400086E RID: 2158
		public readonly GameObject source;

		// Token: 0x0400086F RID: 2159
		public readonly AbstractTurret sourceTurret;

		// Token: 0x04000870 RID: 2160
		public float power;

		// Token: 0x04000871 RID: 2161
		public DamageType type;

		// Token: 0x04000872 RID: 2162
		public float yield;

		// Token: 0x04000873 RID: 2163
		public float criticalChance;

		// Token: 0x04000874 RID: 2164
		public TargetableUnit targetUnit;

		// Token: 0x04000875 RID: 2165
		public Transform hitTransform;

		// Token: 0x04000876 RID: 2166
		public Vector2 hitCoordinates;

		// Token: 0x04000877 RID: 2167
		public Color effectColor;

		// Token: 0x04000878 RID: 2168
		public bool isDamageOverTime;

		// Token: 0x04000879 RID: 2169
		public bool isCoreDamage;

		// Token: 0x0400087A RID: 2170
		public TargetLayer targetLayer;

		// Token: 0x0400087B RID: 2171
		private DamageData.DamageCalculation calculation;

		// Token: 0x0400087C RID: 2172
		public float absorbedByShield;

		// Token: 0x0400087D RID: 2173
		public float absorbedByArmor;

		// Token: 0x0400087E RID: 2174
		public bool reflectedDamage;

		// Token: 0x0400087F RID: 2175
		public bool damageForceMovement;

		// Token: 0x020004DB RID: 1243
		public class DamageCalculation
		{
			// Token: 0x17000632 RID: 1586
			// (get) Token: 0x06002A30 RID: 10800 RVA: 0x000EAC77 File Offset: 0x000E8E77
			// (set) Token: 0x06002A31 RID: 10801 RVA: 0x000EAC7F File Offset: 0x000E8E7F
			public float originalAmount { get; private set; }

			// Token: 0x06002A32 RID: 10802 RVA: 0x000EAC88 File Offset: 0x000E8E88
			public DamageCalculation(float damageAmount, int critCount)
			{
				this.originalAmount = damageAmount;
				this.damageAmount = damageAmount;
				this.critCount = critCount;
			}

			// Token: 0x06002A33 RID: 10803 RVA: 0x000EACA5 File Offset: 0x000E8EA5
			public void OverrideDamageAmount(float damage)
			{
				this.damageAmount = damage;
				this.originalAmount = damage;
			}

			// Token: 0x04001A46 RID: 6726
			public float damageAmount;

			// Token: 0x04001A47 RID: 6727
			public int critCount;
		}
	}
}
