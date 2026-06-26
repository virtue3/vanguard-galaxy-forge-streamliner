using System;
using System.Collections.Generic;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Source.Crew;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Behaviour.UI.HUD
{
	// Token: 0x0200027C RID: 636
	public class CrewHud : MonoBehaviour, ITooltipCustomSource, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x0600173C RID: 5948 RVA: 0x00092E54 File Offset: 0x00091054
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			CrewData crewData = this.ship.spaceShipData.crewData;
			tooltip.AddTextLine("Total crew: " + this.TotalCrew(), 12, 8f);
			foreach (KeyValuePair<string, int> keyValuePair in crewData.crew)
			{
				if (keyValuePair.Value > 0)
				{
					tooltip.AddTextLine(keyValuePair.Key + ": " + keyValuePair.Value.ToString(), 12, 8f);
				}
			}
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x00092F08 File Offset: 0x00091108
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				this.UpdateCrewLabel();
			}
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x00092F3C File Offset: 0x0009113C
		private void UpdateCrewLabel()
		{
			if (this.ship == null)
			{
				if (GameplayManager.Instance == null || GameplayManager.Instance.spaceShip == null)
				{
					return;
				}
				this.ship = GameplayManager.Instance.spaceShip;
			}
			this.crewLabel.text = "Crew: " + this.TotalCrew();
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x00092FA4 File Offset: 0x000911A4
		private string TotalCrew()
		{
			return this.ship.spaceShipData.crewData.totalCrew.ToString() + "/" + this.ship.maxGrunts.ToString();
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x00092FEB File Offset: 0x000911EB
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				if (SidePanel.instance.currentTab == SidePanel.SideTabType.Ship)
				{
					SidePanel.instance.ShowCrewInShip();
					return;
				}
				SidePanel.instance.openCrew = true;
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Ship, 0);
			}
		}

		// Token: 0x04000E55 RID: 3669
		[SerializeField]
		private TMP_Text crewLabel;

		// Token: 0x04000E56 RID: 3670
		private float updateTimer;

		// Token: 0x04000E57 RID: 3671
		private SpaceShip ship;
	}
}
