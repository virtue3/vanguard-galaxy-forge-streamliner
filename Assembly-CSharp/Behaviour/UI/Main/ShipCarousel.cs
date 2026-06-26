using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Transparency;
using Behaviour.UI.HUD;
using Behaviour.UI.ShipCarousel;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Spacestation.Hangar;
using Behaviour.Unit;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Galaxy.POI.Station;
using Source.Item;
using Source.Player;
using Source.Simulation;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Behaviour.UI.Main
{
	// Token: 0x0200026A RID: 618
	public class ShipCarousel : MonoBehaviour
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x0008FA17 File Offset: 0x0008DC17
		public bool crewViewOpen
		{
			get
			{
				return this.crewView.gameObject.activeSelf;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x0008FA29 File Offset: 0x0008DC29
		public Camera CarouselCamera
		{
			get
			{
				return this.carouselCamera;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600169F RID: 5791 RVA: 0x0008FA34 File Offset: 0x0008DC34
		public SpaceShipData selectedShipData
		{
			get
			{
				if (this.ships == null || this.ships.Count <= 0 || this.selectedIndex < 0 || this.selectedIndex >= this.ships.Count)
				{
					return null;
				}
				return this.ships[this.selectedIndex];
			}
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0008FA86 File Offset: 0x0008DC86
		private void Awake()
		{
			this.UpdateCameraViewport();
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x0008FA90 File Offset: 0x0008DC90
		private void Start()
		{
			this.ShowModulesTab();
			SpaceStationInterior componentInParent = base.GetComponentInParent<SpaceStationInterior>();
			if (componentInParent)
			{
				componentInParent.automaticallyOpenTab = true;
			}
			this.startCalled = true;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0008FAC0 File Offset: 0x0008DCC0
		private void Update()
		{
			if (this.selectedShipData == null || !this.selectedShip)
			{
				return;
			}
			this.UpdateHP();
			this.UpdateBars();
			this.HandleZoom();
			this.HandlePan();
			GameObject gameObject = this.dronesButton.gameObject;
			SpaceShipData selectedShipData = this.selectedShipData;
			gameObject.SetActive(((selectedShipData != null) ? selectedShipData.GetEquippedItem(EquipmentSlot.DroneBay) : null) != null);
			if (!this.dronesButton.gameObject.activeSelf)
			{
				this.dronesUI.gameObject.SetActive(false);
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0008FB48 File Offset: 0x0008DD48
		private void HandlePan()
		{
			bool flag = RectTransformUtility.RectangleContainsScreenPoint(base.transform as RectTransform, Input.mousePosition);
			bool flag2 = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			if (flag && flag2 && Input.GetMouseButtonDown(1))
			{
				this._isPanning = true;
				this._panMouseStart = Input.mousePosition;
				this._panCamStart = this.carouselCamera.transform.position;
			}
			if (Input.GetMouseButtonUp(1) || !flag2)
			{
				this._isPanning = false;
			}
			if (!this._isPanning || !Input.GetMouseButton(1))
			{
				return;
			}
			Vector2 vector = (Vector2)Input.mousePosition - this._panMouseStart;
			float num = this.carouselCamera.orthographicSize * 2f / (this.cameraViewport.height * (float)Screen.height) * this.panSpeed;
			Vector3 vector2 = this._panCamStart - new Vector3(vector.x * num, vector.y * num, 0f);
			vector2.x = Mathf.Clamp(vector2.x, this.shipWorldPosition.x - this.maxPanDistance, this.shipWorldPosition.x + this.maxPanDistance);
			vector2.y = Mathf.Clamp(vector2.y, this.shipWorldPosition.y - this.maxPanDistance, this.shipWorldPosition.y + this.maxPanDistance);
			this.carouselCamera.transform.position = vector2;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x0008FCCC File Offset: 0x0008DECC
		public void HandleZoom()
		{
			if (!RectTransformUtility.RectangleContainsScreenPoint(base.transform as RectTransform, Input.mousePosition))
			{
				return;
			}
			if (DroneLoadoutUI.active)
			{
				return;
			}
			float y = Input.mouseScrollDelta.y;
			if (y != 0f)
			{
				float num = this.carouselCamera.orthographicSize - y * this.zoomSpeed * 0.1f;
				num = Mathf.Clamp(num, this.minZoom, this.maxZoom);
				this.carouselCamera.orthographicSize = num;
			}
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x0008FD4B File Offset: 0x0008DF4B
		public void ChangeNameButton()
		{
			this.CreatePopup(delegate(string input)
			{
				this.SetCustomShipName(input);
				if (SpaceStationInterior.instance && SpaceStationInterior.instance.currentTab == SpaceStationFacility.PersonalHangar)
				{
					SpaceStationInterior.instance.Refresh();
					return;
				}
				SidePanel instance = SidePanel.instance;
				if (instance == null)
				{
					return;
				}
				instance.RefreshIfOpen();
			});
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x0008FD5F File Offset: 0x0008DF5F
		public void HideChangeNameButton()
		{
			this.shipNameButton.gameObject.SetActive(false);
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x0008FD72 File Offset: 0x0008DF72
		private void CreatePopup(Action<string> onSubmitAction)
		{
			AlertPopup.ShowInput("@UIShipName", onSubmitAction, "@UIChange", this.selectedShipData.customShipName, true, null, null);
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x0008FD92 File Offset: 0x0008DF92
		private void SetCustomShipName(string shipName)
		{
			this.selectedShipData.customShipName = shipName;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x0008FDA0 File Offset: 0x0008DFA0
		private void UpdateHP()
		{
			if (this.hullHP && this.selectedShipData != null)
			{
				this.hullHP.SetValue(GameMath.FormatNumber(this.selectedShipData.currentHullHP, -1));
			}
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x0008FDD3 File Offset: 0x0008DFD3
		private void UpdateBackgroundSize()
		{
			if (!this.hangarBackground)
			{
				return;
			}
			this.hangarBackground.size = new Vector2(80f, 50f);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x0008FE00 File Offset: 0x0008E000
		private void UpdateCameraViewport()
		{
			float width = (base.transform.parent.transform as RectTransform).rect.width;
			float scaleFactor = GameplayerPrefs.GetScaleFactor();
			this.cameraViewport.width = width * scaleFactor / (float)Screen.width;
			this.cameraViewport.height = (ScreenSettings.doWeStackUI ? (ScreenSettings.clickableScreenPercentage / 2f) : ScreenSettings.clickableScreenPercentage);
			this.cameraViewport.center = new Vector2(this.shipSelectHolder.position.x / (float)Screen.width, this.shipSelectHolder.position.y / (float)Screen.height);
			this.carouselCamera.rect = this.cameraViewport;
			Camera camera = this.carouselCamera;
			float num = ScreenSettings.fullscreen ? 2f : 1.4f;
			SpaceShip spaceShip = this.selectedShip;
			camera.orthographicSize = num / ((spaceShip != null) ? spaceShip.shipyardScale : 1f);
			this.UpdateBackgroundSize();
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x0008FEF9 File Offset: 0x0008E0F9
		public void SetCameraOverride(bool useCamera)
		{
			this.useCamera = useCamera;
			this.carouselCamera.enabled = this.useCamera;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x0008FF13 File Offset: 0x0008E113
		private void OnDisable()
		{
			this.HideShip();
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0008FF1B File Offset: 0x0008E11B
		private void OnEnable()
		{
			this.ShowShip(false);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0008FF24 File Offset: 0x0008E124
		public void SetSelectCallback(Action callback)
		{
			this.selectedShipCallback = callback;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0008FF30 File Offset: 0x0008E130
		public void SetStarterShips(List<string> shipNames)
		{
			this.ships.Clear();
			foreach (string shipName in shipNames)
			{
				this.AddSpaceShipData(shipName, "vg2");
			}
			base.StartCoroutine(this.DelayedShowShip());
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0008FF9C File Offset: 0x0008E19C
		private void AddSpaceShipData(string shipName, string seed = null)
		{
			SpaceShipData spaceShipData = new SpaceShipData(SpaceShip.Get(shipName), true, null)
			{
				faction = Faction.blue
			};
			int level;
			if (MapPointOfInterest.current is EmbassyStation)
			{
				level = Mathf.Min(GamePlayer.current.level, 60);
			}
			else
			{
				MapPointOfInterest current = MapPointOfInterest.current;
				level = ((current != null) ? current.level : 1);
			}
			spaceShipData.LoadDefaultEquipment(level, 0f, seed, null, null, null, false, null);
			this.ships.Add(spaceShipData);
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x0009002C File Offset: 0x0008E22C
		public void SetNewShips(List<ShipyardShip> shipyardShip, string seed)
		{
			this.ships.Clear();
			foreach (ShipyardShip shipyardShip2 in shipyardShip)
			{
				bool flag = true;
				using (List<Storyteller>.Enumerator enumerator2 = GamePlayer.current.storytellers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (!enumerator2.Current.ShipIsPlayerAvailable(shipyardShip2.name))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					this.AddSpaceShipData(shipyardShip2.name, shipyardShip2.name + seed);
				}
			}
			base.StartCoroutine(this.DelayedShowShip());
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000900FC File Offset: 0x0008E2FC
		public void SetPlayerShip()
		{
			this.ships.Clear();
			this.ships.Add(GamePlayer.current.currentSpaceShip);
			this.selectedIndex = 0;
			this.ShowShip(false);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0009012C File Offset: 0x0008E32C
		public void SetPlayerShips(List<SpaceShipData> spaceShipData)
		{
			this.ships.Clear();
			int num = 0;
			foreach (SpaceShipData spaceShipData2 in spaceShipData)
			{
				this.ships.Add(spaceShipData2);
				if (spaceShipData2 == GamePlayer.current.currentSpaceShip)
				{
					this.selectedIndex = num;
				}
				num++;
			}
			base.StartCoroutine(this.DelayedShowShip());
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000901B4 File Offset: 0x0008E3B4
		public void RemoveShip(SpaceShipData spaceShipData)
		{
			this.NextIcon();
			this.ships.Remove(spaceShipData);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x000901C9 File Offset: 0x0008E3C9
		public void NextIcon()
		{
			if (this.ships.Count > 0)
			{
				this.selectedIndex = (this.selectedIndex + 1) % this.ships.Count;
				this.ShowShip(false);
			}
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x000901FA File Offset: 0x0008E3FA
		public void PreviousIcon()
		{
			if (this.ships.Count > 0)
			{
				this.selectedIndex = (this.selectedIndex - 1 + this.ships.Count) % this.ships.Count;
				this.ShowShip(false);
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00090237 File Offset: 0x0008E437
		private void HideCarouselButtons()
		{
			this.previousButton.gameObject.SetActive(false);
			this.nextButton.gameObject.SetActive(false);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0009025B File Offset: 0x0008E45B
		public IEnumerator DelayedShowShip()
		{
			yield return new WaitUntil(() => this.startCalled);
			this.ShowShip(false);
			yield break;
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0009026C File Offset: 0x0008E46C
		public void ShowShip(bool refresh = false)
		{
			if (this.ships.Count == 1)
			{
				this.HideCarouselButtons();
			}
			if (this.selectedShipData == null)
			{
				return;
			}
			if (!refresh && this.selectedShip && this.selectedShip.spaceShipData == this.selectedShipData)
			{
				return;
			}
			if (this.selectedShip)
			{
				UnityEngine.Object.Destroy(this.selectedShip.gameObject);
			}
			this.dronesUI.gameObject.SetActive(false);
			Vector3 vector = Camera.main.ScreenToWorldPoint(this.shipSelectHolder.position);
			this.shipWorldPosition = new Vector2(0f, -0.14f);
			if (this.useCamera)
			{
				this.shipWorldPosition = new Vector2(vector.x + 10000f, vector.y + SeededRandom.Global.RandomRange(10000f, 20000f));
			}
			else
			{
				this.SetMainCameraPos();
			}
			SpaceShip spaceShip = this.selectedShipData.shipClass;
			this.selectedShip = UnityEngine.Object.Instantiate<SpaceShip>(spaceShip, this.shipWorldPosition, Quaternion.identity, base.transform);
			this.selectedShip.SetInShipyard(this.shipWorldPosition);
			this.sunlight.transform.position = new Vector3(this.shipWorldPosition.x + 5f, this.shipWorldPosition.y + 10f, 0f);
			if (spaceShip.displayName == null)
			{
				this.selectedShip.SetDisplayName(spaceShip.name);
			}
			this.selectedShipData.faction = Faction.player;
			this.selectedShip.SetData(this.selectedShipData, false, true);
			this.selectedShip.InitModules();
			SpaceShipData selectedShipData = this.selectedShipData;
			GamePlayer current = GamePlayer.current;
			if (selectedShipData == ((current != null) ? current.currentSpaceShip : null))
			{
				this.selectedShip.spriteRenderer.sprite = GameplayManager.Instance.spaceShip.spriteRenderer.sprite;
			}
			Action action = this.selectedShipCallback;
			if (action != null)
			{
				action();
			}
			this.PopulateEquipmentSlots();
			this.PopulateBadges();
			this.manufacturer.text = this.selectedShip.manufacturer.GetDisplayName();
			if (this.shipClass)
			{
				this.shipClass.SetShipClass(this.selectedShip);
			}
			if (this.hullHP)
			{
				this.hullHP.SetContent(string.Format("{0}", this.selectedShipData.currentHullHP), "@EquipStatHullHP", "@EquipStatHullHPDescription");
			}
			if (this.cargoCapacity)
			{
				this.cargoCapacity.SetContent(string.Format("{0}", this.selectedShipData.shipClass._cargoCapacity), "@EquipStatCargoCapacity", "@EquipStatCargoCapacityDescription");
			}
			if (this.tonnage)
			{
				this.tonnage.SetContent(string.Format("{0}", this.selectedShipData.shipClass.tonnage), "@ShipSelectTonnage", "@ShipSelectTonnageDescription");
			}
			if (this.useCamera)
			{
				this.carouselCamera.transform.position = new Vector3(this.shipWorldPosition.x, this.shipWorldPosition.y, this.carouselCamera.transform.position.z);
				this.carouselCamera.rect = this.cameraViewport;
				this.UpdateCameraViewport();
			}
			this._isPanning = false;
			if (this.hangarBackground)
			{
				this.hangarBackground.transform.position = new Vector3(this.shipWorldPosition.x, this.shipWorldPosition.y, 0.5f);
				this.hangarBackground.transform.localScale = Vector3.one;
				this.hangarBackground.drawMode = SpriteDrawMode.Tiled;
				this.UpdateBackgroundSize();
			}
			this.SetHealth();
			EnergyUsageBar energyUsageBar = this.energyBar;
			if (energyUsageBar == null)
			{
				return;
			}
			energyUsageBar.SetEnergyBar(this.selectedShip);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0009064E File Offset: 0x0008E84E
		public void SetMainCameraPos()
		{
			Camera.main.transform.position = new Vector3(0f, ScreenSettings.fullscreen ? 3.75f : 0f, -10f);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00090681 File Offset: 0x0008E881
		public void PopulateEquipmentSlots()
		{
			this.PopulateModuleSlots();
			this.PopulateBoosterSlots();
			this.PopulateCrewSlots();
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00090698 File Offset: 0x0008E898
		public void PopulateBadges()
		{
			Badge[] componentsInChildren = this.badgeContainer.GetComponentsInChildren<Badge>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
			foreach (BonusBadge bonusBadge in this.selectedShip.GetComponentsInChildren<BonusBadge>())
			{
				Badge original = this.hullBonusBadgePrefab;
				if (bonusBadge.bonusType == BonusType.RangeBonus)
				{
					original = this.rangeBadgePrefab;
				}
				else if (bonusBadge.bonusType == BonusType.Manufacturer)
				{
					original = this.manufacturorBadgePrefab;
				}
				Badge badge = UnityEngine.Object.Instantiate<Badge>(original, this.badgeContainer.transform);
				if (bonusBadge.bonusType == BonusType.HullBonus)
				{
					badge.GetComponent<Image>().color = this.GetBadgeColor(this.selectedShip.shipRoleType.GetGameplayType());
				}
				List<string> list = new List<string>();
				IEquipStatSource[] components = bonusBadge.GetComponents<IEquipStatSource>();
				for (int j = 0; j < components.Length; j++)
				{
					foreach (EquipStatLine equipStatLine in components[j].GetStats())
					{
						list.Add(equipStatLine.ToReadableString(false));
					}
				}
				badge.SetBadgeData(this.selectedShip, bonusBadge.bonusType, list);
			}
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000907E4 File Offset: 0x0008E9E4
		private Color GetBadgeColor(GameplayType type)
		{
			Color result;
			switch (type)
			{
			case GameplayType.Combat:
				result = new Color(1f, 0.5f, 0.5f);
				break;
			case GameplayType.Mining:
				result = new Color(0.65f, 0.65f, 1f);
				break;
			case GameplayType.Salvage:
				result = new Color(1f, 0.85f, 0.5f);
				break;
			default:
				result = Color.aquamarine;
				break;
			}
			return result;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x00090854 File Offset: 0x0008EA54
		private void SetHealth()
		{
			if (this.selectedShipData == null)
			{
				return;
			}
			float maxHullHP = this.selectedShip.maxHullHP;
			float maxShieldHP = this.selectedShip.maxShieldHP;
			float maxArmorHP = this.selectedShip.maxArmorHP;
			if (maxHullHP > 0f)
			{
				this.healthBar.InitBar(maxHullHP, this.selectedShipData.currentHullHP, this.selectedShip.hullHPScale, BarType.Hull);
			}
			if (maxShieldHP > 0f)
			{
				this.shieldBar.gameObject.SetActive(true);
				this.shieldBar.InitBar(maxShieldHP, this.selectedShipData.currentShieldHP, this.selectedShip.shieldHPScale, BarType.Shield);
			}
			else if (this.shieldBar)
			{
				this.shieldBar.gameObject.SetActive(false);
			}
			if (maxArmorHP > 0f)
			{
				this.armorBar.gameObject.SetActive(true);
				this.armorBar.InitBar(maxArmorHP, this.selectedShipData.currentArmorHP, this.selectedShip.armorHPScale, BarType.Armor);
				return;
			}
			if (this.armorBar)
			{
				this.armorBar.gameObject.SetActive(false);
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x00090970 File Offset: 0x0008EB70
		public void UpdateHealth()
		{
			this.healthBar.SetHealth(this.selectedShip.currentHullHP, this.selectedShip.maxHullHP);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x00090993 File Offset: 0x0008EB93
		public void UpdateShield()
		{
			this.shieldBar.SetHealth(this.selectedShip.currentShieldHP, this.selectedShip.maxShieldHP);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x000909B6 File Offset: 0x0008EBB6
		public void UpdateArmor()
		{
			this.armorBar.SetHealth(this.selectedShip.currentArmorHP, this.selectedShip.maxArmorHP);
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x000909DC File Offset: 0x0008EBDC
		private void UpdateBars()
		{
			if (this.selectedShipData == null)
			{
				return;
			}
			if (this.healthBar && this.healthBar.isActiveAndEnabled)
			{
				this.UpdateHealth();
			}
			if (this.shieldBar && this.shieldBar.isActiveAndEnabled)
			{
				this.UpdateShield();
			}
			if (this.armorBar && this.armorBar.isActiveAndEnabled)
			{
				this.UpdateArmor();
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00090A52 File Offset: 0x0008EC52
		public void HideShip()
		{
			if (this.selectedShip)
			{
				UnityEngine.Object.Destroy(this.selectedShip.gameObject);
			}
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x00090A74 File Offset: 0x0008EC74
		public void PopulateModuleSlots()
		{
			SpaceShipData spaceShipData = this.selectedShip.spaceShipData;
			this.moduleContainer.transform.DestroyChildren();
			this.moduleButtons.Clear();
			foreach (SpaceShipModule spaceShipModule in this.selectedShip.moduleSlots)
			{
				InventoryItemType equippedItem = spaceShipData.GetEquippedItem(spaceShipModule.slot);
				ModuleButton moduleButton = UnityEngine.Object.Instantiate<ModuleButton>(this.moduleButton, this.moduleContainer.transform);
				moduleButton.inPersonalHangar = this.inPersonalHangar;
				moduleButton.SetItem<SpaceShipModule>(equippedItem, spaceShipModule);
				this.moduleButtons.Add(moduleButton);
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x00090B10 File Offset: 0x0008ED10
		public void PopulateBoosterSlots()
		{
			SpaceShipData spaceShipData = this.selectedShip.spaceShipData;
			this.boosterContainer.transform.DestroyChildren();
			this.boosterButtons.Clear();
			for (int i = 0; i < this.selectedShip.boosterSlots.Length; i++)
			{
				InventoryItemType item = spaceShipData.boosters[i];
				BoosterButton boosterButton = UnityEngine.Object.Instantiate<BoosterButton>(this.boosterButton, this.boosterContainer.transform);
				boosterButton.inPersonalHangar = this.inPersonalHangar;
				boosterButton.index = i;
				boosterButton.SetItem<SpaceShipBooster>(item, this.selectedShip.boosterSlots[i]);
				this.boosterButtons.Add(boosterButton);
			}
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x00090BB0 File Offset: 0x0008EDB0
		public void PopulateCrewSlots()
		{
			SpaceShipData spaceShipData = this.selectedShip.spaceShipData;
			this.crewContainer.transform.DestroyChildren();
			this.crewButtons.Clear();
			for (int i = 0; i < this.selectedShip.maxOfficers; i++)
			{
				CrewButton crewButton = UnityEngine.Object.Instantiate<CrewButton>(this.crewButton, this.crewContainer.transform);
				crewButton.index = i;
				crewButton.SetCrew(spaceShipData.crewMembers[i]);
				this.crewButtons.Add(crewButton);
			}
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x00090C34 File Offset: 0x0008EE34
		public void ShowCrewTab()
		{
			this.crewContainer.gameObject.SetActive(true);
			if (GamePlayer.current.commander.LeadershipUnlocked())
			{
				this.crewView.gameObject.SetActive(true);
				this.PopulateCrewView();
			}
			this.dronesUI.gameObject.SetActive(false);
			this.boosterContainer.gameObject.SetActive(false);
			this.moduleContainer.gameObject.SetActive(false);
			SpaceStationInterior componentInParent = base.GetComponentInParent<SpaceStationInterior>();
			if (componentInParent != null && componentInParent.automaticallyOpenTab)
			{
				SidePanel instance = SidePanel.instance;
				if (instance == null || instance.currentTab != SidePanel.SideTabType.Crew)
				{
					SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Crew, 0);
				}
			}
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x00090CE8 File Offset: 0x0008EEE8
		public void ShowBoostersTab()
		{
			this.crewContainer.gameObject.SetActive(false);
			this.crewView.gameObject.SetActive(false);
			this.dronesUI.gameObject.SetActive(false);
			this.boosterContainer.gameObject.SetActive(true);
			this.moduleContainer.gameObject.SetActive(false);
			SpaceStationInterior componentInParent = base.GetComponentInParent<SpaceStationInterior>();
			if (componentInParent != null && componentInParent.automaticallyOpenTab)
			{
				SidePanel instance = SidePanel.instance;
				if (instance == null || instance.currentTab != SidePanel.SideTabType.Inventory)
				{
					SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Inventory, 0);
				}
			}
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00090D84 File Offset: 0x0008EF84
		public void ShowModulesTab()
		{
			this.crewContainer.gameObject.SetActive(false);
			this.crewView.gameObject.SetActive(false);
			this.dronesUI.gameObject.SetActive(false);
			this.boosterContainer.gameObject.SetActive(false);
			this.moduleContainer.gameObject.SetActive(true);
			SpaceStationInterior componentInParent = base.GetComponentInParent<SpaceStationInterior>();
			if (componentInParent != null && componentInParent.automaticallyOpenTab)
			{
				SidePanel instance = SidePanel.instance;
				if (instance == null || instance.currentTab != SidePanel.SideTabType.Inventory)
				{
					SidePanel.instance.SwitchToTab(SidePanel.SideTabType.Inventory, 0);
				}
			}
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00090E1F File Offset: 0x0008F01F
		public void PopulateCrewView()
		{
			this.crewView.PopulateCrew();
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00090E2C File Offset: 0x0008F02C
		public void ToggleDroneUI()
		{
			if (this.inPersonalHangar && this.dronesUI.gameObject.activeInHierarchy && this.selectedShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.dronesUI.gameObject.SetActive(!this.dronesUI.gameObject.activeSelf);
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x00090E92 File Offset: 0x0008F092
		private void OnDestroy()
		{
			if (this.selectedShip)
			{
				UnityEngine.Object.Destroy(this.selectedShip.gameObject);
			}
		}

		// Token: 0x04000DD8 RID: 3544
		[SerializeField]
		private ShipSelectButton shipSelectButton;

		// Token: 0x04000DD9 RID: 3545
		[SerializeField]
		private RectTransform shipSelectHolder;

		// Token: 0x04000DDA RID: 3546
		[SerializeField]
		private GameObject moduleContainer;

		// Token: 0x04000DDB RID: 3547
		[SerializeField]
		private ModuleButton moduleButton;

		// Token: 0x04000DDC RID: 3548
		[SerializeField]
		private GameObject boosterContainer;

		// Token: 0x04000DDD RID: 3549
		[SerializeField]
		private BoosterButton boosterButton;

		// Token: 0x04000DDE RID: 3550
		[SerializeField]
		private GameObject crewContainer;

		// Token: 0x04000DDF RID: 3551
		[SerializeField]
		private CrewButton crewButton;

		// Token: 0x04000DE0 RID: 3552
		[SerializeField]
		private CrewView crewView;

		// Token: 0x04000DE1 RID: 3553
		[SerializeField]
		private Button previousButton;

		// Token: 0x04000DE2 RID: 3554
		[SerializeField]
		private Button nextButton;

		// Token: 0x04000DE3 RID: 3555
		[SerializeField]
		private Camera carouselCamera;

		// Token: 0x04000DE4 RID: 3556
		[SerializeField]
		private Rect cameraViewport;

		// Token: 0x04000DE5 RID: 3557
		[SerializeField]
		private Light2D sunlight;

		// Token: 0x04000DE6 RID: 3558
		[SerializeField]
		private EnergyUsageBar energyBar;

		// Token: 0x04000DE7 RID: 3559
		[SerializeField]
		private GameObject badgeContainer;

		// Token: 0x04000DE8 RID: 3560
		[SerializeField]
		private Badge hullBonusBadgePrefab;

		// Token: 0x04000DE9 RID: 3561
		[SerializeField]
		private Badge rangeBadgePrefab;

		// Token: 0x04000DEA RID: 3562
		[SerializeField]
		private Badge manufacturorBadgePrefab;

		// Token: 0x04000DEB RID: 3563
		[SerializeField]
		private float zoomSpeed;

		// Token: 0x04000DEC RID: 3564
		[SerializeField]
		private float minZoom;

		// Token: 0x04000DED RID: 3565
		[SerializeField]
		private float maxZoom;

		// Token: 0x04000DEE RID: 3566
		[Header("Pan")]
		[SerializeField]
		private float panSpeed = 2f;

		// Token: 0x04000DEF RID: 3567
		[SerializeField]
		private float maxPanDistance = 4f;

		// Token: 0x04000DF0 RID: 3568
		[SerializeField]
		private SpriteRenderer hangarBackground;

		// Token: 0x04000DF1 RID: 3569
		private bool _isPanning;

		// Token: 0x04000DF2 RID: 3570
		private Vector2 _panMouseStart;

		// Token: 0x04000DF3 RID: 3571
		private Vector3 _panCamStart;

		// Token: 0x04000DF4 RID: 3572
		[SerializeField]
		private Button dronesButton;

		// Token: 0x04000DF5 RID: 3573
		[SerializeField]
		private DroneLoadoutUI dronesUI;

		// Token: 0x04000DF6 RID: 3574
		public bool inPersonalHangar;

		// Token: 0x04000DF7 RID: 3575
		public bool droneSwapAvailable;

		// Token: 0x04000DF8 RID: 3576
		public List<SpaceShipData> ships = new List<SpaceShipData>();

		// Token: 0x04000DF9 RID: 3577
		public int selectedIndex;

		// Token: 0x04000DFA RID: 3578
		public SpaceShip selectedShip;

		// Token: 0x04000DFB RID: 3579
		public List<ModuleButton> moduleButtons = new List<ModuleButton>();

		// Token: 0x04000DFC RID: 3580
		public List<BoosterButton> boosterButtons = new List<BoosterButton>();

		// Token: 0x04000DFD RID: 3581
		public List<CrewButton> crewButtons = new List<CrewButton>();

		// Token: 0x04000DFE RID: 3582
		[SerializeField]
		private Image spaceshipImage;

		// Token: 0x04000DFF RID: 3583
		[SerializeField]
		private TextMeshProUGUI manufacturer;

		// Token: 0x04000E00 RID: 3584
		[SerializeField]
		private ShipInfoExpanded shipClass;

		// Token: 0x04000E01 RID: 3585
		[SerializeField]
		private ShipInfoIcon hullHP;

		// Token: 0x04000E02 RID: 3586
		[SerializeField]
		private ShipInfoIcon cargoCapacity;

		// Token: 0x04000E03 RID: 3587
		[SerializeField]
		private ShipInfoIcon tonnage;

		// Token: 0x04000E04 RID: 3588
		[SerializeField]
		private AmountBar healthBar;

		// Token: 0x04000E05 RID: 3589
		[SerializeField]
		private AmountBar shieldBar;

		// Token: 0x04000E06 RID: 3590
		[SerializeField]
		private AmountBar armorBar;

		// Token: 0x04000E07 RID: 3591
		private Action selectedShipCallback;

		// Token: 0x04000E08 RID: 3592
		private bool useCamera = true;

		// Token: 0x04000E09 RID: 3593
		private bool startCalled;

		// Token: 0x04000E0A RID: 3594
		private Vector2 shipWorldPosition;

		// Token: 0x04000E0B RID: 3595
		[SerializeField]
		private Button shipNameButton;
	}
}
