using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001E0 RID: 480
	public class InventoryPanelHoldButton : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
	{
		// Token: 0x0600123A RID: 4666 RVA: 0x000785B8 File Offset: 0x000767B8
		private void Update()
		{
			if (this.isHolding)
			{
				this.holdProgress += Time.deltaTime / this.holdTime;
				this.buttonImage.fillAmount = this.holdProgress;
				if (this.holdProgress >= 1f)
				{
					this.isHolding = false;
					this.buttonImage.fillAmount = 1f;
					this.Trigger();
					return;
				}
			}
			else
			{
				this.buttonImage.fillAmount = 1f;
			}
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00078632 File Offset: 0x00076832
		public void OnPointerDown(PointerEventData eventData)
		{
			this.isHolding = true;
			this.holdProgress = 0f;
		}

		// Token: 0x0600123C RID: 4668 RVA: 0x00078646 File Offset: 0x00076846
		public void OnPointerUp(PointerEventData eventData)
		{
			this.isHolding = false;
			this.holdProgress = 0f;
			this.buttonImage.fillAmount = 0f;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0007866C File Offset: 0x0007686C
		private void Trigger()
		{
			switch (this.cargoSelect)
			{
			case CargoSelect.Cargo:
				this.inventoryPanel.MoveAllToCargo();
				return;
			case CargoSelect.Armory:
				this.inventoryPanel.MoveAllToArmory();
				return;
			case CargoSelect.Materials:
				this.inventoryPanel.MoveAllToMaterials();
				return;
			default:
				return;
			}
		}

		// Token: 0x04000A18 RID: 2584
		[SerializeField]
		private Image buttonImage;

		// Token: 0x04000A19 RID: 2585
		[SerializeField]
		private float holdTime = 0.5f;

		// Token: 0x04000A1A RID: 2586
		[SerializeField]
		private InventoryPanel inventoryPanel;

		// Token: 0x04000A1B RID: 2587
		[SerializeField]
		private CargoSelect cargoSelect;

		// Token: 0x04000A1C RID: 2588
		private float holdProgress;

		// Token: 0x04000A1D RID: 2589
		private bool isHolding;
	}
}
