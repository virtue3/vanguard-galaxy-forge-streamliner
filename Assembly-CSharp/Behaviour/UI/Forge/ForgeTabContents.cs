using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crafting;
using Behaviour.Item;
using Behaviour.UI.Spacestation;
using Behaviour.UI.Tooltip;
using Source.Item;
using Source.Mining;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Behaviour.UI.Forge
{
	// Token: 0x02000293 RID: 659
	public class ForgeTabContents : MonoBehaviour
	{
		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x00096E31 File Offset: 0x00095031
		public Source.Mining.Forge forge
		{
			get
			{
				return Source.Mining.Forge.current;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06001824 RID: 6180 RVA: 0x00096E38 File Offset: 0x00095038
		// (set) Token: 0x06001825 RID: 6181 RVA: 0x00096E40 File Offset: 0x00095040
		public CraftingRecipe parentRecipe { get; private set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001826 RID: 6182 RVA: 0x00096E49 File Offset: 0x00095049
		// (set) Token: 0x06001827 RID: 6183 RVA: 0x00096E51 File Offset: 0x00095051
		public CraftingRecipe subRecipe { get; private set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x00096E5A File Offset: 0x0009505A
		// (set) Token: 0x06001829 RID: 6185 RVA: 0x00096E62 File Offset: 0x00095062
		public List<CraftingRecipe> unlockedRecipes { get; private set; }

		// Token: 0x0600182A RID: 6186 RVA: 0x00096E6B File Offset: 0x0009506B
		private void Start()
		{
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00096E70 File Offset: 0x00095070
		public void ToggleSubRecipe()
		{
			if (this.unlockedRecipes.Count < 2)
			{
				return;
			}
			int index = (this.unlockedRecipes.IndexOf(this.subRecipe) + 1) % this.unlockedRecipes.Count;
			this.SetSelectedRecipe(this.parentRecipe, this.unlockedRecipes[index], this.unlockedRecipes);
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00096ECC File Offset: 0x000950CC
		public bool SetRaritySubRecipe(Rarity rarity)
		{
			CraftingRecipe subRecipe = this.unlockedRecipes.FirstOrDefault((CraftingRecipe u) => u.craftingRarity == rarity);
			this.SetSelectedRecipe(this.parentRecipe, subRecipe, this.unlockedRecipes);
			return true;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x00096F14 File Offset: 0x00095114
		public bool SetSelectedRecipe(CraftingRecipe parentRecipe, CraftingRecipe subRecipe, List<CraftingRecipe> availableSubRecipes)
		{
			this.parentRecipe = parentRecipe;
			if (this.subRecipe != subRecipe)
			{
				this.previousSelectionCount = -1;
			}
			this.subRecipe = subRecipe;
			this.unlockedRecipes = (from u in availableSubRecipes
			orderby u.craftingRarity
			select u).ToList<CraftingRecipe>();
			this.SetupButtons();
			this.recipeIcon.sprite = subRecipe.icon;
			this.recipeIcon.GetComponent<ItemTooltipSource>().SetItem(subRecipe.GetResultForTooltip(), 1, true, ItemTooltipContext.CraftingPreview, false, null);
			if (parentRecipe.subRecipes.Count > 1)
			{
				this.ingredientsLabel.offsetMin = new Vector2(138f, this.ingredientsLabel.offsetMin.y);
				this.subRecipeHolder.gameObject.SetActive(true);
			}
			else
			{
				this.ingredientsLabel.offsetMin = new Vector2(16f, this.ingredientsLabel.offsetMin.y);
				this.subRecipeHolder.gameObject.SetActive(false);
			}
			this.ingredientsParent.DestroyChildren();
			this.ingredients.Clear();
			int num = 0;
			foreach (ValueTuple<RefinedMaterial, float> valueTuple in subRecipe.GetIngredientMaterials(0))
			{
				ForgeIngredientRow forgeIngredientRow = UnityEngine.Object.Instantiate<ForgeIngredientRow>(this.ingredientPrefab, this.ingredientsParent);
				forgeIngredientRow.InitializeMaterial(valueTuple.Item1, valueTuple.Item2, false);
				this.ingredients.Add(forgeIngredientRow);
				((RectTransform)forgeIngredientRow.transform).anchoredPosition = new Vector2(0f, (float)num);
				num -= 24;
			}
			foreach (ValueTuple<InventoryItemType, int> valueTuple2 in subRecipe.GetIngredientItems(0))
			{
				ForgeIngredientRow forgeIngredientRow2 = UnityEngine.Object.Instantiate<ForgeIngredientRow>(this.ingredientPrefab, this.ingredientsParent);
				forgeIngredientRow2.InitializeItem(valueTuple2.Item1, valueTuple2.Item2, false);
				this.ingredients.Add(forgeIngredientRow2);
				((RectTransform)forgeIngredientRow2.transform).anchoredPosition = new Vector2(0f, (float)num);
				num -= 24;
			}
			this.ingredientsParent.sizeDelta = new Vector2(this.ingredientsParent.sizeDelta.x, (float)(-(float)num));
			this.ingredientsDivider.anchoredPosition = new Vector2(this.ingredientsDivider.anchoredPosition.x, this.ingredientsParent.anchoredPosition.y + (float)num - 8f);
			this.resultsParent.DestroyChildren();
			this.results.Clear();
			num = 0;
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in subRecipe.GetResultsPreview())
			{
				ForgeIngredientRow forgeIngredientRow3 = UnityEngine.Object.Instantiate<ForgeIngredientRow>(this.ingredientPrefab, this.resultsParent);
				forgeIngredientRow3.InitializeItem(keyValuePair.Key, keyValuePair.Value, true);
				this.results.Add(forgeIngredientRow3);
				((RectTransform)forgeIngredientRow3.transform).anchoredPosition = new Vector2(0f, (float)num);
				num -= 32;
			}
			this.resultsParent.sizeDelta = new Vector2(this.resultsParent.sizeDelta.x, (float)(-(float)num));
			int num2 = subRecipe.CountAvailableForCrafting(Source.Mining.Forge.current);
			this.countSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.ManuallyUpdateSlider));
			this.countSlider.minValue = (float)((num2 == 1) ? 0 : 1);
			this.countSlider.maxValue = (float)num2;
			this.countSlider.value = ((this.previousSelectionCount > -1) ? Mathf.Clamp((float)this.previousSelectionCount, this.countSlider.minValue, this.countSlider.maxValue) : ((float)num2));
			this.countSlider.onValueChanged.AddListener(new UnityAction<float>(this.ManuallyUpdateSlider));
			this.amountText.SetTextWithoutNotify(num2.ToString());
			this.UpdateSlider();
			base.GetComponentInParent<ForgeUI>().UpdateSelectedRecipe(parentRecipe);
			this.notCraftableContent.gameObject.SetActive(this.countSlider.maxValue < 1f);
			this.craftableContent.gameObject.SetActive(this.countSlider.maxValue >= 1f);
			return true;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00097394 File Offset: 0x00095594
		private void SetupButtons()
		{
			RarityButton[] componentsInChildren = this.subRecipeHolder.GetComponentsInChildren<RarityButton>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
			Rarity[] array = (Rarity[])Enum.GetValues(typeof(Rarity));
			for (int j = 0; j < array.Length; j++)
			{
				Rarity rarity = array[j];
				RarityButton rarityButton = UnityEngine.Object.Instantiate<RarityButton>(this.rarityButtonPrefab, this.subRecipeHolder);
				bool flag = this.unlockedRecipes.FirstOrDefault((CraftingRecipe u) => u.craftingRarity == rarity) != null;
				bool active = this.subRecipe.craftingRarity == rarity;
				rarityButton.SetButton(rarity, !flag, active, this);
			}
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x00097457 File Offset: 0x00095657
		public void ManuallyUpdateSlider(float value)
		{
			this.previousSelectionCount = (int)this.countSlider.value;
			this.UpdateSlider();
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x00097474 File Offset: 0x00095674
		public void UpdateSlider()
		{
			this.amountText.SetTextWithoutNotify(Mathf.RoundToInt(this.countSlider.value).ToString());
			this.costText.text = Translation.Translate("@SSRefineryTime", new object[]
			{
				FormatString.FormatTime(this.subRecipe.craftingTime / this.forge.craftingSpeed * this.countSlider.value)
			}) + "\n" + Translation.Translate("@SSRefineryCost", new object[]
			{
				(float)this.subRecipe.craftingCost * this.countSlider.value
			});
			this.UpdateIngredients();
			this.UpdateResults();
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x00097530 File Offset: 0x00095730
		public void UpdateTextValue()
		{
			string text = this.amountText.text;
			if (string.IsNullOrEmpty(text) || text == "-")
			{
				this.countSlider.SetValueWithoutNotify(this.countSlider.minValue);
				return;
			}
			this.previousSelectionCount = int.Parse(text);
			this.countSlider.SetValueWithoutNotify(Mathf.Min(this.countSlider.maxValue, float.Parse(text)));
			this.UpdateSlider();
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000975A8 File Offset: 0x000957A8
		private void UpdateIngredients()
		{
			foreach (ForgeIngredientRow forgeIngredientRow in this.ingredients)
			{
				forgeIngredientRow.UpdateAmount(this.countSlider.value);
			}
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x00097604 File Offset: 0x00095804
		private void UpdateResults()
		{
			foreach (ForgeIngredientRow forgeIngredientRow in this.results)
			{
				forgeIngredientRow.UpdateAmount(this.countSlider.value);
			}
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x00097660 File Offset: 0x00095860
		public void ButtonToggle()
		{
			if (this.countSlider.value < 1f)
			{
				return;
			}
			if (this.forge.TryStartJob(this.subRecipe, Mathf.RoundToInt(this.countSlider.value)))
			{
				this.SetSelectedRecipe(this.parentRecipe, this.subRecipe, this.unlockedRecipes);
				SpaceStationInterior.instance.UpdateJobs();
				return;
			}
			Debug.LogWarning("Failed to start refinery job!");
		}

		// Token: 0x04000F1A RID: 3866
		[SerializeField]
		private RectTransform subRecipeHolder;

		// Token: 0x04000F1B RID: 3867
		[SerializeField]
		private RarityButton rarityButtonPrefab;

		// Token: 0x04000F1C RID: 3868
		private List<RarityButton> subRecipeButtons = new List<RarityButton>();

		// Token: 0x04000F1D RID: 3869
		[SerializeField]
		private Image recipeIcon;

		// Token: 0x04000F1E RID: 3870
		[SerializeField]
		private RectTransform ingredientsLabel;

		// Token: 0x04000F1F RID: 3871
		[SerializeField]
		private RectTransform ingredientsParent;

		// Token: 0x04000F20 RID: 3872
		[SerializeField]
		private ForgeIngredientRow ingredientPrefab;

		// Token: 0x04000F21 RID: 3873
		[SerializeField]
		private RectTransform ingredientsDivider;

		// Token: 0x04000F22 RID: 3874
		[SerializeField]
		private RectTransform resultsParent;

		// Token: 0x04000F23 RID: 3875
		[SerializeField]
		private TMP_InputField amountText;

		// Token: 0x04000F24 RID: 3876
		[SerializeField]
		private Slider countSlider;

		// Token: 0x04000F25 RID: 3877
		[SerializeField]
		private TMP_Text costText;

		// Token: 0x04000F26 RID: 3878
		[SerializeField]
		private RectTransform craftableContent;

		// Token: 0x04000F27 RID: 3879
		[SerializeField]
		private RectTransform notCraftableContent;

		// Token: 0x04000F2B RID: 3883
		private List<ForgeIngredientRow> ingredients = new List<ForgeIngredientRow>();

		// Token: 0x04000F2C RID: 3884
		private List<ForgeIngredientRow> results = new List<ForgeIngredientRow>();

		// Token: 0x04000F2D RID: 3885
		private int previousSelectionCount = -1;
	}
}
