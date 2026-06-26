using System;
using LightJson;
using Source.Data.Persistable;
using Source.Player;
using UnityEngine;

namespace Source.MissionSystem
{
	// Token: 0x020000A1 RID: 161
	public class BountyMission : Mission
	{
		// Token: 0x06000679 RID: 1657 RVA: 0x000370D0 File Offset: 0x000352D0
		public override void ClaimRewards(bool force = false)
		{
			base.ClaimRewards(force);
			GamePlayer.current.maxBountyLevel = Math.Max(GamePlayer.current.maxBountyLevel, base.level);
			if (this.bountyLevel == 2)
			{
				GamePlayer.current.bountyRank++;
			}
			base.mostRecentStep.GetMissionPoi().AddPersistable(new PatrolPortalData
			{
				position = GameplayManager.Instance.spaceShip.transform.position + new Vector3(-10f, SeededRandom.Global.RandomRange(-5f, 5f)),
				targetPoi = this.sourcePoi,
				isForwardPortal = false,
				portalName = "@BountyPortal",
				portalDesc = "@BountyPortalDesc"
			});
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000371A0 File Offset: 0x000353A0
		public override void OnMissionAbandoned()
		{
			base.OnMissionAbandoned();
			base.steps[0].GetMissionPoi().ClearUnits();
			base.steps[0].GetMissionPoi().ClearPayloads();
			if (this.bountyLevel == 2)
			{
				GamePlayer.current.bountyRank = Math.Max(0, GamePlayer.current.bountyRank - 1);
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00037204 File Offset: 0x00035404
		public override JsonValue ToJson()
		{
			JsonValue result = base.ToJson();
			result["pirateName"] = this.pirateName;
			result["bountyLevel"] = new double?((double)this.bountyLevel);
			return result;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0003724D File Offset: 0x0003544D
		public override void DataFromJson(JsonObject data)
		{
			base.DataFromJson(data);
			this.pirateName = data["pirateName"];
			this.bountyLevel = data["bountyLevel"];
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00037282 File Offset: 0x00035482
		public static BountyMission FromJson(JsonObject data)
		{
			BountyMission bountyMission = new BountyMission();
			bountyMission.DataFromJson(data);
			return bountyMission;
		}

		// Token: 0x04000398 RID: 920
		public string pirateName;

		// Token: 0x04000399 RID: 921
		public int bountyLevel;
	}
}
