using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.UI.Spacestation;
using LightJson;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.MissionSystem;
using Source.Player;
using Source.Spacestation;
using Source.Util;
using UnityEngine;

namespace Source.Mining
{
	// Token: 0x020000E8 RID: 232
	public class ForgeJob : IJsonSource, ISpaceStationJob
	{
		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x000453F0 File Offset: 0x000435F0
		public bool cancelAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x000453F3 File Offset: 0x000435F3
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x000453FB File Offset: 0x000435FB
		public int initialAmount { get; protected set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00045404 File Offset: 0x00043604
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0004540C File Offset: 0x0004360C
		public int remainingAmount { get; protected set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x00045415 File Offset: 0x00043615
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0004541D File Offset: 0x0004361D
		public CraftingRecipe recipe { get; private set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x00045426 File Offset: 0x00043626
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0004542E File Offset: 0x0004362E
		public float craftingTime { get; private set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00045437 File Offset: 0x00043637
		public string jobName
		{
			get
			{
				return this.recipe.displayName;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00045444 File Offset: 0x00043644
		public Sprite jobIcon
		{
			get
			{
				return this.recipe.icon;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x00045451 File Offset: 0x00043651
		public float jobProgress
		{
			get
			{
				return this.progress / this.craftingTime;
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00045460 File Offset: 0x00043660
		public ForgeJob(Forge parent, CraftingRecipe recipe, int totalAmount)
		{
			this.parent = parent;
			this.recipe = recipe;
			this.craftingTime = recipe.craftingTime / parent.craftingSpeed;
			this.initialAmount = totalAmount;
			this.remainingAmount = totalAmount;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00045498 File Offset: 0x00043698
		public void ProgressJob(float deltaTime)
		{
			this.progress += deltaTime;
			if (this.progress >= this.craftingTime)
			{
				this.progress = 0f;
				int remainingAmount = this.remainingAmount;
				this.remainingAmount = remainingAmount - 1;
				foreach (KeyValuePair<InventoryItemType, int> keyValuePair in this.recipe.GetResultsForCraft(this.craftedLevel))
				{
					InventoryItemType key = keyValuePair.Key;
					int value = keyValuePair.Value;
					this.AddForgeItemToCargo(key, value);
					MissionObjective.Trigger(MissionTrigger.CraftItem, new ValueTuple<InventoryItemType, int>(key, value), this.parent.spaceStation, false);
					if (key.identifier == "AI Core")
					{
						MissionObjective.Trigger(MissionTrigger.CraftAICore, value, null, true);
					}
					GamePlayer.current.currentSpaceShip.AddCrewExperience((float)GameMath.GetExperienceRewardValue(key.rarity.GetPowerMultiplier(), key.itemLevel), new CommanderSpecialization?(CommanderSpecialization.Industrial));
				}
				if (SeededRandom.Global.RandomBool(SkilltreeNode.industrialForgeBonusCraft.currentIncrease))
				{
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair2 in this.recipe.GetResultsForCraft(this.craftedLevel))
					{
						InventoryItemType key2 = keyValuePair2.Key;
						int value2 = keyValuePair2.Value;
						this.AddForgeItemToCargo(key2, value2);
						MissionObjective.Trigger(MissionTrigger.CraftItem, new ValueTuple<InventoryItemType, int>(key2, value2), this.parent.spaceStation, false);
					}
				}
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00045648 File Offset: 0x00043848
		private void AddForgeItemToCargo(InventoryItemType item, int amount)
		{
			if (SpaceStationInterior.instance && MapPointOfInterest.current == this.parent.spaceStation && !GamePlayer.current.currentSpaceShip.cargo.IsFull(item.m3 * (float)amount) && GamePlayer.current.forgeDepositInCargo && !(this.parent.spaceStation is IndustryStation))
			{
				GamePlayer.current.currentSpaceShip.cargo.Add(item, amount, false, false);
				return;
			}
			if (item.CanGoInArmory())
			{
				GamePlayer.current.globalInventory.Add(item, amount, false, false);
				return;
			}
			if (item.CanGoInMaterials())
			{
				this.parent.spaceStation.materialStorage.Add(item, amount, false, false);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x00045709 File Offset: 0x00043909
		public void CancelJob()
		{
			Forge.current.CancelJob(this);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x00045718 File Offset: 0x00043918
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"recipe",
					this.recipe.identifier
				},
				{
					"initialAmount",
					new double?((double)this.initialAmount)
				},
				{
					"remainingAmount",
					new double?((double)this.remainingAmount)
				},
				{
					"progress",
					new double?((double)this.progress)
				},
				{
					"craftedLevel",
					new double?((double)this.craftedLevel)
				}
			};
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000457C0 File Offset: 0x000439C0
		public static ForgeJob FromJson(Forge parent, JsonValue json)
		{
			if (!json.IsJsonObject)
			{
				return null;
			}
			return new ForgeJob(parent, json["recipe"].AsString, json["initialAmount"])
			{
				remainingAmount = json["remainingAmount"],
				progress = (float)json["progress"].AsNumber,
				craftedLevel = json["craftedLevel"]
			};
		}

		// Token: 0x0400049D RID: 1181
		private float progress;

		// Token: 0x0400049F RID: 1183
		private Forge parent;

		// Token: 0x040004A0 RID: 1184
		public int craftedLevel;
	}
}
