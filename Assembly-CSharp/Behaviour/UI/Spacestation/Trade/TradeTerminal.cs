using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Simulation.Economy;
using Source.Simulation.Story;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Behaviour.UI.Spacestation.Trade
{
	// Token: 0x02000217 RID: 535
	public class TradeTerminal : MonoBehaviour
	{
		// Token: 0x060013E7 RID: 5095 RVA: 0x00080A26 File Offset: 0x0007EC26
		private void Start()
		{
			this.UpdateItems();
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x00080A30 File Offset: 0x0007EC30
		private void UpdateItems()
		{
			this.rowParent.DestroyChildren();
			SpaceStation current = SpaceStation.current;
			LocalEconomy localEconomy = (current != null) ? current.economy : null;
			if (localEconomy == null)
			{
				return;
			}
			if (localEconomy.craftingDealItem != null && localEconomy.craftingDealCount > 0 && SkilltreeNode.economyTradeCraftedDeal.isActive)
			{
				localEconomy.craftingDealCooldown = 10;
				UnityEngine.Object.Instantiate<TradeTerminalCraftingDeal>(this.craftingDealPrefab, this.rowParent).SetDeal(localEconomy.craftingDealItem, localEconomy.craftingDealCount, localEconomy.craftingDealValue);
			}
			List<LocalEconomyItem> list = new List<LocalEconomyItem>(localEconomy.allItems);
			list.Sort((LocalEconomyItem a, LocalEconomyItem b) => a.cost.CompareTo(b.cost));
			foreach (LocalEconomyItem item in list)
			{
				UnityEngine.Object.Instantiate<TradeTerminalRow>(this.rowPrefab, this.rowParent).SetItem(item);
			}
			this.DestroyPopup();
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x00080B38 File Offset: 0x0007ED38
		private void Update()
		{
			this.updateTimer -= Time.deltaTime;
			if (this.updateTimer < 0f)
			{
				this.updateTimer = 0.5f;
				float economyTimer = GamePlayer.current.GetStoryteller<Economy>().economyTimer;
				if (economyTimer > this.economyTime && this.economyTime != 0f)
				{
					this.UpdateItems();
				}
				this.economyTime = economyTimer;
				this.timer.TL("@TTTimer", new object[]
				{
					GameMath.FormatTime(Mathf.CeilToInt(this.economyTime), true)
				});
			}
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x00080BCC File Offset: 0x0007EDCC
		public void StartBuy(LocalEconomyItem item)
		{
			this.actionPopup = UnityEngine.Object.Instantiate<DragableActionPopup>(this.popupPrefab, base.transform);
			this.actionPopup.SetInventoryItem(item.item, "Buy", item.currentSupply, new int?(item.cost), null);
			this.actionPopup.cancelButton.onClick.AddListener(new UnityAction(this.OnCancel));
			this.actionPopup.actionButton.onClick.AddListener(delegate()
			{
				this.BuyAmount(item, this.actionPopup.GetAmount());
				this.UpdateItems();
			});
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x00080C80 File Offset: 0x0007EE80
		private bool BuyAmount(LocalEconomyItem item, int amount)
		{
			if (amount <= 0 || amount > item.currentSupply)
			{
				return false;
			}
			Inventory cargo = GamePlayer.current.currentSpaceShip.cargo;
			if (cargo.IsFull(item.item.m3 * (float)amount))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSCargoFull", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			int num = item.cost * amount;
			if (!GamePlayer.current.CanAfford((float)num))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			GamePlayer.current.RemoveCredits((float)num);
			cargo.Add(item.item, amount, false, false);
			item.currentSupply -= amount;
			this.LogTrade(item, amount);
			return true;
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x00080D60 File Offset: 0x0007EF60
		public void StartSell(LocalEconomyItem item)
		{
			this.actionPopup = UnityEngine.Object.Instantiate<DragableActionPopup>(this.popupPrefab, base.transform);
			this.actionPopup.SetInventoryItem(item.item, "Sell", GamePlayer.current.CountAvailableItems(item.item), new int?(item.cost), null);
			this.actionPopup.cancelButton.onClick.AddListener(new UnityAction(this.OnCancel));
			this.actionPopup.actionButton.onClick.AddListener(delegate()
			{
				this.SellAmount(item, this.actionPopup.GetAmount());
				this.UpdateItems();
			});
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00080E1C File Offset: 0x0007F01C
		private bool SellAmount(LocalEconomyItem item, int amount)
		{
			if (amount <= 0 || amount > GamePlayer.current.CountAvailableItems(item.item))
			{
				return false;
			}
			GamePlayer.current.ConsumeAvailableItems(item.item, amount);
			GamePlayer.current.credits += (long)(item.cost * amount);
			item.currentSupply += amount;
			this.LogTrade(item, -amount);
			return true;
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x00080E88 File Offset: 0x0007F088
		public void StartCraftingDeal()
		{
			TradeTerminal.ClosureClass15_0 _locals_1 = new TradeTerminal.ClosureClass15_0();
			_locals_1._thisRef = this;
			TradeTerminal.ClosureClass15_0 _locals_2 = _locals_1;
			SpaceStation current = SpaceStation.current;
			_locals_2.economy = ((current != null) ? current.economy : null);
			if (_locals_1.economy == null || _locals_1.economy.craftingDealItem == null)
			{
				return;
			}
			this.actionPopup = UnityEngine.Object.Instantiate<DragableActionPopup>(this.popupPrefab, base.transform);
			this.actionPopup.SetInventoryItem(_locals_1.economy.craftingDealItem, "Sell", Mathf.Min(_locals_1.economy.craftingDealCount, GamePlayer.current.CountAvailableItems(_locals_1.economy.craftingDealItem)), new int?(_locals_1.economy.craftingDealValue), null);
			this.actionPopup.cancelButton.onClick.AddListener(new UnityAction(this.OnCancel));
			this.actionPopup.actionButton.onClick.AddListener(delegate()
			{
				_locals_1._thisRef.SellDealAmount(_locals_1.economy, _locals_1._thisRef.actionPopup.GetAmount());
				_locals_1._thisRef.UpdateItems();
			});
		}

		// Token: 0x060013EF RID: 5103 RVA: 0x00080F80 File Offset: 0x0007F180
		private bool SellDealAmount(LocalEconomy econ, int amount)
		{
			if (amount <= 0 || econ.craftingDealItem == null || amount > GamePlayer.current.CountAvailableItems(econ.craftingDealItem))
			{
				return false;
			}
			GamePlayer.current.ConsumeAvailableItems(econ.craftingDealItem, amount);
			GamePlayer.current.credits += (long)(econ.craftingDealValue * amount);
			econ.craftingDealCount -= amount;
			return true;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00080FF0 File Offset: 0x0007F1F0
		private void LogTrade(LocalEconomyItem item, int amount)
		{
			Economy storyteller = GamePlayer.current.GetStoryteller<Economy>();
			if (storyteller == null)
			{
				return;
			}
			storyteller.AddTrade(item.item, amount, item.cost);
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x0008101F File Offset: 0x0007F21F
		private void DestroyPopup()
		{
			if (this.actionPopup)
			{
				UnityEngine.Object.Destroy(this.actionPopup.gameObject);
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x0008103E File Offset: 0x0007F23E
		private void OnCancel()
		{
			this.DestroyPopup();
		}

		// Token: 0x04000B75 RID: 2933
		[SerializeField]
		private TMP_Text timer;

		// Token: 0x04000B76 RID: 2934
		[SerializeField]
		private RectTransform rowParent;

		// Token: 0x04000B77 RID: 2935
		[SerializeField]
		private TradeTerminalRow rowPrefab;

		// Token: 0x04000B78 RID: 2936
		[SerializeField]
		private TradeTerminalCraftingDeal craftingDealPrefab;

		// Token: 0x04000B79 RID: 2937
		[SerializeField]
		private DragableActionPopup popupPrefab;

		// Token: 0x04000B7A RID: 2938
		private DragableActionPopup actionPopup;

		// Token: 0x04000B7B RID: 2939
		private float economyTime;

		// Token: 0x04000B7C RID: 2940
		private float updateTimer;
	}
}
