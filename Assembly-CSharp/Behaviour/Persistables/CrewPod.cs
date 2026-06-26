using System;
using Behaviour.Crew;
using Behaviour.Equipment.Turret;
using Behaviour.Item.Usable;
using Behaviour.Tractoring;
using Behaviour.UI;
using Behaviour.UI.Tooltip;
using Behaviour.Weapons;
using Source.Data.Persistable;
using Source.Util;
using UnityEngine;

namespace Behaviour.Persistables
{
	// Token: 0x020002F2 RID: 754
	public class CrewPod : TractorableItem, ITooltipCustomSource
	{
		// Token: 0x06001B8C RID: 7052 RVA: 0x000A7D18 File Offset: 0x000A5F18
		public void Init(CrewPodData item)
		{
			base.data = item;
			this.crewPodItem = base.data.itemType.GetComponent<CrewPodItem>();
			this.crew = Behaviour.Crew.Crew.Get(this.crewPodItem.crewType);
			base.GetComponent<SpriteRenderer>().sprite = this.crew.icon;
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x000A7D6E File Offset: 0x000A5F6E
		public override bool CanBeDamagedBy(AbstractTurret turret)
		{
			return false;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000A7D71 File Offset: 0x000A5F71
		public override void TakeDamage(DamageData data)
		{
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06001B8F RID: 7055 RVA: 0x000A7D73 File Offset: 0x000A5F73
		public override string targetName { get; }

		// Token: 0x06001B90 RID: 7056 RVA: 0x000A7D7C File Offset: 0x000A5F7C
		public void AddTooltipCustomContent(UITooltip tooltip)
		{
			tooltip.AddHeader("Escape Pod", this.crew.level, 0, 12, 8f);
			string text = Translation.Translate("@" + this.crew.identifier, Array.Empty<object>());
			tooltip.AddTextLine(text.HighlightWithColor(this.crew.rarity.GetColor()) + " x" + this.crewPodItem.crewAmount.ToString(), 12, 8f).Text.color = ColorHelper.offWhite;
			tooltip.AddTextLine(this.crew.GetBonus(), 12, 8f);
			tooltip.AddSeparator(null);
			tooltip.AddTextLine("Description: " + Translation.Translate("@" + this.crew.identifier + "Desc", Array.Empty<object>()), 12, 8f).Text.color = ColorHelper.boringGrey;
		}

		// Token: 0x0400114A RID: 4426
		private CrewPodItem crewPodItem;

		// Token: 0x0400114B RID: 4427
		private Behaviour.Crew.Crew crew;
	}
}
