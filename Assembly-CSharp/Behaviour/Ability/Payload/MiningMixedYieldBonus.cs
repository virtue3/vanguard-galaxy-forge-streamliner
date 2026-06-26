using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Aspect;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Player;
using UnityEngine;

namespace Behaviour.Ability.Payload
{
	// Token: 0x020003D5 RID: 981
	public class MiningMixedYieldBonus : MonoBehaviour
	{
		// Token: 0x06002598 RID: 9624 RVA: 0x000D1A61 File Offset: 0x000CFC61
		public void Start()
		{
			this.CheckForMixedTurrets();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000D1A6C File Offset: 0x000CFC6C
		private void CheckForMixedTurrets()
		{
			SpaceShip componentInParent = base.GetComponentInParent<SpaceShip>();
			if (componentInParent.unitData != GamePlayer.current.currentSpaceShip)
			{
				return;
			}
			MiningModule componentInChildren = componentInParent.GetComponentInChildren<MiningModule>();
			if (componentInChildren == null)
			{
				return;
			}
			List<TargetLayer> list = new List<TargetLayer>();
			foreach (AbstractTurret abstractTurret in componentInChildren.turrets)
			{
				if (!list.Contains(abstractTurret.targetLayer))
				{
					list.Add(abstractTurret.targetLayer);
				}
			}
			if (list.Count == 2 && SkilltreeNode.miningDualYieldBonus.isActive)
			{
				this.payload.gameObject.SetActive(true);
				this.payload.SetStackSize(SkilltreeNode.miningDualYieldBonus.currentPoints);
			}
		}

		// Token: 0x040016DE RID: 5854
		[SerializeField]
		private BoostStat payload;
	}
}
