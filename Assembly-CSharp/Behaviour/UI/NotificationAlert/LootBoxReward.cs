using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.UI.Tooltip;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.NotificationAlert
{
	// Token: 0x02000252 RID: 594
	public class LootBoxReward : MonoBehaviour
	{
		// Token: 0x060015EA RID: 5610 RVA: 0x0008BD64 File Offset: 0x00089F64
		public IEnumerator SetLoot(InventoryItemType item, int amount)
		{
			this.InitLoot(item, amount);
			yield return new WaitForSeconds(0.3f);
			Color color = item.rarity.GetColor();
			color.a = 0f;
			this.badge.color = color;
			this.fadeItems.Add(this.badge, 0f);
			yield return new WaitForSeconds(0.3f);
			this.itemIcon.sprite = item.icon;
			this.fadeItems.Add(this.itemIcon, 0f);
			yield return new WaitForSeconds(0.3f);
			this.amountText.gameObject.SetActive(this.maxAmount > 1);
			if (this.maxAmount > 1)
			{
				this.currentAmount = 0;
				this.SetTextAlpha(this.amountText, 0f, this.creditItem);
				this.fadeItems.Add(this.amountText, 0f);
			}
			this.tooltip = base.gameObject.AddComponent<ItemTooltipSource>();
			this.tooltip.SetItem(item, 1, true, ItemTooltipContext.InInventory, false, null);
			yield break;
		}

		// Token: 0x060015EB RID: 5611 RVA: 0x0008BD84 File Offset: 0x00089F84
		public void SetLootImmediate(InventoryItemType item, int amount)
		{
			this.InitLoot(item, amount);
			this.badge.color = item.rarity.GetColor();
			this.SetImageAlpha(this.badge, 1f);
			this.itemIcon.sprite = item.icon;
			this.SetImageAlpha(this.itemIcon, 1f);
			this.amountText.gameObject.SetActive(this.maxAmount > 1);
			if (this.maxAmount > 1)
			{
				this.currentAmount = this.maxAmount;
				this.SetTextAlpha(this.amountText, 1f, this.creditItem);
			}
			this.tooltip = base.gameObject.AddComponent<ItemTooltipSource>();
			this.tooltip.SetItem(item, 1, true, ItemTooltipContext.InInventory, false, null);
		}

		// Token: 0x060015EC RID: 5612 RVA: 0x0008BE4C File Offset: 0x0008A04C
		private void InitLoot(InventoryItemType item, int amount)
		{
			this.amountText.gameObject.SetActive(false);
			this.SetImageAlpha(this.badge, 0f);
			this.SetImageAlpha(this.itemIcon, 0f);
			this.creditItem = false;
			UsableItem usableItem;
			if (item.TryGetComponent<UsableItem>(out usableItem))
			{
				CreditsItem creditsItem = usableItem as CreditsItem;
				if (creditsItem != null)
				{
					this.creditItem = true;
					this.maxAmount = creditsItem.amount * amount;
					return;
				}
			}
			this.maxAmount = amount;
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0008BEC4 File Offset: 0x0008A0C4
		private void Update()
		{
			foreach (MonoBehaviour monoBehaviour in this.fadeItems.Keys.ToList<MonoBehaviour>())
			{
				float num = this.fadeItems[monoBehaviour];
				Image image = monoBehaviour as Image;
				if (image != null && num < 1f)
				{
					this.SetImageAlpha(image, num);
				}
				else
				{
					TMP_Text tmp_Text = monoBehaviour as TMP_Text;
					if (tmp_Text != null)
					{
						if (num < 1f)
						{
							this.currentTimeFactor += Time.deltaTime * 2f;
							this.currentAmount = (int)((float)this.maxAmount * this.currentTimeFactor);
							this.SetTextAlpha(tmp_Text, num, this.creditItem);
						}
						else
						{
							this.currentAmount = this.maxAmount;
							this.SetTextAlpha(tmp_Text, 1f, this.creditItem);
						}
					}
				}
				num += Time.deltaTime * 2f;
				this.fadeItems[monoBehaviour] = num;
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0008BFD8 File Offset: 0x0008A1D8
		private void SetTextAlpha(TMP_Text text, float alpha, bool creditItem = false)
		{
			Color color = text.color;
			color.a = alpha;
			string text2 = GameMath.FormatNumber((float)this.currentAmount, -1);
			if (creditItem)
			{
				text.color = ColorHelper.creditsColor;
				text.text = "$" + text2;
				return;
			}
			text.text = text2;
			text.color = color;
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0008C030 File Offset: 0x0008A230
		private void SetImageAlpha(Image image, float alpha)
		{
			Color color = image.color;
			color.a = alpha;
			image.color = color;
		}

		// Token: 0x04000D26 RID: 3366
		[SerializeField]
		private Image badge;

		// Token: 0x04000D27 RID: 3367
		[SerializeField]
		private Image itemIcon;

		// Token: 0x04000D28 RID: 3368
		[SerializeField]
		private TMP_Text amountText;

		// Token: 0x04000D29 RID: 3369
		private ItemTooltipSource tooltip;

		// Token: 0x04000D2A RID: 3370
		private Dictionary<MonoBehaviour, float> fadeItems = new Dictionary<MonoBehaviour, float>();

		// Token: 0x04000D2B RID: 3371
		private int currentAmount;

		// Token: 0x04000D2C RID: 3372
		private float currentTimeFactor;

		// Token: 0x04000D2D RID: 3373
		private int maxAmount;

		// Token: 0x04000D2E RID: 3374
		private bool creditItem;
	}
}
