using System;
using Behaviour.UI;
using Behaviour.UI.NotificationAlert;
using Behaviour.UI.Spacestation;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Simulation.World.System;
using Source.Util;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000310 RID: 784
	public class DogTagItem : UsableItem
	{
		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x000B048F File Offset: 0x000AE68F
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000B0492 File Offset: 0x000AE692
		public override void DataFromJson(JsonObject data)
		{
			this.rewardAmount = data["rewardAmount"];
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000B04AA File Offset: 0x000AE6AA
		public override void DataToJson(JsonObject data)
		{
			data["rewardAmount"] = new double?((double)this.rewardAmount);
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000B04C8 File Offset: 0x000AE6C8
		public override bool OnUse()
		{
			InventoryItemType currentCommendation = this.GetCurrentCommendation();
			if (!currentCommendation)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@CommendationDogTagError", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(8f).Show();
				return false;
			}
			int num = GamePlayer.current.CountAvailableItems(base.item);
			if (num == 1)
			{
				GamePlayer.current.currentSpaceShip.AddCargo(currentCommendation, this.rewardAmount, false);
				return true;
			}
			if (num > 0)
			{
				InventoryInteractionManager.Instance.PopupDogTagItem(this);
			}
			return false;
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000B0558 File Offset: 0x000AE758
		public InventoryItemType GetCurrentCommendation()
		{
			SpaceStation current = SpaceStation.current;
			if (current == null || !SpaceStationInterior.instance)
			{
				return null;
			}
			if (current.faction == Faction.bountyGuild)
			{
				return "BountyCurrency";
			}
			if (current.faction == Faction.policeGuild)
			{
				return "PatrolCurrency";
			}
			if (current.faction == Faction.industrialGuild)
			{
				return "IndustryCurrency";
			}
			if (!(current is EmbassyStation))
			{
				ConquestSystem conquestSystem = current.system.storyteller as ConquestSystem;
				if (conquestSystem == null || !conquestSystem.headquarters)
				{
					return null;
				}
			}
			return "ConquestCurrency";
		}

		// Token: 0x04001204 RID: 4612
		public int rewardAmount;
	}
}
