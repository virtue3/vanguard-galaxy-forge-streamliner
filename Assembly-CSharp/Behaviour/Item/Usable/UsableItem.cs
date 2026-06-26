using System;
using Behaviour.UI;
using LightJson;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000320 RID: 800
	public abstract class UsableItem : InventoryItemPart
	{
		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001DF1 RID: 7665 RVA: 0x000B2013 File Offset: 0x000B0213
		public virtual bool canUseInSpacestation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x000B2016 File Offset: 0x000B0216
		public virtual bool keepInCargo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001DF3 RID: 7667
		public abstract bool OnUse();

		// Token: 0x06001DF4 RID: 7668
		public abstract void DataToJson(JsonObject data);

		// Token: 0x06001DF5 RID: 7669
		public abstract void DataFromJson(JsonObject data);

		// Token: 0x06001DF6 RID: 7670 RVA: 0x000B2019 File Offset: 0x000B0219
		public virtual void AddToTooltip(CompareTooltip tooltip)
		{
		}
	}
}
