using System;
using LightJson;
using Source.Galaxy;

namespace Source.MissionSystem.Objectives
{
	// Token: 0x020000C3 RID: 195
	public class ConquestFleetStrength : TriggerObjective
	{
		// Token: 0x060007CC RID: 1996 RVA: 0x0003E4F4 File Offset: 0x0003C6F4
		public override void ProcessMissionTrigger(MissionTrigger trigger, object data)
		{
			ValueTuple<int, Faction> valueTuple = (ValueTuple<int, Faction>)data;
			int item = valueTuple.Item1;
			Faction item2 = valueTuple.Item2;
			if (item2 != null && this.faction != item2)
			{
				return;
			}
			this.currentAmount = Math.Min(this.currentAmount + item, this.requiredAmount);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0003E53A File Offset: 0x0003C73A
		protected override void DataToJson(JsonObject data)
		{
			base.DataToJson(data);
			data["faction"] = this.faction.identifier;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0003E55E File Offset: 0x0003C75E
		protected override void LoadFromJson(JsonObject data)
		{
			base.LoadFromJson(data);
			this.faction = Faction.Get(data["faction"]);
		}

		// Token: 0x04000455 RID: 1109
		public Faction faction;
	}
}
