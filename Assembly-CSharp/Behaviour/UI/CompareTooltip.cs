using System;
using System.Collections.Generic;
using Behavior.Equipment.Booster;
using Behaviour.Crew;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Module;
using Behaviour.Equipment.Turret;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Mining;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Tooltip;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Simulation.Story;
using Source.SpaceShip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F8 RID: 504
	public class CompareTooltip : UITooltip
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0007BCA4 File Offset: 0x00079EA4
		public override float SizeY
		{
			get
			{
				if (this.compareContent == null)
				{
					return base.SizeY;
				}
				float num = base.SizeY;
				foreach (CompareTooltip compareTooltip in this.compareContent)
				{
					num = MathF.Max(num, compareTooltip.SizeY);
				}
				return num;
			}
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0007BD14 File Offset: 0x00079F14
		protected unsafe override void Update()
		{
			base.Update();
			if (this.compareContent != null)
			{
				if (Input.GetKeyDown(KeyCode.LeftShift))
				{
					GameplayerPrefs.SetShiftToggleCompare(!GameplayerPrefs.GetShiftToggleCompare());
				}
				bool shiftToggleCompare = GameplayerPrefs.GetShiftToggleCompare();
				if (shiftToggleCompare)
				{
					RectTransform rectTransform = base.transform as RectTransform;
					float num = rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x - 8f;
					if (num < 0f)
					{
						rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - num, rectTransform.anchoredPosition.y);
					}
				}
				float num2 = Mouse.current.scroll.y.value;
				bool isPressed = Keyboard.current.ctrlKey.isPressed;
				if (num2 != 0f && isPressed)
				{
					this.compareIndex += ((num2 > 0f) ? 1 : -1);
					if (this.compareIndex == this.compareContent.Count)
					{
						this.compareIndex = 0;
					}
					else if (this.compareIndex < 0)
					{
						this.compareIndex = this.compareContent.Count - 1;
					}
				}
				for (int i = 0; i < this.compareContent.Count; i++)
				{
					this.compareContent[i].gameObject.SetActive(shiftToggleCompare && i == this.compareIndex);
				}
				InventoryInteractionManager.Instance.DisableScrolling(isPressed);
			}
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0007BE7E File Offset: 0x0007A07E
		private void OnDestroy()
		{
			InventoryInteractionManager instance = InventoryInteractionManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.DisableScrolling(false);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0007BE90 File Offset: 0x0007A090
		public override void SetContent(TooltipSource tt)
		{
			ItemTooltipSource itemTooltipSource = (ItemTooltipSource)tt;
			this.SetContent(tt, itemTooltipSource.item, itemTooltipSource.count, itemTooltipSource.allowCompare, itemTooltipSource.context, itemTooltipSource.allowBuyback, itemTooltipSource.inInventory);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0007BED0 File Offset: 0x0007A0D0
		private void SetContent(TooltipSource tt, InventoryItemType item, int count, bool allowCompare, ItemTooltipContext context, bool allowBuyback, Inventory.InventoryItem inInventory = null)
		{
			if (item == null)
			{
				return;
			}
			bool contextForItem = CompareTooltip.GetContextForItem(item, ref context);
			bool flag = false;
			if (context == ItemTooltipContext.Compare)
			{
				flag = (count > 1);
				count = 1;
			}
			base.GetComponent<Image>().color = item.rarity.GetBackgroundColor();
			this.header.GetComponent<Image>().color = item.rarity.GetBackgroundColor();
			this.icon.sprite = item.icon;
			if (count > 1)
			{
				this.countLabel.text = GameMath.FormatNumber((float)count, -1);
				this.countLabel.gameObject.SetActive(true);
			}
			else
			{
				this.countLabel.gameObject.SetActive(false);
			}
			AbstractEquipment component = item.GetComponent<AbstractEquipment>();
			AbstractBooster component2 = item.GetComponent<AbstractBooster>();
			if (component && !component2)
			{
				this.headerLabel.TL("@EquipmentTooltipHeader", new object[]
				{
					component.size.GetDisplayName(),
					component.typeDisplayName
				});
			}
			else
			{
				this.headerLabel.TL(item.itemCategory.GetDisplayName(), Array.Empty<object>());
			}
			if (item.itemCategory.CanBeEquiped() && GamePlayer.current != null)
			{
				this.headerLabel.color = (item.CanBeEquippedOn(GamePlayer.current.currentSpaceShip) ? Color.white : ColorHelper.reddish);
			}
			this.header.sizeDelta = new Vector2(this.headerLabel.preferredWidth + 48f, this.header.sizeDelta.y);
			UITooltipText uitooltipText = base.AddTextLine(item.displayName, 16, 8f);
			uitooltipText.Text.rectTransform.offsetMax = new Vector2(-64f, 0f);
			uitooltipText.Text.color = item.rarity.GetColor();
			Manufacturer? manufacturer = item.GetManufacturer();
			if (manufacturer != null)
			{
				UITooltipText uitooltipText2 = base.AddTextLine(manufacturer.Value.GetDisplayName(), 12, 8f);
				uitooltipText2.Text.color = CompareTooltip.detailsColor;
				uitooltipText2.Text.rectTransform.offsetMax = new Vector2(-64f, 0f);
			}
			if (!item.missionItem && (item.equipmentBuilder != null || item.itemLevel > 0))
			{
				this.levelLabel.TL("@TooltipItemLevel", new object[]
				{
					item.itemLevel
				});
			}
			else
			{
				this.levelLabel.gameObject.SetActive(false);
			}
			if (component && !(component is ReactorModule) && !component2)
			{
				this.energyLabel.text = GameMath.FormatNumber(component.energyDraw, 1);
				this.energyLabel.color = ColorHelper.purpleBadge;
				this.energyLabel.rectTransform.sizeDelta = new Vector2(this.energyLabel.preferredWidth, 12f);
			}
			else
			{
				this.energyLabel.gameObject.SetActive(false);
			}
			if (component)
			{
				AbstractTurret abstractTurret = component as AbstractTurret;
				if (abstractTurret != null)
				{
					this.rangeLabel.text = GameMath.FormatNumber(abstractTurret.range, 1);
					this.rangeLabel.color = ColorHelper.greenBadge;
					this.rangeLabel.rectTransform.sizeDelta = new Vector2(this.rangeLabel.preferredWidth, 12f);
					goto IL_355;
				}
			}
			this.rangeLabel.gameObject.SetActive(false);
			IL_355:
			base.AddMinHeightSpacer(60f);
			base.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			UsableItem usableItem;
			if (component)
			{
				MainStat mainStat = component.GetMainStat();
				if (mainStat != null)
				{
					base.AddTextLine("<color=green>" + mainStat.mainStatAmount + "</color> " + Translation.Translate(mainStat.mainStatName, Array.Empty<object>()), 16, 8f).Text.color = CompareTooltip.detailsColor;
				}
				foreach (SubStat subStat in component.GetMainSubStats().subStatsList)
				{
					base.AddTextLine("  " + Translation.Translate(subStat.mainSubStatName, Array.Empty<object>()) + " " + subStat.mainSubStatAmount, 12, 8f).Text.color = CompareTooltip.detailsColor;
				}
				base.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
				bool flag2 = false;
				foreach (ValueTuple<EquipStatLine, int> valueTuple in component.GetStatsWithIndex(false))
				{
					EquipStatLine item2 = valueTuple.Item1;
					base.AddTextLine(item2.ToReadableString(false), 12, 8f).Text.color = CompareTooltip.modifierColor;
					flag2 = true;
				}
				AbstractTurret abstractTurret2 = component as AbstractTurret;
				if (abstractTurret2 != null && abstractTurret2.ammoType != null)
				{
					base.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
					base.AddTextLine("  " + Translation.Highlight("@AmmoType", ColorHelper.white75, new object[]
					{
						abstractTurret2.ammoType.displayName
					}), 12, 8f).Text.color = ColorHelper.boringGrey;
					flag2 = true;
				}
				foreach (AspectSlot aspect in component.aspectSlots)
				{
					flag2 = false;
					CompareTooltipAspect compareTooltipAspect = UnityEngine.Object.Instantiate<CompareTooltipAspect>(this.aspectPrefab, base.Content);
					compareTooltipAspect.SetAspect(aspect);
					base.AddContent(compareTooltipAspect);
				}
				if (flag2)
				{
					base.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
				}
			}
			else if (item.TryGetComponent<UsableItem>(out usableItem))
			{
				usableItem.AddToTooltip(this);
				base.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
			}
			else if (item.itemCategory == ItemCategory.TradeGoods && SkilltreeNode.economyTradeTooltip.isActive)
			{
				Economy storyteller = GamePlayer.current.GetStoryteller<Economy>();
				if (storyteller != null)
				{
					Economy.EconomyTradeResult tradeResult = storyteller.GetTradeResult(item);
					if (tradeResult.purchased > 0)
					{
						base.AddTextLine("@EconomyTradeResultHeader", 12, 8f);
						string text = tradeResult.isAverage ? "@EconomyTradeResultAvg" : "@EconomyTradeResult";
						base.AddTextLine(Translation.TranslateOnly(text, new object[]
						{
							tradeResult.purchased,
							item.displayName,
							tradeResult.cost
						}), 12, 8f);
						base.AddSeparator(new Color(0f, 0f, 0f, 0.5f), 2f, 0f, 8f);
					}
				}
			}
			OreItemData oreItemData;
			if (item.description != "")
			{
				base.AddTextLine("  \"" + Translation.Translate(item.description, Array.Empty<object>()) + "\"", 12, 8f).Text.color = CompareTooltip.detailsColor;
			}
			else if (item.TryGetComponent<OreItemData>(out oreItemData))
			{
				string description = oreItemData.GetDescription();
				base.AddTextLine("  \"" + description + "\"", 12, 8f).Text.color = CompareTooltip.detailsColor;
			}
			if (context == ItemTooltipContext.CraftingPreview && component)
			{
				base.AddTextLine("@SSForgePreview", 12, 8f).Text.color = ColorHelper.reddish;
			}
			if (context != ItemTooltipContext.Compare)
			{
				base.AddTextLine(Translation.Translate((count == 1) ? "@TooltipVolume1" : "@TooltipVolume", new object[]
				{
					item.m3,
					count,
					item.m3 * (float)count
				}), 12, 8f).Text.alignment = TextAlignmentOptions.TopRight;
			}
			item.RecalculateCost();
			int num = allowBuyback ? item.sellValue : item.cost;
			int sellValue = item.sellValue;
			if (context == ItemTooltipContext.Compare && flag)
			{
				base.AddTextLine("@TooltipCompareHintScroll", 12, 8f);
			}
			else if ((context == ItemTooltipContext.InInventory || context == ItemTooltipContext.InCarousel || context == ItemTooltipContext.CraftingPreview || context == ItemTooltipContext.InSalvage || context == ItemTooltipContext.InSpace) && sellValue > 0)
			{
				base.AddTextLine(Translation.Translate((count == 1) ? "@TooltipValue1" : "@TooltipValue", new object[]
				{
					sellValue,
					count,
					sellValue * count
				}), 12, 8f).Text.alignment = TextAlignmentOptions.TopRight;
			}
			else if (context == ItemTooltipContext.ToSell && sellValue > 0)
			{
				base.AddTextLine(Translation.Translate((count == 1) ? "@TooltipSellFor1" : "@TooltipSellFor", new object[]
				{
					sellValue,
					count,
					sellValue * count
				}), 12, 8f).Text.alignment = TextAlignmentOptions.TopRight;
			}
			else if (context == ItemTooltipContext.InShop && ((inInventory != null) ? inInventory.costItem : null))
			{
				UITooltipText uitooltipText3 = base.AddTextLine(Translation.Translate("@TooltipCostItem", new object[]
				{
					inInventory.costItemCount
				}), 12, 8f);
				uitooltipText3.Text.alignment = TextAlignmentOptions.TopRight;
				Vector2 sizeDelta = uitooltipText3.Text.rectTransform.sizeDelta;
				uitooltipText3.Text.rectTransform.sizeDelta = new Vector2(sizeDelta.x - 28f, sizeDelta.y);
				Image image = new GameObject("CurrencyIcon").AddComponent<Image>();
				image.transform.SetParent(uitooltipText3.Text.transform);
				image.rectTransform.anchoredPosition = new Vector2(4f, 8f);
				image.rectTransform.anchorMin = new Vector2(1f, 1f);
				image.rectTransform.anchorMax = new Vector2(1f, 1f);
				image.rectTransform.pivot = new Vector2(0f, 1f);
				image.rectTransform.sizeDelta = new Vector2(24f, 24f);
				image.sprite = inInventory.costItem.icon;
			}
			else if (context == ItemTooltipContext.InShop && num > 0)
			{
				string text2 = (count == 1) ? "@TooltipCost1" : "@TooltipCost";
				base.AddTextLine(allowBuyback ? Translation.Highlight(text2, ColorHelper.greenish, new object[]
				{
					num,
					count,
					num * count
				}) : Translation.Translate(text2, new object[]
				{
					num,
					count,
					num * count
				}), 12, 8f).Text.alignment = TextAlignmentOptions.TopRight;
			}
			if (allowCompare)
			{
				List<InventoryItemType> list = new List<InventoryItemType>(InventoryInteractionManager.Instance.GetItemTypesForCompare(item));
				foreach (InventoryItemType item3 in list)
				{
					CompareTooltip compareTooltip = (CompareTooltip)UnityEngine.Object.Instantiate<UITooltip>(tt.Prefab, base.transform);
					compareTooltip.enabled = false;
					compareTooltip.compareText.gameObject.SetActive(true);
					compareTooltip.SetContent(tt, item3, list.Count, false, ItemTooltipContext.Compare, false, null);
					compareTooltip.gameObject.SetActive(false);
					RectTransform rectTransform = (RectTransform)compareTooltip.transform;
					rectTransform.anchorMin = new Vector2(0f, 1f);
					rectTransform.anchorMax = new Vector2(0f, 1f);
					rectTransform.pivot = new Vector2(1f, 1f);
					rectTransform.anchoredPosition = new Vector2(-8f, 0f);
					if (this.compareContent == null)
					{
						base.AddTextLine("@TooltipCompareHint", 12, 8f);
						this.compareContent = new List<CompareTooltip>();
					}
					this.compareContent.Add(compareTooltip);
				}
				if (this.compareContent == null && item.itemCategory.CanBeEquiped() && !item.CanBeEquippedOn(GamePlayer.current.currentSpaceShip))
				{
					base.AddTextLine("@TooltipEquipUnavailableHint", 12, 8f).Text.color = ColorHelper.reddish;
				}
			}
			if (contextForItem)
			{
				this.AddContextClickActionText(item, context, count);
			}
			else if (context == ItemTooltipContext.InCarousel && SpaceStationInterior.instance && SpaceStationInterior.instance.currentTab == SpaceStationFacility.SalvageWorkshop)
			{
				base.AddTextLine("@TooltipUnequipHint", 12, 8f);
				if (item.CanGoInWorkshop())
				{
					base.AddTextLine("@TooltipWorkshopUsageLC", 12, 8f);
				}
			}
			else if (context == ItemTooltipContext.InCarousel && SpaceStationInterior.instance && SpaceStationInterior.instance.currentTab == SpaceStationFacility.PersonalHangar)
			{
				base.AddTextLine("@TooltipUnequipHint", 12, 8f);
			}
			base.AddTextLine("", 12, 16f);
			base.SetContent(tt);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0007CCF0 File Offset: 0x0007AEF0
		private static bool GetContextForItem(InventoryItemType item, ref ItemTooltipContext context)
		{
			bool result = true;
			GamePlayer current = GamePlayer.current;
			if (context == ItemTooltipContext.InInventory)
			{
				InventoryInteractionManager instance = InventoryInteractionManager.Instance;
				if (instance != null && instance.isShopOpen)
				{
					context = ItemTooltipContext.ToSell;
					return result;
				}
			}
			if (context == ItemTooltipContext.InInventory)
			{
				InventoryInteractionManager instance2 = InventoryInteractionManager.Instance;
				if (instance2 != null && instance2.isSalvageWorkshopOpen)
				{
					if (item.CanGoInWorkshop())
					{
						context = ItemTooltipContext.Workshop;
						return result;
					}
					if (item.CanRefinedProductGoInWorkshop())
					{
						context = ItemTooltipContext.RefinedProductWorkshop;
						return result;
					}
					return result;
				}
			}
			if (context == ItemTooltipContext.InInventory && item.CanBeEquippedOn(current.currentSpaceShip))
			{
				InventoryInteractionManager instance3 = InventoryInteractionManager.Instance;
				if (instance3 != null && instance3.isPersonalHangarOpen)
				{
					context = ItemTooltipContext.ToEquip;
					return result;
				}
			}
			if (context == ItemTooltipContext.InCarousel)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0007CD8C File Offset: 0x0007AF8C
		private void AddContextClickActionText(InventoryItemType item, ItemTooltipContext context, int count)
		{
			if (context == ItemTooltipContext.InInventory && item.CanFavourite())
			{
				base.AddTextLine("@TooltipFavouriteHint", 12, 8f);
			}
			if (context == ItemTooltipContext.InSpace)
			{
				base.AddTextLine("@TooltipTractorHint", 12, 8f);
			}
			else if (context == ItemTooltipContext.InInventory && item.IsUsable() && (!SpaceStationInterior.instance || item.IsUsableInSpaceStation()))
			{
				base.AddTextLine("@TooltipUsableHint", 12, 8f);
			}
			else if (context == ItemTooltipContext.InInventory && item.canJettison && !SpaceStationInterior.instance)
			{
				base.AddTextLine("@TooltipJettisonHint", 12, 8f);
				if (count > 1)
				{
					base.AddTextLine("@TooltipJettisonShiftClick", 12, 8f);
				}
			}
			else if (context == ItemTooltipContext.ToSell && item.canSell)
			{
				base.AddTextLine("@TooltipSellHint", 12, 8f);
				if (count > 1)
				{
					base.AddTextLine("@TooltipSellShiftClick", 12, 8f);
				}
			}
			else if (context == ItemTooltipContext.Workshop)
			{
				base.AddTextLine("@TooltipWorkshopUsage", 12, 8f);
			}
			else if (context == ItemTooltipContext.RefinedProductWorkshop)
			{
				base.AddTextLine("@TooltipWorkshopRefinedProductUsage", 12, 8f);
			}
			else if (context == ItemTooltipContext.ToEquip)
			{
				base.AddTextLine("@TooltipEquipHint", 12, 8f);
			}
			else if (context == ItemTooltipContext.InShop)
			{
				base.AddTextLine("@TooltipPurchaseHint", 12, 8f);
				if (count > 1)
				{
					base.AddTextLine("@TooltipPurchaseShiftClick", 12, 8f);
				}
				if (item.HasInfiniteShopSupply())
				{
					base.AddTextLine("@TooltipPurchaseHintInfinite", 12, 8f);
				}
			}
			if (count > 1 && context != ItemTooltipContext.InSpace && context != ItemTooltipContext.InShop && context != ItemTooltipContext.InCarousel && context != ItemTooltipContext.Compare)
			{
				base.AddTextLine("@TooltipStackHint", 12, 8f);
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0007CF50 File Offset: 0x0007B150
		public override void RefreshContent()
		{
			if (this.compareContent != null)
			{
				foreach (CompareTooltip compareTooltip in this.compareContent)
				{
					UnityEngine.Object.Destroy(compareTooltip.gameObject);
				}
				this.compareContent = null;
			}
			base.RefreshContent();
		}

		// Token: 0x04000AA6 RID: 2726
		[SerializeField]
		private Image icon;

		// Token: 0x04000AA7 RID: 2727
		[SerializeField]
		private TMP_Text countLabel;

		// Token: 0x04000AA8 RID: 2728
		[SerializeField]
		private TMP_Text levelLabel;

		// Token: 0x04000AA9 RID: 2729
		[SerializeField]
		private TMP_Text energyLabel;

		// Token: 0x04000AAA RID: 2730
		[SerializeField]
		private TMP_Text rangeLabel;

		// Token: 0x04000AAB RID: 2731
		[SerializeField]
		private TMP_Text headerLabel;

		// Token: 0x04000AAC RID: 2732
		[SerializeField]
		private TMP_Text compareText;

		// Token: 0x04000AAD RID: 2733
		[SerializeField]
		private RectTransform header;

		// Token: 0x04000AAE RID: 2734
		[SerializeField]
		private CompareTooltipAspect aspectPrefab;

		// Token: 0x04000AAF RID: 2735
		public static Color detailsColor = new Color(1f, 0.8920743f, 0.6650944f);

		// Token: 0x04000AB0 RID: 2736
		public static Color modifierColor = new Color(0.7972299f, 0.5471698f, 0.3639196f);

		// Token: 0x04000AB1 RID: 2737
		private List<CompareTooltip> compareContent;

		// Token: 0x04000AB2 RID: 2738
		private int compareIndex;
	}
}
