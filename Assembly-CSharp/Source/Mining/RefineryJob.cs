using System;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.Mining;
using Behaviour.UI.Spacestation;
using LightJson;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Spacestation;
using Source.Util;
using UnityEngine;

namespace Source.Mining
{
	// Token: 0x020000EA RID: 234
	public class RefineryJob : IJsonSource, ISpaceStationJob
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00045FCF File Offset: 0x000441CF
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x00045FD7 File Offset: 0x000441D7
		public int initialAmount { get; protected set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00045FE0 File Offset: 0x000441E0
		// (set) Token: 0x060008F3 RID: 2291 RVA: 0x00045FE8 File Offset: 0x000441E8
		public int remainingAmount { get; protected set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060008F4 RID: 2292 RVA: 0x00045FF1 File Offset: 0x000441F1
		// (set) Token: 0x060008F5 RID: 2293 RVA: 0x00045FF9 File Offset: 0x000441F9
		public OreItemData ore { get; protected set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00046002 File Offset: 0x00044202
		public bool cancelAvailable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00046005 File Offset: 0x00044205
		public string jobName
		{
			get
			{
				return this.ore.item.displayName;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00046017 File Offset: 0x00044217
		public Sprite jobIcon
		{
			get
			{
				return this.ore.item.icon;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00046029 File Offset: 0x00044229
		public float jobProgress
		{
			get
			{
				return this.progress / this.refineTime;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x00046038 File Offset: 0x00044238
		// (set) Token: 0x060008FB RID: 2299 RVA: 0x00046040 File Offset: 0x00044240
		public float refineTime { get; private set; }

		// Token: 0x060008FC RID: 2300 RVA: 0x00046049 File Offset: 0x00044249
		public RefineryJob(Refinery parent, OreItemData ore, int totalOreAmount)
		{
			this.parent = parent;
			this.ore = ore;
			this.refineTime = ore.refinementTime;
			this.initialAmount = totalOreAmount;
			this.remainingAmount = totalOreAmount;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0004607C File Offset: 0x0004427C
		public void ProgressJob(float deltaTime)
		{
			this.progress += deltaTime;
			bool flag = false;
			if (SkilltreeNode.industrialRefBonusCraft1.isActive && SeededRandom.Global.RandomBool(0.1f))
			{
				flag = true;
			}
			if (this.progress >= this.refineTime)
			{
				this.progress -= this.refineTime;
				int remainingAmount = this.remainingAmount;
				this.remainingAmount = remainingAmount - 1;
				bool flag2 = !this.ore.ignoreExtraRewards;
				foreach (OreRefinementProduct oreRefinementProduct in this.ore.contents)
				{
					this.parent.AddRefinedMaterial(oreRefinementProduct.product, (flag && flag2) ? (oreRefinementProduct.yield * 2f) : oreRefinementProduct.yield);
				}
				if (flag2)
				{
					Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Industrial));
					SkillTreeData skillTreeData = GamePlayer.current.commander.GetSkillTreeData(tree, false);
					if (skillTreeData != null)
					{
						skillTreeData.AddMasteryXp((float)GameMath.GetExperienceRewardValue(this.ore.item.rarity.GetPowerMultiplier(), this.ore.item.itemLevel));
					}
					if (this.ore.item.itemCategory == ItemCategory.Ore && SeededRandom.Global.RandomBool(SkilltreeNode.industrialCrystalRefineChance.currentIncrease))
					{
						InventoryItemType crystalItem = InventoryItemType.GetCrystalItem(SeededRandom.Global);
						if (SpaceStationInterior.instance && MapPointOfInterest.current == this.parent.spaceStation && !GamePlayer.current.currentSpaceShip.cargo.IsFull(crystalItem.m3) && GamePlayer.current.forgeDepositInCargo && !(this.parent.spaceStation is IndustryStation))
						{
							GamePlayer.current.currentSpaceShip.cargo.Add(crystalItem, 1, false, false);
							return;
						}
						if (crystalItem.CanGoInArmory())
						{
							GamePlayer.current.globalInventory.Add(crystalItem, 1, false, false);
							return;
						}
						if (crystalItem.CanGoInMaterials())
						{
							this.parent.spaceStation.materialStorage.Add(crystalItem, 1, false, false);
						}
					}
				}
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x000462BC File Offset: 0x000444BC
		public void CancelJob()
		{
			Refinery.current.CancelJob(this);
			if (SpaceStationInterior.instance.currentTab == SpaceStationFacility.Refinery)
			{
				SpaceStationInterior.instance.Refresh();
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x000462E0 File Offset: 0x000444E0
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"ore",
					this.ore.item.identifier
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
				}
			};
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00046370 File Offset: 0x00044570
		public static RefineryJob FromJson(Refinery parent, JsonValue json)
		{
			if (!json.IsJsonObject)
			{
				return null;
			}
			return new RefineryJob(parent, json["ore"].AsString, json["initialAmount"])
			{
				remainingAmount = json["remainingAmount"],
				progress = (float)json["progress"].AsNumber
			};
		}

		// Token: 0x040004A9 RID: 1193
		private float progress;

		// Token: 0x040004AB RID: 1195
		private Refinery parent;
	}
}
