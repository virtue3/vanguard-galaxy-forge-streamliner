using System;

namespace Source.Item
{
	// Token: 0x020000F1 RID: 241
	public static class EquipStatExtensions
	{
		// Token: 0x0600090B RID: 2315 RVA: 0x00046724 File Offset: 0x00044924
		public static bool IsPercentageStat(this EquipStat s)
		{
			switch (s)
			{
			case EquipStat.ShieldRechargeDelay:
				return true;
			case EquipStat.ShieldGateTime:
				return true;
			case EquipStat.DamageReduction:
				return true;
			case EquipStat.KineticResist:
				return true;
			case EquipStat.EnergyResist:
				return true;
			case EquipStat.RadiationResist:
				return true;
			case EquipStat.HeatResist:
				return true;
			case EquipStat.ColdResist:
				return true;
			case EquipStat.CorrosionResist:
				return true;
			case EquipStat.ExplosiveResist:
				return true;
			case EquipStat.CriticalChance:
				return true;
			case EquipStat.CriticalDamage:
				return true;
			case EquipStat.Damage:
				return true;
			case EquipStat.WeaponRange:
				return true;
			case EquipStat.AttackSpeed:
				return true;
			case EquipStat.ReloadSpeed:
				return true;
			case EquipStat.MagazineSize:
				return true;
			case EquipStat.TurretRotationSpeed:
				return true;
			case EquipStat.ProjectileSpeed:
				return true;
			case EquipStat.KineticDamage:
				return true;
			case EquipStat.EnergyDamage:
				return true;
			case EquipStat.RadiationDamage:
				return true;
			case EquipStat.HeatDamage:
				return true;
			case EquipStat.ColdDamage:
				return true;
			case EquipStat.CorrosionDamage:
				return true;
			case EquipStat.ExplosiveDamage:
				return true;
			case EquipStat.SideThrust:
				return true;
			case EquipStat.RotationalThrust:
				return true;
			case EquipStat.Yield:
				return true;
			}
			return false;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0004686C File Offset: 0x00044A6C
		public static bool IsPowerStat(this EquipStat s)
		{
			switch (s)
			{
			case EquipStat.Power:
				return true;
			case EquipStat.CombatPower:
				return true;
			case EquipStat.MiningPower:
				return true;
			case EquipStat.SalvagePower:
				return true;
			case EquipStat.DronePower:
				return true;
			case EquipStat.TorpedoPower:
				return true;
			}
			return false;
		}
	}
}
