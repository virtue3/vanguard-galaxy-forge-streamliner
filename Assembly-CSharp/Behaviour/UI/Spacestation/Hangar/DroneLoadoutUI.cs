using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Module;
using Behaviour.UI.Main;
using Behaviour.UI.Spacestation.Location;
using Behaviour.Unit;
using Source.Item;
using Source.Player;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Hangar
{
	// Token: 0x02000237 RID: 567
	public class DroneLoadoutUI : MonoBehaviour
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x00088D12 File Offset: 0x00086F12
		// (set) Token: 0x06001538 RID: 5432 RVA: 0x00088D19 File Offset: 0x00086F19
		public static DroneLoadoutUI instance { get; private set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x00088D21 File Offset: 0x00086F21
		public static bool active
		{
			get
			{
				return DroneLoadoutUI.instance && DroneLoadoutUI.instance.gameObject.activeSelf;
			}
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x00088D40 File Offset: 0x00086F40
		private void OnEnable()
		{
			DroneLoadoutUI.instance = this;
			if (!this.parent || this.parent.selectedShipData == null || this.parent.selectedShipData.GetEquippedItem(EquipmentSlot.DroneBay) == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.swapAvailable = ((this.parent.droneSwapAvailable && SpaceStationInterior.instance) || SkilltreeNode.dronesChangeDronesInField.isActive);
			this.parent.selectedShipData.CheckDroneLoadout();
			this.slots = new List<DroneLoadoutSlot>();
			this.slotParent.DestroyChildren();
			DroneBayModule component = this.parent.selectedShipData.GetEquippedItem(EquipmentSlot.DroneBay).GetComponent<DroneBayModule>();
			for (int i = 0; i < component.droneAmount; i++)
			{
				DroneLoadoutSlot droneLoadoutSlot = UnityEngine.Object.Instantiate<DroneLoadoutSlot>(this.slotPrefab, this.slotParent);
				droneLoadoutSlot.SetSlot(this.parent.selectedShipData, i, this.swapAvailable);
				this.slots.Add(droneLoadoutSlot);
			}
			this.optionsParent.DestroyChildren();
			foreach (Drone dr in GamePlayer.current.GetUnlockedDrones())
			{
				UnityEngine.Object.Instantiate<DroneLoadoutOption>(this.optionPrefab, this.optionsParent).SetDrone(dr, this.swapAvailable);
			}
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x00088EB4 File Offset: 0x000870B4
		public void RefreshSlots()
		{
			if (!PersonalHangar.current)
			{
				GameplayManager.Instance.spaceShip.droneBayModule.RebuildDroneLoadout();
			}
			foreach (DroneLoadoutSlot droneLoadoutSlot in this.slots)
			{
				droneLoadoutSlot.SetSlot(droneLoadoutSlot.ship, droneLoadoutSlot.index, this.swapAvailable);
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x00088F38 File Offset: 0x00087138
		public void AddDroneToLoadout(Drone dr)
		{
			foreach (DroneLoadoutSlot droneLoadoutSlot in this.slots)
			{
				if (droneLoadoutSlot.selectedDrone == null)
				{
					if (this.parent.selectedShipData.droneSlots.Count > droneLoadoutSlot.index)
					{
						this.parent.selectedShipData.droneSlots[droneLoadoutSlot.index] = dr;
					}
					else
					{
						this.parent.selectedShipData.droneSlots.Add(dr);
					}
					this.RefreshSlots();
					break;
				}
			}
		}

		// Token: 0x04000C88 RID: 3208
		[SerializeField]
		private Behaviour.UI.Main.ShipCarousel parent;

		// Token: 0x04000C89 RID: 3209
		[SerializeField]
		private RectTransform slotParent;

		// Token: 0x04000C8A RID: 3210
		[SerializeField]
		private DroneLoadoutSlot slotPrefab;

		// Token: 0x04000C8B RID: 3211
		[SerializeField]
		private RectTransform optionsParent;

		// Token: 0x04000C8C RID: 3212
		[SerializeField]
		private DroneLoadoutOption optionPrefab;

		// Token: 0x04000C8D RID: 3213
		private List<DroneLoadoutSlot> slots;

		// Token: 0x04000C8E RID: 3214
		public bool swapAvailable;
	}
}
