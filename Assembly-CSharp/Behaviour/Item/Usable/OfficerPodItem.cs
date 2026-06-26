using System;
using LightJson;
using Source.Crew;
using Source.Player;

namespace Behaviour.Item.Usable
{
	// Token: 0x02000315 RID: 789
	public class OfficerPodItem : UsableItem
	{
		// Token: 0x06001DA7 RID: 7591 RVA: 0x000B1269 File Offset: 0x000AF469
		public override bool OnUse()
		{
			GamePlayer.current.crewMembers.Add(this.crewMember);
			GamePlayer.current.currentSpaceShip.AssignCrewMember(this.crewMember);
			return true;
		}

		// Token: 0x06001DA8 RID: 7592 RVA: 0x000B1297 File Offset: 0x000AF497
		public override void DataToJson(JsonObject data)
		{
			data["crewMember"] = this.crewMember.ToJson();
		}

		// Token: 0x06001DA9 RID: 7593 RVA: 0x000B12AF File Offset: 0x000AF4AF
		public override void DataFromJson(JsonObject data)
		{
			this.crewMember = CrewMemberData.FromJson(data["crewMember"]);
		}

		// Token: 0x0400120D RID: 4621
		public CrewMemberData crewMember;
	}
}
