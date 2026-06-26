using System;
using Behaviour.Equipment.Module;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Hangar
{
	// Token: 0x02000235 RID: 565
	public class DroneLoadoutOption : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, ITooltipTitleSource, ITooltipCustomSource
	{
		// Token: 0x06001527 RID: 5415 RVA: 0x00088A1E File Offset: 0x00086C1E
		public void SetDrone(Drone dr, bool swapAvailable)
		{
			this.contained = dr;
			this.swapAvailable = swapAvailable;
			this.label.text = dr.displayName;
			this.icon.sprite = dr.droneIcon;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x00088A50 File Offset: 0x00086C50
		public string GetTooltipTitle()
		{
			return this.contained.displayName;
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00088A60 File Offset: 0x00086C60
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			SpaceShipData spaceShipData;
			if (PersonalHangar.current)
			{
				spaceShipData = PersonalHangar.current.selectedShipData;
			}
			else
			{
				spaceShipData = GamePlayer.current.currentSpaceShip;
			}
			DroneBayModule component = spaceShipData.GetEquippedItem(EquipmentSlot.DroneBay).GetComponent<DroneBayModule>();
			bool flag = false;
			if (spaceShipData.droneSlots.Count < component.droneAmount)
			{
				flag = true;
			}
			else
			{
				for (int i = 0; i < spaceShipData.droneSlots.Count; i++)
				{
					if (spaceShipData.droneSlots[i] == null)
					{
						flag = true;
						break;
					}
				}
			}
			if (this.swapAvailable)
			{
				tooltip.AddTextLine(flag ? "@UIDroneEquip" : "@UIDroneLoadoutFull", 12, 8f);
				return;
			}
			tooltip.AddTextLine("@UIDroneSwapInSS", 12, 8f);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x00088B1E File Offset: 0x00086D1E
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.swapAvailable && eventData.button == PointerEventData.InputButton.Right)
			{
				base.GetComponentInParent<DroneLoadoutUI>().AddDroneToLoadout(this.contained);
				UITooltip.Refresh();
			}
		}

		// Token: 0x04000C7D RID: 3197
		[SerializeField]
		private Image icon;

		// Token: 0x04000C7E RID: 3198
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000C7F RID: 3199
		private Drone contained;

		// Token: 0x04000C80 RID: 3200
		private bool swapAvailable;
	}
}
