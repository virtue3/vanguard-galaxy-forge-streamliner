using System;
using System.Collections.Generic;
using System.Linq;
using Behaviour.Crew;
using Behaviour.Equipment.Module;
using Behaviour.GalaxyMap;
using Behaviour.Item;
using Behaviour.Item.Usable;
using Behaviour.Managers;
using Behaviour.Mining;
using Behaviour.UI;
using Behaviour.Unit;
using Behaviour.Util;
using Behaviour.Weapons;
using Source.Data.Persistable;
using Source.Player;
using Source.Util;

namespace Source.Galaxy.POI
{
	// Token: 0x02000158 RID: 344
	public class Mining : MapPointOfInterest
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000D23 RID: 3363 RVA: 0x0005DE75 File Offset: 0x0005C075
		public override WorldMapPOI Prefab
		{
			get
			{
				return WorldMapPOI.GetPrefab("Mining");
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000D24 RID: 3364 RVA: 0x0005DE81 File Offset: 0x0005C081
		public override string sceneName
		{
			get
			{
				return "Mining";
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000D25 RID: 3365 RVA: 0x0005DE88 File Offset: 0x0005C088
		public override bool storeLastX
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0005DE8C File Offset: 0x0005C08C
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			foreach (PersistableData persistableData in base.GetPersistables())
			{
				AsteroidData asteroidData = persistableData as AsteroidData;
				if (asteroidData != null)
				{
					if (asteroidData.scanProgress > 0)
					{
						num++;
						if (asteroidData.surfaceAmount > 0)
						{
							num3++;
						}
						if (asteroidData.innerCoreAmount > 0)
						{
							num4++;
						}
					}
					else
					{
						num2++;
					}
				}
			}
			tooltip.AddTextLine(Translation.Translate("@Scanned", Array.Empty<object>()) + " " + num.ToString(), 12, 8f).Text.color = ColorHelper.detailsColor;
			if (num > 0)
			{
				tooltip.AddTextLine(Translation.Translate("@MiningSurface", Array.Empty<object>()) + " " + num3.ToString(), 12, 8f).Text.color = ((num3 > 0) ? ColorHelper.greenish : ColorHelper.reddish);
				tooltip.AddTextLine(Translation.Translate("@MiningCore", Array.Empty<object>()) + " " + num4.ToString(), 12, 8f).Text.color = ((num4 > 0) ? ColorHelper.greenish : ColorHelper.reddish);
			}
			string str = Translation.Translate("@AsteroidsNotScanned", Array.Empty<object>());
			string str2 = " ";
			string text = (num == 0) ? Translation.Translate("@Unknown", Array.Empty<object>()) : num2;
			tooltip.AddTextLine(str + str2 + ((text != null) ? text.ToString() : null), 12, 8f).Text.color = ColorHelper.detailsColor;
			if (SkilltreeNode.miningAdvancedScanning.isActive)
			{
				if (this.lastVisitedTime != 0f)
				{
					using (Dictionary<OreItemData, int>.Enumerator enumerator2 = base.GetOreTotalsInField(false).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<OreItemData, int> keyValuePair = enumerator2.Current;
							OreItemData key = keyValuePair.Key;
							int value = keyValuePair.Value;
							InventoryItemType component = key.GetComponent<InventoryItemType>();
							tooltip.AddTextLine(string.Format("{0} x{1}", Translation.Translate("@" + key.name, Array.Empty<object>()).HighlightWithColor(component.rarity.GetColor()), value), 12, 8f);
						}
						goto IL_354;
					}
				}
				tooltip.AddTextLine(Translation.Translate("@ChanceToFindOres", Array.Empty<object>()) + ":", 12, 8f).Text.color = ColorHelper.offWhite;
				foreach (var _AnonType in from ore in base.asteroidFieldData.GetAllOresInField()
				select new
				{
					Ore = ore,
					ItemType = ore.GetComponent<InventoryItemType>()
				} into o
				orderby o.ItemType.rarity
				select o)
				{
					tooltip.AddTextLine(Translation.Translate("@" + _AnonType.Ore.name, Array.Empty<object>()).HighlightWithColor(_AnonType.ItemType.rarity.GetColor()) ?? "", 12, 8f);
				}
			}
			IL_354:
			if (!GamePlayer.current.currentSpaceShip.HasLoadout(GameplayType.Mining, TargetLayer.Both))
			{
				tooltip.AddTextLine("@MiningNoTurret", 12, 8f).Text.color = ColorHelper.reddish;
			}
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x0005E24C File Offset: 0x0005C44C
		public override bool UnvisitedAndHasRetrievableItem(InventoryItemType itemType, bool overrideVisited = false)
		{
			if (!overrideVisited && this.lastVisitedTime > 0f)
			{
				return false;
			}
			if (MapPointOfInterest.current == this)
			{
				return false;
			}
			if (Singleton<TravelManager>.Instance.targetPoi == this)
			{
				return false;
			}
			GameplayManager instance = GameplayManager.Instance;
			object obj;
			if (instance == null)
			{
				obj = null;
			}
			else
			{
				SpaceShip spaceShip = instance.spaceShip;
				obj = ((spaceShip != null) ? spaceShip.GetModule<MiningModule>() : null);
			}
			object obj2 = obj;
			bool flag = obj2 != null && obj2.CanMineItemFromFieldData(base.asteroidFieldData, itemType);
			if (flag)
			{
				if (this.lastVisitedTime > 0f)
				{
					base.CleanupPersistables();
				}
				this.asteroidsInitialized = false;
				base.InitializeAsteroids(false, false);
			}
			return flag;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x0005E2DC File Offset: 0x0005C4DC
		public override void CleanupPersistables()
		{
			if (!MiningClaimItem.IsMiningClaim(this))
			{
				base.CleanupPersistables();
				if (this.lastVisitedTime == 0f)
				{
					return;
				}
				if (Singleton<TravelManager>.Instance.targetPoi == this)
				{
					return;
				}
				this.lastVisitedTime = 0f;
				base.UpdateLocalPosition(this.system.GetRandomPosition(-20f, 20f, -5f, 5f));
			}
		}
	}
}
