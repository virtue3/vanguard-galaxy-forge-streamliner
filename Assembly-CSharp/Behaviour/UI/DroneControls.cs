using System;
using Behaviour.Equipment.Module;
using Behaviour.Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001D6 RID: 470
	public class DroneControls : MonoBehaviour
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x060011AC RID: 4524 RVA: 0x00075610 File Offset: 0x00073810
		public DroneBayModule droneBay
		{
			get
			{
				GameplayManager instance = GameplayManager.Instance;
				if (instance == null)
				{
					return null;
				}
				SpaceShip spaceShip = instance.spaceShip;
				if (spaceShip == null)
				{
					return null;
				}
				return spaceShip.droneBayModule;
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00075630 File Offset: 0x00073830
		private void Update()
		{
			if (!this.droneBay)
			{
				return;
			}
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				this.UpdateLabel();
			}
			this.rebuildDrone.enabled = this.droneBay.rebuilding;
			if (this.droneBay.rebuilding)
			{
				this.rebuildDrone.fillAmount = this.droneBay.GetRebuildProgress();
			}
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000756B4 File Offset: 0x000738B4
		private void UpdateLabel()
		{
			if (this.droneBay.AreDronesDeployed())
			{
				this.deployReturn.TL("@UIDroneReturn", Array.Empty<object>());
			}
			else
			{
				this.deployReturn.TL("@UIDroneDeploy", Array.Empty<object>());
			}
			this.deployAlpha.alpha = (this.droneBay.CanDeployReturn() ? 1f : 0.25f);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00075720 File Offset: 0x00073920
		public void DeployReturnDrones()
		{
			if (!this.droneBay || !this.droneBay.CanDeployReturn())
			{
				return;
			}
			if (!this.droneBay.AreDronesDeployed() && !this.droneBay.BusyWithAction())
			{
				this.droneBay.DeployDrones();
				return;
			}
			if (!this.droneBay.BusyWithAction())
			{
				this.droneBay.ReturnDrones();
			}
		}

		// Token: 0x040009BD RID: 2493
		[SerializeField]
		private TMP_Text deployReturn;

		// Token: 0x040009BE RID: 2494
		[SerializeField]
		private CanvasGroup deployAlpha;

		// Token: 0x040009BF RID: 2495
		[SerializeField]
		private Image rebuildDrone;

		// Token: 0x040009C0 RID: 2496
		private float updateTimer;
	}
}
