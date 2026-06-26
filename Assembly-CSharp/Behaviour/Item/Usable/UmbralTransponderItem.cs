using System;
using Behaviour.UI.HUD;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using LightJson;
using Source.MissionSystem;
using Source.Player;
using Source.Util;

namespace Behaviour.Item.Usable
{
	// Token: 0x0200031F RID: 799
	public class UmbralTransponderItem : UsableItem
	{
		// Token: 0x17000441 RID: 1089
		// (get) Token: 0x06001DEB RID: 7659 RVA: 0x000B1F49 File Offset: 0x000B0149
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x000B1F4C File Offset: 0x000B014C
		public override bool keepInCargo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000B1F4F File Offset: 0x000B014F
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000B1F51 File Offset: 0x000B0151
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x000B1F54 File Offset: 0x000B0154
		public override bool OnUse()
		{
			if (GamePlayer.current.hasUmbralTransponder)
			{
				Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralTransponderIsActive", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
				return false;
			}
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@UmbralTransponderActivated", Array.Empty<object>())).WithColor(ColorHelper.umbralColor).WithCustomTime(3f).Show();
			GamePlayer.current.hasUmbralTransponder = true;
			MissionObjective.Trigger(MissionTrigger.DecoyTransponderUsed, null, null, false);
			Register.AddCounter("DecoyTransponderUsed", 1, 0);
			GamePlayer.current.hasUmbralTransponder = true;
			HudManager.Instance.SetTransponder();
			return true;
		}
	}
}
