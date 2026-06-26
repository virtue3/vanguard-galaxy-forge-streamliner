using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Managers;
using Behaviour.Persistables;
using Behaviour.Salvage;
using Behaviour.Transparency;
using Behaviour.UI.HUD.Fleet;
using Behaviour.UI.HUD.Turrets;
using Behaviour.UI.Missions;
using Behaviour.UI.Salvage;
using Behaviour.UI.Travel;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Crew;
using Source.Data.Persistable;
using Source.Galaxy.POI;
using Source.Player;
using Source.SpaceShip;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.HUD
{
	// Token: 0x02000282 RID: 642
	public class HudManager : MonoBehaviour
	{
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x00093E10 File Offset: 0x00092010
		// (set) Token: 0x06001765 RID: 5989 RVA: 0x00093E18 File Offset: 0x00092018
		public ShipHealthMonitor shipHealthMonitor { get; private set; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001766 RID: 5990 RVA: 0x00093E21 File Offset: 0x00092021
		// (set) Token: 0x06001767 RID: 5991 RVA: 0x00093E29 File Offset: 0x00092029
		public DroneControls droneControls { get; private set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x00093E32 File Offset: 0x00092032
		// (set) Token: 0x06001769 RID: 5993 RVA: 0x00093E3A File Offset: 0x0009203A
		public AbilityHud abilityHud { get; private set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x00093E43 File Offset: 0x00092043
		// (set) Token: 0x0600176B RID: 5995 RVA: 0x00093E4B File Offset: 0x0009204B
		public TurretControl turretControl { get; private set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x00093E54 File Offset: 0x00092054
		// (set) Token: 0x0600176D RID: 5997 RVA: 0x00093E5C File Offset: 0x0009205C
		public FocusedMissionDisplay focusedMissionDisplay { get; private set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x00093E65 File Offset: 0x00092065
		// (set) Token: 0x0600176F RID: 5999 RVA: 0x00093E6D File Offset: 0x0009206D
		public LocationManager locationManager { get; private set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001770 RID: 6000 RVA: 0x00093E76 File Offset: 0x00092076
		// (set) Token: 0x06001771 RID: 6001 RVA: 0x00093E7E File Offset: 0x0009207E
		public Button dockButton { get; private set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x00093E87 File Offset: 0x00092087
		// (set) Token: 0x06001773 RID: 6003 RVA: 0x00093E8F File Offset: 0x0009208F
		public TransponderStatus transponderStatus { get; private set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001774 RID: 6004 RVA: 0x00093E98 File Offset: 0x00092098
		// (set) Token: 0x06001775 RID: 6005 RVA: 0x00093EA0 File Offset: 0x000920A0
		public BuffHud buffHud { get; private set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001776 RID: 6006 RVA: 0x00093EA9 File Offset: 0x000920A9
		// (set) Token: 0x06001777 RID: 6007 RVA: 0x00093EB1 File Offset: 0x000920B1
		public GameObject seperator { get; private set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001778 RID: 6008 RVA: 0x00093EBA File Offset: 0x000920BA
		// (set) Token: 0x06001779 RID: 6009 RVA: 0x00093EC2 File Offset: 0x000920C2
		public IndustrialOpsUI industrialOps { get; private set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x00093ECB File Offset: 0x000920CB
		// (set) Token: 0x0600177B RID: 6011 RVA: 0x00093ED3 File Offset: 0x000920D3
		public Mask salvageMask { get; private set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x00093EDC File Offset: 0x000920DC
		// (set) Token: 0x0600177D RID: 6013 RVA: 0x00093EE4 File Offset: 0x000920E4
		public RectTransform healthBarContent { get; private set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x00093EED File Offset: 0x000920ED
		// (set) Token: 0x0600177F RID: 6015 RVA: 0x00093EF5 File Offset: 0x000920F5
		public bool showHud { get; private set; }

		// Token: 0x06001780 RID: 6016 RVA: 0x00093EFE File Offset: 0x000920FE
		protected void Awake()
		{
			HudManager.Instance = this;
			this.ToggleTrackShipText(false);
			this.ToggleHealthMonitor(true);
			this.salvageImage = this.salvageMask.GetComponent<Image>();
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00093F25 File Offset: 0x00092125
		private void Start()
		{
			this.progressImage.SetCrewMember(GamePlayer.current.commander);
			base.StartCoroutine(this.InitializeHud());
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x00093F49 File Offset: 0x00092149
		private void LateUpdate()
		{
			this.SetMask();
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00093F51 File Offset: 0x00092151
		public void SetEchoRemarksStatus(bool active)
		{
			if (GameplayerPrefs.GetTravelHints())
			{
				this.echoRemarks.gameObject.SetActive(active);
				return;
			}
			this.echoRemarks.gameObject.SetActive(false);
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00093F7D File Offset: 0x0009217D
		public void SetCrewHudStatus(bool active)
		{
			if (this.crewHud == null)
			{
				return;
			}
			this.crewHud.gameObject.SetActive(active);
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00093F9F File Offset: 0x0009219F
		private void OnDestroy()
		{
			this.healthBars.Clear();
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00093FAC File Offset: 0x000921AC
		private IEnumerator InitializeHud()
		{
			yield return new WaitForSeconds(0.1f);
			this.ToggleHudElements(true);
			yield break;
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00093FBC File Offset: 0x000921BC
		public void ToggleTrackShipText(bool visible)
		{
			if (this.trackShipText && (!Singleton<TravelManager>.HasInstance || !Singleton<TravelManager>.Instance.TravelActive()))
			{
				if (visible && GameplayManager.Instance)
				{
					bool isVisible = GameplayManager.Instance.spaceShip.GetComponent<Renderer>().isVisible;
					this.trackShipText.gameObject.SetActive(!isVisible);
					return;
				}
				this.trackShipText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00094033 File Offset: 0x00092233
		public void ToggleProgressImage(bool visible)
		{
			if (this.progressImage)
			{
				this.progressImage.gameObject.SetActive(visible);
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00094053 File Offset: 0x00092253
		public void ToggleHealthMonitor(bool visible)
		{
			if (this.shipHealthMonitor)
			{
				this.shipHealthMonitor.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00094073 File Offset: 0x00092273
		public void ToggleDroneControls(bool visible)
		{
			if (this.droneControls)
			{
				this.droneControls.gameObject.SetActive(visible && this.droneControls.droneBay);
			}
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000940A8 File Offset: 0x000922A8
		public void ToggleAbilityHUD(bool visible)
		{
			if (this.abilityHud)
			{
				this.abilityHud.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000940C8 File Offset: 0x000922C8
		public void ToggleTurretHUD(bool visible)
		{
			if (this.turretControl)
			{
				this.turretControl.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000940E8 File Offset: 0x000922E8
		public void ToggleBuffHud(bool visible)
		{
			if (this.buffHud)
			{
				this.buffHud.gameObject.SetActive(visible);
			}
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00094108 File Offset: 0x00092308
		public void ToggleTransponder(bool visible)
		{
			if (this.transponderStatus && GamePlayer.current != null)
			{
				this.transponderStatus.gameObject.SetActive(visible && GamePlayer.current.hasUmbralTransponder);
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x0009413E File Offset: 0x0009233E
		public void SetTransponder()
		{
			this.transponderStatus.SetTransponder();
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0009414B File Offset: 0x0009234B
		public void ToggleMissionTracker(bool visible)
		{
			if (this.focusedMissionDisplay)
			{
				this.focusedMissionDisplay.gameObject.SetActive(visible);
			}
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x0009416B File Offset: 0x0009236B
		public void ToggleLocationManager(bool visible)
		{
			if (this.locationManager)
			{
				this.locationManager.SetTextAlpha(visible ? 1f : 0f);
				this.locationManager.SetVisibility(visible);
			}
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000941A0 File Offset: 0x000923A0
		public void ToggleDockButton(bool visible)
		{
			if (this.dockButton)
			{
				this.dockButton.gameObject.SetActive(visible);
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000941C0 File Offset: 0x000923C0
		public void ToggleSalvageWindow(SalvageContainer inWorld, SalvageData data)
		{
			this.salvageMask.enabled = !ScreenSettings.fullscreen;
			if (this.salvageWindow.gameObject.activeSelf)
			{
				this.salvageWindow.gameObject.SetActive(false);
				return;
			}
			this.salvageWindow.SetSalvage(inWorld, data);
			this.salvageWindow.gameObject.SetActive(true);
			if (this.salvageStatus.gameObject.activeSelf)
			{
				this.ToggleSalvageStatus(inWorld, data);
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x0009423C File Offset: 0x0009243C
		public void ToggleSalvageStatus(SalvageContainer inWorld, SalvageData data)
		{
			this.salvageMask.enabled = !ScreenSettings.fullscreen;
			if (this.salvageStatus.gameObject.activeSelf)
			{
				this.salvageStatus.gameObject.SetActive(false);
				return;
			}
			this.salvageStatus.SetSalvageData(inWorld, data);
			this.salvageStatus.gameObject.SetActive(true);
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x0009429E File Offset: 0x0009249E
		public void ToggleFpsCounter()
		{
			this.fpsDisplay.gameObject.SetActive(!this.fpsDisplay.isActiveAndEnabled);
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000942C0 File Offset: 0x000924C0
		public void Dock()
		{
			if (GamePlayer.current.currentPointOfInterest is IndustryStation)
			{
				IndustryMissionDock componentInChildren = BasePoiManager.current.GetComponentInChildren<IndustryMissionDock>();
				if (componentInChildren)
				{
					if (componentInChildren.OverlapsCollider(GameplayManager.Instance.spaceShip.surfaceCollider))
					{
						componentInChildren.DockWithStation();
						return;
					}
					GameplayManager.Instance.spaceShip.SetOverrideDestination(componentInChildren.transform.position, true, false, false);
					return;
				}
			}
			else if (GamePlayer.current.currentPointOfInterest is SpaceStation)
			{
				SpacestationExteriorManager.Instance.CheckForDocking();
				GameplayManager.Instance.spaceShip.manualInputTimer = 0f;
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x00094364 File Offset: 0x00092564
		public void ToggleHudElements(bool visible)
		{
			if (!visible)
			{
				this.HideSalvageHUD();
			}
			else if (GameplayManager.Instance && GameplayManager.Instance.spaceShip.SalvagingItem())
			{
				SalvageContainer currentSalvageTarget = GameplayManager.Instance.spaceShip.GetCurrentSalvageTarget();
				this.ToggleSalvageStatus(currentSalvageTarget, (currentSalvageTarget != null) ? currentSalvageTarget.data : null);
			}
			this.showHud = visible;
			this.ToggleProgressImage(visible);
			this.ToggleHealthMonitor(visible);
			this.ToggleAbilityHUD(visible);
			this.ToggleTurretHUD(visible);
			this.ToggleTransponder(visible);
			this.ToggleMissionTracker(visible);
			this.ToggleLocationManager(visible);
			this.ToggleAllHealthBars(visible);
			this.ToggleAllTargetIndicators(visible);
			this.ToggleDroneControls(visible);
			this.ToggleBuffHud(visible);
			if (GamePlayer.current != null && GamePlayer.current.commander.LeadershipUnlocked())
			{
				this.SetCrewHudStatus(visible);
			}
			bool visible2;
			if (visible)
			{
				GamePlayer current = GamePlayer.current;
				SpaceStation spaceStation = ((current != null) ? current.currentPointOfInterest : null) as SpaceStation;
				if (spaceStation != null && spaceStation.PlayerIsFriendly())
				{
					TravelManager instance = Singleton<TravelManager>.Instance;
					visible2 = (instance == null || !instance.TravelActive());
					goto IL_FB;
				}
			}
			visible2 = false;
			IL_FB:
			this.ToggleDockButton(visible2);
			this.ToggleWingman(visible);
			this.ToggleIndustryOps(visible);
			this.ToggleTrackShipText(visible);
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00094488 File Offset: 0x00092688
		public void ToggleWingman(bool visible)
		{
			foreach (WingmanDisplay wingmanDisplay in this.wingmanDisplays)
			{
				if (wingmanDisplay)
				{
					UnityEngine.Object.Destroy(wingmanDisplay.gameObject);
				}
			}
			this.wingmanDisplays.Clear();
			if (visible && GamePlayer.current != null)
			{
				foreach (SpaceShipData spaceShipData in GamePlayer.current.activeFleet)
				{
					Mercenary mercenary = spaceShipData.commanderData as Mercenary;
					if (mercenary != null)
					{
						WingmanDisplay wingmanDisplay2 = UnityEngine.Object.Instantiate<WingmanDisplay>(this.wingmanDisplayPrefab, this.wingmanDisplayParent);
						wingmanDisplay2.SetWingman(mercenary);
						wingmanDisplay2.gameObject.SetActive(true);
						this.wingmanDisplays.Add(wingmanDisplay2);
					}
				}
			}
			this.RefreshFleetLayout();
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00094584 File Offset: 0x00092784
		public void ToggleIndustryOps(bool visible)
		{
			if ((this.industrialOps && visible == this.industrialOps.isActiveAndEnabled) || !this.shipHealthMonitor)
			{
				return;
			}
			if (!visible)
			{
				this.industrialOps.SetActive(visible);
			}
			else
			{
				this.industrialOps.SetActive(true);
			}
			this.RefreshFleetLayout();
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000945E0 File Offset: 0x000927E0
		public void RefreshFleetLayout()
		{
			if (!this.shipHealthMonitor)
			{
				return;
			}
			float num = (this.shipHealthMonitor.transform as RectTransform).anchoredPosition.y + (this.shipHealthMonitor.transform as RectTransform).sizeDelta.y + 2f;
			foreach (WingmanDisplay wingmanDisplay in this.wingmanDisplays)
			{
				if (wingmanDisplay && wingmanDisplay.gameObject.activeSelf)
				{
					wingmanDisplay.SetAnchoredPosition(num);
					num += (wingmanDisplay.transform as RectTransform).sizeDelta.y + 2f;
				}
			}
			if (this.industrialOps && this.industrialOps.gameObject.activeSelf)
			{
				this.industrialOps.SetAnchoredPosition(num);
			}
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x000946DC File Offset: 0x000928DC
		public void HideSalvageHUD()
		{
			if (this.salvageMask)
			{
				this.salvageMask.enabled = !ScreenSettings.fullscreen;
			}
			if (this.salvageWindow && this.salvageWindow.gameObject.activeSelf)
			{
				this.ToggleSalvageWindow(null, null);
			}
			if (this.salvageStatus && this.salvageStatus.gameObject.activeSelf)
			{
				this.ToggleSalvageStatus(null, null);
			}
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00094757 File Offset: 0x00092957
		public bool ShowingSalvageStatus()
		{
			return this.salvageStatus && this.salvageStatus.gameObject.activeSelf;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00094778 File Offset: 0x00092978
		public void SetMask()
		{
			this.salvageMask.enabled = !ScreenSettings.fullscreen;
			this.salvageImage.enabled = !ScreenSettings.fullscreen;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000947A0 File Offset: 0x000929A0
		public void ShowHealthBar(AbstractUnit unit)
		{
			if (!this.healthBars.ContainsKey(unit) && ((unit.currentHullHP > 1f && unit.currentHullHP < unit.maxHullHP) || (unit.currentArmorHP > 1f && unit.currentArmorHP < unit.maxArmorHP) || (unit.currentShieldHP > 1f && unit.currentShieldHP < unit.maxShieldHP)))
			{
				HealthBar healthBar = UnityEngine.Object.Instantiate<HealthBar>(this.healthBar, this.healthBarContent);
				if (this.healthBarContent.gameObject.activeInHierarchy)
				{
					healthBar.Show(unit);
				}
				this.healthBars.Add(unit, healthBar);
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00094847 File Offset: 0x00092A47
		public void RemoveHealthBar(AbstractUnit unit)
		{
			if (this.healthBars.ContainsKey(unit))
			{
				this.healthBars[unit].Destroy();
				this.healthBars.Remove(unit);
			}
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x00094875 File Offset: 0x00092A75
		public void RemoveAllHealthBars()
		{
			this.healthBarContent.DestroyChildren();
			this.healthBars.Clear();
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00094890 File Offset: 0x00092A90
		public void ToggleAllHealthBars(bool enable)
		{
			if (!this.healthBarContent)
			{
				return;
			}
			enable = (enable && GameplayerPrefs.GetHealthBarsOn());
			this.healthBarContent.gameObject.SetActive(enable);
			if (enable)
			{
				foreach (KeyValuePair<AbstractUnit, HealthBar> keyValuePair in this.healthBars)
				{
					keyValuePair.Value.Show(keyValuePair.Key);
				}
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x00094920 File Offset: 0x00092B20
		public void ToggleAllTargetIndicators(bool enable)
		{
			foreach (KeyValuePair<TargetableUnit, TargetIndicator> keyValuePair in this.targetIndicators)
			{
				keyValuePair.Value.gameObject.SetActive(enable);
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00094980 File Offset: 0x00092B80
		public void ShowTargetIndicator(TargetableUnit unit)
		{
			this.RemoveAllTargetIndicators();
			TargetIndicator targetIndicator = UnityEngine.Object.Instantiate<TargetIndicator>(this.targetIndicatorPrefab, this.salvageMask.transform);
			targetIndicator.Show(unit);
			this.targetIndicators.Add(unit, targetIndicator);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000949C0 File Offset: 0x00092BC0
		public void RemoveAllTargetIndicators()
		{
			foreach (KeyValuePair<TargetableUnit, TargetIndicator> keyValuePair in this.targetIndicators)
			{
				keyValuePair.Value.Destroy();
			}
			this.targetIndicators.Clear();
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x00094A24 File Offset: 0x00092C24
		public void RemoveTargetIndicator(TargetableUnit unit)
		{
			if (this.targetIndicators.ContainsKey(unit))
			{
				this.targetIndicators[unit].Destroy();
				this.targetIndicators.Remove(unit);
			}
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00094A54 File Offset: 0x00092C54
		public void ShowTimerInfo(float totalTime, string message)
		{
			if (!this.timerInfo.gameObject.activeSelf)
			{
				this.timerInfo.gameObject.SetActive(true);
				this.timerInfo.SetTimerEvent(totalTime, message, new Action<int>(this.HideTimerInfo), 1);
				Vector2 anchoredPosition = (this.timerInfo.transform as RectTransform).anchoredPosition;
				if (!TravelInfo.instance.visible)
				{
					anchoredPosition.y = -125f;
				}
				else
				{
					anchoredPosition.y = -325f;
				}
				(this.timerInfo.transform as RectTransform).anchoredPosition = anchoredPosition;
			}
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00094AF0 File Offset: 0x00092CF0
		public void HideTimerInfo(int priority = 0)
		{
			this.timerInfo.gameObject.SetActive(false);
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00094B04 File Offset: 0x00092D04
		public void ShowSubtleTimerInfo(float totalTime, string message, int priority = 2)
		{
			if (!this.subtleTimerInfo.gameObject.activeSelf && priority < this.subtleTimerPriority)
			{
				this.subtleTimerInfo.gameObject.SetActive(true);
				this.subtleTimerInfo.SetTimerEvent(totalTime, message, new Action<int>(this.HideSubtleTimerInfo), priority);
				this.subtleTimerPriority = priority;
			}
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00094B5E File Offset: 0x00092D5E
		public void HideSubtleTimerInfo(int priority = 2)
		{
			if (this.subtleTimerPriority == priority)
			{
				this.subtleTimerInfo.gameObject.SetActive(false);
				this.subtleTimerPriority = 3;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00094B81 File Offset: 0x00092D81
		public void ShowNpcMessage(AbstractUnit unit, AbstractCrewData commander, string message, float duration = 6f)
		{
			if (this.npcMessagePrefab == null || commander == null)
			{
				return;
			}
			UnityEngine.Object.Instantiate<NpcMessageDisplay>(this.npcMessagePrefab, this.wingmanDisplayParent).Setup(unit, commander.sprite, commander.callsign, message, duration);
		}

		// Token: 0x04000E86 RID: 3718
		[SerializeField]
		public TextMeshProUGUI trackShipText;

		// Token: 0x04000E87 RID: 3719
		[SerializeField]
		public LevelProgressImage progressImage;

		// Token: 0x04000E92 RID: 3730
		[SerializeField]
		public HealthBar healthBar;

		// Token: 0x04000E93 RID: 3731
		[SerializeField]
		public TargetIndicator targetIndicatorPrefab;

		// Token: 0x04000E94 RID: 3732
		[SerializeField]
		public TimerInfo timerInfo;

		// Token: 0x04000E95 RID: 3733
		[SerializeField]
		public TimerInfo subtleTimerInfo;

		// Token: 0x04000E96 RID: 3734
		[SerializeField]
		public FpsDisplay fpsDisplay;

		// Token: 0x04000E97 RID: 3735
		[SerializeField]
		public EchoRemarks echoRemarks;

		// Token: 0x04000E99 RID: 3737
		[SerializeField]
		private SalvageWindow salvageWindow;

		// Token: 0x04000E9A RID: 3738
		[SerializeField]
		private SalvageStatusWindow salvageStatus;

		// Token: 0x04000E9B RID: 3739
		[SerializeField]
		private CrewHud crewHud;

		// Token: 0x04000E9C RID: 3740
		[SerializeField]
		private WingmanDisplay wingmanDisplayPrefab;

		// Token: 0x04000E9D RID: 3741
		[SerializeField]
		private Transform wingmanDisplayParent;

		// Token: 0x04000E9E RID: 3742
		[SerializeField]
		private NpcMessageDisplay npcMessagePrefab;

		// Token: 0x04000E9F RID: 3743
		private List<WingmanDisplay> wingmanDisplays = new List<WingmanDisplay>();

		// Token: 0x04000EA2 RID: 3746
		private Image salvageImage;

		// Token: 0x04000EA3 RID: 3747
		public static HudManager Instance;

		// Token: 0x04000EA4 RID: 3748
		private Dictionary<AbstractUnit, HealthBar> healthBars = new Dictionary<AbstractUnit, HealthBar>();

		// Token: 0x04000EA5 RID: 3749
		private Dictionary<TargetableUnit, TargetIndicator> targetIndicators = new Dictionary<TargetableUnit, TargetIndicator>();

		// Token: 0x04000EA6 RID: 3750
		private int subtleTimerPriority = 3;
	}
}
