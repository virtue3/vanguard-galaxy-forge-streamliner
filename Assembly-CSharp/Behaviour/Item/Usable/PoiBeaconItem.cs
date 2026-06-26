using System;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Util;
using LightJson;
using Source.Data.Persistable;
using Source.Galaxy;
using Source.MissionSystem;
using Source.Player;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000316 RID: 790
	public class PoiBeaconItem : UsableItem
	{
		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x000B12CF File Offset: 0x000AF4CF
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DAC RID: 7596 RVA: 0x000B12D4 File Offset: 0x000AF4D4
		public override bool OnUse()
		{
			MapPointOfInterest current = MapPointOfInterest.current;
			if (current == null || (current.system.pointsOfInterest.Contains(current) && current.timeLeft <= 0f))
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@PoiBeaconFail", Array.Empty<object>())).WithColor(ColorHelper.red90).Show();
				return false;
			}
			current.timeLeft = Mathf.Max(current.timeLeft, 1800f);
			if (!current.system.pointsOfInterest.Contains(current))
			{
				current.system.pointsOfInterest.Add(current);
			}
			PrefabData prefabData = new PrefabData();
			prefabData.prefabName = "PoiBeacon";
			SpaceShip spaceShip = GameplayManager.Instance.spaceShip;
			prefabData.position = spaceShip.transform.position + spaceShip.transform.rotation * new Vector3(-2f, 0f, 0f);
			current.AddPersistable(prefabData);
			foreach (Mission mission in GamePlayer.current.allMissions)
			{
				foreach (MissionStep missionStep in mission.steps)
				{
					if (missionStep.dynamicPointOfInterest == current)
					{
						missionStep.dynamicPointOfInterest = null;
						missionStep.poiHint = current;
					}
				}
			}
			return true;
		}

		// Token: 0x06001DAD RID: 7597 RVA: 0x000B1464 File Offset: 0x000AF664
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001DAE RID: 7598 RVA: 0x000B1466 File Offset: 0x000AF666
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001DAF RID: 7599 RVA: 0x000B1468 File Offset: 0x000AF668
		public static PrefabData GetPoiBeacon(MapPointOfInterest poi)
		{
			foreach (PersistableData persistableData in poi.GetPersistables())
			{
				PrefabData prefabData = persistableData as PrefabData;
				if (prefabData != null && prefabData.prefabName == "PoiBeacon")
				{
					return prefabData;
				}
			}
			return null;
		}

		// Token: 0x06001DB0 RID: 7600 RVA: 0x000B14D0 File Offset: 0x000AF6D0
		public static bool HasPoiBeacon(MapPointOfInterest poi)
		{
			return PoiBeaconItem.GetPoiBeacon(poi) != null;
		}

		// Token: 0x06001DB1 RID: 7601 RVA: 0x000B14DC File Offset: 0x000AF6DC
		public static void RemovePoiBeacon(MapPointOfInterest poi)
		{
			PrefabData poiBeacon = PoiBeaconItem.GetPoiBeacon(poi);
			if (poiBeacon != null)
			{
				poi.RemovePersistable(poiBeacon);
			}
		}

		// Token: 0x0400120E RID: 4622
		public const float beaconDuration = 1800f;
	}
}
