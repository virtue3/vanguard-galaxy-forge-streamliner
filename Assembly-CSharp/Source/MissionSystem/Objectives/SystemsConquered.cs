using System;
using LightJson;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000CE RID: 206
	public class SystemsConquered : MissionObjective
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0003F6B8 File Offset: 0x0003D8B8
		public override string statusText
		{
			get
			{
				return string.Concat(new string[]
				{
					"Conquer for ",
					Translation.Translate(this.faction.name, Array.Empty<object>()),
					" ",
					GameMath.FormatNumber(this.currentPercentage * 100f, 2),
					"/",
					GameMath.FormatPercentage(this.targetPercentage, FormatPercentageMode.Default, 2)
				});
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x0003F725 File Offset: 0x0003D925
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x0003F72D File Offset: 0x0003D92D
		public virtual float targetPercentage { get; set; }

		// Token: 0x0600081F RID: 2079 RVA: 0x0003F736 File Offset: 0x0003D936
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0003F739 File Offset: 0x0003D939
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x0003F73C File Offset: 0x0003D93C
		public override bool IsComplete()
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller == null)
			{
				return false;
			}
			ConquestFactionStanding factionStanding = storyteller.GetFactionStanding(this.faction);
			this.currentPercentage = factionStanding.currentConqueredPercentage;
			return this.currentPercentage >= this.targetPercentage;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0003F783 File Offset: 0x0003D983
		protected override void DataToJson(JsonObject data)
		{
			data["faction"] = this.faction.identifier;
			data["percentage"] = new double?((double)this.targetPercentage);
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0003F7BC File Offset: 0x0003D9BC
		protected override void LoadFromJson(JsonObject data)
		{
			this.faction = Faction.Get(data["faction"]);
			this.targetPercentage = (float)data["percentage"].AsNumber;
		}

		// Token: 0x04000473 RID: 1139
		public Faction faction;

		// Token: 0x04000475 RID: 1141
		protected float currentPercentage;
	}
}
