using System;
using System.Collections.Generic;
using Behaviour.Crafting;
using Behaviour.Equipment;
using Behaviour.Equipment.Builder;
using Behaviour.Equipment.Turret;
using Behaviour.GalaxyMap;
using Behaviour.Item;
using Behaviour.Item.Builder;
using Behaviour.Item.Usable;
using Behaviour.Spacestation.Docking;
using Behaviour.UI;
using Behaviour.UI.Spacestation;
using Behaviour.Unit;
using LightJson;
using Source.Data;
using Source.Data.Persistable;
using Source.Dialogues;
using Source.Galaxy.POI.Station;
using Source.Item;
using Source.Mining;
using Source.Player;
using Source.Simulation;
using Source.Simulation.Economy;
using Source.Simulation.World.System;
using Source.Spacestation;
using Source.Util;
using UnityEngine;

namespace Source.Galaxy.POI
{
	// Token: 0x0200015C RID: 348
	public class SpaceStation : MapPointOfInterest
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0005E4AA File Offset: 0x0005C6AA
		public new static SpaceStation current
		{
			get
			{
				return MapPointOfInterest.current as SpaceStation;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0005E4B6 File Offset: 0x0005C6B6
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("SpaceStation");
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0005E4C2 File Offset: 0x0005C6C2
		public override string sceneName
		{
			get
			{
				return "Spacestation";
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x0005E4C9 File Offset: 0x0005C6C9
		public override bool storeLastX
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000D35 RID: 3381 RVA: 0x0005E4CC File Offset: 0x0005C6CC
		// (set) Token: 0x06000D36 RID: 3382 RVA: 0x0005E4D4 File Offset: 0x0005C6D4
		public float shopRefreshTime { get; private set; } = -1f;

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000D37 RID: 3383 RVA: 0x0005E4DD File Offset: 0x0005C6DD
		public override bool hasCombatMusic
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000D38 RID: 3384 RVA: 0x0005E4E0 File Offset: 0x0005C6E0
		public virtual bool canBeHomeStation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0005E4E4 File Offset: 0x0005C6E4
		public ShopInventory shopInventory
		{
			get
			{
				ShopInventory result;
				if ((result = this.generalShopInventory) == null && (result = this.miningShopInventory) == null && (result = this.salvageShopInventory) == null && (result = this.bountyShopInventory) == null && (result = this.patrolShopInventory) == null)
				{
					result = (this.industryShopInventory ?? this.conquestShopInventory);
				}
				return result;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0005E534 File Offset: 0x0005C734
		public float umbralControlLevel
		{
			get
			{
				if (this.faction == Faction.puppeteers && this.system.sector.quadrant == 2)
				{
					return 1f;
				}
				if (!(this is ConquestStation))
				{
					return 0f;
				}
				ConquestSystem conquestSystem = this.system.storyteller as ConquestSystem;
				if (conquestSystem == null)
				{
					return 0f;
				}
				return conquestSystem.umbralControlLevel;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x0005E594 File Offset: 0x0005C794
		public virtual IEnumerable<CraftingRecipe> recipes
		{
			get
			{
				return CraftingRecipe.GetAvailable();
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x0005E59C File Offset: 0x0005C79C
		public void SetFacilities(HashSet<SpaceStationFacility> facilities)
		{
			if (facilities.Contains(SpaceStationFacility.GeneralShop))
			{
				this.generalShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.GeneralShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.MiningShop))
			{
				this.miningShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.MiningShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.SalvageShop))
			{
				this.salvageShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.SalvageShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.Bar))
			{
				this.bar = new Bar(this);
			}
			if (facilities.Contains(SpaceStationFacility.Refinery))
			{
				this.refinery = new Refinery(this);
			}
			if (facilities.Contains(SpaceStationFacility.Refinery))
			{
				this.forge = new Forge(this);
			}
			if (facilities.Contains(SpaceStationFacility.Shipyard))
			{
				this.shipyard = new Shipyard();
			}
			if (facilities.Contains(SpaceStationFacility.MissionBoard))
			{
				this.missionBoard = new Source.Galaxy.POI.Station.MissionBoard(this);
			}
			if (facilities.Contains(SpaceStationFacility.TradeTerminal))
			{
				this.economy = new LocalEconomy(this);
			}
			if (facilities.Contains(SpaceStationFacility.BountyBoard))
			{
				this.bountyBoard = new BountyBoard(this);
			}
			if (facilities.Contains(SpaceStationFacility.PoliceBoard))
			{
				this.patrolBoard = new PatrolBoard(this);
			}
			if (facilities.Contains(SpaceStationFacility.BountyShop))
			{
				this.bountyShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.BountyShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.PatrolShop))
			{
				this.patrolShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.PatrolShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.IndustryShop))
			{
				this.industryShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.IndustryShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.ConquestShop))
			{
				this.conquestShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.ConquestShop
				};
			}
			if (facilities.Contains(SpaceStationFacility.IndustryBoard))
			{
				this.industryBoard = new IndustryBoard(this);
			}
			if (facilities.Contains(SpaceStationFacility.SalvageWorkshop))
			{
				this.salvageWorkshop = new SalvageWorkshop(this);
			}
			if (facilities.Contains(SpaceStationFacility.RecruitmentCenter))
			{
				this.recruitmentCenter = new RecruitmentCenter(this);
			}
			this.personalHangar = new PersonalHangar(this);
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0005E770 File Offset: 0x0005C970
		public virtual bool HasFacility(SpaceStationFacility facility)
		{
			switch (facility)
			{
			case SpaceStationFacility.GeneralShop:
				return this.generalShopInventory != null;
			case SpaceStationFacility.MiningShop:
				return this.miningShopInventory != null;
			case SpaceStationFacility.TradeTerminal:
				return this.economy != null;
			case SpaceStationFacility.BountyBoard:
				return this.bountyBoard != null;
			case SpaceStationFacility.PoliceBoard:
				return this.patrolBoard != null;
			case SpaceStationFacility.Refinery:
				return this.refinery != null;
			case SpaceStationFacility.Forge:
				return this.forge != null;
			case SpaceStationFacility.Bar:
				return this.bar != null;
			case SpaceStationFacility.Shipyard:
				return this.shipyard != null;
			case SpaceStationFacility.MissionBoard:
				return this.missionBoard != null;
			case SpaceStationFacility.PersonalHangar:
				return true;
			case SpaceStationFacility.SalvageShop:
				return this.salvageShopInventory != null;
			case SpaceStationFacility.BountyShop:
				return this.bountyShopInventory != null;
			case SpaceStationFacility.PatrolShop:
				return this.patrolShopInventory != null;
			case SpaceStationFacility.IndustryShop:
				return this.industryShopInventory != null;
			case SpaceStationFacility.ConquestShop:
				return this.conquestShopInventory != null;
			case SpaceStationFacility.IndustryBoard:
				return this.industryBoard != null;
			case SpaceStationFacility.SalvageWorkshop:
				return this.salvageWorkshop != null;
			case SpaceStationFacility.RecruitmentCenter:
				return this.recruitmentCenter != null;
			}
			return false;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0005E8DC File Offset: 0x0005CADC
		public void SetStationSeed()
		{
			this.stationSeed = this.stationSize.ToString() + SeededRandom.Global.RandomRange(0, 7).ToString();
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0005E919 File Offset: 0x0005CB19
		public SpaceStation WithSize(SpaceStation.StationSize size)
		{
			this.stationSize = size;
			return this;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x0005E923 File Offset: 0x0005CB23
		public SpaceStation WithCharacters(List<string> characters)
		{
			this.characters = characters;
			return this;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0005E92D File Offset: 0x0005CB2D
		public override Vector2 GetLocalOffset()
		{
			if (GameplayManager.Instance.spaceShip.rigidbody.position.x > base.GetWorldPosition().x)
			{
				return this.rightWarpInPosition;
			}
			return this.leftWarpInPosition;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0005E962 File Offset: 0x0005CB62
		public override void ActiveUpdate(float delta)
		{
			base.ActiveUpdate(delta);
			this.UpdateJobs(delta);
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x0005E972 File Offset: 0x0005CB72
		public override void AmbientUpdate(float delta)
		{
			base.AmbientUpdate(delta);
			if (MapPointOfInterest.current != this)
			{
				this.UpdateJobs(delta);
			}
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x0005E98C File Offset: 0x0005CB8C
		private void UpdateJobs(float delta)
		{
			Refinery refinery = this.refinery;
			if (refinery != null)
			{
				refinery.ProgressJobs(delta);
			}
			Forge forge = this.forge;
			if (forge != null)
			{
				forge.ProgressJobs(delta);
			}
			Source.Galaxy.POI.Station.MissionBoard missionBoard = this.missionBoard;
			if (missionBoard != null)
			{
				missionBoard.ProgressTimer(delta);
			}
			PersonalHangar personalHangar = this.personalHangar;
			if (personalHangar == null)
			{
				return;
			}
			personalHangar.ProgressJobs(delta);
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x0005E9E0 File Offset: 0x0005CBE0
		public IEnumerable<ISpaceStationJob> GetJobs()
		{
			if (this.personalHangar != null)
			{
				foreach (RepairJob repairJob in this.personalHangar.jobs)
				{
					if (repairJob != null)
					{
						yield return repairJob;
					}
				}
				List<RepairJob>.Enumerator enumerator = default(List<RepairJob>.Enumerator);
			}
			if (this.refinery != null)
			{
				foreach (RefineryJob refineryJob in this.refinery.jobs)
				{
					yield return refineryJob;
				}
				List<RefineryJob>.Enumerator enumerator2 = default(List<RefineryJob>.Enumerator);
			}
			if (this.forge != null)
			{
				foreach (ForgeJob forgeJob in this.forge.jobs)
				{
					yield return forgeJob;
				}
				List<ForgeJob>.Enumerator enumerator3 = default(List<ForgeJob>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x0005E9F0 File Offset: 0x0005CBF0
		public IEnumerable<ISpaceStationJob> GetRepairJobs()
		{
			if (this.personalHangar != null)
			{
				foreach (RepairJob repairJob in this.personalHangar.jobs)
				{
					if (repairJob != null)
					{
						yield return repairJob;
					}
				}
				List<RepairJob>.Enumerator enumerator = default(List<RepairJob>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x0005EA00 File Offset: 0x0005CC00
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			if (this.materialStorage.count > 0)
			{
				List<string> list = new List<string>();
				float num = 0f;
				foreach (Inventory.InventoryItem inventoryItem in this.materialStorage.items)
				{
					num += inventoryItem.item.m3 * (float)inventoryItem.count;
					list.Add(Translation.Translate(inventoryItem.item.displayName, Array.Empty<object>()));
				}
				tooltip.AddTextLine(Translation.Translate("@SSMaterialsStored", new object[]
				{
					num
				}), 12, 8f);
				if (list.Count > 3)
				{
					while (list.Count > 3)
					{
						list.RemoveAt(3);
					}
					list.Add("...");
				}
				tooltip.AddTextLine(string.Join(", ", list), 12, 8f);
				tooltip.AddSeparator(null);
			}
			tooltip.AddTextLine(Translation.Translate("@Facilities", Array.Empty<object>()) + ":", 12, 8f);
			int childCount = tooltip.Content.childCount;
			if (this.generalShopInventory != null)
			{
				tooltip.AddTextLine("General Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.miningShopInventory != null)
			{
				tooltip.AddTextLine("Mining Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.salvageShopInventory != null)
			{
				tooltip.AddTextLine("Salvage Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.bountyShopInventory != null)
			{
				tooltip.AddTextLine("Bounty Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.patrolShopInventory != null)
			{
				tooltip.AddTextLine("Patrol Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.industryShopInventory != null)
			{
				tooltip.AddTextLine("Industry Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.conquestShopInventory != null)
			{
				tooltip.AddTextLine("Conquest Shop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.economy != null)
			{
				tooltip.AddTextLine("Trade Terminal", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.refinery != null)
			{
				tooltip.AddTextLine("Refinery", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.forge != null)
			{
				tooltip.AddTextLine("Forge", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.shipyard != null)
			{
				tooltip.AddTextLine("Shipyard", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.missionBoard != null)
			{
				tooltip.AddTextLine("Mission Board", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.bountyBoard != null)
			{
				tooltip.AddTextLine("Bounty Board", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.patrolBoard != null)
			{
				tooltip.AddTextLine("Patrol Board", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.industryBoard != null)
			{
				tooltip.AddTextLine("Industry Board", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.salvageWorkshop != null)
			{
				tooltip.AddTextLine("Workshop", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.recruitmentCenter != null)
			{
				tooltip.AddTextLine("Recruitment Center", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (this.bar != null)
			{
				tooltip.AddTextLine("Bar", 12, 8f).Text.color = ColorHelper.detailsColor;
			}
			if (childCount == tooltip.Content.childCount)
			{
				tooltip.AddTextLine("(none)", 12, 8f).Text.color = ColorHelper.boringGrey;
			}
			bool flag = false;
			foreach (string text in this.system.GetAvailableMissionHints(this))
			{
				if (!flag)
				{
					tooltip.AddSeparator(ColorHelper.boringGrey, 2f, 0f, 8f);
					tooltip.AddTextLine("@MapPOIAvailableMission", 12, 8f);
					flag = true;
				}
				tooltip.AddTextLine(text, 12, 8f).Text.color = ColorHelper.detailsColor;
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0005EEEC File Offset: 0x0005D0EC
		public int ConsumeAvailableItems(InventoryItemType item, int toConsume)
		{
			int num = this.materialStorage.Remove(item, toConsume);
			if (num == toConsume)
			{
				return num;
			}
			toConsume -= num;
			int num2 = GamePlayer.current.globalInventory.Remove(item, toConsume);
			if (num2 == toConsume)
			{
				return num + num2;
			}
			toConsume -= num2;
			if (GamePlayer.current.currentPointOfInterest == this)
			{
				int num3 = GamePlayer.current.currentSpaceShip.cargo.Remove(item, toConsume);
				return num + num2 + num3;
			}
			return num + num2;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0005EF60 File Offset: 0x0005D160
		public int CountAvailableItems(InventoryItemType item)
		{
			if (GamePlayer.current == null)
			{
				return 0;
			}
			int num = this.materialStorage.GetCount(item) + GamePlayer.current.globalInventory.GetCount(item);
			if (MapPointOfInterest.current != null && MapPointOfInterest.current == this)
			{
				num += GamePlayer.current.currentSpaceShip.cargo.GetCount(item);
			}
			return num;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x0005EFBC File Offset: 0x0005D1BC
		public override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["refreshTime"] = new double?((double)this.shopRefreshTime);
			data["stationSize"] = this.stationSize.ToString();
			data["stationVariant"] = this.stationVariant.ToString();
			if (this.generalShopInventory != null)
			{
				data["shopInventory"] = this.generalShopInventory.ToJson();
			}
			if (this.miningShopInventory != null)
			{
				data["miningShopInventory"] = this.miningShopInventory.ToJson();
			}
			if (this.salvageShopInventory != null)
			{
				data["salvageShopInventory"] = this.salvageShopInventory.ToJson();
			}
			if (this.bountyShopInventory != null)
			{
				data["bountyShopInventory"] = this.bountyShopInventory.ToJson();
			}
			if (this.patrolShopInventory != null)
			{
				data["patrolShopInventory"] = this.patrolShopInventory.ToJson();
			}
			if (this.industryShopInventory != null)
			{
				data["industryShopInventory"] = this.industryShopInventory.ToJson();
			}
			if (this.conquestShopInventory != null)
			{
				data["conquestShopInventory"] = this.conquestShopInventory.ToJson();
			}
			if (this.umbralShopInventory != null)
			{
				data["umbralShopInventory"] = this.umbralShopInventory.ToJson();
			}
			if (this.materialStorage.count > 0)
			{
				data["materialStorage"] = this.materialStorage.ToJson();
			}
			if (this.refinery != null)
			{
				data["refinery"] = this.refinery.ToJson();
			}
			if (this.forge != null)
			{
				data["forge"] = this.forge.ToJson();
			}
			if (this.shipyard != null)
			{
				data["shipyard"] = this.shipyard.ToJson();
			}
			if (this.missionBoard != null)
			{
				data["missionBoard"] = this.missionBoard.ToJson();
			}
			if (this.bar != null)
			{
				data["bar"] = this.bar.ToJson();
			}
			if (this.characters != null && this.characters.Count > 0)
			{
				data["characters"] = this.characters.ToJsonArray();
			}
			if (this.personalHangar != null)
			{
				data["personalHangar"] = this.personalHangar.ToJson();
			}
			if (this.economy != null)
			{
				data["economy"] = this.economy.ToJson();
			}
			if (this.bountyBoard != null)
			{
				data["bountyBoard"] = this.bountyBoard.ToJson();
			}
			if (this.patrolBoard != null)
			{
				data["patrolBoard"] = this.patrolBoard.ToJson();
			}
			if (this.industryBoard != null)
			{
				data["industryBoard"] = this.industryBoard.ToJson();
			}
			if (this.salvageWorkshop != null)
			{
				data["salvageWorkshop"] = this.salvageWorkshop.ToJson();
			}
			if (this.recruitmentCenter != null)
			{
				data["recruitmentCenter"] = this.recruitmentCenter.ToJson();
			}
			if (this.stationSeed != null)
			{
				data["stationSeed"] = this.stationSeed;
			}
			if (this.conquestStationInitialized)
			{
				data["conquestStation"] = new bool?(this.conquestStationInitialized);
			}
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x0005F31C File Offset: 0x0005D51C
		public override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data2);
			this.shopRefreshTime = (float)data2["refreshTime"];
			if (data2.ContainsKey("shopInventory"))
			{
				this.generalShopInventory = (Inventory.FromJson(data2["shopInventory"], this, false) as ShopInventory);
				this.generalShopInventory.facility = SpaceStationFacility.GeneralShop;
			}
			if (data2.ContainsKey("miningShopInventory"))
			{
				this.miningShopInventory = (Inventory.FromJson(data2["miningShopInventory"], this, false) as ShopInventory);
				this.miningShopInventory.facility = SpaceStationFacility.MiningShop;
			}
			if (data2.ContainsKey("salvageShopInventory"))
			{
				this.salvageShopInventory = (Inventory.FromJson(data2["salvageShopInventory"], this, false) as ShopInventory);
				this.salvageShopInventory.facility = SpaceStationFacility.SalvageShop;
			}
			if (data2.ContainsKey("bountyShopInventory"))
			{
				this.bountyShopInventory = (Inventory.FromJson(data2["bountyShopInventory"], this, false) as ShopInventory);
				this.bountyShopInventory.facility = SpaceStationFacility.BountyShop;
			}
			if (data2.ContainsKey("patrolShopInventory"))
			{
				this.patrolShopInventory = (Inventory.FromJson(data2["patrolShopInventory"], this, false) as ShopInventory);
				this.patrolShopInventory.facility = SpaceStationFacility.PatrolShop;
			}
			if (data2.ContainsKey("industryShopInventory"))
			{
				this.industryShopInventory = (Inventory.FromJson(data2["industryShopInventory"], this, false) as ShopInventory);
				this.industryShopInventory.facility = SpaceStationFacility.IndustryShop;
			}
			if (data2.ContainsKey("conquestShopInventory"))
			{
				this.conquestShopInventory = (Inventory.FromJson(data2["conquestShopInventory"], this, false) as ShopInventory);
				this.conquestShopInventory.facility = SpaceStationFacility.ConquestShop;
			}
			if (data2.ContainsKey("umbralShopInventory"))
			{
				this.umbralShopInventory = (Inventory.FromJson(data2["umbralShopInventory"], this, false) as ShopInventory);
				this.umbralShopInventory.facility = SpaceStationFacility.GeneralShop;
			}
			if (data2.ContainsKey("materialStorage"))
			{
				this.materialStorage = Inventory.FromJson(data2["materialStorage"], null, true);
			}
			if (data2.ContainsKey("refinery"))
			{
				this.refinery = Refinery.FromJson(this, data2["refinery"]);
			}
			if (data2.ContainsKey("forge"))
			{
				this.forge = Forge.FromJson(this, data2["forge"]);
			}
			if (data2.ContainsKey("shipyard"))
			{
				this.shipyard = Shipyard.FromJson(data2["shipyard"]);
			}
			if (data2.ContainsKey("missionBoard"))
			{
				this.missionBoard = Source.Galaxy.POI.Station.MissionBoard.FromJson(this, data2["missionBoard"]);
			}
			if (data2.ContainsKey("bar"))
			{
				this.bar = Bar.FromJson(this, data2["bar"]);
			}
			if (data2.ContainsKey("personalHangar"))
			{
				this.personalHangar = PersonalHangar.FromJson(this, data2["personalHangar"]);
			}
			if (data2.ContainsKey("characters"))
			{
				this.characters.FromJsonArray(data2["characters"], (JsonValue data) => data.AsString);
			}
			if (data2.ContainsKey("stationSize"))
			{
				this.stationSize = Enum.Parse<SpaceStation.StationSize>(data2["stationSize"]);
			}
			if (data2.ContainsKey("stationVariant"))
			{
				this.stationVariant = Enum.Parse<SpaceStation.StationVariants>(data2["stationVariant"]);
			}
			else
			{
				this.stationVariant = SeededRandom.Global.ChooseEnum<SpaceStation.StationVariants>(0);
			}
			if (data2.ContainsKey("economy"))
			{
				this.economy = LocalEconomy.FromJson(data2["economy"], this);
			}
			if (data2.ContainsKey("bountyBoard"))
			{
				this.bountyBoard = BountyBoard.FromJson(data2["bountyBoard"], this);
			}
			if (data2.ContainsKey("patrolBoard"))
			{
				this.patrolBoard = PatrolBoard.FromJson(data2["patrolBoard"], this);
			}
			if (data2.ContainsKey("industryBoard"))
			{
				this.industryBoard = IndustryBoard.FromJson(data2["industryBoard"], this);
			}
			if (data2.ContainsKey("salvageWorkshop"))
			{
				this.salvageWorkshop = SalvageWorkshop.FromJson(data2["salvageWorkshop"], this);
			}
			if (data2.ContainsKey("recruitmentCenter"))
			{
				this.recruitmentCenter = RecruitmentCenter.FromJson(data2["recruitmentCenter"], this);
			}
			if (data2.ContainsKey("stationSeed"))
			{
				this.stationSeed = data2["stationSeed"];
			}
			if (data2.ContainsKey("conquestStation"))
			{
				this.conquestStationInitialized = data2["conquestStation"].AsBoolean;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x0005F81D File Offset: 0x0005DA1D
		private void AddAmmoToShop(ShopInventory shop)
		{
			this.AddItemsToShop(shop, ItemCategory.Ammo, (InventoryItemType type) => Math.Min(InventoryItemType.GetMagSizeForAmmo(type) * 3000, 90000), null, true);
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x0005F848 File Offset: 0x0005DA48
		public void GenerateShopInventory()
		{
			SeededRandom global = SeededRandom.Global;
			this.generalShopInventory.Clear();
			this.AddPermanentItemsToShop(this.generalShopInventory);
			this.AddStorytellerItemsToShop(this.generalShopInventory);
			this.AddBlueprintsToShop(this.generalShopInventory, 2, null);
			List<EquipmentBuilder> itemsForGeneralShop = EquipmentBuilder.GetItemsForGeneralShop(this.level);
			if (this.level <= 3)
			{
				EquipmentBuilder equipmentBuilder = EquipmentBuilder.Get("JunkGattlingTurret");
				itemsForGeneralShop.Remove(equipmentBuilder);
				this.AddItemToShop(this.generalShopInventory, equipmentBuilder, null, false, 0);
			}
			this.AddEquipmentToShop(this.generalShopInventory, itemsForGeneralShop, 3);
			ItemBuilder itemBuilder;
			InventoryItemType item;
			if (global.RandomBool(0.5f))
			{
				for (int i = 0; i < 2; i++)
				{
					itemBuilder = ItemBuilder.Get("DepMissileTurret");
					item = itemBuilder.CreateDefensiveTurret(this.level);
					this.generalShopInventory.Add(item, 1, false, false);
				}
			}
			itemBuilder = ItemBuilder.Get("ExplosiveMine");
			item = itemBuilder.CreateExplosiveMineItem(this.level);
			this.generalShopInventory.Add(item, 10, false, false);
			this.AddAmmoToShop(this.generalShopInventory);
			foreach (string id in new List<string>
			{
				"Combat Power I",
				"Power I"
			})
			{
				if (global.RandomBool(0.5f))
				{
					this.generalShopInventory.AddToShop(id, 1, 0f);
				}
			}
			itemBuilder = ItemBuilder.Get("WarpFuel");
			for (int j = 0; j < 3; j++)
			{
				if (this.level > 2 && global.RandomBool(0.5f))
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f);
				}
				else
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				}
				this.generalShopInventory.Add(item, 1, false, false);
			}
			if (this.faction == Faction.policeGuild)
			{
				this.AddJumpgatePassToShop(this.generalShopInventory);
			}
			if (this.faction == Faction.policeGuild || global.RandomBool(0.5f))
			{
				for (int k = 0; k < 2; k++)
				{
					itemBuilder = ItemBuilder.Get("DepShieldPylon");
					item = itemBuilder.CreateDefensiveTurret(this.level);
					this.generalShopInventory.Add(item, 1, false, false);
				}
			}
			if (this.faction == Faction.bountyGuild || global.RandomBool(0.5f))
			{
				for (int l = 0; l < 2; l++)
				{
					itemBuilder = ItemBuilder.Get("DepTauntPylon");
					item = itemBuilder.CreateDefensiveTurret(this.level);
					this.generalShopInventory.Add(item, 1, false, false);
				}
			}
			this.generalShopInventory.AddToShop("PoiBeacon", SeededRandom.Global.RandomRange(2, 5), 0f);
			this.generalShopInventory.SortByCategory();
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x0005FB20 File Offset: 0x0005DD20
		public void GenerateMiningShopInventory()
		{
			this.miningShopInventory.Clear();
			this.AddPermanentItemsToShop(this.miningShopInventory);
			this.AddStorytellerItemsToShop(this.miningShopInventory);
			this.AddBlueprintsToShop(this.miningShopInventory, 2, null);
			List<EquipmentBuilder> itemsWithStat = EquipmentBuilder.GetItemsWithStat(this.miningStats, this.level);
			if (this.level <= 3)
			{
				EquipmentBuilder equipmentBuilder = EquipmentBuilder.Get("JunkSalvageLaser");
				itemsWithStat.Remove(equipmentBuilder);
				this.AddItemToShop(this.miningShopInventory, equipmentBuilder, null, false, 0);
				EquipmentBuilder equipmentBuilder2 = EquipmentBuilder.Get("JunkMiningLaser");
				itemsWithStat.Remove(equipmentBuilder2);
				this.AddItemToShop(this.miningShopInventory, equipmentBuilder2, null, false, 0);
			}
			this.AddEquipmentToShop(this.miningShopInventory, itemsWithStat, 3);
			ItemBuilder itemBuilder = ItemBuilder.Get("MiningClaim");
			InventoryItemType item;
			for (int i = 0; i < 2; i++)
			{
				item = itemBuilder.CreateMiningClaim(this.system, null);
				this.miningShopInventory.Add(item, 1, false, false);
			}
			item = itemBuilder.CreateMaterialMiningClaim(this.system, new RefinedMaterial?(RefinedMaterial.Carbon));
			this.miningShopInventory.Add(item, 1, false, false);
			for (int j = 0; j < 2; j++)
			{
				item = itemBuilder.CreateMaterialMiningClaim(this.system, null);
				this.miningShopInventory.Add(item, 1, false, false);
			}
			this.AddAmmoToShop(this.miningShopInventory);
			this.miningShopInventory.AddToShop("Mining Power I", 1, 0f);
			this.miningShopInventory.AddToShop("HazardProtectionBooster", 1, 0f);
			for (int k = 0; k < 2; k++)
			{
				itemBuilder = ItemBuilder.Get("DepMiningTurret");
				item = itemBuilder.CreateDefensiveTurret(this.level);
				this.miningShopInventory.Add(item, 1, false, false);
			}
			this.miningShopInventory.AddToShop("PoiBeacon", SeededRandom.Global.RandomRange(2, 5), 0f);
			this.miningShopInventory.SortByCategory();
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x0005FD24 File Offset: 0x0005DF24
		public void GenerateSalvageShopInventory()
		{
			this.salvageShopInventory.Clear();
			this.AddPermanentItemsToShop(this.salvageShopInventory);
			this.AddStorytellerItemsToShop(this.salvageShopInventory);
			this.AddBlueprintsToShop(this.salvageShopInventory, 2, null);
			List<EquipmentBuilder> list = EquipmentBuilder.GetItemsWithStat(this.salvageStats, this.level);
			if (this.level <= 3)
			{
				EquipmentBuilder equipmentBuilder = EquipmentBuilder.Get("JunkSalvageLaser");
				list.Remove(equipmentBuilder);
				this.AddItemToShop(this.salvageShopInventory, equipmentBuilder, null, true, 0);
				EquipmentBuilder equipmentBuilder2 = EquipmentBuilder.Get("JunkMiningLaser");
				list.Remove(equipmentBuilder2);
				this.AddItemToShop(this.salvageShopInventory, equipmentBuilder2, null, false, 0);
			}
			this.AddEquipmentToShop(this.salvageShopInventory, list, 3);
			if (this.level >= 15)
			{
				list = EquipmentBuilder.GetItemsForGeneralShop(this.level);
				this.AddBlankItemsToShop(this.salvageShopInventory, list, 3);
			}
			for (int i = 0; i < 2; i++)
			{
				InventoryItemType item = ItemBuilder.Get("SalvageClaim").CreateSalvageClaim(this.system, null);
				this.salvageShopInventory.Add(item, 1, false, false);
			}
			this.AddAmmoToShop(this.salvageShopInventory);
			this.salvageShopInventory.AddToShop("Salvage Power I", 1, 0f);
			this.salvageShopInventory.AddToShop("HazardProtectionBooster", 1, 0f);
			this.salvageShopInventory.AddToShop("PoiBeacon", SeededRandom.Global.RandomRange(2, 5), 0f);
			this.salvageShopInventory.SortByCategory();
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0005FEB4 File Offset: 0x0005E0B4
		public void GenerateBountyShopInventory()
		{
			SeededRandom global = SeededRandom.Global;
			this.bountyShopInventory.Clear();
			this.AddPermanentItemsToShop(this.bountyShopInventory);
			this.AddStorytellerItemsToShop(this.bountyShopInventory);
			this.AddBlueprintsToShop(this.bountyShopInventory, 2, "BountyCurrency");
			this.AddCurrencyItemsToShop(this.bountyShopInventory, "BountyCurrency", Math.Max(this.level, GamePlayer.current.maxBountyLevel) + 1, global.RandomRange(8, 10), global.RandomRange(3, 5), global);
			this.AddAmmoToShop(this.bountyShopInventory);
			ItemBuilder itemBuilder = ItemBuilder.Get("WarpFuel");
			for (int i = 0; i < 3; i++)
			{
				InventoryItemType item;
				if (this.level > 2 && global.RandomBool(0.5f))
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f);
				}
				else
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				}
				this.bountyShopInventory.Add(item, 1, false, false);
			}
			for (int j = 0; j < 2; j++)
			{
				itemBuilder = ItemBuilder.Get("DepTauntPylon");
				InventoryItemType item = itemBuilder.CreateDefensiveTurret(this.level);
				this.bountyShopInventory.Add(item, 1, false, false);
			}
			this.bountyShopInventory.AddToShop("BountyCurrency", Mathf.RoundToInt(50f * GameMath.CostMultiplier(this.level)), 0f);
			this.bountyShopInventory.SortByCategory();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00060018 File Offset: 0x0005E218
		public void GeneratePatrolShopInventory()
		{
			SeededRandom global = SeededRandom.Global;
			this.patrolShopInventory.Clear();
			this.AddPermanentItemsToShop(this.patrolShopInventory);
			this.AddStorytellerItemsToShop(this.patrolShopInventory);
			this.AddBlueprintsToShop(this.patrolShopInventory, 2, "PatrolCurrency");
			this.AddCurrencyItemsToShop(this.patrolShopInventory, "PatrolCurrency", Math.Max(this.level, GamePlayer.current.maxPatrolLevel) + 1, global.RandomRange(3, 5), global.RandomRange(8, 10), global);
			this.AddAmmoToShop(this.patrolShopInventory);
			ItemBuilder itemBuilder = ItemBuilder.Get("WarpFuel");
			for (int i = 0; i < 3; i++)
			{
				InventoryItemType item;
				if (this.level > 2 && global.RandomBool(0.5f))
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f);
				}
				else
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				}
				this.patrolShopInventory.Add(item, 1, false, false);
			}
			if (this.faction == Faction.policeGuild)
			{
				this.AddJumpgatePassToShop(this.patrolShopInventory);
			}
			for (int j = 0; j < 2; j++)
			{
				itemBuilder = ItemBuilder.Get("DepShieldPylon");
				InventoryItemType item = itemBuilder.CreateDefensiveTurret(this.level);
				this.patrolShopInventory.Add(item, 1, false, false);
			}
			this.patrolShopInventory.AddToShop("PatrolCurrency", Mathf.RoundToInt(50f * GameMath.CostMultiplier(this.level)), 0f);
			this.patrolShopInventory.SortByCategory();
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00060198 File Offset: 0x0005E398
		public void GenerateIndustryShopInventory()
		{
			SeededRandom global = SeededRandom.Global;
			this.industryShopInventory.Clear();
			this.AddPermanentItemsToShop(this.industryShopInventory);
			string[] array = new string[]
			{
				"RecipeBallisticCrystal",
				"RecipeEnergyCrystal",
				"RecipeKineticCrystal",
				"RecipeModuleCrystal"
			};
			for (int i = 0; i < array.Length; i++)
			{
				CraftingRecipe cr = CraftingRecipe.Get(array[i]);
				if (!GamePlayer.current.blueprints.Contains(cr))
				{
					InventoryItemType item = ItemBuilder.Get("Blueprint").CreateBlueprint(cr);
					this.industryShopInventory.AddToShop(item, 1, 0f);
				}
			}
			this.AddStorytellerItemsToShop(this.industryShopInventory);
			this.AddBlueprintsToShop(this.industryShopInventory, 5, "IndustryCurrency");
			this.AddCurrencyItemsToShop(this.industryShopInventory, "IndustryCurrency", Math.Max(this.level, GamePlayer.current.maxIndustryLevel) + 1, global.RandomRange(5, 8), global.RandomRange(5, 8), global);
			this.AddAmmoToShop(this.industryShopInventory);
			ItemBuilder itemBuilder = ItemBuilder.Get("WarpFuel");
			for (int j = 0; j < 3; j++)
			{
				InventoryItemType item2;
				if (this.level > 2 && global.RandomBool(0.5f))
				{
					item2 = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f);
				}
				else
				{
					item2 = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				}
				this.industryShopInventory.Add(item2, 1, false, false);
			}
			for (int k = 0; k < 2; k++)
			{
				itemBuilder = ItemBuilder.Get("DepShieldPylon");
				InventoryItemType item2 = itemBuilder.CreateDefensiveTurret(this.level);
				this.industryShopInventory.Add(item2, 1, false, false);
			}
			this.industryShopInventory.AddToShop("IndustryCurrency", Mathf.RoundToInt(50f * GameMath.CostMultiplier(this.level)), 0f);
			this.industryShopInventory.SortByCategory();
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00060384 File Offset: 0x0005E584
		public void GenerateConquestShopInventory()
		{
			SeededRandom global = SeededRandom.Global;
			InventoryItemType inventoryItemType = InventoryItemType.Get("ConquestCurrency");
			this.conquestShopInventory.Clear();
			this.AddPermanentItemsToShop(this.conquestShopInventory);
			this.AddStorytellerItemsToShop(this.conquestShopInventory);
			this.AddBlueprintsToShop(this.conquestShopInventory, 2, inventoryItemType);
			if (GamePlayer.current.commander.bonusSkillPoints < GameMath.MaxBonusSkillPoints)
			{
				Inventory.InventoryItem inventoryItem = this.conquestShopInventory.Add("BonusSkillPointTemplate", 1, false, false);
				inventoryItem.costItem = inventoryItemType;
				inventoryItem.costCount = BonusSkillPoint.GetConquestValue();
			}
			this.AddCurrencyItemsToShop(this.conquestShopInventory, inventoryItemType, Mathf.RoundToInt((float)(this.level + 1) + Mathf.Pow((float)this.faction.GetConquestRank(), 2f)), global.RandomRange(5, 8), global.RandomRange(5, 8), global);
			this.AddAmmoToShop(this.conquestShopInventory);
			ItemBuilder itemBuilder = ItemBuilder.Get("WarpFuel");
			InventoryItemType item;
			for (int i = 0; i < 3; i++)
			{
				if (this.level > 2 && global.RandomBool(0.5f))
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.IonCell, 1f);
				}
				else
				{
					item = itemBuilder.CreateWarpFuel(WarpFuelItem.WarpFuelType.PlasmaCell, 1f);
				}
				this.conquestShopInventory.Add(item, 1, false, false);
			}
			itemBuilder = ItemBuilder.Get("DepShieldPylon");
			item = itemBuilder.CreateDefensiveTurret(this.level);
			this.conquestShopInventory.Add(item, 1, false, false);
			itemBuilder = ItemBuilder.Get("DepTauntPylon");
			item = itemBuilder.CreateDefensiveTurret(this.level);
			this.conquestShopInventory.Add(item, 1, false, false);
			this.conquestShopInventory.AddToShop("PoiBeacon", SeededRandom.Global.RandomRange(2, 5), 0f);
			this.conquestShopInventory.AddToShop(inventoryItemType, Mathf.RoundToInt(50f * GameMath.CostMultiplier(this.level)), 0f);
			if (this.faction == Faction.marauders)
			{
				Inventory.InventoryItem inventoryItem2 = this.conquestShopInventory.Add("UmbralCargoScanner", global.RandomRange(4, 9), false, false);
				inventoryItem2.costItem = inventoryItemType;
				inventoryItem2.costCount = 8;
				Inventory.InventoryItem inventoryItem3 = this.conquestShopInventory.Add("UmbralTrackingBeacon", global.RandomRange(4, 9), false, false);
				inventoryItem3.costItem = inventoryItemType;
				inventoryItem3.costCount = 6;
			}
			this.conquestShopInventory.SortByCategory();
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x000605D0 File Offset: 0x0005E7D0
		public void GenerateUmbralShopInventory()
		{
			SeededRandom global = SeededRandom.Global;
			InventoryItemType inventoryItemType = InventoryItemType.Get("ConquestCurrency");
			this.umbralShopInventory.Clear();
			this.AddPermanentItemsToShop(this.umbralShopInventory);
			this.AddStorytellerItemsToShop(this.umbralShopInventory);
			this.AddBlueprintsToShop(this.umbralShopInventory, 2, inventoryItemType);
			Inventory.InventoryItem inventoryItem = this.umbralShopInventory.Add("UmbralTransponder", global.RandomRange(10, 21), false, false);
			inventoryItem.costItem = inventoryItemType;
			inventoryItem.costCount = 22;
			Inventory.InventoryItem inventoryItem2 = this.umbralShopInventory.Add("UmbralHackingTool", 2, false, false);
			inventoryItem2.costItem = inventoryItemType;
			inventoryItem2.costCount = 95;
			Inventory.InventoryItem inventoryItem3 = this.umbralShopInventory.Add("UmbralCargoScanner", global.RandomRange(10, 21), false, false);
			inventoryItem3.costItem = inventoryItemType;
			inventoryItem3.costCount = 8;
			Inventory.InventoryItem inventoryItem4 = this.umbralShopInventory.Add("UmbralTrackingBeacon", global.RandomRange(10, 21), false, false);
			inventoryItem4.costItem = inventoryItemType;
			inventoryItem4.costCount = 6;
			if (GamePlayer.current.commander.bonusSkillPoints < GameMath.MaxBonusSkillPoints)
			{
				Inventory.InventoryItem inventoryItem5 = this.umbralShopInventory.Add("BonusSkillPointTemplate", 1, false, false);
				inventoryItem5.costItem = inventoryItemType;
				inventoryItem5.costCount = BonusSkillPoint.GetConquestValue();
			}
			this.AddCurrencyItemsToShop(this.umbralShopInventory, inventoryItemType, Mathf.RoundToInt((float)(this.level + 1) + Mathf.Pow((float)this.faction.GetConquestRank(), 2f)), global.RandomRange(5, 8), global.RandomRange(5, 8), global);
			this.umbralShopInventory.AddToShop(inventoryItemType, Mathf.RoundToInt(50f * GameMath.CostMultiplier(this.level)), 0f);
			this.umbralShopInventory.SortByCategory();
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00060784 File Offset: 0x0005E984
		public void SwapConquestShop(bool swap)
		{
			if (swap && this.conquestShopInventory == null)
			{
				this.generalShopInventory = null;
				this.miningShopInventory = null;
				this.salvageShopInventory = null;
				this.patrolShopInventory = null;
				this.bountyShopInventory = null;
				this.industryShopInventory = null;
				this.conquestShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.ConquestShop
				};
				return;
			}
			if (!swap && this.conquestShopInventory != null)
			{
				this.conquestShopInventory = null;
				this.generalShopInventory = new ShopInventory(this)
				{
					facility = SpaceStationFacility.GeneralShop
				};
			}
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00060800 File Offset: 0x0005EA00
		public void SwapConquestCommander(bool commander)
		{
			string item;
			if (this.factionCommanders.TryGetValue(this.faction, out item))
			{
				if (commander)
				{
					if (this.characters.Contains(item))
					{
						return;
					}
					this.characters.Add(item);
					return;
				}
				else
				{
					this.characters.Remove(item);
				}
			}
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00060850 File Offset: 0x0005EA50
		private void AddCurrencyItemsToShop(ShopInventory inv, InventoryItemType currency, int itemLevel, int weaponCount, int moduleCount, SeededRandom random)
		{
			List<EquipmentBuilder> itemsForGeneralShop = EquipmentBuilder.GetItemsForGeneralShop(this.level);
			List<EquipmentBuilder> list = new List<EquipmentBuilder>();
			for (int i = 0; i < itemsForGeneralShop.Count; i++)
			{
				if (itemsForGeneralShop[i].slot != EquipmentSlot.Hardpoint)
				{
					list.Add(itemsForGeneralShop[i]);
					itemsForGeneralShop.RemoveAt(i);
					i--;
				}
			}
			int num = 0;
			while (num < weaponCount && itemsForGeneralShop.Count > 0)
			{
				Rarity rarity;
				if (this.level < 20)
				{
					rarity = (random.RandomBool(0.5f) ? Rarity.HighGrade : Rarity.Enhanced);
				}
				else if (this.level < 25)
				{
					rarity = Rarity.HighGrade;
				}
				else
				{
					rarity = (random.RandomBool(0.125f) ? Rarity.Exotic : Rarity.HighGrade);
				}
				Inventory.InventoryItem inventoryItem = this.AddItemToShop(inv, random.ChooseAndRemove<EquipmentBuilder>(itemsForGeneralShop), new Rarity?(rarity), true, itemLevel);
				inventoryItem.costItem = currency;
				inventoryItem.costCount = Mathf.RoundToInt(random.RandomRange(13f, 15f) * GameMath.CostMultiplier(itemLevel) * rarity.GetCostMultiplier());
				if (rarity == Rarity.Enhanced)
				{
					inventoryItem.costCount /= 3;
				}
				num++;
			}
			int num2 = 0;
			while (num2 < moduleCount && list.Count > 0)
			{
				Rarity rarity2;
				if (this.level < 20)
				{
					rarity2 = (random.RandomBool(0.5f) ? Rarity.HighGrade : Rarity.Enhanced);
				}
				else if (this.level < 25)
				{
					rarity2 = Rarity.HighGrade;
				}
				else
				{
					rarity2 = (random.RandomBool(0.125f) ? Rarity.Exotic : Rarity.HighGrade);
				}
				Inventory.InventoryItem inventoryItem2 = this.AddItemToShop(inv, random.ChooseAndRemove<EquipmentBuilder>(list), new Rarity?(rarity2), true, itemLevel);
				inventoryItem2.costItem = currency;
				inventoryItem2.costCount = Mathf.RoundToInt(random.RandomRange(10f, 13f) * GameMath.CostMultiplier(itemLevel) * rarity2.GetCostMultiplier());
				if (rarity2 == Rarity.Enhanced)
				{
					inventoryItem2.costCount /= 3;
				}
				num2++;
			}
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00060A30 File Offset: 0x0005EC30
		private void AddBlankItemsToShop(ShopInventory shop, List<EquipmentBuilder> equipment, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Inventory.InventoryItem inventoryItem = this.AddItemToShop(shop, SeededRandom.Global.Choose<EquipmentBuilder>(equipment), new Rarity?(Rarity.HighGrade), false, 0);
				InventoryItemType inventoryItemType = (inventoryItem != null) ? inventoryItem.item : null;
				if (!(inventoryItemType == null))
				{
					inventoryItemType.SetBaseValue((int)((float)inventoryItemType.baseCost * 0.75f));
					foreach (AspectSlot aspectSlot in inventoryItemType.GetComponent<AbstractEquipment>().aspectSlots)
					{
						aspectSlot.SetEquipAspect(null);
					}
				}
			}
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00060ADC File Offset: 0x0005ECDC
		private void AddPermanentItemsToShop(ShopInventory shop)
		{
			if (shop.permanentItems != null)
			{
				foreach (Inventory.InventoryItem inventoryItem in shop.permanentItems)
				{
					shop.Add(inventoryItem.item, inventoryItem.count, false, false);
				}
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00060B48 File Offset: 0x0005ED48
		private void AddStorytellerItemsToShop(ShopInventory shop)
		{
			foreach (Storyteller storyteller in GamePlayer.current.storytellers)
			{
				storyteller.AddItemsToShop(this, shop);
			}
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00060BA0 File Offset: 0x0005EDA0
		private void AddBlueprintsToShop(ShopInventory shop, int count, InventoryItemType currency = null)
		{
			SeededRandom global = SeededRandom.Global;
			Rarity[] list = new Rarity[]
			{
				Rarity.Standard,
				Rarity.Enhanced
			};
			List<Rarity> list2 = new List<Rarity>();
			for (int i = 0; i < count; i++)
			{
				if (shop == this.industryShopInventory)
				{
					if (this.level > 25 && global.RandomBool(0.15f))
					{
						list2.Add(Rarity.Exotic);
					}
					else
					{
						list2.Add(Rarity.HighGrade);
					}
				}
				else if (shop == this.bountyShopInventory || shop == this.patrolShopInventory || this.faction == Faction.tradingGuild)
				{
					list2.Add(Rarity.HighGrade);
				}
				else if (this.forge != null)
				{
					list2.Add(global.Choose<Rarity>(list));
				}
			}
			if (list2.Count <= 0)
			{
				return;
			}
			ItemBuilder itemBuilder = ItemBuilder.Get("Blueprint");
			HashSet<string> hashSet = new HashSet<string>();
			foreach (Rarity value in list2)
			{
				int j = 0;
				while (j < 10)
				{
					InventoryItemType inventoryItemType = itemBuilder.CreateRandomBlueprint(this.level, new Rarity?(value), null, false);
					if (inventoryItemType == null)
					{
						break;
					}
					if (hashSet.Add(inventoryItemType.displayName))
					{
						Inventory.InventoryItem inventoryItem = shop.Add(inventoryItemType, 1, false, false);
						if (currency)
						{
							inventoryItem.costItem = currency;
							inventoryItem.costCount = Mathf.RoundToInt(35f * inventoryItemType.rarity.GetCostMultiplier());
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00060D20 File Offset: 0x0005EF20
		private void AddEquipmentToShop(Inventory shop, List<EquipmentBuilder> equipment, int extraItems)
		{
			SeededRandom.Global.Shuffle<EquipmentBuilder>(equipment);
			HashSet<string> hashSet = new HashSet<string>();
			foreach (EquipmentBuilder equipmentBuilder in equipment)
			{
				string text = equipmentBuilder.equipmentSize.ToString() + equipmentBuilder.slot.ToString();
				AbstractTurret abstractTurret;
				if (equipmentBuilder.prefab.TryGetComponent<AbstractTurret>(out abstractTurret))
				{
					text += abstractTurret.GetType().Name;
				}
				if (hashSet.Add(text))
				{
					this.AddItemToShop(shop, equipmentBuilder, null, false, 0);
				}
			}
			if (this.level > 2)
			{
				this.AddItemToShop(shop, SeededRandom.Global.Choose<EquipmentBuilder>(equipment), new Rarity?(Rarity.Enhanced), false, 0);
				extraItems--;
			}
			for (int i = 0; i < extraItems; i++)
			{
				this.AddItemToShop(shop, SeededRandom.Global.Choose<EquipmentBuilder>(equipment), null, false, 0);
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00060E44 File Offset: 0x0005F044
		private void AddItemsToShop(Inventory shop, ItemCategory category, Func<InventoryItemType, int> amountFunc, List<string> itemIds = null, bool includeList = true)
		{
			foreach (InventoryItemType inventoryItemType in InventoryItemType.all)
			{
				if (inventoryItemType.itemCategory == category)
				{
					if (itemIds != null && itemIds.Count > 0)
					{
						bool flag = itemIds.Contains(inventoryItemType.identifier);
						if ((includeList && !flag) || (!includeList && flag))
						{
							continue;
						}
					}
					int num = amountFunc(inventoryItemType);
					if (num > 0)
					{
						shop.Add(inventoryItemType, num, false, false);
					}
				}
			}
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00060ED8 File Offset: 0x0005F0D8
		private void AddJumpgatePassToShop(Inventory shop)
		{
			foreach (JumpGate jumpGate in this.system.sector.GetSectorJumpgates())
			{
				if (!jumpGate.canUseJumpGate && jumpGate.targetSystemGuid != null)
				{
					bool flag = true;
					using (IEnumerator<Inventory.InventoryItem> enumerator2 = GamePlayer.current.currentSpaceShip.cargo.items.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							JumpgatePassItem jumpgatePassItem;
							if (enumerator2.Current.item.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem) && jumpgatePassItem.jumpgateGuid == jumpGate.guid)
							{
								flag = false;
								break;
							}
						}
					}
					if (jumpGate.targetSystem.level > 55 && GamePlayer.current.level < 55)
					{
						flag = false;
					}
					if (flag)
					{
						InventoryItemType item = ItemBuilder.Get("JumpgatePass").CreateJumpgatePass(jumpGate, null);
						shop.Add(item, 1, false, false);
					}
				}
			}
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00060FF0 File Offset: 0x0005F1F0
		private Inventory.InventoryItem AddItemToShop(Inventory shop, EquipmentBuilder builder, Rarity? overrideRarity = null, bool force = false, int overrideLevel = 0)
		{
			if (this.faction == Faction.bountyGuild && builder.slot != EquipmentSlot.Hardpoint && !force)
			{
				return null;
			}
			if (this.faction == Faction.policeGuild && builder.slot == EquipmentSlot.Hardpoint && !force)
			{
				return null;
			}
			int level = (overrideLevel > 0) ? overrideLevel : this.level;
			InventoryItemType inventoryItemType = builder.CreateItemType(overrideRarity ?? RarityExtensions.GetShopRarity(level), level, false, null, false, false);
			Manufacturer? manufacturer;
			Faction faction = (inventoryItemType.GetManufacturer() != null) ? manufacturer.GetValueOrDefault().GetFaction() : null;
			if (force || faction == null || !faction.IsEnemy(this.faction))
			{
				return shop.Add(inventoryItemType, 1, false, false);
			}
			return null;
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000610B0 File Offset: 0x0005F2B0
		public bool RefreshShopsIfNecessary(bool cleanUp)
		{
			if (cleanUp && this.shopInventory != null)
			{
				this.CleanupShopInventory(this.shopInventory);
			}
			if (!this.TimeForRefresh())
			{
				return false;
			}
			if (InventoryInteractionManager.Instance.isShopOpen)
			{
				return false;
			}
			if (this.generalShopInventory != null)
			{
				this.GenerateShopInventory();
			}
			if (this.miningShopInventory != null)
			{
				this.GenerateMiningShopInventory();
			}
			if (this.salvageShopInventory != null)
			{
				this.GenerateSalvageShopInventory();
			}
			if (this.bountyShopInventory != null)
			{
				this.GenerateBountyShopInventory();
			}
			if (this.patrolShopInventory != null)
			{
				this.GeneratePatrolShopInventory();
			}
			if (this.industryShopInventory != null)
			{
				this.GenerateIndustryShopInventory();
			}
			if (this.conquestShopInventory != null)
			{
				this.GenerateConquestShopInventory();
			}
			if (this.umbralShopInventory != null)
			{
				this.GenerateUmbralShopInventory();
			}
			this.SetRefreshTime();
			return true;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00061163 File Offset: 0x0005F363
		private bool TimeForRefresh()
		{
			return this.shopRefreshTime < 0f || (double)(this.shopRefreshTime + 3600f) < GamePlayer.current.elapsedTime;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00061190 File Offset: 0x0005F390
		private void SetRefreshTime()
		{
			int num = Mathf.FloorToInt((float)GamePlayer.current.elapsedTime / 3600f);
			this.shopRefreshTime = 3600f * (float)num;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000611C4 File Offset: 0x0005F3C4
		public void CleanupShopInventory(ShopInventory inv)
		{
			List<Inventory.InventoryItem> list = new List<Inventory.InventoryItem>();
			foreach (Inventory.InventoryItem inventoryItem in inv.items)
			{
				JumpgatePassItem jumpgatePassItem;
				if (inventoryItem.item.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem))
				{
					JumpGate jumpGate = GalaxyMapData.current.GetPointOfInterest(jumpgatePassItem.jumpgateGuid) as JumpGate;
					if (jumpGate != null && jumpGate.canUseJumpGate)
					{
						list.Add(inventoryItem);
					}
					using (IEnumerator<Inventory.InventoryItem> enumerator2 = GamePlayer.current.currentSpaceShip.cargo.items.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							JumpgatePassItem jumpgatePassItem2;
							if (enumerator2.Current.item.TryGetComponent<JumpgatePassItem>(out jumpgatePassItem2) && jumpgatePassItem2.jumpgateGuid == jumpgatePassItem.jumpgateGuid)
							{
								list.Add(inventoryItem);
							}
						}
					}
				}
			}
			foreach (Inventory.InventoryItem item in list)
			{
				inv.Remove(item, 1);
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00061308 File Offset: 0x0005F508
		public void ClearShopInventory()
		{
			ShopInventory shopInventory = this.shopInventory;
			if (shopInventory != null)
			{
				shopInventory.Clear();
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00061325 File Offset: 0x0005F525
		public IEnumerable<SpaceStationFacility> GetFacilities()
		{
			List<SpaceStationFacility> list = new List<SpaceStationFacility>(Enum.GetValues(typeof(SpaceStationFacility)) as SpaceStationFacility[]);
			list.Sort((SpaceStationFacility a, SpaceStationFacility b) => a.GetSortOrder() - b.GetSortOrder());
			foreach (SpaceStationFacility spaceStationFacility in list)
			{
				if (this.HasFacility(spaceStationFacility))
				{
					yield return spaceStationFacility;
				}
			}
			List<SpaceStationFacility>.Enumerator enumerator = default(List<SpaceStationFacility>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00061338 File Offset: 0x0005F538
		public bool HasAvailableMission()
		{
			foreach (string name in this.characters)
			{
				Character character = Characters.GetCharacter(name);
				if (character != null && character.missionAvailable)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000613A0 File Offset: 0x0005F5A0
		public string GetAvailableMissionHint()
		{
			foreach (string name in this.characters)
			{
				string missionAvailableHint = Characters.GetCharacter(name).missionAvailableHint;
				if (missionAvailableHint != null)
				{
					return missionAvailableHint;
				}
			}
			return null;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00061400 File Offset: 0x0005F600
		public bool PlayerIsFriendly()
		{
			return !Faction.player.IsEnemy(this.faction) || this is EmbassyStation;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x0006141F File Offset: 0x0005F61F
		public bool TryAddCharacter(string character)
		{
			if (this.characters.Contains(character))
			{
				return false;
			}
			this.characters.Add(character);
			return true;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0006143E File Offset: 0x0005F63E
		public ShopInventory CreateUmbralShopInventory()
		{
			if (this.umbralShopInventory == null)
			{
				this.umbralShopInventory = new ShopInventory(this);
				this.umbralShopInventory.facility = SpaceStationFacility.GeneralShop;
				this.GenerateUmbralShopInventory();
			}
			return this.umbralShopInventory;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x0006146C File Offset: 0x0005F66C
		public bool DockingAvailableFor(Behaviour.Unit.SpaceShip spaceShip)
		{
			CombatStationData combatStationData = null;
			foreach (PersistableData persistableData in this.persistables)
			{
				CombatStationData combatStationData2 = persistableData as CombatStationData;
				if (combatStationData2 != null)
				{
					combatStationData = combatStationData2;
					break;
				}
			}
			if (combatStationData == null)
			{
				return true;
			}
			foreach (CombatStationPartData combatStationPartData in combatStationData.stationParts)
			{
				DockingOption componentInChildren = combatStationPartData.partPrefab.GetComponentInChildren<DockingOption>();
				if (componentInChildren != null && componentInChildren.CanDock(spaceShip))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000738 RID: 1848
		public const float ShopRefreshInterval = 3600f;

		// Token: 0x04000739 RID: 1849
		public Vector2 leftWarpInPosition = new Vector2(-20f, 0f);

		// Token: 0x0400073A RID: 1850
		public Vector2 rightWarpInPosition = new Vector2(20f, 0f);

		// Token: 0x0400073C RID: 1852
		public SpaceStation.StationSize stationSize = SpaceStation.StationSize.Medium;

		// Token: 0x0400073D RID: 1853
		public SpaceStation.StationVariants stationVariant;

		// Token: 0x0400073E RID: 1854
		public Inventory materialStorage = new Inventory(true);

		// Token: 0x0400073F RID: 1855
		public bool conquestStationInitialized;

		// Token: 0x04000740 RID: 1856
		public string stationSeed;

		// Token: 0x04000741 RID: 1857
		private EquipStat[] miningStats = new EquipStat[]
		{
			EquipStat.MiningPower
		};

		// Token: 0x04000742 RID: 1858
		private EquipStat[] salvageStats = new EquipStat[]
		{
			EquipStat.SalvagePower
		};

		// Token: 0x04000743 RID: 1859
		public ShopInventory generalShopInventory;

		// Token: 0x04000744 RID: 1860
		public ShopInventory miningShopInventory;

		// Token: 0x04000745 RID: 1861
		public ShopInventory salvageShopInventory;

		// Token: 0x04000746 RID: 1862
		public ShopInventory bountyShopInventory;

		// Token: 0x04000747 RID: 1863
		public ShopInventory patrolShopInventory;

		// Token: 0x04000748 RID: 1864
		public ShopInventory industryShopInventory;

		// Token: 0x04000749 RID: 1865
		public ShopInventory conquestShopInventory;

		// Token: 0x0400074A RID: 1866
		public ShopInventory umbralShopInventory;

		// Token: 0x0400074B RID: 1867
		public Bar bar;

		// Token: 0x0400074C RID: 1868
		public Refinery refinery;

		// Token: 0x0400074D RID: 1869
		public Forge forge;

		// Token: 0x0400074E RID: 1870
		public Shipyard shipyard;

		// Token: 0x0400074F RID: 1871
		public Source.Galaxy.POI.Station.MissionBoard missionBoard;

		// Token: 0x04000750 RID: 1872
		public Airlock airlock;

		// Token: 0x04000751 RID: 1873
		public PersonalHangar personalHangar;

		// Token: 0x04000752 RID: 1874
		public LocalEconomy economy;

		// Token: 0x04000753 RID: 1875
		public BountyBoard bountyBoard;

		// Token: 0x04000754 RID: 1876
		public PatrolBoard patrolBoard;

		// Token: 0x04000755 RID: 1877
		public IndustryBoard industryBoard;

		// Token: 0x04000756 RID: 1878
		public SalvageWorkshop salvageWorkshop;

		// Token: 0x04000757 RID: 1879
		public RecruitmentCenter recruitmentCenter;

		// Token: 0x04000758 RID: 1880
		public List<string> characters = new List<string>();

		// Token: 0x04000759 RID: 1881
		private readonly Dictionary<Faction, string> factionCommanders = new Dictionary<Faction, string>
		{
			{
				Faction.blue,
				"ConquestStellarCommander"
			},
			{
				Faction.red,
				"ConquestKolyatovCommander"
			},
			{
				Faction.gold,
				"ConquestLuminateCommander"
			}
		};

		// Token: 0x020004D2 RID: 1234
		public enum StationSize
		{
			// Token: 0x04001A22 RID: 6690
			Small,
			// Token: 0x04001A23 RID: 6691
			Medium,
			// Token: 0x04001A24 RID: 6692
			Large
		}

		// Token: 0x020004D3 RID: 1235
		public enum StationVariants
		{
			// Token: 0x04001A26 RID: 6694
			Neutral1,
			// Token: 0x04001A27 RID: 6695
			Neutral2,
			// Token: 0x04001A28 RID: 6696
			Neutral3,
			// Token: 0x04001A29 RID: 6697
			Neutral4,
			// Token: 0x04001A2A RID: 6698
			Neutral5
		}
	}
}
