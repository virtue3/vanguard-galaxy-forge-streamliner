using System;
using LightJson;
using Source.Galaxy;
using Source.Galaxy.POI;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000312 RID: 786
	public class JumpgatePassItem : UsableItem
	{
		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x000B074D File Offset: 0x000AE94D
		// (set) Token: 0x06001D83 RID: 7555 RVA: 0x000B0755 File Offset: 0x000AE955
		public string systemGuid { get; private set; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x000B075E File Offset: 0x000AE95E
		// (set) Token: 0x06001D85 RID: 7557 RVA: 0x000B0766 File Offset: 0x000AE966
		public string jumpgateGuid { get; private set; }

		// Token: 0x06001D86 RID: 7558 RVA: 0x000B076F File Offset: 0x000AE96F
		public void SetJumpgate(string systemGuid, string jumpgateGuid)
		{
			this.systemGuid = systemGuid;
			this.jumpgateGuid = jumpgateGuid;
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000B077F File Offset: 0x000AE97F
		public override bool OnUse()
		{
			this.UnlockJumpgate();
			return true;
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000B0788 File Offset: 0x000AE988
		public void UnlockJumpgate()
		{
			((JumpGate)GalaxyMapData.current.GetSystem(this.systemGuid).GetPoiWithId(this.jumpgateGuid)).UnlockJumpgate();
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x000B07AF File Offset: 0x000AE9AF
		public override void DataToJson(JsonObject data)
		{
			data["systemGuid"] = this.systemGuid;
			data["jumpgateGuid"] = this.jumpgateGuid;
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000B07DD File Offset: 0x000AE9DD
		public override void DataFromJson(JsonObject data)
		{
			this.systemGuid = data["systemGuid"];
			this.jumpgateGuid = data["jumpgateGuid"];
		}
	}
}
