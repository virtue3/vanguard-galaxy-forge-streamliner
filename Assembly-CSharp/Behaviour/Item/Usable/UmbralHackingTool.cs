using System;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit.Parts;
using Behaviour.Util;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200031D RID: 797
	public class UmbralHackingTool : UsableItem
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x000B1CB5 File Offset: 0x000AFEB5
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x000B1CB8 File Offset: 0x000AFEB8
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000B1CBB File Offset: 0x000AFEBB
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000B1CBD File Offset: 0x000AFEBD
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000B1CC0 File Offset: 0x000AFEC0
		public override bool OnUse()
		{
			ConquestStation conquestStation = MapPointOfInterest.current as ConquestStation;
			if (conquestStation == null)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralHackingToolRequiresConquest", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
				return false;
			}
			if (!conquestStation.PlayerIsFriendly())
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralHackingToolRequiresFriendly", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
				return false;
			}
			if (conquestStation.umbralControlLevel > 0f)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralHackingToolAlreadyInfected", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
				return false;
			}
			bool flag = false;
			foreach (SystemMapData systemMapData in conquestStation.system.GetAdjacentSystems())
			{
				ConquestStation pointOfInterest = systemMapData.GetPointOfInterest<ConquestStation>();
				if (pointOfInterest != null && pointOfInterest.umbralControlLevel > 0.19f)
				{
					flag = true;
					break;
				}
			}
			if (!flag && !this.isUberVariant)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralHackingToolRequiresAdjacent", Array.Empty<object>())).WithColor(ColorHelper.red90).WithCustomTime(5f).Show();
				return false;
			}
			UmbralHackingBot umbralHackingBot = UnityEngine.Object.Instantiate<UmbralHackingBot>(this.botPrefab, BasePoiManager.current.transform);
			umbralHackingBot.controlPercentage = (this.isUberVariant ? 0.7f : 0.05f);
			umbralHackingBot.transform.position = GameplayManager.Instance.spaceShip.transform.position;
			umbralHackingBot.transform.Z(ZIndex.Drone);
			return true;
		}

		// Token: 0x04001216 RID: 4630
		[SerializeField]
		private bool isUberVariant;

		// Token: 0x04001217 RID: 4631
		[SerializeField]
		private UmbralHackingBot botPrefab;
	}
}
