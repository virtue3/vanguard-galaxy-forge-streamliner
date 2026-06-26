using System;
using Behaviour.Crew;
using Behaviour.Mining;
using Behaviour.Weapons;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret
{
	// Token: 0x02000344 RID: 836
	public abstract class AbstractMiningTurret : AbstractTurret
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x000BCD1B File Offset: 0x000BAF1B
		public override EquipStat powerStat
		{
			get
			{
				return EquipStat.MiningPower;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06002002 RID: 8194 RVA: 0x000BCD1F File Offset: 0x000BAF1F
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Mining;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x000BCD22 File Offset: 0x000BAF22
		public override TargetLayer targetLayer
		{
			get
			{
				return TargetLayer.Surface;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x000BCD28 File Offset: 0x000BAF28
		public float yield
		{
			get
			{
				float num = this._yield + this.GetStat(EquipStat.Yield);
				if (base.parent != GameplayManager.Instance.spaceShip)
				{
					return num;
				}
				if (GamePlayer.current.autoPlay)
				{
					float num2 = 0.8f + SkilltreeNode.PromptEngineeringYieldPenaltyReduction.currentIncrease;
					num2 += GamePlayer.current.commander.GetAutopilotPenaltyReductionModifier();
					return num * num2;
				}
				return num;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x000BCD94 File Offset: 0x000BAF94
		public Asteroid asteroidTarget
		{
			get
			{
				return base.currentTarget as Asteroid;
			}
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x000BCDA1 File Offset: 0x000BAFA1
		public virtual bool CanMineAsteroidTarget(Asteroid target)
		{
			return target && !target.IsSurfaceOreDepleted() && base.targetsSurface;
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x000BCDBE File Offset: 0x000BAFBE
		public bool HasValidTarget(Asteroid manualTarget)
		{
			return this.asteroidTarget != null && this.CanMineAsteroidTarget(manualTarget) && (!manualTarget || this.asteroidTarget == manualTarget);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000BCDEF File Offset: 0x000BAFEF
		public bool IsTargetInRange(TargetableUnit target)
		{
			return Vector2.Distance(target.transform.position, this.firePoints[this.firePointIndex].transform.position) <= base.range;
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000BCE30 File Offset: 0x000BB030
		protected override DamageData CreateDamageData(Transform targetTransform = null, Vector2? hitCoordinates = null, TargetLayer targetLayer = TargetLayer.Surface)
		{
			float num = base.GetAttackPower();
			if (base.IsPlayer(true) && GamePlayer.current.autoPlay)
			{
				float num2 = 0.8f + SkilltreeNode.PromptEngineeringMiningPowerPenaltyReduction.currentIncrease;
				num2 += GamePlayer.current.commander.GetAutopilotPenaltyReductionModifier();
				num *= num2;
			}
			return new DamageData(this)
			{
				power = num,
				criticalChance = this.GetStat(EquipStat.CriticalChance),
				type = this.damageType,
				yield = this.yield
			};
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000BCEB4 File Offset: 0x000BB0B4
		protected override void SetMainSubStats()
		{
			base.SetMainSubStats();
			if (base.targetsSurface)
			{
				this.mainSubStats.AddMainSubStat("Surface Mining", "");
			}
			if (base.targetsCore)
			{
				this.mainSubStats.AddMainSubStat("Core Mining", "");
			}
			this.mainSubStats.AddMainSubStat(GameMath.FormatPercentage(this._yield, FormatPercentageMode.Default, 1), "Yield");
		}

		// Token: 0x0400131F RID: 4895
		[SerializeField]
		public float _yield;
	}
}
