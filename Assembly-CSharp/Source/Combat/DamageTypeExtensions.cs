using System;
using Source.Item;
using UnityEngine;

namespace Source.Combat
{
	// Token: 0x02000132 RID: 306
	public static class DamageTypeExtensions
	{
		// Token: 0x06000B91 RID: 2961 RVA: 0x0005417C File Offset: 0x0005237C
		public static EquipStat GetDamageBoostStat(this DamageType type)
		{
			EquipStat result;
			switch (type)
			{
			case DamageType.Kinetic:
				result = EquipStat.KineticDamage;
				break;
			case DamageType.Energy:
				result = EquipStat.EnergyDamage;
				break;
			case DamageType.Radiation:
				result = EquipStat.RadiationDamage;
				break;
			case DamageType.Heat:
				result = EquipStat.HeatDamage;
				break;
			case DamageType.Cold:
				result = EquipStat.ColdDamage;
				break;
			case DamageType.Corrosion:
				result = EquipStat.CorrosionDamage;
				break;
			case DamageType.Explosive:
				result = EquipStat.ExplosiveDamage;
				break;
			default:
				throw new NotImplementedException("Nieuwe DamageType niet volledig geimplementeerd: " + type.ToString());
			}
			return result;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x000541F0 File Offset: 0x000523F0
		public static EquipStat GetResistStat(this DamageType type)
		{
			EquipStat result;
			switch (type)
			{
			case DamageType.Kinetic:
				result = EquipStat.KineticResist;
				break;
			case DamageType.Energy:
				result = EquipStat.EnergyResist;
				break;
			case DamageType.Radiation:
				result = EquipStat.RadiationResist;
				break;
			case DamageType.Heat:
				result = EquipStat.HeatResist;
				break;
			case DamageType.Cold:
				result = EquipStat.ColdResist;
				break;
			case DamageType.Corrosion:
				result = EquipStat.CorrosionResist;
				break;
			case DamageType.Explosive:
				result = EquipStat.ExplosiveResist;
				break;
			default:
				throw new NotImplementedException("Nieuwe DamageType niet volledig geimplementeerd: " + type.ToString());
			}
			return result;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00054264 File Offset: 0x00052464
		public static Color GetColor(this DamageType type)
		{
			Color result;
			switch (type)
			{
			case DamageType.Kinetic:
				result = new Color(0.8f, 0.4f, 0.4f);
				break;
			case DamageType.Energy:
				result = new Color(1f, 0.3f, 0.5f);
				break;
			case DamageType.Radiation:
				result = new Color(0.9f, 0.7f, 0.2f);
				break;
			case DamageType.Heat:
				result = new Color(1f, 0.2f, 0.1f);
				break;
			case DamageType.Cold:
				result = new Color(0.7f, 0.5f, 1f);
				break;
			case DamageType.Corrosion:
				result = new Color(0.8f, 0.5f, 0.2f);
				break;
			case DamageType.Explosive:
				result = new Color(1f, 0.5f, 0.1f);
				break;
			default:
				result = Color.red;
				break;
			}
			return result;
		}
	}
}
