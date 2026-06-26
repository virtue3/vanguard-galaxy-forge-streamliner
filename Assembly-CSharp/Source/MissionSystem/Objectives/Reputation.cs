using System;
using LightJson;
using Source.Galaxy;
using Source.Util;
using UnityEngine;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000CB RID: 203
	public class Reputation : MissionObjective
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x0003F2A0 File Offset: 0x0003D4A0
		public override string statusText
		{
			get
			{
				ValueTuple<int, int> absoluteProgressToTarget = ReputationLevelExtensions.GetAbsoluteProgressToTarget(this.faction.GetReputation(Faction.player), this.reputationLevel);
				int item = absoluteProgressToTarget.Item1;
				int item2 = absoluteProgressToTarget.Item2;
				return string.Format("Become {0} with {1} {2}/{3}", new object[]
				{
					Translation.Translate("@" + this.reputationLevel.ToString(), Array.Empty<object>()),
					Translation.Translate(this.faction.name, Array.Empty<object>()),
					item,
					item2
				});
			}
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0003F337 File Offset: 0x0003D537
		public override Sprite GetIcon()
		{
			return null;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0003F33A File Offset: 0x0003D53A
		public override bool IsComplete()
		{
			return ReputationLevelExtensions.IsLevelOrHigher(this.faction.GetReputationLevel(Faction.player), this.reputationLevel);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0003F357 File Offset: 0x0003D557
		public override MapPointOfInterest GetPoi()
		{
			return null;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0003F35A File Offset: 0x0003D55A
		protected override void DataToJson(JsonObject data)
		{
			data["faction"] = this.faction.identifier;
			data["level"] = this.reputationLevel.ToString();
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0003F398 File Offset: 0x0003D598
		protected override void LoadFromJson(JsonObject data)
		{
			this.faction = Faction.Get(data["faction"]);
			ReputationLevel reputationLevel;
			if (Enum.TryParse<ReputationLevel>(data["level"], out reputationLevel))
			{
				this.reputationLevel = reputationLevel;
				return;
			}
			reputationLevel = ReputationLevel.Neutral;
		}

		// Token: 0x0400046C RID: 1132
		public Faction faction;

		// Token: 0x0400046D RID: 1133
		public ReputationLevel reputationLevel;
	}
}
