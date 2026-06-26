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
	// Token: 0x0200031C RID: 796
	public class UmbralCargoScanner : UsableItem
	{
		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001DD9 RID: 7641 RVA: 0x000B1BF4 File Offset: 0x000AFDF4
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x000B1BF7 File Offset: 0x000AFDF7
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000B1BFA File Offset: 0x000AFDFA
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000B1BFC File Offset: 0x000AFDFC
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000B1C00 File Offset: 0x000AFE00
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
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralCargoScannerRequiresFriendly", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
				return false;
			}
			UmbralCargoScannerBot umbralCargoScannerBot = UnityEngine.Object.Instantiate<UmbralCargoScannerBot>(this.botPrefab, BasePoiManager.current.transform);
			umbralCargoScannerBot.transform.position = GameplayManager.Instance.spaceShip.transform.position;
			umbralCargoScannerBot.transform.Z(ZIndex.Drone);
			return true;
		}

		// Token: 0x04001215 RID: 4629
		[SerializeField]
		private UmbralCargoScannerBot botPrefab;
	}
}
