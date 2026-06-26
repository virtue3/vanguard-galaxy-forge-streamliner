using Behaviour.Crafting;
using Behaviour.UI.Forge;
using Behaviour.UI.Spacestation;
using HarmonyLib;
using Source.Mining;
using UnityEngine;
using UnityEngine.UI;

namespace VanguardMod
{
    public class DebugMenu : MonoBehaviour
    {
        private bool _visible;
        private Rect _window = new Rect(100, 100, 340, 300);

        private void Update()
        {
            if (Input.GetKeyDown(Plugin.ToggleKey.Value))
                _visible = !_visible;
        }

        private void OnGUI()
        {
            if (!_visible) return;
            _window = GUI.Window(9001, _window, DrawWindow, "VG Forge Streamliner");
        }

        private void DrawWindow(int id)
        {
            var forge = Forge.current;
            var forgeUI = ForgeUI.current;

            if (forge == null || forgeUI == null)
            {
                GUILayout.Label("Open the Forge at a space station.");
                GUILayout.Space(8);
                if (GUILayout.Button("Close")) _visible = false;
                GUI.DragWindow();
                return;
            }

            var tabContents = Traverse.Create(forgeUI).Field("tabContents").GetValue<ForgeTabContents>();
            var recipe = tabContents?.subRecipe;
            var slider = tabContents != null
                ? Traverse.Create(tabContents).Field("countSlider").GetValue<Slider>()
                : null;
            int amount = slider != null ? Mathf.Max(1, Mathf.RoundToInt(slider.value)) : 1;

            if (recipe == null)
            {
                GUILayout.Label("No recipe selected.");
                GUILayout.Space(8);
                if (GUILayout.Button("Close")) _visible = false;
                GUI.DragWindow();
                return;
            }

            GUILayout.Label("Recipe: " + recipe.displayName + " x" + amount);
            GUILayout.Space(4);

            bool anyMissing = false;
            bool canCraftAll = true;
            int freeSlots = forge.maxJobs - forge.jobs.Count;
            int slotsPreview = freeSlots;

            GUILayout.Label("Free slots: " + freeSlots + " / " + forge.maxJobs);

            foreach (var pair in recipe.GetIngredientItems(0))
            {
                int needed = pair.Item2 * amount;
                int available = forge.spaceStation.CountAvailableItems(pair.Item1);
                if (available >= needed) continue;

                var src = CraftingRecipe.GetSourceRecipe(pair.Item1);
                if (src == null) continue;

                anyMissing = true;
                int missing = needed - available;
                int yieldPer = Mathf.Max(1, src.GetResultCount(pair.Item1));
                int runs = Mathf.CeilToInt((float)missing / yieldPer);
                bool canCraft = src.CountAvailableForCrafting(forge) >= runs;
                canCraftAll &= canCraft;

                int slots = slotsPreview > 0 ? Mathf.Clamp(slotsPreview, 1, runs) : 0;
                slotsPreview -= slots;

                string slotInfo = slots > 1 ? " → " + slots + " parallel jobs" : "";
                string status = canCraft ? slotInfo : " [NOT ENOUGH MATERIALS]";
                GUILayout.Label(pair.Item1.displayName + ": " + runs + " runs" + status);
            }

            GUILayout.Space(4);

            if (!anyMissing)
            {
                GUILayout.Label("All sub-components available.");
            }
            else
            {
                GUI.enabled = canCraftAll;
                if (GUILayout.Button("Craft Missing Sub-Parts"))
                {
                    CraftSubParts(forge, recipe, amount);
                }
                GUI.enabled = true;
            }

            GUILayout.Space(8);
            if (GUILayout.Button("Close")) _visible = false;
            GUI.DragWindow();
        }

        private static void CraftSubParts(Forge forge, CraftingRecipe recipe, int amount)
        {
            // Collect (source recipe, total runs needed) for each missing ingredient.
            var pending = new System.Collections.Generic.List<(CraftingRecipe src, int runs)>();
            foreach (var pair in recipe.GetIngredientItems(0))
            {
                int needed = pair.Item2 * amount;
                int available = forge.spaceStation.CountAvailableItems(pair.Item1);
                if (available >= needed) continue;
                var src = CraftingRecipe.GetSourceRecipe(pair.Item1);
                if (src == null) continue;
                int missing = needed - available;
                int yieldPer = Mathf.Max(1, src.GetResultCount(pair.Item1));
                int runs = Mathf.CeilToInt((float)missing / yieldPer);
                pending.Add((src, runs));
            }

            // Spread jobs across all available slots.
            // Each ingredient gets at least 1 slot; remaining slots go to whoever needs them most.
            int availableSlots = forge.maxJobs - forge.jobs.Count;

            foreach (var (src, runs) in pending)
            {
                if (availableSlots <= 0) break;

                // Use as many slots as useful (no point using more slots than runs).
                int slots = Mathf.Clamp(availableSlots, 1, runs);
                int baseRuns = runs / slots;
                int remainder = runs % slots;

                for (int i = 0; i < slots; i++)
                {
                    int runsThisJob = baseRuns + (i < remainder ? 1 : 0);
                    if (runsThisJob > 0)
                        forge.TryStartJob(src, runsThisJob);
                }

                availableSlots -= slots;
            }

            if (ForgeUI.current != null) ForgeUI.current.UpdateContent();
            if (SpaceStationInterior.instance != null) SpaceStationInterior.instance.UpdateJobs();
        }
    }
}
