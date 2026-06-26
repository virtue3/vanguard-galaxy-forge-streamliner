using System;
using System.Collections.Generic;
using Behaviour.Ability;
using Behaviour.Crew;
using Behaviour.Unit;
using Source.Crew;
using Source.Item;
using Source.Util;
using UnityEngine;

namespace Behaviour.Equipment.Aspect
{
	// Token: 0x02000378 RID: 888
	public class BoostStat : StackableEffect, IEquipStatSource
	{
		// Token: 0x0600224D RID: 8781 RVA: 0x000C6F77 File Offset: 0x000C5177
		public IEnumerable<EquipStatLine> GetStats()
		{
			return this.GetStats(base.stackSize);
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000C6F88 File Offset: 0x000C5188
		public EquipStatLine? GetStatLine(EquipStat stat)
		{
			if (this.stat != stat)
			{
				return null;
			}
			return new EquipStatLine?(new EquipStatLine(stat, this.GetStatBoost(base.stackSize), this.GetStatMultiplier(base.stackSize), true));
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000C6FCC File Offset: 0x000C51CC
		public string GetName()
		{
			SkilltreeNode skilltreeNode;
			if (base.TryGetComponent<SkilltreeNode>(out skilltreeNode))
			{
				return skilltreeNode.displayName;
			}
			EquipAspect equipAspect;
			if (base.TryGetComponent<EquipAspect>(out equipAspect))
			{
				return Translation.Translate("@Aspect" + base.name.Replace("(Clone)", ""), Array.Empty<object>());
			}
			BonusBadge bonusBadge;
			if (base.TryGetComponent<BonusBadge>(out bonusBadge) && bonusBadge.bonusType == BonusType.HullBonus)
			{
				return Translation.Translate("@" + base.name.Replace("(Clone)", ""), Array.Empty<object>());
			}
			MasteryBonus masteryBonus;
			if (base.TryGetComponent<MasteryBonus>(out masteryBonus))
			{
				return Translation.Translate("@" + base.name.Replace("(Clone)", ""), Array.Empty<object>());
			}
			Milestone milestone;
			if (base.TryGetComponent<Milestone>(out milestone))
			{
				return Translation.Translate("@" + base.name.Replace("(Clone)", ""), Array.Empty<object>());
			}
			return base.name.Replace("(Clone)", "");
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000C70D8 File Offset: 0x000C52D8
		public IEnumerable<EquipStatLine> GetStats(int stackSize = 1)
		{
			yield return new EquipStatLine(this.stat, this.GetStatBoost(stackSize), this.GetStatMultiplier(stackSize), true);
			yield break;
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000C70EF File Offset: 0x000C52EF
		public float GetStatBoost(int stackSize = 1)
		{
			return this.statBoost * (float)stackSize;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000C70FA File Offset: 0x000C52FA
		public float GetStatMultiplier(int stackSize = 1)
		{
			return 1f + (this.statMultiplier - 1f) * (float)stackSize;
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000C7114 File Offset: 0x000C5314
		public float SetStatBoost(float boost)
		{
			this.statBoost = boost;
			return boost;
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000C712B File Offset: 0x000C532B
		public void SetStat(EquipStat equipstat)
		{
			this.stat = equipstat;
		}

		// Token: 0x04001447 RID: 5191
		[SerializeField]
		private float statBoost;

		// Token: 0x04001448 RID: 5192
		[SerializeField]
		private float statMultiplier = 1f;

		// Token: 0x04001449 RID: 5193
		[SerializeField]
		private EquipStat stat;
	}
}
