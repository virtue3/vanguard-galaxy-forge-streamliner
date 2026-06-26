using System;
using Behaviour.Crew;
using Behaviour.Equipment.Aspect;
using Behaviour.Item;
using Behaviour.Unit;
using Behaviour.Weapons;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Ability.Payload.Combat
{
	// Token: 0x020003E1 RID: 993
	public class LoneTurret : MonoBehaviour
	{
		// Token: 0x060025C1 RID: 9665 RVA: 0x000D26A2 File Offset: 0x000D08A2
		public void Start()
		{
			this.CheckForLoneTurret();
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000D26AC File Offset: 0x000D08AC
		private void CheckForLoneTurret()
		{
			SpaceShip componentInParent = base.GetComponentInParent<SpaceShip>();
			if (componentInParent.unitData != GamePlayer.current.currentSpaceShip)
			{
				return;
			}
			int num = 0;
			foreach (InventoryItemType inventoryItemType in componentInParent.unitData.hardpoints)
			{
				if (num > 1)
				{
					return;
				}
				if (inventoryItemType && inventoryItemType.gameplayType == GameplayType.Combat)
				{
					num++;
				}
			}
			if (componentInParent.droneBayModule && componentInParent.droneBayModule.HasLoadout(GameplayType.Combat, TargetLayer.Both, null))
			{
				return;
			}
			this.payload.gameObject.SetActive(true);
			this.payload.SetStackSize(SkilltreeNode.combatLoneTurret.currentPoints);
		}

		// Token: 0x040016F7 RID: 5879
		[SerializeField]
		private BoostStat payload;
	}
}
