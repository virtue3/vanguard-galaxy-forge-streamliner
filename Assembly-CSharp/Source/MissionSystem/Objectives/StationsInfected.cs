using System;
using LightJson;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000CD RID: 205
	public class StationsInfected : MissionObjective
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0003F550 File Offset: 0x0003D750
		public override string statusText
		{
			get
			{
				return string.Concat(new string[]
				{
					"Stations 100% infected for ",
					Translation.Translate(this.faction.name, Array.Empty<object>()),
					": ",
					this.currentAmount.ToString(),
					"/",
					this.targetAmount.ToString()
				});
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0003F5B4 File Offset: 0x0003D7B4
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0003F5B7 File Offset: 0x0003D7B7
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0003F5BC File Offset: 0x0003D7BC
		public override bool IsComplete()
		{
			if (this.nextUpdate <= Time.time)
			{
				Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
				if (storyteller == null)
				{
					return false;
				}
				this.currentAmount = Mathf.Clamp(storyteller.GetUmbralControlledStations(true), 0, this.targetAmount);
				this.nextUpdate = Time.time + 1f;
			}
			GamePlayer current = GamePlayer.current;
			return (current == null || !current.autoPlay) && this.currentAmount >= this.targetAmount;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0003F636 File Offset: 0x0003D836
		protected override void DataToJson(JsonObject data)
		{
			data["faction"] = this.faction.identifier;
			data["amount"] = new double?((double)this.targetAmount);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0003F66F File Offset: 0x0003D86F
		protected override void LoadFromJson(JsonObject data)
		{
			this.faction = Faction.Get(data["faction"]);
			this.targetAmount = data["amount"];
		}

		// Token: 0x0400046F RID: 1135
		public Faction faction = Faction.puppeteers;

		// Token: 0x04000470 RID: 1136
		public int targetAmount;

		// Token: 0x04000471 RID: 1137
		private int currentAmount;

		// Token: 0x04000472 RID: 1138
		private float nextUpdate;
	}
}
