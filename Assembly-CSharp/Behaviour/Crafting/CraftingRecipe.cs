using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.Crew;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Source.Item;
using Source.Mining;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Crafting
{
	// Token: 0x020003AA RID: 938
	public class CraftingRecipe : MonoBehaviour
	{
		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x000CB6FA File Offset: 0x000C98FA
		public static IEnumerable<CraftingRecipe> all
		{
			get
			{
				return CraftingRecipe.allRecipes.Values;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x000CB706 File Offset: 0x000C9906
		// (set) Token: 0x06002414 RID: 9236 RVA: 0x000CB70E File Offset: 0x000C990E
		public string identifier { get; private set; }

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x000CB717 File Offset: 0x000C9917
		// (set) Token: 0x06002416 RID: 9238 RVA: 0x000CB71F File Offset: 0x000C991F
		public float minLevel { get; private set; }

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x000CB728 File Offset: 0x000C9928
		// (set) Token: 0x06002418 RID: 9240 RVA: 0x000CB730 File Offset: 0x000C9930
		public float craftingTime { get; private set; }

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000CB739 File Offset: 0x000C9939
		public IEnumerable<CraftingRecipe.CraftingRecipeItemRow> RawResults
		{
			get
			{
				return this.results;
			}
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000CB741 File Offset: 0x000C9941
		// (set) Token: 0x0600241B RID: 9243 RVA: 0x000CB749 File Offset: 0x000C9949
		public Rarity craftingRarity { get; private set; }

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000CB752 File Offset: 0x000C9952
		// (set) Token: 0x0600241D RID: 9245 RVA: 0x000CB75A File Offset: 0x000C995A
		public ItemCategory category { get; private set; }

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000CB763 File Offset: 0x000C9963
		// (set) Token: 0x0600241F RID: 9247 RVA: 0x000CB76B File Offset: 0x000C996B
		public CraftingRecipe parentRecipe { get; private set; }

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000CB774 File Offset: 0x000C9974
		public string displayName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.customName))
				{
					return this.customName;
				}
				return this.dynamicName;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x000CB790 File Offset: 0x000C9990
		public string recipeCategoryName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.customRecipeCategoryName))
				{
					return this.customRecipeCategoryName;
				}
				return this.dynamicName;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x000CB7AC File Offset: 0x000C99AC
		public Sprite icon
		{
			get
			{
				if (!this.customIcon)
				{
					return this.GetResultPreview(0, this.results[0].item).icon;
				}
				return this.customIcon;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x000CB7E0 File Offset: 0x000C99E0
		public int craftingCost
		{
			get
			{
				if (this.customCost > 0)
				{
					return Mathf.FloorToInt((float)this.customCost * Mathf.Max(0f, 1f - SkilltreeNode.industrialCraftCreditReduction.currentIncrease));
				}
				if (this.dynamicCost < 0)
				{
					this.UpdateDynamicCost();
				}
				return Mathf.FloorToInt((float)this.dynamicCost * Mathf.Max(0f, 1f - SkilltreeNode.industrialCraftCreditReduction.currentIncrease));
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000CB854 File Offset: 0x000C9A54
		public float value
		{
			get
			{
				if (this.dynamicValue < 0f || this.levelingItem)
				{
					this.UpdateDynamicCost();
				}
				return this.dynamicValue;
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000CB878 File Offset: 0x000C9A78
		public void InitializeRecipe(bool isSubRecipe = false)
		{
			this.resultPreviews = new InventoryItemType[this.results.Count];
			if (!isSubRecipe)
			{
				this.parentRecipe = this;
				base.GetComponentsInChildren<CraftingRecipe>(this.subRecipes);
				foreach (CraftingRecipe craftingRecipe in this.subRecipes)
				{
					if (craftingRecipe != this)
					{
						craftingRecipe.identifier = this.identifier + "_SubRecipe_" + craftingRecipe.gameObject.name;
						craftingRecipe.parentRecipe = this;
						craftingRecipe.minLevel = this.minLevel;
						craftingRecipe.results = this.results;
					}
				}
			}
			if (this.results.Count > 0)
			{
				InventoryItemType component = this.results[0].item.GetComponent<InventoryItemType>();
				EquipmentBuilder component2 = this.results[0].item.GetComponent<EquipmentBuilder>();
				ItemBuilder component3 = this.results[0].item.GetComponent<ItemBuilder>();
				if (component)
				{
					this.category = component.itemCategory;
					this.dynamicName = component.displayName;
					this.dynamicIcon = component.icon;
				}
				else if (component2)
				{
					AbstractEquipment component4 = component2.prefab.GetComponent<AbstractEquipment>();
					this.category = component2.prefab.itemCategory;
					this.dynamicName = component4.typeDisplayName + " [" + Translation.Translate(component4.size.ToString(), Array.Empty<object>()) + "]";
					this.dynamicIcon = component2.prefab.icon;
					this.levelingItem = true;
				}
				else if (component3)
				{
					this.category = component3.prefab.itemCategory;
					this.dynamicName = component3.prefab.displayName;
					this.dynamicIcon = component3.prefab.icon;
				}
			}
			foreach (CraftingRecipe craftingRecipe2 in this.subRecipes)
			{
				if (craftingRecipe2 != this)
				{
					craftingRecipe2.InitializeRecipe(true);
				}
			}
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000CBACC File Offset: 0x000C9CCC
		public float GetCostScale(int level)
		{
			if (this.levelingItem)
			{
				return GameMath.CostMultiplier((level > 0) ? level : this.GetAdjustedOutputLevel());
			}
			return 1f;
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000CBAF0 File Offset: 0x000C9CF0
		private void UpdateDynamicCost()
		{
			float num = 0f;
			foreach (ValueTuple<RefinedMaterial, float> valueTuple in this.GetIngredientMaterials(0))
			{
				num += valueTuple.Item1.GetValue() * valueTuple.Item2;
			}
			foreach (ValueTuple<InventoryItemType, int> valueTuple2 in this.GetIngredientItems(0))
			{
				if (valueTuple2.Item1 == null)
				{
					Debug.LogWarning("itemMaterials item is not set: " + base.name);
				}
				num += (float)(valueTuple2.Item1.GetComponent<InventoryItemType>().cost * valueTuple2.Item2);
			}
			this.dynamicValue = num * this.craftingValueMultiplier;
			this.dynamicCost = Mathf.RoundToInt(num / 10f);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000CBBEC File Offset: 0x000C9DEC
		public IEnumerable<ValueTuple<RefinedMaterial, float>> GetIngredientMaterials(int level = 0)
		{
			float scale = this.GetCostScale(level);
			foreach (CraftingRecipe.CraftingRecipeMaterialRow craftingRecipeMaterialRow in this.materials)
			{
				yield return new ValueTuple<RefinedMaterial, float>(craftingRecipeMaterialRow.material, craftingRecipeMaterialRow.amount * scale);
			}
			List<CraftingRecipe.CraftingRecipeMaterialRow>.Enumerator enumerator = default(List<CraftingRecipe.CraftingRecipeMaterialRow>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000CBC03 File Offset: 0x000C9E03
		public IEnumerable<ValueTuple<InventoryItemType, int>> GetIngredientItems(int level = 0)
		{
			float scale = this.GetCostScale(level);
			foreach (CraftingRecipe.CraftingRecipeItemRow craftingRecipeItemRow in this.itemMaterials)
			{
				yield return new ValueTuple<InventoryItemType, int>(craftingRecipeItemRow.item.GetComponent<InventoryItemType>(), Mathf.RoundToInt((float)craftingRecipeItemRow.count * scale));
			}
			List<CraftingRecipe.CraftingRecipeItemRow>.Enumerator enumerator = default(List<CraftingRecipe.CraftingRecipeItemRow>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000CBC1A File Offset: 0x000C9E1A
		public IEnumerable<KeyValuePair<InventoryItemType, int>> GetResultsPreview()
		{
			if (this.results.Count > 0)
			{
				int num;
				for (int i = 0; i < this.results.Count; i = num + 1)
				{
					InventoryItemType resultPreview = this.GetResultPreview(i, this.results[i].item);
					if (resultPreview)
					{
						if (i == this.results.Count - 1)
						{
							this.previewLevel = resultPreview.itemLevel;
						}
						yield return KeyValuePair.Create<InventoryItemType, int>(resultPreview, this.results[i].count);
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000CBC2C File Offset: 0x000C9E2C
		private InventoryItemType GetResultPreview(int i, GameObject item)
		{
			InventoryItemType result;
			if (item.TryGetComponent<InventoryItemType>(out result) || GamePlayer.current == null)
			{
				return result;
			}
			int adjustedOutputLevel = this.GetAdjustedOutputLevel();
			if (this.resultPreviews[i] && this.previewLevel == adjustedOutputLevel)
			{
				return this.resultPreviews[i];
			}
			InventoryItemType inventoryItemType = null;
			EquipmentBuilder equipmentBuilder;
			ItemBuilder itemBuilder;
			if (item.TryGetComponent<EquipmentBuilder>(out equipmentBuilder))
			{
				inventoryItemType = equipmentBuilder.CreateItemType(this.craftingRarity, adjustedOutputLevel, true, null, true, false);
			}
			else if (item.TryGetComponent<ItemBuilder>(out itemBuilder))
			{
				inventoryItemType = itemBuilder.CreateItemType(this.craftingRarity, adjustedOutputLevel, true, null, true);
			}
			this.resultPreviews[i] = inventoryItemType;
			return inventoryItemType;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000CBCBC File Offset: 0x000C9EBC
		public InventoryItemType GetResultForTooltip()
		{
			using (IEnumerator<KeyValuePair<InventoryItemType, int>> enumerator = this.GetResultsPreview().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<InventoryItemType, int> keyValuePair = enumerator.Current;
					return keyValuePair.Key;
				}
			}
			return null;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000CBD10 File Offset: 0x000C9F10
		public IEnumerable<KeyValuePair<InventoryItemType, int>> GetResultsForCraft(int outputLevel)
		{
			if (outputLevel <= 0)
			{
				outputLevel = this.GetAdjustedOutputLevel();
			}
			if (this.results.Count > 0)
			{
				foreach (CraftingRecipe.CraftingRecipeItemRow craftingRecipeItemRow in this.results)
				{
					InventoryItemType component = craftingRecipeItemRow.item.GetComponent<InventoryItemType>();
					EquipmentBuilder component2 = craftingRecipeItemRow.item.GetComponent<EquipmentBuilder>();
					ItemBuilder component3 = craftingRecipeItemRow.item.GetComponent<ItemBuilder>();
					InventoryItemType inventoryItemType = null;
					if (component)
					{
						inventoryItemType = component;
					}
					else if (component2)
					{
						inventoryItemType = component2.CreateItemType(this.craftingRarity, outputLevel, true, null, true, true);
					}
					else if (component3)
					{
						inventoryItemType = component3.CreateItemType(this.craftingRarity, outputLevel, true, null, true);
					}
					if (inventoryItemType)
					{
						yield return KeyValuePair.Create<InventoryItemType, int>(inventoryItemType, craftingRecipeItemRow.count);
					}
					else
					{
						Debug.LogWarning("No InventoryItemType, EquipmentBuilder, ItemBuilder in recipe: " + base.name);
						UnityEngine.Object.Instantiate<GameObject>(craftingRecipeItemRow.item);
					}
				}
				List<CraftingRecipe.CraftingRecipeItemRow>.Enumerator enumerator = default(List<CraftingRecipe.CraftingRecipeItemRow>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000CBD28 File Offset: 0x000C9F28
		public int CountAvailableForCrafting(Forge location)
		{
			int num = int.MaxValue;
			foreach (ValueTuple<RefinedMaterial, float> valueTuple in this.GetIngredientMaterials(0))
			{
				float num2 = GamePlayer.current.CountRefinedMaterial(valueTuple.Item1);
				num = Mathf.Min(num, Mathf.FloorToInt(num2 / valueTuple.Item2));
			}
			foreach (ValueTuple<InventoryItemType, int> valueTuple2 in this.GetIngredientItems(0))
			{
				int num3 = location.spaceStation.CountAvailableItems(valueTuple2.Item1);
				num = Mathf.Min(num, num3 / valueTuple2.Item2);
			}
			return num;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x000CBDFC File Offset: 0x000C9FFC
		public int GetAdjustedOutputLevel()
		{
			int num = (int)SkilltreeNode.industrialT3ExtraCraftingLevel.currentIncrease;
			num += (int)SkilltreeNode.industrialExtraCraftingLevels2.currentIncrease;
			return GamePlayer.current.commander.level + num;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x000CBE34 File Offset: 0x000CA034
		public bool ConsumeIngredientsForCrafting(Forge location, int count)
		{
			if (this.CountAvailableForCrafting(location) < count)
			{
				return false;
			}
			foreach (ValueTuple<RefinedMaterial, float> valueTuple in this.GetIngredientMaterials(0))
			{
				location.spaceStation.refinery.ConsumeRefinedMaterial(valueTuple.Item1, valueTuple.Item2 * (float)count);
			}
			foreach (ValueTuple<InventoryItemType, int> valueTuple2 in this.GetIngredientItems(0))
			{
				location.spaceStation.ConsumeAvailableItems(valueTuple2.Item1, valueTuple2.Item2 * count);
			}
			return true;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000CBEF8 File Offset: 0x000CA0F8
		public int GetResultCount(InventoryItemType item)
		{
			foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.GetResultsPreview())
			{
				if (keyValuePair.Key == item)
				{
					return keyValuePair.Value;
				}
			}
			return 0;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000CBF5C File Offset: 0x000CA15C
		public CraftingRecipe GetSubRecipe(string id)
		{
			foreach (CraftingRecipe craftingRecipe in this.subRecipes)
			{
				if (craftingRecipe.identifier == id)
				{
					return craftingRecipe;
				}
			}
			return null;
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x000CBFC0 File Offset: 0x000CA1C0
		public static IEnumerable<CraftingRecipe> GetAvailable()
		{
			List<CraftingRecipe> list = new List<CraftingRecipe>();
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.allRecipes.Values)
			{
				if (craftingRecipe.unlockedFromStart)
				{
					list.Add(craftingRecipe);
				}
			}
			if (SkilltreeNode.industrialBasicRecipes.isActive)
			{
				foreach (string id in CraftingRecipe.basicRecipes)
				{
					list.Add(id);
				}
			}
			foreach (string name in GamePlayer.current.blueprints)
			{
				list.Add(CraftingRecipe.Get(name));
			}
			foreach (Mission mission in GamePlayer.current.missions)
			{
				if (mission.name == "Craft The Core")
				{
					list.Add(CraftingRecipe.Get("AI Core"));
					break;
				}
				if (mission.storyId == "UmbralMission18")
				{
					list.Add(CraftingRecipe.Get("Isolation Chamber"));
					break;
				}
			}
			return list;
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x000CC138 File Offset: 0x000CA338
		public static void LoadAll()
		{
			CraftingRecipe.allRecipes.Clear();
			CraftingRecipe[] array = Resources.LoadAll<CraftingRecipe>("Recipes");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					array[i].identifier = array[i].gameObject.name;
					CraftingRecipe.allRecipes[array[i].identifier] = array[i];
				}
			}
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.all)
			{
				craftingRecipe.InitializeRecipe(false);
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000CC1E4 File Offset: 0x000CA3E4
		public static CraftingRecipe GetSourceRecipe(InventoryItemType result)
		{
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.allRecipes.Values)
			{
				using (List<CraftingRecipe.CraftingRecipeItemRow>.Enumerator enumerator2 = craftingRecipe.results.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.item == result.gameObject)
						{
							return craftingRecipe;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x000CC288 File Offset: 0x000CA488
		public static IEnumerable<CraftingRecipe> GetBlueprintableRecipes()
		{
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.all)
			{
				foreach (CraftingRecipe craftingRecipe2 in craftingRecipe.subRecipes)
				{
					if (!craftingRecipe2.unlockedFromStart && craftingRecipe2.blueprintAvailable && !GamePlayer.current.blueprints.Contains(craftingRecipe2.identifier))
					{
						yield return craftingRecipe2;
					}
				}
				List<CraftingRecipe>.Enumerator enumerator2 = default(List<CraftingRecipe>.Enumerator);
			}
			IEnumerator<CraftingRecipe> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000CC294 File Offset: 0x000CA494
		public static CraftingRecipe GetBlueprintForItem(string equipBuilderIdentifier, Rarity rarity)
		{
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.GetBlueprintableRecipes())
			{
				foreach (CraftingRecipe.CraftingRecipeItemRow craftingRecipeItemRow in craftingRecipe.results)
				{
					EquipmentBuilder component = craftingRecipeItemRow.item.GetComponent<EquipmentBuilder>();
					if (component != null && component.identifier == equipBuilderIdentifier && craftingRecipe.craftingRarity == rarity)
					{
						return craftingRecipe;
					}
				}
			}
			return null;
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000CC348 File Offset: 0x000CA548
		public static implicit operator string(CraftingRecipe cr)
		{
			return cr.identifier;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000CC350 File Offset: 0x000CA550
		public static implicit operator CraftingRecipe(string id)
		{
			return CraftingRecipe.Get(id);
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000CC358 File Offset: 0x000CA558
		public static CraftingRecipe Get(string name)
		{
			if (name.Contains("_SubRecipe_"))
			{
				return CraftingRecipe.Get(name.Split("_SubRecipe_", 2, StringSplitOptions.None)[0]).GetSubRecipe(name);
			}
			return CraftingRecipe.allRecipes[name];
		}

		// Token: 0x0400157E RID: 5502
		private static string[] basicRecipes = new string[]
		{
			"Small Mining Laser",
			"Small Autocannon",
			"Small Salvage Turret",
			"Small Reactor",
			"RecipeMediumReactor",
			"Small Hull",
			"RecipeSmallTractorBeam",
			"Autocannon Ammo"
		};

		// Token: 0x0400157F RID: 5503
		private static Dictionary<string, CraftingRecipe> allRecipes = new Dictionary<string, CraftingRecipe>();

		// Token: 0x04001582 RID: 5506
		[SerializeField]
		private string customName;

		// Token: 0x04001583 RID: 5507
		[SerializeField]
		private string customRecipeCategoryName;

		// Token: 0x04001584 RID: 5508
		[SerializeField]
		private Sprite customIcon;

		// Token: 0x04001585 RID: 5509
		[SerializeField]
		private int customCost;

		// Token: 0x04001587 RID: 5511
		[SerializeField]
		public bool unlockedFromStart;

		// Token: 0x04001588 RID: 5512
		[SerializeField]
		public bool blueprintAvailable = true;

		// Token: 0x04001589 RID: 5513
		[SerializeField]
		public float craftingValueMultiplier = 1.2f;

		// Token: 0x0400158A RID: 5514
		[SerializeField]
		private List<CraftingRecipe.CraftingRecipeMaterialRow> materials;

		// Token: 0x0400158B RID: 5515
		[SerializeField]
		private List<CraftingRecipe.CraftingRecipeItemRow> itemMaterials;

		// Token: 0x0400158C RID: 5516
		[SerializeField]
		private List<CraftingRecipe.CraftingRecipeItemRow> results;

		// Token: 0x0400158E RID: 5518
		private bool levelingItem;

		// Token: 0x0400158F RID: 5519
		private string dynamicName;

		// Token: 0x04001590 RID: 5520
		private Sprite dynamicIcon;

		// Token: 0x04001593 RID: 5523
		public readonly List<CraftingRecipe> subRecipes = new List<CraftingRecipe>();

		// Token: 0x04001594 RID: 5524
		private float dynamicValue = -1f;

		// Token: 0x04001595 RID: 5525
		private int dynamicCost = -1;

		// Token: 0x04001596 RID: 5526
		private int previewLevel = -1;

		// Token: 0x04001597 RID: 5527
		private InventoryItemType[] resultPreviews;

		// Token: 0x020005EE RID: 1518
		[Serializable]
		public class CraftingRecipeMaterialRow
		{
			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x06002F61 RID: 12129 RVA: 0x000F971F File Offset: 0x000F791F
			// (set) Token: 0x06002F62 RID: 12130 RVA: 0x000F9727 File Offset: 0x000F7927
			public RefinedMaterial material { get; private set; }

			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x06002F63 RID: 12131 RVA: 0x000F9730 File Offset: 0x000F7930
			// (set) Token: 0x06002F64 RID: 12132 RVA: 0x000F9738 File Offset: 0x000F7938
			public float amount { get; private set; }
		}

		// Token: 0x020005EF RID: 1519
		[Serializable]
		public class CraftingRecipeItemRow
		{
			// Token: 0x17000787 RID: 1927
			// (get) Token: 0x06002F66 RID: 12134 RVA: 0x000F9749 File Offset: 0x000F7949
			// (set) Token: 0x06002F67 RID: 12135 RVA: 0x000F9751 File Offset: 0x000F7951
			public GameObject item { get; private set; }

			// Token: 0x17000788 RID: 1928
			// (get) Token: 0x06002F68 RID: 12136 RVA: 0x000F975A File Offset: 0x000F795A
			// (set) Token: 0x06002F69 RID: 12137 RVA: 0x000F9762 File Offset: 0x000F7962
			public int count { get; private set; } = 1;
		}
	}
}
