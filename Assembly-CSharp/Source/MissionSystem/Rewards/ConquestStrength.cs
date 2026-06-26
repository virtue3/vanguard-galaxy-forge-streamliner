using System;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;
using Source.Player;
using Source.Simulation.Story;
using Source.Simulation.World.System;
using Source.Util;

namespace Source.MissionSystem.Rewards
{
	// Token: 0x020000B2 RID: 178
	public class ConquestStrength : MissionReward
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600076C RID: 1900 RVA: 0x0003D7E6 File Offset: 0x0003B9E6
		public override string rewardText
		{
			get
			{
				return Translation.TranslateOnly("@MissionRewardConquestStrength", new object[]
				{
					(this.amount < 0) ? this.amount : ("+" + this.amount.ToString())
				});
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0003D826 File Offset: 0x0003BA26
		public override void DataToJson(JsonObject data)
		{
			data["amount"] = new double?((double)this.amount);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0003D844 File Offset: 0x0003BA44
		public override void LoadFromJson(JsonObject data)
		{
			this.amount = data["amount"];
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0003D85C File Offset: 0x0003BA5C
		public override void OnComplete(Mission m)
		{
			Conquest storyteller = GamePlayer.current.GetStoryteller<Conquest>();
			if (storyteller == null)
			{
				return;
			}
			if (m.sourceFaction != null && this.amount > 0)
			{
				storyteller.GetFactionStanding(m.sourceFaction).playerContribution += this.amount;
				MissionObjective.Trigger(MissionTrigger.EarnCombatStrengthForFaction, new ValueTuple<int, Faction>(this.amount, m.sourceFaction), null, false);
			}
			MapPointOfInterest sourcePoi = m.sourcePoi;
			ConquestSystem conquestSystem = ((sourcePoi != null) ? sourcePoi.system.storyteller : null) as ConquestSystem;
			if (conquestSystem != null)
			{
				conquestSystem.combatStrength += (float)this.amount;
				if (conquestSystem.playerControlLevel == 0)
				{
					conquestSystem.playerControlLevel = 1;
					return;
				}
			}
			else
			{
				EmbassyStation embassyStation = m.sourcePoi as EmbassyStation;
				if (embassyStation != null)
				{
					ConquestSystem headquarters = storyteller.GetHeadquarters(m.sourceFaction);
					if (headquarters != null)
					{
						headquarters.combatStrength += (float)this.amount;
						return;
					}
					embassyStation.combatStrength += (float)this.amount;
				}
			}
		}

		// Token: 0x0400043F RID: 1087
		public int amount;
	}
}
