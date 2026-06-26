using System;
using LightJson;

namespace Source.Galaxy
{
	// Token: 0x0200013E RID: 318
	public class ConquestFactionStanding : IJsonSource
	{
		// Token: 0x06000BC6 RID: 3014 RVA: 0x00056130 File Offset: 0x00054330
		public ConquestFactionStanding(Faction f)
		{
			this.faction = f;
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00056140 File Offset: 0x00054340
		public JsonValue ToJson()
		{
			return new JsonObject
			{
				{
					"playerContribution",
					new double?((double)this.playerContribution)
				},
				{
					"currentConquestArea",
					new double?((double)this.currentConquestArea)
				},
				{
					"maxConquestArea",
					new double?((double)this.maxConquestArea)
				},
				{
					"rejoinConquestCooldown",
					new double?((double)this.rejoinConquestCooldown)
				},
				{
					"currentConqueredPercentage",
					new double?((double)this.currentConqueredPercentage)
				}
			};
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000561E8 File Offset: 0x000543E8
		public static ConquestFactionStanding FromJson(Faction f, JsonValue data)
		{
			return new ConquestFactionStanding(f)
			{
				playerContribution = data["playerContribution"],
				currentConquestArea = data["currentConquestArea"],
				maxConquestArea = data["maxConquestArea"],
				rejoinConquestCooldown = data["rejoinConquestCooldown"],
				currentConqueredPercentage = (float)data["currentConqueredPercentage"].AsNumber
			};
		}

		// Token: 0x04000694 RID: 1684
		public readonly Faction faction;

		// Token: 0x04000695 RID: 1685
		public int playerContribution;

		// Token: 0x04000696 RID: 1686
		public int currentConquestArea;

		// Token: 0x04000697 RID: 1687
		public int maxConquestArea;

		// Token: 0x04000698 RID: 1688
		public int rejoinConquestCooldown;

		// Token: 0x04000699 RID: 1689
		public float currentConqueredPercentage;
	}
}
