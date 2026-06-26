using System;
using System.Collections.Generic;
using Behaviour.Crew;
using Behaviour.Item;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Refinery;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using LightJson;
using Source.Crew;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Source.Mining
{
	// Token: 0x020000E9 RID: 233
	public class Refinery : IJsonSource
	{
		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00045854 File Offset: 0x00043A54
		public static float refinedPerSecond
		{
			get
			{
				float num = 0.5f;
				float num2 = 1f;
				num2 += SkilltreeNode.industrialT1CraftingSpeed.currentIncrease;
				num2 += SkilltreeNode.industrialCraftingSpeed2.currentIncrease;
				Skilltree tree = Skilltree.Get(SkillTreeData.GetSpecializationTreeName(CommanderSpecialization.Engineering));
				int masteryLevel = GamePlayer.current.commander.GetSkillTreeData(tree, false).masteryLevel;
				num2 += (float)masteryLevel * 0.005f;
				return num * num2;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000458B6 File Offset: 0x00043AB6
		public static Refinery current
		{
			get
			{
				SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
				if (spaceStation == null)
				{
					return null;
				}
				return spaceStation.refinery;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x000458CD File Offset: 0x00043ACD
		public int maxJobs
		{
			get
			{
				return 2 + SkilltreeNode.industrialBasicJobs.currentPoints + SkilltreeNode.industrialEnhancedJobs.currentPoints;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060008DF RID: 2271 RVA: 0x000458E6 File Offset: 0x00043AE6
		public float storageMax
		{
			get
			{
				return 10000f;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x000458ED File Offset: 0x00043AED
		private bool cargoAccessible
		{
			get
			{
				return MapPointOfInterest.current == this.spaceStation && SpaceStationInterior.instance;
			}
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00045908 File Offset: 0x00043B08
		public Refinery(SpaceStation spaceStation)
		{
			this.spaceStation = spaceStation;
			int maxJobs = this.maxJobs;
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0004592C File Offset: 0x00043B2C
		public void ProgressJobs(float deltaTime)
		{
			List<RefineryJob> list = new List<RefineryJob>();
			if (GamePlayer.current.autoPlay)
			{
				deltaTime *= 1f + SkilltreeNode.promptRefinerySpeed.currentIncrease;
			}
			foreach (RefineryJob refineryJob in this.jobs)
			{
				refineryJob.ProgressJob(deltaTime);
				if (refineryJob.remainingAmount <= 0)
				{
					list.Add(refineryJob);
					if (refineryJob.initialAmount > 1 && !SpaceStationInterior.instance)
					{
						EventLogManager instance = Singleton<EventLogManager>.Instance;
						string str = "RefineJobFinished";
						OreItemData ore = refineryJob.ore;
						instance.NewEvent(str + ((ore != null) ? ore.ToString() : null), Translation.Translate("@LogRefineryJobFinished", new object[]
						{
							refineryJob.ore.item.displayName,
							this.spaceStation.name
						}));
					}
				}
			}
			foreach (RefineryJob item in list)
			{
				this.jobs.Remove(item);
			}
			if (this.autoRefine && this.jobs.Count < this.maxJobs)
			{
				this.autoRefineTimer -= deltaTime;
				if (this.autoRefineTimer < 0f)
				{
					InventoryInteractionManager instance2 = InventoryInteractionManager.Instance;
					if (!((instance2 != null) ? instance2.selectedItem : null))
					{
						OreItemData oreItemData = null;
						foreach (KeyValuePair<OreItemData, int> keyValuePair in this.GetAvailableItems(false))
						{
							if (!keyValuePair.Key.disableAutoRefine && GamePlayer.current.CanAfford((float)keyValuePair.Key.refinementCost))
							{
								oreItemData = keyValuePair.Key;
								break;
							}
						}
						if (oreItemData != null)
						{
							this.StartJob(oreItemData, 1);
							if (SpaceStationInterior.instance)
							{
								SpaceStationInterior.instance.UpdateJobs();
							}
						}
						this.autoRefineTimer = 4f;
					}
				}
			}
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00045B6C File Offset: 0x00043D6C
		public bool TryStartJob(OreItemData ore, int amount)
		{
			if (this.jobs.Count < this.maxJobs)
			{
				return this.StartJob(ore, amount);
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSRefineryFull", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
			return false;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00045BC0 File Offset: 0x00043DC0
		public bool StartJob(OreItemData ore, int amount)
		{
			float amount2 = (float)(ore.refinementCost * amount);
			if (!GamePlayer.current.CanAfford(amount2))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			if (!this.ConsumeItem(ore, amount))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoItems", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			GamePlayer.current.RemoveCredits(amount2);
			RefineryJob item = new RefineryJob(this, ore, amount);
			this.jobs.Add(item);
			return true;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00045C64 File Offset: 0x00043E64
		public void CancelJob(RefineryJob job)
		{
			this.spaceStation.materialStorage.Add(job.ore.item, job.remainingAmount, false, false);
			GamePlayer.current.credits += (long)(job.ore.refinementCost * job.remainingAmount);
			this.jobs.Remove(job);
			if (RefineryUI.current)
			{
				RefineryUI.current.UpdateContent();
			}
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00045CDC File Offset: 0x00043EDC
		public bool ConsumeRefinedMaterial(RefinedMaterial mat, float amt)
		{
			return GamePlayer.current.ConsumeRefinedMaterial(mat, amt);
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x00045CEA File Offset: 0x00043EEA
		public void AddRefinedMaterial(RefinedMaterial mat, float amt)
		{
			GamePlayer.current.AddRefinedMaterial(mat, amt);
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00045CF8 File Offset: 0x00043EF8
		public IEnumerable<KeyValuePair<OreItemData, int>> GetAvailableItems(bool inlcudeRequiredForMissions = true)
		{
			foreach (Inventory.InventoryItem inventoryItem in this.spaceStation.materialStorage.items)
			{
				OreItemData component = inventoryItem.item.GetComponent<OreItemData>();
				foreach (KeyValuePair<OreItemData, int> keyValuePair in Refinery.GetOreKeyValuePairs(component, inventoryItem, inlcudeRequiredForMissions))
				{
					yield return keyValuePair;
				}
				IEnumerator<KeyValuePair<OreItemData, int>> enumerator2 = null;
			}
			IEnumerator<Inventory.InventoryItem> enumerator = null;
			if (this.cargoAccessible)
			{
				foreach (Inventory.InventoryItem inventoryItem2 in GamePlayer.current.currentSpaceShip.cargo.items)
				{
					OreItemData component2 = inventoryItem2.item.GetComponent<OreItemData>();
					foreach (KeyValuePair<OreItemData, int> keyValuePair2 in Refinery.GetOreKeyValuePairs(component2, inventoryItem2, inlcudeRequiredForMissions))
					{
						yield return keyValuePair2;
					}
					IEnumerator<KeyValuePair<OreItemData, int>> enumerator2 = null;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00045D0F File Offset: 0x00043F0F
		private static IEnumerable<KeyValuePair<OreItemData, int>> GetOreKeyValuePairs(OreItemData ore, Inventory.InventoryItem kv, bool includeRequiredForMissions)
		{
			if (!ore)
			{
				yield break;
			}
			int num = includeRequiredForMissions ? 0 : GamePlayer.current.RequiredItemCountForMissions(ore.item);
			if (ore && kv.count > 0 && num < kv.count)
			{
				yield return KeyValuePair.Create<OreItemData, int>(ore, kv.count - num);
			}
			yield break;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00045D30 File Offset: 0x00043F30
		private bool ConsumeItem(OreItemData item, int count)
		{
			int num = 0;
			foreach (KeyValuePair<OreItemData, int> keyValuePair in this.GetAvailableItems(true))
			{
				if (keyValuePair.Key == item)
				{
					num += keyValuePair.Value;
				}
			}
			if (num < count)
			{
				return false;
			}
			if (this.cargoAccessible)
			{
				count -= GamePlayer.current.currentSpaceShip.cargo.Remove(item.item, count);
			}
			if (count > 0)
			{
				this.spaceStation.materialStorage.Remove(item.item, count);
			}
			return true;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00045DDC File Offset: 0x00043FDC
		public static int GetExtractCost(RefinedMaterial material, int count)
		{
			return Mathf.FloorToInt((float)count * material.GetValue() / 100f);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00045DF4 File Offset: 0x00043FF4
		public void ExtractMaterial(RefinedMaterial material, int count)
		{
			int num = Mathf.FloorToInt(GamePlayer.current.CountRefinedMaterial(material));
			if (count > num)
			{
				return;
			}
			int extractCost = Refinery.GetExtractCost(material, count);
			if (!GamePlayer.current.CanAfford((float)extractCost))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return;
			}
			if (this.ConsumeRefinedMaterial(material, (float)count))
			{
				GamePlayer.current.RemoveCredits((float)extractCost);
				InventoryItemType item = InventoryItemType.Get("Canister" + material.ToString());
				GamePlayer.current.currentSpaceShip.AddCargo(item, count, true);
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00045EA0 File Offset: 0x000440A0
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"jobs",
					this.jobs.ToJsonArray<RefineryJob>()
				},
				{
					"autoRefine",
					new bool?(this.autoRefine)
				}
			};
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00045EF0 File Offset: 0x000440F0
		public static Refinery FromJson(SpaceStation parent, JsonValue json)
		{
			Refinery refinery = new Refinery(parent);
			if (json2["jobs"].IsJsonArray)
			{
				refinery.jobs.FromJsonArray(json2["jobs"], (JsonValue json) => RefineryJob.FromJson(refinery, json));
			}
			if (json2["refinedStorage"].IsJsonArray)
			{
				float[] array = JsonUtil.FloatArrayFromJson(json2["refinedStorage"]);
				for (int i = 0; i < array.Length; i++)
				{
					GamePlayer.current.AddRefinedMaterial((RefinedMaterial)i, array[i]);
				}
			}
			refinery.autoRefine = json2["autoRefine"];
			return refinery;
		}

		// Token: 0x040004A1 RID: 1185
		public static bool autoSell = Register.HasFlag("AutoSell", false);

		// Token: 0x040004A2 RID: 1186
		public bool autoRefine;

		// Token: 0x040004A3 RID: 1187
		public List<RefineryJob> jobs = new List<RefineryJob>();

		// Token: 0x040004A4 RID: 1188
		public readonly SpaceStation spaceStation;

		// Token: 0x040004A5 RID: 1189
		private float autoRefineTimer;
	}
}
