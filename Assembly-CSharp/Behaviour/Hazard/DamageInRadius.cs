using System;
using Behaviour.Equipment.Turret;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.Salvage;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Combat;
using Source.Hazard;
using Source.Util;
using UnityEngine;

namespace Behaviour.Hazard
{
	// Token: 0x02000323 RID: 803
	public class DamageInRadius : LocalHazard
	{
		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x000B3371 File Offset: 0x000B1571
		public override string targetName
		{
			get
			{
				return "Hazard";
			}
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000B3378 File Offset: 0x000B1578
		protected override void Update()
		{
			this.timer += Time.deltaTime;
			this.effectTimer += Time.deltaTime;
			if (this.timer > this.triggerRate)
			{
				this.CheckForTargets();
				this.timer = 0f;
			}
			if (this.effectTimer > this.effectSpawnTime)
			{
				Singleton<EffectManager>.Instance.PlaySmokeEffect(this.source.transform.position, this.data.range * this.rangeMultiplier, new DamageType?(this.data.damageType), 8f, 5);
				this.effectTimer = 0f;
			}
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000B3428 File Offset: 0x000B1628
		protected void CheckForTargets()
		{
			foreach (TargetableUnit targetableUnit in PhysicsInteraction.GetUnitsWithinRange(base.transform.position, this.data.range * this.rangeMultiplier, null))
			{
				if (targetableUnit.gameObject != this.source && !(targetableUnit is Asteroid) && !(targetableUnit is SalvageContainer))
				{
					base.DealDamageToTarget(targetableUnit);
				}
			}
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000B34C0 File Offset: 0x000B16C0
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.source.transform.position, this.data.range);
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000B34EC File Offset: 0x000B16EC
		public override HazardData CreateData(int level, DamageType damageType)
		{
			HazardData hazardData = new HazardData();
			hazardData.range = (float)SeededRandom.Global.RandomRange(3, 5);
			hazardData.damageMultiplier = SeededRandom.Global.RandomRange(3f * GameMath.DamageMultiplier((float)level), 6f * GameMath.DamageMultiplier((float)level));
			hazardData.maxDamageFalloffPercentage = SeededRandom.Global.RandomRange(0.3f, 0.5f);
			hazardData.damageType = damageType;
			base.SetStandardFields(hazardData);
			return hazardData;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x000B3565 File Offset: 0x000B1765
		public override DamageType[] GetDamageTypes()
		{
			return new DamageType[]
			{
				DamageType.Cold,
				DamageType.Corrosion,
				DamageType.Heat,
				DamageType.Radiation
			};
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000B3578 File Offset: 0x000B1778
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return false;
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000B357B File Offset: 0x000B177B
		public override void TakeDamage(DamageData data)
		{
		}

		// Token: 0x04001220 RID: 4640
		private float triggerRate = 0.5f;

		// Token: 0x04001221 RID: 4641
		private float effectSpawnTime = 8f;

		// Token: 0x04001222 RID: 4642
		private float effectTimer = 8f;

		// Token: 0x04001223 RID: 4643
		private float timer;
	}
}
