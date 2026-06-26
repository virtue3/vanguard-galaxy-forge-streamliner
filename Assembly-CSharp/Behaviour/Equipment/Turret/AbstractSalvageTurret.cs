using System;
using Behaviour.Crew;
using Behaviour.Weapons;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Turret
{
	// Token: 0x02000348 RID: 840
	public abstract class AbstractSalvageTurret : AbstractTurret
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000BD526 File Offset: 0x000BB726
		public override EquipStat powerStat
		{
			get
			{
				return EquipStat.SalvagePower;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x000BD52A File Offset: 0x000BB72A
		public override GameplayType gameplayType
		{
			get
			{
				return GameplayType.Salvage;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x000BD530 File Offset: 0x000BB730
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

		// Token: 0x06002028 RID: 8232 RVA: 0x000BD59C File Offset: 0x000BB79C
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
				yield = this.yield,
				type = this.damageType,
				hitTransform = base.trackingTarget.transform,
				hitCoordinates = base.trackingTarget.transform.position,
				targetLayer = targetLayer
			};
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000BD654 File Offset: 0x000BB854
		protected override void SetMainSubStats()
		{
			base.SetMainSubStats();
			if (base.targetsSurface)
			{
				this.mainSubStats.AddMainSubStat("Surface Salvage", "");
			}
			if (base.targetsCore)
			{
				this.mainSubStats.AddMainSubStat("Structural Salvage", "");
			}
			this.mainSubStats.AddMainSubStat(GameMath.FormatPercentage(this._yield, FormatPercentageMode.Default, 1), "Yield");
		}

		// Token: 0x0400132C RID: 4908
		[SerializeField]
		public float _yield;
	}
}
