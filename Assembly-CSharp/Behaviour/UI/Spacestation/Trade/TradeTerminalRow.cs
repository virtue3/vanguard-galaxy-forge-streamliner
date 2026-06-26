using System;
using System.Collections;
using System.Collections.Generic;
using Behaviour.Equipment.Builder;
using Behaviour.UI.Tooltip;
using Source.Player;
using Source.Simulation.Economy;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Spacestation.Trade
{
	// Token: 0x0200021A RID: 538
	public class TradeTerminalRow : MonoBehaviour
	{
		// Token: 0x060013F8 RID: 5112 RVA: 0x00081180 File Offset: 0x0007F380
		public void SetItem(LocalEconomyItem item)
		{
			this.item = item;
			this._tooltip.SetItem(item.item, 1, false, ItemTooltipContext.CraftingPreview, false, null);
			this._itemName.TL(item.item.displayName, Array.Empty<object>());
			this._itemName.color = item.item.rarity.GetColor();
			Manufacturer? manufacturer = item.item.GetManufacturer();
			if (manufacturer != null)
			{
				this._manufacturer.TL(manufacturer.Value.GetDisplayName(), Array.Empty<object>());
			}
			else
			{
				this._manufacturer.gameObject.SetActive(false);
			}
			this._icon.sprite = item.item.icon;
			this._costText.text = "$" + GameMath.FormatNumber((float)item.cost, -1);
			if (item.currentSupply > 0)
			{
				this._buyableCount.TL("@TTItemsToBuy", new object[]
				{
					item.currentSupply
				});
				this._buyableCount.gameObject.SetActive(true);
				this._buyButton.gameObject.SetActive(true);
			}
			else
			{
				this._buyableCount.gameObject.SetActive(false);
				this._buyButton.gameObject.SetActive(false);
			}
			int num = GamePlayer.current.CountAvailableItems(item.item);
			if (num > 0)
			{
				this._sellableCount.TL("@TTItemsToSell", new object[]
				{
					num
				});
				this._sellableCount.gameObject.SetActive(true);
				this._sellButton.gameObject.SetActive(true);
			}
			else
			{
				this._sellableCount.gameObject.SetActive(false);
				this._sellButton.gameObject.SetActive(false);
			}
			float num2 = item.currentValue;
			float num3 = item.currentValue;
			for (int i = 0; i < item.historicalValue.Length; i++)
			{
				num2 = Mathf.Min(num2, item.historicalValue[i]);
				num3 = Mathf.Max(num3, item.historicalValue[i]);
			}
			this._stats.TL("@TTStats", new object[]
			{
				item.GetCost(num3),
				item.GetCost(num2),
				item.GetCost(item.historicalValue[item.historicalValue.Length - 1]),
				item.cost
			});
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x000813EF File Offset: 0x0007F5EF
		private void Start()
		{
			base.StartCoroutine(this.CoroutineGraph());
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x000813FE File Offset: 0x0007F5FE
		private IEnumerator CoroutineGraph()
		{
			yield return null;
			List<float> list = new List<float>(this.item.historicalValue);
			list.Reverse();
			list.Add(this.item.currentValue);
			this.DrawGraph(list, (list[list.Count - 1] > list[0]) ? ColorHelper.greenish : ColorHelper.reddish);
			yield break;
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00081410 File Offset: 0x0007F610
		private void DrawGraph(List<float> values, Color c)
		{
			float num = 4f;
			float num2 = this._graphParent.rect.width - num * 2f;
			float num3 = this._graphParent.rect.height - num * 2f;
			if (num2 <= 0f || num3 <= 0f)
			{
				return;
			}
			float num4 = values[0];
			float num5 = values[0];
			for (int i = 1; i < values.Count; i++)
			{
				if (values[i] < num4)
				{
					num4 = values[i];
				}
				if (values[i] > num5)
				{
					num5 = values[i];
				}
			}
			float num6 = num5 - num4;
			float num7 = num2 / (float)(values.Count - 1);
			Vector2 a = default(Vector2);
			for (int j = 0; j < values.Count; j++)
			{
				float num8 = (values[j] - num4) / num6;
				Vector2 vector = new Vector2(num + num7 * (float)j, num + num8 * num3);
				if (j > 0)
				{
					this.SpawnSegment(a, vector, c);
				}
				a = vector;
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00081528 File Offset: 0x0007F728
		private void SpawnSegment(Vector2 a, Vector2 b, Color c)
		{
			Image image = UnityEngine.Object.Instantiate<Image>(this._linePrefab, this._graphParent);
			RectTransform rectTransform = image.rectTransform;
			Vector2 vector = new Vector2(0f, 0f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			Vector2 vector2 = b - a;
			float magnitude = vector2.magnitude;
			Vector2 anchoredPosition = (a + b) * 0.5f;
			rectTransform.anchoredPosition = anchoredPosition;
			rectTransform.sizeDelta = new Vector2(magnitude, 2f);
			float z = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			rectTransform.localRotation = Quaternion.Euler(0f, 0f, z);
			image.color = c;
			image.gameObject.SetActive(true);
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x000815E6 File Offset: 0x0007F7E6
		public void ButtonBuy()
		{
			base.GetComponentInParent<TradeTerminal>().StartBuy(this.item);
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x000815F9 File Offset: 0x0007F7F9
		public void ButtonSell()
		{
			base.GetComponentInParent<TradeTerminal>().StartSell(this.item);
		}

		// Token: 0x04000B84 RID: 2948
		[SerializeField]
		private ItemTooltipSource _tooltip;

		// Token: 0x04000B85 RID: 2949
		[SerializeField]
		private TMP_Text _itemName;

		// Token: 0x04000B86 RID: 2950
		[SerializeField]
		private TMP_Text _manufacturer;

		// Token: 0x04000B87 RID: 2951
		[SerializeField]
		private Image _icon;

		// Token: 0x04000B88 RID: 2952
		[SerializeField]
		private TMP_Text _costText;

		// Token: 0x04000B89 RID: 2953
		[SerializeField]
		private TMP_Text _buyableCount;

		// Token: 0x04000B8A RID: 2954
		[SerializeField]
		private TMP_Text _sellableCount;

		// Token: 0x04000B8B RID: 2955
		[SerializeField]
		private Button _buyButton;

		// Token: 0x04000B8C RID: 2956
		[SerializeField]
		private Button _sellButton;

		// Token: 0x04000B8D RID: 2957
		[SerializeField]
		private RectTransform _graphParent;

		// Token: 0x04000B8E RID: 2958
		[SerializeField]
		private Image _linePrefab;

		// Token: 0x04000B8F RID: 2959
		[SerializeField]
		private TMP_Text _stats;

		// Token: 0x04000B90 RID: 2960
		private LocalEconomyItem item;

		// Token: 0x04000B91 RID: 2961
		private bool graphDrawn;
	}
}
