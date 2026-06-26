using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Item;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000313 RID: 787
	public class LootBoxItem : UsableItem
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001D8C RID: 7564 RVA: 0x000B0813 File Offset: 0x000AEA13
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x000B0816 File Offset: 0x000AEA16
		// (set) Token: 0x06001D8E RID: 7566 RVA: 0x000B081E File Offset: 0x000AEA1E
		public string seed { get; private set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x000B0827 File Offset: 0x000AEA27
		public SystemMapData systemMapData
		{
			get
			{
				return this.system ?? SystemMapData.current;
			}
		}

		// Token: 0x06001D90 RID: 7568 RVA: 0x000B0838 File Offset: 0x000AEA38
		public void Init(SystemMapData system)
		{
			this.seed = SeededRandom.Global.RandomItemSeed();
			this.system = system;
			base.item.SetBaseCost(GameMath.GetCreditsValue(50f, base.item.itemLevel));
		}

		// Token: 0x06001D91 RID: 7569 RVA: 0x000B0874 File Offset: 0x000AEA74
		public override bool OnUse()
		{
			if (GamePlayer.current.currentPointOfInterest == null)
			{
				return false;
			}
			SpaceStation spaceStation = GamePlayer.current.currentPointOfInterest as SpaceStation;
			if (spaceStation == null || spaceStation.faction != Faction.salvageGuild || !SpaceStationInterior.instance)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@LootBoxSteelVultures", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
				return false;
			}
			if (!this.CanUseKey() && (long)GameMath.GetCreditsValue(50f, base.item.itemLevel) > GamePlayer.current.credits)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@SSNoCredits", Array.Empty<object>())).WithColor(ColorHelper.orange75).WithCustomTime(2f).Show();
				return false;
			}
			Singleton<NotificationManager>.Instance.ShowLootBox(this);
			return false;
		}

		// Token: 0x06001D92 RID: 7570 RVA: 0x000B0960 File Offset: 0x000AEB60
		public List<Dictionary<InventoryItemType, int>> Open(int count = 1)
		{
			List<Dictionary<InventoryItemType, int>> list = new List<Dictionary<InventoryItemType, int>>();
			List<Inventory.InventoryItem> list2 = new List<Inventory.InventoryItem>();
			Inventory.InventoryItem specificBox = Inventory.cargo.GetByExactType(base.item) ?? Inventory.global.GetByExactType(base.item);
			if (specificBox != null)
			{
				list2.Add(specificBox);
			}
			IEnumerable<Inventory.InventoryItem> collection = from b in LootBoxOpener.GetAllLootBoxes()
			where b != specificBox
			select b;
			list2.AddRange(collection);
			int num = 0;
			foreach (Inventory.InventoryItem inventoryItem in list2)
			{
				if (num >= count)
				{
					break;
				}
				int num2 = 0;
				while (num2 < inventoryItem.count && num < count)
				{
					float num3 = (float)GameMath.GetCreditsValue(50f, inventoryItem.item.itemLevel);
					bool flag = GamePlayer.current.CountAvailableItems("LockedContainerKey") > 0;
					if (!flag && (float)GamePlayer.current.credits < num3)
					{
						Debug.Log("Out of money or keys");
						return list;
					}
					if (flag)
					{
						GamePlayer.current.ConsumeAvailableItems("LockedContainerKey", 1);
					}
					else
					{
						GamePlayer.current.RemoveCredits(num3);
					}
					IEnumerable<KeyValuePair<InventoryItemType, int>> source = Singleton<LootManager>.Instance.OpenLootBox(inventoryItem.item.GetComponent<LootBoxItem>());
					inventoryItem.inventory.Remove(inventoryItem.item, 1);
					Dictionary<InventoryItemType, int> dictionary = (from x in source
					orderby UnityEngine.Random.value
					select x).ToDictionary((KeyValuePair<InventoryItemType, int> x) => x.Key, (KeyValuePair<InventoryItemType, int> x) => x.Value);
					foreach (KeyValuePair<InventoryItemType, int> keyValuePair in dictionary)
					{
						UsableItem usableItem;
						if (keyValuePair.Key.TryGetComponent<UsableItem>(out usableItem) && usableItem is CreditsItem)
						{
							for (int i = 0; i < keyValuePair.Value; i++)
							{
								usableItem.OnUse();
							}
						}
						else
						{
							if (keyValuePair.Key.identifier == "BonusSkillPointTemplate")
							{
								Register.AddCounter("LootboxSkillPointsGained", keyValuePair.Value, 0);
							}
							GamePlayer.current.currentSpaceShip.cargo.Add(keyValuePair.Key, keyValuePair.Value, false, false);
						}
					}
					list.Add(dictionary);
					num++;
					num2++;
				}
			}
			return list;
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000B0C50 File Offset: 0x000AEE50
		public bool CanUseKey()
		{
			return GamePlayer.current.CountAvailableItems("LockedContainerKey") > 0;
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000B0C69 File Offset: 0x000AEE69
		public override void DataToJson(JsonObject data)
		{
			data["seed"] = this.seed;
			string key = "system";
			SystemMapData systemMapData = this.system;
			data[key] = ((systemMapData != null) ? systemMapData.guid : null);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000B0CA4 File Offset: 0x000AEEA4
		public override void DataFromJson(JsonObject data)
		{
			this.seed = data["seed"];
			if (!data["system"].IsNull)
			{
				GalaxyMapData.current.LoadSystem(data["system"], delegate(SystemMapData sys)
				{
					this.system = sys;
				});
			}
		}

		// Token: 0x04001209 RID: 4617
		private SystemMapData system;
	}
}
