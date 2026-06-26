using System.Collections.Generic;
using System.Linq;
using Behaviour.Crafting;
using Behaviour.UI.Forge;
using HarmonyLib;
using Source.Mining;

namespace VanguardMod
{
    // When opening a recipe with multiple rarity tiers, default to the highest available.
    [HarmonyPatch(typeof(ForgeUI), "SelectRecipe")]
    internal static class Patch_ForgeUI_SelectRecipe
    {
        static void Prefix(ref CraftingRecipe subRecipe, List<CraftingRecipe> unlockedRecipes)
        {
            if (subRecipe == null && unlockedRecipes != null && unlockedRecipes.Count > 0)
                subRecipe = unlockedRecipes.OrderByDescending(r => r.craftingRarity).First();
        }
    }

    // Cancel the mod queue and all remaining forge jobs when the player cancels any forge job.
    [HarmonyPatch(typeof(Forge), "CancelJob")]
    internal static class Patch_Forge_CancelJob
    {
        static void Postfix()
        {
            ForgeStreamlinerUI.Instance?.CancelQueue();
        }
    }
}
