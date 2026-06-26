using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Behaviour.Crafting;
using Behaviour.Crew;
using Behaviour.Equipment.Aspect;
using Behaviour.Item.Usable;
using Behaviour.Mining;
using Behaviour.Util;
using LightJson;
using Source.Crew;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Mining;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Builder
{
	// Token: 0x02000322 RID: 802
	public class ItemBuilder : MonoBehaviour
	{
		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x000B251F File Offset: 0x000B071F
		// (set) Token: 0x06001E0F RID: 7695 RVA: 0x000B2527 File Offset: 0x000B0727
		public string identifier { get; private set; }

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x000B2530 File Offset: 0x000B0730
		public InventoryItemType prefab
		{
			get
			{
				return this.baseItem;
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x000B2538 File Offset: 0x000B0738
		public InventoryItemType CreateItemType(Rarity rarity, int level, bool exactLevel = false, string seed = null, bool ignoreLevelCap = false)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			UsableItem component = inventoryItemType.GetComponent<UsableItem>();
			JumpgatePassItem component2 = component.GetComponent<JumpgatePassItem>();
			MiningClaimItem component3 = component.GetComponent<MiningClaimItem>();
			DefensiveTurretItem component4 = component.GetComponent<DefensiveTurretItem>();
			ExplosiveMineItem component5 = component.GetComponent<ExplosiveMineItem>();
			WarpFuelItem component6 = component.GetComponent<WarpFuelItem>();
			if (!component2 && !component3)
			{
				if (component4)
				{
					inventoryItemType = this.CreateDefensiveTurret(ignoreLevelCap ? level : GamePlayer.current.commander.level);
				}
				else if (component5)
				{
					inventoryItemType = this.CreateExplosiveMineItem(ignoreLevelCap ? level : GamePlayer.current.commander.level);
				}
				else if (component6)
				{
					inventoryItemType = this.CreateWarpFuel(rarity);
				}
			}
			return inventoryItemType;
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x000B2600 File Offset: 0x000B0800
		public InventoryItemType CreateJumpgatePass(JumpGate jumpgate, string seed = null)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			inventoryItemType.SetRarity(Rarity.HighGrade);
			inventoryItemType.GetComponent<JumpgatePassItem>().SetJumpgate(jumpgate.system.guid, jumpgate.guid);
			SystemMapData system = GalaxyMapData.current.GetSystem(jumpgate.targetSystemGuid);
			string text;
			string name;
			string name2;
			if (jumpgate.sectorJumpgate)
			{
				text = "@ItemNameSectorPass";
				name = jumpgate.system.sector.name;
				name2 = system.sector.name;
			}
			else
			{
				text = "@ItemNameJumpgatePass";
				name = jumpgate.system.name;
				name2 = system.name;
			}
			inventoryItemType.SetDisplayName(Translation.Translate(text, new object[]
			{
				name,
				name2
			}));
			inventoryItemType.SetDescription(Translation.Translate("@JumpgatePassDescription", new object[]
			{
				jumpgate.system.name
			}));
			inventoryItemType.SetBaseValue(GameMath.GetCreditsValue((float)(jumpgate.sectorJumpgate ? 175 : 75), system.level));
			inventoryItemType.InitializeItem(null);
			inventoryItemType.SetItemLevel(system.level);
			return inventoryItemType;
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x000B271C File Offset: 0x000B091C
		public InventoryItemType CreateMiningClaim(SystemMapData system, AsteroidFieldData asteroidFieldData = null)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			MiningClaimItem component = inventoryItemType.GetComponent<MiningClaimItem>();
			string text = "Mining Claim A-" + SeededRandom.Global.RandomRange(100, 999).ToString();
			inventoryItemType.SetDisplayName(text);
			component.SetClaim(system.guid, system.level, text, asteroidFieldData);
			inventoryItemType.SetRarity(Rarity.Enhanced);
			AsteroidFieldData asteroidFieldData2 = component.asteroidFieldData;
			int num = 20;
			float num2 = asteroidFieldData2.wealth * 2f + (float)num;
			num2 *= (float)asteroidFieldData2.amount / 5f;
			inventoryItemType.SetBaseValue(GameMath.GetCreditsValue(num2, system.level));
			inventoryItemType.InitializeItem(null);
			inventoryItemType.SetItemLevel(system.level);
			return inventoryItemType;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000B27E8 File Offset: 0x000B09E8
		public InventoryItemType CreateMaterialMiningClaim(SystemMapData system, RefinedMaterial? rm = null)
		{
			SeededRandom global = SeededRandom.Global;
			RefinedMaterial refinedMaterial2;
			if (rm == null)
			{
				Dictionary<RefinedMaterial, float> dictionary = new Dictionary<RefinedMaterial, float>();
				foreach (RefinedMaterial refinedMaterial in (RefinedMaterial[])Enum.GetValues(typeof(RefinedMaterial)))
				{
					dictionary.Add(refinedMaterial, Mathf.Sqrt(refinedMaterial.GetValue()));
				}
				refinedMaterial2 = global.Choose<RefinedMaterial>(dictionary);
			}
			else
			{
				refinedMaterial2 = rm.Value;
			}
			List<OreItemData> list = new List<OreItemData>();
			foreach (OreItemData oreItemData in OreItemData.regularOres)
			{
				if (oreItemData.HasMaterial(refinedMaterial2))
				{
					list.Add(oreItemData);
				}
			}
			list.Sort((OreItemData a, OreItemData b) => Mathf.Abs(a.item.itemLevel - system.level) - Mathf.Abs(b.item.itemLevel - system.level));
			AsteroidFieldOreSet asteroidFieldOreSet = AsteroidFieldData.CreateOreSet(system.level, true);
			if (list.Count > 0)
			{
				asteroidFieldOreSet.commonOre = list[0];
			}
			AsteroidFieldOreSet asteroidFieldOreSet2 = AsteroidFieldData.CreateOreSet(system.level, false);
			if (list.Count > 1)
			{
				asteroidFieldOreSet2.commonOre = list[1];
			}
			AsteroidFieldData asteroidFieldData = new AsteroidFieldData(global.RandomRange(8, 17), 1f, global.RandomRange(0.6f, 1.2f), asteroidFieldOreSet, asteroidFieldOreSet2, -1f);
			InventoryItemType inventoryItemType = this.CreateMiningClaim(system, asteroidFieldData);
			inventoryItemType.SetDescription(Translation.Translate("@MaterialMiningClaimDescription", new object[]
			{
				refinedMaterial2.GetDisplayName()
			}) + "\n\n" + Translation.Translate("@MiningClaimDescription", Array.Empty<object>()));
			inventoryItemType.SetBaseValue(Mathf.RoundToInt((float)inventoryItemType.baseCost * Mathf.Sqrt(refinedMaterial2.GetValue() / 100f)));
			return inventoryItemType;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000B29CC File Offset: 0x000B0BCC
		public InventoryItemType CreateSalvageClaim(SystemMapData system, List<PersistableData> salvage = null)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			SalvageClaimItem component = inventoryItemType.GetComponent<SalvageClaimItem>();
			string text = "Salvage Claim Q-" + SeededRandom.Global.RandomRange(100, 999).ToString();
			inventoryItemType.SetDisplayName(text);
			component.SetClaim(system.guid, system.level, text, salvage);
			inventoryItemType.SetRarity(Rarity.Enhanced);
			inventoryItemType.SetBaseValue(GameMath.GetCreditsValue(80f, system.level));
			inventoryItemType.InitializeItem(null);
			inventoryItemType.SetItemLevel(system.level);
			return inventoryItemType;
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000B2A6C File Offset: 0x000B0C6C
		public InventoryItemType CreateDefensiveTurret(int level)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			DefensiveTurretItem component = inventoryItemType.GetComponent<DefensiveTurretItem>();
			component.SetTurret(this.baseItem.name);
			component.currentAmmo = component.maxAmmo;
			component.currentHealth = 1f;
			string displayName = this.baseItem.displayName;
			inventoryItemType.SetDisplayName(displayName);
			inventoryItemType.SetBaseValue(GameMath.GetCreditsValue((float)inventoryItemType.baseCost, level));
			inventoryItemType.InitializeItem(null);
			inventoryItemType.SetItemLevel(level);
			return inventoryItemType;
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000B2AF8 File Offset: 0x000B0CF8
		public InventoryItemType CreateExplosiveMineItem(int level)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			inventoryItemType.GetComponent<ExplosiveMineItem>();
			string displayName = this.baseItem.displayName;
			inventoryItemType.SetDisplayName(displayName);
			inventoryItemType.InitializeItem(null);
			inventoryItemType.SetItemLevel(level);
			inventoryItemType.SetBaseValue(GameMath.GetCreditsValue((float)inventoryItemType.baseCost, level));
			return inventoryItemType;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000B2B5C File Offset: 0x000B0D5C
		public InventoryItemType CreateWarpFuel(Rarity rarity)
		{
			InventoryItemType result;
			switch (rarity)
			{
			case Rarity.Standard:
				result = this.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				break;
			case Rarity.Enhanced:
				result = this.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				break;
			case Rarity.HighGrade:
				result = this.CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f);
				break;
			case Rarity.Exotic:
				result = this.CreateWarpFuel(WarpFuelItem.WarpFuelType.HyperCell, 1f);
				break;
			case Rarity.Legendary:
				result = this.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				break;
			default:
				result = this.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				break;
			}
			return result;
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000B2BE0 File Offset: 0x000B0DE0
		public InventoryItemType CreateWarpFuel(WarpFuelItem.WarpFuelType fuelType, float remaining = 1f)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			WarpFuelItem component = inventoryItemType.GetComponent<WarpFuelItem>();
			ValueTuple<string, string, Rarity, float, float, int, float> warpFuelConfig = ItemBuilder.GetWarpFuelConfig(fuelType);
			inventoryItemType.SetDisplayName(warpFuelConfig.Item1);
			inventoryItemType.SetDescription(warpFuelConfig.Item2);
			inventoryItemType.SetRarity(warpFuelConfig.Item3);
			inventoryItemType.InitializeItem(null);
			inventoryItemType.SetItemLevel(warpFuelConfig.Item6);
			inventoryItemType.SetBaseValue(GameMath.GetCreditsValue(warpFuelConfig.Item7, inventoryItemType.itemLevel));
			component.SetWarpFuel(1f, warpFuelConfig.Item4, warpFuelConfig.Item5, remaining);
			return inventoryItemType;
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000B2C80 File Offset: 0x000B0E80
		public InventoryItemType CreateCreditsItem(int amount)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			CreditsItem component = inventoryItemType.GetComponent<CreditsItem>();
			inventoryItemType.SetRarity(Rarity.Standard);
			inventoryItemType.SetDisplayName(Translation.Translate("@CreditsItem", Array.Empty<object>()));
			inventoryItemType.SetDescription(Translation.Translate("@CreditsItemDescription", new object[]
			{
				amount
			}));
			component.SetCredits(amount);
			return inventoryItemType;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000B2CF4 File Offset: 0x000B0EF4
		public InventoryItemType CreateLootboxItem(int level)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			LootBoxItem component = inventoryItemType.GetComponent<LootBoxItem>();
			inventoryItemType.SetRarity(Rarity.HighGrade);
			inventoryItemType.SetItemLevel(level);
			inventoryItemType.SetDisplayName(Translation.Translate("@LootBoxItem", Array.Empty<object>()));
			inventoryItemType.SetDescription(Translation.Translate("@LootBoxItemDescription", Array.Empty<object>()));
			inventoryItemType.InitializeItem(null);
			component.Init(SystemMapData.current);
			return inventoryItemType;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000B2D70 File Offset: 0x000B0F70
		public InventoryItemType CreateRefinedMaterialItem(RefinedMaterial mat, float amount)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			RefinedMaterialsItem component = inventoryItemType.GetComponent<RefinedMaterialsItem>();
			inventoryItemType.SetRarity(Rarity.Standard);
			inventoryItemType.SetDisplayName(Translation.Translate("@RefinedMaterialsItem", new object[]
			{
				mat
			}));
			inventoryItemType.SetDescription(Translation.Translate("@RefinedMaterialsItemDescription", new object[]
			{
				amount,
				mat
			}));
			inventoryItemType.SetBaseValue(Mathf.RoundToInt(mat.GetValue() * amount));
			component.SetMaterial(mat, amount);
			return inventoryItemType;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000B2E0C File Offset: 0x000B100C
		public static ValueTuple<string, string, Rarity, float, float, int, float> GetWarpFuelConfig(WarpFuelItem.WarpFuelType fuelType)
		{
			switch (fuelType)
			{
			case WarpFuelItem.WarpFuelType.PlasmaCell:
				return new ValueTuple<string, string, Rarity, float, float, int, float>("@PlasmaCellName", "@PlasmaCellDescription", Rarity.Enhanced, 2.8f, 120f, 1, 9f);
			case WarpFuelItem.WarpFuelType.IonCell:
				return new ValueTuple<string, string, Rarity, float, float, int, float>("@IonCellName", "@IonCellDescription", Rarity.HighGrade, 3f, 180f, 25, 16f);
			case WarpFuelItem.WarpFuelType.HyperCell:
				return new ValueTuple<string, string, Rarity, float, float, int, float>("@HyperCellName", "@HyperCellDescription", Rarity.Exotic, 3.3f, 250f, 58, 34f);
			default:
				Debug.LogWarning("Trying to createwarpfuelconfig, but enum value is default.");
				return new ValueTuple<string, string, Rarity, float, float, int, float>("@PlasmaCellName", "@PlasmaCellDescription", Rarity.Standard, 1.8f, 200f, 1, 11f);
			}
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000B2EBC File Offset: 0x000B10BC
		public InventoryItemType CreateOfficerPod(CrewMemberData crewData)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			inventoryItemType.GetComponent<OfficerPodItem>().crewMember = crewData;
			inventoryItemType.SetIcon(crewData.icon.sprite);
			inventoryItemType.SetRarity(Rarity.Standard);
			inventoryItemType.SetDisplayName("Officer Pod - " + crewData.GetFullName());
			inventoryItemType.SetDescription(Translation.Translate("@CrewPodDescription", Array.Empty<object>()));
			return inventoryItemType;
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000B2F34 File Offset: 0x000B1134
		public InventoryItemType CreateCrewPod(Behaviour.Crew.Crew crew, int amount)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			CrewPodItem component = inventoryItemType.GetComponent<CrewPodItem>();
			component.crewAmount = amount;
			component.crewType = crew.name;
			inventoryItemType.SetRarity(crew.rarity);
			inventoryItemType.SetDisplayName(Translation.Translate("@" + crew.name, Array.Empty<object>()));
			inventoryItemType.SetDescription(Translation.Translate("@" + crew.name + "Desc", Array.Empty<object>()));
			return inventoryItemType;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x000B2FC8 File Offset: 0x000B11C8
		public InventoryItemType CreateBlueprint(CraftingRecipe cr)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			inventoryItemType.GetComponent<BlueprintItem>().blueprintName = cr.identifier;
			InventoryItemType inventoryItemType2 = null;
			using (IEnumerator<KeyValuePair<InventoryItemType, int>> enumerator = cr.GetResultsPreview().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<InventoryItemType, int> keyValuePair = enumerator.Current;
					inventoryItemType2 = keyValuePair.Key;
				}
			}
			string text = Translation.Translate(((inventoryItemType2 != null) ? inventoryItemType2.itemCategory.GetDisplayName() : null) ?? "item", Array.Empty<object>()).ToLower();
			inventoryItemType.SetRarity(cr.craftingRarity);
			float num = 20f;
			if (cr.craftingRarity >= Rarity.HighGrade)
			{
				num *= Mathf.Pow(2f, (float)cr.craftingRarity);
			}
			inventoryItemType.SetBaseValue(Mathf.RoundToInt((float)cr.craftingCost * num));
			inventoryItemType.SetDisplayName(Translation.Translate("@BlueprintItem", new object[]
			{
				cr.displayName
			}));
			inventoryItemType.SetDescription(Translation.Translate("@BlueprintItemDescription", new object[]
			{
				text
			}));
			inventoryItemType.InitializeItem(null);
			return inventoryItemType;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000B30FC File Offset: 0x000B12FC
		public InventoryItemType CreateRandomBlueprint(int level, Rarity? recipeRarity = null, SeededRandom random = null, bool excludeExoticOrHigher = true)
		{
			if (random == null)
			{
				random = SeededRandom.Global;
			}
			Dictionary<CraftingRecipe, float> dictionary = new Dictionary<CraftingRecipe, float>();
			foreach (CraftingRecipe craftingRecipe in CraftingRecipe.GetBlueprintableRecipes())
			{
				if ((float)level >= craftingRecipe.minLevel && (!excludeExoticOrHigher || (craftingRecipe.craftingRarity != Rarity.Exotic && craftingRecipe.craftingRarity != Rarity.Legendary)) && (recipeRarity == null || recipeRarity.Value == craftingRecipe.craftingRarity))
				{
					dictionary.Add(craftingRecipe, 1f / Mathf.Pow(craftingRecipe.craftingRarity.GetCostMultiplier(), 2f));
				}
			}
			if (dictionary.Count > 0)
			{
				return this.CreateBlueprint(random.Choose<CraftingRecipe>(dictionary));
			}
			return null;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000B31C4 File Offset: 0x000B13C4
		public InventoryItemType CreateAspect(EquipAspect aspect)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			inventoryItemType.GetComponent<AspectItem>().aspectName = aspect.identifier;
			inventoryItemType.SetRarity(aspect.common ? Rarity.Enhanced : Rarity.Exotic);
			int num = 12;
			if (!aspect.common)
			{
				num = 24;
			}
			inventoryItemType.SetBaseValue(500 * num);
			inventoryItemType.SetDisplayName(Translation.Translate("@AspectItem", new object[]
			{
				aspect.displayName
			}));
			string text = Translation.Translate("@AspectItemDescription", Array.Empty<object>());
			inventoryItemType.SetDescription(aspect.description + "\n\n" + text.HighlightWithColor(ColorHelper.boringGrey));
			inventoryItemType.InitializeItem(null);
			return inventoryItemType;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000B3284 File Offset: 0x000B1484
		public InventoryItemType CreateItemType(JsonObject data)
		{
			InventoryItemType inventoryItemType = UnityEngine.Object.Instantiate<InventoryItemType>(this.baseItem, PersistentSingleton<GameManager>.Instance.itemBuilderRoot);
			inventoryItemType.itemBuilder = this;
			inventoryItemType.ItemDataFromJson(data);
			inventoryItemType.InitializeItem(null);
			return inventoryItemType;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000B32B0 File Offset: 0x000B14B0
		public JsonValue ToJson(InventoryItemType type)
		{
			JsonObject jsonObject = new JsonObject();
			type.DataToJson(jsonObject);
			jsonObject["itemType"] = this.identifier;
			return jsonObject;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000B32E6 File Offset: 0x000B14E6
		public static ItemBuilder Get(string name)
		{
			return ItemBuilder.allBuilders[name];
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000B32F4 File Offset: 0x000B14F4
		public static void LoadAll()
		{
			ItemBuilder.allBuilders.Clear();
			ItemBuilder[] array = Resources.LoadAll<ItemBuilder>("ItemBuilder");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].name.StartsWith("_"))
				{
					array[i].identifier = array[i].name;
					ItemBuilder.allBuilders[array[i].identifier] = array[i];
				}
			}
		}

		// Token: 0x0400121D RID: 4637
		private static Dictionary<string, ItemBuilder> allBuilders = new Dictionary<string, ItemBuilder>();

		// Token: 0x0400121F RID: 4639
		[SerializeField]
		private InventoryItemType baseItem;
	}
}
