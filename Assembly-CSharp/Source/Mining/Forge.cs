using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.UI.Forge;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using LightJson;
using Source.Crew;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Mining
{
	// Token: 0x020000E7 RID: 231
	public class Forge : IJsonSource
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00044FA8 File Offset: 0x000431A8
		public float craftingSpeed
		{
			get
			{
				float num = 0.5f;
				float num2 = 1f;
				num2 += SkilltreeNode.industrialT1CraftingSpeed.currentIncrease;
				num2 += SkilltreeNode.industrialCraftingSpeed2.currentIncrease;
				Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Industrial));
				int masteryLevel = GamePlayer.current.commander.GetSkillTreeData(tree, false).masteryLevel;
				num2 += (float)masteryLevel * 0.005f;
				return num * num2;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0004500A File Offset: 0x0004320A
		public static Forge current
		{
			get
			{
				SpaceStation current = SpaceStation.current;
				if (current == null)
				{
					return null;
				}
				return current.forge;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x0004501C File Offset: 0x0004321C
		public int maxJobs
		{
			get
			{
				return 2 + SkilltreeNode.industrialBasicJobs.currentPoints + SkilltreeNode.industrialEnhancedJobs.currentPoints;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00045035 File Offset: 0x00043235
		public IEnumerable<CraftingRecipe> recipes
		{
			get
			{
				return this.spaceStation.recipes;
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00045042 File Offset: 0x00043242
		public Forge(SpaceStation spaceStation)
		{
			this.spaceStation = spaceStation;
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x0004505C File Offset: 0x0004325C
		public void ProgressJobs(float deltaTime)
		{
			List<ForgeJob> list = new List<ForgeJob>();
			foreach (ForgeJob forgeJob in this.jobs)
			{
				forgeJob.ProgressJob(deltaTime);
				if (forgeJob.remainingAmount <= 0)
				{
					list.Add(forgeJob);
					if (!SpaceStationInterior.instance)
					{
						Singleton<EventLogManager>.Instance.NewEvent("ForgeJobFinished" + forgeJob.recipe.identifier, Translation.Translate("@LogForgeJobFinished", new object[]
						{
							forgeJob.recipe.displayName,
							this.spaceStation.name
						}));
					}
				}
			}
			foreach (ForgeJob item in list)
			{
				this.jobs.Remove(item);
				foreach (DeferredJob deferredJob in this.deferredJobs)
				{
					deferredJob.Prerequisites.Remove(item);
				}
			}
			// Activate any deferred jobs whose sub-component prerequisites have all completed
			List<DeferredJob> ready = new List<DeferredJob>();
			foreach (DeferredJob deferredJob in this.deferredJobs)
			{
				if (deferredJob.Prerequisites.Count == 0)
				{
					ready.Add(deferredJob);
				}
			}
			foreach (DeferredJob deferredJob in ready)
			{
				this.deferredJobs.Remove(deferredJob);
				this.ActivateDeferredJob(deferredJob);
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00045160 File Offset: 0x00043360
		public bool TryStartJob(CraftingRecipe recipe, int amount)
		{
			if (this.jobs.Count < this.maxJobs)
			{
				return this.StartJob(recipe, amount);
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSForgeFull", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
			return false;
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000451B4 File Offset: 0x000433B4
		public bool StartJob(CraftingRecipe recipe, int amount)
		{
			int num = recipe.craftingCost * amount;
			if (!GamePlayer.current.CanAfford((float)num))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			// Happy path: all ingredients already available.
			if (recipe.ConsumeIngredientsForCrafting(this, amount))
			{
				GamePlayer.current.RemoveCredits((float)num);
				ForgeJob forgeJob = new ForgeJob(this, recipe, amount);
				forgeJob.craftedLevel = recipe.GetAdjustedOutputLevel();
				this.jobs.Add(forgeJob);
				return true;
			}
			// Queue sub-component jobs across all available slots, then defer the main job.
			List<ForgeJob> prereqs = this.QueueSubComponentJobs(recipe, amount);
			if (prereqs != null)
			{
				DeferredJob deferredJob = new DeferredJob { Recipe = recipe, Amount = amount };
				foreach (ForgeJob prereq in prereqs)
				{
					deferredJob.Prerequisites.Add(prereq);
				}
				this.deferredJobs.Add(deferredJob);
				return true;
			}
			return false;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0004523C File Offset: 0x0004343C
		public void CancelJob(ForgeJob job)
		{
			foreach (ValueTuple<RefinedMaterial, float> valueTuple in job.recipe.GetIngredientMaterials(job.craftedLevel))
			{
				this.spaceStation.refinery.AddRefinedMaterial(valueTuple.Item1, valueTuple.Item2 * (float)job.remainingAmount);
			}
			foreach (ValueTuple<InventoryItemType, int> valueTuple2 in job.recipe.GetIngredientItems(job.craftedLevel))
			{
				this.spaceStation.materialStorage.Add(valueTuple2.Item1, valueTuple2.Item2 * job.remainingAmount, false, false);
			}
			GamePlayer.current.credits += (long)(job.recipe.craftingCost * job.remainingAmount);
			this.jobs.Remove(job);
			// Drop any deferred job that was waiting on this cancelled sub-component job.
			// The main recipe's ingredients were never consumed, so no refund is needed.
			this.deferredJobs.RemoveAll(dj => dj.Prerequisites.Contains(job));
			if (ForgeUI.current)
			{
				ForgeUI.current.UpdateContent();
			}
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00045360 File Offset: 0x00043560
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"jobs",
					this.jobs.ToJsonArray<ForgeJob>()
				}
			};
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00045388 File Offset: 0x00043588
		public static Forge FromJson(SpaceStation parent, JsonValue data)
		{
			Forge forge = new Forge(parent);
			if (data["jobs"].IsJsonArray)
			{
				forge.jobs.FromJsonArray(data["jobs"], (JsonValue json) => ForgeJob.FromJson(forge, json));
			}
			return forge;
		}

		// Validates that all missing item ingredients can be auto-crafted, then queues
		// sub-component ForgeJobs spread evenly across every available job slot.
		// Returns the list of queued sub-component jobs (used as deferred-job prerequisites),
		// or null if auto-crafting is not possible.
		private List<ForgeJob> QueueSubComponentJobs(CraftingRecipe recipe, int amount)
		{
			// --- Validation pass ---
			foreach (ValueTuple<InventoryItemType, int> ingredient in recipe.GetIngredientItems(0))
			{
				InventoryItemType item = ingredient.Item1;
				int needed = ingredient.Item2 * amount;
				int available = this.spaceStation.CountAvailableItems(item);
				if (available >= needed) continue;

				int missing = needed - available;
				CraftingRecipe sourceRecipe = CraftingRecipe.GetSourceRecipe(item);
				if (sourceRecipe == null) return null;

				int yieldPerCraft = Mathf.Max(1, sourceRecipe.GetResultCount(item));
				int totalRuns = Mathf.CeilToInt((float)missing / yieldPerCraft);
				if (sourceRecipe.CountAvailableForCrafting(this) < totalRuns) return null;
				if (!GamePlayer.current.CanAfford((float)(sourceRecipe.craftingCost * totalRuns))) return null;
			}

			// --- Queue pass: spread runs across all available slots ---
			var prereqs = new List<ForgeJob>();
			int availableSlots = this.maxJobs - this.jobs.Count;

			foreach (ValueTuple<InventoryItemType, int> ingredient in recipe.GetIngredientItems(0))
			{
				InventoryItemType item = ingredient.Item1;
				int needed = ingredient.Item2 * amount;
				int available = this.spaceStation.CountAvailableItems(item);
				if (available >= needed) continue;

				int missing = needed - available;
				CraftingRecipe sourceRecipe = CraftingRecipe.GetSourceRecipe(item);
				int yieldPerCraft = Mathf.Max(1, sourceRecipe.GetResultCount(item));
				int totalRuns = Mathf.CeilToInt((float)missing / yieldPerCraft);

				// Distribute totalRuns across as many slots as possible (up to what's left)
				int slotsToUse = Mathf.Clamp(availableSlots, 1, totalRuns);
				int baseRuns = totalRuns / slotsToUse;
				int remainder = totalRuns % slotsToUse;

				for (int i = 0; i < slotsToUse; i++)
				{
					int runsThisJob = baseRuns + (i < remainder ? 1 : 0);
					if (runsThisJob <= 0) continue;
					if (!sourceRecipe.ConsumeIngredientsForCrafting(this, runsThisJob)) break;
					GamePlayer.current.RemoveCredits((float)(sourceRecipe.craftingCost * runsThisJob));
					ForgeJob subJob = new ForgeJob(this, sourceRecipe, runsThisJob);
					subJob.craftedLevel = sourceRecipe.GetAdjustedOutputLevel();
					this.jobs.Add(subJob);
					prereqs.Add(subJob);
				}

				availableSlots -= slotsToUse;
				if (availableSlots <= 0) availableSlots = 0;
			}

			return prereqs.Count > 0 ? prereqs : null;
		}

		// Called by ProgressJobs once all prerequisite sub-component jobs have completed.
		private void ActivateDeferredJob(DeferredJob deferredJob)
		{
			int creditCost = deferredJob.Recipe.craftingCost * deferredJob.Amount;
			if (!GamePlayer.current.CanAfford((float)creditCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (!deferredJob.Recipe.ConsumeIngredientsForCrafting(this, deferredJob.Amount))
			{
				return;
			}
			GamePlayer.current.RemoveCredits((float)creditCost);
			ForgeJob forgeJob = new ForgeJob(this, deferredJob.Recipe, deferredJob.Amount);
			forgeJob.craftedLevel = deferredJob.Recipe.GetAdjustedOutputLevel();
			this.jobs.Add(forgeJob);
			if (ForgeUI.current)
			{
				ForgeUI.current.UpdateContent();
			}
		}

		private class DeferredJob
		{
			public CraftingRecipe Recipe;
			public int Amount;
			public readonly HashSet<ForgeJob> Prerequisites = new HashSet<ForgeJob>();
		}

		// Token: 0x04000498 RID: 1176
		public readonly SpaceStation spaceStation;

		// Token: 0x04000499 RID: 1177
		public List<ForgeJob> jobs = new List<ForgeJob>();

		private readonly List<DeferredJob> deferredJobs = new List<DeferredJob>();
	}
}
