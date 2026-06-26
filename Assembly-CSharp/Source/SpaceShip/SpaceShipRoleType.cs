using System;
using Behaviour.Spacestation.Docking;
using Source.Util;
using UnityEngine;

namespace Source.SpaceShip
{
	// Token: 0x0200005F RID: 95
	[Serializable]
	public class SpaceShipRoleType
	{
		// Token: 0x0600039D RID: 925 RVA: 0x0001E028 File Offset: 0x0001C228
		public SpaceShipRole GetRole()
		{
			return this.role;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0001E030 File Offset: 0x0001C230
		public SpaceShipType GetShipType()
		{
			return this.spaceShipType;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0001E038 File Offset: 0x0001C238
		public GameplayType GetGameplayType()
		{
			GameplayType result;
			switch (this.role)
			{
			case SpaceShipRole.Combat:
				result = GameplayType.Combat;
				break;
			case SpaceShipRole.Mining:
				result = GameplayType.Mining;
				break;
			case SpaceShipRole.Salvaging:
				result = GameplayType.Salvage;
				break;
			case SpaceShipRole.Cargo:
				result = GameplayType.Cargo;
				break;
			default:
				result = GameplayType.Generic;
				break;
			}
			return result;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0001E079 File Offset: 0x0001C279
		public int GetTypeSize()
		{
			return this.spaceShipType.GetShipSizeCategory();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001E086 File Offset: 0x0001C286
		public DockingOptionSize GetDockingOptionSize()
		{
			return this.spaceShipType.GetDockingSize();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001E093 File Offset: 0x0001C293
		public string GetTypeName()
		{
			return Translation.Translate(string.Format("@ShipClass{0}{1}", this.role, this.spaceShipType), Array.Empty<object>());
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001E0BF File Offset: 0x0001C2BF
		public string GetTypeDescription()
		{
			return Translation.Translate(string.Format("@ShipClass{0}{1}Desc", this.role, this.spaceShipType), Array.Empty<object>());
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		public float GetShipRangeBonus()
		{
			float[] array = (this.role == SpaceShipRole.Combat) ? SpaceShipRoleType.CombatRangeBonus : SpaceShipRoleType.IndustrialRangeBonus;
			int num = this.spaceShipType - SpaceShipType.Size1;
			if (num < 0 || num >= array.Length)
			{
				return 0f;
			}
			return array[num];
		}

		// Token: 0x0400020A RID: 522
		private static readonly float[] CombatRangeBonus = new float[]
		{
			0f,
			0.2f,
			0.4f,
			0.7f,
			1f,
			1.2f,
			1.8f,
			2f
		};

		// Token: 0x0400020B RID: 523
		private static readonly float[] IndustrialRangeBonus = new float[]
		{
			0f,
			0.1f,
			0.2f,
			0.3f,
			0.4f,
			0.5f,
			0.7f,
			0.8f
		};

		// Token: 0x0400020C RID: 524
		[SerializeField]
		private SpaceShipRole role;

		// Token: 0x0400020D RID: 525
		[SerializeField]
		private SpaceShipType spaceShipType;
	}
}
