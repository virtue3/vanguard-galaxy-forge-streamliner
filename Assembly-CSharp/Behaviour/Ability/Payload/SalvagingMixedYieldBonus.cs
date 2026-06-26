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
	// Token: 0x020003DA RID: 986
	public class SalvagingMixedYieldBonus : MonoBehaviour
	{
		// Token: 0x060025AF RID: 9647 RVA: 0x000D2216 File Offset: 0x000D0416
		public void Start()
		{
			this.CheckForMixedTurrets();
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x000D2220 File Offset: 0x000D0420
		private void CheckForMixedTurrets()
		{
			SpaceShip componentInParent = base.GetComponentInParent<SpaceShip>();
			if (componentInParent.unitData != GamePlayer.current.currentSpaceShip)
			{
				return;
			}
			SalvageModule componentInChildren = componentInParent.GetComponentInChildren<SalvageModule>();
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
			if (list.Count == 2 && SkilltreeNode.salvagingDualYieldBonus.isActive)
			{
				this.payload.gameObject.SetActive(true);
				this.payload.SetStackSize(SkilltreeNode.salvagingDualYieldBonus.currentPoints);
			}
		}

		// Token: 0x040016EF RID: 5871
		[SerializeField]
		private BoostStat payload;
	}
}
