using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.UI.Spacestation;
using Source.Mining;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Refinery
{
	// Token: 0x0200024A RID: 586
	public class RefineryJobTabContents : MonoBehaviour
	{
		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060015B0 RID: 5552 RVA: 0x0008AC58 File Offset: 0x00088E58
		public Source.Mining.Refinery refinery
		{
			get
			{
				return Source.Mining.Refinery.current;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060015B1 RID: 5553 RVA: 0x0008AC5F File Offset: 0x00088E5F
		// (set) Token: 0x060015B2 RID: 5554 RVA: 0x0008AC67 File Offset: 0x00088E67
		public OreItemData selectedOre { get; private set; }

		// Token: 0x060015B3 RID: 5555 RVA: 0x0008AC70 File Offset: 0x00088E70
		public void Hide()
		{
			base.gameObject.SetActive(false);
			this.emptyRefineryMessage.gameObject.SetActive(true);
			this.autorefineOption.anchoredPosition = new Vector2(this.autorefineOption.anchoredPosition.x, -8f);
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0008ACC0 File Offset: 0x00088EC0
		public void Show()
		{
			base.gameObject.SetActive(true);
			this.emptyRefineryMessage.gameObject.SetActive(false);
			this.autorefineOption.anchoredPosition = new Vector2(this.autorefineOption.anchoredPosition.x, -70f);
			this.oreParent.DestroyChildren();
			List<OreItemData> list = new List<OreItemData>();
			Dictionary<OreItemData, int> dictionary = new Dictionary<OreItemData, int>();
			foreach (KeyValuePair<OreItemData, int> keyValuePair in this.refinery.GetAvailableItems(true))
			{
				if (!list.Contains(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
					dictionary.Add(keyValuePair.Key, 0);
				}
				Dictionary<OreItemData, int> dictionary2 = dictionary;
				OreItemData key = keyValuePair.Key;
				dictionary2[key] += keyValuePair.Value;
			}
			list = (from o in list
			orderby o.GetComponent<InventoryItemType>().itemLevel, o.GetComponent<InventoryItemType>().name
			select o).ToList<OreItemData>();
			this.oreBadges = new List<RefineryOreBadge>();
			int num = 0;
			foreach (OreItemData oreItemData in list)
			{
				RefineryOreBadge refineryOreBadge = UnityEngine.Object.Instantiate<RefineryOreBadge>(this.orePrefab, this.oreParent);
				refineryOreBadge.SetOre(oreItemData, dictionary[oreItemData]);
				this.oreBadges.Add(refineryOreBadge);
				((RectTransform)refineryOreBadge.transform).anchoredPosition = new Vector2((float)num, 0f);
				num += 68;
			}
			this.oreParent.sizeDelta = new Vector2((float)num, this.oreParent.sizeDelta.y);
			if (list.Count > 0)
			{
				this.SetSelectedOre((this.selectedOre != null && list.Contains(this.selectedOre)) ? this.selectedOre : list[0]);
				return;
			}
			this.Hide();
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0008AF00 File Offset: 0x00089100
		public void SetSelectedOre(OreItemData ore)
		{
			if (ore != null)
			{
				this.selectedOre = ore;
				this.nameText.TL(ore.item.displayName, Array.Empty<object>());
				this.nameText.color = ore.item.rarity.GetColor();
				this.amountSlider.value = 1f;
				using (List<RefineryOreBadge>.Enumerator enumerator = this.oreBadges.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RefineryOreBadge refineryOreBadge = enumerator.Current;
						refineryOreBadge.SetSelected(refineryOreBadge.ore == ore);
					}
					goto IL_9E;
				}
			}
			this.Hide();
			IL_9E:
			this.UpdateAvailableOre();
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x0008AFC4 File Offset: 0x000891C4
		private void UpdateAvailableOre()
		{
			this.availableOre = 0;
			foreach (KeyValuePair<OreItemData, int> keyValuePair in this.refinery.GetAvailableItems(true))
			{
				if (keyValuePair.Key == this.selectedOre)
				{
					this.availableOre += keyValuePair.Value;
				}
			}
			this.amountSlider.maxValue = (float)this.availableOre;
			this.amountSlider.minValue = (float)((this.availableOre == 1) ? 0 : 1);
			this.amountSlider.value = (float)this.availableOre;
			this.amountText.SetTextWithoutNotify(this.availableOre.ToString());
			this.UpdateSliderValue();
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x0008B098 File Offset: 0x00089298
		public void UpdateSliderValue()
		{
			if (this.selectedOre == null)
			{
				return;
			}
			this.selectedCount = Mathf.RoundToInt(this.amountSlider.value);
			this.selectedCost = this.selectedOre.refinementCost * this.selectedCount;
			float totalSeconds = this.selectedOre.refinementTime * (float)this.selectedCount;
			this.amountText.SetTextWithoutNotify(this.selectedCount.ToString());
			this.timeText.text = Translation.Translate("@SSRefineryTime", new object[]
			{
				FormatString.FormatTime(totalSeconds)
			});
			this.costText.text = Translation.Translate("@SSRefineryCost", new object[]
			{
				this.selectedCost
			});
			this.resultParent.DestroyChildren();
			int num = 0;
			foreach (OreRefinementProduct oreRefinementProduct in this.selectedOre.contents)
			{
				RefineryResultBadge refineryResultBadge = UnityEngine.Object.Instantiate<RefineryResultBadge>(this.resultPrefab, this.resultParent);
				refineryResultBadge.SetContent(oreRefinementProduct.product, (float)this.selectedCount * oreRefinementProduct.yield);
				((RectTransform)refineryResultBadge.transform).anchoredPosition = new Vector2(0f, (float)num);
				num -= 32;
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0008B1F8 File Offset: 0x000893F8
		public void UpdateTextValue()
		{
			string text = this.amountText.text;
			if (string.IsNullOrEmpty(text) || text == "-")
			{
				this.amountSlider.SetValueWithoutNotify(this.amountSlider.minValue);
				return;
			}
			this.amountSlider.SetValueWithoutNotify(Mathf.Min(this.amountSlider.maxValue, float.Parse(this.amountText.text)));
			this.UpdateSliderValue();
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0008B26E File Offset: 0x0008946E
		public void ButtonToggle()
		{
			if (this.selectedCount < 1)
			{
				return;
			}
			if (this.refinery.TryStartJob(this.selectedOre, this.selectedCount))
			{
				this.Show();
				SpaceStationInterior.instance.UpdateJobs();
				return;
			}
			Debug.LogWarning("Failed to start refinery job!");
		}

		// Token: 0x04000CF3 RID: 3315
		[SerializeField]
		private RectTransform oreParent;

		// Token: 0x04000CF4 RID: 3316
		[SerializeField]
		private RefineryOreBadge orePrefab;

		// Token: 0x04000CF5 RID: 3317
		[SerializeField]
		private TMP_InputField amountText;

		// Token: 0x04000CF6 RID: 3318
		[SerializeField]
		private Slider amountSlider;

		// Token: 0x04000CF7 RID: 3319
		[SerializeField]
		private TMP_Text nameText;

		// Token: 0x04000CF8 RID: 3320
		[SerializeField]
		private TMP_Text costText;

		// Token: 0x04000CF9 RID: 3321
		[SerializeField]
		private TMP_Text timeText;

		// Token: 0x04000CFA RID: 3322
		[SerializeField]
		private RectTransform resultParent;

		// Token: 0x04000CFB RID: 3323
		[SerializeField]
		private RefineryResultBadge resultPrefab;

		// Token: 0x04000CFC RID: 3324
		[SerializeField]
		private RectTransform emptyRefineryMessage;

		// Token: 0x04000CFD RID: 3325
		[SerializeField]
		private RectTransform autorefineOption;

		// Token: 0x04000CFE RID: 3326
		private List<RefineryOreBadge> oreBadges;

		// Token: 0x04000D00 RID: 3328
		private int availableOre;

		// Token: 0x04000D01 RID: 3329
		private int selectedCount;

		// Token: 0x04000D02 RID: 3330
		private int selectedCost;
	}
}
