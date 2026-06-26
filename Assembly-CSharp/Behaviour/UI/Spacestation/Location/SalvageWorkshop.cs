using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crafting;
using Behaviour.Equipment;
using Behaviour.Equipment.Aspect;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Side_Menu;
using Behaviour.UI.Side_Menu.SideTabs;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Item;
using Source.Player;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;

namespace Behaviour.UI.Spacestation.Location
{
	// Token: 0x02000225 RID: 549
	public class SalvageWorkshop : MonoBehaviour, IDraggableWindow
	{
		// Token: 0x17000344 RID: 836
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x00084504 File Offset: 0x00082704
		// (set) Token: 0x0600147B RID: 5243 RVA: 0x0008450B File Offset: 0x0008270B
		public static SalvageWorkshop current { get; private set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x00084513 File Offset: 0x00082713
		// (set) Token: 0x0600147D RID: 5245 RVA: 0x0008451B File Offset: 0x0008271B
		public int rerollStatCost { get; private set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x0600147E RID: 5246 RVA: 0x00084524 File Offset: 0x00082724
		// (set) Token: 0x0600147F RID: 5247 RVA: 0x0008452C File Offset: 0x0008272C
		public int upgradeRarityCost { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x00084535 File Offset: 0x00082735
		// (set) Token: 0x06001481 RID: 5249 RVA: 0x0008453D File Offset: 0x0008273D
		public int upgradeLevelCost { get; private set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x00084546 File Offset: 0x00082746
		// (set) Token: 0x06001483 RID: 5251 RVA: 0x0008454E File Offset: 0x0008274E
		public int extractBlueprintCost { get; private set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x00084557 File Offset: 0x00082757
		// (set) Token: 0x06001485 RID: 5253 RVA: 0x0008455F File Offset: 0x0008275F
		public int installAspectCost { get; private set; }

		// Token: 0x06001486 RID: 5254 RVA: 0x00084568 File Offset: 0x00082768
		private void Awake()
		{
			InventoryInteractionManager.Instance.RegisterSalvageWorkshop(this);
			SalvageWorkshop.current = this;
			this.draggableWindowBar.SetDraggableWindow(this);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00084587 File Offset: 0x00082787
		private void OnEnable()
		{
			this.UpdateSpendage();
			this.SetWorkshopCredit();
			this.rarityButton.Init(this);
			this.levelButton.Init(this);
			this.blueprintButton.Init(this);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x000845BC File Offset: 0x000827BC
		public void SelectItem(InventoryItemType item, bool equipedOnShip = false, SpaceShipData shipData = null)
		{
			this.selectedItem = item;
			this.isItemEquipped = equipedOnShip;
			if (shipData != null)
			{
				this.data = shipData;
			}
			else if (!this.isItemEquipped)
			{
				this.data = null;
			}
			this.equipment = this.selectedItem.GetComponent<AbstractEquipment>();
			this.itemPenalty = 1f + this.equipment.salvageWorkShopPenalty;
			this.rerollStatCost = Mathf.RoundToInt((float)this.selectedItem.sellValue * 1.5f * this.itemPenalty);
			this.upgradeRarityCost = Mathf.RoundToInt((float)this.selectedItem.sellValue * 5f * this.itemPenalty);
			this.upgradeLevelCost = Mathf.RoundToInt((float)this.selectedItem.sellValue * 6f * this.itemPenalty);
			this.installAspectCost = Mathf.RoundToInt((float)this.selectedItem.sellValue * 3f * this.itemPenalty);
			this.extractBlueprintCost = Mathf.RoundToInt((float)this.selectedItem.GetSellValue(false) * 20f * this.selectedItem.rarity.GetCostMultiplier());
			this.workshopItem.gameObject.SetActive(true);
			this.workshopItem.SetContent(this.selectedItem, this);
			this.noItemInformation.SetActive(false);
			this.UpdateSpendage();
			this.UpdateWorkshopCredit();
			UITooltip.Refresh();
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0008471C File Offset: 0x0008291C
		private void UpdateSpendage()
		{
			GamePlayer current = GamePlayer.current;
			string text = "$" + GameMath.FormatNumber((float)current.salvageWorkshopTotalSpent, -1);
			this.totalSpendage.text = Translation.Translate("@SalWorTotalSpendage", Array.Empty<object>()) + ": " + text.HighlightWithColor(ColorHelper.creditsColor);
			string text2 = "$0";
			string text3 = "0";
			if (this.selectedItem != null)
			{
				text2 = "$" + GameMath.FormatNumber((float)this.equipment.salvageWorkShopSpent, -1);
				text3 = this.equipment.salvageWorkShopItemChangedAmount.ToString();
			}
			this.itemSpendage.text = Translation.Translate("@SalWorItemSpendage", Array.Empty<object>()) + ": " + text2.HighlightWithColor(ColorHelper.creditsColor);
			this.itemModified.text = Translation.Translate("@SalWorItemModified", Array.Empty<object>()) + ": " + text3.HighlightWithColor(ColorHelper.lightCyan);
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00084820 File Offset: 0x00082A20
		private void SetWorkshopCredit()
		{
			this.UpdateWorkshopCredit();
			TooltipSource component = this.workshopCreditText.GetComponent<TooltipSource>();
			component.BodyText = Translation.Translate("@SalWorShopCreditDesc", Array.Empty<object>());
			if (GamePlayer.current.salvageWorkshopTradeInMaterialsUnlocked)
			{
				this.itemOfTheDay.gameObject.SetActive(true);
				TooltipSource tooltipSource = component;
				tooltipSource.BodyText += "\n\n";
				TooltipSource tooltipSource2 = component;
				tooltipSource2.BodyText += Translation.Translate("@SalWorShopCreditDailyUnlockedDesc", new object[]
				{
					4f,
					6f
				});
				this.CheckItemOfTheDay(false);
			}
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x000848CC File Offset: 0x00082ACC
		private void UpdateWorkshopCredit()
		{
			string text = GameMath.FormatNumber((float)GamePlayer.current.salvageWorkshopShopCredit, -1) ?? "";
			this.workshopCreditText.text = (Translation.Translate("@SalWorShopCredit", new object[]
			{
				text
			}) ?? "");
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x0008491C File Offset: 0x00082B1C
		private void CheckItemOfTheDay(bool force = false)
		{
			GamePlayer current = GamePlayer.current;
			if (force || new DateTime(current.salvageWorkshopLastUpdateTime).DayOfYear != DateTime.Now.DayOfYear)
			{
				current.salvageWorkshopDailyItem = this.PickItemOfTheDay();
				current.salvageWorkshopLastUpdateTime = DateTime.Now.Ticks;
			}
			this.dailyItem = current.salvageWorkshopDailyItem;
			this.itemOfTheDay.text = (Translation.Translate("@SalWorShopDailyRequired", new object[]
			{
				Translation.Translate(this.dailyItem.displayName, Array.Empty<object>()).HighlightWithColor(this.dailyItem.rarity.GetColor())
			}) ?? "");
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x000849D8 File Offset: 0x00082BD8
		private string PickItemOfTheDay()
		{
			SeededRandom global = SeededRandom.Global;
			GamePlayer current = GamePlayer.current;
			Dictionary<string, InventoryItemType> allItemsInCategory = InventoryItemType.GetAllItemsInCategory(ItemCategory.RefinedProduct);
			int playerLevel = current.level;
			KeyValuePair<string, InventoryItemType> keyValuePair = default(KeyValuePair<string, InventoryItemType>);
			int num = 0;
			do
			{
				keyValuePair = global.Choose<KeyValuePair<string, InventoryItemType>>(from r in allItemsInCategory
				where r.Value.itemLevel <= playerLevel && !r.Value.criticalItem && !r.Value.missionItem
				select r);
				num++;
			}
			while (keyValuePair.Key == current.salvageWorkshopDailyItem && num < 10);
			return keyValuePair.Key;
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00084A58 File Offset: 0x00082C58
		public void AddShopCredit(InventoryItemType item, int itemAmount)
		{
			if (item.criticalItem)
			{
				return;
			}
			int num = this.GetItemTradeInValue(item);
			num *= itemAmount;
			GamePlayer.current.AddSalvageShopCredit(num, itemAmount, true);
			this.UpdateWorkshopCredit();
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00084A8D File Offset: 0x00082C8D
		public bool IsDailyItem(InventoryItemType item)
		{
			return item.identifier == this.dailyItem.identifier;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00084AAC File Offset: 0x00082CAC
		public int GetItemTradeInValue(InventoryItemType item)
		{
			int num = item.cost;
			if (this.IsDailyItem(item))
			{
				num *= 6;
			}
			else
			{
				num *= 4;
			}
			return num;
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x00084AD4 File Offset: 0x00082CD4
		private void EmptySelectedItem()
		{
			this.selectedItem = null;
			this.workshopItem.gameObject.SetActive(false);
			this.noItemInformation.SetActive(true);
			SidePanel.instance.RefreshIfOpen();
			this.UpdateSpendage();
			this.UpdateWorkshopCredit();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00084B10 File Offset: 0x00082D10
		public void RerollStat(EquipStatLine stat, int index)
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (index < 0 || index >= this.equipment.stats.Count)
			{
				Debug.LogWarning(string.Format("Index: {0} is not correct when trying to reroll stat: {1}", index, stat.ToReadableString(false)));
				return;
			}
			if (!this.CanAfford(this.rerollStatCost))
			{
				return;
			}
			this.HandlePaymentAndPenalty(this.rerollStatCost, 0.12f, this.selectedItem);
			this.equipment.stats.RemoveAt(index);
			for (int i = 0; i < this.equipment.stats.Count; i++)
			{
				EquipStatLine value = this.equipment.stats[i];
				value.canReroll = false;
				this.equipment.stats[i] = value;
			}
			this.selectedItem.equipmentBuilder.AddRandomStat(this.selectedItem, index, true);
			this.TryRefreshItem();
			this.Refresh();
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00084C04 File Offset: 0x00082E04
		public void InstallAspect(EquipAspect aspect, int index, InventoryItemType aspectItemToDelete)
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (this.equipment.aspectSlots[index].equipAspect != null && this.equipment.aspectSlots[index].equipAspect.displayName == aspect.displayName)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorSameInsAspect", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!this.CanAfford(this.installAspectCost))
			{
				return;
			}
			this.RemoveItem(aspectItemToDelete);
			InventoryItemType item = this.selectedItem.equipmentBuilder.RebuildItemWithAspect(this.selectedItem, aspect, index);
			this.RemoveItem(null);
			this.HandlePaymentAndPenalty(this.installAspectCost, 0.4f, item);
			this.HandleEquipOrStore(item);
			this.SelectItem(item, false, this.data);
			SidePanel.instance.RefreshIfOpen();
			this.TryRefreshItem();
			this.Refresh();
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00084D04 File Offset: 0x00082F04
		public void SalvageAspect(EquipAspect aspect, int cost)
		{
			InventoryItemType item = ItemBuilder.Get("Aspect").CreateAspect(aspect);
			if (!this.CanAfford(cost))
			{
				return;
			}
			this.HandlePaymentAndPenalty(cost, 0f, this.selectedItem);
			this.RemoveItem(null);
			this.AddItemToCargo(item);
			this.EmptySelectedItem();
			this.Refresh();
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x00084D58 File Offset: 0x00082F58
		public void RerollRarity()
		{
			if (this.selectedItem == null)
			{
				return;
			}
			Rarity rarity = this.selectedItem.rarity;
			rarity++;
			if (rarity > Rarity.HighGrade)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorRarity", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			Rarity rarity2 = this.selectedItem.equipmentBuilder.IsRarityAvailable(rarity);
			if (rarity != rarity2)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorRarityNotAvail", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!this.CanAfford(this.upgradeRarityCost))
			{
				return;
			}
			InventoryItemType item = this.selectedItem.equipmentBuilder.RebuildItemWithRarity(this.selectedItem, rarity);
			this.RemoveItem(null);
			this.HandlePaymentAndPenalty(this.upgradeRarityCost, 0.2f, item);
			this.HandleEquipOrStore(item);
			this.SelectItem(item, false, this.data);
			this.Refresh();
			SidePanel.instance.RefreshIfOpen();
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00084E58 File Offset: 0x00083058
		public void UpgradeLevel()
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (!this.CanAfford(this.upgradeLevelCost))
			{
				return;
			}
			if (this.selectedItem.itemLevel + 1 > GamePlayer.current.commander.level + 6)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorAddOneTooHigh", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			InventoryItemType item = this.selectedItem.equipmentBuilder.RebuildItemWithLevel(this.selectedItem, this.selectedItem.itemLevel + 1);
			this.HandlePaymentAndPenalty(this.upgradeLevelCost, 0.15f, item);
			this.RemoveItem(null);
			this.HandleEquipOrStore(item);
			this.SelectItem(item, false, this.data);
			SidePanel.instance.RefreshIfOpen();
			this.Refresh();
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00084F2E File Offset: 0x0008312E
		private void HandleEquipOrStore(InventoryItemType item)
		{
			if (this.data != null)
			{
				this.data.EquipItem(item, 0);
				return;
			}
			this.AddItemToCargo(item);
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00084F4E File Offset: 0x0008314E
		private void Refresh()
		{
			UITooltip.Refresh();
			GameplayManager.Instance.ReinitPlayerSpaceship();
			PersonalHangar.current.RefreshIfOpen();
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00084F6C File Offset: 0x0008316C
		private bool CanAfford(int cost)
		{
			GamePlayer current = GamePlayer.current;
			long num = (long)cost - current.salvageWorkshopShopCredit;
			if (num <= 0L)
			{
				return true;
			}
			if (!current.CanAfford((float)num))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			return true;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x00084FC8 File Offset: 0x000831C8
		private void HandlePaymentAndPenalty(int cost, float weight, InventoryItemType item)
		{
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			this.RemoveCredits(cost, component);
			this.UpdatePenalty(weight, component);
			this.UpdateItemChanged(component);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x00084FF4 File Offset: 0x000831F4
		private void RemoveCredits(int cost, AbstractEquipment equipment)
		{
			GamePlayer current = GamePlayer.current;
			int num = (int)Math.Min((long)cost, current.salvageWorkshopShopCredit);
			int num2 = cost - num;
			if (num > 0)
			{
				current.RemoveSalvageShopCredit(num);
			}
			if (num2 > 0)
			{
				current.RemoveCredits((float)num2);
			}
			current.AddSalvageCreditsSpent(cost);
			equipment.AddSalvageCreditsSpent(cost);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x0008503F File Offset: 0x0008323F
		private void UpdatePenalty(float weight, AbstractEquipment equipment)
		{
			equipment.AddWorkshopPenalty(weight);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x00085048 File Offset: 0x00083248
		private void UpdateItemChanged(AbstractEquipment equipment)
		{
			equipment.AddItemChanged(1);
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00085054 File Offset: 0x00083254
		public void RerollItemLegendary()
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (!this.CanAfford(10))
			{
				return;
			}
			InventoryItemType item = this.selectedItem.equipmentBuilder.CreateItemType(Rarity.Legendary, this.selectedItem.itemLevel, true, null, false, false);
			this.HandlePaymentAndPenalty(10, 0f, item);
			this.RemoveItem(null);
			this.HandleEquipOrStore(item);
			this.SelectItem(item, false, this.data);
			SidePanel.instance.RefreshIfOpen();
			this.Refresh();
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x000850D6 File Offset: 0x000832D6
		private void TryRefreshItem()
		{
			this.SelectItem(this.selectedItem, false, null);
			UITooltip.Refresh();
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x000850EC File Offset: 0x000832EC
		public void AddItemToCargo(InventoryItemType item)
		{
			InventoryContent inventoryContent = SidePanel.instance.sideTab.currentSideTabContent as InventoryContent;
			if (inventoryContent == null)
			{
				GamePlayer.current.currentSpaceShip.AddCargo(item, 1, true);
				return;
			}
			InventoryType inventoryType = inventoryContent.inventoryType;
			Inventory inventory;
			if (inventoryType != InventoryType.Stash)
			{
				if (inventoryType == InventoryType.Cargo)
				{
					inventory = GamePlayer.current.currentSpaceShip.cargo;
				}
				else
				{
					inventory = GamePlayer.current.currentSpaceShip.cargo;
				}
			}
			else
			{
				inventory = GamePlayer.current.globalInventory;
			}
			if (inventory != null)
			{
				inventory.Add(item, 1, false, false);
				return;
			}
			GamePlayer.current.currentSpaceShip.AddCargo(item, 1, true);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00085188 File Offset: 0x00083388
		private void RemoveItem(InventoryItemType item = null)
		{
			InventoryItemType itemToRemove = (item != null) ? item : this.selectedItem;
			if (Inventory.cargo.items.FirstOrDefault((Inventory.InventoryItem i) => i.item == itemToRemove) != null)
			{
				Inventory.cargo.Remove(itemToRemove, 1);
				this.HandleItemRemoval(itemToRemove, false, null);
				return;
			}
			if (Inventory.global.items.FirstOrDefault((Inventory.InventoryItem i) => i.item == itemToRemove) != null)
			{
				Inventory.global.Remove(itemToRemove, 1);
				this.HandleItemRemoval(itemToRemove, false, null);
				return;
			}
			SpaceShipData spaceShipData = null;
			int hardpointIndex = 0;
			Func<InventoryItemType, bool> _delegate_2;
			Func<Inventory.InventoryItem, bool> _delegate_3;
			foreach (SpaceShipData spaceShipData2 in GamePlayer.current.spaceShips)
			{
				IEnumerable<InventoryItemType> equippedModules = spaceShipData2.equippedModules;
				Func<InventoryItemType, bool> predicate;
				if ((predicate = _delegate_2) == null)
				{
					predicate = (_delegate_2 = ((InventoryItemType i) => i == itemToRemove));
				}
				if (equippedModules.FirstOrDefault(predicate) != null)
				{
					spaceShipData = spaceShipData2;
					break;
				}
				for (int j = 0; j < spaceShipData2.hardpoints.Length; j++)
				{
					if (spaceShipData2.hardpoints[j] == itemToRemove)
					{
						spaceShipData = spaceShipData2;
						hardpointIndex = j;
						break;
					}
				}
				IEnumerable<Inventory.InventoryItem> items = spaceShipData2.cargo.items;
				Func<Inventory.InventoryItem, bool> predicate2;
				if ((predicate2 = _delegate_3) == null)
				{
					predicate2 = (_delegate_3 = ((Inventory.InventoryItem i) => i.item == itemToRemove));
				}
				if (items.FirstOrDefault(predicate2) != null)
				{
					spaceShipData2.cargo.Remove(itemToRemove, 1);
					this.HandleItemRemoval(itemToRemove, false, null);
					return;
				}
			}
			if (spaceShipData != null)
			{
				AbstractModule abstractModule;
				if (itemToRemove.TryGetComponent<AbstractModule>(out abstractModule))
				{
					PersonalHangar.current.DeleteModule(abstractModule.slot, spaceShipData);
					this.HandleItemRemoval(itemToRemove, true, spaceShipData);
					return;
				}
				AbstractTurret abstractTurret;
				if (itemToRemove.TryGetComponent<AbstractTurret>(out abstractTurret))
				{
					PersonalHangar.current.DeleteTurret(spaceShipData, hardpointIndex);
					this.HandleItemRemoval(itemToRemove, true, spaceShipData);
					return;
				}
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x000853A8 File Offset: 0x000835A8
		private void HandleItemRemoval(InventoryItemType itemToRemove, bool equipped = false, SpaceShipData data = null)
		{
			this.isItemEquipped = equipped;
			this.data = data;
			if (!itemToRemove.GetComponent<AspectItem>())
			{
				UnityEngine.Object.Destroy(itemToRemove.gameObject);
			}
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x000853D0 File Offset: 0x000835D0
		public void ChangeItemNameButton()
		{
			if (this.selectedItem == null)
			{
				return;
			}
			this.CreatePopup(delegate(string input)
			{
				this.SetCustomItemName(input);
				this.TryRefreshItem();
			});
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x000853F3 File Offset: 0x000835F3
		private void CreatePopup(Action<string> onSubmitAction)
		{
			AlertPopup.ShowInput("@UIItemName", onSubmitAction, "@UIChange", this.selectedItem.displayName, true, null, null);
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00085413 File Offset: 0x00083613
		private void SetCustomItemName(string itemName)
		{
			this.selectedItem.SetDisplayName(itemName);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00085424 File Offset: 0x00083624
		public void LearnBlueprint()
		{
			if (this.selectedItem == null)
			{
				return;
			}
			if (this.selectedItem.rarity > Rarity.HighGrade)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorRarityTooHigh", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!this.CanAfford(this.extractBlueprintCost))
			{
				return;
			}
			CraftingRecipe blueprintForItem = CraftingRecipe.GetBlueprintForItem(this.selectedItem.equipmentBuilder.identifier, this.selectedItem.rarity);
			if (blueprintForItem == null)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SalWorRarityAlreadyKnown", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			InventoryItemType blueprint = ItemBuilder.Get("Blueprint").CreateBlueprint(blueprintForItem);
			AlertPopup.ShowQuery(Translation.Translate("@SalWorWarningBlueprint", Array.Empty<object>()).HighlightWithColor(ColorHelper.reddish) + "\n" + Translation.Translate("@SalWorContinueBlueprint", Array.Empty<object>()), Translation.Translate("@UIYes", Array.Empty<object>()), Translation.Translate("@UICancel", Array.Empty<object>()), delegate
			{
				this.AddBlueprint(blueprint);
			}, null, null, null);
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00085560 File Offset: 0x00083760
		private void AddBlueprint(InventoryItemType blueprint)
		{
			this.HandlePaymentAndPenalty(this.extractBlueprintCost, 0f, this.selectedItem);
			this.RemoveItem(null);
			GamePlayer.current.currentSpaceShip.AddCargo(blueprint, 1, true);
			this.EmptySelectedItem();
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00085599 File Offset: 0x00083799
		public float GetDefaultWidth()
		{
			return 768f;
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x000855A0 File Offset: 0x000837A0
		public float GetDefaultHeight()
		{
			return 579f;
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000855A7 File Offset: 0x000837A7
		public RectTransform GetRectTransform()
		{
			return base.transform as RectTransform;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000855B4 File Offset: 0x000837B4
		public void UpdateAnchoredPosition(Vector2 pos)
		{
			GameplayerPrefs.SetSalvageWorkshopPosition(pos);
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x000855BC File Offset: 0x000837BC
		public void UpdateScale(float scale)
		{
			GameplayerPrefs.SetSalvageWorkshopScale(scale);
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000855C4 File Offset: 0x000837C4
		public void UpdateSize(Vector2 delta)
		{
			Debug.LogWarning("Not meant to be called");
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000855D0 File Offset: 0x000837D0
		public void OnStartResize()
		{
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000855D2 File Offset: 0x000837D2
		public Vector2 GetAnchoredPosition()
		{
			return GameplayerPrefs.GetSalvageWorkshopPosition();
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x000855D9 File Offset: 0x000837D9
		public float GetScale()
		{
			return GameplayerPrefs.GetSalvageWorkshopScale();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x000855E0 File Offset: 0x000837E0
		public Vector2 GetSize()
		{
			return Vector2.one;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x000855E7 File Offset: 0x000837E7
		public bool IsScalable()
		{
			return true;
		}

		// Token: 0x04000BEA RID: 3050
		public InventoryItemType selectedItem;

		// Token: 0x04000BEB RID: 3051
		public AbstractEquipment equipment;

		// Token: 0x04000BED RID: 3053
		public const float RerollStatCostModifier = 1.5f;

		// Token: 0x04000BEE RID: 3054
		public const float RerollStatPenaltyWeight = 0.12f;

		// Token: 0x04000BF0 RID: 3056
		public const float UpgradeRarityCostModifier = 5f;

		// Token: 0x04000BF1 RID: 3057
		public const float UpgradeRarityPenaltyWeight = 0.2f;

		// Token: 0x04000BF3 RID: 3059
		public const float UpgradeLevelCostModifier = 6f;

		// Token: 0x04000BF4 RID: 3060
		public const float UpgradeLevelPenaltyWeight = 0.15f;

		// Token: 0x04000BF6 RID: 3062
		public const float ExtractBlueprintCostModifier = 20f;

		// Token: 0x04000BF8 RID: 3064
		public const float InstallAspectCostModifier = 3f;

		// Token: 0x04000BF9 RID: 3065
		public const float InstallAspectPenaltyWeight = 0.4f;

		// Token: 0x04000BFA RID: 3066
		public const float TradeRefinedProductMultiplier = 4f;

		// Token: 0x04000BFB RID: 3067
		public const float DailyTradeRefinedProductMultiplier = 6f;

		// Token: 0x04000BFC RID: 3068
		public float itemPenalty;

		// Token: 0x04000BFD RID: 3069
		[SerializeField]
		private DraggableWindowBar draggableWindowBar;

		// Token: 0x04000BFE RID: 3070
		[SerializeField]
		public GameObject noItemInformation;

		// Token: 0x04000BFF RID: 3071
		[SerializeField]
		public TMP_Text itemSpendage;

		// Token: 0x04000C00 RID: 3072
		[SerializeField]
		public TMP_Text itemModified;

		// Token: 0x04000C01 RID: 3073
		[SerializeField]
		public TMP_Text totalSpendage;

		// Token: 0x04000C02 RID: 3074
		[SerializeField]
		public TMP_Text workshopCreditText;

		// Token: 0x04000C03 RID: 3075
		[SerializeField]
		public TMP_Text itemOfTheDay;

		// Token: 0x04000C04 RID: 3076
		[SerializeField]
		private WorkshopItem workshopItem;

		// Token: 0x04000C05 RID: 3077
		[SerializeField]
		private WorkshopItem newWorkshopItem;

		// Token: 0x04000C06 RID: 3078
		private bool isItemEquipped;

		// Token: 0x04000C07 RID: 3079
		private SpaceShipData data;

		// Token: 0x04000C08 RID: 3080
		[SerializeField]
		private UpgradeRarityButton rarityButton;

		// Token: 0x04000C09 RID: 3081
		[SerializeField]
		private UpgradeLevelButton levelButton;

		// Token: 0x04000C0A RID: 3082
		[SerializeField]
		private ExtractBlueprintButton blueprintButton;

		// Token: 0x04000C0B RID: 3083
		private InventoryItemType dailyItem;
	}
}
