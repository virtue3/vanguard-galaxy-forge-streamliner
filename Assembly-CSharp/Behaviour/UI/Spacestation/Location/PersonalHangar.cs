using System;
using System.Collections.Generic;
using System.Linq;
using Behavior.Equipment.Booster;
using Behaviour.Equipment;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Module.DroneBay.OpeningMechanism;
using Behaviour.Equipment.Turret;
using Behaviour.Equipment.Turret.CombatTurrets;
using Behaviour.Item;
using Behaviour.UI.Main;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Spacestation.Hangar;
using Behaviour.UI.Tooltip;
using Behaviour.Unit;
using Behaviour.Util;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.MissionSystem;
using Source.Player;
using Source.SpaceShip;
using Source.Spacestation;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000223 RID: 547
	public class PersonalHangar : MonoBehaviour
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600143A RID: 5178 RVA: 0x00082A6D File Offset: 0x00080C6D
		// (set) Token: 0x0600143B RID: 5179 RVA: 0x00082A74 File Offset: 0x00080C74
		public static PersonalHangar current { get; private set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600143C RID: 5180 RVA: 0x00082A7C File Offset: 0x00080C7C
		// (set) Token: 0x0600143D RID: 5181 RVA: 0x00082A84 File Offset: 0x00080C84
		public Behaviour.UI.Main.ShipCarousel shipSelect { get; private set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00082A8D File Offset: 0x00080C8D
		// (set) Token: 0x0600143F RID: 5183 RVA: 0x00082A95 File Offset: 0x00080C95
		public SpaceShipData selectedShipData { get; private set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x00082A9E File Offset: 0x00080C9E
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x00082AA6 File Offset: 0x00080CA6
		public bool decalModeActive { get; private set; }

		// Token: 0x06001442 RID: 5186 RVA: 0x00082AAF File Offset: 0x00080CAF
		private void Awake()
		{
			InventoryInteractionManager.Instance.RegisterPersonalHanger(this);
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00082ABC File Offset: 0x00080CBC
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.1f;
				this.repairCost.text = "$ " + this.GetRepairCost().ToString();
			}
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00082B18 File Offset: 0x00080D18
		private int GetRepairCost()
		{
			if (!this.shipSelect.selectedShip)
			{
				return 0;
			}
			float num = 1f;
			Faction faction = MapPointOfInterest.current.faction;
			if (faction != null)
			{
				num = faction.GetReputationLevel(Faction.player).GetRepairCostDiscount();
			}
			int num2 = Mathf.CeilToInt(Mathf.Max(this.shipSelect.selectedShip.ArmorDamageTaken(), this.shipSelect.selectedShip.HullDamageTaken()));
			return Mathf.Max(0, Mathf.CeilToInt((float)num2 * (1f - num)));
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00082BA0 File Offset: 0x00080DA0
		private void Start()
		{
			PersonalHangar.current = this;
			this.shipSelect.SetSelectCallback(new Action(this.SelectShip));
			this.ShowShips();
			if (this.addDecalButton)
			{
				this.addDecalButton.onClick.AddListener(new UnityAction(this.AddDecal));
				this.addDecalButton.gameObject.SetActive(false);
			}
			if (this.removeDecalButton)
			{
				this.removeDecalButton.onClick.AddListener(new UnityAction(this.RemoveLastDecal));
				this.removeDecalButton.gameObject.SetActive(false);
			}
			if (this.decalCountText)
			{
				this.decalCountText.gameObject.SetActive(false);
			}
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00082C64 File Offset: 0x00080E64
		private void ShowSellButton()
		{
			if (GamePlayer.current.spaceShips.Count != 1 && GamePlayer.current.currentSpaceShip != this.shipSelect.selectedShip.spaceShipData)
			{
				this.sellButton.gameObject.SetActive(true);
				this.sellValue.text = "$ " + GameMath.FormatNumber(this.shipSelect.selectedShip.totalSellValue, -1);
				return;
			}
			this.sellButton.gameObject.SetActive(false);
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00082CF5 File Offset: 0x00080EF5
		public void ShowShips()
		{
			this.shipSelect.SetPlayerShips(GamePlayer.current.spaceShips);
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00082D0C File Offset: 0x00080F0C
		public void EquipSelectedShip()
		{
			if (this.shipSelect.selectedShipData == null)
			{
				return;
			}
			GamePlayer current = GamePlayer.current;
			if (current.currentSpaceShip != this.shipSelect.selectedShipData)
			{
				current.currentSpaceShip.TransferCargo(true, true, true, false);
				current.SetSpaceShipData(this.shipSelect.selectedShipData);
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.SelectShip();
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x00082D70 File Offset: 0x00080F70
		protected void SelectShip()
		{
			this.selectedShipData = this.shipSelect.selectedShipData;
			this.equipButton.gameObject.SetActive(this.selectedShipData != GamePlayer.current.currentSpaceShip);
			this.ShowSellButton();
			this.UpdateTooltips();
			if (this.decalModeActive)
			{
				this.RefreshDecalSlots();
				this.UpdateDecalControls();
			}
		}

		// Token: 0x0600144A RID: 5194 RVA: 0x00082DD3 File Offset: 0x00080FD3
		public void ToggleDecalsMode()
		{
			if (this.decalModeActive)
			{
				this.HideDecalsMode();
				return;
			}
			this.ShowDecalsMode();
		}

		// Token: 0x0600144B RID: 5195 RVA: 0x00082DEA File Offset: 0x00080FEA
		public void ShowDecalsMode()
		{
			this.decalModeActive = true;
			this.RefreshDecalSlots();
			this.UpdateDecalControls();
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x00082E00 File Offset: 0x00081000
		public void HideDecalsMode()
		{
			this.decalModeActive = false;
			this.DestroyDecalSlots();
			if (this._decalPickerPopup)
			{
				this._decalPickerPopup.gameObject.SetActive(false);
			}
			if (this.addDecalButton)
			{
				this.addDecalButton.gameObject.SetActive(false);
			}
			if (this.removeDecalButton)
			{
				this.removeDecalButton.gameObject.SetActive(false);
			}
			if (this.decalCountText)
			{
				this.decalCountText.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x00082E94 File Offset: 0x00081094
		public void OnDecalSlotClicked(int placementIndex)
		{
			if (!this.decalPickerPopupPrefab)
			{
				return;
			}
			if (!this._decalPickerPopup)
			{
				this._decalPickerPopup = UnityEngine.Object.Instantiate<DecalPickerPopup>(this.decalPickerPopupPrefab, UITooltip.tooltipParent);
			}
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			if (placementIndex < 0 || placementIndex >= selectedShipData.decalPlacements.Count)
			{
				return;
			}
			DecalPlacement decalPlacement = selectedShipData.decalPlacements[placementIndex];
			this._decalPickerPopup.Open(placementIndex, decalPlacement.decalId, decalPlacement.opacity, decalPlacement.scale, decalPlacement.rotation, decalPlacement.color, new Action<int, string, float, float, float, Color>(this.OnDecalPicked));
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x00082F34 File Offset: 0x00081134
		private void OnDecalPicked(int placementIndex, string decalId, float opacity, float scale, float rotation, Color color)
		{
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			if (decalId == null)
			{
				if (placementIndex >= 0 && placementIndex < selectedShipData.decalPlacements.Count)
				{
					selectedShipData.decalPlacements.RemoveAt(placementIndex);
				}
				if (this._decalPickerPopup)
				{
					this._decalPickerPopup.gameObject.SetActive(false);
				}
				this.RefreshDecalSlots();
			}
			else if (placementIndex >= 0 && placementIndex < selectedShipData.decalPlacements.Count)
			{
				DecalPlacement decalPlacement = selectedShipData.decalPlacements[placementIndex];
				decalPlacement.decalId = decalId;
				decalPlacement.opacity = opacity;
				decalPlacement.scale = scale;
				decalPlacement.rotation = rotation;
				decalPlacement.color = color;
				if (placementIndex < this._activeDecalSlots.Count && this._activeDecalSlots[placementIndex])
				{
					this._activeDecalSlots[placementIndex].ApplyScale(scale);
					this._activeDecalSlots[placementIndex].ApplyRotation(rotation);
				}
			}
			this.UpdateDecalControls();
			this.ApplyDecalsToShips(selectedShipData);
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x00083034 File Offset: 0x00081234
		public void OnDecalMoved(int placementIndex, Vector2 newPosition)
		{
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			if (placementIndex < 0 || placementIndex >= selectedShipData.decalPlacements.Count)
			{
				return;
			}
			selectedShipData.decalPlacements[placementIndex].position = newPosition;
			this.ApplyDecalsToShips(selectedShipData);
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0008307C File Offset: 0x0008127C
		public void AddDecal()
		{
			SpaceShip selectedShip = this.shipSelect.selectedShip;
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			if (selectedShip == null || selectedShipData.decalPlacements.Count >= selectedShip.maxDecals)
			{
				return;
			}
			DecalPlacement decalPlacement = new DecalPlacement();
			selectedShipData.decalPlacements.Add(decalPlacement);
			int num = selectedShipData.decalPlacements.Count - 1;
			this.SpawnDecalSlot(selectedShip, num, decalPlacement);
			this.UpdateDecalControls();
			this.OnDecalSlotClicked(num);
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x000830F4 File Offset: 0x000812F4
		public void RemoveLastDecal()
		{
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			if (selectedShipData == null || selectedShipData.decalPlacements.Count == 0)
			{
				return;
			}
			selectedShipData.decalPlacements.RemoveAt(selectedShipData.decalPlacements.Count - 1);
			if (this._decalPickerPopup)
			{
				this._decalPickerPopup.gameObject.SetActive(false);
			}
			this.RefreshDecalSlots();
			this.UpdateDecalControls();
			this.ApplyDecalsToShips(selectedShipData);
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00083168 File Offset: 0x00081368
		private void RefreshDecalSlots()
		{
			this.DestroyDecalSlots();
			SpaceShip selectedShip = this.shipSelect.selectedShip;
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			if (selectedShip == null || selectedShipData == null)
			{
				return;
			}
			for (int i = 0; i < selectedShipData.decalPlacements.Count; i++)
			{
				this.SpawnDecalSlot(selectedShip, i, selectedShipData.decalPlacements[i]);
			}
			this.SpawnDoorDecalSlots(selectedShip);
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x000831D4 File Offset: 0x000813D4
		private void DestroyDecalSlots()
		{
			foreach (DecalSlot decalSlot in this._activeDecalSlots)
			{
				if (decalSlot)
				{
					UnityEngine.Object.Destroy(decalSlot.gameObject);
				}
			}
			this._activeDecalSlots.Clear();
			foreach (DecalSlot decalSlot2 in this._activeDoorDecalSlots)
			{
				if (decalSlot2)
				{
					UnityEngine.Object.Destroy(decalSlot2.gameObject);
				}
			}
			this._activeDoorDecalSlots.Clear();
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x00083298 File Offset: 0x00081498
		private void SpawnDecalSlot(SpaceShip ship, int index, DecalPlacement placement)
		{
			DecalSlot decalSlot = UnityEngine.Object.Instantiate<DecalSlot>(this.decalSlotPrefab, ship.transform);
			decalSlot.Init(ship, index, placement);
			this._activeDecalSlots.Add(decalSlot);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x000832CC File Offset: 0x000814CC
		private void SpawnDoorDecalSlots(SpaceShip ship)
		{
			DroneBayModule[] componentsInChildren = ship.GetComponentsInChildren<DroneBayModule>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				DroneBayModule droneBayModule = componentsInChildren[i];
				if (!(droneBayModule.doorMechanism == null))
				{
					Door[] doors = droneBayModule.doorMechanism.Doors;
					for (int j = 0; j < doors.Length; j++)
					{
						Door door = doors[j];
						List<DecalPlacement> orCreateDoorPlacements = droneBayModule.GetOrCreateDoorPlacements(j);
						if (orCreateDoorPlacements != null)
						{
							for (int k = 0; k < orCreateDoorPlacements.Count; k++)
							{
								this.SpawnDoorDecalSlot(ship, droneBayModule, door, i, j, k, orCreateDoorPlacements[k]);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001456 RID: 5206 RVA: 0x00083360 File Offset: 0x00081560
		private void SpawnDoorDecalSlot(SpaceShip ship, DroneBayModule module, Door door, int bayIdx, int doorIdx, int decalIdx, DecalPlacement placement)
		{
			DecalSlot decalSlot = UnityEngine.Object.Instantiate<DecalSlot>(this.decalSlotPrefab, door.transform);
			decalSlot.InitDoor(ship, door, module, bayIdx, doorIdx, decalIdx, placement);
			this._activeDoorDecalSlots.Add(decalSlot);
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0008339C File Offset: 0x0008159C
		public void OnDoorDecalSlotClicked(int bayIdx, int doorIdx, int placementIdx)
		{
			if (!this.decalPickerPopupPrefab)
			{
				return;
			}
			if (!this._decalPickerPopup)
			{
				this._decalPickerPopup = UnityEngine.Object.Instantiate<DecalPickerPopup>(this.decalPickerPopupPrefab, UITooltip.tooltipParent);
			}
			DroneBayModule[] componentsInChildren = this.shipSelect.selectedShip.GetComponentsInChildren<DroneBayModule>();
			if (bayIdx >= componentsInChildren.Length)
			{
				return;
			}
			List<DecalPlacement> orCreateDoorPlacements = componentsInChildren[bayIdx].GetOrCreateDoorPlacements(doorIdx);
			if (orCreateDoorPlacements == null || placementIdx >= orCreateDoorPlacements.Count)
			{
				return;
			}
			DecalPlacement decalPlacement = orCreateDoorPlacements[placementIdx];
			this._decalPickerPopup.Open(placementIdx, decalPlacement.decalId, decalPlacement.opacity, decalPlacement.scale, decalPlacement.rotation, decalPlacement.color, delegate(int pi, string decalId, float opacity, float scale, float rotation, Color color)
			{
				this.OnDoorDecalPicked(bayIdx, doorIdx, pi, decalId, opacity, scale, rotation, color);
			});
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x00083474 File Offset: 0x00081674
		private void OnDoorDecalPicked(int bayIdx, int doorIdx, int placementIdx, string decalId, float opacity, float scale, float rotation, Color color)
		{
			DroneBayModule[] componentsInChildren = this.shipSelect.selectedShip.GetComponentsInChildren<DroneBayModule>();
			if (bayIdx >= componentsInChildren.Length)
			{
				return;
			}
			List<DecalPlacement> orCreateDoorPlacements = componentsInChildren[bayIdx].GetOrCreateDoorPlacements(doorIdx);
			if (orCreateDoorPlacements == null)
			{
				return;
			}
			if (decalId == null)
			{
				if (placementIdx >= 0 && placementIdx < orCreateDoorPlacements.Count)
				{
					orCreateDoorPlacements.RemoveAt(placementIdx);
				}
				if (this._decalPickerPopup)
				{
					this._decalPickerPopup.gameObject.SetActive(false);
				}
				this.RefreshDecalSlots();
			}
			else if (placementIdx >= 0 && placementIdx < orCreateDoorPlacements.Count)
			{
				DecalPlacement decalPlacement = orCreateDoorPlacements[placementIdx];
				decalPlacement.decalId = decalId;
				decalPlacement.opacity = opacity;
				decalPlacement.scale = scale;
				decalPlacement.rotation = rotation;
				decalPlacement.color = color;
				DecalSlot decalSlot = this._activeDoorDecalSlots.FirstOrDefault((DecalSlot s) => s.BayIndex == bayIdx && s.DoorIndex == doorIdx && s.PlacementIndex == placementIdx);
				if (decalSlot)
				{
					decalSlot.ApplyScale(scale);
					decalSlot.ApplyRotation(rotation);
				}
			}
			this.ApplyDecalsToShips(this.shipSelect.selectedShipData);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x000835AC File Offset: 0x000817AC
		public void OnDoorDecalMoved(int bayIdx, int doorIdx, int placementIdx, Vector2 newPosition)
		{
			DroneBayModule[] componentsInChildren = this.shipSelect.selectedShip.GetComponentsInChildren<DroneBayModule>();
			if (bayIdx >= componentsInChildren.Length)
			{
				return;
			}
			List<DecalPlacement> orCreateDoorPlacements = componentsInChildren[bayIdx].GetOrCreateDoorPlacements(doorIdx);
			if (orCreateDoorPlacements == null || placementIdx >= orCreateDoorPlacements.Count)
			{
				return;
			}
			orCreateDoorPlacements[placementIdx].position = newPosition;
			this.ApplyDecalsToShips(this.shipSelect.selectedShipData);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x00083608 File Offset: 0x00081808
		public void AddDoorDecal(int bayIdx, int doorIdx, Vector2 doorLocalPos)
		{
			SpaceShip selectedShip = this.shipSelect.selectedShip;
			DroneBayModule[] componentsInChildren = selectedShip.GetComponentsInChildren<DroneBayModule>();
			if (bayIdx >= componentsInChildren.Length)
			{
				return;
			}
			DroneBayModule droneBayModule = componentsInChildren[bayIdx];
			List<DecalPlacement> orCreateDoorPlacements = droneBayModule.GetOrCreateDoorPlacements(doorIdx);
			if (orCreateDoorPlacements == null || orCreateDoorPlacements.Count >= selectedShip.maxDecalsPerDoor)
			{
				return;
			}
			DecalPlacement decalPlacement = new DecalPlacement
			{
				position = doorLocalPos
			};
			orCreateDoorPlacements.Add(decalPlacement);
			int num = orCreateDoorPlacements.Count - 1;
			Door door = droneBayModule.doorMechanism.Doors[doorIdx];
			this.SpawnDoorDecalSlot(selectedShip, droneBayModule, door, bayIdx, doorIdx, num, decalPlacement);
			this.OnDoorDecalSlotClicked(bayIdx, doorIdx, num);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00083698 File Offset: 0x00081898
		private void UpdateDecalControls()
		{
			SpaceShip selectedShip = this.shipSelect.selectedShip;
			SpaceShipData selectedShipData = this.shipSelect.selectedShipData;
			int num = (selectedShipData != null) ? selectedShipData.decalPlacements.Count : 0;
			int num2 = (selectedShip != null) ? selectedShip.maxDecals : 0;
			if (this.addDecalButton)
			{
				this.addDecalButton.gameObject.SetActive(this.decalModeActive && num < num2);
			}
			if (this.removeDecalButton)
			{
				this.removeDecalButton.gameObject.SetActive(this.decalModeActive && num > 0);
			}
			if (this.decalCountText)
			{
				this.decalCountText.gameObject.SetActive(this.decalModeActive);
				this.decalCountText.text = string.Format("{0} / {1}", num, num2);
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x00083780 File Offset: 0x00081980
		private void ApplyDecalsToShips(SpaceShipData data)
		{
			this.shipSelect.selectedShip.ChangeDecals(data);
			if (data == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager instance = GameplayManager.Instance;
				if ((instance != null) ? instance.spaceShip : null)
				{
					GameplayManager.Instance.spaceShip.ChangeDecals(data);
				}
			}
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000837D4 File Offset: 0x000819D4
		private void UpdateTooltips()
		{
			TooltipSource component = this.equipButton.GetComponent<TooltipSource>();
			bool flag = this.selectedShipData != GamePlayer.current.currentSpaceShip && !string.IsNullOrEmpty(this.selectedShipData.skillLoadout);
			if (flag)
			{
				component.BodyText = Translation.Translate("@SSPersonalHangarEquipSkill", new object[]
				{
					this.selectedShipData.skillLoadout
				});
			}
			component.enabled = flag;
			TooltipSource component2 = this.skillLoadoutButton.GetComponent<TooltipSource>();
			LoadoutData selectedLoadout = GamePlayer.current.commander.selectedLoadout;
			string text = (selectedLoadout != null) ? selectedLoadout.name : null;
			this.skillLoadoutButton.gameObject.SetActive(text != null);
			if (text != null)
			{
				string str = (!string.IsNullOrEmpty(this.selectedShipData.skillLoadout)) ? (" " + Translation.Translate("@SSCurrentSkillLoadout", new object[]
				{
					this.selectedShipData.skillLoadout
				})) : "";
				component2.BodyText = Translation.Translate("@SSSelectSkillLoadout", new object[]
				{
					text
				}) + str;
			}
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000838E7 File Offset: 0x00081AE7
		public SpaceShip GetSelectedShip()
		{
			return this.shipSelect.selectedShip;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x000838F4 File Offset: 0x00081AF4
		public List<ModuleButton> GetModulesButtons()
		{
			return this.shipSelect.moduleButtons;
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00083904 File Offset: 0x00081B04
		public void RefreshIfOpen()
		{
			SpaceStationInterior instance = SpaceStationInterior.instance;
			if (instance != null && instance.currentTab == SpaceStationFacility.PersonalHangar)
			{
				this.shipSelect.ShowShip(true);
			}
			else
			{
				SpaceStationInterior instance2 = SpaceStationInterior.instance;
				if (instance2 != null && instance2.currentTab == SpaceStationFacility.SalvageWorkshop)
				{
					this.shipSelect.ShowShip(true);
				}
			}
			if (SidePanel.instance.currentTab == SidePanel.SideTabType.Ship)
			{
				SidePanel.instance.RefreshIfOpen();
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00083970 File Offset: 0x00081B70
		public void ReplaceTurret(Inventory.InventoryItem item, SpaceShipHardpoint hardpoint)
		{
			AbstractEquipment componentInChildren = hardpoint.GetComponentInChildren<AbstractEquipment>();
			InventoryItemType inventoryItemType = (componentInChildren != null) ? componentInChildren.item : null;
			InventoryItemType inventoryItemType2 = (item != null) ? item.item : null;
			if (hardpoint.index >= 0)
			{
				this.selectedShipData.hardpoints[hardpoint.index] = inventoryItemType2;
				this.selectedShipData.RecalculateLevel();
				if (item != null)
				{
					item.inventory.Remove(item, 1);
				}
				if (inventoryItemType)
				{
					ReactorModule reactorModule = this.shipSelect.selectedShip.reactorModule;
					if (reactorModule != null)
					{
						reactorModule.DisconnectEquipment(inventoryItemType.GetComponent<AbstractEquipment>());
					}
					this.GetTargetInventory(item).Add(inventoryItemType, 1, false, false);
				}
				this.shipSelect.selectedShip.LoadHardpoint(inventoryItemType2, hardpoint.transform);
				this.shipSelect.selectedShip.InitModules();
				UITooltip.Refresh();
				if (((item != null) ? item.item.GetComponent<MiningTurret>() : null) && item.item)
				{
					MissionObjective.Trigger(MissionTrigger.InstallMiningLaser, null, null, false);
				}
				if (((item != null) ? item.item.GetComponent<AbstractCombatTurret>() : null) && item.item)
				{
					MissionObjective.Trigger(MissionTrigger.InstallCombatTurret, null, null, false);
				}
				if (((item != null) ? item.item.GetComponent<AbstractSalvageTurret>() : null) && item.item)
				{
					MissionObjective.Trigger(MissionTrigger.InstallSalvageLaser, null, null, false);
				}
			}
			if (this.selectedShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.RefreshIfOpen();
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00083AF0 File Offset: 0x00081CF0
		public void DeleteModule(EquipmentSlot module, SpaceShipData spaceShipData)
		{
			InventoryItemType equippedItem = spaceShipData.GetEquippedItem(module);
			if (equippedItem && spaceShipData == this.selectedShipData)
			{
				this.RemoveEquipment<AbstractModule>(equippedItem);
				ReactorModule reactorModule = this.shipSelect.selectedShip.reactorModule;
				if (reactorModule != null)
				{
					reactorModule.DisconnectEquipment(equippedItem.GetComponent<AbstractEquipment>());
				}
			}
			spaceShipData.EquipModule(null, module);
			if (spaceShipData == this.selectedShipData)
			{
				this.shipSelect.PopulateModuleSlots();
			}
			if (spaceShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.RefreshIfOpen();
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x00083B78 File Offset: 0x00081D78
		public void DeleteTurret(SpaceShipData spaceShipData, int hardpointIndex)
		{
			spaceShipData.hardpoints[hardpointIndex] = null;
			if (spaceShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.RefreshIfOpen();
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x00083BA0 File Offset: 0x00081DA0
		public void InstallModule(Inventory.InventoryItem item, EquipmentSlot module)
		{
			InventoryItemType equippedItem = this.selectedShipData.GetEquippedItem(module);
			InventoryItemType inventoryItemType = (item != null) ? item.item : null;
			if (item != null)
			{
				item.inventory.Remove(item, 1);
			}
			if (equippedItem)
			{
				this.RemoveEquipment<AbstractModule>(equippedItem);
				ReactorModule reactorModule = this.shipSelect.selectedShip.reactorModule;
				if (reactorModule != null)
				{
					reactorModule.DisconnectEquipment(equippedItem.GetComponent<AbstractEquipment>());
				}
				this.GetTargetInventory(item).Add(equippedItem, 1, false, false);
			}
			if (inventoryItemType)
			{
				if (module == EquipmentSlot.Reactor)
				{
					ReactorModule reactorModule2 = this.shipSelect.selectedShip.reactorModule;
					List<AbstractEquipment> connectedEquipment = ((reactorModule2 != null) ? reactorModule2.GetConnectedEquipment() : null) ?? new List<AbstractEquipment>();
					this.shipSelect.selectedShip.LoadEquipment(inventoryItemType);
					this.shipSelect.selectedShip.reactorModule.SetConnectedEquipment(connectedEquipment);
				}
				else
				{
					this.shipSelect.selectedShip.LoadEquipment(inventoryItemType);
				}
			}
			this.selectedShipData.EquipModule(inventoryItemType, module);
			this.shipSelect.PopulateModuleSlots();
			if (this.selectedShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.RefreshIfOpen();
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00083CC0 File Offset: 0x00081EC0
		public void InstallBooster(Inventory.InventoryItem item, int index)
		{
			InventoryItemType inventoryItemType = this.selectedShipData.boosters[index];
			InventoryItemType inventoryItemType2 = (item != null) ? item.item : null;
			if (item != null)
			{
				item.inventory.Remove(item, 1);
			}
			if (inventoryItemType)
			{
				this.RemoveEquipment<AbstractBooster>(inventoryItemType);
				ReactorModule reactorModule = this.shipSelect.selectedShip.reactorModule;
				if (reactorModule != null)
				{
					reactorModule.DisconnectEquipment(inventoryItemType.GetComponent<AbstractEquipment>());
				}
				this.GetTargetInventory(item).Add(inventoryItemType, 1, false, false);
			}
			if (inventoryItemType2)
			{
				this.shipSelect.selectedShip.LoadEquipment(inventoryItemType2);
			}
			this.selectedShipData.EquipBooster(inventoryItemType2, index);
			this.shipSelect.PopulateBoosterSlots();
			if (this.selectedShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
			}
			this.RefreshIfOpen();
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00083D90 File Offset: 0x00081F90
		private Inventory GetTargetInventory(Inventory.InventoryItem item)
		{
			if (((item != null) ? item.inventory : null) != null)
			{
				return item.inventory;
			}
			InventoryPanel inventoryPanel = InventoryInteractionManager.Instance.inventoryPanel;
			if (!inventoryPanel || item == null)
			{
				return Inventory.cargo;
			}
			if (inventoryPanel.inventory == Inventory.global && item.item.CanGoInArmory())
			{
				return Inventory.global;
			}
			if (inventoryPanel.inventory == Inventory.materials && item.item.CanGoInMaterials())
			{
				return Inventory.materials;
			}
			return Inventory.cargo;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x00083E14 File Offset: 0x00082014
		private void RemoveEquipment<T>(InventoryItemType item) where T : InventoryItemPart
		{
			T t = this.shipSelect.selectedShip.GetComponentsInChildren<T>().FirstOrDefault((T equipped) => equipped.item == item);
			if (t != null)
			{
				UnityEngine.Object.Destroy(t.gameObject);
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x00083E70 File Offset: 0x00082070
		public void RepairShip()
		{
			int num = Mathf.CeilToInt(Mathf.Max(this.shipSelect.selectedShip.HullDamageTaken(), this.shipSelect.selectedShip.ArmorDamageTaken()));
			int num2 = this.GetRepairCost();
			if (num == 0)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSShipMaxHp", Array.Empty<object>())).WithColor(ColorHelper.greenish).Show();
				return;
			}
			if (!GamePlayer.current.CanAfford((float)num2))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			PersonalHangar.current.StartRepair((float)num, this.shipSelect.selectedShip.spaceShipData.guid, false, num2);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00083F38 File Offset: 0x00082138
		public void CheckRepairs()
		{
			foreach (SpaceShipData spaceShipData in GamePlayer.current.spaceShips)
			{
				float num = Mathf.Max(spaceShipData.HullDamageTaken(), spaceShipData.ArmorDamageTaken());
				if (num > 0f)
				{
					PersonalHangar current = PersonalHangar.current;
					if (current != null)
					{
						current.TryStartAutoRepair(num, spaceShipData.guid);
					}
				}
			}
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00083FBC File Offset: 0x000821BC
		public void UpdateCrewButtons(bool reloadShip)
		{
			this.shipSelect.PopulateCrewSlots();
			this.shipSelect.ShowCrewTab();
			if (reloadShip && this.selectedShipData == GamePlayer.current.currentSpaceShip)
			{
				GameplayManager.Instance.ReinitPlayerSpaceship();
				this.shipSelect.selectedShip.SetCrewMembers(GamePlayer.current.commander, this.selectedShipData.crewMembers);
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00084024 File Offset: 0x00082224
		public void SellShipPopup()
		{
			if (!this.shipSelect.selectedShip.spaceShipData.cargo.IsEmpty())
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSPeronalHangarCargoNotEmpty", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			AlertPopup.ShowQuery(Translation.Translate("@SSPersonalHangarSellShipQuery", Array.Empty<object>()), "Sell", "Cancel", delegate
			{
				this.SellShip(this.shipSelect.selectedShip);
			}, null, null, null);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000840A4 File Offset: 0x000822A4
		private void SellShip(SpaceShip ship)
		{
			GamePlayer.current.credits += (long)Mathf.RoundToInt(ship.totalSellValue);
			foreach (RepairJob repairJob in SpaceStation.current.GetRepairJobs().Cast<RepairJob>().ToList<RepairJob>())
			{
				if (repairJob.spaceshipData == ship.spaceShipData)
				{
					repairJob.CancelJob();
				}
			}
			SpaceStationInterior.instance.UpdateJobs();
			this.RemoveSpaceshipFromFleet(ship.spaceShipData);
			this.shipSelect.RemoveShip(ship.spaceShipData);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00084158 File Offset: 0x00082358
		public void SetDefaultSkillLoadout()
		{
			this.selectedShipData.skillLoadout = GamePlayer.current.commander.selectedLoadout.name;
			Debug.Log("Set skill loadout: " + this.selectedShipData.skillLoadout);
			this.UpdateTooltips();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000841A4 File Offset: 0x000823A4
		public void RemoveSpaceshipFromFleet(SpaceShipData spaceShipData)
		{
			if (GamePlayer.current.spaceShips.Contains(spaceShipData))
			{
				GamePlayer.current.spaceShips.Remove(spaceShipData);
			}
		}

		// Token: 0x04000BCF RID: 3023
		[SerializeField]
		private Button equipButton;

		// Token: 0x04000BD0 RID: 3024
		[SerializeField]
		private Button repairButton;

		// Token: 0x04000BD1 RID: 3025
		[SerializeField]
		private Button sellButton;

		// Token: 0x04000BD2 RID: 3026
		[SerializeField]
		private Button skillLoadoutButton;

		// Token: 0x04000BD3 RID: 3027
		[SerializeField]
		private TextMeshProUGUI repairCost;

		// Token: 0x04000BD4 RID: 3028
		[SerializeField]
		private TextMeshProUGUI sellValue;

		// Token: 0x04000BD6 RID: 3030
		[Header("Decal UI")]
		[SerializeField]
		private DecalPickerPopup decalPickerPopupPrefab;

		// Token: 0x04000BD7 RID: 3031
		[SerializeField]
		private DecalSlot decalSlotPrefab;

		// Token: 0x04000BD8 RID: 3032
		[SerializeField]
		private Button addDecalButton;

		// Token: 0x04000BD9 RID: 3033
		[SerializeField]
		private Button removeDecalButton;

		// Token: 0x04000BDA RID: 3034
		[SerializeField]
		private TextMeshProUGUI decalCountText;

		// Token: 0x04000BDB RID: 3035
		private DecalPickerPopup _decalPickerPopup;

		// Token: 0x04000BDC RID: 3036
		private readonly List<DecalSlot> _activeDecalSlots = new List<DecalSlot>();

		// Token: 0x04000BDD RID: 3037
		private readonly List<DecalSlot> _activeDoorDecalSlots = new List<DecalSlot>();

		// Token: 0x04000BE0 RID: 3040
		private float updateTimer = 0.1f;
	}
}
