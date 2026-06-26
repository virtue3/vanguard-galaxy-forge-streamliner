using System;
using Behaviour.Item;
using Source.Item;
using Source.Mining;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI
{
	// Token: 0x020001F1 RID: 497
	public class DragableActionPopup : AbstractPopup
	{
		// Token: 0x060012CA RID: 4810 RVA: 0x0007AC74 File Offset: 0x00078E74
		public void SetInventoryItem(InventoryItemType item, string actionText, int amount, int? overrideCost = null, InventoryItemType costItem = null)
		{
			this.item = item;
			this.overrideCost = overrideCost;
			this.icon.sprite = item.icon;
			this.title.TL(item.displayName, Array.Empty<object>());
			this.actionLabel.text = actionText;
			this.actionButton.GetComponentInChildren<TextMeshProUGUI>().text = actionText;
			this.maxAmount = amount;
			this.action = actionText;
			this.slider.minValue = 1f;
			this.slider.maxValue = (float)amount;
			this.slider.SetValueWithoutNotify((float)amount);
			this.input.SetTextWithoutNotify(amount.ToString());
			if (actionText == "Trade in")
			{
				this.totalCost.gameObject.SetActive(true);
				this.totalCost.color = ColorHelper.creditsColor;
			}
			else if (actionText != "Transfer")
			{
				this.totalCost.gameObject.SetActive(true);
				this.totalCost.color = ((actionText == "Buy") ? Color.red : Color.green);
			}
			else
			{
				this.totalCost.gameObject.SetActive(false);
			}
			this.currencyItem = costItem;
			this.UpdateCosts(amount);
			this.UpdateVolume(amount);
		}

		// Token: 0x060012CB RID: 4811 RVA: 0x0007ADBC File Offset: 0x00078FBC
		public void InputChanged()
		{
			int num;
			if (int.TryParse(this.input.text, out num))
			{
				num = Mathf.Min(this.maxAmount, num);
				this.input.SetTextWithoutNotify(num.ToString());
				this.slider.SetValueWithoutNotify((float)num);
				this.UpdateCosts(num);
				this.UpdateVolume(num);
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0007AE18 File Offset: 0x00079018
		public void SliderChanged()
		{
			int amount = Mathf.RoundToInt(this.slider.value);
			this.input.SetTextWithoutNotify(amount.ToString());
			this.UpdateCosts(amount);
			this.UpdateVolume(amount);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0007AE58 File Offset: 0x00079058
		private void UpdateCosts(int amount)
		{
			if (this.action == "Buy")
			{
				float num = (float)(amount * (this.overrideCost ?? this.item.cost));
				if (this.currencyItem)
				{
					this.totalCost.TL("@UICommendationCostText", new object[]
					{
						this.currencyItem.displayName,
						num
					});
					this.totalCost.color = (((float)GamePlayer.current.CountAvailableItems(this.currencyItem) < num) ? ColorHelper.reddish : ColorHelper.greenish);
					return;
				}
				this.totalCost.TL("@UICostText", new object[]
				{
					num
				});
				this.totalCost.color = (GamePlayer.current.CanAfford(num) ? ColorHelper.greenish : ColorHelper.reddish);
				return;
			}
			else
			{
				if (this.action == "Sell")
				{
					this.totalCost.TL("@UIProfitText", new object[]
					{
						amount * (this.overrideCost ?? this.item.sellValue)
					});
					this.totalCost.color = ColorHelper.greenish;
					return;
				}
				if (this.action == "Trade in")
				{
					this.totalCost.TL("@SalWorCreditValue", new object[]
					{
						amount * (this.overrideCost ?? this.item.cost)
					});
					this.totalCost.color = ColorHelper.greenish;
					return;
				}
				if (this.action == "Extract")
				{
					float num2 = (float)Source.Mining.Refinery.GetExtractCost(this.extractMaterial, amount);
					this.totalCost.TL("@UICostText", new object[]
					{
						num2
					});
					this.totalCost.color = (GamePlayer.current.CanAfford(num2) ? ColorHelper.greenish : ColorHelper.reddish);
					return;
				}
				if (this.action == "Exchange")
				{
					this.totalCost.text = Translation.Translate(this.exchangeItem.displayName, Array.Empty<object>()) + " x" + GameMath.FormatNumber((float)(this.overrideCost.Value * amount), -1);
				}
				return;
			}
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0007B0D0 File Offset: 0x000792D0
		private void UpdateVolume(int amount)
		{
			float num = (float)amount * this.item.m3;
			this.totalVolume.TL("@UIVolumeText", new object[]
			{
				num
			});
			if (this.action == "Buy" && this.targetInventory != null)
			{
				this.totalVolume.color = ((num < this.targetInventory.GetSpaceAvailable()) ? ColorHelper.greenish : ColorHelper.reddish);
			}
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0007B14A File Offset: 0x0007934A
		public void HideCost()
		{
			this.totalCost.gameObject.SetActive(false);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0007B15D File Offset: 0x0007935D
		public int GetAmount()
		{
			return int.Parse(this.input.text);
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0007B16F File Offset: 0x0007936F
		public override void Reset()
		{
			base.Reset();
			this.slider.value = this.slider.maxValue;
		}

		// Token: 0x04000A89 RID: 2697
		[SerializeField]
		private Image icon;

		// Token: 0x04000A8A RID: 2698
		[SerializeField]
		private Slider slider;

		// Token: 0x04000A8B RID: 2699
		[SerializeField]
		private TextMeshProUGUI totalCost;

		// Token: 0x04000A8C RID: 2700
		[SerializeField]
		private TextMeshProUGUI totalVolume;

		// Token: 0x04000A8D RID: 2701
		private int maxAmount;

		// Token: 0x04000A8E RID: 2702
		private int? overrideCost;

		// Token: 0x04000A8F RID: 2703
		private InventoryItemType currencyItem;

		// Token: 0x04000A90 RID: 2704
		private InventoryItemType item;

		// Token: 0x04000A91 RID: 2705
		public Inventory targetInventory;

		// Token: 0x04000A92 RID: 2706
		private string action;

		// Token: 0x04000A93 RID: 2707
		public RefinedMaterial extractMaterial;

		// Token: 0x04000A94 RID: 2708
		public InventoryItemType exchangeItem;
	}
}
