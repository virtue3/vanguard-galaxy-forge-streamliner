using System;
using Behaviour.Managers;
using Behaviour.UI.NotificationAlert;
using Behaviour.Unit;
using Behaviour.Unit.Parts;
using Behaviour.Util;
using LightJson;
using Source.Util;
using UnityEngine;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200031E RID: 798
	public class UmbralTrackingBeacon : UsableItem
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001DE5 RID: 7653 RVA: 0x000B1E88 File Offset: 0x000B0088
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x000B1E8B File Offset: 0x000B008B
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DE7 RID: 7655 RVA: 0x000B1E8E File Offset: 0x000B008E
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000B1E90 File Offset: 0x000B0090
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000B1E94 File Offset: 0x000B0094
		public override bool OnUse()
		{
			bool flag = false;
			SpaceShip[] componentsInChildren = BasePoiManager.current.GetComponentsInChildren<SpaceShip>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!componentsInChildren[i].IsPlayerEnemy())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralTrackingBeaconRequiresFriendly", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
				return false;
			}
			UmbralTrackingBot umbralTrackingBot = UnityEngine.Object.Instantiate<UmbralTrackingBot>(this.botPrefab, BasePoiManager.current.transform);
			umbralTrackingBot.transform.position = GameplayManager.Instance.spaceShip.transform.position;
			umbralTrackingBot.transform.Z(ZIndex.Drone);
			return true;
		}

		// Token: 0x04001218 RID: 4632
		[SerializeField]
		private UmbralTrackingBot botPrefab;
	}
}
