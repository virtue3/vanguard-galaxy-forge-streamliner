using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Bootstrap;
using Behaviour.Equipment.Module;
using Behaviour.Managers;
using Behaviour.Transparency;
using Behaviour.UI.Forge;
using Behaviour.UI.HUD;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Refinery;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation.Bar;
using Behaviour.UI.Spacestation.Location;
using Behaviour.UI.Spacestation.Shops;
using Behaviour.UI.Spacestation.Trade;
using Behaviour.Util;
using Source.Dialogues;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.Spacestation;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation
{
	// Token: 0x02000216 RID: 534
	public class SpaceStationInterior : MonoBehaviour, INotifyScreenChange
	{
		// Token: 0x17000339 RID: 825
		// (get) Token: 0x060013AF RID: 5039 RVA: 0x0007FBD5 File Offset: 0x0007DDD5
		// (set) Token: 0x060013B0 RID: 5040 RVA: 0x0007FBDD File Offset: 0x0007DDDD
		public SpaceStation spacestation { get; private set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x0007FBE6 File Offset: 0x0007DDE6
		// (set) Token: 0x060013B2 RID: 5042 RVA: 0x0007FBEE File Offset: 0x0007DDEE
		public Sprite repairIcon { get; private set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060013B3 RID: 5043 RVA: 0x0007FBF7 File Offset: 0x0007DDF7
		// (set) Token: 0x060013B4 RID: 5044 RVA: 0x0007FBFF File Offset: 0x0007DDFF
		public RectTransform sideContent { get; private set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x0007FC08 File Offset: 0x0007DE08
		// (set) Token: 0x060013B6 RID: 5046 RVA: 0x0007FC10 File Offset: 0x0007DE10
		public SpaceStationFacility currentTab { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x0007FC19 File Offset: 0x0007DE19
		// (set) Token: 0x060013B8 RID: 5048 RVA: 0x0007FC21 File Offset: 0x0007DE21
		public Airlock currentAirlock { get; private set; }

		// Token: 0x060013B9 RID: 5049 RVA: 0x0007FC2C File Offset: 0x0007DE2C
		private void Awake()
		{
			if (SpaceStation.current == null || GamePlayer.current == null)
			{
				return;
			}
			SpaceStationInterior.instance = this;
			SidePanel sidePanel = SidePanel.instance;
			this.sidePanelWasOpen = (sidePanel != null && sidePanel.IsSideMenuOpen());
			this.defaultArt = this.interactiveArt.sprite;
			this.spacestation = (GamePlayer.current.currentPointOfInterest as SpaceStation);
			this.SetupCharacters(this.spacestation.characters);
			FactionIconSet factionIconSet = FactionIconSet.Get(this.spacestation.faction);
			if ((factionIconSet != null) ? factionIconSet.fullSize : null)
			{
				this.factionIcon.sprite = factionIconSet.fullSize;
				this.hasFactionIcon = true;
			}
			else
			{
				this.hasFactionIcon = false;
			}
			this.buttons = new List<SpaceStationButton>
			{
				this.airlockButton
			};
			this.airlockButton.gameObject.SetActive(false);
			this.tabParent.DestroyActiveChildren();
			foreach (SpaceStationFacility spaceStationFacility in this.spacestation.GetFacilities())
			{
				SpaceStationButton spaceStationButton = UnityEngine.Object.Instantiate<SpaceStationButton>(this.tabButtonPrefab, this.tabParent);
				spaceStationButton.spaceStationFacility = spaceStationFacility;
				this.buttons.Add(spaceStationButton);
			}
			bool flag = this.spacestation.HasFacility(SpaceStationFacility.OutpostAirlock);
			if (!flag)
			{
				this.airlockButton.gameObject.SetActive(true);
				this.airlockButton.transform.SetAsLastSibling();
			}
			this.spacestation.RefreshShopsIfNecessary(true);
			MissionObjective.Trigger(MissionTrigger.DockedWithSpaceStation, null, null, false);
			GamePlayer.current.ReloadDefensiveTurrets();
			GamePlayer.current.RepairFleet();
			if (GamePlayer.current.hasUmbralTransponder)
			{
				GamePlayer.current.hasUmbralTransponder = false;
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralTransponderDeactivated", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
			}
			if (SpaceStation.current.faction.GetReputation(Faction.player) == 0)
			{
				SpaceStation.current.faction.ChangePlayerReputation(20);
			}
			this.tabActions = new Dictionary<SpaceStationFacility, Action>
			{
				{
					SpaceStationFacility.GeneralShop,
					new Action(this.OpenGeneralShop)
				},
				{
					SpaceStationFacility.MiningShop,
					new Action(this.OpenMiningGuild)
				},
				{
					SpaceStationFacility.SalvageShop,
					new Action(this.OpenSalvageGuild)
				},
				{
					SpaceStationFacility.TradeTerminal,
					new Action(this.OpenTradeTerminal)
				},
				{
					SpaceStationFacility.Refinery,
					new Action(this.OpenRefinery)
				},
				{
					SpaceStationFacility.Forge,
					new Action(this.OpenForge)
				},
				{
					SpaceStationFacility.Bar,
					new Action(this.OpenBar)
				},
				{
					SpaceStationFacility.RecruitmentCenter,
					new Action(this.OpenRecruitmentCenter)
				},
				{
					SpaceStationFacility.Shipyard,
					new Action(this.OpenShipyard)
				},
				{
					SpaceStationFacility.MissionBoard,
					new Action(this.OpenMissionBoard)
				},
				{
					SpaceStationFacility.PoliceBoard,
					new Action(this.OpenPatrolBoard)
				},
				{
					SpaceStationFacility.BountyBoard,
					new Action(this.OpenBountyBoard)
				},
				{
					SpaceStationFacility.PersonalHangar,
					new Action(this.OpenPersonalHangar)
				},
				{
					SpaceStationFacility.ExitSpacestation,
					new Action(this.PromptExitSpacestation)
				},
				{
					SpaceStationFacility.Airlock,
					new Action(this.OpenAirlock)
				},
				{
					SpaceStationFacility.BountyShop,
					new Action(this.OpenBountyShop)
				},
				{
					SpaceStationFacility.PatrolShop,
					new Action(this.OpenPatrolShop)
				},
				{
					SpaceStationFacility.IndustryShop,
					new Action(this.OpenIndustryShop)
				},
				{
					SpaceStationFacility.ConquestShop,
					new Action(this.OpenConquestShop)
				},
				{
					SpaceStationFacility.IndustryBoard,
					new Action(this.OpenIndustryBoard)
				},
				{
					SpaceStationFacility.SalvageWorkshop,
					new Action(this.OpenSalvageWorkshop)
				},
				{
					SpaceStationFacility.OutpostAirlock,
					new Action(this.OpenOutpostAirlock)
				}
			};
			this.currentTab = (flag ? SpaceStationFacility.OutpostAirlock : SpaceStationFacility.Airlock);
			this.GoToLocation(this.currentTab, true);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00080020 File Offset: 0x0007E220
		public void SetupCharacters(List<string> charactersStrings)
		{
			this.characters.Clear();
			foreach (string name in charactersStrings)
			{
				this.characters.Add(Characters.GetCharacter(name));
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00080084 File Offset: 0x0007E284
		public void RemoveCharacter(string name)
		{
			this.spacestation.characters.Remove(name);
			this.SetupCharacters(this.spacestation.characters);
			this.GoToLocation(SpaceStationFacility.Airlock, true);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000800B4 File Offset: 0x0007E2B4
		private void Start()
		{
			if (SidePanel.instance.currentTab == SidePanel.SideTabType.Map)
			{
				SidePanel.instance.CloseTab();
			}
			else if (SidePanel.instance.open)
			{
				SidePanel.instance.RefreshIfOpen();
			}
			else
			{
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Inventory, 0);
			}
			HudManager.Instance.ToggleHudElements(false);
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00080109 File Offset: 0x0007E309
		private void OnDestroy()
		{
			SpaceStationInterior.instance = null;
			HudManager hudManager = HudManager.Instance;
			if (hudManager != null)
			{
				hudManager.ToggleHudElements(true);
			}
			GamePlayer current = GamePlayer.current;
			if (current != null && current.autoPlay && !this.sidePanelWasOpen)
			{
				SidePanel.instance.CloseTab();
			}
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00080148 File Offset: 0x0007E348
		private void Update()
		{
			this.creditsText.text = Translation.Translate("@UICredits", Array.Empty<object>()) + ": $" + GameMath.FormatNumber((float)GamePlayer.current.credits, -1);
			if (this.spacestation.RefreshShopsIfNecessary(false) && (this.currentTab == SpaceStationFacility.GeneralShop || this.currentTab == SpaceStationFacility.MiningShop))
			{
				this.GoToLocation(this.currentTab, true);
			}
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.personalHangar.CheckRepairs();
				this.updateTimer = 0.5f;
			}
			if (this.airlockButton)
			{
				this.ShowMissionHighlight();
			}
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00080200 File Offset: 0x0007E400
		private void ShowMissionHighlight()
		{
			bool active = false;
			foreach (Character character in this.characters)
			{
				if (GamePlayer.current.GetCurrentDialogueTrigger(character) != null || character.missionAvailable)
				{
					active = true;
					break;
				}
			}
			this.airlockButton.ToggleMissionHightlight(active);
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0008027C File Offset: 0x0007E47C
		public void ToggleHideUI(bool show)
		{
			base.gameObject.SetActive(show);
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0008028C File Offset: 0x0007E48C
		public void UpdateJobs()
		{
			this.sideContent.DestroyChildren();
			float num = 0f;
			foreach (ISpaceStationJob spaceStationJob in SpaceStation.current.GetJobs())
			{
				if (spaceStationJob.remainingAmount > 0)
				{
					SpaceStationActiveJob spaceStationActiveJob = UnityEngine.Object.Instantiate<SpaceStationActiveJob>(this.jobPrefab, this.sideContent);
					spaceStationActiveJob.SetJob(spaceStationJob);
					(spaceStationActiveJob.transform as RectTransform).anchoredPosition = new Vector2(0f, num);
					num -= 36f;
				}
			}
			this.sideContent.sizeDelta = new Vector2(this.sideContent.sizeDelta.x, -num);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0008034C File Offset: 0x0007E54C
		private void OpenAirlock()
		{
			this.currentAirlock = UnityEngine.Object.Instantiate<Airlock>(this.airlock, this.contentHolder);
			this.currentAirlock.InitCharacters(this.characters);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00080376 File Offset: 0x0007E576
		private void OpenOutpostAirlock()
		{
			UnityEngine.Object.Instantiate<Airlock>(this.outpostAirlock, this.contentHolder);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0008038A File Offset: 0x0007E58A
		private void OpenShop(ShopInventory shop)
		{
			if (SidePanel.instance.currentTab != SidePanel.SideTabType.Inventory)
			{
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Inventory, 0);
			}
			UnityEngine.Object.Instantiate<InventoryShop>(this.shopPrefab, this.contentHolder).SetInventories(shop);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x000803BC File Offset: 0x0007E5BC
		private void OpenGeneralShop()
		{
			this.OpenShop(this.spacestation.generalShopInventory);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x000803CF File Offset: 0x0007E5CF
		private void OpenMiningGuild()
		{
			this.OpenShop(this.spacestation.miningShopInventory);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x000803E2 File Offset: 0x0007E5E2
		private void OpenSalvageGuild()
		{
			this.OpenShop(this.spacestation.salvageShopInventory);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x000803F5 File Offset: 0x0007E5F5
		private void OpenBountyShop()
		{
			this.OpenShop(this.spacestation.bountyShopInventory);
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00080408 File Offset: 0x0007E608
		private void OpenPatrolShop()
		{
			this.OpenShop(this.spacestation.patrolShopInventory);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x0008041B File Offset: 0x0007E61B
		private void OpenIndustryShop()
		{
			this.OpenShop(this.spacestation.industryShopInventory);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0008042E File Offset: 0x0007E62E
		private void OpenConquestShop()
		{
			this.OpenShop(this.spacestation.conquestShopInventory);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00080441 File Offset: 0x0007E641
		private void OpenRefinery()
		{
			UnityEngine.Object.Instantiate<RefineryUI>(this.refinery, this.contentHolder);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00080455 File Offset: 0x0007E655
		private void OpenForge()
		{
			UnityEngine.Object.Instantiate<ForgeUI>(this.forge, this.contentHolder);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00080469 File Offset: 0x0007E669
		private void OpenBar()
		{
			UnityEngine.Object.Instantiate<BarUI>(this.bar, this.contentHolder);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0008047D File Offset: 0x0007E67D
		private void OpenRecruitmentCenter()
		{
			UnityEngine.Object.Instantiate<RecruitmentCenterUI>(this.recruitmentCenter, this.contentHolder);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00080491 File Offset: 0x0007E691
		private void OpenShipyard()
		{
			UnityEngine.Object.Instantiate<Shipyard>(this.shipyard, this.contentHolder);
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x000804A5 File Offset: 0x0007E6A5
		private void OpenMissionBoard()
		{
			UnityEngine.Object.Instantiate<MissionBoard>(this.missionBoard, this.contentHolder);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x000804B9 File Offset: 0x0007E6B9
		private void OpenBountyBoard()
		{
			UnityEngine.Object.Instantiate<BountyBoard>(this.bountyBoard, this.contentHolder);
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000804CD File Offset: 0x0007E6CD
		private void OpenPatrolBoard()
		{
			UnityEngine.Object.Instantiate<PatrolBoard>(this.patrolBoard, this.contentHolder);
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x000804E1 File Offset: 0x0007E6E1
		private void OpenIndustryBoard()
		{
			UnityEngine.Object.Instantiate<IndustryBoard>(this.industryBoard, this.contentHolder);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x000804F5 File Offset: 0x0007E6F5
		private void OpenTradeTerminal()
		{
			UnityEngine.Object.Instantiate<TradeTerminal>(this.tradeTerminal, this.contentHolder);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00080509 File Offset: 0x0007E709
		private void OpenSalvageWorkshop()
		{
			this.workshop = UnityEngine.Object.Instantiate<SalvageWorkshop>(this.salvageWorkshopPrefab, base.transform);
			UnityEngine.Object.Instantiate<Behaviour.UI.Spacestation.Location.PersonalHangar>(this.personalHangar, this.contentHolder);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00080534 File Offset: 0x0007E734
		private void OpenPersonalHangar()
		{
			if (this.automaticallyOpenTab && SidePanel.instance.currentTab != SidePanel.SideTabType.Inventory)
			{
				SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Inventory, 0);
			}
			UnityEngine.Object.Instantiate<Behaviour.UI.Spacestation.Location.PersonalHangar>(this.personalHangar, this.contentHolder);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00080569 File Offset: 0x0007E769
		public bool HasMissionBoard(bool missionCheck = false)
		{
			return this.spacestation.missionBoard != null && (!missionCheck || this.spacestation.faction.offersMissionsForShip);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0008058F File Offset: 0x0007E78F
		public Mission GetMissionAvailableForAutopilot()
		{
			if (this.spacestation.missionBoard == null)
			{
				return null;
			}
			return this.spacestation.missionBoard.GenerateIdleMission();
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000805B0 File Offset: 0x0007E7B0
		private void PromptExitSpacestation()
		{
			if (Singleton<TravelManager>.Instance.CanWeTravel(null) && !GameplayManager.Instance.spaceShip.AmmoInCargoForTurrets(false))
			{
				AlertPopup.ShowQuery("@SSNoAmmoForTurretQuery", null, null, new Action(this.ExitSpacestation), new Action(this.GoToAirlock), null, null);
				return;
			}
			this.ExitSpacestation();
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00080609 File Offset: 0x0007E809
		public void GoToAirlock()
		{
			this.GoToLocation(SpaceStationFacility.Airlock, true);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00080614 File Offset: 0x0007E814
		public void ExitSpacestation()
		{
			if (!Singleton<TravelManager>.Instance.CanWeTravel(null))
			{
				return;
			}
			if (!SpacestationExteriorManager.Instance)
			{
				PersistentSingleton<SceneLoader>.Instance.ToggleSpaceStationInterior(false, false);
				SidePanel.instance.CheckInventoryTabs();
				GameplayManager.Instance.spaceShip.SetEngineState(true, true);
				return;
			}
			if (SpacestationExteriorManager.Instance.undockingRoutine != null)
			{
				string str = "undocking already: ";
				Coroutine undockingRoutine = SpacestationExteriorManager.Instance.undockingRoutine;
				Debug.Log(str + ((undockingRoutine != null) ? undockingRoutine.ToString() : null));
				return;
			}
			this.spacestation.personalHangar.CancelJobLeavingStation(GameplayManager.Instance.spaceShip.spaceShipData.guid, true);
			DroneBayModule droneBayModule = GameplayManager.Instance.spaceShip.droneBayModule;
			if (droneBayModule)
			{
				if (GamePlayer.current.currentSpaceShip.CheckDroneLoadout())
				{
					Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UIDronesChanged", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				}
				droneBayModule.ForceResetDrones();
			}
			PersistentSingleton<SceneLoader>.Instance.ToggleSpaceStationInterior(false, true);
			Debug.Log("Start undocking from SS");
			SpacestationExteriorManager.Instance.StartUndocking();
			SidePanel.instance.CheckInventoryTabs();
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x0008073C File Offset: 0x0007E93C
		public void GoToLocation(SpaceStationFacility facility, bool automaticallyOpenTab = true)
		{
			this.automaticallyOpenTab = automaticallyOpenTab;
			if (facility == SpaceStationFacility.ExitSpacestation && !Singleton<TravelManager>.Instance.CanWeTravel(null))
			{
				return;
			}
			foreach (SpaceStationButton spaceStationButton in this.buttons)
			{
				spaceStationButton.UpdateSelectedFacility(facility);
			}
			bool flag = this.hasFactionIcon && facility == SpaceStationFacility.Airlock;
			this.factionIcon.gameObject.SetActive(flag);
			if (flag && SpaceStation.current.umbralControlLevel > 0f)
			{
				base.StartCoroutine(this.ShowUmbralLogo());
			}
			this.ClearContentHolder();
			Action action = this.tabActions[facility];
			if (action != null)
			{
				action();
			}
			bool flag2 = false;
			foreach (SpaceStationInterior.SpaceStationInteriorArt spaceStationInteriorArt in this.tabArt)
			{
				if (spaceStationInteriorArt.tab == facility)
				{
					this.interactiveArt.sprite = spaceStationInteriorArt.art;
					flag2 = true;
				}
			}
			if (!flag2)
			{
				this.interactiveArt.sprite = this.defaultArt;
			}
			this.currentTab = facility;
			this.SetBgImageEnabled();
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00080884 File Offset: 0x0007EA84
		public void Refresh()
		{
			this.GoToLocation(this.currentTab, true);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x00080893 File Offset: 0x0007EA93
		private void SetBgImageEnabled()
		{
			this.bgimage.enabled = (this.currentTab != SpaceStationFacility.Shipyard && this.currentTab != SpaceStationFacility.PersonalHangar && this.currentTab != SpaceStationFacility.SalvageWorkshop);
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000808C3 File Offset: 0x0007EAC3
		private IEnumerator ShowUmbralLogo()
		{
			while (this.currentTab == SpaceStationFacility.Airlock)
			{
				yield return new WaitForSeconds(SeededRandom.Global.RandomRange(1f, 10f));
				if (!this.factionIcon.gameObject.activeSelf || !this.currentAirlock)
				{
					break;
				}
				this.puppeteersIcon.gameObject.SetActive(true);
				this.currentAirlock.ShowUmbral(true);
				yield return new WaitForSeconds(SeededRandom.Global.RandomRange(0.05f, 0.15f));
				this.puppeteersIcon.gameObject.SetActive(false);
				yield return new WaitForSeconds(1.5f);
				this.currentAirlock.ShowUmbral(false);
			}
			yield break;
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x000808D2 File Offset: 0x0007EAD2
		private void ClearContentHolder()
		{
			this.contentHolder.DestroyChildren();
			this.ClearExtendedHolder();
			this.UpdateJobs();
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x000808EB File Offset: 0x0007EAEB
		private void ClearExtendedHolder()
		{
			if (this.workshop)
			{
				UnityEngine.Object.Destroy(this.workshop.gameObject);
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x0008090C File Offset: 0x0007EB0C
		public void ToggleSpacestationInterior(bool visible)
		{
			foreach (object obj in base.transform)
			{
				((Transform)obj).gameObject.SetActive(visible);
			}
			if (visible)
			{
				this.SetBgImageEnabled();
				return;
			}
			this.bgimage.enabled = false;
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x00080980 File Offset: 0x0007EB80
		public void NewScreenPercentage()
		{
			if (this.currentTab == SpaceStationFacility.PersonalHangar || this.currentTab == SpaceStationFacility.Shipyard || this.currentTab == SpaceStationFacility.SalvageWorkshop)
			{
				base.StartCoroutine(this.AutomaticallySwitchTab(this.currentTab));
			}
			RectTransform rectTransform = base.transform as RectTransform;
			Vector2 anchoredPosition = rectTransform.anchoredPosition;
			if (ScreenSettings.doWeStackUI)
			{
				anchoredPosition.x = 0f;
			}
			else
			{
				anchoredPosition.x = -768f;
			}
			rectTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x000809F6 File Offset: 0x0007EBF6
		public IEnumerator AutomaticallySwitchTab(SpaceStationFacility newTab)
		{
			this.automaticallyOpenTab = false;
			this.ClearContentHolder();
			yield return new WaitForEndOfFrame();
			this.GoToLocation(newTab, this.automaticallyOpenTab);
			yield break;
		}

		// Token: 0x04000B4C RID: 2892
		public static SpaceStationInterior instance;

		// Token: 0x04000B4E RID: 2894
		private List<Character> characters = new List<Character>();

		// Token: 0x04000B4F RID: 2895
		[SerializeField]
		private SpaceStationButton airlockButton;

		// Token: 0x04000B50 RID: 2896
		[SerializeField]
		private SpaceStationButton tabButtonPrefab;

		// Token: 0x04000B51 RID: 2897
		[SerializeField]
		private RectTransform tabParent;

		// Token: 0x04000B52 RID: 2898
		[SerializeField]
		private InventoryShop shopPrefab;

		// Token: 0x04000B53 RID: 2899
		[SerializeField]
		private Airlock airlock;

		// Token: 0x04000B54 RID: 2900
		[SerializeField]
		private Airlock outpostAirlock;

		// Token: 0x04000B55 RID: 2901
		[SerializeField]
		private RefineryUI refinery;

		// Token: 0x04000B56 RID: 2902
		[SerializeField]
		private ForgeUI forge;

		// Token: 0x04000B57 RID: 2903
		[SerializeField]
		private Shipyard shipyard;

		// Token: 0x04000B58 RID: 2904
		[SerializeField]
		private MissionBoard missionBoard;

		// Token: 0x04000B59 RID: 2905
		[SerializeField]
		private Behaviour.UI.Spacestation.Location.PersonalHangar personalHangar;

		// Token: 0x04000B5A RID: 2906
		[SerializeField]
		private BarUI bar;

		// Token: 0x04000B5B RID: 2907
		[SerializeField]
		private RecruitmentCenterUI recruitmentCenter;

		// Token: 0x04000B5C RID: 2908
		[SerializeField]
		private BountyBoard bountyBoard;

		// Token: 0x04000B5D RID: 2909
		[SerializeField]
		private PatrolBoard patrolBoard;

		// Token: 0x04000B5E RID: 2910
		[SerializeField]
		private IndustryBoard industryBoard;

		// Token: 0x04000B5F RID: 2911
		[SerializeField]
		private TradeTerminal tradeTerminal;

		// Token: 0x04000B60 RID: 2912
		[SerializeField]
		private SalvageWorkshop salvageWorkshopPrefab;

		// Token: 0x04000B61 RID: 2913
		[SerializeField]
		private RectTransform contentHolder;

		// Token: 0x04000B62 RID: 2914
		[SerializeField]
		private Image interactiveArt;

		// Token: 0x04000B63 RID: 2915
		[SerializeField]
		private Image factionIcon;

		// Token: 0x04000B64 RID: 2916
		[SerializeField]
		private Image puppeteersIcon;

		// Token: 0x04000B65 RID: 2917
		[SerializeField]
		private List<SpaceStationInterior.SpaceStationInteriorArt> tabArt;

		// Token: 0x04000B66 RID: 2918
		[SerializeField]
		private Image bgimage;

		// Token: 0x04000B67 RID: 2919
		[SerializeField]
		private TMP_Text creditsText;

		// Token: 0x04000B68 RID: 2920
		[SerializeField]
		private SpaceStationActiveJob jobPrefab;

		// Token: 0x04000B6B RID: 2923
		private Dictionary<SpaceStationFacility, Action> tabActions;

		// Token: 0x04000B6E RID: 2926
		public SalvageWorkshop workshop;

		// Token: 0x04000B6F RID: 2927
		private Sprite defaultArt;

		// Token: 0x04000B70 RID: 2928
		public bool automaticallyOpenTab = true;

		// Token: 0x04000B71 RID: 2929
		private bool sidePanelWasOpen;

		// Token: 0x04000B72 RID: 2930
		private bool hasFactionIcon;

		// Token: 0x04000B73 RID: 2931
		private float updateTimer;

		// Token: 0x04000B74 RID: 2932
		private List<SpaceStationButton> buttons;

		// Token: 0x0200050D RID: 1293
		[Serializable]
		public class SpaceStationInteriorArt
		{
			// Token: 0x1700066D RID: 1645
			// (get) Token: 0x06002B22 RID: 11042 RVA: 0x000ED63B File Offset: 0x000EB83B
			// (set) Token: 0x06002B23 RID: 11043 RVA: 0x000ED643 File Offset: 0x000EB843
			public SpaceStationFacility tab { get; private set; }

			// Token: 0x1700066E RID: 1646
			// (get) Token: 0x06002B24 RID: 11044 RVA: 0x000ED64C File Offset: 0x000EB84C
			// (set) Token: 0x06002B25 RID: 11045 RVA: 0x000ED654 File Offset: 0x000EB854
			public Sprite art { get; private set; }
		}
	}
}
