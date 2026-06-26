using System;
using LightJson;
using Source.Galaxy;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World.System;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000BE RID: 190
	public class UmbralControl : MissionReward
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0003DFA2 File Offset: 0x0003C1A2
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly("@MissionRewardUmbralControl", new object[]
				{
					"+" + this.amount.ToString() + "%"
				});
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x0003DFD1 File Offset: 0x0003C1D1
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0003DFEF File Offset: 0x0003C1EF
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0003E008 File Offset: 0x0003C208
		public override void OnComplete(Mission m)
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller == null)
			{
				return;
			}
			storyteller.umbralContribution += Mathf.RoundToInt((float)this.amount / 2f);
			MapPointOfInterest sourcePoi = m.sourcePoi;
			ConquestSystem conquestSystem = ((sourcePoi != null) ? sourcePoi.system.storyteller : null) as ConquestSystem;
			if (conquestSystem != null)
			{
				conquestSystem.umbralControlLevel = Mathf.Clamp01(conquestSystem.umbralControlLevel + (float)this.amount / 100f);
			}
		}

		// Token: 0x0400044D RID: 1101
		public int amount;
	}
}
