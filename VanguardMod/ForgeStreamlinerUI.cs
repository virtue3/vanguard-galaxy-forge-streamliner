using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.UI.Forge;
using Behaviour.UI.Spacestation;
using HarmonyLib;
using Source.Mining;
using Source.Util;
using UnityEngine;
using UnityEngine.UI;

namespace VanguardMod
{
    public class ForgeStreamlinerUI : MonoBehaviour
    {
        internal static ForgeStreamlinerUI Instance { get; private set; }

        private void Awake() => Instance = this;

        private bool _cancellingJobs;

        internal void CancelQueue()
        {
            _craftQueue.Clear();
            _lastForgeJobCount = -1;
            RefreshForgeUI();

            if (_cancellingJobs) return;
            _cancellingJobs = true;
            try
            {
                var forge = Forge.current;
                if (forge == null) return;
                foreach (var job in new List<ForgeJob>(forge.jobs))
                    forge.CancelJob(job);
                RefreshForgeUI();
            }
            finally
            {
                _cancellingJobs = false;
            }
        }

        private bool _visible;
        private bool _wasAtStation;
        private bool _wasForgeOpen;
        private Rect _windowRect = new Rect(100, 100, 360, 500);

        private class CraftOrder
        {
            public CraftingRecipe Recipe;
            public int Amount;
        }

        private readonly List<CraftOrder> _craftQueue = new List<CraftOrder>();
        private int _lastForgeJobCount = -1;

        private void Update()
        {
            if (Input.GetKeyDown(Plugin.ToggleKey.Value))
                _visible = !_visible;

            bool atStation = Forge.current != null;
            if (_wasAtStation && !atStation)
            {
                _visible = false;
                CancelQueue();
            }
            _wasAtStation = atStation;

            bool forgeOpen = ForgeUI.current != null;
            if (!_wasForgeOpen && forgeOpen)
                _visible = true;
            _wasForgeOpen = forgeOpen;

            OnForgeJobTick();
        }

        // Called every frame; advances the queue when the forge goes completely idle.
        // We only trigger on a full-empty transition (not every single job completion) so that
        // manually cancelling one job doesn't cause us to immediately refill the freed slot.
        private void OnForgeJobTick()
        {
            if (_craftQueue.Count == 0) return;

            var forge = Forge.current;
            if (forge == null) { _lastForgeJobCount = -1; return; }

            int currentJobCount = forge.jobs.Count;
            bool forgeJustEmptied = _lastForgeJobCount > 0 && currentJobCount == 0;
            _lastForgeJobCount = currentJobCount;

            if (forgeJustEmptied)
                AdvanceQueue(forge);
        }

        // Called by Patch_Forge_CancelJob when a job is cancelled but the forge is not empty.
        // Resets tracking so the cancel isn't treated as a natural completion.
        internal void ResetJobTracking()
        {
            _lastForgeJobCount = -1;
        }

        private void EnqueueOrder(CraftingRecipe recipe, int amount)
        {
            _craftQueue.Add(new CraftOrder { Recipe = recipe, Amount = amount });
            var forge = Forge.current;
            if (forge == null) return;
            _lastForgeJobCount = forge.jobs.Count;
            // Only kick off the queue if the forge is idle; if jobs are already running
            // the tick loop will advance once they complete, ensuring we re-evaluate
            // sub-component needs with the materials actually in storage at that point.
            if (forge.jobs.Count == 0)
                AdvanceQueue(forge);
            RefreshForgeUI();
        }

        // Drives the front of the queue: starts the main job when sub-components are ready,
        // or queues the next batch of sub-component jobs when the forge is idle.
        private void AdvanceQueue(Forge forge)
        {
            if (_craftQueue.Count == 0) return;

            var order = _craftQueue[0];

            if (AllSubComponentsAvailable(forge, order.Recipe, order.Amount))
            {
                if (forge.TryStartJob(order.Recipe, order.Amount))
                {
                    _craftQueue.RemoveAt(0);
                    RefreshForgeUI();
                    // Chain immediately into the next order if slots remain.
                    if (_craftQueue.Count > 0 && forge.jobs.Count < forge.maxJobs)
                        AdvanceQueue(forge);
                }
                return;
            }

            // Wait for all running jobs to finish before committing materials to the next batch.
            if (forge.jobs.Count > 0) return;

            QueueSubComponentJobs(forge, order.Recipe, order.Amount);
        }

        private static bool AllSubComponentsAvailable(Forge forge, CraftingRecipe recipe, int amount)
        {
            foreach (var (item, countPerCraft) in recipe.GetIngredientItems(0))
            {
                if (forge.spaceStation.CountAvailableItems(item) < countPerCraft * amount)
                    return false;
            }
            return true;
        }

        private void OnGUI()
        {
            if (!_visible) return;
            _windowRect = GUI.Window(9001, _windowRect, DrawWindow, "VG Forge Streamliner");
        }

        private void DrawWindow(int id)
        {
            var forge = Forge.current;
            var forgeUI = ForgeUI.current;

            DrawQueueSection(forge);

            GUILayout.Space(8);

            DrawSelectedRecipeSection(forge, forgeUI);

            GUILayout.Space(8);
            if (GUILayout.Button("Close")) _visible = false;
            GUI.DragWindow();
        }

        private void DrawQueueSection(Forge forge)
        {
            GUILayout.Label("=== Craft Queue ===");

            if (_craftQueue.Count == 0)
            {
                GUILayout.Label("(empty)");
                return;
            }

            int toRemove = -1;
            for (int i = 0; i < _craftQueue.Count; i++)
            {
                var order = _craftQueue[i];
                bool isActive = i == 0;

                GUILayout.BeginHorizontal();
                string prefix = isActive ? "> " : "   ";
                string suffix = isActive && forge == null ? "  [paused]"
                              : isActive && forge != null && forge.jobs.Count > 0 ? "  [crafting...]"
                              : "";
                GUILayout.Label(prefix + Translation.Translate(order.Recipe.displayName, new object[0]) + " x" + order.Amount + suffix);
                if (GUILayout.Button("X", GUILayout.Width(26)))
                    toRemove = i;
                GUILayout.EndHorizontal();

                // Only show sub-components for the item currently being worked on.
                if (isActive && forge != null)
                {
                    foreach (var (item, countPerCraft) in order.Recipe.GetIngredientItems(0))
                    {
                        var subRecipe = CraftingRecipe.GetSourceRecipe(item);
                        if (subRecipe == null) continue;

                        int needed = countPerCraft * order.Amount;
                        int available = forge.spaceStation.CountAvailableItems(item);
                        if (available >= needed) continue;

                        int missing = needed - available;
                        int yieldPerCraft = Mathf.Max(1, subRecipe.GetResultCount(item));
                        int runs = Mathf.CeilToInt((float)missing / yieldPerCraft);
                        bool crafting = forge.jobs.Exists(j => j.recipe == subRecipe);
                        string subPrefix = crafting ? "> " : "  ";
                        GUILayout.Label("    " + subPrefix + Translation.Translate(subRecipe.displayName, new object[0]) + " x" + runs);
                    }
                }
            }

            if (toRemove == 0)
                CancelQueue();                  // active item: also kill running forge jobs
            else if (toRemove > 0)
            {
                _craftQueue.RemoveAt(toRemove); // future item: just drop it from the list
                RefreshForgeUI();
            }
        }

        private void DrawSelectedRecipeSection(Forge forge, ForgeUI forgeUI)
        {
            GUILayout.Label("=== Selected Recipe ===");

            if (forge == null || forgeUI == null)
            {
                GUILayout.Label("Open the Forge at a space station.");
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
                return;
            }

            GUILayout.Label(Translation.Translate(recipe.displayName, new object[0]) + " x" + amount);

            int freeSlots = forge.maxJobs - forge.jobs.Count;
            GUILayout.Label("Free slots: " + freeSlots + " / " + forge.maxJobs);

            bool anyMissing = DrawMissingSubComponents(forge, recipe, amount, freeSlots);

            GUILayout.Space(4);

            if (!anyMissing)
            {
                GUILayout.Label("All sub-components available.");
            }
            else
            {
                if (GUILayout.Button("Craft Missing Sub-Parts Now"))
                    QueueSubComponentJobs(forge, recipe, amount);
            }

            GUILayout.Space(2);
            if (GUILayout.Button("Add to Queue"))
                EnqueueOrder(recipe, amount);
        }

        // Draws the missing sub-component lines and returns whether anything is missing.
        private static bool DrawMissingSubComponents(Forge forge, CraftingRecipe recipe, int amount, int freeSlots)
        {
            bool anyMissing = false;
            int slotsRemaining = freeSlots;

            foreach (var (item, countPerCraft) in recipe.GetIngredientItems(0))
            {
                int needed = countPerCraft * amount;
                int available = forge.spaceStation.CountAvailableItems(item);
                if (available >= needed) continue;

                var subRecipe = CraftingRecipe.GetSourceRecipe(item);
                if (subRecipe == null) continue;

                anyMissing = true;
                int missing = needed - available;
                int yieldPerCraft = Mathf.Max(1, subRecipe.GetResultCount(item));
                int runsNeeded = Mathf.CeilToInt((float)missing / yieldPerCraft);
                bool hasMaterials = subRecipe.CountAvailableForCrafting(forge) >= runsNeeded;

                int parallelJobs = slotsRemaining > 0 ? Mathf.Clamp(slotsRemaining, 1, runsNeeded) : 0;
                slotsRemaining -= parallelJobs;

                string jobInfo = parallelJobs > 1 ? " -> " + parallelJobs + " parallel jobs" : "";
                string warning = hasMaterials ? jobInfo : " [insufficient materials]";
                GUILayout.Label(Translation.Translate(item.displayName, new object[0]) + ": " + runsNeeded + " runs" + warning);
            }

            return anyMissing;
        }

        // Splits the required sub-component runs across all available forge slots.
        private static void QueueSubComponentJobs(Forge forge, CraftingRecipe recipe, int amount)
        {
            var jobsToQueue = new List<(CraftingRecipe subRecipe, int totalRuns)>();

            foreach (var (item, countPerCraft) in recipe.GetIngredientItems(0))
            {
                int needed = countPerCraft * amount;
                int available = forge.spaceStation.CountAvailableItems(item);
                if (available >= needed) continue;

                var subRecipe = CraftingRecipe.GetSourceRecipe(item);
                if (subRecipe == null) continue;

                int missing = needed - available;
                int yieldPerCraft = Mathf.Max(1, subRecipe.GetResultCount(item));
                int totalRuns = Mathf.CeilToInt((float)missing / yieldPerCraft);
                jobsToQueue.Add((subRecipe, totalRuns));
            }

            int availableSlots = forge.maxJobs - forge.jobs.Count;
            foreach (var (subRecipe, totalRuns) in jobsToQueue)
            {
                if (availableSlots <= 0) break;

                int slots = Mathf.Clamp(availableSlots, 1, totalRuns);
                int baseRunsPerJob = totalRuns / slots;
                int remainder = totalRuns % slots;

                for (int i = 0; i < slots; i++)
                {
                    int runsThisJob = baseRunsPerJob + (i < remainder ? 1 : 0);
                    if (runsThisJob > 0)
                        forge.TryStartJob(subRecipe, runsThisJob);
                }

                availableSlots -= slots;
            }

            RefreshForgeUI();
        }

        private static void RefreshForgeUI()
        {
            ForgeUI.current?.UpdateContent();
            SpaceStationInterior.instance?.UpdateJobs();
        }
    }
}
