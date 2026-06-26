using System;
using Behaviour.UI;
using LightJson;
using Source.Simulation.Story;
using Source.Util;

namespace Source.Galaxy.POI
{
	// Token: 0x02000153 RID: 339
	public class EmbassyStation : SpaceStation
	{
		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x0005D559 File Offset: 0x0005B759
		public override string typeName
		{
			get
			{
				if (this.faction != Faction.puppeteers)
				{
					return base.typeName;
				}
				return "@MapPOIUmbralStation";
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0005D574 File Offset: 0x0005B774
		public override void AddTooltipInfo(UITooltip tooltip)
		{
			if (this.combatStrength >= 1f && Conquest.conquestFactions.Contains(this.faction))
			{
				tooltip.AddTextLine(Translation.Translate("@ConquestSystemEmbassyStrength", new object[]
				{
					this.combatStrength
				}), 12, 8f);
				tooltip.AddSeparator(null);
			}
			base.AddTooltipInfo(tooltip);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0005D5E3 File Offset: 0x0005B7E3
		public override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["combatStrength"] = new double?((double)this.combatStrength);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0005D608 File Offset: 0x0005B808
		public override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data);
			this.combatStrength = (float)data["combatStrength"].AsNumber;
		}

		// Token: 0x04000718 RID: 1816
		public float combatStrength;
	}
}
