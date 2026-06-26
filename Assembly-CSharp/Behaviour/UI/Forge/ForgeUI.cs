using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Behaviour.Crafting;
using Behaviour.UI.Refinery;
using Source.Item;
using Source.Mining;
using Source.Player;
using Source.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviour.UI.Forge
{
	// Token: 0x02000294 RID: 660
	public class ForgeUI : MonoBehaviour
	{
		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001836 RID: 6198 RVA: 0x00097701 File Offset: 0x00095901
		// (set) Token: 0x06001837 RID: 6199 RVA: 0x00097708 File Offset: 0x00095908
		public static ForgeUI current { get; private set; }

		// Token: 0x06001838 RID: 6200 RVA: 0x00097710 File Offset: 0x00095910
		private void Awake()
		{
			ForgeUI.current = this;
			this.materialsParent.DestroyChildren();
			Source.Mining.Forge current = Source.Mining.Forge.current;
			int length = Enum.GetValues(typeof(RefinedMaterial)).Length;
			for (int i = 0; i < length; i++)
			{
				RefineryMaterialBadge refineryMaterialBadge = UnityEngine.Object.Instantiate<RefineryMaterialBadge>(this.materialPrefab, this.materialsParent);
				refineryMaterialBadge.SetMaterial((RefinedMaterial)i);
				((RectTransform)refineryMaterialBadge.transform).anchoredPosition = new Vector2(4f, (float)(-4 - i * 34));
			}
			this.recipeParent.DestroyChildren();
			Dictionary<CraftingRecipe, List<CraftingRecipe>> dictionary = new Dictionary<CraftingRecipe, List<CraftingRecipe>>();
			foreach (CraftingRecipe craftingRecipe in current.recipes)
			{
				List<CraftingRecipe> list;
				dictionary.TryGetValue(craftingRecipe.parentRecipe, out list);
				if (list == null)
				{
					list = new List<CraftingRecipe>();
					dictionary[craftingRecipe.parentRecipe] = list;
				}
				list.Add(craftingRecipe);
			}
			List<CraftingRecipe> list2 = new List<CraftingRecipe>(dictionary.Keys);
			list2.Sort(new Comparison<CraftingRecipe>(this.SortRecipes));
			ItemCategory? itemCategory = null;
			bool flag = ForgeUI.preselectRecipe != null;
			foreach (CraftingRecipe craftingRecipe2 in list2)
			{
				if (itemCategory == null)
				{
					goto IL_15E;
				}
				ItemCategory? itemCategory2 = itemCategory;
				ItemCategory category = craftingRecipe2.category;
				if (!(itemCategory2.GetValueOrDefault() == category & itemCategory2 != null))
				{
					goto IL_15E;
				}
				IL_1ED:
				ForgeRecipeRow forgeRecipeRow = UnityEngine.Object.Instantiate<ForgeRecipeRow>(this.recipeRowPrefab, this.recipeParent);
				forgeRecipeRow.SetRecipe(craftingRecipe2, dictionary[craftingRecipe2]);
				ForgeUI.categorizedRecipes[itemCategory.Value].Add(forgeRecipeRow);
				if (!flag)
				{
					this.SelectRecipe(craftingRecipe2, dictionary[craftingRecipe2], null);
					flag = true;
					continue;
				}
				continue;
				IL_15E:
				itemCategory = new ItemCategory?(craftingRecipe2.category);
				ForgeUI.categorizedRecipes[craftingRecipe2.category] = new List<ForgeRecipeRow>();
				Button button = UnityEngine.Object.Instantiate<Button>(this.categoryHeaderPrefab, this.recipeParent);
				button.GetComponentInChildren<TMP_Text>().TL("@ItemCategory" + itemCategory.ToString(), Array.Empty<object>());
				ItemCategory listenCategory = itemCategory.Value;
				button.onClick.AddListener(delegate()
				{
					this.ToggleCategory(listenCategory);
				});
				goto IL_1ED;
			}
			if (ForgeUI.preselectRecipe)
			{
				ValueTuple<CraftingRecipe, List<CraftingRecipe>> valueTuple = this.FindRecipeParent(ForgeUI.preselectRecipe, dictionary);
				this.SelectRecipe(valueTuple.Item1, valueTuple.Item2, ForgeUI.preselectRecipe);
			}
			this.recipeParent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 1f;
			this.cargoToggle.SetIsOnWithoutNotify(GamePlayer.current.forgeDepositInCargo);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00097A0C File Offset: 0x00095C0C
		private ValueTuple<CraftingRecipe, List<CraftingRecipe>> FindRecipeParent(CraftingRecipe recipe, Dictionary<CraftingRecipe, List<CraftingRecipe>> available)
		{
			if (available.ContainsKey(recipe))
			{
				return new ValueTuple<CraftingRecipe, List<CraftingRecipe>>(recipe, available[recipe]);
			}
			foreach (KeyValuePair<CraftingRecipe, List<CraftingRecipe>> keyValuePair in available)
			{
				if (keyValuePair.Value.Contains(recipe))
				{
					return new ValueTuple<CraftingRecipe, List<CraftingRecipe>>(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return new ValueTuple<CraftingRecipe, List<CraftingRecipe>>(recipe, new List<CraftingRecipe>
			{
				recipe
			});
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00097AA4 File Offset: 0x00095CA4
		private void ToggleCategory(ItemCategory category)
		{
			foreach (ForgeRecipeRow forgeRecipeRow in ForgeUI.categorizedRecipes[category])
			{
				forgeRecipeRow.gameObject.SetActive(!forgeRecipeRow.gameObject.activeSelf);
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00097B10 File Offset: 0x00095D10
		private int SortRecipes(CraftingRecipe a, CraftingRecipe b)
		{
			int num = ForgeUI.categoryOrder.IndexOf(a.category);
			int num2 = ForgeUI.categoryOrder.IndexOf(b.category);
			if (num < 0 && num2 >= 0)
			{
				return -1;
			}
			if (num2 < 0 && num >= 0)
			{
				return 1;
			}
			if (num != num2)
			{
				return num - num2;
			}
			if (a.category != b.category)
			{
				return a.category - b.category;
			}
			return Translation.Translate(a.displayName, Array.Empty<object>()).CompareTo(Translation.Translate(b.displayName, Array.Empty<object>()));
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x00097B9C File Offset: 0x00095D9C
		public void SelectRecipe(CraftingRecipe recipe, List<CraftingRecipe> unlockedRecipes, CraftingRecipe subRecipe = null)
		{
			unlockedRecipes = (from r in unlockedRecipes
			orderby r.craftingRarity
			select r).ToList<CraftingRecipe>();
			this.tabContents.SetSelectedRecipe(recipe, subRecipe ?? unlockedRecipes[0], unlockedRecipes);
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x00097BF0 File Offset: 0x00095DF0
		public void UpdateSelectedRecipe(CraftingRecipe recipe)
		{
			foreach (ForgeRecipeRow forgeRecipeRow in this.recipeParent.GetComponentsInChildren<ForgeRecipeRow>())
			{
				forgeRecipeRow.SetSelected(forgeRecipeRow.recipe == recipe);
			}
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x00097C2B File Offset: 0x00095E2B
		public void UpdateContent()
		{
			this.tabContents.SetSelectedRecipe(this.tabContents.parentRecipe, this.tabContents.subRecipe, this.tabContents.unlockedRecipes);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x00097C5A File Offset: 0x00095E5A
		public void OnCargoToggle()
		{
			GamePlayer.current.forgeDepositInCargo = this.cargoToggle.isOn;
		}

		// Token: 0x04000F2E RID: 3886
		private static List<ItemCategory> categoryOrder = new List<ItemCategory>
		{
			ItemCategory.RefinedProduct,
			ItemCategory.Turret,
			ItemCategory.Module,
			ItemCategory.Booster,
			ItemCategory.Ammo
		};

		// Token: 0x04000F30 RID: 3888
		[SerializeField]
		private RectTransform materialsParent;

		// Token: 0x04000F31 RID: 3889
		[SerializeField]
		private RefineryMaterialBadge materialPrefab;

		// Token: 0x04000F32 RID: 3890
		[SerializeField]
		private RectTransform recipeParent;

		// Token: 0x04000F33 RID: 3891
		[SerializeField]
		private ForgeRecipeRow recipeRowPrefab;

		// Token: 0x04000F34 RID: 3892
		[SerializeField]
		private Button categoryHeaderPrefab;

		// Token: 0x04000F35 RID: 3893
		[SerializeField]
		private Toggle cargoToggle;

		// Token: 0x04000F36 RID: 3894
		[SerializeField]
		private ForgeTabContents tabContents;

		// Token: 0x04000F37 RID: 3895
		private static Dictionary<ItemCategory, List<ForgeRecipeRow>> categorizedRecipes = new Dictionary<ItemCategory, List<ForgeRecipeRow>>();

		// Token: 0x04000F38 RID: 3896
		public static CraftingRecipe preselectRecipe;
	}
}
