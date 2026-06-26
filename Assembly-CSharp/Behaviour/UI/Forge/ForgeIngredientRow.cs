using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.Item;
using Behaviour.UI.Tooltip;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviour.UI.Forge
{
	// Token: 0x02000291 RID: 657
	public class ForgeIngredientRow : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600180A RID: 6154 RVA: 0x00096984 File Offset: 0x00094B84
		// (set) Token: 0x0600180B RID: 6155 RVA: 0x0009698C File Offset: 0x00094B8C
		public InventoryItemType item { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x00096995 File Offset: 0x00094B95
		// (set) Token: 0x0600180D RID: 6157 RVA: 0x0009699D File Offset: 0x00094B9D
		public RefinedMaterial material { get; private set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x000969A6 File Offset: 0x00094BA6
		// (set) Token: 0x0600180F RID: 6159 RVA: 0x000969AE File Offset: 0x00094BAE
		public float baseAmount { get; private set; }

		// Token: 0x06001810 RID: 6160 RVA: 0x000969B7 File Offset: 0x00094BB7
		private void Awake()
		{
			this.tooltip = base.GetComponent<ItemTooltipSource>();
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x000969C5 File Offset: 0x00094BC5
		public void InitializeItem(InventoryItemType itemType, int count, bool isResult)
		{
			this.item = itemType;
			this.baseAmount = (float)count;
			this.isResult = isResult;
			this.isItem = true;
			this.UpdateUI(1f);
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x000969EF File Offset: 0x00094BEF
		public void InitializeMaterial(RefinedMaterial materialType, float amount, bool isResult)
		{
			this.material = materialType;
			this.item = null;
			this.baseAmount = amount;
			this.isResult = isResult;
			this.isItem = false;
			this.UpdateUI(1f);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00096A1F File Offset: 0x00094C1F
		public void UpdateAmount(float multiplier = 1f)
		{
			this.UpdateUI(multiplier);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00096A28 File Offset: 0x00094C28
		private void UpdateUI(float multiplier = 1f)
		{
			if (this.isItem)
			{
				this.UpdateItemUI(multiplier);
				return;
			}
			this.UpdateMaterialUI(multiplier);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00096A44 File Offset: 0x00094C44
		private void UpdateItemUI(float multiplier)
		{
			this.icon.sprite = this.item.icon;
			int num = SpaceStation.current.CountAvailableItems(this.item);
			int num2 = Mathf.RoundToInt(this.baseAmount * multiplier);
			this.label.text = Translation.Highlight(this.isResult ? "@CraftingResult" : "@CraftingIngredient", (num >= num2) ? ColorHelper.greenish : ColorHelper.reddish, new object[]
			{
				this.item.displayName,
				num2,
				num
			});
			this.label.rectTransform.sizeDelta = new Vector2(this.label.preferredWidth, this.label.rectTransform.sizeDelta.y);
			this.label.color = this.item.rarity.GetColor();
			if (this.tooltip != null)
			{
				this.tooltip.SetItem(this.item, num2, this.isResult, ItemTooltipContext.CraftingPreview, false, null);
			}
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x00096B5C File Offset: 0x00094D5C
		private void UpdateMaterialUI(float multiplier)
		{
			this.icon.sprite = this.material.GetIcon();
			float num = GamePlayer.current.CountRefinedMaterial(this.material);
			float num2 = this.baseAmount * multiplier;
			this.label.text = Translation.Highlight(this.isResult ? "@CraftingResult" : "@CraftingIngredient", (num >= num2) ? ColorHelper.greenish : ColorHelper.reddish, new object[]
			{
				this.material.GetDisplayName(),
				GameMath.FormatNumber(num2, 1),
				GameMath.FormatNumber(num, 1)
			});
			if (this.tooltip != null)
			{
				this.tooltip.enabled = false;
			}
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x00096C10 File Offset: 0x00094E10
		public void OnPointerClick(PointerEventData eventData)
		{
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.GetAvailable())
			{
				if (!(this.item == null) && craftingRecipe.displayName == this.item.displayName)
				{
					ForgeUI.current.SelectRecipe(craftingRecipe, new List<CraftingRecipe>
					{
						craftingRecipe
					}, null);
					break;
				}
			}
		}

		// Token: 0x04000F08 RID: 3848
		[SerializeField]
		private Image icon;

		// Token: 0x04000F09 RID: 3849
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04000F0D RID: 3853
		private bool isResult;

		// Token: 0x04000F0E RID: 3854
		private bool isItem;

		// Token: 0x04000F0F RID: 3855
		private ItemTooltipSource tooltip;
	}
}
