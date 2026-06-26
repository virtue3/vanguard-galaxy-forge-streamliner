using System;
using System.Collections.Generic;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.SpaceShip;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Hangar
{
	// Token: 0x02000236 RID: 566
	public class DroneLoadoutSlot : MonoBehaviour, ITooltipTitleSource, ITooltipCustomSource, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00088B4F File Offset: 0x00086D4F
		// (set) Token: 0x0600152D RID: 5421 RVA: 0x00088B57 File Offset: 0x00086D57
		public SpaceShipData ship { get; private set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x00088B60 File Offset: 0x00086D60
		// (set) Token: 0x0600152F RID: 5423 RVA: 0x00088B68 File Offset: 0x00086D68
		public int index { get; private set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x00088B71 File Offset: 0x00086D71
		// (set) Token: 0x06001531 RID: 5425 RVA: 0x00088B79 File Offset: 0x00086D79
		public Drone selectedDrone { get; private set; }

		// Token: 0x06001532 RID: 5426 RVA: 0x00088B84 File Offset: 0x00086D84
		public void SetSlot(SpaceShipData ship, int index, bool swapAvailable)
		{
			this.ship = ship;
			this.index = index;
			this.swapAvailable = swapAvailable;
			List<Drone> droneSlots = ship.droneSlots;
			if (droneSlots == null || droneSlots.Count <= index || droneSlots[index] == null)
			{
				this.selectedDrone = null;
				this.randomIcon.gameObject.SetActive(true);
				this.icon.gameObject.SetActive(false);
				return;
			}
			this.selectedDrone = droneSlots[index];
			this.randomIcon.gameObject.SetActive(false);
			this.icon.sprite = this.selectedDrone.droneIcon;
			this.icon.gameObject.SetActive(true);
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00088C37 File Offset: 0x00086E37
		public string GetTooltipTitle()
		{
			if (this.selectedDrone)
			{
				return this.selectedDrone.displayName;
			}
			return "@UIDroneRandom";
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x00088C58 File Offset: 0x00086E58
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			if (!this.swapAvailable)
			{
				tooltip.AddTextLine("@UIDroneSwapInSS", 12, 8f);
				return;
			}
			if (this.selectedDrone)
			{
				tooltip.AddTextLine("@UIDroneUnequip", 12, 8f);
				return;
			}
			tooltip.AddTextLine("@UIDroneRandomDesc", 12, 8f);
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00088CB8 File Offset: 0x00086EB8
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.swapAvailable && eventData.button == PointerEventData.InputButton.Right && this.selectedDrone)
			{
				this.ship.droneSlots[this.index] = null;
				base.GetComponentInParent<DroneLoadoutUI>().RefreshSlots();
				UITooltip.Refresh();
			}
		}

		// Token: 0x04000C81 RID: 3201
		[SerializeField]
		private Image icon;

		// Token: 0x04000C82 RID: 3202
		[SerializeField]
		private RectTransform randomIcon;

		// Token: 0x04000C86 RID: 3206
		public bool swapAvailable;
	}
}
