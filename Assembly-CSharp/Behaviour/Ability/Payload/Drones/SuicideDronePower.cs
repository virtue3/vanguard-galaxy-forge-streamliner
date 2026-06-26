using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.UI.HUD;
using Behaviour.Unit;
using UnityEngine;

namespace Behaviour.Ability.Payload.Drones
{
	// Token: 0x020003DF RID: 991
	public class SuicideDronePower : MonoBehaviour
	{
		// Token: 0x060025BD RID: 9661 RVA: 0x000D254C File Offset: 0x000D074C
		private void Start()
		{
			SpaceShip componentInParent = base.GetComponentInParent<SpaceShip>();
			if (componentInParent == null || componentInParent.droneBayModule == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			List<Drone> list = (from d in componentInParent.droneBayModule.drones
			where d.isDeployed
			select d).ToList<Drone>();
			if (list.Count < this.droneToSuicide)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			foreach (Drone drone in (from d in list
			orderby UnityEngine.Random.value
			select d).Take(this.droneToSuicide).ToList<Drone>())
			{
				drone.OnDestruction();
				HudManager.Instance.RemoveHealthBar(drone);
			}
		}

		// Token: 0x040016F5 RID: 5877
		[SerializeField]
		private int droneToSuicide = 2;
	}
}
