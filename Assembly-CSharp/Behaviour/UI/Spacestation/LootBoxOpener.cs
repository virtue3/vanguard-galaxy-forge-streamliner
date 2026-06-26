using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Tooltip;
using Behaviour.Util;
using Source.Item;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation
{
	// Token: 0x02000213 RID: 531
	public class LootBoxOpener : MonoBehaviour
	{
		// Token: 0x06001392 RID: 5010 RVA: 0x0007EE24 File Offset: 0x0007D024
		private void Awake()
		{
			this.button.gameObject.SetActive(false);
			if (this.skipAnimationsToggle != null)
			{
				this.skipAnimationsToggle.isOn = GameplayerPrefs.GetLootBoxSkipAnimations();
				this.skipAnimationsToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnSkipAnimationsChanged));
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0007EE7C File Offset: 0x0007D07C
		private void OnSkipAnimationsChanged(bool value)
		{
			GameplayerPrefs.SetLootBoxSkipAnimations(value);
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0007EE84 File Offset: 0x0007D084
		public static float GetTotalCost(List<Inventory.InventoryItem> boxes, int count)
		{
			int num = GamePlayer.current.CountAvailableItems("LockedContainerKey");
			float num2 = 0f;
			int num3 = 0;
			int num4 = 0;
			foreach (Inventory.InventoryItem inventoryItem in boxes)
			{
				float num5 = (float)GameMath.GetCreditsValue(50f, inventoryItem.item.itemLevel);
				int num6 = 0;
				while (num6 < inventoryItem.count && num4 < count)
				{
					if (num3 < num)
					{
						num3++;
					}
					else
					{
						num2 += num5;
					}
					num4++;
					num6++;
				}
				if (num4 >= count)
				{
					break;
				}
			}
			return num2;
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0007EF3C File Offset: 0x0007D13C
		public void SetLootBoxItem(LootBoxItem item, LootBoxNotification notification)
		{
			this.lootBoxNotification = notification;
			this.item = item;
			List<Inventory.InventoryItem> allLootBoxes = LootBoxOpener.GetAllLootBoxes();
			int num = allLootBoxes.Sum((Inventory.InventoryItem i) => i.count);
			if (item.CanUseKey())
			{
				this.keyImage.gameObject.SetActive(true);
				this.buttonText.gameObject.SetActive(false);
			}
			else
			{
				this.keyImage.gameObject.SetActive(false);
				this.buttonText.gameObject.SetActive(true);
				this.openPrice = (float)GameMath.GetCreditsValue(50f, item.item.itemLevel);
			}
			if (num > 1)
			{
				float totalCost = LootBoxOpener.GetTotalCost(allLootBoxes, num);
				this.openMultipleTooltip = this.openMultipleButton.GetComponent<TooltipSource>();
				this.buttonText.text = "$ " + GameMath.FormatNumber(this.openPrice, -1);
				this.allButtonText.text = num.ToString();
				if (this.openMultipleTooltip != null)
				{
					string text = "$ " + GameMath.FormatNumber(totalCost, -1);
					int num2 = GamePlayer.current.CountAvailableItems("LockedContainerKey");
					this.openMultipleTooltip.BodyText = Translation.Translate("@LootBoxOpenMore", new object[]
					{
						num,
						num2,
						text
					});
				}
			}
			else
			{
				this.buttonText.text = "$ " + GameMath.FormatNumber(this.openPrice, -1);
			}
			this.button.gameObject.SetActive(true);
			this.SetColor(item.item.rarity.GetColor());
			this.openMultipleContainer.gameObject.SetActive(num > 1);
			this.skipAnimationsToggle.gameObject.SetActive(num > 1);
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0007F11C File Offset: 0x0007D31C
		private void SetColor(Color color)
		{
			ColorBlock colors = this.button.colors;
			colors.highlightedColor = color;
			this.button.colors = colors;
			this.background.SetColor(color);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x0007F158 File Offset: 0x0007D358
		public static List<Inventory.InventoryItem> GetAllLootBoxes()
		{
			List<Inventory.InventoryItem> list = (from i in Inventory.cargo.items
			where i.item.GetComponent<LootBoxItem>()
			select i).ToList<Inventory.InventoryItem>();
			list.AddRange(from i in Inventory.global.items
			where i.item.GetComponent<LootBoxItem>()
			select i);
			return (from i in list
			orderby GameMath.GetCreditsValue(50f, i.item.itemLevel) descending
			select i).ToList<Inventory.InventoryItem>();
		}

		// Token: 0x06001398 RID: 5016 RVA: 0x0007F1F8 File Offset: 0x0007D3F8
		private bool CanAfford(float totalCost)
		{
			if ((float)GamePlayer.current.credits < totalCost)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
				return false;
			}
			return true;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0007F248 File Offset: 0x0007D448
		private void ShowItemMissingNotification()
		{
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSItemNotInInventory", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0007F27C File Offset: 0x0007D47C
		private void ProcessOpening(int count)
		{
			List<Dictionary<InventoryItemType, int>> allLoot = this.item.Open(count);
			this.lootBoxNotification.ShowLoot(allLoot, this.skipAnimationsToggle.isOn);
			this.item = null;
			this.buttonText.text = "";
			this.button.gameObject.SetActive(false);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0007F2D5 File Offset: 0x0007D4D5
		public void OnClickBox()
		{
			this.OpenSingleLootBox();
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x0007F2E0 File Offset: 0x0007D4E0
		public bool OpenSingleLootBox()
		{
			float totalCost = this.item.CanUseKey() ? 0f : this.openPrice;
			if (!this.CanAfford(totalCost))
			{
				return false;
			}
			this.ProcessOpening(1);
			return true;
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x0007F31B File Offset: 0x0007D51B
		public void OnClickOpenAll()
		{
			this.OpenAllLootBoxes();
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x0007F324 File Offset: 0x0007D524
		public bool OpenAllLootBoxes()
		{
			List<Inventory.InventoryItem> allLootBoxes = LootBoxOpener.GetAllLootBoxes();
			int num = GamePlayer.current.CountAvailableItems("LockedContainerKey");
			double num2 = (double)GamePlayer.current.credits;
			int num3 = 0;
			int num4 = num;
			foreach (Inventory.InventoryItem inventoryItem in allLootBoxes)
			{
				float num5 = (float)GameMath.GetCreditsValue(50f, inventoryItem.item.itemLevel);
				for (int i = 0; i < inventoryItem.count; i++)
				{
					if (num4 > 0)
					{
						num4--;
						num3++;
					}
					else
					{
						if (num2 < (double)num5)
						{
							goto IL_A3;
						}
						num2 -= (double)num5;
						num3++;
					}
				}
			}
			IL_A3:
			if (num3 > 0)
			{
				this.ProcessOpening(num3);
				return true;
			}
			return false;
		}

		// Token: 0x04000B30 RID: 2864
		[SerializeField]
		private Button button;

		// Token: 0x04000B31 RID: 2865
		[SerializeField]
		private TMP_Text buttonText;

		// Token: 0x04000B32 RID: 2866
		[SerializeField]
		private Image keyImage;

		// Token: 0x04000B33 RID: 2867
		[SerializeField]
		private LootBoxBackground background;

		// Token: 0x04000B34 RID: 2868
		[SerializeField]
		private RectTransform openMultipleContainer;

		// Token: 0x04000B35 RID: 2869
		[SerializeField]
		private Button openMultipleButton;

		// Token: 0x04000B36 RID: 2870
		[SerializeField]
		private TMP_Text allButtonText;

		// Token: 0x04000B37 RID: 2871
		[SerializeField]
		private Toggle skipAnimationsToggle;

		// Token: 0x04000B38 RID: 2872
		private TooltipSource openMultipleTooltip;

		// Token: 0x04000B39 RID: 2873
		protected LootBoxItem item;

		// Token: 0x04000B3A RID: 2874
		protected float openPrice;

		// Token: 0x04000B3B RID: 2875
		private LootBoxNotification lootBoxNotification;
	}
}
