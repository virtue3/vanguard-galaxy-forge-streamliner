using System;
using System.Collections;
using System.Collections.Generic;
using Behavior.UI.GalaxyMap;
using Behaviour.Dialogues;
using Behaviour.GalaxyMap;
using Behaviour.UI.HUD;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Spacestation;
using Source.Galaxy;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Side_Menu
{
	// Token: 0x0200029A RID: 666
	public class SidePanel : MonoBehaviour
	{
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x0009AAEE File Offset: 0x00098CEE
		// (set) Token: 0x060018B6 RID: 6326 RVA: 0x0009AAF6 File Offset: 0x00098CF6
		public SideTabAutopilot autopilotTab { get; private set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x0009AAFF File Offset: 0x00098CFF
		// (set) Token: 0x060018B8 RID: 6328 RVA: 0x0009AB07 File Offset: 0x00098D07
		public HoldPositionToggle holdPositionToggle { get; private set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x0009AB10 File Offset: 0x00098D10
		// (set) Token: 0x060018BA RID: 6330 RVA: 0x0009AB18 File Offset: 0x00098D18
		public bool open { get; private set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x0009AB21 File Offset: 0x00098D21
		// (set) Token: 0x060018BC RID: 6332 RVA: 0x0009AB29 File Offset: 0x00098D29
		public MapPointOfInterest focussedPoi { get; private set; }

		// Token: 0x060018BD RID: 6333 RVA: 0x0009AB34 File Offset: 0x00098D34
		private void Awake()
		{
			SidePanel.instance = this;
			this.tabActions = new Dictionary<SidePanel.SideTabType, Action>
			{
				{
					SidePanel.SideTabType.Captain,
					new Action(this.OpenCaptainTab)
				},
				{
					SidePanel.SideTabType.Inventory,
					new Action(this.OpenInventoryTab)
				},
				{
					SidePanel.SideTabType.Missions,
					new Action(this.OpenMissionsTab)
				},
				{
					SidePanel.SideTabType.Ship,
					new Action(this.OpenShipTab)
				},
				{
					SidePanel.SideTabType.Crew,
					new Action(this.OpenCrewTab)
				},
				{
					SidePanel.SideTabType.Settings,
					new Action(this.OpenSettingsTab)
				}
			};
			this.tabButtons = new Dictionary<SidePanel.SideTabType, SidePanelMenuButton>();
			foreach (SidePanelMenuButton sidePanelMenuButton in base.GetComponentsInChildren<SidePanelMenuButton>())
			{
				this.tabButtons[sidePanelMenuButton.menuTab] = sidePanelMenuButton;
			}
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0009ABFA File Offset: 0x00098DFA
		private void Start()
		{
			this.CalculateClosedPosition();
			this.SetRectTransform();
			this.sidePanel.localPosition = this.closedPosition;
			this.SetIdleStatus(Translation.Translate("@IdleCalculating", Array.Empty<object>()));
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0009AC33 File Offset: 0x00098E33
		private void Update()
		{
			if (this.waitTimer > 0f)
			{
				this.waitTimer -= Time.deltaTime;
			}
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0009AC54 File Offset: 0x00098E54
		private void CalculateClosedPosition()
		{
			this.closedPosition -= new Vector2(-731f, 0f);
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0009AC76 File Offset: 0x00098E76
		private void SetRectTransform()
		{
			this.sidePanel = base.GetComponent<RectTransform>();
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0009AC84 File Offset: 0x00098E84
		public void ToggleTab(SidePanel.SideTabType type)
		{
			if (DialogueManager.isOpen)
			{
				return;
			}
			if (this.IsSideMenuOpen() && this.currentTab == type)
			{
				this.CloseTab();
				return;
			}
			this.SwitchToTab(type, 0);
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0009ACAE File Offset: 0x00098EAE
		public void ToggleSubTab()
		{
			if (!this.open)
			{
				return;
			}
			this.sideTab.NextTab();
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0009ACC4 File Offset: 0x00098EC4
		public void SwitchToTab(SidePanel.SideTabType tab, int subTabIndex = 0)
		{
			if (tab == SidePanel.SideTabType.Map && !GamePlayer.current.IsMapUsageUnlocked())
			{
				return;
			}
			foreach (SidePanelMenuButton sidePanelMenuButton in this.tabButtons.Values)
			{
				sidePanelMenuButton.DeselectButton();
			}
			this.tabButtons[tab].SelectButton();
			this.SwitchTab(tab, subTabIndex);
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0009AD44 File Offset: 0x00098F44
		private void SwitchTab(SidePanel.SideTabType tab, int index = 0)
		{
			this.SetCurrentTab(tab, index);
			if (!this.open && tab != SidePanel.SideTabType.Map)
			{
				base.StartCoroutine(this.SlidePanelOpen());
			}
			this.UnloadAll();
			if (tab == SidePanel.SideTabType.Map)
			{
				base.StartCoroutine(this.OpenMapTab());
				return;
			}
			if (this.tabActions.ContainsKey(tab))
			{
				Action action = this.tabActions[tab];
				if (action != null)
				{
					action();
				}
				if (SpaceStationInterior.instance)
				{
					SpaceStationInterior.instance.ToggleSpacestationInterior(true);
					HudManager.Instance.ToggleMissionTracker(false);
				}
				else if (HudManager.Instance)
				{
					HudManager.Instance.ToggleHudElements(true);
				}
				if (this.open)
				{
					this.ShowNotificationOnSubTab();
				}
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0009ADF8 File Offset: 0x00098FF8
		public void SetSideTabIndex(int index = 0)
		{
			this.subtabIndex = index;
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0009AE04 File Offset: 0x00099004
		private void OpenTab(SideContentData tabData)
		{
			foreach (SideTabContent item in tabData.GetData())
			{
				this.sideTab.sideTabContents.Add(item);
			}
			if (tabData.GetData().Count > 0)
			{
				SideTabContent currentSideTabContent = this.sideTab.currentSideTabContent;
				base.StartCoroutine(this.sideTab.LoadContentTab(currentSideTabContent ?? tabData.GetData()[this.subtabIndex]));
				this.sideTab.SetNavButtons(tabData.GetData()[this.subtabIndex]);
			}
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0009AEC0 File Offset: 0x000990C0
		private void ShowNotificationOnSubTab()
		{
			if (this.notifySubTab != null)
			{
				this.sideTab.ShowNotification(this.notifySubTab);
				this.notifySubTab = null;
			}
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0009AEE4 File Offset: 0x000990E4
		public void CloseTab()
		{
			if (SpaceStationInterior.instance != null && this.currentTab != SidePanel.SideTabType.Map)
			{
				return;
			}
			if (SpaceStationInterior.instance != null && this.currentTab == SidePanel.SideTabType.Map)
			{
				this.SwitchTab(SidePanel.SideTabType.Captain, 0);
			}
			if (this.currentTab == SidePanel.SideTabType.Ship)
			{
				this.UnloadAll();
			}
			this.Close();
			foreach (SidePanelMenuButton sidePanelMenuButton in this.tabButtons.Values)
			{
				sidePanelMenuButton.DeselectButton();
			}
			if (AbstractGalaxyMapManager.IsShowing())
			{
				this.UnloadAll();
				if (SpaceStationInterior.instance)
				{
					SpaceStationInterior.instance.ToggleSpacestationInterior(true);
				}
				if (HudManager.Instance)
				{
					HudManager.Instance.ToggleHudElements(true);
					if (HudManager.Instance.echoRemarks)
					{
						HudManager.Instance.echoRemarks.ChangeVisibilityStatus(true);
					}
				}
			}
			this.SetCurrentTab(SidePanel.SideTabType.Captain, 0);
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0009AFE8 File Offset: 0x000991E8
		private void SetCurrentTab(SidePanel.SideTabType tab, int index = 0)
		{
			this.currentTab = tab;
			this.SetSideTabIndex(index);
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0009AFF8 File Offset: 0x000991F8
		private void UnloadAll()
		{
			AbstractGalaxyMapManager.UnloadGalaxyMap();
			this.sideTab.ToggleBackgroundImage(true);
			this.sideTab.ClearSideTabNav();
			this.sideTab.ClearSideContent();
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0009B021 File Offset: 0x00099221
		private void OpenCaptainTab()
		{
			this.OpenTab(this.captainTabData);
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0009B02F File Offset: 0x0009922F
		public void OpenInventoryTab()
		{
			this.OpenTab(this.inventoryTabData);
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0009B03D File Offset: 0x0009923D
		public void CheckInventoryTabs()
		{
			base.StartCoroutine(this.CheckInventoryTabsCoroutine());
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0009B04C File Offset: 0x0009924C
		private IEnumerator CheckInventoryTabsCoroutine()
		{
			if (!this.open || this.currentTab != SidePanel.SideTabType.Inventory)
			{
				yield break;
			}
			this.sideTab.ClearSideTabNav();
			this.sideTab.ClearSideContent();
			yield return new WaitUntil(() => SpaceStationInterior.instance == null);
			this.OpenInventoryTab();
			this.SwitchTab(SidePanel.SideTabType.Inventory, 0);
			yield break;
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0009B05B File Offset: 0x0009925B
		private void OpenMissionsTab()
		{
			this.OpenTab(this.missionTabData);
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0009B069 File Offset: 0x00099269
		private IEnumerator OpenMapTab()
		{
			yield return base.StartCoroutine(this.SlidePanelClose());
			if (AbstractGalaxyMapManager.IsShowing())
			{
				if (SpaceStationInterior.instance)
				{
					SpaceStationInterior.instance.ToggleSpacestationInterior(true);
					HudManager.Instance.ToggleMissionTracker(false);
				}
				AbstractGalaxyMapManager.UnloadGalaxyMap();
			}
			else
			{
				AbstractGalaxyMapManager.LoadGalaxyMap();
				if (SpaceStationInterior.instance)
				{
					SpaceStationInterior.instance.ToggleSpacestationInterior(false);
				}
				if (HudManager.Instance)
				{
					HudManager.Instance.ToggleHudElements(false);
					if (HudManager.Instance.echoRemarks)
					{
						HudManager.Instance.echoRemarks.ChangeVisibilityStatus(false);
					}
					HudManager.Instance.ToggleMissionTracker(true);
				}
			}
			yield break;
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0009B078 File Offset: 0x00099278
		private void OpenShipTab()
		{
			base.StartCoroutine(this.DelayThenShowShipTab());
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0009B087 File Offset: 0x00099287
		private IEnumerator DelayThenShowShipTab()
		{
			while (!this.open)
			{
				yield return null;
			}
			this.OpenTab(this.shipTabData);
			yield break;
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0009B096 File Offset: 0x00099296
		public void ShowCrewInShip()
		{
			if (this.sideTab.currentSideTabContent is ShipEquipment)
			{
				this.sideTab.sideContentHolder.GetComponentInChildren<ShipEquipment>().ShowCrewTab();
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0009B0C0 File Offset: 0x000992C0
		public ShipEquipment ShipEquipment()
		{
			if (this.sideTab.currentSideTabContent is ShipEquipment)
			{
				return this.sideTab.sideContentHolder.GetComponentInChildren<ShipEquipment>();
			}
			return null;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0009B0E6 File Offset: 0x000992E6
		private void OpenCrewTab()
		{
			this.OpenTab(this.crewTabData);
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0009B0F4 File Offset: 0x000992F4
		private void OpenSettingsTab()
		{
			this.OpenTab(this.settingsTabData);
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0009B102 File Offset: 0x00099302
		private void Close()
		{
			if (this.open && !this.closing)
			{
				base.StartCoroutine(this.SlidePanelClose());
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0009B121 File Offset: 0x00099321
		public bool IsSideMenuOpen()
		{
			return this.open || AbstractGalaxyMapManager.IsShowing();
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0009B132 File Offset: 0x00099332
		private IEnumerator SlidePanelOpen()
		{
			this.opening = true;
			this.closing = false;
			float elapsedTime = 0f;
			Vector2 startPosition = this.sidePanel.localPosition;
			while (elapsedTime < this.slideDuration)
			{
				float t = elapsedTime / this.slideDuration;
				this.sidePanel.localPosition = Vector2.Lerp(startPosition, this.openPosition, t);
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.sidePanel.localPosition = this.openPosition;
			this.open = true;
			this.opening = false;
			this.ShowNotificationOnSubTab();
			yield break;
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0009B141 File Offset: 0x00099341
		private IEnumerator SlidePanelClose()
		{
			this.opening = false;
			this.closing = true;
			float elapsedTime = 0f;
			Vector2 startPosition = this.sidePanel.localPosition;
			while (elapsedTime < this.slideDuration)
			{
				float t = elapsedTime / this.slideDuration;
				this.sidePanel.localPosition = Vector2.Lerp(startPosition, this.closedPosition, t);
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.sidePanel.localPosition = this.closedPosition;
			this.UnloadAll();
			this.open = false;
			this.closing = false;
			yield break;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0009B150 File Offset: 0x00099350
		public bool IsTabMoving()
		{
			return this.opening || this.closing;
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x0009B162 File Offset: 0x00099362
		public bool IsLoading()
		{
			return this.sideTab.IsLoading();
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x0009B16F File Offset: 0x0009936F
		public void NotifyTab(SidePanel.SideTabType sideTab, string subTab = null)
		{
			this.GetMenuButton(sideTab).ShowNotification();
			if (subTab == null)
			{
				return;
			}
			this.notifySubTab = subTab;
			if (this.open && this.currentTab == sideTab)
			{
				this.ShowNotificationOnSubTab();
			}
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x0009B19F File Offset: 0x0009939F
		public void NotifyAutopilot()
		{
			this.autopilotTab.ToggleHighlight(true);
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x0009B1B0 File Offset: 0x000993B0
		protected SidePanelMenuButton GetMenuButton(SidePanel.SideTabType sideTabs)
		{
			foreach (SidePanelMenuButton sidePanelMenuButton in base.GetComponentsInChildren<SidePanelMenuButton>())
			{
				if (sidePanelMenuButton.menuTab == sideTabs)
				{
					return sidePanelMenuButton;
				}
			}
			return null;
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0009B1E2 File Offset: 0x000993E2
		public void RefreshIfOpen()
		{
			if (this.open)
			{
				this.SwitchToTab(this.currentTab, this.subtabIndex);
			}
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0009B200 File Offset: 0x00099400
		public void ShowIdleStatus(bool show)
		{
			this.holdPositionToggle.gameObject.SetActive(!show && !AbstractGalaxyMapManager.current && !SpaceStationInterior.instance);
			this.idleContent.gameObject.SetActive(show);
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0009B24D File Offset: 0x0009944D
		public void SetIdleStatus(string msg)
		{
			this.idleStatusMessage.text = msg;
			this.idleContent.sizeDelta = new Vector2(this.idleStatusMessage.preferredWidth + 54f, this.idleContent.sizeDelta.y);
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0009B28C File Offset: 0x0009948C
		public void SetAutoPlayActivityFill(float timer)
		{
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x0009B290 File Offset: 0x00099490
		public void SetMapWidgetPosition(bool bottom)
		{
			RectTransform rectTransform = this.mapWidget.transform as RectTransform;
			if (bottom)
			{
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -300f);
				return;
			}
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0f);
		}

		// Token: 0x060018E6 RID: 6374 RVA: 0x0009B2E8 File Offset: 0x000994E8
		public void ShowSectorMapButtons(bool show)
		{
			this.mapWidget.gameObject.SetActive(show);
		}

		// Token: 0x060018E7 RID: 6375 RVA: 0x0009B2FB File Offset: 0x000994FB
		public void ShowMapContent(MapElement content)
		{
			this.mapWidget.ShowMapContent(content);
		}

		// Token: 0x060018E8 RID: 6376 RVA: 0x0009B30C File Offset: 0x0009950C
		public void ShowPoiOnMap(MapPointOfInterest poi)
		{
			bool waitForClose = false;
			if (AbstractGalaxyMapManager.IsShowing())
			{
				AbstractGalaxyMapManager.UnloadGalaxyMap();
				waitForClose = true;
			}
			base.StartCoroutine(this.OpenMapAndFocusPoi(poi, waitForClose));
		}

		// Token: 0x060018E9 RID: 6377 RVA: 0x0009B338 File Offset: 0x00099538
		private IEnumerator OpenMapAndFocusPoi(MapPointOfInterest poi, bool waitForClose = false)
		{
			if (waitForClose)
			{
				this.waitTimer = 1f;
				yield return new WaitUntil(() => AbstractGalaxyMapManager.current == null || this.waitTimer <= 0f);
			}
			this.focussedPoi = poi;
			this.SwitchToTab(SidePanel.SideTabType.Map, 0);
			this.waitTimer = 1f;
			yield return new WaitUntil(() => AbstractGalaxyMapManager.current != null || this.waitTimer <= 0f);
			this.focussedPoi = null;
			yield break;
		}

		// Token: 0x060018EA RID: 6378 RVA: 0x0009B355 File Offset: 0x00099555
		public void ToggleHideUI(bool show)
		{
			base.gameObject.SetActive(show);
		}

		// Token: 0x04000F5B RID: 3931
		public static SidePanel instance;

		// Token: 0x04000F5C RID: 3932
		public SideTab sideTab;

		// Token: 0x04000F5D RID: 3933
		[SerializeField]
		private SideContentData captainTabData;

		// Token: 0x04000F5E RID: 3934
		[SerializeField]
		private SideContentData inventoryTabData;

		// Token: 0x04000F5F RID: 3935
		[SerializeField]
		private SideContentData missionTabData;

		// Token: 0x04000F60 RID: 3936
		[SerializeField]
		private SideContentData mapTabData;

		// Token: 0x04000F61 RID: 3937
		[SerializeField]
		private SideContentData shipTabData;

		// Token: 0x04000F62 RID: 3938
		[SerializeField]
		private SideContentData crewTabData;

		// Token: 0x04000F63 RID: 3939
		[SerializeField]
		private SideContentData settingsTabData;

		// Token: 0x04000F66 RID: 3942
		[SerializeField]
		private RectTransform idleContent;

		// Token: 0x04000F67 RID: 3943
		[SerializeField]
		private TMP_Text idleStatusMessage;

		// Token: 0x04000F68 RID: 3944
		[SerializeField]
		private MapWidget mapWidget;

		// Token: 0x04000F69 RID: 3945
		public bool openCrew;

		// Token: 0x04000F6A RID: 3946
		public SidePanel.SideTabType currentTab;

		// Token: 0x04000F6B RID: 3947
		private RectTransform sidePanel;

		// Token: 0x04000F6D RID: 3949
		private bool opening;

		// Token: 0x04000F6E RID: 3950
		private bool closing;

		// Token: 0x04000F6F RID: 3951
		public float slideDuration = 0.2f;

		// Token: 0x04000F70 RID: 3952
		private Vector2 openPosition = new Vector2(0f, 0f);

		// Token: 0x04000F71 RID: 3953
		private Vector2 closedPosition;

		// Token: 0x04000F73 RID: 3955
		private Dictionary<SidePanel.SideTabType, Action> tabActions;

		// Token: 0x04000F74 RID: 3956
		private Dictionary<SidePanel.SideTabType, SidePanelMenuButton> tabButtons;

		// Token: 0x04000F75 RID: 3957
		private string notifySubTab;

		// Token: 0x04000F76 RID: 3958
		private int subtabIndex;

		// Token: 0x04000F77 RID: 3959
		private float waitTimer;

		// Token: 0x02000548 RID: 1352
		public enum SideTabType
		{
			// Token: 0x04001BD2 RID: 7122
			Captain,
			// Token: 0x04001BD3 RID: 7123
			Inventory,
			// Token: 0x04001BD4 RID: 7124
			Missions,
			// Token: 0x04001BD5 RID: 7125
			Map,
			// Token: 0x04001BD6 RID: 7126
			Ship,
			// Token: 0x04001BD7 RID: 7127
			Crew,
			// Token: 0x04001BD8 RID: 7128
			Settings
		}
	}
}
