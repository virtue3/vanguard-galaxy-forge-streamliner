using System;
using System.Collections.Generic;
using Source.Galaxy;
using Source.SpaceShip.Auto;
using UnityEngine;

namespace Behaviour.Unit.Parts
{
	// Token: 0x020001CC RID: 460
	public class TauntPylonAggro : MonoBehaviour
	{
		// Token: 0x06001165 RID: 4453 RVA: 0x00073713 File Offset: 0x00071913
		private void Start()
		{
			this.turretPart = base.GetComponent<DefensiveTurret>();
			base.InvokeRepeating("DoTaunt", 0.5f, 0.5f);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x00073738 File Offset: 0x00071938
		private void DoTaunt()
		{
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, 50f);
			List<AbstractUnit> list = new List<AbstractUnit>();
			foreach (Collider2D collider2D in array)
			{
				Debug.Log(collider2D.gameObject);
				AbstractUnit abstractUnit;
				if (collider2D.gameObject.TryGetComponent<AbstractUnit>(out abstractUnit) && abstractUnit.autoActions is CombatActions)
				{
					Faction faction = abstractUnit.faction;
					bool flag;
					if (faction == null)
					{
						flag = true;
					}
					else
					{
						DefensiveTurret defensiveTurret = this.turretPart;
						flag = faction.IsEnemy((defensiveTurret != null) ? defensiveTurret.faction : null);
					}
					if (flag)
					{
						list.Add(abstractUnit);
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			SeededRandom.Global.Shuffle<AbstractUnit>(list);
			((CombatActions)list[0].autoActions).TauntedBy(this.turretPart);
		}

		// Token: 0x04000989 RID: 2441
		public const float TickDelay = 0.5f;

		// Token: 0x0400098A RID: 2442
		public const float ScannerRange = 50f;

		// Token: 0x0400098B RID: 2443
		private DefensiveTurret turretPart;
	}
}
