using System;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Simulation.Story;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200030C RID: 780
	public class BonusSkillPoint : UsableItem
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x000AFCD0 File Offset: 0x000ADED0
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x000AFCD3 File Offset: 0x000ADED3
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000AFCD5 File Offset: 0x000ADED5
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000AFCD7 File Offset: 0x000ADED7
		public override bool OnUse()
		{
			SteamAchievement.Trigger("BonusSkillpoint");
			return GamePlayer.current.commander.TryGiveBonusSkillPoints(1, false);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000AFCF4 File Offset: 0x000ADEF4
		public override void OnPurchase(int amount)
		{
			SpaceStation spaceStation = MapPointOfInterest.current as SpaceStation;
			if (spaceStation != null && spaceStation.conquestShopInventory != null)
			{
				Register.AddCounter("ConquestSkillpointPurchase", amount, 0);
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				if (storyteller != null)
				{
					storyteller.UpdateSkillPointPrice();
				}
			}
			else
			{
				Register.AddCounter("BonusSkillpointPurchase", amount, 0);
			}
			base.item.RecalculateCost();
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000AFD53 File Offset: 0x000ADF53
		public override int GetDynamicValue()
		{
			return Mathf.RoundToInt((float)base.item.baseCost * Mathf.Pow(1.5f, (float)Register.GetCounter("BonusSkillpointPurchase", 0)));
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000AFD7D File Offset: 0x000ADF7D
		public static int GetConquestValue()
		{
			return Mathf.RoundToInt(140f * Mathf.Pow(1.2f, (float)Register.GetCounter("ConquestSkillpointPurchase", 0)));
		}
	}
}
