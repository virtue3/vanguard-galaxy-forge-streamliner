using System;
using Behaviour.GalaxyMap;
using Behaviour.UI;
using Behaviour.UI.HUD;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Travel;
using Source.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Behaviour.Gameplay
{
	// Token: 0x02000327 RID: 807
	public class GameControls : MonoBehaviour
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x000B3E21 File Offset: 0x000B2021
		// (set) Token: 0x06001E50 RID: 7760 RVA: 0x000B3E28 File Offset: 0x000B2028
		public static GameControls instance { get; private set; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x000B3E30 File Offset: 0x000B2030
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x000B3E38 File Offset: 0x000B2038
		public PlayerControls controls { get; private set; }

		// Token: 0x06001E53 RID: 7763 RVA: 0x000B3E44 File Offset: 0x000B2044
		private void Awake()
		{
			GameControls.instance = this;
			this.controls = new PlayerControls();
			this.controls.UI.ToggleMap.performed += this.UIToggleMap;
			this.controls.UI.ToggleInventory.performed += this.UIToggleInventory;
			this.controls.UI.Tab.performed += this.UIToggleSubTab;
			this.controls.UI.Escape.performed += this.UIProcessEscape;
			this.controls.UI.HideUI.performed += this.UIHideUI;
			this.controls.UI.ToggleHPBars.performed += this.UIToggleHealthBars;
			this.controls.Player.Follow.performed += this.FollowShip;
			this.controls.Player.ToggleAutopilot.performed += this.ToggleAutopilot;
			this.controls.Player.HoldPosition.performed += this.HoldPosition;
			this.controls.Player.ToggleEchoTravel.performed += this.ToggleEchoTravel;
			this.controls.UI.ToggleAudio.performed += this.ToggleAudio;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000B3FEE File Offset: 0x000B21EE
		private void Update()
		{
			if (this.inputDelay > 0)
			{
				this.inputDelay--;
				if (this.inputDelay == 0)
				{
					PlayerControls controls = this.controls;
					if (controls == null)
					{
						return;
					}
					controls.Enable();
				}
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000B401F File Offset: 0x000B221F
		private void OnEnable()
		{
			PlayerControls controls = this.controls;
			if (controls == null)
			{
				return;
			}
			controls.Enable();
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000B4031 File Offset: 0x000B2231
		private void OnDisable()
		{
			PlayerControls controls = this.controls;
			if (controls == null)
			{
				return;
			}
			controls.Disable();
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000B4043 File Offset: 0x000B2243
		private void UIToggleMap(InputAction.CallbackContext obj)
		{
			if (SidePanel.instance && !Keyboard.current.ctrlKey.isPressed)
			{
				SidePanel.instance.ToggleTab(SidePanel.SideTabType.Map);
			}
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000B406D File Offset: 0x000B226D
		private void UIToggleHealthBars(InputAction.CallbackContext obj)
		{
			GameplayerPrefs.SetHealthBarsOn(!GameplayerPrefs.GetHealthBarsOn());
			HudManager.Instance.ToggleAllHealthBars(GameplayerPrefs.GetHealthBarsOn());
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000B408B File Offset: 0x000B228B
		private void UIToggleInventory(InputAction.CallbackContext obj)
		{
			if (SidePanel.instance)
			{
				SidePanel.instance.ToggleTab(SidePanel.SideTabType.Inventory);
			}
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000B40A4 File Offset: 0x000B22A4
		private void UIToggleSubTab(InputAction.CallbackContext obj)
		{
			SidePanel.instance.ToggleSubTab();
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000B40B0 File Offset: 0x000B22B0
		private void UIProcessEscape(InputAction.CallbackContext obj)
		{
			SidePanel.instance.ToggleTab(SidePanel.SideTabType.Settings);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000B40C0 File Offset: 0x000B22C0
		private void UIHideUI(InputAction.CallbackContext obj)
		{
			this.uiHidden = !this.uiHidden;
			SidePanel.instance.ToggleHideUI(!this.uiHidden);
			HudManager.Instance.ToggleHudElements(!this.uiHidden);
			TravelInfo.instance.ToggleHideUI(this.uiHidden);
			UIInfoTextParent.instance.gameObject.SetActive(!this.uiHidden);
			if (SpaceStationInterior.instance)
			{
				SpaceStationInterior.instance.ToggleHideUI(!this.uiHidden);
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000B4149 File Offset: 0x000B2349
		private void FollowShip(InputAction.CallbackContext obj)
		{
			if (AbstractGalaxyMapManager.current)
			{
				return;
			}
			GameplayManager.Instance.cameraMovement.SetTarget(GameplayManager.Instance.spaceShip, true);
			HudManager.Instance.ToggleTrackShipText(false);
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000B417D File Offset: 0x000B237D
		private void ToggleAutopilot(InputAction.CallbackContext obj)
		{
			if (SidePanel.instance)
			{
				SidePanel.instance.autopilotTab.ToggleChanged();
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000B419A File Offset: 0x000B239A
		private void ToggleEchoTravel(InputAction.CallbackContext obj)
		{
			if (SidePanel.instance)
			{
				SidePanel.instance.autopilotTab.ToggleEchoTravel();
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000B41B7 File Offset: 0x000B23B7
		private void ToggleAudio(InputAction.CallbackContext obj)
		{
			GameplayerPrefs.SetAudioMuted(!GameplayerPrefs.GetAudioMuted());
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000B41C6 File Offset: 0x000B23C6
		private void HoldPosition(InputAction.CallbackContext obj)
		{
			if (SidePanel.instance)
			{
				SidePanel.instance.holdPositionToggle.DoToggle();
			}
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000B41E4 File Offset: 0x000B23E4
		public static void Delay()
		{
			if (GameControls.instance && GameControls.instance.controls != null)
			{
				GameControls.instance.inputDelay = Mathf.Max(5, GameControls.instance.inputDelay + 1);
				GameControls.instance.controls.Disable();
			}
		}

		// Token: 0x04001237 RID: 4663
		private int inputDelay;

		// Token: 0x04001238 RID: 4664
		private bool uiHidden;
	}
}
