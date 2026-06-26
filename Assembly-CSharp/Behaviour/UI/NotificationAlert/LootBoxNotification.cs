using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.UI.Spacestation;
using Source.Item;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.NotificationAlert
{
	// Token: 0x02000251 RID: 593
	public class LootBoxNotification : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x060015E1 RID: 5601 RVA: 0x0008B8D8 File Offset: 0x00089AD8
		public void ShowLoot(List<Dictionary<InventoryItemType, int>> allLoot, bool skipAnimations = false)
		{
			float waitTime = 0.5f;
			if (!skipAnimations)
			{
				int num = 0;
				foreach (Dictionary<InventoryItemType, int> dictionary in allLoot)
				{
					num += dictionary.Count;
				}
				if (num > 10)
				{
					waitTime = 0.2f;
				}
				if (num > 30)
				{
					waitTime = 0.1f;
				}
				if (num > 50)
				{
					waitTime = 0.05f;
				}
			}
			base.StartCoroutine(this.ShowLootOverTime(allLoot, waitTime, skipAnimations));
		}

		// Token: 0x060015E2 RID: 5602 RVA: 0x0008B964 File Offset: 0x00089B64
		private Dictionary<InventoryItemType, int> AggregateLoot(List<Dictionary<InventoryItemType, int>> allLoot)
		{
			Dictionary<InventoryItemType, int> dictionary = new Dictionary<InventoryItemType, int>();
			InventoryItemType inventoryItemType = null;
			int num = 0;
			foreach (Dictionary<InventoryItemType, int> dictionary2 in allLoot)
			{
				using (Dictionary<InventoryItemType, int>.Enumerator enumerator2 = dictionary2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<InventoryItemType, int> pair = enumerator2.Current;
						CreditsItem component = pair.Key.GetComponent<CreditsItem>();
						if (component)
						{
							num += component.amount;
							if (inventoryItemType == null || pair.Key.rarity > inventoryItemType.rarity)
							{
								inventoryItemType = pair.Key;
							}
						}
						else
						{
							KeyValuePair<InventoryItemType, int> keyValuePair = dictionary.FirstOrDefault((KeyValuePair<InventoryItemType, int> x) => x.Key.name == pair.Key.name);
							if (keyValuePair.Key != null)
							{
								Dictionary<InventoryItemType, int> dictionary3 = dictionary;
								InventoryItemType key = keyValuePair.Key;
								dictionary3[key] += pair.Value;
							}
							else
							{
								dictionary[pair.Key] = pair.Value;
							}
						}
					}
				}
			}
			if (inventoryItemType != null)
			{
				InventoryItemType inventoryItemType2 = ItemBuilder.Get("Credits").CreateCreditsItem(num);
				dictionary[inventoryItemType2] = 1;
				inventoryItemType2.GetComponent<CreditsItem>().SetCredits(num);
			}
			return dictionary;
		}

		// Token: 0x060015E3 RID: 5603 RVA: 0x0008BB14 File Offset: 0x00089D14
		private IEnumerator ShowLootOverTime(List<Dictionary<InventoryItemType, int>> allLoot, float waitTime, bool skipAnimations)
		{
			this.rewardsContainer.gameObject.SetActive(true);
			this.rewardsScrollView.gameObject.SetActive(true);
			this.lootBoxOpener.gameObject.SetActive(false);
			this.rewardsContainer.DestroyChildren();
			this.fireworksImage.gameObject.SetActive(true);
			if (!skipAnimations)
			{
				int boxIndex = 0;
				foreach (Dictionary<InventoryItemType, int> loot in allLoot)
				{
					if (boxIndex > 0)
					{
						yield return new WaitForSeconds(waitTime * 2f);
						this.rewardsContainer.DestroyChildren();
					}
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair in loot)
					{
						LootBoxReward lootBoxReward = UnityEngine.Object.Instantiate<LootBoxReward>(this.rewardPrefab, this.rewardsContainer);
						this.UpdateContainerWidth(loot.Count);
						yield return lootBoxReward.SetLoot(keyValuePair.Key, keyValuePair.Value);
						yield return new WaitForSeconds(waitTime);
					}
					Dictionary<InventoryItemType, int>.Enumerator enumerator2 = default(Dictionary<InventoryItemType, int>.Enumerator);
					int num = boxIndex;
					boxIndex = num + 1;
					loot = null;
				}
				List<Dictionary<InventoryItemType, int>>.Enumerator enumerator = default(List<Dictionary<InventoryItemType, int>>.Enumerator);
				yield return new WaitForSeconds(waitTime * 4f);
			}
			this.message.text = Translation.Translate("@LootBoxRewardSummary", Array.Empty<object>());
			this.rewardsContainer.DestroyChildren();
			List<KeyValuePair<InventoryItemType, int>> list = (from x in this.AggregateLoot(allLoot).ToList<KeyValuePair<InventoryItemType, int>>()
			orderby x.Key.GetComponent<CreditsItem>() != null descending, x.Key.rarity descending, x.Value descending
			select x).ToList<KeyValuePair<InventoryItemType, int>>();
			this.UpdateContainerWidth(list.Count);
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair2 in list)
			{
				UnityEngine.Object.Instantiate<LootBoxReward>(this.rewardPrefab, this.rewardsContainer).SetLootImmediate(keyValuePair2.Key, keyValuePair2.Value);
			}
			this.showingLoot = true;
			yield break;
			yield break;
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0008BB38 File Offset: 0x00089D38
		private void UpdateContainerWidth(int itemCount)
		{
			if (itemCount <= 0)
			{
				this.rewardsContainer.sizeDelta = new Vector2(0f, this.rewardsContainer.sizeDelta.y);
				return;
			}
			float num = (float)(itemCount * 52 + (itemCount - 1) * 10);
			this.rewardsContainer.sizeDelta = new Vector2(num, this.rewardsContainer.sizeDelta.y);
			float width = this.rewardsScrollView.rect.width;
			if (num < width)
			{
				this.rewardsContainer.pivot = new Vector2(0.5f, 0.5f);
				this.rewardsContainer.anchorMin = new Vector2(0.5f, 0.5f);
				this.rewardsContainer.anchorMax = new Vector2(0.5f, 0.5f);
				this.rewardsContainer.anchoredPosition = new Vector2(0f, 0f);
			}
			else
			{
				this.rewardsContainer.pivot = new Vector2(0f, 0.5f);
				this.rewardsContainer.anchorMin = new Vector2(0f, 0.5f);
				this.rewardsContainer.anchorMax = new Vector2(0f, 0.5f);
				this.rewardsContainer.anchoredPosition = new Vector2(0f, 0f);
			}
			ScrollRect component = this.rewardsScrollView.GetComponent<ScrollRect>();
			if (component != null)
			{
				component.horizontalNormalizedPosition = 0f;
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0008BCA8 File Offset: 0x00089EA8
		public void ShowLootBox(LootBoxItem lootBoxItem)
		{
			this.showingLoot = false;
			this.fireworksImage.gameObject.SetActive(false);
			this.rewardsContainer.gameObject.SetActive(false);
			this.rewardsScrollView.gameObject.SetActive(false);
			this.lootBoxOpener.SetLootBoxItem(lootBoxItem, this);
			this.borderImage.color = lootBoxItem.item.rarity.GetColor();
			this.borderImageBottom.color = lootBoxItem.item.rarity.GetColor();
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0008BD32 File Offset: 0x00089F32
		public void Hide()
		{
			if (this.showingLoot)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0008BD47 File Offset: 0x00089F47
		public void Destroy()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0008BD54 File Offset: 0x00089F54
		public void OnPointerDown(PointerEventData eventData)
		{
			this.Hide();
		}

		// Token: 0x04000D1D RID: 3357
		[SerializeField]
		public TMP_Text message;

		// Token: 0x04000D1E RID: 3358
		[SerializeField]
		private RectTransform rewardsContainer;

		// Token: 0x04000D1F RID: 3359
		[SerializeField]
		private RectTransform rewardsScrollView;

		// Token: 0x04000D20 RID: 3360
		[SerializeField]
		private LootBoxReward rewardPrefab;

		// Token: 0x04000D21 RID: 3361
		[SerializeField]
		private LootBoxOpener lootBoxOpener;

		// Token: 0x04000D22 RID: 3362
		[SerializeField]
		private Image borderImage;

		// Token: 0x04000D23 RID: 3363
		[SerializeField]
		private Image borderImageBottom;

		// Token: 0x04000D24 RID: 3364
		[SerializeField]
		private RawImage fireworksImage;

		// Token: 0x04000D25 RID: 3365
		private bool showingLoot;
	}
}
