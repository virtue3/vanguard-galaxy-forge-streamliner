using System;
using Behaviour.UI.NotificationAlert;
using Behaviour.Util;
using LightJson;
using Source.Player;
using Source.Util;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000318 RID: 792
	public class RespecSkillsItem : UsableItem
	{
		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001DBD RID: 7613 RVA: 0x000B1609 File Offset: 0x000AF809
		public override bool canUseInSpacestation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000B160C File Offset: 0x000AF80C
		public override void DataFromJson(JsonObject data)
		{
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000B160E File Offset: 0x000AF80E
		public override void DataToJson(JsonObject data)
		{
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000B1610 File Offset: 0x000AF810
		public override bool OnUse()
		{
			this.Respec();
			return true;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000B1619 File Offset: 0x000AF819
		private void Respec()
		{
			GamePlayer.current.commander.RefundAllSkills(true);
			Singleton<NotificationManager>.Instance.CreateNotification(Translation.Translate("@RespecSkills", Array.Empty<object>())).WithColor(ColorHelper.greenish).Show();
		}
	}
}
